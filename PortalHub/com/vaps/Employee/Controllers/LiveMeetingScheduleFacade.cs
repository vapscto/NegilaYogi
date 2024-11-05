using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalHub.com.vaps.Employee.Interfaces;
using PortalHub.com.vaps.Employee.Services;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class LiveMeetingScheduleFacade : Controller
    {

        public LiveMeetingScheduleInterface _org;
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<LiveMeetingScheduleImpl> _log;
        public LiveMeetingScheduleFacade(LiveMeetingScheduleInterface org, DomainModelMsSqlServerContext db, ILogger<LiveMeetingScheduleImpl> loggerFactor)
        {
            _org = org;
            _db = db;
            _log = loggerFactor;
        }


        [Route("getalldetails")]
        public LiveMeetingScheduleDTO getalldetails([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getdatastuacadgrp(data);
        }
        [Route("callback")]
        public JsonResult getdata([FromQuery]string meetingID,bool recordingmarks)
        {
             var meeting = _db.LMS_Live_MeetingDMO.Where(c => c.LMSLMEET_MeetingId== meetingID).ToList();
            {
                var userid = _db.Staff_User_Login.Single(c => c.Emp_Code == meeting.FirstOrDefault().HRME_Id).Id;

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                var ress = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == meeting.FirstOrDefault().LMSLMEET_Id);
                ress.LMSLMEET_Recordflag = recordingmarks;
                ress.LMSLMEET_EndTime = time;
                ress.LMSLMEET_MACAddress = sMacAddress;
                ress.LMSLMEET_IPAddress = myIP1;
                ress.LMSLMEET_UpdatedBy = userid;
                ress.LMSLMEET_UpdatedDate = DateTime.Now;
                _db.Update(ress);
                int ss = _db.SaveChanges();                

                var resss = _db.LMS_Live_Meeting_StudentDMO.Where(e => e.LMSLMEET_Id == meeting.FirstOrDefault().LMSLMEET_Id && (e.LMSLMEETSTD_LoginTime!=null && e.LMSLMEETSTD_LoginTime!="")).ToList();
                foreach (var item in resss)
                {
                    item.LMSLMEETSTD_LogoutTime = time;
                    item.LMSLMEETSTD_MACAddress = sMacAddress;
                    item.LMSLMEETSTD_IPAddress = myIP1;
                    item.LMSLMEETSTD_UpdatedBy = userid;
                    item.LMSLMEETSTD_UpdatedDate = DateTime.Now;
                    _db.Update(item);
                }
                int sss = _db.SaveChanges();                
            }                 

            return Json("");
        }
       

        [Route("getempdetails")]
        public LiveMeetingScheduleDTO getempdetails([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getempdetails(data);
        }
        [Route("getstudentdetails")]
        public LiveMeetingScheduleDTO getstudentdetails([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getstudentdetails(data);
        }
        [Route("endmainmeetingstudent")]
        public LiveMeetingScheduleDTO endmainmeetingstudent([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.endmainmeetingstudent(data);
        }
        [Route("joinmeetingStudent")]
        public LiveMeetingScheduleDTO joinmeetingStudent([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.joinmeetingStudent(data);
        }

        [Route("getclass")]
        public LiveMeetingScheduleDTO getclass([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getclass(data);
        }
        [Route("onstartmeeting")]
        public LiveMeetingScheduleDTO onstartmeeting([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.onstartmeeting(data);
        }
        [Route("ondatechange")]
        public LiveMeetingScheduleDTO ondatechange([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.ondatechange(data);
        }
        [Route("ondatechangestudent")]
        public LiveMeetingScheduleDTO ondatechangestudent([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.ondatechangestudent(data);
        }
        [Route("endmainmeeting")]
        public LiveMeetingScheduleDTO endmainmeeting([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.endmainmeeting(data);
        }
        [Route("joinmeeting")]
        public LiveMeetingScheduleDTO joinmeeting([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.joinmeeting(data);
        }
        [Route("getsection")]
        public LiveMeetingScheduleDTO getsection([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getsection(data);
        }
        [Route("savedata")]
        public LiveMeetingScheduleDTO savedata([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.savedata(data);
        }
        [Route("editdata")]
        public LiveMeetingScheduleDTO editdata([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.editdata(data);
        }
        [Route("getsubject")]
        public LiveMeetingScheduleDTO getsubject([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getsubject(data);
        }
        [Route("checkduplicate")]
        public LiveMeetingScheduleDTO checkduplicate([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.checkduplicate(data);
        }
        
        [Route("deactive")]
        public LiveMeetingScheduleDTO deactive([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.deactive(data);
        }
        //REPORT
         [Route("getschrptdetailsprofile")]
        public LiveMeetingScheduleDTO getschrptdetailsprofile([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getschrptdetailsprofile(data);
        }

        [Route("getschrptdetails")]
        public LiveMeetingScheduleDTO getschrptdetails([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getschrptdetails(data);
        }


        [Route("getschedulereport")]
        public LiveMeetingScheduleDTO getschedulereport([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getschedulereport(data);
        }
        [Route("getstaffprofilereport")]
        public LiveMeetingScheduleDTO getstaffprofilereport([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getstaffprofilereport(data);
        }
        [Route("getstudentprofiledata")]
        public LiveMeetingScheduleDTO getstudentprofiledata([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getstudentprofiledata(data);
        }
        [Route("getstudentprofilereport")]
        public LiveMeetingScheduleDTO getstudentprofilereport([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.getstudentprofilereport(data);
        }
        [Route("sendnotification")]
        public LiveMeetingScheduleDTO sendnotification([FromBody] LiveMeetingScheduleDTO data)
        {
            return _org.sendnotification(data);
        }
        [HttpPost]
        [Route("recordcallback")]
        public string recordcallback(string signed_parameters)
        {
            var stream = signed_parameters;
            _log.LogInformation("stream"+stream);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            _log.LogInformation("jsonToken" + jsonToken.ToString());

            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
            _log.LogInformation("tokenS" + tokenS.ToString());

            string meetingid= tokenS.Payload["meeting_id"].ToString();
            string record_id = tokenS.Payload["record_id"].ToString();
            _log.LogInformation("meetingid" + meetingid+ record_id);
            var meeting = _db.LMS_Live_MeetingDMO.Where(c => c.LMSLMEET_MeetingId == meetingid).ToList();
            if(meeting.Count()>0)
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                var ress = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == meeting.FirstOrDefault().LMSLMEET_Id);
                ress.LMSLMEET_RecordId = record_id;               
                ress.LMSLMEET_UpdatedDate = indianTime;
                _db.Update(ress);
                int ss = _db.SaveChanges();
                if (ss > 0)
                {
                    // data.returnval = true;
                }
                else
                {
                    // data.returnval = true;
                }
            }

            return "";
        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
