using System;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports
{
    public class CollegeHeadWiseCollectionReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();

        public CollegeConcessionDTO getdetails(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "CollegeHeadWiseCollectionReportFacade/getdetails/");
            
        }


        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeHeadWiseCollectionReportFacade/get_courses/");
            
        }


        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeHeadWiseCollectionReportFacade/get_branches/");
          
        }



        public CollegeConcessionDTO get_semisters(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeHeadWiseCollectionReportFacade/get_semisters/");
            
        }



        public CollegeConcessionDTO getgroupheaddetails(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "CollegeHeadWiseCollectionReportFacade/getgroupmappedheads/");

        }

        public CollegeConcessionDTO radiobtndata(CollegeConcessionDTO data)
        {


            return COMMM.POSTDataCollfee(data, "CollegeHeadWiseCollectionReportFacade/radiobtndata/");
            
        }
    }
}
