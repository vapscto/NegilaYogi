using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeMasterDocumentDelegate
    {
        CommonDelegate<CollegeDocumentMasterDTO, CollegeDocumentMasterDTO> _comm = new CommonDelegate<CollegeDocumentMasterDTO, CollegeDocumentMasterDTO>();
        public CollegeDocumentMasterDTO Getdetails(CollegeDocumentMasterDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeMasterDocumentFacade/Getdetails/");
        }
        public CollegeDocumentMasterDTO savedata(CollegeDocumentMasterDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeMasterDocumentFacade/savedata/");
        }
        public CollegeDocumentMasterDTO Editdata(CollegeDocumentMasterDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeMasterDocumentFacade/Editdata/");
        }
        public CollegeDocumentMasterDTO DeleteData(CollegeDocumentMasterDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeMasterDocumentFacade/DeleteData/");
        }
        public CollegeDocumentMasterDTO onchangecourse(CollegeDocumentMasterDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeMasterDocumentFacade/onchangecourse/");
        }
        public CollegeDocumentMasterDTO savedata1(CollegeDocumentMasterDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeMasterDocumentFacade/savedata1/");
        }
        public CollegeDocumentMasterDTO getdoc(CollegeDocumentMasterDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeMasterDocumentFacade/getdoc/");
        }
        public CollegeDocumentMasterDTO deactive_sub(CollegeDocumentMasterDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeMasterDocumentFacade/deactive_sub/");
        }
        
    }
}
