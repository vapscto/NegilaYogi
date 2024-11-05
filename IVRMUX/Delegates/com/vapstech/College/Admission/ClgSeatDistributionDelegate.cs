using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgSeatDistributionDelegate
    {
        CommonDelegate<ClgSeatDistributionDTO, ClgSeatDistributionDTO> _commbranch = new CommonDelegate<ClgSeatDistributionDTO, ClgSeatDistributionDTO>();
        CommonDelegate<Master_Competitive_AdmExamsClgDTO, Master_Competitive_AdmExamsClgDTO> _commexam = new CommonDelegate<Master_Competitive_AdmExamsClgDTO, Master_Competitive_AdmExamsClgDTO>();

        public ClgSeatDistributionDTO getalldetails(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/getalldetails/");
        }
        public ClgSeatDistributionDTO getCoursedata(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/getCoursedata/");
        }
        public ClgSeatDistributionDTO getBranchdata(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/getBranchdata/");

        }
        public ClgSeatDistributionDTO getSemesterdata(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/getSemesterdata/");
        }
        public ClgSeatDistributionDTO get_Category(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/get_Category/");
        }

        public ClgSeatDistributionDTO savedata(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/savedata/");
        }

        public ClgSeatDistributionDTO get_Seattotal(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/get_Seattotal/");
        }
        //master competitive exam
        public Master_Competitive_AdmExamsClgDTO getexamdetails(Master_Competitive_AdmExamsClgDTO obj)
        {
            return _commexam.clgadmissionbypost(obj, "ClgSeatDistributionFacade/getexamdetails/");
        }

        public Master_Competitive_AdmExamsClgDTO saveExamDetails(Master_Competitive_AdmExamsClgDTO add)
        {
            return _commexam.clgadmissionbypost(add, "ClgSeatDistributionFacade/saveExamDetails/");
        }

        public Master_Competitive_AdmExamsClgDTO saveExamMapDetails(Master_Competitive_AdmExamsClgDTO add)
        {
            return _commexam.clgadmissionbypost(add, "ClgSeatDistributionFacade/saveExamMapDetails/");
        }

        public Master_Competitive_AdmExamsClgDTO getexamedit(int id)
        {
            return _commexam.clgadmissionbyid(id, "ClgSeatDistributionFacade/getexamedit/");
        }
        public Master_Competitive_AdmExamsClgDTO getsubedit(int id)
        {
            return _commexam.clgadmissionbyid(id, "ClgSeatDistributionFacade/getsubedit/");
        }

        public Master_Competitive_AdmExamsClgDTO deleterecord(int id)
        {
            return _commexam.clgadmissionbyid(id, "ClgSeatDistributionFacade/deleterecord/");
        }

        public Master_Competitive_AdmExamsClgDTO deleterecordsub(int id)
        {
            return _commexam.clgadmissionbyid(id, "ClgSeatDistributionFacade/deleterecordsub/");
        }
    }
}
