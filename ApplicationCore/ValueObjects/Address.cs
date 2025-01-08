namespace ApplicationCore.ValueObjects
{
    public class Address : ValueObject
    {
        public string Street { get; private set; }
        public string City { get; private set; }

        public Address() { }

        public Address(string street, string city)
        {
            Street = street;
            City = city;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street;
            yield return City;
        }
    }
}
