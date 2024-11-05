using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces.FinancialAccounting
{
    public interface AutoLedgerCreationInterface
    {
        AutoLedgerCreationDTO getdata(AutoLedgerCreationDTO data);
        AutoLedgerCreationDTO savedetails(AutoLedgerCreationDTO pgmod);
        AutoLedgerCreationDTO deleterec(AutoLedgerCreationDTO data);
        AutoLedgerCreationDTO edit(AutoLedgerCreationDTO data);
        //savedatatwo
        AutoLedgerCreationDTO savedatatwo(AutoLedgerCreationDTO pgmod);
        AutoLedgerCreationDTO sectionchange(AutoLedgerCreationDTO pgmod);
    }
}
