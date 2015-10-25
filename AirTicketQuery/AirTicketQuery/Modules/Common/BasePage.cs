using AirTicketQuery.Modules.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace AirTicketQuery.Modules.Common
{
    [CLSCompliant(true)]
    public partial class BasePage : Page
    {
        #region GetRequest
        protected virtual string GetRequestQuery(string strParam)
        {
            string result = this.Request.QueryString[strParam];
            if (string.IsNullOrEmpty(result))
                result = this.Request.Form[strParam];
            return result;
        }
        #endregion

        #region Session Handle

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (!IsPostBack)
            {
                SysUtil.StartLog();
                List<string> lstSessKey = new List<string>();
                List<string> listDelKey = new List<string>();
                foreach (string key in Session.Keys)
                {
                    lstSessKey.Add(key);
                    if (key.StartsWith(this.TmpSessionPrefix))
                    {
                        listDelKey.Add(key);
                    }
                }

                if (listDelKey.Count > 0)
                    Log.LogInfoFormat("RemoveSess:{0}", string.Join(",", listDelKey.ToArray()));
                Log.LogInfoFormat("CurrentSess:{0}", string.Join(",", lstSessKey.ToArray()));

                foreach (string key in listDelKey)
                {
                    SessionHelper.Clear(key);
                }

                SysUtil.StopLog("OnPreInit_RemoveSession");
            }
        }

        private string TmpSessionPrefix
        {
            get
            {
                return "TEMP_" + this.GetType().Name + "_";
            }
        }

        protected void SetValutToSession<T>(string key, T value)
        {
            SessionHelper.SetValutToSession<T>(this.TmpSessionPrefix + key, value);
        }

        protected T GetValueFromSession<T>(string key)
        {
            return SessionHelper.GetValueFromSession<T>(this.TmpSessionPrefix + key, default(T));
        }

        protected T GetValueFromSession<T>(string key, T defaulValue)
        {
            return SessionHelper.GetValueFromSession<T>(this.TmpSessionPrefix + key, defaulValue);
        }

        protected bool CheckValueOfSession(string key)
        {
            return SessionHelper.CheckValueOfSession(this.TmpSessionPrefix + key);
        }

        #endregion

        protected System.Web.UI.WebControls.HiddenField FindHiddenControl(string strControlID)
        {
            System.Web.UI.WebControls.HiddenField result = null;
            System.Web.UI.Control findControl = this.FindControl(strControlID);
            if (findControl == null && this.Master != null)
            {
                findControl = this.Master.FindControl(strControlID);
            }

            if (findControl != null && findControl.GetType() == typeof(System.Web.UI.WebControls.HiddenField))
            {
                result = (System.Web.UI.WebControls.HiddenField)findControl;
            }

            return result;
        }
    }
}