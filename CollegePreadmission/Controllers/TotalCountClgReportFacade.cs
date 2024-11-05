using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePreadmission.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePreadmission.Controllers
{
    [Route("api/[controller]")]
    public class TotalCountClgReportFacade : Controller
    {

        public TotalCountClgReportInterface _AttenRpt;

        public TotalCountClgReportFacade(TotalCountClgReportInterface AttenRpt)
        {
            _AttenRpt = AttenRpt;
        }
        [Route("Get_Intial_data")]

        public  Task<CollegePreadmissionstudnetDto> Get_Intial_data([FromBody]CollegePreadmissionstudnetDto data)
        {
            return _AttenRpt.Get_Intial_data(data);
        }

        [HttpPost]
        [Route("Getdetails")]

        public  Task<CollegePreadmissionstudnetDto> Getdetails([FromBody] CollegePreadmissionstudnetDto reg)
        {
            return _AttenRpt.Getdetails(reg);
        }


        //preadmission status
        [Route("getstatusdata/{mi_id:int}")]
        public Task<CommonDTO> getstatusdata(int mi_id)
        {
            return _AttenRpt.getstatusdata(mi_id);
        }

        [Route("getdataonsearchfilter")]
        public CommonDTO getdataonsearchfilter([FromBody] CommonDTO cdto)
        {
            return _AttenRpt.getdataonsearchfilter(cdto);
        }

        [Route("savedata")]
        public CommonDTO saveData([FromBody] CommonDTO cdto)
        {
            return _AttenRpt.saveData(cdto);
        }


        [Route("Clgapplicationstudocs")]
        public CollegePreadmissionstudnetDto Clgapplicationstudocs([FromBody] CollegePreadmissionstudnetDto cdto)
        {
            return _AttenRpt.Clgapplicationstudocs(cdto);
        }

        [Route("Clgapplicationsturemarks")]
        public CollegePreadmissionstudnetDto Clgapplicationsturemarks([FromBody] CollegePreadmissionstudnetDto cdto)
        {
            return _AttenRpt.Clgapplicationsturemarks(cdto);
        }
    }
}
