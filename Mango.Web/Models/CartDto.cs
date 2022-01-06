namespace Mango.Web.Models
{
    public class CartDto
    {
        public CartDto()
        {
            Header = new CartHeaderDto();
            Details = new List<CartDetailDto>();
        }

        public CartHeaderDto Header { get; set; }
        public IEnumerable<CartDetailDto> Details { get; set; }
    }
}
