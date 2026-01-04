using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace deposito_do_pitty.Domain.Entities
{
    [Table("sales")]
    public class Sale
    {
        [Column("id")]
        public int id { get; set; }

        [Column("customerid")]
        public int? customerid { get; set; }

        [Column("customername")]
        public string? customername { get; set; }

        [Column("date")]
        public DateTimeOffset date { get; set; }

        [Column("paymenttype")]
        public string paymenttype { get; set; } = string.Empty;

        [Column("status")]
        public string status { get; set; } = string.Empty;

        [Column("subtotal")]
        public decimal subtotal { get; set; }

        [Column("discountpercent")]
        public decimal discountpercent { get; set; }

        [Column("discountvalue")]
        public decimal discountvalue { get; set; }

        [Column("total")]
        public decimal total { get; set; }

        [Column("createdat")]
        public DateTimeOffset createdat { get; set; }

        [Column("updatedat")]
        public DateTimeOffset updatedat { get; set; }

        public ICollection<SaleItem> items { get; set; } = new List<SaleItem>();
    }
}
