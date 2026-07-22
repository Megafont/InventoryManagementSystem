using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMS.CoreBusiness
{
	public class InventoryTransaction
	{
		public int InventoryTransactionID { get; set; }
		public string PurchaseOrderNumber { get; set; } = string.Empty;
		public string ProductionNumber { get; set; } = string.Empty;

		[Required]
		public int InventoryID { get; set; }

		[Required]
		public int QuantityBefore { get; set; }
		[Required]
		public InventoryTransactionTypes ActivityType { get; set; }

		[Required]
		public int QuantityAfter { get; set; }

		[Required]
		public decimal? UnitPrice { get; set; }

		[Required]
		public DateTime TransactionDate { get; set; }
		[Required] 
		public string DoneBy { get; set; } = string.Empty;

		public Inventory Inventory { get; set; }
	}

}
