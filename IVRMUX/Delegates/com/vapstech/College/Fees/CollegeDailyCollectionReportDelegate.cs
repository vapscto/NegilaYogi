using System;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports
{
    public class CollegeDailyCollectionReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();
        public CollegeConcessionDTO getdetails(CollegeConcessionDTO data)
        {


            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/getdetails/");
            
          
        }


        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/get_courses/");

        
        }


        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/get_branches/");
         
        }



        public CollegeConcessionDTO get_semisters(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/get_semisters/");
          
        }
         public CollegeConcessionDTO get_semisters_new(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/get_semisters_new/");
          
        }




        //gettting the head ids from group id
        public CollegeConcessionDTO getgroupheaddetails(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/getgroupmappedheads/");
            
        }


        //getting the head id based on head id selections
        public CollegeConcessionDTO getgroupheadsid(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/getgroupheadsid/");
         
        }


        public CollegeConcessionDTO Getreportdetails(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/Getreportdetails/");
         
        }
      

        public CollegeConcessionDTO getdata(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeDailyCollectionReportFacade/getdata/");
          
        }

    }
}
