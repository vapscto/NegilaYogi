using System;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vaps.admission
{
    public class AdmissionImportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ImportStudentWrapperDTO, ImportStudentWrapperDTO> COMMM = new CommonDelegate<ImportStudentWrapperDTO, ImportStudentWrapperDTO>();
        public ImportStudentWrapperDTO savedata(ImportStudentWrapperDTO data)
        {
            return COMMM.POSTDataADM(data, "AdmissionImportFacade/savedata/");
        }
        public ImportStudentWrapperDTO checkvalidation(ImportStudentWrapperDTO data)
        {
            return COMMM.POSTDataADM(data, "AdmissionImportFacade/checkvalidation/");
        }
    }
}
