
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

namespace ExamServiceHub.com.vaps.Interfaces
{
 public   interface MeritListInterface
    {
        MeritListDTO getdetails(MeritListDTO data);
        MeritListDTO onchangeyear(MeritListDTO data);
        MeritListDTO onchangeclass(MeritListDTO data);
        MeritListDTO onchangesection(MeritListDTO data);
        MeritListDTO getAttendetails(MeritListDTO data);
        MeritListDTO getreport(MeritListDTO data);
    }
}
