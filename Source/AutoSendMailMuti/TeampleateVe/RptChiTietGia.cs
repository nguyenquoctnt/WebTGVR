using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.Utils.About;
using DevExpress.XtraReports.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;
using Utility = VDMMutiline.Ultilities.Utility;
using VDMMutiline.Core.UI;

namespace VDMMutiline.SendMailAndSMS.TeampleateVe
{
    public partial class RptChiTietGia : DevExpress.XtraReports.UI.XtraReport
    {
        public RptChiTietGia(int idbooking)
        {
            InitializeComponent();
            var dao = new BK_BookingDao();
            var daoPassenger = new BK_PassengerDao();
            var inforbk = dao.GetInfo(idbooking);
            if (inforbk != null)
            {
                var daoticket = new BK_TicketDao();
                var listTicket = daoticket.GetListByBooking(idbooking);

                double tongtienthanhtoan = 0;
                foreach (var item in listTicket)
                {
                    var row = dSreport1.Tables["CHITIETVE2"].NewRow();
                    if (item.TypeID == Constant.Typeve.LuotDi)
                    {
                        row["Luot"] = "Itinerary 1";
                    }
                    else if (item.TypeID == Constant.Typeve.LuotVe)
                    {
                        row["Luot"] = "Itinerary 2";
                    }
                    row["hanghangkhong"] = UIUty.GetNamebyHang(item.Code);
                    row["ChuyenBay"] = item.FlightNo;
                    row["NgayBay"] = item.StartDate.HasValue ? item.StartDate.Value.ToString("dd/MM/yyyy") : "";
                    row["Loaive"] = item.Class;
                    row["KhoiHanh"] = ((item.StartDate.HasValue
                        ? item.StartDate.Value.ToString("HH:mm")  
                        : "") +
                        getsanbaycode(item.FromCity) + " (" + item.FromCity + ")"
                        );

                    row["Den"] = ((item.EndDate.HasValue
                       ? item.EndDate.Value.ToString("HH:mm") 
                       : "") +
                       getsanbaycode(item.ToCity) + " (" + item.ToCity + ")"
                       );
                    dSreport1.Tables["CHITIETVE2"].Rows.Add(row);
                }
            }

        }
        private string getsanbaycode(string code)
        {
            var dao = new StationDao();
            var obj = dao.Getstringbycode(code);
            if (obj != null)
                return obj.label;
            return "";
        }
        private string tinhthanh(string code)
        {
            var dao = new StationDao();
            var obj = dao.Getbycode(code);
            if (obj != null)
                return obj.Name;
            return "";
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
       
        }
    }
}
