using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.NAAC.LessonPlanner;
using CommonLibrary;


namespace IVRMUX.Delegates.NAAC.LessonPlanner
{
    public class LMSStudentDelegate
    {
        CommonDelegate<LMSStudentDTO, LMSStudentDTO> _comm = new CommonDelegate<LMSStudentDTO, LMSStudentDTO>();

        // College
        public LMSStudentDTO Getdetails(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/Getdetails");
        }
        public LMSStudentDTO onchangesemester(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/onchangesemester");
        }
        public LMSStudentDTO getcollegetopics(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/getcollegetopics");
        }
        public LMSStudentDTO getcollegedocuments(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/getcollegedocuments");
        }


        // School 
        public LMSStudentDTO Getdetailsschool(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/Getdetailsschool");
        }
        public LMSStudentDTO onchangeyear(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/onchangeyear");
        }
        public LMSStudentDTO onchangeclass(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/onchangeclass");
        }
        public LMSStudentDTO getschooltopics(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/getschooltopics");
        }
        public LMSStudentDTO getschooldocuments(LMSStudentDTO data)
        {
            return _comm.naacdetailsbypost(data, "LMSStudentFacade/getschooldocuments");
        }
    }
}
