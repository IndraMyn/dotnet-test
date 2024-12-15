using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    public class ComCustomer
    {
        [Column("COM_CUSTOMER_ID")]
        public int ComCustomerId { get; set; }

        [Column("CUSTOMER_NAME")]
        [StringLength(100)]
        public string? CustomerName { get; set; }
        
        public ICollection<SoOrder>? SoOrders { get; set; }
    }
}
