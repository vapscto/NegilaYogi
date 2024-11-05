using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces.FinancialAccounting
{
   public interface FAMasterGroupInterface
    {
        FAMasterGroupDTO getdata(FAMasterGroupDTO data);
        FAMasterGroupDTO savedetails(FAMasterGroupDTO pgmod);
        FAMasterGroupDTO deleterec(FAMasterGroupDTO data);
        FAMasterGroupDTO edit(FAMasterGroupDTO data);
        //savedatatwo
        FAMasterGroupDTO savedatatwo(FAMasterGroupDTO data);
    }
}
