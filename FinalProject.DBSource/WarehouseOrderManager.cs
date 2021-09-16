using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DBSource
{
    public class WarehouseOrderManager
    {
        /// <summary> 建立倉儲訂單 </summary>
        public static void CreateWarehouseOrder(string sNo, string orderNo, string  warehouseNo, DateTime warehouseProcessDate, string warehouseManagerNo, string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"INSERT INTO [WarehouseOrders]
                                        ([SNo]
                                        ,[OrderNo]
                                        ,[WarehouseNo]
                                        ,[WarehouseProcessDate]
                                        ,[WarehouseManagerNo]
                                        ,[Remark])
                                     VALUES
                                          (@sNo
                                          ,@orderNo
                                          ,@warehouseNo
                                          ,@warehouseProcessDate
                                          ,@warehouseManagerNo
                                          ,@remark);";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@sNo", sNo));
            list.Add(new SqlParameter("@orderNo", orderNo));
            list.Add(new SqlParameter("@warehouseNo", warehouseNo));
            list.Add(new SqlParameter("@warehouseProcessDate", warehouseProcessDate));
            list.Add(new SqlParameter("@warehouseManagerNo", warehouseManagerNo));
            list.Add(new SqlParameter("@remark", remark));
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
        public static DataRow GetWarehouseOrderAmount(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT DISTINCT 
                                         (SELECT Count(WNo) FROM [WarehouseOrders] WHERE WarehouseNo = @employeeID AND (WarehouseCheck = 'Y' OR WarehouseCheck = 'N')) AS FinishedOrderCount, 
                                         (SELECT Count(WNo) FROM [WarehouseOrders] WHERE WarehouseNo = @employeeID AND ((WarehouseCheck != 'Y' AND WarehouseCheck != 'N') OR WarehouseCheck IS NULL)) AS UnFinishedOrderCount
                                         FROM [WarehouseOrders]";

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
        public static DataRow GetWarehouseSectionOrderAmount(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@" SELECT DISTINCT 
                                            (SELECT Count(WNo) FROM [WarehouseOrders] INNER JOIN [CustomerOrders] ON WarehouseOrders.OrderNo = CustomerOrders.OrderNo WHERE WarehouseManagerNo = @employeeID AND WarehouseCheck = 'Y' AND WarehouseManagerCheck = 'Y' AND (CustomerOrders.OrderStatus = 1 OR CustomerOrders.OrderStatus = 4 OR CustomerOrders.OrderStatus = 5)) AS FinishedOrderCount, 
                                            (SELECT Count(WNo) FROM [WarehouseOrders] INNER JOIN [CustomerOrders] ON WarehouseOrders.OrderNo = CustomerOrders.OrderNo WHERE WarehouseManagerNo = @employeeID AND WarehouseCheck = 'Y' AND ((WarehouseManagerCheck != 'Y' AND WarehouseManagerCheck != 'N') OR WarehouseManagerCheck IS NULL) OR (WarehouseManagerCheck = 'Y' AND (CustomerOrders.OrderStatus != 1 AND CustomerOrders.OrderStatus != 4 AND CustomerOrders.OrderStatus != 5))) AS UnFinishedOrderCount,
                                            (SELECT Count(WNo) FROM [WarehouseOrders] INNER JOIN [CustomerOrders] ON WarehouseOrders.OrderNo = CustomerOrders.OrderNo WHERE WarehouseManagerNo = @employeeID AND WarehouseManagerCheck = 'Y' AND (CustomerOrders.OrderStatus = 1 OR CustomerOrders.OrderStatus = 4 OR CustomerOrders.OrderStatus = 5)) AS SectionFinishedOrderCount, 
                                            (SELECT Count(WNo) FROM [WarehouseOrders] INNER JOIN [CustomerOrders] ON WarehouseOrders.OrderNo = CustomerOrders.OrderNo WHERE WarehouseManagerNo = @employeeID AND ((WarehouseManagerCheck != 'Y' AND WarehouseManagerCheck != 'N') OR WarehouseManagerCheck IS NULL)OR (WarehouseManagerCheck = 'Y' AND (CustomerOrders.OrderStatus != 1 AND CustomerOrders.OrderStatus != 4 AND CustomerOrders.OrderStatus != 5))) AS SectionUnFinishedOrderCount
                                        FROM [WarehouseOrders]";

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

        /// <summary> 取得倉儲部門尚未處理之訂單列表</summary>
        public static DataTable GetWarehouseUnFinishOrderList(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            W.WNo
                                            ,W.SNo
                                            ,W.OrderNo
                                            ,C.OrderBranch
                                            ,C.OrderDate
                                        FROM [WarehouseOrders] AS W
                                        INNER JOIN [CustomerOrders] AS C
                                        ON W.OrderNo = C.OrderNo
                                        WHERE W.WarehouseNo = @employeeID AND ((W.WarehouseCheck != 'Y' AND W.WarehouseCheck != 'N') OR W.WarehouseCheck IS NULL)
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

        /// <summary> 取得倉儲部門主管尚未處理之訂單列表 </summary>
        public static DataTable GetWarehouseManagerUnFinishOrderList(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            W.WNo
                                            ,W.SNo
                                            ,W.OrderNo
                                            ,C.OrderBranch
                                            ,C.OrderDate
                                        FROM [WarehouseOrders] AS W
                                        INNER JOIN CustomerOrders AS C
                                        ON W.OrderNo = C.OrderNo
                                        WHERE W.WarehouseManagerNo = @employeeID AND W.WarehouseCheck = 'Y' AND ((W.WarehouseManagerCheck != 'Y' AND W.WarehouseManagerCheck != 'N') OR W.WarehouseManagerCheck IS NULL) OR (W.WarehouseManagerCheck = 'Y' AND C.OrderStatus = 3 )
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

        /// <summary> 取得倉儲部門已處理之訂單列表</summary>
        public static DataTable GetWarehouseFinishOrderList(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            W.WNo
                                            ,W.SNo
                                            ,W.OrderNo
                                            ,C.OrderBranch
                                            ,C.OrderDate
                                        FROM [WarehouseOrders] AS W
                                        INNER JOIN [CustomerOrders] AS C
                                        ON W.OrderNo = C.OrderNo
                                        WHERE W.WarehouseNo = @employeeID AND (W.WarehouseCheck = 'Y' OR W.WarehouseCheck = 'N')
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

        /// <summary> 取得倉儲主管已處理之訂單列表 </summary>
        public static DataTable GetWarehouseManagerFinishOrderList(string employeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            W.WNo
                                            ,W.SNo
                                            ,W.OrderNo
                                            ,C.OrderBranch
                                            ,C.OrderDate
                                        FROM [WarehouseOrders] AS W
                                        INNER JOIN [CustomerOrders] AS C
                                        ON W.OrderNo = C.OrderNo
                                        WHERE W.WarehouseManagerNo = @employeeID AND W.WarehouseManagerCheck = 'Y'
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

        /// <summary> 取得倉儲訂單資訊 </summary>
        public static DataRow GetWarehouseOrderInfo(string WNo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                        	  W.WNo 
                                              ,W.SNo
                                              ,W.OrderNo
                                              ,C.OrderEmployeeID
                                              ,C.OrderBranch
                                              ,C.OrderDate
                                              ,C.PreShipmentDate
                                              ,S.SalesNo
                                              ,S.SalesManagerNo
                                              ,S.SalesManagerProcessDate AS SalesProcessDate
                                              ,W.WarehouseNo
                                              ,W.WarehouseProcessDate
                                              ,W.WarehouseCheck
                                              ,W.WarehouseManagerNo
                                              ,W.WarehouseManagerProcessDate
                                              ,W.WarehouseManagerCheck
                                              ,W.Remark
                                        FROM [WarehouseOrders] AS W
                                        INNER JOIN [CustomerOrders] AS C
                                        ON W.OrderNo = C.OrderNo
                                        INNER JOIN [SalesOrders] AS S
                                        ON W.OrderNo = S.OrderNo
                                        WHERE W.WNo=@WNo";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@WNo", WNo));

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

        /// <summary> 更新倉儲訂單資訊(一般員工) </summary>
        public static bool UpdateWarehouseOrder(string wNo, string employeeID, string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [WarehouseOrders]
                                    SET
                                         WarehouseProcessDate = @warehouseProcessDate
                                         ,WarehouseCheck = @warehouseCheck
                                         ,Remark = @remark
                                     WHERE
                                          WNo = @wNo AND WarehouseNo = @warehouseNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@wNo", wNo));
            list.Add(new SqlParameter("@warehouseNo", employeeID));
            list.Add(new SqlParameter("@warehouseProcessDate", DateTime.Now));
            list.Add(new SqlParameter("@warehouseCheck", "Y"));
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

        /// <summary> 更新倉儲訂單資訊(主管) </summary>
        public static bool UpdateWarehouseManagerOrder(string wNo, string warehouseCheck, string warehouseManagerNo, string warehouseManagerCheck, string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [WarehouseOrders]
                                    SET
                                         WarehouseCheck = @warehouseCheck
                                         ,WarehouseManagerProcessDate = @warehouseManagerProcessDate
                                         ,WarehouseManagerCheck = @warehouseManagerCheck
                                         ,Remark = @remark
                                     WHERE
                                          WNo = @wNo AND WarehouseManagerNo = @warehouseManagerNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@warehouseCheck", warehouseCheck));
            list.Add(new SqlParameter("@WarehouseManagerProcessDate", DateTime.Now));
            list.Add(new SqlParameter("@warehouseManagerCheck", warehouseManagerCheck));
            list.Add(new SqlParameter("@wNo", wNo));
            list.Add(new SqlParameter("@warehouseManagerNo", warehouseManagerNo));
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
    }
}
