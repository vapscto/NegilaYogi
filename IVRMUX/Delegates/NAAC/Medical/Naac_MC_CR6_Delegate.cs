using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class Naac_MC_CR6_Delegate
    {

        CommonDelegate<Naac_MC_CR6_DTO, Naac_MC_CR6_DTO> comm = new CommonDelegate<Naac_MC_CR6_DTO, Naac_MC_CR6_DTO>();
        public Naac_MC_CR6_DTO loaddata(Naac_MC_CR6_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR6Facade/loaddata");
        }
        public Naac_MC_CR6_DTO MedFinancialSupport632Report(Naac_MC_CR6_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR6Facade/MedFinancialSupport632Report");
        }
        public Naac_MC_CR6_DTO MedDevPrograms634634Report(Naac_MC_CR6_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR6Facade/MedDevPrograms634634Report");
        }
        public Naac_MC_CR6_DTO MedFunds643Report(Naac_MC_CR6_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR6Facade/MedFunds643Report");
        }
        public Naac_MC_CR6_DTO MedIQAC652Report(Naac_MC_CR6_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR6Facade/MedIQAC652Report");
        }
        public Naac_MC_CR6_DTO MEDInternalQuality653(Naac_MC_CR6_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR6Facade/MEDInternalQuality653");
        }
        public Naac_MC_CR6_DTO MedEGovernance622Report(Naac_MC_CR6_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR6Facade/MedEGovernance622Report");
        }
        public Naac_MC_CR6_DTO MedDevPrograms633Report(Naac_MC_CR6_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR6Facade/MedDevPrograms633Report");
        }
    }
}
