using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Text;
using System.IO;
using System.Data;
using System.Dynamic;
using System.Data.SqlClient;

namespace WebApplication1.Services
{
    public class ConcessionApprovalImpl : Interfaces.ConcessionApprovalInterface
    {
        private static ConcurrentDictionary<string, Preadmission_School_Registration_CatergoryDTO> _login =
            new ConcurrentDictionary<string, Preadmission_School_Registration_CatergoryDTO>();

        public Preadmission_School_Registration_CatergoryContext _Preadmission_School_Registration_CatergoryContext;
        public ConcessionApprovalImpl(Preadmission_School_Registration_CatergoryContext Preadmission_School_Registration_CatergoryContext)
        {
            _Preadmission_School_Registration_CatergoryContext = Preadmission_School_Registration_CatergoryContext;
        }

        public Preadmission_School_Registration_CatergoryDTO getstudentdetails(Preadmission_School_Registration_CatergoryDTO dta)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                List<StudentApplication> stulst = new List<StudentApplication>();

                var concessiontype = _Preadmission_School_Registration_CatergoryContext.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == dta.FMCC_Id).FMCC_ConcessionFlag;
                dta.concessiontype = concessiontype;

                var Acdemic_preadmission = _Preadmission_School_Registration_CatergoryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == dta.MI_ID).Select(d => d.ASMAY_Id).FirstOrDefault();
                dta.ASMAY_Id = Acdemic_preadmission;

                if (concessiontype == "S")
                {
                    if (dta.configurationsettings.ISPAC_ApplFeeFlag == 1)
                    {
                        stulst = (from a in _Preadmission_School_Registration_CatergoryContext.StudentSibling
                                  from b in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                                  from c in _Preadmission_School_Registration_CatergoryContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                  where (a.PASR_Id == b.pasr_id && b.pasr_id==c.PASA_Id && b.FMCC_ID == dta.FMCC_Id && b.MI_Id == dta.MI_ID && b.ASMAY_Id == dta.ASMAY_Id && c.FYPPA_Type=="R" && a.PASRS_Status != "C" && a.PASRS_Status != "R" && b.PASR_Adm_Confirm_Flag == false
                                  && a.PASRS_SiblingsName != "" && a.PASRS_SiblingsName != null && a.PASRS_SiblingsName != "nil" && a.PASRS_SiblingsName != "na" && a.PASRS_SiblingsName != "nan" && a.PASRS_SiblingsName != "no")
                                  select new StudentApplication
                                  {
                                      PASR_FirstName = b.PASR_FirstName.ToUpper(),
                                      PASR_MiddleName = b.PASR_MiddleName.ToUpper(),
                                      PASR_LastName = b.PASR_LastName.ToUpper(),
                                      pasr_id = b.pasr_id
                                  }
                    ).Distinct().ToList();
                    }
                    else
                    {
                        stulst = (from a in _Preadmission_School_Registration_CatergoryContext.StudentSibling
                                  from b in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                                  where (a.PASR_Id == b.pasr_id && b.FMCC_ID == dta.FMCC_Id && b.MI_Id == dta.MI_ID && b.ASMAY_Id == dta.ASMAY_Id && a.PASRS_Status != "C" && a.PASRS_Status != "R" && b.PASR_Adm_Confirm_Flag == false && a.PASRS_SiblingsName != "" && a.PASRS_SiblingsName != null && a.PASRS_SiblingsName != "nil" && a.PASRS_SiblingsName != "na" && a.PASRS_SiblingsName != "nan" && a.PASRS_SiblingsName != "no")
                                  select new StudentApplication
                                  {
                                      PASR_FirstName = b.PASR_FirstName.ToUpper(),
                                      PASR_MiddleName = b.PASR_MiddleName.ToUpper(),
                                      PASR_LastName = b.PASR_LastName.ToUpper(),
                                      pasr_id = b.pasr_id
                                  }
                    ).Distinct().ToList();
                    }
                       
                }
                else if (concessiontype == "E")
                {
                    List<long> temparr = new List<long>();
                    var employeestudentd = _Preadmission_School_Registration_CatergoryContext.PAStudentEmployee.Where(t => t.MI_Id == dta.MI_ID).ToList();
                    if (employeestudentd.Count() > 0)
                    {
                        foreach (PAStudentEmployee mob in employeestudentd)
                        {
                            temparr.Add(mob.PASR_Id);
                        }
                    }
                    if (dta.configurationsettings.ISPAC_ApplFeeFlag == 1)
                    {
                        stulst = (from a in _Preadmission_School_Registration_CatergoryContext.HR_Master_Employee_DMO
                                  from b in _Preadmission_School_Registration_CatergoryContext.Preadmission_School_Registration_Employee
                                  from c in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                                  from d in _Preadmission_School_Registration_CatergoryContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                  where a.HRME_Id == b.HRME_ID && b.PASR_Id == c.pasr_id && c.pasr_id==d.PASA_Id && a.MI_Id == dta.MI_ID && b.PSRE_ActiveFlag == null && a.HRME_ActiveFlag == true && d.FYPPA_Type=="R" && c.PASR_Adm_Confirm_Flag==false &&  !temparr.Contains(b.PASR_Id)
                                  select new StudentApplication
                                  {
                                      PASR_FirstName = c.PASR_FirstName.ToUpper(),
                                      PASR_MiddleName = c.PASR_MiddleName.ToUpper(),
                                      PASR_LastName = c.PASR_LastName.ToUpper(),
                                      pasr_id = c.pasr_id
                                  }
                                        ).ToList();
                    }
                    else
                    {
                        stulst = (from a in _Preadmission_School_Registration_CatergoryContext.HR_Master_Employee_DMO
                                  from b in _Preadmission_School_Registration_CatergoryContext.Preadmission_School_Registration_Employee
                                  from c in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                                  where a.HRME_Id == b.HRME_ID && b.PASR_Id == c.pasr_id && a.MI_Id == dta.MI_ID && b.PSRE_ActiveFlag == null && a.HRME_ActiveFlag == true && c.PASR_Adm_Confirm_Flag == false && !temparr.Contains(b.PASR_Id)
                                  select new StudentApplication
                                  {
                                      PASR_FirstName = c.PASR_FirstName.ToUpper(),
                                      PASR_MiddleName = c.PASR_MiddleName.ToUpper(),
                                      PASR_LastName = c.PASR_LastName.ToUpper(),
                                      pasr_id = c.pasr_id
                                  }
                                     ).ToList();
                    }
                }
                else if (concessiontype == "G")
                {
                    if (dta.configurationsettings.ISPAC_ApplFeeFlag == 1)
                    {
                        stulst = (from a in _Preadmission_School_Registration_CatergoryContext.StudentSibling
                                  from b in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                                  from c in _Preadmission_School_Registration_CatergoryContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                  where (a.PASR_Id == b.pasr_id && b.pasr_id == c.PASA_Id && b.FMCC_ID == dta.FMCC_Id && b.MI_Id == dta.MI_ID && b.ASMAY_Id == dta.ASMAY_Id && c.FYPPA_Type == "R" && a.PASRS_Status != "C" && a.PASRS_Status != "R" && b.PASR_Adm_Confirm_Flag == false
                                  && a.PASRS_SiblingsName != "" && a.PASRS_SiblingsName != null && a.PASRS_SiblingsName != "nil" && a.PASRS_SiblingsName != "na" && a.PASRS_SiblingsName != "nan" && a.PASRS_SiblingsName != "no")
                                  select new StudentApplication
                                  {
                                      PASR_FirstName = b.PASR_FirstName.ToUpper(),
                                      PASR_MiddleName = b.PASR_MiddleName.ToUpper(),
                                      PASR_LastName = b.PASR_LastName.ToUpper(),
                                      pasr_id = b.pasr_id
                                  }
                    ).Distinct().ToList();
                    }
                    else
                    {
                        stulst = (from a in _Preadmission_School_Registration_CatergoryContext.StudentSibling
                                  from b in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                                  where (a.PASR_Id == b.pasr_id && b.FMCC_ID == dta.FMCC_Id && b.MI_Id == dta.MI_ID && b.ASMAY_Id == dta.ASMAY_Id && a.PASRS_Status != "C" && a.PASRS_Status != "R" && b.PASR_Adm_Confirm_Flag == false && a.PASRS_SiblingsName != "" && a.PASRS_SiblingsName != null && a.PASRS_SiblingsName != "nil" && a.PASRS_SiblingsName != "na" && a.PASRS_SiblingsName != "nan" && a.PASRS_SiblingsName != "no")
                                  select new StudentApplication
                                  {
                                      PASR_FirstName = b.PASR_FirstName.ToUpper(),
                                      PASR_MiddleName = b.PASR_MiddleName.ToUpper(),
                                      PASR_LastName = b.PASR_LastName.ToUpper(),
                                      pasr_id = b.pasr_id
                                  }
                    ).Distinct().ToList();
                    }
                }
                if (concessiontype == "R")
                {
                    if (dta.configurationsettings.ISPAC_ApplFeeFlag == 1)
                    {
                        stulst = (from a in _Preadmission_School_Registration_CatergoryContext.PA_Student_Sibblings
                                  from b in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                                  from c in _Preadmission_School_Registration_CatergoryContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                  where (a.PASR_Id == b.pasr_id && b.pasr_id == c.PASA_Id && b.FMCC_ID == dta.FMCC_Id && b.MI_Id == dta.MI_ID && b.ASMAY_Id == dta.ASMAY_Id && c.FYPPA_Type == "R" && a.PASS_ActiveFlag == false && a.PASS_ConcessionAmt == 100 && b.PASR_Adm_Confirm_Flag == false && a.PASS_ConcessionAmt == 100)
                                  select new StudentApplication
                                  {
                                      PASR_FirstName = b.PASR_FirstName.ToUpper(),
                                      PASR_MiddleName = b.PASR_MiddleName.ToUpper(),
                                      PASR_LastName = b.PASR_LastName.ToUpper(),
                                      pasr_id = b.pasr_id
                                  }
                    ).Distinct().ToList();
                    }
                    else
                    {
                        stulst = (from a in _Preadmission_School_Registration_CatergoryContext.PA_Student_Sibblings
                                  from b in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                                  where (a.PASR_Id == b.pasr_id && b.FMCC_ID == dta.FMCC_Id && b.MI_Id == dta.MI_ID && b.ASMAY_Id == dta.ASMAY_Id && a.PASS_ActiveFlag == false && a.PASS_ConcessionAmt == 100 && b.PASR_Adm_Confirm_Flag == false)
                                  select new StudentApplication
                                  {
                                      PASR_FirstName = b.PASR_FirstName.ToUpper(),
                                      PASR_MiddleName = b.PASR_MiddleName.ToUpper(),
                                      PASR_LastName = b.PASR_LastName.ToUpper(),
                                      pasr_id = b.pasr_id
                                  }
                    ).Distinct().ToList();
                    }

                }

                dta.fillstudentlst = stulst.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dta;
        }

        public Preadmission_School_Registration_CatergoryDTO loadta(Preadmission_School_Registration_CatergoryDTO id)
        {
            Preadmission_School_Registration_CatergoryDTO data = new Preadmission_School_Registration_CatergoryDTO();
            try
            {
                List<Fee_Master_ConcessionDMO> cat = new List<Fee_Master_ConcessionDMO>();
                cat = _Preadmission_School_Registration_CatergoryContext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id == id.MI_ID).ToList();
                data.fillcategory = cat.ToArray();

                var Acdemic_preadmission = _Preadmission_School_Registration_CatergoryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == id.MI_ID).Select(d => d.ASMAY_Id).FirstOrDefault();

                using (var cmd = _Preadmission_School_Registration_CatergoryContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "PreadmissionConcessionListStudent";
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.Add(new SqlParameter("@year",
            SqlDbType.BigInt)
                {
                    Value = Acdemic_preadmission
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
         SqlDbType.VarChar)
                {
                    Value = id.MI_ID
                });
               

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                //  var data = cmd.ExecuteNonQuery();

                try
                {
                    //   var data = cmd.ExecuteNonQuery();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                var datatype = dataReader.GetFieldType(iFiled);
                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow.Add(
                                   dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                               );
                                }
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                        data.concessionliststudent = retObject.ToArray();
                }
                catch (Exception ex)
                {

                }
            }

                using (var cmd = _Preadmission_School_Registration_CatergoryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PreadmissionConcessionListEmployee";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@year",
                SqlDbType.BigInt)
                    {
                        Value = Acdemic_preadmission
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
             SqlDbType.VarChar)
                    {
                        Value = id.MI_ID
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //  var data = cmd.ExecuteNonQuery();

                    try
                    {
                        //   var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                       dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                   );
                                    }
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.concessionliststaff = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Preadmission_School_Registration_CatergoryDTO oncheckgetstudentdetails(Preadmission_School_Registration_CatergoryDTO data)
        {
            try
            {
                var concessiontype = _Preadmission_School_Registration_CatergoryContext.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == data.FMCC_Id).FMCC_ConcessionFlag;
                data.concessiontype = concessiontype;
                if (concessiontype == "S")
                {
                    data.fillstudentlst = (from a in _Preadmission_School_Registration_CatergoryContext.StudentSibling
                                           where (a.PASR_Id == data.PASR_Id && a.PASRS_Status != "C" && a.PASRS_Status != "R")
                                           select new Preadmission_School_Registration_CatergoryDTO
                                           {
                                               PASRS_SiblingsAdmissionNo = a.PASRS_SiblingsAdmissionNo,
                                               PASRS_SiblingsName = a.PASRS_SiblingsName,
                                               PASRS_SiblingsClass = a.PASRS_SiblingsClass,
                                               verrejstatus = "Pending",
                                               PASR_Id = a.PASR_Id,
                                               PASRS_Id = a.PASRS_Id
                                           }
                         ).ToArray();
                }
                else if (concessiontype == "E")
                {
                    List<long> temparr = new List<long>();
                    var employeestudentd = _Preadmission_School_Registration_CatergoryContext.PAStudentEmployee.Where(t => t.MI_Id == data.MI_ID).ToList();
                    if (employeestudentd.Count() > 0)
                    {
                        foreach (PAStudentEmployee mob in employeestudentd)
                        {
                            temparr.Add(mob.PASR_Id);
                        }
                    }
                    data.fillstaff = (from a in _Preadmission_School_Registration_CatergoryContext.HR_Master_Employee_DMO
                                      from b in _Preadmission_School_Registration_CatergoryContext.Preadmission_School_Registration_Employee
                                      from e in _Preadmission_School_Registration_CatergoryContext.HR_Master_Department
                                      from f in _Preadmission_School_Registration_CatergoryContext.HR_Master_Designation
                                      where a.HRME_Id == b.HRME_ID && a.MI_Id == data.MI_ID && a.HRME_ActiveFlag == true && e.HRMD_Id == a.HRMD_Id && f.HRMDES_Id == a.HRMDES_Id && !temparr.Contains(b.PASR_Id) && b.PASR_Id == data.PASR_Id && b.PSRE_ActiveFlag == null
                                      select new StaffLoginDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          stafname = ((a.HRME_EmployeeFirstName == null ? "" : a.HRME_EmployeeFirstName.ToUpper()) + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName.ToUpper()) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName.ToUpper())).Trim(),
                                          HRME_Photo=a.HRME_Photo,
                                          department=e.HRMD_DepartmentName,
                                          designation=f.HRMDES_DesignationName
                                      }).ToArray();
                }
                else if (concessiontype == "R")
                {
                    data.fillstudentlst = (from a in _Preadmission_School_Registration_CatergoryContext.PA_Student_Sibblings
                              from b in _Preadmission_School_Registration_CatergoryContext.StudentApplication
                              from c in _Preadmission_School_Registration_CatergoryContext.AdmissionClass
                              where (b.ASMCL_Id==c.ASMCL_Id && a.PASR_Id==b.pasr_id && a.PASR_Id == data.PASR_Id )
                              select new Preadmission_School_Registration_CatergoryDTO
                              {
                                  PASRS_SiblingsName = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)).Trim(),
                                  PASRS_SiblingsAdmissionNo = b.PASR_RegistrationNo,
                                  PASRS_SiblingsClass=c.ASMCL_ClassName,
                                  PASR_Id = b.pasr_id
                              }
                   ).ToArray();
                }
                else if (concessiontype == "G")
                {
                    data.fillstudentlst = (from a in _Preadmission_School_Registration_CatergoryContext.StudentSibling
                                           where (a.PASR_Id == data.PASR_Id && a.PASRS_Status != "C" && a.PASRS_Status != "R")
                                           select new Preadmission_School_Registration_CatergoryDTO
                                           {
                                               PASRS_SiblingsAdmissionNo = a.PASRS_SiblingsAdmissionNo,
                                               PASRS_SiblingsName = a.PASRS_SiblingsName,
                                               PASRS_SiblingsClass = a.PASRS_SiblingsClass,
                                               verrejstatus = "Pending",
                                               PASR_Id = a.PASR_Id,
                                               PASRS_Id = a.PASRS_Id
                                           }
                         ).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Preadmission_School_Registration_CatergoryDTO confirmdta(Preadmission_School_Registration_CatergoryDTO data)
        {
            int i = 0;
            try
            {
                List<Preadmission_School_Registration_CatergoryDTO> abc = new List<Preadmission_School_Registration_CatergoryDTO>();

                if (data.confirmorrejectstatus == "Check")
                {
                    if (data.studentdetails.Count > 0)
                    {
                        for (i = 0; i < data.studentdetails.Count; i++)
                        {

                            string sibname = data.studentdetails[i].PASRS_SiblingsName.ToString();
                            //string sibclass = data.studentdetails[i].PASRS_SiblingsClass.ToString();

                            if (data.studentdetails[i].PASRS_SiblingsAdmissionNo != null && data.studentdetails[i].PASRS_SiblingsAdmissionNo != "")
                            {
                                string admno = data.studentdetails[i].PASRS_SiblingsAdmissionNo.ToString();
                                data.fillstudentlst = (from a in _Preadmission_School_Registration_CatergoryContext.Adm_M_Student
                                                       from b in _Preadmission_School_Registration_CatergoryContext.School_Adm_Y_StudentDMO
                                                       from c in _Preadmission_School_Registration_CatergoryContext.AdmissionClass
                                                       where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_ID && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && a.AMST_AdmNo == admno)
                                                       select new Preadmission_School_Registration_CatergoryDTO
                                                       {
                                                           PASRS_SiblingsName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : a.AMST_LastName)).Trim(),
                                                           PASRS_SiblingsClass = c.ASMCL_ClassName,
                                                           PASRS_SiblingsAdmissionNo = a.AMST_AdmNo,
                                                           studentphtoto=a.AMST_Photoname,
                                                           fathername=a.AMST_FatherName,
                                                           AMST_Id = a.AMST_Id,

                                                       }
                       ).ToArray();
                            }
                            if (sibname != "" && (data.fillstudentlst == null || data.fillstudentlst.Length == 0))
                            {
                                data.fillstudentlst = (from a in _Preadmission_School_Registration_CatergoryContext.Adm_M_Student
                                                       from b in _Preadmission_School_Registration_CatergoryContext.School_Adm_Y_StudentDMO
                                                       from c in _Preadmission_School_Registration_CatergoryContext.AdmissionClass
                                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && b.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_ID && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : a.AMST_LastName)).Trim().Contains(sibname))
                                                       select new Preadmission_School_Registration_CatergoryDTO
                                                       {
                                                           PASRS_SiblingsName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : a.AMST_LastName)).Trim(),
                                                           PASRS_SiblingsClass = c.ASMCL_ClassName,
                                                           PASRS_SiblingsAdmissionNo = a.AMST_AdmNo,
                                                           studentphtoto = a.AMST_Photoname,
                                                           fathername = a.AMST_FatherName,
                                                           AMST_Id = a.AMST_Id
                                                       }
                       ).ToArray();
                            }

                            if (data.fillstudentlst.Length > 0)
                            {
                                data.studentdetails[i].verrejstatus = "Match";
                            }
                            else
                            {
                                data.studentdetails[i].verrejstatus = "Not Match";
                            }

                        }
                    }
                }

                else if (data.confirmorrejectstatus == "C" || data.confirmorrejectstatus == "R")
                {
                    savestatusconfirmdata(data);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Preadmission_School_Registration_CatergoryDTO savestatusconfirmdata(Preadmission_School_Registration_CatergoryDTO data)
        {
            Preadmission_School_Registration_Concession_StatusDMO statusdta = new Preadmission_School_Registration_Concession_StatusDMO();
            try
            {
                var concessiontype = _Preadmission_School_Registration_CatergoryContext.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == data.FMCC_Id).FMCC_ConcessionFlag;
                if (concessiontype == "S")
                {
                    data.PASRS_Id = data.studentdetails1[0].PASRS_Id;
                }
                var confirmstatus = 0;

                if (data.confirmorrejectstatus == "C" && concessiontype != "R")
                {
                    if (data.studentdetails.Count > 0)
                    {
                        statusdta.Flag = true;
                        for (int i = 0; i < data.studentdetails.Count; i++)
                        {
                            var duplicatecheck = _Preadmission_School_Registration_CatergoryContext.PA_Student_Sibblings.Where(d => d.PASR_Id == data.PASR_Id).ToList();

                            if (duplicatecheck.Count() == 0)
                            {
                                PA_Student_Sibblings siblingsave = new PA_Student_Sibblings();
                                siblingsave.MI_Id = data.MI_ID;
                                siblingsave.PASR_Id = data.PASR_Id;
                                siblingsave.PASS_ActiveFlag = true;
                                siblingsave.PASS_ConcessionAmt = null;
                                siblingsave.PASS_ConcessionPer = null;
                                siblingsave.CreatedDate = DateTime.Now;
                                siblingsave.UpdatedDate = DateTime.Now;
                                _Preadmission_School_Registration_CatergoryContext.Add(siblingsave);
                                _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                                data.PASS_Id = siblingsave.PASS_Id;
                            }
                            else
                            {
                                data.PASS_Id = duplicatecheck.FirstOrDefault().PASS_Id;
                            }

                            if (concessiontype == "S")
                            {
                                var duplicatecheck2 = _Preadmission_School_Registration_CatergoryContext.PA_Student_Sibblings_Details.Where(d => d.PASS_Id == data.PASS_Id).OrderByDescending(t => t.PASSD_SibblingOrder).ToList();
                                if (duplicatecheck2.Count() == 0)
                                {
                                    PA_Student_Sibblings_Details siblingstudent = new PA_Student_Sibblings_Details();
                                    siblingstudent.PASS_Id = data.PASS_Id;
                                    siblingstudent.PASSD_ActiveFlag = true;
                                    siblingstudent.PASSD_SibblingAMST_Id = data.studentdetails[i].AMST_Id;
                                    siblingstudent.PASSD_SibblingOrder = 1;
                                    siblingstudent.CreatedDate = DateTime.Now;
                                    siblingstudent.UpdatedDate = DateTime.Now;
                                    _Preadmission_School_Registration_CatergoryContext.Add(siblingstudent);
                                    confirmstatus = _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                                    if (confirmstatus > 0)
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
                                    var orderodlaststudent = duplicatecheck2.FirstOrDefault().PASSD_SibblingOrder;
                                    PA_Student_Sibblings_Details siblingstudent = new PA_Student_Sibblings_Details();
                                    siblingstudent.PASS_Id = data.PASS_Id;
                                    siblingstudent.PASSD_ActiveFlag = true;
                                    siblingstudent.PASSD_SibblingAMST_Id = data.studentdetails[i].AMST_Id;
                                    siblingstudent.PASSD_SibblingOrder = orderodlaststudent + 1;
                                    siblingstudent.CreatedDate = DateTime.Now;
                                    siblingstudent.UpdatedDate = DateTime.Now;
                                    _Preadmission_School_Registration_CatergoryContext.Add(siblingstudent);
                                    confirmstatus = _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                                    if (confirmstatus > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                    }
                                }
                            }
                            else if (concessiontype == "E")
                            {
                                PAStudentEmployee siblingemployee = new PAStudentEmployee();
                                siblingemployee.PASS_Id = data.PASS_Id;
                                siblingemployee.PASR_Id = data.PASR_Id;
                                siblingemployee.MI_Id = data.MI_ID;
                                siblingemployee.PASE_ActiveFlag = true;
                                siblingemployee.HRME_Id = data.HRME_Id;
                                siblingemployee.CreatedDate = DateTime.Now;
                                siblingemployee.UpdatedDate = DateTime.Now;
                                _Preadmission_School_Registration_CatergoryContext.Add(siblingemployee);
                                confirmstatus = _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                                if (confirmstatus > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }


                                var duplicatecheckEmp = _Preadmission_School_Registration_CatergoryContext.Preadmission_School_Registration_Employee.Where(d => d.PASR_Id == data.PASR_Id).ToList();
                                if (duplicatecheckEmp.Count() > 0)
                                {
                                    var updatesib = _Preadmission_School_Registration_CatergoryContext.Preadmission_School_Registration_Employee.Single(d => d.PASR_Id == data.PASR_Id);
                                    updatesib.PSRE_ActiveFlag = true;
                                    _Preadmission_School_Registration_CatergoryContext.Update(updatesib);
                                    confirmstatus = _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                                    if (confirmstatus > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                    }
                                }
                            }

                            if (data.returnval == true)
                            {
                                if (concessiontype != "E")
                                {
                                    var duplicatecheck3 = _Preadmission_School_Registration_CatergoryContext.StudentSibling.Where(d => d.PASRS_Id == data.PASRS_Id).ToList();
                                    if (duplicatecheck3.Count() > 0)
                                    {
                                        var updatesib = _Preadmission_School_Registration_CatergoryContext.StudentSibling.Single(d => d.PASRS_Id == data.PASRS_Id);
                                        updatesib.PASRS_Status = "C";
                                        _Preadmission_School_Registration_CatergoryContext.Update(updatesib);
                                        _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
                else if (data.confirmorrejectstatus == "R")
                {
                    if (concessiontype != "E")
                    {
                        var duplicatecheck3 = _Preadmission_School_Registration_CatergoryContext.StudentSibling.Where(d => d.PASRS_Id == data.PASRS_Id).ToList();
                        if (duplicatecheck3.Count() > 0)
                        {
                            var updatesib = _Preadmission_School_Registration_CatergoryContext.StudentSibling.Single(d => d.PASRS_Id == data.PASRS_Id);
                            updatesib.PASRS_Status = "R";
                            _Preadmission_School_Registration_CatergoryContext.Update(updatesib);
                            confirmstatus = _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                            if (confirmstatus > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                    else if (concessiontype == "E")
                    {
                        var duplicatecheckEmp = _Preadmission_School_Registration_CatergoryContext.Preadmission_School_Registration_Employee.Where(d => d.PASR_Id == data.PASR_Id).ToList();
                        if (duplicatecheckEmp.Count() > 0)
                        {
                            var updatesib = _Preadmission_School_Registration_CatergoryContext.Preadmission_School_Registration_Employee.Single(d => d.PASR_Id == data.PASR_Id);
                            updatesib.PSRE_ActiveFlag = false;
                            _Preadmission_School_Registration_CatergoryContext.Update(updatesib);
                            confirmstatus = _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                            if (confirmstatus > 0)
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

                if(data.confirmorrejectstatus == "C" && concessiontype == "R")
                {
                    var duplicatecheck3 = _Preadmission_School_Registration_CatergoryContext.PA_Student_Sibblings.Where(d => d.PASR_Id == data.PASR_Id && d.PASS_ConcessionAmt == 100 && d.PASS_ActiveFlag==false).ToList();
                    if (duplicatecheck3.Count() > 0)
                    {
                        var updatesib = _Preadmission_School_Registration_CatergoryContext.PA_Student_Sibblings.Single(d => d.PASR_Id == data.PASR_Id && d.PASS_ConcessionAmt == 100);
                        updatesib.PASS_ActiveFlag = true;
                        _Preadmission_School_Registration_CatergoryContext.Update(updatesib);
                        confirmstatus = _Preadmission_School_Registration_CatergoryContext.SaveChanges();
                        if (confirmstatus > 0)
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

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}


