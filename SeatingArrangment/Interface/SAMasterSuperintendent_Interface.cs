using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Interface
{
    public interface SAMasterSuperintendent_Interface
    {
        // ===============superintendent==================
        SAMasterSuperintendent load_sup(SAMasterSuperintendent data);
        SAMasterSuperintendent Save_sup(SAMasterSuperintendent data);
        SAMasterSuperintendent Edit_sup(SAMasterSuperintendent data);
        SAMasterSuperintendent ActiveDeactive_sup(SAMasterSuperintendent data);
        //=========Absent Student======================
        SAMasterSuperintendent load_AS(SAMasterSuperintendent data);
        SAMasterSuperintendent Save_AS(SAMasterSuperintendent data);
        SAMasterSuperintendent Edit_AS(SAMasterSuperintendent data);
        SAMasterSuperintendent DeleteAbsentStudent(SAMasterSuperintendent data);

        //=========Malpractice Student======================

        SAMasterSuperintendent load_MPS(SAMasterSuperintendent data);
        SAMasterSuperintendent Save_MPS(SAMasterSuperintendent data);
        SAMasterSuperintendent Edit_MPS(SAMasterSuperintendent data);
        SAMasterSuperintendent DeleteMalPraticeStudent(SAMasterSuperintendent data);       

        //=========Chief coordinator======================

        SAMasterSuperintendent load_CC(SAMasterSuperintendent data);
        SAMasterSuperintendent Save_CC(SAMasterSuperintendent data);
        SAMasterSuperintendent Edit_CC(SAMasterSuperintendent data);
        SAMasterSuperintendent ActiveDeactive_CC(SAMasterSuperintendent data);

        // ****************** General Selection ************************ //
        SAMasterSuperintendent GetCourse(SAMasterSuperintendent data);
        SAMasterSuperintendent GetBranch(SAMasterSuperintendent data);
        SAMasterSuperintendent GetSemester(SAMasterSuperintendent data);
        SAMasterSuperintendent GetSubject(SAMasterSuperintendent data);
        SAMasterSuperintendent GetStudent(SAMasterSuperintendent data);
    }
}
