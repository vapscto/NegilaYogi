using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class TaskCreationFromClintFacadeController : Controller
    {

        public TaskCreationFromClintInterface _objinter;

        public TaskCreationFromClintFacadeController(TaskCreationFromClintInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<TaskCreationFromClintDTO> getdetails([FromBody] TaskCreationFromClintDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("savedata")]
        public TaskCreationFromClintDTO savedata([FromBody]TaskCreationFromClintDTO data)
        {
            return _objinter.savedata(data);
        }

        [Route("deactive")]
        public TaskCreationFromClintDTO deactive([FromBody]TaskCreationFromClintDTO data)
        {
            return _objinter.deactive(data);
        }




    }
}
