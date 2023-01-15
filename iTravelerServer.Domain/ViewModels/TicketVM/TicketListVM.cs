using System;
using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Http;

namespace iTravelerServer.Domain.ViewModels.FlightVM
{
    public class TicketListVM
    {
        public int TicketElem_id { get; set; }
        public int FwFlight_id { get; set; }
        public int BwFlight_id { get; set; }
        public string FlightClass { get; set; }
        public int order_id { get; set; }
        
        /// <summary>
        public int FwFirst_ticket_num { get; set; }
        public int FwSecond_ticket_num { get; set; }
        public int FwThird_ticket_num { get; set; }
        public int BwFirst_ticket_num { get; set; }
        public int BwSecond_ticket_num { get; set; }
        public int BwThird_ticket_num { get; set; }
        /// </summary>
        public int NumberOfPassengers { get; set; }
        
        public int TotalPrice { get; set; }
        public int FwPrice { get; set; }
        public int BwPrice { get; set; }
        
        public string FwAircompany_name { get; set; }
        public int FwStandardClassCapacity { get; set; }
        public int FwFirstClassCapacity { get; set; }
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
        public int BwFirstClassCapacity { get; set; }
        
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
}