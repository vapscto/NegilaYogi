using CommonLibrary;
using PreadmissionDTOs.com.vaps.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.COE
{
    public class COEdelegate
    {
       
        CommonDelegate<COEDTO, COEDTO> COMMM = new CommonDelegate<COEDTO, COEDTO>();
        public COEDTO getdata(COEDTO obj)
        {
            return COMMM.POSTDataCOE(obj, "COEFacade/getdata/");
        }
        public COEDTO getEvents(COEDTO obj)
        {
            return COMMM.POSTDataCOE(obj, "COEFacade/getEvents/");
        }
        public COEDTO sendmsg(COEDTO data)
        {
            return COMMM.POSTDataCOE(data, "COEFacade/sendmsg/");
        }
    }
}
