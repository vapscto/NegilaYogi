using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs;

namespace IVRMUX.Delegates
{
    public class InstitutionUserMappingDelegate
    {
        CommonDelegate<InstitutionUserMappingDTO, InstitutionUserMappingDTO> COMMM = new CommonDelegate<InstitutionUserMappingDTO, InstitutionUserMappingDTO>();
        public InstitutionUserMappingDTO loaddata(InstitutionUserMappingDTO data)
        {
            return COMMM.POSTData(data, "InstitutionUserMappingFacade/loaddata/");
        }
        public InstitutionUserMappingDTO onchangeinst(InstitutionUserMappingDTO data)
        {
            return COMMM.POSTData(data, "InstitutionUserMappingFacade/onchangeinst/");
        }
        public InstitutionUserMappingDTO savedetails(InstitutionUserMappingDTO data)
        {
            return COMMM.POSTData(data, "InstitutionUserMappingFacade/savedetails/");
        }
        public InstitutionUserMappingDTO viewdetails(InstitutionUserMappingDTO data)
        {
            return COMMM.POSTData(data, "InstitutionUserMappingFacade/viewdetails/");
        }

        public InstitutionUserMappingDTO savepaymentremarks(InstitutionUserMappingDTO data)
        {
            return COMMM.POSTData(data, "InstitutionUserMappingFacade/savepaymentremarks/");
        }
    }
}