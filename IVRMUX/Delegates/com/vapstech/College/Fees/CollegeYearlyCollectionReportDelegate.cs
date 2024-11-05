﻿using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class CollegeYearlyCollectionReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();
        public CollegeConcessionDTO getdetails(CollegeConcessionDTO data)
        {


            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/getdetails/");


        }


        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/get_courses/");


        }


        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/get_branches/");

        }



        public CollegeConcessionDTO get_semisters(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/get_semisters/");

        }
        public CollegeConcessionDTO get_semisters_new(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/get_semisters_new/");

        }




        //gettting the head ids from group id
        public CollegeConcessionDTO getgroupheaddetails(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/getgroupmappedheads/");

        }


        //getting the head id based on head id selections
        public CollegeConcessionDTO getgroupheadsid(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/getgroupheadsid/");

        }

        public CollegeConcessionDTO Getreportdetails(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/Getreportdetails/");

        }

        public CollegeConcessionDTO getdata(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/getdata/");

        }

        //headwisedetails
        public CollegeConcessionDTO headwisedetails(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeYearlyCollectionReportFacade/headwisedetails/");
        }
    }
}
