using BuildMentor.Database.Entities;
using BuildMentor.Models;
using BuildMentor.Resources;
using BuildMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildMentor.Controllers
{
    [Authorize(Roles = "USER")]
    public class UserController : Controller
    {
        private readonly UnitService _unitService;
        private readonly UserManager<User> _userManager;
        public UserController(UnitService unitService, UserManager<User> userManager)
        {
            _unitService = unitService;
            _userManager = userManager;
        }
        [HttpGet("/User")]
        public async Task<IActionResult> Index()
        {
            var user = _unitService.UserService.Get(int.Parse(_userManager.GetUserId(User)));
            return View(user);
        }
        [HttpGet("/User/BrowseTools")]
        public async Task<IActionResult> BrowseTools()
        {
            var userId = int.Parse(_userManager.GetUserId(User));
            var user = _unitService.UserService.Get(userId);
            var allTools = _unitService.ToolService.GetAll();
            var userTools = user.UserTools.Select(t => t.Tool.Id);
            var requests = _unitService.ToolPermissionRequestService.GetAll().Where(r => r.UserId == userId);
            var toolsToBrowse = allTools.Where(t => !userTools.Contains(t.Id)  && !requests.Any(x=> x.ToolId == t.Id));

            return View(toolsToBrowse);
        }

        [HttpGet]
        [Route("/User/ToolDetails/{id}")]
        public async Task<IActionResult> ToolDetails(int id)
        {
            var tool = _unitService.ToolService.Get(id);
            return View(tool);
        }

        [HttpGet]
        [Route("/User/ToolCondition/{id}")]
        public async Task<IActionResult> ToolCondition(int id)
        {
            var tool = _unitService.UserToolService.Get(id);
            return View(tool);
        }

        [HttpPost]
        [Route("/User/RequestTool/{id}")]
        public async Task<IActionResult> RequestTool(int id, [FromBody] string request)
        {
            var userId = int.Parse(_userManager.GetUserId(User));
            var user = _unitService.UserService.Get(userId);
            var permissionRequest = new ToolPermissionRequest
            {
                ToolId = id,
                UserId = userId,
                CreatedDate = DateTime.Now,
                Comment = request
            };
            if(_unitService.ToolPermissionRequestService.GetAll().Any(r => r.ToolId == permissionRequest.ToolId && r.UserId == permissionRequest.UserId))
            {
                return BadRequest(new { Error = "Request already exists" });
            }
            if(user.UserTools.Any(x=> x.ToolId == id))
            {
                return BadRequest(new { Error = "User already has this tool" });
            }
            _unitService.ToolPermissionRequestService.Add(permissionRequest);
            return Ok(new { Message = "Success" });
        }

        [HttpGet]
        [Route("/User/Notifications")]
        public async Task<IActionResult> Notifications()
        {
            var userId = int.Parse(_userManager.GetUserId(User));
            var notifications = _unitService.UserNotificationService.GetAll().Where(n => n.UserId == userId);
            return View(notifications);
        }

        [HttpPost]
        [Route("/User/RequestAdmin")]
        public async Task<IActionResult> SendAdminRequest([FromBody] string message)
        {
            var userId = int.Parse(_userManager.GetUserId(User));
            var user = _unitService.UserService.Get(userId);
            var time = new TimeSpan(24, 0, 0);
            var adminRequest = new AdminRequest
            {
                SenderId = userId,
                Message = message
            };
            if(_unitService.AdminRequestService.GetAll().Any(r => r.SenderId == adminRequest.SenderId))
            {
                return BadRequest(new { Error = @Resource.ResourceManager.GetString("Request already exists") });
            }
            _unitService.AdminRequestService.Add(adminRequest);
            return Ok(new { Message = "Success" });
        }
    }
}