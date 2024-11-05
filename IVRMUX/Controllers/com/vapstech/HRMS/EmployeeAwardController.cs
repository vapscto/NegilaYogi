using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class EmployeeAwardController : Controller
    {

        EmployeeAwardDelegate del = new EmployeeAwardDelegate();
     
        [Route("getalldetails/{id:int}")]
        public HR_Employee_Awards_DTO getalldetails(int id)
        {
            HR_Employee_Awards_DTO dto = new HR_Employee_Awards_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return del.getalldetails(dto);
        }

        [Route("get_depchange")]
        public HR_Employee_Awards_DTO get_depchange([FromBody]HR_Employee_Awards_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return del.get_depchange(dto);
        }

        [Route("get_employee")]
        public HR_Employee_Awards_DTO get_employee([FromBody]HR_Employee_Awards_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return del.get_employee(dto);
        }

        [Route("saverecord")]
        public HR_Employee_Awards_DTO saverecord([FromBody]HR_Employee_Awards_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.saverecord(dto);
        }

        [Route("editrecord")]
        public HR_Employee_Awards_DTO editrecord([FromBody]HR_Employee_Awards_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.editrecord(dto);
        }

        [Route("deactive")]
        public HR_Employee_Awards_DTO deactive([FromBody]HR_Employee_Awards_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(dto);
        }
        //[Route("viewuploadflies")]
        //public HR_Employee_Awards_DTO viewuploadflies([FromBody] HR_Employee_Awards_DTO data)
        //{
        //    data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return del.viewuploadflies(data);
        //}
        //[Route("deleteuploadfile")]
        //public HR_Employee_Awards_DTO deleteuploadfile([FromBody] HR_Employee_Awards_DTO data)
        //{
        //    data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return del.deleteuploadfile(data);
        //}

    }
}
