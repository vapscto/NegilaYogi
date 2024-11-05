using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface School_M_ClassInterface
    {
        School_M_ClassDTO saveSchool_M_Class(School_M_ClassDTO M_ClassDTO);
        School_M_ClassDTO AllDropdownList(School_M_ClassDTO M_Class);
        // School_M_ClassDTO deleterec(int id);
        School_M_ClassDTO deleterec(School_M_ClassDTO id);
        School_M_ClassDTO getdetails(int id);
        School_M_ClassDTO deletedetails(School_M_ClassDTO id);
        School_M_ClassDTO searchByColumn(School_M_ClassDTO search);
    }
}
