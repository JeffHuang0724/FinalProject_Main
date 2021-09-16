using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DBSource
{
    public class ChainsEmployeeInfoManager
    {/// <summary> 取得登入帳密 </summary>
        public static DataRow GetChainsEmployeeLoginInfo(string account)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            Account 
                                            ,Password
                                        FROM ChainsEmployees
                                        WHERE Account = @account;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@account", account));
            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        /// <summary> 取得GUID </summary>
        public static DataRow GetChainsEmployeeInfoByGUID(string actGUID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            ActGUID 
                                            ,ActTime
                                            ,EmployeeID
                                            ,BranchNo
                                        FROM ChainsEmployees
                                        WHERE ActGUID = @actGUID;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@actGUID", actGUID));
            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        /// <summary> 寫入GUID </summary>
        public static bool CreateChainsEmployeeGUID(string account, Guid actGUID, DateTime actTime)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [ChainsEmployees]
                                    SET
                                         ActGUID = @actGUID
                                         ,ActTime = @actTime
                                     WHERE
                                          Account = @account;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@actGUID", actGUID));
            list.Add(new SqlParameter("@actTime", actTime));
            list.Add(new SqlParameter("@account", account));
            try
            {
                return DBHelper.ModifyData(connectionString, queryString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }
        /// <summary> 更新GUID </summary>
        public static bool UpdateChainsEmployeeGUID(string actGUID, DateTime actTime)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [ChainsEmployees]
                                    SET
                                         ActGUID = @actGUID
                                         ,ActTime = @actTime
                                     WHERE
                                          ActGUID = @actGUID;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@actGUID", actGUID));
            list.Add(new SqlParameter("@actTime", actTime));
            try
            {
                return DBHelper.ModifyData(connectionString, queryString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }

        /// <summary> 取得員工列表 </summary>
        public static DataTable GetChainsEmployeeList()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            EmployeeID
                                            ,EmployeeName
                                            ,EmployeePhone
                                            ,BranchNo
                                        FROM [ChainsEmployees]";

            List<SqlParameter> list = new List<SqlParameter>();

            try
            {
                return DBHelper.ReadDataTable(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

    }
}
