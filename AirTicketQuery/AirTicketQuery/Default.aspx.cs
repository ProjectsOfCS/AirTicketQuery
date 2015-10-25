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
            //AirTicketQuery.DomesticAirline.DomesticAirline wsAirLine = new DomesticAirline.DomesticAirline();
            //DataSet dsCitys = wsAirLine.getDomesticCity();
            //foreach (DataRow dr in dsCitys.Tables[0].Rows)
            //{
            //    //结构为 Item(enCityName)城市英文名称、Item(cnCityName)城市中文名称、Item(Abbreviation)缩写，按城市英文名称升序排列
            //    System.Diagnostics.Debug.WriteLine(string.Format("update dbo.City set C_WS_CODE='{2}' where C_NAME=N'{1}';", dr["enCityName"], dr["cnCityName"], dr["Abbreviation"]));
            //}
        }
    }
}
