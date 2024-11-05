using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class StaffAndOtherFeeGroupMappingImpl : Interfaces.StaffAndOtherFeeGroupMappingInterface
    {
        private static ConcurrentDictionary<string, Clg_StudentFeeGroupMapping_DTO> _login = new ConcurrentDictionary<string, Clg_StudentFeeGroupMapping_DTO>();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<StaffAndOtherFeeGroupMappingImpl> _logger;

        public StaffAndOtherFeeGroupMappingImpl(CollFeeGroupContext objDbcontext)
        {
            _YearlyFeeGroupMappingContext = objDbcontext;
        }

        public Clg_StudentFeeGroupMapping_DTO deleterec_s(Clg_StudentFeeGroupMapping_DTO data)
        {
            //  using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
            //  {
            try
            {
                //var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMapping @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.AMST_Id, data.ASMAY_Id, data.FMG_Id, data.FMSG_Id);
                var paid_amount = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.HRME_Id == data.HRME_Id && s.FMG_Id == data.FMG_Id).Sum(t => t.FCSSST_PaidAmount);
                if (paid_amount == 0)
                {

                    var staff_status_list = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.HRME_Id == data.HRME_Id && s.FMG_Id == data.FMG_Id ).ToList();
                    var staff_group = _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO.Where(g => g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.HRME_Id == data.HRME_Id && g.FMG_Id == data.FMG_Id && g.FMCSTGH_Id == data.FMSTGH_Id).ToList();
                    var staff_grp_head_instmnts = _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHead_InstallmentsDMO.Where(h => h.FMCSTGH_Id == data.FMSTGH_Id).ToList();
                    if (staff_status_list.Any())
                    {
                        foreach(var i in staff_status_list)
                        {
                            _YearlyFeeGroupMappingContext.Remove(i);
                        }
                        //for (int i = 0; staff_status_list.Count > i; i++)
                        //{
                        //    _YearlyFeeGroupMappingContext.Remove(staff_status_list.ElementAt(i));
                        //}
                    }
                    if (staff_grp_head_instmnts.Any())
                    {
                        foreach (var i in staff_grp_head_instmnts)
                        {
                            _YearlyFeeGroupMappingContext.Remove(i);
                        }
                        //for (int i = 0; staff_grp_head_instmnts.Count > i; i++)
                        //{
                        //    _YearlyFeeGroupMappingContext.Remove(staff_grp_head_instmnts.ElementAt(i));
                        //}
                    }
                    if (staff_group.Any())
                    {
                        foreach (var i in staff_group)
                        {
                            _YearlyFeeGroupMappingContext.Remove(i);
                        }
                        //for (int i = 0; staff_group.Count > i; i++)
                        //{
                        //    _YearlyFeeGroupMappingContext.Remove(staff_group.ElementAt(i));
                        //}
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
                                       from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                           // from c in _YearlyFeeGroupMappingContext.MasterEmployee
                                       from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                       from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                           //from c in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHead_InstallmentsDMO
                                       where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id)
                                       select new Clg_StudentFeeGroupMapping_DTO
                                       {
                                           FMSTGH_Id = b.FMCSTGH_Id,
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

        public Clg_StudentFeeGroupMapping_DTO deleterec_o(Clg_StudentFeeGroupMapping_DTO data)
        {
            //  using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
            //  {
            try
            {
                //var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMapping @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.AMST_Id, data.ASMAY_Id, data.FMG_Id, data.FMSG_Id);

                var paid_amount = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.FMCOST_Id == data.FMOST_Id && s.FMG_Id == data.FMG_Id).Sum(t => t.FCSSOST_PaidAmount);
                if (paid_amount == 0)
                {

                    var oth_student_status_list = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.FMCOST_Id == data.FMOST_Id && s.FMG_Id == data.FMG_Id).ToList();
                    var oth_stu_group = _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO.Where(g => g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMCOST_Id == data.FMOST_Id && g.FMG_Id == data.FMG_Id && g.FMCOSTGH_Id == data.FMOSTGH_Id).ToList();
                    var oth_stu_grp_head_instmnts = _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GH_InstlDMO.Where(h => h.FMCOSTGH_Id == data.FMOSTGH_Id).ToList();
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
                data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                             from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                             where (a.MI_Id == b.MI_Id && a.FMCOST_ActiveFlag == true && a.FMCOST_Id == b.FMCOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id)
                                             select new Clg_StudentFeeGroupMapping_DTO
                                             {
                                                 FMOSTGH_Id = b.FMCOSTGH_Id,
                                                 FMOST_Id = a.FMCOST_Id,
                                                 FMOST_StudentName = a.FMCOST_StudentName,
                                                 FMOST_StudentMobileNo = a.FMCOST_StudentMobileNo,
                                                 FMOST_StudentEmailId = a.FMCOST_StudentEmailId,
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
        public Clg_StudentFeeGroupMapping_DTO EditMasterscetionDetails(int id)
        {
            throw new NotImplementedException();
        }

        public Clg_StudentFeeGroupMapping_DTO getdata(Clg_StudentFeeGroupMapping_DTO fee)
        {

            try
            {
                //List<MasterAcademic> allyear = new List<MasterAcademic>();
                fee.academicdrp = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == fee.MI_Id && t.Is_Active == true).OrderByDescending(l => l.ASMAY_Order).ToArray();
                // fee.academicdrp = allyear.Distinct().ToArray();

                fee.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from f in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                       from g in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                       where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == fee.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == fee.user_id && a.ASMAY_Id == fee.ASMAY_Id && a.ASMAY_Id == g.ASMAY_Id)
                                       select new Clg_StudentFeeGroupMapping_DTO
                                       {
                                           FMG_Id = a.FMG_Id,
                                           FMG_GroupName = b.FMG_GroupName
                                       }).Distinct().ToArray();

                fee.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                      from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                      from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                      where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1")
                                      select new Clg_StudentFeeGroupMapping_DTO
                                      {
                                          FMG_Id = b.FMG_Id,
                                          FMH_Id = c.FMH_Id,
                                          FMH_FeeName = c.FMH_FeeName
                                      }).Distinct().ToArray();

                fee.fillinstallment = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                       from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                       where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == fee.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == fee.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == fee.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id)
                                       select new Clg_StudentFeeGroupMapping_DTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FTI_Id = e.FTI_Id,
                                           FTI_Name = e.FTI_Name
                                       }).Distinct().ToArray();

                //        fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                       from c in _YearlyFeeGroupMappingContext.School_M_Class
                //                       from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                       where (d.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                //                       select new Clg_StudentFeeGroupMapping_DTO
                //                       {
                //                           AMST_Id = a.AMST_Id,
                //                           AMST_FirstName = a.AMST_FirstName,
                //                           AMST_MiddleName = a.AMST_MiddleName,
                //                           AMST_LastName = a.AMST_LastName,
                //                           AMST_AdmNo = a.AMST_AdmNo,
                //                           AMST_RegistrationNo = a.AMST_RegistrationNo,
                //                           AMAY_RollNo = d.AMAY_RollNo,
                //                           ASMCL_ClassName = c.ASMCL_ClassName

                //                       }
                //).Distinct().Take(10).ToArray(); 

                //         var fetchmaxfmsgid = _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO.Where(t => t.MI_Id == fee.MI_Id && t.ASMAY_Id == fee.ASMAY_Id).OrderByDescending(t => t.FMSG_Id).Take(5).Select(t => t.FMSG_Id).ToList();

                //         fee.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                //                             from c in _YearlyFeeGroupMappingContext.School_M_Class
                //                             from g in _YearlyFeeGroupMappingContext.school_M_Section
                //                             from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                             from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                             where (d.ASMS_Id == g.ASMS_Id && fetchmaxfmsgid.Contains(a.FMSG_Id) && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == fee.MI_Id && d.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                //                             select new Clg_StudentFeeGroupMapping_DTO
                //                             {
                //                                 AMST_Id = a.AMST_Id,
                //                                 AMST_FirstName = e.AMST_FirstName,
                //                                 AMST_MiddleName = e.AMST_MiddleName,
                //                                 AMST_LastName = e.AMST_LastName,
                //                                 AMST_AdmNo = e.AMST_AdmNo,
                //                                 AMST_RegistrationNo = e.AMST_RegistrationNo,
                //                                 AMAY_RollNo = d.AMAY_RollNo,
                //                                 FMG_GroupName = b.FMG_GroupName,
                //                                 ASMCL_ClassName = c.ASMCL_ClassName,
                //                                 ASMC_SectionName = g.ASMC_SectionName,
                //                                 AMST_Mobile = e.AMST_MobileNo,
                //                                 FMSG_Id = a.FMSG_Id,
                //                                 FMG_Id = b.FMG_Id

                //                             }
                //).Distinct().OrderBy(t => t.FMSG_Id).ToArray();



                //         fee.alldatathirdall = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                             from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                             where (a.AMST_Id==d.AMST_Id && d.AMST_Id==e.AMST_Id && a.MI_Id==fee.MI_Id && d.ASMAY_Id==a.ASMAY_Id && a.ASMAY_Id==fee.ASMAY_Id)
                //                             select new Clg_StudentFeeGroupMapping_DTO
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
                //                       from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                //                       where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id)
                //                       select a.HRME_Id).ToList().Distinct().ToArray();
                //fee.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                //                      from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                //                      from d in _YearlyFeeGroupMappingContext.feeGroup
                //                      from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                //                      from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                //                      where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == fee.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == fee.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id)
                //                      select new Clg_StudentFeeGroupMapping_DTO
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
                //fee.oth_studentlist = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(s => s.MI_Id == fee.MI_Id && s.FMOST_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.FMOST_Id).Take(5).ToArray();

                //fee.saved_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                //                             from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                //                             where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id)
                //                             select a.FMOST_Id).ToList().Distinct().ToArray();
                //fee.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                //                            from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                //                            from c in _YearlyFeeGroupMappingContext.feeGroup
                //                            where (a.MI_Id == b.MI_Id && a.FMOST_ActiveFlag == true && a.FMOST_Id == b.FMOST_Id && b.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id)
                //                            select new Clg_StudentFeeGroupMapping_DTO
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



        public Clg_StudentFeeGroupMapping_DTO savedetails_s(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
            // Clg_StudentFeeGroupMapping_DTO feestumap = new Clg_StudentFeeGroupMapping_DTO();
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
                            Fee_Master_College_Staff_GroupHeadDMO obj1 = new Fee_Master_College_Staff_GroupHeadDMO();
                            obj1.FMCSTGH_Id = pgmod.FMSTGH_Id;
                            obj1.MI_Id = pgmod.MI_Id;
                            obj1.ASMAY_Id = pgmod.ASMAY_Id;
                            obj1.HRME_Id = pgmod.staff_list[i].HRME_Id;
                            obj1.FMG_Id = pgmod.savegrplst[j].FMG_Id;
                            obj1.FMCSTGH_ActiveFlag = true;
                            obj1.FMCSTGH_CreatedDate = DateTime.Now;
                            obj1.FMCSTGH_UpdatedDate = DateTime.Now;
                            obj1.FMCSTGH_UpdatedBy = pgmod.user_id;
                            obj1.FMCSTGH_CreatedBy = pgmod.user_id;
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
                                            Fee_Master_College_Staff_GroupHead_InstallmentsDMO obj2 = new Fee_Master_College_Staff_GroupHead_InstallmentsDMO();
                                            obj2.FMCSTGH_Id = obj1.FMCSTGH_Id;
                                            obj2.FMH_Id = pgmod.saveheadlst[k].FMH_Id;
                                            obj2.FTI_Id = pgmod.saveftilst[l].FTI_Id;
                                            obj2.FMCSTGHI_CreatedBy = pgmod.user_id;
                                            obj2.FMCSTGHI_UpdatedBy = pgmod.user_id;
                                            obj2.FMCSTGHI_UpdatedDate = DateTime.Now;
                                            obj2.FMCSTGHI_CreatedDate = DateTime.Now;
                                            _YearlyFeeGroupMappingContext.Add(obj2);


                                            var amount_list = _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.FMG_Id == pgmod.savegrplst[j].FMG_Id && t.FMH_Id == pgmod.saveheadlst[k].FMH_Id && t.FTI_Id == pgmod.saveftilst[l].FTI_Id && t.FMCAOST_OthStaffFlag == "S" && t.FMCAOST_ActiveFlag == true).Distinct().ToList();

                                            if (amount_list.Count > 0)
                                            {
                                                foreach (var x in amount_list)
                                                {
                                                    Fee_College_Student_Status_Staff obj_status = new Fee_College_Student_Status_Staff();
                                                    obj_status.MI_Id = pgmod.MI_Id;
                                                    obj_status.ASMAY_Id = pgmod.ASMAY_Id;
                                                    obj_status.HRME_Id = pgmod.staff_list[i].HRME_Id;
                                                    obj_status.FMG_Id = pgmod.savegrplst[j].FMG_Id;
                                                    obj_status.FMH_Id = pgmod.saveheadlst[k].FMH_Id;
                                                    obj_status.FTI_Id = pgmod.saveftilst[l].FTI_Id;
                                                    obj_status.FMCAOST_Id = Convert.ToInt64(x.FMCAOST_Id);
                                                    obj_status.FCSSST_OBArrearAmount = 0;
                                                    obj_status.FCSSST_OBExcessAmount = 0;
                                                    obj_status.FCSSST_CurrentYrCharges = Convert.ToInt64(x.FMCAOST_Amount);
                                                    //obj_status.FCSSST_TotalCharges = 0;
                                                    obj_status.FCSSST_ConcessionAmount = 0;
                                                    obj_status.FCSSST_TotalCharges = obj_status.FCSSST_CurrentYrCharges - obj_status.FCSSST_ConcessionAmount;
                                                    obj_status.FCSSST_ToBePaid = Convert.ToInt64(x.FMCAOST_Amount);
                                                    obj_status.FCSSST_WaivedAmount = 0;
                                                    obj_status.FCSSST_PaidAmount = 0;
                                                    obj_status.FCSSST_ExcessPaidAmount = 0;
                                                    obj_status.FCSSST_ExcessAdjustedAmount = 0;
                                                    obj_status.FCSSST_RunningExcessAmount = 0;
                                                    obj_status.FCSSST_AdjustedAmount = 0;
                                                    obj_status.FCSSST_RebateAmount = 0;
                                                    obj_status.FCSSST_FineAmount = 0;
                                                    obj_status.FCSSST_RefundAmount = 0;
                                                    obj_status.FCSSST_RefundAmountAdjusted = 0;
                                                    obj_status.FCSSST_NetAmount = obj_status.FCSSST_CurrentYrCharges;
                                                    obj_status.FCSSST_ChequeBounceAmount = 0;
                                                    obj_status.FCSSST_ArrearFlag = false;
                                                    obj_status.FCSSST_RefundOverFlag = false;
                                                    obj_status.FCSSST_ActiveFlag = true;
                                                    obj_status.FCSSST_CreatedDate = DateTime.Now;
                                                    obj_status.FCSSST_UpdatedDate = DateTime.Now;
                                                    obj_status.FCSSST_CreatedBy = pgmod.user_id;
                                                    obj_status.FCSSST_UpdatedBy = pgmod.user_id;

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

        public Clg_StudentFeeGroupMapping_DTO savedetails_o(Clg_StudentFeeGroupMapping_DTO pgmod)
        {

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
                            Fee_Master_College_OthStudents_GHDMO obj1 = new Fee_Master_College_OthStudents_GHDMO();
                            obj1.FMCOSTGH_Id = pgmod.FMOSTGH_Id;
                            obj1.MI_Id = pgmod.MI_Id;
                            obj1.ASMAY_Id = pgmod.ASMAY_Id;
                            obj1.FMCOST_Id = pgmod.student_list[i].FMOST_Id;
                            obj1.FMG_Id = pgmod.savegrplst[j].FMG_Id;
                            obj1.FMCOSTGH_ActiveFlag = true;
                            obj1.FMCOSTGH_CreatedDate = DateTime.Now;
                            obj1.FMCOSTGH_UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Add(obj1);
                            for (int k = 0; k < pgmod.saveheadlst.Length; k++)
                            {
                                if (pgmod.savegrplst[j].FMG_Id == pgmod.saveheadlst[k].FMG_Id)
                                {
                                    for (int l = 0; l < pgmod.saveftilst.Length; l++)
                                    {

                                        if (pgmod.saveheadlst[k].FMH_Id == pgmod.saveftilst[l].FMH_Id && pgmod.savegrplst[j].FMG_Id == pgmod.saveftilst[l].FMG_Id)
                                        {
                                            Fee_Master_College_OthStudents_GH_InstlDMO obj2 = new Fee_Master_College_OthStudents_GH_InstlDMO();
                                            obj2.FMCOSTGH_Id = obj1.FMCOSTGH_Id;
                                            obj2.FMH_Id = pgmod.saveheadlst[k].FMH_Id;
                                            obj2.FTI_Id = pgmod.saveftilst[l].FTI_Id;
                                            obj2.FMCOSTGHI_CreatedBy = pgmod.user_id;
                                            obj2.FMCOSTGHI_CreatedDate = DateTime.Now;
                                            obj2.FMCOSTGHI_UpdatedDate = DateTime.Now;
                                            obj2.FMCOSTGHI_UpdatedBy = pgmod.user_id;


                                            _YearlyFeeGroupMappingContext.Add(obj2);


                                            var amount_list = _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.FMG_Id == pgmod.savegrplst[j].FMG_Id && t.FMH_Id == pgmod.saveheadlst[k].FMH_Id && t.FTI_Id == pgmod.saveftilst[l].FTI_Id && t.FMCAOST_OthStaffFlag == "O" && t.FMCAOST_ActiveFlag == true).Distinct().ToList();

                                            if (amount_list.Count > 0)
                                            {
                                                foreach (var x in amount_list)
                                                {
                                                    Fee_College_Student_Status_OthStuDMO obj_status = new Fee_College_Student_Status_OthStuDMO();
                                                    obj_status.MI_Id = pgmod.MI_Id;
                                                    obj_status.ASMAY_Id = pgmod.ASMAY_Id;
                                                    obj_status.FMCOST_Id = pgmod.student_list[i].FMOST_Id;
                                                    obj_status.FMG_Id = pgmod.savegrplst[j].FMG_Id;
                                                    obj_status.FMH_Id = pgmod.saveheadlst[k].FMH_Id;
                                                    obj_status.FTI_Id = pgmod.saveftilst[l].FTI_Id;
                                                    obj_status.FMCAOST_Id = Convert.ToInt64(x.FMCAOST_Id);
                                                    obj_status.FCSSOST_OBArrearAmount = 0;
                                                    obj_status.FCSSOST_OBExcessAmount = 0;
                                                    obj_status.FCSSOST_CurrentYrCharges = Convert.ToInt64(x.FMCAOST_Amount);

                                                    obj_status.FCSSOST_ConcessionAmount = 0;
                                                    obj_status.FCSSOST_TotalCharges = obj_status.FCSSOST_CurrentYrCharges - obj_status.FCSSOST_ConcessionAmount;
                                                    obj_status.FCSSOST_ToBePaid = Convert.ToInt64(x.FMCAOST_Amount);
                                                    obj_status.FCSSOST_WaivedAmount = 0;
                                                    obj_status.FCSSOST_PaidAmount = 0;
                                                    obj_status.FCSSOST_ExcessPaidAmount = 0;
                                                    obj_status.FCSSOST_ExcessAdjustedAmount = 0;
                                                    obj_status.FCSSOST_RunningExcessAmount = 0;
                                                    obj_status.FCSSOST_AdjustedAmount = 0;
                                                    obj_status.FCSSOST_RebateAmount = 0;
                                                    obj_status.FCSSOST_FineAmount = 0;
                                                    obj_status.FCSSOST_RefundAmount = 0;
                                                    obj_status.FCSSOST_RefundAmountAdjusted = 0;
                                                    obj_status.FCSSOST_NetAmount = obj_status.FCSSOST_CurrentYrCharges;
                                                    obj_status.FCSSOST_ChequeBounceAmount = 0;
                                                    obj_status.FCSSOST_ArrearFlag = false;
                                                    obj_status.FCSSOST_RefundOverFlag = false;
                                                    obj_status.FCSSOST_ActiveFlag = true;
                                                    obj_status.FCSSOST_CreatedDate = DateTime.Now;
                                                    obj_status.FCSSOST_UpdatedDate = DateTime.Now;
                                                    obj_status.FCSSOST_UpdatedBy = pgmod.user_id;
                                                    obj_status.FCSSOST_CreatedBy = pgmod.user_id;
                                                    _YearlyFeeGroupMappingContext.Add(obj_status);
                                                }
                                            }


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

        public Clg_StudentFeeGroupMapping_DTO getradiofiltereddata(Clg_StudentFeeGroupMapping_DTO data)
        {

            if (data.radioval == "Staff")
            {
                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from b in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                        from c in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                        where (a.FMG_Id == b.FMG_ID && a.MI_Id == data.MI_Id && b.User_Id == data.user_id && a.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMCAOST_ActiveFlag == true && c.FMG_Id == a.FMG_Id && c.FMCAOST_OthStaffFlag == "S")
                                        select new Clg_StudentFeeGroupMapping_DTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName
                                        }).Distinct().ToArray();


                data.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from d in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.FMCAOST_ActiveFlag == true && d.FMG_Id == b.FMG_Id && d.FMCAOST_OthStaffFlag == "S" && d.FMH_Id == c.FMH_Id)
                                       select new Clg_StudentFeeGroupMapping_DTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();



                data.fillinstallment = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        from f in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.FMI_Id == d.FMI_Id && d.FMI_Id == e.FMI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.FMCAOST_ActiveFlag == true && f.FMG_Id == b.FMG_Id && f.FMCAOST_OthStaffFlag == "S" && f.FMH_Id == c.FMH_Id && f.FTI_Id == e.FTI_Id)
                                        select new Clg_StudentFeeGroupMapping_DTO
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
                                        from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                        where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                        select a.HRME_Id).ToList().Distinct().ToArray();
                data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                       from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                       from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                       from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                       where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == data.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id)
                                       select new Clg_StudentFeeGroupMapping_DTO
                                       {
                                           FMSTGH_Id = b.FMCSTGH_Id,
                                           HRME_Id = a.HRME_Id,
                                           HRME_EmployeeCode = a.HRME_EmployeeCode,
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                           HRMD_Id = e.HRMD_Id,
                                           HRMDES_Id = f.HRMDES_Id,

                                           HRMD_DepartmentName = e.HRMD_DepartmentName,
                                           HRMDES_DesignationName = f.HRMDES_DesignationName,
                                           FMG_Id = b.FMG_Id,
                                           FMG_GroupName = d.FMG_GroupName
                                       }).ToList().Distinct().ToArray();



            }
            else if (data.radioval == "Others")
            {
                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from b in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                        from c in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                        where (a.FMG_Id == b.FMG_ID && a.MI_Id == data.MI_Id && b.User_Id == data.user_id && a.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMCAOST_ActiveFlag == true && c.FMG_Id == a.FMG_Id && c.FMCAOST_OthStaffFlag == "O")
                                        select new Clg_StudentFeeGroupMapping_DTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName
                                        }).Distinct().ToArray();


                data.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from d in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.FMCAOST_ActiveFlag == true && d.FMG_Id == b.FMG_Id && d.FMCAOST_OthStaffFlag == "O" && d.FMH_Id == c.FMH_Id)
                                       select new Clg_StudentFeeGroupMapping_DTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();


                data.fillinstallment = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        from f in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.FMI_Id == d.FMI_Id && d.FMI_Id == e.FMI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.FMCAOST_ActiveFlag == true && f.FMG_Id == b.FMG_Id && f.FMCAOST_OthStaffFlag == "O" && f.FMH_Id == c.FMH_Id && f.FTI_Id == e.FTI_Id)
                                        select new Clg_StudentFeeGroupMapping_DTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }).Distinct().ToArray();

       
                data.oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                        where (a.MI_Id == data.MI_Id && a.FMCOST_ActiveFlag == true)
                                        select new Clg_StudentFeeGroupMapping_DTO
                                        {

                                            FMOST_Id = a.FMCOST_Id,
                                            FMOST_StudentName = a.FMCOST_StudentName,
                                            FMOST_StudentMobileNo = a.FMCOST_StudentMobileNo,
                                            FMOST_StudentEmailId = a.FMCOST_StudentEmailId,

                                        }).ToList().Distinct().ToArray();
                data.saved_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                              from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                              where (a.MI_Id == b.MI_Id && a.FMCOST_ActiveFlag == true && a.FMCOST_Id == b.FMCOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)


                                              select a.FMCOST_Id).ToList().Distinct().ToArray();
                data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                             from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                             where (a.MI_Id == b.MI_Id && a.FMCOST_ActiveFlag == true && a.FMCOST_Id == b.FMCOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id)
                                             select new Clg_StudentFeeGroupMapping_DTO
                                             {
                                                 FMOSTGH_Id = b.FMCOSTGH_Id,
                                                 FMOST_Id = a.FMCOST_Id,
                                                 FMOST_StudentName = a.FMCOST_StudentName,
                                                 FMOST_StudentMobileNo = a.FMCOST_StudentMobileNo,
                                                 FMOST_StudentEmailId = a.FMCOST_StudentEmailId,
                                                 FMG_Id = b.FMG_Id,
                                                 FMG_GroupName = c.FMG_GroupName
                                             }).ToList().Distinct().ToArray();

            }


            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO getdataaspercategory(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO studentsavedgroupfacfun(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



        public Clg_StudentFeeGroupMapping_DTO searching_s(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                switch (data.searchType)
                {
                    case "0":
                        data.searchtext = data.searchtext.ToUpper();
                        data.grid_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && ((a.HRME_EmployeeFirstName.ToUpper().Trim() + ' ' + a.HRME_EmployeeMiddleName.ToUpper().Trim() + ' ' + a.HRME_EmployeeLastName.ToUpper().Trim()).Contains(data.searchtext) || a.HRME_EmployeeFirstName.ToUpper().StartsWith(data.searchtext) || a.HRME_EmployeeMiddleName.ToUpper().StartsWith(data.searchtext) || a.HRME_EmployeeLastName.ToUpper().StartsWith(data.searchtext)))
                                               select new Clg_StudentFeeGroupMapping_DTO
                                               {
                                                   FMSTGH_Id = b.FMCSTGH_Id,
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
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && a.HRME_EmployeeCode.Contains(data.searchtext))
                                               select new Clg_StudentFeeGroupMapping_DTO
                                               {
                                                   FMSTGH_Id = b.FMCSTGH_Id,
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
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && e.HRMD_DepartmentName.Contains(data.searchtext))
                                               select new Clg_StudentFeeGroupMapping_DTO
                                               {
                                                   FMSTGH_Id = b.FMCSTGH_Id,
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
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && f.HRMDES_DesignationName.Contains(data.searchtext))
                                               select new Clg_StudentFeeGroupMapping_DTO
                                               {
                                                   FMSTGH_Id = b.FMCSTGH_Id,
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
                                               from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                               from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                               where (a.MI_Id == b.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == e.HRMD_Id && a.HRMDES_Id == f.HRMDES_Id && e.HRMD_ActiveFlag == true && f.HRMDES_ActiveFlag == true && d.MI_Id == a.MI_Id && d.FMG_ActiceFlag == true && d.FMG_Id == b.FMG_Id && d.FMG_GroupName.Contains(data.searchtext))
                                               select new Clg_StudentFeeGroupMapping_DTO
                                               {
                                                   FMSTGH_Id = b.FMCSTGH_Id,
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

        public Clg_StudentFeeGroupMapping_DTO searching_o(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                switch (data.searchType)
                {
                    case "0":
                        data.searchtext = data.searchtext.ToUpper();
                        data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                                     from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                     where (a.MI_Id == b.MI_Id && a.FMCOST_ActiveFlag == true && a.FMCOST_Id == b.FMCOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id && a.FMCOST_StudentName.ToUpper().Contains(data.searchtext))
                                                     select new Clg_StudentFeeGroupMapping_DTO
                                                     {
                                                         FMOSTGH_Id = b.FMCOSTGH_Id,
                                                         FMOST_Id = a.FMCOST_Id,
                                                         FMOST_StudentName = a.FMCOST_StudentName,
                                                         FMOST_StudentMobileNo = a.FMCOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = a.FMCOST_StudentEmailId,
                                                         FMG_Id = b.FMG_Id,
                                                         FMG_GroupName = c.FMG_GroupName
                                                     }).ToList().Distinct().ToArray();

                        break;
                    case "1":
                        data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                                     from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                     where (a.MI_Id == b.MI_Id && a.FMCOST_ActiveFlag == true && a.FMCOST_Id == b.FMCOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id && a.FMCOST_StudentMobileNo.ToString().Contains(data.searchtext))
                                                     select new Clg_StudentFeeGroupMapping_DTO
                                                     {
                                                         FMOSTGH_Id = b.FMCOSTGH_Id,
                                                         FMOST_Id = a.FMCOST_Id,
                                                         FMOST_StudentName = a.FMCOST_StudentName,
                                                         FMOST_StudentMobileNo = a.FMCOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = a.FMCOST_StudentEmailId,
                                                         FMG_Id = b.FMG_Id,
                                                         FMG_GroupName = c.FMG_GroupName
                                                     }).ToList().Distinct().ToArray();

                        break;
                    case "2":
                        data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                                     from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                     where (a.MI_Id == b.MI_Id && a.FMCOST_ActiveFlag == true && a.FMCOST_Id == b.FMCOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id && a.FMCOST_StudentEmailId.Contains(data.searchtext))
                                                     select new Clg_StudentFeeGroupMapping_DTO
                                                     {
                                                         FMOSTGH_Id = b.FMCOSTGH_Id,
                                                         FMOST_Id = a.FMCOST_Id,
                                                         FMOST_StudentName = a.FMCOST_StudentName,
                                                         FMOST_StudentMobileNo = a.FMCOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = a.FMCOST_StudentEmailId,
                                                         FMG_Id = b.FMG_Id,
                                                         FMG_GroupName = c.FMG_GroupName
                                                     }).ToList().Distinct().ToArray();

                        break;
                    case "3":
                        data.grid_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                                     from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                     where (a.MI_Id == b.MI_Id && a.FMCOST_ActiveFlag == true && a.FMCOST_Id == b.FMCOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && c.FMG_ActiceFlag == true && c.FMG_Id == b.FMG_Id && c.FMG_GroupName.Contains(data.searchtext))
                                                     select new Clg_StudentFeeGroupMapping_DTO
                                                     {
                                                         FMOSTGH_Id = b.FMCOSTGH_Id,
                                                         FMOST_Id = a.FMCOST_Id,
                                                         FMOST_StudentName = a.FMCOST_StudentName,
                                                         FMOST_StudentMobileNo = a.FMCOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = a.FMCOST_StudentEmailId,
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
        public Clg_StudentFeeGroupMapping_DTO editstudata(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                // data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                     from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                //                     from c in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                //                     from g in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                     where (a.FMSG_Id==c.FMSG_Id && a.FMG_Id==b.FMG_Id && c.FMH_ID==g.FMH_Id && c.FTI_ID==d.FTI_Id && a.AMST_Id==data.AMST_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id)
                //                     select new Clg_StudentFeeGroupMapping_DTO
                //                     {
                //                         AMST_Id = a.AMST_Id,
                //                         FMG_Id=a.FMG_Id,
                //                         FMG_GroupName = b.FMG_GroupName,
                //                         FMH_Id=g.FMH_Id,
                //                         FMH_FeeName=g.FMH_FeeName,
                //                         FMSG_Id = a.FMSG_Id,
                //                         FTI_Id=d.FTI_Id,
                //                         FTI_Name=d.FTI_Name
                //                     }
                //).Distinct().ToArray();
                //data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                     from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                //                     from c in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                //                     from g in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                     from e in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                //                     from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                     where (f.AMST_Id == a.AMST_Id && f.ASMAY_Id == a.ASMAY_Id && f.MI_Id == a.MI_Id && a.FMG_Id == f.FMG_Id && c.FMH_ID == f.FMH_Id && c.FTI_ID == f.FTI_Id && a.FMSG_Id == c.FMSG_Id && a.FMG_Id == b.FMG_Id && c.FMH_ID == g.FMH_Id && c.FTI_ID == d.FTI_Id && a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == a.FMG_Id && e.FMH_Id == c.FMH_ID && e.FMI_Id == d.FMI_Id)
                //                     select new Clg_StudentFeeGroupMapping_DTO
                //                     {
                //                         AMST_Id = a.AMST_Id,
                //                         FMG_Id = a.FMG_Id,
                //                         FMG_GroupName = b.FMG_GroupName,
                //                         FMH_Id = g.FMH_Id,
                //                         FMH_FeeName = g.FMH_FeeName,
                //                         FMSG_Id = a.FMSG_Id,
                //                         FTI_Id = d.FTI_Id,
                //                         FTI_Name = d.FTI_Name,
                //                         FSS_PaidAmount = f.FSS_PaidAmount,
                //                     }
                //).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO searchingstu(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                string str = "";
                switch (data.searchType)
                {
                    //case "0":



                    //    if (data.radioval != "BR")
                    //    {

                    //        data.searchtext = data.searchtext.ToUpper();

                    //        List<Clg_StudentFeeGroupMapping_DTO> result = new List<Clg_StudentFeeGroupMapping_DTO>();
                    //        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    //        {
                    //            cmd.CommandText = "FEE_STUDENTGROUP_NAME_SEARCH_BEFORE_new";
                    //            cmd.CommandType = CommandType.StoredProcedure;

                    //            cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                    //              SqlDbType.BigInt)
                    //            {
                    //                Value = data.MI_Id
                    //            });

                    //            cmd.Parameters.Add(new SqlParameter("@searchtext",
                    //                         SqlDbType.VarChar)
                    //            {
                    //                Value = data.searchtext
                    //            });


                    //            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                    //           SqlDbType.BigInt)
                    //            {
                    //                Value = data.ASMAY_Id
                    //            });


                    //            cmd.Parameters.Add(new SqlParameter("@type",
                    //              SqlDbType.VarChar)
                    //            {
                    //                Value = 1
                    //            });
                    //            cmd.Parameters.Add(new SqlParameter("@trmr_id",
                    //        SqlDbType.VarChar)
                    //            {
                    //                Value = 0
                    //            });

                    //            if (cmd.Connection.State != ConnectionState.Open)
                    //                cmd.Connection.Open();

                    //            var retObject = new List<dynamic>();

                    //            try
                    //            {
                    //                using (var dataReader = cmd.ExecuteReader())
                    //                {
                    //                    while (dataReader.Read())
                    //                    {
                    //                        result.Add(new Clg_StudentFeeGroupMapping_DTO
                    //                        {

                    //                            AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                    //                            AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                    //                            AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                    //                            AMST_LastName = dataReader["AMST_LastName"].ToString(),
                    //                            AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                    //                            AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString(),
                    //                            AMAY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"].ToString()),
                    //                            ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                    //                            ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),
                    //                            AMST_Mobile = Convert.ToInt64(dataReader["AMST_MobileNo"].ToString()),
                    //                        });
                    //                    }
                    //                }
                    //                data.alldata = result.ToArray();
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                throw ex;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        List<Clg_StudentFeeGroupMapping_DTO> result = new List<Clg_StudentFeeGroupMapping_DTO>();
                    //        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    //        {
                    //            cmd.CommandText = "FEE_STUDENTGROUP_NAME_SEARCH_BEFORE_new";
                    //            cmd.CommandType = CommandType.StoredProcedure;

                    //            cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                    //              SqlDbType.BigInt)
                    //            {
                    //                Value = data.MI_Id
                    //            });

                    //            cmd.Parameters.Add(new SqlParameter("@searchtext",
                    //                         SqlDbType.VarChar)
                    //            {
                    //                Value = data.searchtext
                    //            });


                    //            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                    //           SqlDbType.BigInt)
                    //            {
                    //                Value = data.ASMAY_Id
                    //            });

                    //            cmd.Parameters.Add(new SqlParameter("@type",
                    //         SqlDbType.VarChar)
                    //            {
                    //                Value = 2
                    //            });

                    //            cmd.Parameters.Add(new SqlParameter("@trmr_id",
                    //       SqlDbType.VarChar)
                    //            {
                    //                Value = data.TRMR_Id
                    //            });

                    //            if (cmd.Connection.State != ConnectionState.Open)
                    //                cmd.Connection.Open();

                    //            var retObject = new List<dynamic>();

                    //            try
                    //            {
                    //                using (var dataReader = cmd.ExecuteReader())
                    //                {
                    //                    while (dataReader.Read())
                    //                    {
                    //                        result.Add(new Clg_StudentFeeGroupMapping_DTO
                    //                        {

                    //                            AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                    //                            AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                    //                            AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                    //                            AMST_LastName = dataReader["AMST_LastName"].ToString(),
                    //                            AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                    //                            AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString(),
                    //                            AMAY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"].ToString()),
                    //                            ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                    //                            ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),
                    //                            AMST_Mobile = Convert.ToInt64(dataReader["AMST_MobileNo"].ToString()),

                    //                        });
                    //                    }
                    //                }
                    //                data.alldata = result.ToArray();
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                throw ex;
                    //            }
                    //        }
                    //    }

                    //    break;

                    //                  data.alldata = (from c in _YearlyFeeGroupMappingContext.School_M_Class
                    //                                       from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                    //                                       from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                    //                                       from g in _YearlyFeeGroupMappingContext.school_M_Section
                    //                                       where (d.ASMCL_Id==c.ASMCL_Id && d.ASMS_Id==g.ASMS_Id && e.AMST_SOL == "S" && d.AMST_Id == e.AMST_Id && e.AMST_ActiveFlag == 1 && e.MI_Id == data.MI_Id && d.AMAY_ActiveFlag == 1 && ((e.AMST_FirstName.Trim() + ' ' + e.AMST_MiddleName.Trim() + ' ' + e.AMST_LastName.Trim()).Contains(data.searchtext) || e.AMST_FirstName.StartsWith(data.searchtext) || e.AMST_MiddleName.StartsWith(data.searchtext) || e.AMST_LastName.StartsWith(data.searchtext)))
                    //                                       select new Clg_StudentFeeGroupMapping_DTO
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
                    //               case "1":
                    //                   data.alldata = (from c in _YearlyFeeGroupMappingContext.School_M_Class
                    //                                   from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                    //                                   from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                    //                                   from g in _YearlyFeeGroupMappingContext.school_M_Section
                    //                                   where (d.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == g.ASMS_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && c.ASMCL_ClassName.Contains(data.searchtext) && e.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                    //                                   select new Clg_StudentFeeGroupMapping_DTO
                    //                                   {
                    //                                       AMST_Id = d.AMST_Id,
                    //                                       AMST_FirstName = e.AMST_FirstName,
                    //                                       AMST_MiddleName = e.AMST_MiddleName,
                    //                                       AMST_LastName = e.AMST_LastName,
                    //                                       AMST_AdmNo = e.AMST_AdmNo,
                    //                                       AMST_RegistrationNo = e.AMST_RegistrationNo,
                    //                                       AMAY_RollNo = d.AMAY_RollNo,
                    //                                       ASMCL_ClassName = c.ASMCL_ClassName,
                    //                                       ASMC_SectionName = g.ASMC_SectionName,
                    //                                       AMST_Mobile = e.AMST_MobileNo,
                    //                                   }
                    // ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                    //                   break;
                    //               case "2":
                    //                   data.alldata = (from c in _YearlyFeeGroupMappingContext.School_M_Class
                    //                                   from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                    //                                   from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                    //                                   from g in _YearlyFeeGroupMappingContext.school_M_Section
                    //                                   where (d.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == g.ASMS_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && e.AMST_AdmNo.Contains(data.searchtext) && e.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                    //                                   select new Clg_StudentFeeGroupMapping_DTO
                    //                                   {
                    //                                       AMST_Id = d.AMST_Id,
                    //                                       AMST_FirstName = e.AMST_FirstName,
                    //                                       AMST_MiddleName = e.AMST_MiddleName,
                    //                                       AMST_LastName = e.AMST_LastName,
                    //                                       AMST_AdmNo = e.AMST_AdmNo,
                    //                                       AMST_RegistrationNo = e.AMST_RegistrationNo,
                    //                                       AMAY_RollNo = d.AMAY_RollNo,
                    //                                       ASMCL_ClassName = c.ASMCL_ClassName,
                    //                                       ASMC_SectionName = g.ASMC_SectionName,
                    //                                       AMST_Mobile = e.AMST_MobileNo,
                    //                                   }
                    //).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                    //                   break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO searchingstaff(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                data.saved_stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                        from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                            //from c in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHead_InstallmentsDMO
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

        public Clg_StudentFeeGroupMapping_DTO searchingothers(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                data.saved_oth_studentlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                              from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                                  //from c in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHead_InstallmentsDMO
                                              where (a.MI_Id == b.MI_Id && a.FMCOST_ActiveFlag == true && a.FMCOST_Id == b.FMCOST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                              select a.FMCOST_Id).ToList().Distinct().ToArray();
                switch (data.searchType)
                {
                    case "0":
                        data.oth_studentlist = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(s => s.MI_Id == data.MI_Id && s.FMCOST_ActiveFlag == true && s.FMCOST_StudentName.Contains(data.searchtext)).ToList().Distinct().OrderBy(t => t.FMCOST_StudentName).ToArray();

                        break;
                    case "1":

                        data.oth_studentlist = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(s => s.MI_Id == data.MI_Id && s.FMCOST_ActiveFlag == true && s.FMCOST_StudentMobileNo.ToString().Contains(data.searchtext)).ToList().Distinct().OrderBy(t => t.FMCOST_StudentMobileNo).ToArray();
                        break;
                    case "2":
                        data.oth_studentlist = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(s => s.MI_Id == data.MI_Id && s.FMCOST_ActiveFlag == true && s.FMCOST_StudentEmailId.Contains(data.searchtext)).ToList().Distinct().OrderBy(t => t.FMCOST_StudentEmailId).ToArray();

                        break;

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO getacademicyr(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
            try
            {
                pgmod.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from f in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                         from g in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                         where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == pgmod.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == pgmod.user_id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                         select new Clg_StudentFeeGroupMapping_DTO
                                         {
                                             FMG_Id = a.FMG_Id,
                                             FMG_GroupName = b.FMG_GroupName
                                         }).Distinct().ToArray();

                pgmod.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1")
                                        select new Clg_StudentFeeGroupMapping_DTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FMH_FeeName = c.FMH_FeeName
                                        }).Distinct().ToArray();

                pgmod.fillinstallment = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                         from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == pgmod.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == pgmod.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == pgmod.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id)
                                         select new Clg_StudentFeeGroupMapping_DTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                             FMH_Id = c.FMH_Id,
                                             FTI_Id = e.FTI_Id,
                                             FTI_Name = e.FTI_Name
                                         }).Distinct().ToArray();

                //        pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                         from c in _YearlyFeeGroupMappingContext.School_M_Class
                //                         from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                         where (d.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == pgmod.MI_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                //                         select new Clg_StudentFeeGroupMapping_DTO
                //                         {
                //                             AMST_Id = a.AMST_Id,
                //                             AMST_FirstName = a.AMST_FirstName,
                //                             AMST_MiddleName = a.AMST_MiddleName,
                //                             AMST_LastName = a.AMST_LastName,
                //                             AMST_AdmNo = a.AMST_AdmNo,
                //                             AMST_RegistrationNo = a.AMST_RegistrationNo,
                //                             AMAY_RollNo = d.AMAY_RollNo,
                //                             ASMCL_ClassName = c.ASMCL_ClassName

                //                         }
                //).Distinct().Take(10).ToArray(); /* .Take(5)*/

                //         var fetchmaxfmsgid = _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id).OrderByDescending(t => t.FMSG_Id).Take(5).Select(t => t.FMSG_Id).ToList();

                //         pgmod.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                //                               from c in _YearlyFeeGroupMappingContext.School_M_Class
                //                               from g in _YearlyFeeGroupMappingContext.school_M_Section
                //                               from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                               from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                               from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                               where (d.ASMS_Id == g.ASMS_Id && fetchmaxfmsgid.Contains(a.FMSG_Id) && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == pgmod.MI_Id && d.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                //                               select new Clg_StudentFeeGroupMapping_DTO
                //                               {
                //                                   AMST_Id = a.AMST_Id,
                //                                   AMST_FirstName = e.AMST_FirstName,
                //                                   AMST_MiddleName = e.AMST_MiddleName,
                //                                   AMST_LastName = e.AMST_LastName,
                //                                   AMST_AdmNo = e.AMST_AdmNo,
                //                                   AMST_RegistrationNo = e.AMST_RegistrationNo,
                //                                   AMAY_RollNo = d.AMAY_RollNo,
                //                                   FMG_GroupName = b.FMG_GroupName,
                //                                   ASMCL_ClassName = c.ASMCL_ClassName,
                //                                   ASMC_SectionName = g.ASMC_SectionName,
                //                                   AMST_Mobile = e.AMST_MobileNo,
                //                                   FMSG_Id = a.FMSG_Id,
                //                                   FMG_Id = b.FMG_Id

                //                               }
                //).Distinct().OrderBy(t => t.FMSG_Id).ToArray();

                pgmod.configsetting = _YearlyFeeGroupMappingContext.feemastersettings.Where(s => s.MI_Id == pgmod.MI_Id && s.userid == pgmod.user_id).ToList().Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pgmod;
        }



        public Clg_StudentFeeGroupMapping_DTO geteditdatastaffothers(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                if (data.radioval == "Staff")
                {
                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHead_InstallmentsDMO
                                         from g in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from e in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from f in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                         where (f.HRME_Id == a.HRME_Id && f.ASMAY_Id == a.ASMAY_Id && f.MI_Id == a.MI_Id && a.FMG_Id == f.FMG_Id && c.FMH_Id == f.FMH_Id && c.FTI_Id == f.FTI_Id && a.FMCSTGH_Id == c.FMCSTGH_Id && a.FMG_Id == b.FMG_Id && c.FMH_Id == g.FMH_Id && c.FTI_Id == d.FTI_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == a.FMG_Id && e.FMH_Id == c.FMH_Id && e.FMI_Id == d.FMI_Id)
                                         select new Clg_StudentFeeGroupMapping_DTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             FMG_Id = a.FMG_Id,
                                             FMG_GroupName = b.FMG_GroupName,
                                             FMH_Id = g.FMH_Id,
                                             FMH_FeeName = g.FMH_FeeName,
                                             FMSTGH_Id = a.FMCSTGH_Id,
                                             FTI_Id = d.FTI_Id,
                                             FTI_Name = d.FTI_Name,
                                             FSS_PaidAmount = f.FCSSST_PaidAmount,
                                         }
                ).Distinct().ToArray();
                }
                else if (data.radioval == "Others")
                {
                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GH_InstlDMO
                                         from g in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from e in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from f in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                         where (f.FMCOST_Id == a.FMCOST_Id && f.ASMAY_Id == a.ASMAY_Id && f.MI_Id == a.MI_Id && a.FMG_Id == f.FMG_Id && c.FMH_Id == f.FMH_Id && c.FTI_Id == f.FTI_Id && a.FMCOSTGH_Id == c.FMCOSTGH_Id && a.FMG_Id == b.FMG_Id && c.FMH_Id == g.FMH_Id && c.FTI_Id == d.FTI_Id && a.FMCOST_Id == data.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == a.FMG_Id && e.FMH_Id == c.FMH_Id && e.FMI_Id == d.FMI_Id)
                                         select new Clg_StudentFeeGroupMapping_DTO
                                         {
                                             FMOST_Id = a.FMCOST_Id,
                                             FMG_Id = a.FMG_Id,
                                             FMG_GroupName = b.FMG_GroupName,
                                             FMH_Id = g.FMH_Id,
                                             FMH_FeeName = g.FMH_FeeName,
                                             FMSG_Id = a.FMCOSTGH_Id,
                                             FTI_Id = d.FTI_Id,
                                             FTI_Name = d.FTI_Name,
                                             FSS_PaidAmount = f.FCSSOST_PaidAmount,
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

        public Clg_StudentFeeGroupMapping_DTO saveeditdataothers(Clg_StudentFeeGroupMapping_DTO pgmod)
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
                                                var checkforduplicates1 = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                                                           from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GH_InstlDMO
                                                                           from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                                                           where (a.FMCOSTGH_Id == b.FMCOSTGH_Id && a.FMG_Id == c.FMG_Id && a.FMCOST_Id == c.FMCOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.FMCOST_Id == pgmod.FMOST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                           select b.FMCOSTGHI_Id).Distinct().ToList();
                                                if (checkforduplicates1.Count().Equals(0))
                                                {
                                                    var FMAlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                                                   from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                                   where (a.FTI_Id == b.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMG_Id == pgmod.saveftilst[I].FMG_Id && a.FMH_Id == pgmod.saveftilst[I].FMH_Id && a.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.MI_Id == a.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == a.FMH_Id && a.FMCAOST_OthStaffFlag == "O" && ((a.FMCAOST_Amount >= 0 && c.FMH_Flag != "F" && c.FMH_Flag != "E") || (a.FMCAOST_Amount >= 0 && (c.FMH_Flag == "F" || c.FMH_Flag == "E"))))/* && a.FMA_Amount > 0*/
                                                                   select a.FMCAOST_Id).Distinct().ToList();

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
                                                var checkforduplicatesdel = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GHDMO
                                                                             from b in _YearlyFeeGroupMappingContext.Fee_Master_College_OthStudents_GH_InstlDMO
                                                                             from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                                                             where (a.FMCOSTGH_Id == b.FMCOSTGH_Id && a.FMG_Id == c.FMG_Id && a.FMCOST_Id == c.FMCOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.FMCOST_Id == pgmod.HRME_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.FCSSOST_PaidAmount == 0 && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                             select new { a.ASMAY_Id, a.MI_Id, a.FMG_Id, a.FMCOST_Id, a.FMCOSTGH_Id, b.FMH_Id, b.FTI_Id }).Distinct().ToList();
                                                if (checkforduplicatesdel.Count > 0)
                                                {

                                                    foreach (var a in checkforduplicatesdel)
                                                    {
                                                        using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                                                        {

                                                            var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMappingnew_others @p0,@p1,@p2,@p3,@p4,@p5,@p6", a.MI_Id, a.FMCOST_Id, a.ASMAY_Id, a.FMG_Id, a.FMCOSTGH_Id, a.FMH_Id, a.FTI_Id);

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

        public Clg_StudentFeeGroupMapping_DTO saveeditdatastaff(Clg_StudentFeeGroupMapping_DTO pgmod)
        {

            try
            {
                string returntxt = "";
                if (pgmod.HRME_Id != 0)
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
                                                var checkforduplicates1 = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                                                           from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHead_InstallmentsDMO
                                                                           from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                                                           where (a.FMCSTGH_Id == b.FMCSTGH_Id && a.FMG_Id == c.FMG_Id && a.HRME_Id == c.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.HRME_Id == pgmod.HRME_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                           select b.FMCSTGHI_Id).Distinct().ToList();
                                                if (checkforduplicates1.Count().Equals(0))
                                                {
                                                    var FMAlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                                                   from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                                   where (a.FTI_Id == b.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMG_Id == pgmod.saveftilst[I].FMG_Id && a.FMH_Id == pgmod.saveftilst[I].FMH_Id && a.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.MI_Id == a.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == a.FMH_Id && a.FMCAOST_OthStaffFlag == "S" && ((a.FMCAOST_Amount >= 0 && c.FMH_Flag != "F" && c.FMH_Flag != "E") || (a.FMCAOST_Amount >= 0 && (c.FMH_Flag == "F" || c.FMH_Flag == "E"))))/* && a.FMA_Amount > 0*/
                                                                   select a.FMCAOST_Id).Distinct().ToList();

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
                                                var checkforduplicatesdel = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHeadDMO
                                                                             from b in _YearlyFeeGroupMappingContext.Fee_Master_College_Staff_GroupHead_InstallmentsDMO
                                                                             from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                                                             where (a.FMCSTGH_Id == b.FMCSTGH_Id && a.FMG_Id == c.FMG_Id && a.HRME_Id == c.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.HRME_Id == pgmod.HRME_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.FCSSST_PaidAmount == 0 && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                             select new { a.ASMAY_Id, a.MI_Id, a.FMG_Id, a.HRME_Id, a.FMCSTGH_Id, b.FMH_Id, b.FTI_Id }).Distinct().ToList();
                                                if (checkforduplicatesdel.Count > 0)
                                                {

                                                    foreach (var a in checkforduplicatesdel)
                                                    {
                                                        using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                                                        {

                                                            var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMappingnew_staff @p0,@p1,@p2,@p3,@p4,@p5,@p6", a.MI_Id, a.HRME_Id, a.ASMAY_Id, a.FMG_Id, a.FMCSTGH_Id, a.FMH_Id, a.FTI_Id);

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
