using System.Collections.Generic;
using System.Linq;

namespace BudgetHelper.Common
{
	public class CsvUtil
	{
		public static string Serialize(IEnumerable<object> pieces) =>
			string.Join(", ", pieces);

		public static IEnumerable<string> Deserialize(string line) =>
			line.Split(',').Select(p => p.Trim());
	}
}
