using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class INV_ProductReportFacadeController : Controller
    {
        public INV_ProductReportInterface _pdamasterhead;

        public INV_ProductReportFacadeController(INV_ProductReportInterface maspag)
        {
            _pdamasterhead = maspag;
        }

        [Route("getalldetails")]
        public INV_Master_ProductDTO getalldetails([FromBody]INV_Master_ProductDTO data)
        {
            return _pdamasterhead.getalldetails(data);
        }

        [Route("getdata")]
        public INV_Master_ProductDTO getdata([FromBody]INV_Master_ProductDTO data)
        {
            return _pdamasterhead.getdata(data);
        }

      
        [Route("radiobtndata")]
        public Task<INV_Master_ProductDTO> radiobtndata([FromBody]INV_Master_ProductDTO data)
        {
            return _pdamasterhead.radiobtndata(data);
        }

    }
}
