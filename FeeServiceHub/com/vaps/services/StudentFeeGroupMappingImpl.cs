using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vaps.admission;

namespace FeeServiceHub.com.vaps.services
{
    public class StudentFeeGroupMappingImpl : interfaces.StudentFeeGroupMappingInterface
    {
        private static ConcurrentDictionary<string, FeeStudentGroupMappingDTO> _login =
           new ConcurrentDictionary<string, FeeStudentGroupMappingDTO>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<StudentFeeGroupMappingImpl> _logger;

        public StudentFeeGroupMappingImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<StudentFeeGroupMappingImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }

        public FeeStudentGroupMappingDTO deleterec(FeeStudentGroupMappingDTO data)
        {
            using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
            {
                try
                {
                    var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMapping @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.AMST_Id, data.ASMAY_Id, data.FMG_Id, data.FMSG_Id);

                    if (outputval >= 1)
                    {
                        transaction.Commit();
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }

                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from c in _YearlyFeeGroupMappingContext.School_M_Class
                                         from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                         from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                         from f in _YearlyFeeGroupMappingContext.AcademicYear
                                         where (f.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S")
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = e.AMST_FirstName,
                                             AMST_MiddleName = e.AMST_MiddleName,
                                             AMST_LastName = e.AMST_LastName,
                                             AMST_AdmNo = e.AMST_AdmNo,
                                             AMST_RegistrationNo = e.AMST_RegistrationNo,
                                             AMAY_RollNo = d.AMAY_RollNo,
                                             FMG_GroupName = b.FMG_GroupName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             FMSG_Id = a.FMSG_Id,
                                             FMG_Id = b.FMG_Id
                                         }
          ).ToArray();
                }

                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                    _logger.LogError(ee.Message);
                }

                return data;
            }
        }
        public FeeStudentGroupMappingDTO deleterec_s(FeeStudentGroupMappingDTO data)
        {
            //  using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
            //  {
            try
            {
                //var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMapping @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.AMST_Id, data.ASMAY_Id, data.FMG_Id, data.FMSG_Id);
                var paid_amount = _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.HRME_Id == data.HRME_Id && s.FMG_Id == data.FMG_Id).Sum(t => t.FSSST_PaidAmount);
                if (paid_amount == 0)
                {

                    var staff_status_list = _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.HRME_Id == data.HRME_Id && s.FMG_Id == data.FMG_Id).ToList();
                    var staff_group = _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead.Where(g => g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.HRME_Id == data.HRME_Id && g.FMG_Id == data.FMG_Id && g.FMSTGH_Id == data.FMSTGH_Id).ToList();
                    var staff_grp_head_instmnts = _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead_Installments.Where(h => h.FMSTGH_Id == data.FMSTGH_Id).ToList();
                    if (staff_status_list.Any())
                    {
                        for (int i = 0; staff_status_list.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(staff_status_list.ElementAt(i));
                        }
                    }
                    if (staff_grp_head_instmnts.Any())
                    {
                        for (int i = 0; staff_grp_head_instmnts.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(staff_grp_head_instmnts.ElementAt(i));
                        }
                    }
                    if (staff_group.Any())
                    {
                        for (int i = 0; staff_group.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(staff_group.ElementAt(i));
                        }
                    }
                    var outputval = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (outputval >= 1)
                    {
                        // transaction.Commit();
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }


                }
                else
                {
                    data.returnval = "false";
                }
                data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                       from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                           // from c in _YearlyFeeGroupMappingContext.MasterEmployee
                                       from d in _YearlyFeeGroupMappingContext.feeGroup
                                       from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                       from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                           //from c in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead_Installments
                                       where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMSTGH_Id = b.FMSTGH_Id,
                                           HRME_Id = a.HRME_Id,
                                           HRME_EmployeeCode = a.HRME_EmployeeCode,
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                           HRMD_Id = e.HRMD_Id,
                                           HRMDES_Id = f.HRMDES_Id,
                                           FMG_Id = b.FMG_Id,
                                           HRMD_DepartmentName = e.HRMD_DepartmentName,
                                           HRMDES_DesignationName = f.HRMDES_DesignationName,
                                           FMG_GroupName = d.FMG_GroupName
                                       }).ToList().Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return data;
            //  }
        }

        public FeeStudentGroupMappingDTO deleterec_o(FeeStudentGroupMappingDTO data)
        {
            //  using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
            //  {
            try
            {
                //var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMapping @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.AMST_Id, data.ASMAY_Id, data.FMG_Id, data.FMSG_Id);

                var paid_amount = _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.FMOST_Id == data.FMOST_Id && s.FMG_Id == data.FMG_Id).Sum(t => t.FSSOST_PaidAmount);
                if (paid_amount == 0)
                {

                    var oth_student_status_list = _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.FMOST_Id == data.FMOST_Id && s.FMG_Id == data.FMG_Id).ToList();
                    var oth_stu_group = _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO.Where(g => g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMOST_Id == data.FMOST_Id && g.FMG_Id == data.FMG_Id && g.FMOSTGH_Id == data.FMOSTGH_Id).ToList();
                    var oth_stu_grp_head_instmnts = _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GH_InstlDMO.Where(h => h.FMOSTGH_Id == data.FMOSTGH_Id).ToList();
                    if (oth_student_status_list.Any())
                    {
                        for (int i = 0; oth_student_status_list.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(oth_student_status_list.ElementAt(i));
                        }
                    }
                    if (oth_stu_grp_head_instmnts.Any())
                    {
                        for (int i = 0; oth_stu_grp_head_instmnts.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(oth_stu_grp_head_instmnts.ElementAt(i));
                        }
                    }
                    if (oth_stu_group.Any())
                    {
                        for (int i = 0; oth_stu_group.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(oth_stu_group.ElementAt(i));
                        }
                    }
                    var outputval = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (outputval >= 1)
                    {
                        // transaction.Commit();
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
                else
                {
                    data.returnval = "false";
                }
                data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                             from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                             from c in _YearlyFeeGroupMappingContext.feeGroup
                                             where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id)
                                             select new FeeStudentGroupMappingDTO
                                             {
                                                 FMOSTGH_Id = b.FMOSTGH_Id,
                                                 FMOST_Id = a.FMOST_Id,
                                                 FMOST_StudentName = a.FMOST_StudentName,
                                                 FMOST_StudentMobileNo = a.FMOST_StudentMobileNo,
                                                 FMOST_StudentEmailId = a.FMOST_StudentEmailId,
                                                 FMG_Id = b.FMG_Id,
                                                 FMG_GroupName = c.FMG_GroupName
                                             }).ToList().Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return data;
            //  }
        }
        public FeeStudentGroupMappingDTO EditMasterscetionDetails(int id)
        {
            throw new NotImplementedException();
        }

        public FeeStudentGroupMappingDTO getdata(FeeStudentGroupMappingDTO fee)
        {

            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == fee.MI_Id && t.Is_Active == true).OrderByDescending(l => l.ASMAY_Order).ToList();
                fee.academicdrp = allyear.Distinct().ToArray();

                fee.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                       from b in _YearlyFeeGroupMappingContext.feeGroup
                                       from f in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                       from g in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                       where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == fee.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == fee.user_id && a.ASMAY_Id == fee.ASMAY_Id && a.ASMAY_Id == g.ASMAY_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = a.FMG_Id,
                                           FMG_GroupName = b.FMG_GroupName
                                       }).Distinct().ToArray();

                fee.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                      from b in _YearlyFeeGroupMappingContext.feeGroup
                                      from c in _YearlyFeeGroupMappingContext.feehead
                                      where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1")
                                      select new FeeStudentGroupMappingDTO
                                      {
                                          FMG_Id = b.FMG_Id,
                                          FMH_Id = c.FMH_Id,
                                          FMH_FeeName = c.FMH_FeeName
                                      }).Distinct().ToArray();

                fee.fillinstallment = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                       from b in _YearlyFeeGroupMappingContext.feeGroup
                                       from c in _YearlyFeeGroupMappingContext.feehead
                                       from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                       from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                       where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == fee.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == fee.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == fee.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FTI_Id = e.FTI_Id,
                                           FTI_Name = e.FTI_Name
                                       }).Distinct().ToArray();

                fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                               from c in _YearlyFeeGroupMappingContext.School_M_Class
                               from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                               from e in _YearlyFeeGroupMappingContext.AdmSection
                               where (d.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.AMST_Id == d.AMST_Id && e.ASMS_Id == d.ASMS_Id   &&   d.ASMCL_Id == c.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                               select new FeeStudentGroupMappingDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   AMST_FirstName = a.AMST_FirstName,
                                   AMST_MiddleName = a.AMST_MiddleName,
                                   AMST_LastName = a.AMST_LastName,
                                   AMST_AdmNo = a.AMST_AdmNo,
                                   AMST_RegistrationNo = a.AMST_RegistrationNo,
                                   AMAY_RollNo = d.AMAY_RollNo,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMC_SectionName = e.ASMC_SectionName
                               }
        ).Distinct().Take(10).ToArray(); /* .Take(5)*/

                var fetchmaxfmsgid = _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO.Where(t => t.MI_Id == fee.MI_Id && t.ASMAY_Id == fee.ASMAY_Id).OrderByDescending(t => t.FMSG_Id).Take(5).Select(t => t.FMSG_Id).ToList();

                fee.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.School_M_Class
                                    from g in _YearlyFeeGroupMappingContext.school_M_Section
                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                    where (d.ASMS_Id == g.ASMS_Id && fetchmaxfmsgid.Contains(a.FMSG_Id) && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == fee.MI_Id && d.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = e.AMST_FirstName,
                                        AMST_MiddleName = e.AMST_MiddleName,
                                        AMST_LastName = e.AMST_LastName,
                                        AMST_AdmNo = e.AMST_AdmNo,
                                        AMST_RegistrationNo = e.AMST_RegistrationNo,
                                        AMAY_RollNo = d.AMAY_RollNo,
                                        FMG_GroupName = b.FMG_GroupName,
                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                        ASMC_SectionName = g.ASMC_SectionName,
                                        AMST_Mobile = e.AMST_MobileNo,
                                        FMSG_Id = a.FMSG_Id,
                                        FMG_Id = b.FMG_Id

                                    }
       ).Distinct().Take(10).OrderByDescending(t => t.FMSG_Id).ToArray();



                //         fee.alldatathirdall = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                             from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                             where (a.AMST_Id==d.AMST_Id && d.AMST_Id==e.AMST_Id && a.MI_Id==fee.MI_Id && d.ASMAY_Id==a.ASMAY_Id && a.ASMAY_Id==fee.ASMAY_Id)
                //                             select new FeeStudentGroupMappingDTO
                //                             {
                //                                 AMST_Id = a.AMST_Id,
                //                                 AMST_FirstName = e.AMST_FirstName,
                //                                 AMST_MiddleName = e.AMST_MiddleName,
                //                                 AMST_LastName = e.AMST_LastName,
                //                                 AMST_AdmNo = e.AMST_AdmNo,
                //                                 AMST_RegistrationNo = e.AMST_RegistrationNo,
                //                                 AMAY_RollNo = d.AMAY_RollNo,
                //                                 AMST_Mobile = e.AMST_MobileNo,
                //                                 FMSG_Id = a.FMSG_Id,
                //                             }
                //).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                fee.configsetting = _YearlyFeeGroupMappingContext.feemastersettings.Where(s => s.MI_Id == fee.MI_Id && s.userid == fee.user_id).ToList().Distinct().ToArray();

                //fee.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                //                 from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                //                 from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                //                 where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == fee.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                //                 select new Temp_Staff_DTO
                //                 {
                //                     HRME_Id = a.HRME_Id,
                //                     HRME_EmployeeCode = a.HRME_EmployeeCode,
                //                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                     HRMD_Id = a.HRMD_Id,
                //                     HRMDES_Id = c.HRMDES_Id,
                //                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                //                     HRMDES_DesignationName = c.HRMDES_DesignationName
                //                 }).ToList().Distinct().OrderBy(t => t.HRME_Id).Take(5).ToArray();

                //fee.saved_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                //                       from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                //                       where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id)
                //                       select a.HRME_Id).ToList().Distinct().ToArray();
                //fee.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                //                      from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                //                      from d in _YearlyFeeGroupMappingContext.feeGroup
                //                      from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                //                      from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                //                      where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == fee.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == fee.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id)
                //                      select new FeeStudentGroupMappingDTO
                //                      {
                //                          FMSTGH_Id = b.FMSTGH_Id,
                //                          HRME_Id = a.HRME_Id,
                //                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                //                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                          HRMD_Id = e.HRMD_Id,
                //                          HRMDES_Id = f.HRMDES_Id,
                //                          FMG_Id = b.FMG_Id,
                //                          HRMD_DepartmentName = e.HRMD_DepartmentName,
                //                          HRMDES_DesignationName = f.HRMDES_DesignationName,
                //                          FMG_GroupName = d.FMG_GroupName
                //                      }).ToList().Distinct().ToArray();
                //fee.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == fee.MI_Id && s.FMOST_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.FMOST_Id).Take(5).ToArray();

                //fee.saved_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                //                             from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                //                             where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id)
                //                             select a.FMOST_Id).ToList().Distinct().ToArray();
                //fee.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                //                            from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                //                            from c in _YearlyFeeGroupMappingContext.feeGroup
                //                            where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id)
                //                            select new FeeStudentGroupMappingDTO
                //                            {
                //                                FMOSTGH_Id = b.FMOSTGH_Id,
                //                                FMOST_Id = a.FMOST_Id,
                //                                FMOST_StudentName = a.FMOST_StudentName,
                //                                FMOST_StudentMobileNo = a.FMOST_StudentMobileNo,
                //                                FMOST_StudentEmailId = a.FMOST_StudentEmailId,
                //                                FMG_Id = b.FMG_Id,
                //                                FMG_GroupName = c.FMG_GroupName
                //                            }).ToList().Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;

        }

        public FeeStudentGroupMappingDTO getstucls(FeeStudentGroupMappingDTO data)
        {
            FeeStudentGroupMappingDTO fee = new FeeStudentGroupMappingDTO();
            try
            {
                if (data.radioval == "Regular" && data.classwisecheckboxvalue == true)
                {
                    fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                   from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   from c in _YearlyFeeGroupMappingContext.School_M_Class
                                   from d in _YearlyFeeGroupMappingContext.AdmSection
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMS_Id == d.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                   select new FeeStudentGroupMappingDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = a.AMST_FirstName,
                                       AMST_MiddleName = a.AMST_MiddleName,
                                       AMST_LastName = a.AMST_LastName,
                                       AMST_AdmNo = a.AMST_AdmNo,
                                       AMST_RegistrationNo = a.AMST_RegistrationNo,
                                       AMAY_RollNo = b.AMAY_RollNo,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMC_SectionName = d.ASMC_SectionName

                                   }
   ).OrderBy(t => t.AMST_FirstName).Take(5).ToArray();
                    if (fee.alldata.Length > 0)
                    {
                        fee.returnval = "Yes";
                    }
                    else
                    {
                        fee.returnval = "No";
                    }
                }
                else if (data.radioval == "NewStude" && data.classwisecheckboxvalue == true)
                {

                    if (data.ASMS_Id != null || data.ASMS_Id > 0)
                    {
                        fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                       from c in _YearlyFeeGroupMappingContext.AdmSection
                                       from d in _YearlyFeeGroupMappingContext.School_M_Class
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMS_Id == c.ASMS_Id &&  b.ASMCL_Id==d.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.ASMS_Id==data.ASMS_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_RegistrationNo = a.AMST_RegistrationNo,
                                           AMAY_RollNo = b.AMAY_RollNo,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                           ASMC_SectionName = c.ASMC_SectionName


                                       }
 ).OrderBy(t => t.AMST_FirstName).ToArray();
                    }
                    else
                    {
                        fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                       from c in _YearlyFeeGroupMappingContext.AdmSection
                                       from d in _YearlyFeeGroupMappingContext.School_M_Class
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMS_Id == c.ASMS_Id && b.ASMCL_Id == d.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_RegistrationNo = a.AMST_RegistrationNo,
                                           AMAY_RollNo = b.AMAY_RollNo,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                           ASMC_SectionName = c.ASMC_SectionName
                                       }
       ).OrderBy(t => t.AMST_FirstName).ToArray();
                    }
                    if (fee.alldata.Length > 0)
                    {
                        fee.returnval = "Yes";
                    }
                    else
                    {
                        fee.returnval = "No";
                    }
                }
                else if (data.radioval == "alldata" && data.classwisecheckboxvalue == true)
                {
                    if (data.ASMS_Id != 0)
                    {
                        fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                       from c in _YearlyFeeGroupMappingContext.AdmSection
                                       from d in _YearlyFeeGroupMappingContext.School_M_Class
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMS_Id == c.ASMS_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_RegistrationNo = a.AMST_RegistrationNo,
                                           AMAY_RollNo = b.AMAY_RollNo,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                           ASMC_SectionName = c.ASMC_SectionName
                                       }
      ).OrderBy(t => t.AMST_FirstName).ToArray();
                    }
                    else
                    {
                        fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                         from c in _YearlyFeeGroupMappingContext.AdmSection
                                       from d in _YearlyFeeGroupMappingContext.School_M_Class
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMS_Id == c.ASMS_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_RegistrationNo = a.AMST_RegistrationNo,
                                           AMAY_RollNo = b.AMAY_RollNo,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                           ASMC_SectionName = c.ASMC_SectionName
                                       }
 ).OrderBy(t => t.AMST_FirstName).ToArray();
                    }

                    if (fee.alldata.Length > 0)
                    {
                        fee.returnval = "Yes";
                    }
                    else
                    {
                        fee.returnval = "No";
                    }
                }
                fee.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.School_M_Class
                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from f in _YearlyFeeGroupMappingContext.AdmSection
                                    where (a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && d.AMST_Id == e.AMST_Id && c.ASMCL_Id == data.ASMCL_Id && e.AMST_SOL == "S" && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = e.AMST_FirstName,
                                        AMST_MiddleName = e.AMST_MiddleName,
                                        AMST_LastName = e.AMST_LastName,
                                        AMST_AdmNo = e.AMST_AdmNo,
                                        AMST_RegistrationNo = e.AMST_RegistrationNo,
                                        AMAY_RollNo = d.AMAY_RollNo,
                                        FMG_GroupName = b.FMG_GroupName,
                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                        ASMC_SectionName = f.ASMC_SectionName
                                    }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;
        }

        public FeeStudentGroupMappingDTO getsearchdata(int id, FeeStudentGroupMappingDTO org)
        {
            throw new NotImplementedException();
        }

        public FeeStudentGroupMappingDTO savedetails(FeeStudentGroupMappingDTO pgmod)
        {
            FeeStudentGroupMappingDTO feestumap = new FeeStudentGroupMappingDTO();
            try
            {
                var fee_OBConfiguration = (from a in _YearlyFeeGroupMappingContext.FeeMasterConfiguration
                                           where (a.MI_Id == pgmod.MI_Id && a.ASMAY_ID == pgmod.ASMAY_Id)
                                           select a.FMC_OBAutoAdjustFlg).FirstOrDefault();

                string returntxt = "";
                var fmhid = 0;
                FeeStudentGroupMappingDMO pgmodule = Mapper.Map<FeeStudentGroupMappingDMO>(pgmod);
                if (pgmod.studentdata != null)
                {
                    int j = 0, G = 0, H = 0, I = 0;

                    while (j < pgmod.studentdata.Count())
                    {
                        if (pgmod.studentdata[j].studchecked == true)
                        {
                            FeeStudentGroupMappingDMO pmm = new FeeStudentGroupMappingDMO();
                            FeeStudentGroupInstallmentMappingDMO fsgim = new FeeStudentGroupInstallmentMappingDMO();
                            pmm.AMST_Id = pgmod.studentdata[j].AMST_Id;
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                pmm.MI_Id = pgmod.MI_Id;
                                pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                pmm.FMSG_ActiveFlag = "Y";
                                pmm.FMSG_Id = 0;
                                var FMCC_Idnew = (from a in _YearlyFeeGroupMappingContext.feeYCC
                                                  from b in _YearlyFeeGroupMappingContext.feeYCCC
                                                  from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                  from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                  where (a.FYCC_Id == b.FYCC_Id && a.FMCC_Id == d.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && d.FMG_Id == pgmod.saveheadlst[H].FMG_Id && a.ASMAY_Id == pmm.ASMAY_Id && a.MI_Id == pmm.MI_Id && b.ASMCL_Id == c.ASMCL_Id && c.AMST_Id == pmm.AMST_Id && c.ASMAY_Id == pmm.ASMAY_Id)
                                                  select a.FMCC_Id).FirstOrDefault();
                                if (FMCC_Idnew == 0)
                                {
                                    returntxt = "a";
                                }
                                else
                                {
                                    while (G < pgmod.savegrplst.Count())
                                    {
                                        while (H < pgmod.saveheadlst.Count())
                                        {
                                            while (I < pgmod.saveftilst.Count())
                                            {

                                                if (pgmod.saveftilst[I].FMH_Id == pgmod.saveheadlst[H].FMH_Id && pgmod.saveftilst[I].FMG_Id == pgmod.savegrplst[G].FMG_Id && pgmod.savegrplst[G].FMG_Id == pgmod.saveheadlst[H].FMG_Id)
                                                {
                                                    if (pgmod.savegrplst[G].checkedgrplst == true && pgmod.saveheadlst[H].checkedheadlst == true && pgmod.saveftilst[I].checkedinstallmentlst == true)
                                                    {
                                                        var checkforduplicates1 = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                                                                   from b in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                                                                                   from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                                                   where (a.FMSG_Id == b.FMSG_Id && a.FMG_Id == c.FMG_Id && a.AMST_Id == c.AMST_Id && b.FMH_ID == c.FMH_Id && b.FTI_ID == c.FTI_Id && c.AMST_Id == pmm.AMST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.ASMAY_Id == pgmod.ASMAY_Id)
                                                                                   select b.FMSGI_Id).Distinct().ToList();


                                                        if (checkforduplicates1.Count() > 0)
                                                        {
                                                            pgmod.returnval = "true";

                                                        }

                                                        var asmayold = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Id == pgmod.ASMAY_Id && t.MI_Id == pgmod.MI_Id).ToList();
                                                        if (asmayold.Count > 0)
                                                        {
                                                            var order = asmayold[0].ASMAY_Order - 1;
                                                            var asmayoldid = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Order == order && t.MI_Id == pgmod.MI_Id).ToList();
                                                            if (asmayoldid.Count > 0)
                                                            {
                                                                var pendingfees = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(t => t.ASMAY_Id == asmayoldid[0].ASMAY_Id && t.AMST_Id == pmm.AMST_Id && t.FSS_ToBePaid > 0).ToList();


                                                                if (pendingfees.Count > 0)
                                                                {

                                                                    var feeopeningbal = _YearlyFeeGroupMappingContext.FeeMasterConfiguration.Where(a => a.MI_Id == pgmod.MI_Id && a.ASMAY_ID == pgmod.ASMAY_Id && a.FMC_Areawise_FeeFlg == 1).ToList();

                                                                    if (feeopeningbal.Count > 0)
                                                                    {
                                                                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                                                        {

                                                                            cmd.CommandText = "Fee_ObandExcessTest";
                                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                                            cmd.Parameters.Add(new SqlParameter("@mi_id",
                                                                                SqlDbType.BigInt)
                                                                            {
                                                                                Value = pgmod.MI_Id
                                                                            });
                                                                            cmd.Parameters.Add(new SqlParameter("@Lasmay_id",
                                                                               SqlDbType.BigInt)
                                                                            {
                                                                                Value = asmayoldid[0].ASMAY_Id
                                                                            });
                                                                            cmd.Parameters.Add(new SqlParameter("@Nasmay_id",
                                                                           SqlDbType.BigInt)
                                                                            {
                                                                                Value = pgmod.ASMAY_Id
                                                                            });

                                                                            cmd.Parameters.Add(new SqlParameter("@amst_id1",
                                                                        SqlDbType.BigInt)
                                                                            {
                                                                                Value = pmm.AMST_Id
                                                                            });

                                                                            cmd.Parameters.Add(new SqlParameter("@fmhids",
                                                                        SqlDbType.BigInt)
                                                                            {
                                                                                Value = pgmod.saveftilst[I].FMH_Id
                                                                            });
                                                                            cmd.Parameters.Add(new SqlParameter("@fttiids",
                                                                      SqlDbType.BigInt)
                                                                            {
                                                                                Value = pgmod.saveftilst[I].FTI_Id
                                                                            });

                                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                                cmd.Connection.Open();
                                                                            var data = cmd.ExecuteNonQuery();
                                                                            //cmd.Transaction.Commit();
                                                                            if (data >= 1)
                                                                            {
                                                                                pgmod.returnval = "true";
                                                                            }
                                                                            else
                                                                            {
                                                                                pgmod.returnval = "false";
                                                                            }

                                                                        }
                                                                    }


                                                                }
                                                            }
                                                        }

                                                        fmhid = Convert.ToInt32(pgmod.saveftilst[I].FMH_Id);
                                                        if (checkforduplicates1.Count().Equals(0))
                                                        {
                                                            var FMAlist = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                                           from b in _YearlyFeeGroupMappingContext.feeMIY
                                                                           from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                                           where (a.FTI_Id == b.FTI_Id && a.FMCC_Id == FMCC_Idnew && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMG_Id == pgmod.saveftilst[I].FMG_Id && a.FMH_Id == pgmod.saveftilst[I].FMH_Id && a.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.MI_Id == a.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == a.FMH_Id && ((a.FMA_Amount >= 0 && c.FMH_Flag != "F" && c.FMH_Flag != "E") || (a.FMA_Amount >= 0 && (c.FMH_Flag == "F" || c.FMH_Flag == "E"))))/* && a.FMA_Amount > 0*/
                                                                           select a.FMA_Id).Distinct().ToList();

                                                            if (FMAlist.Distinct().Count().Equals(0))
                                                            {
                                                                returntxt = "a";
                                                            }
                                                            else
                                                            {
                                                                using (var cmd1 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                                                {
                                                                    cmd1.CommandText = "Insert_Fee_Student_Mapnew";
                                                                    cmd1.CommandType = CommandType.StoredProcedure;
                                                                    cmd1.Parameters.Add(new SqlParameter("@fmg_id",
                                                                        SqlDbType.BigInt)
                                                                    {
                                                                        Value = pgmod.saveftilst[I].FMG_Id
                                                                    });
                                                                    cmd1.Parameters.Add(new SqlParameter("@amst_id",
                                                                       SqlDbType.BigInt)
                                                                    {
                                                                        Value = pmm.AMST_Id
                                                                    });
                                                                    cmd1.Parameters.Add(new SqlParameter("@MI_ID",
                                                                   SqlDbType.BigInt)
                                                                    {
                                                                        Value = pmm.MI_Id
                                                                    });

                                                                    cmd1.Parameters.Add(new SqlParameter("@fti_id_new",
                                                                SqlDbType.BigInt)
                                                                    {
                                                                        Value = pgmod.saveftilst[I].FTI_Id
                                                                    });

                                                                    cmd1.Parameters.Add(new SqlParameter("@FMH_ID_new",
                                                                      SqlDbType.BigInt)
                                                                    {
                                                                        Value = pgmod.saveftilst[I].FMH_Id
                                                                    });

                                                                    cmd1.Parameters.Add(new SqlParameter("@userid",
                                                                  SqlDbType.BigInt)
                                                                    {
                                                                        Value = pgmod.user_id
                                                                    });

                                                                    cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                                              SqlDbType.BigInt)
                                                                    {
                                                                        Value = pgmod.ASMAY_Id
                                                                    });

                                                                    if (cmd1.Connection.State != ConnectionState.Open)
                                                                        cmd1.Connection.Open();
                                                                    var data1 = cmd1.ExecuteNonQuery();

                                                                    if (data1 >= 1)
                                                                    {
                                                                        pgmod.returnval = "true";
                                                                    }
                                                                    else
                                                                    {
                                                                        pgmod.returnval = "false";
                                                                    }
                                                                    //cmd1.Transaction.Commit();
                                                                }
                                                            }

                                                        }




                                                    }
                                                }
                                                I++;
                                            }
                                            I = 0;
                                            H++;


                                        }
                                        H = 0;
                                        G++;
                                    }
                                }
                            }
                        }

                        I = 0;
                        H = 0;
                        G = 0;
                        j++;
                    }
                }

                if (returntxt != "")
                {
                    pgmod.returnval = "false";
                }
            }
            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }
        public FeeStudentGroupMappingDTO savedetails_s(FeeStudentGroupMappingDTO pgmod)
        {
            // FeeStudentGroupMappingDTO feestumap = new FeeStudentGroupMappingDTO();
            try
            {

                if (pgmod.FMSTGH_Id > 0)
                {

                }
                else
                {
                    for (int i = 0; i < pgmod.staff_list.Length; i++)
                    {
                        for (int j = 0; j < pgmod.savegrplst.Length; j++)
                        {
                            Fee_Master_Staff_GroupHead obj1 = new Fee_Master_Staff_GroupHead();
                            obj1.FMSTGH_Id = pgmod.FMSTGH_Id;
                            obj1.MI_Id = pgmod.MI_Id;
                            obj1.ASMAY_Id = pgmod.ASMAY_Id;
                            obj1.HRME_Id = pgmod.staff_list[i].HRME_Id;
                            obj1.FMG_Id = pgmod.savegrplst[j].FMG_Id;
                            obj1.FMSTGH_ActiveFlag = true;
                            _YearlyFeeGroupMappingContext.Add(obj1);
                            for (int k = 0; k < pgmod.saveheadlst.Length; k++)
                            {
                                if (pgmod.savegrplst[j].FMG_Id == pgmod.saveheadlst[k].FMG_Id)
                                {
                                    for (int l = 0; l < pgmod.saveftilst.Length; l++)
                                    {
                                        //if(pgmod.saveheadlst[k].FMH_Id== pgmod.saveftilst[l].FMH_Id)
                                        if (pgmod.saveheadlst[k].FMH_Id == pgmod.saveftilst[l].FMH_Id && pgmod.savegrplst[j].FMG_Id == pgmod.saveftilst[l].FMG_Id)
                                        {
                                            Fee_Master_Staff_GroupHead_Installments obj2 = new Fee_Master_Staff_GroupHead_Installments();
                                            obj2.FMSTGH_Id = obj1.FMSTGH_Id;
                                            obj2.FMH_Id = pgmod.saveheadlst[k].FMH_Id;
                                            obj2.FTI_Id = pgmod.saveftilst[l].FTI_Id;
                                            _YearlyFeeGroupMappingContext.Add(obj2);


                                            var amount_list = _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.FMG_Id == pgmod.savegrplst[j].FMG_Id && t.FMH_Id == pgmod.saveheadlst[k].FMH_Id && t.FTI_Id == pgmod.saveftilst[l].FTI_Id && t.FMAOST_OthStaffFlag == "S" && t.FMAOST_ActiveFlag == true).Distinct().ToList();

                                            if (amount_list.Count > 0)
                                            {
                                                foreach (var x in amount_list)
                                                {
                                                    Fee_Student_Status_StaffDMO obj_status = new Fee_Student_Status_StaffDMO();
                                                    obj_status.MI_Id = pgmod.MI_Id;
                                                    obj_status.ASMAY_Id = pgmod.ASMAY_Id;
                                                    obj_status.HRME_Id = pgmod.staff_list[i].HRME_Id;
                                                    obj_status.FMG_Id = pgmod.savegrplst[j].FMG_Id;
                                                    obj_status.FMH_Id = pgmod.saveheadlst[k].FMH_Id;
                                                    obj_status.FTI_Id = pgmod.saveftilst[l].FTI_Id;
                                                    obj_status.FMA_Id = x.FMAOST_Id;
                                                    obj_status.FSSST_OBArrearAmount = 0;
                                                    obj_status.FSSST_OBExcessAmount = 0;
                                                    obj_status.FSSST_CurrentYrCharges = Convert.ToInt64(x.FMAOST_Amount);
                                                    //obj_status.FSSST_TotalCharges = 0;
                                                    obj_status.FSSST_ConcessionAmount = 0;
                                                    obj_status.FSSST_TotalCharges = obj_status.FSSST_CurrentYrCharges - obj_status.FSSST_ConcessionAmount;
                                                    obj_status.FSSST_ToBePaid = Convert.ToInt64(x.FMAOST_Amount);
                                                    obj_status.FSSST_WaivedAmount = 0;
                                                    obj_status.FSSST_PaidAmount = 0;
                                                    obj_status.FSSST_ExcessPaidAmount = 0;
                                                    obj_status.FSSST_ExcessAdjustedAmount = 0;
                                                    obj_status.FSSST_RunningExcessAmount = 0;
                                                    obj_status.FSSST_AdjustedAmount = 0;
                                                    obj_status.FSSST_RebateAmount = 0;
                                                    obj_status.FSSST_FineAmount = 0;
                                                    obj_status.FSSST_RefundAmount = 0;
                                                    obj_status.FSSST_RefundAmountAdjusted = 0;
                                                    obj_status.FSSST_NetAmount = obj_status.FSSST_CurrentYrCharges;
                                                    obj_status.FSSST_ChequeBounceAmount = 0;
                                                    obj_status.FSSST_ArrearFlag = false;
                                                    obj_status.FSSST_RefundOverFlag = false;
                                                    obj_status.FSSST_ActiveFlag = true;
                                                    obj_status.CreatedDate = DateTime.Now;
                                                    obj_status.UpdatedDate = DateTime.Now;
                                                    _YearlyFeeGroupMappingContext.Add(obj_status);
                                                }
                                            }
                                            //else if (amount_list.Count == 0)
                                            //{
                                            //    _YearlyFeeGroupMappingContext.Remove(obj2);
                                            //    _YearlyFeeGroupMappingContext.Remove(obj1);
                                            //}

                                        }
                                    }
                                }

                            }
                        }



                    }
                    var ResultCount = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (ResultCount >= 1)
                    {
                        pgmod.returnval = "Save";
                    }
                    else
                    {
                        pgmod.returnval = "Cancel";
                    }

                }
                //  Fee_Master_Staff_GroupHead pgmodule = Mapper.Map<Fee_Master_Staff_GroupHead>(pgmod);

                //if (feestumap.FMSG_Id > 0)
                //{
                //    if (pgmod.studentdata != null)
                //    {
                //        int j = 0, G = 0, H = 0, I = 0;

                //        while (j < pgmod.studentdata.Count())
                //        {
                //            if (pgmod.studentdata[j].studchecked == true)
                //            {
                //                FeeStudentGroupMappingDMO pmm = new FeeStudentGroupMappingDMO();
                //                FeeStudentGroupInstallmentMappingDMO fsgim = new FeeStudentGroupInstallmentMappingDMO();
                //                pmm.AMST_Id = pgmod.studentdata[j].AMST_Id;
                //                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                //                {
                //                    pmm.MI_Id = pgmod.MI_Id;
                //                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                //                    pmm.FMSG_ActiveFlag = "Y";
                //                    pmm.FMSG_Id = 0;
                //                    while (G < pgmod.savegrplst.Count())
                //                    {
                //                        if (pgmod.savegrplst[G].checkedgrplst == true)
                //                        {
                //                            pmm.FMG_Id = pgmod.savegrplst[G].FMG_Id;

                //                            // _YearlyFeeGroupMappingContext.Add(pmm);
                //                            //_YearlyFeeGroupMappingContext.SaveChanges();
                //                            var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Master_Student_Group @p0,@p1,@p2,@p3,@p4", pmm.MI_Id, pmm.AMST_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMSG_ActiveFlag);

                //                            while (H < pgmod.saveheadlst.Count())
                //                            {
                //                                if (pgmod.saveheadlst[H].checkedheadlst == true)
                //                                {
                //                                    fsgim.FMH_ID = pgmod.saveheadlst[H].FMH_Id;
                //                                    //_YearlyFeeGroupMappingContext.Add(fsgim);
                //                                    //_YearlyFeeGroupMappingContext.SaveChanges();

                //                                    while (I < pgmod.saveftilst.Count())
                //                                    {
                //                                        if (pgmod.saveftilst[I].checkedinstallmentlst == true)
                //                                        {
                //                                            fsgim.FTI_ID = pgmod.saveftilst[I].FTI_Id;

                //                                            fsgim.FMSG_Id = pmm.FMSG_Id;
                //                                            // _YearlyFeeGroupMappingContext.Add(fsgim);
                //                                            // _YearlyFeeGroupMappingContext.SaveChanges();

                //                                            //procedure
                //                                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                //                                            {
                //                                                cmd.CommandText = "Insert_Fee_Student_Map";
                //                                                cmd.CommandType = CommandType.StoredProcedure;
                //                                                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                //                                                    SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pmm.FMG_Id
                //                                                });
                //                                                cmd.Parameters.Add(new SqlParameter("@amst_id",
                //                                                   SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pmm.AMST_Id
                //                                                });
                //                                                cmd.Parameters.Add(new SqlParameter("@MI_ID",
                //                                               SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pmm.MI_Id
                //                                                });

                //                                                cmd.Parameters.Add(new SqlParameter("@fti_id_new",
                //                                            SqlDbType.BigInt)
                //                                                {
                //                                                    Value = fsgim.FTI_ID
                //                                                });

                //                                                cmd.Parameters.Add(new SqlParameter("@FMSG_Id",
                //                                                  SqlDbType.BigInt)
                //                                                {
                //                                                    Value = fsgim.FMSG_Id
                //                                                });

                //                                                cmd.Parameters.Add(new SqlParameter("@FMH_ID_new",
                //                                                  SqlDbType.BigInt)
                //                                                {
                //                                                    Value = fsgim.FMH_ID
                //                                                });

                //                                                cmd.Parameters.Add(new SqlParameter("@userid",
                //                                              SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pgmod.user_id
                //                                                });

                //                                                if (cmd.Connection.State != ConnectionState.Open)
                //                                                    cmd.Connection.Open();
                //                                                var data = cmd.ExecuteNonQuery();

                //                                                if (data >= 1)
                //                                                {
                //                                                    pgmod.returnval = "true";
                //                                                }
                //                                                else
                //                                                {
                //                                                    pgmod.returnval = "false";
                //                                                }
                //                                            }
                //                                            //procedure
                //                                            fsgim.FMSGI_Id = 0;
                //                                        }
                //                                        I++;
                //                                    }
                //                                }
                //                                H++;
                //                            }
                //                        }
                //                        G++;
                //                    }
                //                }
                //            }
                //            j++;
                //        }
                //    }
                //}
                //else
                //{
                //    if (pgmod.studentdata != null)
                //    {
                //        int j = 0, G = 0, H = 0, I = 0;

                //        while (j < pgmod.studentdata.Count())
                //        {
                //            if (pgmod.studentdata[j].studchecked == true)
                //            {
                //                FeeStudentGroupMappingDMO pmm = new FeeStudentGroupMappingDMO();
                //                FeeStudentGroupInstallmentMappingDMO fsgim = new FeeStudentGroupInstallmentMappingDMO();
                //                pmm.AMST_Id = pgmod.studentdata[j].AMST_Id;
                //                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                //                {
                //                    pmm.MI_Id = pgmod.MI_Id;
                //                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                //                    pmm.FMSG_ActiveFlag = "Y";
                //                    pmm.FMSG_Id = 0;
                //                    while (G < pgmod.savegrplst.Count())
                //                    {
                //                        while (H < pgmod.saveheadlst.Count())
                //                        {
                //                            while (I < pgmod.saveftilst.Count())
                //                            {

                //                                if (pgmod.saveftilst[I].FMH_Id == pgmod.saveheadlst[H].FMH_Id && pgmod.saveftilst[I].FMG_Id == pgmod.savegrplst[G].FMG_Id && pgmod.savegrplst[G].FMG_Id == pgmod.saveheadlst[H].FMG_Id)
                //                                {
                //                                    if (pgmod.savegrplst[G].checkedgrplst == true && pgmod.saveheadlst[H].checkedheadlst == true && pgmod.saveftilst[I].checkedinstallmentlst == true)
                //                                    {
                //                                        var checkforduplicates1 = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                                                                   from b in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                //                                                                   from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                                                                   where (a.FMSG_Id == b.FMSG_Id && a.FMG_Id == c.FMG_Id && a.AMST_Id == c.AMST_Id && b.FMH_ID == c.FMH_Id && b.FTI_ID == c.FTI_Id && c.AMST_Id == pmm.AMST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id)
                //                                                                   select b.FMSGI_Id).Distinct().ToList();
                //                                        if (checkforduplicates1.Count().Equals(0))
                //                                        {
                //                                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                //                                            {
                //                                                cmd.CommandText = "Insert_Fee_Student_Mapnew";
                //                                                cmd.CommandType = CommandType.StoredProcedure;
                //                                                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                //                                                    SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pgmod.saveftilst[I].FMG_Id
                //                                                });
                //                                                cmd.Parameters.Add(new SqlParameter("@amst_id",
                //                                                   SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pmm.AMST_Id
                //                                                });
                //                                                cmd.Parameters.Add(new SqlParameter("@MI_ID",
                //                                               SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pmm.MI_Id
                //                                                });

                //                                                cmd.Parameters.Add(new SqlParameter("@fti_id_new",
                //                                            SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pgmod.saveftilst[I].FTI_Id
                //                                                });

                //                                                cmd.Parameters.Add(new SqlParameter("@FMH_ID_new",
                //                                                  SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pgmod.saveftilst[I].FMH_Id
                //                                                });

                //                                                cmd.Parameters.Add(new SqlParameter("@userid",
                //                                              SqlDbType.BigInt)
                //                                                {
                //                                                    Value = pgmod.user_id
                //                                                });

                //                                                if (cmd.Connection.State != ConnectionState.Open)
                //                                                    cmd.Connection.Open();
                //                                                var data = cmd.ExecuteNonQuery();

                //                                                if (data >= 1)
                //                                                {
                //                                                    pgmod.returnval = "true";
                //                                                }
                //                                                else
                //                                                {
                //                                                    pgmod.returnval = "false";
                //                                                }
                //                                            }
                //                                        }
                //                                    }
                //                                }
                //                                I++;
                //                            }
                //                            I = 0;
                //                            H++;
                //                        }
                //                        H = 0;
                //                        G++;
                //                    }

                //                }
                //            }

                //            I = 0;
                //            H = 0;
                //            G = 0;

                //            j++;
                //        }
                //    }
                //}


            }

            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }

        public FeeStudentGroupMappingDTO savedetails_o(FeeStudentGroupMappingDTO pgmod)
        {
            // FeeStudentGroupMappingDTO feestumap = new FeeStudentGroupMappingDTO();
            try
            {

                if (pgmod.FMOSTGH_Id > 0)
                {

                }
                else
                {
                    for (int i = 0; i < pgmod.student_list.Length; i++)
                    {
                        for (int j = 0; j < pgmod.savegrplst.Length; j++)
                        {
                            Fee_Master_OthStudents_GHDMO obj1 = new Fee_Master_OthStudents_GHDMO();
                            obj1.FMOSTGH_Id = pgmod.FMOSTGH_Id;
                            obj1.MI_Id = pgmod.MI_Id;
                            obj1.ASMAY_Id = pgmod.ASMAY_Id;
                            obj1.FMOST_Id = pgmod.student_list[i].FMOST_Id;
                            obj1.FMG_Id = pgmod.savegrplst[j].FMG_Id;
                            obj1.FMOSTGH_ActiveFlag = true;
                            obj1.CreatedDate = DateTime.Now;
                            obj1.UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Add(obj1);
                            for (int k = 0; k < pgmod.saveheadlst.Length; k++)
                            {
                                if (pgmod.savegrplst[j].FMG_Id == pgmod.saveheadlst[k].FMG_Id)
                                {
                                    for (int l = 0; l < pgmod.saveftilst.Length; l++)
                                    {
                                        // if (pgmod.saveheadlst[k].FMH_Id == pgmod.saveftilst[l].FMH_Id)
                                        if (pgmod.saveheadlst[k].FMH_Id == pgmod.saveftilst[l].FMH_Id && pgmod.savegrplst[j].FMG_Id == pgmod.saveftilst[l].FMG_Id)
                                        {
                                            Fee_Master_OthStudents_GH_InstlDMO obj2 = new Fee_Master_OthStudents_GH_InstlDMO();
                                            obj2.FMOSTGH_Id = obj1.FMOSTGH_Id;
                                            obj2.FMH_Id = pgmod.saveheadlst[k].FMH_Id;
                                            obj2.FTI_Id = pgmod.saveftilst[l].FTI_Id;
                                            _YearlyFeeGroupMappingContext.Add(obj2);


                                            var amount_list = _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.FMG_Id == pgmod.savegrplst[j].FMG_Id && t.FMH_Id == pgmod.saveheadlst[k].FMH_Id && t.FTI_Id == pgmod.saveftilst[l].FTI_Id && t.FMAOST_OthStaffFlag == "O" && t.FMAOST_ActiveFlag == true).Distinct().ToList();

                                            if (amount_list.Count > 0)
                                            {
                                                foreach (var x in amount_list)
                                                {
                                                    Fee_Student_Status_OthStuDMO obj_status = new Fee_Student_Status_OthStuDMO();
                                                    obj_status.MI_Id = pgmod.MI_Id;
                                                    obj_status.ASMAY_Id = pgmod.ASMAY_Id;
                                                    obj_status.FMOST_Id = pgmod.student_list[i].FMOST_Id;
                                                    obj_status.FMG_Id = pgmod.savegrplst[j].FMG_Id;
                                                    obj_status.FMH_Id = pgmod.saveheadlst[k].FMH_Id;
                                                    obj_status.FTI_Id = pgmod.saveftilst[l].FTI_Id;
                                                    obj_status.FMA_Id = x.FMAOST_Id;
                                                    obj_status.FSSOST_OBArrearAmount = 0;
                                                    obj_status.FSSOST_OBExcessAmount = 0;
                                                    obj_status.FSSOST_CurrentYrCharges = Convert.ToInt64(x.FMAOST_Amount);
                                                    //obj_status.FSSOST_TotalCharges = 0;
                                                    obj_status.FSSOST_ConcessionAmount = 0;
                                                    obj_status.FSSOST_TotalCharges = obj_status.FSSOST_CurrentYrCharges - obj_status.FSSOST_ConcessionAmount;
                                                    obj_status.FSSOST_ToBePaid = Convert.ToInt64(x.FMAOST_Amount);
                                                    obj_status.FSSOST_WaivedAmount = 0;
                                                    obj_status.FSSOST_PaidAmount = 0;
                                                    obj_status.FSSOST_ExcessPaidAmount = 0;
                                                    obj_status.FSSOST_ExcessAdjustedAmount = 0;
                                                    obj_status.FSSOST_RunningExcessAmount = 0;
                                                    obj_status.FSSOST_AdjustedAmount = 0;
                                                    obj_status.FSSOST_RebateAmount = 0;
                                                    obj_status.FSSOST_FineAmount = 0;
                                                    obj_status.FSSOST_RefundAmount = 0;
                                                    obj_status.FSSOST_RefundAmountAdjusted = 0;
                                                    obj_status.FSSOST_NetAmount = obj_status.FSSOST_CurrentYrCharges;
                                                    obj_status.FSSOST_ChequeBounceAmount = 0;
                                                    obj_status.FSSOST_ArrearFlag = false;
                                                    obj_status.FSSOST_RefundOverFlag = false;
                                                    obj_status.FSSOST_ActiveFlag = true;
                                                    obj_status.CreatedDate = DateTime.Now;
                                                    obj_status.UpdatedDate = DateTime.Now;
                                                    _YearlyFeeGroupMappingContext.Add(obj_status);
                                                }
                                            }
                                            //else if (amount_list.Count == 0)
                                            //{
                                            //    _YearlyFeeGroupMappingContext.Remove(obj2);
                                            //    _YearlyFeeGroupMappingContext.Remove(obj1);
                                            //}

                                        }
                                    }
                                }

                            }
                        }



                    }
                    var ResultCount = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (ResultCount >= 1)
                    {
                        pgmod.returnval = "Save";
                    }
                    else
                    {
                        pgmod.returnval = "Cancel";
                    }

                }
            }

            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }
        public FeeStudentGroupMappingDTO saveeditdata(FeeStudentGroupMappingDTO pgmod)
        {

            try
            {
                string returntxt = "";
                if (pgmod.AMST_Id != 0)
                {
                    int G = 0, H = 0, I = 0;
                    var FMCC_Idnew = (from a in _YearlyFeeGroupMappingContext.feeYCC
                                      from b in _YearlyFeeGroupMappingContext.feeYCCC
                                      from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                      where (a.FYCC_Id == b.FYCC_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == pgmod.MI_Id && b.ASMCL_Id == c.ASMCL_Id && c.AMST_Id == pgmod.AMST_Id && c.ASMAY_Id == pgmod.ASMAY_Id)
                                      select a.FMCC_Id).FirstOrDefault();
                    if (FMCC_Idnew == 0)
                    {
                        returntxt = "a";
                    }
                    else
                    {
                        while (G < pgmod.savegrplst.Count())
                        {

                            while (H < pgmod.saveheadlst.Count())
                            {
                                while (I < pgmod.saveftilst.Count())
                                {

                                    if (pgmod.saveftilst[I].disableins == false)
                                    {
                                        if (pgmod.saveftilst[I].FMH_Id == pgmod.saveheadlst[H].FMH_Id && pgmod.saveftilst[I].FMG_Id == pgmod.savegrplst[G].FMG_Id && pgmod.savegrplst[G].FMG_Id == pgmod.saveheadlst[H].FMG_Id)
                                        {
                                            if (pgmod.savegrplst[G].checkedgrplstedit == true && pgmod.saveheadlst[H].checkedheadlstedit == true && pgmod.saveftilst[I].checkedinstallmentlstedit == true)
                                            {
                                                var checkforduplicates1 = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                                                           from b in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                                                                           from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                                           where (a.FMSG_Id == b.FMSG_Id && a.FMG_Id == c.FMG_Id && a.AMST_Id == c.AMST_Id && b.FMH_ID == c.FMH_Id && b.FTI_ID == c.FTI_Id && c.AMST_Id == pgmod.AMST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                           select b.FMSGI_Id).Distinct().ToList();
                                                if (checkforduplicates1.Count().Equals(0))
                                                {
                                                    var FMAlist = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                                   from b in _YearlyFeeGroupMappingContext.feeMIY
                                                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                                   where (a.FTI_Id == b.FTI_Id && a.FMCC_Id == FMCC_Idnew && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMG_Id == pgmod.saveftilst[I].FMG_Id && a.FMH_Id == pgmod.saveftilst[I].FMH_Id && a.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.MI_Id == a.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == a.FMH_Id && ((a.FMA_Amount >= 0 && c.FMH_Flag != "F" && c.FMH_Flag != "E") || (a.FMA_Amount >= 0 && (c.FMH_Flag == "F" || c.FMH_Flag == "E"))))/* && a.FMA_Amount > 0*/
                                                                   select a.FMA_Id).Distinct().ToList();

                                                    //var FMAlist = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                    //               from b in _YearlyFeeGroupMappingContext.feeMIY
                                                    //               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                    //               where (a.FTI_Id == b.FTI_Id && a.FMCC_Id == FMCC_Idnew && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMG_Id == pgmod.saveftilst[I].FMG_Id && a.FMH_Id == pgmod.saveftilst[I].FMH_Id && a.FTI_Id == pgmod.saveftilst[I].FTI_Id && a.FMA_Amount > 0 )
                                                    //               select a.FMA_Id).Distinct().ToList();

                                                    if (FMAlist.Distinct().Count().Equals(0))
                                                    {
                                                        returntxt = "a";
                                                    }
                                                    else
                                                    {
                                                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Insert_Fee_Student_Mapnew";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                                                SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FMG_Id
                                                            });
                                                            cmd.Parameters.Add(new SqlParameter("@amst_id",
                                                               SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.AMST_Id
                                                            });
                                                            cmd.Parameters.Add(new SqlParameter("@MI_ID",
                                                           SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.MI_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@fti_id_new",
                                                        SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FTI_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@FMH_ID_new",
                                                              SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FMH_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@userid",
                                                          SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.user_id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                                             SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.ASMAY_Id
                                                            });

                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();
                                                            var data = cmd.ExecuteNonQuery();

                                                            if (data >= 1)
                                                            {
                                                                pgmod.returnval = "true";
                                                            }
                                                            else
                                                            {
                                                                pgmod.returnval = "false";
                                                            }
                                                        }
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                var checkforduplicatesdel = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                                                             from b in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                                                                             from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                                             where (a.FMSG_Id == b.FMSG_Id && a.FMG_Id == c.FMG_Id && a.AMST_Id == c.AMST_Id && b.FMH_ID == c.FMH_Id && b.FTI_ID == c.FTI_Id && c.AMST_Id == pgmod.AMST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.FSS_PaidAmount == 0 && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                             select new { a.ASMAY_Id, a.MI_Id, a.FMG_Id, a.AMST_Id, a.FMSG_Id, b.FMH_ID, b.FTI_ID }).Distinct().ToList();
                                                if (checkforduplicatesdel.Count > 0)
                                                {

                                                    foreach (var a in checkforduplicatesdel)
                                                    {
                                                        using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                                                        {

                                                            var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMappingnew @p0,@p1,@p2,@p3,@p4,@p5,@p6", a.MI_Id, a.AMST_Id, a.ASMAY_Id, a.FMG_Id, a.FMSG_Id, a.FMH_ID, a.FTI_ID);

                                                            if (outputval >= 1)
                                                            {
                                                                transaction.Commit();
                                                                pgmod.returnval = "true";
                                                            }
                                                            else
                                                            {
                                                                pgmod.returnval = "false";
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    I++;
                                }
                                I = 0;
                                H++;
                            }
                            H = 0;
                            G++;
                        }
                    }


                }
                if (returntxt != "")
                {
                    pgmod.returnval = "false";
                }
                //pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                //                 from c in _YearlyFeeGroupMappingContext.School_M_Class
                //                 from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                 from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                 from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                 where (a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == pgmod.MI_Id && d.AMST_Id == e.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id)
                //                 select new FeeStudentGroupMappingDTO
                //                 {
                //                     AMST_Id = a.AMST_Id,
                //                     AMST_FirstName = e.AMST_FirstName,
                //                     AMST_MiddleName = e.AMST_MiddleName,
                //                     AMST_LastName = e.AMST_LastName,
                //                     AMST_AdmNo = e.AMST_AdmNo,
                //                     AMST_RegistrationNo = e.AMST_RegistrationNo,
                //                     AMAY_RollNo = d.AMAY_RollNo,
                //                     FMG_GroupName = b.FMG_GroupName,
                //                     ASMCL_ClassName = c.ASMCL_ClassName

                //                 }).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }

        public FeeStudentGroupMappingDTO getradiofiltereddata(FeeStudentGroupMappingDTO data)
        {
            if (data.radioval == "FCC")
            {
                List<FeeClassCategoryDMO> clscat = new List<FeeClassCategoryDMO>();
                clscat = _YearlyFeeGroupMappingContext.FeeClassCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).ToList();
                data.fillfeeclasscategory = clscat.ToArray();
            }
            else if (data.radioval == "AC")
            {
                List<MasterCategory> admclscat = new List<MasterCategory>();
                admclscat = _YearlyFeeGroupMappingContext.masterCategory.Where(t => t.MI_Id == data.MI_Id && t.AMC_ActiveFlag == 1).ToList();
                data.filladmissionclasscategory = admclscat.ToArray();
            }
            else if (data.radioval == "BR")
            {
                data.fillbusroutedet = (from a in _YearlyFeeGroupMappingContext.MasterRouteDMO
                                        from b in _YearlyFeeGroupMappingContext.TR_Student_RouteDMO
                                        where a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && (a.TRMR_Id == b.TRMR_Id || a.TRMR_Id == b.TRMR_Drop_Route) && a.TRMR_ActiveFlg == true && b.TRSR_ActiveFlg == true
                                        select a).Distinct().ToArray();

                var arealist = _YearlyFeeGroupMappingContext.MasterAreaDMO.Where(f => f.MI_Id == data.MI_Id).ToList();
                data.fillarearoute = arealist.ToArray();
            }
            else if (data.radioval == "Classwise")
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillmasterclass = classlist.ToArray();

                List<School_M_Section> sectionlist = new List<School_M_Section>();
                sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.fillmastersection = sectionlist.ToArray();
            }
            else if (data.radioval == "Areawise")
            {
                //List<MasterAreaDMO> arealist = new List<MasterAreaDMO>();
                //arealist = _YearlyFeeGroupMappingContext.masterAreaDMO.ToList();
                //data.fillarearoute = arealist.ToArray();
            }
            else if (data.radioval == "Regular")
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillmasterclass = classlist.ToArray();

                List<School_M_Section> sectionlist = new List<School_M_Section>();
                sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.fillmastersection = sectionlist.ToArray();
            }
            else if (data.radioval == "alldata")
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_ClassCode).ToList();
                data.fillmasterclass = classlist.ToArray();

                List<School_M_Section> sectionlist = new List<School_M_Section>();
                sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.fillmastersection = sectionlist.ToArray();
            }
            else if (data.radioval == "NewStude")
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.fillmasterclass = classlist.ToArray();

                List<School_M_Section> sectionlist = new List<School_M_Section>();
                sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.fillmastersection = sectionlist.ToArray();
            }

            else if (data.radioval == "Staff")
            {
                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                        from b in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                        from c in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                        where (a.FMG_Id == b.FMG_ID && a.MI_Id == data.MI_Id && b.User_Id == data.user_id && a.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMAOST_ActiveFlag == true && c.FMG_Id == a.FMG_Id && c.FMAOST_OthStaffFlag == "S")
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName
                                        }).Distinct().ToArray();


                data.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                       from b in _YearlyFeeGroupMappingContext.feeGroup
                                       from c in _YearlyFeeGroupMappingContext.feehead
                                       from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.FMAOST_ActiveFlag == true && d.FMG_Id == b.FMG_Id && d.FMAOST_OthStaffFlag == "S" && d.FMH_Id == c.FMH_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();



                data.fillinstallment = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                        from b in _YearlyFeeGroupMappingContext.feeGroup
                                        from c in _YearlyFeeGroupMappingContext.feehead
                                        from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                        from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                        from f in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.FMI_Id == d.FMI_Id && d.FMI_Id == e.FMI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.FMAOST_ActiveFlag == true && f.FMG_Id == b.FMG_Id && f.FMAOST_OthStaffFlag == "S" && f.FMH_Id == c.FMH_Id && f.FTI_Id == e.FTI_Id)
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }).Distinct().ToArray();

                data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                  from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                  from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                                  select new Temp_Staff_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                      HRMD_Id = a.HRMD_Id,
                                      HRMDES_Id = c.HRMDES_Id,
                                      HRMD_DepartmentName = b.HRMD_DepartmentName,
                                      HRMDES_DesignationName = c.HRMDES_DesignationName
                                  }).ToList().Distinct().OrderBy(t => t.HRME_Id).Take(5).ToArray();

                data.saved_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                        from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                        where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                        select a.HRME_Id).ToList().Distinct().ToArray();
                data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                       from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                       from d in _YearlyFeeGroupMappingContext.feeGroup
                                       from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                       from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                       where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == data.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMSTGH_Id = b.FMSTGH_Id,
                                           HRME_Id = a.HRME_Id,
                                           HRME_EmployeeCode = a.HRME_EmployeeCode,
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                           HRMD_Id = e.HRMD_Id,
                                           HRMDES_Id = f.HRMDES_Id,
                                           FMG_Id = b.FMG_Id,
                                           HRMD_DepartmentName = e.HRMD_DepartmentName,
                                           HRMDES_DesignationName = f.HRMDES_DesignationName,
                                           FMG_GroupName = d.FMG_GroupName
                                       }).ToList().Distinct().ToArray();



            }
            else if (data.radioval == "Others")
            {
                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                        from b in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                        from c in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                        where (a.FMG_Id == b.FMG_ID && a.MI_Id == data.MI_Id && b.User_Id == data.user_id && a.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMAOST_ActiveFlag == true && c.FMG_Id == a.FMG_Id && c.FMAOST_OthStaffFlag == "O")
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName
                                        }).Distinct().ToArray();


                data.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                       from b in _YearlyFeeGroupMappingContext.feeGroup
                                       from c in _YearlyFeeGroupMappingContext.feehead
                                       from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.FMAOST_ActiveFlag == true && d.FMG_Id == b.FMG_Id && d.FMAOST_OthStaffFlag == "O" && d.FMH_Id == c.FMH_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();


                data.fillinstallment = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                        from b in _YearlyFeeGroupMappingContext.feeGroup
                                        from c in _YearlyFeeGroupMappingContext.feehead
                                        from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                        from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                        from f in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.FMI_Id == d.FMI_Id && d.FMI_Id == e.FMI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.FMAOST_ActiveFlag == true && f.FMG_Id == b.FMG_Id && f.FMAOST_OthStaffFlag == "O" && f.FMH_Id == c.FMH_Id && f.FTI_Id == e.FTI_Id)
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }).Distinct().ToArray();

                data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.FMOST_Id).Take(5).ToArray();

                data.saved_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                              where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)


                                              select a.FMOST_Id).ToList().Distinct().ToArray();
                data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                             from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                             from c in _YearlyFeeGroupMappingContext.feeGroup
                                             where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id)
                                             select new FeeStudentGroupMappingDTO
                                             {
                                                 FMOSTGH_Id = b.FMOSTGH_Id,
                                                 FMOST_Id = a.FMOST_Id,
                                                 FMOST_StudentName = a.FMOST_StudentName,
                                                 FMOST_StudentMobileNo = a.FMOST_StudentMobileNo,
                                                 FMOST_StudentEmailId = a.FMOST_StudentEmailId,
                                                 FMG_Id = b.FMG_Id,
                                                 FMG_GroupName = c.FMG_GroupName
                                             }).ToList().Distinct().ToArray();

            }

            else if (data.radioval == "LeftStudent")
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillmasterclass = classlist.ToArray();

                List<School_M_Section> sectionlist = new List<School_M_Section>();
                sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.fillmastersection = sectionlist.ToArray();

                data.leftstudent = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from f in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from d in _YearlyFeeGroupMappingContext.School_M_Class
                                    from e in _YearlyFeeGroupMappingContext.school_M_Section
                                    where (b.FMG_Id == a.FMG_Id && f.AMST_Id == a.AMST_Id && c.AMST_Id == f.AMST_Id && d.ASMCL_Id == c.ASMCL_Id
                                    && e.ASMS_Id == c.ASMS_Id && c.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && f.AMST_SOL == "L" &&
                                    f.AMST_ActiveFlag == 0 && a.ASMAY_Id == data.ASMAY_Id)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = f.AMST_FirstName,
                                        AMST_MiddleName = f.AMST_MiddleName,
                                        AMST_LastName = f.AMST_LastName,
                                        AMST_AdmNo = f.AMST_AdmNo,
                                        AMST_RegistrationNo = f.AMST_RegistrationNo,
                                        AMAY_RollNo = c.AMAY_RollNo,
                                        FMG_GroupName = b.FMG_GroupName,
                                        ASMCL_ClassName = d.ASMCL_ClassName,
                                        ASMC_SectionName = e.ASMC_SectionName,
                                        AMST_Mobile = f.AMST_MobileNo,
                                        FMSG_Id = a.FMSG_Id,
                                        FMG_Id = b.FMG_Id
                                    }).Distinct().OrderBy(t => t.FMSG_Id).ToArray();


            }

            else if (data.radioval == "CCAT")
            {
                data.castecategorydet = _YearlyFeeGroupMappingContext.castecategoryDMO.Distinct().ToArray();
            }


            if (data.radioval == "alldata")
            {
                data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    // from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.School_M_Class
                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                from f in _YearlyFeeGroupMappingContext.AdmSection
                                    //from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                where (d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == d.AMST_Id && f.ASMS_Id == d.ASMS_Id && d.ASMCL_Id == c.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                select new FeeStudentGroupMappingDTO
                                {
                                    AMST_Id = a.AMST_Id,
                                    AMST_FirstName = a.AMST_FirstName,
                                    AMST_MiddleName = a.AMST_MiddleName,
                                    AMST_LastName = a.AMST_LastName,
                                    AMST_AdmNo = a.AMST_AdmNo,
                                    AMST_RegistrationNo = a.AMST_RegistrationNo,
                                    AMAY_RollNo = d.AMAY_RollNo,
                                    //FMG_GroupName = b.FMG_GroupName,
                                    ASMCL_ClassName = c.ASMCL_ClassName,
                                    ASMC_SectionName = f.ASMC_SectionName
                                }
        ).Distinct().ToArray();
            }

            else if (data.radioval == "LeftStudent")
            {
                data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    // from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.School_M_Class
                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    //from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                from f in _YearlyFeeGroupMappingContext.AdmSection
                                where (d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && f.ASMS_Id == d.ASMS_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && a.AMST_SOL == "L" && a.AMST_ActiveFlag == 0 && d.AMAY_ActiveFlag == 1)
                                select new FeeStudentGroupMappingDTO
                                {
                                    AMST_Id = a.AMST_Id,
                                    AMST_FirstName = a.AMST_FirstName,
                                    AMST_MiddleName = a.AMST_MiddleName,
                                    AMST_LastName = a.AMST_LastName,
                                    AMST_AdmNo = a.AMST_AdmNo,
                                    AMST_RegistrationNo = a.AMST_RegistrationNo,
                                    AMAY_RollNo = d.AMAY_RollNo,
                                    //FMG_GroupName = b.FMG_GroupName,
                                    ASMCL_ClassName = c.ASMCL_ClassName,
                                    ASMC_SectionName = f.ASMC_SectionName

                                }
        ).Distinct().ToArray();
            }

            else
            {
                data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    // from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.School_M_Class
                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    //from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                where (d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == d.AMST_Id && f.ASMS_Id == d.ASMS_Id && d.ASMCL_Id == c.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                select new FeeStudentGroupMappingDTO
                                {
                                    AMST_Id = a.AMST_Id,
                                    AMST_FirstName = a.AMST_FirstName,
                                    AMST_MiddleName = a.AMST_MiddleName,
                                    AMST_LastName = a.AMST_LastName,
                                    AMST_AdmNo = a.AMST_AdmNo,
                                    AMST_RegistrationNo = a.AMST_RegistrationNo,
                                    AMAY_RollNo = d.AMAY_RollNo,
                                    //FMG_GroupName = b.FMG_GroupName,
                                    ASMCL_ClassName = c.ASMCL_ClassName,
                                    ASMC_SectionName = f.ASMC_SectionName

                                }
        ).Distinct().Take(5).ToArray();
            }
            var fetchmaxfmsgid = _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).OrderByDescending(t => t.FMSG_Id).Take(5).Select(t => t.FMSG_Id).ToList();
            data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                 from c in _YearlyFeeGroupMappingContext.School_M_Class
                                 from g in _YearlyFeeGroupMappingContext.school_M_Section
                                 from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                 from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                 from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 where (d.ASMS_Id == g.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && fetchmaxfmsgid.Contains(a.FMSG_Id) && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                 select new FeeStudentGroupMappingDTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                     AMST_FirstName = e.AMST_FirstName,
                                     AMST_MiddleName = e.AMST_MiddleName,
                                     AMST_LastName = e.AMST_LastName,
                                     AMST_AdmNo = e.AMST_AdmNo,
                                     AMST_RegistrationNo = e.AMST_RegistrationNo,
                                     AMAY_RollNo = d.AMAY_RollNo,
                                     FMG_GroupName = b.FMG_GroupName,
                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                     ASMC_SectionName = g.ASMC_SectionName,
                                     AMST_Mobile = e.AMST_MobileNo,
                                     FMSG_Id = a.FMSG_Id,
                                     FMG_Id = b.FMG_Id
                                 }
       ).Distinct().Take(5).OrderBy(t => t.ASMCL_Id).ToArray();
            return data;
        }

        public FeeStudentGroupMappingDTO getdataaspercategory(FeeStudentGroupMappingDTO data)
        {
            try
            {
                if (data.radioval == "AC")
                {
                    //          data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.masterclasscategory
                    //                                       from b in _YearlyFeeGroupMappingContext.admissioncls
                    //                                       where (a.ASMCL_Id==b.ASMCL_Id && a.AMC_Id==data.AMC_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id)
                    //                                       select new FeeStudentGroupMappingDTO
                    //                                       {
                    //                                           ASMCL_Id = b.ASMCL_Id,
                    //                                           ASMCL_ClassName = b.ASMCL_ClassName
                    //                                       }
                    //).ToArray();

                    data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                 from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                 from c in _YearlyFeeGroupMappingContext.admissioncls
                                                 from d in _YearlyFeeGroupMappingContext.AcademicYear
                                                 from e in _YearlyFeeGroupMappingContext.masterclasscategory
                                                 from h in _YearlyFeeGroupMappingContext.AdmSection
                                                 where (a.AMST_Id == b.AMST_Id && d.ASMAY_Id == a.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && e.ASMCL_Id == a.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMS_Id == h.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && e.AMC_Id == data.AMC_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                                 select new FeeStudentGroupMappingDTO
                                                 {
                                                     AMST_Id = a.AMST_Id,
                                                     AMST_FirstName = b.AMST_FirstName,
                                                     AMST_MiddleName = b.AMST_MiddleName,
                                                     AMST_LastName = b.AMST_LastName,
                                                     AMST_AdmNo = b.AMST_AdmNo,
                                                     AMST_RegistrationNo = b.AMST_RegistrationNo,
                                                     AMAY_RollNo = a.AMAY_RollNo,
                                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                                     ASMC_SectionName = h.ASMC_SectionName
                                                 }
          ).Distinct().OrderBy(t => t.AMST_FirstName).Take(5).ToArray();


                }
                else if (data.radioval == "FCC")
                {
                    //             data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                    //                                   from b in _YearlyFeeGroupMappingContext.feeYCC
                    //                                   from c in _YearlyFeeGroupMappingContext.feeYCCC
                    //                                   from d in _YearlyFeeGroupMappingContext.admissioncls
                    //                                   where (a.FMCC_Id==b.FMCC_Id && b.FYCC_Id==c.FYCC_Id && b.FMCC_Id==data.FMCC_Id && d.ASMCL_Id==c.AMCL_Id) 
                    //                                   select new FeeStudentGroupMappingDTO
                    //                                   {
                    //                                      ASMCL_Id=d.ASMCL_Id,
                    //                                      ASMCL_ClassName=d.ASMCL_ClassName
                    //                                   }
                    //).ToArray();

                    data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                                                 from b in _YearlyFeeGroupMappingContext.feeYCC
                                                 from c in _YearlyFeeGroupMappingContext.feeYCCC
                                                 from d in _YearlyFeeGroupMappingContext.admissioncls
                                                 from e in _YearlyFeeGroupMappingContext.AcademicYear
                                                 from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                 from g in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                 from h in _YearlyFeeGroupMappingContext.AdmSection
                                                 where (a.FMCC_Id == data.FMCC_Id && f.ASMAY_Id == e.ASMAY_Id && f.AMST_Id == g.AMST_Id && d.ASMCL_Id == f.ASMCL_Id && c.ASMCL_Id == f.ASMCL_Id && c.FYCC_Id == b.FYCC_Id && b.FMCC_Id == a.FMCC_Id && f.ASMAY_Id == data.ASMAY_Id && g.MI_Id == data.MI_Id  &&   f.ASMS_Id == h.ASMS_Id && g.AMST_SOL == "S" && g.AMST_ActiveFlag == 1 && f.AMAY_ActiveFlag == 1)
                                                 select new FeeStudentGroupMappingDTO
                                                 {
                                                     AMST_Id = g.AMST_Id,
                                                     AMST_FirstName = g.AMST_FirstName,
                                                     AMST_MiddleName = g.AMST_MiddleName,
                                                     AMST_LastName = g.AMST_LastName,
                                                     AMST_AdmNo = g.AMST_AdmNo,
                                                     AMST_RegistrationNo = g.AMST_RegistrationNo,
                                                     AMAY_RollNo = f.AMAY_RollNo,
                                                     ASMCL_ClassName = d.ASMCL_ClassName,
                                                     ASMC_SectionName = h.ASMC_SectionName
                                                 }
      ).OrderBy(t => t.AMST_FirstName).Take(5).ToArray();
                }
                else if (data.radioval == "Regular")
                {
                    //             data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                    //                                          from b in _YearlyFeeGroupMappingContext.feeYCC
                    //                                          from c in _YearlyFeeGroupMappingContext.feeYCCC
                    //                                          from d in _YearlyFeeGroupMappingContext.admissioncls
                    //                                          where (a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && b.FMCC_Id == data.FMCC_Id && d.ASMCL_Id == c.AMCL_Id)
                    //                                          select new FeeStudentGroupMappingDTO
                    //                                          {
                    //                                              ASMCL_Id = d.ASMCL_Id,
                    //                                              ASMCL_ClassName = d.ASMCL_ClassName
                    //                                          }
                    //).ToArray();

                    data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                 from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                 from c in _YearlyFeeGroupMappingContext.AdmSection
                                                 from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                 where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMS_Id == c.ASMS_Id && b.ASMCL_Id == d.ASMCL_Id &&   a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                                 select new FeeStudentGroupMappingDTO
                                                 {
                                                     AMST_Id = a.AMST_Id,
                                                     AMST_FirstName = a.AMST_FirstName,
                                                     AMST_MiddleName = a.AMST_MiddleName,
                                                     AMST_LastName = a.AMST_LastName,
                                                     AMST_AdmNo = a.AMST_AdmNo,
                                                     AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                     AMAY_RollNo = b.AMAY_RollNo,
                                                     ASMCL_ClassName = d.ASMCL_ClassName,
                                                     ASMC_SectionName = c.ASMC_SectionName
                                                 }
     ).OrderBy(t => t.AMST_FirstName).Take(5).ToArray();

                }
                else if (data.radioval == "NewStude")
                {
                    
                    if (data.ASMCL_Id != null || data.ASMCL_Id > 0)
                    {
                        data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                     from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                     from c in _YearlyFeeGroupMappingContext.AdmSection
                                                     from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMS_Id == c.ASMS_Id && b.ASMCL_Id == d.ASMCL_Id &&   a.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id==data.ASMCL_Id)
                                                     select new FeeStudentGroupMappingDTO
                                                     {
                                                         AMST_Id = a.AMST_Id,
                                                         AMST_FirstName = a.AMST_FirstName,
                                                         AMST_MiddleName = a.AMST_MiddleName,
                                                         AMST_LastName = a.AMST_LastName,
                                                         AMST_AdmNo = a.AMST_AdmNo,
                                                         AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                         AMAY_RollNo = b.AMAY_RollNo,
                                                         ASMCL_ClassName = d.ASMCL_ClassName,
                                                         ASMC_SectionName = c.ASMC_SectionName

                                                     }
     ).OrderBy(t => t.AMST_FirstName).ToArray();
                    }
                    else if (data.ASMS_Id!=null || data.ASMS_Id > 0)
                    {
                        data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                     from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                     from c in _YearlyFeeGroupMappingContext.AdmSection
                                                     from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMS_Id == c.ASMS_Id && b.ASMCL_Id == d.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id==data.ASMS_Id)
                                                     select new FeeStudentGroupMappingDTO
                                                     {
                                                         AMST_Id = a.AMST_Id,
                                                         AMST_FirstName = a.AMST_FirstName,
                                                         AMST_MiddleName = a.AMST_MiddleName,
                                                         AMST_LastName = a.AMST_LastName,
                                                         AMST_AdmNo = a.AMST_AdmNo,
                                                         AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                         AMAY_RollNo = b.AMAY_RollNo,
                                                         ASMCL_ClassName = d.ASMCL_ClassName,
                                                         ASMC_SectionName = c.ASMC_SectionName
                                                     }
     ).OrderBy(t => t.AMST_FirstName).ToArray();
                    }
                   
                    else
                    {
                        data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                     from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                     from c in _YearlyFeeGroupMappingContext.AdmSection
                                                     from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMS_Id == c.ASMS_Id && b.ASMCL_Id == d.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                                     select new FeeStudentGroupMappingDTO
                                                     {
                                                         AMST_Id = a.AMST_Id,
                                                         AMST_FirstName = a.AMST_FirstName,
                                                         AMST_MiddleName = a.AMST_MiddleName,
                                                         AMST_LastName = a.AMST_LastName,
                                                         AMST_AdmNo = a.AMST_AdmNo,
                                                         AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                         AMAY_RollNo = b.AMAY_RollNo,
                                                         ASMCL_ClassName = d.ASMCL_ClassName,
                                                         ASMC_SectionName = c.ASMC_SectionName
                                                     }
     ).OrderBy(t => t.AMST_FirstName).ToArray();
                    }

                   

                }
                else if (data.radioval == "CCAT")
                {

                    data.fillfeeclasscategory = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                 from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                 from c in _YearlyFeeGroupMappingContext.castecategoryDMO
                                                 from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                 from e in _YearlyFeeGroupMappingContext.AdmSection
                                                 where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMS_Id == d.ASMCL_Id && e.ASMS_Id == d.ASMCL_Id && c.IMCC_Id == a.IMCC_Id && a.IMCC_Id == data.IMCC_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                                 select new FeeStudentGroupMappingDTO
                                                 {
                                                     AMST_Id = a.AMST_Id,
                                                     AMST_FirstName = a.AMST_FirstName,
                                                     AMST_MiddleName = a.AMST_MiddleName,
                                                     AMST_LastName = a.AMST_LastName,
                                                     AMST_AdmNo = a.AMST_AdmNo,
                                                     AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                     AMAY_RollNo = b.AMAY_RollNo,
                                                     ASMCL_ClassName = d.ASMCL_ClassName,
                                                     ASMC_SectionName = e.ASMC_SectionName
                                                 }
      ).OrderBy(t => t.AMST_FirstName).ToArray();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO studentsavedgroupfacfun(FeeStudentGroupMappingDTO data)
        {
            try
            {

                data.fillmappedgroupforstudents = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                                   from b in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.FMG_Id == b.FMG_Id && a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                                   select new FeeStudentGroupMappingDTO
                                                   {
                                                       FMG_Id = a.FMG_Id,
                                                       FMH_Id = b.FMH_Id,
                                                       FTI_Id = b.FTI_Id
                                                   }
      ).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO searching(FeeStudentGroupMappingDTO data)
        {
            try
            {
                switch (data.searchType)
                {
                    case "0":

                        if (data.radioval != "BR")
                        {
                            List<FeeStudentGroupMappingDTO> result = new List<FeeStudentGroupMappingDTO>();
                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "FEE_STUDENTGROUP_NAME_SEARCH_BEFORE_new";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                                  SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@searchtext",
                                             SqlDbType.VarChar)
                                {
                                    Value = data.searchtext
                                });


                                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@type",
                             SqlDbType.VarChar)
                                {
                                    Value = 0
                                });

                                cmd.Parameters.Add(new SqlParameter("@trmr_id",
                           SqlDbType.VarChar)
                                {
                                    Value = 0
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            result.Add(new FeeStudentGroupMappingDTO
                                            {

                                                AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                                AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                                AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                                                AMST_LastName = dataReader["AMST_LastName"].ToString(),
                                                AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                                                AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString(),
                                                AMAY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"].ToString()),
                                                ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                                ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),
                                                AMST_Mobile = Convert.ToInt64(dataReader["AMST_MobileNo"].ToString()),
                                                FMG_GroupName = dataReader["FMG_GroupName"].ToString(),

                                            });
                                        }
                                    }
                                    data.alldatathird = result.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                        else
                        {
                            List<FeeStudentGroupMappingDTO> result = new List<FeeStudentGroupMappingDTO>();
                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "FEE_STUDENTGROUP_NAME_SEARCH_BEFORE_new";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                                  SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@searchtext",
                                             SqlDbType.VarChar)
                                {
                                    Value = data.searchtext
                                });


                                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@type",
                             SqlDbType.VarChar)
                                {
                                    Value = 2
                                });

                                cmd.Parameters.Add(new SqlParameter("@trmr_id",
                           SqlDbType.VarChar)
                                {
                                    Value = data.TRMR_Id
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            result.Add(new FeeStudentGroupMappingDTO
                                            {

                                                AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                                AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                                AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                                                AMST_LastName = dataReader["AMST_LastName"].ToString(),
                                                AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                                                AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString(),
                                                AMAY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"].ToString()),
                                                ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                                ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),
                                                AMST_Mobile = Convert.ToInt64(dataReader["AMST_MobileNo"].ToString()),

                                            });
                                        }
                                    }
                                    data.alldatathird = result.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }



                        break;
                    case "1":
                        data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from c in _YearlyFeeGroupMappingContext.School_M_Class
                                             from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                             from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                             from g in _YearlyFeeGroupMappingContext.school_M_Section
                                             where (g.ASMS_Id == d.ASMS_Id && f.ASMAY_Id == d.ASMAY_Id && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && c.ASMCL_ClassName.Contains(data.searchtext))
                                             select new FeeStudentGroupMappingDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 AMST_FirstName = e.AMST_FirstName,
                                                 AMST_MiddleName = e.AMST_MiddleName,
                                                 AMST_LastName = e.AMST_LastName,
                                                 AMST_AdmNo = e.AMST_AdmNo,
                                                 AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                 AMAY_RollNo = d.AMAY_RollNo,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 ASMCL_ClassName = c.ASMCL_ClassName,
                                                 ASMC_SectionName = g.ASMC_SectionName,
                                                 AMST_Mobile = e.AMST_MobileNo,
                                                 FMSG_Id = a.FMSG_Id,
                                                 FMG_Id = b.FMG_Id
                                             }
      ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        break;
                    case "2":
                        data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from c in _YearlyFeeGroupMappingContext.School_M_Class
                                             from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                             from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                             from g in _YearlyFeeGroupMappingContext.school_M_Section
                                             where (g.ASMS_Id == d.ASMS_Id && f.ASMAY_Id == d.ASMAY_Id && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && e.AMST_AdmNo.Contains(data.searchtext))
                                             select new FeeStudentGroupMappingDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 AMST_FirstName = e.AMST_FirstName,
                                                 AMST_MiddleName = e.AMST_MiddleName,
                                                 AMST_LastName = e.AMST_LastName,
                                                 AMST_AdmNo = e.AMST_AdmNo,
                                                 AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                 AMAY_RollNo = d.AMAY_RollNo,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 ASMCL_ClassName = c.ASMCL_ClassName,
                                                 ASMC_SectionName = g.ASMC_SectionName,
                                                 AMST_Mobile = e.AMST_MobileNo,
                                                 FMSG_Id = a.FMSG_Id,
                                                 FMG_Id = b.FMG_Id
                                             }
     ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        break;
                    case "3":
                        data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from c in _YearlyFeeGroupMappingContext.School_M_Class
                                             from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                             from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                             from g in _YearlyFeeGroupMappingContext.school_M_Section
                                             where (g.ASMS_Id == d.ASMS_Id && f.ASMAY_Id == d.ASMAY_Id && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && b.FMG_GroupName.Contains(data.searchtext))
                                             select new FeeStudentGroupMappingDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 AMST_FirstName = e.AMST_FirstName,
                                                 AMST_MiddleName = e.AMST_MiddleName,
                                                 AMST_LastName = e.AMST_LastName,
                                                 AMST_AdmNo = e.AMST_AdmNo,
                                                 AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                 AMAY_RollNo = d.AMAY_RollNo,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 ASMCL_ClassName = c.ASMCL_ClassName,
                                                 ASMC_SectionName = g.ASMC_SectionName,
                                                 AMST_Mobile = e.AMST_MobileNo,
                                                 FMSG_Id = a.FMSG_Id,
                                                 FMG_Id = b.FMG_Id
                                             }
      ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO searching_s(FeeStudentGroupMappingDTO data)
        {
            try
            {
                switch (data.searchType)
                {
                    case "0":
                        data.searchtext = data.searchtext.ToUpper();
                        data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                               from d in _YearlyFeeGroupMappingContext.feeGroup
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && ((a.HRME_EmployeeFirstName.ToUpper().Trim() + ' ' + a.HRME_EmployeeMiddleName.ToUpper().Trim() + ' ' + a.HRME_EmployeeLastName.ToUpper().Trim()).Contains(data.searchtext) || a.HRME_EmployeeFirstName.ToUpper().StartsWith(data.searchtext) || a.HRME_EmployeeMiddleName.ToUpper().StartsWith(data.searchtext) || a.HRME_EmployeeLastName.ToUpper().StartsWith(data.searchtext)))
                                               select new FeeStudentGroupMappingDTO
                                               {
                                                   FMSTGH_Id = b.FMSTGH_Id,
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                   HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                                   HRMD_Id = e.HRMD_Id,
                                                   HRMDES_Id = f.HRMDES_Id,
                                                   FMG_Id = b.FMG_Id,
                                                   HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                   HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                   FMG_GroupName = d.FMG_GroupName
                                               }).ToList().Distinct().ToArray();

                        break;
                    case "1":
                        data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                               from d in _YearlyFeeGroupMappingContext.feeGroup
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && a.HRME_EmployeeCode.Contains(data.searchtext))
                                               select new FeeStudentGroupMappingDTO
                                               {
                                                   FMSTGH_Id = b.FMSTGH_Id,
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                   HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                                   HRMD_Id = e.HRMD_Id,
                                                   HRMDES_Id = f.HRMDES_Id,
                                                   FMG_Id = b.FMG_Id,
                                                   HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                   HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                   FMG_GroupName = d.FMG_GroupName
                                               }).ToList().Distinct().ToArray();
                        break;
                    case "2":
                        data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                               from d in _YearlyFeeGroupMappingContext.feeGroup
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && e.HRMD_DepartmentName.Contains(data.searchtext))
                                               select new FeeStudentGroupMappingDTO
                                               {
                                                   FMSTGH_Id = b.FMSTGH_Id,
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                   HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                                   HRMD_Id = e.HRMD_Id,
                                                   HRMDES_Id = f.HRMDES_Id,
                                                   FMG_Id = b.FMG_Id,
                                                   HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                   HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                   FMG_GroupName = d.FMG_GroupName
                                               }).ToList().Distinct().ToArray();
                        break;
                    case "3":
                        data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                               from d in _YearlyFeeGroupMappingContext.feeGroup
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && f.HRMDES_DesignationName.Contains(data.searchtext))
                                               select new FeeStudentGroupMappingDTO
                                               {
                                                   FMSTGH_Id = b.FMSTGH_Id,
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                   HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                                   HRMD_Id = e.HRMD_Id,
                                                   HRMDES_Id = f.HRMDES_Id,
                                                   FMG_Id = b.FMG_Id,
                                                   HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                   HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                   FMG_GroupName = d.FMG_GroupName
                                               }).ToList().Distinct().ToArray();
                        break;
                    case "4":
                        data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                               from d in _YearlyFeeGroupMappingContext.feeGroup
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && d.FMG_GroupName.Contains(data.searchtext))
                                               select new FeeStudentGroupMappingDTO
                                               {
                                                   FMSTGH_Id = b.FMSTGH_Id,
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                   HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                                   HRMD_Id = e.HRMD_Id,
                                                   HRMDES_Id = f.HRMDES_Id,
                                                   FMG_Id = b.FMG_Id,
                                                   HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                   HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                   FMG_GroupName = d.FMG_GroupName
                                               }).ToList().Distinct().ToArray();
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO searching_o(FeeStudentGroupMappingDTO data)
        {
            try
            {
                switch (data.searchType)
                {
                    case "0":
                        data.searchtext = data.searchtext.ToUpper();
                        data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                                     from c in _YearlyFeeGroupMappingContext.feeGroup
                                                     where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id && a.FMOST_StudentName.ToUpper().Contains(data.searchtext))
                                                     select new FeeStudentGroupMappingDTO
                                                     {
                                                         FMOSTGH_Id = b.FMOSTGH_Id,
                                                         FMOST_Id = a.FMOST_Id,
                                                         FMOST_StudentName = a.FMOST_StudentName,
                                                         FMOST_StudentMobileNo = a.FMOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = a.FMOST_StudentEmailId,
                                                         FMG_Id = b.FMG_Id,
                                                         FMG_GroupName = c.FMG_GroupName
                                                     }).ToList().Distinct().ToArray();

                        break;
                    case "1":
                        data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                                     from c in _YearlyFeeGroupMappingContext.feeGroup
                                                     where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id && a.FMOST_StudentMobileNo.ToString().Contains(data.searchtext))
                                                     select new FeeStudentGroupMappingDTO
                                                     {
                                                         FMOSTGH_Id = b.FMOSTGH_Id,
                                                         FMOST_Id = a.FMOST_Id,
                                                         FMOST_StudentName = a.FMOST_StudentName,
                                                         FMOST_StudentMobileNo = a.FMOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = a.FMOST_StudentEmailId,
                                                         FMG_Id = b.FMG_Id,
                                                         FMG_GroupName = c.FMG_GroupName
                                                     }).ToList().Distinct().ToArray();

                        break;
                    case "2":
                        data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                                     from c in _YearlyFeeGroupMappingContext.feeGroup
                                                     where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id && a.FMOST_StudentEmailId.Contains(data.searchtext))
                                                     select new FeeStudentGroupMappingDTO
                                                     {
                                                         FMOSTGH_Id = b.FMOSTGH_Id,
                                                         FMOST_Id = a.FMOST_Id,
                                                         FMOST_StudentName = a.FMOST_StudentName,
                                                         FMOST_StudentMobileNo = a.FMOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = a.FMOST_StudentEmailId,
                                                         FMG_Id = b.FMG_Id,
                                                         FMG_GroupName = c.FMG_GroupName
                                                     }).ToList().Distinct().ToArray();

                        break;
                    case "3":
                        data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                                     from c in _YearlyFeeGroupMappingContext.feeGroup
                                                     where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id && c.FMG_GroupName.Contains(data.searchtext))
                                                     select new FeeStudentGroupMappingDTO
                                                     {
                                                         FMOSTGH_Id = b.FMOSTGH_Id,
                                                         FMOST_Id = a.FMOST_Id,
                                                         FMOST_StudentName = a.FMOST_StudentName,
                                                         FMOST_StudentMobileNo = a.FMOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = a.FMOST_StudentEmailId,
                                                         FMG_Id = b.FMG_Id,
                                                         FMG_GroupName = c.FMG_GroupName
                                                     }).ToList().Distinct().ToArray();
                        break;

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeStudentGroupMappingDTO editstudata(FeeStudentGroupMappingDTO data)
        {
            try
            {
               

               


                data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                     from c in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                                     from g in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                     from e in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                     from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                     where (f.AMST_Id == a.AMST_Id && f.ASMAY_Id == a.ASMAY_Id && f.MI_Id == a.MI_Id && a.FMG_Id == f.FMG_Id && c.FMH_ID == f.FMH_Id && c.FTI_ID == f.FTI_Id && a.FMSG_Id == c.FMSG_Id && a.FMG_Id == b.FMG_Id && c.FMH_ID == g.FMH_Id && c.FTI_ID == d.FTI_Id && a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == a.FMG_Id && e.FMH_Id == c.FMH_ID && e.FMI_Id == d.FMI_Id)
                                     select new FeeStudentGroupMappingDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         FMG_Id = a.FMG_Id,
                                         FMG_GroupName = b.FMG_GroupName,
                                         FMH_Id = g.FMH_Id,
                                         FMH_FeeName = g.FMH_FeeName,
                                         FMSG_Id = a.FMSG_Id,
                                         FTI_Id = d.FTI_Id,
                                         FTI_Name = d.FTI_Name,
                                         FSS_PaidAmount = f.FSS_PaidAmount,
                                     }
                ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO searchingstu(FeeStudentGroupMappingDTO data)
        {
            try
            {
                string str = "";
                switch (data.searchType)
                {
                    case "0":



                        if (data.radioval != "BR")
                        {

                            data.searchtext = data.searchtext.ToUpper();

                            List<FeeStudentGroupMappingDTO> result = new List<FeeStudentGroupMappingDTO>();
                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "FEE_STUDENTGROUP_NAME_SEARCH_BEFORE_new";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                                  SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@searchtext",
                                             SqlDbType.VarChar)
                                {
                                    Value = data.searchtext
                                });


                                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });


                                cmd.Parameters.Add(new SqlParameter("@type",
                                  SqlDbType.VarChar)
                                {
                                    Value = 1
                                });
                                cmd.Parameters.Add(new SqlParameter("@trmr_id",
                            SqlDbType.VarChar)
                                {
                                    Value = 0
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            result.Add(new FeeStudentGroupMappingDTO
                                            {

                                                AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                                AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                                AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                                                AMST_LastName = dataReader["AMST_LastName"].ToString(),
                                                AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                                                AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString(),
                                                AMAY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"].ToString()),
                                                ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                                ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),
                                                AMST_Mobile = Convert.ToInt64(dataReader["AMST_MobileNo"].ToString()),
                                            });
                                        }
                                    }
                                    data.alldata = result.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                        else
                        {
                            List<FeeStudentGroupMappingDTO> result = new List<FeeStudentGroupMappingDTO>();
                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "FEE_STUDENTGROUP_NAME_SEARCH_BEFORE_new";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                                  SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@searchtext",
                                             SqlDbType.VarChar)
                                {
                                    Value = data.searchtext
                                });


                                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@type",
                             SqlDbType.VarChar)
                                {
                                    Value = 2
                                });

                                cmd.Parameters.Add(new SqlParameter("@trmr_id",
                           SqlDbType.VarChar)
                                {
                                    Value = data.TRMR_Id
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            result.Add(new FeeStudentGroupMappingDTO
                                            {

                                                AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                                AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                                AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                                                AMST_LastName = dataReader["AMST_LastName"].ToString(),
                                                AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                                                AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString(),
                                                AMAY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"].ToString()),
                                                ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                                ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),
                                                AMST_Mobile = Convert.ToInt64(dataReader["AMST_MobileNo"].ToString()),

                                            });
                                        }
                                    }
                                    data.alldata = result.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }

                        break;

                    //                  data.alldata = (from c in _YearlyFeeGroupMappingContext.School_M_Class
                    //                                       from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                    //                                       from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                    //                                       from g in _YearlyFeeGroupMappingContext.school_M_Section
                    //                                       where (d.ASMCL_Id==c.ASMCL_Id && d.ASMS_Id==g.ASMS_Id && e.AMST_SOL == "S" && d.AMST_Id == e.AMST_Id && e.AMST_ActiveFlag == 1 && e.MI_Id == data.MI_Id && d.AMAY_ActiveFlag == 1 && ((e.AMST_FirstName.Trim() + ' ' + e.AMST_MiddleName.Trim() + ' ' + e.AMST_LastName.Trim()).Contains(data.searchtext) || e.AMST_FirstName.StartsWith(data.searchtext) || e.AMST_MiddleName.StartsWith(data.searchtext) || e.AMST_LastName.StartsWith(data.searchtext)))
                    //                                       select new FeeStudentGroupMappingDTO
                    //                                       {
                    //                                           AMST_Id = d.AMST_Id,
                    //                                           AMST_FirstName = e.AMST_FirstName,
                    //                                           AMST_MiddleName = e.AMST_MiddleName,
                    //                                           AMST_LastName = e.AMST_LastName,
                    //                                           AMST_AdmNo = e.AMST_AdmNo,
                    //                                           AMST_RegistrationNo = e.AMST_RegistrationNo,
                    //                                           AMAY_RollNo = d.AMAY_RollNo,
                    //                                           ASMCL_ClassName = c.ASMCL_ClassName,
                    //                                           ASMC_SectionName = g.ASMC_SectionName,
                    //                                           AMST_Mobile = e.AMST_MobileNo,
                    //                                       }
                    //).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                    //break;
                    case "1":
                        if (data.radioval == "LeftStudent")
                        {
                            data.alldata = (from c in _YearlyFeeGroupMappingContext.School_M_Class
                                            from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from g in _YearlyFeeGroupMappingContext.school_M_Section
                                            where (d.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == g.ASMS_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "L" && e.AMST_ActiveFlag == 0 && d.AMAY_ActiveFlag == 1 && c.ASMCL_ClassName.Contains(data.searchtext) && e.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                            select new FeeStudentGroupMappingDTO
                                            {
                                                AMST_Id = d.AMST_Id,
                                                AMST_FirstName = e.AMST_FirstName,
                                                AMST_MiddleName = e.AMST_MiddleName,
                                                AMST_LastName = e.AMST_LastName,
                                                AMST_AdmNo = e.AMST_AdmNo,
                                                AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                AMAY_RollNo = d.AMAY_RollNo,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = g.ASMC_SectionName,
                                                AMST_Mobile = e.AMST_MobileNo
                                            }
       ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        }
                        else if (data.radioval == "CCAT")
                        {

                            data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.castecategoryDMO
                                            from d in _YearlyFeeGroupMappingContext.School_M_Class
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && c.IMCC_Id == a.IMCC_Id && a.IMCC_Id == data.IMCC_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && d.ASMCL_Id == b.ASMCL_Id && d.ASMCL_ClassName.Contains(data.searchtext))
                                            select new FeeStudentGroupMappingDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_FirstName,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                AMAY_RollNo = b.AMAY_RollNo
                                            }
              ).OrderBy(t => t.AMST_FirstName).ToArray();

                        }

                        else
                        {
                            data.alldata = (from c in _YearlyFeeGroupMappingContext.School_M_Class
                                            from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from g in _YearlyFeeGroupMappingContext.school_M_Section
                                            where (d.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == g.ASMS_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && c.ASMCL_ClassName.Contains(data.searchtext) && e.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                            select new FeeStudentGroupMappingDTO
                                            {
                                                AMST_Id = d.AMST_Id,
                                                AMST_FirstName = e.AMST_FirstName,
                                                AMST_MiddleName = e.AMST_MiddleName,
                                                AMST_LastName = e.AMST_LastName,
                                                AMST_AdmNo = e.AMST_AdmNo,
                                                AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                AMAY_RollNo = d.AMAY_RollNo,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = g.ASMC_SectionName,
                                                AMST_Mobile = e.AMST_MobileNo
                                            }
          ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        }
                        break;
                    case "2":

                        if (data.radioval == "LeftStudent")
                        {
                            data.alldata = (from c in _YearlyFeeGroupMappingContext.School_M_Class
                                            from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from g in _YearlyFeeGroupMappingContext.school_M_Section
                                            where (d.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == g.ASMS_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "L" && e.AMST_ActiveFlag == 0 && d.AMAY_ActiveFlag == 1 && e.AMST_AdmNo.Contains(data.searchtext) && e.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                            select new FeeStudentGroupMappingDTO
                                            {
                                                AMST_Id = d.AMST_Id,
                                                AMST_FirstName = e.AMST_FirstName,
                                                AMST_MiddleName = e.AMST_MiddleName,
                                                AMST_LastName = e.AMST_LastName,
                                                AMST_AdmNo = e.AMST_AdmNo,
                                                AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                AMAY_RollNo = d.AMAY_RollNo,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = g.ASMC_SectionName,
                                                AMST_Mobile = e.AMST_MobileNo
                                            }
         ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        }
                        else if (data.radioval == "CCAT")
                        {




                            data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.castecategoryDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && c.IMCC_Id == a.IMCC_Id && a.IMCC_Id == data.IMCC_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.AMST_AdmNo.Contains(data.searchtext))
                                            select new FeeStudentGroupMappingDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_FirstName,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                AMAY_RollNo = b.AMAY_RollNo
                                            }
              ).OrderBy(t => t.AMST_FirstName).ToArray();

                        }

                        else
                        {
                            data.alldata = (from c in _YearlyFeeGroupMappingContext.School_M_Class
                                            from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from g in _YearlyFeeGroupMappingContext.school_M_Section
                                            where (d.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == g.ASMS_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && e.AMST_AdmNo.Contains(data.searchtext) && e.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                            select new FeeStudentGroupMappingDTO
                                            {
                                                AMST_Id = d.AMST_Id,
                                                AMST_FirstName = e.AMST_FirstName,
                                                AMST_MiddleName = e.AMST_MiddleName,
                                                AMST_LastName = e.AMST_LastName,
                                                AMST_AdmNo = e.AMST_AdmNo,
                                                AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                AMAY_RollNo = d.AMAY_RollNo,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = g.ASMC_SectionName,
                                                AMST_Mobile = e.AMST_MobileNo
                                            }
         ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        }
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO searchingstaff(FeeStudentGroupMappingDTO data)
        {
            try
            {
                data.saved_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                        from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                            //from c in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead_Installments
                                        where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id)
                                        select a.HRME_Id).ToList().Distinct().ToArray();
                switch (data.searchType)
                {
                    case "0":
                        data.searchtext = data.searchtext.ToUpper();
                        data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                          from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                          from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && (((a.HRME_EmployeeFirstName.ToUpper() == null ? " " : a.HRME_EmployeeFirstName.ToUpper()) + " " + (a.HRME_EmployeeMiddleName.ToUpper() == null ? " " : a.HRME_EmployeeMiddleName.ToUpper()) + " " + (a.HRME_EmployeeLastName.ToUpper() == null ? " " : a.HRME_EmployeeLastName.ToUpper())).Trim().Contains(data.searchtext) || a.HRME_EmployeeFirstName.ToUpper().StartsWith(data.searchtext) || a.HRME_EmployeeMiddleName.ToUpper().StartsWith(data.searchtext) || a.HRME_EmployeeLastName.ToUpper().StartsWith(data.searchtext)))
                                          select new Temp_Staff_DTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                              HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                              HRMD_Id = a.HRMD_Id,
                                              HRMDES_Id = c.HRMDES_Id,
                                              HRMD_DepartmentName = b.HRMD_DepartmentName,
                                              HRMDES_DesignationName = c.HRMDES_DesignationName
                                          }).ToList().Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

                        break;
                    case "1":

                        data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                          from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                          from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.HRME_EmployeeCode.Contains(data.searchtext))
                                          select new Temp_Staff_DTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                              HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                              HRMD_Id = a.HRMD_Id,
                                              HRMDES_Id = c.HRMDES_Id,
                                              HRMD_DepartmentName = b.HRMD_DepartmentName,
                                              HRMDES_DesignationName = c.HRMDES_DesignationName
                                          }).ToList().Distinct().OrderBy(t => t.HRME_EmployeeCode).ToArray();
                        break;
                    case "2":
                        data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                          from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                          from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && b.HRMD_DepartmentName.Contains(data.searchtext))
                                          select new Temp_Staff_DTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                              HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                              HRMD_Id = a.HRMD_Id,
                                              HRMDES_Id = c.HRMDES_Id,
                                              HRMD_DepartmentName = b.HRMD_DepartmentName,
                                              HRMDES_DesignationName = c.HRMDES_DesignationName
                                          }).ToList().Distinct().OrderBy(t => t.HRMD_DepartmentName).ToArray();

                        break;
                    case "3":
                        data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                          from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                          from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && c.HRMDES_DesignationName.Contains(data.searchtext))
                                          select new Temp_Staff_DTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                              HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                              HRMD_Id = a.HRMD_Id,
                                              HRMDES_Id = c.HRMDES_Id,
                                              HRMD_DepartmentName = b.HRMD_DepartmentName,
                                              HRMDES_DesignationName = c.HRMDES_DesignationName
                                          }).ToList().Distinct().OrderBy(t => t.HRMDES_DesignationName).ToArray();

                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO searchingothers(FeeStudentGroupMappingDTO data)
        {
            try
            {
                data.saved_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                                  //from c in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead_Installments
                                              where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                              select a.FMOST_Id).ToList().Distinct().ToArray();
                switch (data.searchType)
                {
                    case "0":
                        data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true && s.FMOST_StudentName.Contains(data.searchtext)).ToList().Distinct().OrderBy(t => t.FMOST_StudentName).ToArray();

                        break;
                    case "1":

                        data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true && s.FMOST_StudentMobileNo.ToString().Contains(data.searchtext)).ToList().Distinct().OrderBy(t => t.FMOST_StudentMobileNo).ToArray();
                        break;
                    case "2":
                        data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true && s.FMOST_StudentEmailId.Contains(data.searchtext)).ToList().Distinct().OrderBy(t => t.FMOST_StudentEmailId).ToArray();

                        break;

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO getacademicyr(FeeStudentGroupMappingDTO pgmod)
        {
            try
            {
                pgmod.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                         from b in _YearlyFeeGroupMappingContext.feeGroup
                                         from f in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                         from g in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                         where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == pgmod.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == pgmod.user_id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             FMG_Id = a.FMG_Id,
                                             FMG_GroupName = b.FMG_GroupName
                                         }).Distinct().ToArray();

                pgmod.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                        from b in _YearlyFeeGroupMappingContext.feeGroup
                                        from c in _YearlyFeeGroupMappingContext.feehead
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1")
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FMH_FeeName = c.FMH_FeeName
                                        }).Distinct().ToArray();

                pgmod.fillinstallment = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                         from b in _YearlyFeeGroupMappingContext.feeGroup
                                         from c in _YearlyFeeGroupMappingContext.feehead
                                         from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                         from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                         where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == pgmod.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == pgmod.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == pgmod.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id)
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                             FMH_Id = c.FMH_Id,
                                             FTI_Id = e.FTI_Id,
                                             FTI_Name = e.FTI_Name
                                         }).Distinct().ToArray();

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                 from c in _YearlyFeeGroupMappingContext.School_M_Class
                                 from e in _YearlyFeeGroupMappingContext.AdmSection
                                 from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                 where (d.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == pgmod.MI_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == d.ASMS_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                 select new FeeStudentGroupMappingDTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                     AMST_FirstName = a.AMST_FirstName,
                                     AMST_MiddleName = a.AMST_MiddleName,
                                     AMST_LastName = a.AMST_LastName,
                                     AMST_AdmNo = a.AMST_AdmNo,
                                     AMST_RegistrationNo = a.AMST_RegistrationNo,
                                     AMAY_RollNo = d.AMAY_RollNo,
                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                     ASMC_SectionName = e.ASMC_SectionName
                                     
                                 }
        ).Distinct().Take(10).ToArray(); /* .Take(5)*/

                var fetchmaxfmsgid = _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id).OrderByDescending(t => t.FMSG_Id).Take(5).Select(t => t.FMSG_Id).ToList();

                pgmod.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                      from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                      from c in _YearlyFeeGroupMappingContext.School_M_Class
                                      from g in _YearlyFeeGroupMappingContext.school_M_Section
                                      from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                      from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                      from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                      where (d.ASMS_Id == g.ASMS_Id && fetchmaxfmsgid.Contains(a.FMSG_Id) && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == pgmod.MI_Id && d.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                      select new FeeStudentGroupMappingDTO
                                      {
                                          AMST_Id = a.AMST_Id,
                                          AMST_FirstName = e.AMST_FirstName,
                                          AMST_MiddleName = e.AMST_MiddleName,
                                          AMST_LastName = e.AMST_LastName,
                                          AMST_AdmNo = e.AMST_AdmNo,
                                          AMST_RegistrationNo = e.AMST_RegistrationNo,
                                          AMAY_RollNo = d.AMAY_RollNo,
                                          FMG_GroupName = b.FMG_GroupName,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = g.ASMC_SectionName,
                                          AMST_Mobile = e.AMST_MobileNo,
                                          FMSG_Id = a.FMSG_Id,
                                          FMG_Id = b.FMG_Id

                                      }
       ).Distinct().OrderBy(t => t.FMSG_Id).ToArray();

                pgmod.configsetting = _YearlyFeeGroupMappingContext.feemastersettings.Where(s => s.MI_Id == pgmod.MI_Id && s.userid == pgmod.user_id).ToList().Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pgmod;
        }

        public FeeStudentGroupMappingDTO fillstudentsroute(FeeStudentGroupMappingDTO data)
        {
            FeeStudentGroupMappingDTO fee = new FeeStudentGroupMappingDTO();
            try
            {
                if (data.radioval == "BR" && data.classwisecheckboxvalue == true)
                {
                    fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                   from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   from c in _YearlyFeeGroupMappingContext.TR_Student_RouteDMO
                                   from d in _YearlyFeeGroupMappingContext.MasterRouteDMO
                                   from h in _YearlyFeeGroupMappingContext.AdmSection
                                   from f in _YearlyFeeGroupMappingContext.School_M_Class
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMS_Id== h.ASMS_Id && b.ASMS_Id == f.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == b.AMST_Id && (c.TRMR_Id == d.TRMR_Id || c.TRMR_Drop_Route == d.TRMR_Id) && d.TRMA_Id == data.TRMA_Id && c.TRSR_ActiveFlg == true && c.MI_Id == a.MI_Id
                    )
                                   select new FeeStudentGroupMappingDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = a.AMST_FirstName,
                                       AMST_MiddleName = a.AMST_MiddleName,
                                       AMST_LastName = a.AMST_LastName,
                                       AMST_AdmNo = a.AMST_AdmNo,
                                       AMST_RegistrationNo = a.AMST_RegistrationNo,
                                       AMAY_RollNo = b.AMAY_RollNo,
                                       ASMCL_ClassName = f.ASMCL_ClassName,
                                       ASMC_SectionName = h.ASMC_SectionName
                                       

                                   }
   ).Distinct().OrderBy(t => t.AMST_FirstName).Take(5).ToArray();
                    if (fee.alldata.Length > 0)
                    {
                        fee.returnval = "Yes";
                    }
                    else
                    {
                        fee.returnval = "No";
                    }
                }
                else
                {
                    fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                   from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   from c in _YearlyFeeGroupMappingContext.TR_Student_RouteDMO
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && c.AMST_Id == b.AMST_Id && c.MI_Id == a.MI_Id && (c.TRMR_Id == data.TRMR_Id || c.TRMR_Drop_Route == data.TRMR_Id) && c.TRSR_ActiveFlg == true && c.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeStudentGroupMappingDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = a.AMST_FirstName,
                                       AMST_MiddleName = a.AMST_MiddleName,
                                       AMST_LastName = a.AMST_LastName,
                                       AMST_AdmNo = a.AMST_AdmNo,
                                       AMST_RegistrationNo = a.AMST_RegistrationNo,
                                       AMAY_RollNo = b.AMAY_RollNo
                                   }
   ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                    if (fee.alldata.Length > 0)
                    {
                        fee.returnval = "Yes";
                    }
                    else
                    {
                        fee.returnval = "No";
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;
        }

        public FeeStudentGroupMappingDTO geteditdatastaffothers(FeeStudentGroupMappingDTO data)
        {
            try
            {
                if (data.radioval == "Staff")
                {
                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead_Installments
                                         from g in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                         from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                         from e in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                         from f in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                         where (f.HRME_Id == a.HRME_Id && f.ASMAY_Id == a.ASMAY_Id && f.MI_Id == a.MI_Id && a.FMG_Id == f.FMG_Id && c.FMH_Id == f.FMH_Id && c.FTI_Id == f.FTI_Id && a.FMSTGH_Id == c.FMSTGH_Id && a.FMG_Id == b.FMG_Id && c.FMH_Id == g.FMH_Id && c.FTI_Id == d.FTI_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == a.FMG_Id && e.FMH_Id == c.FMH_Id && e.FMI_Id == d.FMI_Id)
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             FMG_Id = a.FMG_Id,
                                             FMG_GroupName = b.FMG_GroupName,
                                             FMH_Id = g.FMH_Id,
                                             FMH_FeeName = g.FMH_FeeName,
                                             FMSTGH_Id = a.FMSTGH_Id,
                                             FTI_Id = d.FTI_Id,
                                             FTI_Name = d.FTI_Name,
                                             FSS_PaidAmount = f.FSSST_PaidAmount,
                                         }
                ).Distinct().ToArray();
                }
                else if (data.radioval == "Others")
                {
                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GH_InstlDMO
                                         from g in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                         from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                         from e in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                         from f in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                         where (f.FMOST_Id == a.FMOST_Id && f.ASMAY_Id == a.ASMAY_Id && f.MI_Id == a.MI_Id && a.FMG_Id == f.FMG_Id && c.FMH_Id == f.FMH_Id && c.FTI_Id == f.FTI_Id && a.FMOSTGH_Id == c.FMOSTGH_Id && a.FMG_Id == b.FMG_Id && c.FMH_Id == g.FMH_Id && c.FTI_Id == d.FTI_Id && a.FMOST_Id == data.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == a.FMG_Id && e.FMH_Id == c.FMH_Id && e.FMI_Id == d.FMI_Id)
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             FMOST_Id = a.FMOST_Id,
                                             FMG_Id = a.FMG_Id,
                                             FMG_GroupName = b.FMG_GroupName,
                                             FMH_Id = g.FMH_Id,
                                             FMH_FeeName = g.FMH_FeeName,
                                             FMSG_Id = a.FMOSTGH_Id,
                                             FTI_Id = d.FTI_Id,
                                             FTI_Name = d.FTI_Name,
                                             FSS_PaidAmount = f.FSSOST_PaidAmount,
                                         }
                ).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO saveeditdataothers(FeeStudentGroupMappingDTO pgmod)
        {

            try
            {
                string returntxt = "";
                if (pgmod.FMOST_Id != 0)
                {
                    int G = 0, H = 0, I = 0;

                    var FMCC_Idnew = 1;

                    if (FMCC_Idnew == 0)
                    {
                        returntxt = "a";
                    }
                    else
                    {
                        while (G < pgmod.savegrplst.Count())
                        {

                            while (H < pgmod.saveheadlst.Count())
                            {
                                while (I < pgmod.saveftilst.Count())
                                {

                                    if (pgmod.saveftilst[I].disableins == false)
                                    {
                                        if (pgmod.saveftilst[I].FMH_Id == pgmod.saveheadlst[H].FMH_Id && pgmod.saveftilst[I].FMG_Id == pgmod.savegrplst[G].FMG_Id && pgmod.savegrplst[G].FMG_Id == pgmod.saveheadlst[H].FMG_Id)
                                        {
                                            if (pgmod.savegrplst[G].checkedgrplstedit == true && pgmod.saveheadlst[H].checkedheadlstedit == true && pgmod.saveftilst[I].checkedinstallmentlstedit == true)
                                            {
                                                var checkforduplicates1 = (from a in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                                                           from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GH_InstlDMO
                                                                           from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                                                           where (a.FMOSTGH_Id == b.FMOSTGH_Id && a.FMG_Id == c.FMG_Id && a.FMOST_Id == c.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.FMOST_Id == pgmod.FMOST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                           select b.FMOSTGHI_Id).Distinct().ToList();
                                                if (checkforduplicates1.Count().Equals(0))
                                                {
                                                    var FMAlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                                                   from b in _YearlyFeeGroupMappingContext.feeMIY
                                                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                                   where (a.FTI_Id == b.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMG_Id == pgmod.saveftilst[I].FMG_Id && a.FMH_Id == pgmod.saveftilst[I].FMH_Id && a.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.MI_Id == a.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == a.FMH_Id && a.FMAOST_OthStaffFlag == "O" && ((a.FMAOST_Amount >= 0 && c.FMH_Flag != "F" && c.FMH_Flag != "E") || (a.FMAOST_Amount >= 0 && (c.FMH_Flag == "F" || c.FMH_Flag == "E"))))/* && a.FMA_Amount > 0*/
                                                                   select a.FMAOST_Id).Distinct().ToList();

                                                    if (FMAlist.Distinct().Count().Equals(0))
                                                    {
                                                        returntxt = "a";
                                                    }
                                                    else
                                                    {
                                                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Insert_Fee_Student_Mapnew_others";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                                                SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FMG_Id
                                                            });
                                                            cmd.Parameters.Add(new SqlParameter("@FMOST_Id",
                                                               SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.FMOST_Id
                                                            });
                                                            cmd.Parameters.Add(new SqlParameter("@MI_ID",
                                                           SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.MI_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@fti_id_new",
                                                        SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FTI_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@FMH_ID_new",
                                                              SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FMH_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@userid",
                                                          SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.user_id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                                             SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.ASMAY_Id
                                                            });

                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();
                                                            var data = cmd.ExecuteNonQuery();

                                                            if (data >= 1)
                                                            {
                                                                pgmod.returnval = "true";
                                                            }
                                                            else
                                                            {
                                                                pgmod.returnval = "false";
                                                            }
                                                        }
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                var checkforduplicatesdel = (from a in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GHDMO
                                                                             from b in _YearlyFeeGroupMappingContext.Fee_Master_OthStudents_GH_InstlDMO
                                                                             from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                                                             where (a.FMOSTGH_Id == b.FMOSTGH_Id && a.FMG_Id == c.FMG_Id && a.FMOST_Id == c.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.FMOST_Id == pgmod.HRME_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.FSSOST_PaidAmount == 0 && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                             select new { a.ASMAY_Id, a.MI_Id, a.FMG_Id, a.FMOST_Id, a.FMOSTGH_Id, b.FMH_Id, b.FTI_Id }).Distinct().ToList();
                                                if (checkforduplicatesdel.Count > 0)
                                                {

                                                    foreach (var a in checkforduplicatesdel)
                                                    {
                                                        using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                                                        {

                                                            var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMappingnew_others @p0,@p1,@p2,@p3,@p4,@p5,@p6", a.MI_Id, a.FMOST_Id, a.ASMAY_Id, a.FMG_Id, a.FMOSTGH_Id, a.FMH_Id, a.FTI_Id);

                                                            if (outputval >= 1)
                                                            {
                                                                transaction.Commit();
                                                                pgmod.returnval = "true";
                                                            }
                                                            else
                                                            {
                                                                pgmod.returnval = "false";
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    I++;
                                }
                                I = 0;
                                H++;
                            }
                            H = 0;
                            G++;
                        }
                    }


                }
                if (returntxt != "")
                {
                    pgmod.returnval = "false";
                }
            }

            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }

        public FeeStudentGroupMappingDTO saveeditdatastaff(FeeStudentGroupMappingDTO pgmod)
        {

            try
            {
                string returntxt = "";
                if (pgmod.HRME_Id != 0)
                {
                    int G = 0, H = 0, I = 0;
                    //var FMCC_Idnew = (from a in _YearlyFeeGroupMappingContext.feeYCC
                    //                  from b in _YearlyFeeGroupMappingContext.feeYCCC
                    //                  from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                    //                  where (a.FYCC_Id == b.FYCC_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == pgmod.MI_Id && b.ASMCL_Id == c.ASMCL_Id && c.AMST_Id == pgmod.AMST_Id && c.ASMAY_Id == pgmod.ASMAY_Id)
                    //                  select a.FMCC_Id).FirstOrDefault();
                    var FMCC_Idnew = 1;

                    if (FMCC_Idnew == 0)
                    {
                        returntxt = "a";
                    }
                    else
                    {
                        while (G < pgmod.savegrplst.Count())
                        {

                            while (H < pgmod.saveheadlst.Count())
                            {
                                while (I < pgmod.saveftilst.Count())
                                {

                                    if (pgmod.saveftilst[I].disableins == false)
                                    {
                                        if (pgmod.saveftilst[I].FMH_Id == pgmod.saveheadlst[H].FMH_Id && pgmod.saveftilst[I].FMG_Id == pgmod.savegrplst[G].FMG_Id && pgmod.savegrplst[G].FMG_Id == pgmod.saveheadlst[H].FMG_Id)
                                        {
                                            if (pgmod.savegrplst[G].checkedgrplstedit == true && pgmod.saveheadlst[H].checkedheadlstedit == true && pgmod.saveftilst[I].checkedinstallmentlstedit == true)
                                            {
                                                var checkforduplicates1 = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                                                           from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead_Installments
                                                                           from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                                                           where (a.FMSTGH_Id == b.FMSTGH_Id && a.FMG_Id == c.FMG_Id && a.HRME_Id == c.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.HRME_Id == pgmod.HRME_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                           select b.FMSTGHI_Id).Distinct().ToList();
                                                if (checkforduplicates1.Count().Equals(0))
                                                {
                                                    var FMAlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                                                   from b in _YearlyFeeGroupMappingContext.feeMIY
                                                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                                   where (a.FTI_Id == b.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMG_Id == pgmod.saveftilst[I].FMG_Id && a.FMH_Id == pgmod.saveftilst[I].FMH_Id && a.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.MI_Id == a.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == a.FMH_Id && a.FMAOST_OthStaffFlag == "S" && ((a.FMAOST_Amount >= 0 && c.FMH_Flag != "F" && c.FMH_Flag != "E") || (a.FMAOST_Amount >= 0 && (c.FMH_Flag == "F" || c.FMH_Flag == "E"))))/* && a.FMA_Amount > 0*/
                                                                   select a.FMAOST_Id).Distinct().ToList();

                                                    if (FMAlist.Distinct().Count().Equals(0))
                                                    {
                                                        returntxt = "a";
                                                    }
                                                    else
                                                    {
                                                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Insert_Fee_Student_Mapnew_staff";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                                                SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FMG_Id
                                                            });
                                                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                                               SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.HRME_Id
                                                            });
                                                            cmd.Parameters.Add(new SqlParameter("@MI_ID",
                                                           SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.MI_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@fti_id_new",
                                                        SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FTI_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@FMH_ID_new",
                                                              SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FMH_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@userid",
                                                          SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.user_id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                                             SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.ASMAY_Id
                                                            });

                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();
                                                            var data = cmd.ExecuteNonQuery();

                                                            if (data >= 1)
                                                            {
                                                                pgmod.returnval = "true";
                                                            }
                                                            else
                                                            {
                                                                pgmod.returnval = "false";
                                                            }
                                                        }
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                var checkforduplicatesdel = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                                                             from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead_Installments
                                                                             from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                                                             where (a.FMSTGH_Id == b.FMSTGH_Id && a.FMG_Id == c.FMG_Id && a.HRME_Id == c.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.HRME_Id == pgmod.HRME_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.FSSST_PaidAmount == 0 && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                             select new { a.ASMAY_Id, a.MI_Id, a.FMG_Id, a.HRME_Id, a.FMSTGH_Id, b.FMH_Id, b.FTI_Id }).Distinct().ToList();
                                                if (checkforduplicatesdel.Count > 0)
                                                {

                                                    foreach (var a in checkforduplicatesdel)
                                                    {
                                                        using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                                                        {

                                                            var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMappingnew_staff @p0,@p1,@p2,@p3,@p4,@p5,@p6", a.MI_Id, a.HRME_Id, a.ASMAY_Id, a.FMG_Id, a.FMSTGH_Id, a.FMH_Id, a.FTI_Id);

                                                            if (outputval >= 1)
                                                            {
                                                                transaction.Commit();
                                                                pgmod.returnval = "true";
                                                            }
                                                            else
                                                            {
                                                                pgmod.returnval = "false";
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    I++;
                                }
                                I = 0;
                                H++;
                            }
                            H = 0;
                            G++;
                        }
                    }


                }
                if (returntxt != "")
                {
                    pgmod.returnval = "false";
                }
            }

            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }

    }
}













