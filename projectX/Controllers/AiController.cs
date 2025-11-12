using Microsoft.AspNetCore.Mvc;
using projectX.Services;

namespace projectX.Controllers
{
    public class AiController : Controller
    {
        private readonly IOllamaService _ollamaService;

        public AiController(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Ask(string prompt)
        {
            try
            {
                var response = await _ollamaService.AskAsync(prompt);
                return Json(new { success = true, response = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, response = $"Error: {ex.Message}" });
            }
        }
    }
}