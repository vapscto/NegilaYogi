using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees.Tally
{
    public class Financial_YearDelegate
    {
        CommonDelegate<Financial_YearDTO, Financial_YearDTO> comm = new CommonDelegate<Financial_YearDTO, Financial_YearDTO>();
        public Financial_YearDTO save(Financial_YearDTO data)
        {
            return comm.POSTDatafee(data, "Financial_YearFacade/save");
        }
        public Financial_YearDTO loaddata(Financial_YearDTO data)
        {
            return comm.POSTDatafee(data, "Financial_YearFacade/loaddata");
        }
       
    }
}
