using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class TeressianCertificateIMPL : Interface.TeressianCertificateInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public TeressianCertificateIMPL(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }

        public TeressianCertificateDTO getalldetails(TeressianCertificateDTO data)
        {
            try
            {
                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _clgadmctxt.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                data.check_list = _clgadmctxt.IVRM_College_ReportDMO.ToArray();
                data.studentlist = _clgadmctxt.Adm_Master_College_StudentDMO.Where(t => t.AMCST_ActiveFlag == true).ToArray();

                data.talukalist = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                                   where (a.AMCST_ActiveFlag == true && a.AMCST_Taluk != " ")
                                   select new TeressianCertificateDTO
                                   {
                                       AMCST_Taluk = a.AMCST_Taluk
                                   }).Distinct().ToArray();

                data.districtlist = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                                     where (a.AMCST_ActiveFlag == true && a.AMCST_District != " ")
                                     select new TeressianCertificateDTO
                                     {
                                         AMCST_District = a.AMCST_District
                                     }).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TeressianCertificateDTO getcoursedata(TeressianCertificateDTO data)
        {
            try
            {

                data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                   from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TeressianCertificateDTO getbranchdata(TeressianCertificateDTO data)
        {
            try
            {
                var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                  from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
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
        public TeressianCertificateDTO getsemisterdata(TeressianCertificateDTO data)
        {
            try
            {
                if (data.AMB_ID > 0)
                {
                    var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                        from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_ID && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
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
                    data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }
                else
                {
                    var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true)
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
                    data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TeressianCertificateDTO getsstudentdata(TeressianCertificateDTO data)
        {
            try
            {
                var stude = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                             from b in _clgadmctxt.Adm_College_Yearly_StudentDMO
                             where (a.MI_Id == data.MI_Id && a.AMCST_Id==b.AMCST_Id && b.ASMAY_Id == data.ASMAY_Id 
                             && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == a.AMB_Id && b.AMSE_Id == data.AMSE_ID && b.ACMS_Id == data.ACMS_ID)
                             select new Adm_Master_College_StudentDMO
                             {
                                 AMCST_Id = a.AMCST_Id,
                                 AMCST_FirstName = a.AMCST_FirstName,
                                 AMCST_MiddleName = a.AMCST_MiddleName,
                                 AMCST_LastName = a.AMCST_LastName

                             }).Distinct().ToList();
                data.studentlist = stude.OrderBy(t => t.AMCST_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TeressianCertificateDTO GetCertificate(TeressianCertificateDTO data)
        {
            try
            {

                List<TeressianCertificateDTO> result = new List<TeressianCertificateDTO>();
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Admission_Certificates";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@report_name", SqlDbType.VarChar) { Value = data.report_name });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.Int) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_ID", SqlDbType.Int) { Value = data.AMSE_ID });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_ID", SqlDbType.Int) { Value = data.ACMS_ID });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.Int) { Value = data.AMB_ID });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_ID", SqlDbType.Int) { Value = data.AMCST_ID });
                    cmd.Parameters.Add(new SqlParameter("@param", SqlDbType.VarChar) { Value = data.param });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        if (data.report_name == "conductcer")
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new TeressianCertificateDTO
                                    {
                                        studentname = dataReader["Student_Name"].ToString(),
                                        coursename = dataReader["coursename"].ToString(),
                                        coustart = Convert.ToInt64(dataReader["coustart"].ToString()),
                                        couend = Convert.ToInt64(dataReader["couend"].ToString()),

                                    });
                                    data.getreportdata = result.ToArray();
                                }
                            }

                        }
                        else if (data.report_name == "coursecer")
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new TeressianCertificateDTO
                                    {
                                        studentname = dataReader["Student_Name"].ToString(),
                                        coursename = dataReader["coursename"].ToString(),
                                        coustart = Convert.ToInt64(dataReader["coustart"].ToString()),
                                        couend = Convert.ToInt64(dataReader["couend"].ToString()),
                                        syslabus = dataReader["syslabus"].ToString(),
                                        languages = dataReader["languages"].ToString(),
                                        optionals = dataReader["optionals"].ToString()
                                    });
                                    data.getreportdata = result.ToArray();
                                }
                            }
                        }
                        else if (data.report_name == "tetransfer")
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new TeressianCertificateDTO
                                    {
                                        studentname = dataReader["Student_Name"].ToString(),
                                        admissionno = dataReader["admissionno"].ToString(),
                                        Dob = Convert.ToDateTime(dataReader["Dob"].ToString()),
                                        dobw = dataReader["dobw"].ToString(),
                                        nationality = dataReader["nationality"].ToString(),
                                        fathername = dataReader["fathername"].ToString(),
                                        mothername = dataReader["mothername"].ToString(),
                                        religion = dataReader["religion"].ToString(),
                                        caste = dataReader["caste"].ToString(),
                                        doj = Convert.ToDateTime(dataReader["doj"].ToString()),
                                        languages = dataReader["languages"].ToString(),
                                        optionals = dataReader["optionals"].ToString(),
                                        feedue = dataReader["feedue"].ToString()
                                    });
                                    data.getreportdata = result.ToArray();
                                }
                            }
                        }
                        else if (data.report_name == "taluk" || data.report_name == "district")
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new TeressianCertificateDTO
                                    {
                                        studentname = dataReader["Student_Name"].ToString(),
                                        Dob =Convert.ToDateTime(dataReader["Dob"].ToString()),
                                        fathername = dataReader["fathername"].ToString(),
                                        fatheredu = dataReader["fatheredu"].ToString(),
                                        mothername = dataReader["mothername"].ToString(),
                                        motheredu = dataReader["motheredu"].ToString(),
                                        gendar = dataReader["gendar"].ToString(),
                                        address = dataReader["address"].ToString(),
                                        mobile = dataReader["mobile"].ToString(),
                                        district = dataReader["district"].ToString(),
                                        taluk = dataReader["taluk"].ToString(),
                                    });
                                    data.getreportdata = result.ToArray();
                                }
                            }
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
    }
}
