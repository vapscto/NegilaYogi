using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACSportsInterface
    {
        NAACSportsDTO loaddata(NAACSportsDTO data);
       
        NAACSportsDTO save(NAACSportsDTO data);
        NAACSportsDTO deactiveStudent(NAACSportsDTO data);
        NAACSportsDTO EditData(NAACSportsDTO obj);
        NAACSportsDTO viewuploadflies(NAACSportsDTO obj);
        NAACSportsDTO deleteuploadfile(NAACSportsDTO obj);
        NAACSportsDTO get_course(NAACSportsDTO obj);
        NAACSportsDTO get_branch(NAACSportsDTO obj);
        NAACSportsDTO get_sems(NAACSportsDTO obj);
        NAACSportsDTO get_section(NAACSportsDTO obj);
        NAACSportsDTO GetStudentDetails(NAACSportsDTO obj);
        NAACSportsDTO savemedicaldatawisecomments(NAACSportsDTO obj);
        NAACSportsDTO getcomment(NAACSportsDTO obj);
        NAACSportsDTO getfilecomment(NAACSportsDTO obj);
        NAACSportsDTO savefilewisecomments(NAACSportsDTO obj);

    }
}
