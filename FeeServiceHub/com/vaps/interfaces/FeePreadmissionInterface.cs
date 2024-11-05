using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeePreadmissionInterface
    {
        FeeStudentTransactionDTO getdata(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO selectstu(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO selectgrouppterm(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO savedata(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO printrec(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO search(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO searchfilter(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO recnogen(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO delrec(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO filstude(FeeStudentTransactionDTO data);

    }
}
