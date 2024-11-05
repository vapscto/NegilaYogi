using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Sales
{
    [Produces("application/json")]
    [Route("api/ClientProformaInvoice")]
    public class ClientProformaInvoiceController : Controller
    {
        ClientProformaInvoiceDelegate del = new ClientProformaInvoiceDelegate();

        [Route("loaddata/{id:int}")]
        public ClientProformaInvoiceDTO loaddata(int id)
        {
            ClientProformaInvoiceDTO data = new ClientProformaInvoiceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }

        [Route("savedata")]
        public ClientProformaInvoiceDTO savedata([FromBody]ClientProformaInvoiceDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.savedata(data);
        }

        [Route("EditData")]
        public ClientProformaInvoiceDTO Editdata([FromBody] ClientProformaInvoiceDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("companychange")]
        public ClientProformaInvoiceDTO companychange([FromBody] ClientProformaInvoiceDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.companychange(data);
        }
        [Route("clientDecative")]
        public ClientProformaInvoiceDTO clientDecative([FromBody]ClientProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.clientDecative(data);
        }

        [Route("getProject")]
        public ClientProformaInvoiceDTO getProject([FromBody]ClientProformaInvoiceDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getProject(data);
        }

        [Route("getbom")]
        public ClientProformaInvoiceDTO getbom([FromBody]ClientProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getbom(data);
        }

        [Route("viewdetails")]
        public ClientProformaInvoiceDTO viewdetails([FromBody]ClientProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdetails(data);
        }
        [Route("getinvoiceno")]
        public ClientProformaInvoiceDTO getinvoiceno([FromBody]ClientProformaInvoiceDTO data)
        {

            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getinvoiceno(data);
        }

        [Route("savepaymentdetailsrecord")]
        public ClientProformaInvoiceDTO savepaymentdetailsrecord([FromBody]ClientProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savepaymentdetailsrecord(data);
        }

        // Configuration
        [Route("loaddataconfig/{id:int}")]
        public ClientProformaInvoiceDTO loaddataconfig(int id)
        {
            ClientProformaInvoiceDTO data = new ClientProformaInvoiceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public ClientProformaInvoiceDTO savedataconfig([FromBody]ClientProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport/{id:int}")]
        public ClientProformaInvoiceDTO loaddatareport(int id)
        {
            ClientProformaInvoiceDTO data = new ClientProformaInvoiceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddatareport(data);
        }

        [Route("getreport")]
        public ClientProformaInvoiceDTO getreport([FromBody]ClientProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getreport(data);
        }
    }
}