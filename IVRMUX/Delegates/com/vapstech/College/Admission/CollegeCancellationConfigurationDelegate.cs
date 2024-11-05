using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeCancellationConfigurationDelegate
    {
        CommonDelegate<CollegeCancellationConfigurationDTO, CollegeCancellationConfigurationDTO> _delg = new CommonDelegate<CollegeCancellationConfigurationDTO, CollegeCancellationConfigurationDTO>();
        public CollegeCancellationConfigurationDTO getdata(CollegeCancellationConfigurationDTO data)
        {
            return _delg.clgadmissionbypost(data, "CollegeCancellationConfigurationFacade/getdata/");
        }
        public CollegeCancellationConfigurationDTO saveconfig(CollegeCancellationConfigurationDTO data)
        {
            return _delg.clgadmissionbypost(data, "CollegeCancellationConfigurationFacade/saveconfig/");
        }
        public CollegeCancellationConfigurationDTO editconfig(CollegeCancellationConfigurationDTO data)
        {
            return _delg.clgadmissionbypost(data, "CollegeCancellationConfigurationFacade/editconfig/");
        }
        public CollegeCancellationConfigurationDTO activedeactive(CollegeCancellationConfigurationDTO data)
        {
            return _delg.clgadmissionbypost(data, "CollegeCancellationConfigurationFacade/activedeactive/");
        }
      
    }
}
