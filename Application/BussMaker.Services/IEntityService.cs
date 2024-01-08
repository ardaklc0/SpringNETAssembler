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
        Task<List<Dictionary<string, string>>> CreateDataTransferObjectCreateAsync(int id);
        Task<List<Dictionary<string, string>>> CreateDataTransferObjectUpdateAndGetAsync(int id);
        Task<List<Dictionary<string, string>>> CreateEFRepository(int id);
        Task<List<Dictionary<string, string>>> CreateService(int id);
    }
}
