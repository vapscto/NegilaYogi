using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Training_Feedback : Interfaces.Training_Feedback_Interface
    {
        public VMSContext _vmsconte;
        public HRMSContext _hrms;

        public Training_Feedback(VMSContext vmsContext, HRMSContext hrms)
        {
            _vmsconte = vmsContext;
            _hrms = hrms;

        }
        //===============================Building==============================================
        public HR_Training_Feedback_DTO loaddata(HR_Training_Feedback_DTO data)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.userId).Select(t => t.Emp_Code).FirstOrDefault();

                data.traninglisttrainer = (from a in _vmsconte.HR_Training_Create_DMO_con
                                           from b in _vmsconte.HR_Training_Create_IntTrainer_DMO_con
                                           where (a.HRTCR_Id == b.HRTCR_Id && b.HRME_Id == userhrme_id && a.HRTCR_ActiveFlag == true && a.HRTCR_StatusFlg == 2)
                                           select new HR_Training_Feedback_DTO {
                                               HRTCR_Id = a.HRTCR_Id,
                                               HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                               HRTCR_ProgramDesc = a.HRTCR_ProgramDesc
                                           }).ToArray();

                data.traninglisttrainee = (from a in _vmsconte.HR_Training_Create_DMO_con
                                           from b in _vmsconte.HR_Training_Create_Participants_DMO_con
                                           where (a.HRTCR_Id == b.HRTCR_Id && b.HRME_Id == userhrme_id && a.HRTCR_ActiveFlag == true && a.HRTCR_StatusFlg == 2)
                                           select new HR_Training_Feedback_DTO
                                           {
                                               HRTCR_Id = a.HRTCR_Id,
                                               HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                               HRTCR_ProgramDesc = a.HRTCR_ProgramDesc
                                           }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HR_Training_Feedback_DTO getQuestions(HR_Training_Feedback_DTO data)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.userId).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.Type == "Trainer") {
                    data.trainingFeedbacklist = _vmsconte.HR_Training_Feedback_DMO_con.Where(t => t.HRTCR_Id == data.HRTCR_Id && t.HRTFEED_TrainerId == userhrme_id).ToArray();

                    data.trainerlist = (from a in _vmsconte.HR_Training_Create_DMO_con
                                        from b in _vmsconte.HR_Training_Create_IntTrainer_DMO_con
                                        from c in _vmsconte.HR_Master_Employee_DMO
                                        where (a.HRTCR_Id == b.HRTCR_Id && a.HRTCR_Id == data.HRTCR_Id && b.HRTCRINTTR_ActiveFlg == true && b.HRME_Id == c.HRME_Id)
                                        select new HR_Training_Feedback_DTO {
                                            HRME_Id = b.HRME_Id,
                                            HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null ? "":c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? "":c.HRME_EmployeeLastName)
                                        }).ToArray();
                }
                else if (data.Type == "Trainee") {
                    data.trainingFeedbacklist = _vmsconte.HR_Training_Feedback_DMO_con.Where(t => t.HRTCR_Id == data.HRTCR_Id && t.HRME_Id == userhrme_id).ToArray();

                    data.traineelist = (from a in _vmsconte.HR_Training_Create_DMO_con
                                        from b in _vmsconte.HR_Training_Create_Participants_DMO_con
                                        from c in _vmsconte.HR_Master_Employee_DMO
                                        where (a.HRTCR_Id == b.HRTCR_Id && a.HRTCR_Id == data.HRTCR_Id && b.HRME_Id == c.HRME_Id)
                                        select new HR_Training_Feedback_DTO
                                        {
                                            HRME_Id = b.HRME_Id,
                                            HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null ? "" : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? "" : c.HRME_EmployeeLastName)
                                        }).ToArray();
                }

                data.mappedquestionlist = (from a in _vmsconte.HR_Training_Question_DMO_con
                                           from b in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                           where (a.HRTCR_Id == data.HRTCR_Id && a.HRMFQNS_Id == b.HRMFQNS_Id && a.HRTRQNS_ActiveFlg == true && b.HRMFQNS_QuestionForFlg == data.Type && b.HRMFQNS_ActiveFlg == true)
                                           select new HR_Training_Feedback_DTO {
                                               HRMFQNS_Id = b.HRMFQNS_Id,
                                               HRMFQNS_QuestionName = b.HRMFQNS_QuestionName,
                                           }).ToArray();

                data.mappedoptionlist = (from a in _vmsconte.HR_Training_Question_DMO_con
                                         from b in _vmsconte.HR_Master_Question_Option_DMO_con
                                         from c in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                         where (a.HRTCR_Id == data.HRTCR_Id && a.HRMFQNS_Id == b.HRMFQNS_Id && a.HRTRQNS_ActiveFlg == true && c.HRMFOPT_OptionFor == data.Type && b.HRMQNOP_ActiveFlg == true && b.HRMFOPT_Id == c.HRMFOPT_Id && c.HRMFOPT_ActiveFlg == true && b.MI_Id == c.MI_Id)
                                           select new HR_Training_Feedback_DTO
                                           {
                                               HRMFOPT_Id = b.HRMFOPT_Id,
                                               HRMFOPT_OptionName = c.HRMFOPT_OptionName,
                                               HRMFQNS_Id = b.HRMFQNS_Id
                                           }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HR_Training_Feedback_DTO savetrainerfeedback(HR_Training_Feedback_DTO dto)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();

                for (int counter = 0; counter < dto.question_Answer.Length; counter++)
                {
                    if (dto.question_Answer[counter].HRTFEED_Id > 0)
                    {
                        var result = _vmsconte.HR_Training_Feedback_DMO_con.Single(t => t.HRTFEED_Id == dto.question_Answer[counter].HRTFEED_Id);
                        result.MI_Id = dto.MI_Id;
                        result.HRTCR_Id = dto.HRTCR_Id;
                        result.HRME_Id = userhrme_id;
                        result.HRTFEED_IntORExtFlg = "Internal";
                        result.HRTFEED_TrainerId = dto.HRTFEED_TrainerId;
                        result.HRMFQNS_Id = dto.question_Answer[counter].HRMFQNS_Id;
                        result.HRMFOPT_Id = dto.question_Answer[counter].answer;
                        result.HRTFEED_ActiveFlg = true;
                        result.HRTFEED_CreatedBy = dto.userId;
                        result.HRTFEED_UpdatedBy = dto.userId;
                        result.HRTFEED_CreatedDate = DateTime.Now;
                        result.HRTFEED_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Update";
                    }
                    else
                    {
                        HR_Training_Feedback_DMO hm = new HR_Training_Feedback_DMO();
                        hm.MI_Id = dto.MI_Id;
                        hm.HRTCR_Id = dto.HRTCR_Id;
                        hm.HRME_Id = userhrme_id;
                        hm.HRTFEED_IntORExtFlg = "Internal";
                        hm.HRTFEED_TrainerId = dto.HRTFEED_TrainerId;
                        hm.HRMFQNS_Id = dto.question_Answer[counter].HRMFQNS_Id;
                        hm.HRMFOPT_Id = dto.question_Answer[counter].answer;
                        hm.HRTFEED_ActiveFlg = true;
                        hm.HRTFEED_CreatedBy = dto.userId;
                        hm.HRTFEED_UpdatedBy = dto.userId;
                        hm.HRTFEED_CreatedDate = DateTime.Now;
                        hm.HRTFEED_UpdatedDate = DateTime.Now;
                        _vmsconte.Add(hm);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Add";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Training_Feedback_DTO savetraineefeedback(HR_Training_Feedback_DTO dto)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();

                for (int counter = 0; counter < dto.question_Answer.Length; counter++)
                {
                    if (dto.question_Answer[counter].HRTFEED_Id > 0)
                    {
                        var result = _vmsconte.HR_Training_Feedback_DMO_con.Single(t => t.HRTFEED_Id == dto.question_Answer[counter].HRTFEED_Id);
                        result.MI_Id = dto.MI_Id;
                        result.HRTCR_Id = dto.HRTCR_Id;
                        result.HRME_Id = dto.HRME_Id;
                        result.HRTFEED_IntORExtFlg = "Internal";
                        result.HRTFEED_TrainerId = userhrme_id;
                        result.HRMFQNS_Id = dto.question_Answer[counter].HRMFQNS_Id;
                        result.HRMFOPT_Id = dto.question_Answer[counter].answer;
                        result.HRTFEED_ActiveFlg = true;
                        result.HRTFEED_CreatedBy = dto.userId;
                        result.HRTFEED_UpdatedBy = dto.userId;
                        result.HRTFEED_CreatedDate = DateTime.Now;
                        result.HRTFEED_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Update";
                    }
                    else
                    {
                        HR_Training_Feedback_DMO hm = new HR_Training_Feedback_DMO();
                        hm.MI_Id = dto.MI_Id;
                        hm.HRTCR_Id = dto.HRTCR_Id;
                        hm.HRME_Id = dto.HRME_Id;
                        hm.HRTFEED_IntORExtFlg = "Internal";
                        hm.HRTFEED_TrainerId = userhrme_id;
                        hm.HRMFQNS_Id = dto.question_Answer[counter].HRMFQNS_Id;
                        hm.HRMFOPT_Id = dto.question_Answer[counter].answer;
                        hm.HRTFEED_ActiveFlg = true;
                        hm.HRTFEED_CreatedBy = dto.userId;
                        hm.HRTFEED_UpdatedBy = dto.userId;
                        hm.HRTFEED_CreatedDate = DateTime.Now;
                        hm.HRTFEED_UpdatedDate = DateTime.Now;
                        _vmsconte.Add(hm);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Add";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
