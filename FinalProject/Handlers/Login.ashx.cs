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
    /// Login 的摘要描述
    /// </summary>
    public class Login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
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
                LoginModel orderModel = JsonConvert.DeserializeObject<LoginModel>(content);
                string account = orderModel.Account;
                string password = orderModel.Password;

                DataRow drChainsEmployInfo = ChainsEmployeeInfoManager.GetChainsEmployeeLoginInfo(account);
                if (drChainsEmployInfo == null)
                {
                    string rsStr = "{\"ErrorMessage\":\"查無此用戶，請確認\"}";
                    JObject jobject = JObject.Parse(rsStr);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(jobject);
                    context.Response.StatusCode = 401;
                    return;
                }
                if (drChainsEmployInfo["Password"].ToString() != password)
                {
                    string rsStr = "{\"ErrorMessage\":\"帳號、密碼有誤。請再確認\"}";
                    JObject jobject = JObject.Parse(rsStr);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(jobject);
                    context.Response.StatusCode = 401;
                    return;
                }

                Guid actGUID = Guid.NewGuid();
                DateTime actTime = DateTime.Now;
                if (!ChainsEmployeeInfoManager.CreateChainsEmployeeGUID(account, actGUID, actTime))
                {
                    context.Response.StatusCode = 500;
                    return;
                }
                else
                {
                    string rsString = "{\"KeyID\":\"" + actGUID + "\"}";
                    JObject json = JObject.Parse(rsString);

                    context.Response.ContentType = "Applicaiton/JSON";
                    context.Response.Write(json);
                }
            }
        }

        public class LoginModel
        {
            public string Account { get; set; }
            public string Password { get; set; }
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
                                      'Account': {
                                        'type': 'string'
                                      },
                                      'Password': {
                                        'type': 'string'
                                      }
                                    },
                                    'required': [
                                      'Account',
                                      'Password'
                                    ]
                                  }";
            return jsonSchema;
        }
    }
}