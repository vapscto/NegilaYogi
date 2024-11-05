using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Dynamic;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DataAccessMsSqlServerProvider;
using CommonLibrary;
using System.Text;
using SendGrid.Helpers.Mail;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
using SendGrid;
using System.Text.RegularExpressions;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.Fee;
using System.Net;
using Razorpay.Api;
using Newtonsoft.Json.Linq;
using easebuzz_.net;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeStudentTransactionImpl : interfaces.FeeStudentTransactionInterface
    {
        private static ConcurrentDictionary<string, FeeStudentTransactionDTO> _login =
        new ConcurrentDictionary<string, FeeStudentTransactionDTO>();

        private static readonly Object obj = new Object();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<FeeStudentTransactionImpl> _logger;
        public FeeStudentTransactionImpl(FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context, ILogger<FeeStudentTransactionImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
            _logger = log;
        }
        public FeeStudentTransactionDTO getdata(FeeStudentTransactionDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = allyear.Distinct().ToArray();

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id && t.userid == data.userid).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_Id).ToList();
                data.fillmastergroup = group.ToArray();

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();
                data.Classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();
                data.sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToArray();


                var rolename = _YearlyFeeGroupMappingContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleid).IVRMRT_Role;

                data.rolename = rolename;

                bool recsettingval = false;
                string maxval = ""; int userprev = 0;
                var getcurrsett = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                foreach (var value in getcurrsett)
                {
                    if (value.FMC_AutoReceiptFeeGroupFlag == 1)
                    {
                        recsettingval = true;
                    }
                    else
                    {
                        recsettingval = false;
                    }
                    userprev = value.FMC_USER_PREVILEDGE;
                }

                List<long> fetchmaxfypid = new List<long>();

                if (userprev == 1)
                {
                    fetchmaxfypid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                     where (a.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.user_id == data.userid && a.FYP_Bank_Or_Cash != null)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FYP_Id = a.FYP_Id,
                                     }
              ).Distinct().OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();
                }
                else
                {
                    fetchmaxfypid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                     where (a.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Bank_Or_Cash != null)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FYP_Id = a.FYP_Id,
                                     }
              ).Distinct().OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();
                }


                data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                          from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                          from e in _YearlyFeeGroupMappingContext.admissioncls
                                          from f in _YearlyFeeGroupMappingContext.school_M_Section
                                          where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                          select new FeeStudentTransactionDTO
                                          {
                                              Amst_Id = c.AMST_Id,
                                              AMST_FirstName = c.AMST_FirstName,
                                              AMST_MiddleName = c.AMST_MiddleName,
                                              AMST_LastName = c.AMST_LastName,
                                              FYP_Receipt_No = a.FYP_Receipt_No,
                                              FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                              FYP_Tot_Amount = a.FYP_Tot_Amount,
                                              classname = e.ASMCL_ClassName,
                                              sectionname = f.ASMC_SectionName,
                                              FYP_Id = a.FYP_Id,
                                              AMST_AdmNo = c.AMST_AdmNo,
                                              FYP_Date = a.FYP_Date,
                                              userid = a.user_id,
                                              FYP_Chq_Bounce = a.FYP_Chq_Bounce,
                                          }
     ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                data.specialheaddetails = (from a in _YearlyFeeGroupMappingContext.feespecialHead
                                           from b in _YearlyFeeGroupMappingContext.feeSGGG
                                           from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                           where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id && c.FMH_SpecialFeeFlag == true)//&& a.IVRMSTAUL_Id==data.User_Id
                                           select new FeeSpecialFeeGroupDTO
                                           {
                                               FMSFH_Id = a.FMSFH_Id,
                                               FMSFH_Name = a.FMSFH_Name,
                                               FMSFHFH_Id = b.FMSFHFH_Id,
                                               FMH_ID = b.FMH_Id,
                                               FMH_Name = c.FMH_FeeName
                                           }).Distinct().ToArray();

                var specialheadlist = _YearlyFeeGroupMappingContext.feespecialHead.Where(t => t.MI_Id == data.MI_Id && t.FMSFH_ActiceFlag == true).Distinct().ToList();
                data.specialheadlist = specialheadlist.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO getdatastuacad(FeeStudentTransactionDTO data)
        {
            try
            {
                var fetchmaxfypid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                     where (a.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.user_id == data.userid)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FYP_Id = a.FYP_Id,
                                     }
             ).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                          from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                          from e in _YearlyFeeGroupMappingContext.admissioncls
                                          from f in _YearlyFeeGroupMappingContext.school_M_Section
                                          where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && (string.IsNullOrEmpty(a.FYP_ChallanNo)))
                                          select new FeeStudentTransactionDTO
                                          {
                                              Amst_Id = c.AMST_Id,
                                              AMST_FirstName = c.AMST_FirstName,
                                              AMST_MiddleName = c.AMST_MiddleName,
                                              AMST_LastName = c.AMST_LastName,
                                              FYP_Receipt_No = a.FYP_Receipt_No,
                                              FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                              FYP_Tot_Amount = a.FYP_Tot_Amount,
                                              classname = e.ASMCL_ClassName,
                                              sectionname = f.ASMC_SectionName,
                                              FYP_Id = a.FYP_Id,
                                              AMST_AdmNo = c.AMST_AdmNo,
                                              FYP_Date = a.FYP_Date,
                                              userid = a.user_id,
                                              FYP_Chq_Bounce = a.FYP_Chq_Bounce,
                                          }
     ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO getdatastuacadgrp(FeeStudentTransactionDTO data)
        {
            try
            {
                if (data.configset.Equals("T"))
                {
                    var readterms = (from a in _YearlyFeeGroupMappingContext.feeTr
                                     where (a.MI_Id == data.MI_Id)
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
                        using (var cmd1 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "gettermsstatisticdetails";
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                             SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });


                            cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
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
                                Value = data.userid
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
                }


                if (data.filterinitialdata != "Preadmission")
                {
                    if (data.configset.Equals("T"))
                    {
                        data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                                from b in _YearlyFeeGroupMappingContext.feeTr
                                                from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == c.FTI_Id && c.AMST_Id == data.Amst_Id && c.MI_Id == b.MI_Id && c.ASMAY_Id == data.ASMAY_Id && a.FMH_Id == d.FMH_Id && d.FMH_Flag != "F") /*&& a.fmg_id.Contains(data.fmg_id)*/
                                                select new FeeStudentTransactionDTO
                                                {
                                                    FMG_GroupName = b.FMT_Name,
                                                    FMG_Id = a.FMT_Id,
                                                    FMT_Id = a.FMT_Id,
                                                    fmt_order = b.FMT_Order
                                                }
).Distinct().OrderBy(t => t.fmt_order).ToArray();

                    }
                    else
                    {
                        data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                from c in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                                from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                where (d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == d.MI_Id && d.AMST_Id == data.Amst_Id && a.FMG_Id == d.FMG_Id && a.FMG_ActiceFlag == true && a.user_id == data.userid)
                                                select new FeeStudentTransactionDTO
                                                {
                                                    FMG_GroupName = a.FMG_GroupName,
                                                    FMG_Id = a.FMG_Id,
                                                }
         ).Distinct().ToArray();

                    }


                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.admissioncls
                                        from d in _YearlyFeeGroupMappingContext.school_M_Section
                                        where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                            AMST_RegistrationNo = a.AMST_RegistrationNo,
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            classname = c.ASMCL_ClassName,
                                            sectionname = d.ASMC_SectionName,
                                            fathername = a.AMST_FatherName,
                                            studentdob = a.AMST_DOB,
                                            amst_mobile = a.AMST_MobileNo,
                                            AMST_Photoname = a.AMST_Photoname
                                        }
                   ).Distinct().ToArray();

                    //parveen addead
                    data.filusername = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.admissioncls
                                        from d in _YearlyFeeGroupMappingContext.school_M_Section
                                        from e in _YearlyFeeGroupMappingContext.StudentAppUserLoginDMO
                                        from f in _YearlyFeeGroupMappingContext.applicationUser
                                        where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && e.AMST_ID == a.AMST_Id && e.STD_APP_ID == f.Id)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                            AMST_RegistrationNo = a.AMST_RegistrationNo,
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            classname = c.ASMCL_ClassName,
                                            sectionname = d.ASMC_SectionName,
                                            fathername = a.AMST_FatherName,
                                            studentdob = a.AMST_DOB,
                                            amst_mobile = a.AMST_MobileNo,
                                            portalusername = f.UserName
                                        }
                  ).Distinct().ToArray();
                    //End


                    data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                   from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                   from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                   from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                   from g in _YearlyFeeGroupMappingContext.admissioncls
                                                   from h in _YearlyFeeGroupMappingContext.school_M_Section
                                                   from i in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                   from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                   where (a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id)
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       Amst_Id = f.AMST_Id,
                                                       AMST_FirstName = i.AMST_FirstName,
                                                       AMST_MiddleName = i.AMST_MiddleName,
                                                       AMST_LastName = i.AMST_LastName,
                                                       FMH_FeeName = d.FMH_FeeName,
                                                       FTI_Name = e.FTI_Name,
                                                       FYP_Receipt_No = b.FYP_Receipt_No,
                                                       FTP_Paid_Amt = a.FTP_Paid_Amt,
                                                       FYP_Tot_Concession_Amt = a.FTP_Concession_Amt,
                                                       FSS_FineAmount = a.FTP_Fine_Amt,
                                                       FYP_Date = b.FYP_Date,
                                                       classname = g.ASMCL_ClassName,
                                                       sectionname = h.ASMC_SectionName,
                                                       rollno = f.AMAY_RollNo,
                                                       admno = i.AMST_AdmNo,
                                                       fathername = i.AMST_FatherName,
                                                       FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                                                       FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                                                       FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                                                       FYP_Bank_Name = b.FYP_Bank_Name,
                                                       FYP_Remarks = b.FYP_Remarks,
                                                       AMST_RegistrationNo = i.AMST_RegistrationNo
                                                   }
                   ).Distinct().ToArray();
                }

                var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_CurrentYrCharges = a.FSS_CurrentYrCharges,
                                                 FSS_TotalToBePaid = a.FSS_TotalToBePaid,
                                                 FSS_ToBePaid = a.FSS_ToBePaid,
                                                 FSS_PaidAmount = a.FSS_PaidAmount,
                                                 FSS_ConcessionAmount = a.FSS_ConcessionAmount,
                                                 FSS_AdjustedAmount = a.FSS_AdjustedAmount,
                                                 FSS_WaivedAmount = a.FSS_WaivedAmount,
                                                 FSS_RefundAmount = a.FSS_RefundAmount,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMG_Id = b.FMG_Id,
                                                 FSS_RunningExcessAmount = a.FSS_RunningExcessAmount,
                                                 FSS_FineAmount=a.FSS_FineAmount

                                             }
        ).ToList();

                data.showstaticticsdetails = (from i in showstaticticsdetails
                                              group i by new { i.FMG_Id, i.FMG_GroupName } into g
                                              select new FeeStudentTransactionDTO
                                              {
                                                  FSS_CurrentYrCharges = g.Sum(t => t.FSS_CurrentYrCharges),
                                                  FSS_TotalToBePaid = g.Sum(t => t.FSS_TotalToBePaid),
                                                  FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid),
                                                  FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),
                                                  FSS_ConcessionAmount = g.Sum(t => t.FSS_ConcessionAmount),
                                                  FSS_AdjustedAmount = g.Sum(t => t.FSS_AdjustedAmount),
                                                  FSS_WaivedAmount = g.Sum(t => t.FSS_WaivedAmount),
                                                  FSS_RefundAmount = g.Sum(t => t.FSS_RefundAmount),
                                                  FMG_GroupName = g.Key.FMG_GroupName,
                                                  FMG_Id = g.Key.FMG_Id,
                                                  FSS_RunningExcessAmount = g.Sum(t => t.FSS_RunningExcessAmount),
                                                  FSS_FineAmount = g.Sum(t=>t.FSS_FineAmount)


                                              }).Distinct().ToArray();

                DateTime date = DateTime.Now;
                var route_details = (from a in _YearlyFeeGroupMappingContext.TR_Student_RouteDMO
                                     from b in _YearlyFeeGroupMappingContext.MasterRouteDMO
                                     where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && a.TRSR_ActiveFlg == true && a.TRSR_Date <= date && b.MI_Id == a.MI_Id && b.TRMR_ActiveFlg == true && b.TRMR_Id == a.TRMR_Id)
                                     select new FeeStudentTransactionDTO
                                     {
                                         Amst_Id = a.AMST_Id,
                                         TRMR_Id = a.TRMR_Id,
                                         TRMR_PickRouteName = a.TRMR_Id != 0 ? _YearlyFeeGroupMappingContext.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.TRMR_Id).TRMR_RouteName : "--",

                                         TRMR_Drop_Route = a.TRMR_Drop_Route,

                                         TRMR_DropRouteName = a.TRMR_Drop_Route != null || a.TRMR_Drop_Route != 0 ? _YearlyFeeGroupMappingContext.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.TRMR_Drop_Route).TRMR_RouteName : "--",
                                         TRSR_Date = a.TRSR_Date
                                     }
                                      ).Distinct().ToList();
                if (route_details.Count > 0)
                {
                    data.routedetails = route_details.ToArray();
                }

                data.fillmastergroupforamount = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                                 from c in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                                 from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                 where (d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == d.MI_Id && d.AMST_Id == data.Amst_Id && a.FMG_Id == d.FMG_Id && a.FMG_Id == b.FMG_Id && b.AMST_Id == data.Amst_Id && a.FMG_ActiceFlag == true && c.FMG_ID == b.FMG_Id && c.User_Id == data.userid)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FMG_GroupName = a.FMG_GroupName,
                                                     FMG_Id = a.FMG_Id,
                                                 }
       ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO getstuddet(FeeStudentTransactionDTO data)
        {
            try
            {
                if (data.filterinitialdata.Equals("regular"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("InActive"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("regno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_RegistrationNo,
                                            AMST_MiddleName = "",
                                            AMST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("AdmNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_AdmNo,
                                            AMST_MiddleName = "",
                                            AMST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("Admnoname"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_AdmNo + "-" + a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameAdmno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_AdmNo,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName + "-" + a.AMST_AdmNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameRegNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("RegNoName"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();
                }

            }
            catch (Exception e)
            {
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }
        public FeeStudentTransactionDTO getstuddetnew(FeeStudentTransactionDTO data)
        {
            try
            {
                data.Bankname = (from a in _YearlyFeeGroupMappingContext.Fee_Master_BankDMO
                                 where (a.MI_Id == data.MI_Id && a.FMBANK_ActiveFlg == true)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMBANK_Id = a.FMBANK_Id,
                                     FMBANK_BankName = a.FMBANK_BankName,
                                 }
    ).Distinct().ToArray();

                int ecsflag = 0;
                if (data.filterinitialdata != "Preadmission")
                {

                    var saved_fma = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                     from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                     where (a.FMA_Id == b.FMA_Id && b.FYP_Id == c.FYP_Id && a.AMST_Id == c.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                     select b.FMA_Id
).Distinct().ToList();




                    var fetchclass = (from a in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                      from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                      where (a.AMST_Id == b.AMST_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                      select new FeeStudentTransactionDTO
                                      {
                                          ASMCL_ID = a.ASMCL_Id,
                                          ASMAY_Id = a.ASMAY_Id,
                                          AMST_ECSFlag = b.AMST_ECSFlag
                                      }
).Distinct().ToArray();

                    string classid = "0", academicyearid = "0";

                    for (int s = 0; s < fetchclass.Count(); s++)
                    {
                        classid = fetchclass[s].ASMCL_ID.ToString();
                        academicyearid = fetchclass[s].ASMAY_Id.ToString();
                        ecsflag = Convert.ToInt32(fetchclass[s].AMST_ECSFlag);
                    }

                    var myArray = data.multiplegroups.Split(',');
                    List<long> terms_groups = new List<long>();
                    for (int i = 0; i < myArray.Length; i++)
                    {
                        terms_groups.Add(Convert.ToInt64(myArray[i]));
                    }
                    data.terms_groups = terms_groups.ToArray();
                    //try
                    //{
                    //    var config = _YearlyFeeGroupMappingContext.FeeMasterConfiguration.Where(t => t.MI_Id == data.MI_Id && t.FMC_ReadmitFineCalculationFlg == 1).ToList();

                    //    if (config.Count > 0)
                    //    {
                    //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("ReadmissionFeeInsertStthomasStudentwiseOnload @p0,@p1,@p2,@p3", data.MI_Id, data.Amst_Id, data.FYP_Date.Date.ToString("yyyy-MM-dd"),data.multiplegroups);
                    //    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}



                    if (data.configset.Equals("G"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Student_Fee_Det_all @p0,@p1,@p2,@p3,@p4,@p5,@p6", data.Amst_Id, data.ASMAY_Id, data.FYP_Date, data.MI_Id, data.configset, data.multiplegroups, data.userid);

                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from b in _YearlyFeeGroupMappingContext.feeMIY
                                              from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                              where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new Head_Installments_DTO
                                              {
                                                  FTI_Name = b.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();

                    }
                    else if (data.configset.Equals("T"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Student_Fee_Det_term_all @p0,@p1,@p2,@p3,@p4,@p5,@p6", data.Amst_Id, data.ASMAY_Id, data.FYP_Date, data.MI_Id, data.configset, data.multiplegroups, data.userid);

                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from d in _YearlyFeeGroupMappingContext.feeMIY
                                              from b in _YearlyFeeGroupMappingContext.feeMTH
                                              from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                              from f in _YearlyFeeGroupMappingContext.feeYCCC
                                              from g in _YearlyFeeGroupMappingContext.feeYCC
                                              where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new Head_Installments_DTO
                                              {
                                                  FTI_Name = d.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();

                    }
                }
                else if (data.filterinitialdata.Equals("Preadmission"))
                {
                    _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Student_Fee_Det_Preadmission @p0,@p1,@p2,@p3,@p5", data.Amst_Id, data.ASMAY_Id, data.FYP_Date, data.MI_Id);
                }

                data.alldata = (from a in _YearlyFeeGroupMappingContext.V_StudentPendingDMO
                                from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                where (a.mi_id == data.MI_Id && a.fmg_id == b.FMG_Id && c.MI_Id == data.MI_Id && c.FMH_Id == a.fmh_id)
                                select new FeeStudentTransactionDTO
                                {
                                    FMA_Id = a.fma_id,
                                    FMH_FeeName = a.FMH_FeeName,
                                    FMH_Flag = c.FMH_Flag,
                                    FTI_Name = a.FTI_Name,
                                    FMH_Id = a.fmh_id,
                                    FTI_Id = a.fti_id,
                                    totalamount = a.FSS_NetAmount,
                                    FSS_ToBePaid = a.FSS_ToBePaid,
                                    FSS_ConcessionAmount = a.FSS_ConcessionAmount,
                                    FSS_FineAmount = a.FSS_FineAmount,
                                    FSS_CurrentYrCharges = a.FSS_CurrentYrCharges,
                                    FSS_TotalToBePaid = a.FSS_TotalToBePaid,
                                    FMG_Id = a.fmg_id,
                                    FMG_GroupName = b.FMG_GroupName,
                                    FSS_OBArrearAmount = a.FSS_OBArrearAmount,
                                    FMT_Id = a.FMT_Id,
                                    FSS_RebateAmount = a.FSS_RebateAmount,
                                }

          ).OrderBy(t => t.FMH_Id).ThenBy(t => t.FMT_Id).ToArray();

                if (data.alldata.Length > 0)
                {
                    var count_res = _YearlyFeeGroupMappingContext.V_StudentPendingDMO.Where(r => r.mi_id == data.MI_Id).Select(r => r.fmg_id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new FeeStudentTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

                    data.validationgroupid = count_res.FMG_Id;
                    data.validationgrougidcount = count_res.grp_count;
                    //MB
                    List<FeeStudentTransactionDTO> fines_fma_ids = new List<FeeStudentTransactionDTO>();
                    var fine_amount = 0;


                    ///readmission Condition
                   
                    var readmissionconditoncheck = _YearlyFeeGroupMappingContext.FeeMasterConfiguration.Where(t => t.MI_Id == data.MI_Id && t.FMC_ReadmitFineCalculationFlg == 1).ToList();
                    if (readmissionconditoncheck.Count > 0)
                    {
                        try
                        {
                            using (var cmd1 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd1.CommandText = "Readmissionfeeschecking";
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                               SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                                 SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });


                                cmd1.Parameters.Add(new SqlParameter("@AMST_Id",
                                SqlDbType.VarChar)
                                {
                                    Value = data.Amst_Id
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
                                    data.Readmissionfeeschecking = retObject1.ToArray();
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
                        if (data.Readmissionfeeschecking.Length > 0)
                        {
                            var readmssionflg = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeHeadDMO

                                                 where (a.AMST_Id == data.Amst_Id
                                                 && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMH_Id == b.FMH_Id && b.FMH_Flag == "RA" && a.FSS_ToBePaid > 0)
                                                 select a.AMST_Id
    ).Distinct().ToList();
                            foreach (FeeStudentTransactionDTO x in data.alldata)
                            {
                                FeeStudentTransactionDTO sew = new FeeStudentTransactionDTO();
                                if ((data.Readmissionfeeschecking == null || data.Readmissionfeeschecking.Length == 0) || (readmssionflg.Count == 0))
                                {
                                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        if (ecsflag.Equals(0))
                                        {
                                            cmd.CommandText = "Sp_Calculate_Fine";
                                        }
                                        else
                                        {
                                            cmd.CommandText = "Sp_Calculate_Fine_ECS";
                                        }

                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@On_Date",
                                            SqlDbType.DateTime)
                                        {
                                            Value = data.FYP_Date
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@fma_id",
                                           SqlDbType.BigInt)
                                        {
                                            Value = x.FMA_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                       SqlDbType.BigInt)
                                        {
                                            Value = data.ASMAY_Id
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
                                        fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);

                                        sew.FMA_Id = x.FMA_Id;
                                        sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        fines_fma_ids.Add(sew);
                                    }
                                }
                            }
                            data.Fine_FMA_Ids = fines_fma_ids.Distinct().ToArray();

                            //    foreach (var x in list)
                            //    {
                            //        amount_list.Add(x);
                            //        if ((data.Readmissionfeeschecking == null || data.Readmissionfeeschecking.Length == 0) || (readmssionflg.Count == 0))
                            //        {


                            //            using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                            //            {
                            //                if (ecsflag.Equals(0))
                            //                {
                            //                    cmd.CommandText = "Sp_Calculate_Fine";
                            //                }
                            //                else
                            //                {
                            //                    cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            //                }

                            //                //cmd.CommandText = "Sp_Calculate_Fine";
                            //                cmd.CommandType = CommandType.StoredProcedure;
                            //                cmd.Parameters.Add(new SqlParameter("@On_Date",
                            //                    SqlDbType.DateTime, 100)
                            //                {
                            //                    //Value = currdt
                            //                    Value = indianTime
                            //                });

                            //                cmd.Parameters.Add(new SqlParameter("@fma_id",
                            //                   SqlDbType.NVarChar, 100)
                            //                {
                            //                    Value = x.FMA_Id
                            //                });
                            //                cmd.Parameters.Add(new SqlParameter("@asmay_id",
                            //               SqlDbType.NVarChar, 100)
                            //                {
                            //                    Value = data.ASMAY_Id
                            //                });

                            //                cmd.Parameters.Add(new SqlParameter("@amt",
                            //    SqlDbType.Decimal, 500)
                            //                {
                            //                    Direction = ParameterDirection.Output
                            //                });

                            //                cmd.Parameters.Add(new SqlParameter("@flgArr",
                            //   SqlDbType.Int, 500)
                            //                {
                            //                    Direction = ParameterDirection.Output
                            //                });

                            //                if (cmd.Connection.State != ConnectionState.Open)
                            //                    cmd.Connection.Open();

                            //                var data1 = cmd.ExecuteNonQuery();

                            //                var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                            //                                     where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == data.Amst_Id)
                            //                                     select new FeeStudentTransactionDTO
                            //                                     {
                            //                                         FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                            //                                         FSWO_FineFlg = a.FSWO_FineFlg,
                            //                                         FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                            //                                     }
                            //).Distinct().ToList();

                            //                if (waivedofffine.Count() > 0)
                            //                {
                            //                    if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                            //                    {
                            //                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                            //                        {
                            //                            fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                            //                            totalfine = totalfine + fineamt;
                            //                        }
                            //                    }
                            //                    else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                            //                    {
                            //                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                            //                        {
                            //                            fineamt = 0;
                            //                            totalfine = totalfine + fineamt;
                            //                        }
                            //                    }

                            //                    else
                            //                    {
                            //                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                            //                        {
                            //                            fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            //                            totalfine = totalfine + fineamt;
                            //                        }
                            //                    }

                            //                }
                            //                else
                            //                {
                            //                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                            //                    {
                            //                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            //                        totalfine = totalfine + fineamt;
                            //                    }
                            //                }

                            //                if (Convert.ToInt32(fineamt) > 0)
                            //                {
                            //                    if (fineheadfmaid.Count() > 0)
                            //                    {
                            //                        finelst.Add(new FeeStudentTransactionDTO
                            //                        {
                            //                            FMA_Id = x.FMA_Id,
                            //                            FSS_ToBePaid = Convert.ToInt32(fineamt),
                            //                            FSS_FineAmount = 0,
                            //                            FSS_ConcessionAmount = 0,
                            //                            FSS_WaivedAmount = 0,
                            //                            FMG_Id = 0,
                            //                            FMH_Id = 0,
                            //                            FTI_Id = 0,
                            //                            FSS_PaidAmount = 0,
                            //                            FSS_NetAmount = 0,
                            //                            FSS_RefundAmount = 0,
                            //                            FMH_FeeName = fineheadfmaid[0].FMH_FeeName,
                            //                            FTI_Name = "Anytime",
                            //                            FSS_CurrentYrCharges = 0,
                            //                            FSS_TotalToBePaid = 0,
                            //                        });
                            //                    }
                            //                }
                            //                fineamt = 0;
                            //            }
                            //        }
                            //    }
                        }
                        else
                        {
                        

                            foreach (FeeStudentTransactionDTO x in data.alldata)
                            {
                                FeeStudentTransactionDTO sew = new FeeStudentTransactionDTO();
                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                {
                                    if (ecsflag.Equals(0))
                                    {
                                        cmd.CommandText = "Sp_Calculate_Fine";
                                    }
                                    else
                                    {
                                        cmd.CommandText = "Sp_Calculate_Fine_ECS";
                                    }

                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@On_Date",
                                        SqlDbType.DateTime)
                                    {
                                        Value = data.FYP_Date
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@fma_id",
                                       SqlDbType.BigInt)
                                    {
                                        Value = x.FMA_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                   SqlDbType.BigInt)
                                    {
                                        Value = data.ASMAY_Id
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
                                    fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);

                                    sew.FMA_Id = x.FMA_Id;
                                    sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    fines_fma_ids.Add(sew);
                                }
                            }
                            data.Fine_FMA_Ids = fines_fma_ids.Distinct().ToArray();
                        }

                    }
                    else
                    {







                        foreach (FeeStudentTransactionDTO x in data.alldata)
                        {
                            FeeStudentTransactionDTO sew = new FeeStudentTransactionDTO();
                            using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                            {
                                if (ecsflag.Equals(0))
                                {
                                    cmd.CommandText = "Sp_Calculate_Fine";
                                }
                                else
                                {
                                    cmd.CommandText = "Sp_Calculate_Fine_ECS";
                                }

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@On_Date",
                                    SqlDbType.DateTime)
                                {
                                    Value = data.FYP_Date
                                });

                                cmd.Parameters.Add(new SqlParameter("@fma_id",
                                   SqlDbType.BigInt)
                                {
                                    Value = x.FMA_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@asmay_id",
                               SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
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
                                fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);

                                sew.FMA_Id = x.FMA_Id;
                                sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                fines_fma_ids.Add(sew);
                            }
                        }
                        data.Fine_FMA_Ids = fines_fma_ids.Distinct().ToArray();
                    }

                    ///readmission Condition
                    //MB
                }

                if (data.configset.Equals("T"))
                {
                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TERMWISETOTALAMOUNT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                      SqlDbType.VarChar)
                        {
                            Value = data.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.Amst_Id
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
                            data.termwiseamount = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.configset.Equals("G"))
                {
                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GROUPWISETOTALAMOUNT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                      SqlDbType.VarChar)
                        {
                            Value = data.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.Amst_Id
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
                            data.groupwiseamount = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }


                data.fetchmodeofpayment = (from a in _YearlyFeeGroupMappingContext.IVRM_ModeOfPayment
                                           where (a.MI_Id == data.MI_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               IVRMMOD_Id = a.IVRMMOD_Id,
                                               IVRMMOD_ModeOfPayment = a.IVRMMOD_ModeOfPayment,
                                               IVRMMOD_Flag = a.IVRMMOD_Flag,
                                               IVRMMOD_ModeOfPayment_Code = a.IVRMMOD_ModeOfPayment_Code
                                           }
).Distinct().ToArray();



                data.narrationlist = _YearlyFeeGroupMappingContext.MasterNarrationDMO.Where(t => t.MI_ID == data.MI_Id && t.FMNAR_ActiveFlag==true).ToArray();

            }
            catch (Exception e)
            {
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }
        public async Task<FeeStudentTransactionDTO> savedetails(FeeStudentTransactionDTO pgmod)
        {
            DateTime dt = DateTime.Now;
            int r = 0;

            try
            {
                pgmod.FYP_Chq_Bounce = "CL";
                if (pgmod.FYP_Id != 0)
                {
                    var result = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.FYP_Id == pgmod.FYP_Id && t.ASMAY_ID == pgmod.ASMAY_Id);

                    FeePaymentDetailsDMO pmm = new FeePaymentDetailsDMO();

                    if (pgmod.automanualreceiptno != "Auto" || pgmod.auto_receipt_flag != 1)
                    {
                        result.FYP_Receipt_No = pgmod.FYP_Receipt_No;
                    }

                    result.FYP_Date = Convert.ToDateTime(pgmod.FYPcurr_Date);

                    if (pgmod.FYP_Remarks != "")
                    {
                        result.FYP_Remarks = pgmod.FYP_Remarks;
                    }
                    _YearlyFeeGroupMappingContext.Update(result);

                    //added by kavitha
                    foreach (var mode in pgmod.Modes)
                    {
                        if (mode.FYPPM_TotalPaidAmount > 0)
                        {
                            if (mode.FYPPM_TransactionTypeFlag == "C")
                            {
                                result.FYP_Bank_Name = "";
                                result.FYP_DD_Cheque_Date = dt;
                                result.FYP_DD_Cheque_No = "";
                                result.FYP_Bank_Or_Cash = mode.FYPPM_TransactionTypeFlag;
                            }
                            else if (mode.FYPPM_TransactionTypeFlag == "B" || mode.FYPPM_TransactionTypeFlag == "E" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R")
                            {
                                result.FYP_Bank_Name = mode.FYPPM_BankName;
                                result.FYP_DD_Cheque_Date = mode.FYPPM_DDChequeDate;
                                result.FYP_DD_Cheque_No = mode.FYPPM_DDChequeNo;
                                result.FYP_Bank_Or_Cash = mode.IVRMMOD_ModeOfPayment_Code;
                                _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Update(result);
                            }
                        }
                    }

                    for (int i = 0; i < pgmod.Modes.Length; i++)
                    {
                        var resultupdate = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool.Where(t => t.FYPPM_Id == pgmod.Modes[i].FYPPM_Id).FirstOrDefault();

                        resultupdate.FYP_TransactionTypeFlag = pgmod.Modes[i].IVRMMOD_ModeOfPayment_Code;


                        resultupdate.FYPPM_TotalPaidAmount = pgmod.Modes[i].FYPPM_TotalPaidAmount;
                        resultupdate.FYPPM_LedgerId = 0;
                        resultupdate.FYPPM_BankName = pgmod.Modes[i].FYPPM_TransactionTypeFlag == "C" ? "" : pgmod.Modes[i].FYPPM_BankName;
                        resultupdate.FYPPM_DDChequeNo = pgmod.Modes[i].FYPPM_TransactionTypeFlag == "C" ? "" : pgmod.Modes[i].FYPPM_DDChequeNo;
                        resultupdate.FYPPM_DDChequeDate = pgmod.Modes[i].FYPPM_TransactionTypeFlag == "C" ? pgmod.FYP_Date : pgmod.Modes[i].FYPPM_DDChequeDate;
                        resultupdate.FYPPM_TransactionId = "";
                        resultupdate.FYPPM_PaymentReferenceId = "";

                        resultupdate.FYPPM_ClearanceStatusFlag = "0";
                        resultupdate.FYPPM_ClearanceDate = pgmod.Modes[i].FYPPM_DDChequeDate;
                        _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool.Update(resultupdate);
                    }
                    //added by kavitha

                    //kiran
                    foreach (var y in pgmod.savetmpdata)
                    {
                        var fineheads = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                         from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                         where (a.MI_Id == pgmod.MI_Id && a.FMA_Id == c.FMA_Id && a.FMH_Id == b.FMH_Id && a.FMA_Id == y.FMA_Id && a.ASMAY_Id == pgmod.ASMAY_Id && b.FMH_Flag == "F" && c.FYP_Id == pgmod.FYP_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FMA_Id = a.FMA_Id,
                                         }
                          ).Distinct().Take(1);

                        if (fineheads.Count() > 0)
                        {
                            //tpayment
                            var sta_tpayment = _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Single(t => t.FMA_Id == y.FMA_Id && t.FYP_Id == pgmod.FYP_Id);

                            //status
                            var sta_status = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FMA_Id == y.FMA_Id && t.AMST_Id == pgmod.Amst_Id);

                            if (sta_tpayment.FTP_Paid_Amt > y.FSS_ToBePaid)
                            {
                                var updatedpaid = sta_tpayment.FTP_Paid_Amt - y.FSS_ToBePaid;

                                sta_status.FSS_PaidAmount = sta_status.FSS_PaidAmount - Convert.ToInt32(updatedpaid);

                                sta_status.FSS_FineAmount = sta_status.FSS_FineAmount - Convert.ToInt32(updatedpaid);
                            }
                            else if (sta_tpayment.FTP_Paid_Amt < y.FSS_ToBePaid)
                            {
                                var updatedpaid = y.FSS_ToBePaid - sta_tpayment.FTP_Paid_Amt;

                                sta_status.FSS_PaidAmount = sta_status.FSS_PaidAmount + Convert.ToInt32(updatedpaid);

                                sta_status.FSS_FineAmount = sta_status.FSS_FineAmount + Convert.ToInt32(updatedpaid);
                            }
                            else if (sta_tpayment.FTP_Paid_Amt == y.FSS_ToBePaid)
                            {
                                sta_status.FSS_PaidAmount = sta_status.FSS_PaidAmount;

                                sta_status.FSS_FineAmount = sta_status.FSS_FineAmount;
                            }

                            _YearlyFeeGroupMappingContext.Update(sta_status);
                            //status

                            //ystudent
                            var temppayment = _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO.Single(t => t.FYP_Id == pgmod.FYP_Id && t.AMST_Id == pgmod.Amst_Id);

                            temppayment.FTP_TotalPaidAmount = pgmod.FYP_Tot_Amount;
                            _YearlyFeeGroupMappingContext.Update(temppayment);
                            //ystudent

                            sta_tpayment.FTP_Paid_Amt = y.FSS_ToBePaid;
                            //tpayment

                            _YearlyFeeGroupMappingContext.Update(sta_tpayment);
                        }

                    }
                    //kiran

                    var contactexisttransaction = 0;
                    using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                            dbCtxTxn.Commit();
                            pgmod.returnval = "true";
                            pgmod.displaymessage = "Updated";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            dbCtxTxn.Rollback();
                            pgmod.returnval = "false";
                            pgmod.displaymessage = "Not Updated";
                        }
                    }
                }
                else if (pgmod.filterinitialdata != "Preadmission")
                {
                    int j = 0;
                    string list_values = "";
                    FeePaymentDetailsDMO pmm = new FeePaymentDetailsDMO();

                    if (pgmod.savetmpdata != null)
                    {
                        pmm.MI_Id = pgmod.MI_Id;
                        pmm.ASMAY_ID = pgmod.ASMAY_Id;
                        pmm.user_id = pgmod.userid;
                        pgmod.temp_head_list = pgmod.savetmpdata;

                        pmm.FYP_Date = Convert.ToDateTime(pgmod.FYPcurr_Date);

                        //multimode

                        foreach (var mode in pgmod.Modes)
                        {
                            if (mode.FYPPM_TotalPaidAmount > 0)
                            {
                                if (mode.FYPPM_TransactionTypeFlag == "C")
                                {
                                    pmm.FYP_Bank_Name = "";
                                    pmm.FYP_DD_Cheque_Date = dt;
                                    pmm.FYP_DD_Cheque_No = "";
                                    pmm.FYP_Bank_Or_Cash = mode.FYPPM_TransactionTypeFlag;
                                }
                                else if (mode.FYPPM_TransactionTypeFlag == "B" || mode.FYPPM_TransactionTypeFlag == "E" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R")
                                {
                                    pmm.FYP_Bank_Name = mode.FYPPM_BankName;
                                    pmm.FYP_DD_Cheque_Date = mode.FYPPM_DDChequeDate;
                                    pmm.FYP_DD_Cheque_No = mode.FYPPM_DDChequeNo;
                                    pmm.FYP_Bank_Or_Cash = mode.IVRMMOD_ModeOfPayment_Code;
                                }
                            }
                        }

                        pmm.FYP_Chq_Bounce = pgmod.FYP_Chq_Bounce;
                        pmm.FYP_Remarks = pgmod.FYP_Remarks;
                        pmm.FTCU_Id = 1;
                        pmm.FYP_Tot_Amount = pgmod.FYP_Tot_Amount;
                        pmm.FYP_Tot_Concession_Amt = pgmod.FYP_Tot_Concession_Amt;
                        pmm.FYP_Tot_Fine_Amt = pgmod.FYP_Tot_Fine_Amt;
                        pmm.FYP_Tot_Waived_Amt = pgmod.FYP_Tot_Waived_Amt;
                        pmm.DOE = dt;
                        pmm.CreatedDate = dt;
                        pmm.UpdatedDate = dt;
                        pmm.FYP_OnlineChallanStatusFlag = "Sucessfull";
                        pmm.FYP_PayModeType = "APP";
                        pmm.FYP_PayGatewayType = "";
                    }

                    _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Add(pmm);

                    //Multimode of Payment

                    pmm.FYP_DeviseFlg = pgmod.FYP_PayModeType;

                    foreach (var mode in pgmod.Modes)
                    {
                        if (mode.FYPPM_TotalPaidAmount > 0)
                        {
                            Fee_Y_Payment_PaymentModeSchool obj2 = new Fee_Y_Payment_PaymentModeSchool();
                            obj2.FYP_Id = pmm.FYP_Id;

                            obj2.FYP_TransactionTypeFlag = mode.IVRMMOD_ModeOfPayment_Code;

                            obj2.FYPPM_TotalPaidAmount = mode.FYPPM_TotalPaidAmount;
                            obj2.FYPPM_LedgerId = 0;
                            obj2.FYPPM_BankName = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_BankName;
                            obj2.FYPPM_DDChequeNo = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_DDChequeNo;
                            obj2.FYPPM_DDChequeDate = mode.FYPPM_TransactionTypeFlag == "C" ? pgmod.FYP_Date : mode.FYPPM_DDChequeDate;
                            obj2.FYPPM_TransactionId = "";
                            obj2.FYPPM_PaymentReferenceId = "";
                            obj2.FYPPM_ClearanceStatusFlag = "0";
                            obj2.FYPPM_ClearanceDate = mode.FYPPM_DDChequeDate;
                            _YearlyFeeGroupMappingContext.Add(obj2);
                        }
                    }

                    //Multimode of Payment


                    lock (obj)
                    {
                        get_grp_reptno(pgmod);

                        pmm.FYP_Receipt_No = pgmod.FYP_Receipt_No;

                        if (pmm.FYP_Receipt_No == "" || pmm.FYP_Receipt_No == null)
                        {
                            pgmod.returnval = "Record Not Saved because Receipt No is not Generated Automatic.Settings are missing";
                            return pgmod;
                        }
                        else
                        {

                            Fee_Y_Payment_School_StudentDMO temppayment = new Fee_Y_Payment_School_StudentDMO();

                            temppayment.AMST_Id = pgmod.Amst_Id;
                            temppayment.ASMAY_Id = pmm.ASMAY_ID;
                            temppayment.FTP_TotalPaidAmount = pgmod.FYP_Tot_Amount;
                            temppayment.FTP_TotalWaivedAmount = pgmod.FYP_Tot_Waived_Amt;
                            temppayment.FTP_TotalConcessionAmount = pgmod.FYP_Tot_Concession_Amt;
                            temppayment.FTP_TotalFineAmount = pgmod.FYP_Tot_Fine_Amt;
                            temppayment.FYP_Id = pmm.FYP_Id;
                            _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO.Add(temppayment);

                            while (j < pgmod.savetmpdata.Count())
                            {
                                FeeTransactionPaymentDMO feetrapay = new FeeTransactionPaymentDMO();

                                if (pgmod.savetmpdata[j].FSS_ToBePaid > 0 || pgmod.savetmpdata[j].FSS_OBArrearAmount > 0)
                                {
                                    feetrapay.FTP_Id = 0;
                                    feetrapay.FMA_Id = pgmod.savetmpdata[j].FMA_Id;
                                    feetrapay.FTP_Paid_Amt = pgmod.savetmpdata[j].FSS_ToBePaid;
                                    feetrapay.FTP_Concession_Amt = pgmod.savetmpdata[j].FSS_ConcessionAmount;
                                    feetrapay.FTP_Fine_Amt = pgmod.savetmpdata[j].FSS_FineAmount;
                                    feetrapay.FTP_Waived_Amt = pgmod.savetmpdata[j].FSS_WaivedAmount;
                                    feetrapay.ftp_remarks = pgmod.FYP_Remarks;

                                    feetrapay.FYP_Id = pmm.FYP_Id;

                                    list_values = list_values + "(" + pmm.MI_Id + "," + feetrapay.FYP_Id + "," + feetrapay.FMA_Id + "," + feetrapay.FTP_Paid_Amt + "," + feetrapay.FTP_Fine_Amt + "," + feetrapay.FTP_Concession_Amt + "," + feetrapay.FTP_Waived_Amt + "),";

                                    _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Add(feetrapay);

                                    var obj_status_stf = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.AMST_Id == pgmod.Amst_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id && t.FMG_Id == pgmod.savetmpdata[j].FMG_Id && t.FMA_Id == pgmod.savetmpdata[j].FMA_Id && t.FSS_ActiveFlag == true).FirstOrDefault();

                                    obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + pgmod.savetmpdata[j].FSS_ToBePaid;

                                    //added on 11-07-2018
                                    var fineheadss = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                      from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                      where (a.MI_Id == pgmod.MI_Id && a.FMA_Id == pgmod.savetmpdata[j].FMA_Id && a.ASMAY_Id == pgmod.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == pgmod.Amst_Id && a.FMH_Id == b.FMH_Id)
                                                      select new FeeStudentTransactionDTO
                                                      {
                                                          FMH_Id = a.FMH_Id,
                                                      }
                                       ).Distinct().Take(1);

                                    if (fineheadss.Count() > 0)
                                    {
                                        obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + pgmod.savetmpdata[j].FSS_ToBePaid;
                                    }
                                    //added on 11-07-2018


                                    if (obj_status_stf.FSS_NetAmount != 0 || obj_status_stf.FSS_OBArrearAmount != 0)
                                    {
                                        obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - pgmod.savetmpdata[j].FSS_ToBePaid;
                                    }
                                    else
                                    {
                                        obj_status_stf.FSS_ToBePaid = 0;
                                    }

                                    _YearlyFeeGroupMappingContext.Update(obj_status_stf);
                                }

                                j++;
                            }

                            var contactexisttransaction = 0;
                            using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                                    dbCtxTxn.Commit();

                                    pgmod.returnval = "true";
                                    pgmod.displaymessage = "Saved";
                                    pgmod.FYP_Id = pmm.FYP_Id;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    dbCtxTxn.Rollback();
                                    pgmod.returnval = "false";
                                    pgmod.displaymessage = "Not Saved";
                                }
                            }
                        }
                    }

                    _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("pda_status_update @p0,@p1,@p2,@p3", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.Amst_Id, pgmod.FYP_Tot_Amount);

                    try
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("ReceiptVoucherGeneration @p0,@p1,@p2,@p3,@p4,@p5", pgmod.FYP_Receipt_No, pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.userid, pgmod.FYP_Id, "test");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    try
                    {
                        var config = _YearlyFeeGroupMappingContext.FeeMasterConfiguration.Where(t => t.MI_Id == pgmod.MI_Id && t.FMC_ReadmitFineCalculationFlg == 1).ToList();

                        if (config.Count > 0) {
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("ReadmissionFeeInsertStthomasStudentwise @p0,@p1", pgmod.MI_Id, pgmod.Amst_Id);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                    long mobileno = 0;
                    string MailId = "";

                    var getdetails = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                      where (a.AMST_Id == pgmod.Amst_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          amst_mobile = a.AMST_MobileNo,
                                          amst_email_id = a.AMST_emailId,
                                          ASMAY_Id = a.ASMAY_Id
                                      }
            ).ToList();

                    mobileno = getdetails.FirstOrDefault().amst_mobile;
                    MailId = getdetails.FirstOrDefault().amst_email_id;

                    var getdetailstemplate = (from a in _YearlyFeeGroupMappingContext.sMSEmailSetting
                                              where (a.ISES_Template_Name == "FeeAdmissionTransaction" && a.MI_Id == pgmod.MI_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  institutionname = a.ISES_Template_Name
                                              }
          ).ToList();

                    if (getdetailstemplate.Count > 0)
                    {
                        long yearid = getacademicyearcongig(pgmod);

                        if (getdetails.FirstOrDefault().ASMAY_Id == yearid)
                        {

                            var noofreceipts = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                                from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                where (a.FYP_Id == b.FYP_Id && b.FMA_Id == c.FMA_Id && c.FMG_Id == d.FMG_Id && c.MI_Id == pgmod.MI_Id && c.ASMAY_Id == pgmod.ASMAY_Id && a.AMST_Id == pgmod.Amst_Id && d.FMG_CompulsoryFlag != "R")
                                                select new FeeStudentTransactionDTO
                                                {
                                                    FYP_Id = b.FYP_Id
                                                }
         ).Distinct().ToList();
                            if (noofreceipts.Count == 1)
                            {
                                if (pgmod.smsconfig == "true")
                                {
                                    SMS sms = new SMS(_context);

                                    string s = await sms.sendSms(pgmod.MI_Id, mobileno, "FeeAdmissionTransaction", pgmod.Amst_Id);
                                }

                                if (pgmod.emailconfig == "true")
                                {
                                    Email Email = new Email(_context);

                                    string m = Email.sendmail(pgmod.MI_Id, MailId, "FeeAdmissionTransaction", pgmod.Amst_Id);
                                }
                            }
                            else
                            {
                                if (pgmod.smsconfig == "true")
                                {
                                    SMS sms = new SMS(_context);

                                    string s = await sms.sendSms(pgmod.MI_Id, mobileno, "FeeTransaction", pgmod.Amst_Id);
                                }

                                if (pgmod.emailconfig == "true")
                                {
                                    Email Email = new Email(_context);

                                    string m = Email.sendmail(pgmod.MI_Id, MailId, "FeeTransaction", pgmod.Amst_Id);
                                }
                            }

                        }

                        else
                        {
                            if (pgmod.smsconfig == "true")
                            {
                                SMS sms = new SMS(_context);

                                string s = await sms.sendSms(pgmod.MI_Id, mobileno, "FeeTransaction", pgmod.Amst_Id);
                            }

                            if (pgmod.emailconfig == "true")
                            {
                                Email Email = new Email(_context);

                                string m = Email.sendmail(pgmod.MI_Id, MailId, "FeeTransaction", pgmod.Amst_Id);
                            }

                        }

                    }
                    else
                    {
                        if (pgmod.smsconfig == "true")
                        {
                            SMS sms = new SMS(_context);

                            string s = await sms.sendSms(pgmod.MI_Id, mobileno, "FeeTransaction", pgmod.Amst_Id);
                        }
                        if (pgmod.emailconfig == "true")
                        {
                            Email Email = new Email(_context);

                            string m = Email.sendmail(pgmod.MI_Id, MailId, "FeeTransaction", pgmod.Amst_Id);
                        }
                    }
                }

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == pgmod.MI_Id && t.userid == pgmod.userid).ToList();
                pgmod.feeconfiglist = feemasnum.ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id).ToList();
                pgmod.fillyear = year.ToArray();


                var fetchmaxfypid = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_ID == pgmod.ASMAY_Id).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                pgmod.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                           from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                           from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                           from e in _YearlyFeeGroupMappingContext.admissioncls
                                           from f in _YearlyFeeGroupMappingContext.school_M_Section
                                           where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_ID == pgmod.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               Amst_Id = c.AMST_Id,
                                               AMST_FirstName = c.AMST_FirstName,
                                               AMST_MiddleName = c.AMST_MiddleName,
                                               AMST_LastName = c.AMST_LastName,
                                               FYP_Receipt_No = a.FYP_Receipt_No,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_Tot_Amount = a.FYP_Tot_Amount,
                                               classname = e.ASMCL_ClassName,
                                               sectionname = f.ASMC_SectionName,
                                               FYP_Id = a.FYP_Id,
                                               AMST_AdmNo = c.AMST_AdmNo,
                                               FYP_Date = a.FYP_Date,
                                               amst_mobile = c.AMST_MobileNo,
                                               FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                           }
     ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

            }

            catch (Exception ee)
            {
                pgmod.returnval = "false";
                pgmod.displaymessage = "Not Saved";
                Console.WriteLine(ee.Message);
                _logger.LogError("Error in " + pgmod.MI_Id + " - " + ee.InnerException);
            }

            return pgmod;
        }
        public long getacademicyearcongig(FeeStudentTransactionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var currasmayid = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(indianTime) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(indianTime)).Select(d => d.ASMAY_Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.ASMAY_Id;
        }
        public FeeStudentTransactionDTO delrec(FeeStudentTransactionDTO data)
        {
            try
            {

                var obj_status_stf = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(t => t.FYP_Id == data.FYP_Id).FirstOrDefault();
                obj_status_stf.FYP_Remarks = obj_status_stf.FYP_Remarks + " - " + data.FYP_Remarks;
                _YearlyFeeGroupMappingContext.Update(obj_status_stf);
                _YearlyFeeGroupMappingContext.SaveChanges();

                var lorg1 = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg3 = _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();
                var lorg34 = _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).Select(t => t.FMA_Id).ToList();
                var lorg5 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool.Where(t => t.FYP_Id == data.FYP_Id).ToList();
                var lorg6 = _YearlyFeeGroupMappingContext.FeeStudentRebateDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();


                var delcandel = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 where (a.FMA_Id == b.FMA_Id && c.AMST_Id == a.AMST_Id && c.ASMAY_Id == a.ASMAY_Id && c.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FYP_Id == data.FYP_Id && lorg34.Contains(a.FMA_Id) && a.AMST_Id == lorg2.AMST_Id && (a.FSS_WaivedAmount > 0 || a.FSS_AdjustedAmount > 0 || a.FSS_RunningExcessAmount > 0))
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMH_Id = a.FMH_Id
                                 }
                       ).ToArray();

                if (delcandel.Length == 0)
                {
                    foreach (var x in lorg3)
                    {
                        var FMA_Id = x.FMA_Id;
                        var FSSST_PaidAmount = x.FTP_Paid_Amt;
                        var lorg4 = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FMA_Id == FMA_Id && t.MI_Id == lorg1.MI_Id && t.ASMAY_Id == lorg1.ASMAY_ID && t.AMST_Id == lorg2.AMST_Id);

                        lorg4.FSS_PaidAmount = (lorg4.FSS_PaidAmount) - Convert.ToInt64(x.FTP_Paid_Amt);


                        if (lorg4.FSS_ExcessPaidAmount > 0)
                        {
                            lorg4.FSS_ExcessPaidAmount = (lorg4.FSS_ExcessPaidAmount) - Convert.ToInt64(x.FTP_Paid_Amt);
                        }
                        if (lorg4.FSS_RunningExcessAmount > 0)
                        {
                            lorg4.FSS_RunningExcessAmount = (lorg4.FSS_RunningExcessAmount) - Convert.ToInt64(x.FTP_Paid_Amt);
                        }

                        //added on 11-07-2018
                        var fineheadss = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                          from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                          where (a.MI_Id == data.MI_Id && a.FMA_Id == FMA_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == lorg2.AMST_Id && a.FMH_Id == b.FMH_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMH_Id = a.FMH_Id,
                                          }
                           ).Distinct().Take(1);

                        if (fineheadss.Count() > 0)
                        {
                            lorg4.FSS_FineAmount = lorg4.FSS_FineAmount - Convert.ToInt64(x.FTP_Paid_Amt);
                        }
                        //added on 11-07-2018


                        if (lorg4.FSS_NetAmount != 0 && lorg4.FSS_OBArrearAmount == 0)
                        {
                            lorg4.FSS_ToBePaid = lorg4.FSS_ToBePaid + (Convert.ToInt64(x.FTP_Paid_Amt));
                        }
                        else if (lorg4.FSS_NetAmount != 0 && lorg4.FSS_OBArrearAmount != 0)
                        {
                            lorg4.FSS_ToBePaid = lorg4.FSS_ToBePaid + (Convert.ToInt64(x.FTP_Paid_Amt));
                        }
                        else if (lorg4.FSS_NetAmount == 0 && lorg4.FSS_OBArrearAmount != 0)
                        {
                            lorg4.FSS_ToBePaid = lorg4.FSS_ToBePaid + (Convert.ToInt64(x.FTP_Paid_Amt));
                        }
                        else
                        {
                            lorg4.FSS_ToBePaid = 0;
                        }

                        _YearlyFeeGroupMappingContext.Update(lorg4);
                    }

                    if (lorg3.Any())
                    {
                        for (int i = 0; lorg3.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(lorg3.ElementAt(i));
                        }
                    }
                    foreach (var x in lorg6)
                    {

                        var FMA_Id = x.FMA_Id;
                        var FSSST_PaidAmount = x.FSREB_RebateAmount;
                        var lorg4 = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FMA_Id == FMA_Id && t.MI_Id == lorg1.MI_Id && t.ASMAY_Id == lorg1.ASMAY_ID && t.AMST_Id == lorg2.AMST_Id);
                        if (lorg4.FSS_RebateAmount != 0)
                        {
                            lorg4.FSS_ToBePaid = lorg4.FSS_ToBePaid + Convert.ToInt64(x.FSREB_RebateAmount);
                        }
                        if (lorg4.FSS_RebateAmount != 0)
                        {
                            lorg4.FSS_RebateAmount = lorg4.FSS_RebateAmount - Convert.ToInt64(x.FSREB_RebateAmount);
                        }

                        _YearlyFeeGroupMappingContext.Update(lorg4);

                    }

                    if (lorg6.Any())
                    {
                        for (int i = 0; lorg6.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(lorg6.ElementAt(i));
                        }

                    }
                    foreach (var c in lorg5)
                    {
                        var checkresult = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool.Single(a => a.FYPPM_Id == c.FYPPM_Id);
                        _YearlyFeeGroupMappingContext.Remove(checkresult);

                    }

                    _YearlyFeeGroupMappingContext.Remove(lorg2);
                    _YearlyFeeGroupMappingContext.Remove(lorg1);


                    var contactexisttransaction = 0;
                    using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                            dbCtxTxn.Commit();
                            data.returnval = "true";
                        }
                        catch (Exception ex)
                        {
                            dbCtxTxn.Rollback();
                            data.returnval = "Transaction is not Processed Correctly.Kindly contact Administrator!!!!!";
                        }
                    }

                }
                else
                {
                    data.returnval = "Transaction is not Processed Correctly.Kindly contact Administrator!!!!!";
                }
            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO Printterm(FeeStudentTransactionDTO data)
        {
            try
            {

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Merge_Fee_Installemnts";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.showstudetails = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Janaseva_Fee_Balance_DueAmount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.GroupwiseBalance = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                          from f in _YearlyFeeGroupMappingContext.feespecialHead
                                          from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMH_FeeName = f.FMSFH_Name,
                                              FMH_Id = e.FMH_Id,
                                              FTP_Paid_Amt = c.FTP_Paid_Amt,
                                              FTP_Concession_Amt = c.FTP_Concession_Amt,
                                              FTP_Fine_Amt = c.FTP_Fine_Amt,
                                              FTI_Id = d.FTI_Id,
                                              totalcharges = d.FMA_Amount
                                          }
              ).ToArray();


                data.getfeeheaddetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                          from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                          from e in _YearlyFeeGroupMappingContext.feeGroup

                                          where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id == data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == d.FMG_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMG_Id = d.FMG_Id,
                                              FMG_CompulsoryFlag = e.FMG_CompulsoryFlag,
                                              headflag = e.FMG_Remarks

                                          }
).Distinct().ToArray();

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMG_Id = d.FMG_Id,
                                     FMH_Id = d.FMH_Id
                                 }).Distinct().ToArray();

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    where c.FMT_Id == d.FMT_Id && a.FMA_Id == b.FMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new FeeStudentTransactionDTO
                                    {
                                        termname = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool
                                           where a.FYP_Id == b.FYP_Id && a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeStudentTransactionDTO
                                           {
                                               FTP_Paid_Amt = b.FYPPM_TotalPaidAmount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = b.FYP_TransactionTypeFlag,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
                                               FYPPM_TotalPaidAmount = b.FYPPM_TotalPaidAmount,
                                               FYP_TransactionTypeFlag = b.FYP_TransactionTypeFlag,
                                               FYPPM_BankName = b.FYPPM_BankName,
                                               FYPPM_DDChequeNo = b.FYPPM_DDChequeNo,
                                               FYPPM_DDChequeDate = b.FYPPM_DDChequeDate,
                                               FYPPM_TransactionId = b.FYPPM_TransactionId,
                                               FYPPM_PaymentReferenceId = b.FYPPM_PaymentReferenceId,
                                               IVRMMOD_ModeOfPayment = b.FYP_TransactionTypeFlag,
                                               IVRMMOD_Flag = b.FYP_TransactionTypeFlag
                                           }
               ).ToArray();

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeReceipt_ASOnDateBalance";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.fillstudentviewdetails = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeTermwiseFeeReceiptDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.Readmissionfeeschecking = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TermnameFeeReceiptDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.fillstudenttype = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TermandHeadFeeReceiptDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.getfeeheaddetails = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_RouteHousewise_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.Amst_Id });
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
                            data.fillacclst = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }




                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                         where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FYP_Tot_Amount = a.FYP_Tot_Amount,
                                             FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                             FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                         }
              ).Distinct().ToArray();


                data.pendingamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FSS_ToBePaid = a.FSS_ToBePaid
                                      }
   ).Sum(t => t.FSS_ToBePaid);
                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               where (e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                               select new FeeStudentTransactionDTO
                               {
                                   fmt_id = e.FMT_Id,
                                   fmt_order = g.FMT_Order,
                                   FMT_Year = g.FMT_Year
                               }

                 ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                int fmtorder_end = 0;
                string fmt_id_int = "0", fmt_id_end = "0", fmt_id_end_year = "0";
                fmt_id_int = feeterm[0].fmt_id.ToString();
                fmt_id_end = feeterm[feeterm.Length - 1].fmt_id.ToString();

                fmtorder_end = Convert.ToInt32(feeterm[feeterm.Length - 1].fmt_order);
                fmt_id_end_year = (feeterm[feeterm.Length - 1].FMT_Year).ToString();

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmtorder_end + 1).Select(t => t.FMT_Id);

                List<long> termmidsnew = new List<long>();
                foreach (var item in feeterm)
                {
                    termmidsnew.Add(item.fmt_id);
                }

                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (b.FYP_Id == c.FYP_Id && a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id == data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeStudentTransactionDTO
                                   {
                                       FMG_Id = d.FMG_Id,
                                       FMH_Id = d.FMH_Id,
                                       FTI_Id = d.FTI_Id
                                   }

                           ).Distinct().ToList();

                List<FeeStudentTransactionDTO> temp_group_headnew = new List<FeeStudentTransactionDTO>();
                temp_group_headnew = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                

                                   where ( a.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FSS_ToBePaid>0)
                                   select new FeeStudentTransactionDTO
                                   {
                                       FMG_Id = a.FMG_Id,
                          
                                   }

           ).Distinct().ToList();

                List<long> grp_idsnew = new List<long>();
                List<long> grp_ids = new List<long>();
                List<long> head_ids = new List<long>();
                List<long> inst_ids = new List<long>();
                foreach (var item in temp_group_head)
                {
                    grp_ids.Add(item.FMG_Id);
                    head_ids.Add(item.FMH_Id);
                    inst_ids.Add(item.FTI_Id);
                }
                foreach (var item in temp_group_headnew)
                {
                    grp_idsnew.Add(item.FMG_Id);
                }

                    List<FeeStudentTransactionDTO> fordate = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> fordateinfyp = new List<FeeStudentTransactionDTO>();

                var nextduedate = "0";

                if (term_ids.Count() > 0)
                {
                    nextduedate = (Convert.ToInt32(fmt_id_end) + 1).ToString();

                    fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                               where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                               select new FeeStudentTransactionDTO
                               {
                                   date = f.FTDD_Day,
                                   month = f.FTDD_Month,
                               }
                        ).Distinct().Take(1).ToList();

                }
                else
                {
                    nextduedate = (Convert.ToInt32(fmt_id_end)).ToString();

                    fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                               where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                               select new FeeStudentTransactionDTO
                               {
                                   date = f.FTDD_Day,
                                   month = f.FTDD_Month,
                               }
                        ).Distinct().Take(1).ToList();
                }

                termmidsnew.Add(Convert.ToInt32(nextduedate));

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
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
                    if ( Convert.ToInt32(item) < 3)
                    {
                        months1.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year+1;
                        data.year = nextyr.ToString();
                    }
                    else
                    {
                        months2.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        data.year = curyear.Year.ToString();
                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();
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

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }
                else if (months2.Count() > 0)
                {
                    data.month = months2.Min().ToString();
                    maxmonth = months2.Max().ToString();

                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;

                }
                for (int i = 0; i < months.Count(); i++)
                {
                    if (Convert.ToInt32(data.month) == months[i])
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
                            data.year = curyear.Year.ToString();
                        }
                        else
                        {
                            maxmonth = startperiod.Max().ToString();
                            var curyear = DateTime.Now;
                            data.year = curyear.Year.ToString();
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

                var compusoryflag = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(d => d.MI_Id == data.MI_Id && grp_ids.Contains(d.FMG_Id)).Select(t => t.FMG_CompulsoryFlag).Distinct().ToArray();

                if (compusoryflag[0].ToString() == "T")
                {
                    var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                    var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                    monthnameinitial = termperiodlistint[0].Transport_FromMonth.ToString();
                    monthnameend = termperiodlistend[0].Transport_ToMonth.ToString();

                    string yeardisplay = "0";
                    yeardisplay = fmt_id_end_year;

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
                }
                else
                {
                    var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                    var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                    monthnameinitial = termperiodlistint[0].FromMonth.ToString();
                    monthnameend = termperiodlistend[0].ToMonth.ToString();

                    string yeardisplay = "0";
                    yeardisplay = fmt_id_end_year;

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
                }

                //new one
                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_idsnew.Contains(a.FMG_Id) && termmidsnew.Contains(b.FMT_Id) && c.FMH_Flag != "E")
                                  select new FeeStudentTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FSS_ToBePaid
                                  }
          ).Sum(t => t.FSS_ToBePaid);

                data.streamdetails = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                      from b in _YearlyFeeGroupMappingContext.Adm_School_Master_Stream
                                      where (a.ASMST_Id == b.ASMST_Id && a.AMST_Id == data.Amst_Id && a.AMST_ActiveFlag == 1 && b.ASMST_ActiveFlag == true)
                                      select new FeeStudentTransactionDTO
                                      {
                                          ASMST_StreamName = b.ASMST_StreamName

                                      }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO Printinstallment(FeeStudentTransactionDTO data)
        {
            try
            {
                string yeardisplay = "0";

                data.srkvsdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                     from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                     where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.FMA_Id == c.FMA_Id && a.ASMAY_ID == c.ASMAY_Id && c.AMST_Id == data.Amst_Id
                                     select new FeeStudentTransactionDTO
                                     {
                                         FTP_Paid_Amt = b.FTP_Paid_Amt,
                                         FTP_Concession_Amt = Convert.ToDecimal(c.FSS_ConcessionAmount),
                                         FSS_ToBePaid = c.FSS_ToBePaid,
                                         FSS_TotalToBePaid = c.FSS_TotalToBePaid,
                                         FSS_CurrentYrCharges = c.FSS_CurrentYrCharges
                                     }
          ).ToArray();

                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                          from f in _YearlyFeeGroupMappingContext.feespecialHead
                                          from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          from h in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id && h.MI_Id == f.MI_Id && h.FMH_Id == g.FMH_Id && h.FMA_Id == c.FMA_Id && h.FTI_Id == d.FTI_Id && h.ASMAY_Id == data.ASMAY_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMH_FeeName = f.FMSFH_Name,
                                              FMH_Id = e.FMH_Id,
                                              FTP_Paid_Amt = c.FTP_Paid_Amt,
                                              FTP_Concession_Amt = c.FTP_Concession_Amt,
                                              FTP_Fine_Amt = c.FTP_Fine_Amt,
                                              FTI_Id = d.FTI_Id,
                                              // totalcharges = d.FMA_Amount,
                                              FSS_AdjustedAmount = h.FSS_AdjustedAmount,
                                              FSS_RefundAmount = h.FSS_RefundAmount
                                          }
             ).Distinct().ToArray();

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMG_Id = d.FMG_Id,
                                     FMH_Id = d.FMH_Id
                                 }).Distinct().ToArray();

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    where c.FMT_Id == d.FMT_Id && a.FMA_Id == b.FMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new FeeStudentTransactionDTO
                                    {
                                        termname = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool
                                               //from c in _YearlyFeeGroupMappingContext.IVRM_ModeOfPayment
                                           where a.FYP_Id == b.FYP_Id && a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeStudentTransactionDTO
                                           {
                                               FTP_Paid_Amt = b.FYPPM_TotalPaidAmount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,

                                               FYPPM_TotalPaidAmount = b.FYPPM_TotalPaidAmount,
                                               FYP_TransactionTypeFlag = b.FYP_TransactionTypeFlag,
                                               FYPPM_BankName = b.FYPPM_BankName,
                                               FYPPM_DDChequeNo = b.FYPPM_DDChequeNo,
                                               FYPPM_DDChequeDate = b.FYPPM_DDChequeDate,
                                               FYPPM_TransactionId = b.FYPPM_TransactionId,
                                               FYPPM_PaymentReferenceId = b.FYPPM_PaymentReferenceId,
                                               IVRMMOD_ModeOfPayment = b.FYP_TransactionTypeFlag,
                                               IVRMMOD_Flag = b.FYP_TransactionTypeFlag
                                           }
                ).ToArray();

                List<FeeStudentTransactionDTO> result = new List<FeeStudentTransactionDTO>();

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "installment_transaction_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.fillstudentviewdetails = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                         where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FYP_Tot_Amount = a.FYP_Tot_Amount,
                                             FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                             FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                         }
              ).Distinct().ToArray();



                data.pendingamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FSS_ToBePaid = a.FSS_ToBePaid
                                      }
).Sum(t => t.FSS_ToBePaid);

                //to find next due amount

                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               where (e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
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

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmtorder_end + 1).Select(t => t.FMT_Id);

                List<long> termmidsnew = new List<long>();
                foreach (var item in feeterm)
                {
                    termmidsnew.Add(item.fmt_id);
                }

                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
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

                List<FeeStudentTransactionDTO> fordate = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> fordateinfyp = new List<FeeStudentTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                           from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                           where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && d.User_Id == data.userid)
                           select new FeeStudentTransactionDTO
                           {
                               date = f.FTDD_Day,
                               month = f.FTDD_Month,
                           }

                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
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
                        var nextyr = curyear.Year - 1;
                        data.year = nextyr.ToString();
                    }
                    else
                    {
                        months2.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        data.year = curyear.Year.ToString();
                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();
                    maxmonth = (Convert.ToInt32(data.month) - 1).ToString();

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

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }
                else if (months2.Count() > 0)
                {
                    data.month = months2.Min().ToString();
                    maxmonth = months2.Max().ToString();

                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                    if (monthnameend == "Jan" || monthnameend == "Feb" || monthnameend == "Mar")
                    {
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year;
                        yeardisplay = nextyr.ToString();
                    }
                    else
                    {
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year - 1;
                        yeardisplay = nextyr.ToString();
                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;

                }
                for (int i = 0; i < months.Count(); i++)
                {
                    if (Convert.ToInt32(data.month) == months[i])
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
                            months1.Add(Convert.ToInt32(item));
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year + 1;
                            data.year = nextyr.ToString();
                        }
                        else
                        {
                            months2.Add(Convert.ToInt32(item));
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year - 1;
                            data.year = curyear.ToString();
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

                    if (monthnameend == "Jan" || monthnameend == "Feb" || monthnameend == "Mar")
                    {
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year;
                        yeardisplay = nextyr.ToString();
                    }
                    else
                    {
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year - 1;
                        yeardisplay = nextyr.ToString();


                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
                }


                var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                monthnameinitial = termperiodlistint[0].FromMonth.ToString();
                monthnameend = termperiodlistend[0].ToMonth.ToString();

                if (monthnameend == "Jan" || monthnameend == "Feb" || monthnameend == "Mar")
                {
                    var curyear = DateTime.Now;
                    var nextyr = curyear.Year;
                    yeardisplay = nextyr.ToString();
                }
                else
                {
                    var curyear = DateTime.Now;
                    var nextyr = curyear.Year - 1;
                    yeardisplay = nextyr.ToString();
                }

                yeardisplay = fmt_id_end_year;

                data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;

                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && termmidsnew.Contains(b.FMT_Id) && c.FMH_Flag != "E" && a.User_Id == data.userid)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FSS_ToBePaid
                                  }
        ).Sum(t => t.FSS_ToBePaid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<FeeStudentTransactionDTO> printreceipt([FromBody] FeeStudentTransactionDTO data)
        {
            try
            {

                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id == data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
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

                var asmay_id = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(a => a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id).ToList().FirstOrDefault().ASMAY_ID;

                data.ASMAY_Id = asmay_id;


                var portalusername = (from a in _YearlyFeeGroupMappingContext.applicationUser
                                      from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                      where (a.Id == b.user_id && b.FYP_Id == data.FYP_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          portalusername = a.UserName
                                      }).ToList();

                data.portalusername = portalusername[0].portalusername;


                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                string accountname = "", accesskey = "", html = "";
                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }

                try
                {
                    var checktemplate = (from a in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceiptDMO
                                         from b in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                         where (a.FGAR_Id == b.FGAR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && grp_ids.Contains(b.FMG_Id))
                                         select new FeeStudentTransactionDTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                             receipttemplate = a.FGAR_Template_Name,
                                         }
             ).Distinct().ToArray();

                    if (checktemplate.Count() > 0)
                    {
                        if (checktemplate[0].receipttemplate != "")
                        {
                            html = checktemplate[0].receipttemplate;
                        }
                        else
                        {
                            html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "FeeReceipt.html", 0);
                        }
                    }
                }
                catch (Exception ex)
                { html = ""; }

                data.htmldata = html;

                if (html != "")
                {
                    if (data.minstall == "0")
                    {
                        Printterm(data);
                    }
                    else
                    {
                        Printinstallment(data);
                    }
                }
                else { printcommon(data); }

                List<Institution> masins = new List<Institution>();
                masins = _context.Institute.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.masterinstitution = masins.ToArray();

                var periodnme = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                 where (a.FMG_Id == c.FMG_Id && a.FMG_Id == d.FMG_Id && b.FMA_Id == c.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id == data.FYP_Id && a.AMST_Id == data.Amst_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMG_CompulsoryFlag = d.FMG_CompulsoryFlag,
                                 }
               ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               where (e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                               select new FeeStudentTransactionDTO
                               {
                                   fmt_id = e.FMT_Id,
                                   fmt_order = g.FMT_Order,
                                   FMT_Year = g.FMT_Year
                               }

                ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                string startmonth = "";
                string endmonth = "";
                string year1 = "";
                string year2 = "";
                string fmt_id_int = "0", fmt_id_end = "0";

                for (int i = 0; i < feeterm.Length; i++)
                {
                    fmt_id_int = feeterm[0].fmt_id.ToString();
                    fmt_id_end = feeterm[feeterm.Length - 1].fmt_id.ToString();
                }


                startmonth = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_int) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_FROM_MONTH;

                endmonth = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_end) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_TO_MONTH;

                year1 = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_int) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_Year;

                year2 = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_end) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_Year;

                data.duration = startmonth + '-' + year1 + '/' + endmonth + '-' + year2;


                var feeterm1 = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from e in _YearlyFeeGroupMappingContext.feeMTH
                                from f in _YearlyFeeGroupMappingContext.feeTr
                                where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && e.FMT_Id == f.FMT_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id)
                                select new FeeStudentTransactionDTO
                                {
                                    fmt_id = e.FMT_Id,
                                    fmt_order = f.FMT_Order
                                }

               ).Distinct().OrderByDescending(t => t.fmt_order).ToArray();

                long fmt_id_new = 0;
                long initialfmtids = Convert.ToInt64(feeterm1[0].fmt_order) + 1;

                var actfmtid = _YearlyFeeGroupMappingContext.feeTr.Where(a => a.MI_Id == data.MI_Id && a.FMT_Order == initialfmtids).ToList().FirstOrDefault().FMT_Id;

                fmt_id_new = Convert.ToInt64(actfmtid);

                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && c.FMH_Flag != "E" && a.User_Id == data.userid && b.FMT_Id == fmt_id_new)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FSS_ToBePaid
                                  }
        ).Sum(t => t.FSS_ToBePaid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public FeeStudentTransactionDTO duplicaterecept(FeeStudentTransactionDTO data)
        {
            try
            {
                data.duplicatereceipt = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Receipt_No.Equals(data.FYP_Receipt_No.Trim()))
                                         select new FeeStudentTransactionDTO
                                         {
                                             FYP_Receipt_No = a.FYP_Receipt_No
                                         }
              ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO get_grp_reptno(FeeStudentTransactionDTO data)
        {
            try
            {
                if (data.auto_receipt_flag == 1)
                {
                    List<long> HeadId = new List<long>();
                    List<long> grpid = new List<long>();
                    foreach (var item in data.temp_head_list)
                    {
                        HeadId.Add(item.FMH_Id);
                        grpid.Add(item.FMG_Id);
                    }



                    //string groupidss = "0";
                    //for (int r = 0; r < grpid.Count(); r++)
                    //{
                    //    groupidss = groupidss + ',' + grpid[r];
                    //}

                    string groupidss = "0";
                    string distgroupidss = "0";
                    //foreach (var item in grps)
                    //{
                    //    grpid.Add(item.FMG_Id);
                    //}

                    for (int r = 0; r < grpid.Count(); r++)
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }

                    List<long> distgroup = grpid.Distinct().ToList();
                    for (int y = 0; y < distgroup.Count(); y++)
                    {
                        distgroupidss = distgroupidss + ',' + distgroup[y];
                    }


                    var final_rept_no = "";
                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

                    list_all = (from b in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                }
                         ).Distinct().ToList();

                    data.grp_count = list_all.Count();

                    if (data.grp_count == 1)
                    {

                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "receiptnogeneration";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mi_id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmgid",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = distgroupidss
                            });

                            cmd.Parameters.Add(new SqlParameter("@receiptno",
                SqlDbType.NVarChar, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                        }
                    }
                }

                else if (data.automanualreceiptno == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO getsearchfilter(FeeStudentTransactionDTO data)
        {
            try
             {

                data.searchfilter = data.searchfilter.ToUpper();
                if (data.filterinitialdata.Equals("regular"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id != null && data.ASMS_Id>0)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.School_M_Class
                                            from d in _YearlyFeeGroupMappingContext.school_M_Section
                                            where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_ID && d.ASMS_Id==data.ASMS_Id
                                            && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + c.ASMCL_ClassName + "-" + d.ASMC_SectionName + "-" + a.AMST_AdmNo,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                      ).OrderByDescending(g => (g.AMST_FirstName.Trim().ToUpper() + ' ' + g.AMST_MiddleName.Trim().ToUpper() + ' ' + g.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter)).ToArray();


                    }
                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && (data.ASMS_Id==null || data.ASMS_Id==0))
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.School_M_Class
                                            from d in _YearlyFeeGroupMappingContext.school_M_Section
                                            where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_ID
                                            && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + c.ASMCL_ClassName + "-" + d.ASMC_SectionName + "-" + a.AMST_AdmNo,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                      ).OrderByDescending(g => (g.AMST_FirstName.Trim().ToUpper() + ' ' + g.AMST_MiddleName.Trim().ToUpper() + ' ' + g.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter)).ToArray();


                    }
                    else
                    {

                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.School_M_Class
                                            from d in _YearlyFeeGroupMappingContext.school_M_Section
                                            where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1
                                            && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + c.ASMCL_ClassName + "-" + d.ASMC_SectionName + "-" + a.AMST_AdmNo,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                    ).OrderByDescending(g => (g.AMST_FirstName.Trim().ToUpper() + ' ' + g.AMST_MiddleName.Trim().ToUpper() + ' ' + g.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter)).ToArray();
                    }
                    }

                else if (data.filterinitialdata.Equals("inactive"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id>0 && data.ASMS_Id!=null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "D" && a.AMST_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_ID && b.AMAY_ActiveFlag == 1 && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)) && b.ASMS_Id==data.ASMS_Id)
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
               ).ToArray();
                    }

                   else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && (data.ASMS_Id==null || data.ASMS_Id==0))
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "D" && a.AMST_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_ID && b.AMAY_ActiveFlag == 1 && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
               ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "D" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                        ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("left"))
                {

                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id != null && data.ASMS_Id>0)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_ID && (a.AMST_SOL == "L" || a.AMST_SOL == "T") && a.AMST_FirstName.ToUpper().Contains(data.searchfilter) && a.AMST_ActiveFlag == 0 && b.ASMS_Id==data.ASMS_Id)
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                    ).ToArray();
                    }
                   else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id == null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_ID && (a.AMST_SOL == "L" || a.AMST_SOL == "T") && a.AMST_FirstName.ToUpper().Contains(data.searchfilter) && a.AMST_ActiveFlag == 0)
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                    ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && (a.AMST_SOL == "L" || a.AMST_SOL == "T") && a.AMST_FirstName.ToUpper().Contains(data.searchfilter) && a.AMST_ActiveFlag == 0)
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                    ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("regno"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id>0 && data.ASMS_Id!=null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_RegistrationNo.StartsWith(data.searchfilter) && b.ASMS_Id==data.ASMS_Id)
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_RegistrationNo,
                                                AMST_MiddleName = "",
                                                AMST_LastName = ""

                                            }
                  ).ToArray();
                    }
                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id == null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_RegistrationNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_RegistrationNo,
                                                AMST_MiddleName = "",
                                                AMST_LastName = ""

                                            }
                  ).ToArray();
                    }
                    else
                    {

                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_RegistrationNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_RegistrationNo,
                                                AMST_MiddleName = "",
                                                AMST_LastName = ""

                                            }
                    ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("AdmNo"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id>0 && data.ASMS_Id!=null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_AdmNo.StartsWith(data.searchfilter) && b.ASMS_Id==data.ASMS_Id)
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_AdmNo,
                                                AMST_MiddleName = "",
                                                AMST_LastName = ""

                                            }
                 ).ToArray();
                    }
                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && (data.ASMS_Id==null || data.ASMS_Id==0))
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_AdmNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_AdmNo,
                                                AMST_MiddleName = "",
                                                AMST_LastName = ""

                                            }
                 ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_AdmNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_AdmNo,
                                                AMST_MiddleName = "",
                                                AMST_LastName = ""

                                            }
                    ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("Admnoname"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id>0 && data.ASMS_Id!=null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_AdmNo.StartsWith(data.searchfilter) && b.ASMS_Id==data.ASMS_Id)
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_AdmNo + "-" + ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName
                                            }
                  ).ToArray();
                    }
                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id!=null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_AdmNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_AdmNo + "-" + ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName
                                            }
                  ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_AdmNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_AdmNo + "-" + ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName
                                            }
                    ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("NameAdmno"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id>0 && data.ASMS_Id!=null)
                    {

                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && b.ASMS_Id==data.ASMS_Id && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + a.AMST_AdmNo,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName + "-" + a.AMST_AdmNo,
                                            }
                        ).ToArray();
                    }
                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && (data.ASMS_Id==null || data.ASMS_Id==0))
                    {

                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + a.AMST_AdmNo,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName + "-" + a.AMST_AdmNo,
                                            }
                        ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 0)) && (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + a.AMST_AdmNo,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName + "-" + a.AMST_AdmNo,
                                            }
                    ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("NameRegNo"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + a.AMST_RegistrationNo,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                            }
                        ).ToArray();
                    }
                    else
                    {

                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + a.AMST_RegistrationNo,
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                            }
                    ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("RegNoName"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id>0 && data.ASMS_Id!=null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && b.ASMS_Id==data.ASMS_Id && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_RegistrationNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_RegistrationNo + "-" + ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                        ).ToArray();
                    }
                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && (data.ASMS_Id==null || data.ASMS_Id==0))
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_Order && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_RegistrationNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_RegistrationNo + "-" + ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                        ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1))  && a.AMST_RegistrationNo.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = a.AMST_RegistrationNo + "-" + ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = a.AMST_MiddleName,
                                                AMST_LastName = a.AMST_LastName,
                                            }
                    ).ToArray();
                    }
                }
                else if (data.filterinitialdata.Equals("Preadmission"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.stuapp
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.pasr_id,
                                            AMST_FirstName = a.PASR_FirstName + ' ' + a.PASR_MiddleName + ' ' + a.PASR_LastName,
                                            AMST_MiddleName = a.PASR_MiddleName,
                                            AMST_LastName = a.PASR_LastName,
                                        }
                    ).ToArray();
                }

                else if (data.filterinitialdata.Equals("Mothername"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id > 0 && data.ASMS_Id != null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && b.ASMS_Id == data.ASMS_Id && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_MotherName.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_MotherName == null || a.AMST_MotherName == "0" ? "" : a.AMST_MotherName) +
                                           
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName ="",
                                                AMST_LastName = "",
                                            }
               ).ToArray();
                    }

                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && (data.ASMS_Id == null || data.ASMS_Id == 0))
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_Order && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_MotherName.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_MotherName == null || a.AMST_MotherName == "0" ? "" : a.AMST_MotherName) +

                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName ="",
                                                AMST_LastName ="",
                                            }
               ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_MotherName.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_MotherName == null || a.AMST_MotherName == "0" ? "" : a.AMST_MotherName) +

                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = "",
                                                AMST_LastName = a.AMST_LastName,
                                            }
                        ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("Fathername"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id > 0 && data.ASMS_Id != null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && b.ASMS_Id == data.ASMS_Id && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 0 && a.AMST_ActiveFlag == 0)) && a.AMST_FatherName.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FatherName == null || a.AMST_FatherName == "0" ? "" : a.AMST_FatherName) +

                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = "",
                                                AMST_LastName = "",
                                            }
               ).ToArray();
                    }

                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && (data.ASMS_Id == null || data.ASMS_Id == 0))
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_Order && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_FatherName.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FatherName == null || a.AMST_FatherName == "0" ? "" : a.AMST_FatherName) +

                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = "",
                                                AMST_LastName = "",
                                            }
               ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 0)) && a.AMST_FatherName.StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FatherName == null || a.AMST_FatherName == "0" ? "" : a.AMST_FatherName) +

                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_MiddleName = "",
                                                AMST_LastName = a.AMST_LastName,
                                            }
                        ).ToArray();
                    }
                }

                else if (data.filterinitialdata.Equals("MobileNo"))
                {
                    if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && data.ASMS_Id > 0 && data.ASMS_Id != null)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_ID && b.ASMS_Id == data.ASMS_Id && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_MobileNo.ToString().StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = (a.AMST_MobileNo.ToString() == null || a.AMST_MobileNo.ToString() == "0" ? "" : a.AMST_MobileNo.ToString()).Trim(),

                                                AMST_MiddleName = "",
                                                AMST_LastName = "",
                                            }
               ).ToArray();
                    }

                    else if (data.ASMCL_ID != null && data.ASMCL_ID > 0 && (data.ASMS_Id == null || data.ASMS_Id == 0))
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.ASMCL_Id == data.ASMCL_Order && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1)) && a.AMST_MobileNo.ToString().StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = (a.AMST_MobileNo.ToString() == null || a.AMST_MobileNo.ToString() == "0" ? "" : a.AMST_MobileNo.ToString()).Trim(),
                                                AMST_MiddleName = "",
                                                AMST_LastName = "",
                                            }
               ).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((data.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || ((data.AMST_SOL == "L" || data.AMST_SOL == "D") && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 0)) && a.AMST_MobileNo.ToString().StartsWith(data.searchfilter))
                                            select new FeeStudentTransactionDTO
                                            {
                                                Amst_Id = a.AMST_Id,
                                                AMST_FirstName = (a.AMST_MobileNo.ToString() == null || a.AMST_MobileNo.ToString() == "0" ? "" : a.AMST_MobileNo.ToString()).Trim(),
                                                AMST_MiddleName = "",
                                                AMST_LastName = "",
                                            }
                        ).ToArray();
                    }
                }

            }
            catch (Exception e)
            {
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }
        public long getpreviousyear(FeeStudentTransactionDTO data)
        {
            try
            {
                var newASMAY_Id = (from a in _YearlyFeeGroupMappingContext.AcademicYear
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeStudentTransactionDTO
                                   {
                                       ASMAY_Order = a.ASMAY_Order,
                                   }
                             ).FirstOrDefault();
                var asmayorder = Convert.ToInt64(newASMAY_Id.ASMAY_Order) - 1;

                var nextASMAY_Id = (from a in _YearlyFeeGroupMappingContext.AcademicYear
                                    where (a.MI_Id == data.MI_Id && a.ASMAY_Order == asmayorder)
                                    select new FeeStudentTransactionDTO
                                    {
                                        ASMAY_Id = a.ASMAY_Id,
                                    }
                             ).FirstOrDefault();

                data.PreASMAY_Id = nextASMAY_Id.ASMAY_Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.ASMAY_Id;
        }
        public FeeStudentTransactionDTO searching(FeeStudentTransactionDTO data)
        {
            try
            {
                int userprev = 0;
                var getcurrsett = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                foreach (var value in getcurrsett)
                {
                    userprev = value.FMC_USER_PREVILEDGE;
                }

                if (userprev == 1)
                {
                    switch (data.searchType)
                    {

                        case "0":
                            string str = "";
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && d.ASMS_Id == f.ASMS_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull") && (((c.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.ToUpper().Trim()) == true ? str : c.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.ToUpper().Trim()) == true ? str : c.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_LastName.ToUpper().StartsWith(data.searchtext)))/* && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1*/
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
                  ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                            break;
                        case "1":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && e.ASMCL_ClassName.Equals(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
                  ).Distinct().OrderBy(t => t.classname).ToArray();
                            break;
                        case "2":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
                  ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
                            break;
                        case "3":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Receipt_No.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
                  ).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                            break;
                        case "4":
                            var date_format = data.searchdate.ToString("dd/MM/yyyy");

                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
               ).Distinct().ToArray();

                            break;
                        case "5":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
                  ).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
                            break;
                        case "6":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Bank_Or_Cash.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
                  ).Distinct().OrderBy(t => t.FYP_Bank_Or_Cash).ToArray();

                            break;
                        case "7":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && f.ASMC_SectionName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.user_id == data.userid && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
                  ).Distinct().OrderBy(t => t.sectionname).ToArray();
                            break;
                    }
                }
                else if (userprev == 0)
                {
                    switch (data.searchType)
                    {

                        case "0":
                            string str = "";
                            data.searchtext = data.searchtext.ToUpper();
                            var getdet = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                          from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && (((a.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(a.AMST_MiddleName.ToUpper().Trim()) == true ? str : a.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(a.AMST_LastName.ToUpper().Trim()) == true ? str : a.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || a.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || a.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || a.AMST_LastName.ToUpper().StartsWith(data.searchtext)))
                                          select new FeeStudentTransactionDTO
                                          {
                                              Amst_Id = a.AMST_Id,
                                          }
                                 ).ToList();

                            if (getdet.Count == 0)
                            {
                                getpreviousyear(data);
                                var previousasmay_id = data.PreASMAY_Id;

                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && d.ASMS_Id == f.ASMS_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == previousasmay_id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull") && (((c.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.ToUpper().Trim()) == true ? str : c.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.ToUpper().Trim()) == true ? str : c.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_LastName.ToUpper().StartsWith(data.searchtext)))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                  ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                            }
                            else
                            {
                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && d.ASMS_Id == f.ASMS_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull") && (((c.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.ToUpper().Trim()) == true ? str : c.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.ToUpper().Trim()) == true ? str : c.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_LastName.ToUpper().StartsWith(data.searchtext)))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                  ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                            }
                            break;
                        case "1":

                            var getdet1 = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                           from c in _YearlyFeeGroupMappingContext.School_M_Class
                                           from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                           where (a.AMST_Id == b.AMST_Id && c.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && c.ASMCL_ClassName.Equals(data.searchtext) && b.ASMAY_Id == data.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                           }
                                 ).ToList();

                            if (getdet1.Count == 0)
                            {
                                getpreviousyear(data);
                                var previousasmay_id = data.PreASMAY_Id;

                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && e.ASMCL_ClassName.Equals(data.searchtext) && d.ASMAY_Id == previousasmay_id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id
                                                    }
                  ).Distinct().OrderBy(t => t.classname).ToArray();
                            }
                            else
                            {
                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && e.ASMCL_ClassName.Equals(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                  ).Distinct().OrderBy(t => t.classname).ToArray();
                            }

                            break;
                        case "2":

                            var getdet2 = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                           from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                           where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_AdmNo.Contains(data.searchtext) && b.ASMAY_Id == data.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                           }
                                 ).ToList();

                            if (getdet2.Count == 0)
                            {
                                getpreviousyear(data);
                                var previousasmay_id = data.PreASMAY_Id;

                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == previousasmay_id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                  ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
                            }
                            else
                            {
                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                      ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
                            }
                            break;
                        case "3":

                            var getdet3 = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                           from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                           from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                           from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.FYP_Id == d.FYP_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && d.FYP_OnlineChallanStatusFlag.Equals("Sucessfull") && d.FYP_Receipt_No.Contains(data.searchtext))
                                           select new FeeStudentTransactionDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                           }
                                ).ToList();

                            if (getdet3.Count == 0)
                            {
                                getpreviousyear(data);
                                var previousasmay_id = data.PreASMAY_Id;

                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Receipt_No.Contains(data.searchtext) && d.ASMAY_Id == previousasmay_id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                  ).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                            }
                            else
                            {
                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Receipt_No.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                  ).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                            }

                            break;
                        case "4":
                            var date_format = data.searchdate.ToString("dd/MM/yyyy");

                            var getdet4 = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                           from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                           from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                           from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.FYP_Id == d.FYP_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && d.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && d.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                           select new FeeStudentTransactionDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                           }
                               ).ToList();

                            if (getdet4.Count == 0)
                            {

                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
              ).Distinct().ToArray();

                            }

                            break;
                        case "5":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
              ).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
                            break;
                        case "6":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Bank_Or_Cash.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    Amst_Id = c.AMST_Id,
                                                    AMST_FirstName = c.AMST_FirstName,
                                                    AMST_MiddleName = c.AMST_MiddleName,
                                                    AMST_LastName = c.AMST_LastName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    classname = e.ASMCL_ClassName,
                                                    sectionname = f.ASMC_SectionName,
                                                    FYP_Id = a.FYP_Id,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    FYP_Date = a.FYP_Date,
                                                    userid = a.user_id,
                                                    FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                }
              ).Distinct().OrderBy(t => t.FYP_Bank_Or_Cash).ToArray();
                            break;
                        case "7":

                            var getdet7 = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                           from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                           where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_AdmNo.Contains(data.searchtext) && b.ASMAY_Id == data.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                           }
                             ).ToList();

                            if (getdet7.Count == 0)
                            {
                                getpreviousyear(data);
                                var previousasmay_id = data.PreASMAY_Id;

                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && f.ASMC_SectionName.Contains(data.searchtext) && d.ASMAY_Id == previousasmay_id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                  ).Distinct().OrderBy(t => t.sectionname).ToArray();
                            }
                            else
                            {
                                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                                    from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && f.ASMC_SectionName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        Amst_Id = c.AMST_Id,
                                                        AMST_FirstName = c.AMST_FirstName,
                                                        AMST_MiddleName = c.AMST_MiddleName,
                                                        AMST_LastName = c.AMST_LastName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        classname = e.ASMCL_ClassName,
                                                        sectionname = f.ASMC_SectionName,
                                                        FYP_Id = a.FYP_Id,
                                                        AMST_AdmNo = c.AMST_AdmNo,
                                                        FYP_Date = a.FYP_Date,
                                                        userid = a.user_id,
                                                        FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                                    }
                  ).Distinct().OrderBy(t => t.sectionname).ToArray();
                            }

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
        public FeeStudentTransactionDTO edittra(FeeStudentTransactionDTO data)
        {
            try
            {

                data.Bankname = (from a in _YearlyFeeGroupMappingContext.Fee_Master_BankDMO
                                 where (a.MI_Id == data.MI_Id && a.FMBANK_ActiveFlg == true)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMBANK_Id = a.FMBANK_Id,
                                     FMBANK_BankName = a.FMBANK_BankName,
                                 }
   ).Distinct().ToArray();

                data.disableterms = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                     from b in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                     from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                     from d in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                     where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && b.AMST_Id == c.AMST_Id && d.FYP_Id == c.FYP_Id && d.FMA_Id == b.FMA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && d.FYP_Id == data.FYP_Id)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMG_Id = a.FMT_Id,
                                     }
                                        ).Distinct().ToArray();


                var termlistforspecial = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                          from b in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                          from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from d in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && b.AMST_Id == c.AMST_Id && d.FYP_Id == c.FYP_Id && d.FMA_Id == b.FMA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && d.FYP_Id == data.FYP_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMG_Id = a.FMT_Id,
                                          }
                                   ).Distinct().ToList();

                List<long> terms_groups = new List<long>();
                for (int j = 0; j < termlistforspecial.Count(); j++)
                {
                    terms_groups.Add(termlistforspecial[j].FMG_Id);
                }

                data.terms_groups = terms_groups.ToArray();

                var saved_fma = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 where (a.FMA_Id == b.FMA_Id && b.FYP_Id == c.FYP_Id && a.AMST_Id == c.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                 select b.FMA_Id
).Distinct().ToList();

                var fetchclass = (from a in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                  where (a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                  select new FeeStudentTransactionDTO
                                  {
                                      ASMCL_ID = a.ASMCL_Id,
                                      ASMAY_Id = a.ASMAY_Id
                                  }
).Distinct().ToArray();

                string classid = "0", academicyearid = "0";
                for (int s = 0; s < fetchclass.Count(); s++)
                {
                    classid = fetchclass[s].ASMCL_ID.ToString();
                    academicyearid = fetchclass[s].ASMAY_Id.ToString();
                }

                data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                      from d in _YearlyFeeGroupMappingContext.feeMIY
                                      from b in _YearlyFeeGroupMappingContext.feeMTH
                                      from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                      from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                      from f in _YearlyFeeGroupMappingContext.feeYCCC
                                      from g in _YearlyFeeGroupMappingContext.feeYCC
                                      from h in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                      where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && h.AMST_Id == data.Amst_Id && c.FMA_Id == h.FMA_Id)
                                      select new Head_Installments_DTO
                                      {
                                          FTI_Name = d.FTI_Name,
                                          FTI_Id = c.FTI_Id
                                      }).Distinct().ToList().ToArray();



                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeTr
                                        where (a.FMT_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                        select new FeeStudentTransactionDTO
                                        {
                                            FMG_Id = a.FMT_Id,
                                            FMG_GroupName = a.FMT_Name,
                                        }
         ).OrderBy(t => t.FMH_Id).ToArray();

                data.receiparraydeleteall = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                             from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                             from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             where (d.FMG_Id == e.FMG_Id && a.FMA_Id == d.FMA_Id && b.FMH_Id == d.FMH_Id && c.FTI_Id == d.FTI_Id && a.FYP_Id == data.FYP_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.AMST_Id == data.Amst_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FMG_GroupName = e.FMG_GroupName,
                                                 FMH_FeeName = b.FMH_FeeName,
                                                 FTI_Name = c.FTI_Name,
                                                 FMH_Id = b.FMH_Id,
                                                 FTI_Id = c.FTI_Id,
                                                 FMA_Id = a.FMA_Id,
                                                 FMH_Flag = b.FMH_Flag,
                                                 totalamount = d.FSS_NetAmount,
                                                 FSS_ToBePaid = Convert.ToInt64(a.FTP_Paid_Amt) - d.FSS_OBArrearAmount,
                                                 FSS_ConcessionAmount = d.FSS_ConcessionAmount,
                                                 FSS_FineAmount = d.FSS_FineAmount,
                                                 FSS_CurrentYrCharges = d.FSS_CurrentYrCharges,
                                                 FSS_TotalToBePaid = d.FSS_TotalToBePaid,
                                                 FSS_OBArrearAmount = d.FSS_OBArrearAmount,
                                                 FSS_RebateAmount = d.FSS_RebateAmount

                                             }).ToArray();

                data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                          from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                          from e in _YearlyFeeGroupMappingContext.admissioncls
                                          from f in _YearlyFeeGroupMappingContext.school_M_Section
                                          where (f.ASMS_Id == d.ASMS_Id && a.FYP_Id == data.FYP_Id && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              Amst_Id = c.AMST_Id,
                                              AMST_FirstName = c.AMST_FirstName,
                                              AMST_MiddleName = c.AMST_MiddleName,
                                              AMST_LastName = c.AMST_LastName,
                                              FYP_Receipt_No = a.FYP_Receipt_No,
                                              FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                              FYP_Tot_Amount = a.FYP_Tot_Amount,
                                              classname = e.ASMCL_ClassName,
                                              sectionname = f.ASMC_SectionName,
                                              FYP_Id = a.FYP_Id,
                                              AMST_AdmNo = c.AMST_AdmNo,
                                              FYP_Date = a.FYP_Date,
                                              FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                              FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                              FYP_Bank_Name = a.FYP_Bank_Name,
                                              rollno = d.AMAY_RollNo,
                                              amst_mobile = c.AMST_MobileNo,
                                              fathername = c.AMST_FatherName,
                                              studentdob = c.AMST_DOB,
                                              FYP_Remarks = a.FYP_Remarks,
                                              FYP_DeviceFlag = a.FYP_DeviseFlg,
                                              FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                          }

    ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();

                //parveen addead
                data.filusername = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from c in _YearlyFeeGroupMappingContext.admissioncls
                                    from d in _YearlyFeeGroupMappingContext.school_M_Section
                                    from e in _YearlyFeeGroupMappingContext.StudentAppUserLoginDMO
                                    from f in _YearlyFeeGroupMappingContext.applicationUser
                                    where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && e.AMST_ID == a.AMST_Id && e.STD_APP_ID == f.Id)
                                    select new FeeStudentTransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        classname = c.ASMCL_ClassName,
                                        sectionname = d.ASMC_SectionName,
                                        fathername = a.AMST_FatherName,
                                        studentdob = a.AMST_DOB,
                                        amst_mobile = a.AMST_MobileNo,
                                        portalusername = f.UserName,
                                        AMST_Photoname = a.AMST_Photoname
                                    }
              ).Distinct().ToArray();
                //End

                var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_CurrentYrCharges = a.FSS_CurrentYrCharges,
                                                 FSS_TotalToBePaid = a.FSS_TotalToBePaid,
                                                 FSS_ToBePaid = a.FSS_ToBePaid,
                                                 FSS_PaidAmount = a.FSS_PaidAmount,
                                                 FSS_ConcessionAmount = a.FSS_ConcessionAmount,
                                                 FSS_AdjustedAmount = a.FSS_AdjustedAmount,
                                                 FSS_WaivedAmount = a.FSS_WaivedAmount,
                                                 FSS_RefundAmount = a.FSS_RefundAmount,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMG_Id = b.FMG_Id,
                                                 FSS_RebateAmount = a.FSS_RebateAmount,

                                             }
           ).ToList();

                data.showstaticticsdetails = (from i in showstaticticsdetails
                                              group i by new { i.FMG_Id, i.FMG_GroupName } into g
                                              select new FeeStudentTransactionDTO
                                              {
                                                  FSS_CurrentYrCharges = g.Sum(t => t.FSS_CurrentYrCharges),
                                                  FSS_TotalToBePaid = g.Sum(t => t.FSS_TotalToBePaid),
                                                  FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid),
                                                  FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),
                                                  FSS_ConcessionAmount = g.Sum(t => t.FSS_ConcessionAmount),
                                                  FSS_AdjustedAmount = g.Sum(t => t.FSS_AdjustedAmount),
                                                  FSS_WaivedAmount = g.Sum(t => t.FSS_WaivedAmount),
                                                  FSS_RefundAmount = g.Sum(t => t.FSS_RefundAmount),
                                                  FMG_GroupName = g.Key.FMG_GroupName,
                                                  FMG_Id = g.Key.FMG_Id,
                                                  FSS_RebateAmount = g.Sum(t => t.FSS_RebateAmount)

                                              }).Distinct().ToArray();

                data.fetchmodeofpayment = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                           from g in _YearlyFeeGroupMappingContext.IVRM_ModeOfPayment
                                           from h in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool
                                           where (a.FYP_Id == b.FYP_Id && a.ASMAY_ID == b.ASMAY_Id && b.FYP_Id == h.FYP_Id && a.FYP_Id == h.FYP_Id && g.IVRMMOD_ModeOfPayment_Code == h.FYP_TransactionTypeFlag && a.MI_Id == data.MI_Id && g.MI_Id == data.MI_Id
                                           && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               Bank_Date = h.FYPPM_DDChequeDate,
                                               Bank_No = a.FYP_DD_Cheque_No,
                                               Bank_Amount = h.FYPPM_TotalPaidAmount,
                                               Bank_Name = a.FYP_Bank_Name,
                                               fyppM_TransactionTypeFlag = h.FYP_TransactionTypeFlag,
                                               IVRMMOD_Id = g.IVRMMOD_Id,
                                               IVRMMOD_ModeOfPayment = g.IVRMMOD_ModeOfPayment,
                                               IVRMMOD_Flag = g.IVRMMOD_Flag,
                                               IVRMMOD_ModeOfPayment_Code = g.IVRMMOD_ModeOfPayment_Code,
                                               FYP_Id = a.FYP_Id,
                                               FYPPM_Id = h.FYPPM_Id
                                           }
).Distinct().ToArray();

                data.narrationlist = _YearlyFeeGroupMappingContext.MasterNarrationDMO.Where(t => t.MI_ID == data.MI_Id && t.FMNAR_ActiveFlag == true).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<FeeStudentTransactionDTO> printreceiptnew([FromBody] FeeStudentTransactionDTO data)
        {


            try
            {
                printcommon(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        private FeeStudentTransactionDTO printcommon(FeeStudentTransactionDTO data)
        {
            try
            {

                var fillstudent = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                   from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   from g in _YearlyFeeGroupMappingContext.admissioncls
                                   from h in _YearlyFeeGroupMappingContext.school_M_Section
                                   from i in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                   from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from k in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                   where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id)
                                   select new FeeStudentTransactionDTO
                                   {
                                       Amst_Id = f.AMST_Id,
                                       AMST_FirstName = i.AMST_FirstName,
                                       AMST_MiddleName = i.AMST_MiddleName,
                                       AMST_LastName = i.AMST_LastName,
                                       FMH_Id = d.FMH_Id,
                                       FMH_FeeName = d.FMH_FeeName,
                                       FTI_Name = e.FTI_Name,
                                       FTI_Id = e.FTI_Id,
                                       FYP_Receipt_No = b.FYP_Receipt_No,
                                       FTP_Paid_Amt = a.FTP_Paid_Amt,
                                       FTP_Concession_Amt = a.FTP_Concession_Amt,
                                       FTP_Waived_Amt = a.FTP_Waived_Amt,
                                       FTP_Fine_Amt = a.FTP_Fine_Amt,
                                       FYP_Date = b.FYP_Date,
                                       classname = g.ASMCL_ClassName,
                                       sectionname = h.ASMC_SectionName,
                                       rollno = f.AMAY_RollNo,
                                       admno = i.AMST_AdmNo,
                                       fathername = i.AMST_FatherName,
                                       mothername = i.AMST_MotherName,
                                       FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                                       FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                                       FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                                       FYP_Bank_Name = b.FYP_Bank_Name,
                                       FYP_Remarks = b.FYP_Remarks,
                                       AMST_RegistrationNo = i.AMST_RegistrationNo,
                                       FMCC_ConcessionName = k.FMCC_ConcessionName,
                                       totalcharges = c.FMA_Amount
                                   }
               ).Distinct().ToList();

                data.fillstudentviewdetails = fillstudent.ToArray();

                List<long> lstheadid = new List<long>();
                foreach (var lst in fillstudent)
                {
                    lstheadid.Add(lst.FMH_Id);
                }
                FeeStudentTransactionDTO fst = new FeeStudentTransactionDTO();
                List<FeeStudentTransactionDTO> lstfst = new List<FeeStudentTransactionDTO>();
                foreach (var lst1 in lstheadid.Distinct())
                {
                    decimal totalcharges = 0, Fine_Amt = 0, Concession_Amt = 0, Waived_Amt = 0, paidamt = 0;
                    foreach (var lst in fillstudent)
                    {
                        if (lst1.Equals(lst.FMH_Id))
                        {
                            fst.FMH_FeeName = lst.FMH_FeeName;
                            totalcharges = totalcharges + lst.totalcharges;
                            Fine_Amt = Fine_Amt + lst.FTP_Fine_Amt;
                            Concession_Amt = Concession_Amt + lst.FTP_Concession_Amt;
                            Waived_Amt = Waived_Amt + lst.FTP_Waived_Amt;
                            paidamt = paidamt + lst.FTP_Paid_Amt;
                            fst.totalcharges = totalcharges;
                            fst.FTP_Fine_Amt = Fine_Amt;
                            fst.FTP_Concession_Amt = Concession_Amt;
                            fst.FTP_Waived_Amt = Waived_Amt;
                            fst.FTP_Paid_Amt = paidamt;

                        }
                    }
                    lstfst.Add(fst);
                }
                data.filltotaldetails = lstfst.ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool
                                           where a.FYP_Id == b.FYP_Id && a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeStudentTransactionDTO
                                           {
                                               FTP_Paid_Amt = b.FYPPM_TotalPaidAmount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,

                                               FYPPM_TotalPaidAmount = b.FYPPM_TotalPaidAmount,
                                               FYP_TransactionTypeFlag = b.FYP_TransactionTypeFlag,
                                               FYPPM_BankName = b.FYPPM_BankName,
                                               FYPPM_DDChequeNo = b.FYPPM_DDChequeNo,
                                               FYPPM_DDChequeDate = b.FYPPM_DDChequeDate,
                                               FYPPM_TransactionId = b.FYPPM_TransactionId,
                                               FYPPM_PaymentReferenceId = b.FYPPM_PaymentReferenceId,
                                               IVRMMOD_ModeOfPayment = b.FYP_TransactionTypeFlag,
                                               IVRMMOD_Flag = b.FYP_TransactionTypeFlag
                                           }
               ).ToArray();


                List<Institution> masins = new List<Institution>();
                masins = _context.Institute.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.masterinstitution = masins.ToArray();

                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id)
                               select new FeeStudentTransactionDTO
                               {
                                   fmt_id = e.FMT_Id
                               }

                 ).Distinct().OrderByDescending(t => t.fmt_id).ToArray();

                long fmt_id_new = 0;

                long initialfmtids = Convert.ToInt64(feeterm[0].fmt_id);

                fmt_id_new = Convert.ToInt64(feeterm[0].fmt_id) + 1;
                //fmt_id_new = 10;


                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
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

                List<FeeStudentTransactionDTO> fordate = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> fordateinfyp = new List<FeeStudentTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                           from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                           where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                           select new FeeStudentTransactionDTO
                           {
                               date = f.FTDD_Day,
                               month = f.FTDD_Month,
                           }
                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
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
                        data.year = curyear.Year.ToString();
                    }
                    else
                    {
                        months2.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year + 1;
                        data.year = nextyr.ToString();
                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();
                    // maxmonth = (Convert.ToInt32(data.month) - 1).ToString();
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

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }
                else if (months2.Count() > 0)
                {
                    data.month = months2.Min().ToString();
                    maxmonth = months2.Max().ToString();

                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;

                }
                for (int i = 0; i < months.Count(); i++)
                {
                    if (Convert.ToInt32(data.month) == months[i])
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
                            data.year = curyear.Year.ToString();
                        }
                        else
                        {
                            maxmonth = startperiod.Max().ToString();
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year + 1;
                            data.year = nextyr.ToString();
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

                var termperiodlist = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == initialfmtids).ToArray();

                monthnameinitial = termperiodlist[0].FromMonth.ToString();
                monthnameend = termperiodlist[0].ToMonth.ToString();

                data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;

                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                                  select new FeeStudentTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FSS_ToBePaid
                                  }
             ).Sum(t => t.FSS_ToBePaid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO Search_Chaln_No(FeeStudentTransactionDTO data)
        {
            try
            {
                List<FeeStudentTransactionDTO> excludefine = new List<FeeStudentTransactionDTO>();
                var flag = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ID == data.ASMAY_Id && t.FYP_ChallanNo == data.FYP_ChallanNo && !string.IsNullOrEmpty(t.FYP_ChallanNo)).Select(t => t.FYP_Id).Distinct().ToList();
                if (flag.Count > 0)
                {
                    data.Challan_Flag = true;
                    data.FYP_Id = flag[0];
                    data.Amst_Id = _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO.Single(t => t.FYP_Id == data.FYP_Id).AMST_Id;

                    var ToBePaid_Amount = (from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                           from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                           where (b.FYP_Id == data.FYP_Id && b.FMA_Id == c.FMA_Id && c.AMST_Id == data.Amst_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                           select c.FSS_ToBePaid).Sum();

                    if (ToBePaid_Amount > 0)
                    {
                        data.returnval = "Pay";
                        data.AMST_AdmNo = _YearlyFeeGroupMappingContext.AdmissionStudentDMO.Single(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.Amst_Id).AMST_AdmNo;
                        data.AMST_FirstName = _YearlyFeeGroupMappingContext.AdmissionStudentDMO.Single(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.Amst_Id).AMST_FirstName;

                        if (data.configset.Equals("T"))
                        {
                            var fmh_ids = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                           where (a.FYP_Id == data.FYP_Id && a.FMA_Id == b.FMA_Id)
                                           select b.FMH_Id).Distinct().ToList();
                            var fti_ids = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                           where (a.FYP_Id == data.FYP_Id && a.FMA_Id == b.FMA_Id)
                                           select b.FTI_Id).Distinct().ToList();

                            data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                                    from b in _YearlyFeeGroupMappingContext.feeTr
                                                    where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id && fmh_ids.Contains(a.FMH_Id) && fti_ids.Contains(a.FTI_Id))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        FMG_GroupName = b.FMT_Name,
                                                        FMG_Id = a.FMT_Id,
                                                    }).Distinct().ToArray();

                        }
                        else
                        {
                            var groups = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                          where (a.FYP_Id == data.FYP_Id && a.FMA_Id == b.FMA_Id)
                                          select b.FMG_Id).Distinct().ToList();

                            data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                    where (a.FMG_ActiceFlag == true && groups.Contains(a.FMG_Id))
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        FMG_GroupName = a.FMG_GroupName,
                                                        FMG_Id = a.FMG_Id,
                                                    }
             ).Distinct().ToArray();

                        }


                        excludefine = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                       from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                       from c in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                       from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                       from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                       from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                       where (a.FYP_Id == data.FYP_Id && a.FMA_Id == b.FMA_Id && b.FMG_Id == c.FMG_Id && b.FMH_Id == d.FMH_Id && b.FTI_Id == e.FTI_Id && f.MI_Id == data.MI_Id && f.AMST_Id == data.Amst_Id && f.ASMAY_Id == data.ASMAY_Id && f.FMA_Id == a.FMA_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMA_Id = a.FMA_Id,
                                           FMH_FeeName = d.FMH_FeeName,
                                           FMH_Flag = d.FMH_Flag,
                                           FTI_Name = e.FTI_Name,
                                           FMH_Id = b.FMH_Id,
                                           FTI_Id = b.FTI_Id,
                                           FSS_ToBePaid = Convert.ToInt64(a.FTP_Paid_Amt),
                                           FSS_ConcessionAmount = Convert.ToInt64(a.FTP_Concession_Amt),
                                           FSS_FineAmount = a.FTP_Fine_Amt,
                                           totalamount = (Convert.ToInt64(a.FTP_Paid_Amt) + Convert.ToInt64(a.FTP_Concession_Amt) + a.FTP_Fine_Amt),
                                           FSS_CurrentYrCharges = (Convert.ToInt64(a.FTP_Paid_Amt) + Convert.ToInt64(a.FTP_Concession_Amt) + Convert.ToInt64(a.FTP_Fine_Amt)),
                                           FSS_TotalToBePaid = Convert.ToInt64(a.FTP_Paid_Amt),

                                           FMG_Id = b.FMG_Id,
                                           FMG_GroupName = c.FMG_GroupName,
                                       }).OrderBy(t => t.FMH_Id).ToList();

                        data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                        from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                        from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                        from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                        from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                        where (a.FYP_Id == data.FYP_Id && a.FMA_Id == b.FMA_Id && b.FMG_Id == c.FMG_Id && b.FMH_Id == d.FMH_Id && b.FTI_Id == e.FTI_Id && f.MI_Id == data.MI_Id && f.AMST_Id == data.Amst_Id && f.ASMAY_Id == data.ASMAY_Id && f.FMA_Id == a.FMA_Id)
                                        select new FeeStudentTransactionDTO
                                        {
                                            FMA_Id = a.FMA_Id,
                                            FMH_FeeName = d.FMH_FeeName,
                                            FMH_Flag = d.FMH_Flag,
                                            FTI_Name = e.FTI_Name,
                                            FMH_Id = b.FMH_Id,
                                            FTI_Id = b.FTI_Id,
                                            FSS_ToBePaid = Convert.ToInt64(a.FTP_Paid_Amt),
                                            FSS_ConcessionAmount = Convert.ToInt64(a.FTP_Concession_Amt),
                                            FSS_FineAmount = a.FTP_Fine_Amt,
                                            totalamount = (Convert.ToInt64(a.FTP_Paid_Amt) + Convert.ToInt64(a.FTP_Concession_Amt) + a.FTP_Fine_Amt),
                                            FSS_CurrentYrCharges = (Convert.ToInt64(a.FTP_Paid_Amt) + Convert.ToInt64(a.FTP_Concession_Amt) + Convert.ToInt64(a.FTP_Fine_Amt)),
                                            FSS_TotalToBePaid = Convert.ToInt64(a.FTP_Paid_Amt),

                                            FMG_Id = b.FMG_Id,
                                            FMG_GroupName = c.FMG_GroupName,
                                        }).OrderBy(t => t.FMH_Id).ToArray();


                        if (data.alldata.Length > 0)
                        {
                            List<FeeStudentTransactionDTO> fines_fma_ids = new List<FeeStudentTransactionDTO>();
                            var fine_amount = 0;
                            foreach (FeeStudentTransactionDTO x in data.alldata)
                            {
                                FeeStudentTransactionDTO sew = new FeeStudentTransactionDTO();
                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Sp_Calculate_Fine";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@On_Date",
                                        SqlDbType.DateTime)
                                    {
                                        Value = data.FYP_Date
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@fma_id",
                                       SqlDbType.BigInt)
                                    {
                                        Value = x.FMA_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                   SqlDbType.BigInt)
                                    {
                                        Value = data.ASMAY_Id
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
                                    fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    sew.FMA_Id = x.FMA_Id;
                                    sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    fines_fma_ids.Add(sew);
                                }
                            }
                            data.Fine_FMA_Ids = fines_fma_ids.Distinct().ToArray();
                        }

                    }
                    else
                    {
                        data.returnval = "Paid";
                    }


                }
                else
                {
                    data.Challan_Flag = false;
                }

                foreach (var x in excludefine)
                {
                    if (x.FMH_Flag == "F")
                    {
                        x.FSS_CurrentYrCharges = 0;
                        x.FSS_TotalToBePaid = 0;
                        x.totalamount = 0;
                    }
                }

                data.alldata = excludefine.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return data;
        }
        public FeeStudentTransactionDTO Save_Chaln_No(FeeStudentTransactionDTO data)
        {
            try
            {
                var result = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_ID == data.ASMAY_Id && t.FYP_ChallanNo == data.FYP_ChallanNo && !string.IsNullOrEmpty(t.FYP_ChallanNo) && t.FYP_Id == data.FYP_Id);

                DateTime dt = DateTime.Now;

                //multimode

                foreach (var mode in data.Modes)
                {
                    if (mode.FYPPM_TotalPaidAmount > 0)
                    {
                        if (mode.FYPPM_TransactionTypeFlag == "C")
                        {
                            data.FYP_Bank_Name = "";
                            data.FYP_DD_Cheque_Date = dt;
                            data.FYP_DD_Cheque_No = "";
                            data.FYP_Bank_Or_Cash = mode.FYPPM_TransactionTypeFlag;
                        }
                        else if (mode.FYPPM_TransactionTypeFlag == "B" || mode.FYPPM_TransactionTypeFlag == "E" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R")
                        {
                            data.FYP_Bank_Name = mode.FYPPM_BankName;
                            data.FYP_DD_Cheque_Date = mode.FYPPM_DDChequeDate;
                            data.FYP_DD_Cheque_No = mode.FYPPM_DDChequeNo;
                            data.FYP_Bank_Or_Cash = mode.IVRMMOD_ModeOfPayment_Code;
                        }
                    }
                }

                //multimode

                result.FYP_Bank_Name = data.FYP_Bank_Name;
                result.FYP_Bank_Or_Cash = data.FYP_Bank_Or_Cash;
                result.FYP_DD_Cheque_No = data.FYP_DD_Cheque_No;
                result.FYP_DD_Cheque_Date = data.FYP_DD_Cheque_Date;
                result.FYP_Date = data.FYP_Date;
                result.FYP_Remarks = data.FYP_Remarks;
                result.FYP_Chq_Bounce = "CL";
                result.DOE = DateTime.Now;
                result.user_id = data.userid;

                result.FYP_OnlineChallanStatusFlag = "Sucessfull";

                data.temp_head_list = data.savetmpdata;

                get_grp_reptno(data);

                result.FYP_Receipt_No = data.FYP_Receipt_No;

                if (data.FYP_Receipt_No == "" || data.FYP_Receipt_No == null)
                {
                    data.returnval = "Record Not Saved because Receipt No is not Generated Automatic.Settings are missing";
                    return data;
                }

                else
                {

                    //Multimode of Payment

                    result.FYP_DeviseFlg = data.FYP_PayModeType;

                    foreach (var mode in data.Modes)
                    {
                        if (mode.FYPPM_TotalPaidAmount > 0)
                        {
                            Fee_Y_Payment_PaymentModeSchool obj2 = new Fee_Y_Payment_PaymentModeSchool();
                            obj2.FYP_Id = data.FYP_Id;
                            obj2.FYP_TransactionTypeFlag = mode.IVRMMOD_ModeOfPayment_Code;
                            obj2.FYPPM_TotalPaidAmount = mode.FYPPM_TotalPaidAmount;
                            obj2.FYPPM_LedgerId = 0;
                            obj2.FYPPM_BankName = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_BankName;
                            obj2.FYPPM_DDChequeNo = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_DDChequeNo;
                            obj2.FYPPM_DDChequeDate = mode.FYPPM_TransactionTypeFlag == "C" ? data.FYP_Date : mode.FYPPM_DDChequeDate;
                            obj2.FYPPM_TransactionId = "";
                            obj2.FYPPM_PaymentReferenceId = "";
                            obj2.FYPPM_ClearanceStatusFlag = "0";
                            obj2.FYPPM_ClearanceDate = mode.FYPPM_DDChequeDate;
                            _YearlyFeeGroupMappingContext.Add(obj2);
                        }
                    }

                    //Multimode of Payment

                    result.UpdatedDate = DateTime.Now;

                    _YearlyFeeGroupMappingContext.Update(result);
                    var FMA_Ids = _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).Select(t => t.FMA_Id).Distinct().ToList();

                    foreach (var y in data.savetmpdata)
                    {
                        if (y.FMH_FeeName != "Fine") {
                            var sta_obj = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FMA_Id == y.FMA_Id && t.AMST_Id == data.Amst_Id);

                            sta_obj.FSS_PaidAmount = y.FSS_ToBePaid;
                            sta_obj.FSS_ToBePaid = sta_obj.FSS_ToBePaid - sta_obj.FSS_PaidAmount;

                            //added on 11-07-2018
                            var fineheadss = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                              from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              where (a.MI_Id == data.MI_Id && a.FMA_Id == y.FMA_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == data.Amst_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  FMH_Id = a.FMH_Id,
                                              }
                               ).Distinct().Take(1);

                            if (fineheadss.Count() > 0)
                            {
                                sta_obj.FSS_FineAmount = sta_obj.FSS_FineAmount + sta_obj.FSS_ToBePaid;
                            }
                            //added on 11-07-2018

                            _YearlyFeeGroupMappingContext.Update(sta_obj);
                        }
                        else
                        {
                            var sta_obj = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FMA_Id == y.FMA_Id && t.AMST_Id == data.Amst_Id);

                            sta_obj.FSS_PaidAmount = sta_obj.FSS_PaidAmount + y.FSS_ToBePaid;
                            sta_obj.FSS_ToBePaid =0;

                            //added on 11-07-2018
                            var fineheadss = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                              from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              where (a.MI_Id == data.MI_Id && a.FMA_Id == y.FMA_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == data.Amst_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  FMH_Id = a.FMH_Id,
                                              }
                               ).Distinct().Take(1);

                            if (fineheadss.Count() > 0)
                            {
                                sta_obj.FSS_FineAmount = sta_obj.FSS_FineAmount + y.FSS_ToBePaid;
                            }
                            //added on 11-07-2018

                            _YearlyFeeGroupMappingContext.Update(sta_obj);
                        }

                    }

                    //   }

                    var ResultCount = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (ResultCount >= 1)
                    {
                        data.returnval = "Save";
                    }
                    else
                    {
                        data.returnval = "Cancel";
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
        public FeeStudentTransactionDTO SendEmail(FeeStudentTransactionDTO dto)
        {
            dto.returnval = "";
            try
            {

                Email Email = new Email(_context);
                var email_list = _YearlyFeeGroupMappingContext.AdmissionStudentDMO.Where(t => t.AMST_Id == dto.Amst_Id).ToList();
                string slaryslip = "Fee Receipt Of Student " + email_list.FirstOrDefault().AMST_FirstName + ".pdf";

                if (email_list.Count > 0)
                {
                    string Subject = "Fee Receipt";
                    string m = sendmailfor(dto.MI_Id, email_list.FirstOrDefault().AMST_emailId, dto.Template[0].TemplateString, Subject, slaryslip, email_list.FirstOrDefault().AMST_FirstName);

                    dto.returnval = m;

                    Console.WriteLine(m);
                }
                else
                {
                    dto.returnval = "notFound";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returnval = "Error";
            }

            return dto;

        }
        public string sendmailfor(long MI_Id, string Email, string Template, string Subject, string slaryslip, string StudentName)
        {

            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
            try
            {
                string Mailmsg = getMessage(MI_Id, Template, StudentName);
                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)

                {

                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);

                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")

                    {
                        mailcc = alldetails[0].IVRM_mailcc.ToString();
                    }

                    SendGridMessage message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);
                    if (mailcc != null && mailcc != "")

                    {
                        message.AddCc(mailcc);

                    }
                    StringBuilder sb = new StringBuilder(Template);

                    StringReader sr = new StringReader(sb.ToString());

                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {

                        try
                        {
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                            pdfDoc.Open();

                            htmlparser.Parse(sr);
                            pdfDoc.Close();

                            byte[] bytess = memoryStream.ToArray();

                            memoryStream.Close();

                            var file = Convert.ToBase64String(bytess);
                            string emp;
                            emp = Convert.ToString(sr);
                            string c = "";
                            string v = emp.Replace("System.IO.StringReader", "FeeReceipt.Pdf");
                            message.AddAttachment(v, file);
                            message.HtmlContent = Mailmsg;
                            var client = new SendGridClient(sengridkey);
                            client.SendEmailAsync(message).Wait();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                        }


                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)


                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Fees"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return "Error";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string getMessage(long MI_Id, string TemplateName, string StudentName)
        {
            string strMessage = "";
            string strEmpname = "";
            var smsemaildata = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Fee_Receipt").FirstOrDefault();
            if (smsemaildata != null)
            {
                strMessage = smsemaildata.ISES_MailBody + " " + smsemaildata.ISES_MailFooter;
                strMessage = strMessage.Replace("[Name]", StudentName);

                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(strMessage, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
            }
            else
            {
                strMessage = "Dear Sir/Madam,Please find enclosed PDF attachment. This is auto generated email Don't replay to this email.Thanking You.";
            }
            return strMessage;
        }
        public FeeStudentTransactionDTO getduedates(FeeStudentTransactionDTO data)
        {
            try
            {
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_due_dates";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar, 100)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmayid", SqlDbType.NVarChar, 100)
                    {
                        Value = data.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
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
                        data.getduedates = retObject1.ToArray();
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


            try
            {
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getfeereciptformat";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt, 100)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt, 100)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.BigInt, 100)
                    {
                        Value = data.FYP_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.BigInt, 100)
                    {
                        Value = data.Amst_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.fillstudentviewdetails = retObject1.ToArray();
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

            return data;
        }
        public FeeStudentTransactionDTO getheadwisedetails(FeeStudentTransactionDTO data)
        {
            try
            {
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Headwise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_Id", SqlDbType.BigInt, 100)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt, 100)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMGG_Id", SqlDbType.BigInt, 100)
                    {
                        Value = data.FMGG_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMT_Id", SqlDbType.BigInt, 100)
                    {
                        Value = data.FMT_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.BigInt, 100)
                    {
                        Value = data.Amst_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
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
                        data.getfeeheaddetails = retObject1.ToArray();
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
            return data;
        }
        public FeeStudentTransactionDTO viewstatus(FeeStudentTransactionDTO data)
        {
            try
            {
                RazorpayTransactionLogs(data);



                data.razorpaytransactionlogs = (from a in _YearlyFeeGroupMappingContext.Fee_Payment_Settlement_DetailsDMO
                                                from b in _YearlyFeeGroupMappingContext.Fee_Payment_Overall_Settlement_DetailsDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                where (a.FYPPST_Id == b.FYPPST_Id && c.fyp_transaction_id == a.FYPPSD_Payment_Id && c.ASMAY_ID == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.FYP_Id == data.FYP_Id)
                                                select new FeeStudentTransactionDTO
                                                {
                                                    FYPPST_Settlement_Date = b.FYPPST_Settlement_Date,
                                                }
                                           ).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        private static string getAppDetailsUa()
        {
            List<Dictionary<string, string>> appsDetails = RazorpayClient.AppsDetails;

            string appsDetailsUa = string.Empty;

            foreach (Dictionary<string, string> appsDetail in appsDetails)
            {
                string appUa = string.Empty;

                if (appsDetail.ContainsKey("title"))
                {
                    appUa = appsDetail["title"];

                    if (appsDetail.ContainsKey("version"))
                    {
                        appUa += appsDetail["version"];
                    }
                }

                appsDetailsUa += appUa;
            }

            return appsDetailsUa;
        }
        public FeeStudentTransactionDTO RazorpayTransactionLogs(FeeStudentTransactionDTO data)
        {
            try
            {
                List<Translogsresults> razorpayparam = new List<Translogsresults>();
                List<FeeStudentTransactionDTO> razorpaytransactionlogs = new List<FeeStudentTransactionDTO>();
                razorpaytransactionlogs = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                           from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from c in _YearlyFeeGroupMappingContext.Adm_M_Student
                                           where (a.FYP_Id == b.FYP_Id && a.FYP_Id == data.FYP_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id==c.AMST_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               trans_id = b.fyp_transaction_id,
                                               FYP_PayModeType = b.FYP_PayModeType,
                                               FYP_Date = b.FYP_Date,
                                               FMOT_PayGatewayType = b.FYP_PayGatewayType,
                                               Amst_Id=a.AMST_Id,
                                               amst_email_id=c.AMST_emailId,
                                               amst_mobile=c.AMST_MobileNo,
                                               FYP_Tot_Amount=b.FYP_Tot_Amount,
                                               FYP_PaymentReference_Id=b.FYP_PaymentReference_Id
                                           }
     ).ToList();

                if (razorpaytransactionlogs.Count > 0)
                {
                    if (razorpaytransactionlogs[0].FMOT_PayGatewayType == "RAZORPAY")
                    {
                        for (int i = 0; i < razorpaytransactionlogs.Count(); i++)
                        {
                            string PaymentStatusurl = "https://api.razorpay.com/v1/orders/ID/payments";

                            PaymentDetails response1 = new PaymentDetails();
                            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                            paymentdetails = _YearlyFeeGroupMappingContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();
                            RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                            string method = "GET";
                            PaymentStatusurl = PaymentStatusurl.Replace("ID", razorpaytransactionlogs[i].trans_id);
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PaymentStatusurl);
                            request.Method = method.ToString();
                            request.ContentLength = 0;
                            request.ContentType = "application/json";

                            string userAgent = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                            request.UserAgent = "razorpay-dot-net/" + userAgent;

                            string authString = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                                Encoding.UTF8.GetBytes(authString));

                            System.Net.WebResponse resp = request.GetResponseAsync().Result;
                            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                            string s = sr.ReadToEnd().Trim();
                            JObject joResponse1 = JObject.Parse(s);
                            JArray array1 = (JArray)joResponse1["items"];

                            foreach (JObject root1 in array1)
                            {
                                if ((String)root1["status"] != "failed")
                                {
                                    razorpayparam.Add(new Translogsresults
                                    {
                                        payment_id = (String)root1["id"],
                                        responsestatuslogs = (String)root1["status"],
                                        error_description = (String)root1["error_description"],
                                        order_id = (String)root1["order_id"],
                                        FYP_Date = razorpaytransactionlogs[i].FYP_Date,
                                        FMA_Amount = (Int32)root1["amount"],
                                        FYP_PayModeType = razorpaytransactionlogs[i].FYP_PayModeType,
                                        FMOT_PayGatewayType = razorpaytransactionlogs[i].FMOT_PayGatewayType
                                    });
                                }
                            }
                        }
                        data.translogresults = razorpayparam.ToArray();
                    }
                    else if (razorpaytransactionlogs[0].FMOT_PayGatewayType == "EASEBUZZ")                    {                        for (int i = 0; i < razorpaytransactionlogs.Count(); i++)                        {                            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();                            paymentdetails = _YearlyFeeGroupMappingContext.Fee_PaymentGateway_Details.Where(q => q.MI_Id == data.MI_Id && q.FPGD_PGName == "EASEBUZZ").Distinct().ToList();                            var masterinstitution = _YearlyFeeGroupMappingContext.master_institution.Where(z => z.MI_ActiveFlag == 1 && z.MI_Id == data.MI_Id).ToList();                            string txnid = razorpaytransactionlogs[i].trans_id.ToString();                            string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;                            string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;                            string merchant_email = masterinstitution[0].MI_PGRegisteredEmailId;                            string env = "prod";

                            Easebuzz t = new Easebuzz(secret, key, env);                            var transdate = razorpaytransactionlogs[i].FYP_Date.ToString("dd-MM-yyyy");

                            var res = t.payoutAPI(merchant_email, transdate);

                            JObject joResponse1 = JObject.Parse(res);                            JArray array1 = (JArray)joResponse1["payouts_history_data"];

                            var payment_id = razorpaytransactionlogs[i].FYP_PaymentReference_Id.ToString();


                            var FMA_Amountnew = "";                            DateTime date = DateTime.Now;                            foreach (JObject root1 in array1)                            {                                if ((bool)joResponse1["status"] == true)                                {                                    JArray array2 = (JArray)root1["peb_transactions"];                                    JArray array3 = (JArray)root1["split_payouts"];                                    foreach (JObject root2 in array2)                                    {                                        if ((String)root2["txnid"] == txnid)                                        {                                            payment_id = payment_id;                                            txnid = txnid;                                            FMA_Amountnew = (String)root2["peb_settlement_amount"];                                            date = (DateTime)root1["payout_actual_date"];                                        }                                    }                                }                            }                            if ((bool)joResponse1["status"] == true)                            {                                razorpayparam.Add(new Translogsresults                                {                                    payment_id = payment_id,                                    responsestatuslogs = ((bool)joResponse1["status"]).ToString(),                                    error_description = "",                                    order_id = txnid,                                    FYP_Date = Convert.ToDateTime(date),                                    //FMA_Amount = Convert.ToInt64(FMA_Amountnew),                                    FYP_PayModeType = razorpaytransactionlogs[i].FYP_PayModeType,                                    FMOT_PayGatewayType = razorpaytransactionlogs[i].FMOT_PayGatewayType                                });                            }                        }                        data.translogresults = razorpayparam.ToArray();                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeStudentTransactionDTO viewpaydetails(FeeStudentTransactionDTO data)
        {
            try
            {
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GroupwiePaymentdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_Id", SqlDbType.BigInt, 100)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt, 100)
                    {
                        Value = data.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
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
                        data.getgroupwisepaymentdetails = retObject1.ToArray();
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
            return data;
        }
        public FeeStudentTransactionDTO viewpayexcessdetails(FeeStudentTransactionDTO data)
        {
            try
            {
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "excesspaiddetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_Id", SqlDbType.BigInt, 100)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt, 100)
                    {
                        Value = data.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
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
                        data.getexcesspaidstudents = retObject1.ToArray();
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
            return data;
        }

        public FeeStudentTransactionDTO OBTransfer(FeeStudentTransactionDTO data)
        {
            try
            {
                _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("OB_Transfer_AsOn_FinalacialYearEndDate @p0", data.MI_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //rebate condition
        public FeeStudentTransactionDTO rebateamountcalc(FeeStudentTransactionDTO data)
        {
            try
            {

                string rebateamount = "0";
                var fine_amount = 0;
                string fmtid = "0";
                string fmgid = "0";
                long? fmttotal = 0;

                if (data.configset.Equals("T"))
                {
                    var cnt1 = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id && t.userid == data.userid && t.FMC_RebateAplicableFlg == true && t.FMC_RebateAgainstFullPaymentFlg == true && t.FMC_RebateAgainstPartialPaymentFlg == true).ToList();
                    //var fine_amount = 0;
                    //string fmtid = "0";
                    //long? fmttotal = 0;


                    foreach (var y in data.FMTtotal)
                    {
                        fmtid = fmtid + "," + y.Fmtidnew;
                        fmttotal += Convert.ToInt64(y.FMT_Total);


                    }






                    if (cnt1.Count > 0)
                    {
                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Fee_RebateTermWise_calculation_BOTH";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                     SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                           SqlDbType.VarChar)
                            {
                                Value = data.Amst_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                          SqlDbType.VarChar)
                            {
                                Value = fmtid
                            });
                            cmd.Parameters.Add(new SqlParameter("@paiddate",
                          SqlDbType.DateTime)
                            {
                                Value = data.FYP_Date
                            });
                            cmd.Parameters.Add(new SqlParameter("@paidamount",
                           SqlDbType.VarChar)
                            {
                                Value = fmttotal
                            });

                            cmd.Parameters.Add(new SqlParameter("@USERID",
                           SqlDbType.VarChar)
                            {
                                Value = data.userid
                            });

                            cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
                SqlDbType.BigInt)
                            {
                                Direction = ParameterDirection.Output
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var data1 = cmd.ExecuteNonQuery();
                            rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                        }



                    }
                    else
                    {
                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Fee_RebateTermWise_calculation";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                     SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                           SqlDbType.VarChar)
                            {
                                Value = data.Amst_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                          SqlDbType.VarChar)
                            {
                                Value = fmtid
                            });
                            cmd.Parameters.Add(new SqlParameter("@paiddate",
                          SqlDbType.DateTime)
                            {
                                Value = data.FYP_Date
                            });
                            cmd.Parameters.Add(new SqlParameter("@paidamount",
                           SqlDbType.VarChar)
                            {
                                Value = fmttotal
                            });

                            cmd.Parameters.Add(new SqlParameter("@USERID",
                           SqlDbType.VarChar)
                            {
                                Value = data.userid
                            });

                            cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
                SqlDbType.BigInt)
                            {
                                Direction = ParameterDirection.Output
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var data1 = cmd.ExecuteNonQuery();
                            rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                        }

                    }

                    data.rebateamount = Convert.ToInt64(rebateamount);
                }
                else if (data.configset.Equals("G"))
                {
                    var d = data.FYP_Date.Date.ToString("yyyy-MM-dd");
                    var cnt1 = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id && t.userid == data.userid && t.FMC_RebateAplicableFlg == true && t.FMC_RebateAgainstFullPaymentFlg == true && t.FMC_RebateAgainstPartialPaymentFlg == true).ToList();

                    foreach (var y in data.FMTtotal)
                    {
                        fmtid = fmtid + "," + y.Fmtidnew;
                        fmttotal += y.FMT_Total;
                        fmgid = fmgid + "," + y.FMG_Id;
                    }

                    if (cnt1.Count > 0)
                    {
                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "FEE_RebateGroupwise_Calculation_Both";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                     SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                           SqlDbType.VarChar)
                            {
                                Value = data.Amst_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@FMG_ID",
                          SqlDbType.VarChar)
                            {
                                Value = fmgid
                            });
                            cmd.Parameters.Add(new SqlParameter("@FTI_ID",
                          SqlDbType.VarChar)
                            {
                                Value = fmtid
                            });
                            cmd.Parameters.Add(new SqlParameter("@paiddate",
                          SqlDbType.DateTime)
                            {
                                Value = d
                            });
                            cmd.Parameters.Add(new SqlParameter("@paidamount",
                           SqlDbType.VarChar)
                            {
                                Value = fmttotal
                            });

                            cmd.Parameters.Add(new SqlParameter("@USERID",
                           SqlDbType.VarChar)
                            {
                                Value = data.userid
                            });

                            //cmd.Parameters.Add(new SqlParameter("@totalrebateamount", SqlDbType.BigInt, Int32.MaxValue)
                            //{ Direction = ParameterDirection.Output });

                            //            cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
                            //SqlDbType.BigInt)
                            //            {
                            //                Direction = ParameterDirection.Output
                            //            });

                            cmd.Parameters.Add(new SqlParameter("@totalrebateamount", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var data1 = cmd.ExecuteNonQuery();


                            try
                            {
                                rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                            }
                            catch (Exception ee)
                            {
                                data.returnval = "Error";
                                Console.WriteLine(ee.Message);
                            }

                            //rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                        }



                    }
                    else
                    {
                        using (var cmd2 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd2.CommandText = "FEE_RebateGroupwise_Calculation";
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Add(new SqlParameter("@MI_Id",
                                     SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });

                            cmd2.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd2.Parameters.Add(new SqlParameter("@AMST_ID",
                           SqlDbType.VarChar)
                            {
                                Value = data.Amst_Id
                            });
                            cmd2.Parameters.Add(new SqlParameter("@FMG_Id",
                          SqlDbType.VarChar)
                            {
                                Value = fmgid
                            });
                            cmd2.Parameters.Add(new SqlParameter("@FTI_ID",
                       SqlDbType.VarChar)
                            {
                                Value = fmtid
                            });
                            cmd2.Parameters.Add(new SqlParameter("@paiddate",
                          SqlDbType.DateTime)
                            {
                                Value = d
                            });
                            cmd2.Parameters.Add(new SqlParameter("@paidamount",
                           SqlDbType.VarChar)
                            {
                                Value = fmttotal
                            });

                            cmd2.Parameters.Add(new SqlParameter("@USERID",
                           SqlDbType.VarChar)
                            {
                                Value = data.userid
                            });

                            cmd2.Parameters.Add(new SqlParameter("@totalrebateamount",
                SqlDbType.BigInt)
                            {
                                Direction = ParameterDirection.Output
                            });
                            if (cmd2.Connection.State != ConnectionState.Open)
                                cmd2.Connection.Open();

                            var data1 = cmd2.ExecuteNonQuery();
                            rebateamount = cmd2.Parameters["@totalrebateamount"].Value.ToString();
                        }

                    }

                    data.rebateamount = Convert.ToInt64(rebateamount);
                }


            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public async Task<FeeStudentTransactionDTO> Rebateapplyandsave(FeeStudentTransactionDTO pgmod)
        {
            DateTime dt = DateTime.Now;
            int r = 0;

            try
            {
                pgmod.FYP_Chq_Bounce = "CL";
                if (pgmod.FYP_Id != 0)
                {
                    var result = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.FYP_Id == pgmod.FYP_Id && t.ASMAY_ID == pgmod.ASMAY_Id);

                    FeePaymentDetailsDMO pmm = new FeePaymentDetailsDMO();

                    if (pgmod.automanualreceiptno != "Auto" || pgmod.auto_receipt_flag != 1)
                    {
                        result.FYP_Receipt_No = pgmod.FYP_Receipt_No;
                    }

                    result.FYP_Date = Convert.ToDateTime(pgmod.FYPcurr_Date);

                    if (pgmod.FYP_Remarks != "")
                    {
                        result.FYP_Remarks = pgmod.FYP_Remarks;
                    }
                    _YearlyFeeGroupMappingContext.Update(result);

                    //added by kavitha
                    foreach (var mode in pgmod.Modes)
                    {
                        if (mode.FYPPM_TotalPaidAmount > 0)
                        {
                            if (mode.FYPPM_TransactionTypeFlag == "C")
                            {
                                result.FYP_Bank_Name = "";
                                result.FYP_DD_Cheque_Date = dt;
                                result.FYP_DD_Cheque_No = "";
                                result.FYP_Bank_Or_Cash = mode.FYPPM_TransactionTypeFlag;
                            }
                            else if (mode.FYPPM_TransactionTypeFlag == "B" || mode.FYPPM_TransactionTypeFlag == "E" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R")
                            {
                                result.FYP_Bank_Name = mode.FYPPM_BankName;
                                result.FYP_DD_Cheque_Date = mode.FYPPM_DDChequeDate;
                                result.FYP_DD_Cheque_No = mode.FYPPM_DDChequeNo;
                                result.FYP_Bank_Or_Cash = mode.IVRMMOD_ModeOfPayment_Code;
                                _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Update(result);
                            }
                        }
                    }

                    for (int i = 0; i < pgmod.Modes.Length; i++)
                    {
                        var resultupdate = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool.Where(t => t.FYPPM_Id == pgmod.Modes[i].FYPPM_Id).FirstOrDefault();

                        resultupdate.FYP_TransactionTypeFlag = pgmod.Modes[i].IVRMMOD_ModeOfPayment_Code;


                        resultupdate.FYPPM_TotalPaidAmount = pgmod.Modes[i].FYPPM_TotalPaidAmount;
                        resultupdate.FYPPM_LedgerId = 0;
                        resultupdate.FYPPM_BankName = pgmod.Modes[i].FYPPM_TransactionTypeFlag == "C" ? "" : pgmod.Modes[i].FYPPM_BankName;
                        resultupdate.FYPPM_DDChequeNo = pgmod.Modes[i].FYPPM_TransactionTypeFlag == "C" ? "" : pgmod.Modes[i].FYPPM_DDChequeNo;
                        resultupdate.FYPPM_DDChequeDate = pgmod.Modes[i].FYPPM_TransactionTypeFlag == "C" ? pgmod.FYP_Date : pgmod.Modes[i].FYPPM_DDChequeDate;
                        resultupdate.FYPPM_TransactionId = "";
                        resultupdate.FYPPM_PaymentReferenceId = "";

                        resultupdate.FYPPM_ClearanceStatusFlag = "0";
                        resultupdate.FYPPM_ClearanceDate = pgmod.Modes[i].FYPPM_DDChequeDate;
                        _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool.Update(resultupdate);
                    }
                    //added by kavitha

                    //kiran
                    foreach (var y in pgmod.savetmpdata)
                    {
                        var fineheads = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                         from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                         where (a.MI_Id == pgmod.MI_Id && a.FMA_Id == c.FMA_Id && a.FMH_Id == b.FMH_Id && a.FMA_Id == y.FMA_Id && a.ASMAY_Id == pgmod.ASMAY_Id && b.FMH_Flag == "F" && c.FYP_Id == pgmod.FYP_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FMA_Id = a.FMA_Id,
                                         }
                          ).Distinct().Take(1);

                        if (fineheads.Count() > 0)
                        {
                            //tpayment
                            var sta_tpayment = _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Single(t => t.FMA_Id == y.FMA_Id && t.FYP_Id == pgmod.FYP_Id);

                            //status
                            var sta_status = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FMA_Id == y.FMA_Id && t.AMST_Id == pgmod.Amst_Id);

                            if (sta_tpayment.FTP_Paid_Amt > y.FSS_ToBePaid)
                            {
                                var updatedpaid = sta_tpayment.FTP_Paid_Amt - y.FSS_ToBePaid;

                                sta_status.FSS_PaidAmount = sta_status.FSS_PaidAmount - Convert.ToInt32(updatedpaid);

                                sta_status.FSS_FineAmount = sta_status.FSS_FineAmount - Convert.ToInt32(updatedpaid);
                            }
                            else if (sta_tpayment.FTP_Paid_Amt < y.FSS_ToBePaid)
                            {
                                var updatedpaid = y.FSS_ToBePaid - sta_tpayment.FTP_Paid_Amt;

                                sta_status.FSS_PaidAmount = sta_status.FSS_PaidAmount + Convert.ToInt32(updatedpaid);

                                sta_status.FSS_FineAmount = sta_status.FSS_FineAmount + Convert.ToInt32(updatedpaid);
                            }
                            else if (sta_tpayment.FTP_Paid_Amt == y.FSS_ToBePaid)
                            {
                                sta_status.FSS_PaidAmount = sta_status.FSS_PaidAmount;

                                sta_status.FSS_FineAmount = sta_status.FSS_FineAmount;
                            }

                            _YearlyFeeGroupMappingContext.Update(sta_status);
                            //status

                            //ystudent
                            var temppayment = _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO.Single(t => t.FYP_Id == pgmod.FYP_Id && t.AMST_Id == pgmod.Amst_Id);

                            temppayment.FTP_TotalPaidAmount = pgmod.FYP_Tot_Amount;
                            _YearlyFeeGroupMappingContext.Update(temppayment);
                            //ystudent

                            sta_tpayment.FTP_Paid_Amt = y.FSS_ToBePaid;
                            //tpayment

                            _YearlyFeeGroupMappingContext.Update(sta_tpayment);
                        }

                    }
                    //kiran

                    var contactexisttransaction = 0;
                    using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                            dbCtxTxn.Commit();
                            pgmod.returnval = "true";
                            pgmod.displaymessage = "Updated";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            dbCtxTxn.Rollback();
                            pgmod.returnval = "false";
                            pgmod.displaymessage = "Not Updated";
                        }
                    }
                }
                else if (pgmod.filterinitialdata != "Preadmission")
                {
                    int j = 0;
                    string list_values = "";
                    FeePaymentDetailsDMO pmm = new FeePaymentDetailsDMO();

                    if (pgmod.savetmpdata != null)
                    {
                        pmm.MI_Id = pgmod.MI_Id;
                        pmm.ASMAY_ID = pgmod.ASMAY_Id;
                        pmm.user_id = pgmod.userid;
                        pgmod.temp_head_list = pgmod.savetmpdata;

                        pmm.FYP_Date = Convert.ToDateTime(pgmod.FYPcurr_Date);

                        //multimode

                        foreach (var mode in pgmod.Modes)
                        {
                            if (mode.FYPPM_TotalPaidAmount > 0)
                            {
                                if (mode.FYPPM_TransactionTypeFlag == "C")
                                {
                                    pmm.FYP_Bank_Name = "";
                                    pmm.FYP_DD_Cheque_Date = dt;
                                    pmm.FYP_DD_Cheque_No = "";
                                    pmm.FYP_Bank_Or_Cash = mode.FYPPM_TransactionTypeFlag;
                                }
                                else if (mode.FYPPM_TransactionTypeFlag == "B" || mode.FYPPM_TransactionTypeFlag == "E" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R")
                                {
                                    pmm.FYP_Bank_Name = mode.FYPPM_BankName;
                                    pmm.FYP_DD_Cheque_Date = mode.FYPPM_DDChequeDate;
                                    pmm.FYP_DD_Cheque_No = mode.FYPPM_DDChequeNo;
                                    pmm.FYP_Bank_Or_Cash = mode.IVRMMOD_ModeOfPayment_Code;
                                }
                            }
                        }

                        pmm.FYP_Chq_Bounce = pgmod.FYP_Chq_Bounce;
                        pmm.FYP_Remarks = pgmod.FYP_Remarks;
                        pmm.FTCU_Id = 1;
                        pmm.FYP_Tot_Amount = pgmod.FYP_Tot_Amount;
                        pmm.FYP_Tot_Concession_Amt = pgmod.FYP_Tot_Concession_Amt;
                        pmm.FYP_Tot_Fine_Amt = pgmod.FYP_Tot_Fine_Amt;
                        pmm.FYP_Tot_Waived_Amt = pgmod.FYP_Tot_Waived_Amt;
                        pmm.DOE = dt;
                        pmm.CreatedDate = dt;
                        pmm.UpdatedDate = dt;
                        pmm.FYP_OnlineChallanStatusFlag = "Sucessfull";
                        pmm.FYP_PayModeType = "APP";
                        pmm.FYP_PayGatewayType = "";
                    }

                    _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Add(pmm);

                    //Multimode of Payment

                    pmm.FYP_DeviseFlg = pgmod.FYP_PayModeType;

                    foreach (var mode in pgmod.Modes)
                    {
                        if (mode.FYPPM_TotalPaidAmount > 0)
                        {
                            Fee_Y_Payment_PaymentModeSchool obj2 = new Fee_Y_Payment_PaymentModeSchool();
                            obj2.FYP_Id = pmm.FYP_Id;

                            obj2.FYP_TransactionTypeFlag = mode.IVRMMOD_ModeOfPayment_Code;

                            obj2.FYPPM_TotalPaidAmount = mode.FYPPM_TotalPaidAmount;
                            obj2.FYPPM_LedgerId = 0;
                            obj2.FYPPM_BankName = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_BankName;
                            obj2.FYPPM_DDChequeNo = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_DDChequeNo;
                            obj2.FYPPM_DDChequeDate = mode.FYPPM_TransactionTypeFlag == "C" ? pgmod.FYP_Date : mode.FYPPM_DDChequeDate;
                            obj2.FYPPM_TransactionId = "";
                            obj2.FYPPM_PaymentReferenceId = "";
                            obj2.FYPPM_ClearanceStatusFlag = "0";
                            obj2.FYPPM_ClearanceDate = mode.FYPPM_DDChequeDate;
                            _YearlyFeeGroupMappingContext.Add(obj2);
                        }
                    }

                    //Multimode of Payment


                    lock (obj)
                    {
                        get_grp_reptno(pgmod);

                        pmm.FYP_Receipt_No = pgmod.FYP_Receipt_No;

                        if (pmm.FYP_Receipt_No == "" || pmm.FYP_Receipt_No == null)
                        {
                            pgmod.returnval = "Record Not Saved because Receipt No is not Generated Automatic.Settings are missing";
                            return pgmod;
                        }
                        else
                        {

                            Fee_Y_Payment_School_StudentDMO temppayment = new Fee_Y_Payment_School_StudentDMO();

                            temppayment.AMST_Id = pgmod.Amst_Id;
                            temppayment.ASMAY_Id = pmm.ASMAY_ID;
                            temppayment.FTP_TotalPaidAmount = pgmod.FYP_Tot_Amount;
                            temppayment.FTP_TotalWaivedAmount = pgmod.FYP_Tot_Waived_Amt;
                            temppayment.FTP_TotalConcessionAmount = pgmod.FYP_Tot_Concession_Amt;
                            temppayment.FTP_TotalFineAmount = pgmod.FYP_Tot_Fine_Amt;
                            temppayment.FYP_Id = pmm.FYP_Id;
                            _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO.Add(temppayment);

                            for (var i = 0; i <= pgmod.savetmpdata.Length; i++)
                            {
                                for (var l = 0; l < pgmod.savetmpdata.Length - 1; l++)
                                {
                                    if (pgmod.savetmpdata[l].FSS_ToBePaid > pgmod.savetmpdata[l + 1].FSS_ToBePaid)
                                    {
                                        var temp = pgmod.savetmpdata[l];
                                        pgmod.savetmpdata[l] = pgmod.savetmpdata[l + 1];
                                        pgmod.savetmpdata[l + 1] = temp;
                                    }
                                }
                            }

                            //  while (j < pgmod.savetmpdata.Count())
                            for (j = 0; j < pgmod.savetmpdata.Length; j++)
                            {
                                FeeTransactionPaymentDMO feetrapay = new FeeTransactionPaymentDMO();

                                if (pgmod.savetmpdata[j].FSS_ToBePaid > 0 || pgmod.savetmpdata[j].FSS_OBArrearAmount > 0)
                                {
                                    if (j == pgmod.savetmpdata.Length - 1)
                                    {
                                        feetrapay.FTP_Id = 0;
                                        feetrapay.FMA_Id = pgmod.savetmpdata[j].FMA_Id;
                                        feetrapay.FTP_Paid_Amt = pgmod.savetmpdata[j].FSS_ToBePaid - pgmod.FSS_RebateAmount;
                                        feetrapay.FTP_Concession_Amt = pgmod.savetmpdata[j].FSS_ConcessionAmount;
                                        feetrapay.FTP_Fine_Amt = pgmod.savetmpdata[j].FSS_FineAmount;
                                        feetrapay.FTP_Waived_Amt = pgmod.savetmpdata[j].FSS_WaivedAmount;
                                        feetrapay.ftp_remarks = pgmod.FYP_Remarks;
                                        feetrapay.FTP_RebateAmount = pgmod.FSS_RebateAmount;
                                        feetrapay.FTP_CreatedBy = pgmod.userid;
                                        feetrapay.FTP_UpdatedBy = pgmod.userid;
                                        feetrapay.FTP_CreatedDate = DateTime.Now;
                                        feetrapay.FTP_UpdatedDate = DateTime.Now;


                                        feetrapay.FYP_Id = pmm.FYP_Id;

                                        list_values = list_values + "(" + pmm.MI_Id + "," + feetrapay.FYP_Id + "," + feetrapay.FMA_Id + "," + feetrapay.FTP_Paid_Amt + "," + feetrapay.FTP_Fine_Amt + "," + feetrapay.FTP_Concession_Amt + "," + feetrapay.FTP_Waived_Amt + "),";

                                        _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Add(feetrapay);

                                        var obj_status_stf = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.AMST_Id == pgmod.Amst_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id && t.FMG_Id == pgmod.savetmpdata[j].FMG_Id && t.FMA_Id == pgmod.savetmpdata[j].FMA_Id && t.FSS_ActiveFlag == true).FirstOrDefault();

                                        obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + pgmod.savetmpdata[j].FSS_ToBePaid - pgmod.FSS_RebateAmount;

                                        //added on 11-07-2018
                                        var fineheadss = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                          from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                          where (a.MI_Id == pgmod.MI_Id && a.FMA_Id == pgmod.savetmpdata[j].FMA_Id && a.ASMAY_Id == pgmod.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == pgmod.Amst_Id && a.FMH_Id == b.FMH_Id)
                                                          select new FeeStudentTransactionDTO
                                                          {
                                                              FMH_Id = a.FMH_Id,
                                                          }
                                           ).Distinct().Take(1);

                                        if (fineheadss.Count() > 0)
                                        {
                                            obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + pgmod.savetmpdata[j].FSS_ToBePaid;
                                        }
                                        //added on 11-07-2018


                                        if (obj_status_stf.FSS_NetAmount != 0 || obj_status_stf.FSS_OBArrearAmount != 0)
                                        {
                                            obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - pgmod.FSS_RebateAmount;
                                            pgmod.savetmpdata[j].FSS_ToBePaid = pgmod.savetmpdata[j].FSS_ToBePaid - pgmod.FSS_RebateAmount;
                                            obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - pgmod.savetmpdata[j].FSS_ToBePaid;
                                        }
                                        else
                                        {
                                            obj_status_stf.FSS_ToBePaid = 0;
                                        }
                                        obj_status_stf.FSS_RebateAmount = pgmod.FSS_RebateAmount;
                                        _YearlyFeeGroupMappingContext.Update(obj_status_stf);



                                        FeeStudentRebate feestureb = new FeeStudentRebate();

                                        feestureb.FSREB_Id = 0;
                                        feestureb.FMA_Id = pgmod.savetmpdata[j].FMA_Id;
                                        feestureb.MI_Id = pgmod.MI_Id;
                                        feestureb.AMST_Id = pgmod.Amst_Id;
                                        feestureb.ASMAY_Id = pgmod.ASMAY_Id;
                                        feestureb.FSREB_Date = pgmod.FYP_Date;
                                        feestureb.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                        feestureb.FMG_Id = pgmod.savetmpdata[j].FMG_Id;
                                        feestureb.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                        feestureb.FSREB_RebateAmount = pgmod.FSS_RebateAmount;
                                        feestureb.FSREB_ActiveFlag = true;
                                        feestureb.FSREB_Remarks = pgmod.FYP_Remarks;



                                        feestureb.FYP_Id = pmm.FYP_Id;

                                        _YearlyFeeGroupMappingContext.Add(feestureb);

                                    }
                                    else
                                    {
                                        feetrapay.FTP_Id = 0;
                                        feetrapay.FMA_Id = pgmod.savetmpdata[j].FMA_Id;
                                        feetrapay.FTP_Paid_Amt = pgmod.savetmpdata[j].FSS_ToBePaid;
                                        feetrapay.FTP_Concession_Amt = pgmod.savetmpdata[j].FSS_ConcessionAmount;
                                        feetrapay.FTP_Fine_Amt = pgmod.savetmpdata[j].FSS_FineAmount;
                                        feetrapay.FTP_Waived_Amt = pgmod.savetmpdata[j].FSS_WaivedAmount;
                                        feetrapay.ftp_remarks = pgmod.FYP_Remarks;

                                        feetrapay.FYP_Id = pmm.FYP_Id;

                                        list_values = list_values + "(" + pmm.MI_Id + "," + feetrapay.FYP_Id + "," + feetrapay.FMA_Id + "," + feetrapay.FTP_Paid_Amt + "," + feetrapay.FTP_Fine_Amt + "," + feetrapay.FTP_Concession_Amt + "," + feetrapay.FTP_Waived_Amt + "),";

                                        _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Add(feetrapay);

                                        var obj_status_stf = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.AMST_Id == pgmod.Amst_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id && t.FMG_Id == pgmod.savetmpdata[j].FMG_Id && t.FMA_Id == pgmod.savetmpdata[j].FMA_Id && t.FSS_ActiveFlag == true).FirstOrDefault();

                                        obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + pgmod.savetmpdata[j].FSS_ToBePaid;

                                        //added on 11-07-2018
                                        var fineheadss = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                          from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                          where (a.MI_Id == pgmod.MI_Id && a.FMA_Id == pgmod.savetmpdata[j].FMA_Id && a.ASMAY_Id == pgmod.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == pgmod.Amst_Id && a.FMH_Id == b.FMH_Id)
                                                          select new FeeStudentTransactionDTO
                                                          {
                                                              FMH_Id = a.FMH_Id,
                                                          }
                                           ).Distinct().Take(1);

                                        if (fineheadss.Count() > 0)
                                        {
                                            obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + pgmod.savetmpdata[j].FSS_ToBePaid;
                                        }
                                        //added on 11-07-2018


                                        if (obj_status_stf.FSS_NetAmount != 0 || obj_status_stf.FSS_OBArrearAmount != 0)
                                        {
                                            obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - pgmod.savetmpdata[j].FSS_ToBePaid;
                                        }
                                        else
                                        {
                                            obj_status_stf.FSS_ToBePaid = 0;
                                        }

                                        _YearlyFeeGroupMappingContext.Update(obj_status_stf);

                                    }





                                }


                            }

                            var contactexisttransaction = 0;
                            using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                                    dbCtxTxn.Commit();

                                    pgmod.returnval = "true";
                                    pgmod.displaymessage = "Saved";
                                    pgmod.FYP_Id = pmm.FYP_Id;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    dbCtxTxn.Rollback();
                                    pgmod.returnval = "false";
                                    pgmod.displaymessage = "Not Saved";
                                }
                            }
                        }
                    }


                    _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("pda_status_update @p0,@p1,@p2,@p3", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.Amst_Id, pgmod.FYP_Tot_Amount);




                    long mobileno = 0;
                    string MailId = "";

                    var getdetails = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                      where (a.AMST_Id == pgmod.Amst_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          amst_mobile = a.AMST_MobileNo,
                                          amst_email_id = a.AMST_emailId,
                                          ASMAY_Id = a.ASMAY_Id
                                      }
            ).ToList();

                    mobileno = getdetails.FirstOrDefault().amst_mobile;
                    MailId = getdetails.FirstOrDefault().amst_email_id;

                    var getdetailstemplate = (from a in _YearlyFeeGroupMappingContext.sMSEmailSetting
                                              where (a.ISES_Template_Name == "FeeAdmissionTransaction" && a.MI_Id == pgmod.MI_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  institutionname = a.ISES_Template_Name
                                              }
          ).ToList();

                    if (getdetailstemplate.Count > 0)
                    {
                        long yearid = getacademicyearcongig(pgmod);

                        if (getdetails.FirstOrDefault().ASMAY_Id == yearid)
                        {

                            var noofreceipts = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                                from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                where (a.FYP_Id == b.FYP_Id && b.FMA_Id == c.FMA_Id && c.FMG_Id == d.FMG_Id && c.MI_Id == pgmod.MI_Id && c.ASMAY_Id == pgmod.ASMAY_Id && a.AMST_Id == pgmod.Amst_Id && d.FMG_CompulsoryFlag != "R")
                                                select new FeeStudentTransactionDTO
                                                {
                                                    FYP_Id = b.FYP_Id
                                                }
         ).Distinct().ToList();
                            if (noofreceipts.Count == 1)
                            {
                                if (pgmod.smsconfig == "true")
                                {
                                    SMS sms = new SMS(_context);

                                    string s = await sms.sendSms(pgmod.MI_Id, mobileno, "FeeAdmissionTransaction", pgmod.Amst_Id);
                                }

                                if (pgmod.emailconfig == "true")
                                {
                                    Email Email = new Email(_context);

                                    string m = Email.sendmail(pgmod.MI_Id, MailId, "FeeAdmissionTransaction", pgmod.Amst_Id);
                                }
                            }
                            else
                            {
                                if (pgmod.smsconfig == "true")
                                {
                                    SMS sms = new SMS(_context);

                                    string s = await sms.sendSms(pgmod.MI_Id, mobileno, "FeeTransaction", pgmod.Amst_Id);
                                }

                                if (pgmod.emailconfig == "true")
                                {
                                    Email Email = new Email(_context);

                                    string m = Email.sendmail(pgmod.MI_Id, MailId, "FeeTransaction", pgmod.Amst_Id);
                                }
                            }

                        }

                        else
                        {
                            if (pgmod.smsconfig == "true")
                            {
                                SMS sms = new SMS(_context);

                                string s = await sms.sendSms(pgmod.MI_Id, mobileno, "FeeTransaction", pgmod.Amst_Id);
                            }

                            if (pgmod.emailconfig == "true")
                            {
                                Email Email = new Email(_context);

                                string m = Email.sendmail(pgmod.MI_Id, MailId, "FeeTransaction", pgmod.Amst_Id);
                            }

                        }

                    }
                    else
                    {
                        if (pgmod.smsconfig == "true")
                        {
                            SMS sms = new SMS(_context);

                            string s = await sms.sendSms(pgmod.MI_Id, mobileno, "FeeTransaction", pgmod.Amst_Id);
                        }
                        if (pgmod.emailconfig == "true")
                        {
                            Email Email = new Email(_context);

                            string m = Email.sendmail(pgmod.MI_Id, MailId, "FeeTransaction", pgmod.Amst_Id);
                        }
                    }
                }

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == pgmod.MI_Id && t.userid == pgmod.userid).ToList();
                pgmod.feeconfiglist = feemasnum.ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id).ToList();
                pgmod.fillyear = year.ToArray();


                var fetchmaxfypid = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_ID == pgmod.ASMAY_Id).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                pgmod.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                           from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                           from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                           from e in _YearlyFeeGroupMappingContext.admissioncls
                                           from f in _YearlyFeeGroupMappingContext.school_M_Section
                                           where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_ID == pgmod.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               Amst_Id = c.AMST_Id,
                                               AMST_FirstName = c.AMST_FirstName,
                                               AMST_MiddleName = c.AMST_MiddleName,
                                               AMST_LastName = c.AMST_LastName,
                                               FYP_Receipt_No = a.FYP_Receipt_No,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_Tot_Amount = a.FYP_Tot_Amount,
                                               classname = e.ASMCL_ClassName,
                                               sectionname = f.ASMC_SectionName,
                                               FYP_Id = a.FYP_Id,
                                               AMST_AdmNo = c.AMST_AdmNo,
                                               FYP_Date = a.FYP_Date,
                                               amst_mobile = c.AMST_MobileNo,
                                               FYP_Chq_Bounce = a.FYP_Chq_Bounce
                                           }
     ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

            }

            catch (Exception ee)
            {
                pgmod.returnval = "false";
                pgmod.displaymessage = "Not Saved";
                Console.WriteLine(ee.Message);
                _logger.LogError("Error in " + pgmod.MI_Id + " - " + ee.InnerException);
            }

            return pgmod;
        }

        //rebate condition

        public FeeStudentTransactionDTO Readminssioninsertionfees(FeeStudentTransactionDTO data)
        {
            try
            {

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ReadmissionFeeInsertStthomas";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
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
                        data.translogresults = retObject1.ToArray();
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

    }
}














