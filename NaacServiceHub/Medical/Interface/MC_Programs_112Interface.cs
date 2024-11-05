using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
    public interface MC_Programs_112Interface
    {
        MC_Programs_112_DTO loaddata(MC_Programs_112_DTO data);
        MC_Programs_112_DTO savedata(MC_Programs_112_DTO data);
        MC_Programs_112_DTO editdata(MC_Programs_112_DTO data);
        MC_Programs_112_DTO deactive_Y(MC_Programs_112_DTO data);
        MC_Programs_112_DTO viewuploadflies(MC_Programs_112_DTO data);
        MC_Programs_112_DTO deleteuploadfile(MC_Programs_112_DTO data);
        MC_Programs_112_DTO StaffList_Boss(MC_Programs_112_DTO data);
        MC_Programs_112_DTO StaffList_Council(MC_Programs_112_DTO data);
    }
}
