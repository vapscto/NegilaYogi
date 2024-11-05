using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
  public  interface NAAC_HSU_Course_StaffMapping_122Interface
    {
        NAAC_HSU_Course_StaffMapping_122DTO loaddata(NAAC_HSU_Course_StaffMapping_122DTO data);
        NAAC_HSU_Course_StaffMapping_122DTO save(NAAC_HSU_Course_StaffMapping_122DTO data);
        NAAC_HSU_Course_StaffMapping_122DTO deactive(NAAC_HSU_Course_StaffMapping_122DTO data);
        NAAC_HSU_Course_StaffMapping_122DTO EditData(NAAC_HSU_Course_StaffMapping_122DTO data);
        NAAC_HSU_Course_StaffMapping_122DTO viewuploadflies(NAAC_HSU_Course_StaffMapping_122DTO data);
        NAAC_HSU_Course_StaffMapping_122DTO deleteuploadfile(NAAC_HSU_Course_StaffMapping_122DTO data);
        NAAC_HSU_Course_StaffMapping_122DTO get_course(NAAC_HSU_Course_StaffMapping_122DTO data);
        NAAC_HSU_Course_StaffMapping_122DTO get_designation(NAAC_HSU_Course_StaffMapping_122DTO data);
        NAAC_HSU_Course_StaffMapping_122DTO get_employee(NAAC_HSU_Course_StaffMapping_122DTO data);
    }
}
