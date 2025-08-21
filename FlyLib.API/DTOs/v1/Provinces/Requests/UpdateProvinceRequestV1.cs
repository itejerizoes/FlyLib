namespace FlyLib.API.DTOs.v1.Provinces.Requests
{
    public class UpdateProvinceRequestV1
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public UpdateProvinceRequestV1() { }

        public UpdateProvinceRequestV1(int provinceId, string name, int countryId)
        {
            ProvinceId = provinceId;
            Name = name;
            CountryId = countryId;
        }
    }
}
