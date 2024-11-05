using System;
using System.Collections.Generic;
using IVRMUX.Delegates.com.vapstech.Alumni;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;
using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CLGAlumnistudentsearchController  : Controller
    {
        CLGAlumnistudentsearchDelegate delg = new CLGAlumnistudentsearchDelegate();


        // POST: api/StudentSearch
        [HttpPost]
        public CLGAlumniStudentDTO Post([FromBody] dynamic value)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            CLGAlumniStudentDTO dto = new CLGAlumniStudentDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            try
            {
                JObject jObject = JObject.Parse(value.ToString());
                dto.stuStatus = (string)jObject["stuStatus"];
                dto.ASMAY_Id= (Int64)jObject["ASMAY_Id"];
                dto.AMCO_Id = (Int64)jObject["AMCO_Id"];
                dto.AMB_Id = (Int64)jObject["AMB_Id"];
                dto.AMSE_Id = (Int64)jObject["AMSE_Id"];
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

        [Route("getsemdata")]
        public CLGAlumniStudentDTO getsemdata([FromBody]CLGAlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delg.getsemdata(sddto);
        }

        // PUT: api/StudentSearch/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
