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

namespace FinalProject.SystemAdmin.Warehouse
{
    public partial class OrderItemList : System.Web.UI.Page
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
            if (currentUser.Department != "Warehouse")
            {
                Response.Redirect("/SystemAdmin/UserDefault.aspx");
            }

            // 取得訂單資訊
            string WNo = Request.QueryString["WNo"];
            DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
            if (drWarehouseOrder == null)
            {
                this.plcHasData.Visible = false;
                this.plcNoData.Visible = true;
            }
            else
            {
                // 已結單或是使用者非相關人士，則隱藏下方button
                if (drWarehouseOrder["WarehouseManagerCheck"].ToString() == "Y")
                {
                    this.plcEdit.Visible = false;
                    this.btnOrderSubmit.Visible = false;
                }
                else if (drWarehouseOrder["WarehouseNo"].ToString() == currentUser.EmployeeID && drWarehouseOrder["WarehouseCheck"].ToString() != "Y")
                {
                    this.plcEdit.Visible = true;
                    this.btnOrderSubmit.Visible = true;
                }
                else if (drWarehouseOrder["WarehouseManagerNo"].ToString() == currentUser.EmployeeID && drWarehouseOrder["WarehouseCheck"].ToString() == "Y" && drWarehouseOrder["WarehouseManagerCheck"].ToString() != "Y")
                {
                    this.plcEdit.Visible = true;
                    this.btnOrderSubmit.Visible = true;
                    this.plcManager.Visible = true;
                }
                else
                {
                    this.plcEdit.Visible = false;
                    this.btnOrderSubmit.Visible = false;
                }

                if (!this.IsPostBack)
                {
                    string orderNo = drWarehouseOrder["OrderNo"].ToString();
                    DataTable dtOrderItem = CustomerOrderManager.GetOrderDetailInfo(orderNo);
                    string innerRemark = drWarehouseOrder["Remark"].ToString();
                    this.ltlInnerRemark.Text = innerRemark.Replace("/r/n", "<br />");

                    if (dtOrderItem.Rows.Count > 0)
                    {
                        this.ltlOrderNo.Text = orderNo;
                        this.gvWarehouseOrderItemList.DataSource = dtOrderItem;
                        this.gvWarehouseOrderItemList.DataBind();

                        // 倉儲員工已確認訂單，或是使用者非此訂單之倉儲員工，則隱藏確認是否有貨的按鈕
                        if (drWarehouseOrder["WarehouseCheck"].ToString() == "Y" || drWarehouseOrder["WarehouseNo"].ToString() != currentUser.EmployeeID)
                        {
                            foreach (DataControlField gvColumn in gvWarehouseOrderItemList.Columns)
                            {
                                if (gvColumn.HeaderText == "貨源確認")
                                    gvColumn.Visible = false;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtOrderItem.Rows.Count; i++)
                            {

                                if (dtOrderItem.Rows[i].IsNull(dtOrderItem.Columns["WetherStock"]) || string.IsNullOrWhiteSpace(dtOrderItem.Rows[i]["WetherStock"].ToString()))
                                {
                                    gvWarehouseOrderItemList.Rows[i].FindControl("btnEnough").Visible = true;
                                    gvWarehouseOrderItemList.Rows[i].FindControl("btnNotEnough").Visible = true;
                                }
                                else
                                {
                                    gvWarehouseOrderItemList.Rows[i].FindControl("btnEnough").Visible = false;
                                    gvWarehouseOrderItemList.Rows[i].FindControl("btnNotEnough").Visible = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        this.gvWarehouseOrderItemList.Visible = false;
                        this.plcNoData.Visible = true;
                    }
                    this.lblNowDate.Text = $"現在時間： {DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm")}";
                }
            }

        }

        protected void gvWarehouseOrderItemList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string errMsg = string.Empty;
            if (e.CommandName == "Enough")
            {
                //取得點擊的索引值
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //取得點擊的該列
                GridViewRow row = gvWarehouseOrderItemList.Rows[rowIndex];

                //取得資訊
                string WNo = Request.QueryString["WNo"];
                DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
                string orderNo = drWarehouseOrder["OrderNo"].ToString();
                string itemNo = row.Cells[0].Text;
                string wetherStock = "Y";

                if (!CustomerOrderManager.UpdateOrderDetailStatus(orderNo, itemNo, wetherStock))
                {
                    this.plcErrMsg.Visible = true;
                    errMsg += $"客戶訂單{orderNo}品項{itemNo}更新失敗，請洽系統人員";
                    this.ltlErrMsg.Text = errMsg;
                    return;
                }

                Response.Redirect(Request.Url.ToString());
            }

            if (e.CommandName == "NotEnough")
            {
                //取得點擊的索引值
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //取得點擊的該列
                GridViewRow row = gvWarehouseOrderItemList.Rows[rowIndex];

                //取得資訊
                string WNo = Request.QueryString["WNo"];
                DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
                string orderNo = drWarehouseOrder["OrderNo"].ToString();
                string itemNo = row.Cells[0].Text;
                string wetherStock = "N";

                if (!CustomerOrderManager.UpdateOrderDetailStatus(orderNo, itemNo, wetherStock))
                {
                    this.plcErrMsg.Visible = true;
                    errMsg += $"客戶訂單{orderNo}品項{itemNo}更新失敗，請洽系統人員";
                    this.ltlErrMsg.Text = errMsg;
                    return;
                }

                Response.Redirect(Request.Url.ToString());
            }
        }

        protected void btnOrderSubmit_Click(object sender, EventArgs e)
        {
            if (!this.CheckInput())
            {
                return;
            }
            string errMsg = string.Empty;
            var currentUser = AuthManager.GetCurentUser();
            // 一般員工送出訂單
            if (currentUser.EmployeeLevel == "1")
            {
                string WNo = Request.QueryString["WNo"];
                DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
                string orderNo = drWarehouseOrder["OrderNo"].ToString();
                DataTable dtOrderItem = CustomerOrderManager.GetOrderDetailInfo(orderNo);
                for (int i = 0; i < dtOrderItem.Rows.Count; i++)
                {
                    if (dtOrderItem.Rows[i].IsNull(dtOrderItem.Columns["WetherStock"]))
                    {
                        this.plcErrMsg.Visible = true;
                        errMsg += $"請確認完全部品項之狀態，再送出訂單";
                        this.ltlErrMsg.Text = errMsg;
                        return;
                    }
                }

                string innerRemark = drWarehouseOrder["Remark"].ToString();
                innerRemark += $"{currentUser.EmployeeID}: " + this.txtInnerRemark.Text + "/r/n";
                if (!WarehouseOrderManager.UpdateWarehouseOrder(WNo, currentUser.EmployeeID, innerRemark))
                {
                    this.plcErrMsg.Visible = true;
                    errMsg += $"倉儲單號{WNo}更新失敗，請洽系統人員";
                    this.ltlErrMsg.Text = errMsg;
                    return;
                }
                else
                {
                    Response.Redirect("/SystemAdmin/UserDefault.aspx");
                }
            }
            // 主管送出訂單
            else if (currentUser.EmployeeLevel == "2")
            {
                string WNo = Request.QueryString["WNo"];
                DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
                string orderNo = drWarehouseOrder["OrderNo"].ToString();
                DataTable dtOrderItem = CustomerOrderManager.GetOrderDetailInfo(orderNo);
                // 檢查品項是否存在、數目是否正確
                for (int i = 0; i < dtOrderItem.Rows.Count; i++)
                {
                    string itemNo = dtOrderItem.Rows[i]["ItemNo"].ToString();
                    string wetherStock = dtOrderItem.Rows[i]["WetherStock"].ToString();
                    if (wetherStock == "N")
                    {
                        continue;
                    }
                    int orderCount = Convert.ToInt32(dtOrderItem.Rows[i]["ItemCount"].ToString());
                    DataRow drItemInfo = WarehouseItemManager.GetWarehouseItemInfo(itemNo);
                    if (drItemInfo == null)
                    {
                        errMsg += $"訂購品項{itemNo}已不存在，請檢查\r\n";
                        continue;
                    }
                    else
                    {
                        int stockCount = Convert.ToInt32(drItemInfo["StockCount"].ToString());
                        if (orderCount > stockCount)
                        {
                            errMsg += $"訂購品項{itemNo}數量有誤，請檢查\r\n";
                            continue;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(errMsg))
                {
                    this.plcErrMsg.Visible = true;
                    this.ltlErrMsg.Text = errMsg;
                    return;
                }

                // 更新倉庫品項數量
                int unEnoughCount = 0;
                for (int i = 0; i < dtOrderItem.Rows.Count; i++)
                {
                    string itemNo = dtOrderItem.Rows[i]["ItemNo"].ToString();
                    string wetherStock = dtOrderItem.Rows[i]["WetherStock"].ToString();
                    if (wetherStock == "N")
                    {
                        unEnoughCount++;
                        continue;
                    }
                    int orderCount = Convert.ToInt32(dtOrderItem.Rows[i]["ItemCount"].ToString());
                    //Update Warehouse item 
                    DataRow drItemInfo = WarehouseItemManager.GetWarehouseItemInfo(itemNo);
                    int stockCount = Convert.ToInt32(drItemInfo["StockCount"].ToString());
                    stockCount = stockCount - orderCount;
                    decimal itemPrice = Convert.ToDecimal(drItemInfo["ItemPrice"].ToString());
                    if (!WarehouseItemManager.UpdateWarehouseItem(itemNo, stockCount, itemPrice))
                    {
                        this.plcErrMsg.Visible = true;
                        errMsg += $"倉儲品項{itemNo}更新有誤，請洽系統人員";
                        this.ltlErrMsg.Text = errMsg;
                        return;
                    }
                }

                string innerRemark = drWarehouseOrder["Remark"].ToString();
                innerRemark += ($"{currentUser.EmployeeID}: " + this.txtInnerRemark.Text + "/r/n");


                // 更新訂單狀況
                if (WarehouseOrderManager.UpdateWarehouseManagerOrder(WNo, "Y", currentUser.EmployeeID, "Y", innerRemark))
                {
                    // 完全沒庫存則取消整個客戶訂單
                    if (unEnoughCount == dtOrderItem.Rows.Count)
                    {
                        DataRow customerOrderInfo = CustomerOrderManager.GetOrderInfo(orderNo);
                        string customerRemark = customerOrderInfo["Remark"].ToString();
                        customerRemark = customerRemark + "/r/n 倉儲部門:訂購之商品皆無庫存";
                        if (CustomerOrderManager.UpdateOrderStatusForNoStock(drWarehouseOrder["OrderNo"].ToString(), DateTime.Now, 5, customerRemark))
                        {
                            Response.Redirect($"/SystemAdmin/UserDefault.aspx");
                            return;
                        }
                        else
                        {
                            this.plcErrMsg.Visible = true;
                            errMsg += $"客戶訂單{orderNo}更新有誤，請洽系統人員";
                            this.ltlErrMsg.Text = errMsg;
                            return;
                        }
                    }
                    Response.Redirect($"/SystemAdmin/Warehouse/OrderDetail.aspx?WNo={WNo}");
                }
                else
                {
                    this.plcErrMsg.Visible = true;
                    errMsg += $"倉儲訂單{WNo}更新有誤，請洽系統人員";
                    this.ltlErrMsg.Text = errMsg;
                    return;
                }
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (!this.CheckInput())
            {
                return;
            }
            string errMsg = string.Empty;
            var currentUser = AuthManager.GetCurentUser();
            // 更新訂單品項狀態
            string WNo = Request.QueryString["WNo"];
            DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
            string orderNo = drWarehouseOrder["OrderNo"].ToString();
            DataTable dtOrderItem = CustomerOrderManager.GetOrderDetailInfo(orderNo);

            for (int i = 0; i < dtOrderItem.Rows.Count; i++)
            {
                string itemNo = dtOrderItem.Rows[i]["ItemNo"].ToString();
                string wetherStock = string.Empty;
                if (!CustomerOrderManager.UpdateOrderDetailStatus(orderNo, itemNo, wetherStock))
                {
                    this.plcErrMsg.Visible = true;
                    errMsg += $"訂單品項{itemNo}更新有誤，請洽系統人員";
                    this.ltlErrMsg.Text = errMsg;
                    return;
                }
            }
            //更新倉儲訂單狀態
            string innerRemark = drWarehouseOrder["Remark"].ToString();
            innerRemark += ($"{currentUser.EmployeeID}: " + this.txtInnerRemark.Text + "/r/n");
            if (WarehouseOrderManager.UpdateWarehouseManagerOrder(WNo, string.Empty, currentUser.EmployeeID, string.Empty, innerRemark))
            {
                Response.Redirect($"/SystemAdmin/UserDefault.aspx");
            }
            else
            {
                this.plcErrMsg.Visible = true;
                errMsg += $"倉儲訂單{WNo}更新有誤，請洽系統人員";
                this.ltlErrMsg.Text = errMsg;
                return;
            }
        }

        protected void gvWarehouseOrderItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string WNo = Request.QueryString["WNo"];
            DataRow drWarehouseOrder = WarehouseOrderManager.GetWarehouseOrderInfo(WNo);
            if (drWarehouseOrder == null)
            {
                return;
            }
            if (drWarehouseOrder["WarehouseCheck"].ToString() != "Y")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TableCell itemNoCell = e.Row.Cells[0];
                    TableCell itemCountCell = e.Row.Cells[2];
                    string itemNo = itemNoCell.Text;
                    int orderCount = Convert.ToInt32(itemCountCell.Text);
                    DataRow drItemInfo = WarehouseItemManager.GetWarehouseItemInfo(itemNo);
                    if (drItemInfo == null || orderCount > Convert.ToInt32(drItemInfo["StockCount"].ToString()))
                    {
                        itemCountCell.BackColor = Color.Red;
                    }
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