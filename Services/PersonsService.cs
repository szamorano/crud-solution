using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null) throw new ArgumentNullException(nameof(PersonAddRequest));

            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            _persons.Add(person);

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;

            var person = _persons.FirstOrDefault(p => p.PersonID == personID);
            if (person == null) return null;

            return person.ToPersonResponse();
        }
    }
}
