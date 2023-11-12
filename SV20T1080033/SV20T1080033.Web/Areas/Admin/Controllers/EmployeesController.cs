using Microsoft.AspNetCore.Mvc;
using SV20T1080033.BusinessLayer;
using SV20T1080033.DataLayers;
using SV20T1080033.DomainModels;
using SV20T1080033.Web.Models;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{

   
    [Area("Admin")]
    public class EmployeesController : Controller
    {
		const int PAGE_SIZE = 10;
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IActionResult Index(int page = 1, string searchValue = "")
		{
			var data = CommonDataService.ListOfEmployees(out int rowCount, page, PAGE_SIZE, searchValue ?? "");
			var model = new PaginationSearchEmployee()
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
			var model = new Employee()
			{
				EmployeeID = 0,
			};
			ViewBag.Title = "Bổ sung nhân viên";
			return View(model);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Edit(int id = 0)
		{
			var model = CommonDataService.GetEmployee(id);
			if (model == null)
				return RedirectToAction("Index");

			ViewBag.Title = "Cập nhật nhân viên";
			return View("Create", model);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Delete(int id = 0)
		{
			var model = CommonDataService.GetEmployee(id);
			if (model == null)
				return RedirectToAction("Index");

			if (Request.Method == "POST")
			{
				bool success = CommonDataService.DeleteEmployee(id);
				if (!success)
					TempData["ErrorMessage"] = "Không thể xóa nhân viên " + model.FullName;
				return RedirectToAction("Index");
			}

			return View(model);
		}
		public IActionResult Save(Employee data)
		{
			ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật nhân viên";
			if (data.EmployeeID == 0)
			{
				int EmployeeID = CommonDataService.AddEmployee(data);
				if (EmployeeID > 0)
					return RedirectToAction("Index");

				ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
				return View("Create", data);
			}
			else
			{
				bool success = CommonDataService.UpdateEmployee(data);
				if (success)
					return RedirectToAction("Index");

				ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
				return View("Create", data);
			}
		}
	}
}
