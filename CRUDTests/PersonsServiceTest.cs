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

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;


        public PersonsServiceTest()
        {
            _personService = new PersonsService();
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
    }
}
