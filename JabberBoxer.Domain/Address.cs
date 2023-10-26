namespace JabberBoxer.Domain
{
    public sealed class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string FormattedAddress { get; set; }
        public static Address Create()
        {
            return new Address
            {
                Street = "8551 Rainbow Ave",
                City = "Thornton",
                State = "CO",
                Zip = 80229
            };
        }

    }
}
