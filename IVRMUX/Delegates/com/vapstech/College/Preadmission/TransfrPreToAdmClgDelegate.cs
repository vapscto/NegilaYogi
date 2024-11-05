using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class TransfrPreToAdmClgDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TransfrPreToAdmDTO, TransfrPreToAdmDTO> COMMM = new CommonDelegate<TransfrPreToAdmDTO, TransfrPreToAdmDTO>();

        public TransfrPreToAdmDTO onloadgetdetails(TransfrPreToAdmDTO dto)
        {
            return COMMM.CollegePOSTData(dto, "TransfrPreToAdmClgFacade/onloadgetdetails");
        }
        public TransfrPreToAdmDTO get_branchs(TransfrPreToAdmDTO dt)
        {
            return COMMM.CollegePOSTData(dt, "TransfrPreToAdmClgFacade/get_branchs/");
        }
        public TransfrPreToAdmDTO get_semester(TransfrPreToAdmDTO dt)
        {
            return COMMM.CollegePOSTData(dt, "TransfrPreToAdmClgFacade/get_semester/");
        }
        public TransfrPreToAdmDTO getserdata(TransfrPreToAdmDTO dt)
        {
            return COMMM.CollegePOSTData(dt, "TransfrPreToAdmClgFacade/getserdata/");
        }
        public TransfrPreToAdmDTO expoadmi(TransfrPreToAdmDTO dt)
        {
            return COMMM.CollegePOSTData(dt, "TransfrPreToAdmClgFacade/expoadmi/");
        }
        
    }
}
