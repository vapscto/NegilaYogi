using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Sales
{
    public class MastersModuleDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MastersModule_DTO, MastersModule_DTO> COMMM = new CommonDelegate<MastersModule_DTO, MastersModule_DTO>();
        CommonDelegate<ISM_Master_TaskGroup_DTO, ISM_Master_TaskGroup_DTO> COMMMT = new CommonDelegate<ISM_Master_TaskGroup_DTO, ISM_Master_TaskGroup_DTO>();

        public MastersModule_DTO getdetails(MastersModule_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersModuleFacade/getdetails/");
        }
        public MastersModule_DTO saverecord(MastersModule_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersModuleFacade/saverecord");
        }
        public MastersModule_DTO deactiveY(MastersModule_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersModuleFacade/deactiveY");
        }
        public MastersModule_DTO get_emplist(MastersModule_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersModuleFacade/get_emplist");
        }
        public MastersModule_DTO editlist(MastersModule_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersModuleFacade/editlist");
        }
        public MastersModule_DTO get_MappedDeveloperlist(MastersModule_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersModuleFacade/get_MappedDeveloperlist");
        }
        public MastersModule_DTO deactiveDevpMappingdata(MastersModule_DTO dTO)
        {
            return COMMM.POSTDataInventory(dTO, "MastersModuleFacade/deactiveDevpMappingdata");
        }
        //==================master task group mapping
        public ISM_Master_TaskGroup_DTO getdetails_taskgroup(ISM_Master_TaskGroup_DTO dTO)
        {
            return COMMMT.POSTDataInventory(dTO, "MastersModuleFacade/getdetails_taskgroup");
        }
        public ISM_Master_TaskGroup_DTO getdept(ISM_Master_TaskGroup_DTO dTO)
        {
            return COMMMT.POSTDataInventory(dTO, "MastersModuleFacade/getdept");
        }
        public ISM_Master_TaskGroup_DTO get_task(ISM_Master_TaskGroup_DTO dTO)
        {
            return COMMMT.POSTDataInventory(dTO, "MastersModuleFacade/get_task");
        }
        public ISM_Master_TaskGroup_DTO show_tasklist(ISM_Master_TaskGroup_DTO dTO)
        {
            return COMMMT.POSTDataInventory(dTO, "MastersModuleFacade/show_tasklist");
        }
        public ISM_Master_TaskGroup_DTO save_taskgrpdata(ISM_Master_TaskGroup_DTO dTO)
        {
            return COMMMT.POSTDataInventory(dTO, "MastersModuleFacade/save_taskgrpdata");
        }
        public ISM_Master_TaskGroup_DTO task_view(ISM_Master_TaskGroup_DTO dTO)
        {
            return COMMMT.POSTDataInventory(dTO, "MastersModuleFacade/task_view");
        }
        public ISM_Master_TaskGroup_DTO task_edit(ISM_Master_TaskGroup_DTO dTO)
        {
            return COMMMT.POSTDataInventory(dTO, "MastersModuleFacade/task_edit");
        }
    }
}
