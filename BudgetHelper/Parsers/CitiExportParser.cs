using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BudgetHelper.Common;
using BudgetHelper.DataTypes;

namespace BudgetHelper.Parsers
{
	// ReSharper disable once IdentifierTypo
	public class CitiRawTransactionProvider : IRawTransactionProvider
	{
		// ReSharper disable once IdentifierTypo
		public CitiRawTransactionProvider(string filePath)
		{
			_filePath = filePath;
		}

		public IEnumerable<RawTransaction> GetRawTransactions()
		{
			return File.
				ReadAllLines(_filePath).
				Skip(1).
				Select(l => CsvUtil.Deserialize(l).ToArray()).
				Where(p => !string.IsNullOrWhiteSpace(p[3])).
				Select(p =>
					new RawTransaction(
						DateTime.Parse(p[1]),
						p[2],
						double.Parse(p[3]),
						"Credit Card"));
		}

		private readonly string _filePath;
	}
}
