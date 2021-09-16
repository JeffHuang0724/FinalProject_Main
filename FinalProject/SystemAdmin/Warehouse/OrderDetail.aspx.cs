using FinalProject.Auth;
using FinalProject.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject.SystemAdmin.Warehouse
{
    public partial class OrderDetail : System.Web.UI.Page
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

            // 取得訂單資訊
            string WNo = Request.QueryString["WNo"];
            if (!this.IsPostBack)
            {
                DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
                DataRow drCustomerOrderInfo = CustomerOrderManager.GetOrderInfo(drWarehouseOrder["OrderNo"].ToString());
                if (drWarehouseOrder == null || drCustomerOrderInfo == null)
                {
                    this.plcHasData.Visible = false;
                    this.plcNoData.Visible = true;
                }
                else
                {
                    this.ltlOrderNo.Text = drWarehouseOrder["OrderNo"].ToString();
                    this.ltlSNo.Text = drWarehouseOrder["SNo"].ToString();
                    this.ltlWNo.Text = drWarehouseOrder["WNo"].ToString();
                    this.ltlSalesNo.Text = drWarehouseOrder["SalesNo"].ToString();
                    this.ltlSalesManager.Text = drWarehouseOrder["SalesManagerNo"].ToString();
                    DateTime SalesProcessDate = Convert.ToDateTime(drWarehouseOrder["SalesProcessDate"].ToString());
                    this.ltlSalesProcessDate.Text = SalesProcessDate.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime PreShipmentDate = Convert.ToDateTime(drWarehouseOrder["PreShipmentDate"].ToString());
                    this.ltlPreShipmentDate.Text = PreShipmentDate.ToString("yyyy-MM-dd");
                    // 客戶備註
                    string customerRemark = drCustomerOrderInfo["Remark"].ToString();
                    customerRemark = customerRemark.Replace("/r/n", "<br />");
                    this.lblRemark.Text = customerRemark;
                    //公司內部備註
                    string innerRemark = drWarehouseOrder["Remark"].ToString();
                    innerRemark = innerRemark.Replace("/r/n", "<br />");
                    this.lblInnerRemark.Text = innerRemark;

                    // 如果主管品項確認完成，未完成客戶訂單，則顯示完成訂單的按鈕
                    if (currentUser.EmployeeID == drWarehouseOrder["WarehouseManagerNo"].ToString())
                    {
                        if (drWarehouseOrder["WarehouseCheck"].ToString() == "Y" && drWarehouseOrder["WarehouseManagerCheck"].ToString() == "Y")
                        {
                            if(drCustomerOrderInfo["OrderStatus"].ToString() == "3")
                            {
                                this.plcWriteCustomerRemark.Visible = true;
                                this.btnOrderComplete.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        protected void btnCheckOrderItem_Click(object sender, EventArgs e)
        {
            string WNo = Request.QueryString["WNo"];
            Response.Redirect($"/SystemAdmin/Warehouse/OrderItemList.aspx?WNo={WNo}");
        }

        protected void btnOrderComplete_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            var currentUser = AuthManager.GetCurentUser();
            string WNo = Request.QueryString["WNo"];
            DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
            DataRow drCustomerOrderInfo = CustomerOrderManager.GetOrderInfo(drWarehouseOrder["OrderNo"].ToString());

            // 客戶訂單的備註
            string customerRemark = drCustomerOrderInfo["Remark"].ToString();
            customerRemark += ($"倉儲單位:{this.txtRemarkForCustomer.Text}");

            // 更新倉儲訂單
            if (!WarehouseOrderManager.UpdateWarehouseManagerOrder(WNo, "Y", currentUser.EmployeeID, "Y", drWarehouseOrder["Remark"].ToString()))
            {
                this.plcErrMsg.Visible = true;
                errmsg += $"倉儲單號{WNo}更新失敗，請洽系統人員";
                this.ltlErrMsg.Text = errmsg;
                return;
            } else
            {
                // 設定出貨日
                DateTime shipmentDate = DateTime.Now;
                if(shipmentDate.DayOfWeek.ToString() == "Saturday")
                {
                    shipmentDate.AddDays(2);
                }else if(shipmentDate.DayOfWeek.ToString() == "Sunday")
                {
                    shipmentDate.AddDays(1);
                }


                if (!CustomerOrderManager.UpdateOrderStatusForManager(drWarehouseOrder["OrderNo"].ToString(), shipmentDate, 4, customerRemark))
                {
                    this.plcErrMsg.Visible = true;
                    errmsg += $"客戶訂單{drWarehouseOrder["OrderNo"].ToString()}更新失敗，請洽系統人員";
                    this.ltlErrMsg.Text = errmsg;
                    return;
                }
                else
                {
                    Response.Redirect("/SystemAdmin/UserDefault.aspx");
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }
    }
}