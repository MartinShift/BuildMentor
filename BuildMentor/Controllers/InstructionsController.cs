using Microsoft.AspNetCore.Mvc;
using BuildMentor.Services;
using BuildMentor.Models;
using Microsoft.AspNetCore.Authorization;
using BuildMentor.Database.Entities;
using System.Text.RegularExpressions;

namespace BuildMentor.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("/Admin/Instructions")]
    public class InstructionsController : Controller
    {
        private readonly UnitService _unitService;

        public InstructionsController(UnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        [Route("/Admin/Instructions/{toolId}")]
        public IActionResult Index(int toolId)
        {
            ViewData["ToolId"] = toolId;
            var instructions = _unitService.InstructionService.GetByProduct(toolId);

            return View("/Views/Admin/Instructions/Index.cshtml",instructions);
        }

        [HttpGet]
        [Route("/Admin/Instructions/Add/{toolId}")]
        public IActionResult Add(int toolId)
        {
            return View("/Views/Admin/Instructions/Add.cshtml", new InstructionModel() { ToolId = toolId });
        }

        [HttpPost]
        [Route("/Admin/Instructions/Add")]
        public IActionResult Add([FromBody] InstructionModel model)
        {
            var response = _unitService.InstructionService.Validate(model);
            if (response != string.Empty)
            {
                return BadRequest(new { Error = response });
            }
            model.LastUpdatedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            _unitService.InstructionService.Add(_unitService.InstructionService.MapToEntity(model));

            return Ok();
        }

        [HttpPost]
        [Route("/Admin/Instructions/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var instruction = _unitService.InstructionService.Get(id);
            _unitService.InstructionService.Delete(id);

            return RedirectToAction("Index", new { toolId = instruction.ToolId });
        }
        [HttpGet]
        [Route("/Admin/Instructions/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var instruction = _unitService.InstructionService.Get(id);
            if (instruction == null)
            {
                return NotFound();
            }

            return View("/Views/Admin/Instructions/Edit.cshtml", _unitService.InstructionService.MapToModel(instruction));
        }

        [HttpPost]
        [Route("/Admin/Instructions/Update")]
        public IActionResult Update([FromBody] InstructionModel model)
        {
            var response = _unitService.InstructionService.Validate(model);
            if(response != string.Empty)
            {
                return BadRequest(new {Error = response});
            }
            _unitService.InstructionService.Update(model);
            return Ok();
        }
    }
}
