using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface ClientInvoiceInterface
    {
        ClientInvoiceDTO loaddata(ClientInvoiceDTO data);
        ClientInvoiceDTO savedata(ClientInvoiceDTO data);
        ClientInvoiceDTO Editdata(ClientInvoiceDTO data);
        ClientInvoiceDTO getinvoiceno(ClientInvoiceDTO data);
        ClientInvoiceDTO clientDecative(ClientInvoiceDTO data);
        ClientInvoiceDTO getbom(ClientInvoiceDTO data);
        ClientInvoiceDTO companychange(ClientInvoiceDTO data);
        ClientInvoiceDTO getProject(ClientInvoiceDTO data);
        ClientInvoiceDTO viewdetails(ClientInvoiceDTO data);
        ClientInvoiceDTO savepaymentdetailsrecord(ClientInvoiceDTO data);
        ClientInvoiceDTO loaddataconfig(ClientInvoiceDTO data);
        ClientInvoiceDTO savedataconfig(ClientInvoiceDTO data);
        ClientInvoiceDTO loaddatareport(ClientInvoiceDTO data);
        ClientInvoiceDTO getreport(ClientInvoiceDTO data);
        ClientInvoiceDTO paymentnotification(ClientInvoiceDTO data);
        //ClientInvoiceDTO getYear(ClientInvoiceDTO data);
    }
}
