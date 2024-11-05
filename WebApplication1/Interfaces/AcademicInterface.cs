using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface AcademicInterface
    {
        AcademicDTO saveProsdet(AcademicDTO pros);

        AcademicDTO deleterec(AcademicDTO dto);

        AcademicDTO getallDetails(AcademicDTO acdto);
        AcademicDTO getdetails(int id);

        AcademicDTO deactivate(AcademicDTO id);
        AcademicDTO searchByColumn(AcademicDTO dto);
        AcademicDTO saveorder(AcademicDTO data);

    }
}
