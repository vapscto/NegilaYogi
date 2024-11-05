using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class ManagementDashboardMonthlyIMPL : Interfaces.ManagementDashboardMonthlyInterface
    {
        private static ConcurrentDictionary<string, ManagementDashboardMonthlyIMPL> _login =
       new ConcurrentDictionary<string, ManagementDashboardMonthlyIMPL>();

        private readonly PortalContext _ChairmanDashboardContext;

        public DomainModelMsSqlServerContext _db;
        public ManagementDashboardMonthlyIMPL(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        public ManagementDashboardMonthlyDTO Getdetails(ManagementDashboardMonthlyDTO data)
        {
            try
            {
                data.acayear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).Distinct().ToArray();
                data.fillmonth = _db.month.Where(t => t.Is_Active == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public ManagementDashboardMonthlyDTO getreport(ManagementDashboardMonthlyDTO data)
        {

            try
            {
                data.MI_Address = _ChairmanDashboardContext.Institute.Where(R => R.MI_Id == data.MI_Id).Take(1).Distinct().ToArray();             
                data.Smscount = _db.IVRM_sms_sentBoxDMO.Where(t => t.MI_Id == data.MI_Id && t.Datetime.Month == data.Month && t.Datetime.Year == data.year).Distinct().ToArray();
                data.Emailcount = _db.IVRM_Email_sentBoxDMO.Where(M => M.MI_Id == data.MI_Id && M.Datetime.Year == data.year && M.Datetime.Month == data.Month).Distinct().ToArray();
                data.Birthdaylist = _db.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id   &&  a.AMST_DOB.Month == data.Month && a.AMST_SOL.Equals("S")).Distinct().ToArray();
                //data.Birthdaylist = (from a in _db.Adm_M_Student
                //                     from b in _db.School_Adm_Y_StudentDMO
                //                     from c in _db.School_M_Class 
                //                     from d in _db.School_M_Section
                //                     where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL.Equals("S") && b.AMAY_ActiveFlag == 1 && a.AMST_DOB.DayOfYear >= data.year && a.AMST_DOB.DayOfYear <= data.year && a.AMST_DOB.Month >= data.Month && a.AMST_DOB.Month <= data.Month)
                //                     select a

                //                 ).Distinct().ToArray();
                data.totalstudent = (from a in _db.School_Adm_Y_StudentDMO
                                     from b in _db.admissioncls
                                     where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)
                                     select a
                                   ).Distinct().ToArray();
                data.BoniFiedCertificate = (from a in _ChairmanDashboardContext.Adm_Students_Certificate_Apply_DMO
                                            from b in _ChairmanDashboardContext.Adm_Students_Certificate_Approve_DMO
                                            where (a.ASCA_Id == b.ASCA_Id && a.ASCA_ActiveFlg == true && a.ASCA_CertificateType == "BC" && b.ASCAP_ApproveDate.Month == data.Month)
                                            select a
                                          ).Distinct().ToArray();
                data.Tctaken = _ChairmanDashboardContext.Student_TC.Where(R => R.ASTC_TCIssueDate.Value.Month == data.Month && R.ASTC_TCIssueDate.Value.Year == data.year).Distinct().ToArray();
                data.feecolllection = (from FYP in _ChairmanDashboardContext.FeeYPaymentDMO
                                       from FTP in _ChairmanDashboardContext.FeeTransactionPaymentDMO
                                       from FMA in _ChairmanDashboardContext.FeeAmountEntryDMO
                                       from FMH in _ChairmanDashboardContext.FeeHeadDMO
                                       where (FTP.FYP_Id == FYP.FYP_Id && FMA.FMA_Id == FTP.FMA_Id && FMA.ASMAY_Id == FYP.ASMAY_ID && FMH.FMH_Id == FMA.FMH_Id && FMH.MI_Id == FMA.MI_Id && FMA.MI_Id==data.MI_Id && FMA.ASMAY_Id==data.ASMAY_Id && FYP.ASMAY_ID==data.ASMAY_Id && FYP.FYP_Date.Month==data.Month && FYP.FYP_Date.Year==data.year )
                                       select FYP
                                     ).Distinct().ToArray();
                data.empstrenth = _ChairmanDashboardContext.HR_Master_Employee_DMO.Where(R => R.MI_Id == data.MI_Id && R.HRME_DOJ.Value.Year == data.year).Distinct().ToArray();
                //&& R.HRME_DOJ.Value.Month==data.Month
                data.empleft = _ChairmanDashboardContext.HR_Master_Employee_DMO.Where(Q => Q.MI_Id == data.MI_Id && Q.HRME_DOL.Value.Year==data.year && Q.HRME_DOL.Value.Month==data.Month).Distinct().ToArray();
                data.monthname = _ChairmanDashboardContext.IVRM_Month_DMO.Where(K => K.IVRM_Month_Id == data.Month).Select(U => U.IVRM_Month_Name).FirstOrDefault();
                data.empsalary = _ChairmanDashboardContext.HR_Employee_Salary.Where(M => M.MI_Id == data.MI_Id && M.HRES_Year==data.year.ToString() && M.HRES_Month==data.monthname).Distinct().ToArray();
                data.preadmision = _ChairmanDashboardContext.Enq.Where(R => R.MI_Id == data.MI_Id && R.PASR_Date.Value.Year==data.year && R.PASR_Date.Value.Month==data.Month).Distinct().ToArray();             
                using (var cmd = _ChairmanDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ChairmanDefaulter";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@Year",
                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@month",
                 SqlDbType.BigInt)
                    {
                        Value = data.Month
                    });

                    cmd.Parameters.Add(new SqlParameter("@amay",
                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
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
                       data.Defaluter = retObject.ToArray();

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
