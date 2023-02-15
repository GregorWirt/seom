using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seom.Application.Model
{
    internal class Basket
    {
        public Basket(int id, decimal discount, string currencySymbol, string currencyCode)
        {
            Id = id;
            Discount = discount;
            CurrencySymbol = currencySymbol;
            CurrencyCode = currencyCode;
        }

        public int Id { get; set; }
        public decimal Discount { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCode  { get; set; }

    }
}
