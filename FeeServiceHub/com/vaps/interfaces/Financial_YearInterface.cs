using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public  interface Financial_YearInterface
    {

        Financial_YearDTO save(Financial_YearDTO data);
        Financial_YearDTO loaddata(Financial_YearDTO data);
       
    }
}
