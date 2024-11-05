using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.VMS.Training;
namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class CovidTestUploadDelegate
    {
        CommonDelegate<CovidTestUploadDTO, CovidTestUploadDTO> _com = new CommonDelegate<CovidTestUploadDTO, CovidTestUploadDTO>();

        public CovidTestUploadDTO onloaddata(CovidTestUploadDTO data)
        {
            return _com.POSTPORTALData(data, "CovidTestUploadFacade/onloaddata");
        }
        public CovidTestUploadDTO saverecord(CovidTestUploadDTO data)
        {
            return _com.POSTPORTALData(data, "CovidTestUploadFacade/saverecord");
        }
        public CovidTestUploadDTO deactiveY(CovidTestUploadDTO data)
        {
            return _com.POSTPORTALData(data, "CovidTestUploadFacade/deactiveY");
        }
    }
}
