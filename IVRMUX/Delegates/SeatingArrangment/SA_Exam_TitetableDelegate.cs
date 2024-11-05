using CommonLibrary;
using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.SeatingArrangment
{
    public class SA_Exam_TitetableDelegate
    {
        CommonDelegate<SA_Exam_TitetableDTO, SA_Exam_TitetableDTO> COMM = new CommonDelegate<SA_Exam_TitetableDTO, SA_Exam_TitetableDTO>();

        public SA_Exam_TitetableDTO load_TT(SA_Exam_TitetableDTO dto)
        {
            return COMM.SeatingArrangmentPOST(dto, "SA_Exam_TitetableFacade/load_TT");
        }
        public SA_Exam_TitetableDTO Save_TT(SA_Exam_TitetableDTO dto)
        {
            return COMM.SeatingArrangmentPOST(dto, "SA_Exam_TitetableFacade/Save_TT");
        }
        public SA_Exam_TitetableDTO Edit_TT(SA_Exam_TitetableDTO dto)
        {
            return COMM.SeatingArrangmentPOST(dto, "SA_Exam_TitetableFacade/Edit_TT");
        }
        public SA_Exam_TitetableDTO Deactive_TT(SA_Exam_TitetableDTO dto)
        {
            return COMM.SeatingArrangmentPOST(dto, "SA_Exam_TitetableFacade/Deactive_TT");
        }
         public SA_Exam_TitetableDTO viewTTdetails(SA_Exam_TitetableDTO dto)
        {
            return COMM.SeatingArrangmentPOST(dto, "SA_Exam_TitetableFacade/viewTTdetails");
        }

    }
}
