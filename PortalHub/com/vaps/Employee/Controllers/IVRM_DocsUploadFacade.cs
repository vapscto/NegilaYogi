using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_DocsUploadFacade : Controller
    {
        // GET: api/values
        public IVRM_DocsUploadInterface _docs;
        public IVRM_DocsUploadFacade(IVRM_DocsUploadInterface docs)
        {
            _docs = docs;
        }
        // GET: api/values
       
        [Route("Getdetails")]
        public IVRM_DocsUploadDTO Getdetails([FromBody]IVRM_DocsUploadDTO data)
        {
            return _docs.Getdetails(data);
        }

        [Route("savedetail")]
        public IVRM_DocsUploadDTO savedetail([FromBody]IVRM_DocsUploadDTO data)
        {
            return _docs.savedetail(data);
        }

        [Route("get_classes")]
        public Task<IVRM_DocsUploadDTO> get_classes([FromBody]IVRM_DocsUploadDTO data)
        {
            return _docs.get_classes(data);
        }

        [Route("getsectiondata")]
        public IVRM_DocsUploadDTO getsectiondata([FromBody]IVRM_DocsUploadDTO data)
        {
            return _docs.getsectiondata(data);
        }


        [Route("editData")]
        public IVRM_DocsUploadDTO editData([FromBody]IVRM_DocsUploadDTO data)
        {
            return _docs.editData(data);
        }

        [Route("deactivate")]
        public IVRM_DocsUploadDTO deactivate([FromBody]IVRM_DocsUploadDTO data)
        {
            return _docs.deactivate(data);
        }

    }
}
