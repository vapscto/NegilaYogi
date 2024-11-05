using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;


namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentSearchController : Controller
    {
        StudentSearchDelegate delg = new StudentSearchDelegate();
      

        // POST: api/StudentSearch
        [HttpPost]
        public Adm_M_StudentDTO Post([FromBody] dynamic value)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            Adm_M_StudentDTO dto = new Adm_M_StudentDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            //JArray jsonResponse = JArray.Parse(value.ToString());

            //foreach (var item in jsonResponse)
            //{
            //    JObject jRaces = (JObject)item["output"];
            //    foreach (var rItem in jRaces)
            //    {
            //        string rItemKey = rItem.Key;
            //        JObject rItemValueJson = (JObject)rItem.Value;
            //       // Races rowsResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Races>(rItemValueJson.ToString());
            //    }
            //}


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
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return delg.getData(dto);
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
