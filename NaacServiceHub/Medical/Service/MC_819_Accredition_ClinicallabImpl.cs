using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Dental;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class MC_819_Accredition_ClinicallabImpl:Interface.MC_819_Accredition_ClinicallabInterface
    {
        public GeneralContext _GeneralContext;
        public MC_819_Accredition_ClinicallabImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public MC_819_Accredition_ClinicallabDTO loaddata(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {
                var institutionlist = (from a in _GeneralContext.Institution
                                       from b in _GeneralContext.UserRoleWithInstituteDMO
                                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }

                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new MC_819_Accredition_ClinicallabDTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                
                data.alldata819MC = (from a in _GeneralContext.MC_819_Accredition_ClinicallabDMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id && a.NCMCCL819_Year==y.ASMAY_Id)
                                   select new MC_819_Accredition_ClinicallabDTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       NCMCCL819_Id = a.NCMCCL819_Id,
                                       NCMCCL819_Year = a.NCMCCL819_Year,
                                       NCMCCL819_NABHAccnTechHoslFlg = a.NCMCCL819_NABHAccnTechHoslFlg,
                                       NCMCCL819_NABHAccnTechlabslFlg = a.NCMCCL819_NABHAccnTechlabslFlg,
                                       NCMCCL819_CertificationDeptlFlg = a.NCMCCL819_CertificationDeptlFlg,
                                       NCMCCL819_OtherRecAccCertificationFlg = a.NCMCCL819_OtherRecAccCertificationFlg,                                     
                                       MI_Id = a.MI_Id,
                                   }).Distinct().OrderByDescending(t => t.NCMCCL819_Id).ToArray();

                data.alldata813DC = (from a in _GeneralContext.DC_813_ClinicalTeachingDMO
                                     from y in _GeneralContext.Academic
                                     where (a.MI_Id == data.MI_Id && a.NCDCCL813_Year==y.ASMAY_Id)
                                     select new MC_819_Accredition_ClinicallabDTO
                                     {
                                         ASMAY_Year = y.ASMAY_Year,
                                         NCMCCL819_Id = a.NCDCCL813_Id,
                                         NCDCCL813_Year = a.NCDCCL813_Year,
                                         NCDCCL813_CentralSterileSuppliesDepartmentFlag = a.NCDCCL813_CentralSterileSuppliesDepartmentFlag,
                                         NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag = a.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag,
                                         NCDCCL813_PatientSafetyCurriculumFlag = a.NCDCCL813_PatientSafetyCurriculumFlag,
                                         NCDCCL813_PeriodicFumigationClinicalAreasFlag = a.NCDCCL813_PeriodicFumigationClinicalAreasFlag,
                                         NCDCCL813_ImmunizationOfAllTheCaregiversFlag=a.NCDCCL813_ImmunizationOfAllTheCaregiversFlag,
                                         NCDCCL813_NeedleStickInjuryRegisterFlag=a.NCDCCL813_NeedleStickInjuryRegisterFlag,
                                         MI_Id = a.MI_Id,
                                     }).Distinct().OrderByDescending(t => t.NCDCCL813_Id).ToArray();

                data.alldata815DC = (from a in _GeneralContext.DC_815_EquipmentTrainingDMO
                                     from y in _GeneralContext.Academic
                                     where (a.MI_Id == data.MI_Id && a.NCDCEQT815_Year==y.ASMAY_Id)
                                     select new MC_819_Accredition_ClinicallabDTO
                                     {
                                         ASMAY_Year = y.ASMAY_Year,
                                         NCDCEQT815_Id = a.NCDCEQT815_Id,
                                         NCDCEQT815_Year = a.NCDCEQT815_Year,
                                         NCDCEQT815_ConeBeamComputedTomogramFlag = a.NCDCEQT815_ConeBeamComputedTomogramFlag,
                                         NCDCEQT815_CAMFacilityFlag = a.NCDCEQT815_CAMFacilityFlag,
                                         NCDCEQT815_ImagingMorphomEtricSoftwaresFlag = a.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag,
                                         NCDCEQT815_DentalLASERUnitFlag = a.NCDCEQT815_DentalLASERUnitFlag,
                                         NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag=a.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag,
                                         MI_Id = a.MI_Id,
                                     }).Distinct().OrderByDescending(t => t.NCDCEQT815_Id).ToArray();

                data.alldata816DC = (from a in _GeneralContext.DC_816_SpecializedClinicsDMO
                                     from y in _GeneralContext.Academic
                                     where (a.MI_Id == data.MI_Id && a.NCDCSC816_Year==y.ASMAY_Id)
                                     select new MC_819_Accredition_ClinicallabDTO
                                     {
                                         ASMAY_Year = y.ASMAY_Year,
                                         NCDCSC816_Id = a.NCDCSC816_Id,
                                         NCDCSC816_Year=a.NCDCSC816_Year,
                                         NCDCSC816_ComprehensiveclinicFlag = a.NCDCSC816_ComprehensiveclinicFlag,
                                         NCDCSC816_ImplantClinicFlag = a.NCDCSC816_ImplantClinicFlag,
                                         NCDCSC816_GeriatricClinicFlag = a.NCDCSC816_GeriatricClinicFlag,
                                         NCDCSC816_SpecialHealthCareNeedsClinicFlag = a.NCDCSC816_SpecialHealthCareNeedsClinicFlag,
                                         NCDCSC816_EstheticClinicFlag=a.NCDCSC816_EstheticClinicFlag,      NCDCSC816_TobaccoCessationClinicFlag =a.NCDCSC816_TobaccoCessationClinicFlag,
                                         MI_Id = a.MI_Id,
                                     }).Distinct().OrderByDescending(t => t.NCDCSC816_Id).ToArray();


            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public MC_819_Accredition_ClinicallabDTO savedata(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {

                if (data.NCMCCL819_Id == 0)
                {
                    var duplicate = _GeneralContext.MC_819_Accredition_ClinicallabDMO.Where(t => t.MI_Id == data.MI_Id 
                    && t.NCMCCL819_Year==data.ASMAY_Id && t.NCMCCL819_NABHAccnTechHoslFlg == data.NCMCCL819_NABHAccnTechHoslFlg && t.NCMCCL819_NABHAccnTechlabslFlg == data.NCMCCL819_NABHAccnTechlabslFlg && t.NCMCCL819_CertificationDeptlFlg == data.NCMCCL819_CertificationDeptlFlg && t.NCMCCL819_OtherRecAccCertificationFlg == data.NCMCCL819_OtherRecAccCertificationFlg).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MC_819_Accredition_ClinicallabDMO obj1 = new MC_819_Accredition_ClinicallabDMO();


                        //obj1.NCMCVAC141_Id = data.NCMCVAC141_Id;
                        obj1.MI_Id = data.MI_Id;                        
                        obj1.NCMCCL819_Year = data.ASMAY_Id;                        
                        obj1.NCMCCL819_NABHAccnTechHoslFlg = data.NCMCCL819_NABHAccnTechHoslFlg;
                        obj1.NCMCCL819_NABHAccnTechlabslFlg = data.NCMCCL819_NABHAccnTechlabslFlg;
                        obj1.NCMCCL819_CertificationDeptlFlg = data.NCMCCL819_CertificationDeptlFlg;
                       obj1.NCMCCL819_OtherRecAccCertificationFlg =data.NCMCCL819_OtherRecAccCertificationFlg;
                        obj1.NCMCCL819_ActiveFlag = true;
                        obj1.NCMCCL819_Year = data.ASMAY_Id;
                        obj1.NCMCCL819_CreatedDate = DateTime.Now;
                        obj1.NCMCCL819_UpdatedDate = DateTime.Now;
                        obj1.NCMCCL819_CreatedBy = data.UserId;
                        obj1.NCMCCL819_UpdatedBy = data.UserId;

                        _GeneralContext.Add(obj1);

                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCMCCL819_Id > 0)
                {
                    var duplicate = _GeneralContext.MC_819_Accredition_ClinicallabDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCCL819_Year==data.ASMAY_Id && t.NCMCCL819_Id != data.NCMCCL819_Id &&
                    t.NCMCCL819_NABHAccnTechHoslFlg == data.NCMCCL819_NABHAccnTechHoslFlg && t.NCMCCL819_NABHAccnTechlabslFlg == data.NCMCCL819_NABHAccnTechlabslFlg && t.NCMCCL819_CertificationDeptlFlg == data.NCMCCL819_CertificationDeptlFlg && t.NCMCCL819_OtherRecAccCertificationFlg == data.NCMCCL819_OtherRecAccCertificationFlg).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.MC_819_Accredition_ClinicallabDMO.Where(t => t.NCMCCL819_Id == data.NCMCCL819_Id && t.MI_Id == data.MI_Id).Single();


                       update.NCMCCL819_Year = data.ASMAY_Id;
                        update.NCMCCL819_NABHAccnTechHoslFlg = data.NCMCCL819_NABHAccnTechHoslFlg;
                        update.NCMCCL819_NABHAccnTechlabslFlg = data.NCMCCL819_NABHAccnTechlabslFlg;
                        update.NCMCCL819_CertificationDeptlFlg = data.NCMCCL819_CertificationDeptlFlg;
                        update.NCMCCL819_OtherRecAccCertificationFlg = data.NCMCCL819_OtherRecAccCertificationFlg;
                        
                        update.NCMCCL819_UpdatedDate = DateTime.Now;
                        update.NCMCCL819_UpdatedBy = data.UserId;

                        _GeneralContext.Update(update);


                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MC_819_Accredition_ClinicallabDTO savedata1(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {

                if (data.NCDCCL813_Id == 0)
                {
                    var duplicate = _GeneralContext.DC_813_ClinicalTeachingDMO.Where(t => t.MI_Id == data.MI_Id && t.NCDCCL813_Year==data.ASMAY_Id && t.NCDCCL813_CentralSterileSuppliesDepartmentFlag == data.NCDCCL813_CentralSterileSuppliesDepartmentFlag && t.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag == data.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag && t.NCDCCL813_PatientSafetyCurriculumFlag == data.NCDCCL813_PatientSafetyCurriculumFlag && t.NCDCCL813_PeriodicFumigationClinicalAreasFlag == data.NCDCCL813_PeriodicFumigationClinicalAreasFlag && t.NCDCCL813_ImmunizationOfAllTheCaregiversFlag==data.NCDCCL813_ImmunizationOfAllTheCaregiversFlag && t.NCDCCL813_NeedleStickInjuryRegisterFlag==data.NCDCCL813_NeedleStickInjuryRegisterFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        DC_813_ClinicalTeachingDMO obj1 = new DC_813_ClinicalTeachingDMO();


                        //obj1.NCMCVAC141_Id = data.NCMCVAC141_Id;
                        obj1.MI_Id = data.MI_Id;                        
                        obj1.NCDCCL813_Year = data.ASMAY_Id;                        
                        obj1.NCDCCL813_CentralSterileSuppliesDepartmentFlag = data.NCDCCL813_CentralSterileSuppliesDepartmentFlag;
                        obj1.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag = data.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag;
                        obj1.NCDCCL813_PatientSafetyCurriculumFlag = data.NCDCCL813_PatientSafetyCurriculumFlag;
                       obj1.NCDCCL813_PeriodicFumigationClinicalAreasFlag = data.NCDCCL813_PeriodicFumigationClinicalAreasFlag; obj1.NCDCCL813_ImmunizationOfAllTheCaregiversFlag = data.NCDCCL813_ImmunizationOfAllTheCaregiversFlag; obj1.NCDCCL813_NeedleStickInjuryRegisterFlag = data.NCDCCL813_NeedleStickInjuryRegisterFlag;
                        obj1.NCDCCL813_ActiveFlag = true;
                        obj1.NCDCCL813_CreatedDate = DateTime.Now;
                        obj1.NCDCCL813_UpdatedDate = DateTime.Now;
                        obj1.NCDCCL813_CreatedBy = data.UserId;
                        obj1.NCDCCL813_UpdatedBy = data.UserId;

                        _GeneralContext.Add(obj1);

                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCDCCL813_Id > 0)
                {
                    var duplicate = _GeneralContext.DC_813_ClinicalTeachingDMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCDCCL813_Id != data.NCDCCL813_Id && t.NCDCCL813_Year==data.ASMAY_Id &&
                    t.NCDCCL813_CentralSterileSuppliesDepartmentFlag == data.NCDCCL813_CentralSterileSuppliesDepartmentFlag && t.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag == data.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag && t.NCDCCL813_PatientSafetyCurriculumFlag == data.NCDCCL813_PatientSafetyCurriculumFlag && t.NCDCCL813_PeriodicFumigationClinicalAreasFlag == data.NCDCCL813_PeriodicFumigationClinicalAreasFlag && t.NCDCCL813_ImmunizationOfAllTheCaregiversFlag == data.NCDCCL813_ImmunizationOfAllTheCaregiversFlag && t.NCDCCL813_NeedleStickInjuryRegisterFlag == data.NCDCCL813_NeedleStickInjuryRegisterFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.DC_813_ClinicalTeachingDMO.Where(t => t.NCDCCL813_Id == data.NCDCCL813_Id && t.MI_Id == data.MI_Id).Single();


                       update.NCDCCL813_Year = data.ASMAY_Id;
                        update.NCDCCL813_CentralSterileSuppliesDepartmentFlag = data.NCDCCL813_CentralSterileSuppliesDepartmentFlag;
                        update.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag = data.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag;
                        update.NCDCCL813_PatientSafetyCurriculumFlag = data.NCDCCL813_PatientSafetyCurriculumFlag;
                        update.NCDCCL813_PeriodicFumigationClinicalAreasFlag = data.NCDCCL813_PeriodicFumigationClinicalAreasFlag;                                 update.NCDCCL813_ImmunizationOfAllTheCaregiversFlag =
                           data.NCDCCL813_ImmunizationOfAllTheCaregiversFlag;
                        update.NCDCCL813_NeedleStickInjuryRegisterFlag =
                            data.NCDCCL813_NeedleStickInjuryRegisterFlag;                        
                        update.NCDCCL813_UpdatedDate = DateTime.Now;
                        update.NCDCCL813_UpdatedBy = data.UserId;

                        _GeneralContext.Update(update);


                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MC_819_Accredition_ClinicallabDTO savedata2(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {

                if (data.NCDCEQT815_Id == 0)
                {
                    var duplicate = _GeneralContext.DC_815_EquipmentTrainingDMO.Where(t => t.MI_Id == data.MI_Id && t.NCDCEQT815_Year==data.ASMAY_Id && t.NCDCEQT815_ConeBeamComputedTomogramFlag == data.NCDCEQT815_ConeBeamComputedTomogramFlag && t.NCDCEQT815_CAMFacilityFlag == data.NCDCEQT815_CAMFacilityFlag && t.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag == data.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag && t.NCDCEQT815_DentalLASERUnitFlag == data.NCDCEQT815_DentalLASERUnitFlag && t.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag == data.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        DC_815_EquipmentTrainingDMO obj1 = new DC_815_EquipmentTrainingDMO();


                        //obj1.NCMCVAC141_Id = data.NCMCVAC141_Id;
                        obj1.MI_Id = data.MI_Id;                        
                        obj1.NCDCEQT815_Year = data.NCDCEQT815_Year;                        
                        obj1.NCDCEQT815_ConeBeamComputedTomogramFlag = data.NCDCEQT815_ConeBeamComputedTomogramFlag;
                        obj1.NCDCEQT815_CAMFacilityFlag = data.NCDCEQT815_CAMFacilityFlag;
                        obj1.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag = data.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag;
                       obj1.NCDCEQT815_DentalLASERUnitFlag = data.NCDCEQT815_DentalLASERUnitFlag; obj1.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag = data.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag; 
                        obj1.NCDCEQT815_ActiveFlag = true;
                        obj1.NCDCEQT815_CreatedDate = DateTime.Now;
                        obj1.NCDCEQT815_UpdatedDate = DateTime.Now;
                        obj1.NCDCEQT815_CreatedBy = data.UserId;
                        obj1.NCDCEQT815_UpdatedBy = data.UserId;

                        _GeneralContext.Add(obj1);

                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCDCEQT815_Id > 0)
                {
                    var duplicate = _GeneralContext.DC_815_EquipmentTrainingDMO.Where(t => t.MI_Id == data.MI_Id && t.NCDCEQT815_Year==data.ASMAY_Id
                    && t.NCDCEQT815_Id != data.NCDCEQT815_Id &&
                    t.NCDCEQT815_ConeBeamComputedTomogramFlag == data.NCDCEQT815_ConeBeamComputedTomogramFlag && t.NCDCEQT815_CAMFacilityFlag == data.NCDCEQT815_CAMFacilityFlag && t.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag == data.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag && t.NCDCEQT815_DentalLASERUnitFlag == data.NCDCEQT815_DentalLASERUnitFlag && t.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag == data.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.DC_815_EquipmentTrainingDMO.Where(t => t.NCDCEQT815_Id == data.NCDCEQT815_Id && t.MI_Id == data.MI_Id).Single();


                       // update.NCMCVAC141_year = data.ASMAY_Id;
                        update.NCDCEQT815_ConeBeamComputedTomogramFlag = data.NCDCEQT815_ConeBeamComputedTomogramFlag;
                        update.NCDCEQT815_CAMFacilityFlag = data.NCDCEQT815_CAMFacilityFlag;
                        update.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag = data.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag;
                        update.NCDCEQT815_DentalLASERUnitFlag = data.NCDCEQT815_DentalLASERUnitFlag;        update.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag =
                           data.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag;
                        update.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag =
                            data.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag;                        
                        update.NCDCEQT815_UpdatedDate = DateTime.Now;
                        update.NCDCEQT815_UpdatedBy = data.UserId;

                        _GeneralContext.Update(update);


                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MC_819_Accredition_ClinicallabDTO savedata3(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {

                if (data.NCDCSC816_Id == 0)
                {
                    var duplicate = _GeneralContext.DC_816_SpecializedClinicsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCDCSC816_Year==data.ASMAY_Id && t.NCDCSC816_ComprehensiveclinicFlag == data.NCDCSC816_ComprehensiveclinicFlag && t.NCDCSC816_ImplantClinicFlag == data.NCDCSC816_ImplantClinicFlag && t.NCDCSC816_GeriatricClinicFlag == data.NCDCSC816_GeriatricClinicFlag && t.NCDCSC816_SpecialHealthCareNeedsClinicFlag == data.NCDCSC816_SpecialHealthCareNeedsClinicFlag && t.NCDCSC816_TobaccoCessationClinicFlag == data.NCDCSC816_TobaccoCessationClinicFlag && t.NCDCSC816_EstheticClinicFlag == data.NCDCSC816_EstheticClinicFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        DC_816_SpecializedClinicsDMO obj1 = new DC_816_SpecializedClinicsDMO();


                        //obj1.NCMCVAC141_Id = data.NCMCVAC141_Id;
                        obj1.MI_Id = data.MI_Id;                        
                        obj1.NCDCSC816_Year = data.ASMAY_Id;                        
                        obj1.NCDCSC816_ComprehensiveclinicFlag = data.NCDCSC816_ComprehensiveclinicFlag;
                        obj1.NCDCSC816_ImplantClinicFlag = data.NCDCSC816_ImplantClinicFlag;
                        obj1.NCDCSC816_GeriatricClinicFlag = data.NCDCSC816_GeriatricClinicFlag;
                       obj1.NCDCSC816_SpecialHealthCareNeedsClinicFlag = data.NCDCSC816_SpecialHealthCareNeedsClinicFlag; obj1.NCDCSC816_TobaccoCessationClinicFlag = data.NCDCSC816_TobaccoCessationClinicFlag;
                        obj1.NCDCSC816_EstheticClinicFlag = data.NCDCSC816_EstheticClinicFlag; 
                        obj1.NCDCSC816_ActiveFlag = true;
                        obj1.NCDCSC816_CreatedDate = DateTime.Now;
                        obj1.NCDCSC816_UpdatedDate = DateTime.Now;
                        obj1.NCDCSC816_CreatedBy = data.UserId;
                        obj1.NCDCSC816_UpdatedBy = data.UserId;

                        _GeneralContext.Add(obj1);

                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCDCSC816_Id > 0)
                {
                    var duplicate = _GeneralContext.DC_816_SpecializedClinicsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCDCSC816_Year==data.ASMAY_Id 
                    && t.NCDCSC816_Id != data.NCDCSC816_Id &&
                    t.NCDCSC816_ComprehensiveclinicFlag == data.NCDCSC816_ComprehensiveclinicFlag && t.NCDCSC816_ImplantClinicFlag == data.NCDCSC816_ImplantClinicFlag && t.NCDCSC816_GeriatricClinicFlag == data.NCDCSC816_GeriatricClinicFlag && t.NCDCSC816_SpecialHealthCareNeedsClinicFlag == data.NCDCSC816_SpecialHealthCareNeedsClinicFlag && t.NCDCSC816_TobaccoCessationClinicFlag == data.NCDCSC816_TobaccoCessationClinicFlag && t.NCDCSC816_EstheticClinicFlag==data.NCDCSC816_EstheticClinicFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.DC_816_SpecializedClinicsDMO.Where(t => t.NCDCSC816_Id == data.NCDCSC816_Id && t.MI_Id == data.MI_Id).Single();


                        update.NCDCSC816_Year = data.ASMAY_Id;
                        update.NCDCSC816_ComprehensiveclinicFlag = data.NCDCSC816_ComprehensiveclinicFlag;
                        update.NCDCSC816_ImplantClinicFlag = data.NCDCSC816_ImplantClinicFlag;
                        update.NCDCSC816_GeriatricClinicFlag = data.NCDCSC816_GeriatricClinicFlag;
                        update.NCDCSC816_SpecialHealthCareNeedsClinicFlag = data.NCDCSC816_SpecialHealthCareNeedsClinicFlag;        update.NCDCSC816_TobaccoCessationClinicFlag =
                           data.NCDCSC816_TobaccoCessationClinicFlag;
                        update.NCDCSC816_EstheticClinicFlag =
                            data.NCDCSC816_EstheticClinicFlag;                        
                        update.NCDCSC816_UpdatedDate = DateTime.Now;
                        update.NCDCSC816_UpdatedBy = data.UserId;

                        _GeneralContext.Update(update);


                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MC_819_Accredition_ClinicallabDTO editdata(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {
                var test = _GeneralContext.MC_819_Accredition_ClinicallabDMO.Where(t => t.NCMCCL819_Id == data.NCMCCL819_Id).ToList();

                data.editdata = (from a in _GeneralContext.MC_819_Accredition_ClinicallabDMO
                                 from b in _GeneralContext.Academic
                                 where (a.NCMCCL819_Id == data.NCMCCL819_Id && a.MI_Id == data.MI_Id && a.NCMCCL819_Year==b.ASMAY_Id && a.MI_Id==b.MI_Id)
                                 select new MC_819_Accredition_ClinicallabDTO
                                 {
                                     NCMCCL819_Id = a.NCMCCL819_Id,
                                     NCMCCL819_Year = a.NCMCCL819_Year,
                                     ASMAY_Year = b.ASMAY_Year,
                                     NCMCCL819_NABHAccnTechHoslFlg = a.NCMCCL819_NABHAccnTechHoslFlg,
                                     NCMCCL819_NABHAccnTechlabslFlg = a.NCMCCL819_NABHAccnTechlabslFlg,
                                     NCMCCL819_CertificationDeptlFlg = a.NCMCCL819_CertificationDeptlFlg,
                                     NCMCCL819_OtherRecAccCertificationFlg = a.NCMCCL819_OtherRecAccCertificationFlg,
                                     NCMCCL819_StatusFlg = a.NCMCCL819_StatusFlg,
                                 }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MC_819_Accredition_ClinicallabDTO getcomment(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.MC_819_ClinicalLaboratory_CommentsDMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCMCCL819C_RemarksBy == b.Id && a.NCMCCL819_Id == data.NCMCCL819_Id)
                                    select new MC_819_Accredition_ClinicallabDTO
                                    {

                                        NCMCCL819C_Remarks = a.NCMCCL819C_Remarks,
                                        NCMCCL819C_Id = a.NCMCCL819C_Id,
                                        NCMCCL819C_RemarksBy = a.NCMCCL819C_RemarksBy,
                                        NCMCCL819C_StatusFlg = a.NCMCCL819C_StatusFlg,
                                        NCMCCL819C_ActiveFlag = a.NCMCCL819C_ActiveFlag,
                                        NCMCCL819C_CreatedBy = a.NCMCCL819C_CreatedBy,
                                        NCMCCL819C_CreatedDate = a.NCMCCL819C_CreatedDate,
                                        NCMCCL819C_UpdatedBy = a.NCMCCL819C_UpdatedBy,
                                        NCMCCL819C_UpdatedDate = a.NCMCCL819C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public MC_819_Accredition_ClinicallabDTO deactivate(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {
                var result = _GeneralContext.MC_819_Accredition_ClinicallabDMO.Where(t => t.NCMCCL819_Id == data.NCMCCL819_Id).SingleOrDefault();

                if (result.NCMCCL819_ActiveFlag == true)
                {
                    result.NCMCCL819_ActiveFlag = false;
                }
                else if (result.NCMCCL819_ActiveFlag == false)
                {
                    result.NCMCCL819_ActiveFlag = true;
                }

                result.NCMCCL819_UpdatedDate = DateTime.Now;
                result.NCMCCL819_UpdatedBy = data.UserId;

                _GeneralContext.Update(result);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MC_819_Accredition_ClinicallabDTO savecomments(MC_819_Accredition_ClinicallabDTO data)
        {
            try
            {
                MC_819_ClinicalLaboratory_CommentsDMO obj1 = new MC_819_ClinicalLaboratory_CommentsDMO();
                obj1.NCMCCL819C_Remarks = data.Remarks;
                obj1.NCMCCL819C_RemarksBy = data.UserId;
                obj1.NCMCCL819C_StatusFlg = "";
                obj1.NCMCCL819_Id = data.filefkid;
                obj1.NCMCCL819C_ActiveFlag = true;
                obj1.NCMCCL819C_CreatedBy = data.UserId;
                obj1.NCMCCL819C_UpdatedBy = data.UserId;
                obj1.NCMCCL819C_CreatedDate = DateTime.Now;
                obj1.NCMCCL819C_UpdatedDate = DateTime.Now;
                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
