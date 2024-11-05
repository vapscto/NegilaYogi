
using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace Admission.com.vapstech.Delegates
{
    public class BatchwiseStudentMappingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AdmSchoolAttendanceSubjectBatchDTO, AdmSchoolAttendanceSubjectBatchDTO> COMMM = new CommonDelegate<AdmSchoolAttendanceSubjectBatchDTO, AdmSchoolAttendanceSubjectBatchDTO>();

        public AdmSchoolAttendanceSubjectBatchDTO SaveDetails(AdmSchoolAttendanceSubjectBatchDTO ex_dto)
        {
            return COMMM.POSTDataADM(ex_dto, "BatchwiseStudentMappingFacade/");

        }


        // Get student details by Year and class

        public AdmSchoolAttendanceSubjectBatchDTO GetstudentdetailsbyYearandInstitute(AdmSchoolAttendanceSubjectBatchDTO Section)
        {
            return COMMM.POSTDataADM(Section, "BatchwiseStudentMappingFacade/GetstudentdetailsbyYearandInstitute");            
        }
        public AdmSchoolAttendanceSubjectBatchDTO getbatchwisestdlist(AdmSchoolAttendanceSubjectBatchDTO data)
        {
            return COMMM.POSTDataADM(data, "BatchwiseStudentMappingFacade/getbatchwisestdlist");
        }
        public AdmSchoolAttendanceSubjectBatchDTO getbatchname(AdmSchoolAttendanceSubjectBatchDTO data)
        {
            return COMMM.POSTDataADM(data, "BatchwiseStudentMappingFacade/getbatchname");
        }
        

    }
}
