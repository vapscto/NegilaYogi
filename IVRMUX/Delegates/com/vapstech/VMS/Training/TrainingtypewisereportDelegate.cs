using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class TrainingtypewisereportDelegate
    {
        CommonDelegate<TrainingtypewisereportDTO, TrainingtypewisereportDTO> _com = new CommonDelegate<TrainingtypewisereportDTO, TrainingtypewisereportDTO>();

        public TrainingtypewisereportDTO onloaddata(TrainingtypewisereportDTO data)
        {
            return _com.POSTVMS(data, "TrainingtypewisereportFacade/onloaddata");
        }
        public TrainingtypewisereportDTO getreport(TrainingtypewisereportDTO data)
        {
            return _com.POSTVMS(data, "TrainingtypewisereportFacade/getreport");
        }

    }
}


