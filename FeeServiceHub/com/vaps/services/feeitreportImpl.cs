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

using System.Collections.Generic;
using System.Linq;
using System;
using DomainModel.Model.com.vaps.admission;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using CommonLibrary;


namespace FeeServiceHub.com.vaps.services
{
    public class feeitreportImpl : interfaces.feeitreportInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public feeitreportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO dt)
        {
            FeeTransactionPaymentDTO data = new FeeTransactionPaymentDTO();
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == dt.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                dt.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _FeeGroupContext.admissioncls.Where(c => c.ASMCL_ActiveFlag == true && c.MI_Id == dt.MI_ID).OrderBy(t=>t.ASMCL_Order).ToList();
                dt.fillclass = classname.GroupBy(c => c.ASMCL_ClassName).Select(c => c.First()).ToArray();




                dt.fillmasterhead =  (from a in _FeeGroupContext.FeeHeadDMO
                                           from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                           where (a.FMH_Id == b.FMH_Id && b.ASMAY_Id == dt.yearid && b.MI_Id == dt.MI_ID && a.user_id==dt.userid)
                                           select new FeeStudentGroupMappingDTO
                                           {
                                               FMH_Id = a.FMH_Id,
                                               FMH_FeeName = a.FMH_FeeName,

                                           }
                          ).Distinct().OrderBy(t => t.FMH_Id).ToArray();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }

        public FeeTransactionPaymentDTO getsection(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();
                data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMSC_Id = b.ASMS_Id,
                                        asmc_sectionname = b.ASMC_SectionName,
                                       
                                    }
                          ).Distinct().OrderBy(t=>t.AMSC_Id).ToArray();
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //public FeeTransactionPaymentDTO getstudent(FeeTransactionPaymentDTO data)
        //{
        //    List<long> GrpId = new List<long>();
        //    try
        //    {


        //       // int ecsflag = 0;
        //        List<School_M_Section> section = new List<School_M_Section>();

        //        data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
        //                            from b in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.AMSC_Id)
        //                            select new FeeTransactionPaymentDTO
        //                            {
        //                                Amst_Id = a.AMST_Id,
        //                                AMST_FirstName = a.AMST_FirstName,
        //                                AMST_MiddleName = a.AMST_MiddleName,
        //                                AMST_LastName = a.AMST_LastName,
        //                            }

        //                ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();


        //        List<FeeGroupDMO> termlistforspecial = new List<FeeGroupDMO>();
        //        termlistforspecial = _FeeGroupContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_ID && t.user_id==data.userid).ToList();


        //        data.specialheaddetails = (from a in _FeeGroupContext.feespecialHead
        //                                   from b in _FeeGroupContext.feeSGGG
        //                                   from c in _FeeGroupContext.FeeHeadDMO
        //                                   where (a.MI_Id == data.MI_ID && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_ID && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id && c.FMH_SpecialFeeFlag == true)//&& a.IVRMSTAUL_Id==data.User_Id
        //                                   select new FeeSpecialFeeGroupDTO
        //                                   {
        //                                       FMSFH_Id = a.FMSFH_Id,
        //                                       FMSFH_Name = a.FMSFH_Name,
        //                                       FMSFHFH_Id = b.FMSFHFH_Id,
        //                                       FMH_ID = b.FMH_Id,
        //                                       FMH_Name = c.FMH_FeeName
        //                                   }).Distinct().ToArray();




        //        string[] terms_group = { };

        //        List<long> terms_groups = new List<long>();
        //        for (int j = 0; j < termlistforspecial.Count(); j++)
        //        {
        //            terms_groups.Add(termlistforspecial[j].FMG_Id);
        //        }


        //        data.alldata = (from b in _FeeGroupContext.FeeGroupDMO
        //                        from c in _FeeGroupContext.FeeHeadDMO
        //                        from a in _FeeGroupContext.FeeAmountEntryDMO
        //                        from d in _FeeGroupContext.FeeInstallmentsyearlyDMO
        //                        from f in _FeeGroupContext.feeYCCC
        //                        from g in _FeeGroupContext.feeYCC
        //                        where (f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == a.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(data.ASMCL_Id) && b.MI_Id == data.MI_ID && a.FMG_Id == b.FMG_Id && c.FMH_Id==a.FMH_Id && c.MI_Id == data.MI_ID && a.ASMAY_Id==data.ASMAY_Id && b.user_id==data.userid && c.FMH_Id == c.FMH_Id && d.FTI_Id==a.FTI_Id) 
        //                        select new FeeStudentTransactionDTO
        //                        {
        //                            FMH_FeeName = c.FMH_FeeName,
        //                            FMH_Flag = c.FMH_Flag,
        //                            FTI_Name = d.FTI_Name,
        //                            FMH_Id = a.FMH_Id,
        //                            FTI_Id =a.FTI_Id,
        //                            FMG_Id = a.FMG_Id,
        //                        }
        //                        ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToArray();





        //        var specialheadlist = _FeeGroupContext.feespecialHead.Where(t => t.MI_Id == data.MI_ID && t.FMSFH_ActiceFlag == true).Distinct().ToList();
        //        data.specialheadlist = specialheadlist.ToArray();




        //        data.instalspecial = (from a in _FeeGroupContext.FeeHeadDMO
        //                              from d in _FeeGroupContext.feeMIY
        //                              from b in _FeeGroupContext.feeMTH
        //                              from c in _FeeGroupContext.FeeAmountEntryDMO
        //                              from e in _FeeGroupContext.FeeGroupDMO
        //                              from f in _FeeGroupContext.feeYCCC
        //                              from g in _FeeGroupContext.feeYCC
        //                              where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(data.ASMCL_Id) && c.MI_Id == data.MI_ID && c.ASMAY_Id == data.ASMAY_Id && terms_groups.Contains(e.FMG_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
        //                              select new Head_Installments_DTO
        //                              {
        //                                  FTI_Name = d.FTI_Name,
        //                                  FTI_Id = c.FTI_Id
        //                              }).Distinct().ToList().ToArray();

                

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}

        //public FeeTransactionPaymentDTO getreceipt(FeeTransactionPaymentDTO data)
        //{
        //    List<long> GrpId = new List<long>();
        //    try
        //    {
        //        //List<School_M_Section> section = new List<School_M_Section>();
        //        data.fillterms = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
        //                          from b in _FeeGroupContext.school_M_Section
        //                          from c in _FeeGroupContext.School_M_Class
        //                          from d in _FeeGroupContext.AdmissionStudentDMO
        //                          from e in _FeeGroupContext.FeePaymentDetailsDMO
        //                          from f in _FeeGroupContext.FeeTransactionPaymentDMO
        //                          from g in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
        //                          where (a.ASMS_Id == b.ASMS_Id && a.ASMCL_Id == c.ASMCL_Id && a.AMST_Id == d.AMST_Id && a.AMST_Id == g.AMST_Id && e.FYP_Id == g.FYP_Id && e.FYP_Id == f.FYP_Id && e.ASMAY_ID == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.AMSC_Id && e.MI_Id == data.MI_ID && g.AMST_Id==data.Amst_Id && e.FYP_OnlineChallanStatusFlag== "Sucessfull") orderby e.FYP_Receipt_No
        //                          select new FeeStudentGroupMappingDTO
        //                          {
        //                              fyp_id = e.FYP_Id,
        //                              fyp_receipt_no = e.FYP_Receipt_No
                                    
        //                          }
        //                  ).Distinct().ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}

        public async Task<FeeStudentTransactionDTO> printreceipt(FeeStudentTransactionDTO data)
        {
            try
            {
                var specialheadlist = _FeeGroupContext.feespecialHead.Where(t => t.MI_Id == data.MI_Id && t.FMSFH_ActiceFlag == true).Distinct().ToList();
                data.specialheadlist = specialheadlist.ToArray();

                if(data.returnval== "allr")
                {
                    if (data.minstall == "1")
                    {
                        data.receiptformathead = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                  from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                  from c in _FeeGroupContext.FeeTransactionPaymentDMO
                                                  from d in _FeeGroupContext.FeeAmountEntryDMO
                                                  from e in _FeeGroupContext.FeeHeadDMO
                                                  from f in _FeeGroupContext.feespecialHead
                                                  from g in _FeeGroupContext.feeSGGG
                                                  where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
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
                ).Distinct().ToArray();
                        data.fillstudentviewdetails = (from a in _FeeGroupContext.FeeTransactionPaymentDMO
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
                                                       from l in _FeeGroupContext.FeeStudentTransactionDMO
                                                       where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id && l.AMST_Id == f.AMST_Id && l.FMA_Id == c.FMA_Id && l.FMH_Id == c.FMH_Id && l.ASMAY_Id == f.ASMAY_Id && l.FTI_Id == c.FTI_Id && l.MI_Id == i.MI_Id && l.FMA_Id == a.FMA_Id)
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
                                                           FTP_Concession_Amt = l.FSS_ConcessionAmount,
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
                                                           totalcharges = c.FMA_Amount,
                                                           FSS_AdjustedAmount = l.FSS_AdjustedAmount,
                                                           amst_mobile = i.AMST_MobileNo,
                                                           FYP_ChallanNo = b.FYP_ChallanNo,
                                                           FMH_Order = d.FMH_Order,
                                                           FMA_Amount = c.FMA_Amount,
                                                           FYP_PaymentReference_Id = b.fyp_transaction_id,
                                                           FSS_ToBePaid = l.FSS_ToBePaid,
                                                       }
                 ).Distinct().OrderBy(t => t.FMH_Order).ToArray();

                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ArrearAmount_IT_Receipt";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
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
                                    data.fillstudent = retObject.Distinct().ToArray();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
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


                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
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

                        List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                        temp_group_head = (from a in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                           from b in _FeeGroupContext.Fee_Payment
                                           from c in _FeeGroupContext.FeeTransactionPaymentDMO
                                           from d in _FeeGroupContext.FeeAmountEntryDMO

                                           where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id == data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMG_Id = d.FMG_Id,

                                           }
                                   ).Distinct().ToList();

                        List<long> grp_ids = new List<long>();

                        foreach (var item in temp_group_head)
                        {
                            grp_ids.Add(item.FMG_Id);

                        }
                        var checktemplate = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                             from b in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                             where (a.FGAR_Id == b.FGAR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && grp_ids.Contains(b.FMG_Id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FMG_Id = b.FMG_Id,
                                                 receipttemplate = a.FGAR_Template_Name,
                                             }
            ).Distinct().ToArray();

                        data.htmldata = checktemplate[0].receipttemplate;

                    }
                    else
                    {

                        data.receiptformathead = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                  from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                  from c in _FeeGroupContext.FeeTransactionPaymentDMO
                                                  from d in _FeeGroupContext.FeeAmountEntryDMO
                                                  from e in _FeeGroupContext.FeeHeadDMO
                                                  from f in _FeeGroupContext.feespecialHead
                                                  from g in _FeeGroupContext.feeSGGG
                                                  where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
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
                    ).Distinct().ToArray();
                        data.fillstudentviewdetails = (from a in _FeeGroupContext.FeeTransactionPaymentDMO
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
                                                       from l in _FeeGroupContext.FeeStudentTransactionDMO
                                                       where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id && l.AMST_Id == f.AMST_Id && l.FMA_Id == c.FMA_Id && l.FMH_Id == c.FMH_Id && l.ASMAY_Id == f.ASMAY_Id && l.FTI_Id == c.FTI_Id && l.MI_Id == i.MI_Id && l.FMA_Id == a.FMA_Id)
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
                                                           FTP_Concession_Amt = l.FSS_ConcessionAmount,
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
                                                           totalcharges = c.FMA_Amount,
                                                           FSS_AdjustedAmount = l.FSS_AdjustedAmount,
                                                           amst_mobile = i.AMST_MobileNo,
                                                           FYP_ChallanNo = b.FYP_ChallanNo,
                                                           FMH_Order = d.FMH_Order,
                                                           FMA_Amount = c.FMA_Amount,
                                                           FYP_PaymentReference_Id = b.fyp_transaction_id,
                                                           FSS_ToBePaid = l.FSS_ToBePaid,
                                                       }
                 ).Distinct().OrderBy(t => t.FMH_Order).ToArray();

                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ArrearAmount_IT_Receipt";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
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
                                    data.fillstudent = retObject.Distinct().ToArray();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                        List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                        temp_group_head = (from a in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                           from b in _FeeGroupContext.Fee_Payment
                                           from c in _FeeGroupContext.FeeTransactionPaymentDMO
                                           from d in _FeeGroupContext.FeeAmountEntryDMO

                                           where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id == data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMG_Id = d.FMG_Id,
                                         
                                           }
                                   ).Distinct().ToList();

                        List<long> grp_ids = new List<long>();
                   
                        foreach (var item in temp_group_head)
                        {
                            grp_ids.Add(item.FMG_Id);
                    
                        }
                        var checktemplate = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                             from b in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                             where (a.FGAR_Id == b.FGAR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && grp_ids.Contains(b.FMG_Id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FMG_Id = b.FMG_Id,
                                                 receipttemplate = a.FGAR_Template_Name,
                                             }
            ).Distinct().ToArray();

                        data.htmldata = checktemplate[0].receipttemplate;
                    }
                }

                else if (data.returnval == "Indi")
                {
                    
                    List<long> fmh_ids = new List<long>();
                    List<long> fmhsf_ids = new List<long>();
                    foreach (var x in data.savetmpdata)
                    {
                        //if (x.returnval == "H")
                        //{
                            fmh_ids.Add(x.FMH_Id);
                        //newly added
                            fmhsf_ids.Add(x.FMH_Id);
                        //}
                        //else
                        //{
                        //    fmhsf_ids.Add(x.FMH_Id);
                        //}
                    }

                    var getfmh_ids = (from f in _FeeGroupContext.feespecialHead
                                      from g in _FeeGroupContext.feeSGGG
                                      where (f.FMSFH_Id == g.FMSFH_Id && f.MI_Id == data.MI_Id && fmhsf_ids.Contains(g.FMSFH_Id))
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMH_Id = g.FMH_Id
                                      }
                                     ).Distinct().ToList();

                    if (getfmh_ids.Count > 0)
                    {
                        fmh_ids = new List<long>();
                        foreach (var y in getfmh_ids)
                        {
                            fmh_ids.Add(y.FMH_Id);
                        }
                    }
                    //foreach(var y in getfmh_ids)
                    //{
                    //    fmh_ids.Add(y.FMH_Id);
                    //}

                    data.receiptformathead = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                              from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                              from c in _FeeGroupContext.FeeTransactionPaymentDMO
                                              from d in _FeeGroupContext.FeeAmountEntryDMO
                                              from e in _FeeGroupContext.FeeHeadDMO
                                              from f in _FeeGroupContext.feespecialHead
                                              from g in _FeeGroupContext.feeSGGG
                                              where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && f.MI_Id == data.MI_Id && fmh_ids.Contains(d.FMH_Id) && d.ASMAY_Id == data.ASMAY_Id)
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
                ).Distinct().ToArray();

                    var fillstudentviewdetails = (from a in _FeeGroupContext.FeeTransactionPaymentDMO
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
                                                   from l in _FeeGroupContext.FeeStudentTransactionDMO
                                                   where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && l.AMST_Id == f.AMST_Id && l.FMA_Id == c.FMA_Id && l.FMH_Id == c.FMH_Id && l.ASMAY_Id == f.ASMAY_Id && l.FTI_Id == c.FTI_Id && l.MI_Id == i.MI_Id && l.FMA_Id == a.FMA_Id && fmh_ids.Contains(l.FMH_Id))
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
                                                       FTP_Concession_Amt = l.FSS_ConcessionAmount,
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
                                                       totalcharges = c.FMA_Amount,
                                                       FSS_AdjustedAmount = l.FSS_AdjustedAmount,
                                                       amst_mobile = i.AMST_MobileNo,
                                                       FYP_ChallanNo = b.FYP_ChallanNo,
                                                       FMH_Order = d.FMH_Order,
                                                       FMA_Amount = c.FMA_Amount,
                                                       FYP_PaymentReference_Id = b.fyp_transaction_id,
                                                       FSS_ToBePaid = l.FSS_ToBePaid,
                                                   }
          ).Distinct().OrderBy(t => t.FMH_Order).ToArray();

                    data.fillstudentviewdetails = (from i in fillstudentviewdetails
                                                   group i by new
                                                   {
                                                       i.Amst_Id,
                                                       i.AMST_FirstName,
                                                       i.AMST_MiddleName,
                                                       i.AMST_LastName,
                                                       i.FMH_Id,
                                                       i.FTI_Id,
                                                       i.FMH_FeeName,
                                                       i.FYP_Receipt_No,
                                                       i.FYP_Date,
                                                       i.classname,
                                                       i.sectionname,
                                                       i.AMAY_RollNo,
                                                       i.admno,
                                                       i.fathername,
                                                       i.mothername,
                                                       i.FYP_Bank_Or_Cash,
                                                       i.FYP_DD_Cheque_No,
                                                       i.FYP_DD_Cheque_Date,
                                                       i.FYP_Bank_Name,
                                                       i.FYP_Remarks,
                                                       i.AMST_RegistrationNo,
                                                       i.FMCC_ConcessionName,
                                                       i.amst_mobile,
                                                       i.FYP_ChallanNo,
                                                       i.FMH_Order,
                                                       i.fyp_transaction_id,
                                                   } into g
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       Amst_Id = g.Key.Amst_Id,
                                                       AMST_FirstName = g.Key.AMST_FirstName,
                                                       AMST_MiddleName = g.Key.AMST_MiddleName,
                                                       AMST_LastName = g.Key.AMST_LastName,
                                                       FMH_Id = g.Key.FMH_Id,
                                                       FTI_Id=g.Key.FTI_Id,
                                                       FMH_FeeName = g.Key.FMH_FeeName,
                                                       FYP_Receipt_No = g.Key.FYP_Receipt_No,
                                                       FTP_Paid_Amt = g.Sum(t => t.FTP_Paid_Amt),
                                                       FTP_Concession_Amt = g.Sum(t => t.FTP_Concession_Amt),
                                                       FTP_Fine_Amt = g.Sum(t => t.FTP_Fine_Amt),
                                                       FYP_Date = g.Key.FYP_Date,
                                                       classname = g.Key.classname,
                                                       sectionname = g.Key.sectionname,
                                                       rollno = g.Key.AMAY_RollNo,
                                                       admno = g.Key.admno,
                                                       fathername = g.Key.fathername,
                                                       mothername = g.Key.mothername,
                                                       FYP_Bank_Or_Cash = g.Key.FYP_Bank_Or_Cash,
                                                       FYP_DD_Cheque_No = g.Key.FYP_DD_Cheque_No,
                                                       FYP_DD_Cheque_Date = g.Key.FYP_DD_Cheque_Date,
                                                       FYP_Bank_Name = g.Key.FYP_Bank_Name,
                                                       FYP_Remarks = g.Key.FYP_Remarks,
                                                       AMST_RegistrationNo = g.Key.AMST_RegistrationNo,
                                                       FMCC_ConcessionName = g.Key.FMCC_ConcessionName,
                                                       totalcharges = g.Sum(t => t.FMA_Amount),
                                                       FSS_AdjustedAmount = g.Sum(t => t.FSS_AdjustedAmount),
                                                       amst_mobile = g.Key.amst_mobile,
                                                       FYP_ChallanNo = g.Key.FYP_ChallanNo,
                                                       FMH_Order = g.Key.FMH_Order,
                                                       FMA_Amount = g.Sum(t => t.FMA_Amount),
                                                       FYP_PaymentReference_Id = g.Key.fyp_transaction_id,
                                                       FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid)
                                                   }).Distinct().ToArray();

                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ArrearAmount_IT_Receipt";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
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
                                data.fillstudent = retObject.Distinct().ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                else
                {
                    data.fillstudentviewdetails = (from a in _FeeGroupContext.FeeTransactionPaymentDMO
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
                                                   from l in _FeeGroupContext.FeeStudentTransactionDMO
                                                   where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && l.AMST_Id == f.AMST_Id && l.FMA_Id == c.FMA_Id && l.FMH_Id == c.FMH_Id && l.ASMAY_Id == f.ASMAY_Id && l.FTI_Id == c.FTI_Id && l.MI_Id == i.MI_Id && l.FMA_Id == a.FMA_Id && b.user_id==data.userid)
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
                                                       FTP_Concession_Amt = l.FSS_ConcessionAmount,
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
                                                       totalcharges = c.FMA_Amount,
                                                       FSS_AdjustedAmount = l.FSS_AdjustedAmount,
                                                       amst_mobile = i.AMST_MobileNo,
                                                       FYP_ChallanNo = b.FYP_ChallanNo,
                                                       FMH_Order = d.FMH_Order,
                                                       FMA_Amount = c.FMA_Amount,
                                                       FYP_PaymentReference_Id = b.fyp_transaction_id,
                                                       FSS_ToBePaid = l.FSS_ToBePaid,
                                                   }
     ).Distinct().OrderBy(t => t.FMH_Order).ToArray();
                }
               
                
                var periodnme = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                     from b in _FeeGroupContext.FeeTransactionPaymentDMO
                                     from c in _FeeGroupContext.FeeAmountEntryDMO
                                     from d in _FeeGroupContext.FeeGroupDMO
                                     where (a.FMG_Id == c.FMG_Id && a.FMG_Id == d.FMG_Id && b.FMA_Id == c.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && d.FMG_CompulsoryFlag!="R")
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMG_CompulsoryFlag = d.FMG_CompulsoryFlag,
                                     }
            ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                    var feeterm = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                   from b in _FeeGroupContext.FeeTransactionPaymentDMO
                                   from c in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                   from d in _FeeGroupContext.FeeStudentTransactionDMO
                                   from e in _FeeGroupContext.feeMTH
                                   from f in _FeeGroupContext.FeeHeadDMO
                                   from g in _FeeGroupContext.feeTr
                                   where (e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                                   select new FeeStudentTransactionDTO
                                   {
                                       fmt_id = e.FMT_Id,
                                       fmt_order = g.FMT_Order,
                                       FMT_Year = g.FMT_Year
                                   }

                    ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                if (data.FYP_Id != null)
                {
                    var latestreceiptno = (from a in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                           from b in _FeeGroupContext.FeePaymentDetailsDMO
                                           from c in _FeeGroupContext.FeeStudentTransactionDMO
                                           from d in _FeeGroupContext.FeeGroupDMO
                                           from e in _FeeGroupContext.FeeTransactionPaymentDMO
                                           where (a.FYP_Id == b.FYP_Id && a.AMST_Id == c.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && e.FMA_Id == c.FMA_Id && a.FYP_Id == e.FYP_Id && c.FMG_Id == d.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_ID == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && d.FMG_CompulsoryFlag != "T" && b.FYP_Id == data.FYP_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FYP_Date = b.FYP_Date,
                                               FYP_Receipt_No = b.FYP_Receipt_No
                                           }
                ).Distinct().OrderByDescending(t => t.FYP_Date).ToArray();
                    data.FYP_Receipt_No = latestreceiptno[0].FYP_Receipt_No;
                }
                else
                {
                    var latestreceiptno = (from a in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                           from b in _FeeGroupContext.FeePaymentDetailsDMO
                                           from c in _FeeGroupContext.FeeStudentTransactionDMO
                                           from d in _FeeGroupContext.FeeGroupDMO
                                           from e in _FeeGroupContext.FeeTransactionPaymentDMO
                                           where (a.FYP_Id == b.FYP_Id && a.AMST_Id == c.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && e.FMA_Id == c.FMA_Id && a.FYP_Id == e.FYP_Id && c.FMG_Id == d.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_ID == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && d.FMG_CompulsoryFlag != "T")
                                           select new FeeStudentTransactionDTO
                                           {
                                               FYP_Date = b.FYP_Date,
                                               FYP_Receipt_No = b.FYP_Receipt_No
                                           }
                ).Distinct().OrderByDescending(t => t.FYP_Date).ToArray();
                    data.FYP_Receipt_No = latestreceiptno[0].FYP_Receipt_No;
                }
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
                    
                    startmonth = _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_int) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_FROM_MONTH;

                    endmonth = _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_end) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_TO_MONTH;

                    year1 = _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_int) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_Year;

                    year2 = _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_end) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_Year;

                    data.duration = startmonth + '-' + year1 + '/' + endmonth + '-' + year2;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeeTransactionPaymentDTO getstudent(FeeTransactionPaymentDTO data)
        {
            try
            {
                if (data.AMSC_Id > 0)
                {

                    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.AMSC_Id)
                                        select new FeeTransactionPaymentDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                }

                else
                {
                    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id)
                                        select new FeeTransactionPaymentDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO getreceipt(FeeTransactionPaymentDTO data)
        {
            try
            {
                if (data.studentdata != null && data.studentdata.Length > 0)
                {
                    List<long> std_id = new List<long>();
                    foreach (var item in data.studentdata)
                    {
                        std_id.Add(item.AMST_Id);
                    }

                    data.fillterms = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                      from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                      from c in _FeeGroupContext.FeeTransactionPaymentDMO
                                      where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && std_id.Contains(b.AMST_Id) && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id)
                                      select new FeeStudentGroupMappingDTO
                                      {
                                          fyp_id = a.FYP_Id,
                                          fyp_receipt_no = a.FYP_Receipt_No

                                      }
                              ).Distinct().ToArray();

                }
                else
                {
                    data.fillterms = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                      from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                      from c in _FeeGroupContext.FeeTransactionPaymentDMO
                                      where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id)
                                      select new FeeStudentGroupMappingDTO
                                      {
                                          fyp_id = a.FYP_Id,
                                          fyp_receipt_no = a.FYP_Receipt_No
                                      }
                              ).Distinct().ToArray();
                }


                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Studentlist_Head";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });                    
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                SqlDbType.BigInt)
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
                        data.fillmasterhead = retObject.ToArray();
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

        public FeeTransactionPaymentDTO getreceiptreport(FeeTransactionPaymentDTO data)
        {
            try
            {
                List<long> HeadId = new List<long>();
                List<long> std_id = new List<long>();

                foreach (var item in data.saveheadlst)
                {
                    HeadId.Add(item.FMH_Id);
                }

                foreach (var item in data.studentdata)
                {
                    std_id.Add(item.AMST_Id);
                }

                string headlist = "0";
                string stdlist = "0";
                string rcptlist = "0";



                List<long> recpt_id = new List<long>();
                foreach (var item in data.receiptlist)
                {
                    recpt_id.Add(item.FYP_Id);
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

                for (int c = 0; c < std_id.Count(); c++)
                {

                    if (c == 0)
                    {
                        stdlist = std_id[c].ToString();
                    }
                    else
                    {
                        stdlist = stdlist + ',' + std_id[c];
                    }

                }


                for (int c = 0; c < recpt_id.Count(); c++)
                {

                    if (c == 0)
                    {
                        rcptlist = recpt_id[c].ToString();
                    }
                    else
                    {
                        rcptlist = rcptlist + ',' + recpt_id[c];
                    }
                }

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Fee_BalRegister_Studentlist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                //    cmd.Parameters.Add(new SqlParameter("@FYP_Id",
                //SqlDbType.NVarChar)
                //    {
                //        Value = rcptlist
                //    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.AMSC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                SqlDbType.NVarChar)
                    {
                        Value = stdlist
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
                        data.studentdetaillist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Fee_BalRegister";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FYP_Id",
                SqlDbType.NVarChar)
                    {
                        Value = rcptlist
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.AMSC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                SqlDbType.NVarChar)
                    {
                        Value = stdlist
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                SqlDbType.NVarChar)
                    {
                        Value = headlist
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLag",
                SqlDbType.VarChar)
                    {
                        Value = data.reporttype
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
                        data.receiptdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
    }
}
