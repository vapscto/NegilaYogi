using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class NAAC_HSU_345_TeacherResearchPapersDelegate
    {

        CommonDelegate<NAAC_HSU_345_TeacherResearchPapers_DTO, NAAC_HSU_345_TeacherResearchPapers_DTO> comm = new CommonDelegate<NAAC_HSU_345_TeacherResearchPapers_DTO, NAAC_HSU_345_TeacherResearchPapers_DTO>();
        public NAAC_HSU_345_TeacherResearchPapers_DTO loaddata(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_345_TeacherResearchPapersFacade/loaddata");

        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO save(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_345_TeacherResearchPapersFacade/save");
        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO deactive(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_345_TeacherResearchPapersFacade/deactive");
        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO EditData(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_345_TeacherResearchPapersFacade/EditData");
        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO viewuploadflies(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_345_TeacherResearchPapersFacade/viewuploadflies");
        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO deleteuploadfile(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_345_TeacherResearchPapersFacade/deleteuploadfile");
        }

    }
}
