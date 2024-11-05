using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class NoticeBoardFacade : Controller
    {
        // GET: api/values
        public NoticeBoardInterface _work;
        public NoticeBoardFacade(NoticeBoardInterface work)
        {
            _work = work;
        }
        // GET: api/values
        [Route("savedetail")]
        public IVRM_NoticeBoardDTO savedetail([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.savedetail(data);
        }
        [Route("Getdetails")]
        public IVRM_NoticeBoardDTO Getdetails([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.Getdetails(data);
        }
        [Route("deactivate")]
        public IVRM_NoticeBoardDTO deactivate([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.deactivate(data);
        }
        [Route("editdetails")]
        public IVRM_NoticeBoardDTO editdetails([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.editdetails(data);
        }

         [Route("getsection")]
        public IVRM_NoticeBoardDTO getsection([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.getsection(data);
        }
         [Route("getstudent")]
        public IVRM_NoticeBoardDTO getstudent([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.getstudent(data);
        }
        [Route("Deptselectiondetails")]
        public IVRM_NoticeBoardDTO Deptselectiondetails([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.Deptselectiondetails(data);
        }
         [Route("Desgselectiondetails")]
        public IVRM_NoticeBoardDTO Desgselectiondetails([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.Desgselectiondetails(data);
        }
        [Route("viewData")]
        public IVRM_NoticeBoardDTO viewData([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.viewData(data);
        }
        [Route("viewrecords")]
        public IVRM_NoticeBoardDTO viewrecords([FromBody]IVRM_NoticeBoardDTO data)
        {
            return _work.viewrecords(data);
        }

    }
}
