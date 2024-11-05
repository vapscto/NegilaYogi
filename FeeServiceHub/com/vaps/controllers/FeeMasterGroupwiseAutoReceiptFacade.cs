using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeMasterGroupwiseAutoReceiptFacade : Controller
    {
        public FeeMasterGroupwiseAutoReceiptInterface _IStatus;
        public FeeMasterGroupwiseAutoReceiptFacade(FeeMasterGroupwiseAutoReceiptInterface IStatus)
        {
            _IStatus = IStatus;
        }

        // load initial dropdown
        [Route("getdetails")]
        public Fee_Groupwise_AutoReceiptDTO getInitialData([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            return _IStatus.getinitialdata(data);
        }

        [Route("savedata")]
        public Fee_Groupwise_AutoReceiptDTO savedataa([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            return _IStatus.svedata(data);
        }

        [Route("edit")]
        public Fee_Groupwise_AutoReceiptDTO editdata([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            return _IStatus.editdta(data);
        }

        [Route("delete")]
        public Fee_Groupwise_AutoReceiptDTO deletedata([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            return _IStatus.deletedta(data);
        }

        [Route("getacademicyear")]
        public Fee_Groupwise_AutoReceiptDTO getacademicyear([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            return _IStatus.getacademicyear(data);
        }
        [Route("printreceipt")]
        public Task<Fee_Groupwise_AutoReceiptDTO> printreceipt([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            return _IStatus.printreceipt(data);
        }
        [Route("get_groupdetails")]        public Task<Fee_Groupwise_AutoReceiptDTO> get_prdetails([FromBody] Fee_Groupwise_AutoReceiptDTO data)

        {            return _IStatus.get_groupdetails(data);        }
    }
}
