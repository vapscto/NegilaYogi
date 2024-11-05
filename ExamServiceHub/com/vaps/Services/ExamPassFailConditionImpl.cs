
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.Extensions.Logging;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamPassFailConditionImpl : Interfaces.ExamPassFailConditionInterface
    {
        private static ConcurrentDictionary<string, ExamPassFailConditionDTO> _login =
         new ConcurrentDictionary<string, ExamPassFailConditionDTO>();

        private ExamContext _examcontext;
        ILogger<ExamPassFailConditionImpl> _acdimpl;
        public ExamPassFailConditionImpl(ExamContext masterexamContext, ILogger<ExamPassFailConditionImpl> _acdim)
        {
            _examcontext = masterexamContext;
            _acdimpl = _acdim;
        }

        public ExamPassFailConditionDTO Getdetails(ExamPassFailConditionDTO data)//int IVRMM_Id
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _examcontext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = year.Distinct().ToArray();

                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.EYC_ActiveFlg == true)
                                     select new ExamPassFailConditionDTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EYC_Id = b.EYC_Id,

                                     }).Distinct().ToArray();


                List<exammasterDMO> exams = new List<exammasterDMO>();
                exams = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).ToList();
                data.examlist = exams.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                List<Exm_Condition_MasterDMO> condition = new List<Exm_Condition_MasterDMO>();
                condition = _examcontext.Exm_Condition_MasterDMO.Where(y => y.MI_Id == data.MI_Id && y.ECM_ActiveFlg == true).ToList();
                data.examconditionlist = condition.Distinct().ToArray();

                data.passfailrank_list = (from a in _examcontext.Exm_Condition_MasterDMO
                                          from b in _examcontext.Exm_PassFailRank_ConditionDMO
                                          from c in _examcontext.Exm_Master_CategoryDMO                                           
                                          from e in _examcontext.Exm_Yearly_CategoryDMO
                                          from f in _examcontext.AcademicYear
                                          where (b.EMCA_Id == c.EMCA_Id && b.ASMAY_Id == f.ASMAY_Id && a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && a.ECM_ConditionFlag == b.EPFRC_Condition)
                                          select new ExamPassFailConditionDTO
                                          {
                                              ECM_ConditionName = a.ECM_ConditionName,
                                              EPFRC_From = b.EPFRC_From,
                                              EPFRC_To = b.EPFRC_To,
                                              EPFRC_RankFlag = b.EPFRC_RankFlag,
                                              EPFRC_PassFailFlag = b.EPFRC_PassFailFlag,
                                              EMCA_Id = c.EMCA_Id,
                                              EMCA_CategoryName = c.EMCA_CategoryName,
                                              EME_ExamName = b.EME_Id != 0 ? _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_Id == b.EME_Id).FirstOrDefault().EME_ExamName : "",
                                              EPFRC_ExamFlag = b.EPFRC_ExamFlag,
                                              ASMAY_Year = f.ASMAY_Year,
                                              EPFRC_OverallPercentage = b.EPFRC_OverallPercentage,
                                              EPFRC_ActiveFlag = b.EPFRC_ActiveFlag,
                                              EPFRC_Id = b.EPFRC_Id
                                          }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }

        public DateTime? CreatedDate = DateTime.Now;
        public ExamPassFailConditionDTO get_category(ExamPassFailConditionDTO data)
        {
            try
            {
                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id 
                                     && b.EYC_ActiveFlg == true)
                                     select new ExamPassFailConditionDTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EYC_Id = b.EYC_Id,

                                     }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamPassFailConditionDTO get_subjects(ExamPassFailConditionDTO data)
        {

            try
            {
                data.subjectlist = (from a in _examcontext.Exm_Yearly_Category_GroupDMO
                                    from b in _examcontext.Exm_Yearly_Category_Group_SubjectsDMO
                                    from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                    where (c.MI_Id == data.MI_Id && a.EYC_Id == data.EYC_Id && a.EYCG_Id == b.EYCG_Id && b.ISMS_Id == c.ISMS_Id && b.EYCGS_ActiveFlg == true && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && a.EYCG_ActiveFlg == true)
                                    select new ExamPassFailConditionDTO
                                    {

                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        ISMS_SubjectCode = c.ISMS_SubjectCode,
                                        ISMS_Max_Marks = c.ISMS_Max_Marks,
                                        ISMS_Min_Marks = c.ISMS_Min_Marks,
                                        ISMS_OrderFlag = c.ISMS_OrderFlag,
                                    }).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                List<int> exams = new List<int>();
                exams = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id).Select(t => t.EME_Id).ToList();

                data.examlist = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && !exams.Contains(t.EME_Id)).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamPassFailConditionDTO get_examcondition(ExamPassFailConditionDTO data)
        {
            try
            {
                List<Exm_Condition_MasterDMO> condition = new List<Exm_Condition_MasterDMO>();
                condition = _examcontext.Exm_Condition_MasterDMO.Where(y => y.MI_Id == data.MI_Id && y.ECM_ActiveFlg == true).ToList();
                data.examconditionlist = condition.Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamPassFailConditionDTO get_condition(ExamPassFailConditionDTO data)
        {
            try
            {
                data.conditiontype = (from a in _examcontext.Exm_Condition_MasterDMO
                                      where (a.MI_Id == data.MI_Id && a.ECM_Id == data.ECM_Id && a.ECM_ActiveFlg == true)
                                      select new ExamPassFailConditionDTO
                                      {
                                          ECM_Id = a.ECM_Id,
                                          ECM_ConditionName = a.ECM_ConditionName,
                                          ECM_ConditionFlag = a.ECM_ConditionFlag
                                      }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamPassFailConditionDTO deactive(ExamPassFailConditionDTO data)
        {
            ExamPassFailConditionDTO deact = new ExamPassFailConditionDTO();

            var result = _examcontext.Exm_PassFailRank_ConditionDMO.Where(t => t.EPFRC_Id == data.EPFRC_Id).ToList();
            for (var i = 0; i < result.Count(); i++)
            {
                var elcflag = result[i].EPFRC_ActiveFlag;
                if (elcflag == true)
                {
                    result[i].EPFRC_ActiveFlag = false;
                }
                else
                {
                    result[i].EPFRC_ActiveFlag = true;
                }

                _examcontext.Update(result[i]);
            }
            var flag = _examcontext.SaveChanges();
            if (flag >= 1)
            {
                deact.returnval = true;
            }
            else
            {
                deact.returnval = false;
            }

            return deact;
        }
        public ExamPassFailConditionDTO editdetails(int ID)
        {

            ExamPassFailConditionDTO data = new ExamPassFailConditionDTO();
            try
            {
                data.editlist = (from a in _examcontext.Exm_Condition_MasterDMO
                                 from b in _examcontext.Exm_PassFailRank_ConditionDMO
                                 from c in _examcontext.Exm_Master_CategoryDMO
                                // from d in _examcontext.masterexam
                                 from e in _examcontext.Exm_Yearly_CategoryDMO
                                 from f in _examcontext.AcademicYear

                                 where (b.EMCA_Id == c.EMCA_Id  && b.ASMAY_Id == f.ASMAY_Id && a.MI_Id == b.MI_Id && a.ECM_ConditionFlag == b.EPFRC_Condition && b.EPFRC_Id == ID)
                                 select new ExamPassFailConditionDTO
                                 {
                                     EPFRC_Id = b.EPFRC_Id,
                                     ECM_Id = a.ECM_Id,
                                     EME_Id = b.EME_Id,
                                     ECM_ConditionName = a.ECM_ConditionName,
                                     EPFRC_From = b.EPFRC_From,
                                     EPFRC_To = b.EPFRC_To,
                                     EPFRC_RankFlag = b.EPFRC_RankFlag,
                                     EPFRC_PassFailFlag = b.EPFRC_PassFailFlag,
                                     EMCA_Id = c.EMCA_Id,
                                     EMCA_CategoryName = c.EMCA_CategoryName,
                                     EME_ExamName = b.EME_Id != 0 ? _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_Id == b.EME_Id).FirstOrDefault().EME_ExamName : "",
                                     EPFRC_ExamFlag = b.EPFRC_ExamFlag,
                                     ASMAY_Id = f.ASMAY_Id,
                                     ASMAY_Year = f.ASMAY_Year,
                                     EPFRC_OverallPercentage = b.EPFRC_OverallPercentage,
                                     EPFRC_ActiveFlag = b.EPFRC_ActiveFlag

                                 }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamPassFailConditionDTO savedata(ExamPassFailConditionDTO data)
        {
            try
            {
                var exmcondition = (from a in _examcontext.Exm_Condition_MasterDMO
                                    where (a.MI_Id == data.MI_Id && a.ECM_Id == data.ECM_Id && a.ECM_ActiveFlg == true)
                                    select new ExamPassFailConditionDTO
                                    {
                                        ECM_Id = a.ECM_Id,
                                        ECM_ConditionName = a.ECM_ConditionName,
                                        ECM_ConditionFlag = a.ECM_ConditionFlag
                                    }).Distinct().ToArray();


                if (data.EPFRC_Id > 0)
                {


                    var result = _examcontext.Exm_PassFailRank_ConditionDMO.Where(t => t.MI_Id == data.MI_Id && t.EPFRC_Id == data.EPFRC_Id).ToList();


                    if (result.Count > 0)
                    {
                        //  Exm_PassFailRank_ConditionDMO Exm_PassFailRank = Mapper.Map<Exm_PassFailRank_ConditionDMO>(data);
                        var Exm_PassFailRank = _examcontext.Exm_PassFailRank_ConditionDMO.Single(t => t.MI_Id == data.MI_Id && t.EPFRC_Id == data.EPFRC_Id);
                        Exm_PassFailRank.ASMAY_Id = Convert.ToInt64(data.ASMAY_Id);
                        Exm_PassFailRank.EMCA_Id = Convert.ToInt32(data.EMCA_Id);
                        Exm_PassFailRank.EME_Id = Convert.ToInt32(data.EME_Id);
                        Exm_PassFailRank.EPFRC_Condition = exmcondition[0].ECM_ConditionFlag;
                        Exm_PassFailRank.EPFRC_From = Convert.ToInt32(data.EPFRC_From);
                        Exm_PassFailRank.EPFRC_To = Convert.ToInt32(data.EPFRC_To);
                        Exm_PassFailRank.EPFRC_ExamFlag = data.EPFRC_ExamFlag;
                        Exm_PassFailRank.EPFRC_PassFailFlag = data.EPFRC_PassFailFlag;
                        Exm_PassFailRank.EPFRC_RankFlag = Convert.ToInt32(data.EPFRC_RankFlag);
                        Exm_PassFailRank.EPFRC_OverallPercentage = data.EPFRC_OverallPercentage;

                        // Exm_PassFailRank.EPFRC_Percentage = Convert.ToDecimal(data.EPFRC_Percentage);
                        Exm_PassFailRank.EPFRC_ActiveFlag = true;

                        Exm_PassFailRank.CreatedDate = DateTime.Now;
                        Exm_PassFailRank.UpdatedDate = DateTime.Now;

                        _examcontext.Update(Exm_PassFailRank);

                        int contactExists = _examcontext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {


                    Exm_PassFailRank_ConditionDMO Exm_PassFailRank = Mapper.Map<Exm_PassFailRank_ConditionDMO>(data);
                    Exm_PassFailRank.ASMAY_Id = Convert.ToInt64(data.ASMAY_Id);
                    Exm_PassFailRank.EMCA_Id = Convert.ToInt32(data.EMCA_Id);
                    Exm_PassFailRank.EME_Id = Convert.ToInt32(data.EME_Id);
                    Exm_PassFailRank.EPFRC_Condition = exmcondition[0].ECM_ConditionFlag;
                    Exm_PassFailRank.EPFRC_From = Convert.ToInt32(data.EPFRC_From);
                    Exm_PassFailRank.EPFRC_To = Convert.ToInt32(data.EPFRC_To);
                    Exm_PassFailRank.EPFRC_ExamFlag = data.EPFRC_ExamFlag;
                    Exm_PassFailRank.EPFRC_PassFailFlag = data.EPFRC_PassFailFlag;
                    Exm_PassFailRank.EPFRC_RankFlag = Convert.ToInt32(data.EPFRC_RankFlag);
                    Exm_PassFailRank.EPFRC_OverallPercentage = data.EPFRC_OverallPercentage;
                    Exm_PassFailRank.EPFRC_ActiveFlag = true;
                    Exm_PassFailRank.CreatedDate = DateTime.Now;
                    Exm_PassFailRank.UpdatedDate = DateTime.Now;
                    _examcontext.Add(Exm_PassFailRank);

                    int contactExists = _examcontext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    }
}
