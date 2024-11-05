using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
    public interface MC_121_IntDept_CourseInterface
    {
        MC_121_IntDept_Course_DTO loaddata(MC_121_IntDept_Course_DTO data);
        MC_121_IntDept_Course_DTO savedata(MC_121_IntDept_Course_DTO data);
        MC_121_IntDept_Course_DTO editdata(MC_121_IntDept_Course_DTO data);
        MC_121_IntDept_Course_DTO deactivY(MC_121_IntDept_Course_DTO data);
        Task<MC_121_IntDept_Course_DTO> get_Course(MC_121_IntDept_Course_DTO data);       
        MC_121_IntDept_Course_DTO viewuploadflies(MC_121_IntDept_Course_DTO data);
        MC_121_IntDept_Course_DTO deleteuploadfile(MC_121_IntDept_Course_DTO data);

        MC_121_IntDept_Course_DTO savemedicaldatawisecomments(MC_121_IntDept_Course_DTO data);
        MC_121_IntDept_Course_DTO savefilewisecomments(MC_121_IntDept_Course_DTO data);
        MC_121_IntDept_Course_DTO getcomment(MC_121_IntDept_Course_DTO data);
        MC_121_IntDept_Course_DTO getfilecomment(MC_121_IntDept_Course_DTO data);

    }
}
