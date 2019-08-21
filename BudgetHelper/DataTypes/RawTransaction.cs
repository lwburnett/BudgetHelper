using System;

namespace BudgetHelper.DataTypes
{
	public class RawTransaction
	{
		public RawTransaction(
			DateTime date,
			string vendorDescription,
			double cost, string paymentMethod)
		{
			Date = date;
			VendorDescription = vendorDescription;
			Cost = cost;
			PaymentMethod = paymentMethod;
		}

		public DateTime Date { get; }
		public string VendorDescription { get; }
		public double Cost { get; }
		public string PaymentMethod { get; }
	}
}
