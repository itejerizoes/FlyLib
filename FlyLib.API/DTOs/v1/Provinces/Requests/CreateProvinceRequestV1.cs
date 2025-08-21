namespace FlyLib.API.DTOs.v1.Provinces.Requests
{
    public class CreateProvinceRequestV1
    {
        public string Name { get; set; }
        public int CountryId { get; set; }

        public CreateProvinceRequestV1() { }

        public CreateProvinceRequestV1(string name, int countryId)
        {
            Name = name;
            CountryId = countryId;
        }
    }
}
