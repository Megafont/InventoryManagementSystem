using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMS.CoreBusiness
{
	public class ProductTransaction
	{
		public int ProductionTransactionID { get; set; }
		public string SalesOrderNumber { get; set; } = string.Empty;
		public string ProductionNumber { get; set; } = string.Empty;

		[Required]
		public int ProductID { get; set; }

		[Required]
		public int QuantityBefore { get; set; }
		[Required]
		public ProductTransactionTypes ActivityType { get; set; }

		[Required]
		public int QuantityAfter { get; set; }

		[Required]
		public decimal? UnitPrice { get; set; }

		[Required]
		public DateTime TransactionDate { get; set; }
		[Required] 
		public string DoneBy { get; set; } = string.Empty;

		public Product Product { get; set; }
	}

}
