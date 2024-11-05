using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentCompliantsImpl : Interfaces.StudentCompliantsInterface
    {
        public AdmissionFormContext _context;
        public StudentCompliantsImpl(AdmissionFormContext _con)
        {
            _context = _con;
        }
        public async Task<StudentCompliants_DTO> loaddata(StudentCompliants_DTO data)
        {
            try
            {
                data.allacademicyear = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_StudentCompliants_AllStudents";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
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
                        data.allstudentdivlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
            return data;
        }
        public async Task<StudentCompliants_DTO> getstudents(StudentCompliants_DTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_StudentCompliants_AllStudents";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
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
                        data.allstudentdivlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception E)
            {

                Console.WriteLine(E.Message);
            }
            return data;
        }
        public StudentCompliants_DTO save(StudentCompliants_DTO data)
        {
            try
            {
                if (data.ASCOMP_Id == 0)
                {
                    StudentCompliants_DMO dmo = new StudentCompliants_DMO();
                    dmo.MI_Id = data.MI_Id;
                    dmo.AMST_Id = data.AMST_Id;
                    dmo.ASCOMP_Date = data.ASCOMP_Date;
                    dmo.ASCOMP_Complaints = data.ASCOMP_Complaints;
                    dmo.ASCOMP_Subject = data.ASCOMP_Subject;
                    dmo.ASCOMP_FileName = data.ASCOMP_FileName;
                    dmo.ASCOMP_FilePath = data.ASCOMP_FilePath;
                    dmo.ASCOMP_ComplaintsBy = data.UserId;
                    dmo.ASCOMP_ActiveFlg = true;
                    dmo.ASCOMP_CreatedBy = data.UserId;
                    dmo.ASCOMP_CreatedDate = DateTime.Now;
                    dmo.ASCOMP_UpdatedBy = data.UserId;
                    dmo.ASCOMP_UpdatedDate = DateTime.Now;
                    _context.Add(dmo);
                    int ii = _context.SaveChanges();
                    if (ii > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "notsaved";
                    }
                }
                else if (data.ASCOMP_Id > 0)
                {
                    var dmolist = _context.StudentCompliants_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCOMP_Id == data.ASCOMP_Id).SingleOrDefault();
                    dmolist.ASCOMP_Complaints = data.ASCOMP_Complaints;
                    dmolist.ASCOMP_Subject = data.ASCOMP_Subject;
                    dmolist.ASCOMP_FileName = data.ASCOMP_FileName;
                    dmolist.ASCOMP_FilePath = data.ASCOMP_FilePath;
                    dmolist.AMST_Id = data.AMST_Id;
                    dmolist.ASCOMP_UpdatedBy = data.UserId;
                    dmolist.ASCOMP_UpdatedDate = DateTime.Now;
                    dmolist.ASCOMP_Date = data.ASCOMP_Date;
                    _context.Update(dmolist);
                    int dd = _context.SaveChanges();
                    if (dd > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "notupdated";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentCompliants_DTO getstudentdetails(StudentCompliants_DTO data)
        {
            try
            {
                data.studentinfolist = (from a in _context.Adm_M_Student
                                        from b in _context.SchoolYearWiseStudent
                                        from c in _context.StudentCompliants_DMO
                                        where (a.AMST_Id == b.AMST_Id && b.AMST_Id == data.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == c.MI_Id
                                        && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                        select c).Distinct().OrderByDescending(a => a.ASCOMP_Date).ToArray();


                data.studentdivlist = (from a in _context.Adm_M_Student
                                       from b in _context.SchoolYearWiseStudent
                                       from c in _context.School_M_Class
                                       from d in _context.AdmSection
                                       from e in _context.AcademicYear
                                       where (a.AMST_Id == b.AMST_Id && b.AMST_Id == data.AMST_Id && b.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id
                                       && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == e.ASMAY_Id && e.Is_Active == true
                                       && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1
                                       && a.AMST_SOL == "S")
                                       select new StudentCompliants_DTO
                                       {
                                           AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName)
                                    + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName)
                                                          + (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),

                                           AMST_Id = b.AMST_Id,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMAY_RollNo = b.AMAY_RollNo,
                                           ASMAY_Id = b.ASMAY_Id,
                                           ASMCL_Id = b.ASMCL_Id,
                                           ASMS_Id = b.ASMS_Id,
                                           ASMAY_Year = e.ASMAY_Year,
                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                           ASMC_SectionName = d.ASMC_SectionName,

                                       }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentCompliants_DTO edittab1(StudentCompliants_DTO data)
        {
            try
            {
                data.editlist = _context.StudentCompliants_DMO.Where(a => a.MI_Id == data.MI_Id && a.ASCOMP_Id == data.ASCOMP_Id).ToArray();               
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentCompliants_DTO getorganizationdata(StudentCompliants_DTO data)
        {
            try
            {
                data.viewlist = _context.StudentCompliants_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentCompliants_DTO deactive(StudentCompliants_DTO data)
        {
            try
            {
                var deactivate = _context.StudentCompliants_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCOMP_Id == data.ASCOMP_Id).SingleOrDefault();
                if (deactivate.ASCOMP_ActiveFlg == true)
                {
                    deactivate.ASCOMP_ActiveFlg = false;
                }
                else
                {
                    deactivate.ASCOMP_ActiveFlg = true;
                }
                deactivate.ASCOMP_UpdatedBy = data.UserId;
                deactivate.ASCOMP_UpdatedDate = DateTime.Now;
                _context.Update(deactivate);
                int r = _context.SaveChanges();
                if (r > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentCompliants_DTO searchfilter(StudentCompliants_DTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();

                data.studentlist = (from a in _context.Adm_M_Student
                                    from b in _context.SchoolYearWiseStudent
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals("S")
                                    && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1
                                    && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' '
                                    + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) ||
                                    a.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) ||
                                    a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) ||
                                    a.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                    select new StudentCompliants_DTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? ""
                                        : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                                        ASMAY_Id = b.ASMAY_Id
                                    }).OrderBy(a => a.AMST_FirstName).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentCompliants_DTO report(StudentCompliants_DTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";



                fromdatecon = Convert.ToDateTime(data.FromDate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.ToDate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Student_Compliants_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                    {
                        Value = contodate
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public StudentCompliants_DTO deleterecY(StudentCompliants_DTO data)
        {
            try
            {

                var dmolist = _context.StudentCompliants_DMO.Where(t => t.ASCOMP_Id == data.ASCOMP_Id).SingleOrDefault();
                if (dmolist.ASCOMP_ActiveFlg == true)
                {
                    dmolist.ASCOMP_ActiveFlg = false;
                }
                else
                {
                    dmolist.ASCOMP_ActiveFlg = true;
                }
                dmolist.ASCOMP_UpdatedBy = data.UserId;
                dmolist.ASCOMP_UpdatedDate = DateTime.Now;
                _context.Update(dmolist);
                int dd = _context.SaveChanges();
                if (dd > 0)
                {
                    data.msg = "updated";
                }
                else
                {
                    data.msg = "notupdated";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StudentCompliants_DTO editdetails(StudentCompliants_DTO dto)
        {
            try
            {

                dto.studentlist = (from a in _context.Adm_M_Student
                                   from b in _context.SchoolYearWiseStudent
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_SOL.Equals("S")
                                   && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                   select new StudentCompliants_DTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? ""
                                       : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                                       ASMAY_Id = b.ASMAY_Id
                                   }).OrderBy(a => a.AMST_FirstName).ToArray();

                dto.editdata = (from a in _context.AcademicYear
                                from b in _context.SchoolYearWiseStudent
                                from c in _context.Adm_M_Student
                                from d in _context.School_M_Class
                                from e in _context.AdmSection
                                from f in _context.StudentCompliants_DMO
                                where (a.ASMAY_Id == b.ASMAY_Id && b.AMST_Id == c.AMST_Id && b.ASMCL_Id == d.ASMCL_Id &&
                                b.ASMS_Id == e.ASMS_Id && b.AMST_Id == f.AMST_Id &&
                                f.ASCOMP_Id == dto.ASCOMP_Id && f.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                select new StudentCompliants_DTO
                                {
                                    AMST_Id = c.AMST_Id,
                                    ASMAY_Id = a.ASMAY_Id,
                                    ASMCL_Id = d.ASMCL_Id,
                                    ASMS_Id = e.ASMS_Id,
                                    ASCOMP_Id = f.ASCOMP_Id,
                                    ASCOMP_Subject = f.ASCOMP_Subject,
                                    ASCOMP_FilePath = f.ASCOMP_FilePath,
                                    ASCOMP_Date = f.ASCOMP_Date,
                                    ASCOMP_Complaints = f.ASCOMP_Complaints,
                                    ASCOMP_FileName = f.ASCOMP_FileName,
                                    AMST_FirstName = (c.AMST_FirstName + c.AMST_MiddleName + c.AMST_LastName),
                                    AMST_AdmNo = c.AMST_AdmNo,
                                    ASMCL_ClassName = d.ASMCL_ClassName
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