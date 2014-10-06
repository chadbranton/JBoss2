using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace JBOFarmersMkt.Models
{
    public class JBODatabase
    {
        JBOContext context = new JBOContext();
        public void addCustomer(Customer customer, string username)
        {
            Customer c = new Customer
            {
                firstName = customer.firstName,
                lastName = customer.lastName,
                address = customer.address,
                city = customer.city,
                state = customer.state,
                zip = customer.zip,
                phone = customer.phone,
                email = customer.email,
                username = username
            };

            context.Customers.Add(c);
            context.SaveChanges();
        }

        public void addImportedFile(string filename)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("addFile", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@filename";
                paramName.Value = filename;
                cmd.Parameters.Add(paramName);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

         }
      }
   }
 }
