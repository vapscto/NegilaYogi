using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class CLGSubjectSchemeTypeDelegate
    {
        CommonDelegate<AdmCollegeSchemeTypeDTO, AdmCollegeSchemeTypeDTO> _commschemetype = new CommonDelegate<AdmCollegeSchemeTypeDTO, AdmCollegeSchemeTypeDTO>();
        CommonDelegate<Adm_Prv_Sch_CombinationDTO, Adm_Prv_Sch_CombinationDTO> _commcomb = new CommonDelegate<Adm_Prv_Sch_CombinationDTO, Adm_Prv_Sch_CombinationDTO>();

        public AdmCollegeSchemeTypeDTO savetype(AdmCollegeSchemeTypeDTO data)
        {
            return _commschemetype.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/savetype/");
        }
        public AdmCollegeSchemeTypeDTO savename(AdmCollegeSchemeTypeDTO data)
        {
            return _commschemetype.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/savename/");
        }
        public AdmCollegeSchemeTypeDTO getschema(AdmCollegeSchemeTypeDTO data)
        {
            return _commschemetype.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/getschema/");
        }
        public AdmCollegeSchemeTypeDTO getsubject(AdmCollegeSchemeTypeDTO data)
        {
            return _commschemetype.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/getsubject/");
        }
        public AdmCollegeSchemeTypeDTO activedeactivebatch(AdmCollegeSchemeTypeDTO data)
        {
            return _commschemetype.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/activedeactivebatch/");
        }
        public AdmCollegeSchemeTypeDTO activedeactivebatch1(AdmCollegeSchemeTypeDTO data)
        {
            return _commschemetype.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/activedeactivebatch1/");
        }

        public Adm_Prv_Sch_CombinationDTO getcombination(Adm_Prv_Sch_CombinationDTO data)
        {
            return _commcomb.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/getcombination/");
        }
        public Adm_Prv_Sch_CombinationDTO savecombination(Adm_Prv_Sch_CombinationDTO data)
        {
            return _commcomb.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/savecombination/");
        }
        public Adm_Prv_Sch_CombinationDTO activedeactivecomb(Adm_Prv_Sch_CombinationDTO data)
        {
            return _commcomb.clgadmissionbypost(data, "CLGSubjectSchemeTypeFacade/activedeactivecomb/");
        }
        
    }
}
