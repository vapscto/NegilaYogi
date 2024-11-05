using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PreadmissionDTOs;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class ConcessionApprovalDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Preadmission_School_Registration_CatergoryDTO, Preadmission_School_Registration_CatergoryDTO> COMMM = 
            new CommonDelegate<Preadmission_School_Registration_CatergoryDTO, Preadmission_School_Registration_CatergoryDTO>();
        public Preadmission_School_Registration_CatergoryDTO loadta(Preadmission_School_Registration_CatergoryDTO id)
        {

            return COMMM.POSTData(id, "ConcessionApprovalFacade/loaddata/");
        }

        public Preadmission_School_Registration_CatergoryDTO getstudentdetails(Preadmission_School_Registration_CatergoryDTO dta)
        {
            return COMMM.POSTData(dta, "ConcessionApprovalFacade/catchange/");
        }

        public Preadmission_School_Registration_CatergoryDTO oncheckgetstudentdetails(Preadmission_School_Registration_CatergoryDTO dta)
        {
            return COMMM.POSTData(dta, "ConcessionApprovalFacade/oncheck/");
        }


        public Preadmission_School_Registration_CatergoryDTO confirmdta(Preadmission_School_Registration_CatergoryDTO dta)
        {
            return COMMM.POSTData(dta, "ConcessionApprovalFacade/saveconfirmdata/");
        }
    }
}
