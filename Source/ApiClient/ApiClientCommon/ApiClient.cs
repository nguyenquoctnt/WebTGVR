using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiClient.Common.Constants;
using ApiClient.Models;
using ApiClient.Models.Issue;
using Newtonsoft.Json;

namespace ApiClient.Common
{
    public class ApiClient
    {
        Http.HttpClient httpClient = new Http.HttpClient();
        public SearchResponse Seach(SearchRequest SearchRequest)
        {
            string response = httpClient.PostData(FunctionDTC.search, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<SearchResponse>(response);
        }
        public async Task<SearchResponse> SeachAsync(SearchRequest SearchRequest)
        {
            string response = await httpClient.PostDataAsync(FunctionDTC.search, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<SearchResponse>(response);
        }
        public SearchMinResponse SeachMin(SearchMinRequest SearchMinRequest)
        {
            string response = httpClient.PostData(FunctionDTC.searchmin, JsonConvert.SerializeObject(SearchMinRequest));
            return JsonConvert.DeserializeObject<SearchMinResponse>(response);
        }
        public SearchMonthResponse Searchmonth(SearchMonthRequest SearchMonthRequest)
        {
            string response = httpClient.PostData(FunctionDTC.searchmonth, JsonConvert.SerializeObject(SearchMonthRequest));
            return JsonConvert.DeserializeObject<SearchMonthResponse>(response);
        }
        public FareRuleResponse Getfarerules(FareRuleRequest FareRuleRequest)
        {
            string response = httpClient.PostData(FunctionDTC.getfarerules, JsonConvert.SerializeObject(FareRuleRequest));
            return JsonConvert.DeserializeObject<FareRuleResponse>(response);
        }
        public BaggageResponse Getbaggage(BaggageRequest BaggageRequest)
        {
            string response = httpClient.PostData(FunctionDTC.getbaggage, JsonConvert.SerializeObject(BaggageRequest));
            return JsonConvert.DeserializeObject<BaggageResponse>(response);
        }
        public string Verify(VerifyRequest verifyRequest)
        {
            return httpClient.PostData(FunctionDTC.verifyflight, JsonConvert.SerializeObject(verifyRequest));
        }
        public BookResponse Book(BookRequest BookRequest)
        {
            string response = httpClient.PostData(FunctionDTC.book, JsonConvert.SerializeObject(BookRequest));
            return JsonConvert.DeserializeObject<BookResponse>(response);
        }
        public BookResponse Rebook(ReBookRequest BookRequest)
        {
            string response = httpClient.PostData(FunctionDTC.rebook, JsonConvert.SerializeObject(BookRequest));
            return JsonConvert.DeserializeObject<BookResponse>(response);
        }
        public IssueResponse Issue(IssueRequest IssueRequest)
        {
            string response = httpClient.PostData(FunctionDTC.issue, JsonConvert.SerializeObject(IssueRequest));
            return JsonConvert.DeserializeObject<IssueResponse>(response);
        }
        public VoidResponse Void(VoidRequest VoidRequest)
        {
            string response = httpClient.PostData(FunctionDTC.voids, JsonConvert.SerializeObject(VoidRequest));
            return JsonConvert.DeserializeObject<VoidResponse>(response);
        }
        public CancelResponse Cancel(CancelRequest VoidRequest)
        {
            string response = httpClient.PostData(FunctionDTC.cancel, JsonConvert.SerializeObject(VoidRequest));
            return JsonConvert.DeserializeObject<CancelResponse>(response);
        }
        public SendMailResponse SendMail(SendMailRequest SendMailRequest)
        {
            string response = httpClient.PostData(FunctionDTC.sendmail, JsonConvert.SerializeObject(SendMailRequest));
            return JsonConvert.DeserializeObject<SendMailResponse>(response);
        }
        public ChangePassengerResponse Changepassenger(ChangepassengerRequest changepassengerRequest)
        {
            string response = httpClient.PostData(FunctionDTC.changepassenger, JsonConvert.SerializeObject(changepassengerRequest));
            return JsonConvert.DeserializeObject<ChangePassengerResponse>(response);
        }
        public SplitPassengerResponse Splitpassenger(SplitPassengerRequest splitPassengerRequest)
        {
            string response = httpClient.PostData(FunctionDTC.splitpassenger, JsonConvert.SerializeObject(splitPassengerRequest));
            return JsonConvert.DeserializeObject<SplitPassengerResponse>(response);
        }
        public AddInfantResponse Addinfant(AddInfantRequest addInfantRequest)
        {
            string response = httpClient.PostData(FunctionDTC.addinfant, JsonConvert.SerializeObject(addInfantRequest));
            return JsonConvert.DeserializeObject<AddInfantResponse>(response);
        }
        public AddOnServiceResponse Addonservice(AddOnServiceRequest addOnServiceRequest)
        {
            string response = httpClient.PostData(FunctionDTC.addonservice, JsonConvert.SerializeObject(addOnServiceRequest));
            return JsonConvert.DeserializeObject<AddOnServiceResponse>(response);
        }
        public ChangeFlightResponse ChangeFlight(ChangeFlightRequest changeFlightRequest)
        {
            string response = httpClient.PostData(FunctionDTC.changeFlight, JsonConvert.SerializeObject(changeFlightRequest));
            return JsonConvert.DeserializeObject<ChangeFlightResponse>(response);
        }
        public BookingGetByIdResponse bookingGetbyid(BookingGetByIdRequest getByIdRequest)
        {
            string response = httpClient.PostData(FunctionDTC.bookingGetbyid, JsonConvert.SerializeObject(getByIdRequest));
            return JsonConvert.DeserializeObject<BookingGetByIdResponse>(response);
        }

        public BookingDetailGetByBookingIdResponse bookingDetailGetbybookingid(BookingdetailGetbybookingidRequest detailGetbybookingidRequest)
        {
            string response = httpClient.PostData(FunctionDTC.bookingDetailGetbybookingid, JsonConvert.SerializeObject(detailGetbybookingidRequest));
            return JsonConvert.DeserializeObject<BookingDetailGetByBookingIdResponse>(response);
        }
        public BookingPassengerGetbybookingidResponse bookingPassengerGetbybookingid(BookingPassengerGetbybookingidRequest bookingPassengerGetbybookingidRequest)
        {
            string response = httpClient.PostData(FunctionDTC.bookingPassengerGetbybookingid, JsonConvert.SerializeObject(bookingPassengerGetbybookingidRequest));
            return JsonConvert.DeserializeObject<BookingPassengerGetbybookingidResponse>(response);
        }
        public BookingBaggageGetByBookingIdResponse bookingBaggageGetByBookingId(BookingBaggageGetByBookingIdRequest bookingBaggageGetByBookingIdRequest)
        {
            string response = httpClient.PostData(FunctionDTC.bookingbaggageGetbybookingid, JsonConvert.SerializeObject(bookingBaggageGetByBookingIdRequest));
            return JsonConvert.DeserializeObject<BookingBaggageGetByBookingIdResponse>(response);
        }



        public async Task<VDMFlightSearchResult> JetstarSearchTicketDomtic(VDMSeachParam SearchRequest)
        {
            string response = await httpClient.PostDataAsync(FunctionJQ.SearchTicketDomtic, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<VDMFlightSearchResult>(response);
        }
        public async Task<VDMFlightSearchResult> JetstarSearchTicketInternational(VDMSeachParam SearchRequest)
        {
            string response = await httpClient.PostDataAsync(FunctionJQ.SearchTicketInternational, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<VDMFlightSearchResult>(response);
        }
        public VDMBaggageInfo JetstarGetBaggages(VDMRequestInfo SearchRequest)
        {
            string response = httpClient.PostData(FunctionJQ.GetBaggages, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<VDMBaggageInfo>(response);
        }
        public string JetstargetFareRule(VDMRequestInfo SearchRequest)
        {
            string response = httpClient.PostData(FunctionJQ.getFareRule, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<string>(response);
        }
        public VDMPNR bookJetstart(VDMBookingRequest SearchRequest)
        {
            string response = httpClient.PostData(FunctionJQ.bookJetstart, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<VDMPNR>(response);
        }
        public VDMPNR bookJetstartInternational(VDMBookingRequest SearchRequest)
        {
            string response = httpClient.PostData(FunctionJQ.bookJetstartInternational, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<VDMPNR>(response);
        }
        public VDMPNR bookJetstartCustom(VDMBookingRequestCustom SearchRequest)
        {
            string response = httpClient.PostData(FunctionJQ.bookJetstartCustom, JsonConvert.SerializeObject(SearchRequest));
            return JsonConvert.DeserializeObject<VDMPNR>(response);
        }
        public List<Airport> SearchAirport(string key)
        {
            string Url = FunctionSysCommon.SearchAirport + "?key=" + key;
            string response = httpClient.GetData(Url);
            return JsonConvert.DeserializeObject<List<Airport>>(response);
        }
        public Airport GetAirportByCode(string key)
        {
            string Url = FunctionSysCommon.GetAirportByCode + "?key=" + key;
            string response = httpClient.GetData(Url);
            return JsonConvert.DeserializeObject<Airport>(response);
        }
        public List<Airport> GetAirportByListCode(string key)
        {
            string Url = FunctionSysCommon.GetAirportByListCode + "?key=" + key;
            string response = httpClient.GetData(Url);
            return JsonConvert.DeserializeObject<List<Airport>>(response);
        }
        public List<BaggageVn> GetBaggageVn(string StartPoind, string EndPoind)
        {
            string Url = FunctionSysCommon.GetBaggageVn + "?StartPoind=" + StartPoind + "&EndPoind=" + EndPoind;
            string response = httpClient.GetData(Url);
            return JsonConvert.DeserializeObject<List<BaggageVn>>(response);
        }
        public List<BaggageVnJq> GetBaggageVnJq(string StartPoind, string EndPoind)
        {
            string Url = FunctionSysCommon.GetBaggageVnJq + "?StartPoind=" + StartPoind + "&EndPoind=" + EndPoind;
            string response = httpClient.GetData(Url);
            return JsonConvert.DeserializeObject<List<BaggageVnJq>>(response);
        }
    }
}
