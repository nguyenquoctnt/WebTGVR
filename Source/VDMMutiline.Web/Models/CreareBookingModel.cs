using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDMMutiline.WebBackEnd.Models
{
    public class CreareBookingModel
    {
        public int? Ways { get; set; }
        public int? Itinerary { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public List<int> lstAdult
        {
            get { return new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; }
            set { }
        }
        public List<int> lstChild
        {
            get { return new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; }
            set { }
        }
        public List<int> lstInfant
        {
            get { return new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; }
            set { }
        }
        public int TotalAdult { get; set; }
        public int TotalChild { get; set; }
        public int TotalInfant { get; set; }
        public string rTotal { get; set; }
        public Contacts Contact { get; set; }
        public List<Tickket> lstTicket { get; set; }
        public List<passenger> lstpassenger { get; set; }
    }
    public class AirlineModelInfo
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class Tickket
    {
        public int types { get; set; }
        public string Code { get; set; }
        public string Class { get; set; }
        public string AirlineCode { get; set; }
        public List<AirlineModelInfo> lstAirlineInfo { get; set; }
        public string FlightNo { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public string HoldToDate { get; set; }
        public string HoldTime { get; set; }
        public string PriceNetAdult { get; set; }
        public string PriceTaxAdult { get; set; }
        public string PriceFeeAdult { get; set; }
        public string PriceNetChild { get; set; }
        public string PriceTaxChild { get; set; }
        public string PriceFeeChild { get; set; }
        public string PriceNetInfant { get; set; }
        public string PriceTaxInfant { get; set; }
        public string PriceFeeInfant { get; set; }
    }
    public class Contacts
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
    }
    public class typeways
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class addons
    {
        public List<typeways> lstTypeways
        {
            get
            {
                return new List<typeways> { new typeways { Id = "Đi", Name = "Đi" }, new typeways { Id = "Về", Name = "Về" } };
            }
            set
            {
            }
        }
        public string GuiIDaddon { get; set; }
        public string GuiIDPad { get; set; }
        public string Type { get; set; }
        public string NameAddon { get; set; }
        public string valueAddon { get; set; }
        public string pricebeforeAddon { get; set; }
        public string priceAfterAddon { get; set; }
    }
    public class passenger
    {
        public string GuiID { get; set; }
        public int? Id { get; set; }
        public int Type { get; set; }
        public string Sex { get; set; }
        public double? Price { get; set; }
        public List<string> lstSex
        {
            get
            {
                return new List<string> { "Nam", "Nữ" };
            }
            set { }
        }
        public string FUllName { get; set; }
        public string Birthday { get; set; }
        public List<addons> lstaddons { get; set; }

    }
}