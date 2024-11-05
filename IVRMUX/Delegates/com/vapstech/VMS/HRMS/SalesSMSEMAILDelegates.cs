using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.HRMS
{
    public class SalesSMSEMAILDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SalesSMSEMAILDTO, SalesSMSEMAILDTO> COMMM = new CommonDelegate<SalesSMSEMAILDTO, SalesSMSEMAILDTO>();

        public SalesSMSEMAILDTO onloadgetdetails(SalesSMSEMAILDTO dto)
        {
            return COMMM.POSTVMS(dto, "SalesSMSEMAILFacade/onloadgetdetails");
        }

        public SalesSMSEMAILDTO sendsmsemail(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "SalesSMSEMAILFacade/sendsmsemail/");
        }
        public SalesSMSEMAILDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "SalesSMSEMAILFacade/getRecordById/");
        }
        public SalesSMSEMAILDTO get_state(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "SalesSMSEMAILFacade/get_state/");
        }
        public SalesSMSEMAILDTO getrpt(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "SalesSMSEMAILFacade/getrpt/");
        }
         public SalesSMSEMAILDTO getrpt_lead(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "SalesSMSEMAILFacade/getrpt_lead/");
        }
        public SalesSMSEMAILDTO loadtemplate(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "SalesSMSEMAILFacade/loadtemplate/");
        }
        public SalesSMSEMAILDTO viewtemplatedetails(SalesSMSEMAILDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "SalesSMSEMAILFacade/viewtemplatedetails/");
        }


    }
}
