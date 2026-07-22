using IMS.CoreBusiness;

namespace IMS.UseCases.Reports.Interfaces;

public interface ISearchProductTransactionsUseCase
{
	Task<IEnumerable<ProductTransaction>> ExecuteAsync(
		string inventoryName,
		DateTime? dateFrom,
		DateTime? dateTo,
		ProductTransactionTypes? transactionType
	);
}