using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class Master80CDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_80CDTO, HR_Master_80CDTO> COMMM = new CommonDelegate<HR_Master_80CDTO, HR_Master_80CDTO>();

        public HR_Master_80CDTO onloadgetdetails(HR_Master_80CDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "Master80CFacade/onloadgetdetails");
        }

        public HR_Master_80CDTO savedetails(HR_Master_80CDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "Master80CFacade/");
        }
        public HR_Master_80CDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "Master80CFacade/getRecordById/");
        }
        public HR_Master_80CDTO deleterec(HR_Master_80CDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "Master80CFacade/deactivateRecordById/");
        }

    }
}
