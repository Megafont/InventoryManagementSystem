using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;
public interface IProductTransactionRepository
{
	Task ProduceAsync(string productionNumber, Product product, int quantity, string producedBy);
	Task SellProductAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice, string soldBy);
	Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionTypes? transactionType);
}