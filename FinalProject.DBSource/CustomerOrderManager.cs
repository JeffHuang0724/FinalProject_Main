using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DBSource
{
    public class CustomerOrderManager
    {
        /// <summary> 建立訂單 </summary>
        public static void CreateOrder(string orderEmployeeID, string orderBranch, DateTime orderDate, int orderStatus, string remark)
        {

            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"INSERT INTO [FinalProject].[dbo].[CustomerOrders]
                                         (OrderEmployeeID
                                         ,OrderBranch
                                         ,OrderDate
                                         ,PreshipmentDate
                                         ,Remark
                                         ,OrderStatus)
                                     VALUES
                                          (@orderEmployeeID
                                         ,@orderBranch
                                         ,@orderDate
                                         ,@preshipmentDate
                                         ,@remark
                                         ,@orderStatus);";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderEmployeeID", orderEmployeeID));
            list.Add(new SqlParameter("@orderBranch", orderBranch));
            list.Add(new SqlParameter("@orderDate", orderDate));
            list.Add(new SqlParameter("@preshipmentDate", orderDate.AddDays(7)));
            list.Add(new SqlParameter("@remark", remark));
            list.Add(new SqlParameter("@orderStatus", orderStatus));
            try
            {
                DBHelper.CreateData(connectionString, queryString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary> 取得訂單資訊(製作訂單需要) </summary>
        public static DataRow GetOrderDefault(string orderEmployeeID, string orderBranch, DateTime orderDate, string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT
                                            OrderNo
                                        FROM [FinalProject].[dbo].[CustomerOrders]
                                        WHERE
                                            OrderEmployeeID = @orderEmployeeID 
                                            AND
                                            OrderBranch = @orderBranch
                                            AND
                                            OrderDate = @orderDate
                                            AND
                                            Remark = @remark";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderEmployeeID", orderEmployeeID));
            list.Add(new SqlParameter("@orderBranch", orderBranch));
            list.Add(new SqlParameter("@orderDate", orderDate));
            list.Add(new SqlParameter("@remark", remark));

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

        /// <summary> 建立詳細訂單 </summary>
        public static void CreateOrderDetail(string orderNo, string itemNo, int itemCount, int itemPrice, string remark)
        {

            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"INSERT INTO [FinalProject].[dbo].[CustomerOrderDetails]
                                         (OrderNo
                                         ,ItemNo
                                         ,ItemCount
                                         ,ItemPrice
                                         ,Remark)
                                     VALUES
                                          (@orderNo
                                         ,@itemNo
                                         ,@itemCount
                                         ,@itemPrice
                                         ,@remark);";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));
            list.Add(new SqlParameter("@itemNo", itemNo));
            list.Add(new SqlParameter("@itemCount", itemCount));
            list.Add(new SqlParameter("@itemPrice", itemPrice));
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

        /// <summary> 取得單筆訂單資訊(藉由訂單編號) </summary>
        public static DataRow GetOrderInfo(string orderNo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT
                                            OrderNo,
                                            OrderEmployeeID,
                                            OrderBranch,
                                            OrderDate,
                                            PreShipmentDate,
                                            ShipmentDate,
                                            ArriveDate,
                                            Remark,
                                            OrderStatus
                                        FROM [FinalProject].[dbo].[CustomerOrders]
                                        WHERE
                                            OrderNo = @orderNo";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));
            
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

        /// <summary> 取得訂單資訊列表(員工編號) </summary>
        public static DataTable GetOrderList(string orderEmployeeID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT [OrderNo]
                                            ,[OrderEmployeeID]
                                            ,[OrderBranch]
                                            ,[OrderDate]
                                            ,[PreShipmentDate]
                                            ,[ShipmentDate]
                                            ,[ArriveDate]
                                            ,[Remark]
                                            ,[OrderStatus]
                                        FROM [FinalProject].[dbo].[CustomerOrders]
                                        WHERE OrderEmployeeID = @orderEmployeeID";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderEmployeeID", orderEmployeeID));

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


        /// <summary> 取得訂單品項資訊(藉由訂單編號) </summary>
        public static DataTable GetOrderDetailInfo(string orderNo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT C.[OrderNo]
                                               ,C.[ItemNo]
                                               ,I.[ItemName]
                                               ,C.[ItemCount]
                                               ,C.[ItemPrice]
                                               ,(C.[ItemCount] * C.[ItemPrice]) AS TotalAmount
                                               ,C.[WetherStock]
                                               ,C.[Remark]
                                        FROM [FinalProject].[dbo].[CustomerOrderDetails] AS C
                                        INNER JOIN [FinalProject].[dbo].[Items] AS I
                                        ON C.ItemNo = I.ItemNo
                                        WHERE C.OrderNo=@orderNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));

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
        
        /// <summary> 更新訂單品項狀態 (Warehouse Step. 1) </summary>
        public static bool UpdateOrderDetailStatus(string orderNo, string itemNo, string wetherStock)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [FinalProject].[dbo].[CustomerOrderDetails]
                                    SET
                                         WetherStock = @wetherStock
                                     WHERE
                                          OrderNo = @orderNo AND ItemNo = @itemNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));
            list.Add(new SqlParameter("@itemNo", itemNo));
            list.Add(new SqlParameter("@wetherStock", wetherStock));
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

        /// <summary> 更新訂單狀態 </summary>
        public static bool UpdateOrderStatus (string orderNo, DateTime arriveDate, int orderStatus , string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [FinalProject].[dbo].[CustomerOrders]
                                    SET
                                         ArriveDate = @arriveDate,
                                         OrderStatus = @orderStatus,
                                         Remark = @remark
                                     WHERE
                                          OrderNo = @orderNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));
            list.Add(new SqlParameter("@arriveDate", arriveDate));
            list.Add(new SqlParameter("@orderStatus", orderStatus));
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

        /// <summary> 更新訂單狀態(主管) </summary>
        public static bool UpdateOrderStatusForManager(string orderNo, DateTime shipmeneDate, int orderStatus, string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [FinalProject].[dbo].[CustomerOrders]
                                    SET
                                         ShipmentDate = @shipmeneDate,
                                         OrderStatus = @orderStatus,
                                         Remark = @remark
                                     WHERE
                                          OrderNo = @orderNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));
            list.Add(new SqlParameter("@shipmeneDate", shipmeneDate));
            list.Add(new SqlParameter("@orderStatus", orderStatus));
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

        /// <summary> 更新訂單狀態(倉儲主管) </summary>
        public static bool UpdateOrderStatusForNoStock(string orderNo, DateTime arriveDate, int orderStatus, string remark)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [FinalProject].[dbo].[CustomerOrders]
                                    SET
                                         ArriveDate = @arriveDate,
                                         OrderStatus = @orderStatus,
                                         Remark = @remark
                                     WHERE
                                          OrderNo = @orderNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));
            list.Add(new SqlParameter("@arriveDate", arriveDate));
            list.Add(new SqlParameter("@orderStatus", orderStatus));
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
        
        /// <summary> 更新訂單狀態 (顧客用) </summary>
        public static bool CompleteOrderStatus(string orderNo, DateTime arriveDate, int orderStatus)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [FinalProject].[dbo].[CustomerOrders]
                                    SET
                                         ArriveDate = @arriveDate,
                                         OrderStatus = @orderStatus
                                     WHERE
                                          OrderNo = @orderNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@orderNo", orderNo));
            list.Add(new SqlParameter("@arriveDate", arriveDate));
            list.Add(new SqlParameter("@orderStatus", orderStatus));
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
