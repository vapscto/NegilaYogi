using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
  public interface StudentProjectInterface
    {

        StudentProject_DTO savedata(StudentProject_DTO data);
        Task<StudentProject_DTO> loaddata(StudentProject_DTO data);
        StudentProject_DTO get_branch(StudentProject_DTO data);
        StudentProject_DTO get_student(StudentProject_DTO data);
        StudentProject_DTO deactiveStudent(StudentProject_DTO data);
        StudentProject_DTO editdata(StudentProject_DTO data);
        StudentProject_DTO viewuploadflies(StudentProject_DTO data);
        StudentProject_DTO deleteuploadfile(StudentProject_DTO data);



        StudentProject_DTO MC_Savedata_134(StudentProject_DTO data);
        StudentProject_DTO MC_editdata_134(StudentProject_DTO data);
        StudentProject_DTO MC_viewuploadflies_134(StudentProject_DTO data);
        StudentProject_DTO MC_deleteuploadfile_134(StudentProject_DTO data);


        StudentProject_DTO savemedicaldatawisecomments(StudentProject_DTO data);
        StudentProject_DTO savefilewisecomments(StudentProject_DTO data);
        StudentProject_DTO getcomment(StudentProject_DTO data);
        StudentProject_DTO getfilecomment(StudentProject_DTO data);


        StudentProject_DTO savedatawisecommentsAffi(StudentProject_DTO data);
        StudentProject_DTO savefilewisecommentsAffi(StudentProject_DTO data);
        StudentProject_DTO getcommentAffi(StudentProject_DTO data);
        StudentProject_DTO getfilecommentAffi(StudentProject_DTO data);
        StudentProject_DTO deactiveY(StudentProject_DTO data);

        //added by sanjeev
        StudentProject_DTO saveadvance(StudentProject_DTO data);
        

    }
}
