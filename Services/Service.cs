using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Data.SqlClient;
using V4.Models;


namespace V4.Services
{
    public class Service
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }

        public Service(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

       

        public int Check_ODO(int odo, int plate)
        {
            
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                sqlconn.Open();
                int service_odo = 0;
                string query = "select ODO from Service where Plate = " + plate + "";

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            service_odo = Convert.ToInt32(sdr["ODO"]);
                        }
                    }
                }
                sqlconn.Close();
                if (odo > service_odo + 7000)
                    return 0; //error this car need an oil change
            }
            return 1;
        }

    }
}
