using BussMaker.DataTransferObject.Requests;
using BussMaker.Services;
using Microsoft.AspNetCore.Mvc;

namespace BussMaker.WebUI.Controllers
{
    public class EntityController : Controller
    {
        private readonly IEntityService service;
        public EntityController(IEntityService service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            var entities = await service.GetAllEntitiesAsync();
            return View(entities);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateNewEntityRequest request)
        {
            if (ModelState.IsValid)
            {
                await service.CreateEntityAsync(request);
                return RedirectToAction("Index");
            }
            return View();
        }  
    }
}
