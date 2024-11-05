using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.MobileApp.Interfaces;
using PreadmissionDTOs.com.vaps.MobileApp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.MobileApp.Controllers
{
    [Route("api/[controller]")]
    public class FeesCtrl : Controller
    {
        public FeesInterface _ads;

        public FeesCtrl(FeesInterface adstu)
        {
            _ads = adstu;
        }

        //Fee Detail Page
        [HttpPost]
        [Route("getloaddata")]
        public FeeDTO.getLoadData getloaddata([FromBody] FeeDTO.getLoadData data)
        {
            return _ads.getloaddata(data);
        }


        [Route("Getdetails")]
        public FeeDTO.getDetails Getdetails([FromBody] FeeDTO.getDetails data)
        {
            return _ads.Getdetails(data);
        }

        //Fee Receipt page
        [Route("feereceiptgetloaddata")]
        public FeeDTO.feeReceiptGetLoadData feereceiptgetloaddata([FromBody] FeeDTO.feeReceiptGetLoadData data)
        {
            return _ads.feereceiptgetloaddata(data);
        }

        [Route("getrecdetails")]
        public FeeDTO.getReceiptDetail getrecdetails([FromBody] FeeDTO.getReceiptDetail data)
        {
            return _ads.getrecdetails(data);
        }


        [Route("printreceipt")]
        public FeeDTO.printReceipt printreceipt([FromBody] FeeDTO.printReceipt sddto)
        {
            return _ads.printreceipt(sddto);
        }


        [Route("getduedates")]
        public FeeDTO.dueDate getduedates([FromBody] FeeDTO.dueDate data)
        {
            return _ads.getduedates(data);
        }
        [Route("getFeetotalamount")]
        public FeeDTO.getFeetotalamount getFeetotalamount([FromBody] FeeDTO.getFeetotalamount data)
        {
            return _ads.getFeetotalamount(data);
        }
        [Route("feeAnalysisgetloaddata")]        public FeeDTO.feeAnalysis feeAnalysisgetloaddata([FromBody] FeeDTO.feeAnalysis data)        {            return _ads.feeAnalysisgetloaddata(data);        }        [Route("feeTransactionlog")]        public FeeDTO.feeTransactionlog feeTransactionlog([FromBody] FeeDTO.feeTransactionlog data)        {            return _ads.feeTransactionlog(data);        }        [Route("feeTransactiondetail")]        public FeeDTO.feeTransactionlog feeTransactiondetail([FromBody] FeeDTO.feeTransactionlog data)        {            return _ads.feeTransactiondetail(data);        }        [Route("paymentGatewayrate")]        public FeeDTO.gatewayRate paymentGatewayrate([FromBody] FeeDTO.gatewayRate data)        {            return _ads.paymentGatewayrate(data);        }
        //Fee Online Payment


    }
}
