using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class Naac_MC_CR4_Delegate
    {

        CommonDelegate<Naac_MC_CR4_DTO, Naac_MC_CR4_DTO> comm = new CommonDelegate<Naac_MC_CR4_DTO, Naac_MC_CR4_DTO>();
        public Naac_MC_CR4_DTO loaddata(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/loaddata");
        }
        public Naac_MC_CR4_DTO Report(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/Report");
        }
        public Naac_MC_CR4_DTO InOutPatientReport(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/InOutPatientReport");
        }
        public Naac_MC_CR4_DTO MEDStudentExposed423Report(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/MEDStudentExposed423Report");
        }
        public Naac_MC_CR4_DTO Membership433Report(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/Membership433Report");
        }
        public Naac_MC_CR4_DTO MedExpenditure434Report(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/MedExpenditure434Report");
        }
        public Naac_MC_CR4_DTO Econtent436Report(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/Econtent436Report");
        }
        public Naac_MC_CR4_DTO PhyAcaFacility451Report(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/PhyAcaFacility451Report");
        }
        public Naac_MC_CR4_DTO ClassSeminarhall441Report(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/ClassSeminarhall441Report");
        }
        public Naac_MC_CR4_DTO BandwidthRangeReport(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/BandwidthRangeReport");
        }
        public Naac_MC_CR4_DTO InfrastructureReport(Naac_MC_CR4_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_CR4_Facade/InfrastructureReport");
        }
    }
}
