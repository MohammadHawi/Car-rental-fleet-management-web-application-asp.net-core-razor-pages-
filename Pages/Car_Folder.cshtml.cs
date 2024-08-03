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
    public class Car_FolderModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        public Car_FolderModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<Contract> GetContracts { get; set; }
        public void OnGet(int id)
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            GetContracts = DisplayContract(id);
        }

        public static List<Contract> DisplayContract(int id)
        {
            List<Contract> Contracts = new List<Contract>();
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "select * from Contract,Customer where CID = Customer.Id and Plate = " + id;


                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Contract c = new Contract();

                            c.Plate = Convert.ToInt32(sdr["Plate"]);
                            c.Price = (float)Convert.ToDouble(sdr["Price"]);
                            c.Cname = Convert.ToString(sdr["Cname"]);

                            c.CheckIn = Convert.ToDateTime(sdr["CheckIn"]);
                            c.CheckOut = Convert.ToDateTime(sdr["CheckOut"]);
                            

                            c.Note = Convert.ToString(sdr["Note"]);
                            Contracts.Add(c);
                        }
                    }
                }
            }
            return Contracts;
        }
    }
}

        

        
    

