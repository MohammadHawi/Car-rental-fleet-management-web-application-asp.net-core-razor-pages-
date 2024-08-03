using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using V4.Models;

namespace V4.Pages
{
    public class AddServiceModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        [BindProperty]
        public String Type { get; set; }
        public string[] Types = new[] { "Oil-change", "Brakes-replacment", "Other" };
        public AddServiceModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");

        }
        [BindProperty]
        public ServiceClass service { get; set; }

        public IActionResult OnPost()
        {
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                sqlconn.Open();

                int CheckPlate()
                {
                    string query = "select Plate from Car where Plate = (" + service.Plate + ")";
                    using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                    {
                        using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                return Convert.ToInt32(sdr["Plate"]);
                            }
                        }
                    }
                    return 0;
                }
                int i = CheckPlate();
                int j = service.Plate;
                if (CheckPlate() == 0)
                    ModelState.AddModelError("", "Car is not found");

                if (!ModelState.IsValid)
                    return Page();

                if (service.Note == null)
                    service.Note = "No note";

                if(service.Type == "Oil-change")
                {
                    string q = "Update Car set NeedService = 1 where Car.Plate = " + service.Plate + "";
                    using (SqlCommand sqlcomm = new SqlCommand(q, sqlconn))
                    {
                        sqlcomm.ExecuteNonQuery();
                    }
                }

                string query = " insert into Service values ('" +service.Plate + "','" + service.Type + "','" + service.Date + "','" + service.KiloMeter + "','" + service.Note +"');";

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlcomm.ExecuteNonQuery();
                }

                sqlconn.Close();

            }


            return Page();
        }
    }
}
