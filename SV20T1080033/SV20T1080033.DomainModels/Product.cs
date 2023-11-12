﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080033.DomainModels
{
	public class Product
	{
		public int ProductID { get; set; }
		public string ProductName { get; set; } = "";
		public string ProductDescription { get; set; } = "";
		public string Unit { get; set; } = "";
		public double Price { get; set; }
		public string Photo { get; set; } = "";
		public bool IsSelling { get; set; }
	}
}
