using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class ReligionCasteCategoryReportDelegate
    {
        CommonDelegate<ReligionCasteCategoryReport_DTO, ReligionCasteCategoryReport_DTO> comm = new CommonDelegate<ReligionCasteCategoryReport_DTO, ReligionCasteCategoryReport_DTO>();
        public ReligionCasteCategoryReport_DTO loaddata(ReligionCasteCategoryReport_DTO data)
        {
            return comm.POSTDataADM(data, "ReligionCasteCategoryReportFacade/loaddata");
        }
        public ReligionCasteCategoryReport_DTO showdetails(ReligionCasteCategoryReport_DTO data)
        {
            return comm.POSTDataADM(data, "ReligionCasteCategoryReportFacade/showdetails");
        }
    }
}
