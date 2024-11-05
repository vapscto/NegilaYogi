using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Student;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.Student
{
    public class CollegeStudent_TTDelegate
    {    
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegeStudent_TTDTO, CollegeStudent_TTDTO> COMMM = new CommonDelegate<CollegeStudent_TTDTO, CollegeStudent_TTDTO>();
        public CollegeStudent_TTDTO getloaddata(CollegeStudent_TTDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "CollegeStudent_TTFacade/getloaddata/");
        }
        public CollegeStudent_TTDTO getStudentTT(CollegeStudent_TTDTO sddto)
        {
            return COMMM.CLGPortalPOSTData(sddto, "CollegeStudent_TTFacade/getStudentTT/");
        }
    }
}
