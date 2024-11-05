using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface MasterHostelInterface
    {
        HL_Master_Hostel_DTO Page_loaddata(HL_Master_Hostel_DTO data);
        HL_Master_Hostel_DTO Get_StateData(HL_Master_Hostel_DTO data);
        HL_Master_Hostel_DTO Save_Hostel_Data(HL_Master_Hostel_DTO data);
        HL_Master_Hostel_DTO Edit_Hostel_Row(HL_Master_Hostel_DTO data);
        HL_Master_Hostel_DTO Deactive_Hostel_Row(HL_Master_Hostel_DTO data);
        HL_Master_Hostel_DTO Get_MappedFacility(HL_Master_Hostel_DTO data);
        HL_Master_Hostel_DTO Get_MappedEmpl(HL_Master_Hostel_DTO data);
        HL_Master_Hostel_DTO viewuploadflies(HL_Master_Hostel_DTO data);
        HL_Master_Hostel_DTO deleteuploadfile(HL_Master_Hostel_DTO data);

    }
}
