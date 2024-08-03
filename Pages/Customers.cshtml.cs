using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using V4.Models;

namespace V4.Pages
{
    [Authorize(Policy = "MustSignIn")]
    public class CustomersModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        public CustomersModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Customer> GetCustomers { get; set; }
        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            GetCustomers = DisplayCustomers();
        }

        public static List<Customer> DisplayCustomers()
        {
            List<Customer> Customers = new List<Customer>();
            
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                using (SqlCommand sqlcomm = new SqlCommand("select * from Customer order by Cname", sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Customer c = new Customer();
                            c.Id = Convert.ToInt32(sdr["Id"]);
                            c.Cname = Convert.ToString(sdr["Cname"]);
                            c.phone = Convert.ToString(sdr["phone"]);
                            c.nationality = Convert.ToString(sdr["nationality"]);
                  
                            Customers.Add(c);
                        }
                    }
                    return Customers;
                }
            }


        }
        [BindProperty]
        public string name  { get; set; }

       


        public IActionResult OnPostSearch(string phone)
        {

            
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                sqlconn.Open();

                string getName()
                {
                    string query = "select Cname from Customer where phone ='" + phone.Trim() + "'";
                    using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                    {
                        using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                return Convert.ToString(sdr["Cname"]);
                            }
                        }
                    }
                    return null;
                }

                if (getName() == null)
                {
                    
                    ModelState.AddModelError("", "This phone number is not in database");
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }
                
                name = getName();

                sqlconn.Close();
                
            }
            GetCustomers = DisplayCustomers();

            return Page();
        }
        
        
        public IActionResult OnPostDelete(int id)
        {

            
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "Delete from Customer where Id = " + id;

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    try
                    {
                        sqlconn.Open();
                        sqlcomm.ExecuteNonQuery();
                    } catch(Exception e)
                    {
                    }
                }

            }

            return RedirectToPage("Customers");
        }

        /*public IActionResult OnPostAdd(Customer c)
        {
            
            string conn = "Data Source=DESKTOP-O5QVOR0;Initial Catalog=DanielRentV4;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                sqlconn.Open();

                int getId()
                {
                    string query = "select Id from Customer where phone ='" + c.phone.Trim() + "'";
                    using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                    {                       
                        using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                return Convert.ToInt32(sdr["Id"]);
                            }
                        }                        
                    }
                    return 0;
                }
             
                if (getId() != 0)
                {
                    ModelState.AddModelError("", "This customer is already in the database");
                }


                if (ModelState.IsValid)
                {
                    string query = "insert into Customer values((N'" + c.Cname.Trim() + "')" + ",'" + c.phone.Trim() + "','" + c.nationality.Trim() + "',0,0)";
                    using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                    {

                        sqlcomm.ExecuteNonQuery();

                    }
                }

                else
                {
                    sqlconn.Close();
                    GetCustomers = DisplayCustomers();
                    return Page();
                }
                sqlconn.Close();
            }
            GetCustomers = DisplayCustomers();
            
            return RedirectToPage("Customers");
        }*/
    }
}
