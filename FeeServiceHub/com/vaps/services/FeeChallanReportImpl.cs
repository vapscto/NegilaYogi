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
using CommonLibrary;
using System.Globalization;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeChallanReportImpl : interfaces.FeeChallanReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _db;
        public FeeChallanReportImpl(FeeGroupContext frgContext, DomainModelMsSqlServerContext db)
        {
            _FeeGroupContext = frgContext;
            _db = db;
        }
        public FeeChallanReportDTO getdata123(FeeChallanReportDTO data)
        {
            try
            {
                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _FeeGroupContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.grouplist = group.GroupBy(g => g.FMG_GroupName).Select(g => g.First()).ToArray();

                data.headlist = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                 from b in _FeeGroupContext.feeGroup
                                 from c in _FeeGroupContext.feehead
                                 where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id)
                                 select new FeeChallanReportDTO
                                 {
                                     FMG_Id = b.FMG_Id,
                                     FMH_Id = c.FMH_Id,
                                     FMH_FeeName = c.FMH_FeeName
                                 }
      ).ToArray();
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.acayear = year.ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_ID && t.ASMCL_ActiveFlag == true).ToList();
                data.classlist = allclas.ToArray();

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_ID).ToList();
                data.sectionlist = allsetion.ToArray();

                data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                       from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "S")
                                       select new FeeChallanReportDTO
                                       {
                                           Amst_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                       }
                             ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();







            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public async Task<FeeChallanReportDTO> getreport(FeeChallanReportDTO data)
        {
            try
            {
                if (data.typeofrpt == null)
                {
                    data.typeofrpt = "0";
                }

                string confromdate1 = "";
                string confromdate2 = "";

                DateTime date1 = DateTime.Now;
                date1 = Convert.ToDateTime(data.fromdate.Value.Date.ToString("yyyy-MM-dd"));
                // confromdate = fromdatecon.ToString();
                confromdate1 = date1.ToString("dd/MM/yyyy");

                DateTime date2 = DateTime.Now;

                date2 = Convert.ToDateTime(data.todate.Value.Date.ToString("yyyy-MM-dd"));
                // confromdate = fromdatecon.ToString();
                confromdate2 = date2.ToString("dd/MM/yyyy");

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "fee_challan_report_1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Asmay_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.asmayidpss
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = confromdate1
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
            SqlDbType.VarChar)
                    {
                        Value = confromdate2
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
              SqlDbType.BigInt)
                    {
                        Value = data.typeofrpt
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                        data.reportdatelist = retObject.ToArray();

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
        public FeeChallanReportDTO getinstallment(FeeChallanReportDTO obj)
        {

            try
            {
                obj.feeConfiguration = _FeeGroupContext.feemastersettings.Where(d => d.MI_Id == obj.MI_ID).ToArray();
                obj.grouplist = _FeeGroupContext.FeeGroupDMO.Where(d => d.MI_Id == obj.MI_ID && d.FMG_ActiceFlag == true).ToArray();
                obj.termList = _FeeGroupContext.feeTr.Where(d => d.MI_Id == obj.MI_ID && d.FMT_ActiveFlag == true).ToArray();
                var rolelist = _FeeGroupContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == obj.roleId).ToList();
                obj.roleName = rolelist[0].IVRMRT_Role;

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == obj.MI_ID && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                obj.acayear = year.ToArray();

                if (obj.roleName.ToLower() != "student")
                {
                    obj.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                          from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                          from c in _FeeGroupContext.FeeStudentTransactionDMO
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == obj.MI_ID && b.ASMAY_Id == obj.asmay_id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.AMST_Id == c.AMST_Id)
                                          // group a by a.AMST_Id into g && c.FSS_ToBePaid > 0
                                          select new FeeChallanReportDTO
                                          {
                                              Amst_Id = a.AMST_Id,
                                              admNo = a.AMST_AdmNo,
                                              //studentName = a.AMST_FirstName + ' ' + a.AMST_MiddleName ?? "" + ' ' + a.AMST_LastName ?? "",
                                              studentName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : a.AMST_LastName)).Trim(),
                                              //studentName = g.AMST_FirstName + ' ' + g.AMST_MiddleName ?? "" + ' ' + g.AMST_LastName ?? ""

                                          }
                            ).Distinct().ToArray();
                }


                var fetchmaxfypid = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                     from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                     where (a.FYP_Id == b.FYP_Id && a.MI_Id == obj.MI_ID && a.ASMAY_ID == obj.asmay_id &&
                                     (((a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && a.FYP_ChallanNo != "" && a.FYP_ChallanNo != null)) /*|| a.FYP_OnlineChallanStatusFlag == "Sucessfull"*/
                                     select new FeeStudentTransactionDTO
                                     {
                                         FYP_Id = a.FYP_Id,
                                     }
            ).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();



                if (obj.Amst_Id == 0)
                {
                    obj.receiparraydelete = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                             from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                             from c in _FeeGroupContext.AdmissionStudentDMO
                                             from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                             from e in _FeeGroupContext.admissioncls
                                             from f in _FeeGroupContext.school_M_Section
                                             where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == obj.MI_ID && a.ASMAY_ID == obj.asmay_id && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 Amst_Id = c.AMST_Id,
                                                 AMST_FirstName = c.AMST_FirstName,
                                                 AMST_MiddleName = c.AMST_MiddleName,
                                                 AMST_LastName = c.AMST_LastName,
                                                 FYP_Receipt_No = a.FYP_ChallanNo,
                                                 FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                 FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                 classname = e.ASMCL_ClassName,
                                                 sectionname = f.ASMC_SectionName,
                                                 FYP_Id = a.FYP_Id,
                                                 AMST_AdmNo = c.AMST_AdmNo,
                                                 FYP_Date = a.FYP_Date
                                             }
                                       ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }

                else
                {

                    fetchmaxfypid = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                     from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                     where (a.FYP_Id == b.FYP_Id && a.MI_Id == obj.MI_ID && a.ASMAY_ID == obj.asmay_id && b.AMST_Id == obj.Amst_Id &&
                                     ((a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && a.FYP_ChallanNo != "" && a.FYP_ChallanNo != null)) /*|| a.FYP_OnlineChallanStatusFlag == "Sucessfull"*/
                                     select new FeeStudentTransactionDTO
                                     {
                                         FYP_Id = a.FYP_Id,
                                     }
       ).OrderByDescending(t => t.FYP_Id).Select(t => t.FYP_Id).ToList();


                    obj.receiparraydelete = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                             from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                             from c in _FeeGroupContext.AdmissionStudentDMO
                                             from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                             from e in _FeeGroupContext.admissioncls
                                             from f in _FeeGroupContext.school_M_Section
                                             where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == obj.MI_ID && a.ASMAY_ID == obj.asmay_id && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && b.AMST_Id == obj.Amst_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 Amst_Id = c.AMST_Id,
                                                 AMST_FirstName = c.AMST_FirstName,
                                                 AMST_MiddleName = c.AMST_MiddleName,
                                                 AMST_LastName = c.AMST_LastName,
                                                 FYP_Receipt_No = a.FYP_ChallanNo,
                                                 FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                 FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                 classname = e.ASMCL_ClassName,
                                                 sectionname = f.ASMC_SectionName,
                                                 FYP_Id = a.FYP_Id,
                                                 AMST_AdmNo = c.AMST_AdmNo,
                                                 FYP_Date = a.FYP_Date
                                             }
                                      ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public FeeChallanReportDTO generateHutchingChallan(FeeChallanReportDTO data)
        {
            try
            {
                long netamt = 0;
                data.institution_det = _FeeGroupContext.master_institution.Where(d => d.MI_Id == data.MI_ID).ToArray();
                var studs = (from m in _FeeGroupContext.AdmissionStudentDMO
                             from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                             where (m.AMST_Id == b.AMST_Id && m.MI_Id == data.MI_ID && b.AMST_Id == data.Amst_Id && b.ASMAY_Id == data.asmay_id)
                             select new FeeChallanReportDTO
                             {
                                 Amst_Id = b.AMST_Id,
                                 ASMCL_Id = b.ASMCL_Id,
                                 studentName = ((m.AMST_FirstName == null || m.AMST_FirstName == "0" ? "" : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null || m.AMST_MiddleName == "0" ? "" : m.AMST_MiddleName) + " " + (m.AMST_LastName == null || m.AMST_LastName == "0" ? "" : m.AMST_LastName)).Trim(),
                                 FatherName = m.AMST_FatherName,
                                 mobileNo = m.AMST_MobileNo.ToString(),
                                 admNo = m.AMST_AdmNo,
                                 ASMS_Id = b.ASMS_Id,
                                 AMST_Sex = m.AMST_Sex
                             }
                           ).ToList();

                data.student_det = studs.ToArray();

                var query = _FeeGroupContext.School_M_Class.Where(d => d.ASMCL_Id == studs.FirstOrDefault().ASMCL_Id).ToList();
                data.className = query.FirstOrDefault().ASMCL_ClassName;
                var query1 = _FeeGroupContext.school_M_Section.Where(d => d.ASMS_Id == studs.FirstOrDefault().ASMS_Id).ToList();
                data.sectionName = query1.FirstOrDefault().ASMC_SectionName;
                //Getting Bank Details.
                data.bankDetails = _FeeGroupContext.FeeBankDetailsDMO.Where(d => d.MI_Id == data.MI_ID && d.Class.Equals(query.FirstOrDefault().ASMCL_ClassName)).ToArray();


                var logo = _FeeGroupContext.AdmissionStandardDMO.Where(d => d.MI_Id == data.MI_ID).ToList();
                data.logo = logo.FirstOrDefault().ASC_Logo_Path;

                var feeconfig = _FeeGroupContext.feemastersettings.Where(d => d.MI_Id == data.MI_ID).ToList();



                data.fillclasflg = _FeeGroupContext.FeeStudentTransactionDMO.Where(d => d.MI_Id == data.MI_ID && d.ASMAY_Id == data.asmay_id && d.AMST_Id == data.Amst_Id).Sum(d => d.FSS_OBArrearAmount);








                /*------------------------------------------------------------------------------------------*/
                //Stored Procedure Execution.
                List<FeeChallanReportDTO> result = new List<FeeChallanReportDTO>();
                if (feeconfig.FirstOrDefault().FMC_GroupOrTermFlg.Equals("G"))
                {
                    //for(int i=0;i<data.selectedGroup.Length;i++)
                    //{
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Student_Fee_Det";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@amst_id",
                            SqlDbType.BigInt)
                        {
                            Value = data.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.BigInt)
                        {
                            Value = data.asmay_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@date",
                           SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                           SqlDbType.BigInt)
                        {
                            Value = data.MI_ID
                        });
                        cmd.Parameters.Add(new SqlParameter("@configsettings",
                           SqlDbType.NVarChar)
                        {
                            Value = feeconfig.FirstOrDefault().FMC_GroupOrTermFlg
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmg_id",
                           SqlDbType.BigInt)
                        {
                            Value = data.multiplegrpids
                        });
                        cmd.Parameters.Add(new SqlParameter("@userid",
                          SqlDbType.BigInt)
                        {
                            Value = data.userId
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();
                    }
                    // }

                }
                if (feeconfig.FirstOrDefault().FMC_GroupOrTermFlg.Equals("T"))
                {
                    //for (int i = 0; i < data.selectedTerm.Length; i++)
                    //{
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Student_Fee_Det_term_all";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@amst_id",
                            SqlDbType.VarChar)
                        {
                            Value = data.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.VarChar)
                        {
                            Value = data.asmay_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@date",
                           SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                           SqlDbType.VarChar)
                        {
                            Value = data.MI_ID
                        });
                        cmd.Parameters.Add(new SqlParameter("@configsettings",
                           SqlDbType.NVarChar)
                        {
                            Value = feeconfig.FirstOrDefault().FMC_GroupOrTermFlg
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmt_id",
                           SqlDbType.NVarChar)
                        {
                            Value = data.multipletrmids
                        });
                        cmd.Parameters.Add(new SqlParameter("@userid",
                          SqlDbType.VarChar)
                        {
                            Value = data.userId
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();


                        var data1 = cmd.ExecuteNonQuery();
                    }

                    // }
                }





                /*------------------------------------------------------------------------------------------*/
                //Getting Particulars data for selected student.
                var particulars = (from aa in _FeeGroupContext.V_StudentPendingDMO
                                   from b in _FeeGroupContext.FeeGroupDMO
                                       //from b in _FeeGroupContext.FeeHeadDMO
                                       //from c in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                       //from d in _FeeGroupContext.FeeHeadDMO
                                       //from e in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                   where (aa.fmg_id == b.FMG_Id && aa.mi_id == data.MI_ID) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                   select new FeeStudentTransactionDTO
                                   {
                                       FMA_Id = aa.fma_id,
                                       FMH_FeeName = aa.FMH_FeeName,
                                       FTI_Name = aa.FTI_Name,
                                       FMH_Id = aa.fmh_id,
                                       FTI_Id = aa.fti_id,
                                       totalamount = aa.FSS_NetAmount,
                                       FSS_ToBePaid = aa.FSS_ToBePaid,
                                       FSS_ConcessionAmount = aa.FSS_ConcessionAmount,
                                       FSS_FineAmount = aa.FSS_FineAmount,
                                       FSS_CurrentYrCharges = aa.FSS_CurrentYrCharges,
                                       FSS_TotalToBePaid = aa.FSS_TotalToBePaid,
                                       FMG_Id = aa.fmg_id,
                                       FMG_GroupName = b.FMG_GroupName,
                                   }

                          ).OrderBy(t => t.FTI_Id).ToList();

                data.particularsList = particulars.ToArray();
                if (data.particularsList.Length > 0)
                {



                    List<FeeStudentTransactionDTO> fines_fma_ids = new List<FeeStudentTransactionDTO>();
                    data.fillseccls = 0;
                    FeeStudentTransactionDTO sew = new FeeStudentTransactionDTO();
                    foreach (var x in particulars)
                    {

                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Sp_Calculate_Fine";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime)
                            {
                                Value = DateTime.Now
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.BigInt)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.BigInt)
                            {
                                Value = data.asmay_id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Float)
                            {
                                Direction = ParameterDirection.Output
                            });
                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();
                            //  x.FSS_FineAmount += Convert.ToDecimal(cmd.Parameters["@amt"].Value);
                            data.fillseccls += Convert.ToInt32(cmd.Parameters["@amt"].Value);

                            sew.FMA_Id = x.FMA_Id;
                            sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            fines_fma_ids.Add(sew);
                        }

                    }
                    if (data.fillseccls > 0)
                    {
                        data.FMH_Id = (from c in _FeeGroupContext.FeeStudentTransactionDMO
                                       from b in _FeeGroupContext.FeeHeadDMO
                                       where (c.FMH_Id == b.FMH_Id && c.AMST_Id == data.Amst_Id && c.ASMAY_Id == data.asmay_id && c.MI_Id == data.MI_ID && b.FMH_Flag == "F")
                                       select c.FMA_Id).FirstOrDefault();

                    }



                    // NetAmount Calculation.
                    foreach (FeeStudentTransactionDTO item in data.particularsList)
                    {
                        netamt = netamt + item.FSS_ToBePaid;
                    }

                    netamt = data.fillseccls + netamt;

                    /*------------------------------------------------------------------------------------------*/
                    //Receipt No. generation code starts here.
                    var transnumconfigsettings = _db.Receipt_Numbering.Where(d => d.MI_Id == data.MI_ID).ToList();
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    Master_NumberingDTO num = new Master_NumberingDTO();
                    num.MI_Id = data.MI_ID;
                    num.ASMAY_Id = data.asmay_id;
                    num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IRN_AutoManualFlag;
                    num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IRN_DuplicatesFlag;
                    num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IRN_StartingNo;
                    num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IRN_WidthNumeric;
                    num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IRN_ZeroPrefixFlag;
                    num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixAcadYearCode;
                    num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixFinYearCode;
                    num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixCalYearCode;
                    num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IRN_PrefixParticular;
                    num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixAcadYearCode;
                    num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixFinYearCode;
                    num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixCalYearCode;
                    num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IRN_SuffixParticular;
                    num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IRN_RestartNumFlag;
                    num.IMN_Flag = "Receipt";
                    //  data.receiptNo = a.GenerateNumber(num);

                    try
                    {
                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "receiptnogeneration_new";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mi_id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.MI_ID
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = data.asmay_id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmgid",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = data.config
                            });

                            cmd.Parameters.Add(new SqlParameter("@receiptno",
                SqlDbType.NVarChar, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            data.receiptNo = cmd.Parameters["@receiptno"].Value.ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //Receipt No. generation code ends  here.
                    /*------------------------------------------------------------------------------------------*/
                    //Saving data in Fee_Y_Payment table.
                    try
                    {
                        FeePaymentDetailsDMO dmo = new FeePaymentDetailsDMO();
                        dmo.ASMAY_ID = data.asmay_id;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.DOE = DateTime.Now;
                        dmo.FTCU_Id = 1;
                        dmo.FYP_Date = DateTime.Now;
                        dmo.FYP_DD_Cheque_Date = DateTime.Now;
                        dmo.FYP_Receipt_No = data.receiptNo;
                        dmo.FYP_Tot_Amount = Convert.ToDecimal(netamt);
                        dmo.MI_Id = data.MI_ID;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.user_id = data.userId;
                        dmo.FYP_ChallanNo = data.receiptNo;
                        dmo.FYP_OnlineChallanStatusFlag = "Payment Initiated";
                        _FeeGroupContext.Add(dmo);

                        _FeeGroupContext.FeePaymentDetailsDMO.Add(dmo);
                        Fee_Y_Payment_School_StudentDMO temppayment = new Fee_Y_Payment_School_StudentDMO();

                        temppayment.AMST_Id = data.Amst_Id;
                        temppayment.ASMAY_Id = dmo.ASMAY_ID;
                        temppayment.FTP_TotalPaidAmount = Convert.ToDecimal(netamt);
                        temppayment.FTP_TotalWaivedAmount = dmo.FYP_Tot_Waived_Amt;
                        temppayment.FTP_TotalConcessionAmount = dmo.FYP_Tot_Concession_Amt;
                        temppayment.FTP_TotalFineAmount = dmo.FYP_Tot_Fine_Amt;
                        temppayment.FYP_Id = dmo.FYP_Id;
                        _FeeGroupContext.Fee_Y_Payment_School_StudentDMO.Add(temppayment);

                        /*------------------------------------------------------------------------------------------*/
                        //Saving data in Fee_T_Payment table.
                        for (int i = 0; i < particulars.Count; i++)
                        {
                            if (particulars[i].FSS_ToBePaid > 0)
                            {
                                FeeTransactionPaymentDMO obj = new FeeTransactionPaymentDMO();
                                obj.FMA_Id = particulars[i].FMA_Id;
                                obj.FYP_Id = dmo.FYP_Id;
                                obj.FTP_Paid_Amt = particulars[i].FSS_ToBePaid;
                                _FeeGroupContext.Add(obj);
                            }
                        }
                        _FeeGroupContext.SaveChanges();
                        if (data.fillseccls > 0)
                        {
                            FeeTransactionPaymentDMO obj1 = new FeeTransactionPaymentDMO();
                            obj1.FMA_Id = data.FMH_Id;
                            obj1.FYP_Id = dmo.FYP_Id;
                            obj1.FTP_Paid_Amt = data.fillseccls;
                            _FeeGroupContext.Add(obj1);
                            _FeeGroupContext.SaveChanges();
                        }


                        _FeeGroupContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    data.returnval = "nodata";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeChallanReportDTO checkforchallan(FeeChallanReportDTO data)
        {


            var fmg_ids = "";
            int selectedcount = data.FMGG_Ids.Length;

            if (data.FMGG_Ids.Length > 0)
            {
                foreach (var x in data.FMGG_Ids)
                {
                    fmg_ids += x + ",";
                }
                fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));
            }

            else
            {
                fmg_ids = "0";
            }




            try
            {
                List<FeeChallanReportDTO> result = new List<FeeChallanReportDTO>();
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Check_For_Duplicate_Challan";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 3000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.asmay_id
                    });
                    //cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                    //   SqlDbType.BigInt)
                    //{
                    //    Value = data.FMT_Id
                    //});
                    cmd.Parameters.Add(new SqlParameter("@FMT_Id", SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data1 = cmd.ExecuteNonQuery();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FeeChallanReportDTO
                                {
                                    FMT_Id = Convert.ToInt64(dataReader["FMT_Id"]),
                                    Amst_Id = Convert.ToInt64(dataReader["AMST_Id"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


                if (result.Count >= selectedcount)
                {
                    data.ischallangenerated = "yes";
                }
                else
                {
                    data.ischallangenerated = "no";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public FeeChallanReportDTO getChallandetails(FeeChallanReportDTO data)
        {
            string termids = "";

            var studs = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                         from b in _FeeGroupContext.FeeTransactionPaymentDMO
                         from c in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                         from d in _FeeGroupContext.FeeAmountEntryDMO
                         from e in _FeeGroupContext.feeMTH
                         where (a.FYP_Id == c.FYP_Id && b.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.FYP_Id == data.FYP_Id && c.AMST_Id == data.Amst_Id && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
                         select e.FMT_Id).Distinct().ToList();

            foreach (var w in studs)
            {
                termids += w + ",";
            }
            termids = termids.Substring(0, (termids.Length - 1));

            var query = _FeeGroupContext.FeePaymentDetailsDMO.Where(d => d.FYP_Id == data.FYP_Id).ToList();
            data.asmay_id = query.FirstOrDefault().ASMAY_ID;
            data.datepass = query.FirstOrDefault().FYP_Date;
            data.receiptNo = query.FirstOrDefault().FYP_ChallanNo;

            data.fillclasflg = _FeeGroupContext.FeeStudentTransactionDMO.Where(d => d.MI_Id == data.MI_ID && d.ASMAY_Id == data.asmay_id && d.AMST_Id == data.Amst_Id).Sum(d => d.FSS_OBArrearAmount);

            data.fee_opening_bal = _FeeGroupContext.FeeStudentTransactionDMO.Where(d => d.MI_Id == data.MI_ID && d.ASMAY_Id == data.asmay_id && d.AMST_Id == data.Amst_Id && d.FSS_OBArrearAmount>0).ToArray();

            var fineamt = (from a in _FeeGroupContext.FeeTransactionPaymentDMO
                           from b in _FeeGroupContext.FeeHeadDMO
                           from c in _FeeGroupContext.FeeAmountEntryDMO
                           where (a.FMA_Id == c.FMA_Id && b.FMH_Id == c.FMH_Id && b.FMH_Flag == "F" && a.FYP_Id == data.FYP_Id && a.FTP_Paid_Amt > 0)
                           select new FeeChallanReportDTO
                           {
                               asmayidpss = Convert.ToInt64(a.FTP_Paid_Amt)
                           }).Distinct().ToList();


            if (fineamt.Count > 0)
            {
                data.fillseccls = Convert.ToInt64(fineamt.FirstOrDefault().asmayidpss);
            }


            /*------------------------------------------------------------------------------------------*/
            //Stored Procedure Execution.
            List<FeeChallanReportDTO> result = new List<FeeChallanReportDTO>();
            if (data.config == "G")
            {
                //for(int i=0;i<data.selectedGroup.Length;i++)
                //{
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Student_Fee_Det";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                        SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                       SqlDbType.BigInt)
                    {
                        Value = data.asmay_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                       SqlDbType.DateTime)
                    {
                        Value = DateTime.Now
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                       SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@configsettings",
                       SqlDbType.NVarChar)
                    {
                        Value = data.config
                    });
                    cmd.Parameters.Add(new SqlParameter("@fmg_id",
                       SqlDbType.BigInt)
                    {
                        Value = data.multiplegrpids
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
                      SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data1 = cmd.ExecuteNonQuery();
                }
                // }

            }
            if (data.config == "T")
            {

                //using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Student_Fee_Det_term_all";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@amst_id",
                //        SqlDbType.VarChar)
                //    {
                //        Value = data.Amst_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                //       SqlDbType.VarChar)
                //    {
                //        Value = data.asmay_id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@date",
                //       SqlDbType.DateTime)
                //    {
                //        Value = DateTime.Now

                //    });
                //    cmd.Parameters.Add(new SqlParameter("@mi_id",
                //       SqlDbType.VarChar)
                //    {
                //        Value = data.MI_ID
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@configsettings",
                //       SqlDbType.NVarChar)
                //    {
                //        Value = data.config
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@fmt_id",
                //       SqlDbType.NVarChar)
                //    {
                //        Value = termids
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@userid",
                //      SqlDbType.VarChar)
                //    {
                //        Value = data.userId
                //    });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();


                //    var data1 = cmd.ExecuteNonQuery();
                //}


                data.particularsList = (from a in _FeeGroupContext.FeeTransactionPaymentDMO
                                        from b in _FeeGroupContext.FeePaymentDetailsDMO
                                        from c in _FeeGroupContext.FeeAmountEntryDMO
                                        from d in _FeeGroupContext.FeeHeadDMO
                                        from e in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                        from f in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        from g in _FeeGroupContext.admissioncls
                                        from h in _FeeGroupContext.school_M_Section
                                        from i in _FeeGroupContext.AdmissionStudentDMO
                                        from j in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                        from k in _FeeGroupContext.Fee_Master_ConcessionDMO
                                        where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.asmay_id && i.MI_Id == data.MI_ID && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id && d.FMH_Flag != "F")
                                        select new FeeStudentTransactionDTO
                                        {
                                            FMA_Id = c.FMA_Id,
                                            FMH_FeeName = d.FMH_FeeName,
                                            FTI_Name = e.FTI_Name,
                                            FMH_Id = d.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            totalamount = b.FYP_Tot_Amount,
                                            FSS_ToBePaid = Convert.ToInt64(a.FTP_Paid_Amt),
                                            FSS_ConcessionAmount = Convert.ToInt64(a.FTP_Concession_Amt),
                                            FSS_FineAmount = Convert.ToInt64(a.FTP_Fine_Amt),
                                            //FSS_CurrentYrCharges = aa.FSS_CurrentYrCharges,
                                            //FSS_TotalToBePaid = aa.FSS_TotalToBePaid,

                                        }
                  ).Distinct().ToArray();



            }
            ///*------------------------------------------------------------------------------------------*/
            ////Getting Particulars data for selected student.
            //var particulars = (from aa in _FeeGroupContext.V_StudentPendingDMO
            //                   from b in _FeeGroupContext.FeeGroupDMO
            //                       //from b in _FeeGroupContext.FeeHeadDMO
            //                       //from c in _FeeGroupContext.FeeInstallmentsyearlyDMO
            //                       //from d in _FeeGroupContext.FeeHeadDMO
            //                       //from e in _FeeGroupContext.FeeInstallmentsyearlyDMO
            //                   where (aa.fmg_id == b.FMG_Id && aa.mi_id == data.MI_ID) /*&& a.fmg_id.Contains(data.fmg_id)*/
            //                   select new FeeStudentTransactionDTO
            //                   {
            //                       FMA_Id = aa.fma_id,
            //                       FMH_FeeName = aa.FMH_FeeName,
            //                       FTI_Name = aa.FTI_Name,
            //                       FMH_Id = aa.fmh_id,
            //                       FTI_Id = aa.fti_id,
            //                       totalamount = aa.FSS_NetAmount,
            //                       FSS_ToBePaid = aa.FSS_ToBePaid,
            //                       FSS_ConcessionAmount = aa.FSS_ConcessionAmount,
            //                       FSS_FineAmount = aa.FSS_FineAmount,
            //                       FSS_CurrentYrCharges = aa.FSS_CurrentYrCharges,
            //                       FSS_TotalToBePaid = aa.FSS_TotalToBePaid,
            //                       FMG_Id = aa.fmg_id,
            //                       FMG_GroupName = b.FMG_GroupName,
            //                   }

            //          ).OrderBy(t => t.FTI_Id).ToList();

            //data.particularsList = particulars.ToArray();

            List<FeeStudentTransactionDTO> fines_fma_ids = new List<FeeStudentTransactionDTO>();




            long netamt = 0;
            data.institution_det = _FeeGroupContext.master_institution.Where(d => d.MI_Id == data.MI_ID).ToArray();
            var studs1 = (from m in _FeeGroupContext.AdmissionStudentDMO
                          from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                          where (m.AMST_Id == b.AMST_Id && m.MI_Id == data.MI_ID && b.AMST_Id == data.Amst_Id && b.ASMAY_Id == data.asmay_id)
                          select new FeeChallanReportDTO
                          {
                              Amst_Id = b.AMST_Id,
                              ASMCL_Id = b.ASMCL_Id,
                              studentName = ((m.AMST_FirstName == null || m.AMST_FirstName == "0" ? "" : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null || m.AMST_MiddleName == "0" ? "" : m.AMST_MiddleName) + " " + (m.AMST_LastName == null || m.AMST_LastName == "0" ? "" : m.AMST_LastName)).Trim(),
                              FatherName = m.AMST_FatherName,
                              mobileNo = m.AMST_MobileNo.ToString(),
                              admNo = m.AMST_AdmNo,
                              ASMS_Id = b.ASMS_Id,
                              AMST_Sex = m.AMST_Sex
                          }
                       ).ToList();

            data.student_det = studs1.ToArray();

            var query2 = _FeeGroupContext.School_M_Class.Where(d => d.ASMCL_Id == studs1.FirstOrDefault().ASMCL_Id).ToList();
            data.className = query2.FirstOrDefault().ASMCL_ClassName;
            var query3 = _FeeGroupContext.school_M_Section.Where(d => d.ASMS_Id == studs1.FirstOrDefault().ASMS_Id).ToList();
            data.sectionName = query3.FirstOrDefault().ASMC_SectionName;
            //Getting Bank Details.
            data.bankDetails = _FeeGroupContext.FeeBankDetailsDMO.Where(d => d.MI_Id == data.MI_ID && d.Class.Equals(query2.FirstOrDefault().ASMCL_ClassName)).ToArray();


            var logo = _FeeGroupContext.AdmissionStandardDMO.Where(d => d.MI_Id == data.MI_ID).ToList();
            data.logo = logo.FirstOrDefault().ASC_Logo_Path;

            var feeconfig = _FeeGroupContext.feemastersettings.Where(d => d.MI_Id == data.MI_ID).ToList();
            var feeterm = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                           from b in _FeeGroupContext.FeeTransactionPaymentDMO
                           from c in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                           from d in _FeeGroupContext.FeeStudentTransactionDMO
                           from e in _FeeGroupContext.feeMTH
                           from f in _FeeGroupContext.FeeHeadDMO
                           from g in _FeeGroupContext.feeTr
                           where (e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_ID && a.ASMAY_ID == data.asmay_id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id
                           && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
                           select new FeeStudentTransactionDTO
                           {
                               fmt_id = e.FMT_Id,
                               fmt_order = g.FMT_Order,
                               FMT_Year = g.FMT_Year
                           }

            ).Distinct().OrderBy(t => t.fmt_order).ToArray();


            int fmtorder_end = 0;
            string fmt_id_int = "0", fmt_id_end = "0", fmt_id_end_year = "0"; ;
            fmt_id_int = feeterm[0].fmt_id.ToString();
            fmt_id_end = feeterm[feeterm.Length - 1].fmt_id.ToString();

            fmtorder_end = Convert.ToInt32(feeterm[feeterm.Length - 1].fmt_order);
            fmt_id_end_year = (feeterm[feeterm.Length - 1].FMT_Year).ToString();

            var term_ids = _FeeGroupContext.feeTr.Where(t => t.MI_Id == data.MI_ID && t.FMT_Order == fmtorder_end + 1).Select(t => t.FMT_Id);

            List<long> termmidsnew = new List<long>();
            foreach (var item in feeterm)
            {
                termmidsnew.Add(item.fmt_id);
            }

            List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
            temp_group_head = (from a in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                               from b in _FeeGroupContext.Fee_Payment
                               from c in _FeeGroupContext.FeeTransactionPaymentDMO
                               from d in _FeeGroupContext.FeeAmountEntryDMO

                               where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_ID && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.asmay_id
                               && (b.FYP_OnlineChallanStatusFlag == "Payment Initiated" || b.FYP_OnlineChallanStatusFlag == "Sucessfull"))
                               select new FeeStudentTransactionDTO
                               {

                                   FMG_Id = d.FMG_Id,
                                   FMH_Id = d.FMH_Id,
                                   FTI_Id = d.FTI_Id

                               }

                       ).Distinct().ToList();
            List<long> grp_ids = new List<long>();
            List<long> head_ids = new List<long>();
            List<long> inst_ids = new List<long>();
            foreach (var item in temp_group_head)
            {
                grp_ids.Add(item.FMG_Id);
                head_ids.Add(item.FMH_Id);
                inst_ids.Add(item.FTI_Id);
            }

            var fordateinfyp = (from d in _FeeGroupContext.FeeStudentTransactionDMO
                            from f in _FeeGroupContext.feeTDueDateRegularDMO
                            where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.asmay_id && d.MI_Id == data.MI_ID && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                            select new FeeStudentTransactionDTO
                            {
                                month = f.FTDD_Month,
                            }
                     ).Distinct().ToList();

            List<int> months = new List<int>();
            List<int> dates = new List<int>();
            List<int> months1 = new List<int>();
            List<int> months2 = new List<int>();

            List<int> startperiod = new List<int>();

            List<FeeStudentTransactionDTO> fordate = new List<FeeStudentTransactionDTO>();
          //  List<FeeStudentTransactionDTO> fordateinfyp = new List<FeeStudentTransactionDTO>();
            var durationyear = "";
            foreach (FeeStudentTransactionDTO item in fordate)
            {
                dates.Add(Convert.ToInt32(item.date));
                months.Add(Convert.ToInt32(item.month));
            }

            foreach (FeeStudentTransactionDTO itemperiod in fordateinfyp)
            {
                startperiod.Add(Convert.ToInt32(itemperiod.month));
            }

            foreach (var item in months)
            {
                if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                {
                    months1.Add(Convert.ToInt32(item));
                    var curyear = DateTime.Now;
                    var nextyr = curyear.Year;
                    durationyear = nextyr.ToString();
                }
                else
                {
                    months2.Add(Convert.ToInt32(item));
                    var curyear = DateTime.Now;
                    durationyear = curyear.Year.ToString();
                }
            }

            string maxmonth = "", monthnameinitial = "", monthnameend = "" ,month="";
            if (months1.Count() > 0)
            {
                month = months1.Min().ToString();
                maxmonth = months1.Max().ToString();
                if (startperiod.Count >= 4)
                {
                    monthnameinitial = startperiod.Min().ToString();
                    maxmonth = startperiod.Max().ToString();
                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                }
                else
                {
                    monthnameinitial = startperiod.Max().ToString();
                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                }

                data.duration = monthnameinitial + '-' + monthnameend + '-' + durationyear;
            }
            else if (months2.Count() > 0)
            {
                month = months2.Min().ToString();
                maxmonth = months2.Max().ToString();

                monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(month));
                monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                data.duration = monthnameinitial + '-' + monthnameend + '-' + durationyear;

            }
            for (int i = 0; i < months.Count(); i++)
            {
                if (Convert.ToInt32(month) == months[i])
                {
                    data.date = dates[i].ToString();
                }
            }

            if (months.Count == 0)
            {
                foreach (var item in startperiod)
                {
                    if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                    {
                        monthnameinitial = startperiod.Min().ToString();
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year;
                       durationyear = curyear.Year.ToString();
                    }
                    else
                    {
                        maxmonth = startperiod.Max().ToString();
                        var curyear = DateTime.Now;
                        durationyear = curyear.Year.ToString();
                    }
                }
                if (monthnameinitial != "")
                {
                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                }

                if (monthnameend != "")
                {
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                }
                else
                {
                    maxmonth = startperiod.Max().ToString();
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                }
            }

            var compusoryflag = _FeeGroupContext.FeeGroupDMO.Where(d => d.MI_Id == data.MI_ID && grp_ids.Contains(d.FMG_Id)).Select(t => t.FMG_CompulsoryFlag).Distinct().ToArray();

            if (compusoryflag[0].ToString() == "T")
            {
                var termperiodlistint = _FeeGroupContext.feeTr.Where(d => d.MI_Id == data.MI_ID && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                var termperiodlistend = _FeeGroupContext.feeTr.Where(d => d.MI_Id == data.MI_ID && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                monthnameinitial = termperiodlistint[0].Transport_FromMonth.ToString();
                monthnameend = termperiodlistend[0].Transport_ToMonth.ToString();

                string yeardisplay = "0";
                yeardisplay = fmt_id_end_year;

                data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
            }
            else
            {
                var termperiodlistint = _FeeGroupContext.feeTr.Where(d => d.MI_Id == data.MI_ID && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                var termperiodlistend = _FeeGroupContext.feeTr.Where(d => d.MI_Id == data.MI_ID && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                monthnameinitial = termperiodlistint[0].FromMonth.ToString();
                monthnameend = termperiodlistend[0].ToMonth.ToString();

                string yeardisplay = "0";
                yeardisplay = fmt_id_end_year;

                data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
            }





            return data;
        }


        public FeeChallanReportDTO delrec(FeeChallanReportDTO data)
        {
            try
            {
                var lorg1 = _FeeGroupContext.FeePaymentDetailsDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg2 = _FeeGroupContext.Fee_Y_Payment_School_StudentDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg3 = _FeeGroupContext.FeeTransactionPaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();



                if (lorg3.Any())
                {
                    for (int i = 0; lorg3.Count > i; i++)
                    {
                        _FeeGroupContext.Remove(lorg3.ElementAt(i));
                    }
                }

                _FeeGroupContext.Remove(lorg2);
                _FeeGroupContext.Remove(lorg1);


                var contactexisttransaction = 0;
                using (var dbCtxTxn = _FeeGroupContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _FeeGroupContext.SaveChanges();
                        dbCtxTxn.Commit();
                        data.returnval = "true";
                    }
                    catch (Exception ex)
                    {
                        dbCtxTxn.Rollback();
                        data.returnval = "Transaction is not Processed Correctly.Kindly contact Administrator!!!!!";
                    }
                }

                //var contactExists = _FeeGroupContext.SaveChanges();
            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public FeeChallanReportDTO getstudlistgroup(FeeChallanReportDTO data)
        {

            if (data.config.Equals("T"))
            {
                var readterms = (from a in _FeeGroupContext.feeTr
                                 where (a.MI_Id == data.MI_ID)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMT_Name = a.FMT_Name,
                                     FMT_Id = a.FMT_Id,
                                 }
                                ).Distinct().ToArray();

                string alltrids = "0";
                for (int s = 0; s < readterms.Count(); s++)
                {
                    alltrids = alltrids + ',' + readterms[s].FMT_Id.ToString();
                }

                try
                {
                    using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "gettermsstatisticdetails";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.asmay_id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_ID
                        });

                        cmd1.Parameters.Add(new SqlParameter("@amst_id",
                        SqlDbType.VarChar)
                        {
                            Value = data.Amst_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@fmtids",
                         SqlDbType.VarChar)
                        {
                            Value = alltrids
                        });

                        cmd1.Parameters.Add(new SqlParameter("@userid",
                        SqlDbType.VarChar)
                        {
                            Value = data.userId
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
                            data.disableterms = retObject1.ToArray();
                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //for challan_checking
                var termList = _FeeGroupContext.feeTr.Where(d => d.MI_Id == data.MI_ID && d.FMT_ActiveFlag == true).Select(t => t.FMT_Id).ToList();
                List<FeeChallanReportDTO> terms_challan_flags = new List<FeeChallanReportDTO>();
                foreach (var id in termList)
                {
                    FeeChallanReportDTO obj_term = new FeeChallanReportDTO();
                    obj_term.FMT_Id = id;
                    try
                    {
                        List<FeeChallanReportDTO> result = new List<FeeChallanReportDTO>();
                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Check_For_Duplicate_Challan";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 3000;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.BigInt)
                            {
                                Value = data.MI_ID
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                               SqlDbType.BigInt)
                            {
                                Value = data.asmay_id
                            });
                            //cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                            //   SqlDbType.BigInt)
                            //{
                            //    Value = data.FMT_Id
                            //});
                            cmd.Parameters.Add(new SqlParameter("@FMT_Id", SqlDbType.VarChar)
                            {
                                Value = id
                            });

                            cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                               SqlDbType.BigInt)
                            {
                                Value = data.Amst_Id
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        result.Add(new FeeChallanReportDTO
                                        {
                                            FMT_Id = Convert.ToInt64(dataReader["FMT_Id"]),
                                            Amst_Id = Convert.ToInt64(dataReader["AMST_Id"])
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }

                        if (result.Count >= 1)
                        {
                            obj_term.ischallangenerated = "yes";
                        }
                        else
                        {
                            obj_term.ischallangenerated = "no";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    terms_challan_flags.Add(obj_term);
                }
                data.challanterms = terms_challan_flags.ToArray();
            }

            return data;


        }


        //public FeeStudentTransactionDTO searching(FeeStudentTransactionDTO data)
        //{


        //    try
        //    {

        //        if (data.Amst_Id == 0)
        //        {
        //            switch (data.searchType)
        //            {

        //                case "0":
        //                    string str = "";
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && d.ASMS_Id == f.ASMS_Id && a.ASMAY_ID == data.ASMAY_Id && (((c.AMST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.Trim()) == true ? str : c.AMST_MiddleName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.Trim()) == true ? str : c.AMST_LastName.Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.StartsWith(data.searchtext) || c.AMST_MiddleName.StartsWith(data.searchtext) || c.AMST_LastName.StartsWith(data.searchtext)) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
        //                    break;
        //                case "1":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && e.ASMCL_ClassName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.classname).ToArray();
        //                    break;
        //                case "2":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
        //                    break;
        //                case "3":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.FYP_ChallanNo).ToArray();
        //                    break;
        //                case "4":
        //                    var date_format = data.searchdate.ToString("dd/MM/yyyy");

        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                            // from g in list
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //       ).Distinct().ToArray();

        //                    break;
        //                case "5":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
        //                    break;

        //                case "6":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && f.ASMC_SectionName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull"))
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.sectionname).ToArray();
        //                    break;
        //            }
        //        }


        //        else
        //        {
        //            switch (data.searchType)
        //            {

        //                case "0":
        //                    string str = "";
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && d.ASMS_Id == f.ASMS_Id && a.ASMAY_ID == data.ASMAY_Id && (((c.AMST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.Trim()) == true ? str : c.AMST_MiddleName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.Trim()) == true ? str : c.AMST_LastName.Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.StartsWith(data.searchtext) || c.AMST_MiddleName.StartsWith(data.searchtext) || c.AMST_LastName.StartsWith(data.searchtext)) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && b.AMST_Id == data.Amst_Id)
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
        //                    break;
        //                case "1":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && e.ASMCL_ClassName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && b.AMST_Id == data.Amst_Id)
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.classname).ToArray();
        //                    break;
        //                case "2":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && b.AMST_Id == data.Amst_Id)
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
        //                    break;
        //                case "3":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && b.AMST_Id == data.Amst_Id)
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.FYP_ChallanNo).ToArray();
        //                    break;
        //                case "4":
        //                    var date_format = data.searchdate.ToString("dd/MM/yyyy");

        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                            // from g in list
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && b.AMST_Id == data.Amst_Id)
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //       ).Distinct().ToArray();

        //                    break;
        //                case "5":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && b.AMST_Id == data.Amst_Id)
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
        //                    break;

        //                case "6":
        //                    data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
        //                                        from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                                        from c in _FeeGroupContext.AdmissionStudentDMO
        //                                        from d in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                                        from e in _FeeGroupContext.admissioncls
        //                                        from f in _FeeGroupContext.school_M_Section
        //                                        where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && f.ASMC_SectionName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && (a.FYP_OnlineChallanStatusFlag == "Payment Initiated" || a.FYP_OnlineChallanStatusFlag == "Sucessfull") && b.AMST_Id == data.Amst_Id)
        //                                        select new FeeStudentTransactionDTO
        //                                        {
        //                                            Amst_Id = c.AMST_Id,
        //                                            AMST_FirstName = c.AMST_FirstName,
        //                                            AMST_MiddleName = c.AMST_MiddleName,
        //                                            AMST_LastName = c.AMST_LastName,
        //                                            FYP_Receipt_No = a.FYP_ChallanNo,
        //                                            FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                            FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                            classname = e.ASMCL_ClassName,
        //                                            sectionname = f.ASMC_SectionName,
        //                                            FYP_Id = a.FYP_Id,
        //                                            AMST_AdmNo = c.AMST_AdmNo,
        //                                            FYP_Date = a.FYP_Date
        //                                        }
        //          ).Distinct().OrderBy(t => t.sectionname).ToArray();
        //                    break;
        //            }


        //        }









        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return data;
        //}


        public FeeStudentTransactionDTO searching(FeeStudentTransactionDTO data)
        {


            try
            {

                if (data.Amst_Id == 0)
                {
                    switch (data.searchType)
                    {

                        case "0":
                            string str = "";
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && (((c.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.ToUpper().Trim()) == true ? str : c.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.ToUpper().Trim()) == true ? str : c.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_LastName.ToUpper().StartsWith(data.searchtext)))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                            break;
                        case "1":
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && e.ASMCL_ClassName.ToUpper().Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.classname).ToArray();
                            break;
                        case "2":
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && c.AMST_AdmNo.ToUpper().Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
                            break;
                        case "3":
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && a.FYP_ChallanNo.ToUpper().Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.FYP_ChallanNo).ToArray();
                            break;
                        case "4":
                            var date_format = data.searchdate.ToString("dd/MM/yyyy");

                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                    // from g in list
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
               ).Distinct().ToArray();

                            break;
                        case "5":
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
                            break;

                        case "6":
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && f.ASMC_SectionName.ToUpper().Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.sectionname).ToArray();
                            break;
                    }
                }


                else
                {
                    switch (data.searchType)
                    {

                        case "0":
                            string str = "";
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && (((c.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.ToUpper().Trim()) == true ? str : c.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.ToUpper().Trim()) == true ? str : c.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_LastName.ToUpper().StartsWith(data.searchtext)) && d.ASMAY_Id == data.ASMAY_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null)
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                            break;
                        case "1":
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && b.AMST_Id == data.Amst_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && e.ASMCL_ClassName.ToUpper().Contains(data.searchtext))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.classname).ToArray();
                            break;
                        case "2":
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && b.AMST_Id == data.Amst_Id && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && c.AMST_AdmNo.ToUpper().Contains(data.searchtext))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
                            break;
                        case "3":
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && b.AMST_Id == data.Amst_Id  && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && a.FYP_ChallanNo.ToUpper().Contains(data.searchtext) )
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.FYP_ChallanNo).ToArray();
                            break;
                        case "4":
                            var date_format = data.searchdate.ToString("dd/MM/yyyy");

                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                    // from g in list
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && b.AMST_Id == data.Amst_Id  && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
               ).Distinct().ToArray();

                            break;
                        case "5":
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && b.AMST_Id == data.Amst_Id  && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber) )
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
                            break;

                        case "6":
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                from e in _FeeGroupContext.admissioncls
                                                from f in _FeeGroupContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && b.AMST_Id == data.Amst_Id  && a.FYP_ChallanNo != " " && a.FYP_ChallanNo != null && f.ASMC_SectionName.ToUpper().Contains(data.searchtext) )
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_ChallanNo,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date
                                                }
                  ).Distinct().OrderBy(t => t.sectionname).ToArray();
                            break;
                    }


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
