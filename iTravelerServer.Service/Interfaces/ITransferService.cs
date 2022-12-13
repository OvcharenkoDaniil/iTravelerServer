using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;


namespace iTravelerServer.Service.Interfaces
{
    public interface ITransferService
    {
        Task<BaseResponse<IEnumerable<Transfer>>> GetTransfers();
        BaseResponse<Transfer> GetTransfer(int transferId);
        BaseResponse<bool> UpdateTransfer(int transferId, Transfer transferData);

    }   
}