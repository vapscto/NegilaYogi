using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.FeedBack;
using DomainModel.Model.FeedBack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Services
{
    public class FeedbackTransactionImpl : Interface.FeedbackTransactionInterface
    {
        public FeedBackContext _context;
        public DomainModelMsSqlServerContext _dmcontext;
        ILogger<FeedbackTransactionImpl> _log;
        public FeedbackTransactionImpl(FeedBackContext context, DomainModelMsSqlServerContext dmcontext, ILogger<FeedbackTransactionImpl> log)
        {
            _log = log;
            _context = context;
            _dmcontext = dmcontext;
        }
        public FeedbackTransactionDTO getdetails(FeedbackTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                var rolenamet = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                if (rolenamet.ToUpper() == "STUDENT")
                {
                    data.Flag = "Student";
                }
                else if (rolenamet.ToUpper() == "PARENTS")
                {
                    data.Flag = "Parents";
                }
                else if (rolenamet.ToUpper() == "ALUMNI")
                {
                    data.Flag = "Alumni";
                }
                else
                {
                    data.Flag = "Staff";
                }

                if (rolenamet.ToUpper() != "ALUMNI" && rolenamet.ToUpper() != "STAFF")
                {
                    var checkamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.userid).ToList();

                    data.AMCST_Id = Convert.ToInt64(checkamcstid.FirstOrDefault().AMCST_Id);

                    var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                             from b in _context.Adm_Master_College_StudentDMO
                                             where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id
                                             && a.ACYST_ActiveFlag == 1 && b.AMCST_ActiveFlag == true && b.AMCST_SOL == "S")
                                             select new FeedbackTransactionDTO
                                             {
                                                 AMCO_Id = a.AMCO_Id,
                                                 AMB_Id = a.AMB_Id,
                                                 AMSE_Id = a.AMSE_Id
                                             }).Distinct().ToList();
                    data.getstudentdetails = getstudentdetails.ToArray();
                }


                data.typelistload = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true
                && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTransactionDTO getfeedback(FeedbackTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                var rolenamet = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                if (rolenamet.ToUpper() == "STUDENT")
                {
                    data.Flag = "Student";
                }
                else if (rolenamet.ToUpper() == "PARENTS")
                {
                    data.Flag = "Parents";
                }
                else if (rolenamet.ToUpper() == "ALUMNI")
                {
                    data.Flag = "Alumni";
                }
                else
                {
                    data.Flag = "Staff";
                }

                var typelist = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag
                && a.FMTY_SubjectSpecificFlag == false && a.FMTY_Id == data.FMTY_Id).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.typelist = typelist.ToArray();

                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_ActiveFlag == true).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();

                long checkdatecount = 0;
                long? feedbacknooftimes = 0;

                data.mappedquestiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                               from b in _context.Feedback_Master_QuestionsDMO
                                               from c in _context.Feedback_Type_QuestionsDMO
                                               where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.FMTY_ActiveFlag == true && b.FMQE_ActiveFlag == true
                                               && c.FMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMQE_ActiveFlag == true && a.FMTY_ActiveFlag == true
                                               && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false && a.FMTY_Id == data.FMTY_Id)
                                               select new FeedbackTransactionDTO
                                               {
                                                   FMTY_Id = c.FMTY_Id,
                                                   FMQE_Id = c.FMQE_Id,
                                                   FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                   FMTQ_TQOrder = c.FMTQ_TQOrder,
                                                   FMQE_ManualEntryFlg = b.FMQE_ManualEntryFlg
                                               }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();


                if (typelist.FirstOrDefault().FMTY_QuestionwiseOptionFlg == false)
                {
                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                 from b in _context.Feedback_Master_OptionsDMO
                                                 from c in _context.Feedback_Type_OptionsDMO
                                                 where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                                 && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true && c.FMTO_ActiveFlag == true
                                                 && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false && a.FMTY_Id == data.FMTY_Id)
                                                 select new FeedbackTransactionDTO
                                                 {
                                                     FMTY_Id = c.FMTY_Id,
                                                     FMOP_Id = c.FMOP_Id,
                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                     FMTO_TOOrder = c.FMTO_TQOrder
                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                }
                else
                {
                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                 from b in _context.Feedback_Master_OptionsDMO
                                                 from c in _context.Feedback_Type_QuestionsDMO
                                                 from d in _context.Feedback_Type_Questions_OptionsDMO
                                                 where (d.FMTQ_Id == c.FMTQ_Id && a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == d.FMOP_Id && a.FMTY_ActiveFlag == true
                                                 && b.FMOP_ActiveFlag == true && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true
                                                 && d.FMTQO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false
                                                 && a.FMTY_Id == data.FMTY_Id && c.FMTQ_ActiveFlag == true)
                                                 select new FeedbackTransactionDTO
                                                 {
                                                     FMTY_Id = c.FMTY_Id,
                                                     FMOP_Id = d.FMOP_Id,
                                                     FMQE_Id = c.FMQE_Id,
                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                     FMTO_TOOrder = d.FMTQO_TQOOrder
                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                }

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new FeedbackTransactionDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                if (rolenamet.ToUpper() == "STUDENT")
                {
                    var checkamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.userid).ToList();

                    var check_dataenter = (_context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.AMCST_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().AMCST_Id) && a.FMTY_Id == data.FMTY_Id
                    && a.FCSTR_StudParFlg == data.Flag && a.ASMAY_Id == data.ASMAY_Id)).ToList();
                    data.count = check_dataenter.Count();

                    var count = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.AMCST_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().AMCST_Id) && a.FMTY_Id == data.FMTY_Id
                    && a.FCSTR_StudParFlg == data.Flag && a.ASMAY_Id == data.ASMAY_Id).Distinct().Count();

                    checkdatecount = count;

                    feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByStudent;

                }
                else if (rolenamet.ToUpper() == "PARENTS")
                {
                    var checkamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.userid).ToList();

                    var check_dataenter = (_context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.AMCST_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().AMCST_Id) && a.FMTY_Id == data.FMTY_Id
                    && a.FCSTR_StudParFlg == data.Flag && a.FCSTR_CreatedBy == data.userid && a.ASMAY_Id == data.ASMAY_Id)).ToList();
                    data.count = check_dataenter.Count();

                    var count = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.AMCST_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().AMCST_Id) && a.FMTY_Id == data.FMTY_Id
                    && a.FCSTR_StudParFlg == data.Flag && a.FCSTR_CreatedBy == data.userid && a.ASMAY_Id == data.ASMAY_Id).Count();

                    checkdatecount = count;

                    feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByParent;
                }
                else if (rolenamet.ToUpper() == "ALUMNI")
                {
                    var checkamcstid = _context.CLGAlumni_User_LoginDMO.Where(a => a.IVRMUL_Id == data.userid).ToList();
                    var check_dataenter = (_context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id
                    && a.ALCSREG_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().ALCSREG_Id) && a.FMTY_Id == data.FMTY_Id
                    && a.FCALTR_CreatedBy == data.userid && a.ASMAY_ID == data.ASMAY_Id)).ToList();
                    data.count = check_dataenter.Count();

                    var count = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id
                     && a.ALCSREG_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().ALCSREG_Id) && a.FMTY_Id == data.FMTY_Id
                     && a.FCALTR_CreatedBy == data.userid && a.ASMAY_ID == data.ASMAY_Id).Count();

                    checkdatecount = count;
                    feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByAlumni;

                }
                else
                {
                    var checkamcstid = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();
                    var check_dataenter = (_context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id ==
                    Convert.ToInt64(checkamcstid.FirstOrDefault().Emp_Code) && a.FMTY_Id == data.FMTY_Id && a.FSTTR_ActiveFlag == true)).ToList();
                    data.count = check_dataenter.Count();


                    var count1 = (from a in _context.Feedback_Staff_TransactionDMO
                                  from b in _context.AcademicYear
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().Emp_Code)
                                  && a.FMTY_Id == data.FMTY_Id && a.FSTTR_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id
                                  && Convert.ToDateTime(b.ASMAY_From_Date) <= Convert.ToDateTime(a.FSSTR_FeedbackDate.Date)
                                  && Convert.ToDateTime(b.ASMAY_To_Date) >= Convert.ToDateTime(a.FSSTR_FeedbackDate.Date))
                                  select a).Distinct().Count();


                    //var count = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                    //&& a.HRME_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().Emp_Code) && a.FMTY_Id == data.FMTY_Id
                    //&& a.FSTTR_ActiveFlag == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Count();

                    checkdatecount = count1;

                    feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByParent;
                }

                if ((feedbacknooftimes * data.mappedquestiondeetails.Length) > checkdatecount)
                {
                    data.count = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTransactionDTO savedata(FeedbackTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                if (data.Flag.ToUpper() == "STUDENT" || data.Flag.ToUpper() == "PARENTS")
                {

                    var checkamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.userid).ToList();

                    data.AMCST_Id = Convert.ToInt64(checkamcstid.FirstOrDefault().AMCST_Id);


                    var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                             from b in _context.Adm_Master_College_StudentDMO
                                             where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id
                                             && a.ACYST_ActiveFlag == 1 && b.AMCST_ActiveFlag == true && b.AMCST_SOL == "S")
                                             select new FeedbackTransactionDTO
                                             {
                                                 AMCO_Id = a.AMCO_Id,
                                                 AMB_Id = a.AMB_Id,
                                                 AMSE_Id = a.AMSE_Id
                                             }).Distinct().ToList();



                    if (data.temp.Count() > 0)
                    {
                        for (int k = 0; k < data.temp.Count(); k++)
                        {
                            for (int j = 0; j < data.temp[k].ques.Count(); j++)
                            {
                                Feedback_College_Student_TransactionDMO dmo = new Feedback_College_Student_TransactionDMO();
                                if (data.temp[k].ques[j].manualflg == true)
                                {
                                    dmo.FCSTR_FeedBack = data.temp[k].ques[j].name;
                                }
                                else
                                {
                                    dmo.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                    dmo.FCSTR_FeedBack = "";
                                }
                                dmo.MI_Id = data.MI_Id;
                                dmo.AMCST_Id = data.AMCST_Id;
                                dmo.ASMAY_Id = data.ASMAY_Id;
                                dmo.AMCO_Id = Convert.ToInt64(getstudentdetails.FirstOrDefault().AMCO_Id);
                                dmo.AMB_Id = getstudentdetails.FirstOrDefault().AMB_Id;
                                dmo.AMSE_Id = getstudentdetails.FirstOrDefault().AMSE_Id;
                                dmo.FCSTR_FeedbackDate = DateTime.Now;
                                dmo.FMTY_Id = data.temp[k].FMTY_Id;
                                dmo.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                dmo.FCSTR_StudParFlg = data.Flag;
                                dmo.FCSTR_ActiveFlag = true;
                                dmo.FCSTR_UpdatedBy = data.userid;
                                dmo.FCSTR_CreatedBy = data.userid;
                                dmo.UpdatedDate = DateTime.Now;
                                dmo.CreatedDate = DateTime.Now;
                                _context.Add(dmo);
                            }
                        }

                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                else if (data.Flag.ToUpper() == "ALUMNI")
                {
                    var checkamcstid = _context.CLGAlumni_User_LoginDMO.Where(a => a.IVRMUL_Id == data.userid).ToList();

                    data.AMCST_Id = Convert.ToInt64(checkamcstid.FirstOrDefault().ALCSREG_Id);

                    if (data.temp.Count() > 0)
                    {
                        for (int k = 0; k < data.temp.Count(); k++)
                        {
                            for (int j = 0; j < data.temp[k].ques.Count(); j++)
                            {
                                Feedback_CLG_Alumni_TransactionDMO dmo = new Feedback_CLG_Alumni_TransactionDMO();

                                if (data.temp[k].ques[j].manualflg == true)
                                {
                                    dmo.FCALTR_FeedBack = data.temp[k].ques[j].name;
                                }
                                else
                                {
                                    dmo.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                    dmo.FCALTR_FeedBack = "";
                                }
                                dmo.MI_Id = data.MI_Id;
                                dmo.ALCSREG_Id = data.AMCST_Id;
                                dmo.ASMAY_ID = data.ASMAY_Id;
                                dmo.FCALTR_FeedbackDate = DateTime.Now;
                                dmo.FMTY_Id = data.temp[k].FMTY_Id;
                                dmo.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                dmo.FCALTR_ActiveFlag = true;

                                dmo.FCALTR_UpdatedBy = data.userid;
                                dmo.FCALTR_CreatedBy = data.userid;
                                dmo.FCALTR_UpdatedDate = DateTime.Now;
                                dmo.FCALTR_CreatedDate = DateTime.Now;
                                _context.Add(dmo);
                            }
                        }

                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                else
                {
                    var checkamcstid = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();
                    data.HRME_Id = checkamcstid.FirstOrDefault().Emp_Code;

                    if (data.temp.Count() > 0)
                    {
                        for (int k = 0; k < data.temp.Count(); k++)
                        {
                            for (int j = 0; j < data.temp[k].ques.Count(); j++)
                            {
                                Feedback_Staff_TransactionDMO staff = new Feedback_Staff_TransactionDMO();
                                if (data.temp[k].ques[j].manualflg == true)
                                {
                                    staff.FSTTR_FeedBack = data.temp[k].ques[j].name;
                                }
                                else
                                {
                                    staff.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                    staff.FSTTR_FeedBack = "";
                                }
                                staff.MI_Id = data.MI_Id;
                                staff.HRME_Id = data.HRME_Id;
                                staff.FSSTR_FeedbackDate = DateTime.Now;
                                staff.FMTY_Id = data.temp[k].FMTY_Id;
                                staff.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                //  staff.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                staff.FSTTR_ActiveFlag = true;
                                staff.FSTTR_CreatedBy = data.userid;
                                staff.FSTTR_UpdatedBy = data.userid;
                                staff.CreatedDate = DateTime.Now;
                                staff.UpdatedDate = DateTime.Now;
                                _context.Add(staff);
                            }
                        }
                    }
                    if (data.temp.Count() > 0)
                    {
                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
                _log.LogInformation("College Student Feedback Save Parents :" + ex.Message);
            }
            return data;
        }
        public FeedbackTransactionDTO getstudentstaffdetails(FeedbackTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                         from b in _context.Adm_Master_College_StudentDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && b.MI_Id == data.MI_Id
                                         && a.ACYST_ActiveFlag == 1 && b.AMCST_ActiveFlag == true && b.AMCST_SOL == "S")
                                         select new FeedbackTransactionDTO
                                         {
                                             AMCO_Id = a.AMCO_Id,
                                             AMB_Id = a.AMB_Id,
                                             AMSE_Id = a.AMSE_Id
                                         }).Distinct().ToList();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Studentwise_SubjectList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });

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
                        data.getsubjectlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTransactionDTO getstaffname(FeedbackTransactionDTO data)
        {

            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                         from b in _context.Adm_Master_College_StudentDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id
                                         && a.ACYST_ActiveFlag == 1 && b.AMCST_ActiveFlag == true && b.AMCST_SOL == "S")
                                         select new FeedbackTransactionDTO
                                         {
                                             AMCO_Id = a.AMCO_Id,
                                             AMB_Id = a.AMB_Id,
                                             AMSE_Id = a.AMSE_Id
                                         }).Distinct().ToList();

                List<FeedbackTransactionDTO> details = new List<FeedbackTransactionDTO>();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Staff_Subject_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                details.Add(new FeedbackTransactionDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                    staffname = (dataReader["staffname"]).ToString(),
                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.typelistload = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_ActiveFlag == true).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();


                data.mappedquestiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                               from b in _context.Feedback_Master_QuestionsDMO
                                               from c in _context.Feedback_Type_QuestionsDMO
                                               where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.FMTY_ActiveFlag == true && b.FMQE_ActiveFlag == true
                                               && c.FMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMQE_ActiveFlag == true && a.FMTY_ActiveFlag == true
                                               && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true)
                                               select new FeedbackTransactionDTO
                                               {
                                                   FMTY_Id = c.FMTY_Id,
                                                   FMQE_Id = c.FMQE_Id,
                                                   FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                   FMTQ_TQOrder = c.FMTQ_TQOrder
                                               }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();

                data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                             from b in _context.Feedback_Master_OptionsDMO
                                             from c in _context.Feedback_Type_OptionsDMO
                                             where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                             && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true && c.FMTO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true)
                                             select new FeedbackTransactionDTO
                                             {
                                                 FMTY_Id = c.FMTY_Id,
                                                 FMOP_Id = c.FMOP_Id,
                                                 FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                 FMTO_TOOrder = c.FMTO_TQOrder
                                             }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new FeedbackTransactionDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                ).ToList();


                var checkamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.userid).ToList();


                if (details.Count() > 0)
                {
                    data.HRME_Id = details.FirstOrDefault().HRME_Id;

                    data.getstaffdetails = details.ToArray();

                    var check_dataenter = (_context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id &&
                    a.AMCO_Id == getstudentdetails.FirstOrDefault().AMCO_Id && a.AMB_Id == getstudentdetails.FirstOrDefault().AMB_Id &&
                    a.AMSE_Id == getstudentdetails.FirstOrDefault().AMSE_Id && a.FCSTST_FeedbackDate.Date == (DateTime.Now).Date &&
                    a.ISMS_Id == data.ISMS_Id && a.AMCST_Id == data.AMCST_Id && a.HRME_Id == data.HRME_Id)).ToList();

                    data.count = check_dataenter.Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTransactionDTO getfeedbacksubject(FeedbackTransactionDTO data)
        {
            try
            {
                var typelist = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag
                     && a.FMTY_SubjectSpecificFlag == true && a.FMTY_Id == data.FMTY_Id).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.typelist = typelist.ToArray();

                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                data.typelistload = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag
                && a.FMTY_SubjectSpecificFlag == true && a.FMTY_Id == data.FMTY_Id).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_ActiveFlag == true).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();

                long checkdatecount = 0;
                long? feedbacknooftimes = 0;


                data.mappedquestiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                               from b in _context.Feedback_Master_QuestionsDMO
                                               from c in _context.Feedback_Type_QuestionsDMO
                                               where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.FMTY_ActiveFlag == true && b.FMQE_ActiveFlag == true
                                               && c.FMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMQE_ActiveFlag == true && a.FMTY_ActiveFlag == true
                                               && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true && c.FMTY_Id == data.FMTY_Id)
                                               select new FeedbackTransactionDTO
                                               {
                                                   FMTY_Id = c.FMTY_Id,
                                                   FMQE_Id = c.FMQE_Id,
                                                   FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                   FMTQ_TQOrder = c.FMTQ_TQOrder,
                                                   FMQE_ManualEntryFlg = b.FMQE_ManualEntryFlg
                                               }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();


                if (typelist.FirstOrDefault().FMTY_QuestionwiseOptionFlg == false)
                {
                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                 from b in _context.Feedback_Master_OptionsDMO
                                                 from c in _context.Feedback_Type_OptionsDMO
                                                 where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                                 && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true && c.FMTO_ActiveFlag == true
                                                 && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true && c.FMTY_Id == data.FMTY_Id)
                                                 select new FeedbackTransactionDTO
                                                 {
                                                     FMTY_Id = c.FMTY_Id,
                                                     FMOP_Id = c.FMOP_Id,
                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                     FMTO_TOOrder = c.FMTO_TQOrder
                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                }
                else
                {
                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                 from b in _context.Feedback_Master_OptionsDMO
                                                 from c in _context.Feedback_Type_QuestionsDMO
                                                 from d in _context.Feedback_Type_Questions_OptionsDMO
                                                 where (d.FMTQ_Id == c.FMTQ_Id && a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == d.FMOP_Id && a.FMTY_ActiveFlag == true
                                                 && b.FMOP_ActiveFlag == true && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true
                                                 && d.FMTQO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false
                                                 && a.FMTY_Id == data.FMTY_Id && c.FMTQ_ActiveFlag == true)
                                                 select new FeedbackTransactionDTO
                                                 {
                                                     FMTY_Id = c.FMTY_Id,
                                                     FMOP_Id = d.FMOP_Id,
                                                     FMQE_Id = c.FMQE_Id,
                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                     FMTO_TOOrder = d.FMTQO_TQOOrder
                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();

                }

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new FeedbackTransactionDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                ).ToList();

                var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                         from b in _context.Adm_Master_College_StudentDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id
                                         && a.ACYST_ActiveFlag == 1 && b.AMCST_ActiveFlag == true && b.AMCST_SOL == "S")
                                         select new FeedbackTransactionDTO
                                         {
                                             AMCO_Id = a.AMCO_Id,
                                             AMB_Id = a.AMB_Id,
                                             AMSE_Id = a.AMSE_Id
                                         }).Distinct().ToList();

                var check_dataenter = (_context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id &&
                    a.AMCO_Id == getstudentdetails.FirstOrDefault().AMCO_Id && a.AMB_Id == getstudentdetails.FirstOrDefault().AMB_Id &&
                    a.AMSE_Id == getstudentdetails.FirstOrDefault().AMSE_Id && a.ISMS_Id == data.ISMS_Id && a.AMCST_Id == data.AMCST_Id
                    && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id)).ToList();

                data.count = check_dataenter.Count();

                var count = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id &&
                    a.AMCO_Id == getstudentdetails.FirstOrDefault().AMCO_Id && a.AMB_Id == getstudentdetails.FirstOrDefault().AMB_Id &&
                    a.AMSE_Id == getstudentdetails.FirstOrDefault().AMSE_Id && a.ISMS_Id == data.ISMS_Id && a.AMCST_Id == data.AMCST_Id
                    && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id).Count();

                checkdatecount = count;

                feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByStudent;

                if ((feedbacknooftimes * data.mappedquestiondeetails.Length) > checkdatecount)
                {
                    data.count = 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTransactionDTO studentstaffsavedata(FeedbackTransactionDTO data)
        {
            try
            {

                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                         from b in _context.Adm_Master_College_StudentDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id
                                         && a.ACYST_ActiveFlag == 1 && b.AMCST_ActiveFlag == true && b.AMCST_SOL == "S")
                                         select new FeedbackTransactionDTO
                                         {
                                             AMCO_Id = a.AMCO_Id,
                                             AMB_Id = a.AMB_Id,
                                             AMSE_Id = a.AMSE_Id
                                         }).Distinct().ToList();



                if (data.temp.Count() > 0)
                {
                    for (int k = 0; k < data.temp.Count(); k++)
                    {
                        for (int j = 0; j < data.temp[k].ques.Count(); j++)
                        {
                            Feedback_College_StudentToStaffDMO dmo = new Feedback_College_StudentToStaffDMO();

                            if (data.temp[k].ques[j].manualflg == true)
                            {
                                dmo.FCSTST_FeedBack = data.temp[k].ques[j].name;
                            }
                            else
                            {
                                dmo.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                dmo.FCSTST_FeedBack = "";
                            }

                            dmo.MI_Id = data.MI_Id;
                            dmo.AMCST_Id = data.AMCST_Id;
                            dmo.ASMAY_Id = data.ASMAY_Id;
                            dmo.AMCO_Id = Convert.ToInt64(getstudentdetails.FirstOrDefault().AMCO_Id);
                            dmo.AMB_Id = getstudentdetails.FirstOrDefault().AMB_Id;
                            dmo.AMSE_Id = getstudentdetails.FirstOrDefault().AMSE_Id;
                            dmo.HRME_Id = data.HRME_Id;
                            dmo.ISMS_Id = data.ISMS_Id;
                            dmo.FCSTST_FeedbackDate = DateTime.Now;
                            dmo.FMTY_Id = data.temp[k].FMTY_Id;
                            dmo.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                            dmo.FCSTST_ActiveFlag = true;
                            dmo.FCSTST_UpdatedBy = data.userid;
                            dmo.FCSTST_CreatedBy = data.userid;
                            dmo.UpdatedDate = DateTime.Now;
                            dmo.CreatedDate = DateTime.Now;
                            _context.Add(dmo);
                        }
                    }

                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
                _log.LogInformation("College Student Feedback Save Staff Student Subject :" + ex.Message);
            }
            return data;
        }


        // Feedbcak Modulewise 17-02-2024


        public FeedbackTransactionDTO modulewisefeedback(FeedbackTransactionDTO data)
        {
            try
            {

                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                //var checkamcstid = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();
                //data.HRME_Id = checkamcstid.FirstOrDefault().Emp_Code;



                if (data.savemodulefeedback.Count() > 0)
                {

                    for (int j = 0; j < data.savemodulefeedback.Count(); j++)
                    {
                        Feedback_Staff_TransactionDMO staff = new Feedback_Staff_TransactionDMO();

                        staff.FSTTR_FeedBack = data.savemodulefeedback[j].FSTTR_FeedBack;
                        staff.FMOP_Id = Convert.ToInt64(data.savemodulefeedback[j].name);
                        staff.MI_Id = data.MI_Id;
                        staff.HRME_Id = data.HRME_Id;
                        staff.FSSTR_FeedbackDate = DateTime.Now;
                        staff.FMTY_Id = data.savemodulefeedback[j].FMTY_Id;
                        staff.FMQE_Id = data.savemodulefeedback[j].FMQE_Id;
                        staff.FSTTR_ActiveFlag = true;
                        staff.FSTTR_CreatedBy = data.userid;
                        staff.FSTTR_UpdatedBy = data.userid;
                        staff.CreatedDate = DateTime.Now;
                        staff.UpdatedDate = DateTime.Now;
                        _context.Add(staff);
                    }


                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;

                        feedbacktransactioninsertvms(data);
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }

        public FeedbackTransactionDTO feedbacktransactioninsertvms(FeedbackTransactionDTO data)
        {

            try
            {
                // FeedbackTransactionDTO vmspaymentsubsctiptiondto = new FeedbackTransactionDTO();
                if (data.Flag == "Admin" || data.Flag == "ADMIN")
                {
                    data.staffname = _dmcontext.ApplicationUser.Where(t => t.Id == data.userid).Select(t => t.UserName).FirstOrDefault();
                }
                else
                {
                    //data.staffname = _dmcontext.StaffLoginDMO.Where(t => t.Id == data.userid).Select(t => t.UserName).FirstOrDefault();

                    var staffname = (from a in _dmcontext.Staff_User_Login
                                     from b in _dmcontext.HR_Master_Employee_DMO
                                     where a.Emp_Code == b.HRME_Id && a.Id == data.userid
                                     select new FeedbackTransactionDTO
                                     {

                                         staffname = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim()

                                     }).Distinct().ToList();

                    data.staffname = staffname[0].staffname;
                }
                   

                    string inst_code = "VTS";

                var institutedata = (from a in _context.Institution
                                     where (a.MI_Id == data.MI_Id)
                                     select new FeedbackTransactionDTO
                                     {
                                         IVRM_MI_Id = a.MI_Id,
                                         Institute_Name = a.MI_Name,
                                         Institute_code = a.MI_Subdomain

                                     }).ToList();

                data.IVRM_MI_Id = institutedata[0].IVRM_MI_Id;
                data.Institute_Name = institutedata[0].Institute_Name;
                data.Institute_code = institutedata[0].Institute_code;

                List<FeedbackTransactionDTO> geturl = new List<FeedbackTransactionDTO>();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getvmsbaseurl";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INST_CODE",
                            SqlDbType.VarChar)
                    {
                        Value = inst_code
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }



                                geturl.Add(new FeedbackTransactionDTO
                                {
                                    baseurl = (dataReader["IIVRSCURL_APIURL"].ToString())

                                });

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                    }
                }

                if (geturl.Count > 0)
                {

                    HttpClient client1 = new HttpClient();
                    client1.BaseAddress = new Uri(geturl[0].baseurl);
                    //client1.BaseAddress = new Uri("https://tpreadmission.azurewebsites.net/");
                    // client1.BaseAddress = new Uri("https://ivrmdemo.vapssmartecampus.com:42001/");
                    client1.DefaultRequestHeaders.Accept.Clear();
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client1.PostAsJsonAsync("api/ModuleWiseFeedbackReportFacade/saveModuleWisefeedback", data).Result;

                    string description = string.Empty;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        description = result;

                        data = JsonConvert.DeserializeObject<FeedbackTransactionDTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;



        }

        //Modulewise feedback load data 

        public FeedbackTransactionDTO loadfeedbackquestion(FeedbackTransactionDTO data)
        {
            try
            {

                var checkfeedbackcount = (from a in _context.Feedback_Staff_TransactionDMO
                                          from b in _context.FeedBackMasterTypeDMO
                                          from c in _context.Feedback_Master_QuestionsDMO
                                          from d in _context.Feedback_Type_QuestionsDMO
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.FMTY_Id == b.FMTY_Id && a.FMQE_Id == c.FMQE_Id && b.MI_Id == d.MI_Id && b.FMTY_Id == d.FMTY_Id && c.FMQE_Id == d.FMQE_Id && c.FMQE_FeedbackQRemarks == data.Flag && a.MI_Id == data.MI_Id)
                                          select new FeedbackTransactionDTO
                                          {
                                              FMQE_Id = a.FMQE_Id,
                                              FMTY_Id = a.FMTY_Id,
                                              FSSTR_FeedbackDate = a.FSSTR_FeedbackDate


                                          }).LastOrDefault();


                var totaldiffdays = 0;

                if (checkfeedbackcount != null)
                {

                    TimeSpan difference = (DateTime.Now.Date) - (checkfeedbackcount.FSSTR_FeedbackDate.Date);

                    totaldiffdays = (int)(difference.TotalDays);
                }


                if (checkfeedbackcount == null || totaldiffdays >= 45)
                {
                    data.feedbackquestion = (from a in _context.Feedback_Type_QuestionsDMO
                                             from c in _context.FeedBackMasterTypeDMO
                                             from b in _context.Feedback_Master_QuestionsDMO
                                             where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.FMTY_Id == c.FMTY_Id && a.FMQE_Id == b.FMQE_Id && a.MI_Id == data.MI_Id && b.FMQE_FeedbackQRemarks == data.Flag && b.FMQE_ActiveFlag == true)
                                             select new FeedbackTransactionDTO
                                             {

                                                 FMTY_Id = a.FMTY_Id,
                                                 FMQE_Id = b.FMQE_Id,
                                                 FMTY_FeedbackTypeName = c.FMTY_FeedbackTypeName,
                                                 FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                 FMQE_FeedbackQRemarks = b.FMQE_FeedbackQRemarks,
                                                 FMQE_FQOrder = b.FMQE_FQOrder,
                                                 FMQE_ManualEntryFlg = b.FMQE_ManualEntryFlg

                                             }).OrderBy(a => a.FMQE_FQOrder).ToArray();


                    data.feedbackoption = (from a in _context.Feedback_Master_OptionsDMO
                                           from b in _context.Feedback_Type_OptionsDMO
                                           from c in _context.Feedback_Type_QuestionsDMO
                                           from d in _context.Feedback_Master_QuestionsDMO
                                           where (a.MI_Id == b.MI_Id && a.FMOP_Id == b.FMOP_Id && b.MI_Id == c.MI_Id && b.FMTY_Id == c.FMTY_Id && c.MI_Id == d.MI_Id && c.FMQE_Id == d.FMQE_Id && a.MI_Id == data.MI_Id && d.FMQE_FeedbackQRemarks == data.Flag && a.FMOP_ActiveFlag == true)
                                           select new FeedbackTransactionDTO
                                           {
                                               FMOP_Id = a.FMOP_Id,
                                               FMOP_FeedbackOptions = a.FMOP_FeedbackOptions,
                                               FMOP_FOOrder = a.FMOP_FOOrder,
                                               FMOP_OptionsValue = a.FMOP_OptionsValue,
                                               FMTY_Id = b.FMTY_Id,
                                               FMQE_Id = d.FMQE_Id

                                           }).OrderBy(a => a.FMOP_FOOrder).Distinct().ToArray();


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
