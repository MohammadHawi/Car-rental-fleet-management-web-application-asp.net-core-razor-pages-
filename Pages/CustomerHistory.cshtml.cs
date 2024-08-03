using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using V4.Models;

namespace V4.Pages
{
    [Authorize(Policy = "MustSignIn")]
    public class CustomerHistoryModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        public CustomerHistoryModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<Contract> GetContracts { get; set; }

        [BindProperty]
        public Contract Contract { get; set; }

        [BindProperty]
        public Customer customer { get; set; }

        public void OnGet(int id)
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            GetContracts = DisplayContracts(id);

            customer = DisplayCustomer(id);
        }

        public static List<Contract> DisplayContracts(int id)
        {
            List<Contract> Contracts = new List<Contract>();
            
            
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "select Plate,Price,CheckIn,CheckOut from Customer,Contract where CID =" + id + " and Customer.Id =" + id;

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Contract c = new Contract();
                            
                            c.Plate = Convert.ToInt32(sdr["Plate"]);
                            c.Price = (float)Convert.ToDouble(sdr["Price"]);
                            c.CheckIn = Convert.ToDateTime(sdr["CheckIn"]);
                            c.CheckOut = Convert.ToDateTime(sdr["CheckOut"]);

                           
                            
                            Contracts.Add(c);
                        }
                    }
                    
                    return Contracts;
                }
            }
        }

        public Customer DisplayCustomer(int id) {

            Customer c = new Customer();
           
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "select * from Customer where Id = " + id;

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            c.Cname = Convert.ToString(sdr["Cname"]);
                            c.phone = Convert.ToString(sdr["Phone"]);
                            c.nationality = Convert.ToString(sdr["Nationality"]);

                            

                        }
                    }

                }
                return c;

            }
        }


    }
}
