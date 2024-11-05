using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class StudentMasterConfigurationDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();
        CommonDelegate<MasterConfigurationDTO, MasterConfigurationDTO> COMMM = new CommonDelegate<MasterConfigurationDTO, MasterConfigurationDTO>();

        CommonDelegate<CommonDTO, CommonDTO> COMMMM = new CommonDelegate<CommonDTO, CommonDTO>();
        // empty Constructor, dnt delete please
        public StudentMasterConfigurationDelegates() { }

        public StudentMasterConfigurationDelegates(FacadeUrl config) {  _config = config; fdu = config; }



        public CommonDTO getMasterConfigDropdown(CommonDTO data)
        {
            return COMMMM.POSTData(data, "StudentMasterConfigurationFacade/getmasterdrp/");
        }

        //
        public MasterConfigurationDTO getMasterConfigEditData(int id)
        {
            return COMMM.GetDataById(id, "StudentMasterConfigurationFacade/getmastereditdata/");
        }
        //

        public MasterConfigurationDTO deleteRecord(MasterConfigurationDTO data)
        {
            return COMMM.POSTData(data, "StudentMasterConfigurationFacade/deletedetails/");
        }

        public MasterConfigurationDTO saveMasterConfigData(MasterConfigurationDTO mstConfig)
        {
            return COMMM.POSTData(mstConfig, "StudentMasterConfigurationFacade/");
        }
    }
}
