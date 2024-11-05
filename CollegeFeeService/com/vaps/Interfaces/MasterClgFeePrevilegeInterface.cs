using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
   public interface MasterClgFeePrevilegeInterface
    {
        MasterClgFeePrevilegeDTO getdetails(MasterClgFeePrevilegeDTO data);
        MasterClgFeePrevilegeDTO getusername(MasterClgFeePrevilegeDTO data);
        MasterClgFeePrevilegeDTO delete(int id);
        MasterClgFeePrevilegeDTO savedetail(MasterClgFeePrevilegeDTO objcategory);
        MasterClgFeePrevilegeDTO edit(int id);
    }
}
