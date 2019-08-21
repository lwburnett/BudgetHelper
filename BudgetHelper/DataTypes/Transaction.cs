using System;

namespace BudgetHelper.DataTypes
{
	public class Transaction
	{
		public Transaction(
			DateTime date,
			int month,
			string establishment,
			double amount,
			string category,
			string paymentMethod)
		{
			Date = date;
			Month = month;
			Establishment = establishment;
			Amount = amount;
			Category = category;
			PaymentMethod = paymentMethod;
		}

		public DateTime Date { get; }
		public int Month { get; }
		public string Establishment { get; }
		public double Amount { get; }
		public string Category { get; }
		public string PaymentMethod { get; }
	}
}
