using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class SportsMasterHouseDelegate
    {
        CommonDelegate<SportsMasterHouse_DTO, SportsMasterHouse_DTO> COMSPRT = new CommonDelegate<SportsMasterHouse_DTO, SportsMasterHouse_DTO>();

        public SportsMasterHouse_DTO getdetails(SportsMasterHouse_DTO data)
        {
            return COMSPRT.POSTDataSports(data, "SportsMasterHouseFacade/getdetails/");
        }

       
        public SportsMasterHouse_DTO savedata(SportsMasterHouse_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsMasterHouseFacade/savedata/");
        }
       
        public SportsMasterHouse_DTO deactivate(SportsMasterHouse_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsMasterHouseFacade/deactivate/");
        }

        public SportsMasterHouse_DTO editdata(SportsMasterHouse_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsMasterHouseFacade/editdata/");
        }

    }
}
