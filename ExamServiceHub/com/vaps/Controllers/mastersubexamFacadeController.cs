
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

    public class mastersubexamFacadeController : Controller
    {
        public mastersubexamInterface _mastersubexam;
        public mastersubexamFacadeController(mastersubexamInterface mastersubexam)
        {
            _mastersubexam = mastersubexam;
        }

        [Route("Getdetails")]
        public mastersubexamDTO Getdetails([FromBody]mastersubexamDTO data)//int IVRMM_Id
        {
           
            return _mastersubexam.Getdetails(data);
           
        }

        [Route("editdeatils/{id:int}")]
        public mastersubexamDTO editdeatils(int ID)
        {
            return _mastersubexam.editdeatils(ID);
        }
        
        [Route("savedetails")]
        public mastersubexamDTO savedetails([FromBody] mastersubexamDTO data)
        {
            return _mastersubexam.savedetails(data);
        }
        [Route("validateordernumber")]
        public mastersubexamDTO validateordernumber([FromBody] mastersubexamDTO data)
        {
            return _mastersubexam.validateordernumber(data);
        }


        [Route("deactivate")]
        public mastersubexamDTO deactivate([FromBody] mastersubexamDTO data)
        {
           return _mastersubexam.deactivate(data);
        }

     
       

    }
}
