using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    [Route("api/[controller]")]

    public class EMAILSMSTemplateSettingFacadeController : Controller
    {
        public EMAILSMSTemplateSettingInterface _Em;

        public EMAILSMSTemplateSettingFacadeController(EMAILSMSTemplateSettingInterface EMAILSMSTemplateSetting)
        {
            _Em = EMAILSMSTemplateSetting;
        }

    
      

        [Route("Getdetails/")]
        public EMAILSMSTemplateSettingDTO Getdetails([FromBody] EMAILSMSTemplateSettingDTO data)//int IVRMM_Id
        {
           
            return _Em.Getdetails(data);
           
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public EMAILSMSTemplateSettingDTO GetSelectedRowDetails(int ID)
        {
           
            return _Em.GetSelectedRowDetails(ID);
        }
    
        [HttpPost]
        public EMAILSMSTemplateSettingDTO Post([FromBody] EMAILSMSTemplateSettingDTO masterMDT)
        {
          
            return _Em.MasterActivityData(masterMDT);
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public EMAILSMSTemplateSettingDTO MasterDeleteModulesDATA(int ID)
        {
           
            return _Em.MasterDeleteModulesData(ID);
        }

     
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
