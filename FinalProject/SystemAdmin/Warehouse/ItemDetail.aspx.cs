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
    public partial class ItemDetail : System.Web.UI.Page
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

            string itemNo = Request.QueryString["ItemNo"];
            if (!this.IsPostBack)
            {
                //新增模式
                if (string.IsNullOrWhiteSpace(itemNo))
                {
                    this.lblItemNo.Text = "尚待資料建檔";
                    this.txtItemName.Text = string.Empty;
                    this.txtItemName.ReadOnly = false;
                    this.ddlCategory.SelectedValue = "肉類";
                    this.txtStockCount.Text = string.Empty;
                    this.txtItemPrice.Text = string.Empty;
                }
                //編輯模式
                else
                {
                    DataRow drItemInfo = WarehouseItemManager.GetWarehouseItemInfo(itemNo);
                    if(drItemInfo == null)
                    {
                        Response.Redirect("/SystemAdmin/Warehouse/ItemList.aspx");
                    }
                    this.lblItemNo.Text = drItemInfo["ItemNo"].ToString();
                    this.txtItemName.Text = drItemInfo["ItemName"].ToString();
                    this.txtItemName.ReadOnly = true;
                    this.ddlCategory.SelectedIndex = Convert.ToInt32(drItemInfo["Category"]);
                    this.ddlCategory.Enabled = false;
                    this.txtStockCount.Text = drItemInfo["StockCount"].ToString();
                    this.txtItemPrice.Text = drItemInfo["ItemPrice"].ToString();
                }
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
            if (currentUser.Department != "Warehouse")
            {
                Response.Redirect("/SystemAdmin/UserDefault.aspx");
                return;
            }

            // 取得資訊
            string itemName = this.txtItemName.Text;
            int category = Convert.ToInt32(this.ddlCategory.SelectedValue);
            int stockCount = Convert.ToInt32(this.txtStockCount.Text);
            decimal itemPrice = Convert.ToDecimal(this.txtItemPrice.Text);

            string itemNo = Request.QueryString["ItemNo"];
            //新增模式
            if (string.IsNullOrWhiteSpace(itemNo))
            {
                //// 處理商品編號 /////
                DataRow drCategoryLNo = WarehouseItemManager.GetWarehouseItemLastNo();
                string mLNo = drCategoryLNo["MLastNo"].ToString().Substring(1, 3);
                string sLNo = drCategoryLNo["SLastNo"].ToString().Substring(1, 3);
                string fLNo = drCategoryLNo["FLastNo"].ToString().Substring(1, 3);
                string oLNo = drCategoryLNo["OLastNo"].ToString().Substring(1, 3);

                int mNNO = int.Parse(mLNo) + 1;
                int sNNO = int.Parse(sLNo) + 1;
                int fNNO = int.Parse(fLNo) + 1;
                int oNNO = int.Parse(oLNo) + 1;

                string MNo = string.Empty;
                if (mNNO < 10)
                {
                    MNo = "M00" + mNNO;
                }
                else if (100 > mNNO && mNNO > 9)
                {
                    MNo = "M0" + mNNO;
                }
                else
                {
                    MNo = "M" + mNNO;
                }

                string sNo = string.Empty;
                if (sNNO < 10)
                {
                    sNo = "S00" + sNNO;
                }
                else if (100 > sNNO && sNNO > 9)
                {
                    sNo = "S0" + sNNO;
                }
                else
                {
                    sNo = "S" + sNNO;
                }

                string fNo = string.Empty;
                if (fNNO < 10)
                {
                    fNo = "F00" + fNNO;
                }
                else if (100 > fNNO && fNNO > 9)
                {
                    fNo = "F0" + fNNO;
                }
                else
                {
                    fNo = "F" + fNNO;
                }

                string oNo = string.Empty;
                if (oNNO < 10)
                {
                    oNo = "O00" + oNNO;
                }
                else if (100 > oNNO && oNNO > 9)
                {
                    oNo = "O0" + oNNO;
                }
                else
                {
                    oNo = "O" + oNNO;
                }

                if (ddlCategory.SelectedValue == "0")
                {
                    itemNo = MNo;
                }
                else if (ddlCategory.SelectedValue == "1")
                {
                    itemNo = sNo;
                }
                else if (ddlCategory.SelectedValue == "1")
                {
                    itemNo = fNo;
                }
                else
                {
                    itemNo = oNo;
                }
                //// 處理商品編號 ////
                WarehouseItemManager.CreateGoodsInfo(itemNo, category, itemName, stockCount, itemPrice);
                Response.Redirect("/SystemAdmin/Warehouse/ItemList.aspx");
            }
            // 編輯模式
            else
            {
                if (WarehouseItemManager.UpdateWarehouseItem(itemNo, stockCount, itemPrice))
                {
                    Response.Redirect("/SystemAdmin/Warehouse/ItemList.aspx");
                }
                else
                {

                    ltMsg.Text = "更新失敗";
                }
            }
        }

        private bool CheckInput(out List<string> errMsgList)
        {
            List<string> msgList = new List<string>();
            // check Name
            if (string.IsNullOrWhiteSpace(this.txtItemName.Text))
            {
                msgList.Add("商品名稱為必填欄位");
            }
            // check category
            if (ddlCategory.SelectedIndex == -1)
            {
                msgList.Add("商品類別為必選欄位");
            }
            // check Count
            if (string.IsNullOrWhiteSpace(this.txtStockCount.Text))
            {
                msgList.Add("商品數量為必填欄位");
            }
            // check Price
            if (string.IsNullOrWhiteSpace(this.txtItemPrice.Text))
            {
                msgList.Add("商品單價為必填欄位");
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

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Redirect("/Default.aspx");
        }

    }
}