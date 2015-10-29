using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTicketQuery.Modules.Code;
using Newtonsoft.Json;

namespace AirTicketQuery.Modules.Ajax
{
    public class clsParseCTRIP
    {
        public static List<Flight> ParseJson(string jsonString)
        {
            List<Flight> lstFlight = new List<Flight>();

            try
            {
                if (!jsonString.StartsWith("{"))
                {
                    int findIndex = jsonString.IndexOf("{");
                    if (findIndex > 0)
                        jsonString = jsonString.Remove(0, findIndex);
                }

                var objReturnResult = JsonConvert.DeserializeObject<ReturnResult>(jsonString);
                if (objReturnResult.IsSucceed)
                {
                    if (objReturnResult.FlightRoutes.Count > 0)
                    {
                        FlightRoute flightRoute = objReturnResult.FlightRoutes[0];
                        foreach (var flightInfo in flightRoute.FlightsList)
                        {
                            Flight f = new Flight();
                            f.C_DateSource = "CTRIP API";
                            f.C_From = flightRoute.DCityName;
                            f.C_To = flightRoute.ACityName;
                            f.C_Departure = flightRoute.DDate;
                            f.C_FlightNo = flightInfo.Flight;
                            f.C_Airline = flightInfo.AirlineCode;
                            f.C_DEPTIME = flightInfo.TakeOffTime;
                            f.C_ARRTIME = flightInfo.ArriveTime;
                            FlightClass firstFlightClass = flightInfo.FlightClassList.Find(ff => ff.Class.Equals("F", StringComparison.CurrentCultureIgnoreCase));
                            if (firstFlightClass != null)
                                f.C_FirstClass = Convert.ToDecimal(firstFlightClass.Price);
                            FlightClass economyFlightClass = flightInfo.FlightClassList.Find(ff => ff.Class.Equals("Y", StringComparison.CurrentCultureIgnoreCase));
                            if (economyFlightClass != null)
                                f.C_Economy = Convert.ToDecimal(economyFlightClass.Price);
                            lstFlight.Add(f);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogErr(ex);
            }

            return lstFlight;
        }

        class ReturnResult
        {
            public string ErrorMessage { get; set; }
            public string BaseErrorCode { get; set; }
            public bool IsSucceed { get; set; }
            public string JsonString { get; set; }
            public List<FlightRoute> FlightRoutes { get; set; }
        }

        class FlightRoute
        {
            public string DCityID { get; set; }
            public string DCityName { get; set; }
            public string ACityID { get; set; }
            public string ACityName { get; set; }
            public string DDate { get; set; }
            public string RecordCount { get; set; }
            public string OrderBy { get; set; }
            public string Direction { get; set; }
            public List<FlightInfo> FlightsList { get; set; }
        }

        class FlightInfo
        {
            public string DCityCode { get; set; }
            public string ACityCode { get; set; }
            public string TakeOffTime { get; set; }
            public string ArriveTime { get; set; }
            public string Flight { get; set; }
            public string CraftType { get; set; }
            public string AirlineCode { get; set; }
            public string AirlineName { get; set; }
            public string AdultTax { get; set; }
            public string DPortCode { get; set; }
            public string APortCode { get; set; }
            public string TicketType { get; set; }
            public string PriceType { get; set; }
            public List<FlightClass> FlightClassList { get; set; }
        }

        class FlightClass
        {
            public string Class { get; set; }
            public string ClassDesc { get; set; }
            public string SubClass { get; set; }
            public string DisplaySubclass { get; set; }
            public string Quantity { get; set; }
            public string DisplayRate { get; set; }
            public string Price { get; set; }
            public string StandardPrice { get; set; }
            public string Rernote { get; set; }
        }
    }
}