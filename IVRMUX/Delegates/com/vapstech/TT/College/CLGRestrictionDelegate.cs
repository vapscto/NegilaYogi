using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.TT.College
{
    public class CLGRestrictionDelegate
    {
        CommonDelegate<CLGRestrictionDTO, CLGRestrictionDTO> _comm = new CommonDelegate<CLGRestrictionDTO, CLGRestrictionDTO>();



        //TAB1 START FIXING DAY PERIOD
        public CLGRestrictionDTO savetab1(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/savetab1/");
        }
        
        public CLGRestrictionDTO savedetail(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/savedetail/");
        }
        public CLGRestrictionDTO viewrecordspopup(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/viewrecordspopup/");
        }
        public CLGRestrictionDTO getalldetails(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/getalldetails/");
        }
      
        public CLGRestrictionDTO deactivatetab1(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/deactivatetab1/");
        }
        public CLGRestrictionDTO edittab1(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/edittab1/");
        }
        //TAB1 END FIXING DAY PERIOD


        //TAB2 START FIXING DAY STAFF

        public CLGRestrictionDTO savetab2(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/savetab2/");
        }
        public CLGRestrictionDTO viewtab2grid(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/viewtab2grid/");
        }
         public CLGRestrictionDTO gettab2editdata(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/gettab2editdata/");
        }
        public CLGRestrictionDTO deactivatetab2(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/deactivatetab2/");
        }

        //TAB2 END FIXING DAY STAFF


        
        //TAB3 END FIXING DAY SUBJECT

         public CLGRestrictionDTO savetab3(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/savetab3/");
        }
        public CLGRestrictionDTO viewtab3grid(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/viewtab3grid/");
        }

        public CLGRestrictionDTO edittab3(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/edittab3/");
        }
        public CLGRestrictionDTO deactivatetab3(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/deactivatetab3/");
        }
        //TAB2 END FIXING DAY SUBJECT

        //TAB4 START FIXING PERIOD STAFF

        public CLGRestrictionDTO savetab4(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/savetab4/");
        }
        public CLGRestrictionDTO viewtab4(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/viewtab4/");
        }
        public CLGRestrictionDTO edittab4(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/edittab4/");
        }
        public CLGRestrictionDTO deactivatetab4(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/deactivatetab4/");
        }

        //TAB4 END FIXING PERIOD STAFF
        //TAB5  FIXING PERIOD SUBJECT
        public CLGRestrictionDTO savetab5(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/savetab5/");
        }
         public CLGRestrictionDTO viewtab5(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/viewtab5/");
        }
         public CLGRestrictionDTO edittab5(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/edittab5/");
        }
         public CLGRestrictionDTO deactivatetab5(CLGRestrictionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGRestrictionFacade/deactivatetab5/");
        }


        //TAB5  FIXING PERIOD STAFF



    }
}
