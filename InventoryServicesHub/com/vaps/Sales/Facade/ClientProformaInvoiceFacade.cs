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
    public class ClientProformaInvoiceFacade : Controller
    {
        public ClientProformaInvoiceInterface inter;
        public ClientProformaInvoiceFacade(ClientProformaInvoiceInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public ClientProformaInvoiceDTO loaddata([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public ClientProformaInvoiceDTO savedata([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.savedata(data);
        }
        [Route("EditData")]
        public ClientProformaInvoiceDTO Editdata([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.Editdata(data);
        }
        [Route("companychange")]
        public ClientProformaInvoiceDTO companychange([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.companychange(data);
        }
        [Route("getinvoiceno")]
        public ClientProformaInvoiceDTO getinvoiceno([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.getinvoiceno(data);
        }

        [Route("clientDecative")]
        public ClientProformaInvoiceDTO clientDecative([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.clientDecative(data);
        }
        [Route("getProject")]
        public ClientProformaInvoiceDTO getProject([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.getProject(data);
        }
        [Route("getbom")]
        public ClientProformaInvoiceDTO getbom([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.getbom(data);
        }
        [Route("viewdetails")]
        public ClientProformaInvoiceDTO viewdetails([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.viewdetails(data);
        }
        [Route("savepaymentdetailsrecord")]
        public ClientProformaInvoiceDTO savepaymentdetailsrecord([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.savepaymentdetailsrecord(data);
        }

        // Payment Configuration
        [Route("loaddataconfig")]
        public ClientProformaInvoiceDTO loaddataconfig([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public ClientProformaInvoiceDTO savedataconfig([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport")]
        public ClientProformaInvoiceDTO loaddatareport([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.loaddatareport(data);
        }

        [Route("getreport")]
        public ClientProformaInvoiceDTO getreport([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.getreport(data);
        }

        // Notification Details
        [Route("paymentnotification")]
        public ClientProformaInvoiceDTO paymentnotification([FromBody] ClientProformaInvoiceDTO data)
        {
            return inter.paymentnotification(data);
        }

        //[Route("getYear")]
        //public ClientProformaInvoiceDTO getYear([FromBody] ClientProformaInvoiceDTO data)
        //{
        //    return inter.getYear(data);
        //}
    }
}
