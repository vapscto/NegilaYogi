using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeOnlinePaymentInterface
    {
        FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO getamountdet(FeeStudentTransactionDTO data);

        PaymentDetails payuresponse(PaymentDetails response);

        FeeStudentTransactionDTO getgrouportermdeta(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO generatehashsequence(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO getcusgrp(FeeStudentTransactionDTO data);

        void OnlineTransactionupdate(FeeStudentTransactionDTO data);

        PaymentDetails.BillDeskPayment billdeskPayment(PaymentDetails.BillDeskPayment data);

        //FeeStudentTransactionDTO mobilepayuconnect(FeeStudentTransactionDTO data);
        Task<FeeStudentTransactionDTO> mobilepayuconnect(FeeStudentTransactionDTO data);


        PaymentDetails.PAYTM paytmresponse(PaymentDetails.PAYTM response);
        PaymentDetails razorresponse(PaymentDetails response);

        FeeStudentTransactionDTO Razorpaypaymentsettlementresponse(FeeStudentTransactionDTO response);
    
        PaymentDetails transactionstatuspaytm(PaymentDetails response);

        FeeStudentTransactionDTO RazorpayTransactionLogs(FeeStudentTransactionDTO data);
        //Easebuzz
        PaymentDetails.easybuzz getpaymentresponseeasybuzz(PaymentDetails.easybuzz response);

        PaymentDetails.CCAvenue getpaymentresponseccavenue(PaymentDetails.CCAvenue response);

      
        //Easebuzz
        Task<PaymentDetails.CCAvenue> initiateJuspayPayment(PaymentDetails.CCAvenue data);

        FeeStudentTransactionDTO Easebuzzsettlementresponse(FeeStudentTransactionDTO response);
  
        
        FeeStudentTransactionDTO RazorpayApi(FeeStudentTransactionDTO response);

        FeeStudentTransactionDTO Easebuzzpaymentsplitresponse(FeeStudentTransactionDTO response);

        FeeStudentTransactionDTO getFeetotalamount(FeeStudentTransactionDTO data);

        PaymentDetails.easybuzzmobile getpaymentresponseeasybuzzmobile(PaymentDetails.easybuzzmobile response);

        Task<FeeStudentTransactionDTO> EasebuzzPaymentPendingReceipts(FeeStudentTransactionDTO data);



    }
}




