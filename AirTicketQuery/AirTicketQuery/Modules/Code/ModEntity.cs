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
        public Nullable<decimal> C_FirstClass { get; set; }

        /// <summary>
        /// 公务舱
        /// </summary>
        public Nullable<decimal> C_Business { get; set; }

        /// <summary>
        /// 经济舱
        /// </summary>
        public Nullable<decimal> C_Economy { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public Nullable<decimal> C_Price { get; set; }

        /// <summary>
        /// 其他信息
        /// </summary>
        public string C_Remark { get; set; }
    }
}