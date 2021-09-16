using FinalProject.DBSource;
using FinalProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace FinalProject.Handlers
{
    /// <summary>
    /// Order 的摘要描述
    /// </summary>
    public class Order : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //check actGUID
            string actGUID;
            try
            {
                actGUID = context.Request.Headers.GetValues("KeyID").First();
            }
            catch (Exception)
            {
                string rsString = "{\"ErrorMessage\":\"傳入內容有誤請確認\"}";
                JObject json = JObject.Parse(rsString);
                
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
                context.Response.StatusCode = 401;
                return;
            }
            DataRow drEmployeeInfo = ChainsEmployeeInfoManager.GetChainsEmployeeInfoByGUID(actGUID);
            if (drEmployeeInfo == null)
            {
                string rsString = "{\"ErrorMessage\":\"KeyID有誤\"}";
                JObject json = JObject.Parse(rsString);
                
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
                context.Response.StatusCode = 401;
                return;
            }

            DateTime dtNow = DateTime.Now;
            DateTime actTime = Convert.ToDateTime(drEmployeeInfo["ActTime"]);
            Double ts = (dtNow - actTime).TotalMinutes;

            if (ts > 10)
            {
                string rsString = "{\"ErrorMessage\":\"已超過時效，請重新登入\"}";
                JObject json = JObject.Parse(rsString);
                
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
                context.Response.StatusCode = 401;
                return;
            }
            else
            {
                // 讀取Request JSON Content
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    //Validate JSON Schema
                    string content = reader.ReadToEnd();
                    string orderJsonSchema = JsonSchemaString();
                    JSchema schema = JSchema.Parse(orderJsonSchema);
                    JObject inputContent = JObject.Parse(content);
                    bool valid = inputContent.IsValid(schema);
                    if (!valid)
                    {
                        string rsStr = "{\"ErrorMessage\":\"內容格式有誤，請確認\"}";
                        JObject jobject = JObject.Parse(rsStr);
                        
                        context.Response.ContentType = "application/json";
                        context.Response.Write(jobject);
                        context.Response.StatusCode = 400;
                        return;
                    }

                    OrderModel orderModel = JsonConvert.DeserializeObject<OrderModel>(content);

                    // 先確認 OrderItem 是否存在，數量是否少於庫存
                    for (int i = 0; i < orderModel.OrderDetail.Length; i++)
                    {
                        OrderDetail orderDetail = orderModel.OrderDetail[i];
                        string itemNo = orderDetail.ItemNo;
                        if (string.IsNullOrWhiteSpace(itemNo) || orderDetail.ItemCount < 1)
                        {
                            string rsStr = "{\"ErrorMessage\":\"下訂品項不得為空，且數量不得少於1，請確認\"}";
                            JObject jobject = JObject.Parse(rsStr);
                            
                            context.Response.ContentType = "application/json";
                            context.Response.Write(jobject);
                            context.Response.StatusCode = 400;
                            return;
                        }

                        DataRow drItemInfo = WarehouseItemManager.GetWarehouseItemInfo(itemNo);
                        if (drItemInfo == null)
                        {
                            string rsStr = "{\"ErrorMessage\":\"下訂品項有誤，請確認\"}";
                            JObject jobject = JObject.Parse(rsStr);
                            
                            context.Response.ContentType = "application/json";
                            context.Response.Write(jobject);
                            context.Response.StatusCode = 400;
                            return;
                        }
                        else
                        {
                            if (orderDetail.ItemCount > Convert.ToInt32(drItemInfo["StockCount"].ToString()))
                            {
                                string rsStr = "{\"ErrorMessage\":\"下訂品項數量有誤，請確認\"}";
                                JObject jobject = JObject.Parse(rsStr);
                                
                                context.Response.ContentType = "application/json";
                                context.Response.Write(jobject);
                                context.Response.StatusCode = 400;
                                return;
                            }
                        }
                    }
                    // 確認銷售人員是否正確
                    string orderEmployeeID = drEmployeeInfo["EmployeeID"].ToString();
                    string orderBranch = drEmployeeInfo["BranchNo"].ToString();
                    string salesNo = orderModel.SalesNo; ;
                    DataRow drSalesInfo = EmployeeInfoManager.GetSalesInfo(salesNo);
                    string branchArea = orderBranch.Substring(0, 1);
                    if (drSalesInfo == null || drSalesInfo["Department"].ToString() != "Sales" || drSalesInfo["EmployeeLevel"].ToString() != "1")
                    {
                        string rsStr = "{\"ErrorMessage\":\"銷售人員資訊有誤，請確認\"}";
                        JObject jobject = JObject.Parse(rsStr);

                        context.Response.ContentType = "application/json";
                        context.Response.Write(jobject);
                        context.Response.StatusCode = 400;
                        return;
                    }
                    // 訂單不得跨區
                    if (branchArea == "N")
                    {
                        if (drSalesInfo["Section"].ToString() != "S01")
                        {
                            string rsStr = "{\"ErrorMessage\":\"銷售人員資訊有誤，請確認\"}";
                            JObject jobject = JObject.Parse(rsStr);

                            context.Response.ContentType = "application/json";
                            context.Response.Write(jobject);
                            context.Response.StatusCode = 400;
                            return;
                        }
                    }
                    else if (branchArea == "S")
                    {
                        if (drSalesInfo["Section"].ToString() != "S02")
                        {
                            string rsStr = "{\"ErrorMessage\":\"銷售人員資訊有誤，請確認\"}";
                            JObject jobject = JObject.Parse(rsStr);

                            context.Response.ContentType = "application/json";
                            context.Response.Write(jobject);
                            context.Response.StatusCode = 400;
                            return;
                        }
                    }
                    // 訂單製作
                    DateTime orderDate = orderModel.OrderDate;
                    int orderStatus = 2;
                    string remark = "分店:" + orderModel.Remark + "/r/n";
                    CustomerOrderManager.CreateOrder(orderEmployeeID, orderBranch, orderDate, orderStatus,remark);
                    // 取得訂單號碼
                    DataRow drOrderInfo = CustomerOrderManager.GetOrderDefault(orderEmployeeID, orderBranch, orderDate, remark);
                    if (drOrderInfo == null)
                    {
                        string rsStr = "{\"ErrorMessage\":\"新增訂單錯誤，請洽系統管理員\"}";
                        JObject jobject = JObject.Parse(rsStr);
                        
                        context.Response.ContentType = "application/json";
                        context.Response.Write(jobject);
                        context.Response.StatusCode = 500;
                        return;
                    }
                    string orderNo = drOrderInfo["OrderNo"].ToString();
                    // 迴圈新增訂單品項
                    for (int i = 0; i < orderModel.OrderDetail.Length; i++)
                    {
                        OrderDetail orderDetail = orderModel.OrderDetail[i];
                        string itemNo = orderDetail.ItemNo;
                        DataRow drItemInfo = WarehouseItemManager.GetWarehouseItemInfo(itemNo);
                        int itemCount = orderDetail.ItemCount;
                        int itemPrice = Convert.ToInt32(drItemInfo["ItemPrice"].ToString());
                        string itemRemark = orderDetail.Remark;

                        CustomerOrderManager.CreateOrderDetail(orderNo, itemNo, itemCount, itemPrice, itemRemark);
                    }

                    //由科組取得主管資訊
                    string section = drSalesInfo["Section"].ToString();
                    DataRow drSalesManagerInfo = EmployeeInfoManager.GetEmployeeInfoBySection(section);
                    string salesManagerNo = drSalesManagerInfo["ManagerNo"].ToString();
                    
                    SalesOrderManager.CreateSalesOrder(orderNo, salesNo, salesManagerNo);

                    context.Response.ContentType = "application/json";
                    string rsString = "{\"OrderNo\":\"" + orderNo + "\"}";
                    JObject json = JObject.Parse(rsString);
                    context.Response.Write(json);
                }
                // 更新GUID 時間
                ChainsEmployeeInfoManager.UpdateChainsEmployeeGUID(actGUID, dtNow);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string JsonSchemaString()
        {
            string jsonSchema = @"{
                                    '$schema': 'http://json-schema.org/draft-04/schema#',
                                    'type': 'object',
                                    'properties': {
                                      'OrderDate': {
                                        'type': 'string'
                                      },
                                      'SalesNo': {
                                        'type': 'string'
                                      },
                                      'OrderDetail': {
                                        'type': 'array',
                                        'items': [
                                          {
                                            'type': 'object',
                                            'properties': {
                                              'ItemNo': {
                                                'type': 'string'
                                              },
                                              'ItemCount': {
                                                'type': 'integer'
                                              },
                                              'Remark': {
                                                'type': 'string'
                                              }
                                            },
                                            'required': [
                                              'ItemNo',
                                              'ItemCount',
                                              'Remark'
                                            ]
                                          },
                                          {
                                            'type': 'object',
                                            'properties': {
                                              'ItemNo': {
                                                'type': 'string'
                                              },
                                              'ItemCount': {
                                                'type': 'integer'
                                              },
                                              'Remark': {
                                                'type': 'string'
                                              }
                                            },
                                            'required': [
                                              'ItemNo',
                                              'ItemCount',
                                              'Remark'
                                            ]
                                          }
                                        ]
                                      },
                                      'Remark': {
                                        'type': 'string'
                                      }
                                    },
                                    'required': [
                                      'OrderDate',
                                      'SalesNo',
                                      'OrderDetail',
                                      'Remark'
                                    ]
                                  }";
            return jsonSchema;
        }
    }
}