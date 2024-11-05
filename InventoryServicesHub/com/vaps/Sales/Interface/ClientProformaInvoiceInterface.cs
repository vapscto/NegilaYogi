using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface ClientProformaInvoiceInterface
    {
        ClientProformaInvoiceDTO loaddata(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO savedata(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO Editdata(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO companychange(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO getinvoiceno(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO clientDecative(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO getbom(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO getProject(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO viewdetails(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO savepaymentdetailsrecord(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO loaddataconfig(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO savedataconfig(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO loaddatareport(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO getreport(ClientProformaInvoiceDTO data);
        ClientProformaInvoiceDTO paymentnotification(ClientProformaInvoiceDTO data);
        //ClientProformaInvoiceDTO getYear(ClientProformaInvoiceDTO data);
    }
}
