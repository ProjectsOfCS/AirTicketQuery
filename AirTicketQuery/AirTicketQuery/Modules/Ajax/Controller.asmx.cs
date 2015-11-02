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
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Xml;
using AirTicketQuery.Modules.Code;
using HtmlAgilityPack;
using Newtonsoft.Json;

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
            int page = Convert.ToInt16(req.Form["page"] ?? "0");
            int rowsCnt = Convert.ToInt16(req.Form["rows"] ?? "20");
            string order = req.Form["order"];

            Hashtable ht = new Hashtable();
            DB dbi = new DB(SystemConst.DBConnString);
            string sqlBase = @"SELECT * FROM dbo.FlightInfo";
            string sqlCnt = @"SELECT COUNT(1) FROM dbo.FlightInfo";
            try
            {
                if (!string.IsNullOrEmpty(strFromCity) && !string.IsNullOrEmpty(strToCity) && !string.IsNullOrEmpty(strDeparture))
                {
                    bool getQUNAR = Convert.ToInt16(req.Form["QUNAR"] ?? "0") == 1;
                    bool getCEAIR = Convert.ToInt16(req.Form["CEAIR"] ?? "0") == 1;
                    bool getCSAIR = Convert.ToInt16(req.Form["CSAIR"] ?? "0") == 1;
                    bool getCTRIP = Convert.ToInt16(req.Form["CTRIP"] ?? "0") == 1;
                    bool getWS = Convert.ToInt16(req.Form["WS"] ?? "0") == 1;

                    string sqlCity = @"SELECT * FROM dbo.City WHERE C_CODE=@C_CODE";
                    City fromCity = EntityUtil.Create<City>(dbi.GetDataTable(sqlCity, this.InitSqlParams("C_CODE", strFromCity)).Rows[0]);
                    City toCity = EntityUtil.Create<City>(dbi.GetDataTable(sqlCity, this.InitSqlParams("C_CODE", strToCity)).Rows[0]);

                    string sqlWhere = @" WHERE datediff(MINUTE,C_ADD_TIME,getdate())<60 and C_From=@C_From and C_To=@C_To and datediff(day, C_Departure, @C_Departure)=0";
                    List<SqlParameter> lstParam = new List<SqlParameter>();
                    lstParam.Add(new SqlParameter("@C_From", fromCity.C_NAME));
                    lstParam.Add(new SqlParameter("@C_To", toCity.C_NAME));
                    lstParam.Add(new SqlParameter("@C_Departure", strDeparture));
                    StringBuilder sbdatasoure = new StringBuilder();
                    if (getQUNAR)
                    {
                        sbdatasoure.Append(" OR C_DateSource='QUNAR'");
                        if (Convert.ToInt16(dbi.ExecScalar(sqlCnt + sqlWhere + " AND C_DateSource='QUNAR'", lstParam.ToArray(), "0")) == 0)
                        {
                            List<Flight> lstFlight = this.QUNAR_Get(fromCity, toCity, strDeparture);
                            dbi.WriteData(lstFlight.ToDataTable(), "FlightInfo");
                        }
                    }

                    if (getCEAIR)
                    {
                        sbdatasoure.Append(" OR C_DateSource='CE AIR'");
                        if (Convert.ToInt16(dbi.ExecScalar(sqlCnt + sqlWhere + " AND C_DateSource='CE AIR'", lstParam.ToArray(), "0")) == 0)
                        {
                            List<Flight> lstFlight = this.CEAIR_Get(fromCity, toCity, strDeparture);
                            dbi.WriteData(lstFlight.ToDataTable(), "FlightInfo");
                        }
                    }

                    if (getCSAIR)
                    {
                        sbdatasoure.Append(" OR C_DateSource='CS AIR'");
                        if (Convert.ToInt16(dbi.ExecScalar(sqlCnt + sqlWhere + " AND C_DateSource='CS AIR'", lstParam.ToArray(), "0")) == 0)
                        {
                            List<Flight> lstFlight = this.CSAIR_Get(fromCity, toCity, strDeparture);
                            dbi.WriteData(lstFlight.ToDataTable(), "FlightInfo");
                        }
                    }

                    if (getCTRIP)
                    {
                        sbdatasoure.Append(" OR C_DateSource='CTRIP API'");
                        if (Convert.ToInt16(dbi.ExecScalar(sqlCnt + sqlWhere + " AND C_DateSource='CTRIP API'", lstParam.ToArray(), "0")) == 0)
                        {
                            List<Flight> lstFlight = this.CTRIP_Get(fromCity, toCity, strDeparture);
                            dbi.WriteData(lstFlight.ToDataTable(), "FlightInfo");
                        }
                    }

                    if (getWS)
                    {
                        sbdatasoure.Append(" OR C_DateSource='webxml'");
                        if (Convert.ToInt16(dbi.ExecScalar(sqlCnt + sqlWhere + " AND C_DateSource='webxml'", lstParam.ToArray(), "0")) == 0)
                        {
                            List<Flight> lstFlight = this.WS_Get(fromCity, toCity, strDeparture);
                            dbi.WriteData(lstFlight.ToDataTable(), "FlightInfo");
                        }
                    }

                    if (sbdatasoure.Length > 0)
                        sbdatasoure.Remove(0, 3).Insert(0, " AND (").Append(")");

                    string sqlQuery = sqlBase + sqlWhere + sbdatasoure.ToString();
                    if (!string.IsNullOrEmpty(sort))
                        sqlQuery += string.Format(" ORDER BY {0} {1}", sort, order);
                    int total = Convert.ToInt16(dbi.ExecScalar(sqlCnt, lstParam.ToArray(), "0"));
                    DataTable dt = dbi.GetPageData(sqlQuery, page, rowsCnt, lstParam.ToArray());
                    ht.Add("rows", dt);
                    ht.Add("total", total);
                }
                else
                {
                    ht.Add("rows", dbi.GetDataTable(sqlBase + " WHERE 1=2"));
                    ht.Add("total", 0);
                }
            }
            catch (Exception ex)
            {
                ht.Add("err", ex.Message);
                if (!ht.ContainsKey("rows"))
                {
                    ht.Add("rows", dbi.GetDataTable(sqlBase + " WHERE 1=2"));
                }

                ht.Add("total", 0);
            }

            jsonStr = JsonConvert.SerializeObject(ht);
            ResponseWrite(jsonStr);
        }

        private List<Flight> CSAIR_Get(City fromCity, City toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            // http://b2c.csair.com/B2C40/detail-SHACAN-20151211-1-0-0-0-1-0-0-0-1-0.g2c
            DateTime dtDepart = DateTime.Parse(departDate);
            string strUrl = string.Format("http://b2c.csair.com/B2C40/detail-{0}{1}-{2}-1-0-0-0-1-0-0-0-1-0.g2c",
                fromCity.C_CODE, toCity.C_CODE, dtDepart.ToString("yyyyMMdd"));
            XmlDocument doc = new XmlDocument();
            doc.Load(strUrl);
            XmlHelper xmlHelper = new XmlHelper(doc);
            XmlNodeList nodelist = xmlHelper.GetXmlNodeListByXpath("FLIGHTS/SEGMENT/DATEFLIGHT/DIRECTFLIGHT/FLIGHT");
            foreach (XmlNode node in nodelist)
            {
                Flight f = new Flight();
                f.C_DateSource = "CS AIR";
                f.C_From = fromCity.C_NAME;
                f.C_To = toCity.C_NAME;
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

        private List<Flight> WS_Get(City fromCity, City toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            DateTime dtDepart = DateTime.Parse(departDate);
            AirTicketQuery.DomesticAirline.DomesticAirline wsAirLine = new DomesticAirline.DomesticAirline();
            DataSet dsFlight = wsAirLine.getDomesticAirlinesTime(fromCity.C_NAME, toCity.C_NAME, dtDepart.ToString("yyyy-MM-dd"), string.Empty);
            foreach (DataRow dr in dsFlight.Tables[0].Rows)
            {
                Flight f = new Flight();
                f.C_DateSource = "webxml";
                f.C_From = fromCity.C_NAME;
                f.C_To = toCity.C_NAME;
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

        private List<Flight> CTRIP_Get(City fromCity, City toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            //http://openapi.ctrip.com/logicsvr/AjaxServerNew.ashx?datatype=jsonp&callProxyKey=flightsearch&requestJson={%22AllianceID%22:%20%227480%22,%22SID%22:%20%22172916%22,%22SecretKey%22:%20%220FEFFC1F-D220-4AAD-8F24-642C962092B7%22,%22Routes%22:%20[{%22DepartCity%22:%20%22BJS%22,%22ArriveCity%22:%20%22CAN%22,%22DepartDate%22:%20%222015-11-05%22}]}
            DateTime dtDepart = DateTime.Parse(departDate);
            string strDepatTime = dtDepart.ToString("yyyy-MM-dd");
            string strParams = string.Format("\"DepartCity\": \"{0}\",\"ArriveCity\": \"{1}\",\"DepartDate\": \"{2}\"",
                fromCity.C_WS_CODE, toCity.C_WS_CODE, dtDepart.ToString("yyyy-MM-dd"));
            string strUrl = "http://openapi.ctrip.com/logicsvr/AjaxServerNew.ashx?datatype=jsonp&callProxyKey=flightsearch&requestJson={\"AllianceID\": \"7480\",\"SID\": \"172916\",\"SecretKey\": \"0FEFFC1F-D220-4AAD-8F24-642C962092B7\",\"Routes\": [{" + strParams + "}]}";

            WebClient client = new WebClient();
            string downloadStr = client.DownloadString(new Uri(strUrl));
            //System.Diagnostics.Debug.Write(downloadStr);
            //string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "11.txt");
            //string downloadStr = File.ReadAllText(path1, System.Text.Encoding.GetEncoding("GB2312"));
            //System.Diagnostics.Debug.Write(downloadStr);
            lstFlight = clsParseCTRIP.ParseJson(downloadStr);
            return lstFlight;
        }

        private List<Flight> CEAIR_Get(City fromCity, City toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            DateTime dtDepart = DateTime.Parse(departDate);
            string strUrl = string.Format("http://www.ceair.com/flight2014/{0}-{1}-{2}_CNY.html", fromCity.C_CE_CODE, toCity.C_CE_CODE, dtDepart.ToString("yyMMdd"));
            string downloadStr = string.Empty;
            try
            {
                Thread thread = new Thread(delegate()
                {
                    var p = new PageSnatch();
                    downloadStr = p.Navigate(strUrl, 50, "flight-info");
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch (Exception ex) { throw ex; }

            //string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "article.txt");
            ////string downloadStr = File.ReadAllText(path1, System.Text.Encoding.GetEncoding("GB2312"));
            //string downloadStr = File.ReadAllText(path1, System.Text.Encoding.UTF8);
            HtmlDocument htmlPage = new HtmlDocument();
            htmlPage.LoadHtml(downloadStr);
            HtmlNode docNode = htmlPage.DocumentNode;

            int articleIndex = 1;
            foreach (HtmlNode childNode in docNode.ChildNodes)
            {
                System.Diagnostics.Debug.WriteLine(childNode.Name);
                if (childNode.Name.Equals("article", StringComparison.CurrentCultureIgnoreCase))
                {
                    string xpathPrefix = string.Format("/article[{0}]/ul/li", articleIndex);
                    Flight f = new Flight();
                    f.C_DateSource = "CE AIR";
                    f.C_From = fromCity.C_NAME;
                    f.C_To = toCity.C_NAME;
                    f.C_Departure = departDate;
                    string flightNo = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@class='f-i']").ChildNodes[1]);
                    string[] flightInfo = flightNo.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (flightInfo.Length >= 2)
                    {
                        f.C_Airline = flightInfo[0];
                        f.C_FlightNo = flightInfo[1];
                    }
                    else
                    {
                        f.C_FlightNo = flightNo;
                    }

                    f.C_DEPTIME = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@class='f-i']/div[@class='info clearfix']/div[@class='airport r']").ChildNodes[0]);
                    string depart = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@class='f-i']/div[@class='info clearfix']/div[@class='airport r']"));
                    f.C_ARRTIME = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@class='f-i']/div[@class='info clearfix']/div[@class='airport']").ChildNodes[0]);
                    string arrive = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@class='f-i']/div[@class='info clearfix']/div[@class='airport']"));
                    f.C_TotalTime = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@class='f-i']/dfn"));
                    decimal outPrice;
                    string strFirstPrice = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@name='fb']")).Replace("￥", string.Empty);
                    if (decimal.TryParse(strFirstPrice, out outPrice))
                        f.C_FirstClass = outPrice;

                    string strEconomyPrice = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@name='economy']")).Replace("￥", string.Empty);
                    if (decimal.TryParse(strEconomyPrice, out outPrice))
                        f.C_Economy = outPrice;

                    string strPrice = this.GetInnerText(childNode.SelectSingleNode(xpathPrefix + "[@name='more']")).Replace("￥", string.Empty);
                    StringBuilder sbPriceInfo = new StringBuilder();
                    sbPriceInfo.AppendFormat("超值特惠:{0};", strPrice);
                    if (childNode.SelectNodes(string.Format("/article[{0}]/hgroup/dl", articleIndex)) != null)
                    {
                        foreach (HtmlNode priceNode in childNode.SelectNodes(string.Format("/article[{0}]/hgroup/dl", articleIndex)))
                        {
                            if (priceNode.SelectNodes("dd").Count >= 1)
                                sbPriceInfo.AppendFormat("{0}:{1};",
                                    this.GetInnerText(priceNode.SelectSingleNode("dt")),
                                    this.GetInnerText(priceNode.SelectNodes("dd")[1]));
                        }
                    }

                    f.C_Remark = sbPriceInfo.ToString();
                    lstFlight.Add(f);
                    //string flightNo = this.GetInnerText(childNode.SelectSingleNode("/article[1]/hgroup[3]/ul[1]/li[1]/div[4]/div[@class='flightNo']"));
                    articleIndex++;
                }
            }

            return lstFlight;
        }

        private List<Flight> QUNAR_Get(City fromCity, City toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            DateTime dtDepart = DateTime.Parse(departDate);
            ////http://flight.qunar.com/site/oneway_list.htm?searchDepartureAirport=%E5%B9%BF%E5%B7%9E&searchArrivalAirport=%E5%8C%97%E4%BA%AC&searchDepartureTime=2015-11-03&searchArrivalTime=2015-11-03&nextNDays=0&startSearch=true&fromCode=CAN&toCode=BJS&from=qunarindex&lowestPrice=null
            string strUrl = string.Format("http://flight.qunar.com/site/oneway_list.htm?searchDepartureAirport={0}&searchArrivalAirport={1}&searchDepartureTime={2}&searchArrivalTime=2015-11-03&nextNDays=0&startSearch=true&fromCode=CAN&toCode=BJS&from=qunarindex&lowestPrice=null",
                 Server.UrlEncode(fromCity.C_NAME), Server.UrlEncode(toCity.C_NAME), dtDepart.ToString("yyyy-MM-dd"));
            string downloadStr = string.Empty;
            try
            {
                Thread thread = new Thread(delegate()
                {
                    var p = new PageSnatch();
                    downloadStr = p.Navigate(strUrl, 50, "hdivResultPanel");
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch (Exception ex) { throw ex; }

            //string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "flight.txt");
            ////string downloadStr = File.ReadAllText(path1, System.Text.Encoding.GetEncoding("GB2312"));
            //string downloadStr = File.ReadAllText(path1, System.Text.Encoding.UTF8);

            HtmlDocument htmlPage = new HtmlDocument();
            htmlPage.LoadHtml(downloadStr);
            HtmlNode docNode = htmlPage.DocumentNode;
            HtmlNodeCollection findChildNodes = docNode.ChildNodes;
            if (findChildNodes.Count == 1)
            {
                if (findChildNodes[0].Id.ToUpper().Equals("HDIVRESULTPANEL"))
                    findChildNodes = docNode.ChildNodes[0].ChildNodes;
            }

            foreach (HtmlNode childNode in findChildNodes)
            {
                System.Diagnostics.Debug.WriteLine(childNode.Name);

                if (childNode.Id.ToUpper().StartsWith("ITEMBARXI"))
                {
                    Flight f = new Flight();
                    f.C_DateSource = "QUNAR";
                    f.C_From = fromCity.C_NAME;
                    f.C_To = toCity.C_NAME;
                    f.C_Departure = departDate;
                    StringBuilder sbOtherInfo = new StringBuilder();
                    if (childNode.SelectNodes("div/div")[1].Attributes[0].Value.Equals("c1"))
                    {
                        HtmlNode checkNode = childNode.SelectNodes("div/div")[1];
                        f.C_Airline = this.GetInnerText(checkNode.SelectSingleNode("div[@class='vlc-wp']/div[@class='vlc-con']/div[@class='air-wp']/div[@class='air-row']/div[@class='a-name']"));
                        f.C_FlightNo = this.GetInnerText(checkNode.SelectSingleNode("div[@class='vlc-wp']/div[@class='vlc-con']/div[@class='air-wp']/div[@class='air-row']/div[@class='a-model']/span"));
                    }

                    if (childNode.SelectNodes("div/div")[2].Attributes[0].Value.Equals("c2"))
                    {
                        HtmlNode checkNode = childNode.SelectNodes("div/div")[2];
                        f.C_DEPTIME = this.GetInnerText(checkNode.SelectSingleNode("div[@class='a-dep-time']"));
                        string depAirPort = this.GetInnerText(checkNode.SelectSingleNode("div[@class='a-dep-airport']"));
                        sbOtherInfo.AppendFormat("departAirPort:{0};", depAirPort);
                    }

                    if (childNode.SelectNodes("div/div")[3].Attributes[0].Value.Equals("c3"))
                    {
                        HtmlNode checkNode = childNode.SelectNodes("div/div")[3];
                        f.C_TotalTime = this.GetInnerText(checkNode.SelectSingleNode("div[@class='a-zh-wp']"));
                    }

                    if (childNode.SelectNodes("div/div")[4].Attributes[0].Value.Equals("c4"))
                    {
                        HtmlNode checkNode = childNode.SelectNodes("div/div")[4];
                        f.C_ARRTIME = this.GetInnerText(checkNode.SelectSingleNode("div[@class='a-arr-time']"));
                        string arrAirPort = this.GetInnerText(checkNode.SelectSingleNode("div[@class='a-arr-airport']"));
                        sbOtherInfo.AppendFormat("arriveAirPort:{0};", arrAirPort);
                    }

                    if (childNode.SelectNodes("div/div")[7].Attributes[0].Value.Equals("c7"))
                    {
                        HtmlNode mainPriceNode = childNode.SelectNodes("div/div")[7].SelectNodes("div/div")[1].ChildNodes[1];
                        if (mainPriceNode.Attributes["Style"] != null)
                        {
                            System.Diagnostics.Debug.WriteLine(mainPriceNode.Attributes["Style"].Value);
                            //System.Diagnostics.Debug.WriteLine(mainPriceNode.Attributes["Style"].Value.Equals("width:48px"));

                            SortedList<string, string> lstPrice = new SortedList<string, string>();
                            if (mainPriceNode.SelectNodes("em").Count == 1)
                            {
                                HtmlNode calcutePriceNode = mainPriceNode.SelectSingleNode("em");
                                foreach (HtmlNode priceNode_b in calcutePriceNode.SelectNodes("b"))
                                {
                                    if (priceNode_b.ChildNodes.Count > 1)
                                    {
                                        string styleValue = priceNode_b.Attributes["style"].Value;
                                        string style_LeftValue = styleValue.Split(new char[] { ';' })[1].ToLower().Replace("left: -", string.Empty).Replace("px", string.Empty).Trim();
                                        int outLeftPX = 0;
                                        if (int.TryParse(style_LeftValue, out outLeftPX))
                                        {
                                            foreach (HtmlNode detialPriceNode in priceNode_b.ChildNodes)
                                            {
                                                string key = string.Format("left:-{0}px", outLeftPX);

                                                lstPrice.Add(key, this.GetInnerText(detialPriceNode));
                                                outLeftPX -= 16;
                                            }
                                        }
                                        else
                                        {
                                            lstPrice.Add("left:-16px", this.GetInnerText(priceNode_b.LastChild));
                                        }
                                    }
                                    else
                                    {
                                        string styleValue = priceNode_b.Attributes["style"].Value.ToLower().Replace(" ", string.Empty);
                                        if (lstPrice.ContainsKey(styleValue))
                                            lstPrice[styleValue] = this.GetInnerText(priceNode_b);
                                        else
                                            lstPrice.Add(styleValue, this.GetInnerText(priceNode_b));
                                    }
                                }
                            }

                            if (lstPrice.Count > 0)
                            {
                                decimal price = 0;
                                int i = 1;
                                foreach (var itmPrice in lstPrice)
                                {
                                    price += Convert.ToInt16(itmPrice.Value) * i;
                                    i *= 10;
                                }

                                f.C_Price = price;
                            }
                        }
                    }

                    f.C_Remark = sbOtherInfo.ToString();
                    lstFlight.Add(f);
                }
            }

            return lstFlight;
        }

        /// <summary>
        /// this function can not get the pagesoure when the page with ajax, this just for remark.
        /// </summary>
        /// <param name="fromCity"></param>
        /// <param name="toCity"></param>
        /// <param name="departDate"></param>
        /// <returns></returns>
        private List<Flight> Download(string fromCity, string toCity, string departDate)
        {
            List<Flight> lstFlight = new List<Flight>();
            DateTime dtDepart = DateTime.Parse(departDate);
            string strUrl = string.Format("http://www.ceair.com/flight2014/{0}-{1}-{2}_CNY.html", fromCity, toCity, dtDepart.ToString("yyMMdd"));

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

        private string GetInnerText(HtmlNode checkNode)
        {
            string strInnerText = string.Empty;
            if (checkNode != null)
                strInnerText = checkNode.InnerText.Trim(new char[] { ' ', '\r', '\n' });
            return strInnerText;
        }
        #endregion
    }
}
