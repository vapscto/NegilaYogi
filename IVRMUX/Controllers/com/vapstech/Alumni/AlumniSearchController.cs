using IVRMUX.Delegates.com.vapstech.Alumni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AlumniSearchController : Controller
    {
        AlumniSearchDelegate delg = new AlumniSearchDelegate();
        // POST: api/StudentSearch
        [HttpPost]
        public AlumniStudentDTO Post([FromBody] dynamic value)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            AlumniStudentDTO dto = new AlumniStudentDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

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
                JArray cond = (JArray)(jObject["output3"]);
                foreach (var item3 in cond)
                {
                    var n = cond.Values(l.ToString());
                    dto.condition.Add(n.FirstOrDefault());
                    l++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return delg.getData(dto);
        }

        [Route("Getdetailsreport/")]
        public AlumniStudentDTO Getdetailsreport([FromBody] AlumniStudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return delg.Getdetailsreport(MMD);
        }

        // PUT: api/StudentSearch/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        [HttpGet("{id}")]
        [Route("Getdetails/")]
        public AlumniStudentDTO Getdetails(int MI_Id)
        {
            AlumniStudentDTO data = new AlumniStudentDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delg.GetClassWiseDailyAttendanceData(data);
        }
        [Route("getstate/")]
        public AlumniStudentDTO getstate([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delg.getstate(dto);
        }

        [Route("getdistrict/")]
        public AlumniStudentDTO getdistrict([FromBody] AlumniStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delg.getdistrict(dto);
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
