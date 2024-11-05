using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Sales
{
    public class ClientInvoiceDelegate
    {
        CommonDelegate<ClientInvoiceDTO, ClientInvoiceDTO> comm = new CommonDelegate<ClientInvoiceDTO, ClientInvoiceDTO>();

        public ClientInvoiceDTO loaddata(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/loaddata");
        }
        public ClientInvoiceDTO savedata(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/savedata");
        }
        public ClientInvoiceDTO Editdata(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/Editdata");
        }
        public ClientInvoiceDTO clientDecative(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/clientDecative");
        }
        public ClientInvoiceDTO getProject(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/getProject");
        }
        public ClientInvoiceDTO getbom(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/getbom");
        }
        public ClientInvoiceDTO companychange(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/companychange");
        }
        public ClientInvoiceDTO viewdetails(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/viewdetails");
        }
        public ClientInvoiceDTO getinvoiceno(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/getinvoiceno");
        }
        public ClientInvoiceDTO savepaymentdetailsrecord(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/savepaymentdetailsrecord");
        }
        public ClientInvoiceDTO loaddataconfig(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/loaddataconfig");
        }
        public ClientInvoiceDTO savedataconfig(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/savedataconfig");
        }
        public ClientInvoiceDTO loaddatareport(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/loaddatareport");
        }
        public ClientInvoiceDTO getreport(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/getreport");
        }
        public ClientInvoiceDTO paymentnotification(ClientInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientInvoiceFacade/paymentnotification");
        }
    }
}
