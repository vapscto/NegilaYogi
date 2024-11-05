using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class StaffLoginDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<StaffLoginDTO, StaffLoginDTO> COMMM = new CommonDelegate<StaffLoginDTO, StaffLoginDTO>();

        public StaffLoginDTO getmoduledet(StaffLoginDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "StaffLoginFacade/getmoduledetails/");
        }


        public StaffLoginDTO getpagedetails(StaffLoginDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "StaffLoginFacade/getpagedetailsrolemodulewise/");
        }


        public StaffLoginDTO updateusername(StaffLoginDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "StaffLoginFacade/updateusername/");
        }

        public StaffLoginDTO searchfilter(StaffLoginDTO sddto)
        {
            return COMMM.POSTData(sddto, "StaffLoginFacade/searchfilter/");
        }

        public StaffLoginDTO getstudata(StaffLoginDTO sddto)
        {
            return COMMM.POSTData(sddto, "StaffLoginFacade/getstudata/");
        }

        public StaffLoginDTO onchangeuser (StaffLoginDTO sddto)
        {
            return COMMM.POSTData(sddto, "StaffLoginFacade/onchangeuser/");
        }

        public StaffLoginDTO multionchangeuser(StaffLoginDTO sddto)
        {
            return COMMM.POSTData(sddto, "StaffLoginFacade/multionchangeuser/");
        }

        public StaffLoginDTO multiuserdeletpages(StaffLoginDTO sddto)
        {
            return COMMM.POSTData(sddto, "StaffLoginFacade/multiuserdeletpages/");
        }

        public StaffLoginDTO savedetails(StaffLoginDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "StaffLoginFacade/");
        }

        public StaffLoginDTO deleterec(StaffLoginDTO id)
        {
            return COMMM.POSTData(id, "StaffLoginFacade/deletemodpages/");
        }

        public StaffLoginDTO checkusernmedup(StaffLoginDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "StaffLoginFacade/checkdupli/");
        }


        public StaffLoginDTO getmoduleroledetails(int id)
        {
            return COMMM.GetDataById(id, "StaffLoginFacade/getmodulerolesinswise/");
        }

        public StaffLoginDTO getfilterde(int data, StaffLoginDTO dataa)
        {
            return COMMM.GETSEarchData(data, dataa, "StaffLoginFacade/1/");
        }


        public StaffLoginDTO changeinstitu(StaffLoginDTO data)
        {
            return COMMM.POSTData(data, "StaffLoginFacade/changeinsti/");
        }

        public StaffLoginDTO checktrustfun(StaffLoginDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "StaffLoginFacade/checktrust/");
        }
        public StaffLoginDTO getstaffmobilepages(StaffLoginDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "StaffLoginFacade/getstaffmobilepages/");
        }
    }
}
