using PreadmissionDTOs.com.vaps.MobileApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.MobileApp.Interfaces
{
    public interface FeesInterface
    {

        FeeDTO.getLoadData getloaddata(FeeDTO.getLoadData data);
        FeeDTO.getDetails Getdetails(FeeDTO.getDetails data);
        FeeDTO.feeReceiptGetLoadData feereceiptgetloaddata(FeeDTO.feeReceiptGetLoadData data);
        FeeDTO.getReceiptDetail getrecdetails(FeeDTO.getReceiptDetail data);
        FeeDTO.printReceipt printreceipt(FeeDTO.printReceipt data);
        FeeDTO.dueDate getduedates(FeeDTO.dueDate data);
        FeeDTO.getFeetotalamount getFeetotalamount(FeeDTO.getFeetotalamount data);
        FeeDTO.feeAnalysis feeAnalysisgetloaddata(FeeDTO.feeAnalysis data);        FeeDTO.feeTransactionlog feeTransactionlog(FeeDTO.feeTransactionlog data);        FeeDTO.feeTransactionlog feeTransactiondetail(FeeDTO.feeTransactionlog data);        FeeDTO.gatewayRate paymentGatewayrate(FeeDTO.gatewayRate data);
    }
}
