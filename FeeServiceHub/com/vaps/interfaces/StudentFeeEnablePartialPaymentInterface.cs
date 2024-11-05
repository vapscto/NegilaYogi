using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface StudentFeeEnablePartialPaymentInterface
    {
        StudentFeeEnablePartialPaymentDTO GetYearList(int id);
        StudentFeeEnablePartialPaymentDTO get_student(StudentFeeEnablePartialPaymentDTO data);
        StudentFeeEnablePartialPaymentDTO getsection(StudentFeeEnablePartialPaymentDTO data);
        StudentFeeEnablePartialPaymentDTO savedata(StudentFeeEnablePartialPaymentDTO data);
        StudentFeeEnablePartialPaymentDTO deactivate(StudentFeeEnablePartialPaymentDTO data);
    }
}
