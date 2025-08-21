namespace FlyLib.API.DTOs.v1.Countries.Requests
{
    public class UpdateCountryRequestV1
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public UpdateCountryRequestV1() { }

        public UpdateCountryRequestV1(int countryId, string name, string isoCode)
        {
            CountryId = countryId;
            Name = name;
            IsoCode = isoCode;
        }
    }
}
