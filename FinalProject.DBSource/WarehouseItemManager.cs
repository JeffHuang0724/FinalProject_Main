using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DBSource
{
    public class WarehouseItemManager
    {
        /// <summary> 取得商品列表</summary>
        public static DataTable GetWarehouseItemList()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                            ItemNo
                                            ,Category
                                            ,ItemName
                                            ,StockCount
                                            ,ItemPrice
                                        FROM [Items]
                                        ORDER BY Category";

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

        /// <summary> 取得單項商品資訊 </summary>
        public static DataRow GetWarehouseItemInfo(string itemNo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT 
                                             ItemNo
                                            ,Category
                                            ,ItemName
                                            ,StockCount
                                            ,ItemPrice
                                         FROM [Items]
                                         WHERE ItemNo = @itemNo";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@itemNo", itemNo));

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

        /// <summary> 更新商品資訊 </summary>
        public static bool UpdateWarehouseItem(string itemNo, int stockCount, decimal itemPrice)
        {
            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"UPDATE [Items]
                                    SET
                                         StockCount = @stockCount
                                         ,ItemPrice = @itemPrice
                                     WHERE
                                          ItemNo = @itemNo;";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@stockCount", stockCount));
            list.Add(new SqlParameter("@itemPrice", itemPrice));
            list.Add(new SqlParameter("@itemNo", itemNo));
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
        /// <summary> 取得最後一筆商品編號 </summary>
        public static DataRow GetWarehouseItemLastNo()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString = $@"SELECT
                                            (SELECT TOP(1)
                                            	[ItemNo]
                                              FROM [Items]
                                              WHERE ItemNo LIKE 'M%'
                                              ORDER BY ItemNo DESC) AS MLastNo,
                                            (SELECT TOP(1)
                                            	[ItemNo]
                                              FROM [Items]
                                              WHERE ItemNo LIKE 'F%'
                                              ORDER BY ItemNo DESC) AS FLastNo,
                                            (SELECT TOP(1)
                                            	[ItemNo]
                                              FROM [Items]
                                              WHERE ItemNo LIKE 'O%'
                                              ORDER BY ItemNo DESC) AS OLastNo,
                                            (SELECT TOP(1)
                                            	[ItemNo]
                                              FROM [Items]
                                              WHERE ItemNo LIKE 'S%'
                                              ORDER BY ItemNo DESC) AS SLastNo";

            List<SqlParameter> list = new List<SqlParameter>();

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

        /// <summary> 新增品項 </summary>
        public static void CreateGoodsInfo(string itemNo, int category, string itemName, int stockCount, decimal itemPrice)
        {

            string connectionString = DBHelper.GetConnectionString();
            string queryString = $@"INSERT INTO [Items]
                                         (ItemNo
                                         ,Category
                                         ,ItemName
                                         ,StockCount
                                         ,ItemPrice)
                                     VALUES
                                          (@itemNo
                                         ,@category
                                         ,@itemName
                                         ,@stockCount
                                         ,@itemPrice);";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@itemNo", itemNo));
            list.Add(new SqlParameter("@category", category));
            list.Add(new SqlParameter("@itemName", itemName));
            list.Add(new SqlParameter("@stockCount", stockCount));
            list.Add(new SqlParameter("@itemPrice", itemPrice));
            try
            {
                DBHelper.CreateData(connectionString, queryString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
    }
}
