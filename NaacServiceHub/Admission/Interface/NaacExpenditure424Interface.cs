using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
    public interface NaacExpenditure424Interface
    {
        NaacExpenditure424DTO save(NaacExpenditure424DTO data);
        NaacExpenditure424DTO loaddata(NaacExpenditure424DTO data);
        NaacExpenditure424DTO deactiveStudent(NaacExpenditure424DTO data);
        NaacExpenditure424DTO EditData(NaacExpenditure424DTO data);
        NaacExpenditure424DTO savemedicaldatawisecomments(NaacExpenditure424DTO data);
        NaacExpenditure424DTO savefilewisecomments(NaacExpenditure424DTO data);
        NaacExpenditure424DTO getcomment(NaacExpenditure424DTO data);
        NaacExpenditure424DTO getfilecomment(NaacExpenditure424DTO data);
        NaacExpenditure424DTO viewuploadflies(NaacExpenditure424DTO data);
        NaacExpenditure424DTO deleteuploadfile(NaacExpenditure424DTO data);
    }
}
