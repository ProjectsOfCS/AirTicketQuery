using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace AirTicketQuery.Modules.Code
{
    public partial class City
    {
        public int C_ID { get; set; }
        public string C_NAME { get; set; }
        public string C_CODE { get; set; }
        public string C_CE_CODE { get; set; }
        public string C_WS_CODE { get; set; }
    }

    public partial class Flight
    {
        /// <summary>
        /// 出发城市
        /// </summary>
        public string C_From { get; set; }

        /// <summary>
        /// 到达城市
        /// </summary>
        public string C_To { get; set; }

        /// <summary>
        /// 出发日期
        /// </summary>
        public string C_Departure { get; set; }

        /// <summary>
        /// 数据来源网站
        /// </summary>
        public string C_DateSource { get; set; }

        /// <summary>
        /// 航空公司
        /// </summary>
        public string C_Airline { get; set; }

        /// <summary>
        /// 航班编号
        /// </summary>
        public string C_FlightNo { get; set; }

        /// <summary>
        /// 起飞时间
        /// </summary>
        public string C_DEPTIME { get; set; }

        /// <summary>
        /// 到达时间
        /// </summary>
        public string C_ARRTIME { get; set; }

        /// <summary>
        /// 航程时长
        /// </summary>
        public string C_TotalTime { get; set; }

        /// <summary>
        /// 头等舱
        /// </summary>
        public decimal C_FirstClass { get; set; }

        /// <summary>
        /// 公务舱
        /// </summary>
        public decimal C_Business { get; set; }

        /// <summary>
        /// 经济舱
        /// </summary>
        public decimal C_Economy { get; set; }

        public string C_Remark { get; set; }
        
        //public object this[int index]
        //{
        //    get
        //    {
        //        object result = string.Empty;
        //        switch (index)
        //        {
        //            case 0: result = this.C_From; break;
        //            case 1: result = this.C_To; break;
        //            case 2: result = this.C_Airline; break;
        //            case 3: result = this.C_FlightNo; break;
        //            case 4: result = this.C_DEPTIME; break;
        //            case 5: result = this.C_Landing; break;
        //            case 6: result = this.C_FirstClass; break;
        //            case 7: result = this.C_Economy; break;
        //            case 8: result = this.C_Departure; break;
        //        }

        //        return result;
        //    }
        //    set
        //    {
        //        if (index < 5)
        //        {
        //            string setValue = string.Empty;
        //            if (value != null)
        //                setValue = value.ToString();

        //            switch (index)
        //            {
        //                case 0: this.C_From = setValue; break;
        //                case 1: this.C_To = setValue; break;
        //                case 2: this.C_Airline = setValue; break;
        //                case 3: this.C_FlightNo = setValue; break;
        //                case 4: this.C_DEPTIME = setValue; break;
        //                case 5: this.C_Landing = setValue; break;
        //            }
        //        }
        //        else if (index == 6 || index == 7)
        //        {
        //            if (value != null)
        //            {
        //                decimal setValue = Convert.ToDecimal(value);
        //                switch (index)
        //                {
        //                    case 6: this.C_FirstClass = setValue; break;
        //                    case 7: this.C_Economy = setValue; break;
        //                }
        //            }
        //        }
        //        else if (index == 8)
        //        {
        //            if (value != null)
        //                this.C_Departure = Convert.ToDateTime(value);
        //        }
        //    }
        //}
    }
}