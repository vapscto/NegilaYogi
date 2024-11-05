
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface HHSReport_10thInterface
    {
       Task<HHSReport_10thDTO> Getdetails(HHSReport_10thDTO data);
        Task<HHSReport_10thDTO> savedetails(HHSReport_10thDTO data);
        HHSReport_10thDTO getclass(HHSReport_10thDTO data);
        HHSReport_10thDTO Getsection(HHSReport_10thDTO data);
        HHSReport_10thDTO GetAttendence(HHSReport_10thDTO data);
        //HHSReport_10thDTO GetIndividualAttendence(HHSReport_10thDTO data);

    }
}
