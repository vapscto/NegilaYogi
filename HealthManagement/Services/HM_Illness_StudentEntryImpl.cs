using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.HealthManagement;
using DomainModel.Model.HealthManagement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.HealthManagement;

namespace HealthManagement.Services
{
    public class HM_Illness_StudentEntryImpl : Interfaces.HM_Illness_StudentEntryInterface
    {
        public HealthManagenentMasterContext _context;
        public DomainModelMsSqlServerContext _db;
        public HM_Illness_StudentEntryImpl(HealthManagenentMasterContext _cont, DomainModelMsSqlServerContext db)
        {
            _context = _cont;
            _db = db;
        }

        public HM_Illness_StudentEntryDTO LoadStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                data.GetMasterAcademicYearList = _context.Academic.Where(a => a.MI_Id == data.MI_Id
                && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetMasterIllnessList = _context.HM_M_IllnessDMO.Where(a => a.MI_Id == data.MI_Id && a.HMMILL_ActiveFlg == true).ToArray();

                data.GetTransactionIllnessList = (from a in _context.HM_T_IllnessDMO
                                                  from b in _context.Adm_M_Student
                                                  from c in _context.Academic
                                                  from d in _context.School_M_Class
                                                  from e in _context.School_M_Section
                                                  from f in _context.HM_M_IllnessDMO
                                                  where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                                  && a.HMMILL_Id == f.HMMILL_Id && f.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                                  select new HM_Illness_StudentEntryDTO
                                                  {
                                                      AMST_Id = a.AMST_Id,
                                                      HMTILL_Id = a.HMTILL_Id,
                                                      ASMAY_Id = a.ASMAY_Id,
                                                      ASMCL_Id = a.ASMCL_Id,
                                                      ASMS_Id = a.ASMS_Id,
                                                      AdmissionNo = b.AMST_AdmNo,
                                                      StudentName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                      (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                      (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                      HMTILL_Date = a.HMTILL_Date,
                                                      HMTILL_ActiveFlg = a.HMTILL_ActiveFlg,
                                                      HMMILL_IllnessName = f.HMMILL_IllnessName,
                                                      ClassName = d.ASMCL_ClassName,
                                                      SectionName = e.ASMC_SectionName,
                                                      YearName = c.ASMAY_Year,
                                                      HMTILL_CreatedDate = a.HMTILL_CreatedDate

                                                  }).Distinct().OrderByDescending(a => a.HMTILL_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HM_Illness_StudentEntryDTO SaveStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.HMTILL_Id > 0)
                {
                    data.Message = "Update";

                    var result = _context.HM_T_IllnessDMO.Where(a => a.HMTILL_Id == data.HMTILL_Id).ToList();
                    if (result.Count > 0)
                    {
                        var result_update = _context.HM_T_IllnessDMO.Single(a => a.HMTILL_Id == data.HMTILL_Id);
                        result_update.HMTILL_Date = data.HMTILL_Date;
                        result_update.HMTILL_UpdatedBy = data.UserId;
                        result_update.HMTILL_UpdatedDate = indiantime0;
                        _context.Update(result_update);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.ReturnValue = true;
                        }
                    }
                }
                else
                {
                    data.Message = "Add";
                    HM_T_IllnessDMO hM_T_IllnessDMO = new HM_T_IllnessDMO();
                    hM_T_IllnessDMO.HMMILL_Id = data.HMMILL_Id;
                    hM_T_IllnessDMO.ASMAY_Id = data.ASMAY_Id;
                    hM_T_IllnessDMO.ASMCL_Id = data.ASMCL_Id;
                    hM_T_IllnessDMO.ASMS_Id = data.ASMS_Id;
                    hM_T_IllnessDMO.AMST_Id = data.AMST_Id;
                    hM_T_IllnessDMO.HMTILL_Date = data.HMTILL_Date;
                    hM_T_IllnessDMO.HMTILL_ActiveFlg = true;
                    hM_T_IllnessDMO.HMTILL_CreatedBy = data.UserId;
                    hM_T_IllnessDMO.HMTILL_UpdatedBy = data.UserId;
                    hM_T_IllnessDMO.HMTILL_CreatedDate = indiantime0;
                    hM_T_IllnessDMO.HMTILL_UpdatedDate = indiantime0;
                    _context.Add(hM_T_IllnessDMO);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.ReturnValue = true;
                        data.HMTILL_Id = hM_T_IllnessDMO.HMTILL_Id;

                        var studentdetails = _context.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToList();

                        long mobileno = studentdetails.FirstOrDefault().AMST_MobileNo;
                        string emailid = studentdetails.FirstOrDefault().AMST_emailId;

                        if (data.smschecked == true)
                        {
                            try
                            {
                                SMS sms = new SMS(_db);
                                var d = sms.Send_Student_Illness_SMS(data.MI_Id, mobileno, "Student_Illness_Entry", data.HMTILL_Id, data.AMST_Id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        if (data.emailchecked == true)
                        {
                            try
                            {
                                Email Email = new Email(_db);
                                var d = Email.Send_Student_Illness_Email(data.MI_Id, emailid, "Student_Illness_Entry", data.HMTILL_Id, data.AMST_Id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        if (data.whatsappchecked == true)
                        {
                            try
                            {
                                SmsWithoutTemplate smsWithoutTemplate = new SmsWithoutTemplate(_db);
                                var s = smsWithoutTemplate.Send_StudentIllness_Whatsapp(data.MI_Id, mobileno, "", "", "", "Student_Illness_Entry",
                                    data.HMTILL_Id, data.AMST_Id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HM_Illness_StudentEntryDTO EditStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                var GetEditStudentIllnessData = _context.HM_T_IllnessDMO.Where(a => a.HMTILL_Id == data.HMTILL_Id).ToList();
                data.GetEditStudentIllnessData = GetEditStudentIllnessData.ToArray();

                data.GetEditIllnessData = _context.HM_M_IllnessDMO.Where(a => a.HMMILL_Id == GetEditStudentIllnessData.FirstOrDefault().HMMILL_Id).ToArray();

                data.GetEditStudentData = (from a in _context.Adm_M_Student
                                           from b in _context.Adm_School_Y_StudentDMO
                                           from c in _context.School_M_Class
                                           from d in _context.School_M_Section
                                           from e in _context.Academic
                                           where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                           && a.AMST_Id == GetEditStudentIllnessData.FirstOrDefault().AMST_Id
                                           && b.ASMAY_Id == GetEditStudentIllnessData.FirstOrDefault().ASMAY_Id)
                                           select new HM_Illness_StudentEntryDTO
                                           {
                                               AMST_Id = a.AMST_Id,
                                               StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                               (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) +
                                               (a.AMST_LastName == null ? "" : " " + a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                                               StudentName_Edit = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                               (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) +
                                               (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                               AdmissionNo = a.AMST_AdmNo,
                                               ClassName = c.ASMCL_ClassName,
                                               SectionName = d.ASMC_SectionName,
                                               YearName = e.ASMAY_Year
                                           }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HM_Illness_StudentEntryDTO ActiveDeactiveStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _context.HM_T_IllnessDMO.Where(a => a.HMTILL_Id == data.HMTILL_Id).ToList();
                if (result.Count > 0)
                {
                    var result_update = _context.HM_T_IllnessDMO.Single(a => a.HMTILL_Id == data.HMTILL_Id);
                    result_update.HMTILL_ActiveFlg = result_update.HMTILL_ActiveFlg == true ? false : true;
                    result_update.HMTILL_UpdatedBy = data.UserId;
                    result_update.HMTILL_UpdatedDate = indiantime0;
                    _context.Update(result_update);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.ReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HM_Illness_StudentEntryDTO GetStudentDetailsBySearch(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                data.Searchfilter = data.Searchfilter.ToUpper();
                data.GetMasterStudentList = (from a in _context.Adm_M_Student
                                             from b in _context.Adm_School_Y_StudentDMO
                                             where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                             && b.AMAY_ActiveFlag == 1 && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' '
                                             + a.AMST_LastName.Trim().ToUpper()).Contains(data.Searchfilter) || a.AMST_FirstName.Trim().ToUpper().StartsWith(data.Searchfilter) || a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.Searchfilter) || a.AMST_LastName.Trim().ToUpper().StartsWith(data.Searchfilter)))
                                             select new HM_Illness_StudentEntryDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                                 (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName) +
                                                 ':' + a.AMST_AdmNo).Trim(),
                                             }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HM_Illness_StudentEntryDTO OnStudentNameChange(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                data.GetStudentYearData = (from a in _context.Adm_School_Y_StudentDMO
                                           from b in _context.Adm_M_Student
                                           from c in _context.School_M_Class
                                           from d in _context.School_M_Section
                                           from e in _context.Academic
                                           where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)
                                           select new HM_Illness_StudentEntryDTO
                                           {
                                               AMST_Id = a.AMST_Id,
                                               ASMCL_Id = a.ASMCL_Id,
                                               ASMS_Id = a.ASMS_Id,
                                               ASMAY_Id = a.ASMAY_Id,
                                               StudentName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                 (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                 (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                               AdmissionNo = b.AMST_AdmNo,
                                               ClassName = c.ASMCL_ClassName,
                                               SectionName = d.ASMC_SectionName,
                                               YearName = e.ASMAY_Year

                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Student Illness Report
        public HM_Illness_StudentEntryDTO LoadStudentIllnessReportData(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                data.GetReportAcademicYearList = _context.Academic.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HM_Get_Student_Illness_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });                     

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetReportStudentList = retObject.ToArray();
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
        public HM_Illness_StudentEntryDTO ReportOnChangeYearData(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HM_Get_Student_Illness_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetReportStudentList = retObject.ToArray();
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
        public HM_Illness_StudentEntryDTO ReportStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            try
            {
                data.GetMasterInstitutionDetails = _context.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HM_student_Illness_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@REPORT_TYPE", SqlDbType.VarChar) { Value = data.ReportType });

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetReportDataList = retObject.ToArray();
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
