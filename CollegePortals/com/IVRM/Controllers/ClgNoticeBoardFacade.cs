using CollegePortals.com.Student.Interfaces;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;
using System.Threading.Tasks;

namespace CollegePortals.com.Student.Controllers
{
    [Route("api/[controller]")]
    public class ClgNoticeBoardFacade : Controller
    {
        public ClgNoticeBoardInterface _ads;

        public ClgNoticeBoardFacade(ClgNoticeBoardInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public ClgNoticeBoardDTO getloaddata([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("getbranchdata")]
        public ClgNoticeBoardDTO getbranchdata([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.getbranchdata(data);
        }
        [HttpPost]
        [Route("getsemdata")]
        public ClgNoticeBoardDTO getsemdata([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.getsemdata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public ClgNoticeBoardDTO savedata([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.savedata(data);
        }
        [HttpPost]
        [Route("getmultiuploadedfile")]
        public ClgNoticeBoardDTO getmultiuploadedfile([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.getmultiuploadedfile(data);
        }
        [HttpPost]
        [Route("getNoticedata")]
        public ClgNoticeBoardDTO getNoticedata([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.getNoticedata(data);
        }
        [HttpPost]
        [Route("editdetails")]
        public ClgNoticeBoardDTO editdetails([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.editdetails(data);
        }
        [HttpPost]
        [Route("deactive")]
        public ClgNoticeBoardDTO deactive([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.deactive(data);
        }
        [HttpPost]
        [Route("deactivedetails")]
        public ClgNoticeBoardDTO deactivedetails([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.deactivedetails(data);
        }

        [Route("Getdata_class")]
        public ClgNoticeBoardDTO Getdata_class([FromBody] ClgNoticeBoardDTO dto)
        {
            return _ads.Getdata_class(dto);
        }
       
        [Route("getreportnotice")]
        public ClgNoticeBoardDTO getreportnotice([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.getreportnotice(data);
        }

        [Route("Getdataview")]
        public ClgNoticeBoardDTO Getdataview([FromBody] ClgNoticeBoardDTO dto)
        {
            return _ads.Getdataview(dto);
        }

        //added by roopa
        //[Route("Deptselectiondetails")]
        //public ClgNoticeBoardDTO Deptselectiondetails([FromBody]ClgNoticeBoardDTO data)
        //{
        //    return _ads.Deptselectiondetails(data);
        //}
        //[Route("Desgselectiondetails")]
        //public ClgNoticeBoardDTO Desgselectiondetails([FromBody]ClgNoticeBoardDTO data)
        //{
        //    return _ads.Desgselectiondetails(data);
        //}

        [Route("getstudent")]
        public ClgNoticeBoardDTO getstudent([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.getstudent(data);
        }

        //course
        [HttpPost]
        [Route("getcoursedata")]
        public ClgNoticeBoardDTO getcoursedata([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.getcoursedata(data);
        }

        //Akash
        [Route("Deptselectiondetails")]
        public ClgNoticeBoardDTO Deptselectiondetails([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.Deptselectiondetails(data);
        }

        [Route("Desgselectiondetails")]
        public ClgNoticeBoardDTO Desgselectiondetails([FromBody]ClgNoticeBoardDTO data)
        {
            return _ads.Desgselectiondetails(data);
        }
    }
}
