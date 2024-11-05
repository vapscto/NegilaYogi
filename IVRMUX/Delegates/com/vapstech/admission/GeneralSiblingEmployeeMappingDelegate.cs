using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class GeneralSiblingEmployeeMappingDelegate
    {
        CommonDelegate<GeneralSiblingEmployeeMappingDTO, GeneralSiblingEmployeeMappingDTO> _comm = new CommonDelegate<GeneralSiblingEmployeeMappingDTO, GeneralSiblingEmployeeMappingDTO>();

        public GeneralSiblingEmployeeMappingDTO getalldetails(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/getalldetails");
        }
        public GeneralSiblingEmployeeMappingDTO selectradio(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/selectradio");
        }
        public GeneralSiblingEmployeeMappingDTO onstudentnamechange(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/onstudentnamechange");
        }
        public GeneralSiblingEmployeeMappingDTO onselectstaff(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/onselectstaff");
        }        
        public GeneralSiblingEmployeeMappingDTO savedata(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/savedata");
        }
        public GeneralSiblingEmployeeMappingDTO deleterec(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/deleterec");
        } public GeneralSiblingEmployeeMappingDTO DeletRecordemployee(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/DeletRecordemployee");
        }
        public GeneralSiblingEmployeeMappingDTO viewsiblingdetails(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/viewsiblingdetails");
        }
        public GeneralSiblingEmployeeMappingDTO viewsiblingdetailsemployee(GeneralSiblingEmployeeMappingDTO data)
        {
            return _comm.POSTDataADM(data, "GeneralSiblingEmployeeMappingFacade/viewsiblingdetailsemployee");
        }
    }
}
