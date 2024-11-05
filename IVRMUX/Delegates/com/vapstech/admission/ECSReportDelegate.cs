using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class ECSReportDelegate
    {
        CommonLibrary.CommonDelegate<ECSReportDTO, ECSReportDTO> _comm = new CommonLibrary.CommonDelegate<ECSReportDTO, ECSReportDTO>();

        public ECSReportDTO getloaddata (ECSReportDTO data)
        {
            return _comm.POSTDataADM(data, "ECSReportFacade/getloaddata");
        }
        public ECSReportDTO getclass(ECSReportDTO data)
        {
            return _comm.POSTDataADM(data, "ECSReportFacade/getclass");
        }
        public ECSReportDTO getsection(ECSReportDTO data)
        {
            return _comm.POSTDataADM(data, "ECSReportFacade/getsection");
        }
        public ECSReportDTO getreport(ECSReportDTO data)
        {
            return _comm.POSTDataADM(data, "ECSReportFacade/getreport");
        }
        public ECSReportDTO showecsdetails(ECSReportDTO data)
        {
            return _comm.POSTDataADM(data, "ECSReportFacade/showecsdetails");
        }
        public ECSReportDTO searchByColumn(ECSReportDTO data)
        {
            return _comm.POSTDataADM(data, "ECSReportFacade/searchByColumn");
        }
    }
}
