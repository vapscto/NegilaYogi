using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class VaccineAgeCriteriaDelegate
    {
        CommonDelegate<VaccineAgeCriteriaDTO, VaccineAgeCriteriaDTO> _delg = new CommonDelegate<VaccineAgeCriteriaDTO, VaccineAgeCriteriaDTO>();

        public VaccineAgeCriteriaDTO OnLoadVaccineAgeCriteriaDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/OnLoadVaccineAgeCriteriaDetails");
        }
        public VaccineAgeCriteriaDTO SaveVaccineAgeDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/SaveVaccineAgeDetails");
        }
        public VaccineAgeCriteriaDTO EditVaccineAgeDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/EditVaccineAgeDetails");
        }
        public VaccineAgeCriteriaDTO ActiveDeactiveVaccineAgeDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/ActiveDeactiveVaccineAgeDetails");
        }
        public VaccineAgeCriteriaDTO OnClickViewDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/OnClickViewDetails");
        }
        public VaccineAgeCriteriaDTO ActiveDeactiveVaccineDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/ActiveDeactiveVaccineDetails");
        }

        // Vaccine Student Details
        public VaccineAgeCriteriaDTO OnLoadVaccineStudentDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/OnLoadVaccineStudentDetails");
        }
        public VaccineAgeCriteriaDTO GetStudentDetailsBySearch(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/GetStudentDetailsBySearch");
        }
        public VaccineAgeCriteriaDTO SearchVaccineStudentDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/SearchVaccineStudentDetails");
        }
        public VaccineAgeCriteriaDTO SaveStudentVaccineDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/SaveStudentVaccineDetails");
        }
        public VaccineAgeCriteriaDTO OnClickViewStudentVaccineDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/OnClickViewStudentVaccineDetails");
        }
        public VaccineAgeCriteriaDTO OnLoadIllnessStudentDetails(VaccineAgeCriteriaDTO data)
        {
          return  _delg.POSTDataADM(data, "VaccineAgeCriteriaFacade/OnLoadIllnessStudentDetails");
        }
    }
}