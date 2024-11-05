using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeStudentConcessionReportInterface
    {
        StudentConcesstionDTO getdata123(StudentConcesstionDTO data);
        StudentConcesstionDTO getreport(StudentConcesstionDTO data);

        FeeStudentTransactionDTO get_groups(FeeStudentTransactionDTO data);
    }
}
