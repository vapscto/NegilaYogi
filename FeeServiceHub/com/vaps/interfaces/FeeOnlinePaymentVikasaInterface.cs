using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeOnlinePaymentVikasaInterface
    {

        FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO getamountdet(FeeStudentTransactionDTO data);

        PaymentDetails payuresponse(PaymentDetails response);

        FeeStudentTransactionDTO getgrouportermdeta(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO generatehashsequence(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO getcusgrp(FeeStudentTransactionDTO data);

    
    }
}
