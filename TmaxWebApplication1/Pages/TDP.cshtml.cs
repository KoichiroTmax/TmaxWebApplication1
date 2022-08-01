using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
//using Tibero.DataAccess.Client;
using System.Data;
using System.Diagnostics;

namespace TmaxWebApplication1.Pages
{
    public class TDPModel : PageModel
    {
        private readonly ILogger<TDPModel> _logger;

        public TDPModel(ILogger<TDPModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // get DB name
            string selectDbname = "SELECT NAME FROM V$DATABASE";

            // set a table name you want to use on "useTable"
            string useTable = "EMP";
            string selectTable = "SELECT * FROM " + useTable;
            string selectColumnName = "SELECT column_name FROM USER_TAB_COLUMNS WHERE table_name = '" + useTable + "'";

            DataTable dt = new DataTable(useTable);

            try
            {
                using (OracleConnection conn = new OracleConnection())
                {
                    // Oracle connection information
                    conn.ConnectionString =
                     "User ID=TEST; Password=TEST; Data Source=192.168.56.108/ora12c";
                    conn.Open();

                    // Tibero connection information
                    //conn.ConnectionString =
                    //    "User Id=tibero;" +
                    //            "Password=tmax; " +
                    //            @"Data Source=(DESCRIPTION=((INSTANCE=(HOST=18.176.54.215)(PORT=8629)(DB_NAME=tibero))))";
                    //conn.Open();

                    // get DB name
                    using (OracleCommand cmdDbname = new OracleCommand(selectDbname))
                    {
                        cmdDbname.Connection = conn;
                        cmdDbname.CommandType = CommandType.Text;

                        using (OracleDataReader readerDbname = cmdDbname.ExecuteReader())
                        {
                            while (readerDbname.Read())
                            {
                                ViewData["dbname"] = readerDbname["NAME"];
                            }
                        }
                    }

                    // get column names of EMP
                    using (OracleCommand cmdColumnName = new OracleCommand(selectColumnName))
                    {
                        cmdColumnName.Connection = conn;
                        cmdColumnName.CommandType = CommandType.Text;

                        using (OracleDataReader readerColumnName = cmdColumnName.ExecuteReader())
                        {
                            while (readerColumnName.Read())
                            {
                                object v = readerColumnName["COLUMN_NAME"];
                                Debug.WriteLine(v);

                                dt.Columns.Add(v.ToString(), typeof(string));
                            }
                        }
                    }

                    // get rows of EMP
                    using (OracleCommand cmdEmp = new OracleCommand(selectTable))
                    {
                        cmdEmp.Connection = conn;
                        cmdEmp.CommandType = CommandType.Text;

                        using (OracleDataReader readerEmp = cmdEmp.ExecuteReader())
                        {
                            while (readerEmp.Read())
                            {
                                DataRow row = dt.NewRow();
                                foreach (DataColumn column in dt.Columns)
                                {
                                    row[column.ColumnName] = readerEmp[column.ColumnName];
                                }
                                dt.Rows.Add(row);
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

            ViewData["TableData"] = dt;
            Console.WriteLine(dt);
        }
    }
}