using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniHub.Com.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

namespace AlumniHub.Com.Facade
{
    
    [Route("api/[controller]")]
    public class Alumni_NoticeBoardFacadeController : Controller
    {
        public Alumni_NoticeBoard_Interface _inter;
        public Alumni_NoticeBoardFacadeController(Alumni_NoticeBoard_Interface inter)
        {
            _inter = inter;
        }
        [Route("loaddata")]
        public Alumni_NoticeBoard_DTO loaddata([FromBody] Alumni_NoticeBoard_DTO dto)
        {
            return _inter.loaddata(dto);
        }
        [Route("savedetail")]
        public Alumni_NoticeBoard_DTO savedetail([FromBody] Alumni_NoticeBoard_DTO dto)
        {
            return _inter.savedetail(dto);
        }
         [Route("viewData")]
        public Alumni_NoticeBoard_DTO viewData([FromBody] Alumni_NoticeBoard_DTO dto)
        {
            return _inter.viewData(dto);
        }
        [Route("deactivate")]
        public Alumni_NoticeBoard_DTO deactivate([FromBody] Alumni_NoticeBoard_DTO dto)
        {
            return _inter.deactivate(dto);
        }
        [Route("editdetails")]
        public Alumni_NoticeBoard_DTO editdetails([FromBody] Alumni_NoticeBoard_DTO dto)
        {
            return _inter.editdetails(dto);
        }


    }
}