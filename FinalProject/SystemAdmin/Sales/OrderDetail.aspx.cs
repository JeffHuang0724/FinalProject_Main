using FinalProject.Auth;
using FinalProject.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject.SystemAdmin.Sales
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
            if (currentUser.Department != "Sales")
            {
                Response.Redirect("/SystemAdmin/UserDefault.aspx");
                return;
            }

            // 取得訂單資訊
            string SNo = Request.QueryString["SNo"];
            if (!this.IsPostBack)
            {
                // 取得銷售訂單資訊
                DataRow drSalesOrder = SalesOrderManager.GetSalesOrderInfo(SNo);
                if (drSalesOrder == null)
                {
                    this.plcHasData.Visible = false;
                    this.plcNoData.Visible = true;
                }
                else
                {
                    // 取得訂購之詳細品項
                    string orderNo = drSalesOrder["OrderNo"].ToString();
                    DataTable dtOrderDetail = CustomerOrderManager.GetOrderDetailInfo(orderNo);
                    decimal orderAmout = 0;

                    foreach (DataRow dr in dtOrderDetail.Rows)
                    {
                        int itemCount = Convert.ToInt32(dr["ItemCount"].ToString());
                        decimal itemPrice = decimal.Parse(dr["ItemPrice"].ToString());
                        orderAmout += itemCount * itemPrice;
                    }

                    this.plcHasData.Visible = true;
                    this.plcNoData.Visible = false;

                    this.ltlSNo.Text = drSalesOrder["SNo"].ToString();
                    this.ltlOrderNo.Text = drSalesOrder["OrderNo"].ToString();
                    this.ltlOrderEmployeeID.Text = drSalesOrder["OrderEmployeeID"].ToString();
                    this.ltlOrderBranch.Text = drSalesOrder["OrderBranch"].ToString();
                    // 訂單總額大於50000則呈現紅色警示
                    if(orderAmout > 50000)
                    {
                        lblOrderAmount.ForeColor = Color.Red;
                    }
                    this.lblOrderAmount.Text = "NTD " + string.Format("{0:n0}", orderAmout) + " 元";
                    // 編輯超連結至訂購品項的Literal
                    this.ltlOrderItemList.Text = $"<a href='/SystemAdmin/Sales/OrderItemList.aspx?OrderNo={orderNo}' style='margin-left:1.5rem;'>詳細訂購品項</a>";


                    DateTime orderDate = Convert.ToDateTime(drSalesOrder["OrderDate"].ToString());
                    this.ltlOrderDate.Text = orderDate.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime preShipmentDate = Convert.ToDateTime(drSalesOrder["PreShipmentDate"].ToString());
                    this.ltlPreShipmentDate.Text = preShipmentDate.ToString("yyyy-MM-dd");
                    this.ltlSalesNo.Text = drSalesOrder["SalesNo"].ToString();

                    DataRow drCustomerOrder = CustomerOrderManager.GetOrderInfo(orderNo);
                    string customerRemark = drCustomerOrder["Remark"].ToString();
                    customerRemark = customerRemark.Replace("/r/n", "<br />");
                    this.ltlRemark.Text = customerRemark;

                    // 已結單 按鈕隱藏
                    if (drSalesOrder["SalesManagerCheck"].ToString() == "Y")
                    {
                        this.plcProcessBtn.Visible = false;
                        this.plcEditInnerRemark.Visible = false;
                        this.plcReadInnerRemark.Visible = true;
                        this.lblInnerRemark.Text = (drSalesOrder["Remark"].ToString()).Replace("/r/n", "<br />");
                        return;
                    }
                    // 負責人 打開按鈕
                    if (drSalesOrder["SalesNo"].ToString() == currentUser.EmployeeID)
                    {
                        this.plcProcessBtn.Visible = true;
                        this.btnApprove.Visible = true;
                        this.btnReject.Visible = true;
                        this.plcEditInnerRemark.Visible = true;
                        this.plcReadInnerRemark.Visible = true;
                        this.lblInnerRemark.Text = (drSalesOrder["Remark"].ToString()).Replace("/r/n", "<br />");

                    }
                    else if (drSalesOrder["SalesManagerNo"].ToString() == currentUser.EmployeeID && drSalesOrder["SalesManagerNo"].ToString() != "Y")
                    {
                        this.plcProcessBtn.Visible = true;
                        this.btnApprove.Visible = true;
                        this.btnReject.Visible = true;
                        this.plcEditInnerRemark.Visible = true;
                        this.plcReadInnerRemark.Visible = true;
                        this.lblInnerRemark.Text = (drSalesOrder["Remark"].ToString()).Replace("/r/n", "<br />");
                        this.plcWriteCustomerRemark.Visible = true;

                    }
                    else
                    {
                        this.plcProcessBtn.Visible = false;
                        this.btnApprove.Visible = false;
                        this.btnReject.Visible = false;
                        this.plcEditInnerRemark.Visible = false;
                        this.plcReadInnerRemark.Visible = true;
                        this.lblInnerRemark.Text = (drSalesOrder["Remark"].ToString()).Replace("/r/n", "<br />");
                    }
                }
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            if (!this.CheckInput())
                return;

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

            // 取得使用者資訊
            string employeeLevel = currentUser.EmployeeLevel;
            // 一般員工
            if (employeeLevel == "1")
            {
                // 取得訂單資訊
                string sNo = Request.QueryString["SNo"];
                string salesCheck = "Y";
                DateTime salesProcessDate = DateTime.Now;
                DataRow drSalesOrder = SalesOrderManager.GetSalesOrderInfo(sNo);
                string innerRemark = drSalesOrder["Remark"].ToString();
                innerRemark += $"{currentUser.EmployeeID}: {this.txtInnerRemark.Text} /r/n";
                if (SalesOrderManager.UpdateSalesCheck(sNo, salesCheck, salesProcessDate, innerRemark))
                {
                    Response.Redirect("/SystemAdmin/UserDefault.aspx");
                }
                else
                {
                    errMsg += $"銷售單號: {sNo} 送出失敗";
                    this.ltlErrMsg.Text = errMsg;
                    this.plcErrMsg.Visible = true;
                    return;
                }
            }
            // 管理階級
            else
            {
                // 取得訂單資訊
                string sNo = Request.QueryString["SNo"];
                DataRow drSalesOrder = SalesOrderManager.GetSalesOrderInfo(sNo);
                string salesCheck = drSalesOrder["SalesCheck"].ToString();

                if (salesCheck == "Y")
                {
                    // 更新SalesOrder
                    string salesManagerCheck = "Y";
                    string innerRemark = drSalesOrder["Remark"].ToString();
                    innerRemark += $"{currentUser.EmployeeID}: {this.txtInnerRemark.Text} /r/n";
                    DateTime salesManagerProcessDate = DateTime.Now;
                    if (SalesOrderManager.UpdateSalesManagerCheck(sNo, salesCheck, salesManagerCheck, salesManagerProcessDate, innerRemark))
                    {
                        // 新增WarehouseOrder
                        string orderNo = drSalesOrder["OrderNo"].ToString();
                        string orderBranch = drSalesOrder["OrderBranch"].ToString();
                        string warehouseNo, warehouseManagerNo;
                        DateTime warehouseProcessDate = salesManagerProcessDate;
                        if (orderBranch.Substring(0, 1) == "N")
                        {
                            DataRow drWarehouseInfo = EmployeeInfoManager.GetEmployeeInfoBySection("W01");
                            warehouseNo = drWarehouseInfo["EmployeeNo"].ToString();
                            warehouseManagerNo = drWarehouseInfo["ManagerNo"].ToString();
                        }
                        else
                        {
                            DataRow drWarehouseInfo = EmployeeInfoManager.GetEmployeeInfoBySection("W02");
                            warehouseNo = drWarehouseInfo["EmployeeNo"].ToString();
                            warehouseManagerNo = drWarehouseInfo["ManagerNo"].ToString();
                        }

                        WarehouseOrderManager.CreateWarehouseOrder(sNo, orderNo, warehouseNo, warehouseProcessDate, warehouseManagerNo, innerRemark);

                        // 更新客戶訂單狀態
                        orderNo = orderNo;
                        DateTime shipmentDate = DateTime.Now;
                        int orderStatus = 3;
                        DataRow drCustomerOrder = CustomerOrderManager.GetOrderInfo(orderNo);
                        string customerRemark = drCustomerOrder["Remark"].ToString();
                        customerRemark = customerRemark + "銷售部門:" + this.txtCustomerRemark.Text + "/r/n";
                        if (!CustomerOrderManager.UpdateOrderStatusForManager(orderNo, shipmentDate, orderStatus, customerRemark))
                        {
                            errMsg += $"客戶訂單編號: {orderNo} 送出失敗";
                            this.ltlErrMsg.Text = errMsg;
                            this.plcErrMsg.Visible = true;
                            return;
                        }
                        else
                        {
                            Response.Redirect("/SystemAdmin/UserDefault.aspx");
                        }
                    }
                    else
                    {
                        errMsg += $"銷售單號: {sNo} 送出失敗";
                        this.ltlErrMsg.Text = errMsg;
                        this.plcErrMsg.Visible = true;
                        return;
                    }
                }
                else if (salesCheck == "N")
                {
                    string salesManagerCheck = "Y";
                    string innerRemark = drSalesOrder["Remark"].ToString();
                    innerRemark += $"{currentUser.EmployeeID}: {this.txtInnerRemark.Text} /r/n";
                    DateTime salesManagerProcessDate = DateTime.Now;
                    if (SalesOrderManager.UpdateSalesManagerCheck(sNo, salesCheck, salesManagerCheck, salesManagerProcessDate, innerRemark))
                    {
                        string orderNo = drSalesOrder["OrderNo"].ToString();
                        DateTime arriveDate = salesManagerProcessDate;
                        int orderStatus = 5;
                        DataRow drCustomerOrder = CustomerOrderManager.GetOrderInfo(orderNo);
                        string customerRemark = drCustomerOrder["Remark"].ToString();
                        customerRemark = customerRemark + "銷售部門:" + this.txtCustomerRemark.Text + "/r/n";
                        if (!CustomerOrderManager.UpdateOrderStatus(orderNo, arriveDate, orderStatus, customerRemark))
                        {
                            errMsg += $"客戶訂單編號: {orderNo} 送出失敗";
                            this.ltlErrMsg.Text = errMsg;
                            this.plcErrMsg.Visible = true;
                            return;
                        }
                        else
                        {
                            Response.Redirect("/SystemAdmin/UserDefault.aspx");
                        }
                    }
                }
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            if (!this.CheckInput())
                return;

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
            // 取得使用者資訊
            string employeeLevel = currentUser.EmployeeLevel;
            // 一般員工
            if (employeeLevel == "1")
            {
                // 取得訂單資訊
                string sNo = Request.QueryString["SNo"];
                string salesCheck = "N";
                DateTime salesProcessDate = DateTime.Now;
                DataRow drSalesOrder = SalesOrderManager.GetSalesOrderInfo(sNo);
                string innerRemark = drSalesOrder["Remark"].ToString();
                innerRemark += $"{currentUser.EmployeeID}: {this.txtInnerRemark.Text} /r/n";
                if (SalesOrderManager.UpdateSalesCheck(sNo, salesCheck, salesProcessDate, innerRemark))
                {
                    Response.Redirect("/SystemAdmin/UserDefault.aspx");
                }
                else
                {
                    errMsg += $"銷售單號: {sNo} 送出失敗";
                    this.ltlErrMsg.Text = errMsg;
                    this.plcErrMsg.Visible = true;
                    return;
                }
            }
            // 管理階級退回給員工
            else
            {
                // 取得訂單資訊
                string sNo = Request.QueryString["SNo"];
                string salesCheck = string.Empty;
                string salesManagerCheck = "N";
                DateTime salesManagerProcessDate = DateTime.Now;
                DataRow drSalesOrder = SalesOrderManager.GetSalesOrderInfo(sNo);
                string innerRemark = drSalesOrder["Remark"].ToString();
                innerRemark += ($"{currentUser.EmployeeID}: {this.txtInnerRemark.Text} /r/n");
                if (SalesOrderManager.UpdateSalesManagerCheck(sNo, salesCheck, salesManagerCheck, salesManagerProcessDate, innerRemark))
                {
                    Response.Redirect("/SystemAdmin/UserDefault.aspx");
                }
                else
                {
                    errMsg += $"銷售單號: {sNo} 送出失敗";
                    this.ltlErrMsg.Text = errMsg;
                    this.plcErrMsg.Visible = true;
                    return;
                }
            }
        }

        public bool CheckInput()
        {
            string errMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtInnerRemark.Text))
            {
                errMsg = "備註不得為空";
                this.plcErrMsg.Visible = true;
                this.ltlErrMsg.Text = errMsg;
                return false;
            }
            else
                return true;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }
    }
}