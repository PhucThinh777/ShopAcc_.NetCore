namespace ShopAcc.Models
{
    public class CartItemModel
    {
        public int NickId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string AnhBia { get; set; }
        public decimal Total
        {
            get
            {
                return Quantity * Price;
            }
        }
        public CartItemModel()
        {

        }
        public CartItemModel(NickModel nick)
        {
            NickId = nick.Id;
            Description = nick.Description;
            Quantity = 1;
            Price = nick.Price;
            AnhBia = nick.AnhBia;
        }
    }
}
