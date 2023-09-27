using Microsoft.AspNetCore.Mvc;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Thêm sản phẩm";
            return View();
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật sản phẩm";
            return View("Create");
        }
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
