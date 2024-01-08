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
        public async Task<List<Dictionary<string, string>>> CreateDataTransferObjectCreateAsync(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            JArray? jsonModel = JsonConvert.DeserializeObject(entity.Fields) as JArray;
            if  (jsonModel != null) {
                List<Dictionary<string, string>> newList = new List<Dictionary<string, string>?>();
                List<Dictionary<string, string>?> jsonList = jsonModel.Skip(1).Select(x => x.ToObject<Dictionary<string, string>>()).ToList();
                int rowNumber = 0;
                foreach (var item in jsonList)
                {
                    Dictionary<string, string> keyValuePair = new Dictionary<string, string>();
                    keyValuePair.Add($"row{rowNumber}", $"{item["keyword"]} {item["type"]} {item["variable"]}"+" {get; set;}");
                    newList.Add(keyValuePair);
                    rowNumber++;
                }
                return newList;
            }
            return new List<Dictionary<string, string>>();
        }
        public async Task<List<Dictionary<string, string>>> CreateDataTransferObjectUpdateAndGetAsync(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            JArray? jsonModel = JsonConvert.DeserializeObject(entity.Fields) as JArray;
            if (jsonModel != null)
            {
                List<Dictionary<string, string>> newList = new List<Dictionary<string, string>?>();
                List<Dictionary<string, string>?> jsonList = jsonModel.Select(x => x.ToObject<Dictionary<string, string>>()).ToList();
                int rowNumber = 0;
                foreach (var item in jsonList)
                {
                    Dictionary<string, string> keyValuePair = new Dictionary<string, string>();
                    keyValuePair.Add($"row{rowNumber}", $"{item["keyword"]} {item["type"]} {item["variable"]}" + " {get; set;}");
                    newList.Add(keyValuePair);
                    rowNumber++;
                }
                return newList;
            }
            return new List<Dictionary<string, string>>();
        }
        public async Task<List<Dictionary<string, string>>> CreateService(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            string entityName = entity.Name;
            string nameLower = entityName.ToLower();
            string getDto = "Get" + entityName + "DisplayResponse";
            string updateDto = "UpdateExisting" + entityName + "Request";
            string createDto = "CreateNew" + entityName + "Request";
            JArray? jsonModel = JsonConvert.DeserializeObject(entity.Fields) as JArray;
            List<Dictionary<string, string>?> jsonList = jsonModel.Select(x => x.ToObject<Dictionary<string, string>>()).ToList();
            List<string> variablesForCreate = jsonList.Skip(1).Select(x => x["variable"]).ToList();
            List<string> variablesForGetAndUpdate = jsonList.Select(x => x["variable"]).ToList();
            string mappingsForCreate = "";
            foreach(var variable in variablesForCreate)
            {
                mappingsForCreate += $"\t\t\t{variable} = request.{variable}\n";
            }
            string mappingsForGetAndUpdate = "";
            foreach (var variable in variablesForGetAndUpdate)
            {
                mappingsForGetAndUpdate += $"\t\t\t{variable} = request.{variable}\n";
            }
            List<Dictionary<string, string>> scripts = new()
            {
                new Dictionary<string, string>
                {
                    {"createAsync",
                    $"\tpublic async Task Create{entityName}Async({createDto} request) \n" +
                    "\t{\n" +
                    $"\t\tvar {nameLower} = new {entityName}\n" +
                    "\t\t{\n" +
                    $"{mappingsForCreate}" +
                    "\t\t}\n" +
                    $"\t\tawait repository.CreateAsync({entityName})\n" +
                    "\t}\n"
                    },
                    {"deleteAsync",
                    $"\tpublic async Task Delete{entityName}Async(int id) \n" +
                    "\t{\n" +
                    $"\t\tawait repository.DeleteAsync(id);\n" +
                    "\t}\n"
                    },
                    {"getAllAsync",
                    $"\tpublic async Task<IEnumerable<{getDto}>> GetAll{getDto}Async() \n" +
                    "\t{\n" +
                    $"\t\tvar {nameLower}s = await repository.GetAllAsync();\n" +
                    $"\t\tvar responses = {nameLower}s.Select(request => new {getDto}\n" +
                    "\t\t{\n" +
                    $"{mappingsForGetAndUpdate}" +
                    "\t\t});\n" +
                    "\t\treturn responses;\n" +
                    "\t}\n"
                    },
                    {"getAsync",
                    $"\tpublic async Task<{getDto}> Get{getDto}Async(int id) \n" +
                    "\t{\n" +
                    $"\t\tvar request = await repository.GetByIdAsync(id);\n" +
                    $"\t\tvar response = new {getDto}\n" +
                    "\t\t{\n" +
                    $"{mappingsForGetAndUpdate}" +
                    "\t\t};\n" +
                    "\t\treturn response;\n" +
                    "\t}\n"
                    },
                    {"updateAsync",
                    $"\tpublic async Task Update{getDto}Async({updateDto} request) \n" +
                    "\t{\n" +
                    $"\t\tvar updated{entityName} = new {entityName}\n" +
                    "\t\t{\n" +
                    $"{mappingsForGetAndUpdate}" +
                    "\t\t};\n" +
                    $"\t\treturn repository.UpdateAsync(updated{entityName})\n" +
                    "\t}\n"
                    }
                }
            };
            return scripts;
        }
        public async Task<List<Dictionary<string, string>>> CreateEFRepository(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            string entityName = entity.Name;
            string nameLower = entityName.ToLower();
            string dbContextName = nameLower + "DbContext";
            List<Dictionary<string, string>> scripts = new()
            {
                new Dictionary<string, string>
                {
                    {"createAsync",
                    $"\tpublic async Task CreateAsync({entityName} {nameLower}) \n" +
                    "\t{\n" +
                    $"\t\tawait {dbContextName}.{entityName}.AddAsync({nameLower});\n" +
                    $"\t\tawait {dbContextName}.SaveChangesAsync();\n" +
                    "\t}\n"
                    },
                    {"deleteAsync",
                    $"\tpublic async Task DeleteAsync(int id) \n" +
                    "\t{\n" +
                    $"\t\tvar existing{entityName} = await {dbContextName}.{entityName}.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);\n" +
                    $"\t\tif (existing{entityName} != null)\n"+
                    "\t\t{\n" +
                    $"\t\t\t{dbContextName}.{entityName}.Remove(existing{entityName});\n" +
                    $"\t\t\tawait {dbContextName}.SaveChangesAsync();\n" +
                    "\t\t}\n" +
                    "\t}\n"
                    },
                    {"getAllAsync",
                    $"\tpublic async Task<List<{entityName}>> GetAllAsync() \n" + 
                    "\t{\n" +
                    $"\t\treturn await {dbContextName}.{entityName}.AsNoTracking().ToListAsync();\n" +
                    "\t}\n"
                    },
                    {"getByIdAsync",
                    $"\tpublic async Task<{entityName}> GetByIdAsync(int id) \n" +
                    "\t{\n" +
                    $"\t\treturn await {dbContextName}.{entityName}.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);\n" +
                    "\t}\n"
                    },
                    {"UpdateAsync",
                    $"\tpublic async Task UpdateAsync({entityName} {nameLower}) \n" +
                    "\t{\n" +
                    $"\t\tawait {dbContextName}.{entityName}.Update({nameLower});\n" +
                    $"\t\tawait {dbContextName}.SaveChangesAsync();\n" +
                    "\t}\n"
                    }
                },
            };
            return scripts;
        }
    }
}
