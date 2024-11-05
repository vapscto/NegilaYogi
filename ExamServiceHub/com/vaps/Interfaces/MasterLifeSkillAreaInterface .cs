
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MasterLifeSkillAreaInterface
    {
        MasterLifeSkillAreaDTO savedata(MasterLifeSkillAreaDTO data);
        MasterLifeSkillAreaDTO deactivate(MasterLifeSkillAreaDTO data);
        MasterLifeSkillAreaDTO editdetails(int ID);
        MasterLifeSkillAreaDTO Getdetails(MasterLifeSkillAreaDTO data);
        MasterLifeSkillAreaDTO validateordernumber(MasterLifeSkillAreaDTO data);
    }
}
