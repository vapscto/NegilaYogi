using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using IVRMUX.Delegates.com.vapstech.College.Preadmission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Preadmission
{
    [Route("api/[controller]")]
    public class TotalCountClgController : Controller
    {
        TotalCountClgDelegate TotalCountReportClgDelegates = new TotalCountClgDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("get_intial_data/{id:int}")]
        public CollegePreadmissionstudnetDto get_intial_data(int id)
        {
            CollegePreadmissionstudnetDto data = new CollegePreadmissionstudnetDto();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return TotalCountReportClgDelegates.get_intial_data(data);
        }

        [HttpPost]
        [Route("Getdetails")]
        public CollegePreadmissionstudnetDto Getdetails([FromBody] CollegePreadmissionstudnetDto MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //MMD.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return TotalCountReportClgDelegates.Getdetails(MMD);
        }



        //preadmission status
        [Route("getstatusdata")]
        public CommonDTO getstatusdata()
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //return sad.getInitailData(mi_id);
            CommonDelegate<CommonDTO, CommonDTO> sad1 = new CommonDelegate<CommonDTO, CommonDTO>();
            var aa = sad1.CollegeGetDataById(mi_id, "TotalCountClgReportFacade/getstatusdata/");
            CommonDTO cdto = (CommonDTO)aa;
            return cdto;
        }


        [Route("SearchData")]
        public CommonDTO getStudentOnSearchFilter([FromBody] CommonDTO cdto)
        {
            //return sad.getStudentOnSearchFilter(cdto);
            CommonDelegate<CommonDTO, CommonDTO> sad1 = new CommonDelegate<CommonDTO, CommonDTO>();
            cdto.IVRM_MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            var aa = sad1.CollegePOSTData(cdto, "TotalCountClgReportFacade/getdataonsearchfilter/");
            CommonDTO stu = (CommonDTO)aa;
            return stu;
        }
        [Route("saveData")]
        public CommonDTO saveData([FromBody] CommonDTO studentdata)
        {
            //return sad.saveData(studentdata);

            studentdata.mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            studentdata.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            CommonDelegate<CommonDTO, CommonDTO> sad1 = new CommonDelegate<CommonDTO, CommonDTO>();
            var aa = sad1.CollegePOSTData(studentdata, "TotalCountClgReportFacade/savedata/");
            CommonDTO stu = (CommonDTO)aa;
            return stu;
        }

        [Route("Clgapplicationstudocs")]
        public CollegePreadmissionstudnetDto Clgapplicationstudocs([FromBody] CollegePreadmissionstudnetDto data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
         
            return TotalCountReportClgDelegates.Clgapplicationstudocs(data);
        }
        [Route("Clgapplicationsturemarks")]
        public CollegePreadmissionstudnetDto Clgapplicationsturemarks([FromBody] CollegePreadmissionstudnetDto data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return TotalCountReportClgDelegates.Clgapplicationsturemarks(data);
        }
    }
}
