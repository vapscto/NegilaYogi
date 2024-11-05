using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeMasterDocumentInterface
    {
        // First Tab
        CollegeDocumentMasterDTO Getdetails(CollegeDocumentMasterDTO data);
        CollegeDocumentMasterDTO savedata(CollegeDocumentMasterDTO data);
        CollegeDocumentMasterDTO Editdata(CollegeDocumentMasterDTO data);
        CollegeDocumentMasterDTO DeleteData(CollegeDocumentMasterDTO data);
        //End Of First Tab
        CollegeDocumentMasterDTO onchangecourse(CollegeDocumentMasterDTO data);
        CollegeDocumentMasterDTO savedata1(CollegeDocumentMasterDTO data);
        CollegeDocumentMasterDTO getdoc(CollegeDocumentMasterDTO data);
        CollegeDocumentMasterDTO deactive_sub(CollegeDocumentMasterDTO data);
        
    }
}
