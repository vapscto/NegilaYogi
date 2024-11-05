using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class BankCashReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<BankCashReportDTO, BankCashReportDTO> COMMM = new CommonDelegate<BankCashReportDTO, BankCashReportDTO>();

        public BankCashReportDTO onloadgetdetails(BankCashReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "BankCashReportFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public BankCashReportDTO getEmployeedetailsBySelection(BankCashReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "BankCashReportFacade/getEmployeedetailsBySelection/");
        }

        public BankCashReportDTO get_depts(BankCashReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "BankCashReportFacade/get_depts/");
        }
        public BankCashReportDTO get_desig(BankCashReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "BankCashReportFacade/get_desig/");
        }
    }
}
