﻿using FinalProject.DBSource;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FinalProject.Handlers
{
    /// <summary>
    /// ItemsList 的摘要描述
    /// </summary>
    public class ItemsList : IHttpHandler
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

            DataRow drActInfo = ChainsEmployeeInfoManager.GetChainsEmployeeInfoByGUID(actGUID);
            if (drActInfo == null)
            {
                string rsString = "{\"ErrorMessage\":\"KeyID有誤\"}";
                JObject json = JObject.Parse(rsString);
                
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
                context.Response.StatusCode = 401;
                return;
            }
            DateTime dtNow = DateTime.Now;
            DateTime actTime = Convert.ToDateTime(drActInfo["ActTime"]);
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
                DataTable dtItemsList = WarehouseItemManager.GetWarehouseItemList();
                string jsonTxt = Newtonsoft.Json.JsonConvert.SerializeObject(dtItemsList);
                context.Response.ContentType = "application/json";
                context.Response.Write(jsonTxt);

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
    }
}