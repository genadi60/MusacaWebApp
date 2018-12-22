namespace MusacaWebApp.ViewModels
{
    using System.Collections.Generic;


    public class AllProductsViewModel
    {
        public IEnumerable<ProductViewModel> ProductViewModels { get; set; } = new List<ProductViewModel>();
    }
}
