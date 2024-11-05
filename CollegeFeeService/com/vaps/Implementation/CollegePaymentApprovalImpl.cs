using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using System.Net;
using System.IO;
using SendGrid;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegePaymentApprovalImpl : Interfaces.CollegePaymentApprovalInterface
    {

        public CollFeeGroupContext _ClgFeeGroupContext;
        public DomainModelMsSqlServerContext _context;
        public CollegePaymentApprovalImpl(CollFeeGroupContext frgContext, DomainModelMsSqlServerContext context)
        {
            _ClgFeeGroupContext = frgContext;
            _context = context;
        }

        public CollegePaymentApprovalDTO getdetails(CollegePaymentApprovalDTO data)
        {

            try
            {

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _ClgFeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CollegePaymentApprovalDTO Getreportdetails(CollegePaymentApprovalDTO data)
        {


            try
            {


                //  List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                var feemasnum = _ClgFeeGroupContext.feemastersettings.Where(t => t.MI_Id == data.MI_ID).ToList();//&& t.userid == data.UserId
                var flag = "";
                var makerchecker = "";//  data.feeconfiglist = feemasnum.ToArray();
                if (feemasnum.Count > 0)
                {
                    flag = feemasnum.FirstOrDefault().FMC_GroupOrTermFlg;
                    makerchecker = feemasnum.FirstOrDefault().FMC_MakerCheckerReqdFlg.ToString();
                }

                if (makerchecker == "True")
                {

                    using (var cmd = _ClgFeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG_FEE_PAYMENT_APPROVALSTUDENTLIST2021";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.VarChar)
                        {
                            Value = data.MI_ID

                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@User_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                       SqlDbType.VarChar)
                        {
                            Value = data.fromdate.ToString()

                        });
                        cmd.Parameters.Add(new SqlParameter("@Todate",
                           SqlDbType.VarChar)
                        {
                            Value = data.todate.ToString()

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
                            data.feepaymentreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _ClgFeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "AdvanceFeeCollectionDetails";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
          SqlDbType.VarChar)
                        {
                            Value = data.MI_ID
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                       SqlDbType.VarChar)
                        {
                            Value = data.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                     SqlDbType.VarChar)
                        {
                            Value = data.todate
                        });



                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObjectadvance = new List<dynamic>();
                        try
                        {
                            using (var dataReaderadvance = cmd.ExecuteReader())
                            {
                                while (dataReaderadvance.Read())
                                {
                                    var dataRowadvance = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReaderadvance.FieldCount; iFiled++)
                                    {
                                        dataRowadvance.Add(
                                            dataReaderadvance.GetName(iFiled),
                                            dataReaderadvance.IsDBNull(iFiled) ? null : dataReaderadvance[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObjectadvance.Add((ExpandoObject)dataRowadvance);
                                }
                                data.fillstudentviewdetailsadvance = retObjectadvance.Distinct().ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _ClgFeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SP_PAYMENTREMARKS";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
          SqlDbType.VarChar)
                        {
                            Value = data.MI_ID
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                       SqlDbType.VarChar)
                        {
                            Value = data.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                     SqlDbType.VarChar)
                        {
                            Value = data.todate
                        });



                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObjectremark = new List<dynamic>();
                        try
                        {
                            using (var dataReaderremark = cmd.ExecuteReader())
                            {
                                while (dataReaderremark.Read())
                                {
                                    var dataRowremark = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReaderremark.FieldCount; iFiled++)
                                    {
                                        dataRowremark.Add(
                                            dataReaderremark.GetName(iFiled),
                                            dataReaderremark.IsDBNull(iFiled) ? null : dataReaderremark[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObjectremark.Add((ExpandoObject)dataRowremark);
                                }
                                data.paymentremark = retObjectremark.Distinct().ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                    if (flag == "T")
                    {

                        using (var cmd = _ClgFeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Get_Student_Status_Details_Groupwise";
                            cmd.CommandType = CommandType.StoredProcedure;


                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
              SqlDbType.VarChar)
                            {
                                Value = data.MI_ID
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
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
                                    data.fillstudentviewdetails = retObject.Distinct().ToArray();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                    }
                    else if (flag == "G")
                    {
                        using (var cmd = _ClgFeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Get_Student_Status_Details_Groupwise";
                            cmd.CommandType = CommandType.StoredProcedure;


                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                            {
                                Value = data.MI_ID
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@FromDate",
                      SqlDbType.VarChar)
                            {
                                Value = data.fromdate.ToString()

                            });
                            cmd.Parameters.Add(new SqlParameter("@Todate",
                               SqlDbType.VarChar)
                            {
                                Value = data.todate.ToString()

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
                                    data.fillstudentviewdetails = retObject.Distinct().ToArray();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    }
                }
                else
                {

                }








            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CollegePaymentApprovalDTO savedetails(CollegePaymentApprovalDTO data)
        {
            List<CollegePaymentApprovalDTO> Approver = new List<CollegePaymentApprovalDTO>();

            var hrmEempid = _ClgFeeGroupContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_ID && t.Id == data.userid).Distinct().ToList();
            long empid = 0;
            if (hrmEempid.Count > 0)
            {
                empid = hrmEempid.FirstOrDefault().Emp_Code;
                data.HRME_Id = empid;
            }
            try
            {
                var outputval = 0;
                if (data.studentdata != null)
                {
                    for (int i = 0; i < data.studentdata.Length; i++)
                    {
                        outputval = _ClgFeeGroupContext.Database.ExecuteSqlCommand("CLG_FEE_PAYMENT_APPROVAL2021 @p0,@p1,@p2,@p3,@p4", data.studentdata[i].FYP_Id, data.HRME_Id, data.FYPAPP_ApprovedFlg, data.studentdata[i].FYPAPP_Remarks, data.userid);
                    }
                }

                if (outputval > 0)
                {
                    data.returnval = "save";
                }
                else
                {
                    data.returnval = "Notsave";
                }

                if (data.studentdata != null)
                {
                    for (int p = 0; p< data.studentdata.Length; p++)
                    {
                        var finalapprovalcnt = _ClgFeeGroupContext.Fee_Y_PaymentDMO.Where(t => t.MI_Id == data.MI_ID && t.FYP_Id == data.studentdata[p].FYP_Id && t.FYP_ApprovedFlg == true).ToList();
                        if (finalapprovalcnt.Count > 0)
                        {
                            for (int i = 0; i < data.studentdata.Length; i++)
                            {
                                var emailidofstudent = _ClgFeeGroupContext.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_ID && t.AMCST_Id == data.studentdata[i].AMCST_Id).ToList();

                                //if (finalapproval.Count > 0)
                                //{
                                //    Email Email = new Email(_context);
                                //    string m = Email.sendmail(data.MI_ID, emailidofstudent.FirstOrDefault().AMCST_emailId, "FEEAPPROVEDREC", data.studentdata[i].AMCST_Id);
                                //}
                            }

                            for (int i = 0; i < data.studentdata.Length; i++)
                            {
                                List<CollegePaymentApprovalDTO> Tasklist = new List<CollegePaymentApprovalDTO>();


                                var finalapproval = _ClgFeeGroupContext.Fee_Y_PaymentDMO.Where(t => t.MI_Id == data.MI_ID && t.FYP_Id == data.studentdata[i].FYP_Id && t.FYP_ApprovedFlg == true).ToList();

                                var emailidofstudent = _ClgFeeGroupContext.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_ID && t.AMCST_Id == data.studentdata[i].AMCST_Id).ToList();

                                string emailccdetails = "";
                                string emailbccdetails = "";

                                if (emailidofstudent.FirstOrDefault().AMCST_emailId != "" && emailidofstudent.FirstOrDefault().AMCST_emailId != null)
                                {
                                    var checktemplate = _ClgFeeGroupContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_ID && a.ISES_Template_Name == "FEEAPPROVEDREC"
                                    && a.ISES_MailActiveFlag == true).ToList();

                                    if (checktemplate.Count > 0)
                                    {
                                        emailccdetails = checktemplate.FirstOrDefault().ISES_MailCCId;
                                        emailbccdetails = checktemplate.FirstOrDefault().ISES_MailBCCId;
                                    }
                                    else
                                    {
                                        emailccdetails = "";
                                        emailbccdetails = "";
                                    }

                                    if (checktemplate.Count == 0)
                                    {
                                        data.returnval = "Email Template not Mapped to the selected Institution";
                                    }
                                    else
                                    {
                                        using (var cmd = _ClgFeeGroupContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "installment_transaction_details_CLG";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_ID });
                                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                            cmd.Parameters.Add(new SqlParameter("@Amcst_id", SqlDbType.VarChar) { Value = data.studentdata[i].AMCST_Id });
                                            cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.studentdata[i].FYP_Id });
                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();

                                            var retObject = new List<dynamic>();
                                            try
                                            {
                                                using (var dataReader = cmd.ExecuteReader())
                                                {
                                                    while (dataReader.Read())
                                                    {
                                                        //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                        while (dataReader.Read())
                                                        {
                                                            Tasklist.Add(new CollegePaymentApprovalDTO
                                                            {
                                                                FMH_FeeName = Convert.ToString(dataReader["FMH_FeeName"]),
                                                                FTP_Paid_Amt = Convert.ToString(dataReader["ftcP_PaidAmount"]),
                                                            });
                                                        }
                                                        //retObject.Add((ExpandoObject)dataRow);
                                                    }

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.Write(ex.Message);
                                            }
                                        }


                                        string str = "";
                                        string str1 = "";
                                        if (Tasklist.Count > 0)
                                        {
                                            int rownumber = 0;

                                            str += "<table>" +
                                                  "<thead>" +
                                                "<tr>" +
                                                "<th align=" + "center" + " style=" + ">" + "SL No" + "</td> " +
                                                "<th align=" + "left" + ">" + "FeeName" + "</td>" +
                                                "<th align=" + "left" + ">" + "Paid Amount" + "</td> " +
                                                "</tr>" +
                                                 " <thead >" +
                                                  " <tbody >";
                                            for (int j = 0; j < Tasklist.Count; j++)
                                            {
                                                rownumber += 1;


                                                str += "<tr>" +
                                                  "<td align=" + "left" + ">" + rownumber + "</td> " +
                                                 "<td align=" + "left" + ">" + Tasklist[j].FMH_FeeName + "</td>" +
                                                 "<td align=" + "left" + ">" + Tasklist[j].FTP_Paid_Amt + "</td> " +
                                                 "</tr>";
                                            }

                                            data.username = _ClgFeeGroupContext.applicationUser.Where(d => d.Id == data.userid).Select(t => t.NormalizedUserName).FirstOrDefault();

                                            using (var cmdrec = _ClgFeeGroupContext.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmdrec.CommandText = "Fetch_Final_Level_Approval_Name";
                                                cmdrec.CommandType = CommandType.StoredProcedure;
                                                cmdrec.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_ID });
                                                cmdrec.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                                cmdrec.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = data.studentdata[i].AMCST_Id });
                                                cmdrec.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.studentdata[i].FYP_Id });
                                                if (cmdrec.Connection.State != ConnectionState.Open)
                                                    cmdrec.Connection.Open();

                                                //  var retObjectrec = new List<dynamic>();
                                                try
                                                {
                                                    using (var dataReader = cmdrec.ExecuteReader())
                                                    {
                                                        while (dataReader.Read())
                                                        {
                                                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;

                                                            Approver.Add(new CollegePaymentApprovalDTO
                                                            {
                                                                EmpName = Convert.ToString(dataReader["EmpName"]),

                                                            });

                                                            //retObject.Add((ExpandoObject)dataRow);
                                                        }

                                                    }

                                                    //}
                                                    //data.Approvedbyname = retObjectrec.Distinct().ToArray();

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.Write(ex.Message);
                                                }
                                            }

                                            var fillstudentviewdetails = (from a in _ClgFeeGroupContext.Fee_T_College_PaymentDMO
                                                                          from b in _ClgFeeGroupContext.Fee_Y_PaymentDMO
                                                                          from c in _ClgFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                                                          from l in _ClgFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                                          from d in _ClgFeeGroupContext.FeeHeadClgDMO
                                                                          from e in _ClgFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                                                          from f in _ClgFeeGroupContext.Adm_College_Yearly_StudentDMO
                                                                          from g in _ClgFeeGroupContext.MasterCourseDMO
                                                                          from h in _ClgFeeGroupContext.ClgMasterBranchDMO
                                                                          from k in _ClgFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                                                          from n in _ClgFeeGroupContext.Adm_Master_College_StudentDMO
                                                                          from j in _ClgFeeGroupContext.Fee_Y_Payment_College_StudentDMO
                                                                          from m in _ClgFeeGroupContext.Fee_College_Student_StatusDMO
                                                                          where (a.FYP_Id == b.FYP_Id && a.FCMAS_Id == l.FCMAS_Id && c.FCMA_Id == l.FCMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMCST_Id == j.AMCST_Id && f.AMCO_Id == g.AMCO_Id && f.AMB_Id == h.AMB_Id && f.AMSE_Id == k.AMSE_Id && f.ASMAY_Id == b.ASMAY_Id && n.AMCST_Id == j.AMCST_Id && m.AMCST_Id == f.AMCST_Id && f.ASMAY_Id == m.ASMAY_Id && a.FCMAS_Id == m.FCMAS_Id && f.ASMAY_Id == data.ASMAY_Id && n.MI_Id == data.MI_ID && j.AMCST_Id == data.studentdata[i].AMCST_Id && b.FYP_Id == data.studentdata[i].FYP_Id && f.ACYST_ActiveFlag == 1)
                                                                          select new CollegeFeeTransactionDTO
                                                                          {
                                                                              AMCST_Id = f.AMCST_Id,
                                                                              AMCST_FirstName = n.AMCST_FirstName,
                                                                              AMCST_MiddleName = n.AMCST_MiddleName,
                                                                              AMCST_LastName = n.AMCST_LastName,
                                                                              FMH_Id = d.FMH_Id,
                                                                              FMH_FeeName = d.FMH_FeeName,
                                                                              FTI_Name = e.FTI_Name,
                                                                              FTI_Id = e.FTI_Id,
                                                                              FYP_ReceiptNo = b.FYP_ReceiptNo,
                                                                              FTCP_PaidAmount = a.FTCP_PaidAmount,
                                                                              FTCP_ConcessionAmount = a.FTCP_ConcessionAmount,
                                                                              FTCP_FineAmount = a.FTCP_FineAmount,
                                                                              FYP_ReceiptDate = b.FYP_ReceiptDate,
                                                                              AMCO_CourseName = g.AMCO_CourseName,
                                                                              AMB_BranchName = h.AMB_BranchName,
                                                                              AMSE_SEMName = k.AMSE_SEMName,
                                                                              ACYST_RollNo = f.ACYST_RollNo,
                                                                              AMCST_AdmNo = n.AMCST_AdmNo,
                                                                              AMCST_FatherName = n.AMCST_FatherName,
                                                                              AMCST_MotherName = n.AMCST_MotherName,
                                                                              FYP_Remarks = b.FYP_Remarks,
                                                                              AMCST_RegistrationNo = n.AMCST_RegistrationNo,
                                                                              FCSS_TotalCharges = Convert.ToInt64(l.FCMAS_Amount),
                                                                              ASMAY_Year = g.AMCO_CourseName,
                                                                              overalltot = b.FYP_TotalPaidAmount,

                                                                          }
                     ).Distinct().ToList();

                                            var currpaymentdetails = (from a in _ClgFeeGroupContext.Fee_Y_PaymentDMO
                                                                      from b in _ClgFeeGroupContext.Fee_Y_Payment_College_StudentDMO
                                                                      from c in _ClgFeeGroupContext.Fee_Y_Payment_PaymentModeDMO
                                                                      where a.FYP_Id == data.studentdata[i].FYP_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id == a.FYP_Id && a.FYP_Id == c.FYP_Id && b.FYP_Id == c.FYP_Id
                                                                      select new CollegeFeeTransactionDTO
                                                                      {
                                                                          FTCP_PaidAmount = a.FYP_TotalPaidAmount,
                                                                          FTCP_ConcessionAmount = b.FYPCS_TotalConcessionAmount,
                                                                          FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                                          FYP_Remarks = a.FYP_Remarks,

                                                                          FYPPM_BankName = c.FYPPM_BankName,
                                                                          FYPPM_DDChequeDate = c.FYPPM_DDChequeDate,
                                                                          FYPPM_DDChequeNo = c.FYPPM_DDChequeNo,
                                                                          FYP_TransactionTypeFlag = c.FYPPM_TransactionTypeFlag,
                                                                          FYPPM_Transaction_Id = c.FYPPM_Transaction_Id,
                                                                          FYPPM_PaymentReference_Id = c.FYPPM_PaymentReference_Id,
                                                                          FYPPM_ClearanceDate = c.FYPPM_ClearanceDate
                                                                      }
                     ).Distinct().ToList();



                                            str += "</tbody>" +
                                                "</table>";

                                            str1 += "<table>" +
                                                             "<thead>" +
                                               "<tr>" +
                                               "<th align=" + "left" + ">" + "Mode Of Payment" + "</td>" +
                                               "<th align=" + "left" + ">" + "Bank Name" + "</td> " +
                                                 "<th align=" + "left" + ">" + "Cheque No" + "</td> " +
                                                   "<th align=" + "left" + ">" + "Cheque Date" + "</td> " +
                                                     "<th align=" + "left" + ">" + "Clearance Date" + "</td> " +
                                               "</tr>" +
                                                 "</thead>" +
                                                 "<tbody>";
                                            for (int j = 0; j < currpaymentdetails.Count; j++)
                                            {
                                                rownumber += 1;

                                                str1 += "<tr>" +
                                                 "<td align=" + "left" + ">" + currpaymentdetails.FirstOrDefault().FYP_TransactionTypeFlag + "</td>" +
                                                 "<td align=" + "left" + ">" + currpaymentdetails.FirstOrDefault().FYPPM_BankName + "</td> " +
                                                   "<td align=" + "left" + ">" + currpaymentdetails.FirstOrDefault().FYPPM_DDChequeNo + "</td> " +
                                                     "<td align=" + "left" + ">" + currpaymentdetails.FirstOrDefault().FYPPM_DDChequeDate + "</td> " +
                                                       "<td align=" + "left" + ">" + currpaymentdetails.FirstOrDefault().FYPPM_ClearanceDate + "</td> " +
                                                 "</tr>";
                                            }
                                            str1 += "</tbody>" +
                                                "</table>";
                                            string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                                            if (templatedetails.Contains("[AMCST_FirstName]"))
                                            {
                                                templatedetails = templatedetails.Replace("[AMCST_FirstName]", fillstudentviewdetails.FirstOrDefault().AMCST_FirstName.ToUpper());
                                            }
                                            if (templatedetails.Contains("[FYP_ReceiptNo]"))
                                            {
                                                templatedetails = templatedetails.Replace("[FYP_ReceiptNo]", fillstudentviewdetails.FirstOrDefault().FYP_ReceiptNo.ToUpper());
                                            }
                                            if (templatedetails.Contains("[FYP_ReceiptDate]"))
                                            {
                                                templatedetails = templatedetails.Replace("[FYP_ReceiptDate]", Convert.ToString(fillstudentviewdetails.FirstOrDefault().FYP_ReceiptDate));
                                            }
                                            if (templatedetails.Contains("[AMCST_FatherName]"))
                                            {
                                                templatedetails = templatedetails.Replace("[AMCST_FatherName]", fillstudentviewdetails.FirstOrDefault().AMCST_FatherName.ToUpper());
                                            }
                                            if (templatedetails.Contains("[RegistrationNo]"))
                                            {
                                                templatedetails = templatedetails.Replace("[RegistrationNo]", fillstudentviewdetails.FirstOrDefault().AMCST_RegistrationNo.ToUpper());
                                            }
                                            if (templatedetails.Contains("[CourseName]"))
                                            {
                                                templatedetails = templatedetails.Replace("[CourseName]", fillstudentviewdetails.FirstOrDefault().AMCO_CourseName.ToUpper());
                                            }
                                            if (templatedetails.Contains("[BranchName]"))
                                            {
                                                templatedetails = templatedetails.Replace("[BranchName]", fillstudentviewdetails.FirstOrDefault().AMB_BranchName.ToUpper());
                                            }
                                            if (templatedetails.Contains("[asmaY_Year]"))
                                            {
                                                templatedetails = templatedetails.Replace("[asmaY_Year]", fillstudentviewdetails.FirstOrDefault().ASMAY_Year.ToUpper());
                                            }

                                            if (templatedetails.Contains("[FYP_Remarks]"))
                                            {
                                                templatedetails = templatedetails.Replace("[FYP_Remarks]", fillstudentviewdetails.FirstOrDefault().FYP_Remarks.ToUpper());
                                            }
                                            if (templatedetails.Contains("[overalltot]"))
                                            {
                                                templatedetails = templatedetails.Replace("[overalltot]", fillstudentviewdetails.FirstOrDefault().overalltot.ToString());
                                            }
                                            if (templatedetails.Contains("[words]"))
                                            {
                                                templatedetails = templatedetails.Replace("[words]", fillstudentviewdetails.FirstOrDefault().FCSS_TotalCharges.ToString());
                                            }
                                            if (templatedetails.Contains("[preparedby]"))
                                            {
                                                templatedetails = templatedetails.Replace("[preparedby]", data.username.ToUpper());
                                            }
                                            if (templatedetails.Contains("[approvedby]"))
                                            {
                                                templatedetails = templatedetails.Replace("[approvedby]", Approver[0].EmpName.ToUpper());
                                            }

                                            templatedetails = templatedetails.Replace("[MSG]", str);
                                            templatedetails = templatedetails.Replace("[MESSAGE]", str1);
                                            try
                                            {
                                                sendmailexamtt(data.MI_ID, emailidofstudent.FirstOrDefault().AMCST_emailId, templatedetails, emailccdetails, emailbccdetails, "FEEAPPROVEDREC");
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                data.returnval = "admin";
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails, string temp)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _ClgFeeGroupContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == temp
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _ClgFeeGroupContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = url;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _ClgFeeGroupContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
                    {
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
                        message.AddTo(mail_id);
                        // message.AddTo("kavita@vapstech.com");

                        if (Attechement.Equals("1"))
                        {
                            var img = _ClgFeeGroupContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                        //******************* EMAIL CC DETAILS ****************//

                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //******************* EMAIL BCC DETAILS ****************//




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
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else
                    {
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


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;
                            clientsmtp.UseDefaultCredentials = false;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _ClgFeeGroupContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }

                                //******************* EMAIL CC DETAILS ****************//

                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }

                                //******************* EMAIL BCC DETAILS ****************//

                                //if (emailbccdetails != null && emailbccdetails != "")
                                //{
                                //    string[] bccmaildetails = emailbccdetails.Split(',');

                                //    foreach (var c in bccmaildetails)
                                //    {
                                //        emailMessage.Bcc.Add(c.ToString());
                                //    }
                                //}

                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }

                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _ClgFeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _ClgFeeGroupContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "FEEAPPROVEDREC" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _ClgFeeGroupContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _ClgFeeGroupContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";

        }

    }
}
