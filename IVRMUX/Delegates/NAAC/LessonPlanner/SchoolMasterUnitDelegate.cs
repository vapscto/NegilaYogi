using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner
{
    public class SchoolMasterUnitDelegate
    {
        CommonDelegate<SchoolMasterUnitDTO, SchoolMasterUnitDTO> _comm = new CommonDelegate<SchoolMasterUnitDTO, SchoolMasterUnitDTO>();

        public SchoolMasterUnitDTO Getdetails(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/Getdetails");
        }
        public SchoolMasterUnitDTO savedetails(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/savedetails");
        }
        public SchoolMasterUnitDTO editdeatils(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/editdeatils");
        }
        public SchoolMasterUnitDTO deactivate(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/deactivate");
        }
        public SchoolMasterUnitDTO validateordernumber(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/validateordernumber");
        }

        // Master Unit Topic Mapping
        public SchoolMasterUnitDTO Getdetailsmapping(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/Getdetailsmapping");
        }
        public SchoolMasterUnitDTO gettopicnames(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/gettopicnames");
        }
        public SchoolMasterUnitDTO savemappingdetails(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/savemappingdetails");
        }
        public SchoolMasterUnitDTO deactivatemapping(SchoolMasterUnitDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolMasterUnitFacade/deactivatemapping");
        }
        
    }
}
