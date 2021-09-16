using FinalProject.Auth;
using FinalProject.DBSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject.SystemAdmin.User
{
    public partial class ChangePwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 驗證登入
            if (!AuthManager.IsLogined())
            {
                Response.Redirect("/Default.aspx");
                return;
            }

            var currentUser = AuthManager.GetCurentUser();

            if (currentUser == null)
            {
                Response.Redirect("/Default.aspx");
                return;
            }

            // 銷售部門頁面顯示
            if (currentUser.Department == "Sales")
            {
                this.phSalesManu.Visible = true;
                this.phWarehouseManu.Visible = false;
            }
            // 倉儲部門頁面顯示
            else if (currentUser.Department == "Warehouse")
            {
                this.phSalesManu.Visible = false;
                this.phWarehouseManu.Visible = true;
            }

            // 主管頁面顯示
            if (currentUser.EmployeeLevel == "2")
            {
                this.phManager.Visible = true;
            }
            else
            {
                this.phManager.Visible = false;
            }

            this.lblNowDate.Text = $"現在時間： {DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm")}";

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // 新增List、確認輸入內容，並將錯誤訊息顯示
            List<string> msgList = new List<string>();
            if (!this.CheckInput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br />", msgList);
                return;
            }

            // 訊息提示
            ClientScript.RegisterStartupScript(this.GetType(), "訊息提示", "alert('你確定要變更密碼?');", true);

            // 確認是否為本人
            var currentUser = AuthManager.GetCurentUser();
            if (currentUser == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            // 取得會員ID ，藉以更新密碼，並將訊息提示給使用者
            string employeeID = currentUser.EmployeeID;
            if (EmployeeInfoManager.ChangePassword(employeeID, this.txtNewPwd.Text))
            {
                // 訊息提示
                ClientScript.RegisterStartupScript(this.GetType(), "訊息提示", "alert('密碼變更成功');", true);
                Response.Redirect($"/SystemAdmin/User/UserInfo.aspx");
            }
            else
            {
                // 訊息提示
                ClientScript.RegisterStartupScript(this.GetType(), "訊息提示", "alert('密碼變更失敗');", true);
                return;
            }
        }

        /// <summary>確認檢核Input 內容，回傳布林值以及錯誤訊息</summary>
        private bool CheckInput(out List<string> errMsgList)
        {
            List<string> msgList = new List<string>();
            //確認欄位是否為空
            if (string.IsNullOrWhiteSpace(txtOldPwd.Text) || string.IsNullOrWhiteSpace(txtChkOldPwd.Text) || string.IsNullOrWhiteSpace(txtNewPwd.Text))
            {
                msgList.Add("密碼不得為空，請重新確認");
                errMsgList = msgList;
                return false;
            }
            // 確認原密碼以及確認密碼 兩者是否一致
            if (string.Compare(this.txtOldPwd.Text, this.txtChkOldPwd.Text) != 0)
            {
                msgList.Add("密碼不一致，請重新確認");
                errMsgList = msgList;
                return false;
            }
            else
            {
                // 確認與資料庫密碼是否相符
                var currentUser = AuthManager.GetCurentUser();
                if (String.Compare(currentUser.EmployeePassword, this.txtOldPwd.Text) != 0)
                {
                    msgList.Add("密碼不符，請重新確認");
                    errMsgList = msgList;
                    return false;
                }
            }

            //確認新密碼是否介於8~16個字
            if (this.txtNewPwd.Text.Replace(" ", "").Length < 8 || this.txtNewPwd.Text.Replace(" ", "").Length > 16)
            {
                msgList.Add("密碼長度須為8~16個字，請重新確認");
                errMsgList = msgList;
                return false;
            }
            errMsgList = msgList;
            if (msgList.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }
    }
}