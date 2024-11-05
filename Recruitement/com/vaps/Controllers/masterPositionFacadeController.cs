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
    public class masterPositionFacadeController : Controller
    {
        public masterPositionInterface _ads;

        public masterPositionFacadeController(masterPositionInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Master_PositionDTO getinitialdata([FromBody]HR_Master_PositionDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Master_PositionDTO Post([FromBody]HR_Master_PositionDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }
        [Route("getdata")]
        public HR_Master_PositionDTO getdata([FromBody]HR_Master_PositionDTO dto)
        {
            return _ads.getdata(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Master_PositionDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Master_PositionDTO deactivateRecordById([FromBody]HR_Master_PositionDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
