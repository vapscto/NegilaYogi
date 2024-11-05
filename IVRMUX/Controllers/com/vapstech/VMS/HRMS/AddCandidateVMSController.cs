using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.VMS.HRMS
{
    //[ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AddCandidateVMSController : Controller
    {
        AddCandidateVMSDelegate del = new AddCandidateVMSDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Candidate_DetailsDTO getalldetails(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        [Route("getallgrade/{id:int}")]
        public HR_Candidate_DetailsDTO getallgrade(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getallgrade(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public HR_Candidate_DetailsDTO Post([FromBody]HR_Candidate_DetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRCD_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Candidate_DetailsDTO editRecord(int id)
        {
          //  HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
           // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("paynow")]
        public HR_Candidate_DetailsDTO paynow([FromBody] HR_Candidate_DetailsDTO data)
        {
            HR_Candidate_DetailsDTO dt = new HR_Candidate_DetailsDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ID = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;



            return del.paynow(data);
        }
        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            HR_Candidate_DetailsDTO schoolimp = new HR_Candidate_DetailsDTO();
            schoolimp.MI_Id = Convert.ToInt64(response.udf3);
            string querystring = "";
            var sub = "";
            HR_Candidate_DetailsDTO dtoapp = new HR_Candidate_DetailsDTO();
            dtoapp.MI_Id = Convert.ToInt64(response.udf3);
           
            sub = "addCandidate";        

            try
            {
                dto = del.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/ 12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }

            return Redirect(querystring);
        }
        [Route("Razorpaypaymentresponse/")]
        public ActionResult Razorpaypaymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            var sub = "";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();
            dtoapp.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            response.IVRMOP_MIID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string payid = response.razorpay_payment_id;
          
          
                sub = "addCandidate";
            

            try
            {
                dto = del.razorgetpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/ 12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }

            return Redirect(querystring);
        }


        [Route("Get_Desgination")]
        public HR_Candidate_DetailsDTO Get_Desgination([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Get_Desgination(data);
        }
        [Route("saveAppointmentdata")]
        public HR_Candidate_DetailsDTO saveAppointmentdata([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.saveAppointmentdata(data);
        }

        [Route("addQualification")]
        public HR_Candidate_DetailsDTO addQualification([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.addQualification(data);
        }

        [Route("addexperience")]
        public HR_Candidate_DetailsDTO addexperience([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.addexperience(data);
        }

        [Route("addfamily")]
        public HR_Candidate_DetailsDTO addfamily([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.addfamily(data);
        }

        [Route("addlanguage")]
        public HR_Candidate_DetailsDTO addlanguage([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.addlanguage(data);
        }
        //[Route("ActiveDeactiveRecord/{id:int}")]
        //public HR_Candidate_DetailsDTO ActiveDeactiveRecord(int id)
        //{
        //    HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
        //    dto.HRMET_Id = id;
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

        //    return del.deleterec(dto);
        //}

        
        [Route("sendCallLettermail")]
        public HR_Candidate_DetailsDTO sendCallLettermail([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.sendCallLettermail(data);
        }

        [Route("saveAppointmenttab")]
        public HR_Candidate_DetailsDTO saveAppointmenttab([FromBody]HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saveAppointmenttab(data);
        }
    }
}
