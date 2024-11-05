using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Sales
{
    public class ClientProformaInvoiceDelegate
    {
        CommonDelegate<ClientProformaInvoiceDTO, ClientProformaInvoiceDTO> comm = new CommonDelegate<ClientProformaInvoiceDTO, ClientProformaInvoiceDTO>();

        public ClientProformaInvoiceDTO loaddata(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/loaddata");
        }
        public ClientProformaInvoiceDTO savedata(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/savedata");
        }
        public ClientProformaInvoiceDTO Editdata(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/Editdata");
        }
        public ClientProformaInvoiceDTO companychange(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/companychange");
        }
        public ClientProformaInvoiceDTO clientDecative(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/clientDecative");
        }
        public ClientProformaInvoiceDTO getProject(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/getProject");
        }
        public ClientProformaInvoiceDTO getbom(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/getbom");
        }
        public ClientProformaInvoiceDTO viewdetails(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/viewdetails");
        }
        public ClientProformaInvoiceDTO getinvoiceno(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/getinvoiceno");
        }
        public ClientProformaInvoiceDTO savepaymentdetailsrecord(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/savepaymentdetailsrecord");
        }
        public ClientProformaInvoiceDTO loaddataconfig(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/loaddataconfig");
        }
        public ClientProformaInvoiceDTO savedataconfig(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/savedataconfig");
        }
        public ClientProformaInvoiceDTO loaddatareport(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/loaddatareport");
        }
        public ClientProformaInvoiceDTO getreport(ClientProformaInvoiceDTO data)
        {
            return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/getreport");
        }
        //public ClientProformaInvoiceDTO getYear(ClientProformaInvoiceDTO data)
        //{
        //    return comm.POSTDataInventory(data, "ClientProformaInvoiceFacade/getYear");
        //}
    }
}
