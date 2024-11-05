using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterReligionFacade : Controller
    {
        MasterReligionInterface _interface;
        public MasterReligionFacade(MasterReligionInterface inter)
        {
            _interface = inter;
        }

        // GET api/values/5
        [HttpGet]
        public MasterReligionDTO getData(int id)
        {
            return _interface.getdetails();
        }
      
        // POST api/values
        [HttpPost]
        public MasterReligionDTO save([FromBody]MasterReligionDTO data)
        {
            return _interface.saveData(data);
        }
        [Route("Edit/{id:int}")]
        public MasterReligionDTO EditRecord(int id)
        {
            return _interface.Edit(id);
        }
     
        [Route("deleterec/{id:int}")]
        public MasterReligionDTO deletedetails(int id)
        {
            return _interface.deleterec(id);
        }
        [Route("deactivate")]
        public MasterReligionDTO deactivate([FromBody]MasterReligionDTO dto)
        {
            return _interface.deactivate(dto);
        }
        [Route("SearchByColumn")]
        public MasterReligionDTO SearchByColumn([FromBody]MasterReligionDTO dto)
        {
            return _interface.searchByColumn(dto);
        }
        
    }
}
