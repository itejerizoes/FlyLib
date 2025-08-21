namespace FlyLib.API.DTOs.v1.Countries.Requests
{
    public class CreateCountryRequestV1
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public CreateCountryRequestV1() { }

        public CreateCountryRequestV1(string name, string isoCode)
        {
            Name = name;
            IsoCode = isoCode;
        }
    }
}
