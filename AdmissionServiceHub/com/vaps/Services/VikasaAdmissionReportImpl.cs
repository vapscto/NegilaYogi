using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model.com.vapstech.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class VikasaAdmissionReportImpl : Interfaces.VikasaAdmissionReportInterface
    {
        private static ConcurrentDictionary<string, VikasaAdmissionreportDTO> _login =
          new ConcurrentDictionary<string, VikasaAdmissionreportDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        public VikasaAdmissionReportImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
        }

        public VikasaAdmissionreportDTO getdetails(VikasaAdmissionreportDTO stu)
        {
            VikasaAdmissionreportDTO acdmc = new VikasaAdmissionreportDTO();
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(m => m.MI_Id == stu.MI_Id && m.Is_Active == true).ToList();
                acdmc.AllAcademicYear = allacademic.OrderByDescending(a => a.ASMAY_Order).ToArray();

                acdmc.adm_m_student = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                                       from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                       where (a.AMST_Id == b.AMST_Id //&& a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.AMST_SOL == "S" 
                                       && a.MI_Id == stu.MI_Id)
                                       select new VikasaAdmissionreportDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                                       }).ToArray();

                acdmc.employeedetails = (from a in _DomainModelMsSqlServerContext.HR_Master_Employee_DMO
                                         where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == stu.MI_Id)
                                         select new VikasaAdmissionreportDTO
                                         {
                                             empid = a.HRME_Id,
                                             empname = ((a.HRME_EmployeeFirstName == null ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName)).Trim(),
                                         }).ToArray();


            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }
        public VikasaAdmissionreportDTO getStudDatabyclass(VikasaAdmissionreportDTO data)
        {
            var checkflag = _DomainModelMsSqlServerContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
            var checkflag1 = _DomainModelMsSqlServerContext.GenConfig.Where(a => a.MI_Id == data.MI_Id).ToList();


            if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
            {
                data.fillstudlist = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                     where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id //&& m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = m.AMST_Id,
                                         stuname = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ":" + (m.AMST_AdmNo == null ? " " : m.AMST_AdmNo)).Trim(),
                                         ASMCL_Id = n.ASMCL_Id,
                                         asms_id = n.ASMS_Id,
                                     }).ToArray();
            }

            else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "2")
            {
                data.fillstudlist = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                     where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id //&& m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = m.AMST_Id,
                                         stuname = ((m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                         ASMCL_Id = n.ASMCL_Id,
                                         asms_id = n.ASMS_Id,
                                     }).ToArray();
            }

            else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
            {
                data.fillstudlist = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                     where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id //&& m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = m.AMST_Id,
                                         stuname = ((m.AMST_AdmNo == null ? " " : m.AMST_AdmNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                         ASMCL_Id = n.ASMCL_Id,
                                         asms_id = n.ASMS_Id,
                                     }).ToArray();
            }

            else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "4")
            {
                data.fillstudlist = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                     where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id //&& m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = m.AMST_Id,
                                         stuname = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),
                                         ASMCL_Id = n.ASMCL_Id,
                                         asms_id = n.ASMS_Id,
                                     }).ToArray();
            }
            else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "6")
            {
                data.fillstudlist = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                     where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id //&& m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = m.AMST_Id,
                                         stuname = ((n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString()) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                         ASMCL_Id = n.ASMCL_Id,
                                         asms_id = n.ASMS_Id,
                                     }).ToArray();
            }

            else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "5")
            {
                data.fillstudlist = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                     where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id //&& m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = m.AMST_Id,
                                         stuname = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString())).Trim(),
                                         ASMCL_Id = n.ASMCL_Id,
                                         asms_id = n.ASMS_Id,
                                     }).ToArray();
            }
            else
            {
                data.fillstudlist = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                     where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id //&& m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = m.AMST_Id,
                                         stuname = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),
                                         ASMCL_Id = n.ASMCL_Id,
                                         asms_id = n.ASMS_Id,
                                     }).ToArray();
            }
            return data;

        }
        public VikasaAdmissionreportDTO onacademicyearchange(VikasaAdmissionreportDTO data)
        {
            try
            {
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                if (data.AMST_SOL == "S")
                {
                    flag = "S";
                    flagactive = 1;
                    amatactiveflag = 1;

                }
                else if (data.AMST_SOL == "L")
                {
                    flag = "L";
                    flagactive = 0;
                    amatactiveflag = 0;
                }

                data.fillstudlist = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                     where (a.AMST_Id == b.AMST_Id && a.AMST_SOL == data.AMST_SOL && a.AMST_SOL == flag && a.AMST_ActiveFlag == flagactive && b.AMAY_ActiveFlag == amatactiveflag && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                                         AMST_DOB = a.AMST_DOB,
                                         AMST_DOB_Words = a.AMST_DOB_Words,
                                         AMST_FatherName = a.AMST_FatherName,
                                         AMST_Sex = a.AMST_Sex
                                     }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async System.Threading.Tasks.Task<VikasaAdmissionreportDTO> getStudDetails(VikasaAdmissionreportDTO stuDTO)
        {
            try
            {
                List<StudycertificateDTO> stulist = new List<StudycertificateDTO>();
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                if (stuDTO.AMST_SOL == "S")
                {
                    flag = "S";
                    flagactive = 1;
                    amatactiveflag = 1;

                }
                else if (stuDTO.AMST_SOL == "L")
                {
                    flag = "L";
                    flagactive = 0;
                    amatactiveflag = 0;
                }

                stuDTO.fillstudlist = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                                       from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_SOL == flag && a.AMST_ActiveFlag == flagactive && b.AMAY_ActiveFlag == amatactiveflag && a.MI_Id == stuDTO.MI_Id)
                                       select new VikasaAdmissionreportDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                                           AMST_RegistrationNo = a.AMST_RegistrationNo,
                                           AMST_AdmNo = a.AMST_AdmNo
                                       }).ToArray();

                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == stuDTO.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                stuDTO.AllAcademicYear = allacademic.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public VikasaAdmissionreportDTO searchfilter(VikasaAdmissionreportDTO data)
        {
            //var flag = "";
            data.searchfilter = data.searchfilter.ToUpper();
            int flagactive = 0;
            int amatactiveflag = 0;
            if (data.AMST_SOL == "S")
            {
                //flag = "S";
                flagactive = 1;
                amatactiveflag = 1;

            }
            else if (data.AMST_SOL == "L")
            {
               // flag = "L";
                flagactive = 0;
                amatactiveflag = 0;
            }
            data.fillstudlist = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                                 from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                 where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == flagactive && b.AMAY_ActiveFlag == amatactiveflag
                                 && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().StartsWith(data.searchfilter) || a.AMST_MiddleName.ToUpper().StartsWith(data.searchfilter) || a.AMST_LastName.ToUpper().StartsWith(data.searchfilter)))
                                 select new StudentTCDTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                     StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                                 }).ToArray();
            return data;
        }
        public VikasaAdmissionreportDTO ShowReport(VikasaAdmissionreportDTO data)
        {
            try
            {
                int order = 0;

                string Selectedyear = _DomainModelMsSqlServerContext.AcademicYear.Single(y => y.ASMAY_Id == data.ASMAY_Id).ASMAY_Year;

                int Selectedyearorder = _DomainModelMsSqlServerContext.AcademicYear.Single(y => y.ASMAY_Id == data.ASMAY_Id).ASMAY_Order;

                order = Selectedyearorder - 1;

                if (order > 0)
                {
                    var getyearid = _DomainModelMsSqlServerContext.AcademicYear.Single(y => y.MI_Id == data.MI_Id && y.ASMAY_Order == order).ASMAY_Id;

                    data.previousyear = _DomainModelMsSqlServerContext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.ASMAY_Id == getyearid).ToArray();
                }


                List<VikasaAdmissionreportDTO> fee = new List<VikasaAdmissionreportDTO>();
                List<VikasaAdmissionreportDTO> studentdetails = new List<VikasaAdmissionreportDTO>();
                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Admission_Certificate_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@Amst_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMST_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@Asmay_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
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
                                studentdetails.Add(new VikasaAdmissionreportDTO
                                {
                                    AMST_FirstName = (dataReader["stuname"]).ToString(),
                                    AMST_FatherName = (dataReader["fname"]).ToString(),
                                    address = (dataReader["stuaddress"]).ToString(),
                                    mobileno = Convert.ToInt64((dataReader["phone"]).ToString()),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                    AMST_Sex = (dataReader["amsT_Sex"]).ToString(),
                                    AMST_DOB = Convert.ToDateTime((dataReader["dob"]).ToString()),
                                    AMST_DOB_Words = dataReader["dobw"].ToString(),
                                    classname = dataReader["classname"].ToString(),
                                    sectionname = dataReader["sectionname"].ToString(),
                                    doa = Convert.ToDateTime(dataReader["AMST_Date"].ToString()),
                                });


                            }
                        }
                        //studentdetails = retObject.ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (data.radiobutton == "1")
                {
                    using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Admission_Fee_IT_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.ASMAY_Id)
                        });

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.MI_Id)
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.AMST_Id)
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
                                    fee.Add(new VikasaAdmissionreportDTO
                                    {
                                        paidamount = (dataReader["Collectionamount"]).ToString(),
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }


                if (data.radiobutton == "5")
                {
                    using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Admission_Attendance_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.MI_Id)
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.ASMAY_Id)
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.AMST_Id)
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
                                    fee.Add(new VikasaAdmissionreportDTO
                                    {
                                        paidamount = (dataReader["Percentage"]).ToString(),
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                for (int i = 0; i < fee.Count; i++)
                {
                    studentdetails[i].paidamount = fee[i].paidamount;
                }


                data.mastercompany = _DomainModelMsSqlServerContext.Institute.Where(a => a.MI_Id == data.MI_Id).ToArray();



                if (data.save_flag == "yes")
                {
                    string Reporttype = "";

                    if (Convert.ToInt32(data.radiobutton) == 0)
                    {
                        Reporttype = "Bonafide Certificate";
                    }
                    else if (Convert.ToInt32(data.radiobutton) == 1)
                    {
                        Reporttype = "IT Declaration";
                    }
                    else if (Convert.ToInt32(data.radiobutton) == 2)
                    {
                        Reporttype = "Conduct Certificate";
                    }
                    else if (Convert.ToInt32(data.radiobutton) == 3)
                    {
                        Reporttype = "Admission Note";
                    }
                    else if (Convert.ToInt32(data.radiobutton) == 4)
                    {
                        Reporttype = "Student TC";
                    }
                    else if (Convert.ToInt32(data.radiobutton) == 5)
                    {
                        Reporttype = "Attendance Certificate";
                    }
                    else if (Convert.ToInt32(data.radiobutton) == 6)
                    {
                        Reporttype = "ISC Conduct Certificate";
                    }

                    StudycertificateReportDMO _report = new StudycertificateReportDMO();
                    _report.AMST_Id = data.AMST_Id;
                    _report.MI_Id = data.MI_Id;
                    _report.ASMAY_Id = data.ASMAY_Id;
                    _report.ASC_Date = DateTime.Now;
                    _report.ASC_No = Convert.ToInt32(data.radiobutton);
                    _report.ASC_ReportType = Reporttype;
                    _report.CreatedDate = DateTime.Now;
                    _report.UpdatedDate = DateTime.Now;
                    _report.ASC_Createdby = data.userid;
                    _report.ASC_Updatedby = data.userid;
                    _DomainModelMsSqlServerContext.Add(_report);
                    int i = _DomainModelMsSqlServerContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Record Saved Successfully";
                        var count = (from a in _DomainModelMsSqlServerContext.StudycertificateReportDMO
                                     where a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASC_No == Convert.ToInt32(data.radiobutton)
                                     select new VikasaAdmissionreportDTO
                                     {
                                         AMST_Id = a.AMST_Id
                                     }).ToList().Count();

                        data.count = count;

                    }
                    else
                    {
                        data.message = "Record Not Saved Successfully";
                    }

                }
                else
                {
                    var count = (from a in _DomainModelMsSqlServerContext.StudycertificateReportDMO
                                 where a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASC_No == 1
                                 select new VikasaAdmissionreportDTO
                                 {
                                     AMST_Id = a.AMST_Id
                                 }).ToList().Count();

                    data.count = count;

                }

                data.studentlist = studentdetails.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VikasaAdmissionreportDTO ShowReport1(VikasaAdmissionreportDTO data)
        {
            try
            {
                data.studentlist = (from a in _DomainModelMsSqlServerContext.HR_Master_Employee_DMO
                                    from b in _DomainModelMsSqlServerContext.HR_Master_Designation
                                    from c in _DomainModelMsSqlServerContext.IVRM_Master_Gender
                                    where (a.HRMDES_Id == b.HRMDES_Id && a.IVRMMG_Id == c.IVRMMG_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.empid)
                                    select new VikasaAdmissionreportDTO
                                    {
                                        doa = Convert.ToDateTime(a.HRME_DOJ),
                                        empname = ((a.HRME_EmployeeFirstName == null ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName)).Trim(),
                                        deptname = b.HRMDES_DesignationName,
                                        AMST_Sex = c.IVRMMG_GenderName.ToUpper(),

                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
