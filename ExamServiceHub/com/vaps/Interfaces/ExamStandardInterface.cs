﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamStandardInterface
    {
        ExamStandardDTO savedetails(ExamStandardDTO data); 
        ExamStandardDTO Getdetails(int  id);
    }
}
