
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MasterLifeSkillInterface
    {
        MasterLifeSkillDTO savedata(MasterLifeSkillDTO data); 
        MasterLifeSkillDTO deactivate(MasterLifeSkillDTO data);
        MasterLifeSkillDTO editdetails(MasterLifeSkillDTO data);
        MasterLifeSkillDTO Getdetails(MasterLifeSkillDTO data);

        //Master Life Skill Area
        MasterLifeSkillDTO Savedataarea(MasterLifeSkillDTO data);
        MasterLifeSkillDTO editdetailsarea(MasterLifeSkillDTO data);
        MasterLifeSkillDTO deactivatearea(MasterLifeSkillDTO data);
        MasterLifeSkillDTO validateordernumber(MasterLifeSkillDTO data);
        //Master Life Skill Area
        MasterLifeSkillDTO Savedataareamapping(MasterLifeSkillDTO data);
        MasterLifeSkillDTO editdetailsareamapping(MasterLifeSkillDTO data);
        MasterLifeSkillDTO deactivateareamapping(MasterLifeSkillDTO data);
        MasterLifeSkillDTO getgrade(MasterLifeSkillDTO data);
        
    }
}
