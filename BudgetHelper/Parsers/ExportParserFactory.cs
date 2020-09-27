using System;
using System.Collections.Generic;
using BudgetHelper.Common;
using BudgetHelper.Enums;

namespace BudgetHelper.Parsers
{
	public static class ExportParserFactory
	{
		public static IRawTransactionProvider FromParameter(string providerSetting, string filePath)
		{
			if(!Enum.TryParse(providerSetting, out Provider provider))
				throw new ArgumentException(
					$"Unsupported provider {providerSetting}." +
					$"Supported providers are {string.Join(",", GetSupportedProviders())}");

			switch (provider)
			{
				case Provider.Citi:
					return new CitiRawTransactionProvider(filePath);
				case Provider.Oxford:
					throw new NotSupportedException();
				case Provider.ChaseBank:
					return new ChaseBankParser(filePath);
				case Provider.ChaseCreditCard:
					return new ChaseCreditCardParser(filePath);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static IEnumerable<Provider> GetSupportedProviders() =>
			EnumUtil.GetValues<Provider>();

	}
}
