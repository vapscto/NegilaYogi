using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Exam
{
    public class StudentTransactionDelegate
    {
        CommonDelegate<StudentTransactionDTO, StudentTransactionDTO> COMSPRT = new CommonDelegate<StudentTransactionDTO, StudentTransactionDTO>();

        public StudentTransactionDTO getDetails(StudentTransactionDTO data)
        {
            return COMSPRT.POSTDataExam(data, "StudentTransactionFacade/getDetails/");
        }
        public StudentTransactionDTO onchangeyear(StudentTransactionDTO data)
        {
            return COMSPRT.POSTDataExam(data, "StudentTransactionFacade/onchangeyear/");
        }
        public StudentTransactionDTO onchangeclass(StudentTransactionDTO data)
        {
            return COMSPRT.POSTDataExam(data, "StudentTransactionFacade/onchangeclass/");
        }
        public StudentTransactionDTO getStudentList(StudentTransactionDTO data)
        {
            return COMSPRT.POSTDataExam(data, "StudentTransactionFacade/getStudentList/");
        }
        public StudentTransactionDTO onchangesection(StudentTransactionDTO data)
        {
            return COMSPRT.POSTDataExam(data, "StudentTransactionFacade/onchangesection/");
        } 
        public StudentTransactionDTO onchangeskills(StudentTransactionDTO data)
        {
            return COMSPRT.POSTDataExam(data, "StudentTransactionFacade/onchangeskills/");
        } 
        public StudentTransactionDTO onchangeactivites(StudentTransactionDTO data)
        {
            return COMSPRT.POSTDataExam(data, "StudentTransactionFacade/onchangeactivites/");
        }        
        public StudentTransactionDTO save(StudentTransactionDTO obj)
        {
            return COMSPRT.POSTDataExam(obj, "StudentTransactionFacade/save/");
        }
        public StudentTransactionDTO EditDetails(int id)
        {
            return COMSPRT.GetDataByIdExam(id, "StudentTransactionFacade/EditDetails/");
        }
        public StudentTransactionDTO deactivate(StudentTransactionDTO obj)
        {
            return COMSPRT.POSTDataExam(obj, "StudentTransactionFacade/deactivate/");
        }
        public StudentTransactionDTO onchangeactivitesskillflag(StudentTransactionDTO obj)
        {
            return COMSPRT.POSTDataExam(obj, "StudentTransactionFacade/onchangeactivitesskillflag/");
        }
    }
}
