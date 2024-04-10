namespace Niveau.Areas.User.Models.ShoppingCart
{
    public class ShoppingCart
    {
        //danh sách các sản phẩm trong giỏ hàng
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        //các hàm thêm/ xóa sản phẩm trong giỏ hàng
        public void AddItem(CartItem item)
        {
            //tìm xem sản phẩm đã có trong giỏ hàng chưa
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)//nếu sp đã có trong giỏ hàng
            {
                existingItem.Quantity += item.Quantity;//cập nhật số lượng
            }
            else//nếu chưa có
            {
                Items.Add(item);//thêm mới
            }
        }
        //xóa sản phẩm khỏi giỏ hàng
        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }
    }
}
