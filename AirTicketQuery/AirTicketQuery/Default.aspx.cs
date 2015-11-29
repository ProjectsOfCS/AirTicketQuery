using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace AirTicketQuery
{
    public partial class Default : AirTicketQuery.Modules.Common.BasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Header.Title = Modules.Code.SC.SystemName;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(this.ResolveUrl("~/Modules/Main/FlightQuery.aspx"));            
        }
    }
}
