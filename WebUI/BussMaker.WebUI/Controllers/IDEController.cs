using BussMaker.Services;
using BussMaker.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BussMaker.WebUI.Controllers
{
    public class IDEController : Controller
    {
        private readonly IEntityService service;
        public IDEController(IEntityService service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index(int id)
        {
            var existingEntity = await service.GetEntityAsync(id);
            ViewBag.Title = existingEntity.Name;
            var dtoModel = new DataTransferObjectViewModel
            {
                DataTransferObjectCreate = await service.CreateDataTransferObjectCreateAsync(id),
                DataTransferObjectUpdateAndGet = await service.CreateDataTransferObjectUpdateAndGetAsync(id),
                RepositoryList = await service.CreateEFRepository(id),
                ServiceList = await service.CreateService(id)
            };
            return View(dtoModel);
        }
        public async Task<IActionResult> CreateFirst()
        {
            await service.CreateDataTransferObjectCreateAsync(5);
            return View();
        }

        public async Task<IActionResult> CreateSecond()
        {
            await service.CreateDataTransferObjectUpdateAndGetAsync(5);
            return View();
        }
    }
}
