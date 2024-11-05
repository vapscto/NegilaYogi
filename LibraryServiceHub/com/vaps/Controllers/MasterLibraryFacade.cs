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
    public class MasterLibraryFacade : Controller
    {
        // GET: api/<controller>

        public MasterLibraryInterface _objInter;
        public MasterLibraryFacade(MasterLibraryInterface para)
        {
            _objInter = para;
        }

        [Route("Savedata")]
        public LIB_Master_Library_DTO Savedata([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.Savedata(data);
        }

        [Route("getdetails")]
        public Task<LIB_Master_Library_DTO> getdetails([FromBody]LIB_Master_Library_DTO  id)
        {
            return _objInter.getdetails(id);
        }

        [Route("deactiveY")]
        public LIB_Master_Library_DTO deactiveY([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.deactiveY(data);
        }

        [Route("saveclassdata")]
        public LIB_Master_Library_DTO saveclassdata([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.saveclassdata(data);
        }

        [Route("deactiveYstf")]
        public LIB_Master_Library_DTO deactiveYstf([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.deactiveYstf(data);
        }

        [Route("EditstaffData")]
        public LIB_Master_Library_DTO EditstaffData([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.EditstaffData(data);
        }

        [Route("modalclsslst")]
        public Task<LIB_Master_Library_DTO> modalclsslst([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.modalclsslst(data);
        }

        [Route("deactivclsdata")]
        public LIB_Master_Library_DTO deactivclsdata([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.deactivclsdata(data);
        }

        [Route("getusername")]
        public Task<LIB_Master_Library_DTO> getusername([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.getusername(data);
        }
        [Route("check_userclass")]
        public LIB_Master_Library_DTO check_userclass([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.check_userclass(data);
        }

        [Route("EditclassData")]
        public LIB_Master_Library_DTO EditclassData([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.EditclassData(data);
        }
        [Route("get_MappedClasslist")]
        public LIB_Master_Library_DTO get_MappedClasslist([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.get_MappedClasslist(data);
        }

        [Route("savestaffdata")]
        public LIB_Master_Library_DTO savestaffdata([FromBody]LIB_Master_Library_DTO data)
        {
            return _objInter.savestaffdata(data);
        }

        
    }
}
