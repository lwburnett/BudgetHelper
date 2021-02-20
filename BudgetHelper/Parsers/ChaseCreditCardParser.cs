using BudgetHelper.Common;
using BudgetHelper.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudgetHelper.Parsers
{
    internal class ChaseCreditCardParser : IRawTransactionProvider
    {
        public ChaseCreditCardParser(string iFilePath)
        {
            _filePath = iFilePath;
        }

		public IEnumerable<RawTransaction> GetRawTransactions()
		{
			return File.
				ReadAllLines(_filePath).
				Skip(1).
				Select(l => CsvUtil.Deserialize(l).ToArray()).
				Where(p => p[2] != "AUTOMATIC PAYMENT - THANK").
				Select(p =>
					new RawTransaction(
						DateTime.Parse(p[0]),
						p[2],
						Math.Abs(double.Parse(p[5])),
						"Credit Card"));
		}

		private readonly string _filePath;
	}
}