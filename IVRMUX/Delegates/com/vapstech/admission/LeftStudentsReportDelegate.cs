using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class LeftStudentsReportDelegate
    {
        CommonDelegate<LeftStudentsReportDTO, LeftStudentsReportDTO> _comm = new CommonDelegate<LeftStudentsReportDTO, LeftStudentsReportDTO>();
        public LeftStudentsReportDTO loaddata(LeftStudentsReportDTO data)
        {
            return _comm.POSTDataADM(data, "LeftStudentsReportFacade/loaddata/");
        }
        //getCategory
        public LeftStudentsReportDTO getCategory(LeftStudentsReportDTO data)
        {
            return _comm.POSTDataADM(data, "LeftStudentsReportFacade/getCategory/");
        }
        //getClass
        public LeftStudentsReportDTO getClass(LeftStudentsReportDTO data)
        {
            return _comm.POSTDataADM(data, "LeftStudentsReportFacade/getClass/");
        }
        //getsection
        public LeftStudentsReportDTO getsection(LeftStudentsReportDTO data)
        {
            return _comm.POSTDataADM(data, "LeftStudentsReportFacade/getsection/");
        }
        //report
        public LeftStudentsReportDTO report(LeftStudentsReportDTO data)
        {
            return _comm.POSTDataADM(data, "LeftStudentsReportFacade/report/");
        }
    }
}
