using FinalProject.DBSource;
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
    /// SearchOrder 的摘要描述
    /// </summary>
    public class SearchOrder : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //check actGUID
            string actGUID = string.Empty;
            try
            {
                actGUID = context.Request.Headers.GetValues("KeyID").First();
            }
            catch (Exception ex)
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

                    searchOrderModel orderModel = JsonConvert.DeserializeObject<searchOrderModel>(content);
                    string orderNo = orderModel.OrderNo;
                    string branchNo = drEmployeeInfo["BranchNo"].ToString();
                    DataRow drOrderInfo = CustomerOrderManager.GetOrderInfo(orderNo);
                    DataTable dtOrderDetailInfo = CustomerOrderManager.GetOrderDetailInfo(orderNo);
                    if (drOrderInfo == null)
                    {
                        string rsString = "{\"ErrorMessage\":\"查無此OrderNo資訊，請確認\"}";
                        JObject json = JObject.Parse(rsString);
                        context.Response.ContentType = "application/json";
                        context.Response.Write(json);
                        context.Response.StatusCode = 401;
                        return;
                    }
                    else if (drOrderInfo["OrderBranch"].ToString() != branchNo)
                    {
                        string rsString = "{\"ErrorMessage\":\"非同分店之訂單，無法使用此功能，請確認\"}";
                        JObject json = JObject.Parse(rsString);
                        context.Response.ContentType = "application/json";
                        context.Response.Write(json);
                        context.Response.StatusCode = 401;
                        return;
                    }
                    else
                    {
                        orderNo = drOrderInfo.Field<string>("OrderNo");
                        string orderEmployee = drOrderInfo.Field<string>("OrderEmployeeID");
                        string orderBranch = drOrderInfo.Field<string>("OrderBranch");
                        string orderDate;
                        DateTime dtOrderDate;
                        if (DateTime.TryParse(drOrderInfo["OrderDate"].ToString(), out dtOrderDate))
                        {
                            orderDate = dtOrderDate.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            orderDate = string.Empty;
                        }

                        string preShipmentDate;
                        DateTime dtPreShipmentDate;
                        if (DateTime.TryParse(drOrderInfo["PreShipmentDate"].ToString(), out dtPreShipmentDate))
                        {
                            preShipmentDate = dtPreShipmentDate.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            preShipmentDate = string.Empty;
                        }

                        string shipmentDate;
                        DateTime dtShipmentDate;
                        if (DateTime.TryParse(drOrderInfo["ShipmentDate"].ToString(), out dtShipmentDate))
                        {
                            shipmentDate = dtShipmentDate.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            shipmentDate = string.Empty;
                        }

                        string arriveDate;
                        DateTime dtArriveDate;
                        if (DateTime.TryParse(drOrderInfo["ArriveDate"].ToString(), out dtArriveDate))
                        {
                            arriveDate = dtArriveDate.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            arriveDate = string.Empty;
                        }

                        string remark = drOrderInfo.Field<string>("Remark");
                        string orderStatus = drOrderInfo.Field<int>("OrderStatus").ToString();

                        string orderDetail = string.Empty;
                        for (int i = 0; i < dtOrderDetailInfo.Rows.Count; i++)
                        {
                            orderDetail += "{" +
                                $"\"ItemNo\":\"{dtOrderDetailInfo.Rows[i]["ItemNo"]}\"," +
                                $"\"ItemCount\":\"{dtOrderDetailInfo.Rows[i]["ItemCount"]}\"," +
                                $"\"ItemPrice\":\"{dtOrderDetailInfo.Rows[i]["ItemPrice"]}\"," +
                                $"\"WetherStock\":\"{dtOrderDetailInfo.Rows[i]["WetherStock"]}\"," +
                                $"\"Remark\":\"{dtOrderDetailInfo.Rows[i]["Remark"]}\","
                                + "}";
                            if (i < dtOrderDetailInfo.Rows.Count)
                            {
                                orderDetail += ",";
                            }
                        }
                        string rsString = "{" +
                            $"\"OrderNo\":\"{orderNo}\", " +
                            $"\"OrderEmployee\": \"{orderEmployee}\", " +
                            $"\"OrderBranch\":\"{orderBranch}\"," +
                            $"\"OrderDate\":\"{orderDate}\"," +
                            $"\"PreShipmentDate\":\"{preShipmentDate}\"," +
                            $"\"ShipmentDate\":\"{shipmentDate}\"," +
                            $"\"ArriveDate\":\"{arriveDate}\"," +
                            $"\"Remark\":\"{remark}\"," +
                            $"\"OrderStatus\":\"{orderStatus}\"," +
                            $"\"OrderDetail\":[{orderDetail}]"
                            + "}";
                        JObject json = JObject.Parse(rsString);  
                        context.Response.ContentType = "application/json";
                        context.Response.Write(json);
                    }
                }
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
                                      'OrderNo': {
                                        'type': 'string'
                                      }
                                    },
                                    'required': [
                                      'OrderNo'
                                    ]
                                  }";
            return jsonSchema;
        }
    }

    public class searchOrderModel
    {
        public string OrderNo { get; set; }
    }
}