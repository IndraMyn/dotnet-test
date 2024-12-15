namespace test.Data
{
    public class SoOrderDetailDto : SoOrderDto
    {
        public List<SoItemDto> Items { get; set; }
        public string Address { get; set; }
    }
}
