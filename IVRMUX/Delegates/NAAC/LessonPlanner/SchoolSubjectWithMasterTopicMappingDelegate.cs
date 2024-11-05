using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using PreadmissionDTOs.NAAC.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner
{
    public class SchoolSubjectWithMasterTopicMappingDelegate
    {
        CommonDelegate<SchoolSubjectWithMasterTopicMappingDTO, SchoolSubjectWithMasterTopicMappingDTO> _comm = new CommonDelegate<SchoolSubjectWithMasterTopicMappingDTO, SchoolSubjectWithMasterTopicMappingDTO>();

        CommonDelegate<CollegeSubjTopicMappingDTO, CollegeSubjTopicMappingDTO> _com = new CommonDelegate<CollegeSubjTopicMappingDTO, CollegeSubjTopicMappingDTO>();

        public SchoolSubjectWithMasterTopicMappingDTO Getdetails(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/Getdetails");
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubject(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangesubject");
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangeunit(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangeunit");
        }
        public SchoolSubjectWithMasterTopicMappingDTO savedetails(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/savedetails");
        }
        public SchoolSubjectWithMasterTopicMappingDTO deactivate(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/deactivate");
        }
        public SchoolSubjectWithMasterTopicMappingDTO editdeatils(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/editdeatils");
        }
        public SchoolSubjectWithMasterTopicMappingDTO validateordernumber(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/validateordernumber");
        }
        public SchoolSubjectWithMasterTopicMappingDTO onselecttopic(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onselecttopic");
        }
        public SchoolSubjectWithMasterTopicMappingDTO viewuploadflies(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/viewuploadflies");
        }
        public SchoolSubjectWithMasterTopicMappingDTO deleteuploadfile(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/deleteuploadfile");
        }

        // Topic Resource Mapping
        public SchoolSubjectWithMasterTopicMappingDTO Getdetailsmapping(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/Getdetailsmapping");
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangetopic(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangetopic");
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubtopic(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangesubtopic");
        }

        public SchoolSubjectWithMasterTopicMappingDTO savemapping(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/savemapping");
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangeyear(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangeyear");
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangeclass(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangeclass");
        }

        // College
        public CollegeSubjTopicMappingDTO Getcollegedetails(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/Getcollegedetails");
        }
        public CollegeSubjTopicMappingDTO collegeonchangeyear(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/collegeonchangeyear");
        }
        public CollegeSubjTopicMappingDTO collegeonchangecourse(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/collegeonchangecourse");
        }
        public CollegeSubjTopicMappingDTO collegeonchangebranch(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/collegeonchangebranch");
        }
        public CollegeSubjTopicMappingDTO collegeonchangesemester(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/collegeonchangesemester");
        }
        public CollegeSubjTopicMappingDTO onchangecollegesubject(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangecollegesubject");
        }
        public CollegeSubjTopicMappingDTO onchangecollegeunit(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangecollegeunit");
        }
        public CollegeSubjTopicMappingDTO savecollegedetails(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/savecollegedetails");
        }
        public CollegeSubjTopicMappingDTO editcollegedeatils(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/editcollegedeatils");
        }
        public CollegeSubjTopicMappingDTO collegedeactivate(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/collegedeactivate");
        }
        public CollegeSubjTopicMappingDTO oncollegeselecttopic(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/oncollegeselecttopic");
        }
        public CollegeSubjTopicMappingDTO viewcollegeuploadflies(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/viewcollegeuploadflies");
        }
        public CollegeSubjTopicMappingDTO deletecollegeuploadfile(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/deletecollegeuploadfile");
        }
        public CollegeSubjTopicMappingDTO validatecollegeordernumber(CollegeSubjTopicMappingDTO data)
        {
            return _com.naacdetailsbypost(data, "SchoolSubjectWithMasterTopicMappingFacade/validatecollegeordernumber");
        }

    }
}
