namespace MusacaWebApp.Models
{
    using System.Collections.Generic;

    public class ReceiptOrder
    {
        public string Id { get; set; }

        public string ReceiptId { get; set; }
        public virtual Receipt Receipt { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();
    }
}
