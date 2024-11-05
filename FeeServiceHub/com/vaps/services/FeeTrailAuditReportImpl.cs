using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeTrailAuditReportImpl : interfaces.FeeTrailAuditReportInterface
    {
        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public FeeTrailAuditReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }
        // public List<FeeTrailAuditDTO>  deleted_receipts;
        //   public List<FeeTrailAuditDTO> deleted_students;
        public static List<long> ITAT_Ids_M_U = new List<long>();
        public FeeTrailAuditDTO getdetails(int id)
        {
            FeeTrailAuditDTO ftar = new FeeTrailAuditDTO();
            // Adm_M_StudentDTO FGRDT = new Adm_M_StudentDTO();
            try
            {
                // List<Adm_M_Student> feegrp = new List<Adm_M_Student>();
                //  feegrp = _FeeGroupContext.Adm_M_Student.ToList();
                //  ftar.admsudentslist = feegrp.ToArray();

                ftar.usersnameslist = (from a in _FeeGroupContext.applicationUser

                                       select new FeeTrailAuditDTO
                                       {
                                           NormalizedUserName = a.NormalizedUserName,
                                           userId = a.Id,
                                       }
              ).ToArray();

                ftar.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                       from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == 2 && b.ASMAY_Id == 10 && a.AMST_SOL == "S")
                                       select new FeeTrailAuditDTO
                                       {
                                           Amst_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,

                                       }
                             ).ToArray();

                //    ftar.newreplist = (from bb in _FeeGroupContext.FeePaymentDetailsDMO
                //                       select new FeeTrailAuditDTO
                //                       {
                //                           receiptNo = bb.FYP_Receipt_No,
                //                             paymentid = bb.FYP_Id,
                //                       }
                //).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ftar;

        }
        public FeeTrailAuditDTO getdata123(FeeTrailAuditDTO data)
        {
            //FeeTrailAuditDTO ftar = new FeeTrailAuditDTO();
            try
            {


                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = allyear.Distinct().ToArray();

                //    data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                           from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                           from c in _FeeGroupContext.FeePaymentDetailsDMO
                //                           from d in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                //                           where (a.AMST_Id == b.AMST_Id && a.MI_Id ==data.MI_ID && c.MI_Id==data.MI_ID && c.FYP_Id==d.FYP_Id && d.AMST_Id==a.AMST_Id) 
                //                           select new FeeTrailAuditDTO
                //                           {
                //                               Amst_Id = a.AMST_Id,
                //                               AMST_FirstName = a.AMST_FirstName,
                //                               AMST_MiddleName = a.AMST_MiddleName,
                //                               AMST_LastName = a.AMST_LastName,
                //                               Name = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                //                           }
                //                 ).Distinct().ToArray();

                //    data.newreplist = (from bb in _FeeGroupContext.FeePaymentDetailsDMO
                //                       where ( bb.MI_Id == data.MI_ID)
                //                       select new FeeTrailAuditDTO
                //                       {
                //                           receiptNo = bb.FYP_Receipt_No,
                //                           paymentid = bb.FYP_Id,
                //                       }
                //).Distinct().ToArray();


                //var mi_id_list = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                //                  from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                //                  where (a.ITAT_Id == b.ITAT_Id && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "MI_Id" && (Convert.ToInt64(b.IATD_CurrentValue) == data.MI_ID || Convert.ToInt64(b.IATD_PreviousValue) == data.MI_ID))
                //                  select a.ITAT_Id).Distinct().ToList();

                //var user_id_list = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                //                    from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                //                    where (a.ITAT_Id == b.ITAT_Id && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "user_id")
                //                    select a.ITAT_Id).Distinct().ToList();
                //ITAT_Ids_M_U = (from a in mi_id_list
                //                from b in user_id_list
                //                where (a == b)
                //                select a).Distinct().ToList();



                //  var  deleted_receipts = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                //                        from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                //                        where (a.ITAT_Id == b.ITAT_Id && a.ITAT_TableName == "Fee_Y_Payment" && mi_id_list.Contains(a.ITAT_Id) && user_id_list.Contains(a.ITAT_Id) && a.ITAT_Operation == "D" && b.IATD_ColumnName == "FYP_Receipt_No")
                //                        select new FeeTrailAuditDTO
                //                        {
                //                            receiptNo = b.IATD_PreviousValue,
                //                            paymentid = Convert.ToInt64(a.ITAT_RecordPKID)
                //                        }).Distinct().ToList();
                //    List<string> deleted_fyp_ids = new List<string>();
                //    foreach(var r in deleted_receipts)
                //    {
                //        deleted_fyp_ids.Add(r.paymentid.ToString());
                //    }

                //    var itat_ids = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                //                       from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                //                       where (a.ITAT_Id == b.ITAT_Id && a.ITAT_TableName == "Fee_Y_Payment_School_Student" && deleted_fyp_ids.Contains(b.IATD_PreviousValue) && a.ITAT_Operation == "D" && b.IATD_ColumnName == "FYP_Id")
                //                       select a.ITAT_Id).Distinct().ToList();

                //  var   deleted_students = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                //                        from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                //                        from c in _FeeGroupContext.AdmissionStudentDMO
                //                        where (a.ITAT_Id == b.ITAT_Id && a.ITAT_TableName == "Fee_Y_Payment_School_Student" && itat_ids.Contains(a.ITAT_Id)  && b.IATD_ColumnName == "AMST_Id" && c.AMST_Id== Convert.ToInt64(b.IATD_PreviousValue))
                //                        select new FeeTrailAuditDTO
                //                        {
                //                            Amst_Id = c.AMST_Id,
                //                            AMST_FirstName = c.AMST_FirstName,
                //                            AMST_MiddleName = c.AMST_MiddleName,
                //                            AMST_LastName = c.AMST_LastName,
                //                            Name = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                //                        }).Distinct().ToList();
                //    data.D_Receipts = deleted_receipts.Select(t=>t.receiptNo).Distinct().ToArray();
                //    data.D_Students = deleted_students.ToArray();




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //public async Task<FeeTrailAuditDTO> getreport(FeeTrailAuditDTO data)
        //{

        //  string   IVRM_CLM_coloumn = "";

        //    try
        //    {
        //        if (data.saveflg == "1")
        //        {
        //            IVRM_CLM_coloumn = "S";
        //        }
        //        if (data.updateflg == "1")
        //        {if(IVRM_CLM_coloumn !="")
        //            {
        //                IVRM_CLM_coloumn = IVRM_CLM_coloumn + "," + "U";
        //            }
        //            else
        //            {
        //                IVRM_CLM_coloumn = "U";
        //            }

        //        }
        //        if (data.deleteflg == "1")
        //        {
        //            if (IVRM_CLM_coloumn != "")
        //            {
        //                IVRM_CLM_coloumn = IVRM_CLM_coloumn + "," + "D";
        //            }
        //            else
        //            {
        //                IVRM_CLM_coloumn = "D";
        //            }
        //        }
        //        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "fee_trail_audit_report";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@receiptno",
        //                     SqlDbType.TinyInt)
        //            {
        //                Value = data.paymentid
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@amst",
        //                        SqlDbType.TinyInt)
        //            {
        //                Value = data.Amst_Id
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@mi_id",
        //                SqlDbType.TinyInt)
        //            {
        //                Value = data.MI_ID
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@asmyid",
        //               SqlDbType.TinyInt)
        //            {
        //                Value = data.asmay_id
        //            });

        //            cmd.Parameters.Add(new SqlParameter("@fromdate",
        //               SqlDbType.Date)
        //            {
        //                Value = data.fromdate
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@todate",
        //                           SqlDbType.Date)
        //            {
        //                Value = data.todate
        //            });

        //            cmd.Parameters.Add(new SqlParameter("@userid",
        //                         SqlDbType.TinyInt)
        //            {
        //                Value = data.userId
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@transflag",
        //                       SqlDbType.VarChar)
        //            {
        //                Value = IVRM_CLM_coloumn
        //            });

        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();

        //            var retObject = new List<dynamic>();
        //            //var data = cmd.ExecuteNonQuery();
        //            try
        //            {
        //                using (var dataReader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await dataReader.ReadAsync())
        //                    {
        //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                        {
        //                            dataRow.Add(
        //                                dataReader.GetName(iFiled),
        //                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                            );
        //                        }

        //                        retObject.Add((ExpandoObject)dataRow);
        //                    }
        //                }
        //                data.reportdatelist = retObject.ToArray();

        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }

        //        return data;

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        public FeeTrailAuditDTO getreportold(FeeTrailAuditDTO data)
        {
            if (data.Status_IU_D == "IU")
            {
                data.paymentid = Convert.ToInt64(data.receiptNo);
                List<string> Status_Flags = new List<string>();
                try
                {
                    if (data.Save_flag == false && data.Update_flag == false)//&& data.Delete_flag == false
                    {
                        Status_Flags.Add("I");
                        Status_Flags.Add("U");
                        // Status_Flags.Add("D");
                    }
                    else
                    {
                        Status_Flags = new List<string>();
                        if (data.Save_flag)
                        {
                            Status_Flags.Add("I");
                        }
                        if (data.Update_flag)
                        {
                            Status_Flags.Add("U");
                        }
                        //if (data.Delete_flag)
                        //{
                        //    Status_Flags.Add("D");
                        //}
                    }
                    if (data.Report_Type == "receipt")
                    {
                        var oth_stu_id_cnt = _FeeGroupContext.Fee_Y_Payment_OthStuDMO.Where(t => t.FYP_Id == data.paymentid).Count();
                        var staff_id_cnt = _FeeGroupContext.Fee_Y_Payment_StaffDMO.Where(t => t.FYP_Id == data.paymentid).Count();
                        var stu_id_cnt = _FeeGroupContext.Fee_Y_Payment_School_StudentDMO.Where(t => t.FYP_Id == data.paymentid).Count();
                        var R_pre_stu_id_cnt = _FeeGroupContext.Fee_Y_Payment_PA_RegistrationDMO.Where(t => t.FYP_Id == data.paymentid).Count();
                        var A_pre_stu_id_cnt = _FeeGroupContext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYP_Id == data.paymentid).Count();

                        if (stu_id_cnt > 0)
                        {
                            var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.applicationUser
                                                from c in _FeeGroupContext.AdmissionStudentDMO
                                                from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                    //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                from f in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.AMST_Id == c.AMST_Id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                                //&& a.user_id == data.useridpassing kiran
                                                select new FeeTrailAuditDTO
                                                {
                                                    paymentid = a.FYP_Id,
                                                    Amst_Id = c.AMST_Id,
                                                    // useridpassing = Convert.ToInt64(b.Id),
                                                    ITAT_Id = d.ITAT_Id,
                                                    User_Name = b.NormalizedUserName,
                                                    Name = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    ITAT_Date = d.ITAT_Date,
                                                    ITAT_Time = d.ITAT_Time,
                                                    FYP_Remarks = a.FYP_Remarks,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                    FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                    FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                    ITAT_Operation = d.ITAT_Operation,
                                                    ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                    ITAT_MAACAddress = d.ITAT_MAACAddress
                                                }
                                     ).Distinct().ToList();

                            List<long> ITAT_Ids = new List<long>();
                            foreach (var x in Main_Details)
                            {
                                ITAT_Ids.Add(x.ITAT_Id);
                            }
                            data.Main_Details = Main_Details.ToArray();
                            data.reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToArray();

                            //data.reportdatelist = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                            //                       from b in _FeeGroupContext.applicationUser
                            //                       from c in _FeeGroupContext.AdmissionStudentDMO
                            //                       from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                            //                       from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                            //                       from f in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                            //                       where (c.MI_Id==a.MI_Id && a.MI_Id==data.MI_ID && b.Id == a.user_id && a.user_id==data.useridpassing   && a.FYP_Id==data.paymentid && d.ITAT_TableName.Contains("Fee_Y_Payment") && d.ITAT_RecordPKID==a.FYP_Id.ToString() && e.ITAT_Id==d.ITAT_Id && f.FYP_Id==a.FYP_Id && f.AMST_Id==c.AMST_Id && Status_Flags.Contains(d.ITAT_Operation))
                            //                       select e).ToArray();
                        }
                        else if (staff_id_cnt > 0)
                        {
                            var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.applicationUser
                                                from c in _FeeGroupContext.MasterEmployee
                                                from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                    //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                from f in _FeeGroupContext.Fee_Y_Payment_StaffDMO
                                                where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.HRME_Id == c.HRME_Id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                                //&& a.user_id == data.useridpassing kiran
                                                select new FeeTrailAuditDTO
                                                {
                                                    paymentid = a.FYP_Id,
                                                    HRME_Id = c.HRME_Id,
                                                    // useridpassing = Convert.ToInt64(b.Id),
                                                    ITAT_Id = d.ITAT_Id,
                                                    User_Name = b.NormalizedUserName,
                                                    Name = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    ITAT_Date = d.ITAT_Date,
                                                    ITAT_Time = d.ITAT_Time,
                                                    FYP_Remarks = a.FYP_Remarks,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                    FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                    FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                    ITAT_Operation = d.ITAT_Operation,
                                                    ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                    ITAT_MAACAddress = d.ITAT_MAACAddress
                                                }
                                     ).Distinct().ToList();

                            List<long> ITAT_Ids = new List<long>();
                            foreach (var x in Main_Details)
                            {
                                ITAT_Ids.Add(x.ITAT_Id);
                            }
                            data.Main_Details = Main_Details.ToArray();
                            data.reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToArray();
                        }
                        else if (oth_stu_id_cnt > 0)
                        {
                            var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.applicationUser
                                                from c in _FeeGroupContext.FeeMasterOtherStudentDMO
                                                from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                    //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                from f in _FeeGroupContext.Fee_Y_Payment_OthStuDMO
                                                where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.FMOST_Id == c.FMOST_Id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                                //&& a.user_id == data.useridpassing kiran
                                                select new FeeTrailAuditDTO
                                                {
                                                    paymentid = a.FYP_Id,
                                                    FMOST_Id = c.FMOST_Id,
                                                    // useridpassing = Convert.ToInt64(b.Id),
                                                    ITAT_Id = d.ITAT_Id,
                                                    User_Name = b.NormalizedUserName,
                                                    Name = c.FMOST_StudentName,
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    ITAT_Date = d.ITAT_Date,
                                                    ITAT_Time = d.ITAT_Time,
                                                    FYP_Remarks = a.FYP_Remarks,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                    FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                    FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                    ITAT_Operation = d.ITAT_Operation,
                                                    ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                    ITAT_MAACAddress = d.ITAT_MAACAddress
                                                }
                                     ).Distinct().ToList();


                            List<long> ITAT_Ids = new List<long>();
                            foreach (var x in Main_Details)
                            {
                                ITAT_Ids.Add(x.ITAT_Id);
                            }
                            data.Main_Details = Main_Details.ToArray();
                            data.reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToArray();
                        }
                        else if (R_pre_stu_id_cnt > 0)
                        {
                            var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.applicationUser
                                                from c in _FeeGroupContext.stuapp
                                                from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                    //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                from f in _FeeGroupContext.Fee_Y_Payment_PA_RegistrationDMO
                                                where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.PASR_Id == c.pasr_id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                                //&& a.user_id == data.useridpassing kiran
                                                select new FeeTrailAuditDTO
                                                {
                                                    paymentid = a.FYP_Id,
                                                    pasr_id = c.pasr_id,
                                                    // useridpassing = Convert.ToInt64(b.Id),
                                                    ITAT_Id = d.ITAT_Id,
                                                    User_Name = b.NormalizedUserName,
                                                    Name = ((c.PASR_FirstName == null ? " " : c.PASR_FirstName) + " " + (c.PASR_MiddleName == null ? " " : c.PASR_MiddleName) + " " + (c.PASR_LastName == null ? " " : c.PASR_LastName)).Trim(),
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    ITAT_Date = d.ITAT_Date,
                                                    ITAT_Time = d.ITAT_Time,
                                                    FYP_Remarks = a.FYP_Remarks,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                    FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                    FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                    ITAT_Operation = d.ITAT_Operation,
                                                    ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                    ITAT_MAACAddress = d.ITAT_MAACAddress
                                                }
                                     ).Distinct().ToList();


                            List<long> ITAT_Ids = new List<long>();
                            foreach (var x in Main_Details)
                            {
                                ITAT_Ids.Add(x.ITAT_Id);
                            }
                            data.Main_Details = Main_Details.ToArray();
                            data.reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToArray();
                        }
                        else if (A_pre_stu_id_cnt > 0)
                        {
                            var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                from b in _FeeGroupContext.applicationUser
                                                from c in _FeeGroupContext.stuapp
                                                from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                    //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                from f in _FeeGroupContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.PASA_Id == c.pasr_id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                                //&& a.user_id == data.useridpassing kiran
                                                select new FeeTrailAuditDTO
                                                {
                                                    paymentid = a.FYP_Id,
                                                    pasr_id = c.pasr_id,
                                                    // useridpassing = Convert.ToInt64(b.Id),
                                                    ITAT_Id = d.ITAT_Id,
                                                    User_Name = b.NormalizedUserName,
                                                    Name = ((c.PASR_FirstName == null ? " " : c.PASR_FirstName) + " " + (c.PASR_MiddleName == null ? " " : c.PASR_MiddleName) + " " + (c.PASR_LastName == null ? " " : c.PASR_LastName)).Trim(),
                                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                    ITAT_Date = d.ITAT_Date,
                                                    ITAT_Time = d.ITAT_Time,
                                                    FYP_Remarks = a.FYP_Remarks,
                                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                    FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                    FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                    FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                    ITAT_Operation = d.ITAT_Operation,
                                                    ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                    ITAT_MAACAddress = d.ITAT_MAACAddress
                                                }
                                     ).Distinct().ToList();


                            List<long> ITAT_Ids = new List<long>();
                            foreach (var x in Main_Details)
                            {
                                ITAT_Ids.Add(x.ITAT_Id);
                            }
                            data.Main_Details = Main_Details.ToArray();
                            data.reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToArray();
                        }
                    }
                    else if (data.Report_Type == "studentttt")
                    {
                        var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                            from b in _FeeGroupContext.applicationUser
                                            from c in _FeeGroupContext.AdmissionStudentDMO
                                            from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                            from f in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                            where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == Convert.ToInt64(d.ITAT_RecordPKID) && d.ITAT_TableName == "Fee_Y_Payment" && f.AMST_Id == data.Amst_Id && f.FYP_Id == a.FYP_Id && f.AMST_Id == c.AMST_Id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                            //&& a.user_id == data.useridpassing kiran
                                            select new FeeTrailAuditDTO
                                            {
                                                paymentid = a.FYP_Id,
                                                Amst_Id = c.AMST_Id,
                                                // useridpassing = Convert.ToInt64(b.Id),
                                                ITAT_Id = d.ITAT_Id,
                                                User_Name = b.NormalizedUserName,
                                                Name = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                                // FYP_Receipt_No = a.FYP_Receipt_No,
                                                FYP_Receipt_No = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == d.ITAT_Id && t.IATD_ColumnName == "FYP_Receipt_No").FirstOrDefault().IATD_CurrentValue,
                                                FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                ITAT_Date = d.ITAT_Date,
                                                ITAT_Time = d.ITAT_Time,
                                                FYP_Remarks = a.FYP_Remarks,
                                                FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                ITAT_Operation = d.ITAT_Operation,
                                                ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                ITAT_MAACAddress = d.ITAT_MAACAddress
                                            }
                                    ).Distinct().ToArray();

                        List<long> ITAT_Ids = new List<long>();
                        foreach (var x in Main_Details)
                        {
                            ITAT_Ids.Add(x.ITAT_Id);
                        }
                        data.Main_Details = Main_Details.ToArray();
                        data.reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToArray();
                    }
                    else if (data.Report_Type == "date")
                    {
                        var FYP_ITAT_Ids = _FeeGroupContext.IVRM_Table_AuditTrailDMO.Where(t => t.ITAT_TableName == "Fee_Y_Payment" && (t.ITAT_Date >= data.fromdate && t.ITAT_Date <= data.todate) || (t.ITAT_Date <= data.fromdate && t.ITAT_Date >= data.todate)).Select(t => t.ITAT_RecordPKID).Distinct().ToList();
                        if (FYP_ITAT_Ids.Count() > 0)
                        {
                            List<FeeTrailAuditDTO> S_786 = new List<FeeTrailAuditDTO>();
                            List<IVRM_AuditTrail_DeatilsDMO> SS_786 = new List<IVRM_AuditTrail_DeatilsDMO>();
                            foreach (var FYP_Id in FYP_ITAT_Ids)
                            {
                                //var flag = _FeeGroupContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == data.MI_ID && t.user_id == data.useridpassing && t.FYP_Id == Convert.ToInt64(FYP_Id)).Count();

                                var flag = _FeeGroupContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == data.MI_ID && t.FYP_Id == Convert.ToInt64(FYP_Id)).Count();

                                if (flag > 0)
                                {
                                    data.paymentid = Convert.ToInt64(FYP_Id);
                                    var oth_stu_id_cnt = _FeeGroupContext.Fee_Y_Payment_OthStuDMO.Where(t => t.FYP_Id == data.paymentid).Count();
                                    var staff_id_cnt = _FeeGroupContext.Fee_Y_Payment_StaffDMO.Where(t => t.FYP_Id == data.paymentid).Count();
                                    var stu_id_cnt = _FeeGroupContext.Fee_Y_Payment_School_StudentDMO.Where(t => t.FYP_Id == data.paymentid).Count();
                                    var R_pre_stu_id_cnt = _FeeGroupContext.Fee_Y_Payment_PA_RegistrationDMO.Where(t => t.FYP_Id == data.paymentid).Count();
                                    var A_pre_stu_id_cnt = _FeeGroupContext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYP_Id == data.paymentid).Count();

                                    if (stu_id_cnt > 0)
                                    {
                                        //var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                        //                    from b in _FeeGroupContext.applicationUser
                                        //                    from c in _FeeGroupContext.AdmissionStudentDMO
                                        //                    from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                        //                        //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                        //                    from f in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                        //                    where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.user_id == data.useridpassing && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.AMST_Id == c.AMST_Id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                        //                    select new FeeTrailAuditDTO
                                        //                    {
                                        //                        paymentid = a.FYP_Id,
                                        //                        Amst_Id = c.AMST_Id,
                                        //                        useridpassing = Convert.ToInt64(b.Id),
                                        //                        ITAT_Id = d.ITAT_Id,
                                        //                        User_Name = b.NormalizedUserName,
                                        //                        Name = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                        //                        // FYP_Receipt_No = a.FYP_Receipt_No,
                                        //                        FYP_Receipt_No = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == d.ITAT_Id && t.IATD_ColumnName == "FYP_Receipt_No").FirstOrDefault().IATD_CurrentValue,
                                        //                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                        //                        ITAT_Date = d.ITAT_Date,
                                        //                        ITAT_Time = d.ITAT_Time,
                                        //                        FYP_Remarks = a.FYP_Remarks,
                                        //                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                        //                        FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                        //                        FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                        //                        FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                        //                        ITAT_Operation = d.ITAT_Operation,
                                        //                        ITAT_IPV4Address = d.ITAT_IPV4Address,
                                        //                        ITAT_MAACAddress = d.ITAT_MAACAddress
                                        //                    }
                                        //         ).Distinct().ToList();


                                        var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                            from b in _FeeGroupContext.applicationUser
                                                            from c in _FeeGroupContext.AdmissionStudentDMO
                                                            from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                                //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                            from f in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                                            where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.AMST_Id == c.AMST_Id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                                            select new FeeTrailAuditDTO
                                                            {
                                                                paymentid = a.FYP_Id,
                                                                Amst_Id = c.AMST_Id,
                                                                // useridpassing = Convert.ToInt64(b.Id),
                                                                ITAT_Id = d.ITAT_Id,
                                                                User_Name = b.NormalizedUserName,
                                                                Name = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                                                // FYP_Receipt_No = a.FYP_Receipt_No,
                                                                FYP_Receipt_No = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == d.ITAT_Id && t.IATD_ColumnName == "FYP_Receipt_No").FirstOrDefault().IATD_CurrentValue,
                                                                FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                                ITAT_Date = d.ITAT_Date,
                                                                ITAT_Time = d.ITAT_Time,
                                                                FYP_Remarks = a.FYP_Remarks,
                                                                FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                                FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                                FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                                FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                                ITAT_Operation = d.ITAT_Operation,
                                                                ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                                ITAT_MAACAddress = d.ITAT_MAACAddress
                                                            }
                                                 ).Distinct().ToList();

                                        List<long> ITAT_Ids = new List<long>();
                                        foreach (var x in Main_Details)
                                        {
                                            ITAT_Ids.Add(x.ITAT_Id);
                                            S_786.Add(x);
                                        }
                                        // data.Main_Details = Main_Details.ToArray();
                                        var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToList();
                                        foreach (var x in reportdatelist)
                                        {
                                            SS_786.Add(x);
                                        }
                                    }
                                    else if (staff_id_cnt > 0)
                                    {
                                        var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                            from b in _FeeGroupContext.applicationUser
                                                            from c in _FeeGroupContext.MasterEmployee
                                                            from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                                //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                            from f in _FeeGroupContext.Fee_Y_Payment_StaffDMO
                                                            where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.HRME_Id == c.HRME_Id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id
                                                            //&& a.user_id == data.useridpassing kiran
                                                            select new FeeTrailAuditDTO
                                                            {
                                                                paymentid = a.FYP_Id,
                                                                HRME_Id = c.HRME_Id,
                                                                // useridpassing = Convert.ToInt64(b.Id),
                                                                ITAT_Id = d.ITAT_Id,
                                                                User_Name = b.NormalizedUserName,
                                                                Name = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                                                // FYP_Receipt_No = a.FYP_Receipt_No,
                                                                FYP_Receipt_No = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == d.ITAT_Id && t.IATD_ColumnName == "FYP_Receipt_No").FirstOrDefault().IATD_CurrentValue,
                                                                FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                                ITAT_Date = d.ITAT_Date,
                                                                ITAT_Time = d.ITAT_Time,
                                                                FYP_Remarks = a.FYP_Remarks,
                                                                FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                                FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                                FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                                FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                                ITAT_Operation = d.ITAT_Operation,
                                                                ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                                ITAT_MAACAddress = d.ITAT_MAACAddress
                                                            }
                                                 ).Distinct().ToList();

                                        List<long> ITAT_Ids = new List<long>();
                                        foreach (var x in Main_Details)
                                        {
                                            ITAT_Ids.Add(x.ITAT_Id);
                                            S_786.Add(x);
                                        }
                                        // data.Main_Details = Main_Details.ToArray();
                                        var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToList();
                                        foreach (var x in reportdatelist)
                                        {
                                            SS_786.Add(x);
                                        }
                                    }
                                    else if (oth_stu_id_cnt > 0)
                                    {
                                        var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                            from b in _FeeGroupContext.applicationUser
                                                            from c in _FeeGroupContext.FeeMasterOtherStudentDMO
                                                            from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                                //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                            from f in _FeeGroupContext.Fee_Y_Payment_OthStuDMO
                                                            where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.FMOST_Id == c.FMOST_Id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id 
                                                            //&& a.user_id == data.useridpassing kiran
                                                            select new FeeTrailAuditDTO
                                                            {
                                                                paymentid = a.FYP_Id,
                                                                FMOST_Id = c.FMOST_Id,
                                                                // useridpassing = Convert.ToInt64(b.Id),
                                                                ITAT_Id = d.ITAT_Id,
                                                                User_Name = b.NormalizedUserName,
                                                                Name = c.FMOST_StudentName,
                                                                //  FYP_Receipt_No = a.FYP_Receipt_No,
                                                                FYP_Receipt_No = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == d.ITAT_Id && t.IATD_ColumnName == "FYP_Receipt_No").FirstOrDefault().IATD_CurrentValue,
                                                                FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                                ITAT_Date = d.ITAT_Date,
                                                                ITAT_Time = d.ITAT_Time,
                                                                FYP_Remarks = a.FYP_Remarks,
                                                                FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                                FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                                FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                                FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                                ITAT_Operation = d.ITAT_Operation,
                                                                ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                                ITAT_MAACAddress = d.ITAT_MAACAddress
                                                            }
                                                 ).Distinct().ToList();


                                        List<long> ITAT_Ids = new List<long>();
                                        foreach (var x in Main_Details)
                                        {
                                            ITAT_Ids.Add(x.ITAT_Id);
                                            S_786.Add(x);
                                        }
                                        //  data.Main_Details = Main_Details.ToArray();
                                        var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToList();
                                        foreach (var x in reportdatelist)
                                        {
                                            SS_786.Add(x);
                                        }
                                    }
                                    else if (R_pre_stu_id_cnt > 0)
                                    {
                                        var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                            from b in _FeeGroupContext.applicationUser
                                                            from c in _FeeGroupContext.stuapp
                                                            from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                                //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                            from f in _FeeGroupContext.Fee_Y_Payment_PA_RegistrationDMO
                                                            where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.PASR_Id == c.pasr_id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id 
                                                                                                                                                                                                                                                                                                                                                                                //&& a.user_id == data.useridpassing kiran
                                                            select new FeeTrailAuditDTO
                                                            {
                                                                paymentid = a.FYP_Id,
                                                                pasr_id = c.pasr_id,
                                                                // useridpassing = Convert.ToInt64(b.Id),
                                                                ITAT_Id = d.ITAT_Id,
                                                                User_Name = b.NormalizedUserName,
                                                                Name = ((c.PASR_FirstName == null ? " " : c.PASR_FirstName) + " " + (c.PASR_MiddleName == null ? " " : c.PASR_MiddleName) + " " + (c.PASR_LastName == null ? " " : c.PASR_LastName)).Trim(),
                                                                // FYP_Receipt_No = a.FYP_Receipt_No,
                                                                FYP_Receipt_No = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == d.ITAT_Id && t.IATD_ColumnName == "FYP_Receipt_No").FirstOrDefault().IATD_CurrentValue,
                                                                FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                                ITAT_Date = d.ITAT_Date,
                                                                ITAT_Time = d.ITAT_Time,
                                                                FYP_Remarks = a.FYP_Remarks,
                                                                FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                                FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                                FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                                FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                                ITAT_Operation = d.ITAT_Operation,
                                                                ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                                ITAT_MAACAddress = d.ITAT_MAACAddress
                                                            }
                                                 ).Distinct().ToList();


                                        List<long> ITAT_Ids = new List<long>();
                                        foreach (var x in Main_Details)
                                        {
                                            ITAT_Ids.Add(x.ITAT_Id);
                                        }
                                        data.Main_Details = Main_Details.ToArray();
                                        data.reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToArray();
                                    }
                                    else if (A_pre_stu_id_cnt > 0)
                                    {
                                        var Main_Details = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                                            from b in _FeeGroupContext.applicationUser
                                                            from c in _FeeGroupContext.stuapp
                                                            from d in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                                //  from e in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                            from f in _FeeGroupContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                            where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_ID && Convert.ToInt64(b.Id) == a.user_id && a.FYP_Id == data.paymentid && d.ITAT_TableName == "Fee_Y_Payment" && Convert.ToInt64(d.ITAT_RecordPKID) == data.paymentid && f.FYP_Id == a.FYP_Id && f.PASA_Id == c.pasr_id && Status_Flags.Contains(d.ITAT_Operation)) //&& e.ITAT_Id == d.ITAT_Id 
                                                            //&& a.user_id == data.useridpassing kiran
                                                            select new FeeTrailAuditDTO
                                                            {
                                                                paymentid = a.FYP_Id,
                                                                pasr_id = c.pasr_id,
                                                                //  useridpassing = Convert.ToInt64(b.Id),
                                                                ITAT_Id = d.ITAT_Id,
                                                                User_Name = b.NormalizedUserName,
                                                                Name = ((c.PASR_FirstName == null ? " " : c.PASR_FirstName) + " " + (c.PASR_MiddleName == null ? " " : c.PASR_MiddleName) + " " + (c.PASR_LastName == null ? " " : c.PASR_LastName)).Trim(),
                                                                // FYP_Receipt_No = a.FYP_Receipt_No,
                                                                FYP_Receipt_No = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == d.ITAT_Id && t.IATD_ColumnName == "FYP_Receipt_No").FirstOrDefault().IATD_CurrentValue,
                                                                FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                                ITAT_Date = d.ITAT_Date,
                                                                ITAT_Time = d.ITAT_Time,
                                                                FYP_Remarks = a.FYP_Remarks,
                                                                FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                                FYP_Tot_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                                                FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                                                FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                                                ITAT_Operation = d.ITAT_Operation,
                                                                ITAT_IPV4Address = d.ITAT_IPV4Address,
                                                                ITAT_MAACAddress = d.ITAT_MAACAddress
                                                            }
                                                 ).Distinct().ToList();


                                        List<long> ITAT_Ids = new List<long>();
                                        foreach (var x in Main_Details)
                                        {
                                            ITAT_Ids.Add(x.ITAT_Id);
                                        }
                                        data.Main_Details = Main_Details.ToArray();
                                        data.reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => ITAT_Ids.Contains(t.ITAT_Id)).Distinct().ToArray();
                                    }
                                }
                            }
                            data.Main_Details = S_786.Distinct().ToArray();
                            data.reportdatelist = SS_786.Distinct().ToArray();
                        }


                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            else if (data.Status_IU_D == "D")
            {
                //List<string> Status_Flags = new List<string>();
                try
                {
                    //if (data.Delete_flag)
                    //{
                    //    Status_Flags.Add("D");
                    //}

                    if (data.Report_Type == "receipt" || data.Report_Type == "date")
                    {
                        //var fyp_ids_itat_ids = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                        //                        from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                        //                        where (b.IATD_ColumnName == "FYP_Receipt_No" && b.IATD_PreviousValue == data.receiptNo && a.ITAT_Id == b.ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment")
                        //                        select new FeeTrailAuditDTO
                        //                        {
                        //                            paymentid = Convert.ToInt64(a.ITAT_RecordPKID),
                        //                            ITAT_Id = a.ITAT_Id
                        //                        }
                        //              ).Distinct().ToList();

                        //List<long> itat_ids = new List<long>();
                        //itat_ids = fyp_ids_itat_ids.Select(t => t.ITAT_Id).Distinct().ToList();
                        //List<string> fyp_ids = new List<string>();
                        //fyp_ids = fyp_ids_itat_ids.Select(t => t.paymentid.ToString()).Distinct().ToList();
                        var itat_ids = new List<long>();
                        if (data.Report_Type == "receipt")
                        {
                            itat_ids = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                        from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                        where (b.IATD_ColumnName == "FYP_Receipt_No" && b.IATD_PreviousValue == data.receiptNo && a.ITAT_Id == b.ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment")
                                        select a.ITAT_Id).Distinct().ToList();
                        }
                        else if (data.Report_Type == "date")
                        {
                            itat_ids = _FeeGroupContext.IVRM_Table_AuditTrailDMO.Where(t => t.ITAT_TableName == "Fee_Y_Payment" && t.ITAT_Operation == "D" && (t.ITAT_Date >= data.fromdate && t.ITAT_Date <= data.todate) || (t.ITAT_Date <= data.fromdate && t.ITAT_Date >= data.todate)).Select(t => t.ITAT_Id).Distinct().ToList();
                        }


                        var final_itat_ids = (from a in itat_ids
                                              from b in ITAT_Ids_M_U
                                              where (a == b)
                                              select a).Distinct().ToList();
                        if (final_itat_ids.Count() > 0)
                        {
                            List<FeeTrailAuditDTO> S_786 = new List<FeeTrailAuditDTO>();
                            List<IVRM_AuditTrail_DeatilsDMO> SS_786 = new List<IVRM_AuditTrail_DeatilsDMO>();
                            foreach (var ITAT_Id in final_itat_ids)
                            {
                                var FYP_Id = _FeeGroupContext.IVRM_Table_AuditTrailDMO.Single(t => t.ITAT_Id == ITAT_Id).ITAT_RecordPKID;
                                // var FYP_Id = fyp_ids_itat_ids.SingleOrDefault(t => t.ITAT_Id == ITAT_Id).paymentid.ToString();
                                var oth_stu_id_cnt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                      from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                      where (b.IATD_ColumnName == "FYP_Id" && b.IATD_PreviousValue == FYP_Id && a.ITAT_Id == b.ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment_OthStu")
                                                      select a.ITAT_Id).Distinct().ToList();

                                var staff_id_cnt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                    from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                    where (b.IATD_ColumnName == "FYP_Id" && b.IATD_PreviousValue == FYP_Id && a.ITAT_Id == b.ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment_Staff")
                                                    select a.ITAT_Id).Distinct().ToList();

                                var stu_id_cnt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                  from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                  where (b.IATD_ColumnName == "FYP_Id" && b.IATD_PreviousValue == FYP_Id && a.ITAT_Id == b.ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment_School_Student")
                                                  select a.ITAT_Id).Distinct().ToList();
                                var R_pre_stu_id_cnt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                        from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                        where (b.IATD_ColumnName == "FYP_Id" && b.IATD_PreviousValue == FYP_Id && a.ITAT_Id == b.ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment_PA_Registration")
                                                        select a.ITAT_Id).Distinct().ToList();
                                var A_pre_stu_id_cnt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                        from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                        where (b.IATD_ColumnName == "FYP_Id" && b.IATD_PreviousValue == FYP_Id && a.ITAT_Id == b.ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment_PA_Application")
                                                        select a.ITAT_Id).Distinct().ToList();
                                var paymentid = Convert.ToInt64(FYP_Id);


                                var user_id = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                               from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                               where (a.ITAT_Id == b.ITAT_Id && b.ITAT_Id == ITAT_Id && b.IATD_ColumnName == "user_id")
                                               select b).Distinct().ToList();

                                //var useridpassing = data.useridpassing;
                                //var User_Name = _FeeGroupContext.applicationUser.Single(t => t.Id == data.useridpassing).NormalizedUserName;


                                var User_Name = _FeeGroupContext.applicationUser.Single(t => t.Id == Convert.ToInt64(user_id.FirstOrDefault().IATD_PreviousValue)).NormalizedUserName;

                                var FYP_Receipt_No = "";
                                if (data.Report_Type == "date")
                                {
                                    var FYP_Receipt_No_l = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                            from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                            where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Receipt_No")
                                                            select new { FYP_Receipt_No = b.IATD_PreviousValue }).ToList();
                                    FYP_Receipt_No = FYP_Receipt_No_l[0].FYP_Receipt_No;
                                }
                                else if (data.Report_Type == "receipt")
                                {
                                    FYP_Receipt_No = data.receiptNo;
                                }


                                var FYP_Bank_Or_Cash = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                        from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                        where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Bank_Or_Cash")
                                                        select new { FYP_Bank_Or_Cash = b.IATD_PreviousValue }).ToList();
                                var ITAT_Date_Time = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                      where (a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment")
                                                      select new
                                                      {
                                                          ITAT_Date = a.ITAT_Date,
                                                          ITAT_Time = a.ITAT_Time,
                                                          ITAT_Operation = a.ITAT_Operation,
                                                          ITAT_IPV4Address = a.ITAT_IPV4Address,
                                                          ITAT_MAACAddress = a.ITAT_MAACAddress
                                                      }).ToList();
                                var FYP_Remarks = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                   from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                   where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Remarks")
                                                   select new { FYP_Remarks = b.IATD_PreviousValue }).ToList();
                                var FYP_Tot_Amount = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                      from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                      where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Tot_Amount")
                                                      select new { FYP_Tot_Amount = b.IATD_PreviousValue }).ToList();
                                var FYP_Tot_Waived_Amt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                          from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                          where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Tot_Waived_Amt")
                                                          select new { FYP_Tot_Waived_Amt = b.IATD_PreviousValue }).ToList();
                                var FYP_Tot_Fine_Amt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                        from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                        where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Tot_Fine_Amt")
                                                        select new { FYP_Tot_Fine_Amt = b.IATD_PreviousValue }).ToList();
                                var FYP_Tot_Concession_Amt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                              from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                              where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Tot_Concession_Amt")
                                                              select new { FYP_Tot_Concession_Amt = b.IATD_PreviousValue }).ToList();
                                var Main_Details = new FeeTrailAuditDTO
                                {
                                    paymentid = paymentid,
                                    // Amst_Id = Convert.ToInt64(Amst_Id),
                                    // useridpassing = useridpassing,
                                    ITAT_Id = ITAT_Id,
                                    User_Name = User_Name,
                                    // Name = Name[0].Name,
                                    FYP_Receipt_No = FYP_Receipt_No,
                                    FYP_Bank_Or_Cash = FYP_Bank_Or_Cash[0].FYP_Bank_Or_Cash,
                                    ITAT_Date = ITAT_Date_Time[0].ITAT_Date,
                                    ITAT_Time = ITAT_Date_Time[0].ITAT_Time,
                                    FYP_Remarks = FYP_Remarks[0].FYP_Remarks,
                                    FYP_Tot_Amount = Convert.ToDecimal(FYP_Tot_Amount[0].FYP_Tot_Amount),
                                    FYP_Tot_Waived_Amt = Convert.ToDecimal(FYP_Tot_Waived_Amt[0].FYP_Tot_Waived_Amt),
                                    FYP_Tot_Fine_Amt = Convert.ToDecimal(FYP_Tot_Fine_Amt[0].FYP_Tot_Fine_Amt),
                                    FYP_Tot_Concession_Amt = Convert.ToDecimal(FYP_Tot_Concession_Amt[0].FYP_Tot_Concession_Amt),
                                    ITAT_Operation = ITAT_Date_Time[0].ITAT_Operation,
                                    ITAT_IPV4Address = ITAT_Date_Time[0].ITAT_IPV4Address,
                                    ITAT_MAACAddress = ITAT_Date_Time[0].ITAT_MAACAddress
                                };

                                if (stu_id_cnt.Count() > 0)
                                {
                                    var Amst_Id = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Single(t => stu_id_cnt.Contains(t.ITAT_Id) && t.IATD_ColumnName == "AMST_Id").IATD_PreviousValue;
                                    var Name = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                where (a.MI_Id == data.MI_ID && a.AMST_Id.ToString() == Amst_Id)
                                                select new { Name = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim() }).ToList();

                                    Main_Details.Amst_Id = Convert.ToInt64(Amst_Id);
                                    Main_Details.Name = Name[0].Name;
                                    S_786.Add(Main_Details);

                                    var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == ITAT_Id).Distinct().ToList();
                                    foreach (var x in reportdatelist)
                                    {
                                        SS_786.Add(x);
                                    }
                                }
                                else if (staff_id_cnt.Count() > 0)
                                {
                                    var HRME_Id = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Single(t => staff_id_cnt.Contains(t.ITAT_Id) && t.IATD_ColumnName == "HRME_Id").IATD_PreviousValue;
                                    var Name = (from a in _FeeGroupContext.MasterEmployee
                                                where (a.MI_Id == data.MI_ID && a.HRME_Id.ToString() == HRME_Id)
                                                select new { Name = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim() }).ToList();

                                    Main_Details.HRME_Id = Convert.ToInt64(HRME_Id);
                                    Main_Details.Name = Name[0].Name;
                                    S_786.Add(Main_Details);

                                    var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == ITAT_Id).Distinct().ToList();
                                    foreach (var x in reportdatelist)
                                    {
                                        SS_786.Add(x);
                                    }
                                }
                                else if (oth_stu_id_cnt.Count() > 0)
                                {
                                    var FMOST_Id = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Single(t => oth_stu_id_cnt.Contains(t.ITAT_Id) && t.IATD_ColumnName == "FMOST_Id").IATD_PreviousValue;
                                    var Name = (from a in _FeeGroupContext.FeeMasterOtherStudentDMO
                                                where (a.MI_Id == data.MI_ID && a.FMOST_Id.ToString() == FMOST_Id)
                                                select new { Name = a.FMOST_StudentName }).ToList();

                                    Main_Details.FMOST_Id = Convert.ToInt64(FMOST_Id);
                                    Main_Details.Name = Name[0].Name;
                                    S_786.Add(Main_Details);

                                    var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == ITAT_Id).Distinct().ToList();
                                    foreach (var x in reportdatelist)
                                    {
                                        SS_786.Add(x);
                                    }
                                }
                                else if (R_pre_stu_id_cnt.Count() > 0)
                                {
                                    var pasr_id = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Single(t => R_pre_stu_id_cnt.Contains(t.ITAT_Id) && t.IATD_ColumnName == "pasr_id").IATD_PreviousValue;
                                    var Name = (from a in _FeeGroupContext.stuapp
                                                where (a.MI_Id == data.MI_ID && a.pasr_id.ToString() == pasr_id)
                                                select new { Name = ((a.PASR_FirstName == null ? " " : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null ? " " : a.PASR_MiddleName) + " " + (a.PASR_LastName == null ? " " : a.PASR_LastName)).Trim() }).ToList();

                                    Main_Details.pasr_id = Convert.ToInt64(pasr_id);
                                    Main_Details.Name = Name[0].Name;
                                    S_786.Add(Main_Details);

                                    var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == ITAT_Id).Distinct().ToList();
                                    foreach (var x in reportdatelist)
                                    {
                                        SS_786.Add(x);
                                    }
                                }
                                else if (A_pre_stu_id_cnt.Count() > 0)
                                {
                                    var pasr_id = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Single(t => A_pre_stu_id_cnt.Contains(t.ITAT_Id) && t.IATD_ColumnName == "pasr_id").IATD_PreviousValue;
                                    var Name = (from a in _FeeGroupContext.stuapp
                                                where (a.MI_Id == data.MI_ID && a.pasr_id.ToString() == pasr_id)
                                                select new { Name = ((a.PASR_FirstName == null ? " " : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null ? " " : a.PASR_MiddleName) + " " + (a.PASR_LastName == null ? " " : a.PASR_LastName)).Trim() }).ToList();

                                    Main_Details.pasr_id = Convert.ToInt64(pasr_id);
                                    Main_Details.Name = Name[0].Name;
                                    S_786.Add(Main_Details);

                                    var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == ITAT_Id).Distinct().ToList();
                                    foreach (var x in reportdatelist)
                                    {
                                        SS_786.Add(x);
                                    }
                                }


                            }
                            data.Main_Details = S_786.Distinct().ToArray();
                            data.reportdatelist = SS_786.Distinct().ToArray();
                        }



                    }
                    else if (data.Report_Type == "studentttt")
                    {
                        var flag = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                    from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                    where (a.ITAT_Id == b.ITAT_Id && a.ITAT_TableName == "Fee_Y_Payment_School_Student" && a.ITAT_Operation == "D" && b.IATD_ColumnName == "AMST_Id" && b.IATD_PreviousValue == data.Amst_Id.ToString())
                                    select b.ITAT_Id).ToList();
                        if (flag.Count() > 0)
                        {
                            var flag_1 = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.IATD_ColumnName == "FYP_Id" && flag.Contains(t.ITAT_Id)).Select(t => t.IATD_PreviousValue).ToList();

                            var itat_ids = _FeeGroupContext.IVRM_Table_AuditTrailDMO.Where(t => t.ITAT_TableName == "Fee_Y_Payment" && t.ITAT_Operation == "D" && flag_1.Contains(t.ITAT_RecordPKID)).Select(t => t.ITAT_Id).ToList();

                            var final_itat_ids = (from a in itat_ids
                                                  from b in ITAT_Ids_M_U
                                                  where (a == b)
                                                  select a).Distinct().ToList();
                            if (final_itat_ids.Count() > 0)
                            {
                                List<FeeTrailAuditDTO> S_786 = new List<FeeTrailAuditDTO>();
                                List<IVRM_AuditTrail_DeatilsDMO> SS_786 = new List<IVRM_AuditTrail_DeatilsDMO>();
                                foreach (var ITAT_Id in final_itat_ids)
                                {
                                    var FYP_Id = _FeeGroupContext.IVRM_Table_AuditTrailDMO.Single(t => t.ITAT_Id == ITAT_Id).ITAT_RecordPKID;
                                    //var FYP_Id = fyp_ids_itat_ids.SingleOrDefault(t => t.ITAT_Id == ITAT_Id).paymentid.ToString();

                                    var paymentid = Convert.ToInt64(FYP_Id);
                                    var Amst_Id = data.Amst_Id;
                                    var Name = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                where (a.MI_Id == data.MI_ID && a.AMST_Id == Amst_Id)
                                                select new { Name = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim() }).ToList();
                                    var useridpassing = data.useridpassing;
                                    var User_Name = _FeeGroupContext.applicationUser.Single(t => t.Id == data.useridpassing).NormalizedUserName;

                                    var FYP_Bank_Or_Cash = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                            from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                            where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Bank_Or_Cash")
                                                            select new { FYP_Bank_Or_Cash = b.IATD_PreviousValue }).ToList();
                                    var FYP_Receipt_No = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                          from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                          where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Receipt_No")
                                                          select new { FYP_Receipt_No = b.IATD_PreviousValue }).ToList();
                                    var ITAT_Date_Time = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                          where (a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment")
                                                          select new
                                                          {
                                                              ITAT_Date = a.ITAT_Date,
                                                              ITAT_Time = a.ITAT_Time,
                                                              ITAT_Operation = a.ITAT_Operation,
                                                              ITAT_IPV4Address = a.ITAT_IPV4Address,
                                                              ITAT_MAACAddress = a.ITAT_MAACAddress
                                                          }).ToList();
                                    var FYP_Remarks = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                       from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                       where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Remarks")
                                                       select new { FYP_Remarks = b.IATD_PreviousValue }).ToList();
                                    var FYP_Tot_Amount = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                          from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                          where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Tot_Amount")
                                                          select new { FYP_Tot_Amount = b.IATD_PreviousValue == null ? "0" : b.IATD_PreviousValue }).ToList();
                                    var FYP_Tot_Waived_Amt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                              from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                              where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Tot_Waived_Amt")
                                                              select new { FYP_Tot_Waived_Amt = b.IATD_PreviousValue == null ? "0" : b.IATD_PreviousValue }).ToList();
                                    var FYP_Tot_Fine_Amt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                            from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                            where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Tot_Fine_Amt")
                                                            select new { FYP_Tot_Fine_Amt = b.IATD_PreviousValue == null ? "0" : b.IATD_PreviousValue }).ToList();
                                    var FYP_Tot_Concession_Amt = (from a in _FeeGroupContext.IVRM_Table_AuditTrailDMO
                                                                  from b in _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO
                                                                  where (a.ITAT_Id == b.ITAT_Id && a.ITAT_RecordPKID == FYP_Id && a.ITAT_Id == ITAT_Id && a.ITAT_Operation == "D" && a.ITAT_TableName == "Fee_Y_Payment" && b.IATD_ColumnName == "FYP_Tot_Concession_Amt")
                                                                  select new { FYP_Tot_Concession_Amt = b.IATD_PreviousValue == null ? "0" : b.IATD_PreviousValue }).ToList();
                                    var Main_Details = new FeeTrailAuditDTO
                                    {
                                        paymentid = paymentid,
                                        Amst_Id = Amst_Id,
                                        //  useridpassing = useridpassing,
                                        ITAT_Id = ITAT_Id,
                                        User_Name = User_Name,
                                        Name = Name[0].Name,
                                        FYP_Receipt_No = FYP_Receipt_No[0].FYP_Receipt_No,
                                        FYP_Bank_Or_Cash = FYP_Bank_Or_Cash[0].FYP_Bank_Or_Cash,
                                        ITAT_Date = ITAT_Date_Time[0].ITAT_Date,
                                        ITAT_Time = ITAT_Date_Time[0].ITAT_Time,
                                        FYP_Remarks = FYP_Remarks[0].FYP_Remarks,
                                        FYP_Tot_Amount = Convert.ToDecimal(FYP_Tot_Amount[0].FYP_Tot_Amount),
                                        FYP_Tot_Waived_Amt = Convert.ToDecimal(FYP_Tot_Waived_Amt[0].FYP_Tot_Waived_Amt),
                                        FYP_Tot_Fine_Amt = Convert.ToDecimal(FYP_Tot_Fine_Amt[0].FYP_Tot_Fine_Amt),
                                        FYP_Tot_Concession_Amt = Convert.ToDecimal(FYP_Tot_Concession_Amt[0].FYP_Tot_Concession_Amt),
                                        ITAT_Operation = ITAT_Date_Time[0].ITAT_Operation,
                                        ITAT_IPV4Address = ITAT_Date_Time[0].ITAT_IPV4Address,
                                        ITAT_MAACAddress = ITAT_Date_Time[0].ITAT_MAACAddress
                                    };
                                    S_786.Add(Main_Details);

                                    var reportdatelist = _FeeGroupContext.IVRM_AuditTrail_DeatilsDMO.Where(t => t.ITAT_Id == ITAT_Id).Distinct().ToList();
                                    foreach (var x in reportdatelist)
                                    {
                                        SS_786.Add(x);
                                    }

                                }
                                data.Main_Details = S_786.Distinct().ToArray();
                                data.reportdatelist = SS_786.Distinct().ToArray();
                            }

                        }
                    }


                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }

            return data;
        }
        public FeeTrailAuditDTO searchfilter(FeeTrailAuditDTO data)
        {
            try
            {
                if (data.filterinitialdata.Equals("studentttt"))
                {
                    var getamstid = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                     where (a.MI_Id == data.MI_ID  && a.ASMAY_Id == data.asmay_id)
                                     select new FeeTrailAuditDTO
                                     {
                                         Amst_Id = a.AMST_Id
                                     }).Distinct().ToList();


                    List<long> id = new List<long>();

                    foreach (var c in getamstid)
                    {
                        id.Add(c.Amst_Id);
                    }


                    data.searchfilter = data.searchfilter.ToUpper();

                    data.fillstud = (from a in _FeeGroupContext.AdmissionStudentDMO
                                     from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                         //    from c in _FeeGroupContext.FeeStudentTransactionDMO
                                     from d in _FeeGroupContext.AcademicYear
                                     where (a.AMST_Id == b.AMST_Id
                                     //&& c.AMST_Id == b.AMST_Id && c.ASMAY_Id == d.ASMAY_Id
                                     && d.ASMAY_Id == b.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1
                                     && a.AMST_ActiveFlag == 1 && id.Contains(a.AMST_Id)
                                     // && c.User_Id == data.useridpassing && c.MI_Id == data.MI_ID && c.ASMAY_Id == data.asmay_id
                                     && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id
                                     && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter)
                                     || a.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter)
                                     || a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter)
                                     || a.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                     select new FeeStudentTransactionDTO
                                     {
                                         Amst_Id = a.AMST_Id,
                                         AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                         (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                         (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim()
                                     }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();
                }

                if (data.filterinitialdata.Equals("receipt"))
                {
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstud = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                     from b in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                     from c in _FeeGroupContext.AdmissionStudentDMO
                                     from d in _FeeGroupContext.School_Adm_Y_StudentDMO
                                     from e in _FeeGroupContext.admissioncls
                                     from f in _FeeGroupContext.school_M_Section
                                     where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_ID && a.ASMAY_ID == data.asmay_id && a.FYP_Receipt_No.Contains(data.searchfilter) && d.ASMAY_Id == data.asmay_id && a.FYP_OnlineChallanStatusFlag.Equals("Sucessfull"))/* && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1*/
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
                                     }).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                }



            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeTrailAuditDTO getreport(FeeTrailAuditDTO data)
        {
            try
            {
                if (data.receiptNo == null)
                {
                    data.receiptNo = "0";
                }

                if (data.Report_Type == "date" && data.User_Status == true)
                {
                    data.useridpassing = data.useridpassing;
                }
                else
                {
                    data.useridpassing = 0;
                }


                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Trial_Audit";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10000;
                    cmd.Parameters.Add(new SqlParameter("@FYP_Receipt_No",
                     SqlDbType.NVarChar)
                    {
                        Value = data.receiptNo
                    });

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                     SqlDbType.NVarChar)
                    {
                        Value = data.Report_Type
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                     SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                    SqlDbType.DateTime)
                    {
                        Value = data.fromdate

                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                      SqlDbType.DateTime)
                    {
                        Value = data.todate

                    });
                    cmd.Parameters.Add(new SqlParameter("@statustype",
                      SqlDbType.VarChar)
                    {
                        Value = data.Status_IU_D
                    });


                    cmd.Parameters.Add(new SqlParameter("@Userid",
                  SqlDbType.BigInt)
                    {
                        Value = data.useridpassing
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
                        data.reportdatelist = retObject1.ToArray();
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
        public FeeTrailAuditDTO viewdetails(FeeTrailAuditDTO data)
        {
            try
            {
                if (data.receiptNo == null)
                {
                    data.receiptNo = "0";
                }

                if (data.Report_Type == "date" && data.User_Status == true)
                {
                    data.useridpassing = data.useridpassing;
                }
                else
                {
                    data.useridpassing = 0;
                }


                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Trial_Audit_View_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@FYP_Receipt_No",
                    // SqlDbType.NVarChar)
                    //{
                    //    Value = data.receiptNo
                    //});                   

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@FYP_Id",
                  SqlDbType.NVarChar)
                    {
                        Value = data.FYP_Id
                    });

                    //  cmd.Parameters.Add(new SqlParameter("@type",
                    //   SqlDbType.NVarChar)
                    //  {
                    //      Value = data.Report_Type
                    //  });
                    //  cmd.Parameters.Add(new SqlParameter("@amst_id",
                    //   SqlDbType.BigInt)
                    //  {
                    //      Value = data.Amst_Id
                    //  });
                    //  cmd.Parameters.Add(new SqlParameter("@fromdate",
                    //  SqlDbType.DateTime)
                    //  {
                    //      Value = data.fromdate

                    //  });
                    //  cmd.Parameters.Add(new SqlParameter("@todate",
                    //    SqlDbType.DateTime)
                    //  {
                    //      Value = data.todate

                    //  });
                    //  cmd.Parameters.Add(new SqlParameter("@statustype",
                    //    SqlDbType.VarChar)
                    //  {
                    //      Value = data.Status_IU_D
                    //  });


                    //  cmd.Parameters.Add(new SqlParameter("@Userid",
                    //SqlDbType.BigInt)
                    //  {
                    //      Value = data.useridpassing
                    //  });

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
                        data.getviewdetails = retObject1.ToArray();
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

    }
}
