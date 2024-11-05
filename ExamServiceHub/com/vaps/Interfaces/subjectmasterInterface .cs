
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface subjectmasterInterface
    {
        subjectmasterDTO subjectmasterData(subjectmasterDTO mas);

        subjectmasterDTO MasterDeleteModulesData(int ID);
        subjectmasterDTO GetSelectedRowDetails(int ID);

        subjectmasterDTO GetsubjectmasterData(subjectmasterDTO subjectmasterDTO);
    }
}
