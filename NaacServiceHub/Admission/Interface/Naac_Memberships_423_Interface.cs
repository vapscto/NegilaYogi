using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface Naac_Memberships_423_Interface
    {

        Naac_Memberships_423_DTO deactiveStudent(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO save(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO loaddata(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO getcomment(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO getfilecomment(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO savefilewisecomments(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO savemedicaldatawisecomments(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO EditData(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO viewuploadflies(Naac_Memberships_423_DTO data);
        Naac_Memberships_423_DTO deleteuploadfile(Naac_Memberships_423_DTO data);

    }
}
