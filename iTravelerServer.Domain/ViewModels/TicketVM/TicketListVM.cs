using System;
using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Http;

namespace iTravelerServer.Domain.ViewModels.FlightVM
{
    public class TicketListVM
    {
        public FlightListVM FwFlight { get; set; }
        public FlightListVM BwFlight { get; set; }
        public int TicketElem_id { get; set; }
        public string FlightClass { get; set; }
        public int order_id { get; set; }
        public bool isOneSide { get; set; }
        public int NumberOfPassengers { get; set; }
        public int TotalPrice { get; set; }
        
        public int FwFlight_id { get; set; }
        public int BwFlight_id { get; set; }
        public int FwMaxBaggageWeight { get; set; }
        public int FwStandardBaggageWeight { get; set; }
        public int FwBusinessBaggageWeight { get; set; }
        public int FwStandardhandLuggageWeight { get; set; }
        public int FwBusinesshandLuggageWeight { get; set; }
        public int BwMaxBaggageWeight { get; set; }
        public int BwStandardBaggageWeight { get; set; }
        public int BwBusinessBaggageWeight { get; set; }
        public int BwStandardhandLuggageWeight { get; set; }
        public int BwBusinesshandLuggageWeight { get; set; }
        
        public int FwExtraBaggagePrice { get; set; }
        public int FwMaxExtraBaggagePlacesForPerson { get; set; }
        public int FwBaggageSurchargePrice { get; set; }
        public int FwStandardBaggagePlacesForPerson { get; set; }
        public int FwBusinessBaggagePlacesForPerson { get; set; }
        public int BwExtraBaggagePrice { get; set; }
        public int BwMaxExtraBaggagePlacesForPerson { get; set; }
        public int BwBaggageSurchargePrice { get; set; }
        public int BwStandardBaggagePlacesForPerson { get; set; }
        public int BwBusinessBaggagePlacesForPerson { get; set; }
        
        
        
        
        public int FwPrice { get; set; }
        public int BwPrice { get; set; }
        
        public string FwAircompany_name { get; set; }
        public int FwStandardClassCapacity { get; set; }
        public int FwBusinessClassCapacity { get; set; }
        //public int FwFirstClassCapacity { get; set; }
        public string FwDepartureCity { get; set; }
        public string FwDepartureAirportName { get; set; }
        public string FwArrivalAirportName { get; set; }
        public string FwDepartureAirportCountry { get; set; }
        public string FwArrivalAirportCountry { get; set; }
        
        public string FwArrivalCity { get; set; }
        public DateTime FwDepartureDate { get; set; }
        public DateTime FwArrivalDate { get; set; }
        public string FwDepartureTime { get; set; }
        public string FwArrivalTime { get; set; }
        public string FwFlightDuration { get; set; }
        public int FwNumberOfTransfers { get; set; }
        
        
        public string BwAircompany_name { get; set; }
        public int BwStandardClassCapacity { get; set; }
        public int BwBusinessClassCapacity { get; set; }
        //public int BwFirstClassCapacity { get; set; }
        
        public string BwDepartureCity { get; set; }
        public string BwArrivalCity { get; set; }
        public string BwDepartureAirportName { get; set; }
        public string BwArrivalAirportName { get; set; }
        public string BwDepartureAirportCountry { get; set; }
        public string BwArrivalAirportCountry { get; set; }
        public DateTime BwDepartureDate { get; set; }
        public DateTime BwArrivalDate { get; set; }
        public string BwDepartureTime { get; set; }
        public string BwArrivalTime { get; set; }
        public string BwFlightDuration { get; set; }
        public int BwNumberOfTransfers { get; set; }
        
    }

    public class TicketFreePlacesVM
    {
        public string FlightClass { get; set; }
        public int FwFlight_id { get; set; }
        public int BwFlight_id { get; set; }
    }
}