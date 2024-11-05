using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{ 
    public class StaffAndOtherConcessionDelegate
    {
        CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();
        public CollegeConcessionDTO getdata(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/getalldetails/");

           
               
   
        }

        public CollegeConcessionDTO selectcatorclass(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/onselectclassorcat/");
            
           
            
        }

        public CollegeConcessionDTO fillheaddetailsss(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/fillhead/");


          
             
        }

        public CollegeConcessionDTO fillamount(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/fillamount/");


          
        }


        public CollegeConcessionDTO savedatadelegate(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/savedata/");

            
        }

        public CollegeConcessionDTO delrec(CollegeConcessionDTO enqdto)
        {

            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/deleteconcession/");


                
        }

        public CollegeConcessionDTO EditconcessionDetails(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/EditconcessionDetails/");

           
        }

        public CollegeConcessionDTO fillstaf(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/fillstaff/");

           
        }


        public CollegeConcessionDTO getacadem(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/getacademicyear/");


           
        }

        public CollegeConcessionDTO checkpaiddetails(CollegeConcessionDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "StaffAndOtherConcessionFacade/checkpaiddetails/");


           
        }
    }
}
