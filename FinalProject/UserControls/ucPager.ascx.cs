using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject.UserControls
{
    public partial class ucPager : System.Web.UI.UserControl
    {
        /// <summary> 頁面URL  </summary>
        public string Url { get; set; }
        /// <summary> 總筆數  </summary>
        public int TotalSize { get; set; }
        /// <summary> 頁面筆數  </summary>
        public int PageSize { get; set; }
        /// <summary> 現在頁面  </summary>
        public int CurrentPage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Bind()
        {
            // 檢查一頁筆數
            if (this.PageSize <= 0)
            {
                throw new DivideByZeroException();
            }
            // 計算總頁數
            int totalPage = this.TotalSize / this.PageSize;
            if (this.TotalSize % this.PageSize > 0)
            {
                totalPage += 1;
            }

            this.aLinkFirst.HRef = $"{this.Url}?page=1";
            this.aLinkLast.HRef = $"{this.Url}?page={totalPage}";

            //依目前頁數計算
            this.CurrentPage = this.GetCurrentPage();
            this.ltlCurrentPage.Text = this.CurrentPage.ToString();
            //計算頁數
            int preM1 = this.CurrentPage - 1;
            int preM2 = this.CurrentPage - 2;

            this.aLink2.HRef = $"{this.Url}?page={preM1}";
            this.aLink2.InnerText = preM1.ToString();
            this.aLink1.HRef = $"{this.Url}?page={preM2}";
            this.aLink1.InnerText = preM2.ToString();


            int nextP1 = this.CurrentPage + 1;
            int nextP2 = this.CurrentPage + 2;

            this.aLink4.HRef = $"{this.Url}?page={nextP1}";
            this.aLink4.InnerText = nextP1.ToString();
            this.aLink5.HRef = $"{this.Url}?page={nextP2}";
            this.aLink5.InnerText = nextP2.ToString();

            // 依頁數，決定是否隱藏超連結，並處理提示文字
            this.aLink1.Visible = (preM2 > 0);
            this.aLink2.Visible = (preM1 > 0);
            this.aLink4.Visible = (nextP1 <= totalPage);
            this.aLink5.Visible = (nextP2 <= totalPage);

            this.ltPager.Text = $"共 {this.TotalSize} 筆， 共 {totalPage} 頁， 現在位於第 {this.GetCurrentPage()} 頁<br/><br/>";
        }

        private int GetCurrentPage()
        {
            string pageText = Request.QueryString["page"];

            if (string.IsNullOrWhiteSpace(pageText))
                return 1;

            int pageIndex = 0;
            if (!int.TryParse(pageText, out pageIndex))
                return 1;

            if (pageIndex <= 0)
                return 1;

            return pageIndex;
        }


        private int GetTotalPages()
        {
            int pagers = this.TotalSize / this.PageSize;
            if ((this.TotalSize % this.PageSize) > 0)
            {
                pagers += 1;
            }
            return pagers;
        }
    }
}