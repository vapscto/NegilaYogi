using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface collegeadmissionImportInterface
    {
        Task<CollegeImportStudentWrapperDTO> getdetails(CollegeImportStudentWrapperDTO stu);
        Task<CollegeImportStudentWrapperDTO> checkvalidation(CollegeImportStudentWrapperDTO data);
    }
}
