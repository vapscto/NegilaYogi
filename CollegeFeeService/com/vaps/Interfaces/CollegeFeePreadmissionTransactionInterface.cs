using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeFeePreadmissionTransactionInterface
    {
        CollegeFeeTransactionDTO getdata(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO selectstu(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO selectgrouppterm(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO savedata(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO printrec(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO search(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO searchfilter(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO recnogen(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO delrec(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO filstude(CollegeFeeTransactionDTO data);
    }
}
