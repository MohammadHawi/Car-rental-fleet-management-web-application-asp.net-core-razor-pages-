using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using V4.Models;
using V4.Services;

namespace V4.Pages
{
    [Authorize(Policy = "MustSignIn")]
    public class AddContractModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public readonly Service _service;
        public Service service { get; set; }
        public static string conn { get; set; }
        public static List<int> needServiceList = new List<int>();
        public AddContractModel(IConfiguration configuration,Service service )
        {
            _service = service;
            _configuration = configuration;
        }

       // public Service service = new Service();

        [BindProperty]
        public int ODO { get; set; }

        [BindProperty]
        public Contract Contract { get; set; }

        [BindProperty]
        public int i { get; set; }

        
        
        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult OnPost()
        {
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                sqlconn.Open();

                int getCID()
                {
                    string query = "select Id from Customer where Cname =(N'" + Contract.Cname + "')";
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

                if (getCID() == 0)
                    ModelState.AddModelError("Cname", "This user is not in the database");


                if (Contract.CheckOut > Contract.CheckIn)
                    ModelState.AddModelError("", "Drop In should be greater than Pick out");

                if (_service.Check_ODO(ODO, Contract.Plate) == 0)
                {
                    //   ModelState.AddModelError("Need service", "This car needes serivce asap");
                    // 0 in needservice in db means needs service
                    string qu = "update Car set NeedService = 0 where Plate = " + Contract.Plate + "";
                    using (SqlCommand sqlcomm = new SqlCommand(qu, sqlconn))
                    {
                        sqlcomm.ExecuteNonQuery();
                    }

                }


                if (!ModelState.IsValid)
                {
                    i = 0;
                    return Page();
                }


                if (Contract.Note == null)
                    Contract.Note = "No note";

                double duration = (Contract.CheckIn - Contract.CheckOut).TotalDays;

                double rest = Contract.Price * duration - Contract.Paid;


                string q = "insert into Contract values ('" + Contract.Plate + "','" + getCID() + "','" + Contract.Price + "','" +
                    Contract.CheckIn + "','" + Contract.CheckOut + "','" + rest + "','" + Contract.Paid + "',N'" + Contract.Note + "',0)";

                using (SqlCommand sqlcomm = new SqlCommand(q, sqlconn))
                {
                    sqlcomm.ExecuteNonQuery();
                }

                string SalesQuery = "insert into Sales Values ('" + getCID() + "','" + Contract.Plate + "','" + Contract.CheckOut + "','" + Contract.Paid + "')";

                using (SqlCommand sqlcomm = new SqlCommand(SalesQuery, sqlconn))
                {
                    sqlcomm.ExecuteNonQuery();
                }

                sqlconn.Close();
            }
            return RedirectToPage("Contract");
        }

        
    }
}
