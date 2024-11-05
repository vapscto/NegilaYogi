using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.TT.College
{
    public class CLGTTCommonDelegate
    {
        CommonDelegate<CLGTTCommonDTO, CLGTTCommonDTO> _comm = new CommonDelegate<CLGTTCommonDTO, CLGTTCommonDTO>();
        
      
        public CLGTTCommonDTO getBranch(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/getBranch/");
        }
        public CLGTTCommonDTO getcourse_catg(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/getcourse_catg/");
        }
         public CLGTTCommonDTO getbranch_catg(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/getbranch_catg/");
        }
        public CLGTTCommonDTO multplegetbranch_catg(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/multplegetbranch_catg/");
        }
        public CLGTTCommonDTO get_semister(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_semister/");
        }
        public CLGTTCommonDTO multget_semister(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/multget_semister/");
        }
        public CLGTTCommonDTO get_section(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_section/");
        }
        public CLGTTCommonDTO get_staff(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_staff/");
        }
        public CLGTTCommonDTO get_subject(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_subject/");
        }
        public CLGTTCommonDTO get_subject_onsec(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_subject_onsec/");
        }
        public CLGTTCommonDTO get_semday(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_semday/");
        }
         public CLGTTCommonDTO get_staffaca(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_staffaca/");
        }
        public CLGTTCommonDTO get_course_onstaff(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_course_onstaff/");
        }
        public CLGTTCommonDTO get_branch_onstaff(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_branch_onstaff/");
        }
        public CLGTTCommonDTO get_sem_onstaff(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_sem_onstaff/");
        }
        public CLGTTCommonDTO get_sec_onstaff(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_sec_onstaff/");
        }
        public CLGTTCommonDTO get_subject_onstaff(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_subject_onstaff/");
        }
        public CLGTTCommonDTO get_subjecttab3(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_subjecttab3/");
        }

        public CLGTTCommonDTO get_course_onsubject(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_course_onsubject/");
        }
        public CLGTTCommonDTO get_branch_onsubject(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_branch_onsubject/");
        }
        public CLGTTCommonDTO get_sem_onsubject(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_sem_onsubject/");
        }
        public CLGTTCommonDTO get_sec_onsubject(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_sec_onsubject/");
        }
         public CLGTTCommonDTO get_staff_onsubject(CLGTTCommonDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCommonFacade/get_staff_onsubject/");
        }
        

    }
}
