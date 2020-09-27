using System;
using System.IO;
using System.Linq;
using BudgetHelper.Common;
using BudgetHelper.DataTypes;

namespace BudgetHelper.MapProviders
{
	public class CsvMapProvider : IMapProvider
	{
		public CsvMapProvider(string filePath)
		{
			_filePath = filePath;
		}

		public CategoryMap GetMap()
		{
			var tuples = File.
				ReadAllLines(_filePath).
				Where(l => !string.IsNullOrWhiteSpace(l)).
				Where(l => l.Contains(',')).
				Select(l => CsvUtil.Deserialize(l).ToArray()).
				Select(p => new Tuple<string, string>(p[0], p[1]));

			return new CategoryMap(tuples);
		}


		private readonly string _filePath;
	}
}