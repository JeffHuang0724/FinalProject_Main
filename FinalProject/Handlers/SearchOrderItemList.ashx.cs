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
    /// SearchOrderItemList 的摘要描述
    /// </summary>
    public class SearchOrderItemList : IHttpHandler
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
            } else
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
                    DataTable dtOrderDetailInfo = CustomerOrderManager.GetOrderDetailInfo(orderNo);
                    string jsonTxt = JsonConvert.SerializeObject(dtOrderDetailInfo);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(jsonTxt);

                    ChainsEmployeeInfoManager.UpdateChainsEmployeeGUID(actGUID, dtNow);
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

        public class OrderModel
        {
            public string OrderNo { get; set; }
        }
    }
}