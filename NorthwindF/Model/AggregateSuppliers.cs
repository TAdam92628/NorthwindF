namespace NorthwindF.Model
{
    public class AggregateSuppliers
    {
        public int SupplierID { get; set; }
        public int ProductID { get; set; }
        public string? CompanyName { get; set; }
        public string? ProductName { get; set; }
        public int SumPrice { get; set; }
    }
}
