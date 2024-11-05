using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface BifurcationInterface
    {
        TT_Bifurcation_DTO saveProsdet(TT_Bifurcation_DTO pros);

        TT_Bifurcation_DTO getClassdetails(TT_Bifurcation_DTO pros);

        TT_Bifurcation_DTO deleterec(TT_Bifurcation_DTO dto);

        TT_Bifurcation_DTO getallDetails(TT_Bifurcation_DTO acdto);

        TT_Bifurcation_DTO getdetails(TT_Bifurcation_DTO acdm);

        TT_Bifurcation_DTO getalldetailsviewrecords(TT_Bifurcation_DTO acdm);

        //AcademicDTO deactivate(AcademicDTO id);
        //AcademicDTO searchByColumn(AcademicDTO dto);

    }
}
