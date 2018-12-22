namespace MusacaWebApp.Controllers
{
    using System;
    using System.Linq;

    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels;

    public class ReceiptsController : BaseController
    {
        [Authorize(nameof(Role.Admin))]

        public IHttpResponse All()
        {
            var receipts = Db.Receipts
                .Select(r => new ReceiptViewModel
                {
                    Id = r.Id,
                    IssuedOn = r.IssuedOn.ToString("dd/MM/yyyy"),
                    Total = r.ReceiptOrders.Select(ro => ro.Orders.Sum(o => o.Product.Price * o.Quantity)).Sum(),
                    Cashier = r.Cashier.Username
                })
                .ToList();

            var profile = new UserProfileViewModel
            {
                Receipts = receipts
            };

            return View("/receipts/all", profile);
        }

        [HttpPost]
        public IHttpResponse Create()
        {
            var orders = Db.Orders.Where(o => o.Status == Status.Active).ToList();
                
            orders.ForEach(o => o.Status = Status.Completed);
            
            var receipt = new Receipt
            {
                Cashier = Db.Users.Single(c => c.Username.Equals(User.Username)),
                IssuedOn = DateTime.UtcNow
            };

            Db.Orders.UpdateRange(orders);
            Db.SaveChanges();

            Db.Receipts.Add(receipt);
            Db.SaveChanges();

            var receiptOrder = new ReceiptOrder
                {
                    Orders = orders,
                    Receipt = receipt
                };

            Db.ReceiptOrders.Add(receiptOrder);
            Db.SaveChanges();

            var userReceipt = new UserReceipt
            {
                Receipt = receipt,
                Cashier = receipt.Cashier
            };

            Db.UserReceipts.Add(userReceipt);
            Db.SaveChanges();

            var orderViewModels = receiptOrder.Orders
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    Product = o.Product.Name,
                    Quantity = o.Quantity,
                    Price = o.Product.Price
                })
                .ToList();

            var receiptViewModel = new ReceiptDetailsViewModel
            {
                Id = receipt.Id,
                Orders = orderViewModels,
                IssuedOn = receipt.IssuedOn.ToString("dd/MM/yyyy"),
                Cashier = receipt.Cashier.Username
            };

            var total = orderViewModels.Sum(ovm => ovm.Quantity * ovm.Price);

            receiptViewModel.Total = total;

            return View("/receipts/details", receiptViewModel);
        }

        [Authorize]
        public IHttpResponse Details()
        {
            string id = Request.QueryData["id"].ToString();
            var receiptViewModel = Db.Receipts
                .Select(r => new ReceiptDetailsViewModel
                {
                    Id = r.Id,
                    Orders = r.ReceiptOrders.SelectMany(ro => ro.Orders).Select(o => new OrderViewModel
                    {
                        Id = o.Id,
                        Product = o.Product.Name,
                        Quantity = o.Quantity,
                        Price = o.Product.Price,
                        
                    }).ToList(),
                    IssuedOn = r.IssuedOn.ToString("dd/MM/yyyy"),
                    Cashier = r.Cashier.Username
                })
                .Single(r => r.Id.Equals(id));

            receiptViewModel.Total = receiptViewModel.Orders.Sum(o => o.Price * o.Quantity);

            return View(receiptViewModel);
        }
    }
}
