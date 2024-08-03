using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace V4.Pages
{
    [Authorize(Policy = "MustSignIn")]
    public class ReturnedModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        public ReturnedModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [BindProperty]
        public DateTime Update { get; set; }
        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            Update = DateTime.Now;
        }

        public IActionResult OnPostAsync(int id)
        {
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "update Contract set CheckOut='" + Update + "',Returned = 1 where Id = " + id;
                
                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }

            }

            return RedirectToPage("Contract");
        }
    }
}

