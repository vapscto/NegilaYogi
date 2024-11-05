using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NaacBudget_414_Interface
    {
        NaacBudget_414_DTO loaddata(NaacBudget_414_DTO data);
        NaacBudget_414_DTO save(NaacBudget_414_DTO data);
        NaacBudget_414_DTO EditData(NaacBudget_414_DTO data);
        NaacBudget_414_DTO deactiveStudent(NaacBudget_414_DTO data);
        NaacBudget_414_DTO getcomment(NaacBudget_414_DTO data);
        NaacBudget_414_DTO savemedicaldatawisecomments(NaacBudget_414_DTO data);
        NaacBudget_414_DTO getfilecomment(NaacBudget_414_DTO data);
        NaacBudget_414_DTO savefilewisecomments(NaacBudget_414_DTO data);
        NaacBudget_414_DTO viewuploadflies(NaacBudget_414_DTO data);
        NaacBudget_414_DTO deleteuploadfile(NaacBudget_414_DTO data);
    }
}
