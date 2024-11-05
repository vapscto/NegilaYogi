using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CategoryClassMappInterface
    {
        TT_Category_Class_DTO saveProsdet(TT_Category_Class_DTO pros);

        TT_Category_Class_DTO deleterec(TT_Category_Class_DTO dto);

        TT_Category_Class_DTO getallDetails(TT_Category_Class_DTO acdto);

        TT_Category_Class_DTO getdetails(TT_Category_Class_DTO id);

        //AcademicDTO deactivate(AcademicDTO id);
        //AcademicDTO searchByColumn(AcademicDTO dto);

    }
}
