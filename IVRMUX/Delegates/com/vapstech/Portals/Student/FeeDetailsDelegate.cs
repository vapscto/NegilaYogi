using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class FeeDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentDashboardDTO, StudentDashboardDTO> COMMM = new CommonDelegate<StudentDashboardDTO, StudentDashboardDTO>();
        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
        {     
            return COMMM.POSTPORTALData(data, "FeeDetailsFacade/getloaddata/");
        }
        public StudentDashboardDTO Getdetails(StudentDashboardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "FeeDetailsFacade/Getdetails/");
        }
    }
}
