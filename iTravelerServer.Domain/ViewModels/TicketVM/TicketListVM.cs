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
        
        public int TotalPrice { get; set; }
        
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
        
        
        
        
        
        
        
        
        // [Display(Name = "Название")]
        // [Required(ErrorMessage = "Введите имя")]
        // [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше двух символов")]
        // public string Name { get; set; }
        //
        // [Display(Name = "Описание")]
        // [MinLength(50, ErrorMessage = "Минимальная длина должна быть больше 50 символов")]
        // public string Description { get; set; }
        //
        // [Display(Name = "Модель")]
        // [Required(ErrorMessage = "Укажите модель")]
        // [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше двух символов")]
        // public string Model { get; set; }
        //
        // [Display(Name = "Скорость")]
        // [Required(ErrorMessage = "Укажите скорость")]
        // [Range(0, 600, ErrorMessage = "Длина должна быть в диапазоне от 0 до 600")]
        // public double Speed { get; set; }
        //
        // [Display(Name = "Стоимость")]
        // [Required(ErrorMessage = "Укажите стоимость")]
        // public decimal Price { get; set; }
        
        // public string DateCreate { get; set; }
        
        // [Display(Name = "Тип автомобиля")]
        // [Required(ErrorMessage = "Выберите тип")]
        // public string TypeCar { get; set; }

        // public IFormFile Avatar { get; set; }
        //
        // public byte[]? Image { get; set; }
    }
}