using PackageFoodManagementSystem.Repository.Models;

public interface IOrderService
{
    IEnumerable<Order> GetAllOrders();
    Order GetOrderById(int orderId);
    void PlaceOrder(Order order);
    // Add 'changedBy' to track who or what changed the status
    void UpdateOrderStatus(int orderId, string status, string changedBy, string remarks = "");
    void CancelOrder(int orderId);
    int CreateOrder(int userId, string deliveryAddress);
}