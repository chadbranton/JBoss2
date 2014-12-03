(function ($, _) {
    // Acts as a backup of the original html of the given element and returns
    // a function that will restore it when called.
    var reset_text_to_default = function (elem) {
        var html = $(elem).html();

        return function () {
            $(elem).html(html);
        };
    };

    var input_has_error = function (error_element, name, reason, template) {
        $(error_element).html(template({ name: name, reason: reason }))
        .show()
        .closest(".form-group")
        .removeClass("has-success")
        .addClass("has-error");
    };

    var input_has_success = function (error_element) {
        $(error_element).hide()
        .closest(".form-group")
        .removeClass("has-error")
        .addClass("has-success");
    };

    var display_preview = function (preview_element, template, dt_config) {
        return function (data, f) {
            //$(preview_element).html(template({ rows: data, file: f })).find("table").dataTable(dt_config);
            // Has the same effect as above but runs twice as fast.
            // Appears to be due to the expensive effects of "get offsetWidth" when the original
            // table full of data is visible. Hiding the table reduces the cost to around 10ms.
            // There's probably still room for optimization on large tables.
            var t = $(template({ rows: data, file: f })).hide();
            $(preview_element).html(t);
            $(t).find("table").dataTable(dt_config);
            $(preview_element).show().children().show();
        };
    };

    // Disable form submission if there are any errors
    // or if the form is empty.
    var check_form = function () {
        // Default to disabled
        var disabled = true;

        // Check if both inputs are empty
        // See: http://stackoverflow.com/a/17044272
        var isEmpty = $("input[type=file]").filter(function () { return !this.value }).length == 2;

        var hasErrors = $("form").has(".has-error").length > 0;

        if (isEmpty || hasErrors) {
            disabled = true;
        } else {
            disabled = false;
        }

        $("form input[type='submit']").prop("disabled", disabled);
    };

    // Reset the given file input using the workaround found
    // at: http://stackoverflow.com/a/13351234
    // resetPreviewFn() should clear the preview area for the particular file
    var reset_file_input = function (elem, resetPreviewFn) {
        // Perform the workaround to reset the file input
        var e = $(elem);
        e.wrap('<form>').closest('form').get(0).reset();
        e.unwrap();

        // Remove any error or success classes since the file is no longer selected
        // Also hide any error messages that may exist
        e.closest('.form-group')
            .removeClass('has-error has-success')
            .find('.error-message')
            .hide();

        // The form could now be valid. Check it.
        check_form();

        // Reset the preview area since a file is no longer selected.
        resetPreviewFn();
    };

    // Takes a regex specifying a valid filename.
    // Returns a function that takes a file and compares its name to the regex
    // returning true or false as appropriate.
    var is_valid_filename = function (regex) {
        return function (file) {
            if (!file.name.match(regex)) {
                return false;
            }

            return true;
        }
    }

    var is_valid_product_filename = is_valid_filename(/stock_items.+csv$/);
    var is_valid_sales_filename = is_valid_filename(/sold_items_from_.+_to_.+csv$/);

    // Return the base64 encoded sha256 hash corresponding to string.
    // See: https://github.com/digitalbazaar/forge#sha256
    var compute_hash = function (string) {
        var md = forge.md.sha256.create();
        md.update(string);
        return forge.util.encode64(md.digest().data);
    }

    // Hashes the given file. Returns a promise that will contain the hash or an error.
    var hash_file = function (f) {
        var reader = new FileReader();
        var deferred = $.Deferred();

        reader.onload = function (e) {
            var result = compute_hash(e.target.result)
            deferred.resolve(result);
        }

        reader.onerror = function () {
            deferred.reject(this);
        }

        // Setting to ASCII could be a source of problems, but it seems to be agreeing
        // with the server so far.
        reader.readAsText(f, "ASCII");

        return deferred.promise();
    }

    // Calls hash_file with the file in file_input
    var hash_file_input = function (file_input) {
        return hash_file($(file_input)[0].files[0]);
    }

    // Takes the last import date for the particular import
    // and returns a function that takes the files lastModifiedDate.
    // This function returns true if the file is newer than the last import.
    var is_valid_date = function (lastImportDate) {
        return function (fileDate) {
            return fileDate > lastImportDate;
        }
    }

    // Generic function for parsing the given file input when it changes. Takes care of adding the event handler, data validation, and error handling.
    var parse_file_on_change = function (file_input, error_element, error_template, resetFn, is_valid_filenameFn, is_valid_dateFn, displayFn, import_metadata) {
        var handle_error = function (name, reason) {
            input_has_error(error_element, name, reason, error_template);
            resetFn();
            check_form();
        };

        // Validates the current file input.
        // Returns a promise that will contain the name and reasons
        // on failure to be used with handle_error().
        // Doesn't return any useful data on succcess.
        var validate = function () {
            var errors = [];
            var file = $(file_input)[0].files[0];
            var deferred = $.Deferred();

            if (!file.name.match(/.csv$/)) {
                // The file must be a CSV.
                errors.push("Unsupported file selected. Only CSV files exported from ShopKeep are supported.");
            }

            if (!is_valid_filenameFn(file)) {
                // The filename does not match that of ShopKeep exports.
                errors.push("Wrong file selected. The name of the selected file is not consistent with filenames used by ShopKeep.");
            }

            if (errors.length > 0) {
                // We don't have a valid csv.
                // Don't waste time computing a hash or checking modification time.
                deferred.reject("Invalid filename.", errors);
            } else {
                // The csv seems legit.
                // Check the modification time before hashing
                //
                // The idiom below passes null as the errorFn for most of "then" statements.
                // This allows returning a rejected promise to jump to the last failure handler
                // which ultimately rejects the main promise. This way, hashing is only done if
                // the file seems legitimate.
                $.when(is_valid_dateFn(new Date(file.lastModifiedDate)))
                .then(function (valid) {
                    if (!valid) {
                        // The file is older than the last import.
                        errors.push("This file is out of date.");
                        // Jump to the next failure handler.
                        // This bypasses the hash check since it would be a waste of time.
                        return $.Deferred().reject("Stale file detected.").promise();
                    }
                }, null)
                .then(function () {
                    // Check the hash before parsing.
                    //
                    // Nesting this "then" statement allows it to properly handle any problems
                    // that come from hashing the file without preventing previous promises
                    // from short-circuiting the process by returning a rejected promise.
                    return hash_file(file).then(function (hash) {
                        // Return a new promise so this can be chained with other validations.
                        return $.Deferred(function (d) {
                            if (_.contains(import_metadata.hashes, hash)) {
                                // The file is a duplicate
                                errors.push("This file has already been imported.");
                                // Jump to the next failure handler
                                d.reject("Duplicate file")
                            } else {
                                // The file is legit.
                                d.resolve();
                            }
                        }).promise();
                    }, function (error) {
                        // Something happened reading the file.
                        // Let the server worry about the duplicate check.
                        console.log(error);
                        return $.Deferred().resolve().promise();
                    })
                }, null)
                .then(function () {
                    // All validations passed.
                    deferred.resolve();
                }, function (name) {
                    // There was a problem.
                    deferred.reject(name, errors);
                });
            }

            return deferred.promise();
        }

        $(file_input).change(function () {
            if (this.files[0] == null) {
                // The user hit cancel and the input is empty again...

                // This fixes a potential bad state in the form and it
                // prevents a javascript error where file is not defined below.
                // Reset it.
                $(this).trigger("validate.reset");
                // Nothing more to do.
                return;
            }
            validate().then(function () {
                // This file has passed validation.
                input_has_success(error_element);

                $(file_input).parse({
                    config: {
                        error: function (error, file) {
                            //console.log("An error occurred with the chosen file: ", error);
                            handle_error(error.name, error.message);
                        },
                        complete: function (results, file) {
                            displayFn(results.data, file);
                            check_form();
                        }
                    },
                    //#region Unused beforeFn
                    //before: function (file, input_element) {
                    //    // This function is called before the file is parsed
                    //    // and can be used to abort parsing. Most validation
                    //    // should now be occurring in validate(), however.
                    //    var a = { action: "continue" };
                    //    var r = [];

                    //    // If there are any reasons to abort, we should be aborting...
                    //    if (r.length > 0) {
                    //        a = { action: "abort" };
                    //    }

                    //    a.reason = r;

                    //    return a;
                    //},
                    //#endregion

                    // Called when there is an error parsing files
                    error: function (error, file, input_element, reason) {
                        //console.log("Error: ", error, " - ", reason);
                        handle_error(error.name, reason);
                    }
                })
            }, function (name, errors) {
                //This file failed validation.
                handle_error(name, errors);
            });
        });
    };

    $(function () {
        var table_template = _.template($("#table-template").html());
        var error_template = _.template($("#error-template").html());

        var reset_product_preview = function () {
            // Store the original text
            var reset = reset_text_to_default("#product-preview");

            // Return a function that will destroy the DataTable (allows garbage collection)
            // and reset the text.
            return function () {
                $("#product-preview table.dataTable").dataTable({ "bRetrieve": true }).fnDestroy();
                reset();
            }
        }();

        var reset_sales_preview = function () {
            // Store the original text
            var reset = reset_text_to_default("#sales-preview");
            // Return a function that will destroy the DataTable (allows garbage collection)
            // and reset the text.
            return function () {
                $("#sales-preview table.dataTable").dataTable({ "bRetrieve": true }).fnDestroy();
                reset();
            }
        }();

        var import_metadata = $.parseJSON($("#import-metadata").text()) || {};
        var is_valid_product_date = is_valid_date(new Date(import_metadata.lastProductsImportDate));
        var is_valid_sales_date = is_valid_date(new Date(import_metadata.lastSalesImportDate))

        var display_products = display_preview("#product-preview", table_template,
            {
                "order": [1, "asc"],
                "searching": false,
                "lengthChange": false,
                "columnDefs": [
                    {
                        "targets": [3, 7, 8, 9, 11, 13, 14, 15, 17, 18, 19],
                        "visible": false
                    },
                    {
                        "targets": "_all",
                        "orderable": false
                    }
                ]
            });

        var display_sales = display_preview("#sales-preview", table_template,
            {
                "order": [1, "asc"],
                "searching": false,
                "lengthChange": false,
                "columnDefs": [
                    {
                        "targets": [0, 2, 5, 6, 7, 8, 10, 11, 12, 13, 14],
                        "visible": false
                    },
                    {
                        "targets": "_all",
                        "orderable": false
                    }
                ]
            });

        // Setup reset handlers
        $("#products").on("validate.reset", function (e) { reset_file_input("#products", reset_product_preview); });
        $("#sales").on("validate.reset", function (e) { reset_file_input("#sales", reset_sales_preview) });
        $("#reset-products-input").click(function (e) { e.preventDefault(); $("#products").trigger("validate.reset"); });
        $("#reset-sales-input").click(function (e) { e.preventDefault(); $("#sales").trigger("validate.reset"); });

        // Parse products file input on change
        parse_file_on_change(
            "input[type='file']#products",
            "#products-error",
            error_template,
            reset_product_preview,
            is_valid_product_filename,
            is_valid_product_date,
            display_products,
            import_metadata
            );

        // Parse sales file input on change
        parse_file_on_change(
            "input[type='file']#sales",
            "#sales-error",
            error_template,
            reset_sales_preview,
            is_valid_sales_filename,
            is_valid_sales_date,
            display_sales,
            import_metadata
            );

        // Handle form submission
        $("form#import").submit(function (e) {
            e.preventDefault();

            var formData = new FormData(this);

            $.ajax({
                url: '/Import/Upload',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false
            }).done(function (data) {
                console.log(data);
            });
        });

        // Display the last import times
        var displayLastImportDate = function (preview_element, date) {
            var d = new Date(date);
            var time = d.toLocaleTimeString();
            var day = d.toLocaleDateString();

            $(preview_element)
                        .siblings("h2")
                        .first()
                        .append(" <small>(Last imported at " + time + " on " + day + ")</small>");
        }

        displayLastImportDate("#products-preview", import_metadata.lastProductsImportDate);
        displayLastImportDate("#sales-preview", import_metadata.lastSalesImportDate);
    });
})(jQuery, _);