using System;
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
    public class OverDateModel : PageModel
    {
        public readonly IConfiguration _configuration;
        //public static string conn = "Data Source=DESKTOP-O5QVOR0;Initial Catalog=DanielRentV4;Integrated Security=True";


        [BindProperty]
        public DateTime Update { get; set; }
        public static string conn { get; set; }

        

        public OverDateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<Contract> GetContracts { get; set; }

        public void OnGet()
        {
            //conn = _configuration.GetConnectionString("DefaultConnection");
            conn = "Data Source = DESKTOP - 4JK94BB; Initial Catalog = DanielRentV4; Integrated Security = True";
            GetContracts = DisplayContracts();
            Update = DateTime.Now;
        }

        public IActionResult OnPostReturned(int id)
        {
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "update Contract set CheckIn='" + Update + "',Returned = 1 where Id = " + id;
                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }
            }
            return RedirectToPage("OverDate");
        }

        public IActionResult CheckOut(string id)
        {
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "update Contract set Returned = '1' where Id = " +id;
                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }
            }
            return Page();
        }

        public static List<Contract> DisplayContracts()
        {
            conn = "Data Source=DESKTOP-4JK94BB;Initial Catalog=DanielRentV4;Integrated Security=True";
            List<Contract> Contracts = new List<Contract>();
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "select * from OverContracts order by CheckIn desc";
                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Contract c = new Contract();
                            c.Id = Convert.ToInt32(sdr["Id"]);
                            c.Plate = Convert.ToInt32(sdr["Plate"]);
                            c.Price = (float)Convert.ToDouble(sdr["Price"]);
                            c.Cname = Convert.ToString(sdr["Cname"]);
                            c.CID = Convert.ToInt32(sdr["CID"]);
                            c.CheckIn = Convert.ToDateTime(sdr["CheckIn"]);
                            c.CheckOut = Convert.ToDateTime(sdr["CheckOut"]);
                            Contracts.Add(c);
                        }
                    }
                    sqlconn.Close();
                    return Contracts;
                }
            }
        }
    }
}
