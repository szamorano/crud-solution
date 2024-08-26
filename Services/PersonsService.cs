using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
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

        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {
                _persons.Add(new Person() 
                { 
                    PersonID = Guid.Parse("0BC94089-6317-4E1C-BC70-21757338A705"),
                    PersonName = "Felicity",
                    Email = "fminty0@mlb.com",
                    DateOfBirth = DateTime.Parse("1997-11-05"),
                    Gender = "Female",
                    Address = "41 Ronald Regan Pass",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("C9751974-B47E-439B-914D-38DCFAF62964") 
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("4AEFDF1E-B3A0-4C52-A0B6-BB0DCD687E3E"),
                    PersonName = "Kimmi",
                    Email = "kschulke1@purevolume.com",
                    DateOfBirth = DateTime.Parse("1995-03-30"),
                    Gender = "Female",
                    Address = "340 Del Sol Circle",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("DF4783B2-57E1-4A27-992D-9BA95DE36A76")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("50936198-E787-40C4-B75D-CE632DC18779"),
                    PersonName = "Gisela",
                    Email = "gnehl2@edublogs.org",
                    DateOfBirth = DateTime.Parse("2000-02-18"),
                    Gender = "Female",
                    Address = "21 Lighthouse Bay Park",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("3297A9F3-06AE-4191-ADAF-313CD19FF0EB")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("DB9B429E-EA89-4067-8EBF-0C4CF6059964"),
                    PersonName = "Nathan",
                    Email = "nzoellner3@paypal.com",
                    DateOfBirth = DateTime.Parse("1993-12-01"),
                    Gender = "Male",
                    Address = "954 Moose Park",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("C9A67099-C44D-46A8-8761-5563FE224E06")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("DD05AC95-88E0-46FD-845F-7C7A1EE15D21"),
                    PersonName = "Madelina",
                    Email = "mmacilhench4@ehow.com",
                    DateOfBirth = DateTime.Parse("1995-05-16"),
                    Gender = "Female",
                    Address = "694 Cambridge Park",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("53075EE2-E93D-482D-AD4A-C814AF21434B")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("92AB4ECE-08AC-4D1B-9FA2-60185E8712DC"),
                    PersonName = "Randolf",
                    Email = "rskipsey5@washington.edu",
                    DateOfBirth = DateTime.Parse("2000-01-04"),
                    Gender = "Male",
                    Address = "337 Daystar Crossing",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("C9751974-B47E-439B-914D-38DCFAF62964")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("B2F4D8AA-CC2B-40A2-99A3-8075A8C4B355"),
                    PersonName = "Rickie",
                    Email = "rcursons6@latimes.com",
                    DateOfBirth = DateTime.Parse("1991-03-30"),
                    Gender = "Male",
                    Address = "70346 Ohio Park",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("DF4783B2-57E1-4A27-992D-9BA95DE36A76")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("50F6F5EB-1214-4ED9-B4AC-E6B4650344B6"),
                    PersonName = "Marcelle",
                    Email = "msowrey7@go.com",
                    DateOfBirth = DateTime.Parse("1992-06-10"),
                    Gender = "Female",
                    Address = "33124 Ohio Circle",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("3297A9F3-06AE-4191-ADAF-313CD19FF0EB")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("12C851FF-9CCB-4AF5-B138-1721BB0B35A7"),
                    PersonName = "Cecile",
                    Email = "cmose8@tuttocitta.it",
                    DateOfBirth = DateTime.Parse("1992-04-21"),
                    Gender = "Female",
                    Address = "796 Di Loreto Drive",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("C9A67099-C44D-46A8-8761-5563FE224E06")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("66F68515-7331-4173-88F1-939CB2B5ADB5"),
                    PersonName = "Bernard",
                    Email = "abernardini9@adobe.com",
                    DateOfBirth = DateTime.Parse("1998-01-28"),
                    Gender = "Male",
                    Address = "5152 Barby Junction",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("53075EE2-E93D-482D-AD4A-C814AF21434B")
                });
            }
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
            return _persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;

            var person = _persons.FirstOrDefault(p => p.PersonID == personID);
            if (person == null) return null;

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) return matchingPersons;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.PersonName) ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Email) ? p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(p => (p.DateOfBirth != null) ? p.DateOfBirth.Value.ToString("MMMM dd yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Gender) ? p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.CountryID):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Country) ? p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Address) ? p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.Ascending) => allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.Descending) => allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.Ascending) => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.Descending) => allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.Ascending) => allPersons.OrderBy(p => p.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.Descending) => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.Ascending) => allPersons.OrderBy(p => p.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.Descending) => allPersons.OrderByDescending(p => p.Age).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.Ascending) => allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.Descending) => allPersons.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.Ascending) => allPersons.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.Descending) => allPersons.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.Ascending) => allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.Descending) => allPersons.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.Ascending) => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.Descending) => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),
                _ => allPersons
            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(Person));

            //validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? matchingPerson = _persons.FirstOrDefault(p => p.PersonID == personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person Id doesn't exist.");
            }

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null) throw new ArgumentNullException(nameof(personID));

            Person? person = _persons.FirstOrDefault(p => p.PersonID == personID);
            if (person == null) return false;

            _persons.RemoveAll(p => p.PersonID == personID);

            return true;
        }
    }
}
