using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetHelper.DataTypes
{
	public class CategoryMap
	{
		public CategoryMap(IEnumerable<Tuple<string, string>> data)
		{
			var theData = data.ToList();

			for (var i = 0; i < theData.Count; i++)
			{
				var thisKey = theData[i].Item1;
				if (theData.
					Where((t, j) => j != i).
					Any(t => t.Item1.
						ToUpperInvariant().
						Contains(thisKey.ToUpperInvariant())))
					throw new Exception($"Key {thisKey} is contained in another key.");
			}

			_data = theData.
				Where(d =>
					d != null &&
					!string.IsNullOrWhiteSpace(d.Item1) &&
					!string.IsNullOrWhiteSpace(d.Item2)).
				ToDictionary(
					d => string.Copy(d.Item1).ToUpperInvariant(),
					d => new CategoryMapping(d.Item1, d.Item2));
		}

		public CategoryMapping this[string description]
		{
			get { return _data.FirstOrDefault(d => description.ToUpperInvariant().Contains(d.Key)).Value; }
		}

		private readonly Dictionary<string, CategoryMapping> _data;
	}
}
