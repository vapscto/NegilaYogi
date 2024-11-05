using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class QRCode_GenerationController : Controller
    {
        QRCode_GenerationDelegate _delegate = new QRCode_GenerationDelegate();

     
        [Route("Getdetails/{id:int}")]
        public QRCode_GenerationDTO Getdetails(int id)
        {
            QRCode_GenerationDTO data = new QRCode_GenerationDTO();
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.Getdetails(data);
        }



        [HttpPost]
        [Route("SaveQR_Code")]
        public QRCode_GenerationDTO SaveQR_Code([FromBody]QRCode_GenerationDTO data)
        {
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.SaveQR_Code(data);
        }
        [HttpPost]
        [Route("STAFFSaveQR_Code")]
        public QRCode_GenerationDTO STAFFSaveQR_Code([FromBody]QRCode_GenerationDTO data)
        {
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.STAFFSaveQR_Code(data);
        }
        [Route("get_classes")]
        public QRCode_GenerationDTO get_classes([FromBody] QRCode_GenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.get_classes(data);
        }
        [Route("get_cls_sections")]
        public QRCode_GenerationDTO get_cls_sections([FromBody] QRCode_GenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.get_cls_sections(data);
        }
        [Route("GetStudents")]
        public QRCode_GenerationDTO GetStudents([FromBody] QRCode_GenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.GetStudents(data);
        }
        [Route("QRReportDetails")]
        public QRCode_GenerationDTO QRReportDetails([FromBody] QRCode_GenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.QRReportDetails(data);
        }
       
        
        [Route("StaffGetdetails")]
        public QRCode_GenerationDTO StaffGetdetails([FromBody] QRCode_GenerationDTO data)
        {           
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.StaffGetdetails(data);
        }


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public QRCode_GenerationDTO getalldetails(int id)
        {
            QRCode_GenerationDTO dto = new QRCode_GenerationDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.onloadgetdetails(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public QRCode_GenerationDTO getEmployeedetailsBySelection([FromBody]QRCode_GenerationDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.getEmployeedetailsBySelection(dto);
        }



        [Route("get_depts")]
        public QRCode_GenerationDTO get_depts([FromBody]QRCode_GenerationDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.get_depts(dto);
        }

        [Route("get_desig")]
        public QRCode_GenerationDTO get_desig([FromBody]QRCode_GenerationDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.get_desig(dto);
        }
        [Route("filterEmployeedetailsBySelection")]
        public QRCode_GenerationDTO filterEmployeedetailsBySelection([FromBody]QRCode_GenerationDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.filterEmployeedetailsBySelection(dto);
        }
        [Route("QRcodegeneration")]
        public QRCode_GenerationDTO QRcodegeneration([FromBody]QRCode_GenerationDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.QRcodegeneration(dto);
        }

        [Route("StudentQRCode")]
        public QRCode_GenerationDTO StudentQRCode([FromBody]QRCode_GenerationDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.StudentQRCode(dto);
        }



    }
}
