using PreadmissionDTOs.com.vaps.Fees.Tally;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeHeadLedMapInter
    {
        HeadLedgerCodeMapDTO loaddata(HeadLedgerCodeMapDTO data);
        HeadLedgerCodeMapDTO getgroupdetails(HeadLedgerCodeMapDTO data);
        HeadLedgerCodeMapDTO getheaddetails(HeadLedgerCodeMapDTO data);
        HeadLedgerCodeMapDTO savedata(HeadLedgerCodeMapDTO data);
        HeadLedgerCodeMapDTO deletedata(HeadLedgerCodeMapDTO data);
        
    }
}
