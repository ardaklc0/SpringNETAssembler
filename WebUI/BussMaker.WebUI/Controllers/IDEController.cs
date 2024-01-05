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
        public async Task<IActionResult> Index(int id)
        {
            var existingEntity = await service.GetEntityAsync(id);
            ViewBag.Title = existingEntity.Name;
            return View();
        }

    }
}
