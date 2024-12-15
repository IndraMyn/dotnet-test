using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    public class SoOrder
    {
        [Column("SO_ORDER_ID")]
        public long SoOrderId { get; set; }

        [Column("ORDER_NO")]
        [StringLength(20)]
        public string OrderNo { get; set; } = string.Empty;

        [Column("ORDER_DATE")]
        public DateTime OrderDate { get; set; }

        [Column("COM_CUSTOMER_ID")]
        public int ComCustomerId { get; set; }
        public ComCustomer? ComCustomer { get; set; }

        [Column("ADDRESS")]
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;

        public ICollection<SoItem>? SoItems { get; set; }
    }
}
