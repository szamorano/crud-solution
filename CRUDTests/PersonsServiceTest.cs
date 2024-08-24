using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ServiceContracts.Enums;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;


        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }



        #region AddPerson


        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }


        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }


        [Fact]
        public void AddPerson_PersonProperDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Person name...", Email = "person@example.com", Address = "sample address",
            CountryID = Guid.NewGuid(), Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("2000-01-01"), ReceiveNewsLetters = true };

            //Act
            PersonResponse person_response_from_add =  _personService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personService.GetAllPersons();

            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);    
        }




        #endregion



        #region GetPersonByPersonID


        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(person_response_from_get);
        }


        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse country_response = _countriesService.AddCountry(country_request);

            //Act
            PersonAddRequest person_request = new PersonAddRequest() { PersonName = "person name...", Email = "email@example.com",
                Address = "address", CountryID = country_response.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"), Gender = GenderOptions.Male, ReceiveNewsLetters = false };

            PersonResponse person_response_from_add = _personService.AddPerson(person_request);

            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.PersonID);
            
            //Assert
            Assert.Equal(person_response_from_add, person_response_from_get);
        }


        #endregion



        #region GetAllPersons


        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();

            //Assert
            Assert.Empty(persons_from_get);
        }


        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };

            var country_response1 = _countriesService.AddCountry(country_request1);
            var country_response2 = _countriesService.AddCountry(country_request2);

            PersonAddRequest person_request1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "user@example.com",
                Gender = GenderOptions.Male,
                Address = "smith address",
                CountryID = country_response1.CountryID,
                DateOfBirth = DateTime.Parse("2002-01-01"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Gender = GenderOptions.Female,
                Address = "mary address",
                CountryID = country_response2.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_request3 = new PersonAddRequest()
            {
                PersonName = "Steve",
                Email = "steve@example.com",
                Gender = GenderOptions.Male,
                Address = "steve address",
                CountryID = country_response1.CountryID,
                DateOfBirth = DateTime.Parse("2002-07-01"),
                ReceiveNewsLetters = true
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            { 
                person_request1,
                person_request2,
                person_request3
            };

            foreach (PersonAddRequest request in person_requests)
            {
                var person_response = _personService.AddPerson(request);
                person_response_list_from_add.Add(person_response);
            }


            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_get = _personService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }
        }


        #endregion
    }
}
