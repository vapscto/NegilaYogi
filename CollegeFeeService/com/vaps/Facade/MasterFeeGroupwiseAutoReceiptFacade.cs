using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class MasterFeeGroupwiseAutoReceiptFacade : Controller
    {
        public MasterFeeGroupwiseAutoReceiptInterface _IStatus;
        public MasterFeeGroupwiseAutoReceiptFacade(MasterFeeGroupwiseAutoReceiptInterface IStatus)
        {
            _IStatus = IStatus;
        }
        // load initial dropdown
        [HttpPost]
        [Route("getdetails")]
        public MasterFeeGroupwiseAutoReceiptDTO getInitialData([FromBody] MasterFeeGroupwiseAutoReceiptDTO data)
        {
            return _IStatus.getinitialdata(data);
        }
        [Route("savedata")]
        public MasterFeeGroupwiseAutoReceiptDTO savedataa([FromBody] MasterFeeGroupwiseAutoReceiptDTO data)
        {
            return _IStatus.svedata(data);
        }
        [Route("edit")]
        public MasterFeeGroupwiseAutoReceiptDTO editdata([FromBody] MasterFeeGroupwiseAutoReceiptDTO data)
        {
            return _IStatus.editdta(data);
        }
        [Route("delete")]
        public MasterFeeGroupwiseAutoReceiptDTO deletedata([FromBody] MasterFeeGroupwiseAutoReceiptDTO data)
        {
            return _IStatus.deletedta(data);
        }
    }
}
