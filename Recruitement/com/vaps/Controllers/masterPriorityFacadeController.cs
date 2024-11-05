using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class masterPriorityFacadeController : Controller
    {
        public masterPriorityInterface _ads;

        public masterPriorityFacadeController(masterPriorityInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Master_PriorityDTO getinitialdata([FromBody]HR_Master_PriorityDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Master_PriorityDTO Post([FromBody]HR_Master_PriorityDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }
        [Route("getdata")]
        public HR_Master_PriorityDTO getdata([FromBody]HR_Master_PriorityDTO dto)
        {
            return _ads.getdata(dto);
        }
        [Route("getRecordById/{id:int}")]

        public HR_Master_PriorityDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Master_PriorityDTO deactivateRecordById([FromBody]HR_Master_PriorityDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
