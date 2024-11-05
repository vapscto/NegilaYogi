using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class HouseInchargeDelegate
    {

        CommonDelegate<SPCC_Master_House_Staff_DTO, SPCC_Master_House_Staff_DTO> COMSPRT = new CommonDelegate<SPCC_Master_House_Staff_DTO, SPCC_Master_House_Staff_DTO>();

        public SPCC_Master_House_Staff_DTO Getdetails(SPCC_Master_House_Staff_DTO data)
        {
            return COMSPRT.POSTDataSports(data, "HouseInchargeFacade/Getdetails/");
        }
        public SPCC_Master_House_Staff_DTO get_House(SPCC_Master_House_Staff_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "HouseInchargeFacade/get_House/");
        }
        public SPCC_Master_House_Staff_DTO saverecord(SPCC_Master_House_Staff_DTO data)
        {
            return COMSPRT.POSTDataSports(data, "HouseInchargeFacade/saverecord/");
        }
        public SPCC_Master_House_Staff_DTO editrecord(SPCC_Master_House_Staff_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "HouseInchargeFacade/editrecord/");
        }
        public SPCC_Master_House_Staff_DTO deactive(SPCC_Master_House_Staff_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "HouseInchargeFacade/deactive/");
        }

        public SPCC_Master_House_Staff_DTO getdepchange(SPCC_Master_House_Staff_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "HouseInchargeFacade/getdepchange/");
        }
        public SPCC_Master_House_Staff_DTO get_staff1(SPCC_Master_House_Staff_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "HouseInchargeFacade/get_staff1/");
        }
        
    }
}
