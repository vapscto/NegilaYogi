using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ActivateDeactivateStudentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ActivateDeactivateStudentDTO, ActivateDeactivateStudentDTO> COMMM = new CommonDelegate<ActivateDeactivateStudentDTO, ActivateDeactivateStudentDTO>();
        public ActivateDeactivateStudentDTO getdetails(int id)
        {
            return COMMM.GetDataByIdADM(id, "ActivateDeactivateStudentFacade/getdata/");

        }

        public ActivateDeactivateStudentDTO getlistone(ActivateDeactivateStudentDTO id)
        {
            return COMMM.POSTDataADM(id, "ActivateDeactivateStudentFacade/getACS/");

          
        }

        public ActivateDeactivateStudentDTO getlisttwo(ActivateDeactivateStudentDTO student)
        {
            return COMMM.POSTDataADM(student, "ActivateDeactivateStudentFacade/savedata/");

        }

        public ActivateDeactivateStudentDTO getlistthree(ActivateDeactivateStudentDTO student)
        {

            return COMMM.POSTDataADM(student, "ActivateDeactivateStudentFacade/getS/");

          
        }
    }
}
