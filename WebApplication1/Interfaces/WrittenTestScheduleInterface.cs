using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface WrittenTestScheduleInterface
    {
        Task<WrittenTestScheduleDTO> WrittenTestScheduleData(WrittenTestScheduleDTO mas);
         WrittenTestScheduleDTO WrittenTestScheduleDeletesData(int ID);
      //  WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData(int ID,int MID);
         WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData(WrittenTestScheduleDTO WrittenTestScheduleDTO);
        WrittenTestScheduleDTO GetSelectedRowDetails(int ID);
        StudentDetailsDTO GetSelectedStudentData(int ID);
        StudentDetailsDTO GetWrittenTestScheduleData(StudentDetailsDTO StudentDetailsDTO);

       // WrittenTestScheduleDTO GetWrittenTestScheduleData(WrittenTestScheduleDTO WrittenTestScheduleDTO);


       
    }
}
