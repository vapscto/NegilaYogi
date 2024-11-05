using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterClassCategoryFacade : Controller
    {
        // GET: api/<controller>

        public MasterClassCategoryInterface _objInter;
        public MasterClassCategoryFacade(MasterClassCategoryInterface para)
        {
            _objInter = para;
        }

        [Route("Savedata")]
        public LIB_Master_ClassCategory_DTO Savedata([FromBody]LIB_Master_ClassCategory_DTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("getdetails")]
        public Task<LIB_Master_ClassCategory_DTO> getdetails([FromBody]LIB_Master_ClassCategory_DTO  id)
        {
            return _objInter.getdetails(id);
        }
        [Route("deactiveY")]
        public LIB_Master_ClassCategory_DTO deactiveY([FromBody]LIB_Master_ClassCategory_DTO data)
        {
            return _objInter.deactiveY(data);
        }

    }
}
