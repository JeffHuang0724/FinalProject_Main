using FinalProject.Auth;
using FinalProject.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject.SystemAdmin
{
    public partial class UserDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {// 驗證登入
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
            string department = currentUser.Department;
            string employeeLevel = currentUser.EmployeeLevel;

            // 銷售部門頁面顯示
            if (department == "Sales")
            {
                this.phSalesManu.Visible = true;
                this.phSalesOrderManagement.Visible = true;
                this.phWarehouseManu.Visible = false;
                this.phWarehouseOrderManagement.Visible = false;

                if (employeeLevel == "1")
                {
                    this.phManager.Visible = false;
                    DataRow dr = SalesOrderManager.GetSalesOrderAmount(employeeID);
                    if (dr == null)
                    {
                        this.lblUnFinishedOrderCount.Text = "總計 0 筆";
                        this.lblFinishedOrderCount.Text = "總計 0 筆";
                        this.lblTotalOrderCount.Text = "總計 0 筆";
                    }
                    else
                    {
                        int unFinishedOrderCount = Convert.ToInt32(dr["UnFinishedOrderCount"]);
                        int finishedOrderCount = Convert.ToInt32(dr["FinishedOrderCount"]);
                        int totalOrderCount = unFinishedOrderCount + finishedOrderCount;

                        this.lblUnFinishedOrderCount.Text = $"總計 {unFinishedOrderCount} 筆";
                        this.lblFinishedOrderCount.Text = $"總計 {finishedOrderCount} 筆";
                        this.lblTotalOrderCount.Text = $"總計 {totalOrderCount} 筆";
                    }
                }
                else if (employeeLevel == "2")
                {
                    this.phManager.Visible = true;
                    DataRow dr = SalesOrderManager.GetSalesSectionOrderAmount(employeeID);
                    if (dr == null)
                    {
                        this.lblUnFinishedOrderCount.Text = "總計 0 筆";
                        this.lblFinishedOrderCount.Text = "總計 0 筆";
                        this.lblTotalOrderCount.Text = "總計 0 筆";
                        this.lblSecUnFinishedOrderCount.Text = "總計 0 筆";
                        this.lblSecFinishedOrderCount.Text = "總計 0 筆";
                        this.lblSecTotalOrderCount.Text = "總計 0 筆";
                    }
                    else
                    {
                        int unFinishedOrderCount = Convert.ToInt32(dr["UnFinishedOrderCount"]);
                        int finishedOrderCount = Convert.ToInt32(dr["FinishedOrderCount"]);
                        int totalOrderCount = unFinishedOrderCount + finishedOrderCount;

                        int secUnFinishedOrderCount = Convert.ToInt32(dr["SectionUnFinishedOrderCount"]);
                        int secFinishedOrderCount = Convert.ToInt32(dr["SectionFinishedOrderCount"]);
                        int secTotalOrderCount = secUnFinishedOrderCount + secFinishedOrderCount;

                        this.lblUnFinishedOrderCount.Text = $"總計 {unFinishedOrderCount} 筆";
                        this.lblFinishedOrderCount.Text = $"總計 {finishedOrderCount} 筆";
                        this.lblTotalOrderCount.Text = $"總計 {totalOrderCount} 筆";
                        this.lblSecUnFinishedOrderCount.Text = $"總計 {secUnFinishedOrderCount} 筆";
                        this.lblSecFinishedOrderCount.Text = $"總計 {secFinishedOrderCount} 筆";
                        this.lblSecTotalOrderCount.Text = $"總計 {secTotalOrderCount} 筆";
                    }
                }
            }
            // 倉儲部門頁面顯示
            else if (department == "Warehouse")
            {
                this.phSalesManu.Visible = false;
                this.phSalesOrderManagement.Visible = false;
                this.phWarehouseManu.Visible = true;
                this.phWarehouseOrderManagement.Visible = true;

                if(employeeLevel == "1")
                {
                    this.phManager.Visible = false;
                    DataRow dr = WarehouseOrderManager.GetWarehouseOrderAmount(employeeID);
                    if (dr == null)
                    {
                        this.lblUnFinishedOrderCount.Text = "總計 0 筆";
                        this.lblFinishedOrderCount.Text = "總計 0 筆";
                        this.lblTotalOrderCount.Text = "總計 0 筆";
                    }
                    else
                    {
                        int unFinishedOrderCount = Convert.ToInt32(dr["UnFinishedOrderCount"]);
                        int finishedOrderCount = Convert.ToInt32(dr["FinishedOrderCount"]);
                        int totalOrderCount = unFinishedOrderCount + finishedOrderCount;

                        this.lblUnFinishedOrderCount.Text = $"總計 {unFinishedOrderCount} 筆";
                        this.lblFinishedOrderCount.Text = $"總計 {finishedOrderCount} 筆";
                        this.lblTotalOrderCount.Text = $"總計 {totalOrderCount} 筆";
                    }
                } else if(employeeLevel == "2")
                {
                    this.phManager.Visible = true;
                    DataRow dr = WarehouseOrderManager.GetWarehouseSectionOrderAmount(employeeID);
                    if (dr == null)
                    {
                        this.lblUnFinishedOrderCount.Text = "總計 0 筆";
                        this.lblFinishedOrderCount.Text = "總計 0 筆";
                        this.lblTotalOrderCount.Text = "總計 0 筆";
                        this.lblSecUnFinishedOrderCount.Text = "總計 0 筆";
                        this.lblSecFinishedOrderCount.Text = "總計 0 筆";
                        this.lblSecTotalOrderCount.Text = "總計 0 筆";
                    }
                    else
                    {
                        int unFinishedOrderCount = Convert.ToInt32(dr["UnFinishedOrderCount"]);
                        int finishedOrderCount = Convert.ToInt32(dr["FinishedOrderCount"]);
                        int totalOrderCount = unFinishedOrderCount + finishedOrderCount;

                        int secUnFinishedOrderCount = Convert.ToInt32(dr["SectionUnFinishedOrderCount"]);
                        int secFinishedOrderCount = Convert.ToInt32(dr["SectionFinishedOrderCount"]);
                        int secTotalOrderCount = secUnFinishedOrderCount + secFinishedOrderCount;

                        this.lblUnFinishedOrderCount.Text = $"總計 {unFinishedOrderCount} 筆";
                        this.lblFinishedOrderCount.Text = $"總計 {finishedOrderCount} 筆";
                        this.lblTotalOrderCount.Text = $"總計 {totalOrderCount} 筆";
                        this.lblSecUnFinishedOrderCount.Text = $"總計 {secUnFinishedOrderCount} 筆";
                        this.lblSecFinishedOrderCount.Text = $"總計 {secFinishedOrderCount} 筆";
                        this.lblSecTotalOrderCount.Text = $"總計 {secTotalOrderCount} 筆";
                    }
                }
            }
            this.lblNowDate.Text = $"現在時間： {DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm")}";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }
    }
}