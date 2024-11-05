using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces.FinancialAccounting
{
   public interface FiancialAccountingVoucherInterface
    {
        FiancialAccountingVoucherDTO getdata(FiancialAccountingVoucherDTO data);
        FiancialAccountingVoucherDTO savedetails(FiancialAccountingVoucherDTO pgmod);
        FiancialAccountingVoucherDTO deleterec(FiancialAccountingVoucherDTO data);
        FiancialAccountingVoucherDTO edit(FiancialAccountingVoucherDTO data);
        //savedatatwo
        FiancialAccountingVoucherDTO savedatatwo(FiancialAccountingVoucherDTO pgmod);
    }
}
