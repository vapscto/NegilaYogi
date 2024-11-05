using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using PreadmissionDTOs.NAAC.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Interface
{
    public interface SchoolSubjectWithMasterTopicMappingInterface
    {
        SchoolSubjectWithMasterTopicMappingDTO Getdetails(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO onchangesubject(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO onchangeunit(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO savedetails(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO deactivate(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO validateordernumber(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO editdeatils(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO onselecttopic(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO viewuploadflies(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO deleteuploadfile(SchoolSubjectWithMasterTopicMappingDTO data);

        // Topic Resource Mapping 
        SchoolSubjectWithMasterTopicMappingDTO Getdetailsmapping(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO onchangetopic(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO onchangesubtopic(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO savemapping(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO onchangeyear(SchoolSubjectWithMasterTopicMappingDTO data);
        SchoolSubjectWithMasterTopicMappingDTO onchangeclass(SchoolSubjectWithMasterTopicMappingDTO data);

        // College 
        CollegeSubjTopicMappingDTO Getcollegedetails(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO collegeonchangeyear(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO collegeonchangecourse(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO collegeonchangebranch(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO collegeonchangesemester(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO onchangecollegesubject(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO onchangecollegeunit(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO savecollegedetails(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO editcollegedeatils(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO collegedeactivate(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO oncollegeselecttopic(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO validatecollegeordernumber(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO viewcollegeuploadflies(CollegeSubjTopicMappingDTO data);
        CollegeSubjTopicMappingDTO deletecollegeuploadfile(CollegeSubjTopicMappingDTO data);
    }
}
