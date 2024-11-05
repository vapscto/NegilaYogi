using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface InstitutionTemplateInterface
    {
        Task<CommonDTO> getBasicData(int id);
        InstitutionTemplateDTO getEditData(int Id);
        InstitutionTemplateDTO SaveInstituteTemp(InstitutionTemplateDTO InstTemp);
        void deleteRec(int id);
        void DeactiveActive(int id);
    }
}
