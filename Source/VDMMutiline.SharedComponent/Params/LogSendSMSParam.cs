using System;
using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class LogSendSMSParam : BaseParam
    {


        public int Take { get; set; }

        public int Skip { get; set; }


        public tbl_LogSendSM LogSendSM { get; set; }

        public List<tbl_LogSendSM> LogSendSMs { get; set; }

        public LogSendSMSInfo LogSendSMSInfo { get; set; }

        public List<LogSendSMSInfo> LogSendSMSInfos { get; set; }

        public LogSendSMSFilter LogSendSMSFilter { get; set; }
    }

    public class LogSendSMSFilter : BaseFilter
    {
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public string TuNgayDenNgay { get; set; }
    } 
}
