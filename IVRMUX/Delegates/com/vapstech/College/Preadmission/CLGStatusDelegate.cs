using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class CLGStatusDelegate 
    {
        CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto> common = new CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto>();
        CommonDelegate<Master_Competitive_ExamsClgDTO, Master_Competitive_ExamsClgDTO> comm = new CommonDelegate<Master_Competitive_ExamsClgDTO, Master_Competitive_ExamsClgDTO>();
        public CollegePreadmissionstudnetDto Getdetails(CollegePreadmissionstudnetDto obj)
        {
            return common.CollegePOSTData(obj, "CLGStatusFacade/Getdetails/");
        }
        public CollegePreadmissionstudnetDto getCourse(CollegePreadmissionstudnetDto data)
        {
            return common.CollegePOSTData(data, "CLGStatusFacade/getCourse/");
        }
        public CollegePreadmissionstudnetDto getBranch(CollegePreadmissionstudnetDto dt)
        {
            return common.CollegePOSTData(dt, "CLGStatusFacade/getBranch/");
        }
        public CollegePreadmissionstudnetDto SearchData(CollegePreadmissionstudnetDto dt)
        {
            return common.CollegePOSTData(dt, "CLGStatusFacade/SearchData/");
        }

        //master competitive exam
        public Master_Competitive_ExamsClgDTO getexamdetails(Master_Competitive_ExamsClgDTO obj)
        {
            return comm.CollegePOSTData(obj, "CLGStatusFacade/getexamdetails/");
        }

        public Master_Competitive_ExamsClgDTO saveExamDetails(Master_Competitive_ExamsClgDTO add)
        {
            return comm.CollegePOSTData(add, "CLGStatusFacade/saveExamDetails/");
        }

        public Master_Competitive_ExamsClgDTO saveExamMapDetails(Master_Competitive_ExamsClgDTO add)
        {
            return comm.CollegePOSTData(add, "CLGStatusFacade/saveExamMapDetails/");
        }

        public Master_Competitive_ExamsClgDTO getexamedit(int id)
        {
            return comm.CollegeGetDataById(id, "CLGStatusFacade/getexamedit/");
        }
        public Master_Competitive_ExamsClgDTO getsubedit(int id)
        {
            return comm.CollegeGetDataById(id, "CLGStatusFacade/getsubedit/");
        }

        public Master_Competitive_ExamsClgDTO deleterecord(int id)
        {
            return comm.CollegeGetDataById(id, "CLGStatusFacade/deleterecord/");
        }

        public Master_Competitive_ExamsClgDTO deleterecordsub(int id)
        {
            return comm.CollegeGetDataById(id, "CLGStatusFacade/deleterecordsub/");
        }

    }
}
