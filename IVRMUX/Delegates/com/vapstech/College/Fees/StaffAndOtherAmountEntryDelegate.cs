using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class StaffAndOtherAmountEntryDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<FeeAmountEntryDTO, FeeAmountEntryDTO> COMMM = new CommonDelegate<FeeAmountEntryDTO, FeeAmountEntryDTO>();
        public FeeAmountEntryDTO getdata(FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/getalldetails/");
  
        }

        public FeeAmountEntryDTO EditDetails(FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/Editdetails/");

    
        }

        public FeeAmountEntryDTO savedetails(FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/");

      

               
          
        }

        public FeeAmountEntryDTO deleterec(FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/deletemodpages/");

          

           
        }

        public FeeAmountEntryDTO getsearchdata(int data, FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/Editdetails/");

           
            
        
        }


        public FeeAmountEntryDTO getgroupheaddetails(FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/getgroupmappedheads/");

   
            

        }

        public FeeAmountEntryDTO paymentdetailsfnc(FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/paymentdetails/");

       
            

        }


        public FeeAmountEntryDTO selectacade(FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/selectacademicyear/");

          

               
           
        }
        public FeeAmountEntryDTO getalldetailsOnselectiontype(FeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherAmountEntryFacade/getalldetailsOnselectiontype/");

         

            
        }
    }
}
