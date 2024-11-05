using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Alumni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Alumni;

namespace IVRMUX.Controllers.com.vapstech.Alumni
{

    [Route("api/[controller]")]
    public class Alumni_Student_FriendRequestController : Controller
    {
        Alumni_Student_FriendRequestDelegate _asf = new Alumni_Student_FriendRequestDelegate();

        //Alumnim Request send
        [Route("getdata/{id:int}")] 
        public Alumni_FriendRequestDTO getdata(int id)
        {
            Alumni_FriendRequestDTO dto = new Alumni_FriendRequestDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _asf.getdata(dto);

        }
        [Route("getsearch_data")]
        public Alumni_FriendRequestDTO getsearch_data([FromBody] dynamic value)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            Alumni_FriendRequestDTO dto = new Alumni_FriendRequestDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            try
            {
                JObject jObject = JObject.Parse(value.ToString());
                dto.stuStatus = (string)jObject["stuStatus"];

                JArray fld = (JArray)(jObject["output"]);
                foreach (var item in fld)
                {
                    var n = fld.Values(i.ToString());
                    dto.field.Add(n.FirstOrDefault());
                    i++;
                }
                JArray lke = (JArray)(jObject["output1"]);
                foreach (var item1 in lke)
                {
                    var n = lke.Values(j.ToString());
                    dto.Operator.Add(n.FirstOrDefault());
                    j++;
                }
                JArray val = (JArray)(jObject["output2"]);
                foreach (var item2 in val)
                {
                    var n = val.Values(k.ToString());
                    dto.value.Add(n.FirstOrDefault());
                    k++;
                }
                //JArray cond = (JArray)(jObject["output3"]);
                //foreach (var item3 in cond)
                //{
                //    var n = cond.Values(l.ToString());
                //    dto.condition.Add(n.FirstOrDefault());
                //    l++;
                //}
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return _asf.getsearch_data(dto);
        }


        [Route("sendrequest")]
        public Alumni_FriendRequestDTO sendrequest([FromBody] Alumni_FriendRequestDTO dto)
        {
           
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _asf.sendrequest(dto);

        }
        [Route("viewprofile")]
        public Alumni_FriendRequestDTO viewprofile([FromBody] Alumni_FriendRequestDTO dto)
        {
           
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _asf.viewprofile(dto);

        }
        //Alumnim Request accept

        [Route("getdata_request/{id:int}")] 
        public Alumni_FriendRequestDTO getdata_request(int id)
        {
            Alumni_FriendRequestDTO dto = new Alumni_FriendRequestDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _asf.getdata_request(dto);

        }
        [Route("FriendrequestAccept")] 
        public Alumni_FriendRequestDTO FriendrequestAccept([FromBody] Alumni_FriendRequestDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _asf.FriendrequestAccept(dto);

        }

    }
}