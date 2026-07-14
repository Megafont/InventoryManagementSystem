using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
	public interface IInventoryTransactionRepository
	{
		void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string purchasedBy, decimal price);
	}
}