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

namespace FeeServiceHub.com.vaps.services
{
    public class StaffFeeGroupMappingImpl : interfaces.StaffFeeGroupMappingInterface
    {
        private static ConcurrentDictionary<string, FeeStudentGroupMappingDTO> _login =
          new ConcurrentDictionary<string, FeeStudentGroupMappingDTO>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<StaffFeeGroupMappingImpl> _logger;

        public StaffFeeGroupMappingImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<StaffFeeGroupMappingImpl> log)
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

        public FeeStudentGroupMappingDTO EditMasterscetionDetails(int id)
        {
            throw new NotImplementedException();
        }

        public FeeStudentGroupMappingDTO getdata(FeeStudentGroupMappingDTO fee)
        {
            try
            {
                fee.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                       from b in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMG_Id == b.FMG_ID && a.MI_Id == fee.MI_Id && b.User_Id == fee.user_id && a.FMG_ActiceFlag == true)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = a.FMG_Id,
                                           FMG_GroupName = a.FMG_GroupName
                                       }
    ).Distinct().ToArray();

                fee.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                      from b in _YearlyFeeGroupMappingContext.feeGroup
                                      from c in _YearlyFeeGroupMappingContext.feehead
                                      where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id && c.FMH_ActiveFlag == true)
                                      select new FeeStudentGroupMappingDTO
                                      {
                                          FMG_Id = b.FMG_Id,
                                          FMH_Id = c.FMH_Id,
                                          FMH_FeeName = c.FMH_FeeName
                                      }
       ).ToArray();


                fee.fillinstallment = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                       from b in _YearlyFeeGroupMappingContext.feeGroup
                                       from c in _YearlyFeeGroupMappingContext.feehead
                                       from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                       from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.FMI_Id == d.FMI_Id && d.FMI_Id == e.FMI_Id && b.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FTI_Id = e.FTI_Id,
                                           FTI_Name = e.FTI_Name
                                       }
      ).ToArray();

                fee.alldata = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                    from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                    from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                    where (a.HRME_ActiveFlag == true && a.MI_Id==fee.MI_Id && a.HRMD_Id==b.HRMD_Id && a.HRMDES_Id==c.HRMDES_Id)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                        HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                        HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                        HRMD_DepartmentName = b.HRMD_DepartmentName,
                                        HRMDES_DesignationName=c.HRMDES_DesignationName,
                                        HRMD_Id = a.HRMD_Id,
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeCode = a.HRME_EmployeeCode
                                    }
     ).Distinct().OrderBy(t => t.HRMD_Id).ToArray();


                var fetchmaxfmsgid = _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead.Where(t => t.MI_Id == fee.MI_Id && t.ASMAY_Id == fee.ASMAY_Id).OrderByDescending(t => t.FMSTGH_Id).Take(5).Select(t => t.FMSTGH_Id).ToList();

                fee.alldatathird = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.HR_Master_Department
                                    from g in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                    from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                    from f in _YearlyFeeGroupMappingContext.Fee_Staff_Status
                                    where (d.HRMDES_Id == g.HRMDES_Id && fetchmaxfmsgid.Contains(a.FMSTGH_Id) && a.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == b.MI_Id && a.HRME_Id == d.HRME_Id && a.FMG_Id == b.FMG_Id && d.HRMD_Id == c.HRMD_Id && a.HRME_Id == d.HRME_Id && d.HRME_ActiveFlag == true && f.HRME_ID==a.HRME_Id && f.FMG_Id==a.FMG_Id)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.HRME_Id,
                                        HRME_EmployeeFirstName = d.HRME_EmployeeFirstName,
                                        HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                        HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                        HRME_EmployeeCode = d.HRME_EmployeeCode,
                                        HRMD_DepartmentName = c.HRMD_DepartmentName,
                                        HRMDES_DesignationName = g.HRMDES_DesignationName,
                                        FMG_GroupName = b.FMG_GroupName,
                                        HRME_MobileNo = d.HRME_MobileNo,
                                        FMSG_Id = a.FMSTGH_Id,
                                        FMG_Id = b.FMG_Id
                                    }
       ).Distinct().OrderBy(t => t.HRMD_Id).ToArray();

                fee.alldatathirdall = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                       from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                       where (a.HRME_Id == d.HRME_Id && d.HRME_Id == d.HRME_Id && a.MI_Id == fee.MI_Id && a.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == fee.ASMAY_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           HRME_Id = d.HRME_Id,
                                           HRME_EmployeeFirstName = d.HRME_EmployeeFirstName,
                                           HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                           HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                           HRME_MobileNo = d.HRME_MobileNo,
                                           FMSG_Id = a.FMSTGH_Id,
                                       }
       ).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

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
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
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
                    fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                   from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
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
                else if (data.radioval == "alldata" && data.classwisecheckboxvalue == true)
                {
                    fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                   from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
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



                fee.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.School_M_Class
                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    where (a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && d.AMST_Id == e.AMST_Id && c.ASMCL_Id == data.ASMCL_Id && e.AMST_SOL == "S" && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
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
                                        ASMCL_ClassName = c.ASMCL_ClassName

                                    }
  ).ToArray();

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
                FeeStudentGroupMappingDMO pgmodule = Mapper.Map<FeeStudentGroupMappingDMO>(pgmod);

                if (feestumap.FMSG_Id > 0)
                {
                    if (pgmod.studentdata != null)
                    {
                        int j = 0, G = 0, H = 0, I = 0;

                        while (j < pgmod.studentdata.Count())
                        {
                            if (pgmod.studentdata[j].staffchecked == true)
                            {
                                Fee_Master_Staff_GroupHead pmm = new Fee_Master_Staff_GroupHead();
                                Fee_Master_Staff_GroupHead_Installments fsgim = new Fee_Master_Staff_GroupHead_Installments();
                                pmm.HRME_Id = pgmod.studentdata[j].HRME_Id;
                                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                                {
                                    pmm.MI_Id = pgmod.MI_Id;
                                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                    //pmm.FMSTGH_ActiveFlag = "Y";
                                    pmm.FMSTGH_Id = 0;
                                    while (G < pgmod.savegrplst.Count())
                                    {
                                        if (pgmod.savegrplst[G].checkedgrplst == true)
                                        {
                                            pmm.FMG_Id = pgmod.savegrplst[G].FMG_Id;

                                            // _YearlyFeeGroupMappingContext.Add(pmm);
                                            //_YearlyFeeGroupMappingContext.SaveChanges();
                                            var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Master_Staff_Group @p0,@p1,@p2,@p3,@p4", pmm.MI_Id, pmm.HRME_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMSTGH_ActiveFlag);

                                            while (H < pgmod.saveheadlst.Count())
                                            {
                                                if (pgmod.saveheadlst[H].checkedheadlst == true)
                                                {
                                                    fsgim.FMH_Id = pgmod.saveheadlst[H].FMH_Id;
                                                    //_YearlyFeeGroupMappingContext.Add(fsgim);
                                                    //_YearlyFeeGroupMappingContext.SaveChanges();

                                                    while (I < pgmod.saveftilst.Count())
                                                    {
                                                        if (pgmod.saveftilst[I].checkedinstallmentlst == true)
                                                        {
                                                            fsgim.FTI_Id = pgmod.saveftilst[I].FTI_Id;

                                                            fsgim.FMSTGH_Id = pmm.FMSTGH_Id;
                                                            // _YearlyFeeGroupMappingContext.Add(fsgim);
                                                            // _YearlyFeeGroupMappingContext.SaveChanges();

                                                            //procedure
                                                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                                            {
                                                                cmd.CommandText = "Insert_Fee_Staff_Map";
                                                                cmd.CommandType = CommandType.StoredProcedure;
                                                                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                                                    SqlDbType.BigInt)
                                                                {
                                                                    Value = pmm.FMG_Id
                                                                });
                                                                cmd.Parameters.Add(new SqlParameter("@hrme_id",
                                                                   SqlDbType.BigInt)
                                                                {
                                                                    Value = pmm.HRME_Id
                                                                });
                                                                cmd.Parameters.Add(new SqlParameter("@MI_ID",
                                                               SqlDbType.BigInt)
                                                                {
                                                                    Value = pmm.MI_Id
                                                                });

                                                                cmd.Parameters.Add(new SqlParameter("@fti_id_new",
                                                            SqlDbType.BigInt)
                                                                {
                                                                    Value = fsgim.FTI_Id
                                                                });

                                                                cmd.Parameters.Add(new SqlParameter("@FMSG_Id",
                                                                  SqlDbType.BigInt)
                                                                {
                                                                    Value = fsgim.FMSTGH_Id
                                                                });

                                                                cmd.Parameters.Add(new SqlParameter("@FMH_ID_new",
                                                                  SqlDbType.BigInt)
                                                                {
                                                                    Value = fsgim.FMH_Id
                                                                });

                                                                cmd.Parameters.Add(new SqlParameter("@userid",
                                                              SqlDbType.BigInt)
                                                                {
                                                                    Value = pgmod.user_id
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
                                                            //procedure
                                                            fsgim.FMSTGHI_Id = 0;
                                                        }
                                                        I++;
                                                    }
                                                }
                                                H++;
                                            }
                                        }
                                        G++;
                                    }
                                }
                            }
                            j++;
                        }
                    }
                }
                else
                {
                    if (pgmod.studentdata != null)
                    {
                        int j = 0, G = 0, H = 0, I = 0;

                        while (j < pgmod.studentdata.Count())
                        {
                            if (pgmod.studentdata[j].staffchecked == true)
                            {
                                Fee_Master_Staff_GroupHead pmm = new Fee_Master_Staff_GroupHead();
                                Fee_Master_Staff_GroupHead_Installments fsgim = new Fee_Master_Staff_GroupHead_Installments();
                                pmm.HRME_Id = pgmod.studentdata[j].HRME_Id;
                                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                                {
                                    pmm.MI_Id = pgmod.MI_Id;
                                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                    //pmm.FMSTGH_ActiveFlag = "Y";
                                    pmm.FMSTGH_Id = 0;
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
                                                        var checkforduplicates1 = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead
                                                                                   from b in _YearlyFeeGroupMappingContext.Fee_Master_Staff_GroupHead_Installments
                                                                                   from c in _YearlyFeeGroupMappingContext.Fee_Staff_Status
                                                                                   where (a.FMSTGH_Id == b.FMSTGH_Id && a.FMG_Id == c.FMG_Id && a.HRME_Id == c.HRME_ID && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.HRME_ID == pmm.HRME_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id)
                                                                                   select b.FMSTGHI_Id).Distinct().ToList();
                                                        if (checkforduplicates1.Count().Equals(0))
                                                        {
                                                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                                            {
                                                                cmd.CommandText = "Insert_Fee_Staff_Map_New";
                                                                cmd.CommandType = CommandType.StoredProcedure;
                                                                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                                                    SqlDbType.BigInt)
                                                                {
                                                                    Value = pgmod.saveftilst[I].FMG_Id
                                                                });
                                                                cmd.Parameters.Add(new SqlParameter("@hrme_id",
                                                                   SqlDbType.BigInt)
                                                                {
                                                                    Value = pmm.HRME_Id
                                                                });
                                                                cmd.Parameters.Add(new SqlParameter("@MI_ID",
                                                               SqlDbType.BigInt)
                                                                {
                                                                    Value = pmm.MI_Id
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

                            I = 0;
                            H = 0;
                            G = 0;

                            j++;
                        }
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

                if (pgmod.AMST_Id != 0)
                {
                    int G = 0, H = 0, I = 0;

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
                                                                       where (a.FMSG_Id == b.FMSG_Id && a.FMG_Id == c.FMG_Id && a.AMST_Id == c.AMST_Id && b.FMH_ID == c.FMH_Id && b.FTI_ID == c.FTI_Id && c.AMST_Id == pgmod.AMST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id)
                                                                       select b.FMSGI_Id).Distinct().ToList();
                                            if (checkforduplicates1.Count().Equals(0))
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
                                        else
                                        {
                                            var checkforduplicatesdel = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                                                         from b in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                                                                         from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                                         where (a.FMSG_Id == b.FMSG_Id && a.FMG_Id == c.FMG_Id && a.AMST_Id == c.AMST_Id && b.FMH_ID == c.FMH_Id && b.FTI_ID == c.FTI_Id && c.AMST_Id == pgmod.AMST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && c.FSS_PaidAmount == 0)
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
                //List<BusRouteDMO> busroute = new List<BusRouteDMO>();
                //busroute = _YearlyFeeGroupMappingContext.busRouteDMO.ToList();
                //data.fillbusroutedet = busroute.ToArray();
            }
            else if (data.radioval == "Classwise")
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillmasterclass = classlist.ToArray();
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
            }
            else if (data.radioval == "NewStude")
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.fillmasterclass = classlist.ToArray();
            }
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
                                                 where (a.AMST_Id == b.AMST_Id && d.ASMAY_Id == a.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && e.ASMCL_Id == a.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.AMC_Id == data.AMC_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                                 select new FeeStudentGroupMappingDTO
                                                 {
                                                     AMST_Id = a.AMST_Id,
                                                     AMST_FirstName = b.AMST_FirstName,
                                                     AMST_MiddleName = b.AMST_MiddleName,
                                                     AMST_LastName = b.AMST_LastName,
                                                     AMST_AdmNo = b.AMST_AdmNo,
                                                     AMST_RegistrationNo = b.AMST_RegistrationNo,
                                                     AMAY_RollNo = a.AMAY_RollNo,
                                                     ASMCL_ClassName = c.ASMCL_ClassName
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
                                                 where (a.FMCC_Id == data.FMCC_Id && f.ASMAY_Id == e.ASMAY_Id && f.AMST_Id == g.AMST_Id && d.ASMCL_Id == f.ASMCL_Id && c.ASMCL_Id == f.ASMCL_Id && c.FYCC_Id == b.FYCC_Id && b.FMCC_Id == a.FMCC_Id && f.ASMAY_Id == data.ASMAY_Id && g.MI_Id == data.MI_Id && g.AMST_SOL == "S" && g.AMST_ActiveFlag == 1 && f.AMAY_ActiveFlag == 1)
                                                 select new FeeStudentGroupMappingDTO
                                                 {
                                                     AMST_Id = g.AMST_Id,
                                                     AMST_FirstName = g.AMST_FirstName,
                                                     AMST_MiddleName = g.AMST_MiddleName,
                                                     AMST_LastName = g.AMST_LastName,
                                                     AMST_AdmNo = g.AMST_AdmNo,
                                                     AMST_RegistrationNo = g.AMST_RegistrationNo,
                                                     AMAY_RollNo = f.AMAY_RollNo,
                                                     ASMCL_ClassName = d.ASMCL_ClassName
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
                                                 where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
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
     ).OrderBy(t => t.AMST_FirstName).Take(5).ToArray();

                }
                else if (data.radioval == "NewStude")
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
                                                 where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
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
      ).OrderBy(t => t.AMST_FirstName).Take(5).ToArray();

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
                        string str = "";
                        data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from c in _YearlyFeeGroupMappingContext.School_M_Class
                                             from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                             from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                             from g in _YearlyFeeGroupMappingContext.school_M_Section
                                             where (g.ASMS_Id == d.ASMS_Id && f.ASMAY_Id == d.ASMAY_Id && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (((e.AMST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(e.AMST_MiddleName.Trim()) == true ? str : e.AMST_MiddleName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(e.AMST_LastName.Trim()) == true ? str : e.AMST_LastName.Trim())).Trim().Contains(data.searchtext) || e.AMST_FirstName.StartsWith(data.searchtext) || e.AMST_MiddleName.StartsWith(data.searchtext) || e.AMST_LastName.StartsWith(data.searchtext)))
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

        public FeeStudentGroupMappingDTO editstudata(FeeStudentGroupMappingDTO data)
        {
            try
            {
                // data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                //                     from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                //                     from c in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                //                     from g in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                     where (a.FMSG_Id==c.FMSG_Id && a.FMG_Id==b.FMG_Id && c.FMH_ID==g.FMH_Id && c.FTI_ID==d.FTI_Id && a.AMST_Id==data.AMST_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id)
                //                     select new FeeStudentGroupMappingDTO
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
                switch (data.searchType)
                {
                    case "0":
                        data.alldata = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                        from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                        from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                        where (a.MI_Id==data.MI_Id &&a.HRME_ActiveFlag==true && a.HRMD_Id==b.HRMD_Id && a.HRMDES_Id==c.HRMDES_Id && ((a.HRME_EmployeeFirstName.Trim() + ' ' + a.HRME_EmployeeMiddleName.Trim() + ' ' + a.HRME_EmployeeLastName.Trim()).Contains(data.searchtext) || a.HRME_EmployeeFirstName.StartsWith(data.searchtext) || a.HRME_EmployeeMiddleName.StartsWith(data.searchtext) || a.HRME_EmployeeLastName.StartsWith(data.searchtext)))
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                            HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                            HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                            HRMD_DepartmentName = b.HRMD_DepartmentName,
                                            HRMDES_DesignationName = c.HRMDES_DesignationName,
                                            HRMD_Id = a.HRMD_Id,
                                            HRME_Id = a.HRME_Id,
                                            HRME_EmployeeCode = a.HRME_EmployeeCode
                                        }
      ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        break;
                    case "1":
                        data.alldata = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                        from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                        from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_DepartmentName.Contains(data.searchtext))
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                            HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                            HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                            HRMD_DepartmentName = b.HRMD_DepartmentName,
                                            HRMDES_DesignationName = c.HRMDES_DesignationName,
                                            HRMD_Id = a.HRMD_Id,
                                            HRME_Id = a.HRME_Id,
                                            HRME_EmployeeCode = a.HRME_EmployeeCode
                                        }
      ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        break;
                    case "2":
                        data.alldata = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                        from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                        from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && c.HRMDES_DesignationName.Contains(data.searchtext))
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                            HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                            HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                            HRMD_DepartmentName = b.HRMD_DepartmentName,
                                            HRMDES_DesignationName = c.HRMDES_DesignationName,
                                            HRMD_Id = a.HRMD_Id,
                                            HRME_Id = a.HRME_Id,
                                            HRME_EmployeeCode = a.HRME_EmployeeCode
                                        }
     ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                        break;
                    case "3":
                        data.alldata = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                        from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                        from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_EmployeeCode.Contains(data.searchtext))
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                            HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                            HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                            HRMD_DepartmentName = b.HRMD_DepartmentName,
                                            HRMDES_DesignationName = c.HRMDES_DesignationName,
                                            HRMD_Id = a.HRMD_Id,
                                            HRME_Id = a.HRME_Id,
                                            HRME_EmployeeCode = a.HRME_EmployeeCode
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

    }
}
