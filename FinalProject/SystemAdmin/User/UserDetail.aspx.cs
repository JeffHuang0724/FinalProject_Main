using FinalProject.Auth;
using FinalProject.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject.SystemAdmin.User
{
    public partial class UserDetail : System.Web.UI.Page
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

            string editUser = Request.QueryString["EID"];
            if (!this.IsPostBack)
            {
                //新增模式
                if (string.IsNullOrWhiteSpace(editUser))
                {
                    if(currentUser.EmployeeLevel != "2")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('非管理階級無法新增員工');", true);
                        Response.Redirect("/SystemAdmin/UserDefault.aspx");
                        return;
                    }
                    this.lblEmployeeID.Text = "尚待資料建檔";
                    this.plcEditEmployeename.Visible = true;
                    this.plcReadEmployeename.Visible = false;
                    this.txtEmployeeName.Text = string.Empty;
                    this.plcEditPhone.Visible = true;
                    this.plcReadPhone.Visible = false;
                    this.txtPhone.Text = string.Empty;
                    this.plcEditAccount.Visible = true;
                    this.plcReadAccount.Visible = false;
                    this.txtAccount.Text = string.Empty;
                    this.btnChangePwd.Visible = false;
                    this.btnRemakePwd.Visible = false;
                    string department = currentUser.Department;
                    if (department == "Sales")
                        this.lblDepartment.Text = "銷售部門";
                    else if (department == "Warehouse")
                        this.lblDepartment.Text = "倉儲部門";
                    else
                        this.lblDepartment.Text = "尚待指派";
                    this.lblSection.Text = currentUser.Section;
                    this.lblEmployeeLevel.Text = "1";
                }
                //編輯模式
                else if (currentUser.EmployeeID == editUser)
                {
                    // 取得頁面資訊
                    this.lblEmployeeID.Text = currentUser.EmployeeID;
                    this.plcEditEmployeename.Visible = true;
                    this.plcReadEmployeename.Visible = false;
                    this.txtEmployeeName.Text = currentUser.EmployeeName;
                    this.plcEditPhone.Visible = true;
                    this.plcReadPhone.Visible = false;
                    this.txtPhone.Text = currentUser.EmployeePhone;
                    this.plcEditAccount.Visible = true;
                    this.plcReadAccount.Visible = false;
                    this.txtAccount.Text = currentUser.EmployeeAccount;
                    this.txtAccount.ReadOnly = true;
                    this.btnRemakePwd.Visible = false;
                    if (currentUser.Department == "Sales")
                        this.lblDepartment.Text = "銷售部門";
                    else if (currentUser.Department == "Warehouse")
                        this.lblDepartment.Text = "倉儲部門";
                    else
                        this.lblDepartment.Text = "尚待指派";
                    this.lblSection.Text = currentUser.Section;
                    this.lblEmployeeLevel.Text = currentUser.EmployeeLevel;
                }
                // 瀏覽模式
                else
                {
                    string employeeID = this.Request.QueryString["EID"];
                    // 取得頁面資訊
                    DataRow drEmployeeInfo = EmployeeInfoManager.GetEmployeeByEID(employeeID);
                    if (drEmployeeInfo == null)
                    {
                        this.plcEmployeeInfo.Visible = false;
                        this.plcNoData.Visible = true;
                        return;
                    }
                    string employeeName = drEmployeeInfo["employeeName"].ToString();
                    string employeePhone = drEmployeeInfo["employeePhone"].ToString();
                    string employeeAccount = drEmployeeInfo["employeeAccount"].ToString();
                    string department = drEmployeeInfo["department"].ToString();
                    string section = drEmployeeInfo["section"].ToString();
                    string employeeLevel = drEmployeeInfo["employeeLevel"].ToString();


                    this.lblEmployeeID.Text = employeeID;
                    this.plcEditEmployeename.Visible = false;
                    this.plcReadEmployeename.Visible = true;
                    this.lblEmployeeName.Text = employeeName;
                    this.plcEditPhone.Visible = false;
                    this.plcReadPhone.Visible = true;
                    this.lblPhone.Text = employeePhone;
                    this.plcEditAccount.Visible = false;
                    this.plcReadAccount.Visible = true;
                    this.lblAccount.Text = employeeAccount;
                    if (department == "Sales")
                        this.lblDepartment.Text = "銷售部門";
                    else if (department == "Warehouse")
                        this.lblDepartment.Text = "倉儲部門";
                    else
                        this.lblDepartment.Text = "尚待指派";
                    this.lblSection.Text = section;
                    this.lblEmployeeLevel.Text = employeeLevel;

                    this.btnSave.Visible = false;
                    this.btnChangePwd.Visible = false;

                    //確認當前使用者是否為此頁面資訊的主管，如果是則顯示重製密碼按鈕
                    string userSection = currentUser.Section;
                    string userLevel = currentUser.EmployeeLevel;
                    if(userSection == section && userLevel == "2")
                    {
                        this.btnRemakePwd.Visible = true;
                    }
                }
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


            // 新增員工功能 (主管頁面顯示)
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
            List<string> msgList = new List<string>();
            // Check Input
            if (!this.CheckInput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br />", msgList);
                return;
            }

            EmployeeInfoModel currentUser = AuthManager.GetCurentUser();
            if (currentUser == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            // 取得資訊
            string employeeName = this.txtEmployeeName.Text;
            string employeePhone = this.txtPhone.Text;
            string employeeAccount = this.txtAccount.Text;
            string department = currentUser.Department;
            string section = currentUser.Section;
            // 預設職等皆為1
            string employeeLevel = "1";

            // 新增模式
            if (string.IsNullOrWhiteSpace(this.Request.QueryString["EID"]))
            {
                // 確認帳號是否重複
                DataRow drEmployee = EmployeeInfoManager.GetEmployeeByAccount(employeeAccount);
                if (drEmployee != null)
                {
                    this.ltMsg.Text = "<p style=\"color: red; background-color: yellow; font-size: large\">帳號重複，請重新輸入</p>";
                    return;
                }
                EmployeeInfoManager.CreateEmployeeInfo(employeeName, employeeAccount, department, section, employeeLevel, employeePhone);
                Response.Redirect("/SystemAdmin/User/UserList.aspx");
            }

            // 更新模式
            else
            {
                string employeeID = currentUser.EmployeeID;
                if (EmployeeInfoManager.UpdateEmployeeInfo(employeeID, employeeName, employeeAccount, employeePhone))
                {
                    Response.Redirect("/SystemAdmin/User/UserInfo.aspx");
                }
                else
                {
                    this.ltMsg.Text = "<p style=\"color: red; background-color: yellow; font-size: large\">更新失敗</p>";
                }
            }
        }

        private bool CheckInput(out List<string> errMsgList)
        {
            List<string> msgList = new List<string>();
            // check Account
            if (string.IsNullOrWhiteSpace(this.txtAccount.Text))
            {
                msgList.Add("帳號為必填欄位");
            }
            // check Name
            if (string.IsNullOrWhiteSpace(this.txtEmployeeName.Text))
            {
                msgList.Add("姓名為必填欄位");
            }
            // check Email
            if (string.IsNullOrWhiteSpace(this.txtPhone.Text))
            {
                msgList.Add("電話為必填欄位");
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

        protected void btnChangePwd_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/User/ChangePwd.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }

        protected void btnRemakePwd_Click(object sender, EventArgs e)
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

            // 重製員工密碼，預設12345678
            string employeeID = this.Request.QueryString["EID"];
            string defaultPwd = "12345678";
            if (!EmployeeInfoManager.ChangePassword(employeeID, defaultPwd))
            {
                this.ltMsg.Text = "<p style=\"color: red; background-color: yellow; font-size: large\">密碼重製失敗，請洽資訊人員</p>";
            } else
            {
                this.ltMsg.Text = "<p style=\"color: red; background-color: yellow; font-size: large\">密碼已重製</p>";
            } 
        }
    }
}