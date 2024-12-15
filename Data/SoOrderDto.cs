namespace test.Data
{
    public class SoOrderDto
    {
        public long ID { get; set; }
        public string SalesOrder { get; set; }
        public DateOnly OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
    }
}
