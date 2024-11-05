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
    public class TransfrPreAdmtoAdmDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Adm_M_StudentDTO, Adm_M_StudentDTO> COMMM = new CommonDelegate<Adm_M_StudentDTO, Adm_M_StudentDTO>();


        public Adm_M_StudentDTO getAcademicdata(int  id)
        {
            return COMMM.GetDataById(id, "TransfrPreAdmtoAdmFacade/TrnfPreadmtoAdm/");

        }

        public Adm_M_StudentDTO getserdata(Adm_M_StudentDTO data)
        {
            return COMMM.POSTData(data, "TransfrPreAdmtoAdmFacade/searchdata/");
        }


        public Adm_M_StudentDTO expoadmi(Adm_M_StudentDTO data)
        {

            return COMMM.POSTData(data, "TransfrPreAdmtoAdmFacade/exporttoadmission/");
        }

    }
}
