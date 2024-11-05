using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    public class BuspassFormImpl : Interfaces.BuspassFormInterface
    {
        private static ConcurrentDictionary<string, StudentApplicationDTO> _login =
             new ConcurrentDictionary<string, StudentApplicationDTO>();
        public StudentApplicationContext _StudentApplicationContext;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly DomainModelMsSqlServerContext _db;
        public FeeGroupContext _feecontext;
        public ProspectusContext _ProspectusContext;

        public BuspassFormImpl(StudentApplicationContext StudentApplicationContext, UserManager<ApplicationUser> UserManager, DomainModelMsSqlServerContext db,  FeeGroupContext feecontext, ProspectusContext ProspectusContext)
        {
            _StudentApplicationContext = StudentApplicationContext;
            _UserManager = UserManager;
            _feecontext = feecontext;
            _ProspectusContext = ProspectusContext;
            _db = db;
        }

        public StudentHelthcertificateDTO getloaddata(StudentHelthcertificateDTO data)
        {
            try
            {
                List<StudentHelthcertificateDMO> allRegStudent = new List<StudentHelthcertificateDMO>();
                allRegStudent = _StudentApplicationContext.StudentHelthcertificate.Where(d => d.MI_Id.Equals(data.MI_Id)).ToList();
                data.studentDetailsTEmp = allRegStudent.ToArray();

                List<PA_Student_Transport_ApplicationDMO> transport  = new List<PA_Student_Transport_ApplicationDMO>();
                transport = _StudentApplicationContext.PA_Student_Transport_ApplicationDMO.Where(d => d.MI_Id.Equals(data.MI_Id)).ToList();
                data.transport = transport.ToArray();

                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                data.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "T").ToArray();

                List<long> temparr = new List<long>();
                for (int i = 0; i < data.studentDetailsTEmp.Length; i++)
                {
                    temparr.Add(allRegStudent[i].PASR_Id);
                }


                List<long> temparrtransport  = new List<long>();
                for (int i = 0; i < data.transport.Length; i++)
                {
                    temparrtransport.Add(transport[i].PASR_Id);
                }

                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleId).IVRMRT_Role;

                if (rolename == "OnlinePreadmissionUser")
                {

                    data.studentDetails = (from a in _StudentApplicationContext.Enq
                                           from b in _StudentApplicationContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                           from c in _StudentApplicationContext.StudentHelthcertificate
                                               // from d in _StudentApplicationContext.PA_Student_Transport_ApplicationDMO
                                           where (a.pasr_id == b.PASA_Id && a.pasr_id == c.PASR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && !temparrtransport.Contains(a.pasr_id) && a.PASR_Adm_Confirm_Flag == false && a.Id==data.Id)
                                           select new StudentHelthcertificateDTO
                                           {
                                               PASR_Id = a.pasr_id,
                                               PASR_FirstName = a.PASR_FirstName,
                                               PASR_MiddleName = a.PASR_MiddleName,
                                               PASR_LastName = a.PASR_LastName
                                           }
                 ).ToList().ToArray();



                    //---GridData
                    data.routeDetails = (from a in _StudentApplicationContext.PA_Student_Transport_ApplicationDMO
                                         from b in _StudentApplicationContext.MasterLocationDMO
                                         from c in _StudentApplicationContext.MasterRouteDMO
                                         from d in _StudentApplicationContext.Enq
                                         where (a.PASTA_PickUp_TRML_Id == b.TRML_Id && a.PASTA_PickUp_TRMR_Id == c.TRMR_Id && a.PASTA_Drop_TRMR_Id == c.TRMR_Id && a.PASR_Id == d.pasr_id
                                         && d.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.Id == data.Id)
                                         select new StudentHelthcertificateDTO
                                         {
                                             PASTA_Id = a.PASTA_Id,
                                             PASR_Id = a.PASR_Id,
                                             PASR_FirstName = d.PASR_FirstName,
                                             PASTA_PickUp_TRML_Id = a.PASTA_PickUp_TRML_Id,
                                             TRML_DropLocationName = b.TRML_LocationName,
                                             PASTA_PickUp_TRMR_Id = a.PASTA_PickUp_TRMR_Id,
                                             TRMR_RouteName = c.TRMR_RouteName,
                                             PASTA_Drop_TRML_Id = a.PASTA_Drop_TRML_Id,
                                             TRML_PickLocationName = b.TRML_LocationName,
                                             PASTA_Drop_TRMR_Id = a.PASTA_Drop_TRMR_Id
                                         }
                    ).ToList().ToArray();
                }
                else
                {
                    data.studentDetails = (from a in _StudentApplicationContext.Enq
                                           from b in _StudentApplicationContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                           from c in _StudentApplicationContext.StudentHelthcertificate
                                               // from d in _StudentApplicationContext.PA_Student_Transport_ApplicationDMO
                                           where (a.pasr_id == b.PASA_Id && a.pasr_id == c.PASR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && !temparrtransport.Contains(a.pasr_id) && a.PASR_Adm_Confirm_Flag == false)
                                           select new StudentHelthcertificateDTO
                                           {
                                               PASR_Id = a.pasr_id,
                                               PASR_FirstName = a.PASR_FirstName,
                                               PASR_MiddleName = a.PASR_MiddleName,
                                               PASR_LastName = a.PASR_LastName
                                           }
             ).ToList().ToArray();



                    //---GridData
                    data.routeDetails = (from a in _StudentApplicationContext.PA_Student_Transport_ApplicationDMO
                                         from b in _StudentApplicationContext.MasterLocationDMO
                                         from c in _StudentApplicationContext.MasterRouteDMO
                                         from d in _StudentApplicationContext.Enq
                                         where (a.PASTA_PickUp_TRML_Id == b.TRML_Id && a.PASTA_PickUp_TRMR_Id == c.TRMR_Id && a.PASTA_Drop_TRMR_Id == c.TRMR_Id && a.PASR_Id == d.pasr_id
                                         && d.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                         select new StudentHelthcertificateDTO
                                         {
                                             PASTA_Id = a.PASTA_Id,
                                             PASR_Id = a.PASR_Id,
                                             PASR_FirstName = d.PASR_FirstName,
                                             PASTA_PickUp_TRML_Id = a.PASTA_PickUp_TRML_Id,
                                             TRML_DropLocationName = b.TRML_LocationName,
                                             PASTA_PickUp_TRMR_Id = a.PASTA_PickUp_TRMR_Id,
                                             TRMR_RouteName = c.TRMR_RouteName,
                                             PASTA_Drop_TRML_Id = a.PASTA_Drop_TRML_Id,
                                             TRML_PickLocationName = b.TRML_LocationName,
                                             PASTA_Drop_TRMR_Id = a.PASTA_Drop_TRMR_Id
                                         }
                    ).ToList().ToArray();

                }

                    //----------Area
                    List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                    saa = _StudentApplicationContext.MasterAreaDMO.Where(r => r.MI_Id == data.MI_Id).ToList();
                    data.areaList = saa.ToArray();
                }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentHelthcertificateDTO getroutedata(StudentHelthcertificateDTO data)
        {
            try
            {
                data.routelist = (from a in _StudentApplicationContext.MasterAreaDMO
                                  from b in _StudentApplicationContext.MasterRouteDMO
                                  where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                  select new StudentHelthcertificateDTO
                                  {
                                      TRMR_Id = b.TRMR_Id,
                                      TRMR_RouteName = b.TRMR_RouteName
                                  }
                 ).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentHelthcertificateDTO getlocationdata(StudentHelthcertificateDTO data)
        {
            try
            {
                data.locationlist = (from a in _StudentApplicationContext.Route_Location
                                     from b in _StudentApplicationContext.MasterRouteDMO
                                     from c in _StudentApplicationContext.MasterLocationDMO
                                     from d in _StudentApplicationContext.MasterAreaDMO
                                     where (a.TRMR_Id == b.TRMR_Id && b.TRMA_Id==d.TRMA_Id && a.TRML_Id == c.TRML_Id && d.MI_Id == data.MI_Id && b.TRMR_Id==data.TRMR_Id && b.TRMA_Id==data.TRMA_Id)
                                     select new StudentHelthcertificateDTO
                                     {
                                         TRML_Id = c.TRML_Id,
                                         TRML_LocationName = c.TRML_LocationName
                                     }
                 ).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<StudentHelthcertificateDTO> getstudata(StudentHelthcertificateDTO data)
        {
            try
            {
                using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "bus_pass_app";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@pasr",
                SqlDbType.BigInt)
                    {
                        Value = data.pasr_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.BigInt)
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
                        data.studetailslist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.studentpercountry = (from a in _StudentApplicationContext.Enq
                                          from b in _StudentApplicationContext.country
                                          where (a.PASR_PerCountry == Convert.ToString(b.IVRMMC_Id) && a.pasr_id == data.pasr_id)
                                          select new StudentHelthcertificateDTO
                                          {
                                              IVRMMC_Id = b.IVRMMC_Id,
                                              IVRMMC_CountryName = b.IVRMMC_CountryName
                                          }).ToArray();
                data.studentconstate = (from a in _StudentApplicationContext.Enq
                                        from b in _StudentApplicationContext.state
                                        where (a.PASR_ConState == b.IVRMMS_Id && a.pasr_id == data.pasr_id)
                                        select new StudentHelthcertificateDTO
                                        {
                                            IVRMMS_Id = b.IVRMMS_Id,
                                            IVRMMS_Name = b.IVRMMS_Name
                                        }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<StudentHelthcertificateDTO> getbuspassdata(StudentHelthcertificateDTO data)
        {
            try
            {
                using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "bus_pass_details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@pasr",
                SqlDbType.BigInt)
                    {
                        Value = data.pasr_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.BigInt)
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
                        data.buspassdatalist = retObject.ToArray();
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

        public StudentHelthcertificateDTO savedata(StudentHelthcertificateDTO data)
        {
            try
            {
                var duplicatecount = _StudentApplicationContext.PA_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.PASR_Id == data.pasr_id && t.TRMA_Id == data.TRMA_Id).Count();
                if (duplicatecount == 0)
                {
                    PA_Student_Transport_ApplicationDMO admiss = Mapper.Map<PA_Student_Transport_ApplicationDMO>(data);

                    admiss.PASTA_PickUp_TRML_Id = data.TRML_Idp;
                    admiss.PASTA_Drop_TRML_Id = data.TRML_Idd;
                    admiss.PASTA_Drop_TRMR_Id = data.TRMR_Id;
                    admiss.PASTA_PickUp_TRMR_Id = data.TRMR_Id;

                    admiss.CreatedDate = DateTime.Now;
                    admiss.UpdatedDate = DateTime.Now;

                    _StudentApplicationContext.Add(admiss);
                    List<StudentApplication> studentdetails  = new List<StudentApplication>();
                    studentdetails = _StudentApplicationContext.Enq.Where(t => t.pasr_id == data.pasr_id).ToList();
                    if(studentdetails.Count()>0)
                    {
                        data.ASMCL_Id = studentdetails.FirstOrDefault().ASMCL_Id;
                        data.ASMAY_Id = studentdetails.FirstOrDefault().ASMAY_Id;
                        data.PASR_MobileNo = studentdetails.FirstOrDefault().PASR_MobileNo;
                        data.PASR_emailId= studentdetails.FirstOrDefault().PASR_emailId;
                       
                            data.paymentapplicable = "Pay";
                            data.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "T" && t.PASA_Id == data.pasr_id).Count();

                            if (data.payementcheck == 0)
                            {
                                data.paydet = paymentPart(data);
                            }
                      
                    }

                    int contactExists = _StudentApplicationContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    data.message = "Duplicate";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public Array paymentPart(StudentHelthcertificateDTO enq)
        {
            Payment pay = new Payment(_db);
            ProspectusDTO data = new ProspectusDTO();
            List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            int autoinc = 1, totpayableamount = 0;

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            //enq.ASMAY_Id = 7;
            try
            {
                paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
                // ProspectusDTO ProspectusDTO = new ProspectusDTO();
                var FeeAmountresult = (from a in _feecontext.feeYCC

                                       from c in _feecontext.feeYCCC
                                       from d in _feecontext.FeeAmountEntryDMO

                                       from g in _feecontext.FeeHeadDMO
                                       from e in _feecontext.FeeGroupDMO
                                       where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == enq.ASMAY_Id && d.MI_Id == enq.MI_Id &&   d.FMG_Id == e.FMG_Id && g.FMH_Flag == "T" && e.FMG_CompulsoryFlag == "T" && c.ASMCL_Id == enq.ASMCL_Id)
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
                        cmd1.CommandText = "Preadmission_Transport_Split_Payment_Registration";
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
                            Value = enq.PASR_Id
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


                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
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

                    PaymentDetailsDto.productinfo = payinfo;
                    PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);
                    PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                    PaymentDetailsDto.firstname = enq.PASR_FirstName;


                    PaymentDetailsDto.email = enq.PASR_emailId;

                    PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
                    PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
                    PaymentDetailsDto.phone = enq.PASR_MobileNo;
                    PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
                    PaymentDetailsDto.udf2 = Convert.ToString(enq.PASR_Id);
                    PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                    PaymentDetailsDto.udf4 = enq.ASMCL_Id.ToString();
                    PaymentDetailsDto.udf5 = enq.ASMAY_Id.ToString();
                    PaymentDetailsDto.udf6 = enq.ASMCL_Id.ToString();
                    // PaymentDetailsDto.transaction_response_url = "";
                    PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/BuspassForm/paymentresponse/";
                    PaymentDetailsDto.status = "success";
                    PaymentDetailsDto.service_provider = "payu_paisa";

                    PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);



                    FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
                    feepaydet.MI_Id = enq.MI_Id;
                    feepaydet.ASMAY_ID = enq.ASMAY_Id;

                    feepaydet.FTCU_Id = 1;
                    feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
                    feepaydet.FYP_Bank_Name = "";
                    feepaydet.FYP_Bank_Or_Cash = "O";
                    feepaydet.FYP_DD_Cheque_No = "";
                    feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
                    feepaydet.FYP_Date = DateTime.Now;
                    feepaydet.FYP_Tot_Amount = PaymentDetailsDto.amount;
                    feepaydet.FYP_Tot_Waived_Amt = 0;
                    feepaydet.FYP_Tot_Fine_Amt = 0;
                    feepaydet.FYP_Tot_Concession_Amt = 0;
                    feepaydet.FYP_Remarks = "Preadmission Transport Registration";
                    feepaydet.FYP_Chq_Bounce = "CL";
                    feepaydet.DOE = DateTime.Now;
                    feepaydet.CreatedDate = DateTime.Now;
                    feepaydet.UpdatedDate = DateTime.Now;
                    feepaydet.user_id = enq.Id;
                    feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
                    feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
                    feepaydet.FYP_PaymentReference_Id = "";

                    _feecontext.FeePaymentDetailsDMO.Add(feepaydet);
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
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Transport_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf6, recno);
                }
                else
                {
                    recno = response.txnid;
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Transport_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf6, recno);
                }



                if (confirmstatus > 0)
                {

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();

                    //if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                    //{
                    //    Email Email = new Email(_db);

                    //    Email.sendmail(stu.MI_Id, stu.PASR_emailId, "STUDENT_REGISTRATION", stu.pasR_Id);
                    //}

                    //if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                    //{
                    //    SMS sms = new SMS(_db);
                    //    sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "STUDENT_REGISTRATION", stu.pasR_Id);

                    //}
                }



            }
            else
            {
                dto.status = response.status;
            }

            return response;
        }

        public string get_grp_reptno(FeeStudentTransactionDTO data)
        {
            try
            {

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _db.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<long> temparr = new List<long>();
                for (int i = 0; i < feemasnum.Count; i++)
                {
                    data.auto_receipt_flag = feemasnum[i].FMC_AutoReceiptFeeGroupFlag;
                }

                if (data.auto_receipt_flag == 1)
                {

                    var FeeAmountresult = (from a in _feecontext.feeYCC

                                           from c in _feecontext.feeYCCC
                                           from d in _feecontext.FeeAmountEntryDMO

                                           from g in _feecontext.FeeHeadDMO
                                           from e in _feecontext.FeeGroupDMO
                                           where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.FMG_Id == e.FMG_Id && g.FMH_Flag == "T" && e.FMG_CompulsoryFlag == "T" && c.ASMCL_Id == data.ASMCL_ID)
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

                        //data.auto_FYP_Receipt_No = final_rept_no;

                        //data.FYP_Receipt_No = final_rept_no;
                    }
                }
                else
                {
                    data.FYP_Receipt_No = "0";
                }

                //else if (data.automanualreceiptno == "Auto")
                //{
                //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                //    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                //}

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data.FYP_Receipt_No;
        }
        public StudentHelthcertificateDTO paynow(StudentHelthcertificateDTO dt)
        {

            try
            {
                var alreadyExistEmailId = _StudentApplicationContext.Enq.Where(d => d.pasr_id == dt.PASR_Id).ToList();
                var stuid = _StudentApplicationContext.Enq.Single(d => d.pasr_id == dt.PASR_Id).ASMAY_Id;

                dt.ASMCL_Id = alreadyExistEmailId.FirstOrDefault().ASMCL_Id;
                dt.ASMAY_Id = alreadyExistEmailId.FirstOrDefault().ASMAY_Id;
                dt.PASR_FirstName = alreadyExistEmailId.FirstOrDefault().PASR_FirstName;
                dt.PASR_emailId = alreadyExistEmailId.FirstOrDefault().PASR_emailId;
                dt.PASR_MobileNo = alreadyExistEmailId.FirstOrDefault().PASR_MobileNo;


               
                    dt.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "T" && t.PASA_Id == dt.PASR_Id).Count();

                    if (dt.payementcheck == 0)
                    {
                        dt.paydet = paymentPart(dt);
                    }
               

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return dt;
        }

    }
}
