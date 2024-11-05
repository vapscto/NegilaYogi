using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public interface MasterServiceStationInterface
    {
        //Master Hirer Group.
        ServiceStationDTO getrptparam(ServiceStationDTO obj);
        ServiceStationDTO Getreportdetails(ServiceStationDTO data);
      
        //Master Parts.
        ServiceStationDTO Servicebillload(ServiceStationDTO obj);
        ServiceStationDTO get_srvdetails(ServiceStationDTO obj);
        ServiceStationDTO saveBilldata(ServiceStationDTO obj);
        ServiceStationDTO PayBill(ServiceStationDTO obj);
        ServiceStationDTO FinalPayBill(ServiceStationDTO obj);
        ServiceStationDTO findservice(ServiceStationDTO obj);
        ServiceStationDTO duprecpcheck(ServiceStationDTO obj);
        ServiceStationDTO viewreq(ServiceStationDTO obj);
        ServiceStationDTO getbillreport(ServiceStationDTO obj);
        ServiceStationDTO delete_rec(ServiceStationDTO obj);
        ServiceStationDTO getpartsdata(ServiceStationDTO obj);
        ServiceStationDTO savepartsdata(ServiceStationDTO data);
     
        ServiceStationDTO activedeactiveparts(ServiceStationDTO data);
        ServiceStationDTO editpartsdata(int id);
        ServiceStationDTO viewitems(int id);
     
        //Master station
        ServiceStationDTO loadservicestation(ServiceStationDTO obj);
        ServiceStationDTO savestation(ServiceStationDTO data);
        ServiceStationDTO Editstation(int id);
        ServiceStationDTO deactivatestation(ServiceStationDTO dto);
        //Master parttype
        ServiceStationDTO loadparttype(ServiceStationDTO obj);
        ServiceStationDTO saveparttype(ServiceStationDTO data);
        ServiceStationDTO Editparttype(int id);
        ServiceStationDTO deactivateparttype(ServiceStationDTO dto);

    }
}
