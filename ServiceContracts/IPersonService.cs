using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest personAddRequest);

        List<PersonResponse> GetAllPersons();
    }
}
