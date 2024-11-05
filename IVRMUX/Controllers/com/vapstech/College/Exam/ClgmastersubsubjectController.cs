using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgmastersubsubjectController : Controller
    {
        ClgmastersubsubjectDelegates mastersubsubjectdelStr = new ClgmastersubsubjectDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public mastersubsubjectDTO Getdetails(mastersubsubjectDTO mastersubsubjectDTO)
        {
            mastersubsubjectDTO.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersubsubjectdelStr.GetmastersubsubjectData(mastersubsubjectDTO);
        }

        [Route("savedetails")]
        public mastersubsubjectDTO savedetails([FromBody] mastersubsubjectDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersubsubjectdelStr.savedetails(data);
        }


        [Route("validateordernumber")]
        public mastersubsubjectDTO validateordernumber([FromBody] mastersubsubjectDTO data)
        {
            return mastersubsubjectdelStr.validateordernumber(data);
        }


        [Route("deactivate")]
        public mastersubsubjectDTO deactivate([FromBody] mastersubsubjectDTO data)
        {

            return mastersubsubjectdelStr.deactivate(data);
        }


        [Route("editdeatils/{id:int}")]
        public mastersubsubjectDTO editdeatils(int ID)
        {
            return mastersubsubjectdelStr.editdeatils(ID);
        }
    }
}
