using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080033.DomainModels
{
	public class Employee
	{
		public int EmployeeID { get; set; }
		public string FullName { get; set; } = "";
		public DateOnly Birthday { get; set; }
		public string Address { get; set; } = "";
		public string Phone { get; set; } = "";
		public string Email { get; set; } = "";
		public string Photo { get; set; } = "";
		public bool IsWorking { get; set; }
	}
}
