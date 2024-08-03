using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using V4.Models;

namespace V4.Pages
{
    public class loginModel : PageModel
    {
        
        public void OnGet()
        {
        }

        [BindProperty]
        public Login l { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            //Data Source=DESKTOP-O5QVOR0;Initial Catalog=DanielRentV4;Integrated Security=True;
            string conn = "Data Source=DESKTOP-4JK94BB;Initial Catalog=DanielRentV4;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                sqlconn.Open();

                int getUser()
                {
                    string query = "select Id from Login where Username='" + l.Username + "'";
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

                if (getUser() == 0)
                {
                    ModelState.AddModelError("","User not found");
                }
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                int validateUser()
                {
                    string pass =null;
                    sqlconn.Open();
                    string query = "select Password from Login where Username='" + l.Username + "'";
                    using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                    {
                        using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                pass = Convert.ToString(sdr["Password"]);
                            }
                            if (pass.Trim() == l.Password)
                                return 1;
                        }
                    }
                    sqlconn.Close();
                    
                    return 0;
                }
                sqlconn.Close();
                

                if (validateUser() == 0)
                {
                    ModelState.AddModelError("", "Wrong Password!");
                }
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name,"mohammad"), 
                    new Claim(ClaimTypes.Email,"admin@mywebsite.com"), 
                    new Claim("admin", "true") };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");


                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                
                
                return RedirectToPage("/Contract");


            }
        }
    }
}
