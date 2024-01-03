using BussMaker.DataTransferObject.Requests;
using BussMaker.DataTransferObject.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussMaker.Services
{
    public interface IEntityService
    {
        Task<GetEntityDisplayResponse> GetEntityAsync(int id);
        Task<IEnumerable<GetEntityDisplayResponse>> GetAllEntitiesAsync();
        Task CreateEntityAsync(CreateNewEntityRequest request);
        Task DeleteEntityAsync(int id);
        Task UpdateEntityAsync(UpdateExistingEntityRequest request);
    }
}
