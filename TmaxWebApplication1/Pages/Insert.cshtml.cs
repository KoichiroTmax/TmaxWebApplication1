using Microsoft.AspNetCore.Mvc.RazorPages;
//using Oracle.ManagedDataAccess.Client;
using Tibero.DataAccess.Client;
using System.Data;
using System.Diagnostics;

namespace TmaxWebApplication1.Pages
{
    public class InsertModel : PageModel
    {
        private ILogger<InsertModel> _logger;

        public InsertModel(ILogger<InsertModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string selectDbname = "SELECT NAME FROM V$DATABASE";

            try
            {
                using (TiberoConnection conn = new TiberoConnection())
                {
                    conn.ConnectionString =
                        "User Id=tibero;" +
                               "Password=tmax; " +
                              @"Data Source=(DESCRIPTION=((INSTANCE=(HOST=18.176.54.215)(PORT=8629)(DB_NAME=tibero))))";
                    conn.Open();

                    // get DB name
                    using (TiberoCommand cmdDbname = new TiberoCommand(selectDbname))
                    {
                        cmdDbname.Connection = conn;
                        cmdDbname.CommandType = CommandType.Text;

                        using (TiberoDataReader readerDbname = cmdDbname.ExecuteReader())
                        {
                            while (readerDbname.Read())
                            {
                                ViewData["dbname"] = readerDbname["NAME"];
                            }
                        }
                    }


                }
            }

            // Error handling
            catch (Exception e)
            {
                ViewData["dbname"] = e.Message;
            }
        }
    }
}