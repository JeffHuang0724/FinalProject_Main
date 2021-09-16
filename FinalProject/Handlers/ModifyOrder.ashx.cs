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
    /// CompleteOrder 的摘要描述
    /// </summary>
    public class CompleteOrder : IHttpHandler
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
                    string orderNo = orderModel.OrderNo;
                    int modifyCode = orderModel.ModifyCode;
                    string branchNo = drEmployeeInfo["BranchNo"].ToString();
                    DataRow drOrderInfo = CustomerOrderManager.GetOrderInfo(orderNo);
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


                    int orderStatus = Convert.ToInt32(drOrderInfo["OrderStatus"].ToString());
                    if (modifyCode != 1 && modifyCode != 5)
                    {
                        string rsStr = "{\"ErrorMessage\":\"內容格式有誤，請確認\"}";
                        JObject jobject = JObject.Parse(rsStr);

                        context.Response.ContentType = "application/json";
                        context.Response.Write(jobject);
                        context.Response.StatusCode = 400;
                        return;
                    }
                    if (modifyCode == 1 && orderStatus != 4)
                    {
                        string rsStr = "{\"ErrorMessage\":\"訂單尚未出貨，無法完成訂單\"}";
                        JObject jobject = JObject.Parse(rsStr);

                        context.Response.ContentType = "application/json";
                        context.Response.Write(jobject);
                        context.Response.StatusCode = 200;
                        return;
                    }
                    if (modifyCode == 5 && orderStatus != 2)
                    {
                        string rsStr = "{\"ErrorMessage\":\"訂單已接單，無法取消\"}";
                        JObject jobject = JObject.Parse(rsStr);

                        context.Response.ContentType = "application/json";
                        context.Response.Write(jobject);
                        context.Response.StatusCode = 200;
                        return;
                    }


                    DateTime arriveDate = DateTime.Now;
                    if (CustomerOrderManager.CompleteOrderStatus(orderNo, arriveDate, modifyCode))
                    {
                        if(modifyCode == 1)
                        {
                            string rsString = "{" +
                                                   $"\"OrderNo\":\"{orderNo}\", " +
                                                   $"\"Message\":\"Success\"" +
                                                   "}";

                            JObject json = JObject.Parse(rsString);
                            context.Response.ContentType = "application/json";
                            context.Response.Write(json);
                            return;
                        }

                        //如顧客取消訂單，則連帶刪除銷售訂單
                        if (modifyCode == 5)
                        {
                            if (SalesOrderManager.DeleteSalesOrder(orderNo))
                            {
                                string rsString = "{" +
                                                   $"\"OrderNo\":\"{orderNo}\", " +
                                                   $"\"Message\":\"Success\"" +
                                                   "}";

                                JObject json = JObject.Parse(rsString);
                                context.Response.ContentType = "application/json";
                                context.Response.Write(json);
                                return;
                            } else
                            {
                                string rsString = "{\"ErrorMessage\":\"系統錯誤，請洽系統服務人員\"}";
                                JObject json = JObject.Parse(rsString);

                                context.Response.ContentType = "application/json";
                                context.Response.Write(json);
                                context.Response.StatusCode = 500;
                                return;
                            }
                        }
                    }
                    else
                    {
                        string rsString = "{\"ErrorMessage\":\"系統錯誤，請洽系統服務人員\"}";
                        JObject json = JObject.Parse(rsString);

                        context.Response.ContentType = "application/json";
                        context.Response.Write(json);
                        context.Response.StatusCode = 500;
                        return;
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

        public class OrderModel
        {
            public string OrderNo { get; set; }
            public int ModifyCode { get; set; }
        }

        private string JsonSchemaString()
        {
            string jsonSchema = @"{
                                    '$schema': 'http://json-schema.org/draft-04/schema#',
                                    'type': 'object',
                                    'properties': {
                                      'OrderNo': {
                                        'type': 'string'
                                      },
                                      'ModifyCode': {
                                        'type': 'string'
                                      }
                                    },
                                    'required': [
                                      'OrderNo',
                                      'ModifyCode'
                                    ]
                                  }";
            return jsonSchema;
        }
    }
}