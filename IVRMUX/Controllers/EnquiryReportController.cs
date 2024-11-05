using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]

    public class EnquiryReportController : Controller
    {

        // GET: /<controller>/
        enquiryreportDelegates enquiryreportDelegates = new enquiryreportDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


       



       


        //[Route("Getdetails/{id:int}")]
        //public WrittenTestMarksBindDataDTO GetdetailsOLD(int ID)
        //{
        //    return TotalCountReportDelegates.GetData(ID);
        //}

        [HttpPost]
        [Route("searchdata/")]
        public WrittenTestMarksBindDataDTO Getdetails([FromBody] WrittenTestMarksBindDataDTO MMD)
        {

            
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            return enquiryreportDelegates.GetData(MMD);
        }

        [Route("searchenquiry")]
        public WrittenTestMarksBindDataDTO searchenquiry([FromBody] WrittenTestMarksBindDataDTO dto)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.MI_Id = mid;

            return enquiryreportDelegates.searchenquiry(dto);
        }



        [Route("getdetails/{id:int}")]
        public WrittenTestMarksBindDataDTO getdetails(int id)
        {


            WrittenTestMarksBindDataDTO data = new WrittenTestMarksBindDataDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return enquiryreportDelegates.getdetails(data);
        }

        [Route("SendSms/")]
        public string SendSms([FromBody] WrittenTestMarksBindDataDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return enquiryreportDelegates.SendSms(MMD);
        }

        [Route("SendMail/")]
        public string SendMail([FromBody] WrittenTestMarksBindDataDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return enquiryreportDelegates.SendMail(MMD);
        }

        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            return enquiryreportDelegates.ExportToExcle(MMD);
        }

        //[Route("getEnquirySearchedDetails")]
        //public Enq SearchedDetails([FromBody] SortingPagingInfoDTO Ins)
        //{
        //    Ins.PageSize = 5;
        //    return enquiryreportDelegates.getenqSearchedDetails(Ins);
        //}
    }
}
