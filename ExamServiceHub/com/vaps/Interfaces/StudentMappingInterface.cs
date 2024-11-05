
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface StudentMappingInterface
    {
        StudentMappingDTO savedetails(StudentMappingDTO data);
        StudentMappingDTO deactivate(StudentMappingDTO data);
        StudentMappingDTO editdetails(int ID); 
        StudentMappingDTO getalldetailsviewrecords(int ID);
        StudentMappingDTO getcategory(StudentMappingDTO data);
        StudentMappingDTO getclassid(StudentMappingDTO data); 
        StudentMappingDTO getsubject(StudentMappingDTO data);
        StudentMappingDTO Getdetails(StudentMappingDTO data); 
        StudentMappingDTO Studentdetails(StudentMappingDTO data);
        StudentMappingDTO get_cls_sections(StudentMappingDTO data);
        StudentMappingDTO OnClickRemove(StudentMappingDTO data);

        //Student Wise Question Paper Type Mapping
        StudentMappingDTO BindData_PT(StudentMappingDTO data);
        StudentMappingDTO OnChangeYear_GetClass_PT(StudentMappingDTO data);
        StudentMappingDTO OnChangeClass_GetSection_PT(StudentMappingDTO data);
        StudentMappingDTO OnChangeSection_GetExam_PT(StudentMappingDTO data);
        StudentMappingDTO OnChangeExam_GetSubject_PT(StudentMappingDTO data);
        StudentMappingDTO OnSearch_PT(StudentMappingDTO data);
        StudentMappingDTO OnSave_PT(StudentMappingDTO data);
        StudentMappingDTO OnClickRemove_PT(StudentMappingDTO data);
    }
}
