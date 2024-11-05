using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;


namespace CollegeServiceHub.Impl
{
    public class StudentAddressBookImpl : Interface.StudentAddressBookInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public StudentAddressBookImpl(ClgAdmissionContext n)
        {
            _clgadmctxt = n;
        }
        public StudentAddressBookDTO loaddata(StudentAddressBookDTO data)
        {
            try
            {
                data.academiclist = _clgadmctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentAddressBookDTO getcourse(StudentAddressBookDTO data)
        {
            try
            {
                data.courselist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   from b in _clgadmctxt.MasterCourseDMO
                                   from c in _clgadmctxt.AcademicYear
                                   where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id
                                   && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id)
                                   select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentAddressBookDTO getbranch(StudentAddressBookDTO data)
        {
            try
            {
                data.branchlist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   from b in _clgadmctxt.MasterCourseDMO
                                   from c in _clgadmctxt.AcademicYear
                                   from d in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                   from e in _clgadmctxt.ClgMasterBranchDMO
                                   where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                   && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                   && d.ACAYCB_ActiveFlag == true && e.AMB_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id)
                                   select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentAddressBookDTO getsemester(StudentAddressBookDTO data)
        {
            try
            {
                var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                    from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                    && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id
                                    && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                                    && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select a).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentAddressBookDTO onselectBranch(StudentAddressBookDTO data)
        {
            try
            {
                List<long> GrpId = new List<long>();

                foreach (var item in data.Temp_branchDTOreport)
                {
                    GrpId.Add(item.AMB_Id);
                }

                var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                    from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                    && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id
                                    && GrpId.Contains(c.AMB_Id) && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                                    && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select a).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAddressBookDTO getsection(StudentAddressBookDTO data)
        {
            try
            {
                List<long> GrpId = new List<long>();

                if (data.Temp_branchDTOreport != null && data.Temp_branchDTOreport.Length > 0)
                {
                    foreach (var item in data.Temp_branchDTOreport)
                    {
                        GrpId.Add(item.AMB_Id);
                    }
                }

                data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAddressBookDTO getstudent(StudentAddressBookDTO data)
        {
            try
            {
                data.studentlist = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                                    from b in _clgadmctxt.Adm_College_Yearly_StudentDMO
                                    from c in _clgadmctxt.MasterCourseDMO
                                    from d in _clgadmctxt.ClgMasterBranchDMO
                                    from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    from f in _clgadmctxt.Adm_College_Master_SectionDMO
                                    from g in _clgadmctxt.AcademicYear
                                    where (a.AMCST_Id == b.AMCST_Id && b.ASMAY_Id == g.ASMAY_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id
                                    && b.AMSE_Id == e.AMSE_Id && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id
                                    && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id)
                                    select new StudentAddressBookDTO
                                    {
                                        studentname = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : a.AMCST_FirstName) +
                                        (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                        (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName) +
                                        (a.AMCST_AdmNo == null || a.AMCST_AdmNo == "" ? "" : " : " + a.AMCST_AdmNo)),
                                        AMCST_AdmNo = a.AMCST_AdmNo,
                                        AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                        AMCST_Id = b.AMCST_Id
                                    }).Distinct().OrderBy(a => a.studentname).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAddressBookDTO Report(StudentAddressBookDTO data)
        {
            try
            {
                string branchid = "";
                for (int i = 0; i < data.Temp_branchDTOreport.Count(); i++)
                {
                    if (i == 0)
                    {
                        branchid = Convert.ToString(data.Temp_branchDTOreport[i].AMB_Id);
                    }
                    else
                    {
                        branchid = branchid + "," + Convert.ToString(data.Temp_branchDTOreport[i].AMB_Id);
                    }
                }

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Admission_Student_Address_Book_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = branchid });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.reportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentAddressBookDTO AddressBookFormat2(StudentAddressBookDTO data)
        {
            try
            {
                string amcstids = "0";
                if (data.Temp_StudentListDto != null)
                {
                    foreach (var c in data.Temp_StudentListDto)
                    {
                        amcstids = amcstids + "," + c.AMCST_Id;
                    }
                }

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Admission_Student_Address_Book_Report_Format2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcstids });
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
                        data.reportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
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
