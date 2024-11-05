using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Sales.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Sales.Facade
{
    [Route("api/[controller]")]
    public class ClientInvoiceFacade : Controller
    {
        public ClientInvoiceInterface inter;
        public ClientInvoiceFacade(ClientInvoiceInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public ClientInvoiceDTO loaddata([FromBody] ClientInvoiceDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public ClientInvoiceDTO savedata([FromBody] ClientInvoiceDTO data)
        {
            return inter.savedata(data);
        }
        [Route("EditData")]
        public ClientInvoiceDTO Editdata([FromBody] ClientInvoiceDTO data)
        {
            return inter.Editdata(data);
        }
        [Route("getinvoiceno")]
        public ClientInvoiceDTO getinvoiceno([FromBody] ClientInvoiceDTO data)
        {
            return inter.getinvoiceno(data);
        }

        [Route("clientDecative")]
        public ClientInvoiceDTO clientDecative([FromBody] ClientInvoiceDTO data)
        {
            return inter.clientDecative(data);
        }
        [Route("getProject")]
        public ClientInvoiceDTO getProject([FromBody] ClientInvoiceDTO data)
        {
            return inter.getProject(data);
        }
        [Route("getbom")]
        public ClientInvoiceDTO getbom([FromBody] ClientInvoiceDTO data)
        {
            return inter.getbom(data);
        }
        [Route("companychange")]
        public ClientInvoiceDTO companychange([FromBody] ClientInvoiceDTO data)
        {
            return inter.companychange(data);
        }
        [Route("viewdetails")]
        public ClientInvoiceDTO viewdetails([FromBody] ClientInvoiceDTO data)
        {
            return inter.viewdetails(data);
        }
        [Route("savepaymentdetailsrecord")]
        public ClientInvoiceDTO savepaymentdetailsrecord([FromBody] ClientInvoiceDTO data)
        {
            return inter.savepaymentdetailsrecord(data);
        }

        // Payment Configuration
        [Route("loaddataconfig")]
        public ClientInvoiceDTO loaddataconfig([FromBody] ClientInvoiceDTO data)
        {
            return inter.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public ClientInvoiceDTO savedataconfig([FromBody] ClientInvoiceDTO data)
        {
            return inter.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport")]
        public ClientInvoiceDTO loaddatareport([FromBody] ClientInvoiceDTO data)
        {
            return inter.loaddatareport(data);
        }

        [Route("getreport")]
        public ClientInvoiceDTO getreport([FromBody] ClientInvoiceDTO data)
        {
            return inter.getreport(data);
        }

        // Notification Details
        [Route("paymentnotification")]
        public ClientInvoiceDTO paymentnotification([FromBody] ClientInvoiceDTO data)
        {
            return inter.paymentnotification(data);
        }

        //[Route("getYear")]
        //public ClientInvoiceDTO getYear([FromBody] ClientInvoiceDTO data)
        //{
        //    return inter.getYear(data);
        //}
    }
}
