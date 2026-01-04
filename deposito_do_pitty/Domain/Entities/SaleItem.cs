using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace deposito_do_pitty.Domain.Entities
{
    [Table("sale_items")]
    public class SaleItem
    {
        [Column("id")]
        public int id { get; set; }

        [Column("saleid")]
        public int saleid { get; set; }

        [Column("productid")]
        public int productid { get; set; }

        [Column("quantity")]
        public int quantity { get; set; }

        [Column("unitprice")]
        public decimal unitprice { get; set; }

        [Column("subtotal")]
        public decimal subtotal { get; set; }

        [Column("createdat")]
        public DateTimeOffset createdat { get; set; }

        [ForeignKey(nameof(saleid))]
        public Sale? sale { get; set; }
    }
}
