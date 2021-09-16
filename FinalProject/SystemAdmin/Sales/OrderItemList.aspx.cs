using FinalProject.Auth;
using FinalProject.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject.SystemAdmin.Sales
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
            if (currentUser.Department != "Sales")
            {
                Response.Redirect("/SystemAdmin/UserDefault.aspx");
                return;
            }

            // 取得訂單資訊
            string orderNo = Request.QueryString["OrderNo"];
            DataTable dtOrderItem = CustomerOrderManager.GetOrderDetailInfo(orderNo);
            if(dtOrderItem.Rows.Count > 0)
            {
                this.gvSalesOrderItemList.DataSource = dtOrderItem;
                this.gvSalesOrderItemList.DataBind();
                this.plcHasData.Visible = true;
                this.plcNoData.Visible = false;
            }else
            {
                this.plcHasData.Visible = false;
                this.plcNoData.Visible = true;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }
    }
}