using Microsoft.AspNetCore.Mvc;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm nhân viên";
            return View();
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật nhân viên";
            return View("Create");
        }

        public IActionResult Repass()
        {
            ViewBag.Title = "Đổi mật khẩu";
            return View();
        }
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
