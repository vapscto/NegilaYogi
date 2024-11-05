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

namespace AdmissionServiceHub.com.vaps.Services
{
    public class DOBcertificateImpl : Interfaces.DOBcertificateInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login =
            new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        public DOBcertificateImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
        }

        public Adm_M_StudentDTO getdetails(Adm_M_StudentDTO stu)
        {
            Adm_M_StudentDTO acdmc = new Adm_M_StudentDTO();
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(m => m.MI_Id == stu.MI_Id && m.Is_Active == true).ToList();
                //   acdmc.AllAcademicYear = allacademic.OrderByDescending(a => a.ASMAY_Order).ToArray();
                stu.AllAcademicYear = allacademic.OrderByDescending(a => a.ASMAY_Order).ToArray();
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _DomainModelMsSqlServerContext.School_M_Class.Where(d => d.MI_Id == stu.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
               // acdmc.AllClass = classlist.OrderBy(c => c.ASMCL_Order).ToArray();
               stu.AllClass = classlist.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _DomainModelMsSqlServerContext.Section.Where(d => d.MI_Id == stu.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                //  acdmc.AllSection = seclist.OrderBy(s => s.ASMC_Order).ToArray();
                stu.AllSection = seclist.OrderBy(s => s.ASMC_Order).ToArray();

                //List<Adm_M_Student> adm_m_student1 = new List<Adm_M_Student>();
                //adm_m_student1 = _DomainModelMsSqlServerContext.Adm_M_Student.Where(s=>s.MI_Id==stu.MI_Id
                //&& s.AMST_ActiveFlag==1).ToList();
                //acdmc.adm_m_student = adm_m_student1.OrderBy(s=>s.AMST_FirstName).ToArray();


                var cat = _DomainModelMsSqlServerContext.GenConfig.Where(g => g.MI_Id == stu.MI_Id && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {


                    stu.category_list = _DomainModelMsSqlServerContext.mastercategory.Where(f => f.MI_Id == stu.MI_Id && f.AMC_ActiveFlag == 1).ToArray();
                    stu.categoryflag = true;
                }
                else
                {
                    stu.categoryflag = false;
                }


            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }

        //public Adm_M_StudentDTO getStudDatabyclass(Adm_M_StudentDTO data)
        //{
        //    Adm_M_StudentDTO acdmc = new Adm_M_StudentDTO();
        //    try
        //    {
        //        List<Adm_M_Student> adm_m_student = new List<Adm_M_Student>();
        //        adm_m_student = _DomainModelMsSqlServerContext.Adm_M_Student.Where(t => t.ASMCL_Id == ASMCL_Id).ToList();
        //        acdmc.fillstudlist =  adm_m_student.OrderBy(s=>s.AMST_FirstName).ToArray();

        //    }

        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return acdmc;
        //}

        public Adm_M_StudentDTO getStudDatabyclass(Adm_M_StudentDTO data)
        {
            var checkflag = _DomainModelMsSqlServerContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
            var checkflag1 = _DomainModelMsSqlServerContext.GenConfig.Where(a => a.MI_Id == data.MI_Id).ToList();


            //using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
            //{
            //    cmd.CommandText = "Student_Name_Binding_DOB_Cer";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt)
            //    {
            //        Value = Convert.ToInt64(data.ASMAY_Id)
            //    });
            //    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.BigInt)
            //    {
            //        Value = Convert.ToInt64(data.ASMCL_Id)
            //    });
            //    cmd.Parameters.Add(new SqlParameter("@secid", SqlDbType.BigInt)
            //    {
            //        Value = Convert.ToInt64(data.ASMC_Id)
            //    });
            //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
            //    {
            //        Value = Convert.ToInt64(data.MI_Id)
            //    });



            //    if (cmd.Connection.State != ConnectionState.Open)
            //        cmd.Connection.Open();

            //    var retObject = new List<dynamic>();

            //    try
            //    {
            //        using (var dataReader =  cmd.ExecuteReader())
            //        {
            //            while (dataReader.Read())
            //            {
            //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                {
            //                    dataRow.Add(
            //                        dataReader.GetName(iFiled),
            //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                    );
            //                }

            //                retObject.Add((ExpandoObject)dataRow);
            //            }
            //        }
            //        data.fillstudlist = retObject.ToArray();
            //    }


            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    
            //}
            if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
            {
                data.fillstudlist = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                     where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new Adm_M_StudentDTO
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
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new Adm_M_StudentDTO
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
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new Adm_M_StudentDTO
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
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new Adm_M_StudentDTO
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
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new Adm_M_StudentDTO
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
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new Adm_M_StudentDTO
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
                                     && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_Id && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                     select new Adm_M_StudentDTO
                                     {
                                         AMST_Id = m.AMST_Id,
                                         stuname = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),
                                         ASMCL_Id = n.ASMCL_Id,
                                         asms_id = n.ASMS_Id,
                                     }).ToArray();
            }
            return data;

        }



        public async System.Threading.Tasks.Task<Adm_M_StudentDTO> getStudDetails(Adm_M_StudentDTO stuDTO)
        {
            var amcid = "0";

            if (stuDTO.AMC_Id > 0)
            {
                
                 amcid = stuDTO.AMC_Id.ToString();

                stuDTO.AMC_logo = _DomainModelMsSqlServerContext.mastercategory.Where(p => p.AMC_Id == stuDTO.AMC_Id && p.MI_Id == stuDTO.MI_Id && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();


            }

            //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
            List<Adm_M_StudentDTO> HFClist = new List<Adm_M_StudentDTO>();
            try
            {

                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_DOB_cerificate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                    //cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
                    cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.BigInt) { Value = stuDTO.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.BigInt) { Value = stuDTO.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.BigInt) { Value = stuDTO.ASMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@studid", SqlDbType.BigInt) { Value = stuDTO.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = stuDTO.AMC_Id });
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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "" : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                      dataReader.GetName(iFiled),
                                      dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                                  );
                                    }


                                }
                                try
                                {
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }



                        stuDTO.studentlist = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }
    }
}
