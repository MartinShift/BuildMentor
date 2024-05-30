using Microsoft.AspNetCore.Mvc;
using BuildMentor.Services;

namespace BuildMentor.Controllers
{
    public class ViewController : Controller
    {
        private readonly UnitService unitService;

        public ViewController(UnitService unitService)
        {
            this.unitService = unitService;
        }

        [Route("/View/Tool/{id}")]
        [HttpGet]
        public IActionResult Tool(int id)
        {
            var tool = unitService.ToolService.Get(id);
            return View(tool);
        }

        [Route("/View/Instruction/{id}")]
        [HttpGet]
        public IActionResult Instruction(int id)
        {
            var instruction = unitService.InstructionService.Get(id);
            return View(instruction);
        }

    }
}
