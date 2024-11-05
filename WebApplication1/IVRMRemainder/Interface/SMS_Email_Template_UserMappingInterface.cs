using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.IVRMRemainder.Interface
{
    public interface SMS_Email_Template_UserMappingInterface
    {
        IVRM_RemaindersDTO OnChangeOfInstitution(IVRM_RemaindersDTO data);
        IVRM_RemaindersDTO OnSaveUserMapping(IVRM_RemaindersDTO data);
    }
}
