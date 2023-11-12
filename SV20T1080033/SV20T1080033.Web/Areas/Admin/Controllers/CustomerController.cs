using Microsoft.AspNetCore.Mvc;
using SV20T1080033.BusinessLayer;
using SV20T1080033.DataLayers;
using SV20T1080033.DomainModels;
using SV20T1080033.Web.Models;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("Admin")]
    public class CustomerController : Controller
    {
        const int PAGE_SIZE = 10;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            var data = CommonDataService.ListOfCustomers(out int rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new PaginationSearchCustomer()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            string errorMessage = Convert.ToString(TempData["ErrorMessage"]);
            ViewBag.ErrorMessage = errorMessage;
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var model = new Customer()
            {
                CustomerID = 0,
            };
            ViewBag.Title = "Bổ sung khách hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetCustomer(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật khách hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id = 0)
        {
            var model = CommonDataService.GetCustomer(id);
            if (model == null)
                return RedirectToAction("Index");

            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteCustomer(id);
                if (!success)
                    TempData["ErrorMessage"] = "Không thể xóa khách hàng " + model.CustomerName;
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public IActionResult Save(Customer data)
        {
            ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";
            if (data.CustomerID == 0)
            {
                int customerID = CommonDataService.AddCustomer(data);
                if (customerID > 0)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
                return View("Create", data);
            }
            else
            {
                bool success = CommonDataService.UpdateCustomer(data);
                if (success)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
                return View("Create", data);
            }
        }
    }
}
