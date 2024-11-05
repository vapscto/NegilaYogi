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
    [Route("api/ClientInvoice")]
    public class ClientInvoiceController : Controller
    {
        ClientInvoiceDelegate del = new ClientInvoiceDelegate();

        [Route("loaddata/{id:int}")]
        public ClientInvoiceDTO loaddata(int id)
        {
            ClientInvoiceDTO data = new ClientInvoiceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }

        [Route("savedata")]
        public ClientInvoiceDTO savedata([FromBody]ClientInvoiceDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.savedata(data);
        }

        [Route("EditData")]
        public ClientInvoiceDTO Editdata([FromBody] ClientInvoiceDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("clientDecative")]
        public ClientInvoiceDTO clientDecative([FromBody]ClientInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.clientDecative(data);
        }

        [Route("getProject")]
        public ClientInvoiceDTO getProject([FromBody]ClientInvoiceDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getProject(data);
        }

        [Route("getbom")]
        public ClientInvoiceDTO getbom([FromBody]ClientInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getbom(data);
        }
        [Route("companychange")]
        public ClientInvoiceDTO companychange([FromBody]ClientInvoiceDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.companychange(data);
        }

        [Route("viewdetails")]
        public ClientInvoiceDTO viewdetails([FromBody]ClientInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdetails(data);
        }

        [Route("getinvoiceno")]
        public ClientInvoiceDTO getinvoiceno([FromBody]ClientInvoiceDTO data)
        {

            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getinvoiceno(data);
        }

        [Route("savepaymentdetailsrecord")]
        public ClientInvoiceDTO savepaymentdetailsrecord([FromBody]ClientInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savepaymentdetailsrecord(data);
        }

        // Configuration
        [Route("loaddataconfig/{id:int}")]
        public ClientInvoiceDTO loaddataconfig(int id)
        {
            ClientInvoiceDTO data = new ClientInvoiceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public ClientInvoiceDTO savedataconfig([FromBody]ClientInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport/{id:int}")]
        public ClientInvoiceDTO loaddatareport(int id)
        {
            ClientInvoiceDTO data = new ClientInvoiceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddatareport(data);
        }

        [Route("getreport")]
        public ClientInvoiceDTO getreport([FromBody]ClientInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getreport(data);
        }
        //paymentnotification
        [Route("paymentnotification")]
        public ClientInvoiceDTO paymentnotification([FromBody]ClientInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.paymentnotification(data);
        }
    }
}