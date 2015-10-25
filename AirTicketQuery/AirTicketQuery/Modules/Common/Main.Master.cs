using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using AirTicketQuery.Modules.Code;

namespace AirTicketQuery.Modules.Common
{
    public partial class Main : MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.SystemTitle.Text = Code.SC.SystemName;
            if (string.IsNullOrEmpty(this.litVersion.Text.Trim()))
            {
                this.litVersion.Text = "Version:" + SysUtil.GetAssemblyVersion();
            }

            if (string.IsNullOrEmpty(this.litSystemName.Text))
            {
                this.litSystemName.Text = Code.SC.SystemName;
            }

#if DEBUG
            this.litConfig.Text = "[Debug Mode]";
#endif
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.hfBaseUrl.Value))
                this.hfBaseUrl.Value = this.ResolveUrl("~/Modules/");
        }
    }
}
