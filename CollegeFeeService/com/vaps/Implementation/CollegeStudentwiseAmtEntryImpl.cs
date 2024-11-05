using System;
using System.Linq;
using DataAccessMsSqlServerProvider;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using System.Collections.Generic;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using DomainModel.Model.com.vapstech.College.Admission;
using System.Dynamic;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeStudentwiseAmtEntryImpl : interfaces.CollegeStudentwiseAmtEntryInterfaces
    {
        private static ConcurrentDictionary<string, CLGFeeAmountEntryDTO> _login =
            new ConcurrentDictionary<string, CLGFeeAmountEntryDTO>();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<CollegeStudentwiseAmtEntryImpl> _logger;
        public CollegeStudentwiseAmtEntryImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, ILogger<CollegeStudentwiseAmtEntryImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }
        public CLGFeeAmountEntryDTO Getinitialformload(CLGFeeAmountEntryDTO fee)
        {
            try
            {
                CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);

                fee.academicdrp = clgcomm.Get_All_Academicyear(fee.MI_Id);

                fee.currfillyear = clgcomm.Get_Current_Academicyear(fee.MI_Id, fee.ASMAY_Id);

                fee.Fillcourse = clgcomm.Get_Yearly_Course(fee.MI_Id, fee.ASMAY_Id);

                fee.Fillgroup = clgcomm.Get_Yearly_groups(fee.MI_Id, fee.ASMAY_Id);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return fee;
        }
        public CLGFeeAmountEntryDTO getbranchdetails(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                // data.fillbranch = clgcomm.Get_Yearly_Course_Branch(data.MI_Id, data.ASMAY_Id, data.AMCO_Id);
                var branchlist = (from a in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                  from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && data.AMCO_Ids.Contains(b.AMCO_Id))
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
                data.fillbranch = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAmountEntryDTO getsemesterdetails(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                //data.fillsemester = clgcomm.Get_Yearly_Course_Branch_Semesters(data.MI_Id, data.ASMAY_Id, data.AMCO_Id, data.AMB_Id);


                var semisterlist = (from a in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                    from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag && data.AMCO_Ids.Contains(b.AMCO_Id) && data.AMB_Ids.Contains(c.AMB_Id))
                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.fillsemester = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAmountEntryDTO selectacademicyear(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                data.temp = clgcomm.Get_Yearly_Course(data.MI_Id, data.ASMAY_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAmountEntryDTO selectsem(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                //data.fillstudent = clgcomm.Get_Yearly_Course(data.MI_Id, data.ASMAY_Id);


                var fillstudent = (from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                   from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from d in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                   from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                   from g in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                   from h in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                   from i in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                   from f in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                   where (f.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag 
                                   && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id 
                                   && d.ACAYCB_Id == c.ACAYCB_Id && d.ACAYCBS_ActiveFlag && data.AMCO_Ids.Contains(b.AMCO_Id) && data.AMB_Ids.Contains(c.AMB_Id) && data.AMSE_Ids.Contains(d.AMSE_Id)
                                   && b.AMCO_Id==e.AMCO_Id && g.AMB_Id==c.AMB_Id && h.AMSE_Id==d.AMSE_Id && i.ASMAY_Id==data.ASMAY_Id && i.ACYST_ActiveFlag==1 &&  i.AMCST_Id==f.AMCST_Id && b.AMCO_Id==i.AMCO_Id && c.AMB_Id==i.AMB_Id && d.AMSE_Id==i.AMSE_Id

                                   )
                                   select new CollegeConcessionDTO
                                   {
                                       AMCST_FirstName = f.AMCST_FirstName,
                                       AMCST_MiddleName = f.AMCST_MiddleName,
                                       AMCST_LastName = f.AMCST_LastName,
                                       //ACYST_RollNo = e.ACYST_RollNo,
                                       AMCST_AdmNo = f.AMCST_AdmNo,
                                       AMCST_Id = f.AMCST_Id,
                                       AMCST_RegistrationNo = f.AMCST_RegistrationNo,
                                       AMCO_CourseName=e.AMCO_CourseName,
                                       AMB_BranchName=g.AMB_BranchName,
                                       AMSE_SEMName=h.AMSE_SEMName
                                   }).Distinct().ToList();
                data.fillstudent = fillstudent.ToList().Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAmountEntryDTO getgroupmapped(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                List<CLG_Fee_Yearly_Group_Head_Mapping> commamtflag = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
                commamtflag = _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id).ToList();
                data.commountamountflag = commamtflag.ToArray();


                data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                        select new CLGFeeAmountEntryDTO
                                        {
                                            FMH_Id = a.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FMG_GroupName = b.FMG_GroupName,
                                            FMH_FeeName = c.FMH_FeeName,
                                            FMI_Name = e.FTI_Name,
                                            FCMAS_Amount = 0,
                                            FMH_Order = c.FMH_Order,
                                            FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                        }
  ).OrderByDescending(t => t.FMH_Order).ToArray();

                //feeamtentry.allgroupheaddata var p1
                data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                         from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                         from g in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                         from h in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                                         where (f.FMH_Id == c.FMH_Id && f.FMG_Id == b.FMG_Id && f.FTI_Id == e.FTI_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && data.AMCO_Ids.Contains(f.AMCO_Id) && data.AMB_Ids.Contains(f.AMB_Id) && data.AMSE_Ids.Contains(g.AMSE_Id) && d.FMI_Id == e.FMI_Id && f.FCMA_Id == g.FCMA_Id && g.FCMAS_Id == h.FCMAS_Id && a.ASMAY_Id == f.ASMAY_Id && g.FCMAS_Amount == 0)
                                         select new CLGFeeAmountEntryDTO
                                         {
                                             FCMAS_Id = g.FCMAS_Id,
                                             FMH_Id = a.FMH_Id,
                                             FTI_Id = e.FTI_Id,
                                             FMG_GroupName = b.FMG_GroupName,
                                             FMH_FeeName = c.FMH_FeeName,
                                             FMI_Name = e.FTI_Name,
                                             FCMAS_Amount = Convert.ToInt64(g.FCMAS_Amount),
                                             FCTDD_Day = h.FCTDD_Day,
                                             FCTDD_Month = h.FCTDD_Month,
                                             FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                         }
      ).OrderByDescending(t => t.FMH_Id).ToArray();


                data.fineslabdetails = (from a in _YearlyFeeGroupMappingContext.feeFS
                                        from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Fine_Slabs
                                        from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                        from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                        where (a.FMFS_Id == b.FMFS_Id && b.FCMAS_Id == d.FCMAS_Id && c.FCMA_Id == d.FCMA_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id && d.AMSE_Id == data.AMSE_Id)
                                        select new CLGFeeAmountEntryDTO
                                        {
                                            FCMAS_Id = d.FCMAS_Id,
                                            FCTFS_Amount = b.FCTFS_Amount,
                                            FMFS_FineType = a.FMFS_FineType,
                                            FMFS_ECSFlag = a.FMFS_ECSFlag,
                                            FCTFS_FineType = b.FCTFS_FineType,
                                            FMG_Id = c.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = c.FTI_Id,
                                            FMFS_FromDay = a.FMFS_FromDay,
                                            FMFS_ToDay = a.FMFS_ToDay,

                                        }
                    ).ToArray();

                List<MasterMonthDMO> mon = new List<MasterMonthDMO>();
                mon = _YearlyFeeGroupMappingContext.IVRM_Month.ToList();
                data.fillmonth = mon.ToArray();

                //           if (data.allgroupheaddata.Length <= 0)
                //           {
                //               //var p2
                //               data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                //                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                //                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                //                                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                //                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                //                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                //                                        select new CLGFeeAmountEntryDTO
                //                                        {
                //                                            FMH_Id = a.FMH_Id,
                //                                            FTI_Id = e.FTI_Id,
                //                                            FMG_GroupName = b.FMG_GroupName,
                //                                            FMH_FeeName = c.FMH_FeeName,
                //                                            FMI_Name = e.FTI_Name,
                //                                            FCMAS_Amount = 0,
                //                                            FMH_Order = c.FMH_Order,
                //                                            FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                //                                        }
                //).OrderByDescending(t => t.FMH_Order).ToArray();

                //           }
                var amco_ids = "";

                foreach (var x in data.AMCO_Ids)
                {
                    amco_ids += x + ",";
                }
                amco_ids = amco_ids.Substring(0, (amco_ids.Length - 1));


                var amb_ids = "";
                foreach (var x in data.AMB_Ids)
                {
                    amb_ids += x + ",";
                }
                amb_ids = amb_ids.Substring(0, (amb_ids.Length - 1));



                var amcst_ids = "";
                foreach (var x in data.AMCST_Ids)
                {
                    amcst_ids += x + ",";
                }
                amcst_ids = amcst_ids.Substring(0, (amcst_ids.Length - 1));


                //  var studentdata = _YearlyFeeGroupMappingContext.Clg_Fee_Studentwise_DMO.Where(t => t.MI_Id == data.MI_Id && amcst_ids.Contains(t.AMCST_Id.ToString()) && amco_ids.Contains(t.AMCO_Id.ToString()) && amb_ids.Contains(t.AMB_Id.ToString())).Select(s => s.FCSA_Id).ToList();


                var studentdata = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO

                                   from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   from c in _YearlyFeeGroupMappingContext.Clg_Fee_Studentwise_DMO
                                   where (c.FCMAS_Id == b.FCMAS_Id && b.FCMAS_Id == a.FCMA_Id && c.MI_Id == data.MI_Id && amcst_ids.Contains(c.AMCST_Id.ToString()) && amco_ids.Contains(c.AMCO_Id.ToString()) && amb_ids.Contains(c.AMB_Id.ToString()) && a.FMG_Id == data.FMG_Id && a.FCMA_ActiveFlg == true && b.FCMAS_ActiveFlg == true && c.FCSA_ActiveFlg == true && a.ASMAY_Id== data.ASMAY_Id)
                                   select c
                   ).ToList();


                var feegroupcount = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping

                                     from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                     from c in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                     from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                     where (a.FMI_Id == b.FMI_Id && c.FMI_Id == b.FMI_Id && a.FMG_Id == d.FMG_Id && a.FMH_Id == d.FMH_Id && d.FTI_Id == c.FTI_Id && d.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMG_Id == data.FMG_Id && b.FMI_ActiceFlag == true && d.FCMA_ActiveFlg == true && amco_ids.Contains(d.AMCO_Id.ToString()) && amb_ids.Contains(d.AMB_Id.ToString()))

                                     select a
            ).ToList();


                var semid = "0,";
                for (int i = 0; i < studentdata.Count; i++)
                {
                    semid += studentdata[i].AMSE_Id + ",";
                }
                if (semid.EndsWith(','))
                {
                    semid = semid.Trim(',');
                }
                if (studentdata.Count == feegroupcount.Count)
                {
                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StudentwiseAmountEntry_List";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.ASMAY_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_ID",
                         SqlDbType.VarChar)
                        {
                            Value = amco_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_ID",
                         SqlDbType.VarChar)
                        {
                            Value = amb_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_ID",
                         SqlDbType.VarChar)
                        {
                            Value = amcst_ids
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow1);

                                }
                            }
                            data.savedrecord = retObject.ToArray();
                            data.returnval = "student";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (studentdata.Count == 0)
                {
                    using (var cmd1 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd1.CommandText = "SP_Student_Wise_Amount_Entry";
                        cmd1.CommandType = CommandType.StoredProcedure;


                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@AMCO_Id",
                            SqlDbType.VarChar)
                        {
                            Value = amco_ids

                        });
                        cmd1.Parameters.Add(new SqlParameter("@AMB_Id",
                            SqlDbType.VarChar)
                        {
                            Value = amb_ids

                        });
                        cmd1.Parameters.Add(new SqlParameter("@FMG_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.FMG_Id

                        });



                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        var retObject1 = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd1.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.searchdatalist = retObject1.ToArray();
                            data.returnval = "amount";
                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StudentwiseAmountEntry_List";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.ASMAY_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_ID",
                         SqlDbType.VarChar)
                        {
                            Value = amco_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_ID",
                         SqlDbType.VarChar)
                        {
                            Value = amb_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_ID",
                         SqlDbType.VarChar)
                        {
                            Value = amcst_ids
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow1);

                                }
                            }
                            data.savedrecord = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd1 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd1.CommandText = "SP_Student_Wise_Amount_Entry_Semwise";
                        cmd1.CommandType = CommandType.StoredProcedure;


                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@AMCO_Id",
                            SqlDbType.VarChar)
                        {
                            Value = amco_ids

                        });
                        cmd1.Parameters.Add(new SqlParameter("@AMB_Id",
                            SqlDbType.VarChar)
                        {
                            Value = amb_ids

                        });
                        cmd1.Parameters.Add(new SqlParameter("@FMG_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.FMG_Id

                        });
                        cmd1.Parameters.Add(new SqlParameter("AMSE_Id",
                            SqlDbType.VarChar)
                        {
                            Value = semid
                        });



                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        var retObject1 = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd1.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.searchdatalist = retObject1.ToArray();

                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.returnval = "both";
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAmountEntryDTO fillslabde(CLGFeeAmountEntryDTO data)
        {
            try
            {
                List<FeeFineSlabDMO> head = new List<FeeFineSlabDMO>();
                head = _YearlyFeeGroupMappingContext.feeFS.Where(t => t.MI_Id == data.MI_Id && t.FMFS_ActiveFlag == true).ToList();
                data.fillslab = head.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAmountEntryDTO getalldetailsOnselectiontype(CLGFeeAmountEntryDTO data)
        {
            try
            {
                data.alldata = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FCMA_Id == e.FCMA_Id)
                                select new CLGFeeAmountEntryDTO
                                {
                                    FMG_GroupName = b.FMG_GroupName,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FCMAS_Amount = Convert.ToInt64(e.FCMAS_Amount),
                                    FMA_Id = a.FCMA_Id
                                }
          ).OrderBy(t => t.FMH_FeeName).ToArray();
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAmountEntryDTO savedata(CLGFeeAmountEntryDTO pgmod)
        {
            bool returnresult = false;
            CLGFeeAmountEntryDTO someObj = new CLGFeeAmountEntryDTO();
            try
            {
                CLGFeeAmountEntryDTO pgmodule = Mapper.Map<CLGFeeAmountEntryDTO>(pgmod);


                foreach (var std in pgmod.AMCST_Ids)
                {
                    foreach (var crse in pgmod.AMCO_Ids)
                    {
                        foreach (var brnch in pgmod.AMB_Ids)
                        {
                            //foreach (var sem in pgmod.AMSE_Ids)
                            //{
                            if (pgmod.savetmpdata[0].FCMAS_Id > 0)
                            {
                                var a = "";
                                if (pgmod.savetmpdata != null)
                                {
                                    int j = 0;
                                    while (j < pgmod.savetmpdata.Count())
                                    {
                                        //var feestutrans = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Where(t => t.ASMAY_Id == pgmod.ASMAY_Id && t.MI_Id == pgmod.MI_Id && t.FCMAS_Id == pgmod.savetmpdata[j].FCMAS_Id && t.FMG_Id == pgmod.FMG_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id).Select(t=>t.FCSS_Id).ToList();

                                        var feestutrans = _YearlyFeeGroupMappingContext.Clg_Fee_Studentwise_DMO.Where(t => t.MI_Id == pgmod.MI_Id && t.FCMAS_Id == pgmod.savetmpdata[j].FCMAS_Id && t.AMCST_Id == std && t.AMCO_Id == crse && t.AMSE_Id == pgmod.savetmpdata[j].AMSE_Id && t.AMB_Id == brnch).Select(s => s.FCSA_Id).ToList();


                                        Clg_Fee_Studentwise_DMO pmm = new Clg_Fee_Studentwise_DMO();
                                        Fee_College_Student_StatusDMO status = new Fee_College_Student_StatusDMO();
                                        if (feestutrans.Count() > 0)

                                        {
                                            var result_obj2 = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.FMG_Id == pgmod.FMG_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id && t.FCMAS_Id == pgmod.savetmpdata[j].FCMAS_Id);


                                            //var studentamount = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.FMG_Id == pgmod.FMG_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id).Sum(t => t.FCSS_PaidAmount);


                                            //if (Convert.ToInt64(studentamount) > 0)
                                            //{
                                            //    pgmod.amtentrystatus = "student already made payment";
                                            //}

                                            //else
                                            //{
                                            pmm.MI_Id = pgmod.MI_Id;
                                            pmm.AMCST_Id = std;
                                            pmm.AMCO_Id = crse;
                                            pmm.AMB_Id = brnch;
                                            // pmm.AMSE_Id = sem;
                                            pmm.AMSE_Id = pgmod.savetmpdata[j].AMSE_Id;
                                            pmm.FCMAS_Id = pgmod.savetmpdata[j].FCMAS_Id;
                                            pmm.FCSA_Amount = pgmod.savetmpdata[j].FCMAS_Amount;
                                            pmm.FCSA_Currency = pgmod.Currencyfactor;
                                            pmm.FCSA_ActiveFlg = true;
                                            pmm.CreatedDate = DateTime.Now;
                                            pmm.UpdatedDate = DateTime.Now;
                                            _YearlyFeeGroupMappingContext.Update(pmm);




                                            if (result_obj2.FCSS_PaidAmount == 0)
                                            {
                                                result_obj2.FCSS_CurrentYrCharges = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);
                                                result_obj2.FCSS_ToBePaid = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);
                                                result_obj2.FCSS_NetAmount = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);
                                                result_obj2.FCSS_CurrentYrCharges = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);
                                            }


                                            _YearlyFeeGroupMappingContext.Update(result_obj2);


                                            var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                            if (contactExists == 1)
                                            {
                                                pgmod.amtentrystatus = "Updated";
                                            }
                                            else
                                            {
                                                pgmod.amtentrystatus = "Update Failed";
                                            }
                                            // }

                                        }

                                        else
                                        {

                                            pmm.MI_Id = pgmod.MI_Id;
                                            pmm.AMCST_Id = std;
                                            pmm.AMCO_Id = crse;
                                            pmm.AMB_Id = brnch;
                                            //pmm.AMSE_Id = sem;
                                            pmm.AMSE_Id = pgmod.savetmpdata[j].AMSE_Id;
                                            pmm.FCMAS_Id = pgmod.savetmpdata[j].FCMAS_Id;
                                            pmm.FCSA_Amount = pgmod.savetmpdata[j].FCMAS_Amount;
                                            pmm.FCSA_Currency = pgmod.Currencyfactor;
                                            pmm.FCSA_ActiveFlg = true;
                                            pmm.CreatedDate = DateTime.Now;
                                            pmm.UpdatedDate = DateTime.Now;

                                            _YearlyFeeGroupMappingContext.Add(pmm);


                                            status.MI_Id = pgmod.MI_Id;
                                            status.ASMAY_Id = pgmod.ASMAY_Id;
                                            status.AMCST_Id = std;
                                            status.FMG_Id = pgmod.FMG_Id;
                                            status.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                            status.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                            status.FCMAS_Id = pgmod.savetmpdata[j].FCMAS_Id;
                                            status.FCSS_OBArrearAmount = 0;
                                            status.FCSS_OBExcessAmount = 0;
                                            status.FCSS_CurrentYrCharges = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);
                                            status.FCSS_TotalCharges = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);
                                            status.FCSS_ConcessionAmount = 0;
                                            status.FCSS_WaivedAmount = 0;
                                            status.FCSS_ToBePaid = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);
                                            status.FCSS_PaidAmount = 0;
                                            status.FCSS_RefundableAmount = 0;
                                            status.FCSS_ExcessPaidAmount = 0;
                                            status.FCSS_ExcessAmountAdjusted = 0;
                                            status.FCSS_RunningExcessAmount = 0;
                                            status.FCSS_AdjustedAmount = 0;
                                            status.FCSS_RebateAmount = 0;
                                            status.FCSS_FineAmount = 0;
                                            status.FCSS_RefundAmount = 0;
                                            status.FCSS_RefundAmountAdjusted = 0;
                                            status.FCSS_NetAmount = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);
                                            status.FCSS_ChequeBounceFlg = false;
                                            status.FCSS_ArrearFlag = false;
                                            status.FCSS_RefundOverFlag = false;
                                            status.FCSS_ActiveFlag = true;
                                            status.User_Id = pgmod.user_id;
                                            status.FCSS_CurrentYrCharges = Convert.ToInt64(pgmod.savetmpdata[j].FCMAS_Amount);

                                            _YearlyFeeGroupMappingContext.Add(status);

                                            var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                            if (contactExists >= 1)
                                            {
                                                pgmod.amtentrystatus = "Saved";
                                            }
                                            else
                                            {
                                                pgmod.amtentrystatus = "Not Saved";
                                            }
                                        }
                                        j++;
                                    }
                                }
                                //int r = 0;
                                //while (r < pgmod.savefineslabreg.Count())
                                //{
                                //    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                                //    {
                                //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("CLG_Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.AMCO_Id, pgmod.AMB_Id, pgmod.AMSE_Id);
                                //    }
                                //    r++;
                                //}

                                //r = 0;

                            }

                        }
                    }

                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                 from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                 where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FCMA_Id == e.FCMA_Id)
                                 select new CLGFeeAmountEntryDTO
                                 {
                                     FMG_GroupName = b.FMG_GroupName,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FTI_Name = d.FTI_Name,
                                     FCMAS_Amount = Convert.ToInt64(e.FCMAS_Amount)
                                 }
                    ).ToArray();




                pgmod.savedrecord = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_Studentwise_DMO
                                     from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                     from c in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                     from e in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                     from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                     from g in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                     from h in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                     from i in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                     from j in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                     from k in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                     where (e.AMCST_Id == c.AMCST_Id && c.AMCO_Id == g.AMCO_Id && c.AMB_Id == f.AMB_Id && c.AMSE_Id == h.AMSE_Id && c.AMCST_Id == a.AMCST_Id && i.FCMAS_Id == a.FCMAS_Id && i.FCMA_Id == j.FCMA_Id && c.AMCST_Id == k.AMCST_Id && b.FMH_Id == j.FMH_Id && c.ASMAY_Id == pgmod.ASMAY_Id && k.MI_Id == pgmod.MI_Id)
                                     select new CollegeConcessionDTO
                                     {

                                         AMCST_FirstName = e.AMCST_FirstName,
                                         AMCST_MiddleName = e.AMCST_MiddleName,
                                         AMCST_LastName = e.AMCST_LastName,
                                         ACYST_RollNo = c.ACYST_RollNo,
                                         AMCST_AdmNo = e.AMCST_AdmNo,
                                         AMCST_Id = a.AMCST_Id,
                                         AMCST_RegistrationNo = e.AMCST_RegistrationNo,
                                         FMH_FeeName = b.FMH_FeeName,
                                         FMH_Id = k.FMH_Id,
                                         AMB_BranchName = f.AMB_BranchName,
                                         AMSE_SEMName = h.AMSE_SEMName,
                                         AMCO_CourseName = g.AMCO_CourseName,
                                         FCSA_Amount = a.FCSA_Amount,
                                         FCSA_Id = a.FCSA_Id,
                                         FCMAS_Id = k.FCMAS_Id
                                     }
                                ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
                pgmod.returnval = "false";
            }

            return pgmod;
        }
        public CLGFeeAmountEntryDTO deleterec(CLGFeeAmountEntryDTO data)
        {
            try
            {
                using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                {
                    List<Clg_Fee_Studentwise_DMO> lorgduedate = new List<Clg_Fee_Studentwise_DMO>();

                    var lorg = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_Studentwise_DMO

                                where (a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && a.FCSA_Id == data.FCSA_Id)
                                select a).ToList();

                    //var lorg = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_Studentwise_DMO
                    //            from b in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                    //            where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && a.FCSA_Id == data.FCSA_Id && b.ASMAY_Id == data.ASMAY_Id
                    //            && b.AMCST_Id == data.AMCST_Id)
                    //            select b).ToList();

                    lorgduedate = _YearlyFeeGroupMappingContext.Clg_Fee_Studentwise_DMO.Where(t => t.FCSA_Id.Equals(data.FCSA_Id)).ToList();


                    //  var studentamount = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FCMAS_Id == data.FCMAS_Id && t.AMCST_Id == data.AMCST_Id).Select(s => s.FCSS_PaidAmount).Sum();

                    try
                    {
                        //if (Convert.ToInt64(studentamount) > 0)
                        //{
                        //    data.returnval = "Student Already Paid";
                        //}

                        //else
                        //{

                        if (lorg.Any())
                        {
                            foreach (var x in lorg)
                            {
                                _YearlyFeeGroupMappingContext.Remove(x);
                            }

                            _YearlyFeeGroupMappingContext.Remove(lorgduedate.ElementAt(0));
                            var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                            if (contactExists >= 1)
                            {
                                data.returnval = "true";
                                transaction.Commit();
                            }
                            else
                            {
                                data.returnval = "false";
                            }
                        }
                        //}
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                        _logger.LogError(ee.Message);
                        data.returnval = "false";
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        // Holy- Cross 14-03-2024


        public CLGFeeAmountEntryDTO getmappedconcessionheads(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {

                var amco_ids = "";

                foreach (var x in data.AMCO_Ids)
                {
                    amco_ids += x + ",";
                }
                amco_ids = amco_ids.Substring(0, (amco_ids.Length - 1));


                var amb_ids = "";
                foreach (var x in data.AMB_Ids)
                {
                    amb_ids += x + ",";
                }
                amb_ids = amb_ids.Substring(0, (amb_ids.Length - 1));

                var sem_ids = "";
                foreach (var x in data.AMSE_Ids)
                {
                    sem_ids += x + ",";
                }
                sem_ids = sem_ids.Substring(0, (sem_ids.Length - 1));


                var amcst_ids = "";
                foreach (var x in data.AMCST_Ids)
                {
                    amcst_ids += x + ",";
                }
                amcst_ids = amcst_ids.Substring(0, (amcst_ids.Length - 1));


                data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                         from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                         from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from d in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                         where (a.MI_Id == b.MI_Id && a.FCMA_Id == b.FCMA_Id && a.MI_Id == c.MI_Id && a.FMG_Id == c.FMG_Id && a.MI_Id == d.MI_Id && a.FMH_Id == d.FMH_Id   && a.FTI_Id == e.FTI_Id && a.FMG_Id == f.FMG_Id && a.FMH_Id == f.FMH_Id && a.FTI_Id == f.FTI_Id && b.FCMAS_Id == f.FCMAS_Id && a.MI_Id == d.MI_Id  && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id  &&  amcst_ids.Contains(f.AMCST_Id.ToString()) && amco_ids.Contains(a.AMCO_Id.ToString()) &&  amb_ids.Contains(a.AMB_Id.ToString()) && sem_ids.Contains(b.AMSE_Id.ToString()) && d.FMH_Flag == "LS")
                                         select new CLGFeeAmountEntryDTO
                                         {
                                             FMH_Id = a.FMH_Id,
                                             FTI_Id = e.FTI_Id,
                                             FMG_Id = c.FMG_Id,
                                             FMG_GroupName = c.FMG_GroupName,
                                             FMH_FeeName = d.FMH_FeeName,
                                             FMI_Name = e.FTI_Name,
                                             FCMAS_Id = f.FCMAS_Id,
                                             FCMAS_Amount = f.FCSS_NetAmount,
                                             FMH_Order = d.FMH_Order,

                                         }
).OrderByDescending(t => t.FMH_Order).ToArray();

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CLGFeeAmountEntryDTO savescholershpheaddata(CLGFeeAmountEntryDTO data)
        {
            
            try
            {
                decimal totalamount = 0;

                foreach (var i in data.savetmpdata)
                {
                    totalamount = totalamount + i.FCMAS_Amount;

                    var collegestatus = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id &&
                    a.FMG_Id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.FTI_Id == i.FTI_Id && a.FCMAS_Id == i.FCMAS_Id && a.FCSS_NetAmount == 0).ToList();

                    if (collegestatus.Count > 0)
                    {
                        var result_data = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id &&
                        a.FMG_Id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.FTI_Id == i.FTI_Id && a.FCMAS_Id == i.FCMAS_Id);


                        result_data.FCSS_NetAmount = Convert.ToInt64(i.FCMAS_Amount);
                        _YearlyFeeGroupMappingContext.Update(result_data);

                    }

                }

                var TransactionExists = _YearlyFeeGroupMappingContext.SaveChanges();

                    if(TransactionExists >= 1 && totalamount > 0)
                    {

                        var checktutionfeedata = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                                  from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                  where (a.MI_Id == b.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.FMH_Id == b.FMH_Id && b.FMH_ActiveFlag == true && b.FMH_Flag == "TU" && a.FCSS_ToBePaid > 0)
                                                  select a).ToList();

                        if(checktutionfeedata.Count > 0)
                        {
                            var result_tutiondata = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                                      from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                      where (a.MI_Id == b.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.FMH_Id == b.FMH_Id && b.FMH_ActiveFlag == true && b.FMH_Flag == "TU" && a.FCSS_ToBePaid > 0)
                                                      select a).FirstOrDefault();


                            result_tutiondata.FCSS_ConcessionAmount = Convert.ToInt64(totalamount);
                            result_tutiondata.FCSS_ToBePaid = result_tutiondata.FCSS_ToBePaid - Convert.ToInt64(totalamount);

                            _YearlyFeeGroupMappingContext.Update(result_tutiondata);

                            var tutionfeetransactioncount = _YearlyFeeGroupMappingContext.SaveChanges();

                            if(tutionfeetransactioncount > 0)
                            {
                                data.returnval = "updated";
                            }


                        }



                    }




                

//                data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
//                                         from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
//                                         from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
//                                         from d in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
//                                         from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
//                                         from f in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
//                                         where (a.MI_Id == b.MI_Id && a.FCMA_Id == b.FCMA_Id && a.MI_Id == c.MI_Id && a.FMG_Id == c.FMG_Id && a.MI_Id == d.MI_Id && a.FMH_Id == d.FMH_Id && a.FTI_Id == e.FTI_Id && a.FMG_Id == f.FMG_Id && a.FMH_Id == f.FMH_Id && a.FTI_Id == f.FTI_Id && b.FCMAS_Id == f.FCMAS_Id && a.MI_Id == d.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && amcst_ids.Contains(f.AMCST_Id.ToString()) && amco_ids.Contains(a.AMCO_Id.ToString()) && amb_ids.Contains(a.AMB_Id.ToString()) && sem_ids.Contains(b.AMSE_Id.ToString()) && d.FMH_Flag == "LS")
//                                         select new CLGFeeAmountEntryDTO
//                                         {
//                                             FMH_Id = a.FMH_Id,
//                                             FTI_Id = e.FTI_Id,
//                                             FMG_Id = c.FMG_Id,
//                                             FMG_GroupName = c.FMG_GroupName,
//                                             FMH_FeeName = d.FMH_FeeName,
//                                             FMI_Name = e.FTI_Name,
//                                             FCMAS_Id = f.FCMAS_Id,
//                                             FCMAS_Amount = f.FCSS_NetAmount,
//                                             FMH_Order = d.FMH_Order,

//                                         }
//).OrderByDescending(t => t.FMH_Order).ToArray();

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}
