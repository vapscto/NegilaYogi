using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.College.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.COE
{
    public class ClgCOEReportDelegate
    {
        CommonDelegate<ClgMasterCOEDTO, ClgMasterCOEDTO> COMMM = new CommonDelegate<ClgMasterCOEDTO, ClgMasterCOEDTO>();
     
        //public ClgMasterCOEDTO getdetails(ClgMasterCOEDTO categorypage)
        //{
        //    return COMMM.POSTClgCOE(categorypage, "ClgMasterCOEFacade/getdetails");
        //}

        public ClgMasterCOEDTO getdetails(int id)
        {
            return COMMM.GetDataByIdCOE(id, "ClgReportCOEFacade/getdata/");
        }
        public ClgMasterCOEDTO getReport(ClgMasterCOEDTO dto)
        {
            return COMMM.POSTDataCOE(dto, "ClgReportCOEFacade/getReport");
        }

        public ClgMasterCOEDTO mothreport(ClgMasterCOEDTO dto)
        {
            return COMMM.POSTDataCOE(dto, "ClgReportCOEFacade/mothreport");
        }
    }
}
