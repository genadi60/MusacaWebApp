namespace MusacaWebApp.Controllers
{
    using System.Linq;

    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    
    public class OrdersController : BaseController
    {
        [Authorize]
        public IHttpResponse Create(string barcode, int quantity)
        {
            var product = Db.Products.FirstOrDefault(p => p.Barcode.Equals(barcode));
            if (product == null)
            {
                return View();
            }
            
            var order = new Order
            {
                Product = product,
                Quantity = quantity,
                Cashier = Db.Users.Single(u => u.Username.Equals(User.Username))
            };

            Db.Orders.Add(order);
            Db.SaveChanges();

            return Redirect("/");
        }
    }
}