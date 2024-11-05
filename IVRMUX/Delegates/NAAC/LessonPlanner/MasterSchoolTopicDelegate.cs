using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using PreadmissionDTOs.NAAC.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner
{
    public class MasterSchoolTopicDelegate
    {
        CommonDelegate<MasterSchoolTopicDTO, MasterSchoolTopicDTO> _comm = new CommonDelegate<MasterSchoolTopicDTO, MasterSchoolTopicDTO>();
        CommonDelegate<LP_Master_MainTopic_CollegeDTO, LP_Master_MainTopic_CollegeDTO> _com = new CommonDelegate<LP_Master_MainTopic_CollegeDTO, LP_Master_MainTopic_CollegeDTO>();

        // School
        public MasterSchoolTopicDTO Getdetails(MasterSchoolTopicDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterSchoolTopicFacade/Getdetails");
        }
        public MasterSchoolTopicDTO savedetails(MasterSchoolTopicDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterSchoolTopicFacade/savedetails");
        }
        public MasterSchoolTopicDTO editdeatils(MasterSchoolTopicDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterSchoolTopicFacade/editdeatils");
        }
        public MasterSchoolTopicDTO deactivate(MasterSchoolTopicDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterSchoolTopicFacade/deactivate");
        }
        public MasterSchoolTopicDTO validateordernumber(MasterSchoolTopicDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterSchoolTopicFacade/validateordernumber");
        }
        public MasterSchoolTopicDTO gettopicdetails(MasterSchoolTopicDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterSchoolTopicFacade/gettopicdetails");
        }
        public MasterSchoolTopicDTO onchangeyear(MasterSchoolTopicDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterSchoolTopicFacade/onchangeyear");
        }
        public MasterSchoolTopicDTO onchangeclass(MasterSchoolTopicDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterSchoolTopicFacade/onchangeclass");
        }

        // College 
        public LP_Master_MainTopic_CollegeDTO getcollegedetails(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/getcollegedetails");
        }
        public LP_Master_MainTopic_CollegeDTO onchangecollegeyear(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/onchangecollegeyear");
        }
        public LP_Master_MainTopic_CollegeDTO onchangecourse(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/onchangecourse");
        }
        public LP_Master_MainTopic_CollegeDTO onchangebranch(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/onchangebranch");
        }
        public LP_Master_MainTopic_CollegeDTO onchangesemester(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/onchangesemester");
        }
        public LP_Master_MainTopic_CollegeDTO savecollegedetails(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/savecollegedetails");
        }
        public LP_Master_MainTopic_CollegeDTO editcollegedeatils(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/editcollegedeatils");
        }
        public LP_Master_MainTopic_CollegeDTO collegedeactivate(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/collegedeactivate");
        }
        public LP_Master_MainTopic_CollegeDTO getcollegetopicdetails(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/getcollegetopicdetails");
        }
        public LP_Master_MainTopic_CollegeDTO validatecollegeordernumber(LP_Master_MainTopic_CollegeDTO data)
        {
            return _com.naacdetailsbypost(data, "MasterSchoolTopicFacade/validatecollegeordernumber");
        }

    }
}
