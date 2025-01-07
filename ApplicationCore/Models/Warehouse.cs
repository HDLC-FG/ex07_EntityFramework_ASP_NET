namespace ApplicationCore.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int PostalCode { get; set; }

        /// <summary>
        /// Cette liste contient les codes accès du warehouse en md5.
        /// </summary>
        public IList<string> CodeAccesMD5 { get; set; } = new List<string>();
        public IList<Order> Orders { get; set; }
    }
}
