using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Produces("application/json")]
    [Route("api/HomeworkUploadFacade")]
    public class HomeworkUploadFacadeController : Controller
    {
        public HomeworkUploadInterface _inter;
        public HomeworkUploadFacadeController(HomeworkUploadInterface inter)
        {
            _inter = inter;
        }

        [Route("Getdata_class")]
        public HomeWorkUploadDTO Getdata_class([FromBody] HomeWorkUploadDTO dto)
        {
            return _inter.Getdata_class(dto);
        }

        [Route("getreport_class")]
        public HomeWorkUploadDTO getreport_class([FromBody] HomeWorkUploadDTO dto)
        {
            return _inter.getreport_class(dto);
        }

         [Route("getreport_home")]
        public HomeWorkUploadDTO getreport_home([FromBody] HomeWorkUploadDTO dto)
        {
            return _inter.getreport_home(dto);
        }
      
        [Route("getreport_notice")]
        public HomeWorkUploadDTO getreport_notice([FromBody] HomeWorkUploadDTO dto)
        {
            return _inter.getreport_notice(dto);
        }

        [Route("Getdataview")]
        public HomeWorkUploadDTO Getdataview([FromBody] HomeWorkUploadDTO dto)
        {
            return _inter.Getdataview(dto);
        }

        [Route("getsection")]
        public HomeWorkUploadDTO getsection([FromBody] HomeWorkUploadDTO dto)
        {
            return _inter.getsection(dto);
        }

        [Route("getseenreport")]
        public HomeWorkUploadDTO getseenreport([FromBody] HomeWorkUploadDTO dto)
        {
            return _inter.getseenreport(dto);
        }
        [Route("Getdataview_seen")]
        public HomeWorkUploadDTO Getdataview_seen([FromBody] HomeWorkUploadDTO dto)
        {
            return _inter.Getdataview_seen(dto);
        }

        [Route("viewData")]
        public HomeWorkUploadDTO viewData([FromBody]HomeWorkUploadDTO data)
        {
            return _inter.viewData(data);
        }
    }
}