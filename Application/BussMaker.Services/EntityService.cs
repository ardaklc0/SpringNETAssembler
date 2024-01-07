using BussMaker.DataTransferObject.Requests;
using BussMaker.DataTransferObject.Responses;
using BussMaker.Entities;
using BussMaker.Infrastructure.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BussMaker.Services
{
    public class EntityService : IEntityService
    {
        private readonly IEntityRepository repository;
        public EntityService(IEntityRepository repository)
        {
            this.repository = repository;
        }
        public async Task CreateEntityAsync(CreateNewEntityRequest request)
        {
            var entity = new Entity
            {
                Name = request.Name,
                Fields = request.Fields
            };
            await repository.CreateAsync(entity);
        }

        public async Task DeleteEntityAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetEntityDisplayResponse>> GetAllEntitiesAsync()
        {
            var entities = await repository.GetAllAsync();
            var responses = entities.Select(e => new GetEntityDisplayResponse
            {
                Id = e.Id,
                Name = e.Name,
                Fields = e.Fields
            });
            return responses;
        }

        public async Task<GetEntityDisplayResponse> GetEntityAsync(int id)
        {
            var user = await repository.GetByIdAsync(id);
            var response = new GetEntityDisplayResponse
            {
                Id = user.Id,
                Name = user.Name,
                Fields = user.Fields
            };
            return response;
        }

        public Task UpdateEntityAsync(UpdateExistingEntityRequest request)
        {
            var updatedUser = new Entity
            {
                Id = request.Id,
                Name = request.Name,
                Fields = request.Fields
            };
            return repository.UpdateAsync(updatedUser);
        }

        public async Task<string> CreateDataTransferObjectCreateAsync(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            JArray? jsonModel = JsonConvert.DeserializeObject(entity.Fields) as JArray;
            if  (jsonModel != null) {
                List<Dictionary<string, string>?> newList = new List<Dictionary<string, string>?>();
                newList = jsonModel.Skip(1).Select(x => x.ToObject<Dictionary<string, string>>()).ToList();
                var json = JsonConvert.SerializeObject(newList);
                return json;
            }
            return "";
        }

        public async Task<string> CreateDataTransferObjectUpdateAndGetAsync(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            JArray? jsonModel = JsonConvert.DeserializeObject(entity.Fields) as JArray;
            if (jsonModel != null)
            {
                List<Dictionary<string, string>?> newList = new List<Dictionary<string, string>?>();
                newList = jsonModel.Select(x => x.ToObject<Dictionary<string, string>>()).ToList();
                var json = JsonConvert.SerializeObject(jsonModel);
                return json;
            }
            return "";
        }
    }
}
