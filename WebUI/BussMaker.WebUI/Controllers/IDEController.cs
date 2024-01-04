using BussMaker.Services;
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
        public async Task<IActionResult> Index()
        {
            var existingEntity = await service.GetEntityAsync(1);
            ViewBag.Title = existingEntity.Name;
            return View();
        }
    }
}
