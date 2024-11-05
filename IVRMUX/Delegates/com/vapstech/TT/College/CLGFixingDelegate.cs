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
    public class CLGFixingDelegate
    {
        CommonDelegate<CLGFixingDTO, CLGFixingDTO> _comm = new CommonDelegate<CLGFixingDTO, CLGFixingDTO>();



        //TAB1 START FIXING DAY PERIOD
        public CLGFixingDTO savetab1(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/savetab1/");
        }
        
        public CLGFixingDTO savedetail(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/savedetail/");
        }
        public CLGFixingDTO viewrecordspopup(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/viewrecordspopup/");
        }
        public CLGFixingDTO getalldetails(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/getalldetails/");
        }
      
        public CLGFixingDTO deactivatetab1(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/deactivatetab1/");
        }
        public CLGFixingDTO edittab1(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/edittab1/");
        }
        //TAB1 END FIXING DAY PERIOD


        //TAB2 START FIXING DAY STAFF

        public CLGFixingDTO savetab2(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/savetab2/");
        }
        public CLGFixingDTO viewtab2grid(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/viewtab2grid/");
        }
         public CLGFixingDTO gettab2editdata(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/gettab2editdata/");
        }
        public CLGFixingDTO deactivatetab2(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/deactivatetab2/");
        }

        //TAB2 END FIXING DAY STAFF


        
        //TAB3 END FIXING DAY SUBJECT

         public CLGFixingDTO savetab3(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/savetab3/");
        }
        public CLGFixingDTO viewtab3grid(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/viewtab3grid/");
        }

        public CLGFixingDTO edittab3(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/edittab3/");
        }
        public CLGFixingDTO deactivatetab3(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/deactivatetab3/");
        }
        //TAB2 END FIXING DAY SUBJECT

        //TAB4 START FIXING PERIOD STAFF

        public CLGFixingDTO savetab4(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/savetab4/");
        }
        public CLGFixingDTO viewtab4(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/viewtab4/");
        }
        public CLGFixingDTO edittab4(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/edittab4/");
        }
        public CLGFixingDTO deactivatetab4(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/deactivatetab4/");
        }

        //TAB4 END FIXING PERIOD STAFF
        //TAB5  FIXING PERIOD SUBJECT
        public CLGFixingDTO savetab5(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/savetab5/");
        }
         public CLGFixingDTO viewtab5(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/viewtab5/");
        }
         public CLGFixingDTO edittab5(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/edittab5/");
        }
         public CLGFixingDTO deactivatetab5(CLGFixingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGFixingFacade/deactivatetab5/");
        }


        //TAB5  FIXING PERIOD STAFF



    }
}
