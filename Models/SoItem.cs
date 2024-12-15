using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    public class SoItem
    {
        [Column("SO_ITEM_ID")]
        public long SoItemId { get; set; }

        [Column("SO_ORDER_ID")]
        public long SoOrderId { get; set; }
        public SoOrder? SoOrder { get; set; }

        [Column("ITEM_NAME")]
        [StringLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [Column("QUANTITY")]
        public int Quantity { get; set; }

        [Column("PRICE")]
        public double Price { get; set; }
    }
}
