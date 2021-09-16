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
    public partial class order : System.Web.UI.Page
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
            if(currentUser.Department != "Warehouse")
            {
                Response.Redirect("/SystemAdmin/UserDefault.aspx");
                return;
            }

            // 取得表單資訊
            string employeeID = currentUser.EmployeeID;
            string employeeLevel = currentUser.EmployeeLevel;
            DataTable dtWarehouseOrder;
            if(employeeLevel == "1")
                dtWarehouseOrder = WarehouseOrderManager.GetWarehouseUnFinishOrderList(employeeID);
            else
                dtWarehouseOrder = WarehouseOrderManager.GetWarehouseManagerUnFinishOrderList(employeeID);

            if (dtWarehouseOrder.Rows.Count > 0)
            {
                // 控制項
                var dtPaged = this.GetPageDataTable(dtWarehouseOrder);

                this.ucPager.TotalSize = dtWarehouseOrder.Rows.Count;
                this.ucPager.Bind();

                this.gvWarehouseOrderList.DataSource = dtPaged;
                this.gvWarehouseOrderList.DataBind();
            }
            else
            {
                this.ucPager.Visible = false;
                this.gvWarehouseOrderList.Visible = false;
                this.plcNoData.Visible = true;
            }
            this.lblNowDate.Text = $"現在時間： {DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm")}";
        }

        /// <summary>取得當前GridView頁數/ </summary>
        private int GetCurrentPage()
        {
            string pageText = Request.QueryString["Page"];

            if (string.IsNullOrWhiteSpace(pageText))
                return 1;

            int intPage;
            if (!int.TryParse(pageText, out intPage))
                return 1;

            if (intPage <= 0)
                return 1;

            return intPage;
        }
        /// <summary>取得GridView 內容/ </summary>
        private DataTable GetPageDataTable(DataTable dt)
        {
            DataTable dtPaged = dt.Clone();
            int pageSize = this.ucPager.PageSize;

            int startIndex = (this.GetCurrentPage() - 1) * pageSize;
            int endIndex = (this.GetCurrentPage()) * pageSize;

            if (endIndex > dt.Rows.Count)
                endIndex = dt.Rows.Count;

            for (var i = startIndex; i < endIndex; i++)
            {
                DataRow dr = dt.Rows[i];
                var drNew = dtPaged.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    drNew[dc.ColumnName] = dr[dc];
                }
                dtPaged.Rows.Add(drNew);
            }
            return dtPaged;
        }

        protected void gvWarehouseOrderList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if (row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = row.FindControl("lblOrderDate") as Label;
                var dr = row.DataItem as DataRowView;
                DateTime OrderDate = dr.Row.Field<DateTime>("OrderDate");
                lbl.Text = OrderDate.ToString("yyyy-MM-dd");

                if ((DateTime.Now - (dr.Row.Field<DateTime>("OrderDate"))).Days > 5)
                {
                    lbl.ForeColor = Color.Red;
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