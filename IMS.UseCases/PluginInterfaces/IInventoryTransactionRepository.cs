using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
	public interface IInventoryTransactionRepository
	{
		Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string purchasedBy, decimal price);
		Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string producedBy, decimal price);
	}
}