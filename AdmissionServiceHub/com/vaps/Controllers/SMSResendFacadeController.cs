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

    public class SMSResendFacadeController : Controller
    {
        public SMSResendInterface _mastercaste;

        public SMSResendFacadeController(SMSResendInterface mastercaste)
        {
            _mastercaste = mastercaste;
        }

    

        [Route("Getdetailsstatus/")]
        public SMSResendDTO Getdetailsstatus([FromBody]SMSResendDTO SMSResendDTO)//int IVRMM_Id
        {
           
            return _mastercaste.Getdetailsstatus(SMSResendDTO);
           
        }
        [Route("Gettransnostatus/")]
        public SMSResendDTO Gettransnostatus([FromBody]SMSResendDTO SMSResendDTO)//int IVRMM_Id
        {
           
            return _mastercaste.Gettransnostatus(SMSResendDTO);
           
        }
        [Route("getstatusreport/")]
        public SMSResendDTO getstatusreport([FromBody]SMSResendDTO SMSResendDTO)//int IVRMM_Id
        {
           
            return _mastercaste.getstatusreport(SMSResendDTO);
           
        }
        [Route("checksmsstatus/")]
        public SMSResendDTO checksmsstatus([FromBody]SMSResendDTO SMSResendDTO)//int IVRMM_Id
        {
           
            return _mastercaste.checksmsstatus(SMSResendDTO);
           
        }




        [Route("Getdetails/")]
        public SMSResendDTO Getdetails([FromBody]SMSResendDTO SMSResendDTO)//int IVRMM_Id
        {
           
            return _mastercaste.Getdetails(SMSResendDTO);
           
        }

        [Route("resendMsg/")]
        public Task<SMSResendDTO> resendMsg([FromBody]SMSResendDTO SMSResendDTO)//int IVRMM_Id
        {
           
            return _mastercaste.resendMsg(SMSResendDTO);
           
        }


        [Route("Gettransno/")]
        public SMSResendDTO Gettransno([FromBody]SMSResendDTO SMSResendDTO)//int IVRMM_Id
        {

            return _mastercaste.Gettransno(SMSResendDTO);

        }
        [Route("showdata/")]
        public SMSResendDTO showdata([FromBody]SMSResendDTO SMSResendDTO)//int IVRMM_Id
        {

            return _mastercaste.showdata(SMSResendDTO);

        }
        


        //[Route("GetSelectedRowDetails/{id:int}")]
        //public SMSResendDTO GetSelectedRowDetails(int ID)
        //{

        //    return _mastercaste.GetSelectedRowDetails(ID);
        //}



        //[HttpDelete]
        //[Route("MasterDeleteModulesDATA/{id:int}")]
        //public SMSResendDTO MasterDeleteModulesDATA(int ID)
        //{

        //    return _mastercaste.MasterDeleteModulesData(ID);
        //}


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
