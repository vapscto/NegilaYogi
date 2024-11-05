
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface CategorySubjectMappingInterface
    {
        CategorySubjectMappingDTO savedetail(CategorySubjectMappingDTO objcategory);
        CategorySubjectMappingDTO deactivate(CategorySubjectMappingDTO data);
        CategorySubjectMappingDTO getdetails(int id);
        CategorySubjectMappingDTO get_category(CategorySubjectMappingDTO data);
        CategorySubjectMappingDTO get_subjects(CategorySubjectMappingDTO data);
        CategorySubjectMappingDTO getpageedit(int id);
        CategorySubjectMappingDTO getalldetailsviewrecords(int id);
        CategorySubjectMappingDTO deleterec(int id);

        /* Category Dates Mapping */
        CategorySubjectMappingDTO OnLoadCategoryDates(CategorySubjectMappingDTO data);
        CategorySubjectMappingDTO get_categoryDates(CategorySubjectMappingDTO data);
        CategorySubjectMappingDTO savedatadates(CategorySubjectMappingDTO data);
    }
}
