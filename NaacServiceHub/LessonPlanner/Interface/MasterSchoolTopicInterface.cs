using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using PreadmissionDTOs.NAAC.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Interface
{
    public interface MasterSchoolTopicInterface
    {
        // School
        MasterSchoolTopicDTO Getdetails(MasterSchoolTopicDTO data);
        MasterSchoolTopicDTO savedetails(MasterSchoolTopicDTO data);
        MasterSchoolTopicDTO editdeatils(MasterSchoolTopicDTO data);
        MasterSchoolTopicDTO deactivate(MasterSchoolTopicDTO data);
        MasterSchoolTopicDTO gettopicdetails(MasterSchoolTopicDTO data);        
        MasterSchoolTopicDTO validateordernumber(MasterSchoolTopicDTO data);
        MasterSchoolTopicDTO onchangeyear(MasterSchoolTopicDTO data);
        MasterSchoolTopicDTO onchangeclass(MasterSchoolTopicDTO data);
        //College
        LP_Master_MainTopic_CollegeDTO getcollegedetails(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO onchangecollegeyear(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO onchangecourse(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO onchangebranch(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO onchangesemester(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO savecollegedetails(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO editcollegedeatils(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO collegedeactivate(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO getcollegetopicdetails(LP_Master_MainTopic_CollegeDTO data);
        LP_Master_MainTopic_CollegeDTO validatecollegeordernumber(LP_Master_MainTopic_CollegeDTO data);
    }
}
