using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Sales
{
    public class ISM_ClientProject_MappingDelegate
    {
        CommonDelegate<ISM_ClientProject_MappingDTO, ISM_ClientProject_MappingDTO> comm = new CommonDelegate<ISM_ClientProject_MappingDTO, ISM_ClientProject_MappingDTO>();
        public ISM_ClientProject_MappingDTO loaddata(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTDataInventory(data, "ISM_ClientProject_MappingFacade/loaddata");
        }
        public ISM_ClientProject_MappingDTO savedata(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTDataInventory(data, "ISM_ClientProject_MappingFacade/savedata");
        }
        public ISM_ClientProject_MappingDTO Editdata(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTDataInventory(data, "ISM_ClientProject_MappingFacade/Editdata");
        }
        public ISM_ClientProject_MappingDTO getproject(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTDataInventory(data, "ISM_ClientProject_MappingFacade/getproject");
        }
        public ISM_ClientProject_MappingDTO getmodule(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTDataInventory(data, "ISM_ClientProject_MappingFacade/getmodule");
        }
        public ISM_ClientProject_MappingDTO clientDecative(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTDataInventory(data, "ISM_ClientProject_MappingFacade/clientDecative");
        }
    }
}
