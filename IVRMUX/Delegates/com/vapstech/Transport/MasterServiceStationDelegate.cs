using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterServiceStationDelegate
    {
        CommonDelegate<ServiceStationDTO, ServiceStationDTO> _com = new CommonDelegate<ServiceStationDTO, ServiceStationDTO>();

        //Report.
        public ServiceStationDTO getrptparam(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/getrptparam/");
        }
        public ServiceStationDTO Getreportdetails(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/Getreportdetails/");
        }
        //Bill

        public ServiceStationDTO Servicebillload(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/Servicebillload/");
        }
        public ServiceStationDTO get_srvdetails(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/get_srvdetails/");
        }
        public ServiceStationDTO saveBilldata(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/saveBilldata/");
        }
          public ServiceStationDTO PayBill(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/PayBill/");
        }
        public ServiceStationDTO FinalPayBill(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/FinalPayBill/");
        }
        public ServiceStationDTO findservice(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/findservice/");
        }
        public ServiceStationDTO duprecpcheck(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/duprecpcheck/");
        }
        public ServiceStationDTO delete_rec(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/delete_rec/");
        }

        //Bill End



        //Master parts.
        public ServiceStationDTO getpartsdata(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/getpartsdata/");
        }
        public ServiceStationDTO savepartsdata(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/savepartsdata/");
        }
        public ServiceStationDTO EditRate(int id)
        {
            return _com.GetDataByIdTransport(id, "MasterServiceStationFacade/EditRate/");
        }
        public ServiceStationDTO activedeactiveparts(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/activedeactiveparts/");
        }
        public ServiceStationDTO viewreq(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/viewreq/");
        }
        public ServiceStationDTO getbillreport(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/getbillreport/");
        }
        public ServiceStationDTO editpartsdata(int id)
        {
            return _com.GetDataByIdTransport(id, "MasterServiceStationFacade/editpartsdata/");
        }

      
        //Master Hirer.
        public ServiceStationDTO loadservicestation(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/loadservicestation/");
        }
      
        public ServiceStationDTO savestation(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/savestation/");
        }
        public ServiceStationDTO Editstation(int id)
        {
            return _com.GetDataByIdTransport(id, "MasterServiceStationFacade/Editstation/");
        }
        public ServiceStationDTO viewitems(int id)
        {
            return _com.GetDataByIdTransport(id, "MasterServiceStationFacade/viewitems/");
        }
        public ServiceStationDTO deactivatestation(ServiceStationDTO dto)
        {
            return _com.POSTDataTransport(dto, "MasterServiceStationFacade/deactivatestation/");
        }

        //Master Parttype.
        public ServiceStationDTO loadparttype(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/loadparttype/");
        }

        public ServiceStationDTO saveparttype(ServiceStationDTO data)
        {
            return _com.POSTDataTransport(data, "MasterServiceStationFacade/saveparttype/");
        }
        public ServiceStationDTO Editparttype(int id)
        {
            return _com.GetDataByIdTransport(id, "MasterServiceStationFacade/Editparttype/");
        }
        public ServiceStationDTO deactivateparttype(ServiceStationDTO dto)
        {
            return _com.POSTDataTransport(dto, "MasterServiceStationFacade/deactivateparttype/");
        }



    }
}
