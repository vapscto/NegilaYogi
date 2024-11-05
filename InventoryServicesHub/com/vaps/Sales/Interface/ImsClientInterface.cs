using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface ImsClientInterface
    {
        Clients_DTO getdetails(Clients_DTO data);
        Clients_DTO OnChangeTab1Inst(Clients_DTO data);
        Clients_DTO saveClientdata(Clients_DTO data);
        Clients_DTO clientDecative(Clients_DTO data);
        Clients_DTO editClientdata(Clients_DTO data);
        Clients_DTO OnChangeTab2Inst(Clients_DTO data);
        Clients_DTO saveClientMappingdata(Clients_DTO data);
        Clients_DTO editClientMappingdata(Clients_DTO data);
        Clients_DTO deactiveClientMappingdata(Clients_DTO data);
        Clients_DTO modalListdata(Clients_DTO data);

        //VMS Client And IVRM Client Mapping
        Clients_DTO OnChangeClientTab3(Clients_DTO data);
        Clients_DTO SaveVMSIVRMMapping(Clients_DTO data);
    }
}
