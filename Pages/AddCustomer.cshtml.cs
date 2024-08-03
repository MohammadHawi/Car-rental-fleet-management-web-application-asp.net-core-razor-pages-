using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;
using V4.Models;

namespace V4.Pages
{
    [Authorize(Policy = "MustSignIn")]
    public class AddCustomerModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        [BindProperty]
        public string Cname { get; set; }
        public AddCustomerModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [BindProperty]
        public Customer c { get; set; }
        public void OnGet(int id)
        {
            conn = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string q = "select * from Customer where Id = " + id;
                using (SqlCommand sqlcomm = new SqlCommand(q, sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        Customer cn = new Customer();
                        while (sdr.Read())
                        {

                            cn.Id = Convert.ToInt32(sdr["Id"]);
                            cn.Cname = Convert.ToString(sdr["Cname"]);
                            cn.phone = Convert.ToString(sdr["phone"]);
                            cn.nationality = Convert.ToString(sdr["nationality"]);


                        }
                        c = cn;
                    }

                }
            }
        }
        public IActionResult OnPost()
        {
            

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
                    string getCname(int id)
                    {
                        id = getId();
                        string q = "select Cname from Customer where Id = " + id ;
                        using (SqlCommand sqlcomm = new SqlCommand(q, sqlconn))
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

                    Cname = getCname(getId());
                    ModelState.AddModelError("", "This customer is already in the database");
                }

                if (!ModelState.IsValid)
                    return Page();
                if (ModelState.IsValid)
                {
                    var filePath = Path.GetTempFileName();
                    if (c.PassPort != null)
                    {
                        using (var stream = System.IO.File.Create(filePath))
                        { 
                            c.PassPort.CopyToAsync(stream);
                        }
                    }
                    string query = "insert into Customer values((N'" + c.Cname.Trim() + "')" + ",'" + c.phone.Trim() + "','" + c.nationality.Trim() + "',0,0,'"+filePath+"')";
                    using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                    {

                        sqlcomm.ExecuteNonQuery();

                    }
                }

                
                sqlconn.Close();
            }

            return RedirectToPage("AddContract",c.Cname);
        }

        public IActionResult OnPostUpdate(int id)
        {
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                sqlconn.Open();
                string query = "update Customer Set Cname = '" + c.Cname + "',phone = '" + c.phone + "',nationality = '" + c.nationality + "' where Id = " + id;
                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlcomm.ExecuteNonQuery();
                    
                }
                sqlconn.Close();
            }

            return RedirectToPage("Customers");
        }
    }
}
