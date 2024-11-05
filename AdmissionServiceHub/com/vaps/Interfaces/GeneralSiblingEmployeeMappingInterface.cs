using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface GeneralSiblingEmployeeMappingInterface
    {
        GeneralSiblingEmployeeMappingDTO getalldetails(GeneralSiblingEmployeeMappingDTO data);
        GeneralSiblingEmployeeMappingDTO selectradio(GeneralSiblingEmployeeMappingDTO data);
        GeneralSiblingEmployeeMappingDTO onstudentnamechange(GeneralSiblingEmployeeMappingDTO data);
        GeneralSiblingEmployeeMappingDTO onselectstaff(GeneralSiblingEmployeeMappingDTO data);        
        GeneralSiblingEmployeeMappingDTO savedata(GeneralSiblingEmployeeMappingDTO data);
        GeneralSiblingEmployeeMappingDTO deleterec(GeneralSiblingEmployeeMappingDTO data);
        GeneralSiblingEmployeeMappingDTO DeletRecordemployee(GeneralSiblingEmployeeMappingDTO data);
        GeneralSiblingEmployeeMappingDTO viewsiblingdetails(GeneralSiblingEmployeeMappingDTO data);
        GeneralSiblingEmployeeMappingDTO viewsiblingdetailsemployee(GeneralSiblingEmployeeMappingDTO data);       
    }
}
