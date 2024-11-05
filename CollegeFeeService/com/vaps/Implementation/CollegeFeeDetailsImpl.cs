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
    public class CollegeFeeDetailsImpl: CollegeFeeDetailsInterface
    {
        private static ConcurrentDictionary<string, CollegeStudentLedgerDTO> _login =
           new ConcurrentDictionary<string, CollegeStudentLedgerDTO>();

        public CollFeeGroupContext CollFeeGroupContext;

        public CollegeFeeDetailsImpl(CollFeeGroupContext objDbcontext)
        {
            CollFeeGroupContext = objDbcontext;
        }
        public CollegeStudentLedgerDTO GetYearList(int id)
        {
            CollegeStudentLedgerDTO data = new CollegeStudentLedgerDTO();
            try
            {
                data.yearlist = CollFeeGroupContext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active).Distinct().ToArray();

                data.sectionlist = CollFeeGroupContext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == id && t.ACMS_ActiveFlag == true).OrderBy(t => t.ACMS_Order).ToArray();
                data.quotalist = CollFeeGroupContext.Clg_Adm_College_QuotaDMO.Where(t => t.MI_Id == id && t.ACQ_ActiveFlg == true).ToArray();


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
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.AMB_ActiveFlag == true && a.AMB_Id == c.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && b.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id)
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
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == c.AMSE_Id && c.ACAYCBS_ActiveFlag == true && d.AMSE_ActiveFlg == true)
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

        public async Task<CollegeStudentLedgerDTO> get_report(CollegeStudentLedgerDTO data)
        {
            try
            {
                var fmg_ids = "";
                foreach (var x in data.FMG_Ids)
                {
                    fmg_ids += x + ",";
                }
                fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));


                var fmh_ids = "";
                foreach(var y in data.FMH_Ids)
                {
                    fmh_ids += y + ",";
                }
                fmh_ids = fmh_ids.Substring(0, (fmh_ids.Length - 1));

               
                using (var cmd = CollFeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Feedetails_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                   
                    cmd.Parameters.Add(new SqlParameter("@amay_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amco_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amb_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@amse_id",
                   SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asms_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fmg_id",
                  SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@fmh_id",
                  SqlDbType.VarChar)
                    {
                        Value = fmh_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_id",
                 SqlDbType.VarChar)
                    {
                        Value = data.user_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                       
                            data.Studentreport = retObject.ToArray();
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return data;
                }


            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message);
            }
            return data;
        }



        public async Task<CollegeStudentLedgerDTO> get_concession_report(CollegeStudentLedgerDTO data)
        {
            try
            {
               
                using (var cmd = CollFeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Studentconcession_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@amay_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amco_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amb_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@amse_id",
                   SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asms_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                  
                    cmd.Parameters.Add(new SqlParameter("@user_id",
                 SqlDbType.VarChar)
                    {
                        Value = data.user_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }

                        data.Studentreport = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return data;
                }


            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message);
            }
            return data;
        }

    }
}
