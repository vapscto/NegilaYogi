using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeGroupWiseStudentReportInterface
    {
        FeeGroupWiseStudentReportDTO getInitailData(FeeGroupWiseStudentReportDTO data);
        FeeGroupWiseStudentReportDTO SearchData(FeeGroupWiseStudentReportDTO Clscatag);
        FeeGroupWiseStudentReportDTO Getclass(FeeGroupWiseStudentReportDTO Clscatag); 
             FeeGroupWiseStudentReportDTO GetSection(FeeGroupWiseStudentReportDTO Clscatag); 
            FeeGroupWiseStudentReportDTO GetStudent(FeeGroupWiseStudentReportDTO Clscatag);
    }
}
