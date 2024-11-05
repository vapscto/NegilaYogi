using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class CLGStudentFeeEnablePartialPaymentDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegeOverallFeeStatusDTO, CollegeOverallFeeStatusDTO> COMMM = new CommonDelegate<CollegeOverallFeeStatusDTO, CollegeOverallFeeStatusDTO>();
        public CollegeOverallFeeStatusDTO GetYearList(int id)
        {
            return COMMM.GETClgFee(id, "CLGStudentFeeEnablePartialPaymentFacade/GetYearList/");
        }
        public CollegeOverallFeeStatusDTO get_courses(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CLGStudentFeeEnablePartialPaymentFacade/get_courses/");
        }
        public CollegeOverallFeeStatusDTO get_branches(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CLGStudentFeeEnablePartialPaymentFacade/get_branches/");
        }
        public CollegeOverallFeeStatusDTO get_semisters(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CLGStudentFeeEnablePartialPaymentFacade/get_semisters/");
        }
        public CollegeOverallFeeStatusDTO get_student(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CLGStudentFeeEnablePartialPaymentFacade/get_student/");
        }
        public CollegeOverallFeeStatusDTO savedata(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CLGStudentFeeEnablePartialPaymentFacade/savedata/");
        }
        public CollegeOverallFeeStatusDTO deactivate(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CLGStudentFeeEnablePartialPaymentFacade/deactivate/");
        }
    }
}
