using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Portals.Employee;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeStudentHomeworkImpl : Interfaces.EmployeeStudentHomeworkInterface
    {

        public PortalContext _PrincipalDashboardContext;
        public ExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        public EmployeeStudentHomeworkImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, ExamContext exm)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _examcontext = exm;
        }
        public IVRM_Homework_DTO Getdetails(IVRM_Homework_DTO data)
        {
            try
            {
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_homeworkclasswork_MarksUpdate_list_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = data.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Homework" });
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
                        data.marksupdate_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.academicyear = _db.AcademicYear.Where(a => a.ASMAY_Id == data.ASMAY_Id).ToArray();


                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = _examcontext.Staff_User_Login.Single(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).Emp_Code;

                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_StaffwiseClassStdata";

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                            data.classlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    data.HRME_Id = 0;

                    data.classlist = (from a in _PrincipalDashboardContext.School_M_Class
                                      from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                      from c in _PrincipalDashboardContext.AcademicYearDMO
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true)
                                      select new IVRM_ClassWorkDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,

                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                data.yearlist = _PrincipalDashboardContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_ActiveFlag == 1).OrderByDescending(t => t.ASMAY_Order).ToArray();


                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_ClassHomeworkGrid_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = data.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "Homework" });

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
                        data.classwork = retObject.ToArray();
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
        public async Task<IVRM_Homework_DTO> get_classes(IVRM_Homework_DTO data)
        {
            try
            {

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        // cmd.CommandText = "IVRM_StaffwiseClassStdata";
                        cmd.CommandText = "IVRM_homeworkclasswork_list";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = data.Login_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Homework" });
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
                            data.class_gridlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_StaffwiseClassStdata";

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                            data.classlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else
                {
                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "IVRM_homeworkclasswork_list";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = data.Login_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Homework" });
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
                            data.class_gridlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.classlist = (from a in _PrincipalDashboardContext.School_M_Class
                                      from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                      from c in _PrincipalDashboardContext.AcademicYearDMO
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true)
                                      select new IVRM_ClassWorkDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,

                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_Homework_DTO getsectiondata(IVRM_Homework_DTO data)
        {
            try
            {

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                    var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();
                    var sectionexamid = (from e in _examcontext.Staff_User_Login
                                         from f in _examcontext.Exm_Login_PrivilegeDMO
                                         from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                         where (e.Id == data.Login_Id &&
                                           f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMCL_Id == data.ASMCL_Id && secid.Contains(i.ASMS_Id)
                                           && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true && i.ASMCL_Id == data.ASMCL_Id)
                                         select new IVRM_Homework_DTO
                                         {
                                             ASMS_Id = i.ASMS_Id
                                         }).Distinct().Select(t => t.ASMS_Id).ToArray();

                    List<School_M_Section> seclist = new List<School_M_Section>();
                    seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
                    data.sectionlist = seclist.Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                }

                else
                {
                    data.sectionlist = (from a in _PrincipalDashboardContext.School_M_Section
                                        from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                        from c in _PrincipalDashboardContext.AcademicYearDMO
                                        from e in _PrincipalDashboardContext.School_M_Class
                                        where (a.MI_Id == c.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMC_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id)
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = a.ASMC_SectionName,
                                            ASMCL_ClassName = e.ASMCL_ClassName,
                                            ASMC_Order = a.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_Homework_DTO getsubject(IVRM_Homework_DTO data)
        {
            try
            {
                List<long> list1 = new List<long>();
                foreach (var item in data.hm_section_list)
                {
                    list1.Add(item.ASMS_Id);
                }
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = _examcontext.Staff_User_Login.Single(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).Emp_Code;


                    data.subjectlist = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                        from f in _PrincipalDashboardContext.School_M_Section
                                        from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                        from h in _PrincipalDashboardContext.Staff_User_Login
                                        from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                        from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                        from k in _PrincipalDashboardContext.School_M_Class
                                        where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                                        && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && j.ASMAY_Id == data.ASMAY_Id && g.MI_Id == data.MI_Id && list1.Contains(d.ASMS_Id) && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true && i.HRME_LeftFlag == false && h.Emp_Code == data.HRME_Id && d.ASMCL_Id == data.ASMCL_Id)
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ISMS_Id = g.ISMS_Id,
                                            ISMS_SubjectName = g.ISMS_SubjectName,
                                            //ASMC_SectionName = f.ASMC_SectionName,
                                            ISMS_SubjectCode = g.ISMS_SubjectCode,
                                            ISMS_order = g.ISMS_OrderFlag

                                        }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                }
                else
                {
                    data.subjectlist = (from d in _PrincipalDashboardContext.Exm_Yearly_CategoryDMO
                                        from e in _PrincipalDashboardContext.Exm_Yearly_Category_GroupDMO
                                        from h in _PrincipalDashboardContext.Exm_Yearly_Category_Group_SubjectsDMO
                                        from i in _PrincipalDashboardContext.Exm_Category_ClassDMO
                                        from f in _PrincipalDashboardContext.School_M_Section
                                        from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                        from k in _PrincipalDashboardContext.School_M_Class
                                        where (d.EYC_Id == e.EYC_Id && e.EYCG_Id == h.EYCG_Id && d.EMCA_Id == i.EMCA_Id && i.ASMCL_Id == k.ASMCL_Id
                                        && i.ASMS_Id == f.ASMS_Id && h.ISMS_Id == g.ISMS_Id && d.EYC_ActiveFlg == true && e.EYCG_ActiveFlg == true
                                        && h.EYCGS_ActiveFlg == true && i.ECAC_ActiveFlag == true && d.ASMAY_Id == data.ASMAY_Id && i.ASMAY_Id == data.ASMAY_Id
                                        && i.ASMCL_Id == data.ASMCL_Id && list1.Contains(i.ASMS_Id) && g.MI_Id == data.MI_Id)
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ISMS_Id = g.ISMS_Id,
                                            ISMS_SubjectName = g.ISMS_SubjectName,
                                            //ASMC_SectionName = f.ASMC_SectionName,
                                            ISMS_SubjectCode = g.ISMS_SubjectCode,
                                            ISMS_order = g.ISMS_OrderFlag
                                        }).Distinct().OrderBy(t => t.ISMS_order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_Homework_DTO savedetail(IVRM_Homework_DTO data)
        {
            try
            {
                var header_flg = "HomeWork";
                var chekemployee = _examcontext.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                if (chekemployee.Count > 0)
                {
                    var hrmeid = _PrincipalDashboardContext.Staff_User_Login.Single(a => a.Id == data.UserId);

                    if (data.IHW_Id > 0)
                    {
                        //var Duplicate = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.IHW_Id != data.IHW_Id && t.IHW_Topic == data.IHW_Topic && t.IHW_Assignment == data.IHW_Assignment && t.ISMS_Id == data.ISMS_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IHW_Date == data.IHW_Date).ToList();

                        //if (Duplicate.Count > 0)
                        //{
                        //    data.dulicate = true;
                        //}
                        //else
                        //{
                            var update = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.IHW_Id == data.IHW_Id).SingleOrDefault();

                            foreach (var item in data.hm_section_list)
                            {
                                var sub = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                           from f in _PrincipalDashboardContext.School_M_Section
                                           from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                           from h in _PrincipalDashboardContext.Staff_User_Login
                                           from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                           from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                           from k in _PrincipalDashboardContext.School_M_Class
                                           where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                                           && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && j.ASMAY_Id == data.ASMAY_Id && g.MI_Id == data.MI_Id && d.ASMS_Id == item.ASMS_Id && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true && i.HRME_LeftFlag == false && h.Emp_Code == hrmeid.Emp_Code && d.ASMCL_Id == data.ASMCL_Id)
                                           select new IVRM_ClassWorkDTO
                                           {
                                               ISMS_Id = g.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ASMC_SectionName = f.ASMC_SectionName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               ISMS_order = g.ISMS_OrderFlag

                                           }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                                if (sub.Count() > 0)
                                {
                                    foreach (var item1 in sub)
                                    {
                                        if (item1.ISMS_Id == data.ISMS_Id)
                                        {

                                            update.ASMAY_Id = data.ASMAY_Id;
                                            update.IHW_AssignmentNo = data.IHW_AssignmentNo;
                                            update.ISMS_Id = data.ISMS_Id;
                                            update.ASMCL_Id = data.ASMCL_Id;
                                            update.ASMS_Id = item.ASMS_Id;
                                            update.IHW_Date = data.IHW_Date;
                                            update.IHW_Topic = data.IHW_Topic;
                                            update.IHW_Assignment = data.IHW_Assignment;
                                            update.IHW_Attachment = data.IHW_Attachment;
                                            update.UpdatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Update(update);




                                        }
                                    }
                                    if (data.IHW_FilePath_Array != null && data.IHW_FilePath_Array.Length>0)
                                    {
                                        var result = _PrincipalDashboardContext.IVRM_HomeWork_Attatchment_DMO_con.Where(a => a.IHW_Id == data.IHW_Id).ToList();
                                        if (result.Count > 0)
                                        {

                                            foreach (var e in result)
                                            {
                                                var result1 = _PrincipalDashboardContext.IVRM_HomeWork_Attatchment_DMO_con.Single(a => a.IHW_Id == data.IHW_Id && a.IHWATT_Id == e.IHWATT_Id);
                                                _PrincipalDashboardContext.Remove(result1);
                                            }
                                        }



                                        foreach (var itm in data.IHW_FilePath_Array)
                                        {
                                            IVRM_HomeWork_Attatchment_DMO had = new IVRM_HomeWork_Attatchment_DMO();

                                            had.IHW_Id = data.IHW_Id;
                                            had.IHWATT_Attachment = itm.IHW_FilePath;
                                            had.IHWATT_FileName = itm.FileName;
                                            had.IHWATT_ActiveFlag = true;
                                            had.CreatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Add(had);
                                        }
                                    }
                                }
                            }
                        List<long> amst = new List<long>();
                        foreach (var item1 in data.hm_section_list)
                        {
                            amst.Add(item1.ASMS_Id);
                        }
                        int rowAffected = _PrincipalDashboardContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                            //=========================================== Push Notification
                            var hm = _PrincipalDashboardContext.IVRM_Homework_DMO.ToList();

                            var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                              from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                              where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                              select new IVRM_Homework_DTO
                                              {
                                                  AMST_MobileNo = a.AMST_MobileNo,
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                              }).Distinct().ToList();
                            IVRM_Homework_DMO dd = new IVRM_Homework_DMO();
                            dd.IHW_Id = hm.LastOrDefault().IHW_Id;
                            var hw_id = (from a in _PrincipalDashboardContext.IVRM_Homework_DMO
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.IHW_ActiveFlag == true)
                                         select new IVRM_Homework_DTO
                                         {
                                             IHW_Id = a.IHW_Id
                                         }).OrderByDescending(r => r.IHW_Id).FirstOrDefault();
                            var homeid = hw_id.IHW_Id;
                            IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                            dto.devicelist12 = devicelist;
                            data.deviceArray = devicelist.ToArray();
                            var deviceidsnew = "";
                            var devicenew = "";
                            var redirecturl = "";
                            long revieveduserid = 0;

                            if (devicelist.Count > 0)
                            {
                                foreach (var device_id in devicelist)
                                {
                                    if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                    {


                                        revieveduserid = (from a in _PrincipalDashboardContext.StudentUserLoginDMO
                                                          from b in _PrincipalDashboardContext.ApplicationUser
                                                          where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                          select b).Select(t => t.Id).FirstOrDefault();



                                        PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);
                                        push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, homeid, data.IHW_Topic, "Homework", "Homework");

                                    }
                                }
                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        //var Duplicate = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_Id == data.ISMS_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IHW_Date == data.IHW_Date).ToList();

                        //if (Duplicate.Count > 0)
                        //{
                        //    data.dulicate = true;
                        //}
                        //else
                        //{
                        foreach (var item in data.hm_section_list)
                        {
                            var sub = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                       from f in _PrincipalDashboardContext.School_M_Section
                                       from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                       from h in _PrincipalDashboardContext.Staff_User_Login
                                       from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                       from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                       from k in _PrincipalDashboardContext.School_M_Class
                                       where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                                       && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && j.ASMAY_Id == data.ASMAY_Id && g.MI_Id == data.MI_Id && d.ASMS_Id == item.ASMS_Id && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true && i.HRME_LeftFlag == false && h.Emp_Code == hrmeid.Emp_Code && d.ASMCL_Id == data.ASMCL_Id)
                                       select new IVRM_ClassWorkDTO
                                       {
                                           ISMS_Id = g.ISMS_Id,
                                           ISMS_SubjectName = g.ISMS_SubjectName,
                                           ASMC_SectionName = f.ASMC_SectionName,
                                           ISMS_SubjectCode = g.ISMS_SubjectCode,
                                           ISMS_order = g.ISMS_OrderFlag

                                       }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                            if (sub.Count() > 0)
                            {
                                foreach (var item1 in sub)
                                {
                                    if (item1.ISMS_Id == data.ISMS_Id)
                                    {

                                        IVRM_Homework_DMO obj = new IVRM_Homework_DMO();
                                        obj.IHW_Id = data.IHW_Id;
                                        obj.MI_Id = data.MI_Id;
                                        obj.ASMAY_Id = data.ASMAY_Id;
                                        obj.IHW_AssignmentNo = data.IHW_AssignmentNo;
                                        obj.ISMS_Id = data.ISMS_Id;
                                        obj.IVRMUL_Id = data.Login_Id;
                                        obj.ASMCL_Id = data.ASMCL_Id;
                                        obj.ASMS_Id = item.ASMS_Id;
                                        obj.IHW_Date = data.IHW_Date;
                                        obj.IHW_Topic = data.IHW_Topic;
                                        obj.IHW_Assignment = data.IHW_Assignment;
                                        obj.IHW_Attachment = data.IHW_Attachment;

                                        obj.IHW_ActiveFlag = true;
                                        obj.CreatedDate = DateTime.Now;
                                        obj.UpdatedDate = DateTime.Now;
                                        _PrincipalDashboardContext.Add(obj);
                                        if (data.IHW_FilePath_Array != null && data.IHW_FilePath_Array.Length>0)
                                        {
                                            foreach (var itm in data.IHW_FilePath_Array)
                                            {
                                                IVRM_HomeWork_Attatchment_DMO had = new IVRM_HomeWork_Attatchment_DMO();

                                                had.IHW_Id = obj.IHW_Id;
                                                had.IHWATT_Attachment = itm.IHW_FilePath;
                                                had.IHWATT_FileName = itm.FileName;
                                                had.IHWATT_ActiveFlag = true;
                                                had.CreatedDate = DateTime.Now;
                                                _PrincipalDashboardContext.Add(had);
                                            }
                                        }


                                    }
                                }
                            }

                        }
                        List<long> amst = new List<long>();
                        foreach (var item1 in data.hm_section_list)
                        {
                            amst.Add(item1.ASMS_Id);
                        }
                        int rowAffected = _PrincipalDashboardContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                            //=========================================== Push Notification
                            var hm = _PrincipalDashboardContext.IVRM_Homework_DMO.ToList();
                            //var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                            //                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                            //                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == 150168895)
                            //                  select new IVRM_Homework_DTO
                            //                  {
                            //                      AMST_MobileNo = a.AMST_MobileNo,
                            //                      AMST_Id = a.AMST_Id,
                            //                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                            //                  }).Distinct().ToList();

                            var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                              from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                              where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id )
                                              select new IVRM_Homework_DTO
                                              {
                                                  AMST_MobileNo = a.AMST_MobileNo,
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                              }).Distinct().ToList();
                            IVRM_Homework_DMO dd = new IVRM_Homework_DMO();
                            dd.IHW_Id = hm.LastOrDefault().IHW_Id;
                            // var homeid = dd.IHW_Id;
                            // var homeid= hm.LastOrDefault().IHW_Id;
                            var hw_id = (from a in _PrincipalDashboardContext.IVRM_Homework_DMO
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.IHW_ActiveFlag == true)
                                         select new IVRM_Homework_DTO
                                         {
                                             IHW_Id = a.IHW_Id
                                         }).OrderByDescending(r => r.IHW_Id).FirstOrDefault();
                            var homeid = hw_id.IHW_Id;
                            IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                            dto.devicelist12 = devicelist;
                            data.deviceArray = devicelist.ToArray();



                            var deviceidsnew = "";
                            var devicenew = "";
                            var redirecturl = "";
                            long revieveduserid = 0;

                            if (devicelist.Count > 0)
                            {
                                foreach (var device_id in devicelist)
                                {
                                    if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId !="")
                                    {


                                        revieveduserid = (from a in _PrincipalDashboardContext.StudentUserLoginDMO
                                                          from b in _PrincipalDashboardContext.ApplicationUser
                                                          where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                          select b).Select(t => t.Id).FirstOrDefault();



                                        PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);
                                        push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, homeid, data.IHW_Topic, "Homework", "Homework");

                                    }
                                }
                            }

                            //var deviceidsnew = "";
                            //var devicenew = "";
                            //if (devicelist.Count > 0)
                            //{
                            //    int k = 0;
                            //    foreach (var deviceid in devicelist)
                            //    {
                            //        if (k == 0)
                            //        {
                            //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            //            k = 1;
                            //        }
                            //        else
                            //        {
                            //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            //        }

                            //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                            //        {
                            //            callnotification(deviceid.AMST_AppDownloadedDeviceId, homeid, data.MI_Id, dto, header_flg);
                            //        }
                            //    }
                            //    devicenew = "[" + deviceidsnew + "]";                               
                            //}
                        }
                        else
                        {
                            data.returnval = false;
                        }
                        //}
                    }
                }

                else
                {
                    if (data.IHW_Id > 0)
                    {
                        //var Duplicate = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.IHW_Id != data.IHW_Id && t.IHW_Topic == data.IHW_Topic && t.IHW_Assignment == data.IHW_Assignment && t.ISMS_Id == data.ISMS_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IHW_Date == data.IHW_Date).ToList();

                        //if (Duplicate.Count > 0)
                        //{
                        //    data.dulicate = true;
                        //}
                        //else
                        //{
                            var update = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.IHW_Id == data.IHW_Id).SingleOrDefault();

                            foreach (var item in data.hm_section_list)
                            {
                                var sub = (from d in _PrincipalDashboardContext.Exm_Yearly_CategoryDMO
                                           from e in _PrincipalDashboardContext.Exm_Yearly_Category_GroupDMO
                                           from h in _PrincipalDashboardContext.Exm_Yearly_Category_Group_SubjectsDMO
                                           from i in _PrincipalDashboardContext.Exm_Category_ClassDMO
                                           from f in _PrincipalDashboardContext.School_M_Section
                                           from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                           from k in _PrincipalDashboardContext.School_M_Class
                                           where (d.EYC_Id == e.EYC_Id && e.EYCG_Id == h.EYCG_Id && d.EMCA_Id == i.EMCA_Id && i.ASMCL_Id == k.ASMCL_Id
                                           && i.ASMS_Id == f.ASMS_Id && h.ISMS_Id == g.ISMS_Id && d.EYC_ActiveFlg == true && e.EYCG_ActiveFlg == true
                                           && h.EYCGS_ActiveFlg == true && i.ECAC_ActiveFlag == true && d.ASMAY_Id == data.ASMAY_Id && i.ASMAY_Id == data.ASMAY_Id
                                           && i.ASMCL_Id == data.ASMCL_Id && i.ASMS_Id == item.ASMS_Id && g.MI_Id == data.MI_Id)
                                           select new IVRM_ClassWorkDTO
                                           {
                                               ISMS_Id = g.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ASMC_SectionName = f.ASMC_SectionName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               ISMS_order = g.ISMS_OrderFlag
                                           }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                                if (sub.Count() > 0)
                                {
                                    foreach (var item1 in sub)
                                    {
                                        if (item1.ISMS_Id == data.ISMS_Id)
                                        {
                                            update.ASMAY_Id = data.ASMAY_Id;
                                            update.IHW_AssignmentNo = data.IHW_AssignmentNo;
                                            update.ISMS_Id = data.ISMS_Id;
                                            update.ASMCL_Id = data.ASMCL_Id;
                                            update.ASMS_Id = item.ASMS_Id;
                                            update.IHW_Date = data.IHW_Date;
                                            update.IHW_Topic = data.IHW_Topic;
                                            update.IHW_Assignment = data.IHW_Assignment;
                                            update.IHW_Attachment = data.IHW_Attachment;
                                            update.UpdatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Update(update);



                                        }
                                    }
                                    if (data.IHW_FilePath_Array != null && data.IHW_FilePath_Array.Length>0)
                                    {
                                        var result = _PrincipalDashboardContext.IVRM_HomeWork_Attatchment_DMO_con.Where(a => a.IHW_Id == data.IHW_Id).ToList();
                                        if (result.Count > 0)
                                        {
                                            foreach (var res in result)
                                            {

                                                var result1 = _PrincipalDashboardContext.IVRM_HomeWork_Attatchment_DMO_con.Single(a => a.IHW_Id == data.IHW_Id && a.IHWATT_Id == res.IHWATT_Id);
                                                _PrincipalDashboardContext.Remove(result1);
                                            }
                                        }


                                        foreach (var itm in data.IHW_FilePath_Array)
                                        {
                                            IVRM_HomeWork_Attatchment_DMO had = new IVRM_HomeWork_Attatchment_DMO();

                                            had.IHW_Id = data.IHW_Id;
                                            had.IHWATT_Attachment = itm.IHW_FilePath;
                                            had.IHWATT_FileName = itm.FileName;
                                            had.IHWATT_ActiveFlag = true;
                                            had.CreatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Add(had);
                                        }
                                    }
                                }
                            }
                            List<long> amst = new List<long>();
                            foreach (var item1 in data.hm_section_list)
                            {
                                amst.Add(item1.ASMS_Id);
                            }
                            int rowAffected = _PrincipalDashboardContext.SaveChanges();
                            if (rowAffected > 0)
                            {
                                data.returnval = true;
                                //=========================================== Push Notification
                                var hm = _PrincipalDashboardContext.IVRM_Homework_DMO.ToList();
                                //var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                //                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                //                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id== 150168895)
                                //                  select new IVRM_Homework_DTO
                                //                  {
                                //                      AMST_MobileNo = a.AMST_MobileNo,
                                //                      AMST_Id = a.AMST_Id,
                                //                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                //                  }).Distinct().ToList();

                                //var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                //                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                //                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == 2 && b.ASMS_Id == 1 && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == 150168895)
                                //                  select new IVRM_Homework_DTO
                                //                  {
                                //                      AMST_MobileNo = a.AMST_MobileNo,
                                //                      AMST_Id = a.AMST_Id,
                                //                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                //                  }).Distinct().ToList();

                                var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id )
                                                  select new IVRM_Homework_DTO
                                                  {
                                                      AMST_MobileNo = a.AMST_MobileNo,
                                                      AMST_Id = a.AMST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();
                                IVRM_Homework_DMO dd = new IVRM_Homework_DMO();
                                dd.IHW_Id = hm.LastOrDefault().IHW_Id;
                                //var homeid = dd.IHW_Id;
                                // var homeid = hm.LastOrDefault().IHW_Id;

                                var hw_id = (from a in _PrincipalDashboardContext.IVRM_Homework_DMO
                                             where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.IHW_ActiveFlag == true)
                                             select new IVRM_Homework_DTO
                                             {
                                                 IHW_Id = a.IHW_Id
                                             }).OrderByDescending(r => r.IHW_Id).FirstOrDefault();
                                var homeid = hw_id.IHW_Id;
                                IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                dto.devicelist12 = devicelist;
                                data.deviceArray = devicelist.ToArray();

                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                        if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                        {


                                            revieveduserid = (from a in _PrincipalDashboardContext.StudentUserLoginDMO
                                                              from b in _PrincipalDashboardContext.ApplicationUser
                                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, homeid, data.IHW_Topic, "Homework", "Homework");

                                        }
                                    }
                                }
                                //var deviceidsnew = "";
                                //var devicenew = "";
                                //if (devicelist.Count > 0)
                                //{
                                //    int k = 0;
                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }
                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, homeid, data.MI_Id, dto, header_flg);
                                //        }
                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";                                    
                                //}

                            }
                            else
                            {
                                data.returnval = false;
                            }
                        //}
                    }
                    else
                    {
                        var Duplicate = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_Id == data.ISMS_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IHW_Date == data.IHW_Date).ToList();

                        foreach (var item in data.hm_section_list)
                        {
                            var sub = (from d in _PrincipalDashboardContext.Exm_Yearly_CategoryDMO
                                       from e in _PrincipalDashboardContext.Exm_Yearly_Category_GroupDMO
                                       from h in _PrincipalDashboardContext.Exm_Yearly_Category_Group_SubjectsDMO
                                       from i in _PrincipalDashboardContext.Exm_Category_ClassDMO
                                       from f in _PrincipalDashboardContext.School_M_Section
                                       from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                       from k in _PrincipalDashboardContext.School_M_Class
                                       where (d.EYC_Id == e.EYC_Id && e.EYCG_Id == h.EYCG_Id && d.EMCA_Id == i.EMCA_Id && i.ASMCL_Id == k.ASMCL_Id
                                       && i.ASMS_Id == f.ASMS_Id && h.ISMS_Id == g.ISMS_Id && d.EYC_ActiveFlg == true && e.EYCG_ActiveFlg == true
                                       && h.EYCGS_ActiveFlg == true && i.ECAC_ActiveFlag == true && d.ASMAY_Id == data.ASMAY_Id && i.ASMAY_Id == data.ASMAY_Id
                                       && i.ASMCL_Id == data.ASMCL_Id && i.ASMS_Id == item.ASMS_Id && g.MI_Id == data.MI_Id)
                                       select new IVRM_ClassWorkDTO
                                       {
                                           ISMS_Id = g.ISMS_Id,
                                           ISMS_SubjectName = g.ISMS_SubjectName,
                                           ASMC_SectionName = f.ASMC_SectionName,
                                           ISMS_SubjectCode = g.ISMS_SubjectCode,
                                           ISMS_order = g.ISMS_OrderFlag
                                       }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                            if (sub.Count() > 0)
                            {
                                foreach (var item1 in sub)
                                {
                                    if (item1.ISMS_Id == data.ISMS_Id)
                                    {

                                        IVRM_Homework_DMO obj = new IVRM_Homework_DMO();
                                        obj.IHW_Id = data.IHW_Id;
                                        obj.MI_Id = data.MI_Id;
                                        obj.ASMAY_Id = data.ASMAY_Id;
                                        obj.IHW_AssignmentNo = data.IHW_AssignmentNo;
                                        obj.ISMS_Id = data.ISMS_Id;
                                        obj.IVRMUL_Id = data.Login_Id;
                                        obj.ASMCL_Id = data.ASMCL_Id;
                                        obj.ASMS_Id = item.ASMS_Id;
                                        obj.IHW_Date = data.IHW_Date;
                                        obj.IHW_Topic = data.IHW_Topic;
                                        obj.IHW_Assignment = data.IHW_Assignment;
                                        obj.IHW_Attachment = data.IHW_Attachment;
                                        obj.IHW_FilePath = data.IHW_FilePath;
                                        obj.IHW_ActiveFlag = true;
                                        obj.CreatedDate = DateTime.Now;
                                        obj.UpdatedDate = DateTime.Now;
                                        _PrincipalDashboardContext.Add(obj);

                                        if (data.IHW_FilePath_Array != null && data.IHW_FilePath_Array.Length>0)
                                        {
                                            foreach (var itm in data.IHW_FilePath_Array)
                                            {
                                                IVRM_HomeWork_Attatchment_DMO had = new IVRM_HomeWork_Attatchment_DMO();

                                                had.IHW_Id = obj.IHW_Id;
                                                had.IHWATT_Attachment = itm.IHW_FilePath;
                                                had.IHWATT_FileName = itm.FileName;
                                                had.IHWATT_ActiveFlag = true;
                                                had.CreatedDate = DateTime.Now;
                                                _PrincipalDashboardContext.Add(had);
                                            }
                                        }

                                    }
                                }
                            }

                        }
                        List<long> amst = new List<long>();
                        foreach (var item1 in data.hm_section_list)
                        {
                            amst.Add(item1.ASMS_Id);
                        }
                        int rowAffected = _PrincipalDashboardContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                            //=========================================== Push Notification
                            var hm = _PrincipalDashboardContext.IVRM_Homework_DMO.ToList();
                            //var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                            //                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                            //                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == 150168895)
                            //                  select new IVRM_Homework_DTO
                            //                  {
                            //                      AMST_MobileNo = a.AMST_MobileNo,
                            //                      AMST_Id = a.AMST_Id,
                            //                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                            //                  }).Distinct().ToList();

                            var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                              from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                              where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id )
                                              select new IVRM_Homework_DTO
                                              {
                                                  AMST_MobileNo = a.AMST_MobileNo,
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                              }).Distinct().ToList();
                            IVRM_Homework_DMO dd = new IVRM_Homework_DMO();
                            dd.IHW_Id = hm.LastOrDefault().IHW_Id;
                            // var homeid = dd.IHW_Id;

                            // var homeid = hm.LastOrDefault().IHW_Id;
                            var hw_id = (from a in _PrincipalDashboardContext.IVRM_Homework_DMO
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.IHW_ActiveFlag == true)
                                         select new IVRM_Homework_DTO
                                         {
                                             IHW_Id = a.IHW_Id
                                         }).OrderByDescending(r => r.IHW_Id).FirstOrDefault();
                            var homeid = hw_id.IHW_Id;
                            IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                            dto.devicelist12 = devicelist;
                            data.deviceArray = devicelist.ToArray();
                            var deviceidsnew = "";
                            var devicenew = "";
                            var redirecturl = "";
                            long revieveduserid = 0;

                            if (devicelist.Count > 0)
                            {
                                foreach (var device_id in devicelist)
                                {
                                    if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                    {


                                        revieveduserid = (from a in _PrincipalDashboardContext.StudentUserLoginDMO
                                                          from b in _PrincipalDashboardContext.ApplicationUser
                                                          where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                          select b).Select(t => t.Id).FirstOrDefault();



                                        PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);
                                        push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, homeid, data.IHW_Topic, "Homework", "Homework");

                                    }
                                }
                            }
                            //var deviceidsnew = "";
                            //var devicenew = "";
                            //if (devicelist.Count > 0)
                            //{
                            //    int k = 0;
                            //    foreach (var deviceid in devicelist)
                            //    {
                            //        if (k == 0)
                            //        {
                            //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            //            k = 1;
                            //        }
                            //        else
                            //        {
                            //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            //        }

                            //        if(deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null )
                            //        {
                            //            callnotification(deviceid.AMST_AppDownloadedDeviceId, homeid, data.MI_Id, dto, header_flg);
                            //        }

                            //    }
                            //    devicenew = "[" + deviceidsnew + "]";

                            //}

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
            }
            return data;
        }

        //end
        public string callnotification(string devicenew, long ihw_Id, long mi_id, IVRM_Homework_DTO dto, string header_flg)
        {
            try
            {
                var key = _PrincipalDashboardContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;
                IVRM_Homework_DTO data = new IVRM_Homework_DTO();
                var homework = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(h => h.MI_Id == mi_id && h.IHW_ActiveFlag == true && h.IHW_Id == ihw_Id).Distinct().ToList();
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                // devicenew = "fWUOo9LZQI6RalksnM1_6E:APA91bFDemb9Fo0_O5ATF_95Zr0OKYKbw828EGqun1nVK8w2lqpZq4e3b_SWlUtQ35FhB_XEKYiUhxhM8rJkhaV0ACmrIUuuJ_tmUSHtnkAg6dMHE4rSbgP5ChYfBJY6GyuWp6KmFh3l";
              //  devicenew = "ftDWn6YET0-U_W3DxP0pPx:APA91bFi4bf9CqC4mZbXyzapTykkkgvfgHGv6uBENF1ylEkTHKbaVkpR9Uv-i8i4V40te-mT7_qco76K4DDKk8LOHZM8WIu9yqMgbWF0VLm-3_5f-s9gLI5djIcSbpZkYydavl1g12Rr";
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + homework.FirstOrDefault().IHW_Topic + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + homework.FirstOrDefault().IHW_Assignment + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                string sound = "default";
                string notId = "3";
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Homework" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + homework.FirstOrDefault().IHW_Topic + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";
                // daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //"" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Homework" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + homework.FirstOrDefault().IHW_Topic + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } " + "," + '"' + "data" + '"' + ":" + "{" + '"' + "page" + '"' + ":" + '"' + "homework" + '"' + "}" + "}";


                //daata = "{" + "to" +  ":" + devicenew + "," + "notification" +  " : " + "{"  + "body" + ":" + '"' + homework.FirstOrDefault().IHW_Topic + '"' + '"' + "title" + '"' + ":" + '"' + "Homework" + '"'+ " } "  + "}";


                Dictionary<string, object> input = new Dictionary<string, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("body", homework.FirstOrDefault().IHW_Topic);
                transfersnotes.Add("title", "Homework");

                input.Add("to", devicenew);
                input.Add("notification", transfersnotes);


                var myContent = JsonConvert.SerializeObject(input);
                String postdata = myContent;




                //               {
                //                   "to" : "FCM_TOKEN_OR_TOPIC_WILL_BE_HERE",
                //"notification" : {
                //                       "body" : "Body of Your Notification",
                //    "title": "Title of Your Notification"
                //}
                //               }


                //notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                //var mycontent = notificationparams[0];
                //string postdata = mycontent.ToString();
               // key = "AAAAfjt2mMY:APA91bEH56luie3txuirafAEWkGqIZtu9aK9adQq26jXyQDGl45_9Xif_sYsAyvCOXpvls3eCqFzYGINMLPtsOew9fJuUt18Hf180vV3wAKLNjll37_kJrRQxJZXcLjmfIQ5wdSz8CyT";
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
                PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);

                push_noti.Insert_PushNotification_homework(ihw_Id, mi_id, dto, header_flg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }
        public IVRM_Homework_DTO deactivate(IVRM_Homework_DTO data)
        {
            try
            {
                var query = _PrincipalDashboardContext.IVRM_Homework_DMO.Single(s => s.MI_Id == data.MI_Id && s.IHW_Id == data.IHW_Id);

                if (query.IHW_ActiveFlag == true)
                {
                    query.IHW_ActiveFlag = false;
                }
                else
                {
                    query.IHW_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _PrincipalDashboardContext.Update(query);
                var contactExists = _PrincipalDashboardContext.SaveChanges();
                if (contactExists == 1)
                {
                    data.returnval = true;
                    //data.returnsavestatus = "saved";
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_Homework_DTO editData(IVRM_Homework_DTO data)
        {
            try
            {
                var editdata = (from m in _PrincipalDashboardContext.IVRM_Homework_DMO
                                from a in _PrincipalDashboardContext.AcademicYearDMO
                                from n in _PrincipalDashboardContext.School_M_Class
                                from o in _PrincipalDashboardContext.School_M_Section
                                from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                where (m.IHW_Id == data.IHW_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.MI_Id == n.MI_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id && m.ASMAY_Id == a.ASMAY_Id && m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id)
                                select new IVRM_Homework_DTO
                                {
                                    ASMCL_Id = n.ASMCL_Id,
                                    ASMCL_ClassName = n.ASMCL_ClassName,
                                    ASMAY_Id = a.ASMAY_Id,
                                    ASMS_Id = o.ASMS_Id,
                                    ISMS_Id = p.ISMS_Id,
                                    IHW_AssignmentNo = m.IHW_AssignmentNo,
                                    IHW_Attachment = m.IHW_Attachment,
                                    IHW_Assignment = m.IHW_Assignment,
                                    IHW_FilePath = m.IHW_FilePath,
                                    IHW_Date = m.IHW_Date,
                                    IHW_Topic = m.IHW_Topic,
                                    IVRMUL_Id = m.IVRMUL_Id,
                                    IHW_Id = m.IHW_Id,

                                }).Distinct().ToList();

                data.editlist = editdata.ToArray();

                data.ASMCL_Id = editdata.FirstOrDefault().ASMCL_Id;
                var imgdata = (from a in _PrincipalDashboardContext.IVRM_HomeWork_Attatchment_DMO_con
                               from b in _PrincipalDashboardContext.IVRM_Homework_DMO
                               where a.IHW_Id == data.IHW_Id && a.IHW_Id == b.IHW_Id
                               select new IVRM_Homework_DTO
                               {
                                   IHWATT_Attachment = a.IHWATT_Attachment,
                                   IHW_Attachment = b.IHW_Attachment,
                                   IHWATT_FileName = a.IHWATT_FileName,
                                   IHW_Id = a.IHW_Id
                               }).ToArray();
                if (imgdata.Length > 0)
                {
                    data.attachementlist = imgdata;
                }
                data.editlist_section = (from m in _PrincipalDashboardContext.IVRM_Homework_DMO
                                         from a in _PrincipalDashboardContext.AcademicYearDMO
                                         from n in _PrincipalDashboardContext.School_M_Class
                                         from o in _PrincipalDashboardContext.School_M_Section
                                         from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                         where (m.IHW_Id == data.IHW_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.MI_Id == n.MI_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id && m.ASMAY_Id == a.ASMAY_Id && m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id)
                                         select new IVRM_Homework_DTO
                                         {

                                             ASMS_Id = o.ASMS_Id,
                                             ASMC_SectionName = o.ASMC_SectionName

                                         }).Distinct().ToArray();

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    data.sectionlist = (from a in _PrincipalDashboardContext.School_M_Section
                                        from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                        from c in _PrincipalDashboardContext.AcademicYearDMO
                                        from e in _PrincipalDashboardContext.School_M_Class
                                        where (a.MI_Id == c.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMC_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id)
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = a.ASMC_SectionName,
                                            ASMCL_ClassName = e.ASMCL_ClassName,
                                            ASMC_Order = a.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                    var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();
                    var sectionexamid = (from e in _examcontext.Staff_User_Login
                                         from f in _examcontext.Exm_Login_PrivilegeDMO
                                         from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                         where (e.Id == data.Login_Id &&
                                           f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMCL_Id == data.ASMCL_Id && secid.Contains(i.ASMS_Id)
                                           && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true && i.ASMCL_Id == data.ASMCL_Id)
                                         select new IVRM_Homework_DTO
                                         {
                                             ASMS_Id = i.ASMS_Id
                                         }).Distinct().Select(t => t.ASMS_Id).ToArray();

                    List<School_M_Section> seclist = new List<School_M_Section>();
                    seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
                    data.sectionlist = seclist.Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_Homework_DTO viewData(IVRM_Homework_DTO dto)
        {
            try
            {
                dto.attachementlist = (from a in _PrincipalDashboardContext.IVRM_HomeWork_Attatchment_DMO_con
                                       from b in _PrincipalDashboardContext.IVRM_Homework_DMO
                                       where a.IHW_Id == dto.IHW_Id && a.IHW_Id == b.IHW_Id
                                       select new IVRM_Homework_DTO
                                       {
                                           IHWATT_Attachment = a.IHWATT_Attachment,
                                           IHW_Attachment = b.IHW_Attachment,
                                           IHWATT_FileName = a.IHWATT_FileName,
                                           IHW_Id = a.IHW_Id
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        //============ home work marks enter===========

        public IVRM_Homework_DTO gethomework_student(IVRM_Homework_DTO dto)
        {
            try
            {
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_HomeWork_Marks_Student_List";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
               SqlDbType.BigInt)
                    {
                        Value = dto.ASMS_Id
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
                        dto.studentlist1 = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_Homework_DTO getsubjectlist(IVRM_Homework_DTO dto)
        {
            try
            {
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_SubjectList_Modify";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = dto.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = dto.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = dto.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = dto.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@Type",SqlDbType.VarChar){Value = "HomeWork"});
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = dto.UserId });
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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.getsubject_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_Homework_DTO gethomework_list(IVRM_Homework_DTO dto)
        {
            try
            {
                //var amst_id = "";

                //foreach (var x in dto.studentarray)
                //{
                //    amst_id += x.AMST_Id1 + ",";
                //}
                // amst_id = amst_id.Substring(0, (amst_id.Length - 1));
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_List_Modify";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = dto.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Homework" });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt) { Value = dto.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });

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
                        dto.gethomework_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //doclist

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_List_Modify_Doclist";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = dto.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Homework" });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt) { Value = dto.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });

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
                        dto.viewhomework = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_homeworkclasswork_MarksUpdate_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.Login_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
                SqlDbType.VarChar)
                    {
                        Value = "Homework"
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
                        dto.marksupdate_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_classwork_Topiclist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.Login_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
                SqlDbType.VarChar)
                    {
                        Value = "Homework"
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
                        dto.TopicList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public IVRM_Homework_DTO homework_marks_update(IVRM_Homework_DTO dto)
        {
            try
            {
                foreach (var item1 in dto.gethomework_list_array)
                {
                    var result = _PrincipalDashboardContext.IVRM_HomeWork_Upload_DMO_con.Where(a => a.IHW_Id == item1.IHW_Id
                    && a.AMST_Id == item1.AMST_Id && a.IHWUPL_Id == item1.IHWUPL_Id).ToList();

                    if (result.Count > 0)
                    {
                        var rslt = _PrincipalDashboardContext.IVRM_HomeWork_Upload_DMO_con.Single(a => a.IHW_Id == item1.IHW_Id 
                        && a.AMST_Id == item1.AMST_Id && a.IHWUPL_Id == item1.IHWUPL_Id);
                        rslt.IHWUPL_Marks = item1.Marks;
                        rslt.IHWUPL_StaffRemarks = item1.IHWUPL_StaffRemarks;
                        rslt.IHWUPL_StaffUpload = item1.IHWUPL_StaffUpload;
                        rslt.IHWUPL_FileName = item1.IHWUPL_FileName;
                        rslt.UpdatedDate = DateTime.Now;
                        _PrincipalDashboardContext.Update(rslt);

                        if (item1.doclist_temp != null && item1.doclist_temp.Length > 0)
                        {
                            foreach (var d in item1.doclist_temp)
                            {
                                var dd = _PrincipalDashboardContext.IVRM_HomeWork_Upload_Attatchment_DMO_con.Where(a => a.IHWUPL_Id == item1.IHWUPL_Id
                                 && a.IHWUPLATT_Id == d.IHWUPLATT_Id).Count();

                                if (dd > 0)
                                {
                                    var result_update = _PrincipalDashboardContext.IVRM_HomeWork_Upload_Attatchment_DMO_con.Single(a => a.IHWUPL_Id == item1.IHWUPL_Id
                                 && a.IHWUPLATT_Id == d.IHWUPLATT_Id);

                                    result_update.IHWUPLATT_StaffUpload = d.FilePath1;
                                    result_update.IHWUPLATT_StaffRemarks = d.Remarks;
                                    result_update.IHWUPLATT_UpdatedDate = DateTime.Now;
                                    _PrincipalDashboardContext.Update(result_update);
                                }
                            }
                        }
                    }
                }
                var update = _PrincipalDashboardContext.SaveChanges();
                if (update > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_Homework_DTO edit_homework_mark(IVRM_Homework_DTO dto)
        {
            try
            {
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_edit_modfiy";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IHW_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.IHW_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
               SqlDbType.VarChar)
                    {
                        Value = "Homework"
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
                        dto.edit_mark_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeworkstudent";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IHW_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.IHW_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
               SqlDbType.VarChar)
                    {
                        Value = "Homework"
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
                        dto.editstudent = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_Homework_DTO viewhomework(IVRM_Homework_DTO dto)
        {
            try
            {
                dto.viewhomework = (from a in _PrincipalDashboardContext.IVRM_Homework_DMO
                                    from b in _PrincipalDashboardContext.IVRM_HomeWork_Attatchment_DMO_con
                                    where a.IHW_Id == b.IHW_Id && a.MI_Id == dto.MI_Id && a.IHW_Id == dto.IHW_Id
                                    select new IVRM_Homework_DTO
                                    {
                                        IHW_Id = a.IHW_Id,
                                        IHW_Topic = a.IHW_Topic,
                                        IHW_Assignment = a.IHW_Assignment,
                                        IHWATT_Attachment = b.IHWATT_Attachment,
                                        IHW_Attachment = a.IHW_Attachment,
                                        IHWATT_FileName = b.IHWATT_FileName
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_Homework_DTO viewstudentupload(IVRM_Homework_DTO dto)
        {
            try
            {
                dto.viewstudentupload = (from a in _PrincipalDashboardContext.IVRM_Homework_DMO
                                         from b in _PrincipalDashboardContext.IVRM_HomeWork_Upload_DMO_con
                                         from c in _PrincipalDashboardContext.IVRM_HomeWork_Upload_Attatchment_DMO_con
                                         where a.IHW_Id == b.IHW_Id && b.IHWUPL_Id == c.IHWUPL_Id && a.MI_Id == dto.MI_Id && a.IHW_Id == dto.IHW_Id
                                         && b.AMST_Id == dto.AMST_Id && b.IHWUPL_Id == dto.IHWUPL_Id
                                         select new IVRM_Homework_DTO
                                         {
                                             IHWUPL_Id = b.IHWUPL_Id,
                                             IHWUPL_Attachment = c.IHWUPLATT_Attachment,
                                             IHWUPL_FileName = c.IHWUPLATT_FileName,
                                             IHWUPLATT_StaffUpload = c.IHWUPLATT_StaffUpload,
                                             IHWUPLATT_StaffRemarks = c.IHWUPLATT_StaffRemarks,
                                             IHWUPLATT_Id = c.IHWUPLATT_Id,
                                         }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_Homework_DTO stfupload(IVRM_Homework_DTO dto)
        {
            try
            {
                foreach (var item in dto.doclist)
                {
                    IVRM_HomeWork_Upload_Attatchment_DMO hp = new IVRM_HomeWork_Upload_Attatchment_DMO();
                    hp.IHWUPL_Id = item.IHWUPL_Id;
                    hp.IHWUPLATT_Attachment = item.FilePath1;
                    hp.IHWUPLATT_FileName = item.FileName1;
                    hp.IHWUPLATT_StaffRemarks = item.Remarks;
                    hp.IHWUPLATT_StaffUpload = "Staff";
                    hp.IHWUPLATT_ActiveFlag = true;
                    hp.IHWUPLATT_CreatedDate = DateTime.Today;
                    hp.IHWUPLATT_UpdatedDate = DateTime.Today;
                    _PrincipalDashboardContext.Add(hp);
                }

                var update = _PrincipalDashboardContext.SaveChanges();


                if (update > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //Homework
        public IVRM_Homework_DTO gethomework_listTopic(IVRM_Homework_DTO dto)
        {
            try
            {

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_TopicList_Modify";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = dto.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Homework" });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt) { Value = dto.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });
                    cmd.Parameters.Add(new SqlParameter("@Topic_Id", SqlDbType.BigInt) { Value = dto.IHW_Id });

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
                        dto.gethomework_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "IVRM_homeclasswork_List_Modify_Doclist";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = 0 });                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = dto.Login_Id });                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Homework" });                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt) { Value = dto.ISMS_Id });                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        dto.viewhomework = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }                //
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_homeworkclasswork_MarksUpdate_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.Login_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
                SqlDbType.VarChar)
                    {
                        Value = "Homework"
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
                        dto.marksupdate_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}

