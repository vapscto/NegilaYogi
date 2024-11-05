using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SubjectwisePeriodSettingsInterface
    {
        SubjectwisePeriodSettingsDTO GetData(SubjectwisePeriodSettingsDTO SubjectwisePeriodSettingsDTO);

        SubjectwisePeriodSettingsDTO SaveData(SubjectwisePeriodSettingsDTO mas);
        SubjectwisePeriodSettingsDTO subjectMaxPeriod(SubjectwisePeriodSettingsDTO mas);
        

    }
}
