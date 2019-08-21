using System.Collections.Generic;
using BudgetHelper.DataTypes;

namespace BudgetHelper.Parsers
{
	public interface IRawTransactionProvider
	{
		IEnumerable<RawTransaction> GetRawTransactions();
	}
}
