using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using V4.Models;

namespace V4.Pages
{
    public class CheckOutModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        public CheckOutModel(IConfiguration configuration)
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
                string query = "update Contract set CheckIn='" + Update+"' where Id = "+id;
                Console.WriteLine(Update);
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
