using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria8
{
   public interface NC_818_EmpCommitteesInterface
    {
        Task<NC_818_EmpCommitteesDTO> loaddata(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO savedata(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO editdata(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO deactivY(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO viewuploadflies(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO deleteuploadfile(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO getcomment(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO getfilecomment(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO savecomments(NC_818_EmpCommitteesDTO data);
        NC_818_EmpCommitteesDTO savefilewisecomments(NC_818_EmpCommitteesDTO data);
    }
}
