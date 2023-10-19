using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.SendMailAndSMS.TeampleateVe
{
    public partial class RptVe : DevExpress.XtraReports.UI.XtraReport
    {
        public RptVe(int Idbooking)
        {
            InitializeComponent();
            var dao = new BK_TicketDao();
            var PassengerDao = new BK_PassengerDao();
            var list = dao.GetListByBooking(Idbooking);
            foreach (var item in list)
            {
                string gioitinh = "";
                var paramPassenger = new BK_PassengerParam { BookingID = Idbooking, Veid = item.ID };
                PassengerDao.GetbyBooking(paramPassenger);
                foreach (var itemhanhkhach in paramPassenger.BK_PassengerInfos)
                {
                    if (itemhanhkhach.TypeID == Constant.TypePassenger.TreEm)
                        gioitinh = "Trẻ em";
                    else if (itemhanhkhach.TypeID == Constant.TypePassenger.EmBe)
                    {
                        gioitinh = "Em bé";
                    }
                    else
                    {
                        if (itemhanhkhach.Sex == "Nam")
                        {

                            gioitinh = "ÔNG";
                        }
                        else
                        {
                            gioitinh = "BÀ";
                        }
                    }
                  
                    var row = dSreport1.Tables["Ve"].NewRow();
                    row["HK"] = (itemhanhkhach.FirstName + " " + itemhanhkhach.Name).ToUpper();
                    row["QUYDANH"] = gioitinh;
                    if (itemhanhkhach.TypeID == Constant.TypePassenger.EmBe)
                    {
                        row["HanhLyXachTay"] = "";
                    }
                    else
                        row["HanhLyXachTay"] = "7kg";

                    string hanhlykygui = ((!string.IsNullOrEmpty(itemhanhkhach.BaggageDepartValue)
                          ? itemhanhkhach.BaggageDepartValue
                          : "0") + " kg");
                    if (item.Code == "VN")
                    {
                        if (String.IsNullOrEmpty(itemhanhkhach.BaggageDepartValue))
                            hanhlykygui = "20kg";
                    }
                    row["HanhLy"] = hanhlykygui;
                    dSreport1.Tables["Ve"].Rows.Add(row);
                }
                break;
            }
        }

        private void Getlisthanhkhachbybooking(int? type, int bookingid, int veid, ref  string Khachhang, ref  string HanhLy, ref  string gioitinh)
        {
            var dao = new BK_PassengerDao();
            var param = new BK_PassengerParam { BookingID = bookingid, Veid = veid };
            dao.GetbyBooking(param);
            foreach (var item in param.BK_PassengerInfos)
            {
                Khachhang += (item.FirstName + " " + item.Name).ToUpper() + "\n";
                if (type == Constant.Typeve.LuotDi)
                {
                    if (!string.IsNullOrEmpty(item.BaggageDepartValue))
                        HanhLy += (item.BaggageDepartValue + " kg") + "\n";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.BaggageReturnValue))
                        HanhLy += (item.BaggageReturnValue + " kg") + "\n";
                }
                if (item.Sex == "Nam")
                {
                    gioitinh = "ÔNG";
                }
                else
                {
                    gioitinh = "BÀ";
                }
            }
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
