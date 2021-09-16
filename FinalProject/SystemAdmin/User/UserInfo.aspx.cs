using FinalProject.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject.SystemAdmin.User
{
    public partial class UserInfo : System.Web.UI.Page
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

            // 取得頁面資訊
            string employeeID = currentUser.EmployeeID;
            string employeeName = currentUser.EmployeeName;
            string employeePhone = currentUser.EmployeePhone;
            string department = currentUser.Department;
            string employeeLevel = currentUser.EmployeeLevel;

            this.lblEmployeeID.Text = employeeID;
            this.lblEmployeeName.Text = employeeName;
            this.lblEmployeePhone.Text = employeePhone;

            // 銷售部門頁面顯示
            if (department == "Sales")
            {
                this.phSalesManu.Visible = true;
                this.phWarehouseManu.Visible = false;
            }
            // 倉儲部門頁面顯示
            else if (department == "Warehouse")
            {
                this.phSalesManu.Visible = false;
                this.phWarehouseManu.Visible = true;
            }


            // 新增員工功能 (主管頁面顯示)
            if (employeeLevel == "2")
            {
                this.phManager.Visible = true;
            }
            else
            {
                this.phManager.Visible = false;
            }

            this.lblNowDate.Text = $"現在時間： {DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm")}";
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            var currentUser = AuthManager.GetCurentUser();
            Response.Redirect($"/SystemAdmin/User/UserDetail.aspx?EID={currentUser.EmployeeID}");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }
    }
}