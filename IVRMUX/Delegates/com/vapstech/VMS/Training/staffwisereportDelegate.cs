using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class staffwisereportDelegate
    {

        CommonDelegate<staffwisereportDTO, staffwisereportDTO> _com = new CommonDelegate<staffwisereportDTO, staffwisereportDTO>();

        public staffwisereportDTO onloaddata(staffwisereportDTO data)
        {
            return _com.POSTVMS(data, "staffwisereportFacade/onloaddata");
        }
        public staffwisereportDTO getreport(staffwisereportDTO data)
        {
            return _com.POSTVMS(data, "staffwisereportFacade/getreport");
        }
    }
}
