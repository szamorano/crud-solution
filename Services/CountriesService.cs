using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>() 
                {
                    new Country() { CountryID = Guid.Parse("C9751974-B47E-439B-914D-38DCFAF62964"), CountryName = "USA" },
                    new Country() { CountryID = Guid.Parse("DF4783B2-57E1-4A27-992D-9BA95DE36A76"), CountryName = "Canada" },
                    new Country() { CountryID = Guid.Parse("3297A9F3-06AE-4191-ADAF-313CD19FF0EB"), CountryName = "UK" },
                    new Country() { CountryID = Guid.Parse("C9A67099-C44D-46A8-8761-5563FE224E06"), CountryName = "India" },
                    new Country() { CountryID = Guid.Parse("53075EE2-E93D-482D-AD4A-C814AF21434B"), CountryName = "Australia" },
                });
            }
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (_countries.Where(country => country.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Country name already exists.");
            }

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }

            Country? country_response_from_list = _countries.FirstOrDefault(country => country.CountryID == countryID);

            if (country_response_from_list == null)
            {
                return null;
            }

            return country_response_from_list.ToCountryResponse();

        }
    }
}
