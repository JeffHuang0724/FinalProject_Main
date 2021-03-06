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
    public partial class HistoryOrder : System.Web.UI.Page
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
            }

            // 取得表單資訊
            string employeeID = currentUser.EmployeeID;
            string employeeLevel = currentUser.EmployeeLevel;
            DataTable dtWarehouseHistoryOrder;

            if (employeeLevel == "1")
                dtWarehouseHistoryOrder = WarehouseOrderManager.GetWarehouseFinishOrderList(employeeID);
            else
                dtWarehouseHistoryOrder = WarehouseOrderManager.GetWarehouseManagerFinishOrderList(employeeID);

            if (dtWarehouseHistoryOrder.Rows.Count > 0)
            {
                // 控制項
                var dtPaged = this.GetPageDataTable(dtWarehouseHistoryOrder);

                this.ucPager.TotalSize = dtWarehouseHistoryOrder.Rows.Count;
                this.ucPager.Bind();

                this.gvWarehouseHistoryOrderList.DataSource = dtPaged;
                this.gvWarehouseHistoryOrderList.DataBind();
            }
            else
            {
                this.ucPager.Visible = false;
                this.gvWarehouseHistoryOrderList.Visible = false;
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

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }
    }
}