

namespace API.DTO
{
    public class BasketDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }

        public List<BasketItemDto> Items { get; set; }

        public string PaymentIntendId { get; set; }

        public string ClientSecret { get; set; }
    }

}