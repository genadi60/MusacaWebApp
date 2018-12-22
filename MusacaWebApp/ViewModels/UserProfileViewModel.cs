namespace MusacaWebApp.ViewModels
{
    using System.Collections.Generic;


    public class UserProfileViewModel
    {
        public IEnumerable<ReceiptViewModel> Receipts { get; set; } = new List<ReceiptViewModel>();
    }
}
