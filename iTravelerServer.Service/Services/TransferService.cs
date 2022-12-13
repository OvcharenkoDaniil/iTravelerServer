using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Service.Services;

public class TransferService : ITransferService
{
    private readonly IBaseRepository<Transfer> _transferRepository;

    public TransferService(IBaseRepository<Transfer> transferRepository)
    {
        _transferRepository = transferRepository;
    }

    public async Task<BaseResponse<IEnumerable<Transfer>>> GetTransfers()
    {
        var baseResponse = new BaseResponse<IEnumerable<Transfer>>();
        try
        {
            var transfers = await _transferRepository.GetAll().ToListAsync();

            // .Select(x => new CarViewModel()
            // {
            //     Id = x.Id,
            //     Speed = x.Speed,
            //     Name = x.Name,
            //     Description = x.Description,
            //     Model = x.Model,
            //     DateCreate = x.DateCreate.ToLongDateString(),
            //     Price = x.Price,
            //     TypeCar = x.TypeCar.GetDisplayName()
            // })
            // .Where(x => EF.Functions.Like(x.Name, $"%{term}%"))
            // .ToDictionaryAsync(x => x.Id, t => t.Name);

            baseResponse.Data = transfers;
            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<IEnumerable<Transfer>>()
            {
                Description = $"[GetTransfers] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
    
    public BaseResponse<Transfer> GetTransfer(int transferId)
    {
        var baseResponse = new BaseResponse<Transfer>();
        try
        {
            var transfer = _transferRepository.Get(transferId);

            baseResponse.Data = transfer;
            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<Transfer>()
            {
                Description = $"[GetTransfer] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
    
    public BaseResponse<bool> UpdateTransfer(int transferId, Transfer transferData)
    {
        var baseResponse = new BaseResponse<bool>();
        try
        {
            var dbTransfer = _transferRepository.Get(transferId);
            if (dbTransfer == null)
            {
                baseResponse.Description = "Transfer not found";
                baseResponse.StatusCode = StatusCode.NotFound;
                return baseResponse;
            }
                
            dbTransfer.NumberOfTransfers = transferData.NumberOfTransfers;
            dbTransfer.FirstTransferCity = transferData.FirstTransferCity;
            dbTransfer.FirstTransferDuration = transferData.FirstTransferDuration;
            dbTransfer.SecondTransferCity = transferData.SecondTransferCity;
            dbTransfer.SecondTransferDuration = transferData.SecondTransferDuration;
                
            var res = _transferRepository.UpdateSync(dbTransfer);
                
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