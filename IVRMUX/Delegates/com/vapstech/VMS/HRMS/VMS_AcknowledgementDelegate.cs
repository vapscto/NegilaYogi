using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.HRMS
{
    public class VMS_AcknowledgementDelegate
    {


        CommonDelegate<HR_VMS_AcknowledgementDTO, HR_VMS_AcknowledgementDTO> comm = new CommonDelegate<HR_VMS_AcknowledgementDTO, HR_VMS_AcknowledgementDTO>();
        public HR_VMS_AcknowledgementDTO loaddata(HR_VMS_AcknowledgementDTO data)
        {
            return comm.POSTVMS(data, "VMS_AcknowledgementFacade/loaddata");
        }
    }
}
