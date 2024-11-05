using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces.FinancialAccounting
{
  public  interface FiancialAccuntingLedgerInterface
    {
        FiancialAccuntingLedgerDTO getdata(FiancialAccuntingLedgerDTO data);
        FiancialAccuntingLedgerDTO savedetails(FiancialAccuntingLedgerDTO pgmod);
        FiancialAccuntingLedgerDTO deleterec(FiancialAccuntingLedgerDTO data);
        FiancialAccuntingLedgerDTO edit(FiancialAccuntingLedgerDTO data);
        //savedatatwo
        FiancialAccuntingLedgerDTO savedatatwo(FiancialAccuntingLedgerDTO pgmod);
    }
}
