using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface StudentRouteMappingInterface
    {


        StudentRouteMappingDTO getdata123(StudentRouteMappingDTO data);
        StudentRouteMappingDTO get_sections(StudentRouteMappingDTO data);
        StudentRouteMappingDTO get_cls_secs(StudentRouteMappingDTO data);
      StudentRouteMappingDTO on_pic_route_change(StudentRouteMappingDTO data);
      StudentRouteMappingDTO on_drp_route_change(StudentRouteMappingDTO data);
     
        StudentRouteMappingDTO getlisttwo(StudentRouteMappingDTO stu);
        // Task<StudentRouteMappingDTO> getreport(StudentRouteMappingDTO data);
        StudentRouteMappingDTO getreport(StudentRouteMappingDTO data);
        StudentRouteMappingDTO getreportedit(StudentRouteMappingDTO data);
        
        StudentRouteMappingDTO deactivate(StudentRouteMappingDTO data);
        StudentRouteMappingDTO searching(StudentRouteMappingDTO data);
        StudentRouteMappingDTO get_loca_sches(StudentRouteMappingDTO data);
        StudentRouteMappingDTO viewrecordspopup(StudentRouteMappingDTO data);
        StudentRouteMappingDTO SearchByColumn(StudentRouteMappingDTO data);
        StudentRouteMappingDTO checkduplicateno(StudentRouteMappingDTO data);
        
    }
    
}
