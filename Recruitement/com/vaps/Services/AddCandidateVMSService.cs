using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Payment = CommonLibrary.Payment;
using Razorpay.Api;

namespace Recruitment.com.vaps.Services
{
    public class AddCandidateVMSService : Interfaces.AddCandidateVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public FeeGroupContext _feecontext;
        public AddCandidateVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext, FeeGroupContext feecontext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
            _feecontext = feecontext;
        }

        public HR_Candidate_DetailsDTO getBasicData(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                var institutionlist = (from a in _VMSContext.Institution
                                       where a.MI_ActiveFlag == 1
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                dto.institutionlist = institutionlist.ToArray();
                if (dto.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        dto.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Candidate_DetailsDTO SaveUpdate(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Candidate_DetailsDMO dmoObj = Mapper.Map<HR_Candidate_DetailsDMO>(dto);
                HR_MRF_ListDMO mrfListObj = new HR_MRF_ListDMO();

                var duplicatecountresult = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMPT_Id == dto.HRMPT_Id && t.HRMC_Id == dto.HRMC_Id && t.HRCD_MRFNO == dto.HRCD_MRFNO && t.HRCD_FirstName == dto.HRCD_FirstName && t.HRCD_MiddleName == dto.HRCD_MiddleName && t.HRCD_LastName == dto.HRCD_LastName && t.HRMJ_Id == dto.HRMJ_Id && t.HRCD_Skills == dto.HRCD_Skills && t.HRCD_DOB == dto.HRCD_DOB && t.IVRMMG_Id == dto.IVRMMG_Id && t.HRCD_MobileNo == dto.HRCD_MobileNo && t.HRCD_EmailId == dto.HRCD_EmailId && t.HRCD_AadharNo == dto.HRCD_AadharNo).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRCD_Id > 0)
                    {
                        var result = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dmoObj.HRCD_Id);
                        dto.UpdatedDate = DateTime.Now;
                        dto.HRCD_ActiveFlg = true;
                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        dto.HRCD_Id = result.HRCD_Id;
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                            if (dto.welcomenotice == true)
                            {
                                Welcomemail(dto, "Welcome Notification", "Welcome Notification", dto.MI_Id);
                            }
                            else if (dto.thanksnotice == true)
                            {
                                Thanksgivingmail(dto, "Thanks Giving Notification", "Thanks Giving Notification", dto.MI_Id);
                            }

                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRCD_ActiveFlg = true;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRMPT_Id = dto.HRMPT_Id;
                        dmoObj.HRMC_Id = dto.HRMC_Id;
                        dmoObj.HRCD_MRFNO = dto.HRCD_MRFNO;
                        dmoObj.HRCD_FirstName = dto.HRCD_FirstName;
                        dmoObj.HRCD_MiddleName = dto.HRCD_MiddleName;
                        dmoObj.HRCD_LastName = dto.HRCD_LastName;
                        dmoObj.HRMJ_Id = dto.HRMJ_Id;
                        dmoObj.HRCD_Skills = dto.HRCD_Skills;
                        dmoObj.HRCD_DOB = dto.HRCD_DOB;
                        dmoObj.IVRMMG_Id = dto.IVRMMG_Id;
                        dmoObj.HRCD_MobileNo = dto.HRCD_MobileNo;
                        dmoObj.HRCD_EmailId = dto.HRCD_EmailId;
                        dmoObj.HRCD_ExpFrom = dto.HRCD_ExpFrom;
                        dmoObj.HRCD_ExpTo = dto.HRCD_ExpTo;
                        dmoObj.HRCD_CurrentCompany = dto.HRCD_CurrentCompany;
                        dmoObj.HRCD_ResumeSource = dto.HRCD_ResumeSource;
                        dmoObj.HRCD_JobPortalName = dto.HRCD_JobPortalName;
                        dmoObj.HRCD_RefCode = dto.HRCD_RefCode;
                        dmoObj.HRCD_LastCTC = dto.HRCD_LastCTC;
                        dmoObj.HRCD_ExpectedCTC = dto.HRCD_ExpectedCTC;
                        dmoObj.HRCD_AppDate = dto.HRCD_AppDate;
                        dmoObj.HRCD_InterviewDate = dto.HRCD_InterviewDate;
                        dmoObj.HRCD_NoticePeriod = dto.HRCD_NoticePeriod;
                        dmoObj.HRCD_Remarks = dto.HRCD_Remarks;
                        dmoObj.HRCD_Resume = dto.HRCD_Resume;
                        dmoObj.HRCD_RecruitmentStatus = dto.HRCD_RecruitmentStatus;
                        dmoObj.HRCD_CreatedBy = dto.HRCD_UpdatedBy;
                        //dmoObj.HRCD_UpdatedBy = dto.HRCD_UpdatedBy;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.HRCD_Photo = dto.HRCD_Photo;
                        dmoObj.HRCD_AadharNo = dto.HRCD_AadharNo;
                        dmoObj.HRCD_PAN = dto.HRCD_PAN;
                        dmoObj.HRCD_AddressLocal = dto.HRCD_AddressLocal;
                        dmoObj.HRCD_AddressLocal2 = dto.HRCD_AddressLocal2;
                        dmoObj.HRCD_AddLocalPlace = dto.HRCD_AddLocalPlace;
                        dmoObj.HRCD_AddLocalPIN = dto.HRCD_AddLocalPIN;
                        dmoObj.HRCD_AddressPermanent = dto.HRCD_AddressPermanent;
                        dmoObj.HRCD_AddressPermanent2 = dto.HRCD_AddressPermanent2;
                        dmoObj.HRCD_AddPermanentPlace = dto.HRCD_AddPermanentPlace;
                        dmoObj.HRCD_AddPermanentPIN = dto.HRCD_AddPermanentPIN;

                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        dto.HRCD_Id = dmoObj.HRCD_Id;
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                            SendNotification(dto);
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
                //dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Candidate_DetailsDTO paynow(HR_Candidate_DetailsDTO dt)
        {
            try
            {
                var alreadyExistEmailId = _VMSContext.HR_Candidate_DetailsDMO.Where(d => d.HRCD_Id == dt.HRCD_Id).ToList();

                dt.HRCD_FirstName = alreadyExistEmailId.FirstOrDefault().HRCD_FirstName;
                dt.HRCD_FirstName = ((alreadyExistEmailId.FirstOrDefault().HRCD_FirstName == null || alreadyExistEmailId.FirstOrDefault().HRCD_FirstName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().HRCD_FirstName) + " " + (alreadyExistEmailId.FirstOrDefault().HRCD_MiddleName == null || alreadyExistEmailId.FirstOrDefault().HRCD_MiddleName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().HRCD_MiddleName) + " " + (alreadyExistEmailId.FirstOrDefault().HRCD_LastName == null || alreadyExistEmailId.FirstOrDefault().HRCD_LastName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().HRCD_LastName)).Trim();
                dt.HRCD_EmailId = alreadyExistEmailId.FirstOrDefault().HRCD_EmailId;
                dt.HRCD_MobileNo = alreadyExistEmailId.FirstOrDefault().HRCD_MobileNo;
                dt.HRCD_AddLocalPlace= alreadyExistEmailId.FirstOrDefault().HRCD_AddLocalPlace;

                dt.paydet = paymentPart(dt);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return dt;
        }

        public Array paymentPart(HR_Candidate_DetailsDTO enq)
        {
            Payment pay = new Payment(_Context);
            ProspectusDTO data = new ProspectusDTO();
            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            int autoinc = 1, totpayableamount = 0;
            string orderId = "";
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

            // List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
            //enq.ASMAY_Id = 7;
            try
            {
                paymentdetails = _feecontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                //paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
                // ProspectusDTO ProspectusDTO = new ProspectusDTO();
                var FeeAmountresult = (from a in _feecontext.feeYCC

                                       from c in _feecontext.feeYCCC
                                       from d in _feecontext.FeeAmountEntryDMO

                                       from g in _feecontext.FeeHeadDMO
                                       where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == enq.ASMAY_Id && d.MI_Id == enq.MI_Id && g.FMH_Flag == "R")
                                       select new FeeAmountEntryDMO
                                       {
                                           FMA_Id = d.FMA_Id,
                                           FMA_Amount = d.FMA_Amount
                                       }
            ).FirstOrDefault();

                try
                {
                    // string ids = enq.ftiidss;

                    using (var cmd1 = _feecontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "Preadmission_Split_Payment_Registration";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.BigInt)
                        {
                            Value = enq.MI_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                        SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                        SqlDbType.VarChar)
                        {
                            Value = enq.HRCD_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@paymentgateway",
                      SqlDbType.VarChar)
                        {
                            Value = "RAZORPAY"
                        });

                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd1.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new FeeSlplitOnlinePayment
                                    {
                                        name = "splitId" + autoinc.ToString(),
                                        merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                        value = dataReader["balance"].ToString(),
                                        commission = "0",
                                        description = "Online Payment",
                                    });

                                    autoinc = autoinc + 1;
                                }
                            }
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

                if (FeeAmountresult != null)
                {


                    PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                    foreach (FeeSlplitOnlinePayment x in result)
                    {
                        totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                    }

                    var item = new
                    {
                        paymentParts = result
                    };

                    string payinfo = JsonConvert.SerializeObject(item);

                    if (enq.onlinepaygteway == "PAYU")
                    {
                        PaymentDetailsDto.trans_id = "PAYU" + enq.MI_Id + DateTime.Now.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (DateTime.Now.Millisecond)).Trim();

                        PaymentDetailsDto.productinfo = payinfo;
                        PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);
                        PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                        PaymentDetailsDto.firstname = enq.HRCD_FirstName;

                        PaymentDetailsDto.email = enq.HRCD_EmailId;

                        PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                        PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().FPGD_URL;
                        PaymentDetailsDto.phone = enq.HRCD_MobileNo;
                        PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
                        PaymentDetailsDto.udf2 = Convert.ToString(enq.HRCD_Id);
                        PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                        PaymentDetailsDto.udf4 = "87";
                        PaymentDetailsDto.udf5 = enq.ASMAY_Id.ToString();
                        PaymentDetailsDto.udf6 = "84";
                        PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/AddCandidateVMSFacade/paymentresponse/";
                        PaymentDetailsDto.status = "success";
                        PaymentDetailsDto.service_provider = "payu_paisa";

                        PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                    }
                    else if (enq.onlinepaygteway == "RAZORPAY")
                    {

                        List<MOBILE_INSTITUTION> instidet = new List<MOBILE_INSTITUTION>();
                        instidet = _feecontext.MOBILE_INSTITUTION.Where(t => t.MI_ID == enq.MI_Id).ToList();


                        List<PaymentDetails> paydet = new List<PaymentDetails>();

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetailsrazorpay = new List<Fee_PaymentGateway_DetailsDMO>();

                        paymentdetailsrazorpay = _feecontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                        List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                        PAYMENTPARAMDETAILS = (from a in _feecontext.PAYUDETAILS
                                               where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   IMPG_IndustryType = a.IMPG_IndustryType,
                                                   IMPG_Website = a.IMPG_Website
                                               }
                        ).ToList();

                        // For Testing Purpose
                        totpayableamount = Convert.ToInt32(totpayableamount);



                        Dictionary<string, object> input = new Dictionary<string, object>();
                        //input.Add("amount", 1 * 100);
                        input.Add("amount", totpayableamount * 100); // this amount should be same as transaction amount
                        input.Add("currency", "INR");
                        input.Add("receipt", "");
                        input.Add("payment_capture", 1);

                        string key = paymentdetailsrazorpay.FirstOrDefault().FPGD_SaltKey;
                        string secret = paymentdetailsrazorpay.FirstOrDefault().FPGD_AuthorisationKey;

                        RazorpayClient client = new RazorpayClient(key, secret);

                        Razorpay.Api.Order order = client.Order.Create(input);
                        orderId = order["id"].ToString();

                        PaymentDetails aa = new PaymentDetails();

                        //added for change in receiptno
                        PaymentDetailsDto.trans_id = orderId;

                        aa.trans_id = orderId;
                        aa.IVRMOP_MERCHANT_KEY = paymentdetailsrazorpay.FirstOrDefault().FPGD_SaltKey;
                        aa.FMA_Amount = totpayableamount;
                        aa.splitpayinformation = payinfo;

                        aa.firstname = enq.HRCD_FirstName;
                        aa.mobile = enq.HRCD_MobileNo.ToString();
                        aa.email = enq.HRCD_EmailId.ToString();
                        aa.PASR_RegistrationNo = enq.HRCD_Id.ToString();
                        if (enq.HRCD_AddLocalPlace != null && enq.HRCD_AddLocalPlace != "")
                        {
                            aa.stuaddress = enq.HRCD_AddLocalPlace.ToString();
                        }
                        else
                        {
                            aa.stuaddress = enq.HRCD_AddLocalPlace.ToString();
                        }

                        aa.institutioname = instidet[0].INSTITUTION_NAME;
                        aa.institulogo = instidet[0].INSTITUTION_LOGO;
                        aa.PASR_ID = enq.HRCD_Id;
                        paydet.Add(aa);
                        PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();
                    }

                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = PaymentDetailsDto.trans_id;
                    onlinemtrans.FMOT_Amount = totpayableamount;
                    onlinemtrans.FMOT_Date = indianTime;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.PASR_Id = enq.HRCD_Id;
                    onlinemtrans.AMST_Id = 0;
                    onlinemtrans.FMOT_Receipt_no = PaymentDetailsDto.trans_id;
                    onlinemtrans.FYP_PayModeType = "APP";
                    onlinemtrans.MI_Id = enq.MI_Id;
                    onlinemtrans.ASMAY_ID =Convert.ToInt64(enq.ASMAY_Id);
                    onlinemtrans.FMOT_PayGatewayType = enq.onlinepaygteway;
                    _feecontext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);
                    _feecontext.SaveChanges();
                    PaymentDetailsDto.paymentdetails = "True";
                }
                else
                {
                    PaymentDetailsDto.paymentdetails = "false";
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return PaymentDetailsDto.PaymentDetailsList;
        }
        public PaymentDetails payuresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
            if (response.status == "success")
            {
                stu.MI_Id = Convert.ToInt64(response.udf3);
                stu.PASR_MobileNo = response.phone;
                stu.pasR_Id = Convert.ToInt64(response.udf2);
                stu.PASR_emailId = response.email;
                stu.ASMAY_Id = Convert.ToInt64(response.udf5);


                data.MI_Id = Convert.ToInt64(response.udf3);
                data.ASMCL_ID = Convert.ToInt64(response.udf4);
                data.ASMAY_Id = Convert.ToInt64(response.udf5);

                string recno = get_grp_reptno(data);

                var confirmstatus = 0;

                if (recno != "0")
                {
                    confirmstatus = _Context.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf6, recno);
                }
                else
                {
                    recno = response.txnid;
                    confirmstatus = _Context.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf6, recno);
                }



                if (confirmstatus > 0)
                {
                    Email Email = new Email(_Context);
                    Email.sendmail(stu.MI_Id, stu.PASR_emailId, "STUDENT_REGISTRATION", stu.pasR_Id);

                    SMS sms = new SMS(_Context);
                    sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "STUDENT_REGISTRATION", stu.pasR_Id);

                }
            }
            else
            {
                dto.status = response.status;
            }

            return response;
        }

        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {
            try
            {
                PaymentDetails dto = new PaymentDetails();
                StudentApplicationDTO stu = new StudentApplicationDTO();
                FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string url = "https://api.razorpay.com/v1/payments/" + response.razorpay_payment_id + "/transfers";
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _Context.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.IVRMOP_MIID && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();
                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);
                response.order_id = payment.Attributes["order_id"];

                //single account added on 17/12/2019

                var accountvalidation = (from a in _Context.Fee_PaymentGateway_Details
                                         where (a.MI_Id == response.IVRMOP_MIID && a.FPGD_PGName == "RAZORPAY")
                                         select new FeeStudentTransactionDTO
                                         {
                                             FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                         }).Distinct().ToArray();

                //single account added on 17/12/2019

                var fetchfmhotid = (from a in _Context.Fee_M_Online_TransactionDMO
                                    where (a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Amount > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Amount = a.FMOT_Amount,
                                        MI_Id = a.MI_Id,
                                        ASMAY_Id = a.ASMAY_ID,
                                        PASR_Id = a.PASR_Id,
                                        FMOT_Receipt_no = a.FMOT_Receipt_no
                                    }).ToArray();
                var fetchstudentdeatils = (from a in _Context.StudentApplication
                                           where (a.pasr_id == Convert.ToInt64(fetchfmhotid[0].PASR_Id))
                                           select new FeeStudentTransactionDTO
                                           {
                                               pasR_MobileNo = a.PASR_MobileNo,
                                               pasR_emailId = a.PASR_emailId,
                                               ASMCL_ID = a.ASMCL_Id,
                                               PASR_RegistrationNo = a.PASR_RegistrationNo,
                                               PASR_FirstName = a.PASR_FirstName + ' ' + a.PASR_MiddleName + ' ' + a.PASR_LastName,
                                               PASR_Id = a.pasr_id
                                           }).ToArray();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();
                Dictionary<String, object> transfers = new Dictionary<String, object>();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    transfers.Clear();
                    var fetchaccountid = (from a in _feecontext.FeeAmountEntryDMO
                                          from b in _feecontext.FeeHeadDMO
                                          from c in _feecontext.Fee_OnlinePayment_Mapping
                                          from d in _feecontext.Fee_PaymentGateway_Details
                                          from e in _feecontext.PAYUDETAILS
                                          from f in _feecontext.feeYCC
                                          from g in _feecontext.feeYCCC
                                          where (a.FMH_Id == b.FMH_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == c.fmg_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && a.FMCC_Id == f.FMCC_Id && f.FYCC_Id == g.FYCC_Id && a.ASMAY_Id == f.ASMAY_Id && a.MI_Id == fetchfmhotid[0].MI_Id && a.ASMAY_Id == fetchfmhotid[0].ASMAY_Id && g.ASMCL_Id == fetchstudentdeatils[0].ASMCL_ID && b.FMH_Flag == "R" && e.IMPG_PGFlag == "RAZORPAY")
                                          select new FeeStudentTransactionDTO
                                          {
                                              FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                          }).Distinct().ToArray();

                    transfersnotes.Add("notes_1", fetchstudentdeatils[0].PASR_FirstName);
                    transfersnotes.Add("notes_2", fetchstudentdeatils[0].PASR_RegistrationNo);
                    transfersnotes.Add("notes_3", fetchstudentdeatils[0].PASR_Id);
                    transfersnotes.Add("notes_4", fetchstudentdeatils[0].pasR_MobileNo);
                    transfersnotes.Add("notes_5", fetchstudentdeatils[0].pasR_emailId);

                    transfers.Add("account", (fetchaccountid.FirstOrDefault().FPGD_SubMerchantId));
                    transfers.Add("amount", (Convert.ToInt32(fetchfmhotid.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString());
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
                            _Context.Add(fet);
                            var contactExists = _Context.SaveChanges();
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
                        fet.AMOUNT = (Convert.ToInt32(fetchfmhotid.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString();
                        fet.CREATED_AT = indianTime.ToString();
                        fet.ORDER_ID = response.order_id;

                        fet.PAYMENT_ID = response.razorpay_payment_id;
                        fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                        fet.SETTLEMENT_FLAG = "0";

                        fet.CREATED_BY = indianTime;
                        fet.UPDATED_BY = indianTime;
                        _Context.Add(fet);
                        var contactExists = _Context.SaveChanges();
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
                //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
                if (response.status == "success")
                {
                    stu.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    stu.PASR_MobileNo = fetchstudentdeatils[0].pasR_MobileNo;
                    stu.pasR_Id = Convert.ToInt64(fetchfmhotid[0].PASR_Id);
                    stu.PASR_emailId = fetchstudentdeatils[0].pasR_emailId;
                    stu.ASMAY_Id = Convert.ToInt64(fetchfmhotid[0].ASMAY_Id);
                    data.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    data.ASMCL_ID = Convert.ToInt64(fetchstudentdeatils[0].ASMCL_ID);
                    data.ASMAY_Id = Convert.ToInt64(fetchfmhotid[0].ASMAY_Id);
                    response.amount = fetchfmhotid.FirstOrDefault().FMA_Amount;
                    //response.responseupdate = fetchfmhotid.FirstOrDefault().FMOT_Receipt_no;
                    string recno = get_grp_reptno(data);
                    var confirmstatus = 0;
                    if (recno != "0")
                    {
                        response.responseupdate = recno;
                        confirmstatus = _Context.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", stu.MI_Id, data.ASMCL_ID, stu.pasR_Id, stu.ASMAY_Id, response.amount, response.order_id, response.razorpay_payment_id, response.udf6, recno);
                    }
                    else
                    {
                        response.responseupdate = response.txnid;
                        recno = response.txnid;
                        confirmstatus = _Context.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", stu.MI_Id, data.ASMCL_ID, stu.pasR_Id, stu.ASMAY_Id, response.amount, response.order_id, response.razorpay_payment_id, response.udf6, recno);
                    }
                    if (confirmstatus > 0)
                    {
                        
                            Email Email = new Email(_Context);
                            Email.sendmail(stu.MI_Id, stu.PASR_emailId, "CANDIDATE", stu.pasR_Id);
                       
                            SMS sms = new SMS(_Context);
                            sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "CANDIDATE", stu.pasR_Id);
                       
                    }
                }
                else
                {
                    dto.status = response.status;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }
        public string get_grp_reptno(FeeStudentTransactionDTO data)
        {
            try
            {
                var FeeAmountresult = (from a in _feecontext.feeYCC
                                       from c in _feecontext.feeYCCC
                                       from d in _feecontext.FeeAmountEntryDMO
                                       from g in _feecontext.FeeHeadDMO
                                       where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && g.FMH_Flag == "R" && c.ASMCL_Id == data.ASMCL_ID)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMH_Id = d.FMH_Id,
                                       }
       ).ToList();

                List<long> HeadId = new List<long>();
                foreach (var item in FeeAmountresult)
                {
                    HeadId.Add(item.FMH_Id);
                }

                List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                grps = (from b in _feecontext.FeeYearlygroupHeadMappingDMO

                        where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                        select new FeeStudentTransactionDTO
                        {
                            FMG_Id = b.FMG_Id
                        }
                       ).Distinct().ToList();

                List<long> grpid = new List<long>();
                string groupidss = "0";
                foreach (var item in grps)
                {
                    grpid.Add(item.FMG_Id);
                }

                for (int r = 0; r < grpid.Count(); r++)
                {
                    groupidss = groupidss + ',' + grpid[r];
                }

                var final_rept_no = "";
                List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

                list_all = (from b in _feecontext.Fee_Groupwise_AutoReceiptDMO
                            from c in _feecontext.Fee_Groupwise_AutoReceipt_GroupsDMO
                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                            select new FeeStudentTransactionDTO
                            {
                                FGAR_PrefixName = b.FGAR_PrefixName,
                                FGAR_SuffixName = b.FGAR_SuffixName,
                                //FGAR_Name = b.FGAR_Name,
                                //FMG_Id = c.FMG_Id
                            }
                     ).Distinct().ToList();

                data.grp_count = list_all.Count();

                if (data.grp_count == 1)
                {


                    using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
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

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data.FYP_Receipt_No;
        }
        public HR_Candidate_DetailsDTO editData(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.retrunMsg = "";
            try
            {
                dto.VMSCandidateList = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                        from b in _VMSContext.HR_Master_PostionTypeDMO
                                        from c in _VMSContext.HR_Master_Course
                                        from d in _VMSContext.IVRM_Master_Gender
                                        where (a.HRCD_Id.Equals(id) && a.HRMPT_Id == b.HRMPT_Id && a.HRMC_Id == c.HRMC_Id && a.IVRMMG_Id == d.IVRMMG_Id)
                                        select new HR_Candidate_DetailsDTO
                                        {
                                            HRCD_Id = a.HRCD_Id,
                                            HRMPT_Id = a.HRMPT_Id,
                                            HRMPT_Name = b.HRMPT_Name,
                                            HRMC_Id = a.HRMC_Id,
                                            HRMC_QulaificationName = c.HRMC_QulaificationName,
                                            HRCD_MRFNO = a.HRCD_MRFNO,
                                            HRCD_FirstName = a.HRCD_FirstName,
                                            HRCD_MiddleName = a.HRCD_MiddleName,
                                            HRCD_LastName = a.HRCD_LastName,
                                            HRMJ_Id = a.HRMJ_Id,
                                            HRCD_Skills = a.HRCD_Skills,
                                            HRCD_DOB = a.HRCD_DOB,
                                            IVRMMG_Id = a.IVRMMG_Id,
                                            IVRMMG_GenderName = d.IVRMMG_GenderName,
                                            HRCD_MobileNo = a.HRCD_MobileNo,
                                            HRCD_EmailId = a.HRCD_EmailId,
                                            HRCD_ExpFrom = a.HRCD_ExpFrom,
                                            HRCD_ExpTo = a.HRCD_ExpTo,
                                            HRCD_CurrentCompany = a.HRCD_CurrentCompany,
                                            HRCD_ResumeSource = a.HRCD_ResumeSource,
                                            HRCD_JobPortalName = a.HRCD_JobPortalName,
                                            HRCD_RefCode = a.HRCD_RefCode,
                                            HRCD_LastCTC = a.HRCD_LastCTC,
                                            HRCD_ExpectedCTC = a.HRCD_ExpectedCTC,
                                            HRCD_AppDate = a.HRCD_AppDate,
                                            HRCD_InterviewDate = a.HRCD_InterviewDate,
                                            HRCD_NoticePeriod = a.HRCD_NoticePeriod,
                                            HRCD_Remarks = a.HRCD_Remarks,
                                            HRCD_Resume = a.HRCD_Resume,
                                            HRCD_RecruitmentStatus = a.HRCD_RecruitmentStatus,
                                            CreatedDate = a.CreatedDate,
                                            HRCD_CreatedBy = a.HRCD_CreatedBy
                                        }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Candidate_DetailsDTO deactivate(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRCD_Id > 0)
                {
                    var result = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dto.HRCD_Id);

                    if (result.HRCD_ActiveFlg == true)
                    {
                        result.HRCD_ActiveFlg = false;
                    }
                    else if (result.HRCD_ActiveFlg == false)
                    {
                        result.HRCD_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRCD_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Candidate_DetailsDTO GetAllDropdownAndDatatableDetails(HR_Candidate_DetailsDTO dto)
        {
            List<HR_Candidate_DetailsDMO> datalist = new List<HR_Candidate_DetailsDMO>();
            List<HR_Candidate_DetailsDTO> DTOdatalistEarning = new List<HR_Candidate_DetailsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();
            try
            {
                dto.MasterPosTypeList = (from emp in _VMSContext.HR_Master_PostionTypeDMO
                                         where emp.HRMPT_ActiveFlg == true
                                         select new HR_Candidate_DetailsDTO
                                         {
                                             HRMPT_Id = emp.HRMPT_Id,
                                             HRMPT_Name = emp.HRMPT_Name
                                         }).ToArray();

                dto.MasterQualification = (from emp in _VMSContext.HR_Master_Course
                                           where emp.HRMC_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                           select new HR_Candidate_DetailsDTO
                                           {
                                               HRMC_Id = emp.HRMC_Id,
                                               HRMC_QulaificationName = emp.HRMC_QulaificationName
                                           }).ToArray();

                dto.MasterGender = (from emp in _VMSContext.IVRM_Master_Gender
                                    where emp.IVRMMG_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                    select new HR_Candidate_DetailsDTO
                                    {
                                        IVRMMG_Id = emp.IVRMMG_Id,
                                        IVRMMG_GenderName = emp.IVRMMG_GenderName
                                    }).ToArray();

                dto.masterjob = (from a in _VMSContext.HR_Master_JobsDMO
                                 where a.HRMJ_ActiveFlg == true
                                 select new HR_Candidate_DetailsDTO
                                 {
                                     HRMJ_Id = a.HRMJ_Id,
                                     HRMJ_JobCode = a.HRMJ_JobCode,
                                     HRMJ_JobTiTle = a.HRMJ_JobTiTle
                                 }).ToArray();

                dto.mastercountry = _VMSContext.IVRM_Master_Country.ToArray();
                dto.masterReligion = _VMSContext.MasterReligionDMO.Where(t => t.Is_Active == true).ToArray();
                dto.masterCaste = _VMSContext.mastercasteDMO.Where(t => t.MI_Id == dto.MI_Id).ToArray();
                dto.mastermaritalstatus = _VMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id == dto.MI_Id && t.IVRMMMS_ActiveFlag == true).ToArray();

                dto.departmenlist = (from a in _Context.HR_Master_Department
                                     where a.MI_Id == dto.MI_Id && a.HRMD_ActiveFlag == true
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRMD_Id = a.HRMD_Id,
                                         HRMD_DepartmentName = a.HRMD_DepartmentName,
                                     }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();

                dto.candidatelist = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                     where (a.MI_Id == dto.MI_Id && a.HRCD_ActiveFlg == true)
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRCD_Id = a.HRCD_Id,
                                         HRCD_FirstName = ((a.HRCD_FirstName == null ? " " : a.HRCD_FirstName) + " " + (a.HRCD_MiddleName == null ? " " : a.HRCD_MiddleName) + " " + (a.HRCD_LastName == null ? " " : a.HRCD_LastName)).Trim(),
                                     }).Distinct().OrderBy(t => t.HRCD_Id).ToArray();

                dto.earingdeductionlist = (from a in _VMSContext.HR_Master_EarningsDeductions
                                           where (a.MI_Id == dto.MI_Id && a.HRMED_ActiveFlag == true)
                                           select new HR_Candidate_DetailsDTO
                                           {
                                               HRMED_Name = a.HRMED_Name,
                                               HRMED_AmountPercentFlag = a.HRMED_AmountPercentFlag,
                                           }).Distinct().ToArray();

                List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
                //Earning list
                earningdatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning") && t.HRMED_ActiveFlag == true).ToList();

                if (earningdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                    {
                        HR_Candidate_DetailsDTO phdto = Mapper.Map<HR_Candidate_DetailsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistEarning.Add(phdto);

                    }

                }

                dto.earningList = DTOdatalistEarning.ToArray();

                List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();
                //Deduction List
                deductiondatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Deduction") && t.HRMED_ActiveFlag == true).ToList();


                if (deductiondatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in deductiondatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistDeduction.Add(phdto);

                    }

                }

                dto.detectionList = DTOdatalistDeduction.ToArray();

                string rolename = _VMSContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == dto.roleId).IVRMRT_Role;
                if (!rolename.Equals("Student", StringComparison.OrdinalIgnoreCase) && !rolename.Equals("Alumni", StringComparison.OrdinalIgnoreCase) && !rolename.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase) && rolename.Equals("Candidate", StringComparison.OrdinalIgnoreCase))
                {
                    var mrfdatalist = from a in _VMSContext.HR_Candidate_DetailsDMO
                                      from b in _VMSContext.HR_Master_JobsDMO
                                      where (a.HRMJ_Id == b.HRMJ_Id && a.MI_Id == dto.MI_Id && a.HRCD_CreatedBy == dto.UserId)
                                      select new HR_Candidate_DetailsDTO
                                      {
                                          HRCD_Id = a.HRCD_Id,
                                          HRCD_FullName = a.HRCD_FirstName + " " + (a.HRCD_MiddleName == null ? "" : a.HRCD_MiddleName) + " " + (a.HRCD_LastName == null ? "" : a.HRCD_LastName),
                                          HRCD_FirstName = a.HRCD_FirstName,
                                          HRCD_MiddleName = a.HRCD_MiddleName,
                                          HRCD_LastName = a.HRCD_LastName,
                                          HRMJ_Id = a.HRMJ_Id,
                                          HRCD_ExpFrom = a.HRCD_ExpFrom,
                                          HRCD_ExpTo = a.HRCD_ExpTo,
                                          applydate = a.HRCD_AppDate.ToString("dd-MM-yyyy"),
                                          HRCD_RecruitmentStatus = a.HRCD_RecruitmentStatus,
                                          HRMJ_JobTiTle = b.HRMJ_JobTiTle,
                                          HRCD_EmailId = a.HRCD_EmailId,
                                          HRCD_MobileNo = a.HRCD_MobileNo
                                      };
                    dto.VMSMRFList = mrfdatalist.ToArray();
                }
                else
                {
                    var mrfdatalist = from a in _VMSContext.HR_Candidate_DetailsDMO
                                      from b in _VMSContext.HR_Master_JobsDMO
                                      where (a.HRMJ_Id == b.HRMJ_Id && a.MI_Id == dto.MI_Id)
                                      select new HR_Candidate_DetailsDTO
                                      {
                                          HRCD_Id = a.HRCD_Id,
                                          HRCD_FullName = a.HRCD_FirstName + " " + (a.HRCD_MiddleName == null ? "" : a.HRCD_MiddleName) + " " + (a.HRCD_LastName == null ? "" : a.HRCD_LastName),
                                          HRCD_FirstName = a.HRCD_FirstName,
                                          HRCD_MiddleName = a.HRCD_MiddleName,
                                          HRCD_LastName = a.HRCD_LastName,
                                          HRMJ_Id = a.HRMJ_Id,
                                          HRCD_ExpFrom = a.HRCD_ExpFrom,
                                          HRCD_ExpTo = a.HRCD_ExpTo,
                                          applydate = a.HRCD_AppDate.ToString("dd-MM-yyyy"),
                                          HRCD_RecruitmentStatus = a.HRCD_RecruitmentStatus,
                                          HRMJ_JobTiTle = b.HRMJ_JobTiTle
                                      };
                    dto.VMSMRFList = mrfdatalist.ToArray();

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }



            return dto;
        }

        public HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO data)
        {
            try
            {
                data.desgnationlist = (from a in _Context.HR_Master_Employee_DMO
                                       from b in _Context.HR_Master_Department
                                       from c in _Context.HR_Master_Designation
                                       where (a.HRMD_Id == data.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                       select new HR_Candidate_DetailsDTO
                                       {
                                           HRMDES_Id = c.HRMDES_Id,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                                       }).Distinct().OrderBy(t => t.HRMDES_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Candidate_DetailsDTO saveAppointmentdata(HR_Candidate_DetailsDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Candidate_DetailsDTO addQualification(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int icount = 0; icount < dto.QualificationsDTO.Length; icount++)
                {
                    HR_Candidate_QualificationsDMO dmoObj = Mapper.Map<HR_Candidate_QualificationsDMO>(dto.QualificationsDTO[icount]);
                    var duplicatecountresult = _VMSContext.HR_Candidate_QualificationsDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.Employeedto.HRCD_Id && t.HRCQUAL_Course == dmoObj.HRCQUAL_Course && t.HRCQUAL_Board == dmoObj.HRCQUAL_Board && t.HRCQUAL_PassingYear == dmoObj.HRCQUAL_PassingYear).Count();
                    if (duplicatecountresult == 0)
                    {
                        if (dmoObj.HRCQUAL_Id > 0)
                        {
                            var result = _VMSContext.HR_Candidate_QualificationsDMO.Where(t => t.HRCQUAL_Id == dmoObj.HRCQUAL_Id).FirstOrDefault();
                            result.HRCQUAL_UpdatedBy = dto.UserId;
                            result.UpdatedDate = DateTime.Now;
                            result.HRCQUAL_ActiveFlag = true;
                            Mapper.Map(dmoObj, result);
                            _VMSContext.Update(result);
                            var flag = _VMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            dmoObj.HRCD_Id = dto.Employeedto.HRCD_Id;
                            dmoObj.MI_Id = dto.Employeedto.MI_Id;
                            dmoObj.HRCQUAL_ActiveFlag = true;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRCQUAL_CreatedBy = dto.UserId;
                            dmoObj.HRCQUAL_UpdatedBy = dto.UserId;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Candidate_DetailsDTO addexperience(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int icount = 0; icount < dto.ExperienceDTO.Length; icount++)
                {
                    HR_Candidate_ExperienceDMO dmoObj = Mapper.Map<HR_Candidate_ExperienceDMO>(dto.ExperienceDTO[icount]);
                    var duplicatecountresult = _VMSContext.HR_Candidate_ExperienceDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.Employeedto.HRCD_Id && t.HRCEXP_CompanyName == dmoObj.HRCEXP_CompanyName && t.HRCEXP_Designation == dmoObj.HRCEXP_Designation && t.HRCEXP_From == dmoObj.HRCEXP_From && t.HRCEXP_To == dmoObj.HRCEXP_To && t.HRCEXP_Salary == dmoObj.HRCEXP_Salary).Count();
                    if (duplicatecountresult == 0)
                    {
                        if (dmoObj.HRCEXP_Id > 0)
                        {
                            var result = _VMSContext.HR_Candidate_ExperienceDMO.Where(t => t.HRCEXP_Id == dmoObj.HRCEXP_Id).FirstOrDefault();
                            result.HRCEXP_UpdatedBy = dto.UserId;
                            result.UpdatedDate = DateTime.Now;
                            result.HRCEXP_ActiveFlag = true;
                            Mapper.Map(dmoObj, result);
                            _VMSContext.Update(result);
                            var flag = _VMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            dmoObj.HRCD_Id = dto.Employeedto.HRCD_Id;
                            dmoObj.MI_Id = dto.Employeedto.MI_Id;
                            dmoObj.HRCEXP_ActiveFlag = true;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRCEXP_CreatedBy = dto.UserId;
                            dmoObj.HRCEXP_UpdatedBy = dto.UserId;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Candidate_DetailsDTO addfamily(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int icount = 0; icount < dto.FamilyDTO.Length; icount++)
                {
                    HR_Candidate_FamilyDMO dmoObj = Mapper.Map<HR_Candidate_FamilyDMO>(dto.FamilyDTO[icount]);
                    var duplicatecountresult = _VMSContext.HR_Candidate_FamilyDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.Employeedto.HRCD_Id && t.HRCFAM_Name == dmoObj.HRCFAM_Name && t.HRCFAM_Relationship == dmoObj.HRCFAM_Relationship && t.HRCFAM_Occupation == dmoObj.HRCFAM_Occupation && t.HRCFAM_CompanyName == dmoObj.HRCFAM_CompanyName && t.HRCFAM_Age == dmoObj.HRCFAM_Age).Count();
                    if (duplicatecountresult == 0)
                    {
                        if (dmoObj.HRCFAM_Id > 0)
                        {
                            var result = _VMSContext.HR_Candidate_FamilyDMO.Where(t => t.HRCFAM_Id == dmoObj.HRCFAM_Id).FirstOrDefault();
                            result.HRCFAM_UpdatedBy = dto.UserId;
                            result.UpdatedDate = DateTime.Now;
                            result.HRCFAM_ActiveFlag = true;
                            Mapper.Map(dmoObj, result);
                            _VMSContext.Update(result);
                            var flag = _VMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            dmoObj.HRCD_Id = dto.Employeedto.HRCD_Id;
                            dmoObj.MI_Id = dto.Employeedto.MI_Id;
                            dmoObj.HRCFAM_ActiveFlag = true;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRCFAM_CreatedBy = dto.UserId;
                            dmoObj.HRCFAM_UpdatedBy = dto.UserId;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Candidate_DetailsDTO addlanguage(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int icount = 0; icount < dto.LanguagesDTO.Length; icount++)
                {
                    HR_Candidate_LanguagesDMO dmoObj = Mapper.Map<HR_Candidate_LanguagesDMO>(dto.LanguagesDTO[icount]);
                    var duplicatecountresult = _VMSContext.HR_Candidate_LanguagesDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.Employeedto.HRCD_Id && t.HRCLAN_ToRead == dmoObj.HRCLAN_ToRead && t.HRCLAN_ToWrite == dmoObj.HRCLAN_ToWrite && t.HRCLAN_ToSpeak == dmoObj.HRCLAN_ToSpeak).Count();
                    if (duplicatecountresult == 0)
                    {
                        if (dmoObj.HRCLAN_Id > 0)
                        {
                            var result = _VMSContext.HR_Candidate_LanguagesDMO.Where(t => t.HRCLAN_Id == dmoObj.HRCLAN_Id).FirstOrDefault();
                            result.HRCLAN_UpdatedBy = dto.UserId;
                            result.UpdatedDate = DateTime.Now;
                            result.HRCLAN_ActiveFlag = true;
                            Mapper.Map(dmoObj, result);
                            _VMSContext.Update(result);
                            var flag = _VMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            dmoObj.HRCD_Id = dto.Employeedto.HRCD_Id;
                            dmoObj.MI_Id = dto.Employeedto.MI_Id;
                            dmoObj.HRCLAN_ActiveFlag = true;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRCLAN_CreatedBy = dto.UserId;
                            dmoObj.HRCLAN_UpdatedBy = dto.UserId;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public void SendNotification(HR_Candidate_DetailsDTO data)
        {
            try
            {
                string companyname = _VMSContext.Institution.Where(t => t.MI_Id == data.MI_Id).Select(t => t.MI_Name).FirstOrDefault();

                string str = "";
                str += "<table width='100%' border= '1'><tr align='center' width='100%'>Dear " + data.HRCD_FirstName + " Welcome to " + companyname + " </tr></table>";

                if (data.HRCD_EmailId != null && data.HRCD_EmailId != "")
                {
                    SendEmail(data.HRCD_EmailId, "Welcome Notification", str, 17);
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void SendEmail(string mailid, string subject, string body, long id)
        {
            //mailid = "goutamkumar@vapstech.com";
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_Template_Name.Equals("FO", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }

                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);

                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }


                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
        }

        public HR_Candidate_DetailsDTO sendCallLettermail(HR_Candidate_DetailsDTO dto)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == "CallLetterTemplate" && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    dto.retrunMsg = "Email Template not Mapped to the selected Institution";
                    return dto;
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == dto.MI_Id).ToList();
                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == dto.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                //TEMPLATE EDITING
                Mailmsg = dto.Template;
                //TEMPLATE EDITING

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');
                        mailcc = ccmail[0].ToString();
                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }
                    }
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(dto.HRCD_EmailId);

                    if (Attechement.Equals("1"))
                    {
                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == "CallLetterTemplate" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = dto.HRCD_EmailId
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
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
                            dto.retrunMsg = ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.retrunMsg = ex.Message;
            }

            dto.retrunMsg = "success";
            VMS_CallLetter_sentDMO obj = new VMS_CallLetter_sentDMO();
            obj.VMSCS_EmailId = dto.HRCD_EmailId;
            obj.VMSCS_SentDate = DateTime.Now;
            obj.VMSCS_CreatedBy = dto.UserId;
            obj.VMSCS_updatedBy = dto.UserId;
            _VMSContext.Add(obj);
            _VMSContext.SaveChanges();
            return dto;
        }

        public void Thanksgivingmail(HR_Candidate_DetailsDTO obj, string subject, string body, long id)
        {
            try
            {
                string jobtitle = _VMSContext.HR_Master_JobsDMO.Where(t => t.HRMJ_Id == obj.HRMJ_Id).Select(t => t.HRMJ_JobTiTle).FirstOrDefault();
                string companyname = _VMSContext.Institution.Where(t => t.MI_Id == obj.MI_Id).Select(t => t.MI_Name).FirstOrDefault();

                string mailid = obj.HRCD_EmailId;
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Thanks_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = "hr@vapsknowledge.com";
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = message.HtmlContent.Replace("[DATE]", DateTime.Today.ToString("dd/MM/yyyy"));
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", obj.HRCD_FirstName + " " + (obj.HRCD_MiddleName == null ? "" : obj.HRCD_MiddleName) + " " + (obj.HRCD_LastName == null ? "" : obj.HRCD_LastName));
                        message.HtmlContent = message.HtmlContent.Replace("[JOBPOST]", jobtitle);
                        message.HtmlContent = message.HtmlContent.Replace("[COMPANY]", companyname);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Thanks_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Welcomemail(HR_Candidate_DetailsDTO obj, string subject, string body, long id)
        {
            try
            {
                string mailid = "hr@vapstech.com";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Selected_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", obj.HRCD_FirstName + " " + (obj.HRCD_MiddleName == null ? "" : obj.HRCD_MiddleName) + " " + (obj.HRCD_LastName == null ? "" : obj.HRCD_LastName));
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }
                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Selected_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public HR_Candidate_DetailsDTO saveAppointmenttab(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Candidate_AppointmentDMO dmoObj = Mapper.Map<HR_Candidate_AppointmentDMO>(dto);
                var duplicatecountresult = _VMSContext.HR_Candidate_AppointmentDMO.Where(t => t.HRCD_Id == dto.Employeedto.HRCD_Id && t.HRCA_FirstDocName == dto.HRCA_FirstDocName && t.HRCA_FirstDocDate == dto.HRCA_FirstDocDate && t.HRCA_SecDocName == dto.HRCA_SecDocName && t.HRCA_SecDocDate == dto.HRCA_SecDocDate && t.HRCA_AnnualCTC == dto.HRCA_AnnualCTC && t.HRCA_MonthlyCTC == dto.HRCA_MonthlyCTC && t.HRCA_AppointmentRefNo == dto.HRCA_AppointmentRefNo && t.HRCA_AcknowledgementRefNo == dto.HRCA_AcknowledgementRefNo).Count();
                if (duplicatecountresult == 0)
                {
                    if (dto.HRCA_Id > 0)
                    {
                        var result = _VMSContext.HR_Candidate_AppointmentDMO.Where(t => t.HRCA_Id == dto.HRCA_Id).FirstOrDefault();
                        result.UpdatedDate = DateTime.Now;
                        result.HRCA_FirstDocName = dto.HRCA_FirstDocName;
                        result.HRCA_FirstDocDate = dto.HRCA_FirstDocDate;
                        result.HRCA_SecDocName = dto.HRCA_SecDocName;
                        result.HRCA_SecDocDate = dto.HRCA_SecDocDate;
                        result.HRCA_AnnualCTC = dto.HRCA_AnnualCTC;
                        result.HRCA_MonthlyCTC = dto.HRCA_MonthlyCTC;
                        result.HRCA_AppointmentRefNo = dto.HRCA_AppointmentRefNo;
                        result.HRCA_AcknowledgementRefNo = dto.HRCA_AcknowledgementRefNo;
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRCD_Id = dto.Employeedto.HRCD_Id;
                        dmoObj.HRCA_FirstDocName = dto.HRCA_FirstDocName;
                        dmoObj.HRCA_FirstDocDate = dto.HRCA_FirstDocDate;
                        dmoObj.HRCA_SecDocName = dto.HRCA_SecDocName;
                        dmoObj.HRCA_SecDocDate = dto.HRCA_SecDocDate;
                        dmoObj.HRCA_AnnualCTC = dto.HRCA_AnnualCTC;
                        dmoObj.HRCA_MonthlyCTC = dto.HRCA_MonthlyCTC;
                        dmoObj.HRCA_AppointmentRefNo = dto.HRCA_AppointmentRefNo;
                        dmoObj.HRCA_AcknowledgementRefNo = dto.HRCA_AcknowledgementRefNo;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.UpdatedDate = DateTime.Now;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
    }
}
