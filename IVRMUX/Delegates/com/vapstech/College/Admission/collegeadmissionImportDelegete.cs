using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class collegeadmissionImportDelegete
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegeImportStudentWrapperDTO, CollegeImportStudentWrapperDTO> COMMM = new CommonDelegate<CollegeImportStudentWrapperDTO, CollegeImportStudentWrapperDTO>();
        public CollegeImportStudentWrapperDTO savedata(CollegeImportStudentWrapperDTO data)
        {
            return COMMM.clgadmissionbypost(data, "collegeadmissionImportFacade/savedata/");
        }
        public CollegeImportStudentWrapperDTO checkvalidation(CollegeImportStudentWrapperDTO data)
        {
            return COMMM.clgadmissionbypost(data, "collegeadmissionImportFacade/checkvalidation/");
        }
    }
}
