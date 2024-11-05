using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class NAAC_MC_VACcommonImpl : Interface.NAAC_MC_VACcommonInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_MC_VACcommonImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }


        public NAAC_MC_VACcommon_DTO loaddata(NAAC_MC_VACcommon_DTO data)
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
                                 select new NAAC_MC_VACcommon_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();



                data.alldata141 = (from a in _GeneralContext.NAAC_MC_VAC_141_DMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id
                                   && y.ASMAY_Id == a.NCMCVAC141_year)
                                   select new NAAC_MC_VACcommon_DTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       NCMCVAC141_Id = a.NCMCVAC141_Id,
                                       NCMCVAC141_FKFromStudents = a.NCMCVAC141_FKFromStudents,
                                       NCMCVAC141_FKFromteachers = a.NCMCVAC141_FKFromteachers,
                                       NCMCVAC141_FKFromemployers = a.NCMCVAC141_FKFromemployers,
                                       NCMCVAC141_FKFromalumni = a.NCMCVAC141_FKFromalumni,
                                       FkCollFromOtherProfs = a.FkCollFromOtherProfs,
                                       MI_Id = a.MI_Id,
                                   }).Distinct().OrderByDescending(t => t.NCMCVAC141_Id).ToArray();

                data.alldata142 = (from a in _GeneralContext.NAAC_MC_VAC_142_DMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id
                                   && y.ASMAY_Id == a.NCMCVAC142_year)
                                   select new NAAC_MC_VACcommon_DTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       NCMCVAC142_Id = a.NCMCVAC142_Id,
                                       NCMCVAC142_FKCollAnlInstWebsite = a.NCMCVAC142_FKCollAnlInstWebsite,
                                       NCMCVAC142_FKCollAnlFk = a.NCMCVAC142_FKCollAnlFk,
                                       NCMCVAC142_FKCollanalysed = a.NCMCVAC142_FKCollanalysed,
                                       NCMCVAC142_FKcollected = a.NCMCVAC142_FKcollected,
                                       NCMCVAC142_FKNotcollected = a.NCMCVAC142_FKNotcollected,
                                       MI_Id = a.MI_Id,

                                   }).Distinct().OrderByDescending(t => t.NCMCVAC141_Id).ToArray();

                data.alldata221 = (from a in _GeneralContext.NAAC_MC_Measures_221_DMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id
                                   && y.ASMAY_Id == a.NCMCM221_Year)
                                   select new NAAC_MC_VACcommon_DTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       MI_Id = a.MI_Id,
                                       NCMCM221_Id = a.NCMCM221_Id,
                                       NCMCM221_Year = a.NCMCM221_Year,
                                       NCMCM221_MesCrFolldRegSlowPerFlag = a.NCMCM221_MesCrFolldRegSlowPerFlag,
                                       NCMCM221_MesCrFolldAdLersFlag = a.NCMCM221_MesCrFolldAdLersFlag,
                                       NCMCM221_SpecialprogCrLowORAdlersFlag = a.NCMCM221_SpecialprogCrLowORAdlersFlag,
                                       NCMCM221_ProclsMeasureAchsFlag = a.NCMCM221_ProclsMeasureAchsFlag
                                   }).Distinct().ToArray();

                data.alldata232 = (from a in _GeneralContext.NAAC_MC_232_SKills_DMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id
                                   && y.ASMAY_Id == a.NCMCS232_Year)
                                   select new NAAC_MC_VACcommon_DTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       NCMCS232_Id = a.NCMCS232_Id,
                                       MI_Id = a.MI_Id,
                                       NCMCS232_Year = a.NCMCS232_Year,
                                       NCMCS232_InstClinicalSkillsFlag = a.NCMCS232_InstClinicalSkillsFlag,
                                       NCMCS232_InstAdvsimulationBasedTrainingFlag = a.NCMCS232_InstAdvsimulationBasedTrainingFlag,
                                       NCMCS232_StuProgTrAsstofStudentsFlag = a.NCMCS232_StuProgTrAsstofStudentsFlag,
                                       NCMCS232_StuProgTrAsstClORSimulationLrnFlag = a.NCMCS232_StuProgTrAsstClORSimulationLrnFlag,
                                   }).Distinct().ToArray();

                data.alldata254 = (from a in _GeneralContext.NAAC_MC_254_CourseImprovement_DMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id
                                   && y.ASMAY_Id == a.NCMCCI254_Year)
                                   select new NAAC_MC_VACcommon_DTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       NCMCCI254_Id = a.NCMCCI254_Id,
                                       MI_Id = a.MI_Id,
                                       NCMCCI254_Year = a.NCMCCI254_Year,
                                       NCMCCI254_TimelyAdministrationCIEFlag = a.NCMCCI254_TimelyAdministrationCIEFlag,
                                       NCMCCI254_OnTimeAssessmentFeedbackFlag = a.NCMCCI254_OnTimeAssessmentFeedbackFlag,
                                       NCMCCI254_MakeupAssignmentsFlag = a.NCMCCI254_MakeupAssignmentsFlag,
                                       NCMCCI254_RemedialTeachingFlag = a.NCMCCI254_RemedialTeachingFlag,
                                   }).Distinct().ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_MC_VACcommon_DTO savedata141(NAAC_MC_VACcommon_DTO data)
        {
            try
            {

                if (data.NCMCVAC141_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_VAC_141_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMCVAC141_year == data.ASMAY_Id && t.NCMCVAC141_FKFromStudents == data.NCMCVAC141_FKFromStudents && t.NCMCVAC141_FKFromteachers == data.NCMCVAC141_FKFromteachers && t.NCMCVAC141_FKFromemployers == data.NCMCVAC141_FKFromemployers && t.NCMCVAC141_FKFromalumni == data.NCMCVAC141_FKFromalumni && t.FkCollFromOtherProfs == data.FkCollFromOtherProfs).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                   
                    else
                    {
                       
                        NAAC_MC_VAC_141_DMO obj1 = new NAAC_MC_VAC_141_DMO();


                        //obj1.NCMCVAC141_Id = data.NCMCVAC141_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMCVAC141_year = data.ASMAY_Id;
                        obj1.NCMCVAC141_FKFromStudents = data.NCMCVAC141_FKFromStudents;
                        obj1.NCMCVAC141_FKFromteachers = data.NCMCVAC141_FKFromteachers;
                        obj1.NCMCVAC141_FKFromemployers = data.NCMCVAC141_FKFromemployers;
                        obj1.NCMCVAC141_FKFromalumni = data.NCMCVAC141_FKFromalumni;
                        obj1.FkCollFromOtherProfs = data.FkCollFromOtherProfs;
                        obj1.NCMCVAC141_CreatedDate = DateTime.Now;
                        obj1.NCMCVAC141_UpdatedDate = DateTime.Now;
                        obj1.NCMCVAC141_CreatedBy = data.UserId;
                        obj1.NCMCVAC141_UpdatedBy = data.UserId;

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
                else if (data.NCMCVAC141_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_VAC_141_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMCVAC141_Id != data.NCMCVAC141_Id && t.NCMCVAC141_year == data.ASMAY_Id
                    && t.NCMCVAC141_FKFromStudents == data.NCMCVAC141_FKFromStudents
                    && t.NCMCVAC141_FKFromteachers == data.NCMCVAC141_FKFromteachers
                    && t.NCMCVAC141_FKFromStudents == data.NCMCVAC141_FKFromStudents
                    && t.NCMCVAC141_FKFromalumni == data.NCMCVAC141_FKFromalumni
                    && t.FkCollFromOtherProfs == data.FkCollFromOtherProfs).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_MC_VAC_141_DMO.Where(t => t.NCMCVAC141_Id == data.NCMCVAC141_Id && t.MI_Id == data.MI_Id).Single();


                        update.NCMCVAC141_year = data.ASMAY_Id;
                        update.NCMCVAC141_FKFromStudents = data.NCMCVAC141_FKFromStudents;
                        update.NCMCVAC141_FKFromteachers = data.NCMCVAC141_FKFromteachers;
                        update.NCMCVAC141_FKFromemployers = data.NCMCVAC141_FKFromemployers;
                        update.NCMCVAC141_FKFromalumni = data.NCMCVAC141_FKFromalumni;
                        update.FkCollFromOtherProfs = data.FkCollFromOtherProfs;
                        update.NCMCVAC141_UpdatedDate = DateTime.Now;
                        update.NCMCVAC141_UpdatedBy = data.UserId;

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
        public NAAC_MC_VACcommon_DTO editdata141(NAAC_MC_VACcommon_DTO data)
        {
            try
            {
                data.editdata = (from a in _GeneralContext.NAAC_MC_VAC_141_DMO
                                 where (a.NCMCVAC141_Id == data.NCMCVAC141_Id && a.MI_Id == data.MI_Id)
                                 select new NAAC_MC_VACcommon_DTO
                                 {
                                     NCMCVAC141_Id = a.NCMCVAC141_Id,
                                     MI_Id = a.MI_Id,
                                     NCMCVAC141_year = a.NCMCVAC141_year,
                                     NCMCVAC141_FKFromStudents = a.NCMCVAC141_FKFromStudents,
                                     NCMCVAC141_FKFromteachers = a.NCMCVAC141_FKFromteachers,
                                     NCMCVAC141_FKFromemployers = a.NCMCVAC141_FKFromemployers,
                                     NCMCVAC141_FKFromalumni = a.NCMCVAC141_FKFromalumni,
                                     FkCollFromOtherProfs = a.FkCollFromOtherProfs,
                                 }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_MC_VACcommon_DTO savedata142(NAAC_MC_VACcommon_DTO data)
        {
            try
            {

                if (data.NCMCVAC142_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_VAC_142_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMCVAC142_year == data.ASMAY_Id && t.NCMCVAC142_FKCollAnlInstWebsite == data.NCMCVAC142_FKCollAnlInstWebsite && t.NCMCVAC142_FKCollanalysed == data.NCMCVAC142_FKCollanalysed && t.NCMCVAC142_FKcollected == data.NCMCVAC142_FKcollected
                    && t.NCMCVAC142_FKCollAnlFk == data.NCMCVAC142_FKCollAnlFk).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_VAC_142_DMO obj1 = new NAAC_MC_VAC_142_DMO();


                        //obj1.NCMCVAC142_Id = data.NCMCVAC142_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMCVAC142_year = data.ASMAY_Id;
                        obj1.NCMCVAC142_FKCollAnlInstWebsite = data.NCMCVAC142_FKCollAnlInstWebsite;
                        obj1.NCMCVAC142_FKCollAnlFk = data.NCMCVAC142_FKCollAnlFk;
                        obj1.NCMCVAC142_FKCollanalysed = data.NCMCVAC142_FKCollanalysed;
                        obj1.NCMCVAC142_FKcollected = data.NCMCVAC142_FKcollected;
                        obj1.NCMCVAC142_FKNotcollected = data.NCMCVAC142_FKNotcollected;
                        obj1.NCMCVAC142_CreatedDate = DateTime.Now;
                        obj1.NCMCVAC142_UpdatedDate = DateTime.Now;
                        obj1.NCMCVAC142_CreatedBy = data.UserId;
                        obj1.NCMCVAC142_UpdatedBy = data.UserId;

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
                else if (data.NCMCVAC142_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_VAC_142_DMO.Where(t => t.NCMCVAC142_Id != data.NCMCVAC142_Id
                    && t.MI_Id == data.MI_Id
                    && t.NCMCVAC142_year == data.ASMAY_Id && t.NCMCVAC142_FKCollAnlInstWebsite == data.NCMCVAC142_FKCollAnlInstWebsite && t.NCMCVAC142_FKCollanalysed == data.NCMCVAC142_FKCollanalysed && t.NCMCVAC142_FKcollected == data.NCMCVAC142_FKcollected
                    && t.NCMCVAC142_FKCollAnlFk == data.NCMCVAC142_FKCollAnlFk).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_MC_VAC_142_DMO.Where(t => t.NCMCVAC142_Id == data.NCMCVAC142_Id && t.MI_Id == data.MI_Id).Single();

                        update.NCMCVAC142_year = data.ASMAY_Id;
                        update.NCMCVAC142_FKCollAnlInstWebsite = data.NCMCVAC142_FKCollAnlInstWebsite;
                        update.NCMCVAC142_FKCollAnlFk = data.NCMCVAC142_FKCollAnlFk;
                        update.NCMCVAC142_FKCollanalysed = data.NCMCVAC142_FKCollanalysed;
                        update.NCMCVAC142_FKcollected = data.NCMCVAC142_FKcollected;
                        update.NCMCVAC142_FKNotcollected = data.NCMCVAC142_FKNotcollected;
                        update.NCMCVAC142_UpdatedDate = DateTime.Now;
                        update.NCMCVAC142_UpdatedBy = data.UserId;

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
        public NAAC_MC_VACcommon_DTO M_savedata221(NAAC_MC_VACcommon_DTO data)
        {
            try
            {

                if (data.NCMCM221_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_Measures_221_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMCM221_Year == data.ASMAY_Id && t.NCMCM221_MesCrFolldRegSlowPerFlag == data.NCMCM221_MesCrFolldRegSlowPerFlag && t.NCMCM221_MesCrFolldAdLersFlag == data.NCMCM221_MesCrFolldAdLersFlag && t.NCMCM221_SpecialprogCrLowORAdlersFlag == data.NCMCM221_SpecialprogCrLowORAdlersFlag
                    && t.NCMCM221_ProclsMeasureAchsFlag == data.NCMCM221_ProclsMeasureAchsFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_Measures_221_DMO obj1 = new NAAC_MC_Measures_221_DMO();


                        //obj1.NCMCVAC142_Id = data.NCMCVAC142_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMCM221_Year = data.ASMAY_Id;
                        obj1.NCMCM221_MesCrFolldRegSlowPerFlag = data.NCMCM221_MesCrFolldRegSlowPerFlag;
                        obj1.NCMCM221_MesCrFolldAdLersFlag = data.NCMCM221_MesCrFolldAdLersFlag;
                        obj1.NCMCM221_SpecialprogCrLowORAdlersFlag = data.NCMCM221_SpecialprogCrLowORAdlersFlag;
                        obj1.NCMCM221_ProclsMeasureAchsFlag = data.NCMCM221_ProclsMeasureAchsFlag;
                        obj1.NCMCM221_CreatedDate = DateTime.Now;
                        obj1.NCMCM221_UpdatedDate = DateTime.Now;
                        obj1.NCMCM221_CreatedBy = data.UserId;
                        obj1.NCMCM221_UpdatedBy = data.UserId;

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
                else if (data.NCMCM221_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_Measures_221_DMO.Where(t => t.NCMCM221_Id != data.NCMCM221_Id && t.MI_Id == data.MI_Id
                    && t.NCMCM221_Year == data.ASMAY_Id && t.NCMCM221_MesCrFolldRegSlowPerFlag == data.NCMCM221_MesCrFolldRegSlowPerFlag && t.NCMCM221_MesCrFolldAdLersFlag == data.NCMCM221_MesCrFolldAdLersFlag && t.NCMCM221_SpecialprogCrLowORAdlersFlag == data.NCMCM221_SpecialprogCrLowORAdlersFlag
                    && t.NCMCM221_ProclsMeasureAchsFlag == data.NCMCM221_ProclsMeasureAchsFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_MC_Measures_221_DMO.Where(t => t.NCMCM221_Id == data.NCMCM221_Id && t.MI_Id == data.MI_Id).Single();

                        update.NCMCM221_Year = data.ASMAY_Id;
                        update.NCMCM221_MesCrFolldRegSlowPerFlag = data.NCMCM221_MesCrFolldRegSlowPerFlag;
                        update.NCMCM221_MesCrFolldAdLersFlag = data.NCMCM221_MesCrFolldAdLersFlag;
                        update.NCMCM221_SpecialprogCrLowORAdlersFlag = data.NCMCM221_SpecialprogCrLowORAdlersFlag;
                        update.NCMCM221_ProclsMeasureAchsFlag = data.NCMCM221_ProclsMeasureAchsFlag;
                        update.NCMCM221_UpdatedDate = DateTime.Now;
                        update.NCMCM221_UpdatedBy = data.UserId;

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
        public NAAC_MC_VACcommon_DTO M_savedata232(NAAC_MC_VACcommon_DTO data)
        {
            try
            {

                if (data.NCMCS232_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_232_SKills_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMCS232_Year == data.ASMAY_Id && t.NCMCS232_InstClinicalSkillsFlag == data.NCMCS232_InstClinicalSkillsFlag && t.NCMCS232_InstAdvsimulationBasedTrainingFlag == data.NCMCS232_InstAdvsimulationBasedTrainingFlag && t.NCMCS232_StuProgTrAsstofStudentsFlag == data.NCMCS232_StuProgTrAsstofStudentsFlag
                    && t.NCMCS232_StuProgTrAsstClORSimulationLrnFlag == data.NCMCS232_StuProgTrAsstClORSimulationLrnFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_232_SKills_DMO obj1 = new NAAC_MC_232_SKills_DMO();


                        //obj1.NCMCVAC142_Id = data.NCMCVAC142_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMCS232_Year = data.ASMAY_Id;
                        obj1.NCMCS232_InstClinicalSkillsFlag = data.NCMCS232_InstClinicalSkillsFlag;
                        obj1.NCMCS232_InstAdvsimulationBasedTrainingFlag = data.NCMCS232_InstAdvsimulationBasedTrainingFlag;
                        obj1.NCMCS232_StuProgTrAsstofStudentsFlag = data.NCMCS232_StuProgTrAsstofStudentsFlag;
                        obj1.NCMCS232_StuProgTrAsstClORSimulationLrnFlag = data.NCMCS232_StuProgTrAsstClORSimulationLrnFlag;
                        obj1.NCMCS232_CreateDate = DateTime.Now;
                        obj1.NCMCS232_UpdatedDate = DateTime.Now;
                        obj1.NCMCS232_CreatedBy = data.UserId;
                        obj1.NCMCS232_UpdatedBy = data.UserId;

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
                else if (data.NCMCS232_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_232_SKills_DMO.Where(t => t.NCMCS232_Id != data.NCMCS232_Id && t.MI_Id == data.MI_Id
                   && t.NCMCS232_Year == data.ASMAY_Id && t.NCMCS232_InstClinicalSkillsFlag == data.NCMCS232_InstClinicalSkillsFlag && t.NCMCS232_InstAdvsimulationBasedTrainingFlag == data.NCMCS232_InstAdvsimulationBasedTrainingFlag && t.NCMCS232_StuProgTrAsstofStudentsFlag == data.NCMCS232_StuProgTrAsstofStudentsFlag
                   && t.NCMCS232_StuProgTrAsstClORSimulationLrnFlag == data.NCMCS232_StuProgTrAsstClORSimulationLrnFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_MC_232_SKills_DMO.Where(t => t.NCMCS232_Id == data.NCMCS232_Id && t.MI_Id == data.MI_Id).Single();

                        update.NCMCS232_Year = data.ASMAY_Id;
                        update.NCMCS232_InstClinicalSkillsFlag = data.NCMCS232_InstClinicalSkillsFlag;
                        update.NCMCS232_InstAdvsimulationBasedTrainingFlag = data.NCMCS232_InstAdvsimulationBasedTrainingFlag;
                        update.NCMCS232_StuProgTrAsstofStudentsFlag = data.NCMCS232_StuProgTrAsstofStudentsFlag;
                        update.NCMCS232_StuProgTrAsstClORSimulationLrnFlag = data.NCMCS232_StuProgTrAsstClORSimulationLrnFlag;
                        update.NCMCS232_UpdatedDate = DateTime.Now;
                        update.NCMCS232_UpdatedBy = data.UserId;

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
        public NAAC_MC_VACcommon_DTO M_savedata254(NAAC_MC_VACcommon_DTO data)
        {
            try
            {

                if (data.NCMCCI254_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_254_CourseImprovement_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMCCI254_Year == data.ASMAY_Id && t.NCMCCI254_TimelyAdministrationCIEFlag == data.NCMCCI254_TimelyAdministrationCIEFlag && t.NCMCCI254_OnTimeAssessmentFeedbackFlag == data.NCMCCI254_OnTimeAssessmentFeedbackFlag && t.NCMCCI254_MakeupAssignmentsFlag == data.NCMCCI254_MakeupAssignmentsFlag
                    && t.NCMCCI254_RemedialTeachingFlag == data.NCMCCI254_RemedialTeachingFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_254_CourseImprovement_DMO obj1 = new NAAC_MC_254_CourseImprovement_DMO();


                        //obj1.NCMCVAC142_Id = data.NCMCVAC142_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMCCI254_Year = data.ASMAY_Id;
                        obj1.NCMCCI254_TimelyAdministrationCIEFlag = data.NCMCCI254_TimelyAdministrationCIEFlag;
                        obj1.NCMCCI254_OnTimeAssessmentFeedbackFlag = data.NCMCCI254_OnTimeAssessmentFeedbackFlag;
                        obj1.NCMCCI254_MakeupAssignmentsFlag = data.NCMCCI254_MakeupAssignmentsFlag;
                        obj1.NCMCCI254_RemedialTeachingFlag = data.NCMCCI254_RemedialTeachingFlag;
                        obj1.NCMCCI254_CreateDate = DateTime.Now;
                        obj1.NCMCCI254_UpdatedDate = DateTime.Now;
                        obj1.NCMCCI254_CreatedBy = data.UserId;
                        obj1.NCMCCI254_UpdatedBy = data.UserId;
                       
                        _GeneralContext.Add(obj1);
                        data.duplicate = false;
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
                else if (data.NCMCCI254_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_254_CourseImprovement_DMO.Where(t => t.NCMCCI254_Id != data.NCMCCI254_Id && t.MI_Id == data.MI_Id
                     && t.NCMCCI254_Year == data.ASMAY_Id && t.NCMCCI254_TimelyAdministrationCIEFlag == data.NCMCCI254_TimelyAdministrationCIEFlag && t.NCMCCI254_OnTimeAssessmentFeedbackFlag == data.NCMCCI254_OnTimeAssessmentFeedbackFlag && t.NCMCCI254_MakeupAssignmentsFlag == data.NCMCCI254_MakeupAssignmentsFlag
                     && t.NCMCCI254_RemedialTeachingFlag == data.NCMCCI254_RemedialTeachingFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        data.duplicate = false;
                        var update = _GeneralContext.NAAC_MC_254_CourseImprovement_DMO.Where(t => t.NCMCCI254_Id == data.NCMCCI254_Id && t.MI_Id == data.MI_Id).Single();


                        update.NCMCCI254_Year = data.ASMAY_Id;
                        update.NCMCCI254_TimelyAdministrationCIEFlag = data.NCMCCI254_TimelyAdministrationCIEFlag;
                        update.NCMCCI254_OnTimeAssessmentFeedbackFlag = data.NCMCCI254_OnTimeAssessmentFeedbackFlag;
                        update.NCMCCI254_MakeupAssignmentsFlag = data.NCMCCI254_MakeupAssignmentsFlag;
                        update.NCMCCI254_RemedialTeachingFlag = data.NCMCCI254_RemedialTeachingFlag;
                        update.NCMCCI254_UpdatedDate = DateTime.Now;
                        update.NCMCCI254_UpdatedBy = data.UserId;

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


    }
}
