using System.Collections.Generic;
using BudgetHelper.DataTypes;

namespace BudgetHelper
{
	public interface ITransactionsGenerator
	{
		IEnumerable<Transaction> Generate();
	}
}
