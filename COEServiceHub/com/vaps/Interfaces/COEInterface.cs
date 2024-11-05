using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.COE;

namespace CoeServiceHub.com.vaps.Interfaces
{
   public interface COEInterface
    {
        COEDTO getdata(COEDTO dto);
        COEDTO getEvents(COEDTO dto);
        COEDTO sendmsg(COEDTO data);
    }
}
