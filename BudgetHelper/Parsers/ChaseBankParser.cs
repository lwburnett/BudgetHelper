using BudgetHelper.Common;
using BudgetHelper.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudgetHelper.Parsers
{
    public class ChaseBankParser : IRawTransactionProvider
	{
		public ChaseBankParser(string filePath)
		{
			_filePath = filePath;
		}

		public IEnumerable<RawTransaction> GetRawTransactions()
		{
			return File.
				ReadAllLines(_filePath).
				Skip(1).
				Select(l => CsvUtil.Deserialize(l).ToArray()).
				Where(p => !p[2].Contains("CHASE CREDIT CRD AUTOPAY")).
				Select(p =>
					new RawTransaction(
						DateTime.Parse(p[1]),
						p[2],
						Math.Abs(double.Parse(p[3])),
						p[4] == "DEBIT_CARD" ? "Debit Card" : string.Empty));
		}

		private readonly string _filePath;
	}
}
