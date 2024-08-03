using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using V4.Models;

namespace V4.Pages
{
    [Authorize(Policy = "MustSignIn")]
    public class AvailableCarsModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        public AvailableCarsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<Car> GetCars { get; set; }
        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            GetCars = DisplayCars();
        }

        public static List<Car> DisplayCars()
        {
            List<Car> Cars = new List<Car>();
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                using (SqlCommand sqlcomm = new SqlCommand("select * from AvailableCarsV2 order by Brand,Class", sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Car c = new Car();
                            c.Plate = Convert.ToInt32(sdr["Plate"]);
                            c.Brand = Convert.ToString(sdr["Brand"]);
                            c.Class = Convert.ToString(sdr["Class"]);
                            c.Model = Convert.ToInt32(sdr["Model"]);
                            c.Color = Convert.ToString(sdr["Color"]);
                            Cars.Add(c);
                        }
                    }
                    return Cars;
                }
            }


        }
    }
}

