
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface HHSReport_5to7Interface
    {
       Task<HHSReport_5to7DTO> Getdetails(HHSReport_5to7DTO data);
        Task<HHSReport_5to7DTO> savedetails(HHSReport_5to7DTO data);
        HHSReport_5to7DTO getclass(HHSReport_5to7DTO data);
        HHSReport_5to7DTO Getsection(HHSReport_5to7DTO data);
        HHSReport_5to7DTO GetAttendence(HHSReport_5to7DTO data);
        HHSReport_5to7DTO Get_primary_savedetails(HHSReport_5to7DTO data);
    }
}