using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterHirer_Group_Rate_Delegate
    {
        CommonDelegate<MasterHirer_Group_RateDTO, MasterHirer_Group_RateDTO> _com = new CommonDelegate<MasterHirer_Group_RateDTO, MasterHirer_Group_RateDTO>();

        //Master Hirer Group.
        public MasterHirer_Group_RateDTO load(MasterHirer_Group_RateDTO data)
        {
            return _com.POSTDataTransport(data, "MasterHirer_Group_Rate_Facade/getdata/");
        }
        public MasterHirer_Group_RateDTO save(MasterHirer_Group_RateDTO data)
        {
            return _com.POSTDataTransport(data, "MasterHirer_Group_Rate_Facade/save/");
        }
        public MasterHirer_Group_RateDTO edit(int id)
        {
            return _com.GetDataByIdTransport(id, "MasterHirer_Group_Rate_Facade/edit/");
        }
        public MasterHirer_Group_RateDTO deactivate(MasterHirer_Group_RateDTO dto)
        {
            return _com.POSTDataTransport(dto, "MasterHirer_Group_Rate_Facade/deactivate/");
        }
        //Master Hirer Rate.
        public MasterHirer_Group_RateDTO loadRateData(MasterHirer_Group_RateDTO data)
        {
            return _com.POSTDataTransport(data, "MasterHirer_Group_Rate_Facade/loadRateData/");
        }
        public MasterHirer_Group_RateDTO saveRate(MasterHirer_Group_RateDTO data)
        {
            return _com.POSTDataTransport(data, "MasterHirer_Group_Rate_Facade/saveRate/");
        }
        public MasterHirer_Group_RateDTO EditRate(int id)
        {
            return _com.GetDataByIdTransport(id, "MasterHirer_Group_Rate_Facade/EditRate/");
        }
        //Master Hirer.
        public MasterHirer_Group_RateDTO loadHirerData(MasterHirer_Group_RateDTO data)
        {
            return _com.POSTDataTransport(data, "MasterHirer_Group_Rate_Facade/loadHirerData/");
        }
      
        public MasterHirer_Group_RateDTO saveHirer(MasterHirer_Group_RateDTO data)
        {
            return _com.POSTDataTransport(data, "MasterHirer_Group_Rate_Facade/saveHirer/");
        }
        public MasterHirer_Group_RateDTO EditHirer(int id)
        {
            return _com.GetDataByIdTransport(id, "MasterHirer_Group_Rate_Facade/EditHirer/");
        }
        public MasterHirer_Group_RateDTO deactivateHirer(MasterHirer_Group_RateDTO dto)
        {
            return _com.POSTDataTransport(dto, "MasterHirer_Group_Rate_Facade/deactivateHirer/");
        }


    }
}
