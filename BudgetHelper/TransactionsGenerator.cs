using System.Collections.Generic;
using System.Linq;
using BudgetHelper.DataTypes;
using BudgetHelper.MapProviders;
using BudgetHelper.Parsers;

namespace BudgetHelper
{
	public class TransactionsGenerator : ITransactionsGenerator
	{
		public TransactionsGenerator(IMapProvider mapProvider, IRawTransactionProvider exportParser)
		{
			_mapProvider = mapProvider;
			_exportParser = exportParser;
		}

		public IEnumerable<Transaction> Generate()
		{
			var map = _mapProvider.GetMap();

			var rawTransactions = _exportParser.GetRawTransactions();

			return GetTransactions(rawTransactions, map);
		}

		#region Implementation

		private readonly IMapProvider _mapProvider;
		private readonly IRawTransactionProvider _exportParser;



		private static IEnumerable<Transaction> GetTransactions(
			IEnumerable<RawTransaction> rawData,
			CategoryMap map)
		{
			return rawData.Where(r => r != null).Select(r =>
			{
				var categoryInfo = map[r.VendorDescription];
				var establishment = categoryInfo?.EstablishmentName ?? r.VendorDescription;
				var category = categoryInfo?.CategoryName ?? "UNKNOWN";

				return new Transaction(
					r.Date,
					r.Date.Month,
					establishment,
					r.Cost,
					category,
					r.PaymentMethod);
			});
		}

		#endregion
	}
}