using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public interface MasterHire_Group_RateInterface
    {
        //Master Hirer Group.
        MasterHirer_Group_RateDTO getdata(MasterHirer_Group_RateDTO obj);
        MasterHirer_Group_RateDTO save(MasterHirer_Group_RateDTO data);
        MasterHirer_Group_RateDTO edit(int id);
        MasterHirer_Group_RateDTO deactivate(MasterHirer_Group_RateDTO dto);
        //Master Hirer Rate.
        MasterHirer_Group_RateDTO getRatedata(MasterHirer_Group_RateDTO obj);
        MasterHirer_Group_RateDTO saveRate(MasterHirer_Group_RateDTO data);
        MasterHirer_Group_RateDTO editRate(int id);
        //Master Hirer.
        MasterHirer_Group_RateDTO loadHirerData(MasterHirer_Group_RateDTO obj);
        MasterHirer_Group_RateDTO saveHirer(MasterHirer_Group_RateDTO data);
        MasterHirer_Group_RateDTO EditHirer(int id);
        MasterHirer_Group_RateDTO deactivateHirer(MasterHirer_Group_RateDTO dto);

    }
}
