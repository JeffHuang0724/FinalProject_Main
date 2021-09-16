using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DBSource
{
    public class DBHelper
    {
        public static string GetConnectionString()
        {
            string val = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return val;
        }

        public static void CreateData(string connectionString, string queryString, List<SqlParameter> list)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryString, conn))
                {
                    command.Parameters.AddRange(list.ToArray());
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public static bool ModifyData(string connectionString, string queryString, List<SqlParameter> list)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryString, conn))
                {
                    command.Parameters.AddRange(list.ToArray());

                    conn.Open();
                    int effectRows = command.ExecuteNonQuery();
                    if (effectRows == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public static DataTable ReadDataTable(string connectionString, string dbCommandString, List<SqlParameter> list)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(dbCommandString, connection))
                {
                    command.Parameters.AddRange(list.ToArray());

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    return dt;
                }
            }
        }
        public static DataRow ReadDataRow(string connectionString, string dbCommandString, List<SqlParameter> list)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(dbCommandString, connection))
                {
                    command.Parameters.AddRange(list.ToArray());
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        if (dt.Rows.Count == 0)
                        {
                            return null;
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            return dr;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }
                }
            }
        }
    }
}
