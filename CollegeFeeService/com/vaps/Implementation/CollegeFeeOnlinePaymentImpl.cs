using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using paytm.security;
using paytm.util;
using paytm.exception;
using Razorpay.Api;
using Payment = CommonLibrary.Payment;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using easebuzz_.net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using CollegeFeeService.com.vaps.interfaces;
using CollegeFeeService.com.vaps.Interfaces;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeFeeOnlinePaymentImpl : CollegeFeeOnlinePaymentInterface
    {
        private static ConcurrentDictionary<string, CollegeFeeTransactionDTO> MsCadm = new ConcurrentDictionary<string, CollegeFeeTransactionDTO>();

        public CollFeeGroupContext _mastercourse;
        public DomainModelMsSqlServerContext _context;
        public CollegeFeeOnlinePaymentImpl(CollFeeGroupContext mscadm, DomainModelMsSqlServerContext context)
        {
            _mastercourse = mscadm;
            _context = context;
        }

        public CollegeFeeTransactionDTO pageload(CollegeFeeTransactionDTO data)
        {
            try
            {
                data.ASMAY_Id = getacademicyearcongig(data);

                data.ASMAY_Year = _mastercourse.AcademicYear.Where(t => t.ASMAY_Id == data.ASMAY_Id).Select(t => t.ASMAY_Year).FirstOrDefault();
                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _mastercourse.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                data.studwisepartialpayment = _mastercourse.Fee_Student_EnablePartialPayment_CollegeDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id && t.ASMAY_Id == data.ASMAY_Id).ToArray();


                data.fillgroup = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                  from b in _mastercourse.FeeGroupClgDMO
                                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id)
                                  select new CollegeFeeTransactionDTO
                                  {
                                      FMG_GroupName = b.FMG_GroupName,
                                      FMG_Id = a.FMG_Id,
                                      FMG_CompulsoryFlag=b.FMG_CompulsoryFlag,
                                  }
     ).Distinct().ToArray();

                var groupid = (from a in _mastercourse.Fee_College_Student_StatusDMO
                               from b in _mastercourse.FeeGroupClgDMO
                               where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id)
                               select new CollegeFeeTransactionDTO
                               {

                                   FMG_Id = a.FMG_Id,
                               }
         ).Distinct().ToList();

                List<long> FMG_Id = new List<long>();
                for (int s = 0; s < groupid.Count(); s++)
                {

                    FMG_Id.Add(groupid[s].FMG_Id);
                }

                data.fillinstallmentnew = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                           from b in _mastercourse.Clg_Fee_Installments_Yearly_DMO
                                           where (a.FTI_Id == b.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && FMG_Id.Contains(a.FMG_Id))
                                           select new CollegeFeeTransactionDTO
                                           {

                                               FMG_Id = a.FMG_Id,
                                               FTI_Id = a.FTI_Id,
                                               FTI_Name = b.FTI_Name,
                                           }
         ).Distinct().ToArray();


                data.filonlinepaymentgrid = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                             where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id)
                                             select new CollegeFeeTransactionDTO
                                             {
                                                 FCSS_NetAmount = a.FCSS_NetAmount,
                                                 FCSS_ConcessionAmount = a.FCSS_ConcessionAmount,
                                                 FCSS_FineAmount = a.FCSS_FineAmount,
                                                 FCSS_ToBePaid = a.FCSS_ToBePaid,
                                                 FCSS_PaidAmount = a.FCSS_PaidAmount,
                                                 AMCST_Id = a.AMCST_Id
                                             }
).ToArray();


                data.fillstudent = (from a in _mastercourse.Adm_Master_College_StudentDMO
                                    from b in _mastercourse.Adm_College_Yearly_StudentDMO
                                    from c in _mastercourse.MasterCourseDMO
                                    from d in _mastercourse.ClgMasterBranchDMO
                                    from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                                    from f in _mastercourse.Adm_College_Master_SectionDMO
                                    where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ACMS_Id == f.ACMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && b.ACYST_ActiveFlag == 1)
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
                                        AMCO_Id = c.AMCO_Id,
                                        AMB_Id = d.AMB_Id,
                                        AMSE_Id = e.AMSE_Id,
                                        ACMS_Id = f.ACMS_Id,
                                        AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                        AMCST_emailId = a.AMCST_emailId,
                                        AMCST_StudentPhoto = a.AMCST_StudentPhoto
                                    }
                  ).Distinct().ToArray();

                List<MOBILE_INSTITUTION> instidet = new List<MOBILE_INSTITUTION>();
                instidet = _mastercourse.MOBILE_INSTITUTION.Where(t => t.MI_ID == data.MI_Id).ToList();
                data.institutiondet = instidet.ToArray();

                data.fillpaymentgateway = (from a in _mastercourse.PAYUDETAILS
                                           from b in _mastercourse.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_PGActiveFlag == "1")
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }
).Distinct().ToArray();




                if (data.configset.Equals("T"))
                {
                    var readterms = (from a in _mastercourse.feeTr
                                     where (a.MI_Id == data.MI_Id)
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FMT_Name = a.FMT_Name,
                                         FMT_Id = a.FMT_Id,
                                     }
    ).Distinct().ToArray();

                    string alltrids = "0";
                    List<long> FMT_Ids = new List<long>();
                    for (int s = 0; s < readterms.Count(); s++)
                    {
                        alltrids = alltrids + ',' + readterms[s].FMT_Id.ToString();
                        FMT_Ids.Add(readterms[s].FMT_Id);
                    }
                    data.disableterms = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                         from b in _mastercourse.feeMTH
                                         where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.AMCST_Id == data.AMCST_Id && a.User_Id == data.UserId && FMT_Ids.Contains(b.FMT_Id) && a.ASMAY_Id == data.ASMAY_Id)
                                         group new { a, b } by new { b.FMT_Id } into g
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FCSS_NetAmount = g.Sum(t => t.a.FCSS_NetAmount) + g.Sum(t => t.a.FCSS_OBArrearAmount),
                                             FCSS_ToBePaid = g.Sum(t => t.a.FCSS_ToBePaid) + g.Sum(t => t.a.FCSS_OBArrearAmount),
                                             FCSS_PaidAmount = g.Sum(t => t.a.FCSS_PaidAmount) + g.Sum(t => t.a.FCSS_ConcessionAmount),
                                             FMT_Id = g.Key.FMT_Id
                                         }).Distinct().ToArray();
                }
                else if (data.configset.Equals("G"))
                {
                    data.disableterms = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                         from b in _mastercourse.FeeGroupClgDMO
                                         from c in _mastercourse.Clg_Fee_Installments_Yearly_DMO
                                         where (a.AMCST_Id == data.AMCST_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && c.FTI_Id == a.FTI_Id)
                                         group a by new { a.FTI_Id, a.FMG_Id, b.FMG_Order, c.FTI_Name } into g
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FCSS_NetAmount = g.Sum(t => t.FCSS_NetAmount) + g.Sum(t => t.FCSS_OBArrearAmount),
                                             FCSS_ToBePaid = g.Sum(t => t.FCSS_ToBePaid),
                                             FCSS_PaidAmount = g.Sum(t => t.FCSS_PaidAmount) + g.Sum(t => t.FCSS_ConcessionAmount),
                                             FMT_Id = g.Key.FMG_Id,
                                             FMG_Order = g.Key.FMG_Order,
                                             FTI_Id = g.Key.FTI_Id,
                                             FTI_Name = g.Key.FTI_Name

                                         }).Distinct().OrderBy(t => t.FMG_Order).ToArray();
                }



            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public long getacademicyearcongig(CollegeFeeTransactionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                var currasmayid = (from a in _mastercourse.AcademicYear
                                   where (a.MI_Id == data.MI_Id && indianTime > a.ASMAY_From_Date && indianTime < a.ASMAY_To_Date)
                                   select new CollegeFeeTransactionDTO
                                   {
                                       ASMAY_Id = a.ASMAY_Id,
                                       ASMAY_Order = a.ASMAY_Order
                                   }
                                ).FirstOrDefault();

                data.ASMAY_Id = Convert.ToInt32(currasmayid.ASMAY_Id);

                var readterms = (from a in _mastercourse.feemastersettings
                                 where (a.MI_Id == data.MI_Id)
                                 select new CollegeFeeTransactionDTO
                                 {
                                     FMC_Online_Payment_Aca_Yr_Flag = a.FMC_Online_Payment_Aca_Yr_Flag,
                                 }
                                 ).FirstOrDefault();

                if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "C")
                {
                    data.ASMAY_Id = data.ASMAY_Id;

                    var studentaca = _mastercourse.Adm_College_Yearly_StudentDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.AMCST_Id).ToList();

                    if (studentaca.Count == 0)
                    {
                        var currasmayidnew = (from a in _mastercourse.AcademicYear
                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Order == Convert.ToInt32(currasmayid.ASMAY_Order + 1))
                                              select new CollegeFeeTransactionDTO
                                              {
                                                  ASMAY_Id = a.ASMAY_Id,
                                                  ASMAY_Order = a.ASMAY_Order
                                              }
                             ).FirstOrDefault();

                        data.ASMAY_Id = Convert.ToInt32(currasmayidnew.ASMAY_Id);
                    }



                }
                else if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "N")
                {
                    var newASMAY_Id = (from a in _mastercourse.AcademicYear
                                       where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                       select new CollegeFeeTransactionDTO
                                       {
                                           ASMAY_Order = a.ASMAY_Order,
                                       }
                                 ).FirstOrDefault();
                    var asmayorder = Convert.ToInt64(newASMAY_Id.ASMAY_Order) + 1;

                    var nextASMAY_Id = (from a in _mastercourse.AcademicYear
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Order == asmayorder)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            ASMAY_Id = a.ASMAY_Id,
                                        }
                                 ).FirstOrDefault();

                    data.ASMAY_Id = nextASMAY_Id.ASMAY_Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.ASMAY_Id;
        }

        public long getcurrentclass(CollegeFeeTransactionDTO data)
        {
            try
            {
                var readterms = (from a in _mastercourse.feemastersettings
                                 where (a.MI_Id == data.MI_Id)
                                 select new CollegeFeeTransactionDTO
                                 {
                                     FMC_Online_Payment_Aca_Yr_Flag = a.FMC_Online_Payment_Aca_Yr_Flag,
                                 }
                                 ).FirstOrDefault();

                if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "N")
                {
                    var newclassorder = (from a in _mastercourse.CLG_Adm_Master_SemesterDMO
                                         where (a.MI_Id == data.MI_Id && a.AMSE_Id == data.AMSE_Id)
                                         select new CollegeFeeTransactionDTO
                                         {
                                             AMSE_SEMOrder = a.AMSE_SEMOrder
                                         }
                                 ).FirstOrDefault();
                    var clasorder = Convert.ToInt64(newclassorder.AMSE_SEMOrder) + 1;

                    var nextasmclid = (from a in _mastercourse.CLG_Adm_Master_SemesterDMO
                                       where (a.MI_Id == data.MI_Id && a.AMSE_SEMOrder == clasorder)
                                       select new CollegeFeeTransactionDTO
                                       {
                                           AMSE_Id = a.AMSE_Id,
                                       }
                                 ).FirstOrDefault();

                    data.AMSE_Id = nextasmclid.AMSE_Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.AMSE_Id;
        }

        public CollegeFeeTransactionDTO getheaddetails(CollegeFeeTransactionDTO data)
        {
            try
            {

                long yrid = getacademicyearcongig(data);

                data.ASMAY_Id = yrid; 

                var feemasnum = _mastercourse.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                data.studwisepartialpayment = _mastercourse.Fee_Student_EnablePartialPayment_CollegeDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id && t.ASMAY_Id == data.ASMAY_Id).ToArray();



                List<CollegeFeeTransactionDTO> V_StudentPendinglist = new List<CollegeFeeTransactionDTO>();
                var retObject = new List<dynamic>();
                var retObject1 = new List<dynamic>();


                for (int l = 0; l < data.selected_listgroup.Length; l++)
                {

                    string instid = "0";
                    foreach (var x in data.selected_listgroup[l].trm_list)
                    {

                        instid = instid + "," + x.FTI_Id;
                    }
                    using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_Student_Status_Details_Online_Installmentwise";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar, 10)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                     SqlDbType.VarChar, 10)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                            SqlDbType.VarChar, 10)
                        {
                            Value = data.AMCST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                            SqlDbType.VarChar, 100)
                        {
                            Value = data.selected_listgroup[l].grp.FMG_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                           SqlDbType.VarChar, 100)
                        {
                            Value = instid
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        //var retObject = new List<dynamic>();
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
                            data.alldata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            //_logger.LogError(ex.Message);
                            //_logger.LogDebug(ex.Message);
                        }
                    }

                    using (var cmd1 = _mastercourse.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "Get_Student_Status_AdvanceDetailsOnline_Installmentwise";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar, 10)
                        {
                            Value = data.MI_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                     SqlDbType.VarChar, 10)
                        {
                            Value = data.ASMAY_Id
                        });

                        //cmd1.Parameters.Add(new SqlParameter("@User_Id",
                        //    SqlDbType.VarChar, 10)
                        //{
                        //    Value = data.UserId
                        //});

                        cmd1.Parameters.Add(new SqlParameter("@AMCST_Id",
                            SqlDbType.VarChar, 10)
                        {
                            Value = data.AMCST_Id
                        });
                        cmd1.Parameters.Add(new SqlParameter("@FMG_Id",
                             SqlDbType.VarChar, 100)
                        {
                            Value = data.selected_listgroup[l].grp.FMG_Id
                        });
                        cmd1.Parameters.Add(new SqlParameter("@FTI_Id",
                           SqlDbType.VarChar, 100)
                        {
                            Value = instid
                        });
                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();
                        try

                        {

                            using (var dataReader = cmd1.ExecuteReader())
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

                                    retObject1.Add((ExpandoObject)dataRow1);
                                }

                            }
                            data.advancefee = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            //_logger.LogError(ex.Message);
                            // _logger.LogDebug(ex.Message);
                        }
                    }
                }
                var fine_amount = 0;
                var fine_amountadv = 0;
                if (data.alldata.Length > 0)
                {
                    var count_res = retObject.Select(r => r.FMG_Id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new CollegeFeeTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

                    data.validationgroupid = count_res.FMG_Id;
                    data.validationgrougidcount = count_res.grp_count;
                    //MB

                    List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                    List<CollegeFeeTransactionDTO> fines_fma_idsadvan = new List<CollegeFeeTransactionDTO>();
                    foreach (var x in retObject)
                    {
                        CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                        using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Sp_Calculate_Fine_CLG";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime)
                            {
                                Value = data.FYP_ReceiptDate
                            });

                            cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                               SqlDbType.BigInt)
                            {
                                Value = x.FCMAS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                         SqlDbType.BigInt)
                            {
                                Value = data.AMCST_Id
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

                            sew.FCMAS_Id = x.FCMAS_Id;
                            sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            fines_fma_ids.Add(sew);
                        }
                    }

                    data.Fine_FCMAS_Ids = fines_fma_ids.Distinct().ToArray();
                    foreach (var x in retObject1)
                    {
                        CollegeFeeTransactionDTO sew1 = new CollegeFeeTransactionDTO();
                        using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Sp_Calculate_Fine_CLG";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime)
                            {
                                Value = data.FYP_ReceiptDate
                            });

                            cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                               SqlDbType.BigInt)
                            {
                                Value = x.FCMAS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.AMCST_Id
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
                            fine_amountadv += Convert.ToInt32(cmd.Parameters["@amt"].Value);

                            sew1.FCMAS_Id = x.FCMAS_Id;
                            sew1.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            fines_fma_idsadvan.Add(sew1);

                        }
                    }
                    data.Fine_FCMAS_IdsAdvance = fines_fma_idsadvan.Distinct().ToArray();

                    List<CollegeFeeTransactionDTO> finelst = new List<CollegeFeeTransactionDTO>();
                    List<CollegeFeeTransactionDTO> finelstadv = new List<CollegeFeeTransactionDTO>();

                    List<clgfeefinefmaDTO> fineheadfmaid = new List<clgfeefinefmaDTO>();
                    List<clgfeefinefmaDTO> fineheadfmaidall = new List<clgfeefinefmaDTO>();

                    var myArray = data.groupfmgidss.Split(',');
                    List<long> terms_groups = new List<long>();
                    for (int i = 0; i < myArray.Length; i++)
                    {
                        terms_groups.Add(Convert.ToInt64(myArray[i]));
                    }

                    fineheadfmaid = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _mastercourse.Fee_College_Student_StatusDMO
                                     from c in _mastercourse.FeeHeadClgDMO
                                     from d in _mastercourse.Clg_Fee_Installments_Yearly_DMO
                                     from e in _mastercourse.CLG_Fee_College_Master_Amount_Semesterwise
                                     where (b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.FCMAS_Id == e.FCMAS_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMCST_Id == data.AMCST_Id && c.FMH_Flag == "F" && terms_groups.Contains(a.fmg_id))
                                     select new clgfeefinefmaDTO
                                     {
                                         FCMAS_Id = b.FCMAS_Id,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FMH_Id = b.FMH_Id,
                                         FMG_Id = b.FMG_Id,
                                         FTI_Id = b.FTI_Id,
                                         FTI_Name = d.FTI_Name,
                                         FCMAS_DueDate = e.FCMAS_DueDate
                                     }
                  ).Distinct().ToList();

                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }

                    if (Convert.ToInt32(fine_amount) > 0)
                    {
                        if (data.Fine_FCMAS_Ids.Length > 0)
                        {
                            finelst.Add(new CollegeFeeTransactionDTO
                            {
                                FCMAS_Id = fineheadfmaid[0].FCMAS_Id,
                                FCSS_ToBePaid = Convert.ToInt32(fine_amount),
                                FCSS_FineAmount = 0,
                                FCSS_ConcessionAmount = 0,
                                FCSS_WaivedAmount = 0,
                                FMG_Id = fineheadfmaid[0].FMG_Id,
                                FMH_Id = fineheadfmaid[0].FMH_Id,
                                FTI_Id = fineheadfmaid[0].FTI_Id,
                                FCSS_PaidAmount = 0,
                                FCSS_NetAmount = 0,
                                FCSS_RefundAmount = 0,
                                FMH_FeeName = fineheadfmaid[0].FMH_FeeName,
                                FTI_Name = fineheadfmaid[0].FTI_Name,
                                FCSS_CurrentYrCharges = 0,
                                FCSS_TotalCharges = 0,
                                FCMAS_DueDate = fineheadfmaid[0].FCMAS_DueDate,
                            });
                        }
                    }
                    data.finearray = finelst.ToArray();  //MB
                }

                data.fillpaymentgateway = (from a in _mastercourse.PAYUDETAILS
                                           from b in _mastercourse.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_PGActiveFlag == "1")
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }
).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO generatehashsequence(CollegeFeeTransactionDTO data)
        {
            try
            {
                long yrid = getacademicyearcongig(data);

                data.ASMAY_Id = yrid;
                data.fillstudent = (from a in _mastercourse.Adm_Master_College_StudentDMO
                                    from b in _mastercourse.Adm_College_Yearly_StudentDMO
                                    from c in _mastercourse.MasterCourseDMO
                                    from d in _mastercourse.ClgMasterBranchDMO
                                    from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id)
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
                                        AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                        AMCST_emailId = a.AMCST_emailId,
                                    }
             ).ToArray();


                var config = _mastercourse.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();


                if (data.onlinepaygteway == "PAYU")
                {
                    data.paydet = paymentPart(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "BILLDESK")
                {
                    //data = paymentPartbilldesk(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "EBS")
                {
                    //data.paydet = paymentPartebs(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "PAYTM")
                {
                    data.paydet = paymentPartpaytm(data, data.topayamount);
                    //data.paydet = paymentPartpaytmmobile(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "RAZORPAY")
                {
                    data.paydet = paymentPartRazorPay(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "EASEBUZZ")
                {
                    if (config.Count > 0)
                    {

                        if (config[0].FMC_EnablePartialPaymentFlg == 1 || config[0].FMC_EnablePartialPaymentFlg == 2)
                        {
                            data = paymentPartEasebuzzPartial(data);
                        }

                        else
                        {
                            data = paymentPartEasebuzz(data);

                        }

                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Array paymentPartRazorPay(CollegeFeeTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                long yrid = getacademicyearcongig(enq);

                enq.ASMAY_Id = yrid;
                enq.grpidss = "0";
                string headidss = "0";

                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();
                foreach (var x in enq.selected_list)
                {
                    grpids.Add(x.FMG_Id);
                    headids.Add(x.FMH_Id);
                    enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                    headidss = headidss + "," + x.FMH_Id;
                }
                //adv
                enq.advgrpid = "0";
                string advheadid = "0";
                long tobepaid = 0;
                List<long> grpidadv = new List<long>();
                List<long> headidadv = new List<long>();
                foreach (var x in enq.advancedata)
                {
                    grpidadv.Add(x.FMG_Id);
                    headidadv.Add(x.FMH_Id);
                    enq.advgrpid = enq.advgrpid + "," + x.FMG_Id;
                    advheadid = advheadid + "," + x.FMH_Id;
                    tobepaid = tobepaid + x.FCSS_ToBePaid;
                }
                //adv

                string fpgdids = "";
                var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _mastercourse.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _mastercourse.Fee_College_Student_StatusDMO
                                     from c in _mastercourse.Fee_PaymentGateway_Details
                                     where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FCSS_ToBePaid = b.FCSS_ToBePaid,
                                     }
          ).Sum(t => t.FCSS_ToBePaid);





                //Fine Calculation

                var showstudetails = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                      from b in _mastercourse.Fee_College_Student_StatusDMO
                                      from c in _mastercourse.Fee_PaymentGateway_Details
                                      where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FCMAS_Id = b.FCMAS_Id,
                                      }
          ).ToList();


                List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                var fine_amount = 0;
                var retObject = new List<dynamic>();
                foreach (var x in showstudetails)
                {
                    CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                    using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sp_Calculate_Fine_CLG";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@On_Date",
                            SqlDbType.DateTime)
                        {
                            Value = indianTime
                        });

                        cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                           SqlDbType.BigInt)
                        {
                            Value = x.FCMAS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.BigInt)
                        {
                            Value = enq.AMCST_Id
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

                        sew.FCMAS_Id = x.FCMAS_Id;
                        sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                        fines_fma_ids.Add(sew);
                    }
                }

                //Fine Calculation

                //advance//

                //advance//

                int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    value = (((Convert.ToInt32(enq.pendingamount)) + fine_amount) + (Convert.ToInt32(tobepaid))).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }
                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                string orderId;

                //added

                //added
                Dictionary<string, object> input = new Dictionary<string, object>();

                input.Add("amount", totamount * 100); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", PaymentDetailsDto.trans_id);
                input.Add("payment_capture", 1);

                //input.Add("amount", 1 * 100);
                // input.Add("amount", totpayableamount * 100); // this amount should be same as transaction amount
                //input.Add("amount", totamount);
                //input.Add("currency", "INR");
                //input.Add("receipt", totamount);
                // input.Add("receipt", PaymentDetailsDto.trans_id);
                // input.Add("payment_capture", 1);
                //input.Add("receipt", PaymentDetailsDto.trans_id);
                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                RazorpayClient client = new RazorpayClient(key, secret);

                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();

                enq.trans_id = orderId;
                enq.merchantkey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                enq.FMA_Amount = totamount;
                // enq.FMA_Amount = totpayableamount;
                enq.splitpayinformation = payinfo;

                get_grp_reptno(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PaymentDetailsDto.PaymentDetailsList;
        }


        public Array paymentPart(CollegeFeeTransactionDTO enq, long totamount)
        {
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                long yrid = getacademicyearcongig(enq);

                enq.ASMAY_Id = yrid;
                enq.grpidss = "0";
                string headidss = "0";

                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();
                foreach (var x in enq.selected_list)
                {
                    grpids.Add(x.FMG_Id);
                    headids.Add(x.FMH_Id);
                    enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                    headidss = headidss + "," + x.FMH_Id;
                }

                string fpgdids = "";
                var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _mastercourse.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();



                enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _mastercourse.Fee_College_Student_StatusDMO
                                     from c in _mastercourse.Fee_PaymentGateway_Details
                                     where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FCSS_ToBePaid = b.FCSS_ToBePaid,
                                     }
          ).Sum(t => t.FCSS_ToBePaid);

                int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    value = (Convert.ToInt32(enq.pendingamount)).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                }

                PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
                enq.trans_id = PaymentDetailsDto.trans_id.ToString();

                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }
                var item = new
                {
                    paymentParts = result
                };

                Payment pay = new Payment(_context);
                string payinfo = JsonConvert.SerializeObject(item);
                PaymentDetailsDto.productinfo = payinfo;
                PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount.ToString());
                PaymentDetailsDto.firstname = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_FirstName + ' ' + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MiddleName + ' ' + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_LastName.Trim();
                PaymentDetailsDto.email = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim();
                PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().FPGD_SaltKey.Trim();
                PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().FPGD_URL.Trim();

                PaymentDetailsDto.phone = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo;
                PaymentDetailsDto.udf1 = enq.ASMAY_Id.ToString().Trim();
                PaymentDetailsDto.udf2 = Convert.ToString(enq.AMCST_Id);
                PaymentDetailsDto.udf3 = enq.MI_Id.ToString();

                PaymentDetailsDto.udf6 = enq.AMCO_Id.ToString();
                PaymentDetailsDto.udf4 = enq.grpidss.ToString().Trim();

                PaymentDetailsDto.udf5 = headidss.ToString();
                PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponse/";
                PaymentDetailsDto.status = "success";
                PaymentDetailsDto.service_provider = "payu_paisa";

                PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                get_grp_reptno(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PaymentDetailsDto.PaymentDetailsList;
        }

        public Array paymentPartpaytm(CollegeFeeTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                long yrid = getacademicyearcongig(enq);

                enq.ASMAY_Id = yrid;
                enq.grpidss = "0";
                string headidss = "0";

                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();
                foreach (var x in enq.selected_list)
                {
                    grpids.Add(x.FMG_Id);
                    headids.Add(x.FMH_Id);
                    enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                    headidss = headidss + "," + x.FMH_Id;
                }

                string fpgdids = "";
                var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _mastercourse.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _mastercourse.Fee_College_Student_StatusDMO
                                     from c in _mastercourse.Fee_PaymentGateway_Details
                                     where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FCSS_ToBePaid = b.FCSS_ToBePaid,
                                     }
          ).Sum(t => t.FCSS_ToBePaid);

                int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    value = (Convert.ToInt32(enq.pendingamount)).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                //if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                //{
                //GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                //enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                //enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                //PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                //PaymentDetailsDto.trans_id = "RegularOnlinePAYTM" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();
                //  }


                PaymentDetailsDto.trans_id = "RegularOnlinePAYTM" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                enq.trans_id = PaymentDetailsDto.trans_id;
                get_grp_reptno(enq);

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                PAYMENTPARAMDETAILS = (from a in _mastercourse.PAYUDETAILS
                                       where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                       select new FeeStudentTransactionDTO
                                       {
                                           IMPG_IndustryType = a.IMPG_IndustryType,
                                           IMPG_Website = a.IMPG_Website,
                                           //IMPG_CallBackURL = a.IMPG_CallBackURL
                                       }
           ).ToList();

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
                enq.trans_id = PaymentDetailsDto.trans_id.ToString();

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }
                var item = new
                {
                    paymentParts = result
                };

                PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
                parameters.Add("ORDER_ID", PaymentDetailsDto.trans_id);
                parameters.Add("CUST_ID", enq.MI_Id.ToString());
                parameters.Add("TXN_AMOUNT", totpayableamount.ToString());
                parameters.Add("CHANNEL_ID", "WEB");

                //for production
                parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
                parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);

                //for test
                //parameters.Add("INDUSTRY_TYPE_ID", "Retail");
                //parameters.Add("WEBSITE", "WEB");

                parameters.Add("MOBILE_NO", ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo.ToString());
                parameters.Add("EMAIL", ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim());
                parameters.Add("MERC_UNQ_REF", enq.ASMAY_Id.ToString().Trim() + "_" + enq.AMCST_Id + "_" + enq.MI_Id.ToString().Trim() + "_" + enq.grpidss.ToString() + "_" + headidss.ToString() + "_" + enq.AMCO_Id + "_" + enq.trans_id.ToString() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo + "_" + totpayableamount.ToString());

                string url = paymentdet.FirstOrDefault().merchanturl;

                parameters.Add("CALLBACK_URL", "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponsePAYTM/");

                string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);

                aa.MID = paymentdet.FirstOrDefault().merchantid;
                aa.ORDER_ID = PaymentDetailsDto.trans_id;
                aa.CUST_ID = enq.MI_Id.ToString();
                aa.TXN_AMOUNT = Convert.ToDecimal(totpayableamount);
                aa.CHANNEL_ID = "WEB";

                //for production
                aa.INDUSTRY_TYPE_ID = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType;
                aa.WEBSITE = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website;

                //for test
                //  aa.INDUSTRY_TYPE_ID = "Retail";
                //  aa.WEBSITE = "WEB";

                aa.MOBILE_NO = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo;
                aa.EMAIL = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim();
                aa.payu_URL = url;

                aa.CHECKSUMHASH = checksum;
                aa.MERC_UNQ_REF = enq.ASMAY_Id.ToString().Trim() + "_" + enq.AMCST_Id + "_" + enq.MI_Id.ToString().Trim() + "_" + enq.grpidss.ToString() + "_" + headidss.ToString() + "_" + enq.AMCO_Id + "_" + enq.trans_id.ToString() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo + "_" + totpayableamount.ToString();

                List<PaymentDetails.PAYTM> paydet = new List<PaymentDetails.PAYTM>();
                paydet.Add(aa);

                PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PaymentDetailsDto.PaymentDetailsList;
        }

        public CollegeFeeTransactionDTO get_grp_reptno(CollegeFeeTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string onlineheadmapid = "0", groupidss = "0", instidss = "0";

                long fineamountcal = 0, fethchfmaidsfine = 0, finecount = 0;

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                List<long> HeadId = new List<long>();
                List<long> grpid = new List<long>();
                List<long> instids = new List<long>();

                foreach (var item in data.selected_list)
                {
                    HeadId.Add(item.FMH_Id);
                    grpid.Add(item.FMG_Id);
                    instids.Add(item.FTI_Id);
                    groupidss = groupidss + ',' + item.FMG_Id;
                    instidss = instidss + "," + item.FTI_Id;

                }

                List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();


                list_all = (from b in _mastercourse.Fee_Groupwise_AutoReceiptDMO
                            from c in _mastercourse.Fee_Groupwise_AutoReceipt_GroupsDMO
                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                            select new CollegeFeeTransactionDTO
                            {
                                FGAR_PrefixName = b.FGAR_PrefixName,
                                FGAR_SuffixName = b.FGAR_SuffixName,
                                FGAR_Id = c.FGAR_Id,
                            }
                     ).Distinct().ToList();



                decimal groupwiseamount = 0;

                groupwiseamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                   from b in _mastercourse.Fee_College_Student_StatusDMO
                                   from c in _mastercourse.Fee_PaymentGateway_Details
                                   where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCST_Id == data.AMCST_Id && b.FCSS_ToBePaid > 0 && grpid.Contains(a.fmg_id) && HeadId.Contains(b.FMH_Id) && instids.Contains(b.FTI_Id))
                                   select new CollegeFeeTransactionDTO
                                   {
                                       FCSS_ToBePaid = b.FCSS_ToBePaid,
                                   }
        ).Sum(t => t.FCSS_ToBePaid);



                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "receiptnogeneration_CLG";
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
                        Value = groupidss
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

                    //   var rece_amt = new { receiptno = data.FYP_Receipt_No, amt = groupwiseamount };
                    var rece_amt = new { receiptno = data.FYP_Receipt_No, amt = data.FMA_Amount };

                    dynamicrecgen.Add(rece_amt);

                    List<CollegeFeeTransactionDTO> groupwisefmaids = new List<CollegeFeeTransactionDTO>();

                    groupwisefmaids = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                       from c in _mastercourse.FeeHeadClgDMO
                                       from i in _mastercourse.Fee_College_Student_StatusDMO
                                       from b in _mastercourse.Fee_PaymentGateway_Details
                                       where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && grpid.Contains(a.fmg_id) && ((i.FCSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMCST_Id == data.AMCST_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && HeadId.Contains(i.FMH_Id) && instids.Contains(i.FTI_Id))
                                       select new CollegeFeeTransactionDTO
                                       {
                                           FCMAS_Id = i.FCMAS_Id,
                                           FCSS_ToBePaid = Convert.ToInt64(i.FCSS_ToBePaid),
                                           FMH_Flag = c.FMH_Flag
                                       }
                    ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
                    onlinemtrans.FMOT_Amount = rece_amt.amt;
                    onlinemtrans.FMOT_Date = indianTime;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.PASR_Id = 0;
                    onlinemtrans.AMST_Id = data.AMCST_Id;
                    onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

                    onlinemtrans.FYP_PayModeType = "APP";
                    onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;

                    onlinemtrans.MI_Id = data.MI_Id;
                    onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                    _mastercourse.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < groupwisefmaids.Count(); s++)
                    {
                        if (groupwisefmaids[s].FMH_Flag == "F")
                        {
                            fethchfmaidsfine = groupwisefmaids[s].FCMAS_Id;
                        }
                        else
                        {
                            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                            onlinettrans.FMA_Id = groupwisefmaids[s].FCMAS_Id;
                            onlinettrans.FTOT_Amount = groupwisefmaids[s].FCSS_ToBePaid;
                            onlinettrans.FTOT_Created_date = indianTime;
                            onlinettrans.FTOT_Updated_date = indianTime;
                            onlinettrans.FTOT_Concession = 0;
                            onlinettrans.FTOT_Fine = 0;

                            using (var cmd2 = _mastercourse.Database.GetDbConnection().CreateCommand())
                            {
                                cmd2.CommandText = "Sp_Calculate_Fine_CLG";
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Add(new SqlParameter("@On_Date",
                                    SqlDbType.DateTime, 100)
                                {
                                    //Value = curdt
                                    Value = indianTime
                                });

                                cmd2.Parameters.Add(new SqlParameter("@FCMAS_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = groupwisefmaids[s].FCMAS_Id
                                });
                                cmd2.Parameters.Add(new SqlParameter("@ASMAY_Id",
                               SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                cmd2.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.BigInt)
                                {
                                    Value = data.AMCST_Id
                                });

                                cmd2.Parameters.Add(new SqlParameter("@amt",
                        SqlDbType.Decimal, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                cmd2.Parameters.Add(new SqlParameter("@flgArr",
                   SqlDbType.Int, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                if (cmd2.Connection.State != ConnectionState.Open)
                                    cmd2.Connection.Open();

                                var execfine = cmd2.ExecuteNonQuery();

                                if (Convert.ToInt32(cmd2.Parameters["@amt"].Value) > 0)
                                {
                                    fineamountcal = fineamountcal + Convert.ToInt32(cmd2.Parameters["@amt"].Value);
                                }

                            }

                            _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                        }

                    }



                    for (int s = 0; s < groupwisefmaids.Count(); s++)
                    {
                        if (groupwisefmaids[s].FMH_Flag == "F")
                        {
                            if (finecount < 1)
                            {
                                finecount = finecount + 1;
                                fethchfmaidsfine = groupwisefmaids[s].FCMAS_Id;
                                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                onlinettrans.FMA_Id = groupwisefmaids[s].FCMAS_Id;
                                onlinettrans.FTOT_Amount = fineamountcal;
                                onlinettrans.FTOT_Created_date = indianTime;
                                onlinettrans.FTOT_Updated_date = indianTime;
                                onlinettrans.FTOT_Concession = 0;
                                onlinettrans.FTOT_Fine = 0;
                                _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                            }
                        }
                    }
                    for (int s = 0; s < data.advancedata.Count(); s++)
                    {

                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                        onlinettrans.FMA_Id = data.advancedata[s].FCMAS_Id;
                        onlinettrans.FTOT_Amount = data.advancedata[s].FCSS_ToBePaid;
                        onlinettrans.FTOT_Created_date = indianTime;
                        onlinettrans.FTOT_Updated_date = indianTime;
                        onlinettrans.FTOT_Concession = 0;
                        onlinettrans.FTOT_Fine = 0;


                        _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);


                    }

                    //commented on 19012018
                    //  onlinemtrans.FMOT_Amount = Convert.ToInt32(rece_amt.amt) + Convert.ToInt32(fineamountcal);

                    groupidss = "0";
                    fineamountcal = 0;
                    finecount = 0;
                }


                var contactexisttransaction = 0;

                using (var dbCtxTxn = _mastercourse.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _mastercourse.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

                if (dynamicrecgen.Count() > 0)
                {
                    data.recenocol = dynamicrecgen.ToArray();
                }

            }
            catch (Exception ex)
            {

            }

            return data;
        }

        public PaymentDetails payuresponse(PaymentDetails response)
        {
            try
            {
                CollegeFeeTransactionDTO pgmod = new CollegeFeeTransactionDTO();
                PaymentDetails dto = new PaymentDetails();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (response.status == "success")
                {
                    string txnid = response.txnid.ToString();

                    var groups = (from a in _mastercourse.Fee_Y_PaymentDMO
                                  where (a.MI_Id == Convert.ToInt32(response.udf3) && a.ASMAY_Id == Convert.ToInt32(response.udf1) && a.FYP_Transaction_Id == response.txnid)
                                  select new CollegeFeeTransactionDTO
                                  {
                                      FYP_Receipt_No = a.FYP_ReceiptNo
                                  }
                          ).Distinct().ToList();

                    if (groups.Count == 0)
                    {
                        var confirmstatus = insertdatainfeetables(response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf1, indianTime, "0");

                        if (Convert.ToInt32(confirmstatus) > 0)
                        {
                            pgmod.MI_Id = Convert.ToInt64(response.udf3);
                            pgmod.AMCST_MobileNo = response.phone;
                            pgmod.AMCST_Id = Convert.ToInt64(response.udf2);
                            pgmod.AMCST_emailId = response.email;

                            SMS sms = new SMS(_context);

                            sms.sendSms(Convert.ToInt32(response.udf3), Convert.ToInt64(response.phone), "FEEONLINEPAYMENT", Convert.ToInt32(response.udf2));

                            Email Email = new Email(_context);

                            Email.sendmail(Convert.ToInt32(response.udf3), response.email, "FEEONLINEPAYMENT", Convert.ToInt32(response.udf2));

                        }
                    }

                }
                else
                {
                    SMS sms = new SMS(_context);

                    sms.sendSms(Convert.ToInt32(response.udf3), Convert.ToInt64(response.phone), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(response.udf2));

                    Email Email = new Email(_context);

                    Email.sendmail(Convert.ToInt32(response.udf3), response.email, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(response.udf2));

                    dto.status = response.status;
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public string insertdatainfeetables(string miid, string groupidss, string studentid, string headid, decimal amount, string transid, string refid, string yearid, DateTime indianTime, string transactionid)
        {
            var contactexisttransaction = 0;

            try
            {
                string recnoen = "";
                var fetchfmhotid = (from a in _mastercourse.Fee_M_Online_TransactionDMO
                                    where (a.AMST_Id == Convert.ToInt64(studentid) && a.FMOT_Trans_Id == transid.ToString())
                                    select new CollegeFeeTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FMA_Amount = a.FMOT_Amount
                                    }).ToArray();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    var fethchfmgids = (from a in _mastercourse.Fee_T_Online_TransactionDMO
                                        from b in _mastercourse.CLG_Fee_College_Master_Amount_Semesterwise
                                        from c in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                        from d in _mastercourse.Clg_Fee_AmountEntry_DMO
                                        where (d.FCMA_Id == b.FCMA_Id && a.FMA_Id == b.FCMAS_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(miid))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            FMG_Id = d.FMG_Id,
                                            FMH_Id = d.FMH_Id
                                        }).Distinct().ToArray();

                    List<long> grpid = new List<long>();
                    List<long> headidss = new List<long>();

                    groupidss = "0";
                    foreach (var item in fethchfmgids)
                    {
                        grpid.Add(item.FMG_Id);
                        headidss.Add(item.FMH_Id);
                        groupidss = groupidss + ',' + item.FMG_Id;
                    }

                    List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();
                    List<CollegeFeeTransactionDTO> list_repts = new List<CollegeFeeTransactionDTO>();
                    list_all = (from b in _mastercourse.Fee_Groupwise_AutoReceiptDMO
                                from c in _mastercourse.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid) && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new CollegeFeeTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    FGAR_Id = c.FGAR_Id,
                                }
                         ).Distinct().ToList();

                    using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "receiptnogeneration_CLG";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                            SqlDbType.VarChar, 100)
                        {
                            Value = Convert.ToInt32(miid)
                        });

                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                           SqlDbType.NVarChar, 100)
                        {
                            Value = Convert.ToInt32(yearid)
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmgid",
                       SqlDbType.NVarChar, 100)
                        {
                            Value = groupidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@receiptno",
            SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        recnoen = cmd.Parameters["@receiptno"].Value.ToString();

                        var groupwisefmaids = (from a in _mastercourse.Fee_T_Online_TransactionDMO
                                               from c in _mastercourse.Fee_M_Online_TransactionDMO
                                               where (a.FMOT_Id == c.FMOT_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && c.AMST_Id == Convert.ToInt64(studentid))
                                               select new CollegeFeeTransactionDTO
                                               {
                                                   FCMAS_Id = a.FMA_Id,
                                                   FCSS_ToBePaid = Convert.ToInt64(a.FTOT_Amount)
                                               }
                             ).ToArray();

                        Fee_Y_PaymentDMO onlinemtrans = new Fee_Y_PaymentDMO();

                        onlinemtrans.ASMAY_Id = Convert.ToInt64(yearid);
                        onlinemtrans.FYP_ReceiptNo = recnoen;

                        onlinemtrans.FYP_ReceiptDate = indianTime;
                        onlinemtrans.FYP_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinemtrans.FYP_Remarks = "Online Regular Payment";
                        onlinemtrans.FYP_Currency = "1";
                        onlinemtrans.FYP_PayModeType = "Single";
                        onlinemtrans.FYP_TransactionTypeFlag = "O";

                        onlinemtrans.FYP_ChequeBounceFlag = "CL";
                        onlinemtrans.MI_Id = Convert.ToInt64(miid);
                        onlinemtrans.FYP_DOE = indianTime;
                        onlinemtrans.CreatedDate = indianTime;
                        onlinemtrans.UpdatedDate = indianTime;
                        onlinemtrans.FYP_PayGatewayType = "EASEBUZZ";

                        //added on 02-07-2018
                        var Euserid = (from a in _mastercourse.FeeGroupClgDMO
                                       where (a.MI_Id == Convert.ToInt64(miid) && grpid.Contains(a.FMG_Id))
                                       select new CollegeFeeTransactionDTO
                                       {
                                           enduserid = a.user_id,
                                       }
                           ).Distinct().Take(1).ToArray();
                        //added on 02-07-2018

                        onlinemtrans.User_Id = Convert.ToInt64(Euserid[0].enduserid);
                        onlinemtrans.FYP_Transaction_Id = transid;

                        onlinemtrans.FYP_ChallanStatusFlag = "Sucessfull";
                        onlinemtrans.FYP_PaymentReference_Id = refid.ToString();
                        onlinemtrans.FYP_ChallanNo = "";

                        _mastercourse.Fee_Y_PaymentDMO.Add(onlinemtrans);


                        Fee_Y_Payment_PaymentModeDMO onlinestuappmode = new Fee_Y_Payment_PaymentModeDMO();

                        onlinestuappmode.FYP_Id = onlinemtrans.FYP_Id;
                        onlinestuappmode.FYPPM_BankName = "";
                        onlinestuappmode.FYPPM_ClearanceDate = indianTime;
                        onlinestuappmode.FYPPM_ClearanceStatusFlag = "1";
                        onlinestuappmode.FYPPM_DDChequeDate = indianTime;
                        onlinestuappmode.FYPPM_DDChequeNo = "0";
                        onlinestuappmode.FYPPM_PaymentReference_Id = refid.ToString();
                        onlinestuappmode.FYPPM_TransactionTypeFlag = "O";
                        onlinestuappmode.FYPPM_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinestuappmode.FYPPM_LedgerId = 0;
                        onlinestuappmode.FYPPM_Transaction_Id = transid;

                        _mastercourse.Fee_Y_Payment_PaymentModeDMO.Add(onlinestuappmode);


                        Fee_Y_Payment_College_StudentDMO onlinestuapp = new Fee_Y_Payment_College_StudentDMO();

                        onlinestuapp.FYP_Id = onlinemtrans.FYP_Id;
                        onlinestuapp.AMCST_Id = Convert.ToInt64(studentid);
                        onlinestuapp.ASMAY_Id = Convert.ToInt64(yearid);
                        onlinestuapp.FYPCS_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinestuapp.FYPCS_TotalConcessionAmount = 0;
                        onlinestuapp.FYPCS_TotalFineAmount = 0;
                        onlinestuapp.FYPCS_TotalWaivedAmount = 0;

                        _mastercourse.Fee_Y_Payment_College_StudentDMO.Add(onlinestuapp);

                        for (int s = 0; s < groupwisefmaids.Count(); s++)
                        {
                            Fee_T_College_PaymentDMO onlinettrans = new Fee_T_College_PaymentDMO();
                            onlinettrans.FYP_Id = onlinemtrans.FYP_Id;
                            onlinettrans.FCMAS_Id = groupwisefmaids[s].FCMAS_Id;
                            onlinettrans.FTCP_PaidAmount = groupwisefmaids[s].FCSS_ToBePaid;
                            onlinettrans.FTCP_FineAmount = 0;
                            onlinettrans.FTCP_ConcessionAmount = 0;
                            onlinettrans.FTCP_WaivedAmount = 0;
                            onlinettrans.FTCP_Remarks = "Online Regular Payment";

                            _mastercourse.Fee_T_College_PaymentDMO.Add(onlinettrans);

                            var obj_status_stf = _mastercourse.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == Convert.ToInt64(miid) && t.ASMAY_Id == Convert.ToInt64(yearid) && t.AMCST_Id == Convert.ToInt64(studentid) && t.FCMAS_Id == groupwisefmaids[s].FCMAS_Id).FirstOrDefault();


                            if (obj_status_stf == null)
                            {
                                obj_status_stf = null;
                            }
                            else if (obj_status_stf.FCSS_Id != null)
                            {
                                obj_status_stf.FCSS_PaidAmount = obj_status_stf.FCSS_PaidAmount + groupwisefmaids[s].FCSS_ToBePaid;

                                //added on 11-07-2018
                                var fineheadss = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                                  from b in _mastercourse.FeeHeadClgDMO
                                                  where (a.MI_Id == Convert.ToInt64(miid) && a.FCMAS_Id == groupwisefmaids[s].FCMAS_Id && a.ASMAY_Id == Convert.ToInt64(yearid) && b.FMH_Flag == "F" && a.AMCST_Id == Convert.ToInt64(studentid) && a.FMH_Id == b.FMH_Id)
                                                  select new CollegeFeeTransactionDTO
                                                  {
                                                      FMH_Id = a.FMH_Id,
                                                  }
                                   ).Distinct().Take(1);

                                if (fineheadss.Count() > 0)
                                {
                                    obj_status_stf.FCSS_FineAmount = obj_status_stf.FCSS_FineAmount + groupwisefmaids[s].FCSS_ToBePaid;
                                }
                                //added on 11-07-2018


                                if (obj_status_stf.FCSS_NetAmount != 0)
                                {
                                    var tobepaid = obj_status_stf.FCSS_ToBePaid - groupwisefmaids[s].FCSS_ToBePaid;
                                    obj_status_stf.FCSS_ToBePaid = tobepaid;
                                }
                                else
                                {
                                    var tobepaid= obj_status_stf.FCSS_ToBePaid - groupwisefmaids[s].FCSS_ToBePaid;
                                    obj_status_stf.FCSS_ToBePaid = tobepaid;
                                }
                                if (obj_status_stf != null)
                                {
                                    _mastercourse.Update(obj_status_stf);
                                }

                            }



                        }

                        groupidss = "0";
                    }

                }

                using (var dbCtxTxn = _mastercourse.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _mastercourse.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return contactexisttransaction.ToString();
        }

        public static String generateCheckSum(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            try
            {
                foreach (string key in parameters.Keys)
                {
                    // below code snippet is mandatory, so that no one can use your checksumgeneration url for other purpose .
                    if (parameters[key].Trim().ToUpper().Contains("REFUND") || parameters[key].Trim().Contains("|"))
                    {
                        parameter.Add(key.Trim(), "");
                    }

                    //if (parameters.ContainsKey(key.Trim()))
                    //{
                    //    parameters[key.Trim()] = parameters[key].Trim();
                    //}
                    else
                    {
                        parameter.Add(key.Trim(), parameters[key].Trim());
                    }
                }

                String checkSumParams = SecurityUtils.createCheckSumString(parameter);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);

                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);

                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }
        }

        public static String generateCheckSumForRefund(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }

        public static String generateCheckSumByJson(String masterKey, string json)
        {
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = json;
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;

                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                hashedCheckSum += salt;

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }

        public static Boolean verifyCheckSumByjson(String masterKey, string json, String checkSum)
        {
            // Validate Input
            validateVerifyCheckSumInput(masterKey, checkSum);

            try
            {
                String hashedCheckSum = Crypto.Decrypt(checkSum, masterKey);

                if (hashedCheckSum == null || hashedCheckSum.Length < Constants.SALT_LENGTH)
                {
                    return false;
                }

                String salt = hashedCheckSum.Substring(hashedCheckSum.Length - Constants.SALT_LENGTH, Constants.SALT_LENGTH);
                MessageConsole.WriteLine("Salt:::: " + salt);


                MessageConsole.WriteLine(); MessageConsole.WriteLine("Input CheckSum:::: " + checkSum);

                // Now creating hashed checkSum string from given checkSum string
                String checkSumParams = json;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String:::: " + checkSumParams);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String with Salt:::: " + checkSumParams);

                String hashedInputCheckSum = SecurityUtils.getHashedString(checkSumParams);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String:::: " + hashedInputCheckSum);
                hashedInputCheckSum += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String with Salt:::: " + hashedInputCheckSum);

                return hashedInputCheckSum.Equals(hashedCheckSum);
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while verifying CheckSum. " + e.Message);
            }

        }

        public static Boolean verifyCheckSum(String masterKey, Dictionary<String, String> parameters, String checkSum)
        {
            // Validate Input
            validateVerifyCheckSumInput(masterKey, checkSum);

            try
            {
                String hashedCheckSum = Crypto.Decrypt(checkSum, masterKey);

                if (hashedCheckSum == null || hashedCheckSum.Length < Constants.SALT_LENGTH)
                {
                    return false;
                }

                String salt = hashedCheckSum.Substring(hashedCheckSum.Length - Constants.SALT_LENGTH, Constants.SALT_LENGTH);
                MessageConsole.WriteLine("Salt:::: " + salt);


                MessageConsole.WriteLine(); MessageConsole.WriteLine("Input CheckSum:::: " + checkSum);

                // Now creating hashed checkSum string from given checkSum string
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String:::: " + checkSumParams);
                checkSumParams += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String with Salt:::: " + checkSumParams);

                String hashedInputCheckSum = SecurityUtils.getHashedString(checkSumParams);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String:::: " + hashedInputCheckSum);
                hashedInputCheckSum += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String with Salt:::: " + hashedInputCheckSum);

                return hashedInputCheckSum.Equals(hashedCheckSum);
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while verifying CheckSum. " + e.Message);
            }

        }

        private static void validateGenerateCheckSumInput(String masterKey)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

        }

        private static void validateVerifyCheckSumInput(String masterKey, String checkSum)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

            if (checkSum == null)
            {
                throw new ArgumentNullException("Parameter cannot be null", "checkSum");
            }
        }

        public static string Encrypt(String CardDetails, String masterKey)
        {
            return Crypto.Encrypt(CardDetails, masterKey);
        }

        public static String Decrypt(String carddetails, String masterKey)
        {
            return Crypto.Decrypt(carddetails, masterKey);
        }
        public PaymentDetails.PAYTM payuresponsepaytm(PaymentDetails.PAYTM response)
        {
            try
            {
                CollegeFeeTransactionDTO pgmod = new CollegeFeeTransactionDTO();
                PaymentDetails dto = new PaymentDetails();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string[] tokens = response.MERC_UNQ_REF.Split('_');

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                              where (a.MI_Id == Convert.ToInt32(tokens[2]) && a.FPGD_PGName == "PAYTM")
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                PAYMENTPARAMDETAILS = (from a in _mastercourse.PAYUDETAILS
                                       where (a.IMPG_PGFlag == "PAYTM")
                                       select new FeeStudentTransactionDTO
                                       {
                                           IMPG_IndustryType = a.IMPG_IndustryType,
                                           IMPG_Website = a.IMPG_Website,
                                           //IMPG_CallBackURL = a.IMPG_CallBackURL
                                       }
           ).ToList();

                PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

                //commeneted
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("MID", paymentdet[0].merchantid);
                parameters.Add("ORDER_ID", tokens[6]);
                parameters.Add("CUST_ID", tokens[5]);
                parameters.Add("TXN_AMOUNT", tokens[9]);
                parameters.Add("CHANNEL_ID", "WEB");

                //for production
                parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
                parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);

                parameters.Add("MOBILE_NO", tokens[8]);
                parameters.Add("EMAIL", tokens[7]);
                parameters.Add("MERC_UNQ_REF", tokens[0].ToString().Trim() + "_" + tokens[1].ToString() + "_" + tokens[2].ToString() + "_" + tokens[3].ToString() + "_" + Convert.ToString(tokens[4]) + "_" + tokens[5] + "_" + tokens[6].ToString() + "_" + tokens[7].Trim() + "_" + tokens[8] + "_" + tokens[9].ToString());

                string url = paymentdet.FirstOrDefault().merchanturl;
                parameters.Add("CALLBACK_URL", "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponsePAYTM/");

                string checksum = generateCheckSum(paymentdet[0].merchantkey, parameters);

                // CODE SNIPPET FOR CHECKSUM VALIDATION
                Boolean res = false;
                res = verifyCheckSum(paymentdet[0].merchantkey, parameters, checksum);
                if (res == true && response.status == "TXN_SUCCESS")
                {
                    dto.udf1 = tokens[0];

                    dto.udf3 = tokens[2];
                    dto.udf4 = tokens[3];
                    dto.udf2 = tokens[1];
                    dto.udf5 = tokens[4];
                    dto.trans_id = tokens[6];
                    dto.email = tokens[7];
                    dto.phone = Convert.ToInt64(tokens[8]);
                    dto.amount = Convert.ToInt64(tokens[9]);

                    dto.txnid = response.txnid;
                    dto.BANKTXNID = response.BANKTXNID;

                    var groups = (from a in _mastercourse.Fee_Y_PaymentDMO
                                  where (a.MI_Id == dto.IVRMOP_MIID && a.ASMAY_Id == Convert.ToInt32(dto.udf1) && a.FYP_Transaction_Id == dto.trans_id)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FYP_Receipt_No = a.FYP_ReceiptNo
                                  }
                          ).Distinct().ToList();

                    if (groups.Count == 0)
                    {
                        var confirmstatus = insertdatainfeetables(dto.udf3, dto.udf4, dto.udf2, dto.udf5, dto.amount, dto.trans_id, dto.txnid, dto.udf1, indianTime, dto.BANKTXNID);

                        if (Convert.ToInt32(confirmstatus) > 0)
                        {
                            pgmod.MI_Id = Convert.ToInt64(dto.udf3);
                            pgmod.AMCST_MobileNo = dto.phone;
                            pgmod.AMCST_Id = Convert.ToInt64(dto.udf2);
                            pgmod.AMCST_emailId = dto.email;

                            SMS sms = new SMS(_context);

                            sms.sendSms(Convert.ToInt32(dto.udf3), Convert.ToInt64(dto.phone), "FEEONLINEPAYMENT", Convert.ToInt32(dto.udf2));

                            Email Email = new Email(_context);

                            Email.sendmail(Convert.ToInt32(dto.udf3), dto.email, "FEEONLINEPAYMENT", Convert.ToInt32(dto.udf2));

                        }
                    }
                }
                else
                {
                    SMS sms = new SMS(_context);

                    sms.sendSms(Convert.ToInt32(dto.udf3), Convert.ToInt64(dto.phone), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(dto.udf2));

                    Email Email = new Email(_context);

                    Email.sendmail(Convert.ToInt32(dto.udf3), dto.email, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(dto.udf2));

                    dto.status = response.status;
                }

            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public PaymentDetails getpaymentresponserazorpay(PaymentDetails response)
        {
            try
            {
                FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
                PaymentDetails dto = new PaymentDetails();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //single account added on 17/12/2019

                var accountvalidation = (from a in _mastercourse.Fee_PaymentGateway_Details
                                         where (a.MI_Id == response.IVRMOP_MIID && a.FPGD_PGName == "RAZORPAY")
                                         select new FeeStudentTransactionDTO
                                         {
                                             FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                         }).Distinct().ToArray();

                //single account added on 17/12/2019

                //TRANSFER API

                string url = "https://api.razorpay.com/v1/payments/" + response.razorpay_payment_id + "/transfers";

                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.IVRMOP_MIID && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);

                response.order_id = payment.Attributes["order_id"];

                //FETCH SUBMERCHANT IDS

                var fetchfmhotid = (from a in _mastercourse.Fee_M_Online_TransactionDMO
                                    where (a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Amount > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FMA_Amount = a.FMOT_Amount,
                                        MI_Id = a.MI_Id,
                                        ASMAY_Id = a.ASMAY_ID,
                                        Amst_Id = a.AMST_Id,

                                    }).ToArray();

                var fetchstudentdeatils = (from a in _mastercourse.Adm_Master_College_StudentDMO
                                           from b in _mastercourse.Adm_College_Yearly_StudentDMO
                                           where (a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && b.ASMAY_Id == Convert.ToInt64(fetchfmhotid[0].ASMAY_Id) && a.MI_Id == Convert.ToInt64(fetchfmhotid[0].MI_Id))
                                           select new CollegeFeeTransactionDTO
                                           {
                                               Mobile_No = a.AMCST_MobileNo,
                                               AMCST_emailId = a.AMCST_emailId,
                                               AMCO_Id = b.AMCO_Id,
                                               AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) +
                                        (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) +
                                        (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),
                                               AMCST_AdmNo = a.AMCST_AdmNo,
                                               AMCST_Id = a.AMCST_Id
                                           }).ToArray();

                Dictionary<String, object> transfers = new Dictionary<String, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                List<FeeStudentTransactionDTO> fetchaccountid = new List<FeeStudentTransactionDTO>();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    transfers.Clear();
                    transfersnotes.Clear();

                    fetchaccountid = (from a in _mastercourse.Fee_T_Online_TransactionDMO
                                      from f in _mastercourse.Clg_Fee_AmountEntry_DMO
                                      from b in _mastercourse.CLG_Fee_College_Master_Amount_Semesterwise
                                      from c in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                      from d in _mastercourse.Fee_PaymentGateway_Details
                                      from e in _mastercourse.PAYUDETAILS
                                      where (f.FCMA_Id == b.FCMA_Id && a.FMA_Id == b.FCMAS_Id && f.FMG_Id == c.fmg_id && f.FMH_Id == c.FMH_Id && f.FTI_Id == c.fti_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && e.IMPG_PGFlag == "RAZORPAY" && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(fetchfmhotid[r].MI_Id))
                                      select new FeeStudentTransactionDTO
                                      {
                                          FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                      }).Distinct().ToList();

                    var fetchamount = (from a in _mastercourse.Fee_M_Online_TransactionDMO
                                       where (a.AMST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMA_Amount = a.FMOT_Amount
                                       }).ToArray();

                    transfersnotes.Add("notes_1", fetchstudentdeatils[0].AMCST_FirstName);
                    transfersnotes.Add("notes_2", fetchstudentdeatils[0].AMCST_AdmNo);
                    transfersnotes.Add("notes_3", fetchstudentdeatils[0].AMCST_Id);
                    transfersnotes.Add("notes_4", fetchstudentdeatils[0].Mobile_No);
                    transfersnotes.Add("notes_5", fetchstudentdeatils[0].AMCST_emailId);

                    transfers.Add("account", (fetchaccountid.FirstOrDefault().FPGD_SubMerchantId));
                    transfers.Add("amount", (Convert.ToInt32(fetchamount.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString());
                    transfers.Add("currency", "INR");
                    transfers.Add("notes", transfersnotes);

                    if (accountvalidation.Count() > 1)
                    {
                        Razorpay.Api.Transfer payment1 = client.Transfer.Create(transfers);

                        transferAPI trapay = new transferAPI();

                        if (payment1.Attributes["id"] != "")
                        {
                            trapay.transfer_id = payment1.Attributes["id"];
                            trapay.entity = payment1.Attributes["entity"];
                            trapay.source = payment1.Attributes["source"];
                            trapay.recipient = payment1.Attributes["recipient"];
                            trapay.amount = payment1.Attributes["amount"];
                            trapay.created_at = payment1.Attributes["created_at"];

                            FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                            fet.TRANSFER_ID = trapay.transfer_id;
                            fet.ENTITY = trapay.entity;
                            fet.SOURCE = trapay.source;
                            fet.RECIPIENT = trapay.recipient;
                            fet.AMOUNT = (Convert.ToInt32(trapay.amount) / 100).ToString();
                            fet.CREATED_AT = trapay.created_at;
                            fet.ORDER_ID = response.order_id;

                            fet.PAYMENT_ID = response.razorpay_payment_id;
                            fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                            fet.SETTLEMENT_FLAG = "0";

                            fet.CREATED_BY = indianTime;
                            fet.UPDATED_BY = indianTime;
                            _mastercourse.Add(fet);
                            var contactExists = _mastercourse.SaveChanges();
                            if (contactExists == 1)
                            {
                                response.status = "success";
                            }
                            else
                            {
                                response.status = "Failure";
                            }
                        }
                    }
                    else
                    {
                        FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                        fet.TRANSFER_ID = "";
                        fet.ENTITY = "";
                        fet.SOURCE = "";
                        fet.RECIPIENT = "";
                        fet.AMOUNT = (Convert.ToInt32(fetchamount.FirstOrDefault().FMA_Amount.ToString())).ToString();
                        fet.CREATED_AT = indianTime.ToString();
                        fet.ORDER_ID = response.order_id;

                        fet.PAYMENT_ID = response.razorpay_payment_id;
                        fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                        fet.SETTLEMENT_FLAG = "0";

                        fet.CREATED_BY = indianTime;
                        fet.UPDATED_BY = indianTime;
                        _mastercourse.Add(fet);
                        var contactExists = _mastercourse.SaveChanges();
                        if (contactExists == 1)
                        {
                            response.status = "success";
                        }
                        else
                        {
                            response.status = "Failure";
                        }
                    }

                }

                if (response.status == "success")
                {
                    var groups = (from a in _mastercourse.Fee_Y_PaymentDMO
                                  from b in _mastercourse.Fee_Y_Payment_PaymentModeDMO
                                  where (a.MI_Id == Convert.ToInt32(fetchfmhotid[0].MI_Id) && a.ASMAY_Id == Convert.ToInt32(fetchfmhotid[0].ASMAY_Id) && b.FYPPM_Transaction_Id == response.order_id)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FYP_Receipt_No = a.FYP_ReceiptNo
                                  }
                          ).Distinct().ToList();

                    if (groups.Count == 0)
                    {
                        var confirmstatus = insertdatainfeetables(fetchfmhotid[0].MI_Id.ToString(), "0", fetchfmhotid[0].Amst_Id.ToString(), "0", fetchfmhotid[0].FMA_Amount, response.order_id, response.razorpay_payment_id, fetchfmhotid[0].ASMAY_Id.ToString(), indianTime, "0");

                        if (Convert.ToInt32(confirmstatus) > 0)
                        {
                            SMS sms = new SMS(_context);

                            sms.sendSms(Convert.ToInt32(fetchfmhotid[0].MI_Id), Convert.ToInt64(fetchstudentdeatils[0].Mobile_No), "FEEONLINEPAYMENT", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                            Email Email = new Email(_context);

                            Email.sendmail(Convert.ToInt32(fetchfmhotid[0].MI_Id), fetchstudentdeatils[0].AMCST_emailId, "FEEONLINEPAYMENT", Convert.ToInt32(fetchfmhotid[0].Amst_Id));
                        }
                    }
                }
                else
                {
                    SMS sms = new SMS(_context);

                    sms.sendSms(Convert.ToInt32(fetchfmhotid[0].MI_Id), Convert.ToInt64(fetchstudentdeatils[0].Mobile_No), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                    Email Email = new Email(_context);

                    Email.sendmail(Convert.ToInt32(fetchfmhotid[0].MI_Id), fetchstudentdeatils[0].AMCST_emailId, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                    dto.status = response.status;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        //Display only Amount
        public CollegeFeeTransactionDTO generatehashsequencedisplay(CollegeFeeTransactionDTO data)
        {
            try
            {
                data.fillstudent = (from a in _mastercourse.Adm_Master_College_StudentDMO
                                    from b in _mastercourse.Adm_College_Yearly_StudentDMO
                                    from c in _mastercourse.MasterCourseDMO
                                    from d in _mastercourse.ClgMasterBranchDMO
                                    from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id)
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
                                        AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                        AMCST_emailId = a.AMCST_emailId,
                                    }
             ).ToArray();


                if (data.onlinepaygteway == "PAYU")
                {
                    data.paydet = paymentPart(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "BILLDESK")
                {
                    //data = paymentPartbilldesk(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "EBS")
                {
                    //data.paydet = paymentPartebs(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "PAYTM")
                {
                    data.paydet = paymentPartdisplaypaytm(data, data.topayamount);
                    //data.paydet = paymentPartpaytmmobile(data, data.topayamount);
                }
                if (data.onlinepaygteway == "RAZORPAY")
                {
                    data.paydet = paymentPartdisplayRazorPay(data, data.topayamount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Array paymentPartdisplaypaytm(CollegeFeeTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                long yrid = getacademicyearcongig(enq);

                enq.ASMAY_Id = yrid;
                enq.grpidss = "0";
                string headidss = "0";

                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();

                List<CollegeFeeTransactionDTO> groupheadorderlist = new List<CollegeFeeTransactionDTO>();

                groupheadorderlist = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                      from b in _mastercourse.FeeGroupClgDMO
                                      from c in _mastercourse.FeeHeadClgDMO
                                      where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.AMCST_Id == enq.AMCST_Id && a.FCSS_ToBePaid > 0)
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FMG_Id = b.FMG_Id,
                                          FMH_Id = c.FMH_Id,
                                          FMG_Order = b.FMG_Order,
                                          FMH_Order = c.FMH_Order,
                                          FCSS_CurrentYrCharges = a.FCSS_CurrentYrCharges,
                                          FCSS_ToBePaid = a.FCSS_ToBePaid,
                                          FCSS_TotalCharges = a.FCSS_TotalCharges
                                      }
        ).OrderBy(t => t.FMG_Order).OrderBy(t => t.FMH_Order).ToList();

                long tempamount = 0;
                List<CollegeFeeTransactionDTO> selected_list = new List<CollegeFeeTransactionDTO>();
                tempamount = enq.topayamount;
                for (int r = 0; r < groupheadorderlist.Count(); r++)
                {
                    if (tempamount > 0)
                    {
                        if (tempamount < groupheadorderlist[r].FCSS_ToBePaid)
                        {
                            groupheadorderlist[r].FCSS_ToBePaid = tempamount;
                            tempamount = 0;
                        }
                        else if (tempamount >= groupheadorderlist[r].FCSS_ToBePaid)
                        {
                            tempamount = tempamount - groupheadorderlist[r].FCSS_ToBePaid;
                        }

                        grpids.Add(groupheadorderlist[r].FMG_Id);
                        headids.Add(groupheadorderlist[r].FMH_Id);
                        enq.grpidss = enq.grpidss + "," + groupheadorderlist[r].FMG_Id;
                        headidss = headidss + "," + groupheadorderlist[r].FMH_Id;

                    }

                }

                //foreach (var x in enq.selected_list)
                //{
                //    grpids.Add(x.FMG_Id);
                //    headids.Add(x.FMH_Id);
                //    enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                //    headidss = headidss + "," + x.FMH_Id;
                //}

                string fpgdids = "";
                var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _mastercourse.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                //      enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                //                           from b in _mastercourse.Fee_College_Student_StatusDMO
                //                           from c in _mastercourse.Fee_PaymentGateway_Details
                //                           where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                //                           select new CollegeFeeTransactionDTO
                //                           {
                //                               FCSS_ToBePaid = b.FCSS_ToBePaid,
                //                           }
                //).Sum(t => t.FCSS_ToBePaid);

                int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    //value = (Convert.ToInt32(enq.pendingamount)).ToString(),
                    value = (Convert.ToInt32(enq.topayamount)).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                //if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                //{
                //GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                //enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                //enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                //PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                //PaymentDetailsDto.trans_id = "RegularOnlinePAYTM" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();
                //  }


                PaymentDetailsDto.trans_id = "RegularOnlinePAYTM" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                enq.trans_id = PaymentDetailsDto.trans_id;
                get_grp_reptno_display(enq);

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                PAYMENTPARAMDETAILS = (from a in _mastercourse.PAYUDETAILS
                                       where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                       select new FeeStudentTransactionDTO
                                       {
                                           IMPG_IndustryType = a.IMPG_IndustryType,
                                           IMPG_Website = a.IMPG_Website,
                                           //IMPG_CallBackURL = a.IMPG_CallBackURL
                                       }
           ).ToList();

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
                enq.trans_id = PaymentDetailsDto.trans_id.ToString();

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }
                var item = new
                {
                    paymentParts = result
                };

                PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
                parameters.Add("ORDER_ID", PaymentDetailsDto.trans_id);
                parameters.Add("CUST_ID", enq.MI_Id.ToString());
                parameters.Add("TXN_AMOUNT", totpayableamount.ToString());
                parameters.Add("CHANNEL_ID", "WEB");

                //for production
                parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
                parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);

                //for test
                //parameters.Add("INDUSTRY_TYPE_ID", "Retail");
                //parameters.Add("WEBSITE", "WEB");

                parameters.Add("MOBILE_NO", ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo.ToString());
                parameters.Add("EMAIL", ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim());
                parameters.Add("MERC_UNQ_REF", enq.ASMAY_Id.ToString().Trim() + "_" + enq.AMCST_Id + "_" + enq.MI_Id.ToString().Trim() + "_" + enq.grpidss.ToString() + "_" + headidss.ToString() + "_" + enq.AMCO_Id + "_" + enq.trans_id.ToString() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo + "_" + totpayableamount.ToString());

                string url = paymentdet.FirstOrDefault().merchanturl;

                parameters.Add("CALLBACK_URL", "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponsePAYTM/");

                string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);

                aa.MID = paymentdet.FirstOrDefault().merchantid;
                aa.ORDER_ID = PaymentDetailsDto.trans_id;
                aa.CUST_ID = enq.MI_Id.ToString();
                aa.TXN_AMOUNT = Convert.ToDecimal(totpayableamount);
                aa.CHANNEL_ID = "WEB";

                //for production
                aa.INDUSTRY_TYPE_ID = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType;
                aa.WEBSITE = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website;

                //for test
                //  aa.INDUSTRY_TYPE_ID = "Retail";
                //  aa.WEBSITE = "WEB";

                aa.MOBILE_NO = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo;
                aa.EMAIL = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim();
                aa.payu_URL = url;

                aa.CHECKSUMHASH = checksum;
                aa.MERC_UNQ_REF = enq.ASMAY_Id.ToString().Trim() + "_" + enq.AMCST_Id + "_" + enq.MI_Id.ToString().Trim() + "_" + enq.grpidss.ToString() + "_" + headidss.ToString() + "_" + enq.AMCO_Id + "_" + enq.trans_id.ToString() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_emailId.Trim() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCST_MobileNo + "_" + totpayableamount.ToString();

                List<PaymentDetails.PAYTM> paydet = new List<PaymentDetails.PAYTM>();
                paydet.Add(aa);

                PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PaymentDetailsDto.PaymentDetailsList;
        }

        public Array paymentPartdisplayRazorPay(CollegeFeeTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                long yrid = getacademicyearcongig(enq);

                enq.ASMAY_Id = yrid;
                enq.grpidss = "0";
                string headidss = "0";

                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();

                List<CollegeFeeTransactionDTO> groupheadorderlist = new List<CollegeFeeTransactionDTO>();

                groupheadorderlist = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                      from b in _mastercourse.FeeGroupClgDMO
                                      from c in _mastercourse.FeeHeadClgDMO
                                      where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.AMCST_Id == enq.AMCST_Id && a.FCSS_ToBePaid > 0)
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FMG_Id = b.FMG_Id,
                                          FMH_Id = c.FMH_Id,
                                          FMG_Order = b.FMG_Order,
                                          FMH_Order = c.FMH_Order,
                                          FCSS_CurrentYrCharges = a.FCSS_CurrentYrCharges,
                                          FCSS_ToBePaid = a.FCSS_ToBePaid,
                                          FCSS_TotalCharges = a.FCSS_TotalCharges
                                      }
        ).OrderBy(t => t.FMG_Order).OrderBy(t => t.FMH_Order).ToList();

                long tempamount = 0;
                List<CollegeFeeTransactionDTO> selected_list = new List<CollegeFeeTransactionDTO>();
                tempamount = enq.topayamount;
                for (int r = 0; r < groupheadorderlist.Count(); r++)
                {
                    if (tempamount > 0)
                    {


                        grpids.Add(groupheadorderlist[r].FMG_Id);
                        headids.Add(groupheadorderlist[r].FMH_Id);
                        enq.grpidss = enq.grpidss + "," + groupheadorderlist[r].FMG_Id;
                        headidss = headidss + "," + groupheadorderlist[r].FMH_Id;
                    }
                }


                string fpgdids = "";
                var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _mastercourse.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _mastercourse.Fee_College_Student_StatusDMO
                                     from c in _mastercourse.Fee_PaymentGateway_Details
                                     where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FCSS_ToBePaid = b.FCSS_ToBePaid,
                                     }
          ).Sum(t => t.FCSS_ToBePaid);

                int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    value = (Convert.ToInt32(enq.pendingamount)).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }
                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                string orderId;

                Dictionary<string, object> input = new Dictionary<string, object>();
                //input.Add("amount", 1 * 100);
                input.Add("amount", totamount * 100); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", PaymentDetailsDto.trans_id);
                input.Add("payment_capture", 1);

                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                RazorpayClient client = new RazorpayClient(key, secret);

                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();

                enq.trans_id = orderId;
                enq.merchantkey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                enq.FMA_Amount = totpayableamount;
                enq.splitpayinformation = payinfo;

                //get_grp_reptno(enq);
                get_grp_reptno_display(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PaymentDetailsDto.PaymentDetailsList;
        }

        public CollegeFeeTransactionDTO get_grp_reptno_display(CollegeFeeTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //DateTime curdt = DateTime.Now;
                string onlineheadmapid = "0", groupidss = "0";

                long fineamountcal = 0, fethchfmaidsfine = 0, finecount = 0;

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                List<long> HeadId = new List<long>();
                List<long> grpid = new List<long>();
                List<CollegeFeeTransactionDTO> groupheadorderlist = new List<CollegeFeeTransactionDTO>();

                groupheadorderlist = (from a in _mastercourse.Fee_College_Student_StatusDMO
                                      from b in _mastercourse.FeeGroupClgDMO
                                      from c in _mastercourse.FeeHeadClgDMO
                                      where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.FCSS_ToBePaid > 0)
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FMG_Id = b.FMG_Id,
                                          FMH_Id = c.FMH_Id,
                                          FMG_Order = b.FMG_Order,
                                          FMH_Order = c.FMH_Order,
                                          FCSS_CurrentYrCharges = a.FCSS_CurrentYrCharges,
                                          FCSS_ToBePaid = a.FCSS_ToBePaid,
                                          FCSS_TotalCharges = a.FCSS_TotalCharges
                                      }
       ).OrderBy(t => t.FMG_Order).OrderBy(t => t.FMH_Order).ToList();

                long tempamount = 0;
                List<CollegeFeeTransactionDTO> selected_list = new List<CollegeFeeTransactionDTO>();
                tempamount = data.topayamount;
                for (int r = 0; r < groupheadorderlist.Count(); r++)
                {
                    if (tempamount > 0)
                    {
                        if (tempamount < groupheadorderlist[r].FCSS_ToBePaid)
                        {
                            groupheadorderlist[r].FCSS_ToBePaid = tempamount;
                            tempamount = 0;
                        }
                        else if (tempamount >= groupheadorderlist[r].FCSS_ToBePaid)
                        {
                            tempamount = tempamount - groupheadorderlist[r].FCSS_ToBePaid;
                        }

                        grpid.Add(groupheadorderlist[r].FMG_Id);
                        HeadId.Add(groupheadorderlist[r].FMH_Id);
                        groupidss = data.grpidss + "," + groupheadorderlist[r].FMG_Id;
                    }

                }


                //foreach (var item in data.selected_list)
                //{
                //    HeadId.Add(item.FMH_Id);
                //    grpid.Add(item.FMG_Id);
                //    groupidss = groupidss + ',' + item.FMG_Id;
                //}

                List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();


                list_all = (from b in _mastercourse.Fee_Groupwise_AutoReceiptDMO
                            from c in _mastercourse.Fee_Groupwise_AutoReceipt_GroupsDMO
                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                            select new CollegeFeeTransactionDTO
                            {
                                FGAR_PrefixName = b.FGAR_PrefixName,
                                FGAR_SuffixName = b.FGAR_SuffixName,
                                FGAR_Id = c.FGAR_Id,
                            }
                     ).Distinct().ToList();

                decimal groupwiseamount = 0;
                groupwiseamount = data.topayamount;

                //        groupwiseamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                //                           from b in _mastercourse.Fee_College_Student_StatusDMO
                //                           from c in _mastercourse.Fee_PaymentGateway_Details
                //                           where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCST_Id == data.AMCST_Id && b.FCSS_ToBePaid > 0 && grpid.Contains(a.fmg_id) && HeadId.Contains(b.FMH_Id))
                //                           select new CollegeFeeTransactionDTO
                //                           {
                //                               FCSS_ToBePaid = b.FCSS_ToBePaid,
                //                           }
                //).Sum(t => t.FCSS_ToBePaid);

                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "receiptnogeneration_CLG";
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
                        Value = groupidss
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

                    var rece_amt = new { receiptno = data.FYP_Receipt_No, amt = groupwiseamount };

                    dynamicrecgen.Add(rece_amt);

                    long revisedamount = data.topayamount;

                    List<CollegeFeeTransactionDTO> groupwisefmaids = new List<CollegeFeeTransactionDTO>();

                    groupwisefmaids = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                       from c in _mastercourse.FeeHeadClgDMO
                                       from i in _mastercourse.Fee_College_Student_StatusDMO
                                       from b in _mastercourse.Fee_PaymentGateway_Details
                                       where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && grpid.Contains(a.fmg_id) && ((i.FCSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMCST_Id == data.AMCST_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && HeadId.Contains(i.FMH_Id))
                                       select new CollegeFeeTransactionDTO
                                       {
                                           FCMAS_Id = i.FCMAS_Id,
                                           FCSS_ToBePaid = Convert.ToInt64(i.FCSS_ToBePaid),
                                           FMH_Flag = c.FMH_Flag
                                       }
                    ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
                    //onlinemtrans.FMOT_Amount = data.topayamount;
                    onlinemtrans.FMOT_Amount = rece_amt.amt;
                    onlinemtrans.FMOT_Date = indianTime;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.PASR_Id = 0;
                    onlinemtrans.AMST_Id = data.AMCST_Id;
                    onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

                    onlinemtrans.FYP_PayModeType = "APP";
                    onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;

                    onlinemtrans.MI_Id = data.MI_Id;
                    onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                    _mastercourse.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < groupwisefmaids.Count(); s++)
                    {
                        if (groupwisefmaids[s].FMH_Flag == "F")
                        {
                            fethchfmaidsfine = groupwisefmaids[s].FCMAS_Id;
                        }
                        else
                        {
                            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                            onlinettrans.FMA_Id = groupwisefmaids[s].FCMAS_Id;

                            if (revisedamount > 0)
                            {
                                if (revisedamount >= groupwisefmaids[s].FCSS_ToBePaid)
                                {
                                    revisedamount = revisedamount - groupwisefmaids[s].FCSS_ToBePaid;
                                }
                                else if (revisedamount < groupwisefmaids[s].FCSS_ToBePaid)
                                {
                                    groupwisefmaids[s].FCSS_ToBePaid = revisedamount;
                                }
                            }

                            onlinettrans.FTOT_Amount = groupwisefmaids[s].FCSS_ToBePaid;
                            onlinettrans.FTOT_Created_date = indianTime;
                            onlinettrans.FTOT_Updated_date = indianTime;
                            onlinettrans.FTOT_Concession = 0;
                            onlinettrans.FTOT_Fine = 0;

                            using (var cmd1 = _mastercourse.Database.GetDbConnection().CreateCommand())
                            {
                                cmd1.CommandText = "Sp_Calculate_Fine";
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                    SqlDbType.DateTime, 100)
                                {
                                    //Value = curdt
                                    Value = indianTime
                                });

                                cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                   SqlDbType.NVarChar, 100)
                                {
                                    Value = groupwisefmaids[s].FCMAS_Id
                                });
                                cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                               SqlDbType.NVarChar, 100)
                                {
                                    Value = data.ASMAY_Id
                                });

                                cmd1.Parameters.Add(new SqlParameter("@amt",
                    SqlDbType.Decimal, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                cmd1.Parameters.Add(new SqlParameter("@flgArr",
                   SqlDbType.Int, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                if (cmd1.Connection.State != ConnectionState.Open)
                                    cmd1.Connection.Open();

                                var execfine = cmd1.ExecuteNonQuery();

                            }

                            _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                        }

                    }

                    for (int s = 0; s < groupwisefmaids.Count(); s++)
                    {
                        if (groupwisefmaids[s].FMH_Flag == "F")
                        {
                            if (finecount < 1)
                            {
                                finecount = finecount + 1;
                                fethchfmaidsfine = groupwisefmaids[s].FCMAS_Id;
                                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                onlinettrans.FMA_Id = groupwisefmaids[s].FCMAS_Id;
                                onlinettrans.FTOT_Amount = fineamountcal;
                                onlinettrans.FTOT_Created_date = indianTime;
                                onlinettrans.FTOT_Updated_date = indianTime;
                                onlinettrans.FTOT_Concession = 0;
                                onlinettrans.FTOT_Fine = 0;
                                _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                            }
                        }
                    }

                    //commented on 19012018
                    onlinemtrans.FMOT_Amount = Convert.ToInt32(rece_amt.amt) + Convert.ToInt32(fineamountcal);

                    groupidss = "0";
                    fineamountcal = 0;
                    finecount = 0;
                }


                var contactexisttransaction = 0;

                using (var dbCtxTxn = _mastercourse.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _mastercourse.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

                if (dynamicrecgen.Count() > 0)
                {
                    data.recenocol = dynamicrecgen.ToArray();
                }

            }
            catch (Exception ex)
            {

            }

            return data;
        }


        public CollegeFeeTransactionDTO paymentPartEasebuzz(CollegeFeeTransactionDTO enq)
        {

            var fetchecs = (from a in _mastercourse.Adm_Master_College_StudentDMO
                            from b in _mastercourse.Adm_College_Yearly_StudentDMO
                            from c in _mastercourse.MasterCourseDMO
                            from d in _mastercourse.ClgMasterBranchDMO
                            from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                            where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ASMAY_Id == enq.ASMAY_Id && a.MI_Id == enq.MI_Id && a.AMCST_Id == enq.AMCST_Id)
                            select new CollegeFeeTransactionDTO
                            {
                                AMCST_Id = a.AMCST_Id,

                                AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) +
                                        (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) +
                                        (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),

                                AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                AMCST_AdmNo = a.AMCST_AdmNo,
                                ACYST_RollNo = b.ACYST_RollNo,
                                AMCO_CourseName = c.AMCO_CourseName,
                                AMB_BranchName = d.AMB_BranchName,
                                AMSE_SEMName = e.AMSE_SEMName,
                                AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                AMCST_emailId = a.AMCST_emailId,
                                AMSE_Id = e.AMSE_Id,
                                AMCO_Id = c.AMCO_Id,
                                AMB_Id = d.AMB_Id
                            }
             ).ToList();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                long yrid = getacademicyearcongig(enq);

                enq.ASMAY_Id = yrid;
                enq.grpidss = "0";
                string headidss = "0";
                string instidss = "0";


                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();
                List<long> instids = new List<long>();
                string groupids = "0";
                string headidsstr = "0";



                foreach (var x in enq.selected_list)
                {
                    grpids.Add(x.FMG_Id);
                    headids.Add(x.FMH_Id);
                    instids.Add(x.FTI_Id);
                    enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                    headidss = headidss + "," + x.FMH_Id;
                    instidss = instidss + "," + x.FTI_Id;

                }
                //adv
                enq.advgrpid = "0";
                string advheadid = "0";
                string advinstid = "0";
                long tobepaid = 0;
                List<long> grpidadv = new List<long>();
                List<long> headidadv = new List<long>();
                List<long> instidadv = new List<long>();
                foreach (var x in enq.advancedata)
                {
                    grpidadv.Add(x.FMG_Id);
                    headidadv.Add(x.FMH_Id);
                    enq.advgrpid = enq.advgrpid + "," + x.FMG_Id;
                    advheadid = advheadid + "," + x.FMH_Id;
                    advinstid = advinstid + "," + x.FTI_Id;
                    tobepaid = tobepaid + x.FCSS_ToBePaid;
                }
                //adv

                string fpgdids = "";
                var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _mastercourse.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _mastercourse.Fee_College_Student_StatusDMO
                                     from c in _mastercourse.Fee_PaymentGateway_Details
                                     where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id) && instids.Contains(b.FTI_Id))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FCSS_ToBePaid = b.FCSS_ToBePaid,
                                         FPGD_SubMerchantId = c.FPGD_SubMerchantId,
                                     }
          ).Sum(t => t.FCSS_ToBePaid);


                List<multipleaccountscashfree> accountdetails = new List<multipleaccountscashfree>();
                List<multipleaccountscashfree> accountdetailsadv = new List<multipleaccountscashfree>();

                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CollegeOnlineFeeAccountDetailsInstallmentwise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                    {
                        Value = enq.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                            SqlDbType.BigInt)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                            SqlDbType.BigInt)
                    {
                        Value = enq.AMCST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMG_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = enq.grpidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMH_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = headidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@GATEWAYNAME",
                           SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });
                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = instidss
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject1 = new List<dynamic>();
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }



                                accountdetails.Add(new multipleaccountscashfree
                                {
                                    amount = Convert.ToInt64(dataReader["FCS_Tobepaid"].ToString()),
                                    vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                });

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CollegeOnlineFeeAccountDetailsInstallmentwise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.AMCST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMG_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = enq.advgrpid
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMH_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = advheadid
                    });
                    cmd.Parameters.Add(new SqlParameter("@GATEWAYNAME",
                           SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });
                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = advinstid
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject1 = new List<dynamic>();
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }



                                accountdetailsadv.Add(new multipleaccountscashfree
                                {
                                    amount = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
                                    vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                });

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }






                var showstudetails = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                      from b in _mastercourse.Fee_College_Student_StatusDMO
                                      from c in _mastercourse.Fee_PaymentGateway_Details
                                      where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FCMAS_Id = b.FCMAS_Id,
                                      }
          ).ToList();


                List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                var fine_amount = 0;
                var retObject = new List<dynamic>();
                foreach (var x in showstudetails)
                {
                    CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                    using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sp_Calculate_Fine_CLG";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@On_Date",
                            SqlDbType.DateTime)
                        {
                            Value = indianTime
                        });

                        cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                           SqlDbType.BigInt)
                        {
                            Value = x.FCMAS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.BigInt)
                        {
                            Value = enq.AMCST_Id
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

                        sew.FCMAS_Id = x.FCMAS_Id;
                        sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                        fines_fma_ids.Add(sew);
                    }
                }


                int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    value = (((Convert.ToInt32(enq.pendingamount)) + fine_amount) + (Convert.ToInt32(tobepaid))).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });

                List<multipleaccountscashfree> mulacc = new List<multipleaccountscashfree>();
                Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();

                if (accountdetails.Count() > 0)
                {
                    foreach (var q in accountdetails)
                    {
                        mulacc.Add(q);
                    }
                }
                if (accountdetailsadv.Count() > 0)
                {
                    foreach (var q in accountdetailsadv)
                    {
                        mulacc.Add(q);
                    }
                }


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }
                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                string orderId;


                string split = "";
                if (mulacc.Count > 0)
                {
                    split += "{";
                    for (var j = 0; j < mulacc.Count; j++)
                    {
                        split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                        transfersnotescash.Add(mulacc[j].vendor_id.ToString(), mulacc[j].amount);

                    }

                    split = split.Substring(0, (split.Length - 1));




                    split += "}";
                }


                var myContent = JsonConvert.SerializeObject(transfersnotescash);
                String postData = myContent;
                //String postData = res;
                string split_payments = postData;



                string amount = (totpayableamount).ToString();
                string firstname = fetchecs[0].AMCST_FirstName;
                string email = fetchecs[0].AMCST_emailId;
                string phone = (fetchecs[0].AMCST_MobileNo).ToString();

                string surl = "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponseeasybuzz/";
                string furl = "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponseeasybuzz/";



                enq.trans_id = "EASEBUZZ" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                string Txnid = (enq.trans_id).ToString();
                string UDF1 = (enq.AMCST_Id).ToString();

                string UDF2 = (enq.grpidss).ToString();

                string UDF3 = (enq.MI_Id).ToString();

                string UDF4 = (enq.ASMAY_Id).ToString();

                string UDF5 = (headidss).ToString();

                string UDF6 = "";

                string UDF7 = "";
                string UDF8 = "";
                string UDF9 = "";
                string UDF10 = "";
                string productinfo = (fetchecs[0].AMSE_Id.ToString());
                string Show_payment_mode = "";

                //Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();
                //string split = "";

                //string split_payments = split.ToString();
                string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                Easebuzz t1 = new Easebuzz(secret, key, env);
                //split_payments = "";
                // sub_merchant_id = "";


                string strForm = t1.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6, UDF7, UDF8, UDF9, UDF10, Show_payment_mode, split_payments, sub_merchant_id);

                //enq.FYP_Tot_Amount = totpayableamount;
                //enq.FYP_Tot_Amount = totpayableamount;
                enq.strForm = strForm;
                enq.FMA_Amount = totpayableamount;


                get_grp_reptno(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return enq;
        }


        public PaymentDetails.easybuzz PaymentEasebuzzResponse(PaymentDetails.easybuzz response)
        {
            try
            {
                string paymentref = "";
                FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
                PaymentDetails dto = new PaymentDetails();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //single account added on 17/12/2019

                var accountvalidation = (from a in _mastercourse.Fee_PaymentGateway_Details
                                         where (a.MI_Id == Convert.ToInt64(response.UDF3.ToString()) && a.FPGD_PGName == "EASEBUZZ")
                                         select new FeeStudentTransactionDTO
                                         {
                                             FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                         }).Distinct().ToArray();



                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == Convert.ToInt64(response.UDF3.ToString()) && t.FPGD_PGName == "EASEBUZZ").Distinct().ToList();


                //FETCH SUBMERCHANT IDS

                var fetchfmhotid = (from a in _mastercourse.Fee_M_Online_TransactionDMO
                                    where (a.FMOT_Trans_Id == response.txnid.ToString() && a.FMOT_Amount > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FMA_Amount = a.FMOT_Amount,
                                        MI_Id = a.MI_Id,
                                        ASMAY_Id = a.ASMAY_ID,
                                        Amst_Id = a.AMST_Id,

                                    }).ToArray();

                var fetchstudentdeatils = (from a in _mastercourse.Adm_Master_College_StudentDMO
                                           from b in _mastercourse.Adm_College_Yearly_StudentDMO
                                           where (a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && b.ASMAY_Id == Convert.ToInt64(fetchfmhotid[0].ASMAY_Id) && a.MI_Id == Convert.ToInt64(fetchfmhotid[0].MI_Id))
                                           select new CollegeFeeTransactionDTO
                                           {
                                               Mobile_No = a.AMCST_MobileNo,
                                               AMCST_emailId = a.AMCST_emailId,
                                               AMCO_Id = b.AMCO_Id,
                                               AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) +
                                        (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) +
                                        (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),
                                               AMCST_AdmNo = a.AMCST_AdmNo,
                                               AMCST_Id = a.AMCST_Id
                                           }).ToArray();

                Dictionary<String, object> transfers = new Dictionary<String, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                List<FeeStudentTransactionDTO> fetchaccountid = new List<FeeStudentTransactionDTO>();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    transfers.Clear();
                    transfersnotes.Clear();

                    fetchaccountid = (from a in _mastercourse.Fee_T_Online_TransactionDMO
                                      from f in _mastercourse.Clg_Fee_AmountEntry_DMO
                                      from b in _mastercourse.CLG_Fee_College_Master_Amount_Semesterwise
                                      from c in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                      from d in _mastercourse.Fee_PaymentGateway_Details
                                      from e in _mastercourse.PAYUDETAILS
                                      where (f.FCMA_Id == b.FCMA_Id && a.FMA_Id == b.FCMAS_Id && f.FMG_Id == c.fmg_id && f.FMH_Id == c.FMH_Id && f.FTI_Id == c.fti_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && e.IMPG_PGFlag == "EASEBUZZ" && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(fetchfmhotid[r].MI_Id))
                                      select new FeeStudentTransactionDTO
                                      {
                                          FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                      }).Distinct().ToList();

                    var fetchamount = (from a in _mastercourse.Fee_M_Online_TransactionDMO
                                       where (a.AMST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && a.FMOT_Trans_Id == response.txnid.ToString() && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMA_Amount = a.FMOT_Amount
                                       }).ToArray();



                    string txnid = response.txnid.ToString();
                    string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                    string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                    string env = "prod";

                    Easebuzz t = new Easebuzz(secret, key, env);

                    var res = t.transactionAPI(txnid, response.amount, response.email, response.phone);

                    JObject joResponse1 = JObject.Parse(res);

                   paymentref = joResponse1["msg"]["easepayid"].ToString();
                  

                }

                if (response.status == "success")
                {
                    var groups = (from a in _mastercourse.Fee_Y_PaymentDMO
                                  from b in _mastercourse.Fee_Y_Payment_PaymentModeDMO
                                  where (a.MI_Id == Convert.ToInt32(fetchfmhotid[0].MI_Id) && a.ASMAY_Id == Convert.ToInt32(fetchfmhotid[0].ASMAY_Id) && b.FYPPM_Transaction_Id == response.txnid)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FYP_Receipt_No = a.FYP_ReceiptNo
                                  }
                          ).Distinct().ToList();

                    if (groups.Count == 0)
                    {
                        var confirmstatus = insertdatainfeetables(fetchfmhotid[0].MI_Id.ToString(), "0", fetchfmhotid[0].Amst_Id.ToString(), "0", fetchfmhotid[0].FMA_Amount, response.txnid, paymentref, fetchfmhotid[0].ASMAY_Id.ToString(), indianTime, "0");

                        if (Convert.ToInt32(confirmstatus) > 0)
                        {
                            SMS sms = new SMS(_context);

                            sms.sendSms(Convert.ToInt32(fetchfmhotid[0].MI_Id), Convert.ToInt64(fetchstudentdeatils[0].Mobile_No), "FEEONLINEPAYMENT", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                            Email Email = new Email(_context);

                            Email.sendmail(Convert.ToInt32(fetchfmhotid[0].MI_Id), fetchstudentdeatils[0].AMCST_emailId, "FEEONLINEPAYMENT", Convert.ToInt32(fetchfmhotid[0].Amst_Id));
                        }
                    }
                }
                else
                {
                    SMS sms = new SMS(_context);

                    sms.sendSms(Convert.ToInt32(fetchfmhotid[0].MI_Id), Convert.ToInt64(fetchstudentdeatils[0].Mobile_No), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                    Email Email = new Email(_context);

                    Email.sendmail(Convert.ToInt32(fetchfmhotid[0].MI_Id), fetchstudentdeatils[0].AMCST_emailId, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                    dto.status = response.status;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }



        public CollegeFeeTransactionDTO paymentPartEasebuzzPartial(CollegeFeeTransactionDTO enq)
        {

            var fetchecs = (from a in _mastercourse.Adm_Master_College_StudentDMO
                            from b in _mastercourse.Adm_College_Yearly_StudentDMO
                            from c in _mastercourse.MasterCourseDMO
                            from d in _mastercourse.ClgMasterBranchDMO
                            from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                            where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ASMAY_Id == enq.ASMAY_Id && a.MI_Id == enq.MI_Id && a.AMCST_Id == enq.AMCST_Id)
                            select new CollegeFeeTransactionDTO
                            {
                                AMCST_Id = a.AMCST_Id,

                                AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) +
                                        (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) +
                                        (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),

                                AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                AMCST_AdmNo = a.AMCST_AdmNo,
                                ACYST_RollNo = b.ACYST_RollNo,
                                AMCO_CourseName = c.AMCO_CourseName,
                                AMB_BranchName = d.AMB_BranchName,
                                AMSE_SEMName = e.AMSE_SEMName,
                                AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                AMCST_emailId = a.AMCST_emailId,
                                AMSE_Id = e.AMSE_Id,
                                AMCO_Id = c.AMCO_Id,
                                AMB_Id = d.AMB_Id
                            }
             ).ToList();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                long yrid = getacademicyearcongig(enq);

                enq.ASMAY_Id = yrid;
                enq.grpidss = "0";
                string headidss = "0";
                string instidss = "0";


                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();
                List<long> instids = new List<long>();
                string groupids = "0";
                string headidsstr = "0";

                Dictionary<long, long> groupdetailids = new Dictionary<long, long>();
                Dictionary<long, long> groupdetailidsadv = new Dictionary<long, long>();


                foreach (var x in enq.selected_list)
                {

                    if (groupdetailids.Count == 0 || groupdetailids == null)
                    {
                        groupdetailids.Add(x.FMG_Id, x.FCSS_ToBePaid);
                    }
                    else
                    {

                        if (groupdetailids.ContainsKey(x.FMG_Id))
                        {
                            groupdetailids[x.FMG_Id] += x.FCSS_ToBePaid;
                        }
                        else
                        {
                            groupdetailids.Add(x.FMG_Id, x.FCSS_ToBePaid);
                        }

                    }

                    grpids.Add(x.FMG_Id);
                    headids.Add(x.FMH_Id);
                    instids.Add(x.FTI_Id);
                    enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                    headidss = headidss + "," + x.FMH_Id;
                    instidss = instidss + "," + x.FTI_Id;

                }
                //adv
                enq.advgrpid = "0";
                string advheadid = "0";
                string advinstid = "0";
                long tobepaid = 0;
                List<long> grpidadv = new List<long>();
                List<long> headidadv = new List<long>();
                List<long> instidadv = new List<long>();
                foreach (var x in enq.advancedata)
                {
                    if (groupdetailidsadv.Count == 0 || groupdetailidsadv == null)
                    {
                        groupdetailidsadv.Add(x.FMG_Id, x.FCSS_ToBePaid);
                    }
                    else
                    {

                        if (groupdetailidsadv.ContainsKey(x.FMG_Id))
                        {
                            groupdetailidsadv[x.FMG_Id] += x.FCSS_ToBePaid;
                        }
                        else
                        {
                            groupdetailidsadv.Add(x.FMG_Id, x.FCSS_ToBePaid);
                        }

                    }
                    grpidadv.Add(x.FMG_Id);
                    headidadv.Add(x.FMH_Id);
                    enq.advgrpid = enq.advgrpid + "," + x.FMG_Id;
                    advheadid = advheadid + "," + x.FMH_Id;
                    advinstid = advinstid + "," + x.FTI_Id;
                    tobepaid = tobepaid + x.FCSS_ToBePaid;
                }
                //adv

                string fpgdids = "";
                var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _mastercourse.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _mastercourse.Fee_College_Student_StatusDMO
                                     from c in _mastercourse.Fee_PaymentGateway_Details
                                     where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id) && instids.Contains(b.FTI_Id))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FCSS_ToBePaid = b.FCSS_ToBePaid,
                                         FPGD_SubMerchantId = c.FPGD_SubMerchantId,
                                     }
          ).Sum(t => t.FCSS_ToBePaid);


                List<multipleaccountscashfree> accountdetails = new List<multipleaccountscashfree>();
                List<multipleaccountscashfree> accountdetailsadv = new List<multipleaccountscashfree>();




                if (groupdetailids.Count > 0)
                {
                    foreach (KeyValuePair<long, long> entry in groupdetailids)
                    {
                        using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "CollegeOnlineFeeAccountDetailsGroupwise";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                    SqlDbType.BigInt)
                            {
                                Value = enq.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                    SqlDbType.BigInt)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                    SqlDbType.BigInt)
                            {
                                Value = enq.AMCST_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@FMG_Ids",
                                    SqlDbType.VarChar)
                            {
                                Value = entry.Key
                            });

                            cmd.Parameters.Add(new SqlParameter("@GATEWAYNAME",
                                   SqlDbType.VarChar)
                            {
                                Value = enq.onlinepaygteway
                            });
                            cmd.Parameters.Add(new SqlParameter("@Amount",
                                    SqlDbType.VarChar)
                            {
                                Value = entry.Value
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject1 = new List<dynamic>();
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
                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                            );
                                        }



                                        accountdetails.Add(new multipleaccountscashfree
                                        {
                                            amount = Convert.ToInt64(dataReader["FCSS_ToBePaid"].ToString()),
                                            vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                        });

                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                }



                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CollegeOnlineFeeAccountDetailsInstallmentwise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.AMCST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMG_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = enq.advgrpid
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMH_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = advheadid
                    });
                    cmd.Parameters.Add(new SqlParameter("@GATEWAYNAME",
                           SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });
                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = advinstid
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject1 = new List<dynamic>();
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }



                                accountdetailsadv.Add(new multipleaccountscashfree
                                {
                                    amount = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
                                    vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                });

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // List<multipleaccountscashfree> Accountdetailsfinal = new List<multipleaccountscashfree>();
                Dictionary<string, long> Accountdetailsfinal = new Dictionary<string, long>();

                long totalamtnew = 0;

                foreach (var entry in accountdetails)
                {
                    if (Accountdetailsfinal.Count == 0 || Accountdetailsfinal == null)
                    {
                        Accountdetailsfinal.Add(entry.vendor_id, entry.amount);
                        totalamtnew += entry.amount;
                    }
                    else
                    {

                        if (Accountdetailsfinal.ContainsKey(entry.vendor_id))
                        {
                            Accountdetailsfinal[entry.vendor_id] += entry.amount;
                            totalamtnew += entry.amount;
                        }
                        else
                        {
                            Accountdetailsfinal.Add(entry.vendor_id, entry.amount);
                            totalamtnew += entry.amount;
                        }

                    }

                }


                var showstudetails = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                      from b in _mastercourse.Fee_College_Student_StatusDMO
                                      from c in _mastercourse.Fee_PaymentGateway_Details
                                      where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FCMAS_Id = b.FCMAS_Id,
                                      }
      ).ToList();


                List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                var fine_amount = 0;
                var retObject = new List<dynamic>();
                foreach (var x in showstudetails)
                {
                    CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                    using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sp_Calculate_Fine_CLG";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@On_Date",
                            SqlDbType.DateTime)
                        {
                            Value = indianTime
                        });

                        cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                           SqlDbType.BigInt)
                        {
                            Value = x.FCMAS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.BigInt)
                        {
                            Value = enq.AMCST_Id
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

                        sew.FCMAS_Id = x.FCMAS_Id;
                        sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                        fines_fma_ids.Add(sew);
                    }
                }


                int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    value = (((Convert.ToInt32(totalamtnew)) + fine_amount) + (Convert.ToInt32(tobepaid))).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });

                List<multipleaccountscashfree> mulacc = new List<multipleaccountscashfree>();
                Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();

                if (accountdetails.Count() > 0)
                {
                    foreach (var q in accountdetails)
                    {
                        mulacc.Add(q);
                    }
                }
                if (accountdetailsadv.Count() > 0)
                {
                    foreach (var q in accountdetailsadv)
                    {
                        mulacc.Add(q);
                    }
                }


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }
                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                string orderId;


                string split = "";
                //if (mulacc.Count > 0)
                //{
                //    split += "{";
                //    for (var j = 0; j < mulacc.Count; j++)
                //    {
                //        split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                //        transfersnotescash.Add(mulacc[j].vendor_id.ToString(), mulacc[j].amount);

                //    }

                //    split = split.Substring(0, (split.Length - 1));




                //    split += "}";
                //}


                var myContent = JsonConvert.SerializeObject(Accountdetailsfinal);
                String postData = myContent;
                //String postData = res;
                string split_payments = postData;



                string amount = (totpayableamount).ToString();
                string firstname = fetchecs[0].AMCST_FirstName;
                string email = fetchecs[0].AMCST_emailId;
                string phone = (fetchecs[0].AMCST_MobileNo).ToString();

                string surl = "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponseeasybuzz/";
                string furl = "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponseeasybuzz/";



                enq.trans_id = "EASEBUZZ" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                string Txnid = (enq.trans_id).ToString();
                string UDF1 = (enq.AMCST_Id).ToString();

                string UDF2 = (enq.grpidss).ToString();

                string UDF3 = (enq.MI_Id).ToString();

                string UDF4 = (enq.ASMAY_Id).ToString();

                string UDF5 = (headidss).ToString();

                string UDF6 = "";

                string UDF7 = "";
                string UDF8 = "";
                string UDF9 = "";
                string UDF10 = "";
                string productinfo = (fetchecs[0].AMSE_Id.ToString());
                string Show_payment_mode = "";


                string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                Easebuzz t1 = new Easebuzz(secret, key, env);



                string strForm = t1.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6, UDF7, UDF8, UDF9, UDF10, Show_payment_mode, split_payments, sub_merchant_id);

                enq.strForm = strForm;
                enq.FMA_Amount = totpayableamount;


                get_grp_reptnopartial(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return enq;
        }

        public CollegeFeeTransactionDTO paymentPartEasebuzzStudentwisePartial(CollegeFeeTransactionDTO enq)
        {

            var fetchecs = (from a in _mastercourse.Adm_Master_College_StudentDMO
                            from b in _mastercourse.Adm_College_Yearly_StudentDMO
                            from c in _mastercourse.MasterCourseDMO
                            from d in _mastercourse.ClgMasterBranchDMO
                            from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                            where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ASMAY_Id == enq.ASMAY_Id && a.MI_Id == enq.MI_Id && a.AMCST_Id == enq.AMCST_Id)
                            select new CollegeFeeTransactionDTO
                            {
                                AMCST_Id = a.AMCST_Id,

                                AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) +
                                        (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) +
                                        (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),

                                AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                AMCST_AdmNo = a.AMCST_AdmNo,
                                ACYST_RollNo = b.ACYST_RollNo,
                                AMCO_CourseName = c.AMCO_CourseName,
                                AMB_BranchName = d.AMB_BranchName,
                                AMSE_SEMName = e.AMSE_SEMName,
                                AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                AMCST_emailId = a.AMCST_emailId,
                                AMSE_Id = e.AMSE_Id,
                                AMCO_Id = c.AMCO_Id,
                                AMB_Id = d.AMB_Id
                            }
             ).ToList();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                long yrid = getacademicyearcongig(enq);

                enq.ASMAY_Id = yrid;
                enq.grpidss = "0";
                string headidss = "0";
                string instidss = "0";


                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();
                List<long> instids = new List<long>();
                string groupids = "0";
                string headidsstr = "0";



                foreach (var x in enq.selected_list)
                {
                    grpids.Add(x.FMG_Id);
                    headids.Add(x.FMH_Id);
                    instids.Add(x.FTI_Id);
                    enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                    headidss = headidss + "," + x.FMH_Id;
                    instidss = instidss + "," + x.FTI_Id;

                }
                //adv
                enq.advgrpid = "0";
                string advheadid = "0";
                string advinstid = "0";
                long tobepaid = 0;
                List<long> grpidadv = new List<long>();
                List<long> headidadv = new List<long>();
                List<long> instidadv = new List<long>();
                foreach (var x in enq.advancedata)
                {
                    grpidadv.Add(x.FMG_Id);
                    headidadv.Add(x.FMH_Id);
                    enq.advgrpid = enq.advgrpid + "," + x.FMG_Id;
                    advheadid = advheadid + "," + x.FMH_Id;
                    advinstid = advinstid + "," + x.FTI_Id;
                    tobepaid = tobepaid + x.FCSS_ToBePaid;
                }
                //adv

                string fpgdids = "";
                var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _mastercourse.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _mastercourse.Fee_College_Student_StatusDMO
                                     from c in _mastercourse.Fee_PaymentGateway_Details
                                     where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id) && instids.Contains(b.FTI_Id))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FCSS_ToBePaid = b.FCSS_ToBePaid,
                                         FPGD_SubMerchantId = c.FPGD_SubMerchantId,
                                     }
          ).Sum(t => t.FCSS_ToBePaid);


                List<multipleaccountscashfree> accountdetails = new List<multipleaccountscashfree>();
                List<multipleaccountscashfree> accountdetailsadv = new List<multipleaccountscashfree>();

                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CollegeOnlineFeeAccountDetailsInstallmentwise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                    {
                        Value = enq.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                            SqlDbType.BigInt)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                            SqlDbType.BigInt)
                    {
                        Value = enq.AMCST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMG_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = enq.grpidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMH_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = headidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@GATEWAYNAME",
                           SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });
                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = instidss
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject1 = new List<dynamic>();
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }



                                accountdetails.Add(new multipleaccountscashfree
                                {
                                    amount = Convert.ToInt64(dataReader["FCS_Tobepaid"].ToString()),
                                    vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                });

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CollegeOnlineFeeAccountDetailsInstallmentwise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                            SqlDbType.VarChar)
                    {
                        Value = enq.AMCST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMG_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = enq.advgrpid
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMH_Ids",
                            SqlDbType.VarChar)
                    {
                        Value = advheadid
                    });
                    cmd.Parameters.Add(new SqlParameter("@GATEWAYNAME",
                           SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });
                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = advinstid
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject1 = new List<dynamic>();
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }



                                accountdetailsadv.Add(new multipleaccountscashfree
                                {
                                    amount = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
                                    vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                });

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }






                var showstudetails = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                      from b in _mastercourse.Fee_College_Student_StatusDMO
                                      from c in _mastercourse.Fee_PaymentGateway_Details
                                      where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FCMAS_Id = b.FCMAS_Id,
                                      }
          ).ToList();


                List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                var fine_amount = 0;
                var retObject = new List<dynamic>();
                foreach (var x in showstudetails)
                {
                    CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                    using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sp_Calculate_Fine_CLG";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@On_Date",
                            SqlDbType.DateTime)
                        {
                            Value = indianTime
                        });

                        cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                           SqlDbType.BigInt)
                        {
                            Value = x.FCMAS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.BigInt)
                        {
                            Value = enq.AMCST_Id
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

                        sew.FCMAS_Id = x.FCMAS_Id;
                        sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                        fines_fma_ids.Add(sew);
                    }
                }


                int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    value = (((Convert.ToInt32(enq.pendingamount)) + fine_amount) + (Convert.ToInt32(tobepaid))).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });

                List<multipleaccountscashfree> mulacc = new List<multipleaccountscashfree>();
                Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();

                if (accountdetails.Count() > 0)
                {
                    foreach (var q in accountdetails)
                    {
                        mulacc.Add(q);
                    }
                }
                if (accountdetailsadv.Count() > 0)
                {
                    foreach (var q in accountdetailsadv)
                    {
                        mulacc.Add(q);
                    }
                }


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }
                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                string orderId;


                string split = "";
                if (mulacc.Count > 0)
                {
                    split += "{";
                    for (var j = 0; j < mulacc.Count; j++)
                    {
                        split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                        transfersnotescash.Add(mulacc[j].vendor_id.ToString(), mulacc[j].amount);

                    }

                    split = split.Substring(0, (split.Length - 1));




                    split += "}";
                }


                var myContent = JsonConvert.SerializeObject(transfersnotescash);
                String postData = myContent;
                //String postData = res;
                string split_payments = postData;



                string amount = (totpayableamount).ToString();
                string firstname = fetchecs[0].AMCST_FirstName;
                string email = fetchecs[0].AMCST_emailId;
                string phone = (fetchecs[0].AMCST_MobileNo).ToString();

                string surl = "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponseeasybuzz/";
                string furl = "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponseeasybuzz/";



                enq.trans_id = "EASEBUZZ" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                string Txnid = (enq.trans_id).ToString();
                string UDF1 = (enq.AMCST_Id).ToString();

                string UDF2 = (enq.grpidss).ToString();

                string UDF3 = (enq.MI_Id).ToString();

                string UDF4 = (enq.ASMAY_Id).ToString();

                string UDF5 = (headidss).ToString();

                string UDF6 = "";

                string UDF7 = "";
                string UDF8 = "";
                string UDF9 = "";
                string UDF10 = "";
                string productinfo = (fetchecs[0].AMSE_Id.ToString());
                string Show_payment_mode = "";


                string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                Easebuzz t1 = new Easebuzz(secret, key, env);



                string strForm = t1.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6, UDF7, UDF8, UDF9, UDF10, Show_payment_mode, split_payments, sub_merchant_id);

                enq.strForm = strForm;
                enq.FMA_Amount = totpayableamount;


                get_grp_reptno(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return enq;
        }


        public CollegeFeeTransactionDTO get_grp_reptnopartial(CollegeFeeTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string onlineheadmapid = "0", groupidss = "0", instidss = "0";

                long fineamountcal = 0, fethchfmaidsfine = 0, finecount = 0;

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                List<long> HeadId = new List<long>();
                List<long> grpid = new List<long>();
                List<long> instids = new List<long>();

                foreach (var item in data.selected_list)
                {
                    HeadId.Add(item.FMH_Id);
                    grpid.Add(item.FMG_Id);
                    instids.Add(item.FTI_Id);
                    groupidss = groupidss + ',' + item.FMG_Id;
                    instidss = instidss + "," + item.FTI_Id;

                }

                List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();


                list_all = (from b in _mastercourse.Fee_Groupwise_AutoReceiptDMO
                            from c in _mastercourse.Fee_Groupwise_AutoReceipt_GroupsDMO
                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                            select new CollegeFeeTransactionDTO
                            {
                                FGAR_PrefixName = b.FGAR_PrefixName,
                                FGAR_SuffixName = b.FGAR_SuffixName,
                                FGAR_Id = c.FGAR_Id,
                            }
                     ).Distinct().ToList();



                decimal groupwiseamount = 0;

                groupwiseamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                   from b in _mastercourse.Fee_College_Student_StatusDMO
                                   from c in _mastercourse.Fee_PaymentGateway_Details
                                   where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCST_Id == data.AMCST_Id && b.FCSS_ToBePaid > 0 && grpid.Contains(a.fmg_id) && HeadId.Contains(b.FMH_Id) && instids.Contains(b.FTI_Id))
                                   select new CollegeFeeTransactionDTO
                                   {
                                       FCSS_ToBePaid = b.FCSS_ToBePaid,
                                   }
        ).Sum(t => t.FCSS_ToBePaid);



                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "receiptnogeneration_CLG";
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
                        Value = groupidss
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

                    var rece_amt = new { receiptno = data.FYP_Receipt_No, amt = data.FMA_Amount };

                    dynamicrecgen.Add(rece_amt);

                    List<CollegeFeeTransactionDTO> groupwisefmaids = new List<CollegeFeeTransactionDTO>();

                    groupwisefmaids = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                       from c in _mastercourse.FeeHeadClgDMO
                                       from i in _mastercourse.Fee_College_Student_StatusDMO
                                       from b in _mastercourse.Fee_PaymentGateway_Details
                                       where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && grpid.Contains(a.fmg_id) && ((i.FCSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMCST_Id == data.AMCST_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && HeadId.Contains(i.FMH_Id) && instids.Contains(i.FTI_Id))
                                       select new CollegeFeeTransactionDTO
                                       {
                                           FCMAS_Id = i.FCMAS_Id,
                                           FCSS_ToBePaid = Convert.ToInt64(i.FCSS_ToBePaid),
                                           FMH_Flag = c.FMH_Flag
                                       }
                    ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
                    onlinemtrans.FMOT_Amount = rece_amt.amt;
                    onlinemtrans.FMOT_Date = indianTime;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.PASR_Id = 0;
                    onlinemtrans.AMST_Id = data.AMCST_Id;
                    onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

                    onlinemtrans.FYP_PayModeType = "APP";
                    onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;

                    onlinemtrans.MI_Id = data.MI_Id;
                    onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                    _mastercourse.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < data.selected_list.Count(); s++)
                    {
                        if (data.selected_list[s].FMH_Flag == "F")
                        {
                            fethchfmaidsfine = data.selected_list[s].FCMAS_Id;
                        }
                        else
                        {
                            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                            onlinettrans.FMA_Id = data.selected_list[s].FCMAS_Id;
                            onlinettrans.FTOT_Amount = data.selected_list[s].FCSS_ToBePaid;
                            onlinettrans.FTOT_Created_date = indianTime;
                            onlinettrans.FTOT_Updated_date = indianTime;
                            onlinettrans.FTOT_Concession = 0;
                            onlinettrans.FTOT_Fine = 0;

                            using (var cmd2 = _mastercourse.Database.GetDbConnection().CreateCommand())
                            {
                                cmd2.CommandText = "Sp_Calculate_Fine_CLG";
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Add(new SqlParameter("@On_Date",
                                    SqlDbType.DateTime, 100)
                                {
                                    //Value = curdt
                                    Value = indianTime
                                });

                                cmd2.Parameters.Add(new SqlParameter("@FCMAS_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.selected_list[s].FCMAS_Id
                                });
                                cmd2.Parameters.Add(new SqlParameter("@ASMAY_Id",
                               SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                cmd2.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.BigInt)
                                {
                                    Value = data.AMCST_Id
                                });

                                cmd2.Parameters.Add(new SqlParameter("@amt",
                        SqlDbType.Decimal, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                cmd2.Parameters.Add(new SqlParameter("@flgArr",
                   SqlDbType.Int, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                if (cmd2.Connection.State != ConnectionState.Open)
                                    cmd2.Connection.Open();

                                var execfine = cmd2.ExecuteNonQuery();

                                if (Convert.ToInt32(cmd2.Parameters["@amt"].Value) > 0)
                                {
                                    fineamountcal = fineamountcal + Convert.ToInt32(cmd2.Parameters["@amt"].Value);
                                }

                            }

                            _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                        }

                    }



                    for (int s = 0; s < data.selected_list.Count(); s++)
                    {
                        if (data.selected_list[s].FMH_Flag == "F")
                        {
                            if (finecount < 1)
                            {
                                finecount = finecount + 1;
                                fethchfmaidsfine = data.selected_list[s].FCMAS_Id;
                                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                onlinettrans.FMA_Id = data.selected_list[s].FCMAS_Id;
                                onlinettrans.FTOT_Amount = fineamountcal;
                                onlinettrans.FTOT_Created_date = indianTime;
                                onlinettrans.FTOT_Updated_date = indianTime;
                                onlinettrans.FTOT_Concession = 0;
                                onlinettrans.FTOT_Fine = 0;
                                _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                            }
                        }
                    }
                    for (int s = 0; s < data.advancedata.Count(); s++)
                    {

                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                        onlinettrans.FMA_Id = data.advancedata[s].FCMAS_Id;
                        onlinettrans.FTOT_Amount = data.advancedata[s].FCSS_ToBePaid;
                        onlinettrans.FTOT_Created_date = indianTime;
                        onlinettrans.FTOT_Updated_date = indianTime;
                        onlinettrans.FTOT_Concession = 0;
                        onlinettrans.FTOT_Fine = 0;


                        _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);


                    }

                    groupidss = "0";
                    fineamountcal = 0;
                    finecount = 0;
                }


                var contactexisttransaction = 0;

                using (var dbCtxTxn = _mastercourse.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _mastercourse.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

                if (dynamicrecgen.Count() > 0)
                {
                    data.recenocol = dynamicrecgen.ToArray();
                }

            }
            catch (Exception ex)
            {

            }

            return data;
        }

        public async Task<CollegeFeeTransactionDTO> mobilepayuconnect(CollegeFeeTransactionDTO data)
        {
            try
            {
                long yrid = getacademicyearcongig(data);

                data.ASMAY_Id = yrid;
                data.fillstudent = (from a in _mastercourse.Adm_Master_College_StudentDMO
                                    from b in _mastercourse.Adm_College_Yearly_StudentDMO
                                    from c in _mastercourse.MasterCourseDMO
                                    from d in _mastercourse.ClgMasterBranchDMO
                                    from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id)
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
                                        AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                        AMCST_emailId = a.AMCST_emailId,
                                    }
             ).ToArray();


                var config = _mastercourse.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();


                if (data.onlinepaygteway == "PAYU")
                {
                    data.paydet = paymentPart(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "BILLDESK")
                {
                    //data = paymentPartbilldesk(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "EBS")
                {
                    //data.paydet = paymentPartebs(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "PAYTM")
                {
                    data.paydet = paymentPartpaytm(data, data.topayamount);
                    //data.paydet = paymentPartpaytmmobile(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "RAZORPAY")
                {
                    data.paydet = paymentPartRazorPay(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "EASEBUZZ")
                {
                    if (config.Count > 0)
                    {


                        data = await paymentPartEasebuzzPartialMobile(data);


                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async System.Threading.Tasks.Task<CollegeFeeTransactionDTO> paymentPartEasebuzzPartialMobile(CollegeFeeTransactionDTO enq)
        {

            try
            {
                var fineamt = 0;

                var fetchecs = (from a in _mastercourse.Adm_Master_College_StudentDMO
                                from b in _mastercourse.Adm_College_Yearly_StudentDMO
                                from c in _mastercourse.MasterCourseDMO
                                from d in _mastercourse.ClgMasterBranchDMO
                                from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                                where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ASMAY_Id == enq.ASMAY_Id && a.MI_Id == enq.MI_Id && a.AMCST_Id == enq.AMCST_Id)
                                select new CollegeFeeTransactionDTO
                                {
                                    AMCST_Id = a.AMCST_Id,

                                    AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) +
                                            (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) +
                                            (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),

                                    AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                    AMCST_AdmNo = a.AMCST_AdmNo,
                                    ACYST_RollNo = b.ACYST_RollNo,
                                    AMCO_CourseName = c.AMCO_CourseName,
                                    AMB_BranchName = d.AMB_BranchName,
                                    AMSE_SEMName = e.AMSE_SEMName,
                                    AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                    AMCST_emailId = a.AMCST_emailId,
                                    AMSE_Id = e.AMSE_Id,
                                    AMCO_Id = c.AMCO_Id,
                                    AMB_Id = d.AMB_Id
                                }
           ).ToList();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                PaymentDetails PaymentDetailsDto = new PaymentDetails();
                try
                {
                    long yrid = getacademicyearcongig(enq);

                    enq.ASMAY_Id = yrid;
                    enq.grpidss = "0";
                    string headidss = "0";
                    string instidss = "0";


                    List<long> grpids = new List<long>();
                    List<long> headids = new List<long>();
                    List<long> instids = new List<long>();
                    string groupids = "0";
                    string headidsstr = "0";

                    Dictionary<long, long> groupdetailids = new Dictionary<long, long>();
                    Dictionary<long, long> groupdetailidsadv = new Dictionary<long, long>();
                    List<CollegeFeeTransactionDTO> selected_listupdated = new List<CollegeFeeTransactionDTO>();
                    List<CollegeFeeTransactionDTO> dd = new List<CollegeFeeTransactionDTO>();




                    foreach (var x in enq.selected_listgroup)
                    {



                        long instsum = 0;
                        foreach (var q in x.trm_list)
                        {

                            if (x.grp.FMG_Id == q.FMG_Id)
                            {
                                instids.Add(q.FTI_Id);
                                instsum = instsum + q.FTI_Amount;
                            }


                        }



                        var insttalmentwiseamt = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                                  from b in _mastercourse.Fee_College_Student_StatusDMO
                                                  from c in _mastercourse.Fee_PaymentGateway_Details
                                                  from d in _mastercourse.FeeHeadClgDMO
                                                  where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && x.grp.FMG_Id == a.fmg_id && instids.Contains(b.FTI_Id) && a.FMH_Id == d.FMH_Id)
                                                  select new CollegeFeeTransactionDTO
                                                  {
                                                      FCSS_ToBePaid = b.FCSS_ToBePaid,
                                                      FMH_Id = b.FMH_Id,
                                                      FMG_Id = b.FMG_Id,
                                                      FTI_Id = b.FTI_Id,
                                                      FCMAS_Id = b.FCMAS_Id,
                                                      FMH_Flag = d.FMH_Flag

                                                  }
                        ).ToList();


                        if (insttalmentwiseamt.Count > 0)
                        {
                            if (instsum > 0)
                            {
                                for (var c = 0; c < insttalmentwiseamt.Count; c++)
                                {

                                    if (instsum >= insttalmentwiseamt[c].FCSS_ToBePaid)
                                    {
                                        selected_listupdated.Add(new CollegeFeeTransactionDTO
                                        {

                                            FMG_Id = insttalmentwiseamt[c].FMG_Id,
                                            FMH_Id = insttalmentwiseamt[c].FMH_Id,
                                            FTI_Id = insttalmentwiseamt[c].FTI_Id,
                                            FCMAS_Id = insttalmentwiseamt[c].FCMAS_Id,
                                            FCMAS_Amount = insttalmentwiseamt[c].FCSS_ToBePaid,
                                            FCSS_ToBePaid = insttalmentwiseamt[c].FCSS_ToBePaid,
                                            FMH_Flag = insttalmentwiseamt[c].FMH_Flag,


                                        });





                                        instsum = instsum - insttalmentwiseamt[c].FCSS_ToBePaid;
                                    }
                                    else if (instsum == 0)
                                    {
                                        selected_listupdated.Add(new CollegeFeeTransactionDTO
                                        {

                                            FMG_Id = insttalmentwiseamt[c].FMG_Id,
                                            FMH_Id = insttalmentwiseamt[c].FMH_Id,
                                            FTI_Id = insttalmentwiseamt[c].FTI_Id,
                                            FCMAS_Id = insttalmentwiseamt[c].FCMAS_Id,
                                            FCMAS_Amount = 0,
                                            FCSS_ToBePaid = 0,
                                            FMH_Flag = insttalmentwiseamt[c].FMH_Flag,



                                        });



                                    }
                                    else if (instsum < insttalmentwiseamt[c].FCSS_ToBePaid)
                                    {


                                        selected_listupdated.Add(new CollegeFeeTransactionDTO
                                        {

                                            FMG_Id = insttalmentwiseamt[c].FMG_Id,
                                            FMH_Id = insttalmentwiseamt[c].FMH_Id,
                                            FTI_Id = insttalmentwiseamt[c].FTI_Id,
                                            FCMAS_Id = insttalmentwiseamt[c].FCMAS_Id,
                                            FCMAS_Amount = instsum,
                                            FCSS_ToBePaid = instsum,
                                            FMH_Flag = insttalmentwiseamt[c].FMH_Flag,



                                        });




                                        instsum = 0;
                                    }

                                }
                            }

                        }





                    }



                    foreach (var x in selected_listupdated)
                    {
                        if (groupdetailids.Count == 0 || groupdetailids == null)
                        {
                            groupdetailids.Add(x.FMG_Id, x.FCSS_ToBePaid);
                        }
                        else
                        {

                            if (groupdetailids.ContainsKey(x.FMG_Id))
                            {
                                groupdetailids[x.FMG_Id] += x.FCSS_ToBePaid;
                            }
                            else
                            {
                                groupdetailids.Add(x.FMG_Id, x.FCSS_ToBePaid);
                            }

                        }

                        grpids.Add(x.FMG_Id);
                        headids.Add(x.FMH_Id);
                        instids.Add(x.FTI_Id);
                        enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                        headidss = headidss + "," + x.FMH_Id;
                        instidss = instidss + "," + x.FTI_Id;

                    }

                    // enq.selected_list = selected_listupdated


                    string fpgdids = "";
                    var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                    from b in _mastercourse.Fee_PaymentGateway_Details
                                    where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                    select new CollegeFeeTransactionDTO
                                    {
                                        merchantid = b.FPGD_SubMerchantId
                                    }
                    ).Distinct().ToList();

                    fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                    enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                         from b in _mastercourse.Fee_College_Student_StatusDMO
                                         from c in _mastercourse.Fee_PaymentGateway_Details
                                         where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id) && instids.Contains(b.FTI_Id))
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FCSS_ToBePaid = b.FCSS_ToBePaid,
                                             FPGD_SubMerchantId = c.FPGD_SubMerchantId,
                                         }
              ).Sum(t => t.FCSS_ToBePaid);


                    List<multipleaccountscashfree> accountdetails = new List<multipleaccountscashfree>();
                    List<multipleaccountscashfree> accountdetailsadv = new List<multipleaccountscashfree>();




                    if (groupdetailids.Count > 0)
                    {
                        foreach (KeyValuePair<long, long> entry in groupdetailids)
                        {
                            using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CollegeOnlineFeeAccountDetailsGroupwise";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                        SqlDbType.BigInt)
                                {
                                    Value = enq.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                        SqlDbType.BigInt)
                                {
                                    Value = enq.ASMAY_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                        SqlDbType.BigInt)
                                {
                                    Value = enq.AMCST_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@FMG_Ids",
                                        SqlDbType.VarChar)
                                {
                                    Value = entry.Key
                                });

                                cmd.Parameters.Add(new SqlParameter("@GATEWAYNAME",
                                       SqlDbType.VarChar)
                                {
                                    Value = enq.onlinepaygteway
                                });
                                cmd.Parameters.Add(new SqlParameter("@Amount",
                                        SqlDbType.VarChar)
                                {
                                    Value = entry.Value
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                var retObject1 = new List<dynamic>();
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
                                                    dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                                );
                                            }



                                            accountdetails.Add(new multipleaccountscashfree
                                            {
                                                amount = Convert.ToInt64(dataReader["FCSS_ToBePaid"].ToString()),
                                                vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                            });

                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }

                    }




                    Dictionary<string, long> Accountdetailsfinal = new Dictionary<string, long>();

                    long totalamtnew = 0;

                    foreach (var entry in accountdetails)
                    {
                        if (Accountdetailsfinal.Count == 0 || Accountdetailsfinal == null)
                        {
                            Accountdetailsfinal.Add(entry.vendor_id, entry.amount);
                            totalamtnew += entry.amount;
                        }
                        else
                        {

                            if (Accountdetailsfinal.ContainsKey(entry.vendor_id))
                            {
                                Accountdetailsfinal[entry.vendor_id] += entry.amount;
                                totalamtnew += entry.amount;
                            }
                            else
                            {
                                Accountdetailsfinal.Add(entry.vendor_id, entry.amount);
                                totalamtnew += entry.amount;
                            }

                        }

                    }


                    var showstudetails = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                          from b in _mastercourse.Fee_College_Student_StatusDMO
                                          from c in _mastercourse.Fee_PaymentGateway_Details
                                          where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                          select new CollegeFeeTransactionDTO
                                          {
                                              FCMAS_Id = b.FCMAS_Id,
                                          }
          ).ToList();


                    List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                    var fine_amount = 0;
                    var retObject = new List<dynamic>();
                    foreach (var x in showstudetails)
                    {
                        CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                        using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Sp_Calculate_Fine_CLG";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime)
                            {
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                               SqlDbType.BigInt)
                            {
                                Value = x.FCMAS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                            {
                                Value = enq.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                           SqlDbType.BigInt)
                            {
                                Value = enq.AMCST_Id
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

                            sew.FCMAS_Id = x.FCMAS_Id;
                            sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            fines_fma_ids.Add(sew);
                        }
                    }


                    int autoinc = 1, totpayableamount = 0;
                    List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (((Convert.ToInt32(totalamtnew)) + fine_amount)).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });

                    List<multipleaccountscashfree> mulacc = new List<multipleaccountscashfree>();
                    Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();

                    if (accountdetails.Count() > 0)
                    {
                        foreach (var q in accountdetails)
                        {
                            mulacc.Add(q);
                        }
                    }


                    List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                    paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                    PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                    List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                    paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                                  where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                                  select new FeeStudentTransactionDTO
                                  {
                                      merchantid = a.FPGD_MerchantId,
                                      merchantkey = a.FPGD_AuthorisationKey,
                                      merchanturl = a.FPGD_URL
                                  }
                                   ).ToList();

                    PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                    foreach (FeeSlplitOnlinePayment x in result)
                    {
                        totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                    }
                    var item = new
                    {
                        paymentParts = result
                    };

                    string payinfo = JsonConvert.SerializeObject(item);

                    string orderId;


                    string split = "";


                    enq.FMA_Amount = Convert.ToInt64(totpayableamount);
                    string amount = (totpayableamount).ToString();
                    string firstname = fetchecs[0].AMCST_FirstName;
                    string email = fetchecs[0].AMCST_emailId;
                    string phone = (fetchecs[0].AMCST_MobileNo).ToString();


                    string surl = "http://localhost:57606/api/FeeOnlinePayment/getpaymentresponseeasybuzzmobile/";
                    string furl = "http://localhost:57606/api/FeeOnlinePayment/getpaymentresponseeasybuzzmobile/";



                    enq.trans_id = "EASEBUZZ" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                    string Txnid = (enq.trans_id).ToString();
                    string UDF1 = (enq.AMCST_Id).ToString();

                    string UDF2 = (enq.UserId).ToString();

                    string UDF3 = (enq.MI_Id).ToString();

                    string UDF4 = (enq.ASMAY_Id).ToString();

                    string UDF5 = "";

                    string UDF6 = "";

                    string UDF7 = "";
                    string UDF8 = "";
                    string UDF9 = "";
                    string UDF10 = "";
                    string productinfo = (fetchecs[0].AMSE_Id).ToString();
                    string Show_payment_mode = "";



                    var z = 0;
                    if (mulacc.Count > 0)
                    {


                        foreach (var entry in accountdetails)
                        {


                            if (fineamt > 0)
                            {

                                transfersnotescash.Add(entry.vendor_id.ToString(), entry.amount + fineamt);
                            }
                            else
                            {
                                transfersnotescash.Add(entry.vendor_id.ToString(), entry.amount);
                            }




                        }






                    }
                    var myContent = JsonConvert.SerializeObject(transfersnotescash);
                    String postData = myContent;

                    string split_payments = postData;

                    string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                    string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                    string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                    string url = "https://pay.easebuzz.in/payment/initiateLink";

                    string strForm = secret + "|" + Txnid + "|" + amount + "|" + productinfo + "|" + firstname + "|" + email + "|" + UDF1 + "|" + UDF2 + "|" + UDF3 + "|" + UDF4 + "|" + UDF5 + "|" + UDF6 + "|" + UDF7 + "|" + UDF8 + "|" + UDF9 + "|" + UDF10 + "|" + key;
                    string hash = SHA512Alg(strForm);


                    string hash1 = hash.ToLower();





                    var client = new HttpClient();

                    var requestnew = new HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Post,
                        RequestUri = new Uri("https://pay.easebuzz.in/payment/initiateLink"),
                        Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "key", secret },
        { "txnid", Txnid },
        { "amount", amount },
        { "productinfo", productinfo },
        { "firstname", firstname },
        { "phone", phone },
        { "email", email },
        { "surl", surl },
        { "furl", furl },
        { "hash", hash1 },
        { "udf1", UDF1 },
        { "udf2", UDF2 },
        { "udf3", UDF3 },
        { "udf4", UDF4 },
        { "udf5", UDF5 },
        { "udf6", UDF6 },
        { "udf7", UDF7 },
        { "address1", "" },
        { "address2", "" },
        { "city", "" },
        { "state", "" },
        { "country", "" },
        { "zipcode", "" },

         { "split_payments", split_payments },
         { "sub_merchant_id", sub_merchant_id }


    }),
                    };
                    using (var response123 = await client.SendAsync(requestnew))
                    {
                        response123.EnsureSuccessStatusCode();
                        var body = await response123.Content.ReadAsStringAsync();

                        Console.WriteLine(body);
                        JObject joResponse1 = JObject.Parse(body);
                        enq.easebuzzstatus = (Int64)joResponse1["status"];
                        enq.easebuzzdata = (String)joResponse1["data"];

                        enq.easebuzzenv = "production";



                    }





                    if (enq.FYP_PayModeType == "MOBILE")
                    {
                        get_grp_reptnopartialmobile(enq, selected_listupdated);
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return enq;

        }
        public async System.Threading.Tasks.Task<CollegeFeeTransactionDTO> paymentPartEasebuzzMobile(CollegeFeeTransactionDTO enq)
        {

            try
            {
                var fineamt = 0;

                var fetchecs = (from a in _mastercourse.Adm_Master_College_StudentDMO
                                from b in _mastercourse.Adm_College_Yearly_StudentDMO
                                from c in _mastercourse.MasterCourseDMO
                                from d in _mastercourse.ClgMasterBranchDMO
                                from e in _mastercourse.CLG_Adm_Master_SemesterDMO
                                where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ASMAY_Id == enq.ASMAY_Id && a.MI_Id == enq.MI_Id && a.AMCST_Id == enq.AMCST_Id)
                                select new CollegeFeeTransactionDTO
                                {
                                    AMCST_Id = a.AMCST_Id,

                                    AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) +
                                            (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) +
                                            (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),

                                    AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                    AMCST_AdmNo = a.AMCST_AdmNo,
                                    ACYST_RollNo = b.ACYST_RollNo,
                                    AMCO_CourseName = c.AMCO_CourseName,
                                    AMB_BranchName = d.AMB_BranchName,
                                    AMSE_SEMName = e.AMSE_SEMName,
                                    AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                    AMCST_emailId = a.AMCST_emailId,
                                    AMSE_Id = e.AMSE_Id,
                                    AMCO_Id = c.AMCO_Id,
                                    AMB_Id = d.AMB_Id
                                }
          ).ToList();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                PaymentDetails PaymentDetailsDto = new PaymentDetails();
                try
                {
                    long yrid = getacademicyearcongig(enq);

                    enq.ASMAY_Id = yrid;
                    enq.grpidss = "0";
                    string headidss = "0";
                    string instidss = "0";


                    List<long> grpids = new List<long>();
                    List<long> headids = new List<long>();
                    List<long> instids = new List<long>();
                    string groupids = "0";
                    string headidsstr = "0";



                    foreach (var x in enq.selected_list)
                    {
                        grpids.Add(x.FMG_Id);
                        headids.Add(x.FMH_Id);
                        instids.Add(x.FTI_Id);
                        enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                        headidss = headidss + "," + x.FMH_Id;
                        instidss = instidss + "," + x.FTI_Id;

                    }


                    string fpgdids = "";
                    var fpgdids1 = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                    from b in _mastercourse.Fee_PaymentGateway_Details
                                    where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                    select new CollegeFeeTransactionDTO
                                    {
                                        merchantid = b.FPGD_SubMerchantId
                                    }
             ).Distinct().ToList();

                    fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

                    enq.pendingamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                         from b in _mastercourse.Fee_College_Student_StatusDMO
                                         from c in _mastercourse.Fee_PaymentGateway_Details
                                         where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id) && instids.Contains(b.FTI_Id))
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FCSS_ToBePaid = b.FCSS_ToBePaid,
                                             FPGD_SubMerchantId = c.FPGD_SubMerchantId,
                                         }
              ).Sum(t => t.FCSS_ToBePaid);


                    List<multipleaccountscashfree> accountdetails = new List<multipleaccountscashfree>();
                    List<multipleaccountscashfree> accountdetailsadv = new List<multipleaccountscashfree>();

                    using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CollegeOnlineFeeAccountDetailsInstallmentwise";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.BigInt)
                        {
                            Value = enq.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                SqlDbType.BigInt)
                        {
                            Value = enq.AMCST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMG_Ids",
                                SqlDbType.VarChar)
                        {
                            Value = enq.grpidss
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMH_Ids",
                                SqlDbType.VarChar)
                        {
                            Value = headidss
                        });
                        cmd.Parameters.Add(new SqlParameter("@GATEWAYNAME",
                               SqlDbType.VarChar)
                        {
                            Value = enq.onlinepaygteway
                        });
                        cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                SqlDbType.VarChar)
                        {
                            Value = instidss
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject1 = new List<dynamic>();
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
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                        );
                                    }



                                    accountdetails.Add(new multipleaccountscashfree
                                    {
                                        amount = Convert.ToInt64(dataReader["FCS_Tobepaid"].ToString()),
                                        vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                    });

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }








                    var showstudetails = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                          from b in _mastercourse.Fee_College_Student_StatusDMO
                                          from c in _mastercourse.Fee_PaymentGateway_Details
                                          where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                          select new CollegeFeeTransactionDTO
                                          {
                                              FCMAS_Id = b.FCMAS_Id,
                                          }
              ).ToList();


                    List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                    var fine_amount = 0;
                    var retObject = new List<dynamic>();
                    foreach (var x in showstudetails)
                    {
                        CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                        using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Sp_Calculate_Fine_CLG";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime)
                            {
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                               SqlDbType.BigInt)
                            {
                                Value = x.FCMAS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                            {
                                Value = enq.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                           SqlDbType.BigInt)
                            {
                                Value = enq.AMCST_Id
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

                            sew.FCMAS_Id = x.FCMAS_Id;
                            sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            fines_fma_ids.Add(sew);
                        }
                    }


                    int autoinc = 1, totpayableamount = 0;
                    List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (((Convert.ToInt32(enq.pendingamount)) + fine_amount)).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });

                    List<multipleaccountscashfree> mulacc = new List<multipleaccountscashfree>();
                    Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();

                    if (accountdetails.Count() > 0)
                    {
                        foreach (var q in accountdetails)
                        {
                            mulacc.Add(q);
                        }
                    }
                    if (accountdetailsadv.Count() > 0)
                    {
                        foreach (var q in accountdetailsadv)
                        {
                            mulacc.Add(q);
                        }
                    }


                    List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                    paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                    PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                    List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                    paymentdet = (from a in _mastercourse.Fee_PaymentGateway_Details
                                  where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                                  select new FeeStudentTransactionDTO
                                  {
                                      merchantid = a.FPGD_MerchantId,
                                      merchantkey = a.FPGD_AuthorisationKey,
                                      merchanturl = a.FPGD_URL
                                  }
               ).ToList();

                    PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                    foreach (FeeSlplitOnlinePayment x in result)
                    {
                        totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                    }
                    var item = new
                    {
                        paymentParts = result
                    };

                    string payinfo = JsonConvert.SerializeObject(item);

                    string orderId;


                    string split = "";
                    if (mulacc.Count > 0)
                    {
                        split += "{";
                        for (var j = 0; j < mulacc.Count; j++)
                        {
                            split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                            transfersnotescash.Add(mulacc[j].vendor_id.ToString(), mulacc[j].amount);

                        }

                        split = split.Substring(0, (split.Length - 1));




                        split += "}";
                    }




                    enq.FMA_Amount = Convert.ToInt64(totpayableamount);
                    string amount = (totpayableamount).ToString();
                    string firstname = fetchecs[0].AMCST_FirstName;
                    string email = fetchecs[0].AMCST_emailId;
                    string phone = (fetchecs[0].AMCST_MobileNo).ToString();


                    string surl = "http://localhost:57606/api/FeeOnlinePayment/getpaymentresponseeasybuzzmobile/";
                    string furl = "http://localhost:57606/api/FeeOnlinePayment/getpaymentresponseeasybuzzmobile/";



                    enq.trans_id = enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                    string Txnid = (enq.trans_id).ToString();
                    string UDF1 = (enq.AMCST_Id).ToString();

                    string UDF2 = (enq.UserId).ToString();

                    string UDF3 = (enq.MI_Id).ToString();

                    string UDF4 = (enq.ASMAY_Id).ToString();

                    string UDF5 = (enq.ftiidss).ToString();

                    string UDF6 = "";

                    string UDF7 = "";
                    string UDF8 = "";
                    string UDF9 = "";
                    string UDF10 = "";
                    string productinfo = (enq.AMSE_Id).ToString();
                    string Show_payment_mode = "";




                    var z = 0;
                    if (mulacc.Count > 0)
                    {
                        split += "{";

                        for (var j = 0; j < mulacc.Count; j++)
                        {
                            if (j == 0)
                            {
                                if (fineamt > 0)
                                {
                                    split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                                    transfersnotescash.Add(mulacc[j].vendor_id.ToString(), (mulacc[j].amount + fineamt));
                                }
                            }
                            else
                            {
                                split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                                transfersnotescash.Add(mulacc[j].vendor_id.ToString(), mulacc[j].amount);
                            }



                        }

                        split = split.Substring(0, (split.Length - 1));

                        split += "}";
                    }
                    var myContent = JsonConvert.SerializeObject(transfersnotescash);
                    String postData = myContent;

                    string split_payments = postData;

                    string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                    string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                    string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                    string url = "https://pay.easebuzz.in/payment/initiateLink";

                    string strForm = secret + "|" + Txnid + "|" + amount + "|" + productinfo + "|" + firstname + "|" + email + "|" + UDF1 + "|" + UDF2 + "|" + UDF3 + "|" + UDF4 + "|" + UDF5 + "|" + UDF6 + "|" + UDF7 + "|" + UDF8 + "|" + UDF9 + "|" + UDF10 + "|" + key;
                    string hash = SHA512Alg(strForm);


                    string hash1 = hash.ToLower();





                    var client = new HttpClient();

                    var requestnew = new HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Post,
                        RequestUri = new Uri("https://pay.easebuzz.in/payment/initiateLink"),
                        Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "key", secret },
        { "txnid", Txnid },
        { "amount", amount },
        { "productinfo", productinfo },
        { "firstname", firstname },
        { "phone", phone },
        { "email", email },
        { "surl", surl },
        { "furl", furl },
        { "hash", hash1 },
        { "udf1", UDF1 },
        { "udf2", UDF2 },
        { "udf3", UDF3 },
        { "udf4", UDF4 },
        { "udf5", UDF5 },
        { "udf6", UDF6 },
        { "udf7", UDF7 },
        { "address1", "" },
        { "address2", "" },
        { "city", "" },
        { "state", "" },
        { "country", "" },
        { "zipcode", "" },

         { "split_payments", split_payments },
         { "sub_merchant_id", sub_merchant_id }


    }),
                    };
                    using (var response123 = await client.SendAsync(requestnew))
                    {
                        response123.EnsureSuccessStatusCode();
                        var body = await response123.Content.ReadAsStringAsync();

                        Console.WriteLine(body);
                        JObject joResponse1 = JObject.Parse(body);
                        enq.easebuzzstatus = (Int64)joResponse1["status"];
                        enq.easebuzzdata = (String)joResponse1["data"];

                        enq.easebuzzenv = "production";



                    }




                    if (enq.FYP_PayModeType == "MOBILE")
                    {
                        get_grp_reptno(enq);
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return enq;
        }


        public static string SHA512Alg(string inputString)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }



        public PaymentDetails.easybuzzmobile getpaymentresponseeasybuzzmobile(PaymentDetails.easybuzzmobile response)
        {

            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            if (response.payment_response.status == "success")
            {
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _mastercourse.Fee_PaymentGateway_Details.Where(q => q.MI_Id == Convert.ToInt64(response.payment_response.udf3) && q.FPGD_PGName == "EASEBUZZ").Distinct().ToList();

                string txnid = response.payment_response.txnid.ToString();
                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                string paymentref = response.payment_response.easepayid;


                var groups = (from a in _mastercourse.Fee_Y_PaymentDMO
                              where (a.MI_Id == Convert.ToInt32(response.payment_response.udf3) && a.ASMAY_Id == Convert.ToInt32(response.payment_response.udf4) && a.FYP_Transaction_Id == response.payment_response.txnid)
                              select new FeeStudentTransactionDTO
                              {
                                  FYP_Receipt_No = a.FYP_ReceiptNo
                              }
                      ).Distinct().ToList();

                if (groups.Count == 0)
                {
                    // public string insertdatainfeetables(string miid, string termid, string studentid, string classid, decimal amount, string transid, string refid, string yearid, DateTime indianTime, string transactionid)

                    var confirmstatus = insertdatainfeetables(response.payment_response.udf3, response.payment_response.udf5.ToString(), response.payment_response.udf1.ToString(), response.payment_response.productinfo.ToString(), Convert.ToDecimal(response.payment_response.amount), response.payment_response.txnid, paymentref.ToString(), response.payment_response.udf4, indianTime, "0");

                    if (Convert.ToInt32(confirmstatus) > 0)
                    {
                        pgmod.MI_Id = Convert.ToInt64(response.payment_response.udf3);
                        pgmod.amst_mobile = Convert.ToInt64(response.payment_response.phone);
                        pgmod.Amst_Id = Convert.ToInt64(response.payment_response.udf3);
                        pgmod.amst_email_id = response.payment_response.email;
                        response.status = "true";
                        SMS sms = new SMS(_context);

                        sms.sendSms(Convert.ToInt32(response.payment_response.udf3), Convert.ToInt64(response.payment_response.phone), "FEEONLINEPAYMENT", Convert.ToInt32(response.payment_response.udf2));

                        Email Email = new Email(_context);

                        Email.sendmail(Convert.ToInt32(response.payment_response.udf3), response.payment_response.email, "FEEONLINEPAYMENT", Convert.ToInt32(response.payment_response.udf2));

                    }
                }
            }
            else
            {
                response.status = "failed";
                SMS sms = new SMS(_context);

                sms.sendSms(Convert.ToInt64(response.payment_response.udf4), Convert.ToInt64(response.payment_response.phone), "FEEONLINEPAYMENTFAIL", Convert.ToInt64(response.payment_response.udf2));

                Email Email = new Email(_context);

                Email.sendmail(Convert.ToInt64(response.payment_response.udf4), response.payment_response.email, "FEEONLINEPAYMENTFAIL", Convert.ToInt64(response.payment_response.udf2));

                dto.status = response.payment_response.status;
            }

            return response;
        }


        public CollegeFeeTransactionDTO get_grp_reptnopartialmobile(CollegeFeeTransactionDTO data, List<CollegeFeeTransactionDTO> selecteditem)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string onlineheadmapid = "0", groupidss = "0", instidss = "0";

                long fineamountcal = 0, fethchfmaidsfine = 0, finecount = 0;

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                List<long> HeadId = new List<long>();
                List<long> grpid = new List<long>();
                List<long> instids = new List<long>();

                foreach (var item in selecteditem)
                {
                    HeadId.Add(item.FMH_Id);
                    grpid.Add(item.FMG_Id);
                    instids.Add(item.FTI_Id);
                    groupidss = groupidss + ',' + item.FMG_Id;
                    instidss = instidss + "," + item.FTI_Id;

                }

                List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();


                list_all = (from b in _mastercourse.Fee_Groupwise_AutoReceiptDMO
                            from c in _mastercourse.Fee_Groupwise_AutoReceipt_GroupsDMO
                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                            select new CollegeFeeTransactionDTO
                            {
                                FGAR_PrefixName = b.FGAR_PrefixName,
                                FGAR_SuffixName = b.FGAR_SuffixName,
                                FGAR_Id = c.FGAR_Id,
                            }
                     ).Distinct().ToList();



                decimal groupwiseamount = 0;

                groupwiseamount = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                   from b in _mastercourse.Fee_College_Student_StatusDMO
                                   from c in _mastercourse.Fee_PaymentGateway_Details
                                   where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCST_Id == data.AMCST_Id && b.FCSS_ToBePaid > 0 && grpid.Contains(a.fmg_id) && HeadId.Contains(b.FMH_Id) && instids.Contains(b.FTI_Id))
                                   select new CollegeFeeTransactionDTO
                                   {
                                       FCSS_ToBePaid = b.FCSS_ToBePaid,
                                   }
        ).Sum(t => t.FCSS_ToBePaid);



                using (var cmd = _mastercourse.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "receiptnogeneration_CLG";
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
                        Value = groupidss
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

                    var rece_amt = new { receiptno = data.FYP_Receipt_No, amt = data.FMA_Amount };

                    dynamicrecgen.Add(rece_amt);

                    List<CollegeFeeTransactionDTO> groupwisefmaids = new List<CollegeFeeTransactionDTO>();

                    groupwisefmaids = (from a in _mastercourse.CLG_Fee_OnlinePayment_MappingDMO
                                       from c in _mastercourse.FeeHeadClgDMO
                                       from i in _mastercourse.Fee_College_Student_StatusDMO
                                       from b in _mastercourse.Fee_PaymentGateway_Details
                                       where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && grpid.Contains(a.fmg_id) && ((i.FCSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMCST_Id == data.AMCST_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && HeadId.Contains(i.FMH_Id) && instids.Contains(i.FTI_Id))
                                       select new CollegeFeeTransactionDTO
                                       {
                                           FCMAS_Id = i.FCMAS_Id,
                                           FCSS_ToBePaid = Convert.ToInt64(i.FCSS_ToBePaid),
                                           FMH_Flag = c.FMH_Flag
                                       }
                    ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
                    onlinemtrans.FMOT_Amount = rece_amt.amt;
                    onlinemtrans.FMOT_Date = indianTime;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.PASR_Id = 0;
                    onlinemtrans.AMST_Id = data.AMCST_Id;
                    onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

                    onlinemtrans.FYP_PayModeType = "APP";
                    onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;

                    onlinemtrans.MI_Id = data.MI_Id;
                    onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                    _mastercourse.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < selecteditem.Count(); s++)
                    {
                        if (selecteditem[s].FMH_Flag == "F")
                        {
                            fethchfmaidsfine = selecteditem[s].FCMAS_Id;
                        }
                        else
                        {
                            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                            onlinettrans.FMA_Id = selecteditem[s].FCMAS_Id;
                            onlinettrans.FTOT_Amount = selecteditem[s].FCSS_ToBePaid;
                            onlinettrans.FTOT_Created_date = indianTime;
                            onlinettrans.FTOT_Updated_date = indianTime;
                            onlinettrans.FTOT_Concession = 0;
                            onlinettrans.FTOT_Fine = 0;

                            using (var cmd2 = _mastercourse.Database.GetDbConnection().CreateCommand())
                            {
                                cmd2.CommandText = "Sp_Calculate_Fine_CLG";
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Add(new SqlParameter("@On_Date",
                                    SqlDbType.DateTime, 100)
                                {
                                    //Value = curdt
                                    Value = indianTime
                                });

                                cmd2.Parameters.Add(new SqlParameter("@FCMAS_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = selecteditem[s].FCMAS_Id
                                });
                                cmd2.Parameters.Add(new SqlParameter("@ASMAY_Id",
                               SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                cmd2.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.BigInt)
                                {
                                    Value = data.AMCST_Id
                                });

                                cmd2.Parameters.Add(new SqlParameter("@amt",
                        SqlDbType.Decimal, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                cmd2.Parameters.Add(new SqlParameter("@flgArr",
                   SqlDbType.Int, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                if (cmd2.Connection.State != ConnectionState.Open)
                                    cmd2.Connection.Open();

                                var execfine = cmd2.ExecuteNonQuery();

                                if (Convert.ToInt32(cmd2.Parameters["@amt"].Value) > 0)
                                {
                                    fineamountcal = fineamountcal + Convert.ToInt32(cmd2.Parameters["@amt"].Value);
                                }

                            }

                            _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                        }

                    }



                    for (int s = 0; s < selecteditem.Count(); s++)
                    {
                        if (selecteditem[s].FMH_Flag == "F")
                        {
                            if (finecount < 1)
                            {
                                finecount = finecount + 1;
                                fethchfmaidsfine = data.selected_list[s].FCMAS_Id;
                                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                onlinettrans.FMA_Id = data.selected_list[s].FCMAS_Id;
                                onlinettrans.FTOT_Amount = fineamountcal;
                                onlinettrans.FTOT_Created_date = indianTime;
                                onlinettrans.FTOT_Updated_date = indianTime;
                                onlinettrans.FTOT_Concession = 0;
                                onlinettrans.FTOT_Fine = 0;
                                _mastercourse.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                            }
                        }
                    }

                    groupidss = "0";
                    fineamountcal = 0;
                    finecount = 0;
                }


                var contactexisttransaction = 0;

                using (var dbCtxTxn = _mastercourse.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _mastercourse.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

                if (dynamicrecgen.Count() > 0)
                {
                    data.recenocol = dynamicrecgen.ToArray();
                }

            }
            catch (Exception ex)
            {

            }

            return data;
        }





    }
}












