using PreadmissionDTOs.com.vaps.Fees.Tally;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeTallyTransactionInterface
    {
        TallyMTransactionDTO loaddata(TallyMTransactionDTO data);
        TallyMTransactionDTO getstudentdetails(TallyMTransactionDTO data);
        TallyMTransactionDTO savedata(TallyMTransactionDTO data);
        TallyMTransactionDTO deletedata(TallyMTransactionDTO data);
        TallyMTransactionDTO Paymentdata(TallyMTransactionDTO data);
        TallyMTransactionDTO Concessiondata(TallyMTransactionDTO data);

        TallyMTransactionDTO getalldetails(TallyMTransactionDTO data);

        TallyMTransactionDTO getvouchertypedetails(TallyMTransactionDTO data);

        //=============tally reports================== 
        TallyMTransactionDTO get_tally_data(TallyMTransactionDTO data);

    }
}
