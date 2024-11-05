using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.IVRM
{
    public class Interaction_Delete_Report_Delegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Interaction_Delete_Report_DTO, Interaction_Delete_Report_DTO> COMMMw = new CommonDelegate<Interaction_Delete_Report_DTO, Interaction_Delete_Report_DTO>();
        public Interaction_Delete_Report_DTO getreport(Interaction_Delete_Report_DTO dto)
        {
            return COMMMw.POSTPORTALData(dto, "Interaction_Delete_Facade/getreport/");
           
        }
        public Interaction_Delete_Report_DTO getintreport(Interaction_Delete_Report_DTO dto)
        {
            return COMMMw.POSTPORTALData(dto, "Interaction_Delete_Facade/getintreport/");
           
        }

        public Interaction_Delete_Report_DTO loadreportdata(Interaction_Delete_Report_DTO dto)
        {
            return COMMMw.POSTPORTALData(dto, "Interaction_Delete_Facade/loadreportdata/");
           
        }
        //===================mobile app download
        public Interaction_Delete_Report_DTO mobload(Interaction_Delete_Report_DTO dto)
        {
            return COMMMw.POSTPORTALData(dto, "Interaction_Delete_Facade/mobload/");
           
        }
         public Interaction_Delete_Report_DTO mobreport(Interaction_Delete_Report_DTO dto)
        {
            return COMMMw.POSTPORTALData(dto, "Interaction_Delete_Facade/mobreport/");
           
        }

    }
}
