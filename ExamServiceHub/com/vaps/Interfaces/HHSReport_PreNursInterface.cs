
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface HHSReport_PreNursInterface
    {
       Task<HHSReport_PreNursDTO> Getdetails(HHSReport_PreNursDTO data);
        Task<HHSReport_PreNursDTO> savedetails(HHSReport_PreNursDTO data);
        HHSReport_PreNursDTO getclass(HHSReport_PreNursDTO data);
        HHSReport_PreNursDTO Getsection(HHSReport_PreNursDTO data);
        HHSReport_PreNursDTO GetAttendence(HHSReport_PreNursDTO data);
        //HHSReport_PreNursDTO GetIndividualAttendence(HHSReport_PreNursDTO data);

    }
}
