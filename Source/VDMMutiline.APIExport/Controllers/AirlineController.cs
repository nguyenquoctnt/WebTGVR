using ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Ultilities;
using WebProject.Ultilities;

namespace VDMMutiline.APIExport.Controllers
{
    public class AirlineController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> SearchDomestic([FromBody]VDMMutiline.APIExport.Requests.SearchRequest input)
        {
            try
            {
                if (input.HeaderUser.ToUpper() == "TGVR" && input.HeaderPass.ToUpper() == "TGVR123")
                {
                    var SeachAndBookCommon = new SeachAndBookCommon();
                    var listsetting = new List<SettingUserInfo>();
                    var param = await SeachAndBookCommon.seachTrongNuocAsync(listsetting, input.Itinerary, input.StartPoint, input.EndPoint, input.StartDate, input.ReturnDate, input.Adt, input.Children, input.Infant, "", "", "", "");
                    var list = new List<SeachParam>();
                    list.Add(param);
                    var obj = (from en in list
                               select new
                               {
                                   DepartureFlights = en.DepartureFlights,
                                   ReturnFlights = en.ReturnFlights,
                                   Itinerary = en.Itinerary,
                                   StartPoint = en.DepartureAirportCode,
                                   EndPoint = en.DestinationAirportCode,
                                   StartDate = en.DepartureDate,
                                   ReturnDate = en.ReturnDate,
                                   Adt = en.Adult,
                                   Children = en.Children,
                                   Infant = en.Infant,
                               }).FirstOrDefault();
                    return Json(obj);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);

            }


        }
    }
}
