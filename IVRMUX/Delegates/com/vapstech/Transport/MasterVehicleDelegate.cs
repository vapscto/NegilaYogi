using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterVehicleDelegate
    {
        CommonDelegate<MasterVehicleDTO, MasterVehicleDTO> _comm = new CommonDelegate<MasterVehicleDTO, MasterVehicleDTO>();
        public MasterVehicleDTO getdata(int id)
        {
            return _comm.GetDataByIdTransport(id, "MasterVehicleFacade/getdata/");
        }
        public MasterVehicleDTO savedata(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/savedata/");
        }
        public MasterVehicleDTO edit(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/edit/");
        }
        public MasterVehicleDTO activedeactive(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/activedeactive/");
        }
        public MasterVehicleDTO validaevehicleno(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/validaevehicleno/");
        }
        public MasterVehicleDTO rcreport(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/rcreport/");
        }
        public MasterVehicleDTO validaevhassiseno(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/validaevhassiseno/");
        }
        public MasterVehicleDTO validaeengineno(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/validaeengineno/");
        }

        public MasterVehicleDTO viewuploadflies(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/viewuploadflies/");
        }

        public MasterVehicleDTO deleteuploadfile(MasterVehicleDTO id)
        {
            return _comm.POSTDataTransport(id, "MasterVehicleFacade/deleteuploadfile/");
        }
    }
}
