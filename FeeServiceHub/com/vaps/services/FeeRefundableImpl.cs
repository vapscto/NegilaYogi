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
using DomainModel.Model.com.vaps.admission;
using CommonLibrary;
using System.Dynamic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Net;
using DomainModel.Model.com.vapstech.Fee;
using Newtonsoft.Json;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeRefundableImpl : interfaces.FeeRefundableInterface
    {
        private static ConcurrentDictionary<string, FeeChequeBounceDTO> _login =
        new ConcurrentDictionary<string, FeeChequeBounceDTO>();
        public DomainModelMsSqlServerContext _context;
        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public FeeRefundableImpl(FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
        }
        public FeeMasterRefundDTO getdata(FeeMasterRefundDTO id)
        {
            try
            {

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == id.MI_Id && t.IMN_Flag == "Refund").ToList();
                // id.transnumbconfigurationsettingsss = masnum.ToArray();
               

                foreach (var ab in masnum)
                {
                    if (ab.IMN_AutoManualFlag == "Auto")
                    {
                        Master_NumberingDTO transnumbconfigurationsettingsss = Mapper.Map<Master_NumberingDTO>(ab);

                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        transnumbconfigurationsettingsss.MI_Id = id.MI_Id;
                        transnumbconfigurationsettingsss.ASMAY_Id = id.ASMAY_ID;
                        id.FR_RefundNo = a.GenerateNumber(transnumbconfigurationsettingsss);
                      
                    }
                    id.IMN_AutoManualFlag = ab.IMN_AutoManualFlag;
                }
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _YearlyFeeGroupMappingContext.AcademicYear.Where(t=>t.MI_Id== id.MI_Id && t.Is_Active==true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                id.fillyear = year.Distinct().ToArray();

                List<MasterAcademic> currentYear = new List<MasterAcademic>();
                currentYear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true && t.ASMAY_Id==id.ASMAY_ID).OrderByDescending(t=>t.ASMAY_Order).ToList();
                id.currentYear = currentYear.ToArray();

                List<School_M_Class> clslst = new List<School_M_Class>();
                clslst = _YearlyFeeGroupMappingContext.admissioncls.Where(m => m.MI_Id == id.MI_Id && m.ASMCL_ActiveFlag == true).ToList();
                id.fillclass = clslst.ToArray();

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_refundable_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = id.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
                      SqlDbType.BigInt)
                    {
                        Value = id.userid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                      SqlDbType.BigInt)
                    {
                        Value = id.ASMAY_ID
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
                        id.fillthirdgriddata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                //id.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.FeeMasterRefundDMO
                //                          from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                          from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                          from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                          from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO 
                //                          //from e in _YearlyFeeGroupMappingContext.FeeMasterTermHeadsDMO
                //                          //from f in _YearlyFeeGroupMappingContext.feeTr
                //                        where (a.AMST_ID == b.AMST_Id && a.FMH_ID == c.FMH_Id && b.AMST_Id == d.AMST_Id && d.ASMAY_Id == id.ASMAY_ID && a.MI_Id == id.MI_Id && e.FTI_Id==a.FTI_Id && e.FTI_Id==a.FTI_Id && a.FR_RefundFlag.Equals("true") && a.User_Id==id.userid)
                //                          select new FeeMasterRefundDTO
                //                          {
                //                              AMST_ID = a.AMST_ID,
                //                              AMST_FirstName = b.AMST_FirstName,
                //                              AMST_MiddleName = b.AMST_MiddleName,
                //                              AMST_LastName = b.AMST_LastName,
                //                              FMH_FeeName = c.FMH_FeeName,
                //                              FR_RefundAmount = a.FR_RefundAmount,
                //                              FR_Date = a.FR_Date,
                //                              FR_ID = a.FR_ID,
                //                              FR_RefundNo = a.FR_RefundNo,
                //                              FTI_Name=e.FTI_Name,
                //                              //FMT_Name=f.FMT_Name,
                //                              //FMT_Order=f.FMT_Order
                //                          }).OrderByDescending(t=>t.FMT_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }

        public FeeMasterRefundDTO deleterec(FeeMasterRefundDTO id)
        {
            var lorg = _YearlyFeeGroupMappingContext.FeeMasterRefundDMO.Single(t => t.FR_ID==id.FR_ID);

            try
            {
                if (lorg!=null)
                {
                    var lorg1 = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.AMST_Id == lorg.AMST_ID && t.FMG_Id== lorg.FMG_Id && t.FMH_Id== lorg.FMH_ID && t.FTI_Id== lorg.FTI_Id && t.User_Id==lorg.User_Id && t.ASMAY_Id== lorg.ASMAY_ID);

                    var updateStudentStatus = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FSS_Id == lorg1.FSS_Id);

                    if(updateStudentStatus.FSS_RefundAmount >= Convert.ToInt64(lorg.FR_RefundAmount))
                    {
                        var gethead = _YearlyFeeGroupMappingContext.feehead.Single(t => t.FMH_Id == lorg1.FMH_Id);
                        if (gethead.FMH_RefundFlag == true)
                        {
                            updateStudentStatus.FSS_RefundableAmount = updateStudentStatus.FSS_RefundableAmount + Convert.ToInt64(lorg.FR_RefundAmount);
                               
                        }
                        else
                        {
                            updateStudentStatus.FSS_RunningExcessAmount = updateStudentStatus.FSS_RunningExcessAmount + Convert.ToInt64(lorg.FR_RefundAmount);
                            
                        }
                    updateStudentStatus.FSS_RefundAmount = updateStudentStatus.FSS_RefundAmount - Convert.ToInt64(lorg.FR_RefundAmount);
                    _YearlyFeeGroupMappingContext.Update(updateStudentStatus);

                    lorg.FR_RefundFlag = "False";
                        _YearlyFeeGroupMappingContext.Update(lorg);
                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                        if (contactExists > 1)
                        {

                            id.returnVal = true;
                        }
                        else
                        {
                            id.returnVal = false;
                           
                        }
                    }
                    else
                    {
                        id.returnVal = false;
                    }
                    
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }

        public FeeMasterRefundDTO getdatastuacad(FeeMasterRefundDTO data)
        {
            try
            {

                data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_ID)
                                    select new FeeStudentTransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
              ).Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

            // throw new NotImplementedException();
        }

        public FeeMasterRefundDTO GetStudentListByYear(FeeMasterRefundDTO data)
        {

            FeeMasterRefundDTO stu = new FeeMasterRefundDTO();
            try
            {

                List<School_M_Class> clslst = new List<School_M_Class>();
                clslst = _YearlyFeeGroupMappingContext.admissioncls.Where(m => m.MI_Id == data.MI_Id && m.ASMCL_ActiveFlag == true).ToList();
                stu.fillclass = clslst.ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return stu;

            // throw new NotImplementedException();
        }

        public FeeMasterRefundDTO GetSection(FeeMasterRefundDTO data)
        {
            try
            {


                data.fillsection = (from a in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from b in _YearlyFeeGroupMappingContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_ID && a.ASMCL_Id == data.ASMCL_ID)
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        ASMS_Id = a.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }
        public FeeMasterRefundDTO GetStudent(FeeMasterRefundDTO stu)
        {
            try
            {



                stu.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                   from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                   where (a.AMST_Id == b.AMST_Id && c.AMST_Id==b.AMST_Id && c.ASMAY_Id==b.ASMAY_Id && b.ASMCL_Id == stu.ASMCL_ID &&  b.ASMS_Id == stu.ASMS_Id && a.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_ID && (c.FSS_RunningExcessAmount>0 || c.FSS_RefundableAmount>0))
                                   select new FeeStudentTransactionDTO
                                   {
                                       Amst_Id = a.AMST_Id,
                                       AMST_FirstName = a.AMST_FirstName,
                                       AMST_MiddleName = a.AMST_MiddleName,
                                       AMST_LastName = a.AMST_LastName,
                                   }
              ).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return stu;
        }
        public FeeMasterRefundDTO GetStudentListByamst(FeeMasterRefundDTO stu)
        {
            try
            {
                bool refundflag = false;
                if (stu.filterrefund == "Refunable")
                    refundflag = true;
                stu.fillgroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                 where (c.FMH_Id == b.FMH_Id && a.FMG_Id == b.FMG_Id && b.AMST_Id == stu.AMST_ID && a.MI_Id == stu.MI_Id  && b.ASMAY_Id == stu.ASMAY_ID && a.user_id==b.User_Id && a.user_id==stu.userid && c.FMH_RefundFlag==refundflag)// && a.FMG_ActiceFlag==true
                                   select new FeeStudentTransactionDTO
                                   {
                                       FMG_Id = a.FMG_Id,
                                       FMG_GroupName = a.FMG_GroupName                                     
                                   }
             ).Distinct().ToArray();

                var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                             from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                             where (a.FYP_Id == b.FYP_Id && a.MI_Id == stu.MI_Id && a.ASMAY_ID == b.ASMAY_Id && a.ASMAY_ID == stu.ASMAY_ID && a.user_id == stu.userid && b.AMST_Id == stu.AMST_ID) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FYP_Receipt_No = a.FYP_Receipt_No,
                                                 FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                                 FYP_Tot_Amount = a.FYP_Tot_Amount
                                             }
                    ).ToList();
                stu.showstaticticsdetails = showstaticticsdetails.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return stu;
        }

        public FeeMasterRefundDTO getdatastuacadgrp(FeeMasterRefundDTO id)
        {
            throw new NotImplementedException();
        }

        public FeeMasterRefundDTO geteditdet(FeeMasterRefundDTO data)
        {
            try
            {
                var query = _YearlyFeeGroupMappingContext.FeeMasterRefundDMO.Where(d => d.FR_ID == data.FR_ID).ToArray();

                data.ASMCL_ID  = _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO.Single(c => c.AMST_Id == query.FirstOrDefault().AMST_ID).ASMCL_Id;
                data.FMH_FeeName = _YearlyFeeGroupMappingContext.FeeHeadDMO.Single(d => d.FMH_Id == query.FirstOrDefault().FMH_ID).FMH_FeeName;

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeMasterRefundDTO getstuddet(FeeMasterRefundDTO data)
        {
            throw new NotImplementedException();
        }

        public FeeMasterRefundDTO savedetails(FeeMasterRefundDTO data)
        {
            int j = 0;
          
            FeeMasterRefundDTO pmm = new FeeMasterRefundDTO();
            try
            {
                if(data.FR_ID>0)
                {
                    if (data.savetmpdata.Length > 0)
                    {
                        if (data.ASMAY_ID > 0 && data.MI_Id > 0)
                        {
                            while (j < data.savetmpdata.Count())
                            {
                                var updateStudentStatus = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FSS_Id == data.savetmpdata[j].FSS_Id);
                                var gethead = _YearlyFeeGroupMappingContext.feehead.Single(t => t.FMH_Id == updateStudentStatus.FMH_Id);
                                if (gethead.FMH_RefundFlag == true)
                                {
                                    if (updateStudentStatus.FSS_RefundableAmount >= Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount))
                                    {
                                        updateStudentStatus.FSS_RefundableAmount = updateStudentStatus.FSS_RefundableAmount - Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount);

                                    }
                                }
                                else
                                {
                                    if (updateStudentStatus.FSS_RunningExcessAmount >= Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount))
                                    {
                                        updateStudentStatus.FSS_RunningExcessAmount = updateStudentStatus.FSS_RunningExcessAmount - Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount);

                                    }
                                }

                                if (data.selectedRefundOverList.Length > 0)
                                {
                                    if (data.selectedRefundOverList[j].FSS_Id == updateStudentStatus.FSS_Id)
                                    {
                                        updateStudentStatus.FSS_RefundOverFlag = true;
                                    }
                                }
                                _YearlyFeeGroupMappingContext.Update(updateStudentStatus);
                                var fmr = _YearlyFeeGroupMappingContext.FeeMasterRefundDMO.Single(d=>d.FR_ID==data.FR_ID);
                                fmr.FMH_ID = data.savetmpdata[j].FMH_ID;
                                fmr.AMST_ID = data.AMST_ID;
                                fmr.FR_Date = data.FR_Date;
                                fmr.FR_RefundAmount =fmr.FR_RefundAmount + data.savetmpdata[j].FR_RefundAmount;
                                fmr.FR_RefundRemarks = data.FR_RefundRemarks;
                                fmr.FR_RefundNo = data.FR_RefundNo;
                                fmr.FR_BANK_CASH = data.FR_BANK_CASH;
                                fmr.FR_CheqDate = data.FR_CheqDate;
                                fmr.FR_CheqNo = data.FR_CheqNo;
                                fmr.FMG_Id = data.FMG_Id;
                                fmr.FR_BankName = data.FR_BankName;
                                _YearlyFeeGroupMappingContext.Update(fmr);
                                 var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                    if (contactExists > 0)
                                    {
                                        data.returnVal = true;
                                        data.returntxt = "Updated";
                                    }
                                    else
                                    {
                                        data.returnVal =false;
                                        data.returntxt = "not Updated";
                                    }
                                    j++;
                                }
                        }
                    }
                }
                else
                {
                    if(data.savetmpdata.Length>0)
                    {
                        if (data.ASMAY_ID > 0 && data.MI_Id > 0)
                        {
                            if (data.FR_BANK_CASH == "O")
                            {
                                List<long> terms_groups = new List<long>();
                                List<long> terms_heads = new List<long>();
                                List<long> terms_installments = new List<long>();
                                long terms_amount = 0;
                                for (int r = 0; r < data.savetmpdata.Length; r++)
                                {
                                    terms_groups.Add(Convert.ToInt64(data.savetmpdata[r].FMG_Id));
                                    terms_heads.Add(Convert.ToInt64(data.savetmpdata[r].FMH_ID));
                                    terms_installments.Add(Convert.ToInt64(data.savetmpdata[r].FTI_Id));
                                    terms_amount = terms_amount + (Convert.ToInt64(data.savetmpdata[r].FR_RefundAmount));
                                }

                                var onlinepaid = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                  from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                                  from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                                  where (a.AMST_Id == b.AMST_Id && b.FYP_Id == d.FYP_Id && c.FYP_Id==d.FYP_Id && c.FMA_Id==a.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_ID && d.FYP_Bank_Or_Cash == "O" && terms_groups.Contains(a.FMG_Id) && terms_heads.Contains(a.FMH_Id) && terms_installments.Contains(a.FTI_Id) && a.AMST_Id == data.AMST_ID)
                                                  select new FeeStudentTransactionDTO
                                                  {
                                                      FYP_Bank_Or_Cash = d.FYP_Bank_Or_Cash,
                                                      FYP_PaymentReference_Id = d.FYP_PaymentReference_Id,
                                                      FYP_Tot_Amount = d.FYP_Tot_Amount
                                                  }).Distinct().ToArray();

                                if (onlinepaid.FirstOrDefault().FYP_Bank_Or_Cash == "O")
                                {
                                    if (onlinepaid.FirstOrDefault().FYP_Tot_Amount <= terms_amount)
                                    {
                                        List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                                        paymentdetails = _YearlyFeeGroupMappingContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                                        var fetchstudentdeatils = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                                   from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                                   where (a.AMST_Id == b.AMST_Id && a.AMST_Id == data.AMST_ID && b.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_Id)
                                                                   select new FeeStudentTransactionDTO
                                                                   {
                                                                       amst_mobile = a.AMST_MobileNo,
                                                                       amst_email_id = a.AMST_emailId,
                                                                       ASMCL_ID = b.ASMCL_Id,
                                                                       AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                                                       AMST_AdmNo = a.AMST_AdmNo,
                                                                       Amst_Id = a.AMST_Id
                                                                   }).ToArray();

                                        Dictionary<String, object> transfersnotes = new Dictionary<String, object>();
                                        Dictionary<String, object> refundparameters = new Dictionary<String, object>();

                                        transfersnotes.Add("notes_key_1", fetchstudentdeatils[0].AMST_FirstName);
                                        transfersnotes.Add("notes_key_2", fetchstudentdeatils[0].AMST_AdmNo + onlinepaid.FirstOrDefault().FYP_PaymentReference_Id);

                                        string url = "https://api.razorpay.com/v1/payments/paymentid/refund";
                                        url = url.Replace("paymentid", onlinepaid.FirstOrDefault().FYP_PaymentReference_Id);

                                        if (data.FR_BANK_CASH == "O" && data.instantrefund == "true")
                                        {
                                            refundparameters.Add("speed", "optimum");
                                        }

                                        if (onlinepaid.FirstOrDefault().FYP_Tot_Amount < terms_amount)
                                        {
                                            refundparameters.Add("amount", terms_amount);
                                        }

                                        refundparameters.Add("receipt", data.FR_RefundNo);
                                        refundparameters.Add("notes", transfersnotes);

                                        var myContent = JsonConvert.SerializeObject(refundparameters);

                                        String postData = myContent;
                                        HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                                        connection.ContentType = "application/json";

                                        connection.Method = "POST";
                                        string username = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                                        string password = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                                        string authInfo = username + ":" + password;
                                        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                                        connection.Headers["Authorization"] = "Basic " + authInfo;

                                        using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                                        {
                                            requestWriter.Write(postData);
                                        }
                                        string responseData = string.Empty;

                                        using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                                        {
                                            responseData = responseReader.ReadToEnd();

                                            JObject joResponse1 = JObject.Parse(responseData);
                                            string refundid = (string)joResponse1["id"];
                                            string amount = (string)joResponse1["amount"];
                                            string payment_id = (string)joResponse1["payment_id"];

                                            while (j < data.savetmpdata.Count())
                                            {
                                                //Updating Columns in Fee_Student_Status table
                                                var updateStudentStatus = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FSS_Id == data.savetmpdata[j].FSS_Id);
                                                var gethead = _YearlyFeeGroupMappingContext.feehead.Single(t => t.FMH_Id == updateStudentStatus.FMH_Id);
                                                if (gethead.FMH_RefundFlag == true)
                                                {
                                                    if (updateStudentStatus.FSS_RefundableAmount >= Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount))
                                                    {
                                                        updateStudentStatus.FSS_RefundableAmount = updateStudentStatus.FSS_RefundableAmount - Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount);
                                                    }
                                                }
                                                else
                                                {
                                                    if (updateStudentStatus.FSS_RunningExcessAmount >= Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount))
                                                    {
                                                        updateStudentStatus.FSS_RunningExcessAmount = updateStudentStatus.FSS_RunningExcessAmount - Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount);

                                                    }
                                                }
                                                for (int i = 0; i < data.selectedRefundOverList.Length; i++)
                                                {
                                                    if (data.selectedRefundOverList[i].FSS_Id == updateStudentStatus.FSS_Id)
                                                    {
                                                        updateStudentStatus.FSS_RefundOverFlag = true;
                                                    }
                                                }
                                                for (int i = 0; i < data.savetmpdata.Length; i++)
                                                {
                                                    updateStudentStatus.FSS_RefundAmount = Convert.ToInt32(updateStudentStatus.FSS_RefundAmount) + Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount);
                                                }
                                                _YearlyFeeGroupMappingContext.Update(updateStudentStatus);
                                                //**********************************************************

                                                //Saving data in Fee_Refund table
                                                FeeMasterRefundDMO pgmodule = Mapper.Map<FeeMasterRefundDMO>(data);

                                                pgmodule.FMH_ID = data.savetmpdata[j].FMH_ID;
                                                pgmodule.FTI_Id = data.savetmpdata[j].FTI_Id;
                                                pgmodule.FMG_Id = data.savetmpdata[j].FMG_Id;

                                                pgmodule.AMST_ID = data.AMST_ID;
                                                pgmodule.FR_Date = data.FR_Date;
                                                pgmodule.FR_RefundAmount = data.savetmpdata[j].FR_RefundAmount;
                                                pgmodule.FR_RefundRemarks = data.FR_RefundRemarks + "-" + refundid + "-" + payment_id;
                                                pgmodule.FR_RefundNo = data.FR_RefundNo;
                                                pgmodule.FR_BANK_CASH = data.FR_BANK_CASH;
                                                pgmodule.FR_CheqDate = data.FR_CheqDate;
                                                pgmodule.FR_CheqNo = data.FR_CheqNo;
                                                pgmodule.FR_BankName = refundid;
                                                pgmodule.FR_RefundFlag = "true";

                                                _YearlyFeeGroupMappingContext.Add(pgmodule);
                                                var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                                if (contactExists > 0)
                                                {
                                                    data.returnVal = true;
                                                    data.returntxt = "Saved";
                                                }
                                                else
                                                {
                                                    data.returnVal = false;
                                                    data.returntxt = "not Saved";
                                                }
                                                j++;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        data.returntxt = "Selected payment is greater than online paid amount! Kindly recheck Fee paid details";
                                    }

                                }
                                else
                                {
                                    data.returntxt = "Transaction is not done online...So it cannot be proceeded!!";
                                }
                            }
                            else
                            {
                                while (j < data.savetmpdata.Count())
                                {
                                    //Updating Columns in Fee_Student_Status table
                                    var updateStudentStatus = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.FSS_Id == data.savetmpdata[j].FSS_Id);
                                    var gethead = _YearlyFeeGroupMappingContext.feehead.Single(t => t.FMH_Id == updateStudentStatus.FMH_Id);
                                    if (gethead.FMH_RefundFlag == true)
                                    {
                                        if (updateStudentStatus.FSS_RefundableAmount >= Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount))
                                        {
                                            updateStudentStatus.FSS_RefundableAmount = updateStudentStatus.FSS_RefundableAmount - Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount);

                                        }
                                    }
                                    else
                                    {
                                        if (updateStudentStatus.FSS_RunningExcessAmount >= Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount))
                                        {
                                            updateStudentStatus.FSS_RunningExcessAmount = updateStudentStatus.FSS_RunningExcessAmount - Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount);

                                        }
                                    }
                                    for (int i = 0; i < data.selectedRefundOverList.Length; i++)
                                    {
                                        if (data.selectedRefundOverList[i].FSS_Id == updateStudentStatus.FSS_Id)
                                        {
                                            updateStudentStatus.FSS_RefundOverFlag = true;
                                        }
                                    }
                                    for (int i = 0; i < data.savetmpdata.Length; i++)
                                    {
                                        updateStudentStatus.FSS_RefundAmount = Convert.ToInt32(updateStudentStatus.FSS_RefundAmount) + Convert.ToInt64(data.savetmpdata[j].FR_RefundAmount);
                                    }
                                    _YearlyFeeGroupMappingContext.Update(updateStudentStatus);
                                    //**********************************************************

                                    //Saving data in Fee_Refund table
                                    FeeMasterRefundDMO pgmodule = Mapper.Map<FeeMasterRefundDMO>(data);
                                    pgmodule.FMH_ID = data.savetmpdata[j].FMH_ID;
                                    pgmodule.FR_RefundAmount = data.savetmpdata[j].FR_RefundAmount;
                                    pgmodule.FTI_Id = data.savetmpdata[j].FTI_Id;
                                    pgmodule.FMG_Id = data.savetmpdata[j].FMG_Id;
                                    pgmodule.FR_RefundFlag = "true";
                                    _YearlyFeeGroupMappingContext.Add(pgmodule);
                                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                    if (contactExists > 0)
                                    {
                                        data.returnVal = true;
                                        data.returntxt = "Saved";
                                    }
                                    else
                                    {
                                        data.returnVal = false;
                                        data.returntxt = "not Saved";
                                    }
                                    j++;
                                }
                            }
                           // }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                data.returntxt = "Failed";
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeMasterRefundDTO getdataclawisestude(FeeMasterRefundDTO clsid)
        {
            try
            {
                clsid.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO 
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == clsid.MI_Id && b.ASMAY_Id == clsid.ASMAY_ID && b.ASMCL_Id==clsid.ASMCL_ID)
                                    select new FeeStudentTransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
              ).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return clsid;
        }

        public FeeMasterRefundDTO getgroupheaddetails(FeeMasterRefundDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.fillhead = (from a in _YearlyFeeGroupMappingContext.feehead
                                 from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 where (a.FMH_Id == b.FMH_Id && b.ASMAY_Id == data.ASMAY_ID &&
                                 b.MI_Id == data.MI_Id && b.AMST_Id == data.AMST_ID
                                 && data.multiplegroupF.Contains(Convert.ToString(b.FMG_Id)) && c.FTI_Id == b.FTI_Id && a.user_id==data.userid && a.FMH_RefundFlag== refundflag && ( (b.FSS_RunningExcessAmount + b.FSS_RefundableAmount) >0)) //a.FMH_Flag == "E" || a.FMH_Flag != "F"   && b.FSS_RefundOverFlag==false
                                 select new FeeMasterRefundDTO
                                    {
                                        FSS_Id=b.FSS_Id,
                                        FMH_ID=a.FMH_Id,
                                        FMH_FeeName=a.FMH_FeeName,
                                        FTI_Id=b.FTI_Id,
                                        FTI_Name= c.FTI_Name,
                                        FSS_RunningExcessAmount = b.FSS_RunningExcessAmount,
                                        FSS_RefundableAmount = b.FSS_RefundableAmount,
                                        FSS_RefundAmount = b.FSS_RefundAmount,                                       
                                       FMG_Id=b.FMG_Id
                                 }
              ).ToArray();

              
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeMasterRefundDTO getmodeofpaymentdata(FeeMasterRefundDTO data)
        {
            try
            {
                if (data.FR_BANK_CASH == "C")
                {
                }
                else if (data.FR_BANK_CASH == "B" || data.FR_BANK_CASH == "R" || data.FR_BANK_CASH == "S")
                {
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeMasterRefundDTO searching(FeeMasterRefundDTO data)
        {

            try
            {

                switch (data.searchType)
                {
                    case "0":
                        string str = "";
                        data.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.FeeMasterRefundDMO
                                                  from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                  from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_ID == b.AMST_Id && a.FMH_ID == c.FMH_Id && b.AMST_Id == d.AMST_Id && d.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_Id  && a.FR_RefundFlag.Equals("true") && (((b.AMST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(b.AMST_MiddleName.Trim()) == true ? str : b.AMST_MiddleName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(b.AMST_LastName.Trim()) == true ? str : b.AMST_LastName.Trim())).Trim().Contains(data.searchtext) || b.AMST_FirstName.StartsWith(data.searchtext) || b.AMST_MiddleName.StartsWith(data.searchtext) || b.AMST_LastName.StartsWith(data.searchtext)) && a.User_Id == data.userid)
                                                  select new FeeMasterRefundDTO
                                                  {
                                                      AMST_ID = a.AMST_ID,
                                                      AMST_FirstName = b.AMST_FirstName,
                                                      AMST_MiddleName = b.AMST_MiddleName,
                                                      AMST_LastName = b.AMST_LastName,
                                                      FMH_FeeName = c.FMH_FeeName,
                                                      FR_RefundAmount = a.FR_RefundAmount,
                                                      FR_Date = a.FR_Date,
                                                      FR_ID = a.FR_ID
                                                  }).OrderBy(t => t.AMST_FirstName).ToList().ToArray();


                        break;
                    case "1":
                        data.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.FeeMasterRefundDMO
                                                  from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                  from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_ID == b.AMST_Id && a.FMH_ID == c.FMH_Id && b.AMST_Id == d.AMST_Id && d.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_Id && a.FR_RefundFlag.Equals("true") && c.FMH_FeeName.Contains(data.searchtext)) && a.User_Id == data.userid
                                                  select new FeeMasterRefundDTO
                                                  {
                                                      AMST_ID = a.AMST_ID,
                                                      AMST_FirstName = b.AMST_FirstName,
                                                      AMST_MiddleName = b.AMST_MiddleName,
                                                      AMST_LastName = b.AMST_LastName,
                                                      FMH_FeeName = c.FMH_FeeName,
                                                      FR_RefundAmount = a.FR_RefundAmount,
                                                      FR_Date = a.FR_Date,
                                                      FR_ID = a.FR_ID
                                                  }).OrderBy(t => t.FMH_FeeName).ToList().ToArray();

                       
                        break;
                    case "2":
                        data.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.FeeMasterRefundDMO
                                                  from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                  from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_ID == b.AMST_Id && a.FMH_ID == c.FMH_Id && b.AMST_Id == d.AMST_Id && d.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_Id && a.FR_RefundFlag.Equals("true") && a.FR_RefundAmount.ToString().Contains(data.searchnumber) && a.User_Id == data.userid)
                                                  select new FeeMasterRefundDTO
                                                  {
                                                      AMST_ID = a.AMST_ID,
                                                      AMST_FirstName = b.AMST_FirstName,
                                                      AMST_MiddleName = b.AMST_MiddleName,
                                                      AMST_LastName = b.AMST_LastName,
                                                      FMH_FeeName = c.FMH_FeeName,
                                                      FR_RefundAmount = a.FR_RefundAmount,
                                                      FR_Date = a.FR_Date,
                                                      FR_ID = a.FR_ID
                                                  }).OrderBy(t => t.FR_RefundAmount).ToList().ToArray();
                        break;
                    case "3":
                        data.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.FeeMasterRefundDMO
                                                  from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                  from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_ID == b.AMST_Id && a.FMH_ID == c.FMH_Id && b.AMST_Id == d.AMST_Id && d.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_Id && a.FR_RefundFlag.Equals("true") && Convert.ToDateTime(a.FR_Date).ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && a.User_Id == data.userid)
                                                  select new FeeMasterRefundDTO
                                                  {
                                                      AMST_ID = a.AMST_ID,
                                                      AMST_FirstName = b.AMST_FirstName,
                                                      AMST_MiddleName = b.AMST_MiddleName,
                                                      AMST_LastName = b.AMST_LastName,
                                                      FMH_FeeName = c.FMH_FeeName,
                                                      FR_RefundAmount = a.FR_RefundAmount,
                                                      FR_Date = a.FR_Date,
                                                      FR_ID = a.FR_ID
                                                  }).OrderBy(t => t.FR_Date).ToList().ToArray();
                        break;                   
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeMasterRefundDTO onselectacademicyear(FeeMasterRefundDTO dto)
        {
            try
            {

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_refundable_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
                      SqlDbType.BigInt)
                    {
                        Value = dto.userid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                      SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_ID
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
                        dto.fillthirdgriddata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                //dto.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.FeeMasterRefundDMO
                //                        from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                        from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                        from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                        from e in _YearlyFeeGroupMappingContext.FeeMasterTermHeadsDMO
                //                        from f in _YearlyFeeGroupMappingContext.feeTr
                //                        where (a.AMST_ID == b.AMST_Id && a.FMH_ID == c.FMH_Id && b.AMST_Id == d.AMST_Id && e.FTI_Id == a.FTI_Id && a.ASMAY_ID==d.ASMAY_Id && a.ASMAY_ID == dto.ASMAY_ID && a.MI_Id == dto.MI_Id && e.FMH_Id == a.FMH_ID  && e.FMT_Id == f.FMT_Id && a.FR_RefundFlag.Equals("true") && a.User_Id == dto.userid) /*&& e.FTI_Id == a.FTI_Id*/
                //                         select new FeeMasterRefundDTO
                //                        {
                //                            AMST_ID = a.AMST_ID,
                //                            AMST_FirstName = b.AMST_FirstName,
                //                            AMST_MiddleName = b.AMST_MiddleName,
                //                            AMST_LastName = b.AMST_LastName,
                //                            FMH_FeeName = c.FMH_FeeName,
                //                            FR_RefundAmount = a.FR_RefundAmount,
                //                            FR_Date = a.FR_Date,
                //                            FR_ID = a.FR_ID,
                //                            FR_RefundNo = a.FR_RefundNo,
                //                            FMT_Name = f.FMT_Name,
                //                            FMT_Order = f.FMT_Order
                //                        }).OrderByDescending(t => t.FMT_Order).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public FeeMasterRefundDTO get_recepts(FeeMasterRefundDTO dto)
        {
            try
            {
                List<Institution> masins = new List<Institution>();
                masins = _context.Institute.Where(t => t.MI_Id == dto.MI_Id).ToList();
                dto.masterinstitution = masins.ToArray();
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_refundable_amount_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
                      SqlDbType.BigInt)
                    {
                        Value = dto.userid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                      SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                      SqlDbType.BigInt)
                    {
                        Value = dto.AMST_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@FR_RefundNo",
                      SqlDbType.VarChar)
                    {
                        Value = dto.FR_RefundNo
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
                        dto.fillstudentviewdetails = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                dto.student_details = (from a in _YearlyFeeGroupMappingContext.Adm_M_Student
                                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                       from c in _YearlyFeeGroupMappingContext.AdmSection
                                       from d in _YearlyFeeGroupMappingContext.admissioncls
                                       from e in _YearlyFeeGroupMappingContext.AcademicYear
                                       from f in _YearlyFeeGroupMappingContext.FeeMasterRefundDMO
                                       where (a.AMST_Id==b.AMST_Id && b.ASMCL_Id==d.ASMCL_Id && b.ASMS_Id==c.ASMS_Id && b.ASMAY_Id==e.ASMAY_Id && b.ASMAY_Id==dto.ASMAY_ID && a.AMST_Id==dto.AMST_ID && a.MI_Id==dto.MI_Id && a.AMST_Id==f.AMST_ID && a.MI_Id==f.MI_Id && f.FR_RefundNo==dto.FR_RefundNo)
                                       select new FeeMasterRefundDTO
                                       {
                                           AMST_FirstName=a.AMST_FirstName,
                                           AMST_MiddleName=a.AMST_MiddleName,
                                           AMST_LastName=a.AMST_LastName,
                                           AMST_FatherName = a.AMST_FatherName,
                                           AMST_MotherName = a.AMST_MotherName,
                                           ASMAY_Year = e.ASMAY_Year,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                           ASMC_SectionName = c.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           FR_RefundNo=f.FR_RefundNo

                                       }).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
