using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class SalesLeadImportDelegate
    {
    
        CommonVMSDelegate<SalesLeadImportDTO, SalesLeadImportDTO> COMMLD = new CommonVMSDelegate<SalesLeadImportDTO, SalesLeadImportDTO>();

       
       
        public SalesLeadImportDTO saveadvance(SalesLeadImportDTO dto)
        {
            return COMMLD.POSTData(dto, "SalesLeadImportFacade/saveadvance/");
        }
    }
}
