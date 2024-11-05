using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class MasterCourseDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterCourseDTO, MasterCourseDTO> COMMM = new CommonDelegate<MasterCourseDTO, MasterCourseDTO>();

       
        public MasterCourseDTO Savedetails(MasterCourseDTO dt)
        {
            return COMMM.clgadmissionbypost(dt, "MasterCourseFacade/Savedetails");
        }
        public MasterCourseDTO getalldetails(int id)
        {
            return COMMM.clgadmissionbyid(id, "MasterCourseFacade/getalldetails/");
        }
        public MasterCourseDTO Deletedetails(MasterCourseDTO id)
        {
            return COMMM.clgadmissionbypost(id, "MasterCourseFacade/Deletedetails");
        }
        public MasterCourseDTO getOrder(MasterCourseDTO id)
        {
            return COMMM.clgadmissionbypost(id, "MasterCourseFacade/getOrder");
        }
        public MasterCourseDTO EditData(MasterCourseDTO id)
        {
            return COMMM.clgadmissionbypost(id, "MasterCourseFacade/EditData");
        }
        
    }
}
