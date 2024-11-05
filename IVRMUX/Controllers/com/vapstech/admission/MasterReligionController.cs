using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterReligionController : Controller
    {
        MasterReligionDelegate deleg = new MasterReligionDelegate();

        [HttpGet]
        public MasterReligionDTO getData()
        {
            return deleg.getdetails(1);
        }
        [HttpPost]
        public MasterReligionDTO saveData([FromBody] MasterReligionDTO data)
        {
            return deleg.saveRecord(data);
        }
        [Route("Editdetails/{id:int}")]
        public MasterReligionDTO Edit(int id)
        {
            return deleg.Edit(id);
        }
        [HttpDelete]
        [Route("DeleteRecord/{id:int}")]
        public MasterReligionDTO deletedetails(int id)
        {
            return deleg.deleterec(id);
        }
        [Route("deactivate")]
        public MasterReligionDTO deactivate([FromBody] MasterReligionDTO rel)
        {
            return deleg.deactivate(rel);
        }
        [Route("SearchByColumn")]
        public MasterReligionDTO SearchByColumn([FromBody] MasterReligionDTO rel)
        {
            return deleg.SearchByColumn(rel);
        }
        
    }
}
