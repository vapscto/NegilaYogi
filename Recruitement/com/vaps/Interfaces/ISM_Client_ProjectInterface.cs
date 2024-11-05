using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface ISM_Client_ProjectInterface
    {
        //client Components module
        ISM_Client_Project_DTO getdate_cmc(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_cmc(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_cmc(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_cmc(ISM_Client_Project_DTO dto);

        //========================BOM
        ISM_Client_Project_DTO getdata_BOM(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_BOM(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_BOM(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_BOM(ISM_Client_Project_DTO dto); 
        
        //========================Man power
        ISM_Client_Project_DTO getdata_MP(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_MP(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_MP(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_MP(ISM_Client_Project_DTO dto);

         //======================== DOC
        ISM_Client_Project_DTO getdata_DOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_DOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_DOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_DOC(ISM_Client_Project_DTO dto);
         //======================== MASTER DOC
        ISM_Client_Project_DTO getdata_MDOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_MDOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_MDOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_MDOC(ISM_Client_Project_DTO dto);


    }
}
