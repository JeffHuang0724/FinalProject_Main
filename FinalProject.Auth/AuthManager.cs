using FinalProject.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FinalProject.Auth
{
    public class AuthManager
    {
        public static bool IsLogined()
        {
            if (HttpContext.Current.Session["Account"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary> 取得已登入的使用者資訊(如果沒登入就回傳null)</summary>
        public static EmployeeInfoModel GetCurentUser()
        {
            string EmployeeAccount = HttpContext.Current.Session["Account"] as string;

            if (EmployeeAccount == null)
                return null;

            DataRow userDr = EmployeeInfoManager.GetEmployeeByAccount(EmployeeAccount);

            if (userDr == null)
            {
                HttpContext.Current.Session["Account"] = null;
                return null;
            }

            EmployeeInfoModel model = new EmployeeInfoModel();
            model.EmployeeID = userDr["EmployeeID"].ToString();
            model.EmployeeName = userDr["EmployeeName"].ToString();
            model.EmployeeAccount = userDr["EmployeeAccount"].ToString();
            model.EmployeePassword = userDr["EmployeePassword"].ToString();
            model.Department = userDr["Department"].ToString();
            model.Section = userDr["Section"].ToString();
            model.EmployeeLevel = userDr["EmployeeLevel"].ToString();
            model.EmployeePhone = userDr["EmployeePhone"].ToString();

            return model;
        }
        /// <summary> 登入 </summary>
        public static bool TryLogin(string EmployeeAccount, string EmployeePassword, out string errMsg)
        {
            // check empty
            if (string.IsNullOrWhiteSpace(EmployeeAccount) || string.IsNullOrWhiteSpace(EmployeePassword))
            {
                errMsg = "Account / Password is required";
                return false;
            }

            var dr = EmployeeInfoManager.GetEmployeeByAccount(EmployeeAccount);
            // check dr null
            if (dr == null)
            {
                errMsg = "Account doesn't exisit";
                return false;
            }

            // check account / password
            if (string.Compare(dr["EmployeeAccount"].ToString(), EmployeeAccount, true) == 0 &&
                string.Compare(dr["EmployeePassword"].ToString(), EmployeePassword, false) == 0)
            {
                HttpContext.Current.Session["Account"] = dr["EmployeeAccount"].ToString();
                errMsg = string.Empty;
                return true;
            }
            else
            {
                errMsg = "Login fail. Please check Account / Password.";
                return false;
            }
        }
        /// <summary> 登出 </summary>
        public static void Logout()
        {
            HttpContext.Current.Session["Account"] = null;
        }
    }
}
