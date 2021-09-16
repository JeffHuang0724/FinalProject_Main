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
    public partial class ItemList : System.Web.UI.Page
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
                return;
            }

            // 取得表單資訊
            DataTable dtWarehouseItem = WarehouseItemManager.GetWarehouseItemList();
            if (dtWarehouseItem.Rows.Count > 0)
            {
                // 控制項
                var dtPaged = this.GetPageDataTable(dtWarehouseItem);

                this.ucPager.TotalSize = dtWarehouseItem.Rows.Count;
                this.ucPager.Bind();

                this.gvWarehouseItemList.DataSource = dtPaged;
                this.gvWarehouseItemList.DataBind();
            }
            else
            {
                this.ucPager.Visible = false;
                this.gvWarehouseItemList.Visible = false;
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

        protected void gvWarehouseItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if (row.RowType == DataControlRowType.DataRow)
            {
                Literal ltl = row.FindControl("ltlCategory") as Literal;
                Label lbl = row.FindControl("lblStockCount") as Label;
                var dr = row.DataItem as DataRowView;
                int category = dr.Row.Field<int>("Category");
                int stockCount = dr.Row.Field<int>("StockCount");
                lbl.Text = stockCount.ToString();
                ltl.Text = category.ToString();

                if (category == 0)
                {
                    ltl.Text = "肉類";
                }
                else if (category == 1)
                {
                    ltl.Text = "海鮮類";
                }
                else if (category == 2)
                {
                    ltl.Text = "蔬果類";
                }
                else
                {
                    ltl.Text = "火鍋料類";
                }


                if (stockCount < 500)
                {
                    lbl.ForeColor = Color.Red;
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/Warehouse/ItemDetail.aspx");
            return;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }
    }
}