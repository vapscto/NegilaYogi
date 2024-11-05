using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports
{
    public class MakerAndCheckerReportDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonLibrary.CommonDelegate<CollegePaymentApprovalDTO, CollegePaymentApprovalDTO> COMMM = new CommonDelegate<CollegePaymentApprovalDTO, CollegePaymentApprovalDTO>();
        public CollegePaymentApprovalDTO getdetails(CollegePaymentApprovalDTO data)
        {



            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/getdetails/");


        }


        public CollegePaymentApprovalDTO get_courses(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/get_courses/");


        }


        public CollegePaymentApprovalDTO get_branches(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/get_branches/");

        }



        public CollegePaymentApprovalDTO get_semisters(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/get_semisters/");

        }
        public CollegePaymentApprovalDTO get_semisters_new(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/get_semisters_new/");

        }




        //gettting the head ids from group id
        public CollegePaymentApprovalDTO getgroupheaddetails(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/getgroupmappedheads/");

        }


        //getting the head id based on head id selections
        public CollegePaymentApprovalDTO getgroupheadsid(CollegePaymentApprovalDTO data)
        {

            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/getgroupheadsid/");

        }


        public CollegePaymentApprovalDTO Getreportdetails(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/Getreportdetails/");

        }


        public CollegePaymentApprovalDTO getdata(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "MakerAndCheckerReportFacade/getdata/");

        }
    }
}
