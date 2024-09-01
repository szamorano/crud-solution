﻿using ServiceContracts;
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
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;


        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            var dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);
            
            _countriesService = new CountriesService(null);
            _personService = new PersonsService(dbContext, _countriesService);
            _testOutputHelper = testOutputHelper;
        }



        #region AddPerson


        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
            Func<Task> action = async() =>
            {
                await _personService.AddPerson(personAddRequest);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
            //await Assert.ThrowsAsync<ArgumentNullException>;
        }


        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();
            //PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _personService.AddPerson(personAddRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
            //await Assert.ThrowsAsync<ArgumentException>(
        }


        [Fact]
        public async Task AddPerson_PersonProperDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "user@example.com").Create();

            //PersonAddRequest? personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Person name...",
            //    Email = "person@example.com",
            //    Address = "sample address",
            //    CountryID = Guid.NewGuid(),
            //    Gender = GenderOptions.Male,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    ReceiveNewsLetters = true
            //};

            //Act
            PersonResponse person_response_from_add = await _personService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = await _personService.GetAllPersons();

            //Assert
            //Assert.True(person_response_from_add.PersonID != Guid.Empty);
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);

            //Assert.Contains(person_response_from_add, persons_list);
            persons_list.Should().Contain(person_response_from_add);
        }




        #endregion



        #region GetPersonByPersonID


        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(personID);

            //Assert
            //Assert.Null(person_response_from_get);
            person_response_from_get.Should().BeNull();
        }


        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            //CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };

            CountryResponse country_response = await _countriesService.AddCountry(country_request);

            //Act
            PersonAddRequest person_request = _fixture.Build<PersonAddRequest>().With(temp => temp.Email,"user@example.com").Create();
           
            PersonResponse person_response_from_add = await _personService.AddPerson(person_request);

            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_add.PersonID);

            //Assert
            //Assert.Equal(person_response_from_add, person_response_from_get);
            person_response_from_get.Should().Be(person_response_from_add);
        }


        #endregion



        #region GetAllPersons


        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = await _personService.GetAllPersons();

            //Assert
            //Assert.Empty(persons_from_get);
            persons_from_get.Should().BeEmpty();
        }


        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request2 = _fixture.Create<CountryAddRequest>();

            var country_response1 = await _countriesService.AddCountry(country_request1);
            var country_response2 = await _countriesService.AddCountry(country_request2);

            PersonAddRequest person_request1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "user1@example.com").Create();
       
            PersonAddRequest person_request2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "user2@example.com").Create();

            PersonAddRequest person_request3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "user3@example.com").Create();

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request1,
                person_request2,
                person_request3
            };

            foreach (PersonAddRequest request in person_requests)
            {
                var person_response = await _personService.AddPerson(request);
                person_response_list_from_add.Add(person_response);
            }


            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_get = await _personService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_get);
            //}

            persons_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
        }


        #endregion


        #region GetFilteredPersons


        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request2 = _fixture.Create<CountryAddRequest>();

            var country_response1 = await _countriesService.AddCountry(country_request1);
            var country_response2 = await _countriesService.AddCountry(country_request2);

            PersonAddRequest person_request1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "user1@example.com").Create();

            PersonAddRequest person_request2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "user2@example.com").Create();

            PersonAddRequest person_request3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "user3@example.com").Create();

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request1,
                person_request2,
                person_request3
            };

            foreach (PersonAddRequest request in person_requests)
            {
                var person_response = await _personService.AddPerson(request);
                person_response_list_from_add.Add(person_response);
            }


            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_search);
            //}

            persons_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);
        }



        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request2 = _fixture.Create<CountryAddRequest>();

            var country_response1 = await _countriesService.AddCountry(country_request1);
            var country_response2 = await _countriesService.AddCountry(country_request2);

            PersonAddRequest person_request1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "user1@example.com")
                .With(temp => temp.CountryID, country_response1.CountryID)
                .Create();

            PersonAddRequest person_request2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Mary")
                .With(temp => temp.Email, "user2@example.com")
                .With(temp => temp.CountryID, country_response1.CountryID)
                .Create();

            PersonAddRequest person_request3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Scott")
                .With(temp => temp.Email, "user3@example.com")
                .With(temp => temp.CountryID, country_response2.CountryID)
                .Create();

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request1,
                person_request2,
                person_request3
            };

            foreach (PersonAddRequest request in person_requests)
            {
                var person_response = await _personService.AddPerson(request);
                person_response_list_from_add.Add(person_response);
            }


            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    if (person_response_from_add.PersonName != null)
            //    {
            //        if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            //        {
            //            Assert.Contains(person_response_from_add, persons_list_from_search);
            //        }
            //    }
            //}

            persons_list_from_search.Should().OnlyContain(temp => temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }

        #endregion





        #region GetSortedPersons


        [Fact]
        public async Task GetSortedPersons()
        {
            //Arrange
            CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request2 = _fixture.Create<CountryAddRequest>();

            var country_response1 = await _countriesService.AddCountry(country_request1);
            var country_response2 = await _countriesService.AddCountry(country_request2);

            PersonAddRequest person_request1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "user1@example.com")
                .With(temp => temp.CountryID, country_response1.CountryID)
                .Create();

            PersonAddRequest person_request2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Mary")
                .With(temp => temp.Email, "user2@example.com")
                .With(temp => temp.CountryID, country_response1.CountryID)
                .Create();

            PersonAddRequest person_request3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Scott")
                .With(temp => temp.Email, "user3@example.com")
                .With(temp => temp.CountryID, country_response2.CountryID)
                .Create();

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request1,
                person_request2,
                person_request3
            };

            foreach (PersonAddRequest request in person_requests)
            {
                var person_response = await _personService.AddPerson(request);
                person_response_list_from_add.Add(person_response);
            }


            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List<PersonResponse> allPersons = await _personService.GetAllPersons();

            //Act
            List<PersonResponse> persons_list_from_sort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.Descending);

            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //person_response_list_from_add = person_response_list_from_add.OrderByDescending(p => p.PersonName).ToList();

            //Assert
            //for (int i = 0; i < person_response_list_from_add.Count; i++)
            //{
            //    Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            //}

            //persons_list_from_sort.Should().BeEquivalentTo(person_response_list_from_add);

            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }


        #endregion




        #region UpdatePerson


        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Act
            Func<Task> action = async () =>
            {
                //Act
                await _personService.UpdatePerson(person_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();    
        }


        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = _fixture.Build<PersonUpdateRequest>().Create();
            //PersonUpdateRequest? person_update_request = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };


            //Act
            Func<Task> action = async () =>
            {
                //Act
                await _personService.UpdatePerson(person_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();

            var country_response1 = await _countriesService.AddCountry(country_request1);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "user1@example.com")
                .With(temp => temp.CountryID, country_response1.CountryID)
                .Create();

            
            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            //Act
            Func<Task> action = async () =>
            {
                //Act
                await _personService.UpdatePerson(person_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }



        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdate()
        {
            //Arrange
            CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();

            var country_response1 = await _countriesService.AddCountry(country_request1);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "user1@example.com")
                .With(temp => temp.CountryID, country_response1.CountryID)
                .Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = await _personService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_update.PersonID);

            //Assert
            //Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().Be(person_response_from_get);
        }




        #endregion





        #region DeletePerson


        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();

            var country_response1 = await _countriesService.AddCountry(country_request1);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "user1@example.com")
                .With(temp => temp.CountryID, country_response1.CountryID)
                .Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personService.DeletePerson(person_response_from_add.PersonID);

            //Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }



        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "USA" };
            var country_response_from_add = await _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "Jones",
                Email = "user@example.com",
                Gender = GenderOptions.Male,
                Address = "jones address",
                CountryID = country_response_from_add.CountryID,
                DateOfBirth = DateTime.Parse("2002-01-01"),
                ReceiveNewsLetters = true
            };

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            //Assert
            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }



        #endregion
    }
}
