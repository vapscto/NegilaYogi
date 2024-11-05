using System;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class AdmissionStandardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AdmissionStandardDTO, AdmissionStandardDTO> COMMM = new CommonDelegate<AdmissionStandardDTO, AdmissionStandardDTO>();
        public AdmissionStandardDTO getlisttwo(AdmissionStandardDTO student)
        {
            return COMMM.POSTDataADM(student, "AdmissionStandardFacade/savedata/");            
        }
        public AdmissionStandardDTO getlistdata(int id)
        {
            return COMMM.GetDataByIdADM(id, "AdmissionStandardFacade/loaddata/");           
        }

        // Admission Cancel Configuration
        public AdmissionStandardDTO CancelConfigLoad(AdmissionStandardDTO data)
        {
            return COMMM.POSTDataADM(data, "AdmissionStandardFacade/CancelConfigLoad/");
        }
        public AdmissionStandardDTO SaveCancelConfigData(AdmissionStandardDTO data)
        {
            return COMMM.POSTDataADM(data, "AdmissionStandardFacade/SaveCancelConfigData/");
        }
        public AdmissionStandardDTO EditCancelConfig(AdmissionStandardDTO data)
        {
            return COMMM.POSTDataADM(data, "AdmissionStandardFacade/EditCancelConfig/");
        }
        public AdmissionStandardDTO ActiveDeactiveCancelConfig(AdmissionStandardDTO data)
        {
            return COMMM.POSTDataADM(data, "AdmissionStandardFacade/ActiveDeactiveCancelConfig/");
        }
    }
}