using DataAccessMsSqlServerProvider.NAAC;
using DataAccessMsSqlServerProvider.NAAC.Documents;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class HSU_MasterCR2Impl : Interface.HSU_MasterCR2Interface
    {
        public DocumentsContext _DocumentsContext;
        public GeneralContext _GeneralContext;
        public HSU_MasterCR2Impl(DocumentsContext para1, GeneralContext para2)
        {
            _DocumentsContext = para1;
            _GeneralContext = para2;
        }

        public HSU_MasterCR2_DTO loaddata(HSU_MasterCR2_DTO data)
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
                                 select new HSU_MasterCR2_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();



                data.alldata221 = (from a in _GeneralContext.NAAC_HSU_StudentLearningLevels_221_DMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id
                                   && y.ASMAY_Id == a.NCHSUSLL221_Year)
                                   select new HSU_MasterCR2_DTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       NCHSUSLL221_Year = a.NCHSUSLL221_Year,
                                       NCHSUSLL221_MsCrRegSlowPerformersFlag = a.NCHSUSLL221_MsCrRegSlowPerformersFlag,
                                       NCHSUSLL221_MsCrRegAdLearnersFlag = a.NCHSUSLL221_MsCrRegAdLearnersFlag,
                                       NCHSUSLL221_SplProgSlowAdLearnersFlag = a.NCHSUSLL221_SplProgSlowAdLearnersFlag,
                                       NCHSUSLL221_ProtocolsmesAchievementsFlag = a.NCHSUSLL221_ProtocolsmesAchievementsFlag,                                       
                                       MI_Id = a.MI_Id,
                                   }).Distinct().OrderByDescending(t => t.NCHSUSLL221_Id).ToArray();

                data.alldata232 = (from a in _GeneralContext.NAAC_HSU_ClinicalSkills_232_DMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id
                                   && y.ASMAY_Id == a.NCHSUCS232_Year)
                                   select new HSU_MasterCR2_DTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       NCHSUCS232_Id = a.NCHSUCS232_Id,
                                       NCHSUCS232_Year = a.NCHSUCS232_Year,
                                       NCHSUCS232_CsTrclinicalskillsRelevantFlag = a.NCHSUCS232_CsTrclinicalskillsRelevantFlag,
                                       NCHSUCS232_PatientSimulatorsSimulationbasedFlag = a.NCHSUCS232_PatientSimulatorsSimulationbasedFlag,
                                       NCHSUCS232_StProgConductedSssessmentStudentsFlag = a.NCHSUCS232_StProgConductedSssessmentStudentsFlag,
                                       NCHSUCS232_TrProgConForCsSblearningFlag = a.NCHSUCS232_TrProgConForCsSblearningFlag,                                      
                                       MI_Id = a.MI_Id,

                                   }).Distinct().OrderByDescending(t => t.NCHSUCS232_Id).ToArray();

                data.alldata255 = (from a in _GeneralContext.NAAC_HSU_ExaminationManagement_255_DMO
                                   from y in _GeneralContext.Academic
                                   where (a.MI_Id == data.MI_Id
                                   && y.ASMAY_Id == a.NCHSUEM255_Year)
                                   select new HSU_MasterCR2_DTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       MI_Id = a.MI_Id,
                                       NCHSUEM255_Id = a.NCHSUEM255_Id,
                                       NCHSUEM255_Year = a.NCHSUEM255_Year,
                                       NCHSUEM255_AnDivImpEMFlag = a.NCHSUEM255_AnDivImpEMFlag,
                                       NCHSUEM255_StuRegHtIssueProcessingFlag = a.NCHSUEM255_StuRegHtIssueProcessingFlag,
                                       NCHSUEM255_StuRegResultProcFlag = a.NCHSUEM255_StuRegResultProcFlag,
                                       NCHSUEM255_ResultProcAtdFlag = a.NCHSUEM255_ResultProcAtdFlag
                                   }).Distinct().ToArray();

              

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public HSU_MasterCR2_DTO save_HSU_221(HSU_MasterCR2_DTO data)
        {
            try
            {

                if (data.NCHSUSLL221_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_HSU_StudentLearningLevels_221_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCHSUSLL221_Year == data.ASMAY_Id && t.NCHSUSLL221_MsCrRegSlowPerformersFlag == data.NCHSUSLL221_MsCrRegSlowPerformersFlag && t.NCHSUSLL221_MsCrRegAdLearnersFlag == data.NCHSUSLL221_MsCrRegAdLearnersFlag && t.NCHSUSLL221_SplProgSlowAdLearnersFlag == data.NCHSUSLL221_SplProgSlowAdLearnersFlag && t.NCHSUSLL221_ProtocolsmesAchievementsFlag == data.NCHSUSLL221_ProtocolsmesAchievementsFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_HSU_StudentLearningLevels_221_DMO obj1 = new NAAC_HSU_StudentLearningLevels_221_DMO();

                       
                        obj1.NCHSUSLL221_Id = data.NCHSUSLL221_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCHSUSLL221_Year = data.ASMAY_Id;
                        obj1.NCHSUSLL221_MsCrRegSlowPerformersFlag = data.NCHSUSLL221_MsCrRegSlowPerformersFlag;
                        obj1.NCHSUSLL221_MsCrRegAdLearnersFlag = data.NCHSUSLL221_MsCrRegAdLearnersFlag;
                        obj1.NCHSUSLL221_SplProgSlowAdLearnersFlag = data.NCHSUSLL221_SplProgSlowAdLearnersFlag;
                        obj1.NCHSUSLL221_ProtocolsmesAchievementsFlag = data.NCHSUSLL221_ProtocolsmesAchievementsFlag;
                        obj1.NCHSUSLL221_CreatedDate = DateTime.Now;
                        obj1.NCHSUSLL221_UpdatedDate = DateTime.Now;
                        obj1.NCHSUSLL221_CreatedBy = data.UserId;
                        obj1.NCHSUSLL221_UpdatedBy = data.UserId;

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
                else if (data.NCHSUSLL221_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_HSU_StudentLearningLevels_221_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCHSUSLL221_Id != data.NCHSUSLL221_Id && t.NCHSUSLL221_Year == data.ASMAY_Id
                    && t.NCHSUSLL221_MsCrRegSlowPerformersFlag == data.NCHSUSLL221_MsCrRegSlowPerformersFlag
                    && t.NCHSUSLL221_MsCrRegAdLearnersFlag == data.NCHSUSLL221_MsCrRegAdLearnersFlag
                    && t.NCHSUSLL221_MsCrRegSlowPerformersFlag == data.NCHSUSLL221_MsCrRegSlowPerformersFlag
                    && t.NCHSUSLL221_ProtocolsmesAchievementsFlag == data.NCHSUSLL221_ProtocolsmesAchievementsFlag
                  ).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_HSU_StudentLearningLevels_221_DMO.Where(t => t.NCHSUSLL221_Id == data.NCHSUSLL221_Id && t.MI_Id == data.MI_Id).Single();


                        update.NCHSUSLL221_Year = data.ASMAY_Id;
                        update.NCHSUSLL221_MsCrRegSlowPerformersFlag = data.NCHSUSLL221_MsCrRegSlowPerformersFlag;
                        update.NCHSUSLL221_MsCrRegAdLearnersFlag = data.NCHSUSLL221_MsCrRegAdLearnersFlag;
                        update.NCHSUSLL221_SplProgSlowAdLearnersFlag = data.NCHSUSLL221_SplProgSlowAdLearnersFlag;
                        update.NCHSUSLL221_ProtocolsmesAchievementsFlag = data.NCHSUSLL221_ProtocolsmesAchievementsFlag;                        
                        update.NCHSUSLL221_UpdatedDate = DateTime.Now;
                        update.NCHSUSLL221_UpdatedBy = data.UserId;

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
        public HSU_MasterCR2_DTO save_HSU_232(HSU_MasterCR2_DTO data)
        {
            try
            {

                if (data.NCHSUCS232_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_HSU_ClinicalSkills_232_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCHSUCS232_Year == data.ASMAY_Id && t.NCHSUCS232_CsTrclinicalskillsRelevantFlag == data.NCHSUCS232_CsTrclinicalskillsRelevantFlag && t.NCHSUCS232_PatientSimulatorsSimulationbasedFlag == data.NCHSUCS232_PatientSimulatorsSimulationbasedFlag && t.NCHSUCS232_StProgConductedSssessmentStudentsFlag == data.NCHSUCS232_StProgConductedSssessmentStudentsFlag
                    && t.NCHSUCS232_TrProgConForCsSblearningFlag == data.NCHSUCS232_TrProgConForCsSblearningFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_HSU_ClinicalSkills_232_DMO obj1 = new NAAC_HSU_ClinicalSkills_232_DMO();


                        //obj1.NCHSUCS232_Id = data.NCHSUCS232_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCHSUCS232_Year = data.ASMAY_Id;
                        obj1.NCHSUCS232_CsTrclinicalskillsRelevantFlag = data.NCHSUCS232_CsTrclinicalskillsRelevantFlag;
                        obj1.NCHSUCS232_TrProgConForCsSblearningFlag = data.NCHSUCS232_TrProgConForCsSblearningFlag;
                        obj1.NCHSUCS232_PatientSimulatorsSimulationbasedFlag = data.NCHSUCS232_PatientSimulatorsSimulationbasedFlag;
                        obj1.NCHSUCS232_StProgConductedSssessmentStudentsFlag = data.NCHSUCS232_StProgConductedSssessmentStudentsFlag;
                        
                        obj1.NCHSUCS232_CreatedDate = DateTime.Now;
                        obj1.NCHSUCS232_UpdatedDate = DateTime.Now;
                        obj1.NCHSUCS232_CreatedBy = data.UserId;
                        obj1.NCHSUCS232_UpdatedBy = data.UserId;

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
                else if (data.NCHSUCS232_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_HSU_ClinicalSkills_232_DMO.Where(t => t.NCHSUCS232_Id != data.NCHSUCS232_Id
                    && t.MI_Id == data.MI_Id
                    && t.NCHSUCS232_Year == data.ASMAY_Id && t.NCHSUCS232_CsTrclinicalskillsRelevantFlag == data.NCHSUCS232_CsTrclinicalskillsRelevantFlag && t.NCHSUCS232_PatientSimulatorsSimulationbasedFlag == data.NCHSUCS232_PatientSimulatorsSimulationbasedFlag && t.NCHSUCS232_StProgConductedSssessmentStudentsFlag == data.NCHSUCS232_StProgConductedSssessmentStudentsFlag
                    && t.NCHSUCS232_TrProgConForCsSblearningFlag == data.NCHSUCS232_TrProgConForCsSblearningFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_HSU_ClinicalSkills_232_DMO.Where(t => t.NCHSUCS232_Id == data.NCHSUCS232_Id && t.MI_Id == data.MI_Id).Single();

                        update.NCHSUCS232_Year = data.ASMAY_Id;
                        update.NCHSUCS232_CsTrclinicalskillsRelevantFlag = data.NCHSUCS232_CsTrclinicalskillsRelevantFlag;
                        update.NCHSUCS232_TrProgConForCsSblearningFlag = data.NCHSUCS232_TrProgConForCsSblearningFlag;
                        update.NCHSUCS232_PatientSimulatorsSimulationbasedFlag = data.NCHSUCS232_PatientSimulatorsSimulationbasedFlag;
                        update.NCHSUCS232_StProgConductedSssessmentStudentsFlag = data.NCHSUCS232_StProgConductedSssessmentStudentsFlag;
                      
                        update.NCHSUCS232_UpdatedDate = DateTime.Now;
                        update.NCHSUCS232_UpdatedBy = data.UserId;

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
        public HSU_MasterCR2_DTO save_HSU_255(HSU_MasterCR2_DTO data)
        {
            try
            {

                if (data.NCHSUEM255_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_HSU_ExaminationManagement_255_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCHSUEM255_Year == data.ASMAY_Id && t.NCHSUEM255_AnDivImpEMFlag == data.NCHSUEM255_AnDivImpEMFlag && t.NCHSUEM255_StuRegHtIssueProcessingFlag == data.NCHSUEM255_StuRegHtIssueProcessingFlag && t.NCHSUEM255_StuRegResultProcFlag == data.NCHSUEM255_StuRegResultProcFlag
                    && t.NCHSUEM255_ResultProcAtdFlag == data.NCHSUEM255_ResultProcAtdFlag 
                    && t.NCHSUEM255_ManualMethodologyFlag==data.NCHSUEM255_ManualMethodologyFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_HSU_ExaminationManagement_255_DMO obj1 = new NAAC_HSU_ExaminationManagement_255_DMO();


                        //obj1.NCHSUCS232_Id = data.NCHSUCS232_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCHSUEM255_Year = data.ASMAY_Id;
                        obj1.NCHSUEM255_AnDivImpEMFlag = data.NCHSUEM255_AnDivImpEMFlag;
                        obj1.NCHSUEM255_StuRegHtIssueProcessingFlag = data.NCHSUEM255_StuRegHtIssueProcessingFlag;
                        obj1.NCHSUEM255_StuRegResultProcFlag = data.NCHSUEM255_StuRegResultProcFlag;
                        obj1.NCHSUEM255_ResultProcAtdFlag = data.NCHSUEM255_ResultProcAtdFlag;
                        obj1.NCHSUEM255_ManualMethodologyFlag = data.NCHSUEM255_ManualMethodologyFlag;
                        obj1.NCHSUEM255_CreatedDate = DateTime.Now;
                        obj1.NCHSUEM255_UpdatedDate = DateTime.Now;
                        obj1.NCHSUEM255_CreatedBy = data.UserId;
                        obj1.NCHSUEM255_UpdatedBy = data.UserId;

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
                else if (data.NCHSUEM255_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_HSU_ExaminationManagement_255_DMO.Where(t => t.NCHSUEM255_Id != data.NCHSUEM255_Id && t.MI_Id == data.MI_Id
                    && t.NCHSUEM255_Year == data.ASMAY_Id && t.NCHSUEM255_AnDivImpEMFlag == data.NCHSUEM255_AnDivImpEMFlag && t.NCHSUEM255_StuRegHtIssueProcessingFlag == data.NCHSUEM255_StuRegHtIssueProcessingFlag && t.NCHSUEM255_StuRegResultProcFlag == data.NCHSUEM255_StuRegResultProcFlag
                    && t.NCHSUEM255_ResultProcAtdFlag == data.NCHSUEM255_ResultProcAtdFlag).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_HSU_ExaminationManagement_255_DMO.Where(t => t.NCHSUEM255_Id == data.NCHSUEM255_Id && t.MI_Id == data.MI_Id).Single();

                        update.NCHSUEM255_Year = data.ASMAY_Id;
                        update.NCHSUEM255_AnDivImpEMFlag = data.NCHSUEM255_AnDivImpEMFlag;
                        update.NCHSUEM255_StuRegHtIssueProcessingFlag = data.NCHSUEM255_StuRegHtIssueProcessingFlag;
                        update.NCHSUEM255_StuRegResultProcFlag = data.NCHSUEM255_StuRegResultProcFlag;
                        update.NCHSUEM255_ResultProcAtdFlag = data.NCHSUEM255_ResultProcAtdFlag;
                        update.NCHSUEM255_ManualMethodologyFlag = data.NCHSUEM255_ManualMethodologyFlag;
                        update.NCHSUEM255_UpdatedDate = DateTime.Now;
                        update.NCHSUEM255_UpdatedBy = data.UserId;

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
