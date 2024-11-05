using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Principal
{
    public class StudentSearchDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentSearchDTO, StudentSearchDTO> COMMM = new CommonDelegate<StudentSearchDTO, StudentSearchDTO>();


        public StudentSearchDTO getalldetails(StudentSearchDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentSearchFacade/getalldetails/");
        }
        public StudentSearchDTO getstudentdetails(StudentSearchDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentSearchFacade/getstudentdetails/");
        }
        public StudentSearchDTO GetStudentDetails1(StudentSearchDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentSearchFacade/GetStudentDetails1/");
        }
        public StudentSearchDTO showsectionGrid(StudentSearchDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentSearchFacade/showsectionGrid/");
        }
       
    }
}
