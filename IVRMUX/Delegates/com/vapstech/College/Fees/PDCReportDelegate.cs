using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports
{
    public class PDCReportDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonLibrary.CommonDelegate<PDC_EntryFormDTO, PDC_EntryFormDTO> COMMM = new CommonDelegate<PDC_EntryFormDTO, PDC_EntryFormDTO>();
        public PDC_EntryFormDTO getdetails(PDC_EntryFormDTO data)
        {



            return COMMM.POSTDataCollfee(data, "PDCReportFacade/getdetails/");


        }


        public PDC_EntryFormDTO get_courses(PDC_EntryFormDTO data)
        {
            return COMMM.POSTDataCollfee(data, "PDCReportFacade/get_courses/");


        }


        public PDC_EntryFormDTO get_branches(PDC_EntryFormDTO data)
        {
            return COMMM.POSTDataCollfee(data, "PDCReportFacade/get_branches/");

        }



        public PDC_EntryFormDTO get_semisters(PDC_EntryFormDTO data)
        {
            return COMMM.POSTDataCollfee(data, "PDCReportFacade/get_semisters/");

        }
        public PDC_EntryFormDTO get_semisters_new(PDC_EntryFormDTO data)
        {
            return COMMM.POSTDataCollfee(data, "PDCReportFacade/get_semisters_new/");

        }




        //gettting the head ids from group id
        public PDC_EntryFormDTO getgroupheaddetails(PDC_EntryFormDTO data)
        {
            return COMMM.POSTDataCollfee(data, "PDCReportFacade/getgroupmappedheads/");

        }


        //getting the head id based on head id selections
        public PDC_EntryFormDTO getgroupheadsid(PDC_EntryFormDTO data)
        {

            return COMMM.POSTDataCollfee(data, "PDCReportFacade/getgroupheadsid/");

        }


        public PDC_EntryFormDTO Getreportdetails(PDC_EntryFormDTO data)
        {
            return COMMM.POSTDataCollfee(data, "PDCReportFacade/Getreportdetails/");

        }


        public PDC_EntryFormDTO getdata(PDC_EntryFormDTO data)
        {
            return COMMM.POSTDataCollfee(data, "PDCReportFacade/getdata/");

        }
    }
}
