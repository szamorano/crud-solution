using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person name can't be empty.")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be empty.")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }


        public Person ToPerson()
        {
            return new Person() 
            { 
                PersonName = PersonName, 
                Email = Email, 
                DateOfBirth = DateOfBirth, 
                Gender = Gender.ToString(), 
                Address = Address, 
                CountryID = CountryID,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
