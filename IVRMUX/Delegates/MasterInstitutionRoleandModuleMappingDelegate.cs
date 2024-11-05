using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class MasterInstitutionRoleandModuleMappingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<InstitutionRolePrivilegesDTO, InstitutionRolePrivilegesDTO> COMMM = new CommonDelegate<InstitutionRolePrivilegesDTO, InstitutionRolePrivilegesDTO>();
        
             CommonDelegate<MasterRolePreviledgeDTO, MasterRolePreviledgeDTO> COMMMM = new CommonDelegate<MasterRolePreviledgeDTO, MasterRolePreviledgeDTO>();
        public InstitutionRolePrivilegesDTO saveInstitution_Module_Pagedetails(InstitutionRolePrivilegesDTO TnDTO)
        {
            return COMMM.POSTData(TnDTO, "MasterInstitutionRoleandModuleMappingFacade/");
        }

        //

        public InstitutionRolePrivilegesDTO Institution_Module_PageDetails(InstitutionRolePrivilegesDTO data)
        {
            return COMMM.POSTData(data, "MasterInstitutionRoleandModuleMappingFacade/getdetails/");
        }


        public InstitutionRolePrivilegesDTO getModuleDropdownList(int id)
        {
            return COMMM.GetDataById(id, "MasterInstitutionRoleandModuleMappingFacade/getModuleDropdownList/");
        }

        public MasterRolePreviledgeDTO Module_PageDetails(int id)
        {
            return COMMMM.GetDataById(id, "MasterInstitutionRoleandModuleMappingFacade/getPageDetailsBiModuleId/");
        }


        public MasterRolePreviledgeDTO Module_PageDetailsByRoleType(MasterRolePreviledgeDTO orgdet)
        {
            return COMMMM.POSTData(orgdet, "MasterInstitutionRoleandModuleMappingFacade/getPagedetailsByRoletype/");
        }


        public InstitutionRolePrivilegesDTO deleterec(InstitutionRolePrivilegesDTO id)
        {
            return COMMM.POSTData(id, "MasterInstitutionRoleandModuleMappingFacade/deletedetails/");

        }

    }
}
