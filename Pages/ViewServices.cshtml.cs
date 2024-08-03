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
    public class ViewServicesModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public IEnumerable<ServiceClass> GetServices { get; set; }

        public IEnumerable<int> NeedServiceList { get; set; }

        public static string conn { get; set; }
        

        public ViewServicesModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            GetServices = DisplayServices();
            NeedServiceList = DisplayNeedService();
        }

        public List<int> DisplayNeedService()
        {
            List<int> l = new List<int>();
            string q = "select Car.Plate from Car where Car.NeedService = 0";
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {

                using (SqlCommand sqlcomm = new SqlCommand(q, sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while(sdr.Read())
                        {
                            l.Add(Convert.ToInt32(sdr["Plate"]));
                        }
                    }
                }
            }
            return l;
        }

        public static List<ServiceClass> DisplayServices()
        {
            List<ServiceClass> services = new List<ServiceClass>();
            string query = "select * from Service";

            using (SqlConnection sqlconn = new SqlConnection(conn))
            {

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ServiceClass s = new ServiceClass();
                            s.Plate = Convert.ToInt32(sdr["Plate"]);
                            s.KiloMeter = Convert.ToInt32(sdr["ODO"]);
                            s.Type = Convert.ToString(sdr["Type"]);
                            s.Date = Convert.ToDateTime(sdr["Date"]);
                            s.Note = Convert.ToString(sdr["Note"]);
                            services.Add(s);
                        }
                    }
                    return services;
                }
            }
        }
    }
}
