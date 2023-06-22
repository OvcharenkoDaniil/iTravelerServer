using iTravelerServer.DAL;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlaceController: Controller
{
    private readonly IBaseRepository<Place> _placeRepository;
    private readonly ApplicationDbContext _db;
    
    public PlaceController(ApplicationDbContext db,IBaseRepository<Place> placeRepository)
    {
        _db = db;
        _placeRepository = placeRepository;
    }

    
    //[Authorize(Roles = "User")]
    [Route("GetAllPlaces")]
    [HttpGet]
    public async Task<List<Place>> GetAllPlaces()
    {
        var places = await _placeRepository.GetAll().ToListAsync();
        if (places.Count!=0)
        {
            return places;
        }
        return null;
    }
}