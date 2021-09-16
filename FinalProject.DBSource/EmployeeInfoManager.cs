using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DBSource
{
    public class EmployeeInfoManager
    {
        /// <summary> 取得銷售人員列表 </summary>
        public static DataTable GetSalesList(string section)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            EmployeeID
                                            ,EmployeeName
                                        FROM [FinalProject].[dbo].[HeadquartersEmployees]
                                        WHERE Section = @section AND EmployeeLevel = @employeeLevel
                                        ORDER BY EmployeeID";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@section", section));
            list.Add(new SqlParameter("@employeeLevel", "1"));
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
        /// <summary> 取得Sales Info </summary>
        public static DataRow GetSalesInfo(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT Section,
                                               Department,
                                               Section,
                                               EmployeeLevel
                                        FROM HeadquartersEmployees
                                        WHERE EmployeeID = @employeeID;";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@employeeID", employeeID));
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
        /// <summary> 取得EmployeeNo、ManagerNo (區域) </summary>
        public static DataRow GetEmployeeInfoBySection(string section)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT DISTINCT
                                            (SELECT TOP(1) EmployeeID FROM HeadquartersEmployees WHERE EmployeeLevel = '1' AND Section = @section ORDER BY NEWID()) AS EmployeeNo,
                                            (SELECT EmployeeID FROM HeadquartersEmployees WHERE EmployeeLevel = '2' AND Section = @section) AS ManagerNo
                                        FROM HeadquartersEmployees;";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@section", section));
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

        /// <summary> 取得帳戶資訊(帳號) </summary>
        public static DataRow GetEmployeeByAccount(string EmployeeAccount)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            EmployeeID 
                                            ,EmployeeName
                                            ,EmployeeAccount
                                            ,EmployeePassword
                                            ,Department
                                            ,Section
                                            ,EmployeeLevel
                                            ,EmployeePhone
                                        FROM HeadquartersEmployees
                                        WHERE EmployeeAccount = @EmployeeAccount;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@EmployeeAccount", EmployeeAccount));
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

        /// <summary> 取得帳戶資訊(EmployeeID) </summary>
        public static DataRow GetEmployeeByEID(string EmployeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            EmployeeID 
                                            ,EmployeeName
                                            ,EmployeeAccount
                                            ,EmployeePassword
                                            ,Department
                                            ,Section
                                            ,EmployeeLevel
                                            ,EmployeePhone
                                        FROM HeadquartersEmployees
                                        WHERE EmployeeID = @EmployeeID;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@EmployeeID", EmployeeID));
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

        /// <summary> 取得員工列表 </summary>
        public static DataTable GetDepartEmployeeList()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            EmployeeID
                                            ,EmployeeName
                                            ,Department
                                            ,EmployeePhone
                                        FROM [FinalProject].[dbo].[HeadquartersEmployees]
                                        ORDER BY Department";

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

        /// <summary> 建立員工資訊 </summary>
        public static void CreateEmployeeInfo(string EmployeeName, string EmployeeAccount, string Department, string Section, string EmployeeLevel, string EmployeePhone)
        {

            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"INSERT INTO [FinalProject].[dbo].[HeadquartersEmployees]
                                         (EmployeeName
                                         ,EmployeeAccount
                                         ,EmployeePassword
                                         ,Department
                                         ,Section
                                         ,EmployeeLevel
                                         ,EmployeePhone)
                                     VALUES
                                          (@EmployeeName
                                         ,@EmployeeAccount
                                         ,@EmployeePassword
                                         ,@Department
                                         ,@Section
                                         ,@EmployeeLevel
                                         ,@EmployeePhone);";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@EmployeeName", EmployeeName));
            list.Add(new SqlParameter("@EmployeeAccount", EmployeeAccount));
            //密碼預設為12345
            list.Add(new SqlParameter("@EmployeePassword", "12345"));
            list.Add(new SqlParameter("@Department", Department));
            list.Add(new SqlParameter("@Section", Section));
            list.Add(new SqlParameter("@EmployeeLevel", EmployeeLevel));
            list.Add(new SqlParameter("@EmployeePhone", EmployeePhone));
            try
            {
                DBHelper.CreateData(connectionString, queryString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary> 更新帳戶資訊 </summary>
        public static bool UpdateEmployeeInfo(string EmployeeID, string EmployeeName, string EmployeeAccount, string EmployeePhone)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [FinalProject].[dbo].[HeadquartersEmployees]
                                    SET
                                         EmployeeName = @EmployeeName
                                         ,EmployeeAccount = @EmployeeAccount
                                         ,EmployeePhone = @EmployeePhone
                                     WHERE
                                          EmployeeID = @EmployeeID;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@EmployeeName", EmployeeName));
            list.Add(new SqlParameter("@EmployeeAccount", EmployeeAccount));
            list.Add(new SqlParameter("@EmployeePhone", EmployeePhone));
            list.Add(new SqlParameter("@EmployeeID", EmployeeID));
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
        /// <summary> 更新密碼 </summary>
        public static bool ChangePassword(string EmployeeID, string EmployeePassword)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [FinalProject].[dbo].[HeadquartersEmployees]
                                    SET
                                         EmployeePassword = @EmployeePassword
                                     WHERE
                                          EmployeeID = @EmployeeID;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@EmployeePassword", EmployeePassword));
            list.Add(new SqlParameter("@EmployeeID", EmployeeID));
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
    }
}
