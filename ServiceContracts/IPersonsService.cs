using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        List<PersonResponse> GetAllPersons();

        PersonResponse? GetPersonByPersonID(Guid? personID);

        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
    }
}
