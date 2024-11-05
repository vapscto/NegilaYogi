
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface exammasterInterface
    {
        exammasterDTO savedetails(exammasterDTO data); 
        exammasterDTO validateordernumber(exammasterDTO data);
        exammasterDTO deactivate(exammasterDTO data);
        exammasterDTO editdetails(int ID);
        exammasterDTO Getdetails(exammasterDTO data);

        // Master Exam Paper Type
        exammasterDTO BindData_PaperType(exammasterDTO data);
        exammasterDTO Saveddata_PT(exammasterDTO data);
        exammasterDTO Editdata_PT(exammasterDTO data);
        exammasterDTO DeactivateActivateMasterExam_PT(exammasterDTO data);
    }
}