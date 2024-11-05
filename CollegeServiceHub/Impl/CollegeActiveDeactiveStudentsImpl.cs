using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
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
    public class CollegeActiveDeactiveStudentsImpl : Interface.CollegeActiveDeactiveStudentsInterface
    {
        public ClgAdmissionContext _clgadmctxt;

        public CollegeActiveDeactiveStudentsImpl(ClgAdmissionContext clgadmctxt)
        {
            _clgadmctxt = clgadmctxt;
        }
        public CollegeActiveDeactiveStudentsDTO getdata(CollegeActiveDeactiveStudentsDTO data)
        {
            try
            {
                data.yearlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeActiveDeactiveStudentsDTO onacademicyearchange(CollegeActiveDeactiveStudentsDTO data)
        {
            try
            {
                data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                   from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                   && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeActiveDeactiveStudentsDTO oncoursechange(CollegeActiveDeactiveStudentsDTO data)
        {
            try
            {
                var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                  from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag
                                  && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided

                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeActiveDeactiveStudentsDTO onbranchchange(CollegeActiveDeactiveStudentsDTO data)
        {
            try
            {
                var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                    from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && c.ACAYC_Id == b.ACAYC_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id
                                    && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id
                                    && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                                    && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)

                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();

                data.semesterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeActiveDeactiveStudentsDTO onchangesemester(CollegeActiveDeactiveStudentsDTO data)
        {
            try
            {
                data.sectionlist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeActiveDeactiveStudentsDTO search(CollegeActiveDeactiveStudentsDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Student_Data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_SOL", SqlDbType.VarChar) { Value = Convert.ToString(data.AMCST_SOL) });


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
                            data.studentlist = retObject.ToArray();
                        }
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
        public CollegeActiveDeactiveStudentsDTO savedata(CollegeActiveDeactiveStudentsDTO data)
        {
            try
            {
                if (data.savetmpdata.Count() > 0)
                {
                    foreach (CollegeActiveDeactiveStudentsDTO ph in data.savetmpdata)
                    {
                        if (ph.checkedvalue == true)
                        {
                            var Phone_Noresult = _clgadmctxt.Adm_Master_College_StudentDMO.Single(t => t.AMCST_Id == ph.AMCST_Id);
                            Phone_Noresult.AMCST_SOL = data.AMCST_SOL_activate;
                            _clgadmctxt.Update(Phone_Noresult);

                            var check_alreadyexists = _clgadmctxt.CollegeActiveDeactiveStudentsReasonDMO.Where(a => a.MI_Id == data.MI_Id &&
                            a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMCST_Id == ph.AMCST_Id && a.ACSDE_ActivedFlg == false
                            && a.ACSDE_ActiveFlag == true && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id).ToList();

                            if (check_alreadyexists.Count() > 0)
                            {
                                var check_alreadyexistsupdate = _clgadmctxt.CollegeActiveDeactiveStudentsReasonDMO.Single(a => a.MI_Id == data.MI_Id &&
                            a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMCST_Id == ph.AMCST_Id && a.ACSDE_ActivedFlg == false
                            && a.ACSDE_ActiveFlag == true && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id);

                                check_alreadyexistsupdate.ACSDE_ActivatedReason = ph.remarks;
                                check_alreadyexistsupdate.ACSDE_ActivatedDate = DateTime.Now;
                                check_alreadyexistsupdate.ACSDE_UpdatedBy = data.userid;
                                check_alreadyexistsupdate.UpdatedDate = DateTime.Now;
                                check_alreadyexistsupdate.ACSDE_ActiveFlag = true;
                                _clgadmctxt.Update(check_alreadyexistsupdate);
                            }
                            else
                            {
                                CollegeActiveDeactiveStudentsReasonDMO rema = new CollegeActiveDeactiveStudentsReasonDMO();
                                rema.MI_Id = data.MI_Id;
                                rema.ASMAY_Id = data.ASMAY_Id;
                                rema.AMCO_Id = data.AMCO_Id;
                                rema.AMB_Id = data.AMB_Id;
                                rema.AMSE_Id = data.AMSE_Id;
                                rema.ACMS_Id = data.ACMS_Id;
                                rema.AMCST_Id = ph.AMCST_Id;
                                if (data.AMCST_SOL_activate == "D")
                                {
                                    rema.ACSDE_DeactivatedReason = ph.remarks;
                                    rema.ACSDE_DeactivatedDate = DateTime.Now;
                                    rema.ACSDE_ActivatedReason = "";
                                    rema.ACSDE_ActivedFlg = false;
                                }
                                else
                                {
                                    rema.ACSDE_ActivatedReason = ph.remarks;
                                    rema.ACSDE_ActivatedDate = DateTime.Now;
                                    rema.ACSDE_DeactivatedReason = "";
                                    rema.ACSDE_ActivedFlg = true;
                                }
                                rema.ACSDE_ActiveFlag = true;
                                rema.ACSDE_CreatedBy = data.userid;
                                rema.ACSDE_UpdatedBy = data.userid;
                                rema.CreatedDate = DateTime.Now;
                                rema.UpdatedDate = DateTime.Now;

                                _clgadmctxt.Add(rema);
                            }

                            var contactExists = _clgadmctxt.SaveChanges();

                            if (contactExists > 0)
                            {
                                data.returnval = true;

                                var getstdappid = _clgadmctxt.CollegeStudentlogin.Where(a => a.AMCST_Id == ph.AMCST_Id).Select(a => a.IVRMUL_Id).ToList();

                                var chckuser = _clgadmctxt.UserRoleWithInstituteDMO.Where(a => a.Id == getstdappid[0]).ToList();

                                if (chckuser.Count() > 0)
                                {
                                    var updatecheckuser = _clgadmctxt.UserRoleWithInstituteDMO.Single(a => a.Id == getstdappid[0]);

                                    if (data.AMCST_SOL_activate == "D")
                                    {
                                        updatecheckuser.Activeflag = 0;
                                    }
                                    else
                                    {
                                        updatecheckuser.Activeflag = 1;
                                    }

                                    _clgadmctxt.Update(updatecheckuser);
                                    var i = _clgadmctxt.SaveChanges();
                                    if (i > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = true;
                                    }
                                }
                            }
                            else
                            {
                                data.returnval = false;
                            }
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

        public CollegeActiveDeactiveStudentsDTO getreport(CollegeActiveDeactiveStudentsDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Active_Deacctive_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) }); 

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
                            data.getreport = retObject.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
