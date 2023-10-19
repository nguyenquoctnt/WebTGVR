using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Dao.Issue;
using VDMMutiline.WebBackEnd.Common;

namespace VDMMutiline.WebBackEnd.Areas.Issue.Controllers
{
    public class TicketInfoController : Controller
    {
        // GET: Issue/TicketInfo
        //  IssueGetInfoTicketDao IssueGetInfoTicketDao = new IssueGetInfoTicketDao();
        BookingDTC booking = new BookingDTC();
        public ActionResult GetInfoALL()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetInfoALL(string Airline, string BookingCode)
        {
            if (!string.IsNullOrEmpty(BookingCode))
            {
                if (Airline == Constant.AirlineDomestic.JQ)
                {

                }
                else
                {
                    var obj = booking.GetBookingByCode(BookingCode, Airline);
                    if (obj != null)
                    {
                        if(Airline != obj.AirlineCode)
                        {
                            ViewBag.erro = "Không tìm thấy booking.";
                        }
                        return View(obj);
                    }
                    else
                    {
                        ViewBag.erro = "Không tìm thấy booking.";
                    }

                }
            }
            else
            {
                ViewBag.erro = "Không tìm thấy booking.";
            }
            return View();
        }
     
    }
}