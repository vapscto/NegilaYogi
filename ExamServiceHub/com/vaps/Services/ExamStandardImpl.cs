
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

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamStandardImpl : Interfaces.ExamStandardInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _examcontext;

        public ExamStandardImpl(ExamContext examContext)
        {
            _examcontext = examContext;
        }

        public ExamStandardDTO Getdetails(int id)//int IVRMM_Id
        {
            ExamStandardDTO data = new ExamStandardDTO();
            try
            {
                List<Exm_ConfigurationDMO> config = new List<Exm_ConfigurationDMO>();
                config = _examcontext.Exm_ConfigurationDMO.Where(t => t.MI_Id == id).ToList();
                data.exm_config = config.Distinct().ToArray();

                if (config.Count > 0)
                {
                    data.ExmConfig_Id = config[0].ExmConfig_Id;
                    data.ExmConfig_RankingMethod = config[0].ExmConfig_RankingMethod;
                    data.ExmConfig_PromotionFlag = config[0].ExmConfig_PromotionFlag;
                    data.ExmConfig_PassFailRankFlag = config[0].ExmConfig_PassFailRankFlag;
                    data.ExmConfig_Recordsearchtype = config[0].ExmConfig_Recordsearchtype;
                    data.ExmConfig_Remarks = config[0].ExmConfig_Remarks;
                    data.ExmConfig_GraceAplFlg = config[0].ExmConfig_GraceAplFlg;
                    data.ExmConfig_BonusAplFlag = config[0].ExmConfig_BonusAplFlag;
                    data.ExmConfig_MinAttAplFlag = config[0].ExmConfig_MinAttAplFlag;
                    data.ExmConfig_MarksMultiply = config[0].ExmConfig_MarksMultiply;
                    data.ExmConfig_NoOfDecimal = config[0].ExmConfig_NoOfDecimal;
                    data.ExmConfig_GroupMarksToResultFlg = config[0].ExmConfig_GroupMarksToResultFlg;
                    data.ExmConfig_EnableFractionFlg = config[0].ExmConfig_EnableFractionFlg;
                    data.ExmConfig_EntryDateRestFlg = config[0].ExmConfig_EntryDateRestFlg;
                    data.ExmConfig_AdmnoColumnDisplay = config[0].ExmConfig_AdmnoColumnDisplay;
                    data.ExmConfig_RegnoColumnDisplay = config[0].ExmConfig_RegnoColumnDisplay;
                    data.ExmConfig_RollnoColumnDisplay = config[0].ExmConfig_RollnoColumnDisplay;
                    data.ExmConfig_RoundoffFlag = config[0].ExmConfig_RoundoffFlag;
                    data.ExmConfig_NoOfDecimalValues = config[0].ExmConfig_NoOfDecimalValues;
                    data.ExmConfig_PerRoundoffFlag = config[0].ExmConfig_PerRoundoffFlag;
                    data.ExmConfig_HallTicketFlg = config[0].ExmConfig_HallTicketFlg;

                    data.ExmConfig_FailBoldFlg = config[0].ExmConfig_FailBoldFlg;
                    data.ExmConfig_FailItalicFlg = config[0].ExmConfig_FailItalicFlg;
                    data.ExmConfig_FailUnderscoreFlg = config[0].ExmConfig_FailUnderscoreFlg;
                    data.ExmConfig_FailColorFlg = config[0].ExmConfig_FailColorFlg;
                    data.ExmConfig_AllSubjectAbsentFlg = config[0].ExmConfig_AllSubjectAbsentFlg;
                    data.ExmConfig_FeeDefaulterDisplayFlg = config[0].ExmConfig_FeeDefaulterDisplayFlg;
                    data.ExmConfig_AdmNoRegNoRollNo_DefaultFlag = config[0].ExmConfig_AdmNoRegNoRollNo_DefaultFlag;

                    data.ExmConfig_ClassPositionFlg = config[0].ExmConfig_ClassPositionFlg;
                    data.ExmConfig_SectionPositionFlg = config[0].ExmConfig_SectionPositionFlg;
                    data.ExmConfig_ClassRankFlg = config[0].ExmConfig_ClassRankFlg;
                    data.ExmConfig_SecRankFlg = config[0].ExmConfig_SecRankFlg;
                    data.ExmConfig_FailRankFlg = config[0].ExmConfig_FailRankFlg;
                    data.ExmConfig_GroupwiseMarksFlg = config[0].ExmConfig_GroupwiseMarksFlg;
                    data.ExmConfig_SubjectwiseSecRankFlg = config[0].ExmConfig_SubjectwiseSecRankFlg;
                    data.ExmConfig_SubjectwiseClassRankFlg = config[0].ExmConfig_SubjectwiseClassRankFlg;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }


        public ExamStandardDTO savedetails(ExamStandardDTO data)
        {
            Exm_ConfigurationDMO objpge = Mapper.Map<Exm_ConfigurationDMO>(data);
            try
            {
                if (objpge.ExmConfig_Id > 0)
                {
                    var result = _examcontext.Exm_ConfigurationDMO.Single(t => t.ExmConfig_Id == objpge.ExmConfig_Id && t.MI_Id == objpge.MI_Id);
                    result.ExmConfig_RankingMethod = objpge.ExmConfig_RankingMethod;
                    result.ExmConfig_PromotionFlag = objpge.ExmConfig_PromotionFlag;
                    result.ExmConfig_PassFailRankFlag = objpge.ExmConfig_PassFailRankFlag;
                    result.ExmConfig_Recordsearchtype = objpge.ExmConfig_Recordsearchtype;
                    result.ExmConfig_Remarks = objpge.ExmConfig_Remarks;
                    result.ExmConfig_GraceAplFlg = objpge.ExmConfig_GraceAplFlg;
                    result.ExmConfig_BonusAplFlag = objpge.ExmConfig_BonusAplFlag;
                    result.ExmConfig_MinAttAplFlag = objpge.ExmConfig_MinAttAplFlag;
                    result.ExmConfig_MarksMultiply = objpge.ExmConfig_MarksMultiply;
                    result.ExmConfig_NoOfDecimal = objpge.ExmConfig_NoOfDecimal;
                    result.ExmConfig_GroupMarksToResultFlg = objpge.ExmConfig_GroupMarksToResultFlg;
                    result.ExmConfig_EnableFractionFlg = objpge.ExmConfig_EnableFractionFlg;
                    result.ExmConfig_EntryDateRestFlg = objpge.ExmConfig_EntryDateRestFlg;
                    result.ExmConfig_AdmnoColumnDisplay = objpge.ExmConfig_AdmnoColumnDisplay;
                    result.ExmConfig_RegnoColumnDisplay = objpge.ExmConfig_RegnoColumnDisplay;
                    result.ExmConfig_RollnoColumnDisplay = objpge.ExmConfig_RollnoColumnDisplay;
                    result.ExmConfig_RoundoffFlag = objpge.ExmConfig_RoundoffFlag;
                    result.ExmConfig_NoOfDecimalValues = objpge.ExmConfig_NoOfDecimalValues;
                    result.ExmConfig_PerRoundoffFlag = objpge.ExmConfig_PerRoundoffFlag;
                    result.ExmConfig_HallTicketFlg = objpge.ExmConfig_HallTicketFlg;
                    result.ExmConfig_FailBoldFlg = objpge.ExmConfig_FailBoldFlg;
                    result.ExmConfig_FailItalicFlg = objpge.ExmConfig_FailItalicFlg;
                    result.ExmConfig_FailUnderscoreFlg = objpge.ExmConfig_FailUnderscoreFlg;
                    result.ExmConfig_FailColorFlg = objpge.ExmConfig_FailColorFlg;
                    result.ExmConfig_AllSubjectAbsentFlg = objpge.ExmConfig_AllSubjectAbsentFlg;
                    result.ExmConfig_FeeDefaulterDisplayFlg = objpge.ExmConfig_FeeDefaulterDisplayFlg;
                    result.ExmConfig_AdmNoRegNoRollNo_DefaultFlag = objpge.ExmConfig_AdmNoRegNoRollNo_DefaultFlag;

                    result.ExmConfig_ClassPositionFlg = objpge.ExmConfig_ClassPositionFlg;
                    result.ExmConfig_SectionPositionFlg = objpge.ExmConfig_SectionPositionFlg;
                    result.ExmConfig_ClassRankFlg = objpge.ExmConfig_ClassRankFlg;
                    result.ExmConfig_SecRankFlg = objpge.ExmConfig_SecRankFlg;
                    result.ExmConfig_FailRankFlg = objpge.ExmConfig_FailRankFlg;
                    result.ExmConfig_GroupwiseMarksFlg = objpge.ExmConfig_GroupwiseMarksFlg;
                    result.ExmConfig_SubjectwiseSecRankFlg = objpge.ExmConfig_SubjectwiseSecRankFlg;
                    result.ExmConfig_SubjectwiseClassRankFlg = objpge.ExmConfig_SubjectwiseClassRankFlg;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    objpge.CreatedDate = DateTime.Now;
                    objpge.UpdatedDate = DateTime.Now;
                    _examcontext.Add(objpge);
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists == 1)
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
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    }
}
