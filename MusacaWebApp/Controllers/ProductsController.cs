namespace MusacaWebApp.Controllers
{
    using System.Linq;

    using InputModels;
    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels;

    public class ProductsController : BaseController
    {
        public IHttpResponse Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(nameof(Role.Admin))]
        public IHttpResponse Create(ProductInputModel model)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Picture = model.Picture,
                Barcode = model.Barcode
            };

            Db.Products.Add(product);
            Db.SaveChanges();

            return Redirect("/products/all");
        }

        [Authorize]
        public IHttpResponse All()
        {
            var productsViewModel = Db.Products
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Barcode = p.Barcode,
                    Picture = p.Picture
                })
                .ToList();
            var allProducts = new AllProductsViewModel
            {
                ProductViewModels = productsViewModel
            };
            return View(allProducts);
        }
    }
}