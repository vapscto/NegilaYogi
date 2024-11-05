using System;
using CollegeFeeService.com.vaps.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using PreadmissionDTOs.com.vaps.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vaps.Fee;
using AutoMapper;
using DomainModel.Model.com.vapstech.College.Fees;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeStudentLedgerImpl : CollegeStudentLedgerInterface
    {
        private static ConcurrentDictionary<string, CollegeStudentLedgerDTO> _login =
             new ConcurrentDictionary<string, CollegeStudentLedgerDTO>();

        public CollFeeGroupContext CollFeeGroupContext;

        public CollegeStudentLedgerImpl(CollFeeGroupContext objDbcontext)
        {
            CollFeeGroupContext = objDbcontext;
        }


        public CollegeStudentLedgerDTO GetYearList(int id)
        {
            CollegeStudentLedgerDTO data = new CollegeStudentLedgerDTO();
            try
            {
                data.yearlist = CollFeeGroupContext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active).Distinct().ToArray();

                data.sectionlist = CollFeeGroupContext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == id && t.ACMS_ActiveFlag == true).OrderBy(t=>t.ACMS_Order).ToArray();
                data.quotalist = CollFeeGroupContext.Clg_Adm_College_QuotaDMO.Where(t=>t.MI_Id==id&&t.ACQ_ActiveFlg==true).ToArray();


            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            return data;
        }
        public CollegeStudentLedgerDTO get_courses(CollegeStudentLedgerDTO data)
        {

            try
            {

                data.courselist = (from a in CollFeeGroupContext.MasterCourseDMO
                                   from b in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true
                                   && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                   && b.ACAYC_ActiveFlag == true && b.AMCO_Id == a.AMCO_Id)
                                   select new CollegeStudentLedgerDTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName
                                   }).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

                data.fillmastergroup = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in CollFeeGroupContext.FeeGroupClgDMO
                                        from f in CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                                        from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO

                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == data.user_id && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id)
                                        select new CollegeYearlyStatusReportDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = b.FMG_GroupName
                                        }).Distinct().ToArray();

                data.fillmasterhead = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in CollFeeGroupContext.FeeGroupClgDMO
                                       from c in CollFeeGroupContext.FeeHeadClgDMO
                                       from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id)
                                       select new CollegeYearlyStatusReportDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();

                data.fillinstallment = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in CollFeeGroupContext.FeeGroupClgDMO
                                        from c in CollFeeGroupContext.FeeHeadClgDMO
                                        from d in CollFeeGroupContext.Clg_Fee_Installment_DMO
                                        from e in CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                        from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == e.FTI_Id)
                                        select new CollegeYearlyStatusReportDTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeStudentLedgerDTO get_branches(CollegeStudentLedgerDTO data)
        {
            try
            {
                var branchlist = (from a in CollFeeGroupContext.ClgMasterBranchDMO
                                  from b in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                  from c in CollFeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id==b.MI_Id&& a.MI_Id==c.MI_Id &&a.AMB_ActiveFlag==true && a.AMB_Id==c.AMB_Id && b.AMCO_Id==data.AMCO_Id && b.ACAYC_Id==c.ACAYC_Id && b.ACAYC_ActiveFlag==true &&c.ACAYCB_ActiveFlag==true && b.ASMAY_Id==data.ASMAY_Id )
                                  select new CollegeStudentLedgerDTO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_Order = a.AMB_Order,
                                      //AMB_BranchCode = a.AMB_BranchCode,
                                      //AMB_BranchInfo = a.AMB_BranchInfo,
                                      //AMB_BranchType = a.AMB_BranchType,
                                      //AMB_StudentCapacity = a.AMB_StudentCapacity,

                                      //AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeStudentLedgerDTO get_semisters(CollegeStudentLedgerDTO data)
        {
            try
            {
                data.semesterlist = (from a in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                     from b in CollFeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from c in CollFeeGroupContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     from d in CollFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                     where(a.MI_Id==b.MI_Id && a.MI_Id==c.MI_Id && a.MI_Id==d.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.MI_Id==data.MI_Id && a.AMCO_Id==data.AMCO_Id && b.AMB_Id==data.AMB_Id && a.ACAYC_Id==b.ACAYC_Id && b.ACAYCB_Id==c.ACAYCB_Id && d.AMSE_Id==c.AMSE_Id && c.ACAYCBS_ActiveFlag==true && d.AMSE_ActiveFlg==true)
                                     select new CollegeStudentLedgerDTO
                                     {
                                         AMSE_Id = d.AMSE_Id,
                                         AMSE_SEMName = d.AMSE_SEMName,
                                         AMSE_SEMOrder = d.AMSE_SEMOrder

                                     }).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeStudentLedgerDTO get_report(CollegeStudentLedgerDTO data)
        {
            try
            {
                data.studentlist = (from a in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                    from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Ids.Contains(a.AMCO_Id) && data.AMB_Ids.Contains(a.AMB_Id) && a.ACYST_ActiveFlag == 1 && data.AMSE_Ids.Contains(a.AMSE_Id) && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag)
                                    select new CollegeStudentLedgerDTO
                                    {
                                        ACAYC_Id = a.ACYST_Id,
                                        AMCST_Id = a.AMCST_Id,
                                        ACYST_RollNo = a.ACYST_RollNo,
                                        AMCST_FirstName = b.AMCST_FirstName,
                                        AMCST_MiddleName = b.AMCST_MiddleName,
                                        AMCST_LastName = b.AMCST_LastName,
                                        AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                        AMCST_AdmNo = b.AMCST_AdmNo
                                    }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();

                data.fillmastergroup = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in CollFeeGroupContext.FeeGroupClgDMO
                                        from f in CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                                        from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO

                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == data.user_id && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id)
                                        select new CollegeStudentLedgerDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = b.FMG_GroupName
                                        }).Distinct().ToArray();

                data.fillmasterhead = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in CollFeeGroupContext.FeeGroupClgDMO
                                       from c in CollFeeGroupContext.FeeHeadClgDMO
                                       from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id)
                                       select new CollegeStudentLedgerDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();

                data.fillinstallment = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in CollFeeGroupContext.FeeGroupClgDMO
                                        from c in CollFeeGroupContext.FeeHeadClgDMO
                                        from d in CollFeeGroupContext.Clg_Fee_Installment_DMO
                                        from e in CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                        from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == e.FTI_Id)
                                        select new CollegeStudentLedgerDTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }).Distinct().ToArray();

                data.Studentreport = (from a in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                      from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                      from c in CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO
                                      from d in CollFeeGroupContext.FeeGroupClgDMO
                                      from e in CollFeeGroupContext.AcademicYear
                                      where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Ids.Contains(a.AMCO_Id) && data.AMB_Ids.Contains(a.AMB_Id) && data.AMSE_Ids.Contains(a.AMSE_Id) && a.AMCST_Id == c.AMCST_Id && a.ACYST_ActiveFlag == 1 && c.FMG_Id == d.FMG_Id && c.MI_Id == d.MI_Id && d.FMG_ActiceFlag == true && a.ASMAY_Id == e.ASMAY_Id && e.MI_Id == data.MI_Id)
                                      select new CollegeStudentLedgerDTO
                                      {
                                          ACAYC_Id = a.ACYST_Id,
                                          AMCST_Id = a.AMCST_Id,
                                          ASMAY_Id = a.ASMAY_Id,
                                          ASMAY_Year = e.ASMAY_Year,
                                          FMG_Id = c.FMG_Id,
                                          FCMSGH_Id = c.FCMSGH_Id,
                                          ACYST_RollNo = a.ACYST_RollNo,
                                          AMCST_FirstName = b.AMCST_FirstName,
                                          AMCST_MiddleName = b.AMCST_MiddleName,
                                          AMCST_LastName = b.AMCST_LastName,
                                          AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                          AMCST_AdmNo = b.AMCST_AdmNo,
                                          FMG_GroupName = d.FMG_GroupName

                                      }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();

            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message);
            }
            return data;
        }
        public CollegeStudentLedgerDTO get_student(CollegeStudentLedgerDTO data)
        {
            try
            {
                if (data.ACMS_Id>0)
                {
                    data.studentlist = (from a in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                        from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == b.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id
                                        && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag)
                                        select new CollegeStudentLedgerDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            ACYST_RollNo = a.ACYST_RollNo,
                                           // AMCST_FirstName = b.AMCST_FirstName,
                                            AMCST_MiddleName = b.AMCST_MiddleName,
                                            AMCST_LastName = b.AMCST_LastName,
                                            AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                            AMCST_AdmNo = b.AMCST_AdmNo,
                                            AMCST_FirstName = ((b.AMCST_FirstName == null ? " " : b.AMCST_FirstName) + (b.AMCST_MiddleName == null ? " " : b.AMCST_MiddleName) + (b.AMCST_LastName == null ? " " : b.AMCST_LastName)).Trim(),
                                        }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();
                }
                else
                {
                    data.studentlist = (from a in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                        from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == b.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == data.AMSE_Id 
                                        && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag)
                                        select new CollegeStudentLedgerDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            ACYST_RollNo = a.ACYST_RollNo,
                                            AMCST_FirstName = b.AMCST_FirstName,
                                            AMCST_MiddleName = b.AMCST_MiddleName,
                                            AMCST_LastName = b.AMCST_LastName,
                                            AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                            AMCST_AdmNo = b.AMCST_AdmNo
                                        }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        

        public CollegeStudentLedgerDTO DeleteRecord(CollegeStudentLedgerDTO data)
        {
            try
            {

                var remlve = CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO.Single(t => t.FCMSGH_Id == data.FCMSGH_Id);
                var already_cnt = CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == remlve.ASMAY_Id && t.AMCST_Id == remlve.AMCST_Id && t.FMG_Id == remlve.FMG_Id && t.FCSS_PaidAmount > 0).ToList().Count;
                if (already_cnt == 0)
                {

                    var remlve1 = CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO.Where(t => t.FCMSGH_Id == data.FCMSGH_Id).ToList();
                    if (remlve1.Any())
                    {
                        for (int y = 0; remlve1.Count > y; y++)
                        {
                            CollFeeGroupContext.Remove(remlve1.ElementAt(y));
                        }
                    }
                    var remlve2 = CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == remlve.ASMAY_Id && t.AMCST_Id == remlve.AMCST_Id && t.FMG_Id == remlve.FMG_Id).ToList();
                    if (remlve2.Any())
                    {
                        for (int z = 0; remlve2.Count > z; z++)
                        {
                            CollFeeGroupContext.Remove(remlve2.ElementAt(z));
                        }
                    }

                    CollFeeGroupContext.Remove(remlve);

                    var exists = CollFeeGroupContext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
                else
                {
                    data.returnval = "Depend";
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeStudentLedgerDTO savedata(CollegeStudentLedgerDTO data)
        {

            try
            {
                List<long> HeadId = new List<long>();
                List<long> grpid = new List<long>();
                foreach (var item in data.savegrplst)
                {

                    grpid.Add(item.FMG_Id);
                }
                foreach (var item in data.saveheadlst)
                {
                    HeadId.Add(item.FMH_Id);

                }
               
                string groupidss = "0";
                string headlist = "0";
            

                for (int r = 0; r < grpid.Count(); r++)
                {
                    if (r == 0)
                    {
                        groupidss = grpid[r].ToString();
                    }
                    else
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }

                }
                for (int c = 0; c < HeadId.Count(); c++)
                {

                    if (c == 0)
                    {
                        headlist = HeadId[c].ToString();
                    }
                    else
                    {
                        headlist = headlist + ',' + HeadId[c];
                    }

                }

          
                using (var cmd = CollFeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_StudentLedgerReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                   
                  
                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                      SqlDbType.NVarChar)
                    {
                        Value = groupidss
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = headlist
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
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
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.feedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);

            }

            return data;
        }

        public CollegeStudentLedgerDTO editdata(CollegeStudentLedgerDTO data)
        {
            try
            {
                //data.editdatalist= (from a in CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO
                //                    from b in CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO
                //                    where (a.FCMSGH_Id == b.FCMSGH_Id && a.ASMAY_Id == data.ASMAY_Id
                //                    && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id 
                //                    && a.FCMSGH_ActiveFlag == true)

                //                    select new CollegeStudentLedgerDTO
                //                    {

                //                        AMSE_Id=a.ASMAY_Id,
                //                        MI_Id = a.MI_Id,
                //                        AMCST_Id =a.AMCST_Id,
                //                        FMG_Id =a.FMG_Id,
                //                        FCMSGHI_Id=b.FCMSGHI_Id,
                //                        FMH_Id=b.FMH_Id,
                //                        FTI_Id=b.FTI_Id
                //                    }).Distinct().ToArray();

                data.editdatalist = (from a in CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO
                                     from b in CollFeeGroupContext.FeeGroupClgDMO
                                     from c in CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO
                                     from g in CollFeeGroupContext.FeeHeadClgDMO
                                     from d in CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                     from e in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                     from f in CollFeeGroupContext.Fee_College_Student_StatusDMO
                                     where (f.AMCST_Id == a.AMCST_Id && f.ASMAY_Id == a.ASMAY_Id && f.MI_Id == a.MI_Id && a.FMG_Id == f.FMG_Id && c.FMH_Id == f.FMH_Id && c.FTI_Id == f.FTI_Id && a.FCMSGH_Id == c.FCMSGH_Id && a.FMG_Id == b.FMG_Id && c.FMH_Id == g.FMH_Id && c.FTI_Id == d.FTI_Id && a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == a.FMG_Id && e.FMH_Id == c.FMH_Id && e.FMI_Id == d.FMI_Id)

                                     select new CollegeStudentLedgerDTO
                                     {
                                         AMCST_Id = a.AMCST_Id,
                                         FMG_Id = a.FMG_Id,
                                         FMG_GroupName = b.FMG_GroupName,
                                         FMH_Id = g.FMH_Id,
                                         FMH_FeeName = g.FMH_FeeName,
                                         FCMSGH_Id = a.FCMSGH_Id,
                                         FTI_Id = d.FTI_Id,
                                         FTI_Name = d.FTI_Name,
                                         FCSS_PaidAmount = f.FCSS_PaidAmount
                                     }
                ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}



