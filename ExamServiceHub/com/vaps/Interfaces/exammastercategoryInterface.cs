
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface exammastercategoryInterface
    {
        exammastercategoryDTO savedetail1(exammastercategoryDTO objcategory);
        exammastercategoryDTO savedetail2(exammastercategoryDTO objcategory);
        exammastercategoryDTO getalldetailsviewrecords(exammastercategoryDTO objcategory);
        exammastercategoryDTO deactivate_sec(exammastercategoryDTO objcategory);
        exammastercategoryDTO geteventdetails(exammastercategoryDTO objcategory);
        exammastercategoryDTO deactivate1(exammastercategoryDTO data);
        exammastercategoryDTO deactivate2(exammastercategoryDTO data);
        exammastercategoryDTO getdetails(int id);
        exammastercategoryDTO getpageedit1(int id);
        exammastercategoryDTO getpageedit2(exammastercategoryDTO data); 
        exammastercategoryDTO get_cate_class(exammastercategoryDTO data);
        exammastercategoryDTO get_cls_sections(exammastercategoryDTO data);
        exammastercategoryDTO getalldetailsviewrecords1(int id);
        exammastercategoryDTO getalldetailsviewrecords2(int id);
        exammastercategoryDTO deleterec(int id);
        exammastercategoryDTO Save_ReportCard_Format(exammastercategoryDTO data);
        exammastercategoryDTO deactive_format(exammastercategoryDTO data);
    }
}