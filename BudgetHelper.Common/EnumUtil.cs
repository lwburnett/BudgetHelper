using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetHelper.Common
{
	public class EnumUtil
	{
		public static IEnumerable<T> GetValues<T>() =>
			Enum.GetValues(typeof(T)).Cast<T>();
	}
}
