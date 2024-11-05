using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class HomeworkUploadDelegate
    {
        CommonDelegate<HomeWorkUploadDTO, HomeWorkUploadDTO> comm = new CommonDelegate<HomeWorkUploadDTO, HomeWorkUploadDTO>();

        public HomeWorkUploadDTO Getdata_class(HomeWorkUploadDTO dto)
        {
            return comm.POSTPORTALData(dto, "HomeworkUploadFacade/Getdata_class/");
        }
      
        public HomeWorkUploadDTO getreport_class(HomeWorkUploadDTO dto)
        {
            return comm.POSTPORTALData(dto, "HomeworkUploadFacade/getreport_class/");
        }
        public HomeWorkUploadDTO getreport_home(HomeWorkUploadDTO dto)
        {
            return comm.POSTPORTALData(dto, "HomeworkUploadFacade/getreport_home/");
        }
      
        public HomeWorkUploadDTO getreport_notice(HomeWorkUploadDTO dto)
        {
            return comm.POSTPORTALData(dto, "HomeworkUploadFacade/getreport_notice/");
        }

        public HomeWorkUploadDTO Getdataview(HomeWorkUploadDTO dto)
        {
            return comm.POSTPORTALData(dto, "HomeworkUploadFacade/Getdataview/");
        }

        public HomeWorkUploadDTO getsection(HomeWorkUploadDTO dto)
        {
            return comm.POSTPORTALData(dto, "HomeworkUploadFacade/getsection/");
        }

        public HomeWorkUploadDTO getseenreport(HomeWorkUploadDTO dto)
        {
            return comm.POSTPORTALData(dto, "HomeworkUploadFacade/getseenreport/");
        }

        public HomeWorkUploadDTO Getdataview_seen(HomeWorkUploadDTO dto)
        {
            return comm.POSTPORTALData(dto, "HomeworkUploadFacade/Getdataview_seen/");
        }

        public HomeWorkUploadDTO viewData(HomeWorkUploadDTO data)
        {
            return comm.POSTPORTALData(data, "HomeworkUploadFacade/viewData/");
        }
    }
}
