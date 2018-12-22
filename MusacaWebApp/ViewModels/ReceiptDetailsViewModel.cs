namespace MusacaWebApp.ViewModels
{
    using System.Collections.Generic;

    public class ReceiptDetailsViewModel
    {
        public string Id { get; set; }

        public decimal Total { get; set; }

        public string IssuedOn { get; set; }

        public string Cashier { get; set; }

        public virtual IEnumerable<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
       
    }
}
