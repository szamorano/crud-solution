﻿using Entities;
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

            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) return matchingPersons;

            switch (searchBy) 
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.PersonName)? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Email) ? p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(p => (p.DateOfBirth != null) ? p.DateOfBirth.Value.ToString("MMMM dd yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Gender) ? p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Country) ? p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(Person.Address):
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
