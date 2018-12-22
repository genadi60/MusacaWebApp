namespace MusacaWebApp.ViewModels
{
    using System.Collections.Generic;

    public class LoggedInUserViewModel
    {
        public ICollection<OrderViewModel> OrderViewModels { get; set; } = new List<OrderViewModel>();

        public decimal Total { get; set; }
    }
}
