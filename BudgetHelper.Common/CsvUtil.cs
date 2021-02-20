using System.Collections.Generic;
using System.Linq;

namespace BudgetHelper.Common
{
	public class CsvUtil
	{
		public static string Serialize(IEnumerable<object> pieces) =>
			string.Join(", ", pieces);

		public static IEnumerable<string> Deserialize(string line, System.StringSplitOptions splitOptions = System.StringSplitOptions.RemoveEmptyEntries) =>
			line.Split(new[] { ',' }, splitOptions).Select(p => p.Trim());
	}
}
