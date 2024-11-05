using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class ISM_Client_ProjectDelegate
    {
        CommonVMSDelegate<ISM_Client_Project_DTO, ISM_Client_Project_DTO> CMMICP = new CommonVMSDelegate<ISM_Client_Project_DTO, ISM_Client_Project_DTO>();

        public ISM_Client_Project_DTO getdate_cmc(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdate_cmc/");
        }
        public ISM_Client_Project_DTO details_cmc(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_cmc/");
        }
         public ISM_Client_Project_DTO SaveEdit_cmc(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_cmc/");
        }
         public ISM_Client_Project_DTO deactivate_cmc(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_cmc/");
        }

        //==================BOM

        public ISM_Client_Project_DTO getdata_BOM(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_BOM/");
        }

        public ISM_Client_Project_DTO details_BOM(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_BOM/");
        }
        public ISM_Client_Project_DTO SaveEdit_BOM(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_BOM/");
        }
        public ISM_Client_Project_DTO deactivate_BOM(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_BOM/");
        }
        
        //==================Man power

        public ISM_Client_Project_DTO getdata_MP(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_MP/");
        }

        public ISM_Client_Project_DTO details_MP(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_MP/");
        }
        public ISM_Client_Project_DTO SaveEdit_MP(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_MP/");
        }
        public ISM_Client_Project_DTO deactivate_MP(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_MP/");
        }
        
        //==================DOC

        public ISM_Client_Project_DTO getdata_DOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_DOC/");
        }

        public ISM_Client_Project_DTO details_DOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_DOC/");
        }
        public ISM_Client_Project_DTO SaveEdit_DOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_DOC/");
        }
        public ISM_Client_Project_DTO deactivate_DOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_DOC/");
        }

        //================== MASTER DOC

        public ISM_Client_Project_DTO getdata_MDOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_MDOC/");
        }

        public ISM_Client_Project_DTO details_MDOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_MDOC/");
        }
        public ISM_Client_Project_DTO SaveEdit_MDOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_MDOC/");
        }
        public ISM_Client_Project_DTO deactivate_MDOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_MDOC/");
        }

    }
}
