using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    //private const String JsonContentType = "application/json; charset=utf-8";
    public class StudentFeeEnablePartialPaymentDelegate
    {
        CommonDelegate<StudentFeeEnablePartialPaymentDTO, StudentFeeEnablePartialPaymentDTO> COMMM = new CommonDelegate<StudentFeeEnablePartialPaymentDTO, StudentFeeEnablePartialPaymentDTO>();
        public StudentFeeEnablePartialPaymentDTO GetYearList(int id)
        {
            return COMMM.GETFees(id, "StudentFeeEnablePartialPaymentFacade/GetYearList/");
        }
        public StudentFeeEnablePartialPaymentDTO get_student(StudentFeeEnablePartialPaymentDTO id)
        {
            return COMMM.POSTDatafee(id, "StudentFeeEnablePartialPaymentFacade/get_student/");
        }
        public StudentFeeEnablePartialPaymentDTO getsection(StudentFeeEnablePartialPaymentDTO id)
        {
            return COMMM.POSTDatafee(id, "StudentFeeEnablePartialPaymentFacade/getsection/");
        }
        public StudentFeeEnablePartialPaymentDTO savedata(StudentFeeEnablePartialPaymentDTO id)
        {
            return COMMM.POSTDatafee(id, "StudentFeeEnablePartialPaymentFacade/savedata/");
        }
        public StudentFeeEnablePartialPaymentDTO deactivate(StudentFeeEnablePartialPaymentDTO id)
        {
            return COMMM.POSTDatafee(id, "StudentFeeEnablePartialPaymentFacade/deactivate/");
        }
    }
}
