using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public class ContractModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public IEnumerable<Contract> GetContracts { get; set; }

        [BindProperty]
        public InputModel Contract { get; set; }

        [BindProperty]
        public int ContractId { get; set; }

        public static string conn { get; set; }

        public ContractModel(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            GetContracts = DisplayContracts();
            Update = DateTime.Now;

        }

        

        public static List<Contract> DisplayContracts()
        {
            List<Contract> Contracts = new List<Contract>();
            // string conn = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            //data source=.\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true

            

            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "select * from LatestContractsList as l,Contract,Customer where l.CID=Customer.Id and l.Id = Contract.Id and Contract.Returned = 0 order by l.Plate ";

                    
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
                            c.Price = (float) Convert.ToDouble(sdr["Price"]);
                            c.Cname = Convert.ToString(sdr["Cname"]);
                            c.CID = Convert.ToInt32(sdr["CID"]);
                            c.CheckIn = Convert.ToDateTime(sdr["CheckIn"]);
                            c.CheckOut = Convert.ToDateTime(sdr["CheckOut"]);
                            c.Paid = Convert.ToInt32(sdr["Paid"]);                           
                            c.Deposit =(float) Convert.ToDouble(sdr["Deposit"]);
                            c.Note = Convert.ToString(sdr["Note"]);
                            Contracts.Add(c);
                        }
                    }
                    return Contracts;
                }
            }
        }

        

        /*public IActionResult OnPostAdd()
        {
            GetContracts = DisplayContracts();
            string conn = "Data Source=DESKTOP-O5QVOR0;Initial Catalog=DanielRentV4;Integrated Security=True";
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
                {
                    Contract.Cname = "";
                    ModelState.AddModelError(string.Empty, "This user is not in the database");
                }
    
                
                if (Contract.Note == null)
                    Contract.Note = "No note";

                double duration = (Contract.CheckIn - Contract.CheckOut).TotalDays;

                double rest = Contract.Price * duration - Contract.Paid;

                if (ModelState.IsValid)
                {
                    string q = "insert into Contract values ('" + Contract.Plate + "','" + getCID() + "','" + Contract.Price + "','" +
                        Contract.CheckIn + "','" + Contract.CheckOut + "','" + rest + "','" + Contract.Paid + "',N'" + Contract.Note + "',0)";

                    using (SqlCommand sqlcomm = new SqlCommand(q, sqlconn))
                    {

                        sqlcomm.ExecuteNonQuery();
                    }
                }

                sqlconn.Close();
            }
            
            GetContracts = DisplayContracts();
            Update = DateTime.Now;
            return Page();

        }*/

        [BindProperty]
        public DateTime Update { get; set; }


        public IActionResult OnPostReturned(int id)
        {
           
            //string conn = "Data Source=DESKTOP-O5QVOR0;Initial Catalog=DanielRentV4;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "update Contract set CheckIn='" + Update + "',Returned = 1 where Id = " + id;

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }

            }
            
            return RedirectToPage("Contract");
        }

            public IActionResult OnPostDelete(int id)
        {

            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "Delete from Contract where Id = " + id;

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }

            }

            return RedirectToPage("Contract");
        }

        public class InputModel
        {
            public int Plate { get; set; }
            public string Cname { get; set; }
            public float Price { get; set; }
            public DateTime CheckIn { get; set; }
            public DateTime CheckOut { get; set; }
            public int Paid { get; set; }
            public float Deposit { get; set; }
            public string Note { get; set; }
        }
    }
}
