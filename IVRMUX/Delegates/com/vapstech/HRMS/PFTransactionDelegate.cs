using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class PFTransactionDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PFReportsDTO, PFReportsDTO> COMMM = new CommonDelegate<PFReportsDTO, PFReportsDTO>();

        public PFReportsDTO onloadgetdetails(PFReportsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "PFTransactionFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public PFReportsDTO SavePFData(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/SavePFData/");
        }

        public PFReportsDTO getReport(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/getReport/");
        }

        public PFReportsDTO FilterEmployeeData(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/FilterEmployeeData/");
        }

        public PFReportsDTO editdata(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/editdata/");
        }

        public PFReportsDTO get_depts(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/get_depts/");
        }
        public PFReportsDTO get_desig(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/get_desig/");
        }

        public PFReportsDTO getEmployeedetailsBySelectionStJames(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/getEmployeedetailsBySelectionStJames/");
        }

        public PFReportsDTO getloaddata(PFReportsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "PFTransactionFacade/getloaddata");
        }
        public PFReportsDTO savedetails(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/savedetails/");
        }
        public PFReportsDTO deactive(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/deactive/");
        }
        public PFReportsDTO PFBlurcalculation(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/PFBlurcalculation/");
        }
        public PFReportsDTO EditSave(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/EditSave/");
        }
        public PFReportsDTO finalverify(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFTransactionFacade/finalverify/");
        }

    }
}
