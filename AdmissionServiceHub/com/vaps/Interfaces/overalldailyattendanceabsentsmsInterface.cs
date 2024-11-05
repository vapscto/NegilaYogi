using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface overalldailyattendanceabsentsmsInterface
    {
        Task<OveralldailyattendanceabsentsmsDTO> getInitailData(OveralldailyattendanceabsentsmsDTO id);
        Task<OveralldailyattendanceabsentsmsDTO> getserdata(OveralldailyattendanceabsentsmsDTO data);
        OveralldailyattendanceabsentsmsDTO getstudentDet(OveralldailyattendanceabsentsmsDTO data);
        OveralldailyattendanceabsentsmsDTO sendsms(OveralldailyattendanceabsentsmsDTO data);
        OveralldailyattendanceabsentsmsDTO sendsms_twice(OveralldailyattendanceabsentsmsDTO data);
        
        OveralldailyattendanceabsentsmsDTO sendemail(OveralldailyattendanceabsentsmsDTO data);
        OveralldailyattendanceabsentsmsDTO smartcardatt(OveralldailyattendanceabsentsmsDTO data);
        OveralldailyattendanceabsentsmsDTO createuser(OveralldailyattendanceabsentsmsDTO data);
        
    }
}
