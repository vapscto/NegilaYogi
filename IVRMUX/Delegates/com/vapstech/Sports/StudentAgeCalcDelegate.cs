using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class StudentAgeCalcDelegate
    {
        CommonDelegate<StudentAgeCalcDTO, StudentAgeCalcDTO> COMSPRT = new CommonDelegate<StudentAgeCalcDTO, StudentAgeCalcDTO>();

        public StudentAgeCalcDTO Getdetails(StudentAgeCalcDTO data)
        {
            return COMSPRT.POSTDataSports(data, "StudentAgeCalcFacade/Getdetails/");
        }
        public StudentAgeCalcDTO getStudents(StudentAgeCalcDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "StudentAgeCalcFacade/getStudents/");
        }
        public StudentAgeCalcDTO save(StudentAgeCalcDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "StudentAgeCalcFacade/save/");
        }

        internal StudentAgeCalcDTO Getdetails(House_Report_DTO data)
        {
            throw new NotImplementedException();
        }

        public StudentAgeCalcDTO report(StudentAgeCalcDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "StudentAgeCalcFacade/report/");
        }
        public StudentAgeCalcDTO Get_Class_House(StudentAgeCalcDTO data)
        {
            return COMSPRT.POSTDataSports(data, "StudentAgeCalcFacade/Get_Class_House/");
        }

    }
}
