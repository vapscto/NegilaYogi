using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NaacCommiteeInterface
    {

        NAAC_AC_Committee_DTO loaddata(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO saverecord(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO get_Designation(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO get_Employee(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO savefilewisecomments(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO getfilecomment(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO savefilewisecommentsmember(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO getfilecommentmember(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO getcomment(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO savemedicaldatawisecommentsmember(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO savemedicaldatawisecomments(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO deactiveStudent(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO getcommentmember(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO EditData(NAAC_AC_Committee_DTO data);
        Task<NAAC_AC_Committee_DTO> get_MappedStaff(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO deactive_staff(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO viewdocument_MainActUploadFiles(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO delete_MainActUploadFiles(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO viewdocument_StaffActUploadFiles(NAAC_AC_Committee_DTO data);
        NAAC_AC_Committee_DTO delete_StaffActUploadFiles(NAAC_AC_Committee_DTO data);

    }
}
