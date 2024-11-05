using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.College.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using Microsoft.SqlServer.Server;
using DomainModel.Model.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class fromtabledata
    {
        public long From_FMG_id { get; set; }
        public long From_FMH_id { get; set; }
        public long From_FTI_id { get; set; }
        public long From_FMA_id { get; set; }
        public long RunningExcessAmount { get; set; }
    }
    public class totabledata
    {
        public long To_FMG_id { get; set; }
        public long To_FMH_id { get; set; }
        public long To_FTI_id { get; set; }
        public long To_FMA_id { get; set; }
        public long adjustedamount { get; set; }
    }
    public class fromCollection : List<fromtabledata>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                  new SqlMetaData("From_FMG_id", SqlDbType.BigInt),
                  new SqlMetaData("From_FMH_id", SqlDbType.BigInt),
                  new SqlMetaData("From_FTI_id", SqlDbType.BigInt),
                  new SqlMetaData("From_FMA_id", SqlDbType.BigInt),
                  new SqlMetaData("RunningExcessAmount", SqlDbType.BigInt));
            foreach (fromtabledata from in this)
            {
                
                sqlRow.SetSqlInt64(0, from.From_FMG_id);
                sqlRow.SetSqlInt64(1, from.From_FMH_id);
                sqlRow.SetSqlInt64(2, from.From_FTI_id);
                sqlRow.SetSqlInt64(3, from.From_FMA_id);
                sqlRow.SetSqlInt64(4, from.RunningExcessAmount);
                yield return sqlRow;
            }
        }
    }
    public class toCollection : List<totabledata>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                  new SqlMetaData("To_FMG_id", SqlDbType.BigInt),
                  new SqlMetaData("To_FMH_id", SqlDbType.BigInt),
                  new SqlMetaData("To_FTI_id", SqlDbType.BigInt),
                  new SqlMetaData("To_FMA_id", SqlDbType.BigInt),
                  new SqlMetaData("adjustedamount", SqlDbType.BigInt));
            foreach (totabledata to in this)
            {

                sqlRow.SetSqlInt64(0, to.To_FMG_id);
                sqlRow.SetSqlInt64(1, to.To_FMH_id);
                sqlRow.SetSqlInt64(2, to.To_FTI_id);
                sqlRow.SetSqlInt64(3, to.To_FMA_id);
                sqlRow.SetSqlInt64(4, to.adjustedamount);
                yield return sqlRow;
            }
        }
    }
    public class CLGFeeAdjustmentImpl : CLGFeeAdjustmentInterface
    {
       
        private static ConcurrentDictionary<string, CLGFeeAdjustmentDTO> _login =
        new ConcurrentDictionary<string, CLGFeeAdjustmentDTO>();

        public CollFeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _context;
       // readonly ILogger<FeeAdjustmentImpl> _logger;
        public CLGFeeAdjustmentImpl(CollFeeGroupContext FeeGroupContext, DomainModelMsSqlServerContext context)
        {
            _FeeGroupContext = FeeGroupContext;
            _context = context;
           // _logger = log;
        }
        


        public CLGFeeAdjustmentDTO getdata(CLGFeeAdjustmentDTO data)
        {
            try
            {
                //List<MasterAcademic> allyear = new List<MasterAcademic>();
                ////allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();
                ////data.fillyear = allyear.ToArray();
                //List<MasterAcademic> allacademic = new List<MasterAcademic>();
                //allacademic = _FeeGroupContext.AcademicYear.Where(d => d.Is_Active == true && d.MI_Id == data.MI_Id).ToList();
                //data.fillyear = allacademic.ToArray();

                //data.currentYear = _FeeGroupContext.AcademicYear.Where(d => d.Is_Active == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToArray();
                //var result = _FeeGroupContext.AcademicYear.First(d => d.Is_Active == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id);
                //data.ASMAY_Id = result.ASMAY_Id;

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.fillyear = year.Distinct().ToArray();



                //data.fillclass = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                  from b in _FeeGroupContext.School_M_Class
                //                  where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                //                  select new CLGFeeAdjustmentDTO
                //                  {
                //                      ASMCL_Id = a.ASMCL_Id,
                //                      ASMCL_ClassName = b.ASMCL_ClassName,
                //                  }).Distinct().OrderBy(t=>t.ASMCL_Id).ToArray();
                //data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                    from b in _FeeGroupContext.school_M_Section
                //                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                //                    select new CLGFeeAdjustmentDTO
                //                    {
                //                        ASMS_Id = a.ASMS_Id,
                //                        ASMC_SectionName = b.ASMC_SectionName,
                //                    }).Distinct().OrderBy(t=>t.ASMS_Id).ToArray();

                data.filldata = (from a in _FeeGroupContext.Fee_College_Student_AdjustmentDMO
                                 from b in _FeeGroupContext.FeeHeadClgDMO
                                 from c in _FeeGroupContext.FeeHeadClgDMO
                                 from d in _FeeGroupContext.Adm_Master_College_StudentDMO
                                 from e in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                 from f in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                 where (a.FCSA_From_FMH_Id == b.FMH_Id && a.FCSA_To_FMH_Id == c.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FCSA_FromFTI_Id == e.FTI_Id && a.FCSA_ToFTI_Id == f.FTI_Id && a.User_Id == data.userid)

                                 select new CLGFeeAdjustmentDTO
                                 {
                                     FSA_Id = a.FCSA_Id,
                                     AMST_FirstName = d.AMCST_FirstName,
                                     AMST_MiddleName = d.AMCST_MiddleName,
                                     AMST_LastName = d.AMCST_LastName,
                                     FMH_FeeNameF = b.FMH_FeeName,
                                     FMH_FeeNameT = c.FMH_FeeName,
                                     FTI_NameF = e.FTI_Name,
                                     FTI_NameT = f.FTI_Name,
                                     FSA_AdjustedAmount = a.FCSA_AdjustedAmount,
                                     FSA_Date = a.FCSA_Date,

                                 }).Distinct().OrderBy(t => t.FSA_Id).ToList().ToArray();
                //data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                //                    select new CLGFeeAdjustmentDTO
                //                    {
                //                        AMST_Id = a.AMST_Id,
                //                        AMST_FirstName = a.AMST_FirstName,
                //                        AMST_MiddleName = a.AMST_MiddleName,
                //                        AMST_LastName = a.AMST_LastName

                //                    }
                //                    ).Distinct().ToArray();

                //var resulta = _FeeGroupContext.AdmissionStudentDMO.FirstOrDefault(b => b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id);
                //data.AMST_Id = resulta.AMST_Id;

                //data.fillfromgroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                //                      from b in _FeeGroupContext.FeeGroupDMO
                //                      where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.FMG_ActiceFlag == true && a.FSS_RunningExcessAmount > 0)
                //                      select new CLGFeeAdjustmentDTO
                //                      {
                //                          FSA_From_FMG_Id = b.FMG_Id,
                //                          FMG_GroupNameF = b.FMG_GroupName,
                //                      }
                //                ).Distinct().ToArray();
                //data.filltogroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                //                    from b in _FeeGroupContext.FeeGroupDMO
                //                    where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.FMG_ActiceFlag == true && a.FSS_RunningExcessAmount == 0 && a.FSS_ToBePaid > 0)
                //                    select new CLGFeeAdjustmentDTO
                //                    {
                //                        FSA_To_FMG_Id = b.FMG_Id,
                //                        FMG_GroupNameT = b.FMG_GroupName,
                //                    }
                //              ).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAdjustmentDTO getdataclassdet(CLGFeeAdjustmentDTO data)
        {
            try
            {
                //data.fillclass = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                 from b in _FeeGroupContext.School_M_Class
                //                 where (a.ASMCL_Id==b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_ActiveFlag == true)
                //                 select new CLGFeeAdjustmentDTO
                //                 {
                //                     ASMCL_Id = a.ASMCL_Id,
                //                     ASMCL_ClassName = b.ASMCL_ClassName,
                //                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                data.courselist = (from a in _FeeGroupContext.MasterCourseDMO
                                   from b in _FeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGFeeAdjustmentDTO getdatasectiondet(CLGFeeAdjustmentDTO data)
        {
            try
            {

                //data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                    from b in _FeeGroupContext.school_M_Section
                //                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.ASMC_ActiveFlag == 1)
                //                    select new CLGFeeAdjustmentDTO
                //                    {
                //                        ASMS_Id = a.ASMS_Id,
                //                        ASMC_SectionName = b.ASMC_SectionName,
                //                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                //}
                var branchlist = (from a in _FeeGroupContext.ClgMasterBranchDMO
                                  from b in _FeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _FeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CLGFeeAdjustmentDTO getdatastudentdet(CLGFeeAdjustmentDTO data)
        {
            try
            {
                //// if (data.ASMCL_Id == 0 && data.ASMS_Id == 0)                    
                ////{
                //    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                        where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                //                        select new CLGFeeAdjustmentDTO
                //                        {
                //                            AMST_Id = a.AMST_Id,
                //                            AMST_FirstName = a.AMST_FirstName,
                //                            AMST_MiddleName = a.AMST_MiddleName,
                //                            AMST_LastName = a.AMST_LastName

                //                        }).Distinct().ToArray();
                //}
                //else if (data.ASMCL_Id != 0 && data.ASMS_Id == 0)
                //{
                //    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                        where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id)
                //                        select new CLGFeeAdjustmentDTO
                //                        {
                //                            AMST_Id = a.AMST_Id,
                //                            AMST_FirstName = a.AMST_FirstName,
                //                            AMST_MiddleName = a.AMST_MiddleName,
                //                            AMST_LastName = a.AMST_LastName

                //                        }).Distinct().ToArray();
                //}
                //else if (data.ASMCL_Id == 0 && data.ASMS_Id != 0)
                //{
                //    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                        where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMS_Id == data.ASMS_Id)
                //                        select new CLGFeeAdjustmentDTO
                //                        {
                //                            AMST_Id = a.AMST_Id,
                //                            AMST_FirstName = a.AMST_FirstName,
                //                            AMST_MiddleName = a.AMST_MiddleName,
                //                            AMST_LastName = a.AMST_LastName

                //                        }).Distinct().ToArray();
                //}
                //else
                //{
                var fillstudent = (from a in _FeeGroupContext.Adm_Master_College_StudentDMO
                                   from b in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                   from c in _FeeGroupContext.MasterCourseDMO
                                   from d in _FeeGroupContext.ClgMasterBranchDMO
                                   from e in _FeeGroupContext.CLG_Adm_Master_SemesterDMO
                                   from f in _FeeGroupContext.Adm_College_Master_SectionDMO
                                   where (b.AMCO_Id == c.AMCO_Id && d.AMB_Id == b.AMB_Id && a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && e.AMSE_Id == b.AMSE_Id && f.ACMS_Id == b.ACMS_Id && b.ACYST_ActiveFlag==1)
                                   select new CollegeFeeTransactionDTO
                                   {
                                       AMCST_Id = a.AMCST_Id,
                                       AMCST_FirstName = a.AMCST_FirstName,
                                       AMCST_MiddleName = a.AMCST_MiddleName,
                                       AMCST_LastName = a.AMCST_LastName,
                                       AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                       AMCST_AdmNo = a.AMCST_AdmNo,
                                       ACYST_RollNo = b.ACYST_RollNo,
                                       AMCO_CourseName = c.AMCO_CourseName,
                                       AMB_BranchName = d.AMB_BranchName,
                                       AMSE_SEMName = e.AMSE_SEMName,
                                       ACMS_SectionName = f.ACMS_SectionName,
                                       AMCST_FatherName = a.AMCST_FatherName,
                                       AMCST_DOB = a.AMCST_DOB,
                                       AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo)
                                   }
             ).Distinct().OrderBy(t => t.ACYST_RollNo).ToList();
                data.fillstudent = fillstudent.ToArray();
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGFeeAdjustmentDTO getdatabothgroupdet(CLGFeeAdjustmentDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.fillfromgroup = (from a in _FeeGroupContext.Fee_College_Student_StatusDMO
                                      from b in _FeeGroupContext.FeeGroupClgDMO
                                      from c in _FeeGroupContext.FeeHeadClgDMO
                                      where ( a.FMH_Id==c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && b.FMG_ActiceFlag == true && a.User_Id==b.user_id && b.user_id==c.user_id && a.User_Id==data.userid && c.FMH_RefundFlag== refundflag)
                                      select new CLGFeeAdjustmentDTO
                                      {
                                          FSA_From_FMG_Id = b.FMG_Id,
                                          FMG_GroupNameF = b.FMG_GroupName,
                                      }
                              ).Distinct().ToArray();
                //data.filltogroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                //                    from b in _FeeGroupContext.FeeGroupDMO
                //                    where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.FMG_ActiceFlag == true && a.FSS_RunningExcessAmount == 0 && a.FSS_ToBePaid > 0)
                //                    select new CLGFeeAdjustmentDTO
                //                    {
                //                        FSA_To_FMG_Id = b.FMG_Id,
                //                        FMG_GroupNameT = b.FMG_GroupName,
                //                    }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGFeeAdjustmentDTO getdatafromheaddet(CLGFeeAdjustmentDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.filltogroup = (from a in _FeeGroupContext.Fee_College_Student_StatusDMO
                                    from b in _FeeGroupContext.FeeGroupClgDMO
                                    where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id  && a.User_Id == b.user_id  && a.User_Id == data.userid && a.FCSS_ToBePaid > 0)
                                    select new CLGFeeAdjustmentDTO
                                    {
                                        FSA_To_FMG_Id = b.FMG_Id,
                                        FMG_GroupNameT = b.FMG_GroupName,
                                    }).Distinct().ToArray();
                data.fillfromhead = (from a in _FeeGroupContext.Fee_College_Student_StatusDMO
                                     from b in _FeeGroupContext.FeeHeadClgDMO
                                     from c in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                     where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.User_Id == b.user_id && a.User_Id == data.userid && data.multiplegroupF.ToString().Contains(Convert.ToString(a.FMG_Id)) && b.FMH_RefundFlag== refundflag  && ((a.FCSS_RunningExcessAmount > 0 || b.FMH_Flag == "E" || (a.FCSS_RefundableAmount > 0 )) && b.FMH_Flag != "F" || (a.FCSS_RunningExcessAmount + a.FCSS_RefundableAmount) > 0))
                                     select new CLGFeeAdjustmentDTO
                                     {
                                         FSA_From_FMH_Id = b.FMH_Id,
                                         FMH_FeeNameF = b.FMH_FeeName,
                                         FSA_From_FTI_Id = c.FTI_Id,
                                         FTI_NameF = c.FTI_Name,
                                         FSA_From_FMG_Id = a.FMG_Id,
                                         FSA_From_FMA_Id = a.FCMAS_Id,
                                         FSS_RunningExcessAmount =  a.FCSS_RunningExcessAmount + a.FCSS_RefundableAmount,

                                     }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGFeeAdjustmentDTO getdatatoheaddet(CLGFeeAdjustmentDTO data)
        {
            try
            {
                data.filltohead = (from a in _FeeGroupContext.Fee_College_Student_StatusDMO
                                   from b in _FeeGroupContext.FeeHeadClgDMO
                                   from c in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                   where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.User_Id == b.user_id  && a.User_Id == data.userid && data.multiplegroupT.ToString().Contains(Convert.ToString(a.FMG_Id)) && a.FCSS_ToBePaid > 0)// && a.FSS_RunningExcessAmount == 0 
                                   select new CLGFeeAdjustmentDTO
                                   {
                                       FSA_To_FMH_Id = b.FMH_Id,
                                       FMH_FeeNameT = b.FMH_FeeName,
                                       FSA_TO_FTI_Id = c.FTI_Id,
                                       FTI_NameT = c.FTI_Name,
                                       FSA_To_FMG_Id = a.FMG_Id,
                                       FSA_To_FMA_Id = a.FCMAS_Id,
                                       tobepaid=a.FCSS_ToBePaid
                                   }
                                     ).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        
        public CLGFeeAdjustmentDTO savedatadelegate(CLGFeeAdjustmentDTO data)
        {
            try
            {
                CLGFeeAdjustmentDTO feepge = data;
               // CLGFeeAdjustmentDTO feepge = Mapper.Map<CLGFeeAdjustmentDTO>(data);
                if (feepge.FSA_Id > 0)
                {
                    var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("CLG_Fee_Student_Adjustment_Edit @p0, @p1", data.FSA_Id, data.tolist[0].FSA_AdjustedAmount);
                    if (contactExists > 0)
                    {

                        data.returnduplicatestatus = "Updated";
                    }
                    else
                    {
                        data.returnduplicatestatus = "not Updated";
                    }
                }
                else
                {
                    var result = _FeeGroupContext.Fee_College_Student_AdjustmentDMO.Where(t => t.MI_Id == feepge.MI_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.AMCST_Id == feepge.AMCST_Id && t.FCSA_From_FMG_Id == feepge.fromlist[0].FSA_From_FMG_Id && t.FCSA_From_FMH_Id == feepge.fromlist[0].FSA_From_FMH_Id && t.FCSA_FromFTI_Id == feepge.fromlist[0].FSA_From_FTI_Id && t.FCSA_FromFMA_Id == feepge.fromlist[0].FSA_From_FMA_Id && t.FCSA_To_FMG_Id == feepge.tolist[0].FSA_To_FMG_Id && t.FCSA_To_FMH_Id == feepge.tolist[0].FSA_To_FMH_Id && t.FCSA_ToFTI_Id == feepge.tolist[0].FSA_TO_FTI_Id && t.FCSA_ToFMA_Id == feepge.tolist[0].FSA_To_FMA_Id).ToList();
                    if (result.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate Record";
                    }
                    else
                    {
                        fromCollection fromdata = new fromCollection();

                        for (int i = 0; i < data.fromlist.Count; i++)
                        {
                            fromtabledata df = new fromtabledata();
                            df.From_FMG_id = data.fromlist[i].FSA_From_FMG_Id;
                            df.From_FMH_id = data.fromlist[i].FSA_From_FMH_Id;
                            df.From_FTI_id = data.fromlist[i].FSA_From_FTI_Id;
                            df.From_FMA_id = data.fromlist[i].FSA_From_FMA_Id;
                            df.RunningExcessAmount = data.fromlist[i].FSS_RunningExcessAmount;
                            fromdata.Add(df);
                        }
                        toCollection todata = new toCollection();
                        for (int i = 0; i < data.tolist.Count; i++)
                        {
                            totabledata dt = new totabledata();
                            dt.To_FMG_id = data.tolist[i].FSA_To_FMG_Id;
                            dt.To_FMH_id = data.tolist[i].FSA_To_FMH_Id;
                            dt.To_FTI_id = data.tolist[i].FSA_TO_FTI_Id;
                            dt.To_FMA_id = data.tolist[i].FSA_To_FMA_Id;
                            dt.adjustedamount = data.tolist[i].FSA_AdjustedAmount;
                            todata.Add(dt);
                        }


                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "CLG_Fee_Student_Adjustment_insert";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@miid",
                                SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@amstid",
                           SqlDbType.BigInt)
                            {
                                Value = data.AMCST_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@fsa_date",
                        SqlDbType.DateTime)
                            {
                                Value = data.FSA_Date
                            });

                            cmd.Parameters.Add(new SqlParameter("@fsa_flag",
                              SqlDbType.Bit)
                            {
                                Value = 1
                            });

                            cmd.Parameters.Add(new SqlParameter("@INFO_ARRAYF",
                              SqlDbType.Structured)
                            {
                                Value = fromdata
                            });
                            cmd.Parameters.Add(new SqlParameter("@INFO_ARRAYT",
                           SqlDbType.Structured)
                            {
                                Value = todata
                            });
                            cmd.Parameters.Add(new SqlParameter("@userid",
                               SqlDbType.Decimal)
                            {
                                Value = data.userid
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            if (data1 >= 1)
                            {
                                data.returnduplicatestatus = "Saved";
                            }
                            else
                            {
                                data.returnduplicatestatus = "not Saved";
                            }
                        }
                    }
                }
               
            }
            catch (Exception ee)
            {
                data.returnduplicatestatus = "Error Found";
                // data.returnduplicatestatus = "fail";
                //   _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CLGFeeAdjustmentDTO getpageedit(int id)
        {
            CLGFeeAdjustmentDTO data = new CLGFeeAdjustmentDTO();
            try
            {

                var result = _FeeGroupContext.Fee_College_Student_AdjustmentDMO.Single(t => t.FCSA_Id == id);
                data.FSA_Id = result.FCSA_Id;
                data.MI_Id = result.MI_Id;
                data.AMCST_Id = result.AMCST_Id;
                data.ASMAY_Id = result.ASMAY_Id;
                data.FSA_Date = result.FCSA_Date;
                data.FSA_From_FMH_Id = result.FCSA_From_FMH_Id;
                data.FSA_From_FMG_Id = result.FCSA_From_FMG_Id;
                data.FSA_From_FTI_Id = result.FCSA_FromFTI_Id;
                data.FSA_From_FMA_Id = result.FCSA_FromFMA_Id;
                data.FSA_AdjustedAmount = result.FCSA_AdjustedAmount;
                data.FSA_To_FMH_Id = result.FCSA_To_FMH_Id;
                data.FSA_To_FMG_Id = result.FCSA_To_FMG_Id;
                data.FSA_TO_FTI_Id = result.FCSA_ToFTI_Id;
                data.FSA_To_FMA_Id = result.FCSA_ToFMA_Id;
                data.FSA_ActiveFlag = result.FCSA_ActiveFlag;
                data.userid = result.User_Id;

                var resASMCL_Id = (from a in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                   from b in _FeeGroupContext.MasterCourseDMO
                                   where (a.AMCO_Id == b.AMCO_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.ACYST_ActiveFlag==1)
                                   select new CLGFeeAdjustmentDTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       ASMCL_ClassName = b.AMCO_CourseName,
                                   }).Distinct().Single();

                data.AMCO_Id = resASMCL_Id.AMCO_Id;
                data.courselist = (from a in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                   from b in _FeeGroupContext.MasterCourseDMO
                                   where (a.AMCO_Id == b.AMCO_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMCO_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id && a.ACYST_ActiveFlag==1)
                                  select b).Distinct().OrderBy(t => t.AMCO_Id).ToArray();
                //select new CLGFeeAdjustmentDTO
                //{
                //    AMCO_Id = a.AMCO_Id,
                //    ASMCL_ClassName = b.AMCO_CourseName,
                //}
                var resASMS_Id = (from a in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                 from b in _FeeGroupContext.ClgMasterBranchDMO
                                 where (a.AMB_Id == b.AMB_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMCST_Id == data.AMCST_Id && a.ACYST_ActiveFlag==1)
                                 select new CLGFeeAdjustmentDTO
                                 {
                                     AMB_Id = a.AMB_Id,
                                     ASMC_SectionName = b.AMB_BranchName,
                                 }).Distinct().Single();
                data.AMB_Id = resASMS_Id.AMB_Id;
                data.branchlist = (from a in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                   from b in _FeeGroupContext.ClgMasterBranchDMO
                                   where (a.AMB_Id == b.AMB_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_ActiveFlag == true && a.AMB_Id == data.AMB_Id && a.ACYST_ActiveFlag==1)
                                   select new ClgMasterBranchDMO
                                   {
                                       AMB_Id = b.AMB_Id,
                                       AMB_BranchName = b.AMB_BranchName,
                                       AMB_BranchCode = b.AMB_BranchCode,
                                      // AMB_BranchInfo = b.AMB_BranchInfo,
                                       //AMB_BranchType = b.AMB_BranchType,
                                       //AMB_StudentCapacity = b.AMB_StudentCapacity,
                                       //AMB_Order = b.AMB_Order,
                                       //AMB_AidedUnAided = b.AMB_AidedUnAided
                                   }).Distinct().ToArray();
                var resAMSE_Id = (from a in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                  from b in _FeeGroupContext.CLG_Adm_Master_SemesterDMO
                                  where (a.AMSE_Id == b.AMSE_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMCST_Id == data.AMCST_Id && a.AMB_Id==data.AMB_Id && a.ACYST_ActiveFlag==1)
                                  select new CLGFeeAdjustmentDTO
                                  {
                                      AMSE_Id = a.AMSE_Id,
                                      ASMC_SectionName = b.AMSE_SEMName,
                                  }).Distinct().Single();
                data.AMSE_Id = resAMSE_Id.AMSE_Id;
                data.semisterlist = (from a in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                   from b in _FeeGroupContext.CLG_Adm_Master_SemesterDMO
                                   where (a.AMSE_Id == b.AMSE_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id  && b.AMSE_ActiveFlg == true && a.AMSE_Id==data.AMSE_Id && a.AMB_Id == data.AMB_Id && a.ACYST_ActiveFlag==1)
                                     select b).Distinct().OrderBy(t => t.AMSE_Id).ToArray();
                //select new CLGFeeAdjustmentDTO
                //{
                //    AMSE_Id = a.AMSE_Id,
                //    ASMC_SectionName = b.AMSE_SEMName,
                //}).Distinct().OrderBy(t => t.AMSE_Id).ToArray();
                data.fillstudent = (from a in _FeeGroupContext.Adm_Master_College_StudentDMO
                                    from b in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                    where (a.AMCST_Id == b.AMCST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && a.AMCST_Id == data.AMCST_Id && b.AMSE_Id==data.AMSE_Id && b.ACYST_ActiveFlag==1)
                                    select new CollegeFeeTransactionDTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        AMCST_FirstName = a.AMCST_FirstName,
                                        AMCST_MiddleName = a.AMCST_MiddleName,
                                        AMCST_LastName = a.AMCST_LastName,
                                        AMCST_RegistrationNo = a.AMCST_RegistrationNo,

                                    }).Distinct().ToArray();
                data.fillfromgroup = (from a in _FeeGroupContext.Fee_College_Student_StatusDMO
                                      from b in _FeeGroupContext.FeeGroupClgDMO
                                      where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && b.FMG_Id == data.FSA_From_FMG_Id && a.User_Id == b.user_id && a.User_Id == data.userid)
                                      select new CLGFeeAdjustmentDTO
                                      {
                                          FSA_From_FMG_Id = b.FMG_Id,
                                          FMG_GroupNameF = b.FMG_GroupName,
                                      }
                              ).Distinct().ToArray();
                data.filltogroup = (from a in _FeeGroupContext.Fee_College_Student_StatusDMO
                                    from b in _FeeGroupContext.FeeGroupClgDMO
                                    where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && b.FMG_Id == data.FSA_To_FMG_Id && a.User_Id == b.user_id && a.User_Id == data.userid)
                                    select new CLGFeeAdjustmentDTO
                                    {
                                        FSA_To_FMG_Id = b.FMG_Id,
                                        FMG_GroupNameT = b.FMG_GroupName,
                                    }
                              ).Distinct().ToArray();
                data.fillfromhead = (from a in _FeeGroupContext.Fee_College_Student_StatusDMO
                                     from b in _FeeGroupContext.FeeHeadClgDMO
                                     from c in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                     where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.FMG_Id == data.FSA_From_FMG_Id && a.FMH_Id == data.FSA_From_FMH_Id && a.FTI_Id == data.FSA_From_FTI_Id && a.FCMAS_Id == data.FSA_From_FMA_Id && a.User_Id == b.user_id && a.User_Id == data.userid)
                                     select new CLGFeeAdjustmentDTO
                                     {
                                         FSA_From_FMH_Id = b.FMH_Id,
                                         FMH_FeeNameF = b.FMH_FeeName,
                                         FSA_From_FTI_Id = c.FTI_Id,
                                         FTI_NameF = c.FTI_Name,
                                         FSA_From_FMG_Id = a.FMG_Id,
                                         FSA_From_FMA_Id = a.FCMAS_Id,
                                         FSS_RunningExcessAmount = a.FCSS_RunningExcessAmount + a.FCSS_RefundableAmount + data.FSA_AdjustedAmount,

                                     }).Distinct().ToArray();
                data.filltohead = (from a in _FeeGroupContext.Fee_College_Student_StatusDMO
                                   from b in _FeeGroupContext.FeeHeadClgDMO
                                   from c in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                   where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.FMG_Id == data.FSA_To_FMG_Id && a.FMH_Id == data.FSA_To_FMH_Id && a.FTI_Id == data.FSA_TO_FTI_Id && a.FCMAS_Id == data.FSA_To_FMA_Id && a.User_Id == b.user_id && a.User_Id == data.userid)
                                   select new CLGFeeAdjustmentDTO
                                   {
                                       FSA_To_FMH_Id = b.FMH_Id,
                                       FMH_FeeNameT = b.FMH_FeeName,
                                       FSA_TO_FTI_Id = c.FTI_Id,
                                       FTI_NameT = c.FTI_Name,
                                       FSA_To_FMG_Id = a.FMG_Id,
                                       FSA_To_FMA_Id = a.FCMAS_Id,
                                       FSS_RunningExcessAmount = a.FCSS_RunningExcessAmount + a.FCSS_RefundableAmount,
                                       tobepaid = a.FCSS_ToBePaid + data.FSA_AdjustedAmount
                                   }
                                    ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
               // _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public CLGFeeAdjustmentDTO deleterec(int id)
        {

            CLGFeeAdjustmentDTO page = new CLGFeeAdjustmentDTO();
            List<Fee_College_Student_AdjustmentDMO> lorg = new List<Fee_College_Student_AdjustmentDMO>();
            lorg = _FeeGroupContext.Fee_College_Student_AdjustmentDMO.Where(t => t.FCSA_Id.Equals(id)).ToList();


            try
            {
                if (lorg.Any())
                {
                    var result = _FeeGroupContext.Fee_College_Student_AdjustmentDMO.FirstOrDefault(t => t.FCSA_Id == id);
                    var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("CLG_Fee_Student_Adjustment_Delete @p0", result.FCSA_Id);
                    if (contactExists > 0)
                    {

                        page.returnduplicatestatus = "success";
                    }
                    else
                    {
                        page.returnduplicatestatus = "fail";
                    }
                }


            }
            catch (Exception ee)
            {
                // _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return page;
        }


        public CLGFeeAdjustmentDTO searching(CLGFeeAdjustmentDTO data)
        {

            try
            {

                switch (data.searchType)
                {

                    case "1":
                        string str = "";
                        data.filldata = (from a in _FeeGroupContext.Fee_College_Student_AdjustmentDMO
                                         from b in _FeeGroupContext.FeeHeadClgDMO
                                         from c in _FeeGroupContext.FeeHeadClgDMO
                                         from d in _FeeGroupContext.Adm_Master_College_StudentDMO
                                         from e in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         where (a.FCSA_From_FMH_Id == b.FMH_Id && a.FCSA_To_FMH_Id == c.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FCSA_FromFTI_Id == e.FTI_Id && a.FCSA_ToFTI_Id == f.FTI_Id && (((d.AMCST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(d.AMCST_MiddleName.Trim()) == true ? str : d.AMCST_LastName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(d.AMCST_LastName.Trim()) == true ? str : d.AMCST_LastName.Trim())).Trim().Contains(data.searchtext) || d.AMCST_FirstName.Contains(data.searchtext) || d.AMCST_MiddleName.Contains(data.searchtext) || d.AMCST_LastName.Contains(data.searchtext)) && a.User_Id == data.userid)

                                         select new CLGFeeAdjustmentDTO
                                         {
                                             FSA_Id = a.FCSA_Id,
                                             AMST_FirstName = d.AMCST_FirstName,
                                             AMST_MiddleName = d.AMCST_MiddleName,
                                             AMST_LastName = d.AMCST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FCSA_AdjustedAmount,
                                             FSA_Date = a.FCSA_Date,

                                         }).Distinct().OrderByDescending(t => t.AMST_FirstName).ToList().ToArray();

                        break;
                    case "2":
                        data.filldata = (from a in _FeeGroupContext.Fee_College_Student_AdjustmentDMO
                                         from b in _FeeGroupContext.FeeHeadClgDMO
                                         from c in _FeeGroupContext.FeeHeadClgDMO
                                         from d in _FeeGroupContext.Adm_Master_College_StudentDMO
                                         from e in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         where (a.FCSA_From_FMH_Id == b.FMH_Id && a.FCSA_To_FMH_Id == c.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FCSA_FromFTI_Id == e.FTI_Id && a.FCSA_ToFTI_Id == f.FTI_Id && b.FMH_FeeName.Contains(data.searchtext) && a.User_Id == data.userid)

                                         select new CLGFeeAdjustmentDTO
                                         {
                                             FSA_Id = a.FCSA_Id,
                                             AMST_FirstName = d.AMCST_FirstName,
                                             AMST_MiddleName = d.AMCST_MiddleName,
                                             AMST_LastName = d.AMCST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FCSA_AdjustedAmount,
                                             FSA_Date = a.FCSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FMH_FeeNameF).ToList().ToArray();
                        break;
                    case "3":
                        data.filldata = (from a in _FeeGroupContext.Fee_College_Student_AdjustmentDMO
                                         from b in _FeeGroupContext.FeeHeadClgDMO
                                         from c in _FeeGroupContext.FeeHeadClgDMO
                                         from d in _FeeGroupContext.Adm_Master_College_StudentDMO
                                         from e in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         where (a.FCSA_From_FMH_Id == b.FMH_Id && a.FCSA_To_FMH_Id == c.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FCSA_FromFTI_Id == e.FTI_Id && a.FCSA_ToFTI_Id == f.FTI_Id && e.FTI_Name.Contains(data.searchtext) && a.User_Id == data.userid)

                                         select new CLGFeeAdjustmentDTO
                                         {
                                             FSA_Id = a.FCSA_Id,
                                             AMST_FirstName = d.AMCST_FirstName,
                                             AMST_MiddleName = d.AMCST_MiddleName,
                                             AMST_LastName = d.AMCST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FCSA_AdjustedAmount,
                                             FSA_Date = a.FCSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FMH_FeeNameT).ToList().ToArray();

                        break;
                    case "4":
                        data.filldata = (from a in _FeeGroupContext.Fee_College_Student_AdjustmentDMO
                                         from b in _FeeGroupContext.FeeHeadClgDMO
                                         from c in _FeeGroupContext.FeeHeadClgDMO
                                         from d in _FeeGroupContext.Adm_Master_College_StudentDMO
                                         from e in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         where (a.FCSA_From_FMH_Id == b.FMH_Id && a.FCSA_To_FMH_Id == c.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FCSA_FromFTI_Id == e.FTI_Id && a.FCSA_ToFTI_Id == f.FTI_Id && c.FMH_FeeName.Contains(data.searchtext) && a.User_Id == data.userid)

                                         select new CLGFeeAdjustmentDTO
                                         {
                                             FSA_Id = a.FCSA_Id,
                                             AMST_FirstName = d.AMCST_FirstName,
                                             AMST_MiddleName = d.AMCST_MiddleName,
                                             AMST_LastName = d.AMCST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FCSA_AdjustedAmount,
                                             FSA_Date = a.FCSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FTI_NameF).ToList().ToArray();
                        break;
                    case "5":
                        data.filldata = (from a in _FeeGroupContext.Fee_College_Student_AdjustmentDMO
                                         from b in _FeeGroupContext.FeeHeadClgDMO
                                         from c in _FeeGroupContext.FeeHeadClgDMO
                                         from d in _FeeGroupContext.Adm_Master_College_StudentDMO
                                         from e in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         where (a.FCSA_From_FMH_Id == b.FMH_Id && a.FCSA_To_FMH_Id == c.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FCSA_FromFTI_Id == e.FTI_Id && a.FCSA_ToFTI_Id == f.FTI_Id && f.FTI_Name.Contains(data.searchtext) && a.User_Id == data.userid)

                                         select new CLGFeeAdjustmentDTO
                                         {
                                             FSA_Id = a.FCSA_Id,
                                             AMST_FirstName = d.AMCST_FirstName,
                                             AMST_MiddleName = d.AMCST_MiddleName,
                                             AMST_LastName = d.AMCST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FCSA_AdjustedAmount,
                                             FSA_Date = a.FCSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FTI_NameT).ToList().ToArray();

                        break;
                    case "6":
                        data.filldata = (from a in _FeeGroupContext.Fee_College_Student_AdjustmentDMO
                                         from b in _FeeGroupContext.FeeHeadClgDMO
                                         from c in _FeeGroupContext.FeeHeadClgDMO
                                         from d in _FeeGroupContext.Adm_Master_College_StudentDMO
                                         from e in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         where (a.FCSA_From_FMH_Id == b.FMH_Id && a.FCSA_To_FMH_Id == c.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FCSA_FromFTI_Id == e.FTI_Id && a.FCSA_ToFTI_Id == f.FTI_Id && a.FCSA_AdjustedAmount.ToString().Contains(data.searchnumber) && a.User_Id == data.userid)

                                         select new CLGFeeAdjustmentDTO
                                         {
                                             FSA_Id = a.FCSA_Id,
                                             AMST_FirstName = d.AMCST_FirstName,
                                             AMST_MiddleName = d.AMCST_MiddleName,
                                             AMST_LastName = d.AMCST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FCSA_AdjustedAmount,
                                             FSA_Date = a.FCSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FSA_AdjustedAmount).ToList().ToArray();

                        break;
                    case "7":

                        data.filldata = (from a in _FeeGroupContext.Fee_College_Student_AdjustmentDMO
                                         from b in _FeeGroupContext.FeeHeadClgDMO
                                         from c in _FeeGroupContext.FeeHeadClgDMO
                                         from d in _FeeGroupContext.Adm_Master_College_StudentDMO
                                         from e in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         where (a.FCSA_From_FMH_Id == b.FMH_Id && a.FCSA_To_FMH_Id == c.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FCSA_FromFTI_Id == e.FTI_Id && a.FCSA_ToFTI_Id == f.FTI_Id && a.FCSA_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && a.User_Id == data.userid)

                                         select new CLGFeeAdjustmentDTO
                                         {
                                             FSA_Id = a.FCSA_Id,
                                             AMST_FirstName = d.AMCST_FirstName,
                                             AMST_MiddleName = d.AMCST_MiddleName,
                                             AMST_LastName = d.AMCST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FCSA_AdjustedAmount,
                                             FSA_Date = a.FCSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FSA_Date).ToList().ToArray();
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


