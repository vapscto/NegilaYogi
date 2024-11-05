using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;


namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeDefaulterReportInterface
    {

        FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data);

        //FeeTransactionPaymentDTO Getdetailsreport(FeeTransactionPaymentDTO temp);

        FeeTransactionPaymentDTO get_groups(FeeTransactionPaymentDTO data);
        Task<FeeTransactionPaymentDTO> getradiofiltereddata(FeeTransactionPaymentDTO temp);

        FeeTransactionPaymentDTO getsection(FeeTransactionPaymentDTO data);
        Task<FeetransactionSMS> sendsms(FeetransactionSMS data);
        FeeTransactionPaymentDTO sendemail(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO getgrpterms(FeeTransactionPaymentDTO data);

        //=============================================Staff Portal
        Task<FeeTransactionPaymentDTO> getstaffwiseclass(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO getStaffterms(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO saveremark(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO feeremarkreport(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO feeremarkload(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO feeremarksection(FeeTransactionPaymentDTO data);

        FeeTransactionPaymentDTO feedefaultersmstriggering(FeeTransactionPaymentDTO data);





    }
}
