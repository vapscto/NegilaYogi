using System;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports
{
    public class CollegeDefaultersReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();

        CommonDelegate<FeetransactionSMS, FeetransactionSMS> COMMM1 = new CommonDelegate<FeetransactionSMS, FeetransactionSMS>();

        public CollegeConcessionDTO getdetails(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "CollegeDefaultersReportFacade/getdetails/");
            
        }


        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDefaultersReportFacade/get_courses/");
        
        }


        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDefaultersReportFacade/get_branches/");
         
        }



        public CollegeConcessionDTO get_semisters(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDefaultersReportFacade/get_semisters/");
            
        }



        public CollegeConcessionDTO radiobtndata(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDefaultersReportFacade/radiobtndata/");
            
        }



        public FeetransactionSMS sendsms(FeetransactionSMS data)
        {
            return COMMM1.POSTDataCollfee(data, "CollegeDefaultersReportFacade/sendsms/");

        }



        public FeetransactionSMS sendemail(FeetransactionSMS data)
        {
            return COMMM1.POSTDataCollfee(data, "CollegeDefaultersReportFacade/sendemail/");

        }







    }
}
