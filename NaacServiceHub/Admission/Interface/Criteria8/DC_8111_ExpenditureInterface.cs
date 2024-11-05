using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria8
{
    public interface DC_8111_ExpenditureInterface
    {
        Task<DC_8111_ExpenditureDTO> loaddata(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO savedata(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO editdata(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO deactivY(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO viewuploadflies(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO deleteuploadfile(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO getcomment(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO getfilecomment(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO savecomments(DC_8111_ExpenditureDTO data);
        DC_8111_ExpenditureDTO savefilewisecomments(DC_8111_ExpenditureDTO data);
    }
}
