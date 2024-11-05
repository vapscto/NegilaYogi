using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces.FinancialAccounting
{
  public  interface FAUser_GroupInterface
    {
        FAUser_GroupDTO getdata(FAUser_GroupDTO data);
        FAUser_GroupDTO savedetails(FAUser_GroupDTO pgmod);
        FAUser_GroupDTO deleterec(FAUser_GroupDTO data);
        FAUser_GroupDTO edit(FAUser_GroupDTO data);
        //savedatatwo
        FAUser_GroupDTO savedatatwo(FAUser_GroupDTO pgmod);
        //Userchange
        FAUser_GroupDTO Userchange(FAUser_GroupDTO pgmod);
    }
}
