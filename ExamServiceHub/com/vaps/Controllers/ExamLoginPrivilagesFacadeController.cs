
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]

    public class ExamLoginPrivilagesFacadeController : Controller
    {
        public ExamLoginPrivilagesInterface _studentmapping;
        public ExamLoginPrivilagesFacadeController(ExamLoginPrivilagesInterface studentmapping)
        {

            _studentmapping = studentmapping;
        }
       
        [Route("Getdetails")]
        public Exm_Login_PrivilegeDTO Getdetails([FromBody]Exm_Login_PrivilegeDTO data)//int IVRMM_Id
        {
            return _studentmapping.Getdetails(data);
        }
        

       [Route("Studentdetails")]
        public Exm_Login_PrivilegeDTO Studentdetails([FromBody]Exm_Login_PrivilegeDTO data)//int IVRMM_Id
        {
            return _studentmapping.Studentdetails(data);
        }
        [Route("getcategory")]
        public Exm_Login_PrivilegeDTO getcategory([FromBody] Exm_Login_PrivilegeDTO data)
        {
            return _studentmapping.getcategory(data);

        }
        [Route("getclassid")]
        public Exm_Login_PrivilegeDTO getclassid([FromBody] Exm_Login_PrivilegeDTO data)
        {
            return _studentmapping.getclassid(data);

        }
        
        [Route("getclstechdetails")]
        public Exm_Login_PrivilegeDTO getclstechdetails([FromBody]Exm_Login_PrivilegeDTO data)
        {
            return _studentmapping.getclstechdetails(data);

        }

        [HttpPost]
        [Route("editdetails")]
        public Exm_Login_PrivilegeDTO editdetails([FromBody]Exm_Login_PrivilegeDTO data)
        {
            return _studentmapping.editdetails(data);
        }


        [HttpPost]
        [Route("getalldetailsviewrecords")]
        public Exm_Login_PrivilegeDTO getalldetailsviewrecords([FromBody]Exm_Login_PrivilegeDTO data)
        {
            return _studentmapping.getalldetailsviewrecords(data);
        }
        

       [Route("savedetails")]
        public Exm_Login_PrivilegeDTO savedetails([FromBody] Exm_Login_PrivilegeDTO data)
        {
            return _studentmapping.savedetails(data);
        }

       
        [Route("deactivate")]
        public Exm_Login_PrivilegeDTO deactivate([FromBody] Exm_Login_PrivilegeDTO data)
        {
           return _studentmapping.deactivate(data);
        }
        [Route("OnAcdyear")]
        public Exm_Login_PrivilegeDTO OnAcdyear([FromBody] Exm_Login_PrivilegeDTO data)
        {
            return _studentmapping.OnAcdyear(data);
        }
        



    }
}
