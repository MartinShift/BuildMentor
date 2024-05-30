using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BuildMentor.Database.Entities;
using BuildMentor.Services;
using BuildMentor.Models;
using BuildMentor.Database.Enums;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Authorization;

namespace BuildMentor.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("/Admin/Tools")]
    public class ToolsController : Controller
    {
        private readonly UnitService unitService;

        public ToolsController(UnitService unitService)
        {
            this.unitService = unitService;
        }

        [HttpGet]
        [Route("/Admin/Tools")]
        public IActionResult Index()
        {
            var tools = unitService.ToolService.GetAll();
            return View("/Views/Admin/Tools/Index.cshtml", tools);
        }

        [HttpGet]
        [Route("/Admin/Tools/Add")]
        public IActionResult Add()
        {
            return View("Views/Admin/Tools/Add.cshtml");
        }

        [HttpPost]
        [Route("/Admin/Tools/Add")]
        public async Task<IActionResult> Add([FromForm] ToolModel toolModel)
        {
            var image = toolModel.UploadedImage != null ? await unitService.ImageService.Upload(toolModel.UploadedImage) : null;
            var tool = unitService.ToolService.MapToEntity(toolModel);
            tool.Image = image;
            unitService.ToolService.Add(tool);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/Admin/Tools/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            unitService.ToolService.Delete(id);
            return Ok();
        }

        [HttpGet]
        [Route("/Admin/Tools/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var model = unitService.ToolService.MapToUpdateModel(unitService.ToolService.Get(id));
            return View("Views/Admin/Tools/Edit.cshtml", model);
        }

        [HttpPost]
        [Route("/Admin/Tools/Update")]
        public async Task<IActionResult> Update([FromForm] ToolUpdateModel toolModel)
        {
            var image = toolModel.UploadedImage != null ? await unitService.ImageService.Upload(toolModel.UploadedImage) : null;
            var tool = unitService.ToolService.MapUpdateToEntity(toolModel);
            if (image != null)
            {
                tool.Image = image;
            }
            unitService.ToolService.Update(tool);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Admin/Tools/View/{id}")]
        public IActionResult View(int id)
        {
            var tool = unitService.ToolService.Get(id);
            return View(tool);
        }
    }
}
