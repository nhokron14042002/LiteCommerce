﻿using Dapper;
using SV20T1080033.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080033.DataLayers.SQLServer
{
	public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
	{
		public SupplierDAL(string connectionString) : base(connectionString)
		{
		}

		public int Add(Supplier data)
		{
			int id = 0;
			using (var connection = OpenConnection())
			{
				var sql = @"if exists(select * from Suppliers where Email = @Email)
                                select -1
                            else
                                begin
                                    insert into Suppliers(SupplierName,ContactName,Province,Address,Phone,Email)
                                    values(@SupplierName,@ContactName,@Province,@Address,@Phone,@Email);
                                    select @@identity;
                                end";
				var parameters = new
				{
					SupplierName = data.SupplierName ?? "",
					ContactName = data.ContactName ?? "",
					Province = data.Province ?? "",
					Address = data.Address ?? "",
					Phone = data.Phone ?? "",
					Email = data.Email ?? ""
					
				};
				id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
				connection.Close();
			}
			return id;
		}

		public int Count(string searchValue = "")
		{
			int count = 0;
			if (!string.IsNullOrEmpty(searchValue))
				searchValue = "%" + searchValue + "%";
			using (var connection = OpenConnection())
			{
				var sql = @"select count(*) from Suppliers
                            where (@searchValue = N'') or (SupplierName like @searchValue)";
				var parameter = new
				{
					searchValue,
				};
				count = connection.ExecuteScalar<int>(sql: sql, param: parameter, commandType: CommandType.Text);
				connection.Close();
			}

			return count;
		}

		public bool Delete(int id)
		{
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "delete from Suppliers where SupplierID = @SupplierId and not exists(select * from Orders where SupplierID = @SupplierId)";
                var parameters = new { SupplierId = id };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

		public Supplier? Get(int id)
		{
            Supplier? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select * from Suppliers where SupplierId = @SupplierId";
                var parameters = new { SupplierId = id };
                data = connection.QueryFirstOrDefault<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

		public bool InUsed(int id)
		{
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where SupplierId = @SupplierId)
                                select 1
                            else 
                                select 0";
                var parameters = new { SupplierId = id };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

		public IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
		{
			List<Supplier> data = new List<Supplier>();
			if (!string.IsNullOrEmpty(searchValue))
				searchValue = "%" + searchValue + "%";
			using (var connection = OpenConnection())
			{
				var sql = @"with cte as
                            (
	                            select *, ROW_NUMBER() over (order by SupplierName) as RowNumber
	                            from Suppliers
	                            where (@searchValue = N'' or (SupplierName like @searchValue))
                            )
                            select * from cte
                            where (@pageSize = 0)
	                            or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";

				var parameters = new
				{
					page,   //nếu trùng tên
					pageSize = pageSize,
					searchValue = searchValue
				};
				data = (connection.Query<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text)).ToList();
				connection.Close();
			}
			if (data == null)
				data = new List<Supplier>();
			return data;
		}

		public bool Update(Supplier data)
		{
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Suppliers where SupplierID <> @SupplierId and Phone = @Phone)
                                begin
                                    update Suppliers 
                                    set SupplierName = @SupplierName,
                                        Phone = @Phone                
                                    where SupplierID = @SupplierId
                                end";
                var parameters = new
				{
					SupplierId= data.SupplierID ,
					SupplierName = data.SupplierName ?? "",
                    Phone = data.Phone ?? ""
                   
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }
	}
}
