using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeOpeningBalanceImpl : interfaces.FeeOpeningBalanceInterface
    {


        public FeeGroupContext _FeeGroupContext;
        public FeeOpeningBalanceImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public FeeOpeningBalanceDTO getdata123(FeeOpeningBalanceDTO data)
        {
            try
            {               
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active==true ).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.acayear = year.Distinct().ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_ID && t.ASMCL_ActiveFlag==true).OrderBy(t => t.ASMCL_ClassCode).ToList();
                data.classlist = allclas.Distinct().ToArray();

                List<FeeClassCategoryDMO> allcategory = new List<FeeClassCategoryDMO>();
                allcategory = _FeeGroupContext.FeeClassCategoryDMO.Where(c => c.MI_Id == data.MI_ID && c.FMCC_ActiveFlag == true).ToList();
                data.Class_Category_List = allcategory.GroupBy(c => c.FMCC_ClassCategoryName).Select(c => c.First()).ToArray();

                //data.fillmastergroup = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //                       from b in _FeeGroupContext.feeGroup
                //                       from f in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                //                       from g in _FeeGroupContext.FeeAmountEntryDMO
                //                       where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == data.MI_ID && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id)// && f.User_Id == data.user_id
                //                        select new FeeStudentGroupMappingDTO
                //                       {
                //                           FMG_Id = a.FMG_Id,
                //                           FMG_GroupName = b.FMG_GroupName
                //                       }).Distinct().ToArray();
                data.fillmasterhead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       from b in _FeeGroupContext.feeGroup
                                       from c in _FeeGroupContext.feehead
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && c.FMH_RefundFlag == true)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();



                data.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                  from b in _FeeGroupContext.feeOpeningBalance
                                                  from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                  from d in _FeeGroupContext.admissioncls
                                                  from e in _FeeGroupContext.school_M_Section
                                                      // from f in _FeeGroupContext.feeGroup
                                                      // from g in _FeeGroupContext.feehead
                                                      // from h in _FeeGroupContext.FeeInstallmentsyearlyDMO b.FMG_Id==f.FMG_Id && b.FMH_Id==g.FMH_Id && b.FTI_Id==h.FTI_Id && 
                                                  where ( a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id )
                                                  select new FeeOpeningBalanceDTO
                                                  {
                                                      Amst_Id = a.AMST_Id,
                                                      AMST_FirstName = a.AMST_FirstName,
                                                      AMST_MiddleName = a.AMST_MiddleName,
                                                      AMST_LastName = a.AMST_LastName,
                                                      FMOB_Id = b.FMOB_Id,
                                                      asmay_id = b.ASMAY_Id,
                                                      FMOB_EntryDate = b.FMOB_EntryDate,
                                                      FMOB_Student_Due = b.FMOB_Student_Due,
                                                      FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                      ASMCL_ClassName = d.ASMCL_ClassName,
                                                      ASMC_SectionName = e.ASMC_SectionName,
                                                      //FMG_GroupName = f.FMG_GroupName,
                                                      //FMH_FeeName = g.FMH_FeeName,
                                                      //FTI_Name = h.FTI_Name,
                                                  }
                             ).Distinct().OrderByDescending(t=>t.FMOB_Id).ToArray();



                

                                 data.reportdatelist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                        from b in _FeeGroupContext.PDA_StatusDMO
                                                        from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                        from d in _FeeGroupContext.admissioncls
                                                        from e in _FeeGroupContext.school_M_Section
                                                                   where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == data.asmay_id)
                                                                   select new FeeOpeningBalanceDTO
                                                                   {
                                                                       Amst_Id = a.AMST_Id,
                                                                       AMST_FirstName = a.AMST_FirstName,
                                                                       AMST_MiddleName = a.AMST_MiddleName,
                                                                       AMST_LastName = a.AMST_LastName,
                                                                       FMOB_Id = b.PDAS_Id,
                                                                       asmay_id = b.ASMAY_Id,
                                                                     //  FMOB_EntryDate = b.FMOB_EntryDate,
                                                                       FMOB_Student_Due = b.PDAS_OBStudentDue,
                                                                       FMOB_Institution_Due = b.PDAS_OBExcessPaid,
                                                                       ASMCL_ClassName = d.ASMCL_ClassName,
                                                                       ASMC_SectionName = e.ASMC_SectionName,
                                                                     
                                                                   }
                             ).Distinct().OrderByDescending(t => t.FMOB_Id).ToArray();

                //Term addition for stthomas defaulter report

                data.busroutelist = _FeeGroupContext.feeTr.Where(t => t.MI_Id == data.MI_ID && t.FMT_ActiveFlag == true).ToArray();





            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeOpeningBalanceDTO getclshead(FeeOpeningBalanceDTO data)
        {
            try
            {
                if(data.ASMCL_Id > 0)
                {
                    data.sectionlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        from b in _FeeGroupContext.school_M_Section
                                        where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && a.ASMCL_Id == data.ASMCL_Id && b.ASMC_ActiveFlag == 1)
                                        select new FeeStudentAdjustmentDTO
                                        {
                                            ASMS_Id = a.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                        }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                }
                else
                {
                    data.sectionlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        from b in _FeeGroupContext.school_M_Section
                                        where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id  && b.ASMC_ActiveFlag == 1)
                                        select new FeeStudentAdjustmentDTO
                                        {
                                            ASMS_Id = a.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                        }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                }
                

                data.fillmasterhead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       from b in _FeeGroupContext.feeGroup
                                       from c in _FeeGroupContext.feehead
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && c.FMH_RefundFlag == true )
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeOpeningBalanceDTO getgroup(FeeOpeningBalanceDTO data)
        {
            try
            {
                //data.fillmasterhead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //                      from b in _FeeGroupContext.feeGroup
                //                      from c in _FeeGroupContext.feehead
                //                      where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && a.FMG_Id==data.FMG_Id)
                //                      select new FeeStudentGroupMappingDTO
                //                      {
                //                          FMG_Id = b.FMG_Id,
                //                          FMH_Id = c.FMH_Id,
                //                          FMH_FeeName = c.FMH_FeeName
                //                      }).Distinct().ToArray();
                data.fillinstallment = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                        from b in _FeeGroupContext.feeGroup
                                        from c in _FeeGroupContext.feehead
                                        from d in _FeeGroupContext.FeeInstallmentDMO
                                        from e in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == data.MI_ID && c.MI_Id == data.MI_ID  && d.MI_Id == data.MI_ID  && e.FMI_Id == a.FMI_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id )
                                        select new FeeStudentGroupMappingDTO
                                        {                                          
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeOpeningBalanceDTO gethead(FeeOpeningBalanceDTO data)
        {
            try
            {
                data.fillmastergroup = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                        from b in _FeeGroupContext.feeGroup
                                        where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && a.FMH_Id==data.FMH_Id )// 
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = b.FMG_GroupName
                                        }).Distinct().ToArray();
                //if(data.fillmastergroup.Length==1)
                //{
                //    data.FMG_Id=((FeeOpeningBalanceDTO)data.fillmastergroup.GetValue(0)).FMG_Id;
                    
                //    data.fillinstallment = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //                            from b in _FeeGroupContext.feeGroup
                //                            from c in _FeeGroupContext.feehead
                //                            from d in _FeeGroupContext.FeeInstallmentDMO
                //                            from e in _FeeGroupContext.FeeInstallmentsyearlyDMO
                //                            where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == data.MI_ID && b.FMG_ActiceFlag == true && c.MI_Id == data.MI_ID && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_ID && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id)
                //                            select new FeeStudentGroupMappingDTO
                //                            {
                //                                FMG_Id = b.FMG_Id,
                //                                FMH_Id = c.FMH_Id,
                //                                FTI_Id = e.FTI_Id,
                //                                FTI_Name = e.FTI_Name
                //                            }).Distinct().ToArray();
                //}               

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeOpeningBalanceDTO getreport(FeeOpeningBalanceDTO data)
        {
            try
            {
                string ftiid = "0";
                foreach (FeeOpeningBalanceDTO x in data.TempararyArrayhEADListnew)
                {
                    ftiid = ftiid + ',' + x.FTI_Id;
                }
                    List<FeeOpeningBalanceDTO> result = new List<FeeOpeningBalanceDTO>();
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "Fee_Opening_Balance_Gridview";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Type",
                      SqlDbType.VarChar)
                    {
                        Value = data.typeofrpt
                    });
                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Amay_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmay_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Asmcl_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.fillclasflg
                    });
                    cmd.Parameters.Add(new SqlParameter("@Asms_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.fillseccls
                    });
                    cmd.Parameters.Add(new SqlParameter("@Amst_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fmcC_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.fmcC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status",
                   SqlDbType.VarChar)
                    {
                        Value = data.studenttype
                    });
                    cmd.Parameters.Add(new SqlParameter("@fmgid",
                                          SqlDbType.BigInt)
                    {
                        Value = data.FMG_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fmhid",
                     SqlDbType.BigInt)
                    {
                        Value = data.FMH_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ftiid",
                     SqlDbType.VarChar)
                    {
                        Value = ftiid
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
                   SqlDbType.BigInt)
                    {
                        Value = data.userid
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
                                result.Add(new FeeOpeningBalanceDTO
                                {
                                    Amst_Id = Convert.ToInt64(dataReader["ID"].ToString()),
                                    AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                    AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                                    AMST_LastName = dataReader["AMST_LastName"].ToString(),
                                    FMOB_Student_Due = Convert.ToDecimal(dataReader["FMOB_Student_Due"].ToString()),
                                    FMOB_Institution_Due = Convert.ToDecimal(dataReader["FMOB_Institution_Due"].ToString()),
                                });                                
                            }
                            data.saveheadlst = result.Distinct().GroupBy(s => s.Amst_Id).Select(s => s.First()).ToArray();
                        }
                        data.returntxt = "Saved";
                        foreach (var a in data.saveheadlst)
                        {
                            if (a.FMOB_Student_Due > 0 || a.FMOB_Institution_Due > 0)
                            {
                                data.returntxt = "Updated";
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeOpeningBalanceDTO getlisttwo(FeeOpeningBalanceDTO stu)
        {
            FeeOpeningBalanceDTO acdmc = new FeeOpeningBalanceDTO();
            try
            {
                //int fsscount = 0;

                if (stu.fillbusroutestudents == "fees")
                {
                    if (stu.savetmpdata.Count() > 0)
                    {
                        foreach (FeeOpeningBalanceDTO ph in stu.savetmpdata)
                        {
                            foreach (FeeOpeningBalanceDTO x in stu.TempararyArrayhEADListnew)
                            {



                                if (ph.checkedvalue == true)
                                {

                                    var FMCC_Idnew = (from a in _FeeGroupContext.feeYCC
                                                      from b in _FeeGroupContext.feeYCCC
                                                      from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                      where (a.FYCC_Id == b.FYCC_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == stu.asmay_id && a.MI_Id == stu.MI_ID && b.ASMCL_Id == c.ASMCL_Id && c.AMST_Id == ph.Amst_Id && c.ASMAY_Id == stu.asmay_id)
                                                      select a.FMCC_Id).FirstOrDefault();
                                    if (FMCC_Idnew == 0)
                                    {
                                        stu.returntxt = "nocategoryandamountentry";
                                    }
                                    else
                                    {
                                        var FMAlist = (from a in _FeeGroupContext.FeeAmountEntryDMO
                                                       from b in _FeeGroupContext.FeeStudentTransactionDMO
                                                       where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == stu.MI_ID && a.ASMAY_Id == stu.asmay_id && a.FMG_Id == stu.FMG_Id && a.FMH_Id == stu.FMH_Id && a.FTI_Id == x.FTI_Id && b.AMST_Id == ph.Amst_Id && a.FMCC_Id == FMCC_Idnew)//((a.FMA_Amount >= 0 && c.FMH_Flag != "F" && c.FMH_Flag != "E") || (a.FMA_Amount >= 0 && (c.FMH_Flag == "F" || c.FMH_Flag == "E"))) && a.FMCC_Id == FMCC_Idnew
                                                       select a.FMA_Id).FirstOrDefault();
                                        if (FMAlist == 0)
                                        {
                                            stu.returntxt = "nocategoryandamountentry";
                                        }
                                        else
                                        {
                                            var fssnew = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                                          where (a.AMST_Id == ph.Amst_Id && a.FMG_Id == stu.FMG_Id && a.FMH_Id == stu.FMH_Id && a.FTI_Id == x.FTI_Id && a.MI_Id == stu.MI_ID && a.ASMAY_Id == stu.asmay_id && a.User_Id == stu.userid && a.FMA_Id == FMAlist && a.FSS_PaidAmount > 0)
                                                          select a.FSS_Id).FirstOrDefault();
                                            if (fssnew > 0)
                                            {
                                                if (stu.filterrefund == "Refunable")
                                                {
                                                    stu.returntxt = "contactadministrator";//this is for paidamount >0 for refunable which should not alter now
                                                }
                                            }
                                            if (stu.returntxt != "contactadministrator")
                                            {
                                                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "Fee_Opening_Balance_Report";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@miid",
                                                  SqlDbType.BigInt)
                                                    {
                                                        Value = stu.MI_ID
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@amstid",
                                                     SqlDbType.BigInt)
                                                    {
                                                        Value = ph.Amst_Id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@asmayid",
                                                      SqlDbType.BigInt)
                                                    {
                                                        Value = stu.asmay_id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@fmgid",
                                                      SqlDbType.BigInt)
                                                    {
                                                        Value = stu.FMG_Id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@fmhid",
                                                     SqlDbType.BigInt)
                                                    {
                                                        Value = stu.FMH_Id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@ftiid",
                                                     SqlDbType.BigInt)
                                                    {
                                                        Value = x.FTI_Id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@fmaid",
                                           SqlDbType.VarChar)
                                                    {
                                                        Value = FMAlist
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@fmobentrydate",
                                                  SqlDbType.DateTime)
                                                    {
                                                        Value = stu.FMOB_EntryDate
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@fmobstud_due",
                                                  SqlDbType.Decimal)
                                                    {
                                                        Value = ph.FMOB_Student_Due
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@fmobinst_due",
                                                  SqlDbType.Decimal)
                                                    {
                                                        Value = ph.FMOB_Institution_Due
                                                    });

                                                    cmd.Parameters.Add(new SqlParameter("@userid",
                                              SqlDbType.Decimal)
                                                    {
                                                        Value = stu.userid
                                                    });

                                                    cmd.Parameters.Add(new SqlParameter("@refund",
                                            SqlDbType.VarChar)
                                                    {
                                                        Value = stu.filterrefund
                                                    });

                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();

                                                    var count = cmd.ExecuteNonQuery();
                                                    if (count >= 2)
                                                    {
                                                        stu.returnval = true;
                                                    }
                                                    else
                                                    {
                                                        stu.returnval = false;
                                                    }
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                else
                {
                    if (stu.savetmpdata.Count() > 0)
                    {
                        foreach (FeeOpeningBalanceDTO ph in stu.savetmpdata)
                        {
                            

                                if (ph.checkedvalue == true)
                                {
                                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "PDA_Opening_Balance";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@miid",
                                      SqlDbType.BigInt)
                                        {
                                            Value = stu.MI_ID
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@amstid",
                                         SqlDbType.BigInt)
                                        {
                                            Value = ph.Amst_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                                          SqlDbType.BigInt)
                                        {
                                            Value = stu.asmay_id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@fmobstud_due",
                                      SqlDbType.Decimal)
                                        {
                                            Value = ph.FMOB_Student_Due
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@fmobinst_due",
                                      SqlDbType.Decimal)
                                        {
                                            Value = ph.FMOB_Institution_Due
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@userid",
                                  SqlDbType.Decimal)
                                        {
                                            Value = stu.userid
                                        });

                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();

                                        var count = cmd.ExecuteNonQuery();
                                        if (count >= 1)
                                        {
                                            stu.returnval = true;
                                        }
                                        else
                                        {
                                            stu.returnval = false;
                                        }
                                    }
                                }
                          
                        }
                    }
                }
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }
        public FeeOpeningBalanceDTO getstuddet(FeeOpeningBalanceDTO data)
        {
            if (data.filterinitialdata == null)
            {
                data.filterinitialdata = "NameRegNo";
            }

            try
            {
                if (data.studenttype == "Active")
                {
                    if (data.filterinitialdata == "NameRegNo")
                    {

                        if (data.fillclasflg != null && data.fillseccls != null && data.fillclasflg != 0 && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                   && b.ASMCL_Id == data.fillclasflg && b.ASMS_Id == data.fillseccls && a.AMST_SOL == "S" && b.AMAY_ActiveFlag==1)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName==null?"": a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().OrderBy(t=>t.AMST_FirstName).ToArray();

                        }
                        else if (data.fillclasflg != null && data.fillclasflg != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                      && b.ASMCL_Id == data.fillclasflg && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

                        }
                        else if (data.fillseccls != null && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                       && b.ASMS_Id == data.fillseccls && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                        AMST_FirstName = a.AMST_FirstName==null?"": a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }
                                 ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

                        }
                        else
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }
                                 ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        }

                    }
                    else if (data.filterinitialdata == "RegNoName")
                    {

                        if (data.fillclasflg != null && data.fillseccls != null && data.fillclasflg != 0 && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                    && b.ASMCL_Id == data.fillclasflg && b.ASMS_Id == data.fillseccls && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" :  a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

                        }
                        else if (data.fillclasflg != null && data.fillclasflg != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                      && b.ASMCL_Id == data.fillclasflg && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        }
                        else if (data.fillseccls != null && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                       && b.ASMS_Id == data.fillseccls && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        }
                        else
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        }
                    }
                }

                else if (data.studenttype == "Left")
                {
                    if (data.filterinitialdata == "NameRegNo")
                    {

                        if (data.fillclasflg != null && data.fillseccls != null && data.fillclasflg != 0 && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                   && b.ASMCL_Id == data.fillclasflg && b.ASMS_Id == data.fillseccls && a.AMST_SOL == "L" && b.AMAY_ActiveFlag == 0)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().ToArray();

                        }
                        else if (data.fillclasflg != null && data.fillclasflg != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                      && b.ASMCL_Id == data.fillclasflg && a.AMST_SOL == "L" && b.AMAY_ActiveFlag == 0)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().ToArray();

                        }
                        else if (data.fillseccls != null && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                       && b.ASMS_Id == data.fillseccls && a.AMST_SOL == "L" && b.AMAY_ActiveFlag == 0)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().ToArray();

                        }
                        else
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "L" && b.AMAY_ActiveFlag == 0)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().ToArray();
                        }

                    }
                    else if (data.filterinitialdata == "RegNoName")
                    {

                        if (data.fillclasflg != null && data.fillseccls != null && data.fillclasflg != 0 && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                    && b.ASMCL_Id == data.fillclasflg && b.ASMS_Id == data.fillseccls && a.AMST_SOL == "L" && b.AMAY_ActiveFlag == 0)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().ToArray();

                        }
                        else if (data.fillclasflg != null && data.fillclasflg != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                      && b.ASMCL_Id == data.fillclasflg && a.AMST_SOL == "L" && b.AMAY_ActiveFlag == 0)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().ToArray();
                        }
                        else if (data.fillseccls != null && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                       && b.ASMS_Id == data.fillseccls && a.AMST_SOL == "L" && b.AMAY_ActiveFlag == 0)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().ToArray();
                        }
                        else
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "L" && b.AMAY_ActiveFlag == 0)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().ToArray();
                        }


                    }
                }
                else
                {
                    if (data.filterinitialdata == "NameRegNo")
                    {

                        if (data.fillclasflg != null && data.fillseccls != null && data.fillclasflg != 0 && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                   && b.ASMCL_Id == data.fillclasflg && b.ASMS_Id == data.fillseccls)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().ToArray();

                        }
                        else if (data.fillclasflg != null && data.fillclasflg != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                      && b.ASMCL_Id == data.fillclasflg  )
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().ToArray();

                        }
                        else if (data.fillseccls != null && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                       && b.ASMS_Id == data.fillseccls )
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().ToArray();
                        }
                        else
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id )
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_RegistrationNo,
                                                   }).Distinct().ToArray();
                        }

                    }
                    else if (data.filterinitialdata == "RegNoName")
                    {

                        if (data.fillclasflg != null && data.fillseccls != null && data.fillclasflg != 0 && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                    && b.ASMCL_Id == data.fillclasflg && b.ASMS_Id == data.fillseccls)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().ToArray();

                        }
                        else if (data.fillclasflg != null && data.fillclasflg != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                      && b.ASMCL_Id == data.fillclasflg )
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().ToArray();
                        }
                        else if (data.fillseccls != null && data.fillseccls != 0)
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                                       && b.ASMS_Id == data.fillseccls)
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().ToArray();
                        }
                        else
                        {
                            data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                   from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id )
                                                   select new FeeOpeningBalanceDTO
                                                   {
                                                       Amst_Id = a.AMST_Id,
                                                       AMST_FirstName = a.AMST_RegistrationNo + "-" + (a.AMST_FirstName == null ? "" : a.AMST_FirstName),
                                                       AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                                       AMST_LastName = a.AMST_LastName == null ? "" : a.AMST_LastName,
                                                   }).Distinct().ToArray();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeOpeningBalanceDTO getrefund(FeeOpeningBalanceDTO data)
        {
            try
            {
                var arefund=false;
                if(data.filterrefund== "Refunable")
                { arefund = true; }
                else
                { arefund = false; }
                data.fillmasterhead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       from b in _FeeGroupContext.feeGroup
                                       from c in _FeeGroupContext.feehead
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && c.FMH_RefundFlag==arefund )
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeOpeningBalanceDTO DeleteEntry(FeeOpeningBalanceDTO data)
        {
            FeeOpeningBalanceDTO fee_opening_balance = new FeeOpeningBalanceDTO();
            try
            {
                if (data.fillbusroutestudents == "fees")
                {

                    var result1 = _FeeGroupContext.feeOpeningBalance.Single(s => s.FMOB_Id == data.FMOB_Id);

                    var updateStudentStatus = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.AMST_Id == result1.AMST_Id && t.ASMAY_Id == result1.ASMAY_Id && t.MI_Id == result1.MI_Id && t.FMG_Id == result1.FMG_Id && t.FMH_Id == result1.FMH_Id && t.FTI_Id == result1.FTI_Id && t.FSS_PaidAmount == 0);

                    var updateStudentStatus1 = _FeeGroupContext.FeeStudentTransactionDMO.Where(t => t.AMST_Id == result1.AMST_Id && t.ASMAY_Id == result1.ASMAY_Id && t.MI_Id == result1.MI_Id && t.FMG_Id == result1.FMG_Id && t.FMH_Id == result1.FMH_Id && t.FTI_Id == result1.FTI_Id && t.FSS_PaidAmount == 0).ToList();

                    if (updateStudentStatus1.Count() > 0)
                    {
                        if (result1.FMOB_Student_Due > 0 || result1.FMOB_Institution_Due > 0)
                        {
                            if (updateStudentStatus.FSS_OBArrearAmount >= Convert.ToInt64(result1.FMOB_Student_Due) && updateStudentStatus.FSS_OBExcessAmount >= Convert.ToInt64(result1.FMOB_Institution_Due) && (updateStudentStatus.FSS_RunningExcessAmount >= Convert.ToInt64(result1.FMOB_Institution_Due) || updateStudentStatus.FSS_RefundableAmount >= Convert.ToInt64(result1.FMOB_Institution_Due)))
                            {
                                var gethead = _FeeGroupContext.feehead.Single(t => t.FMH_Id == result1.FMH_Id);
                              
                                if (updateStudentStatus.FSS_ToBePaid >= Convert.ToInt64(result1.FMOB_Student_Due))
                                {
                                    updateStudentStatus.FSS_ToBePaid = updateStudentStatus.FSS_ToBePaid - Convert.ToInt64(result1.FMOB_Student_Due);

                                    if (updateStudentStatus.FSS_TotalToBePaid - Convert.ToInt64(result1.FMOB_Student_Due) >= 0 && updateStudentStatus.FSS_TotalToBePaid - Convert.ToInt64(result1.FMOB_Institution_Due) >= 0)
                                    {
                                        updateStudentStatus.FSS_TotalToBePaid = updateStudentStatus.FSS_TotalToBePaid - Convert.ToInt64(result1.FMOB_Student_Due);
                                    }
                                }

                                updateStudentStatus.FSS_OBArrearAmount = updateStudentStatus.FSS_OBArrearAmount - Convert.ToInt64(result1.FMOB_Student_Due);
                                updateStudentStatus.FSS_OBExcessAmount = updateStudentStatus.FSS_OBExcessAmount - Convert.ToInt64(result1.FMOB_Institution_Due);
                                if (gethead.FMH_RefundFlag == true)
                                {
                                    if (updateStudentStatus.FSS_RefundableAmount >= Convert.ToInt64(result1.FMOB_Institution_Due))
                                    {
                                        updateStudentStatus.FSS_RefundableAmount = updateStudentStatus.FSS_RefundableAmount - Convert.ToInt64(result1.FMOB_Institution_Due);
                                    }
                                }
                                else
                                {
                                    if (updateStudentStatus.FSS_RunningExcessAmount >= Convert.ToInt64(result1.FMOB_Institution_Due))
                                    {
                                        updateStudentStatus.FSS_RunningExcessAmount = updateStudentStatus.FSS_RunningExcessAmount - Convert.ToInt64(result1.FMOB_Institution_Due);
                                    }
                                }

                                _FeeGroupContext.feeOpeningBalance.Remove(result1);
                                _FeeGroupContext.Update(updateStudentStatus);
                                var contactExists = _FeeGroupContext.SaveChanges();
                                if (contactExists == 2)
                                {
                                    fee_opening_balance.returnval = true;
                                }
                                else
                                {
                                    fee_opening_balance.returnval = false;
                                }
                            }
                            else
                            {
                                fee_opening_balance.returnval = false;
                                fee_opening_balance.returntxt = "a";
                            }

                        }
                        else
                        {
                            fee_opening_balance.returnval = false;
                        }
                    }
                }
                else
                {
                    var result1 = _FeeGroupContext.PDA_StatusDMO.Single(s => s.PDAS_Id == data.FMOB_Id);


                    _FeeGroupContext.PDA_StatusDMO.Remove(result1);

                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists>1)
                    {
                        fee_opening_balance.returnval = true;
                    }
                    else
                    {
                        fee_opening_balance.returnval = false;
                    }
                }
            }
            
            catch (Exception ee)
            {
                fee_opening_balance.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return fee_opening_balance;
        }
        public FeeOpeningBalanceDTO searching(FeeOpeningBalanceDTO stu)
        {

            try
            {

                switch (stu.searchType)
                {
                    case "0":
                        string str = "";
                        stu.searchtext = stu.searchtext.ToUpper();



                        List<FeeOpeningBalanceDTO> result = new List<FeeOpeningBalanceDTO>();
                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "FEE_OPENINGBALANCE_NAME_SEARCH";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                              SqlDbType.BigInt)
                            {
                                Value = stu.MI_ID
                            });

                            cmd.Parameters.Add(new SqlParameter("@searchtext",
                                         SqlDbType.VarChar)
                            {
                                Value = stu.searchtext
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.BigInt)
                            {
                                Value = stu.asmay_id
                            });
                            cmd.Parameters.Add(new SqlParameter("@USERID",
                           SqlDbType.BigInt)
                            {
                                Value = stu.userid
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
                                        result.Add(new FeeOpeningBalanceDTO
                                        {

                                            Amst_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                            AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                            AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                                            AMST_LastName = dataReader["AMST_LastName"].ToString(),
                                            FMOB_Id = Convert.ToInt64(dataReader["FMOB_Id"].ToString()),
                                            asmay_id = Convert.ToInt64(dataReader["asmay_id"].ToString()),
                                            FMOB_EntryDate = Convert.ToDateTime(dataReader["FMOB_EntryDate"].ToString()),
                                            FMOB_Student_Due = Convert.ToDecimal(dataReader["FMOB_Student_Due"].ToString()),
                                            FMOB_Institution_Due = Convert.ToDecimal(dataReader["FMOB_Institution_Due"].ToString()),
                                            ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                            ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),

                                        });
                                    }
                                }
                                stu.TempararyArrayhEADListnew = result.ToArray();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }





                        //stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                        //                                 from b in _FeeGroupContext.feeOpeningBalance
                        //                                 from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                        //                                 from d in _FeeGroupContext.admissioncls
                        //                                 from e in _FeeGroupContext.school_M_Section
                        //                                 where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_ID && b.ASMAY_Id == stu.asmay_id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && (((a.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(a.AMST_MiddleName.ToUpper().Trim()) == true ? str : a.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(a.AMST_LastName.ToUpper().Trim()) == true ? str : a.AMST_LastName.ToUpper().Trim())).Trim().Contains(stu.searchtext) || a.AMST_FirstName.ToUpper().Trim().StartsWith(stu.searchtext) || a.AMST_MiddleName.ToUpper().Trim().StartsWith(stu.searchtext) || a.AMST_LastName.ToUpper().Trim().StartsWith(stu.searchtext)) && b.User_Id == stu.userid)
                        //                                 select new FeeOpeningBalanceDTO
                        //                                 {
                        //                                     Amst_Id = a.AMST_Id,
                        //                                     AMST_FirstName = a.AMST_FirstName,
                        //                                     AMST_MiddleName = a.AMST_MiddleName,
                        //                                     AMST_LastName = a.AMST_LastName,
                        //                                     FMOB_Id = b.FMOB_Id,
                        //                                     asmay_id = b.ASMAY_Id,
                        //                                     FMOB_EntryDate = b.FMOB_EntryDate,
                        //                                     FMOB_Student_Due = b.FMOB_Student_Due,
                        //                                     FMOB_Institution_Due = b.FMOB_Institution_Due,
                        //                                     ASMCL_ClassName = d.ASMCL_ClassName,
                        //                                     ASMC_SectionName = e.ASMC_SectionName,
                        //                                     //Amst_Id = a.AMST_Id,
                        //                                     //AMST_FirstName = a.AMST_FirstName,
                        //                                     //AMST_MiddleName = a.AMST_MiddleName,
                        //                                     //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                        //                                 }
                        //   ).Distinct().OrderByDescending(t => t.AMST_FirstName).ToList().ToArray();

                        break;
                    case "1":
                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_ID && b.ASMAY_Id == stu.asmay_id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && d.ASMCL_ClassName.ToLower().Contains(stu.searchtext.ToLower()) )
                                                         select new FeeOpeningBalanceDTO
                                                         {
                                                             Amst_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             asmay_id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                           ).Distinct().OrderByDescending(t => t.ASMCL_ClassName).ToList().ToArray();
                        break;
                    case "2":
                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_ID && b.ASMAY_Id == stu.asmay_id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && e.ASMC_SectionName.ToLower().Contains(stu.searchtext.ToLower()) )
                                                         select new FeeOpeningBalanceDTO
                                                         {
                                                             Amst_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             asmay_id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                           ).Distinct().OrderByDescending(t => t.ASMC_SectionName).ToList().ToArray();
                        break;
                    case "3":
                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_ID && b.ASMAY_Id == stu.asmay_id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && b.FMOB_Student_Due.ToString().Contains(stu.searchnumber) )
                                                         select new FeeOpeningBalanceDTO
                                                         {
                                                             Amst_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             asmay_id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                          ).Distinct().OrderByDescending(t => t.FMOB_Student_Due).ToList().ToArray();
                        break;
                    case "4":

                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_ID && b.ASMAY_Id == stu.asmay_id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && b.FMOB_Institution_Due.ToString().Contains(stu.searchnumber) )
                                                         select new FeeOpeningBalanceDTO
                                                         {
                                                             Amst_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             asmay_id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                          ).Distinct().OrderByDescending(t => t.FMOB_Institution_Due).ToList().ToArray();
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }

        public FeeOpeningBalanceDTO filterstudents(FeeOpeningBalanceDTO data)
        {
            throw new NotImplementedException();
        }

        public FeeOpeningBalanceDTO onselectacademicyear(FeeOpeningBalanceDTO data)
        {
            try
            {
                data.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                  from b in _FeeGroupContext.feeOpeningBalance
                                                  from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                  from d in _FeeGroupContext.admissioncls
                                                  from e in _FeeGroupContext.school_M_Section
                                                  where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id )
                                                  select new FeeOpeningBalanceDTO
                                                  {
                                                      Amst_Id = a.AMST_Id,
                                                      AMST_FirstName = a.AMST_FirstName,
                                                      AMST_MiddleName = a.AMST_MiddleName,
                                                      AMST_LastName = a.AMST_LastName,
                                                      FMOB_Id = b.FMOB_Id,
                                                      asmay_id = b.ASMAY_Id,
                                                      FMOB_EntryDate = b.FMOB_EntryDate,
                                                      FMOB_Student_Due = b.FMOB_Student_Due,
                                                      FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                      ASMCL_ClassName = d.ASMCL_ClassName,
                                                      ASMC_SectionName = e.ASMC_SectionName,
                                                  }
                          ).Distinct().OrderByDescending(t => t.FMOB_Id).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}







