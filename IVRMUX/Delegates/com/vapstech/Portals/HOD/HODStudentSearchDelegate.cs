using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.HOD
{
    public class HODStudentSearchDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentSearchDTO, StudentSearchDTO> COMMM = new CommonDelegate<StudentSearchDTO, StudentSearchDTO>();


        public StudentSearchDTO getalldetails(StudentSearchDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODStudentSearchFacade/getalldetails/");
        }
        public StudentSearchDTO getstudentdetails(StudentSearchDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODStudentSearchFacade/getstudentdetails/");
        }
    }
}
