using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class CandidateInterviewListVMSService : Interfaces.CandidateInterviewListVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public CandidateInterviewListVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public HR_CandidateInterviewScheduleDTO getBasicData(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO SaveUpdate(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_CandidateInterviewScheduleDMO dmoObj = Mapper.Map<HR_CandidateInterviewScheduleDMO>(dto);
                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public async System.Threading.Tasks.Task<HR_CandidateInterviewScheduleDTO> editDataAsync(int id)
        {
            HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_CandidateInterviewScheduleDMO> lorg = new List<HR_CandidateInterviewScheduleDMO>();

                dto.VMSEditValue = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                   from b in _VMSContext.HR_Candidate_DetailsDMO
                                   where (a.HRCD_Id == b.HRCD_Id && a.HRCISC_Id == id)
                                   select new HR_CandidateInterviewScheduleDTO
                                   {
                                       HRCISC_Id = a.HRCISC_Id,
                                       HRCD_Id = a.HRCD_Id,
                                       HRCD_FullName = b.HRCD_FirstName + " " + (b.HRCD_MiddleName == null ? "" : b.HRCD_MiddleName) + " " + (b.HRCD_LastName == null ? "" : b.HRCD_LastName),
                                       HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                       HRCISC_InterviewDateTime = a.HRCISC_InterviewDateTime,
                                       HRCISC_InterviewVenue = a.HRCISC_InterviewVenue,
                                       HRCISC_NotifyEmail = a.HRCISC_NotifyEmail,
                                       HRCISC_NotifySMS = a.HRCISC_NotifySMS,
                                       HRCISC_Interviewer = a.HRCISC_Interviewer,
                                       HRCD_Photo = b.HRCD_Photo,
                                       HRCD_Resume = b.HRCD_Resume
                                   }).ToArray();

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INTERVIEW_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                    {
                        Value = id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.completedintvw = retObject.ToArray();
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO GetAllDropdownAndDatatableDetails(HR_CandidateInterviewScheduleDTO dto)
        {
            List<HR_CandidateInterviewScheduleDMO> datalist = new List<HR_CandidateInterviewScheduleDMO>();
            try
            {
                dto.VMSCandidateInterviewList = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                  from b in _VMSContext.HR_Candidate_DetailsDMO
                                  from c in _VMSContext.Institution
                                  where (a.HRCD_Id == b.HRCD_Id && b.MI_Id == c.MI_Id && b.HRCD_ActiveFlg == true && a.HRCISC_Interviewer == dto.UserId && a.HRCISC_Status != "Complete" && c.MI_ActiveFlag == 1)
                                  select new HR_CandidateInterviewScheduleDTO
                                  {
                                      HRCISC_Id = a.HRCISC_Id,
                                      HRCD_Id = a.HRCD_Id,
                                      HRCD_FullName = b.HRCD_FirstName + " " + (b.HRCD_MiddleName == null || b.HRCD_MiddleName == " " || b.HRCD_MiddleName == "0" ? " " : b.HRCD_MiddleName) + " " + (b.HRCD_LastName == null || b.HRCD_LastName == " " || b.HRCD_LastName == "0" ? " " : b.HRCD_LastName),
                                      HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                      HRCISC_InterviewDate = a.HRCISC_InterviewDateTime.ToString("dd-MM-yyyy"),
                                      HRCISC_Status = a.HRCISC_Status
                                  }).ToArray();

                //dto.VMSCandidateInterviewList = mrfdatalist.OrderByDescending(HRCISC_InterviewDateTime).ToArray();

                dto.CandidateDetailsList = (from emp in _VMSContext.HR_Candidate_DetailsDMO
                                            from c in _VMSContext.Institution
                                            where emp.HRCD_ActiveFlg == true && emp.MI_Id == c.MI_Id && c.MI_ActiveFlag == 1
                                            select new HR_CandidateInterviewScheduleDTO
                                          {
                                                HRCD_Id = emp.HRCD_Id,
                                                HRCD_FullName = emp.HRCD_FirstName + " " + emp.HRCD_MiddleName + " " + emp.HRCD_LastName,
                                                HRCD_FirstName = emp.HRCD_FirstName,
                                                HRCD_MiddleName = emp.HRCD_MiddleName,
                                                HRCD_LastName = emp.HRCD_LastName
                                            }).ToArray();

                dto.InterviewerList = (from emp in _VMSContext.IVRM_Staff_User_Login
                                       from empl in _VMSContext.HR_Master_Employee_DMO
                                       from empi in _VMSContext.Institution
                                       where (emp.Emp_Code == empl.HRME_Id && empl.MI_Id == empi.MI_Id && empl.HRME_ActiveFlag == true && empl.HRME_LeftFlag == false && empi.MI_ActiveFlag == 1)
                                       select new HR_CandidateInterviewScheduleDTO
                                       {
                                           Id = emp.Id,
                                           HRME_Id = empl.HRME_Id,
                                           HRME_EmployeeFirstName = empl.HRME_EmployeeFirstName,
                                           HRME_EmployeeMiddleName = empl.HRME_EmployeeMiddleName,
                                           HRME_EmployeeLastName = empl.HRME_EmployeeLastName
                                       }).ToArray();

                #region  CALENDER 
                try
                {
                    dto.calenderlist = (from m in _VMSContext.HR_CandidateInterviewScheduleDMO
                                        from n in _VMSContext.HR_Candidate_DetailsDMO
                                        where (m.HRCISC_InterviewDateTime != null && m.HRCD_Id == n.HRCD_Id && n.MI_Id == dto.MI_Id)
                                        select new HR_CandidateInterviewScheduleDTO
                                        {
                                            HRCISC_InterviewRounds = m.HRCISC_InterviewRounds,
                                            HRCD_FullName = n.HRCD_FirstName + " " + n.HRCD_MiddleName + " " + n.HRCD_LastName,
                                            HRCISC_InterviewDateTime = m.HRCISC_InterviewDateTime,
                                        }).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion

                dto.gradelist = _VMSContext.HR_Candidate_Master_GradeDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCMG_ActiveFlag==true).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO getallwithoutcondtn(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto.VMSCandidateInterviewList = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                                 from b in _VMSContext.HR_Candidate_DetailsDMO
                                                 from c in _VMSContext.Institution
                                                 where (a.HRCD_Id == b.HRCD_Id && b.MI_Id == c.MI_Id && b.HRCD_ActiveFlg == true && a.HRCISC_Status != "Complete" && c.MI_ActiveFlag == 1)
                                                 select new HR_CandidateInterviewScheduleDTO
                                                 {
                                                     HRCISC_Id = a.HRCISC_Id,
                                                     HRCD_Id = a.HRCD_Id,
                                                     HRCD_FullName = b.HRCD_FirstName + " " + (b.HRCD_MiddleName == null || b.HRCD_MiddleName == " " || b.HRCD_MiddleName == "0" ? " " : b.HRCD_MiddleName) + " " + (b.HRCD_LastName == null || b.HRCD_LastName == " " || b.HRCD_LastName == "0" ? " " : b.HRCD_LastName),
                                                     HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                                     HRCISC_InterviewDate = a.HRCISC_InterviewDateTime.ToString("dd-MM-yyyy"),
                                                     HRCISC_Status = a.HRCISC_Status,
                                                     HRCISC_ActiveFlg = a.HRCISC_ActiveFlg
                                                 }).ToArray();

                dto.CandidateDetailsList = (from emp in _VMSContext.HR_Candidate_DetailsDMO
                                            from c in _VMSContext.Institution
                                            where emp.HRCD_ActiveFlg == true && emp.MI_Id == c.MI_Id && c.MI_ActiveFlag == 1
                                            select new HR_CandidateInterviewScheduleDTO
                                            {
                                                HRCD_Id = emp.HRCD_Id,
                                                HRCD_FullName = emp.HRCD_FirstName + " " + emp.HRCD_MiddleName + " " + emp.HRCD_LastName,
                                                HRCD_FirstName = emp.HRCD_FirstName,
                                                HRCD_MiddleName = emp.HRCD_MiddleName,
                                                HRCD_LastName = emp.HRCD_LastName
                                            }).ToArray();

                dto.InterviewerList = (from emp in _VMSContext.IVRM_Staff_User_Login
                                       from empl in _VMSContext.HR_Master_Employee_DMO
                                       from empi in _VMSContext.Institution
                                       where (emp.Emp_Code == empl.HRME_Id && empl.MI_Id == empi.MI_Id && empl.HRME_ActiveFlag == true && empl.HRME_LeftFlag == false && empi.MI_ActiveFlag == 1)
                                       select new HR_CandidateInterviewScheduleDTO
                                       {
                                           Id = emp.Id,
                                           HRME_Id = empl.HRME_Id,
                                           HRME_EmployeeFirstName = empl.HRME_EmployeeFirstName,
                                           HRME_EmployeeMiddleName = empl.HRME_EmployeeMiddleName,
                                           HRME_EmployeeLastName = empl.HRME_EmployeeLastName
                                       }).ToArray();

                #region  CALENDER 
                try
                {
                    dto.calenderlist = (from m in _VMSContext.HR_CandidateInterviewScheduleDMO
                                        from n in _VMSContext.HR_Candidate_DetailsDMO
                                        where (m.HRCISC_InterviewDateTime != null && m.HRCD_Id == n.HRCD_Id && n.MI_Id == dto.MI_Id)
                                        select new HR_CandidateInterviewScheduleDTO
                                        {
                                            HRCISC_InterviewRounds = m.HRCISC_InterviewRounds,
                                            HRCD_FullName = n.HRCD_FirstName + " " + n.HRCD_MiddleName + " " + n.HRCD_LastName,
                                            HRCISC_InterviewDateTime = m.HRCISC_InterviewDateTime,
                                        }).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO deactivateRecordById(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRCISC_Id > 0)
                {
                    var result = _VMSContext.HR_CandidateInterviewScheduleDMO.Single(t => t.HRCISC_Id == dto.HRCISC_Id);

                    if (result.HRCISC_ActiveFlg == true)
                    {
                        result.HRCISC_ActiveFlg = false;
                    }
                    else if (result.HRCISC_ActiveFlg == false)
                    {
                        result.HRCISC_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRCISC_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto.VMSCandidateInterviewList = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                                     from b in _VMSContext.HR_Candidate_DetailsDMO
                                                     from c in _VMSContext.Institution
                                                     where (a.HRCD_Id == b.HRCD_Id && b.MI_Id == c.MI_Id && b.HRCD_ActiveFlg == true && a.HRCISC_Status != "Complete" && c.MI_ActiveFlag == 1)
                                                     select new HR_CandidateInterviewScheduleDTO
                                                     {
                                                         HRCISC_Id = a.HRCISC_Id,
                                                         HRCD_Id = a.HRCD_Id,
                                                         HRCD_FullName = b.HRCD_FirstName + " " + (b.HRCD_MiddleName == null || b.HRCD_MiddleName == " " || b.HRCD_MiddleName == "0" ? " " : b.HRCD_MiddleName) + " " + (b.HRCD_LastName == null || b.HRCD_LastName == " " || b.HRCD_LastName == "0" ? " " : b.HRCD_LastName),
                                                         HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                                         HRCISC_InterviewDate = a.HRCISC_InterviewDateTime.ToString("dd-MM-yyyy"),
                                                         HRCISC_Status = a.HRCISC_Status,
                                                         HRCISC_ActiveFlg = a.HRCISC_ActiveFlg
                                                     }).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
    }
}
