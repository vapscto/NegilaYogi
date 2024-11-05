
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MasterLifeSkillAreaMappingInterface
    {
        MasterLifeSkillAreaMappingDTO savedata(MasterLifeSkillAreaMappingDTO data);
        MasterLifeSkillAreaMappingDTO deactivate(MasterLifeSkillAreaMappingDTO data);
        MasterLifeSkillAreaMappingDTO editdetails(int ID);
        MasterLifeSkillAreaMappingDTO Getdetails(MasterLifeSkillAreaMappingDTO data);
        MasterLifeSkillAreaMappingDTO getgrade(MasterLifeSkillAreaMappingDTO data);
    }
}
