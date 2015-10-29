using AirTicketQuery.Modules.Code;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace AirTicketQuery.Modules.Ajax
{
    /// <summary>
    /// Summary description for Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Controller : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public void City_Get()
        {
            string jsonStr = string.Empty;
            HttpRequest req = Context.Request;

            Hashtable ht = new Hashtable();
            DB dbi = new DB(SystemConst.DBConnString);
            string sqlBase = @"SELECT * FROM dbo.City";
            try
            {
                string sql = sqlBase + " ORDER BY C_ID";
                ht.Add("rows", dbi.GetDataTable(sql));
            }
            catch (Exception ex)
            {
                ht.Add("err", ex.Message);
                if (!ht.ContainsKey("rows"))
                {
                    ht.Add("rows", dbi.GetDataTable(sqlBase + " WHERE 1=2"));
                }
            }

            jsonStr = JsonConvert.SerializeObject(ht);
            ResponseWrite(jsonStr);
        }

        [WebMethod(EnableSession = true)]
        public void Flight_Get()
        {
            string jsonStr = string.Empty;
            HttpRequest req = Context.Request;
            string strFromCity = req.Form["FromCity"];
            string strToCity = req.Form["ToCity"];
            string strDeparture = req.Form["Departure"];
            string sort = req.Form["sort"];
            int page = Convert.ToInt16(req.Form["page"] ?? "1") - 1;
            int rowsCnt = Convert.ToInt16(req.Form["rows"] ?? "100");
            string order = req.Form["order"];
            Hashtable ht = new Hashtable();
            List<Flight> lstFlight = new List<Flight>();
            try
            {
                List<Flight> rsltLstFlight = new List<Flight>();
                if (!string.IsNullOrEmpty(strFromCity) && !string.IsNullOrEmpty(strToCity) && !string.IsNullOrEmpty(strDeparture))
                {
                    DB dbi = new DB(SystemConst.DBConnString);
                    string sqlCity = @"SELECT * FROM dbo.City WHERE C_CODE=@C_CODE";
                    City fromCity = EntityUtil.Create<City>(dbi.GetDataTable(sqlCity, this.InitSqlParams("C_CODE", strFromCity)).Rows[0]);
                    City toCity = EntityUtil.Create<City>(dbi.GetDataTable(sqlCity, this.InitSqlParams("C_CODE", strToCity)).Rows[0]);

                    //lstFlight.AddRange(this.CSAIR_Get(strFromCity, strToCity, strDeparture));
                    //lstFlight.AddRange(this.WS_Get(fromCity.C_NAME, toCity.C_NAME, strDeparture));
                    lstFlight.AddRange(this.CTRIP_Get(fromCity.C_CODE, toCity.C_CODE, strDeparture));

                    //todo: This function not stable, I still work on it. 
                    //lstFlight.AddRange(this.CEAIR_Get(strFromCity, strToCity, strDeparture));
                    if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
                        lstFlight = EntityUtil.SortList(lstFlight, sort, order.ToEnum<EntityUtil.SortOrder>());

                    Flight[] tmpFlights = new Flight[rowsCnt];
                    if (lstFlight.Count > (page * rowsCnt + rowsCnt))
                        lstFlight.CopyTo(page * rowsCnt, tmpFlights, 0, rowsCnt);
                    else if (lstFlight.Count > page * rowsCnt)
                    {
                        tmpFlights = new Flight[lstFlight.Count - page * rowsCnt];
                        lstFlight.CopyTo(page * rowsCnt, tmpFlights, 0, lstFlight.Count - page * rowsCnt);
                    }
                    else
                    {
                        tmpFlights = new Flight[lstFlight.Count];
                        lstFlight.CopyTo(0, tmpFlights, 0, lstFlight.Count);
                    }

                    rsltLstFlight.AddRange(tmpFlights);
                }

                int total = lstFlight.Count;
                ht.Add("rows", EntityUtil.Create(rsltLstFlight));
                ht.Add("total", total);
            }
            catch (Exception ex)
            {
                ht.Add("err", ex.Message);
                if (!ht.ContainsKey("rows"))
                {
                    ht.Add("rows", EntityUtil.Create(lstFlight));
                }

                ht.Add("total", 0);
            }

            jsonStr = JsonConvert.SerializeObject(ht);
            ResponseWrite(jsonStr);
        }

        private List<Flight> CSAIR_Get(string fromCity, string toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            // http://b2c.csair.com/B2C40/detail-SHACAN-20151211-1-0-0-0-1-0-0-0-1-0.g2c
            DateTime dtDepart = DateTime.Parse(departDate);
            string strUrl = string.Format("http://b2c.csair.com/B2C40/detail-{0}{1}-{2}-1-0-0-0-1-0-0-0-1-0.g2c", fromCity, toCity, dtDepart.ToString("yyyyMMdd"));
            XmlDocument doc = new XmlDocument();
            doc.Load(strUrl);
            XmlHelper xmlHelper = new XmlHelper(doc);
            XmlNodeList nodelist = xmlHelper.GetXmlNodeListByXpath("FLIGHTS/SEGMENT/DATEFLIGHT/DIRECTFLIGHT/FLIGHT");
            foreach (XmlNode node in nodelist)
            {
                Flight f = new Flight();
                f.C_DateSource = "CS AIR";
                f.C_From = fromCity;
                f.C_To = toCity;
                f.C_Departure = departDate;
                f.C_FlightNo = XmlNodeHelper.ParseByNode(node, "FLIGHTNO");
                f.C_Airline = XmlNodeHelper.ParseByNode(node, "AIRLINE");
                f.C_DEPTIME = XmlNodeHelper.ParseByNode(node, "DEPTIME");
                f.C_ARRTIME = XmlNodeHelper.ParseByNode(node, "ARRTIME");
                f.C_TotalTime = XmlNodeHelper.ParseByNode(node, "TIMEDURINGFLIGHT_en");
                StringBuilder sbPriceInfo = new StringBuilder();
                XmlNodeList xnlPrice = node.SelectNodes("CABINS/CABIN");
                foreach (XmlNode childNodePrice in xnlPrice)
                {
                    string nodeName = XmlNodeHelper.ParseByNode(childNodePrice, "NAME");
                    string strPrice = XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE");
                    if (nodeName.Equals("P") && !string.IsNullOrEmpty(strPrice))
                    {
                        f.C_FirstClass = Convert.ToDecimal(strPrice);
                    }
                    else if (nodeName.Equals("Y") && !string.IsNullOrEmpty(strPrice))
                    {
                        f.C_Economy = Convert.ToDecimal(strPrice);
                    }
                    else if (nodeName.Equals("D") && !string.IsNullOrEmpty(strPrice))
                    {
                        f.C_Business = Convert.ToDecimal(strPrice);
                    }
                    else
                    {
                        sbPriceInfo.AppendFormat("nodeName:{0}->ADULTPRICE:{1}->DISCOUNT:{2}->ADULTFAREBASIS:{3}->GBADULTPRICE:{4}"
                            + "->BRANDTYPE:{5}->MILEAGESTANDARD:{6}",
                            nodeName, XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE") ?? string.Empty
                            , XmlNodeHelper.ParseByNode(childNodePrice, "DISCOUNT") ?? string.Empty
                            , XmlNodeHelper.ParseByNode(childNodePrice, "ADULTFAREBASIS") ?? string.Empty
                            , XmlNodeHelper.ParseByNode(childNodePrice, "GBADULTPRICE") ?? string.Empty
                            , XmlNodeHelper.ParseByNode(childNodePrice, "BRANDTYPE") ?? string.Empty
                            , XmlNodeHelper.ParseByNode(childNodePrice, "MILEAGESTANDARD") ?? string.Empty);
                    }
                }

                f.C_Remark = sbPriceInfo.ToString();
                lstFlight.Add(f);
            }

            return lstFlight;
        }

        private List<Flight> WS_Get(string fromCity, string toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            DateTime dtDepart = DateTime.Parse(departDate);
            AirTicketQuery.DomesticAirline.DomesticAirline wsAirLine = new DomesticAirline.DomesticAirline();
            DataSet dsFlight = wsAirLine.getDomesticAirlinesTime(fromCity, toCity, dtDepart.ToString("yyyy-MM-dd"), string.Empty);
            foreach (DataRow dr in dsFlight.Tables[0].Rows)
            {
                Flight f = new Flight();
                f.C_DateSource = "webxml";
                f.C_From = fromCity;
                f.C_To = toCity;
                f.C_Departure = departDate;
                f.C_Airline = dr["Company"].ToString();
                f.C_FlightNo = dr["AirlineCode"].ToString();
                f.C_DEPTIME = dr["StartTime"].ToString();
                f.C_ARRTIME = dr["ArriveTime"].ToString();
                f.C_Remark = string.Format("出发机场:{0}->到达机场:{1}->机型:{2}->经停:{3}->飞行周期（星期）:{4}",
                    dr["StartDrome"], dr["ArriveDrome"], dr["Mode"], dr["AirlineStop"], dr["Week"]);
                lstFlight.Add(f);
            }

            return lstFlight;
        }

        private List<Flight> CTRIP_Get(string fromCity, string toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            //http://openapi.ctrip.com/logicsvr/AjaxServerNew.ashx?datatype=jsonp&callProxyKey=flightsearch&requestJson={%22AllianceID%22:%20%227480%22,%22SID%22:%20%22172916%22,%22SecretKey%22:%20%220FEFFC1F-D220-4AAD-8F24-642C962092B7%22,%22Routes%22:%20[{%22DepartCity%22:%20%22BJS%22,%22ArriveCity%22:%20%22CAN%22,%22DepartDate%22:%20%222015-11-05%22}]}
            DateTime dtDepart = DateTime.Parse(departDate);
            string strDepatTime = dtDepart.ToString("yyyy-MM-dd");
            string strParams = string.Format("\"DepartCity\": \"{0}\",\"ArriveCity\": \"{1}\",\"DepartDate\": \"{2}\"", fromCity, toCity, dtDepart.ToString("yyyy-MM-dd"));
            string strUrl = "http://openapi.ctrip.com/logicsvr/AjaxServerNew.ashx?datatype=jsonp&callProxyKey=flightsearch&requestJson={\"AllianceID\": \"7480\",\"SID\": \"172916\",\"SecretKey\": \"0FEFFC1F-D220-4AAD-8F24-642C962092B7\",\"Routes\": [{" + strParams + "}]}";

            WebClient client = new WebClient();
            string downloadStr = client.DownloadString(new Uri(strUrl));
            System.Diagnostics.Debug.Write(downloadStr);
            //string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "11.txt");
            //string downloadStr = File.ReadAllText(path1, System.Text.Encoding.GetEncoding("GB2312"));
            System.Diagnostics.Debug.Write(downloadStr);
            lstFlight = clsParseCTRIP.ParseJson(downloadStr);
            return lstFlight;
        }

        private List<Flight> CEAIR_Get(string fromCity, string toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            DateTime dtDepart = DateTime.Parse(departDate);
            string strUrl = string.Format("http://www.ceair.com/flight2014/{0}-{1}-{2}_CNY.html", fromCity, toCity, dtDepart.ToString("yyMMdd"));
            var p = new PageSnatch(strUrl, 20000);
            p.Navigate();
            if (p.Error != null)
                System.Diagnostics.Debug.Write(p.Error);
            else
            {
                System.Diagnostics.Debug.Write(p.Text);
                System.Diagnostics.Debug.Write("=".PadLeft(50, '='));
                System.Diagnostics.Debug.Write(p.TextAsync);
                //string pagePart = Regex.Match(pageHtml, "<table\\swidth=\"100%\"\\sborder=\"0\"\\scellspacing=\"1\"\\scellpadding=\"0\"\\sclass=\"flight_info\"\\sid=\"go_table\">\\s[\\s\\S]*</table>").Value;

                //XmlDocument doc = new XmlDocument();
                //doc.Load(strUrl);
                //XmlHelper xmlHelper = new XmlHelper(doc);
                //XmlNodeList nodelist = xmlHelper.GetXmlNodeListByXpath("FLIGHTS/SEGMENT/DATEFLIGHT/DIRECTFLIGHT/FLIGHT");
                //foreach (XmlNode node in nodelist)
                //{
                //    Flight f = new Flight();
                //    f.C_DateSource = "CS AIR";
                //    f.C_From = fromCity;
                //    f.C_To = toCity;
                //    f.C_Departure = departDate;
                //    f.C_FlightNo = XmlNodeHelper.ParseByNode(node, "FLIGHTNO");
                //    f.C_Airline = XmlNodeHelper.ParseByNode(node, "AIRLINE");
                //    f.C_DEPTIME = XmlNodeHelper.ParseByNode(node, "DEPTIME");
                //    f.C_ARRTIME = XmlNodeHelper.ParseByNode(node, "ARRTIME");
                //    f.C_TotalTime = XmlNodeHelper.ParseByNode(node, "TIMEDURINGFLIGHT_en");
                //    StringBuilder sbPriceInfo = new StringBuilder();
                //    XmlNodeList xnlPrice = node.SelectNodes("CABINS/CABIN");
                //    foreach (XmlNode childNodePrice in xnlPrice)
                //    {
                //        string nodeName = XmlNodeHelper.ParseByNode(childNodePrice, "NAME");
                //        //if (nodeName.Equals("J"))
                //        //    f.C_FirstClass = Convert.ToDecimal(XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE"));
                //        //else if (nodeName.Equals("C"))
                //        //    f.C_Economy = Convert.ToDecimal(XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE"));
                //        //else if (nodeName.Equals("D"))
                //        //    f.C_Business = Convert.ToDecimal(XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE"));
                //        sbPriceInfo.AppendFormat("nodeName:{0}->ADULTPRICE:{1}->DISCOUNT:{2}->ADULTFAREBASIS:{3}->GBADULTPRICE:{4}"
                //            + "->BRANDTYPE:{5}->MILEAGESTANDARD:{6}",
                //            nodeName, XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE") ?? string.Empty
                //            , XmlNodeHelper.ParseByNode(childNodePrice, "DISCOUNT") ?? string.Empty
                //            , XmlNodeHelper.ParseByNode(childNodePrice, "ADULTFAREBASIS") ?? string.Empty
                //            , XmlNodeHelper.ParseByNode(childNodePrice, "GBADULTPRICE") ?? string.Empty
                //            , XmlNodeHelper.ParseByNode(childNodePrice, "BRANDTYPE") ?? string.Empty
                //            , XmlNodeHelper.ParseByNode(childNodePrice, "MILEAGESTANDARD") ?? string.Empty);
                //    }

                //    f.C_Remark = sbPriceInfo.ToString();
                //    lstFlight.Add(f);
                //}
            }

            return lstFlight;
        }

        private List<Flight> CEAIR_Get1(string fromCity, string toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            DateTime dtDepart = DateTime.Parse(departDate);
            string strUrl = string.Format("http://www.ceair.com/flight2014/{0}-{1}-{2}_CNY.html", fromCity, toCity, dtDepart.ToString("yyMMdd"));
            var p = new PageSnatch(strUrl, 20000);
            p.Navigate();
            if (p.Error != null)
                System.Diagnostics.Debug.Write(p.Error);
            else
            {
                System.Diagnostics.Debug.Write(p.Text);
                System.Diagnostics.Debug.Write("=".PadLeft(50, '='));
                System.Diagnostics.Debug.Write(p.TextAsync);
            }

            WebClient client = new WebClient();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.txt");
            client.DownloadFile(new Uri(strUrl), path);
            byte[] pageData = client.DownloadData(new Uri(strUrl));
            string pageHtml = Encoding.UTF8.GetString(pageData);
            System.Diagnostics.Debug.Write("Encoding.UTF8.GetString::" + pageHtml);
            using (StreamReader sr = new StreamReader(new MemoryStream(pageData)))
            {
                string strWebData = sr.ReadToEnd();
                System.Diagnostics.Debug.Write(strWebData);
            }

            System.Diagnostics.Debug.WriteLine("=".PadLeft(20, '='));
            System.Diagnostics.Debug.WriteLine("Method2:");
            //Create a WebRequest to get the file
            HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            //Create a response for this request
            HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
            using (StreamReader reader = new StreamReader(fileResp.GetResponseStream(), Encoding.UTF8))
            {
                string strWebData = reader.ReadToEnd();
                System.Diagnostics.Debug.Write(strWebData);
            }

            HttpWebRequest fileReq1 = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            HttpWebResponse fileResp1 = (HttpWebResponse)fileReq1.GetResponse();
            string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test1.txt");
            //Get the Stream returned from the response
            using (Stream responseStream = fileResp1.GetResponseStream())
            {
                using (FileStream localFileStream = new FileStream(path1, FileMode.Create))
                {
                    var buffer = new byte[4096];
                    long totalBytesRead = 0;
                    int bytesRead;

                    while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytesRead += bytesRead;
                        localFileStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("=".PadLeft(20, '='));
            System.Diagnostics.Debug.WriteLine("Method3:");

            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(strUrl);

            wReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50215;)";
            wReq.Method = "GET";
            wReq.Timeout = 12000;
            HttpWebResponse wResp = (HttpWebResponse)wReq.GetResponse();
            Stream respStream = wResp.GetResponseStream();
            using (StreamReader reader = new StreamReader(respStream, Encoding.UTF8))
            {
                string strWebData = reader.ReadToEnd();
                System.Diagnostics.Debug.Write(strWebData);
            }

            //string pagePart = Regex.Match(pageHtml, "<table\\swidth=\"100%\"\\sborder=\"0\"\\scellspacing=\"1\"\\scellpadding=\"0\"\\sclass=\"flight_info\"\\sid=\"go_table\">\\s[\\s\\S]*</table>").Value;

            //XmlDocument doc = new XmlDocument();
            //doc.Load(strUrl);
            //XmlHelper xmlHelper = new XmlHelper(doc);
            //XmlNodeList nodelist = xmlHelper.GetXmlNodeListByXpath("FLIGHTS/SEGMENT/DATEFLIGHT/DIRECTFLIGHT/FLIGHT");
            //foreach (XmlNode node in nodelist)
            //{
            //    Flight f = new Flight();
            //    f.C_DateSource = "CS AIR";
            //    f.C_From = fromCity;
            //    f.C_To = toCity;
            //    f.C_Departure = departDate;
            //    f.C_FlightNo = XmlNodeHelper.ParseByNode(node, "FLIGHTNO");
            //    f.C_Airline = XmlNodeHelper.ParseByNode(node, "AIRLINE");
            //    f.C_DEPTIME = XmlNodeHelper.ParseByNode(node, "DEPTIME");
            //    f.C_ARRTIME = XmlNodeHelper.ParseByNode(node, "ARRTIME");
            //    f.C_TotalTime = XmlNodeHelper.ParseByNode(node, "TIMEDURINGFLIGHT_en");
            //    StringBuilder sbPriceInfo = new StringBuilder();
            //    XmlNodeList xnlPrice = node.SelectNodes("CABINS/CABIN");
            //    foreach (XmlNode childNodePrice in xnlPrice)
            //    {
            //        string nodeName = XmlNodeHelper.ParseByNode(childNodePrice, "NAME");
            //        //if (nodeName.Equals("J"))
            //        //    f.C_FirstClass = Convert.ToDecimal(XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE"));
            //        //else if (nodeName.Equals("C"))
            //        //    f.C_Economy = Convert.ToDecimal(XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE"));
            //        //else if (nodeName.Equals("D"))
            //        //    f.C_Business = Convert.ToDecimal(XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE"));
            //        sbPriceInfo.AppendFormat("nodeName:{0}->ADULTPRICE:{1}->DISCOUNT:{2}->ADULTFAREBASIS:{3}->GBADULTPRICE:{4}"
            //            + "->BRANDTYPE:{5}->MILEAGESTANDARD:{6}",
            //            nodeName, XmlNodeHelper.ParseByNode(childNodePrice, "ADULTPRICE") ?? string.Empty
            //            , XmlNodeHelper.ParseByNode(childNodePrice, "DISCOUNT") ?? string.Empty
            //            , XmlNodeHelper.ParseByNode(childNodePrice, "ADULTFAREBASIS") ?? string.Empty
            //            , XmlNodeHelper.ParseByNode(childNodePrice, "GBADULTPRICE") ?? string.Empty
            //            , XmlNodeHelper.ParseByNode(childNodePrice, "BRANDTYPE") ?? string.Empty
            //            , XmlNodeHelper.ParseByNode(childNodePrice, "MILEAGESTANDARD") ?? string.Empty);
            //    }

            //    f.C_Remark = sbPriceInfo.ToString();
            //    lstFlight.Add(f);
            //}

            return lstFlight;
        }

        #region Private Methods
        private void ResponseWrite(string msg)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "text/json";
            Context.Response.Write(msg);
        }

        private object CheckNull(object strValue)
        {
            if (strValue == null || string.IsNullOrEmpty(strValue.ToString()))
                return DBNull.Value;
            else
                return strValue;
        }

        private string CheckStrNull(object strValue)
        {
            if (strValue == null || string.IsNullOrEmpty(strValue.ToString()))
                return string.Empty;
            else
                return strValue.ToString();
        }

        private SqlParameter[] InitSqlParams(string paramsName, object paramsValue)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("@" + paramsName, paramsValue));
            return lstParam.ToArray();
        }
        #endregion
    }
}
