using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeMasterGroupwiseAutoReceiptInterface
    {
        Fee_Groupwise_AutoReceiptDTO getinitialdata(Fee_Groupwise_AutoReceiptDTO data);

        Fee_Groupwise_AutoReceiptDTO svedata(Fee_Groupwise_AutoReceiptDTO data);
        Fee_Groupwise_AutoReceiptDTO editdta(Fee_Groupwise_AutoReceiptDTO data);
        Fee_Groupwise_AutoReceiptDTO deletedta(Fee_Groupwise_AutoReceiptDTO data);
        Fee_Groupwise_AutoReceiptDTO getacademicyear(Fee_Groupwise_AutoReceiptDTO data);
        Task<Fee_Groupwise_AutoReceiptDTO> printreceipt(Fee_Groupwise_AutoReceiptDTO data);
        Task<Fee_Groupwise_AutoReceiptDTO> get_groupdetails(Fee_Groupwise_AutoReceiptDTO data);




    }
}
