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
    public class FeeRemittanceCertificateFacade : Controller
    {
        public FeeRemittanceCertificateInterface _IStatus;

        public FeeRemittanceCertificateFacade(FeeRemittanceCertificateInterface IStatus)
        {
            _IStatus = IStatus;

        }
        // load initial dropdown
        [Route("getinitialdata/{mi_id:int}")]
        public FeeRemittanceCertificateDTO getInitialData(int mi_id)
        {
            return _IStatus.getInitailData(mi_id);
        }

        [HttpPost]
        [Route("searchdata")]
        public FeeRemittanceCertificateDTO SearchData([FromBody] FeeRemittanceCertificateDTO Clscatag)
        {
            return _IStatus.SearchData(Clscatag);
        }
        [Route("getAdm_Name")]
        public FeeRemittanceCertificateDTO getAdm_Name([FromBody] FeeRemittanceCertificateDTO Clscatag)
        {
            return _IStatus.getAdm_Name(Clscatag);
        }

        
    }
}
