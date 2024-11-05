using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Sales
{
    public class MastersProjectDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MastersProject_DTO, MastersProject_DTO> COMMM = new CommonDelegate<MastersProject_DTO, MastersProject_DTO>();

        public MastersProject_DTO getdetails(MastersProject_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersProjectFacade/getdetails/");
        }
        public MastersProject_DTO OnChangeInstitution(MastersProject_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersProjectFacade/OnChangeInstitution/");
        }
        public MastersProject_DTO saverecord(MastersProject_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersProjectFacade/saverecord/");
        }
        public MastersProject_DTO deactiveY(MastersProject_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersProjectFacade/deactiveY/");
        }
    }
}
