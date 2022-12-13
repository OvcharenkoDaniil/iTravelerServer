using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTravelerServer.DAL;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Service.Services
{
    public class FlightService : IFlightService
    {
        private readonly IBaseRepository<Flight> _flightRepository;
        private readonly ApplicationDbContext _db;

        public FlightService(IBaseRepository<Flight> flightRepository, ApplicationDbContext db)
        {
            _flightRepository = flightRepository;
            _db = db;
        }


        public BaseResponse<List<FlightListVM>> GetFlightList()
        {
            // var result2 = _db.Airport.ToList();
            // var result1 = _db.Account.ToList();
            // var result3 = _db.Plane.ToList();
            // var result4 = _db.Ticket.ToList();
            // var result5 = _db.Transfer.ToList();
            // var result7 = _db.Order.ToList();
            // var result6 = _db.Flight.ToList();

            //var baseResponse = new BaseResponse<IEnumerable<TicketListVM>>();
            try
            {
                //tickets in _db.Ticket join on tickets.FwFlight_id equals flights.Flight_id
                var fwDepAirport = (from flights in _db.Flight
                    join airports in _db.Airport on flights.DepartureAirport_id equals airports.Airport_id
                    select airports).ToList();

                var fwArrAirport = (from flights in _db.Flight
                    join airports in _db.Airport on flights.ArrivalAirport_id equals airports.Airport_id
                    select airports).ToList();

                var fwValues = (from flights in _db.Flight
                    join planes in _db.Plane on flights.Plane_id equals planes.Plane_id
                    join transfers in _db.Transfer on flights.Transfer_id equals transfers.Transfer_id
                    select new
                    {
                        flights.Price,
                        planes.Aircompany_name,
                        planes.StandardClassCapacity,
                        planes.FirstClassCapacity,
                        flights.DepartureDate,
                        flights.DepartureTime,
                        flights.ArrivalDate,
                        flights.Flight_id,
                        flights.ArrivalTime,
                        transfers.NumberOfTransfers,
                        flights.FlightDuration
                    }).ToList();

                // var bwDepAirport = (from tickets in _db.Ticket
                //     join flights in _db.Flight on tickets.BwFlight_id equals flights.Flight_id
                //     join airports in _db.Airport on flights.DepartureAirport_id equals airports.Airport_id
                //     select airports).ToList();
                //
                // var bwArrAirport = (from tickets in _db.Ticket
                //     join flights in _db.Flight on tickets.BwFlight_id equals flights.Flight_id
                //     join airports in _db.Airport on flights.ArrivalAirport_id equals airports.Airport_id
                //     select airports).ToList();
                //
                // var bwValues = (from tickets in _db.Ticket
                //     join flights in _db.Flight on tickets.BwFlight_id equals flights.Flight_id
                //     join planes in _db.Plane on flights.Plane_id equals planes.Plane_id
                //     join transfers in _db.Transfer on flights.Transfer_id equals transfers.Transfer_id
                //     select new
                //     {
                //         tickets.Price,
                //         planes.Aircompany_name,
                //         planes.StandardClassCapacity,
                //         planes.FirstClassCapacity,
                //         flights.DepartureDate,
                //         flights.DepartureTime,
                //         flights.ArrivalDate,
                //         flights.ArrivalTime,
                //         transfers.NumberOfTransfers,
                //         flights.FlightDuration
                //     }).ToList();

                var flightList = new List<FlightListVM>();
                var flight = new FlightListVM();
                for (int i = 0; i < fwValues.Count; i++)
                {
                    flight = new FlightListVM();
                    flight.FwPrice = fwValues[i].Price;
                    //ticket.TotalPrice = fwValues[i].Price + bwValues[i].Price;
                    flight.Flight_id = fwValues[i].Flight_id;
                    flight.FwDepartureCity = fwDepAirport[i].City;
                    flight.FwDepartureAirportName = fwDepAirport[i].Name;
                    flight.FwDepartureAirportCountry = fwDepAirport[i].Country;
                    flight.FwArrivalCity = fwArrAirport[i].City;
                    flight.FwArrivalAirportName = fwArrAirport[i].Name;
                    flight.FwArrivalAirportCountry = fwArrAirport[i].Country;

                    flight.FwAircompany_name = fwValues[i].Aircompany_name;
                    flight.FwStandardClassCapacity = fwValues[i].StandardClassCapacity;
                    flight.FwFirstClassCapacity = fwValues[i].FirstClassCapacity;

                    flight.FwDepartureDate = fwValues[i].DepartureDate;
                    flight.FwDepartureTime = fwValues[i].DepartureTime;
                    flight.FwArrivalDate = fwValues[i].ArrivalDate;
                    flight.FwArrivalTime = fwValues[i].ArrivalTime;
                    flight.FwFlightDuration = fwValues[i].FlightDuration;
                    flight.FwNumberOfTransfers = fwValues[i].NumberOfTransfers;

                    // ticket.BwDepartureCity = bwDepAirport[i].City;
                    // ticket.BwDepartureAirportName = bwDepAirport[i].Name;
                    // ticket.BwDepartureAirportCountry = bwDepAirport[i].Country;
                    // ticket.BwArrivalCity = bwArrAirport[i].City;
                    // ticket.BwArrivalAirportName = bwArrAirport[i].Name;
                    // ticket.BwArrivalAirportCountry = bwArrAirport[i].Country;
                    //
                    // ticket.BwAircompany_name = bwValues[i].Aircompany_name;
                    // ticket.BwFirstClassCapacity = bwValues[i].FirstClassCapacity;
                    // ticket.BwStandardClassCapacity = bwValues[i].StandardClassCapacity;
                    //
                    // ticket.BwDepartureDate = bwValues[i].DepartureDate;
                    // ticket.BwDepartureTime = bwValues[i].DepartureTime;
                    // ticket.BwArrivalDate = bwValues[i].ArrivalDate;
                    // ticket.BwArrivalTime = bwValues[i].ArrivalTime;
                    // ticket.BwFlightDuration = bwValues[i].FlightDuration;
                    // ticket.BwNumberOfTransfers = bwValues[i].NumberOfTransfers;

                    flightList.Add(flight);
                }

                return new BaseResponse<List<FlightListVM>>()
                {
                    Data = flightList,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<FlightListVM>>()
                {
                    Description = $"[GetFlightList] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<Flight>>> GetFlights()
        {
            var baseResponse = new BaseResponse<IEnumerable<Flight>>();
            try
            {
                var flights = await _flightRepository.GetAll().ToListAsync();
                baseResponse.Data = flights;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Flight>>()
                {
                    Description = $"[GetFlights] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<Flight> GetFlight(int flightId)
        {
            var baseResponse = new BaseResponse<Flight>();
            try
            {
                var flight = _flightRepository.Get(flightId);

                baseResponse.Data = flight;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Flight>()
                {
                    Description = $"[GetFlight] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        
        public BaseResponse<bool> UpdateFlight(int flightId, Flight flightData)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var dbFlight = _flightRepository.Get(flightId);
                if (dbFlight == null)
                {
                    baseResponse.Description = "Car not found";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
        
                
                dbFlight.DepartureDate = flightData.DepartureDate;
                dbFlight.ArrivalDate = flightData.ArrivalDate;
                dbFlight.DepartureTime = flightData.DepartureTime;
                dbFlight.ArrivalTime = flightData.ArrivalTime;
                dbFlight.FlightDuration = flightData.FlightDuration;
                dbFlight.Price = flightData.Price;

                var a = dbFlight;
                var res = _flightRepository.UpdateSync(dbFlight);
                
                return new BaseResponse<bool>()
                {
                    
                    Description = "Updated",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[UpdateFlight] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}