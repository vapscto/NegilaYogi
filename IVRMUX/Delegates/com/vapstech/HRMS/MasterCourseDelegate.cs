using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterCourseDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_CourseDTO, HR_Master_CourseDTO> COMMM = new CommonDelegate<HR_Master_CourseDTO, HR_Master_CourseDTO>();

        public HR_Master_CourseDTO onloadgetdetails(HR_Master_CourseDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterCourseFacade/onloadgetdetails");
        }

        public HR_Master_CourseDTO Onchangedetails(HR_Master_CourseDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterCourseFacade/Onchangedetails");
        }


        public HR_Master_CourseDTO savedetails(HR_Master_CourseDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterCourseFacade/");
        }
        public HR_Master_CourseDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterCourseFacade/getRecordById/");
        }
        public HR_Master_CourseDTO deleterec(HR_Master_CourseDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterCourseFacade/deactivateRecordById/");
        }

       

       

    }
}
