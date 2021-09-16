using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DBSource
{
    public class SalesOrderManager
    {

        /// <summary> 建立銷售訂單 </summary>
        public static void CreateSalesOrder(string orderNo, string salesNo, string salesManagerNo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"INSERT INTO [SalesOrders]
                                         (OrderNo
                                         ,SalesNo
                                         ,SalesProcessDate
                                         ,SalesManagerNo)
                                     VALUES
                                          (@orderNo
                                         ,@salesNo
                                         ,@salesProcessDate
                                         ,@salesManagerNo);";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));
            list.Add(new SqlParameter("@salesNo", salesNo));
            list.Add(new SqlParameter("@salesProcessDate", DateTime.Now));
            list.Add(new SqlParameter("@salesManagerNo", salesManagerNo));
            try
            {
                DBHelper.CreateData(connectionString, queryString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary> 取得訂單數量(一般員工) </summary>
        public static DataRow GetSalesOrderAmount(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT DISTINCT 
                                         (SELECT Count(SNo) FROM [SalesOrders] WHERE SalesNo = @employeeID AND (SalesCheck = 'Y' OR SalesCheck = 'N')) AS FinishedOrderCount, 
                                         (SELECT Count(SNo) FROM [SalesOrders] WHERE SalesNo = @employeeID AND ((SalesCheck != 'Y' AND SalesCheck != 'N') OR SalesCheck IS NULL)) AS UnFinishedOrderCount
                                         FROM [SalesOrders]";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@employeeID", employeeID));

            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary> 取得訂單數量(主管) </summary>
        public static DataRow GetSalesSectionOrderAmount(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@" SELECT DISTINCT 
                                            (SELECT Count(SNo) FROM [SalesOrders] WHERE SalesManagerNo = @employeeID AND (SalesCheck = 'Y' OR SalesCheck = 'N') AND SalesManagerCheck = 'Y') AS FinishedOrderCount, 
                                            (SELECT Count(SNo) FROM [SalesOrders] WHERE SalesManagerNo = @employeeID AND (SalesCheck = 'Y' OR SalesCheck = 'N') AND ((SalesManagerCheck != 'Y' AND SalesManagerCheck != 'N') OR SalesManagerCheck IS NULL)) AS UnFinishedOrderCount,
                                            (SELECT Count(SNo) FROM [SalesOrders] WHERE SalesManagerNo = @employeeID AND SalesManagerCheck = 'Y') AS SectionFinishedOrderCount, 
                                            (SELECT Count(SNo) FROM [SalesOrders] WHERE SalesManagerNo = @employeeID AND ((SalesManagerCheck != 'Y' AND SalesManagerCheck != 'N') OR SalesManagerCheck IS NULL)) AS SectionUnFinishedOrderCount
                                        FROM [SalesOrders]";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@employeeID", employeeID));

            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

       /// <summary> 取得銷售部門尚未處理之訂單列表 </summary>
        public static DataTable GetSalesUnFinishOrderList(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            S.SNo
                                            ,S.OrderNo
                                            ,C.OrderBranch
                                            ,C.OrderDate
                                        FROM [SalesOrders] AS S
                                        INNER JOIN [CustomerOrders] AS C
                                        ON S.OrderNo = C.OrderNo
                                        WHERE S.SalesNo = @employeeID AND ((S.SalesCheck != 'Y' AND S.SalesCheck != 'N') OR S.SalesCheck IS NULL)
                                        ORDER BY C.OrderDate";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@employeeID", employeeID));

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

        /// <summary> 取得銷售部門主管尚未處理之訂單列表 </summary>
        public static DataTable GetSalesManagerUnFinishOrderList(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            S.SNo
                                            ,S.OrderNo
                                            ,C.OrderBranch
                                            ,C.OrderDate
                                        FROM [SalesOrders] AS S
                                        INNER JOIN [CustomerOrders] AS C
                                        ON S.OrderNo = C.OrderNo
                                        WHERE S.SalesManagerNo = @employeeID AND (S.SalesCheck = 'Y' OR S.SalesCheck = 'N') AND ((S.SalesManagerCheck != 'Y' AND S.SalesManagerCheck != 'N') OR S.SalesManagerCheck IS NULL)
                                        ORDER BY C.OrderDate";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@employeeID", employeeID));

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

        /// <summary> 取得銷售部門已處理之訂單列表 </summary>
        public static DataTable GetSalesFinishOrderList(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            S.SNo
                                            ,S.OrderNo
                                            ,C.OrderBranch
                                            ,C.OrderDate
                                        FROM [SalesOrders] AS S
                                        INNER JOIN[CustomerOrders] AS C
                                        ON S.OrderNo = C.OrderNo
                                        WHERE S.SalesNo = @employeeID AND (S.SalesCheck = 'Y' OR S.SalesCheck = 'N')
                                        ORDER BY S.SalesProcessDate DESC";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@employeeID", employeeID));

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

        /// <summary> 取得銷售主管已處理之訂單列表 </summary>
        public static DataTable GetSalesManagerFinishOrderList(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            S.SNo
                                            ,S.OrderNo
                                            ,C.OrderBranch
                                            ,C.OrderDate
                                        FROM [SalesOrders] AS S
                                        INNER JOIN [CustomerOrders] AS C
                                        ON S.OrderNo = C.OrderNo
                                        WHERE S.SalesManagerNo = @employeeID AND S.SalesManagerCheck = 'Y'
                                        ORDER BY S.SalesManagerProcessDate";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@employeeID", employeeID));

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

        /// <summary> 取得訂單資訊 </summary>
        public static DataRow GetSalesOrderInfo(string SNo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                        	S.SNo
                                        	,S.OrderNo
                                        	,C.OrderEmployeeID
                                        	,C.OrderBranch
                                        	,C.OrderDate
                                        	,C.PreShipmentDate
                                        	,S.SalesNo
                                        	,S.SalesProcessDate
                                        	,S.SalesCheck
                                        	,S.SalesManagerNo
                                        	,S.SalesManagerProcessDate
                                        	,S.SalesManagerCheck
                                        	,S.Remark
                                        FROM [SalesOrders] AS S
                                        INNER JOIN [CustomerOrders] AS C
                                        ON S.OrderNo = C.OrderNo
                                        WHERE SNo= @SNo";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SNo", SNo));

            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary> 更新SalesCheck </summary>
        public static bool UpdateSalesCheck(string sNo, string salesCheck, DateTime salesProcessDate, string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [SalesOrders]
                                    SET
                                         SalesCheck = @salesCheck,
                                         SalesProcessDate = @salesProcessDate,
                                         SalesManagerCheck = @salesManagerCheck,
                                         Remark = @remark
                                     WHERE
                                          SNo = @sNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@sNo", sNo));
            list.Add(new SqlParameter("@salesCheck", salesCheck));
            list.Add(new SqlParameter("@SalesProcessDate", salesProcessDate));
            list.Add(new SqlParameter("@salesManagerCheck", string.Empty));
            list.Add(new SqlParameter("@remark", remark));
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

        /// <summary> 更新SalesManagerCheck </summary>
        public static bool UpdateSalesManagerCheck(string sNo, string salesCheck, string salesManagerCheck, DateTime salesManagerProcessDate,string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [SalesOrders]
                                    SET
                                         SalesCheck = @salesCheck,
                                         SalesManagerCheck = @salesManagerCheck,
                                         SalesManagerProcessDate = @salesManagerProcessDate,
                                         Remark = @remark
                                     WHERE
                                          SNo = @sNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@sNo", sNo));
            list.Add(new SqlParameter("@salesCheck", salesCheck));
            list.Add(new SqlParameter("@salesManagerCheck", salesManagerCheck));
            list.Add(new SqlParameter("@salesManagerProcessDate", salesManagerProcessDate));
            list.Add(new SqlParameter("@remark", remark));
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

        /// <summary>刪除銷售訂單</summary>
        public static bool DeleteSalesOrder(string orderNo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = @"DELETE 
                                   FROM [dbo].[SalesOrders] 
                                   WHERE OrderNo = @orderNo ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));

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
