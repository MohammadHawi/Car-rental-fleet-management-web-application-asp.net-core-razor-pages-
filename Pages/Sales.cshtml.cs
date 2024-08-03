using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Policy;
using V4.Models;

namespace V4.Pages
{
    public class SalesModel : PageModel
    {


        public static string conn { get; set; }
        public readonly IConfiguration _configuration;
        public int Total { get; set; }


        public IEnumerable<Sales> GetSales { get; set; }


        public SalesModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            GetSales = DisplaySales();
            Total = GetTotal();
        }

        public static List<Sales> DisplaySales()
        {
            List<Sales> list = new List<Sales>();

            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                sqlconn.Open();

                string q = "select * from Sales where MONTH(Date) = '" + DateTime.Now.Month + "' and YEAR(Date) = '" + DateTime.Now.Year + "'   ";


                using (SqlCommand sqlcomm = new SqlCommand(q, sqlconn))
                {
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Sales s = new Sales();
                            s.Id = Convert.ToInt32(sdr["Id"]);
                            s.Plate = Convert.ToInt32(sdr["Plate"]);
                            s.Amount = Convert.ToInt32(sdr["Amount"]);
                            s.Date = Convert.ToDateTime(sdr["Date"]);
                            list.Add(s);
                        }
                    }

                }
                sqlconn.Close();
            }
            return list;


        }

        public int GetTotal()
        {
            List<Sales> list = DisplaySales();

            int total = 0;

            foreach (var item in list)
            {
                total += item.Amount;
            }
            return total;
        }

    }
}
