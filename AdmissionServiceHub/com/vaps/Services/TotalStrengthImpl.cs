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
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class TotalStrengthImpl : Interfaces.TotalStrengthInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login =
            new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        public ActivateDeactivateContext _ActivateDeactivateContext;
        public TotalStrengthImpl(ActivateDeactivateContext ActivateDeactivateContext)
        {
            _ActivateDeactivateContext = ActivateDeactivateContext;
        }

        public Adm_M_StudentDTO getdetails(Adm_M_StudentDTO stu)
        {
            Adm_M_StudentDTO acdmc = new Adm_M_StudentDTO();
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _ActivateDeactivateContext.academicYear.Where(y => y.MI_Id == stu.MI_Id && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                stu.AllAcademicYear = allacademic.ToArray();

                List<School_M_Class> admissionClass = new List<School_M_Class>();
                admissionClass = _ActivateDeactivateContext.admissionClass.Where(d => d.MI_Id == stu.MI_Id && d.ASMCL_ActiveFlag == true).OrderBy(c => c.ASMCL_Order).ToList();
                stu.AllClass = admissionClass.ToArray();

                List<School_M_Section> masterSection = new List<School_M_Section>();
                masterSection = _ActivateDeactivateContext.masterSection.Where(d => d.MI_Id == stu.MI_Id && d.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                stu.AllSection = masterSection.ToArray();

                var cat = _ActivateDeactivateContext.GenConfig.Where(g => g.MI_Id == stu.MI_Id && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {
                    stu.category_list = _ActivateDeactivateContext.category.Where(f => f.MI_Id == stu.MI_Id && f.AMC_ActiveFlag == 1).ToArray();

                    //stu.category_list = (from m in _ActivateDeactivateContext.category
                    //                     from n in _ActivateDeactivateContext.masterclasscategory
                    //                                        where m.AMC_Id == n.AMC_Id && n.MI_Id == stu.MI_Id &&
                    //                                        n.Is_Active == true
                    //                                        select new MasterClassCategoryDTO
                    //                                        {
                    //                                            ASMCC_Id = n.ASMCC_Id,
                    //                                            className = m.AMC_Name
                    //                                        }).ToArray();
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
        public async System.Threading.Tasks.Task<Adm_M_StudentDTO> getStudDetails(Adm_M_StudentDTO stuDTO)
        {
            //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
            List<Adm_M_StudentDTO> classlist = new List<Adm_M_StudentDTO>();
            string ISMS_Id = "0";
            var amcid = "0";
            if (stuDTO.AMC_Id > 0)
            {

                amcid = stuDTO.AMC_Id.ToString();

                stuDTO.AMC_logo = _ActivateDeactivateContext.category.Where(p => p.AMC_Id == stuDTO.AMC_Id && p.MI_Id == stuDTO.MI_Id && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();



            }

            if (stuDTO.Electivearray != null && stuDTO.Electivearray.Length > 0)
            {
                foreach (var d in stuDTO.Electivearray)
                {
                    ISMS_Id = ISMS_Id + ',' + d.ISMS_Id;
                }
            }



            try
            {
                if (stuDTO.AMST_SOL == "O")
                {
                    using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Total_strength_count_All_Category_test";
                        //Total_strength_count_All
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.BigInt) { Value = stuDTO.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.BigInt) { Value = stuDTO.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.BigInt) { Value = stuDTO.ASMC_Id });
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = stuDTO.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@status_flag", SqlDbType.VarChar) { Value = stuDTO.Status_Flag });
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
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
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
                else
                {
                    using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                    {
                        //Total_Strength_new_1
                        cmd.CommandText = "Total_Strength_new_1_Category_test";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                        cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
                        cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.VarChar) { Value = stuDTO.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar) { Value = stuDTO.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.VarChar) { Value = stuDTO.ASMC_Id });
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = stuDTO.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@status_flag", SqlDbType.VarChar) { Value = stuDTO.Status_Flag });
                        cmd.Parameters.Add(new SqlParameter("@withtc", SqlDbType.VarChar) { Value = stuDTO.withtc });
                        cmd.Parameters.Add(new SqlParameter("@withdeactive", SqlDbType.VarChar) { Value = stuDTO.withdeactive });
                        cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar) { Value = stuDTO.AMC_Id });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
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

                var data = _ActivateDeactivateContext.AdmissionStandardDMO.Where(t => t.MI_Id == stuDTO.MI_Id).ToList();
                if (data.Count > 0)
                {
                    stuDTO.gender1 = data.FirstOrDefault().ASC_Default_Gender;
                }

                string ASMCL_Id = "0";
                if (stuDTO.Status_Flag == "indi")
                {
                    ASMCL_Id = stuDTO.ASMCL_Id.ToString();

                }
                else if (stuDTO.Status_Flag == "all")
                {
                    var Dataclaslist = _ActivateDeactivateContext.admissionClass.Where(R => R.MI_Id == stuDTO.MI_Id && R.ASMCL_ActiveFlag == true).ToList();
                    if (Dataclaslist != null && Dataclaslist.Count > 0)
                    {

                        foreach (var d in Dataclaslist)
                        {
                            ASMCL_Id = ASMCL_Id + ',' + d.ASMCL_Id;
                        }
                    }
                }

                if (stuDTO.Electivearray != null && stuDTO.Electivearray.Length > 0)
                {

                    using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                    {
                        //Total_Strength_new_1
                        cmd.CommandText = "Total_Strength_new_1_Category_Electives";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                        cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
                        cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.VarChar) { Value = stuDTO.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar) { Value = ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.VarChar) { Value = stuDTO.ASMC_Id });
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = stuDTO.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@status_flag", SqlDbType.VarChar) { Value = stuDTO.Status_Flag });
                        cmd.Parameters.Add(new SqlParameter("@withtc", SqlDbType.VarChar) { Value = stuDTO.withtc });
                        cmd.Parameters.Add(new SqlParameter("@withdeactive", SqlDbType.VarChar) { Value = stuDTO.withdeactive });
                        cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar) { Value = stuDTO.AMC_Id });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = ISMS_Id });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            stuDTO.studentElectivelist = retObject.ToArray();

                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }


                    using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                    {
                        //Total_Strength_new_1
                        cmd.CommandText = "Total_Strength_new_1_Category_ElectivesSum";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                        cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
                        cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.VarChar) { Value = stuDTO.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar) { Value = stuDTO.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.VarChar) { Value = stuDTO.ASMC_Id });
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = stuDTO.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@status_flag", SqlDbType.VarChar) { Value = stuDTO.Status_Flag });
                        cmd.Parameters.Add(new SqlParameter("@withtc", SqlDbType.VarChar) { Value = stuDTO.withtc });
                        cmd.Parameters.Add(new SqlParameter("@withdeactive", SqlDbType.VarChar) { Value = stuDTO.withdeactive });
                        cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar) { Value = stuDTO.AMC_Id });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = ISMS_Id });


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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);

                                }
                            }


                            stuDTO.ElectiveSum = retObject.ToArray();

                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                var distinctgender = _ActivateDeactivateContext.Adm_M_Studentd.Where(t => t.MI_Id == stuDTO.MI_Id).Select(t => t.AMST_Sex).Distinct().ToList();
                if (distinctgender.Count > 0)
                {
                    stuDTO.totalgender = distinctgender.Count;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }

        //public Adm_M_StudentDTO getStudDetails(Adm_M_StudentDTO stuDTO)
        //{
        //    //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
        //    List<Adm_M_StudentDTO> classlist = new List<Adm_M_StudentDTO>();
        //    string mywhere = null;
        //    string sol = null;
        //    //long yearId = 0;
        //    //long ClassId = 0;
        //    //long SectionId = 0;
        //    try
        //    {
        //        if (stuDTO.AMST_SOL == "L" || stuDTO.AMST_SOL == "S")
        //        {
        //            mywhere += " SOL =" + stuDTO.AMST_SOL;
        //        }
        //        else
        //        {
        //            //sol = " ";
        //            sol += " SOL =" + stuDTO.AMST_SOL;
        //        }

        //        if (stuDTO.ASMAY_Id != 0)
        //        {
        //            //yearId =  stuDTO.ASMAY_Id;
        //            mywhere += " YearId =" + stuDTO.ASMAY_Id;
        //        }
        //        if (stuDTO.ASMCL_Id != 0)
        //        {
        //            //ClassId = stuDTO.ASMCL_Id;
        //            mywhere += " AND ";
        //            mywhere += "Classid =" + stuDTO.ASMCL_Id;
        //        }
        //        if (stuDTO.ASMC_Id != 0)
        //        {
        //            //SectionId = stuDTO.ASMC_Id;
        //            mywhere += " AND ";
        //            mywhere += "Sectionid =" + stuDTO.ASMC_Id;
        //        }

        //        using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "Total_Strength";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
        //            cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = Convert.ToString(sol) });
        //            //cmd.Parameters.Add(new SqlParameter("@YearId", SqlDbType.VarChar) { Value = Convert.ToString(yearId) });
        //            //cmd.Parameters.Add(new SqlParameter("@Classid", SqlDbType.VarChar) { Value = Convert.ToString(ClassId) });
        //            //cmd.Parameters.Add(new SqlParameter("@Sectionid", SqlDbType.VarChar) { Value = Convert.ToString(SectionId) });
        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();

        //            var retObject = new List<dynamic>();
        //            try
        //            {
        //                using (var dataReader = cmd.ExecuteReader())
        //                {
        //                    while (dataReader.Read())
        //                    {
        //                        classlist.Add(new Adm_M_StudentDTO
        //                        {
        //                            Class = Convert.ToInt64(dataReader["ASMCL_ClassName"]),
        //                            Section = Convert.ToString(dataReader["ASMC_SectionName"]),
        //                            Boys = Convert.ToString(dataReader["totalmail"]),
        //                            Girls = Convert.ToInt64(dataReader["totalfemale"]),
        //                            Total = Convert.ToInt64(dataReader["totalS"]),
        //                        });
        //                        stuDTO.studentlist = classlist.ToArray();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.Write(ex.Message);
        //            }
        //        }

        //        //if (stuDTO.AMST_SOL == "L")
        //        //{
        //        //    stuDTO.studentlist = (//from a in _ActivateDeactivateContext.school_Adm_Y_StudentDMO
        //        //                       from b in _ActivateDeactivateContext.adm_M_Student
        //        //                       //where (a.AMST_Id == b.AMST_Id && a.AMAY_Id == StudDet.ASMAY_Id && b.AMST_SOL == StudDet.AMST_SOL)
        //        //                       where (b.ASMAY_Id == stuDTO.ASMAY_Id && b.AMST_SOL == stuDTO.AMST_SOL)
        //        //                       select new Adm_M_StudentDTO
        //        //                       {
        //        //                           AMST_Id = b.AMST_Id,
        //        //                           Student = b.AMST_FirstName,
        //        //                           stuMN = b.AMST_MiddleName,
        //        //                           stuLN = b.AMST_LastName,
        //        //                           RegNo = b.AMST_RegistrationNo,
        //        //                           Admno = b.AMST_AdmNo,
        //        //                           Gender = b.AMST_Sex,
        //        //                           DOB = b.AMST_DOB,
        //        //                           FatherOcc = b.AMST_FatherOccupation
        //        //                       }
        //        // ).ToArray();

        //        //}

        //        //else if(stuDTO.AMST_SOL == "S")
        //        //{
        //        //    List<Adm_M_StudentDTO> classlist = new List<Adm_M_StudentDTO>();
        //        //    stuDTO.studentlist = (//from a in _ActivateDeactivateContext.school_Adm_Y_StudentDMO
        //        //                       from b in _ActivateDeactivateContext.adm_M_Student
        //        //                           //where (a.AMST_Id == b.AMST_Id && a.AMAY_Id == StudDet.ASMAY_Id && b.AMST_SOL == StudDet.AMST_SOL)
        //        //                       where (b.ASMAY_Id == stuDTO.ASMAY_Id && b.AMST_SOL == stuDTO.AMST_SOL)
        //        //                       select new Adm_M_StudentDTO
        //        //                       {
        //        //                           AMST_Id = b.AMST_Id,
        //        //                           Student = b.AMST_FirstName,
        //        //                           stuMN = b.AMST_MiddleName,
        //        //                           stuLN = b.AMST_LastName,
        //        //                           RegNo = b.AMST_RegistrationNo,
        //        //                           Admno = b.AMST_AdmNo,
        //        //                           Gender = b.AMST_Sex,
        //        //                           DOB = b.AMST_DOB,
        //        //                           FatherOcc = b.AMST_FatherOccupation
        //        //                       }
        //        // ).ToArray();

        //        //}

        //        //else
        //        //{
        //        //    List<Adm_M_StudentDTO> classlist = new List<Adm_M_StudentDTO>();
        //        //    stuDTO.studentlist = (//from a in _ActivateDeactivateContext.school_Adm_Y_StudentDMO
        //        //                       from b in _ActivateDeactivateContext.adm_M_Student
        //        //                           //where (a.AMST_Id == b.AMST_Id && a.AMAY_Id == StudDet.ASMAY_Id && b.AMST_SOL == StudDet.AMST_SOL)
        //        //                       where (b.ASMAY_Id == stuDTO.ASMAY_Id)
        //        //                       select new Adm_M_StudentDTO
        //        //                       {
        //        //                           AMST_Id = b.AMST_Id,
        //        //                           Student = b.AMST_FirstName,
        //        //                           stuMN = b.AMST_MiddleName,
        //        //                           stuLN = b.AMST_LastName,
        //        //                           RegNo = b.AMST_RegistrationNo,
        //        //                           Admno = b.AMST_AdmNo,
        //        //                           Gender = b.AMST_Sex,
        //        //                           DOB = b.AMST_DOB,
        //        //                           FatherOcc = b.AMST_FatherOccupation
        //        //                       }
        //        // ).ToArray();

        //        //}
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return stuDTO;
        //}


        public Adm_M_StudentDTO getsection(Adm_M_StudentDTO data)
        {
            try
            {

                var sectiondata = (from a in _ActivateDeactivateContext.admissionClass
                                   from b in _ActivateDeactivateContext.masterSection
                                   from c in _ActivateDeactivateContext.academicYear
                                   from d in _ActivateDeactivateContext.masterclasscategory
                                   from e in _ActivateDeactivateContext.AdmSchoolMasterClassCatSec
                                   where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                                   && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == data.ASMCL_Id && e.ASMCCS_ActiveFlg == true)
                                   select b
                                 ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();


                if (sectiondata.Length > 0)
                {
                    data.AllSection = sectiondata.ToArray();
                }
                else
                {
                    List<School_M_Section> secname = new List<School_M_Section>();
                    secname = _ActivateDeactivateContext.masterSection.Where(s => s.MI_Id == data.MI_Id && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                    data.AllSection = secname.ToArray();
                }


                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    //AlumnistudentsearchReport_new
                    //AlumnistudentsearchReport
                    cmd.CommandText = "Total_Strength_Electives";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = data.AMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                        data.AllElectives = retObject.ToArray();
                        if (data.AllElectives.Length > 0)
                        {
                            data.count = data.AllElectives.Length;
                        }
                        else
                        {
                            data.count = 0;
                        }
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

        public Adm_M_StudentDTO getelective(Adm_M_StudentDTO data)
        {
            try
            {



                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    //AlumnistudentsearchReport_new
                    //AlumnistudentsearchReport
                    cmd.CommandText = "Total_Strength_Electives";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                        data.AllElectives = retObject.ToArray();
                        if (data.AllElectives.Length > 0)
                        {
                            data.count = data.AllElectives.Length;
                        }
                        else
                        {
                            data.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                //if (sectiondata.Length > 0)
                //{
                //    data.AllSection = sectiondata.ToArray();
                //}
                //else
                //{
                //    List<School_M_Section> secname = new List<School_M_Section>();
                //    secname = _ActivateDeactivateContext.masterSection.Where(s => s.MI_Id == data.MI_Id && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                //    data.AllSection = secname.ToArray();
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Adm_M_StudentDTO getclass(Adm_M_StudentDTO data)
        {
            try
            {



                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    //AlumnistudentsearchReport_new
                    //AlumnistudentsearchReport
                    cmd.CommandText = "Total_strength_class";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = data.AMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Emp_Id", SqlDbType.BigInt) { Value = 0 });

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
                        data.AllClass = retObject.ToArray();
                        if (data.AllClass.Length > 0)
                        {
                            data.count = data.AllClass.Length;
                        }
                        else
                        {
                            data.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    //AlumnistudentsearchReport_new
                    //AlumnistudentsearchReport
                    cmd.CommandText = "Total_Strength_Electives";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = data.AMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                        data.AllElectives = retObject.ToArray();
                        if (data.AllElectives.Length > 0)
                        {
                            data.count = data.AllElectives.Length;
                        }
                        else
                        {
                            data.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                //if (sectiondata.Length > 0)
                //{
                //    data.AllSection = sectiondata.ToArray();
                //}
                //else
                //{
                //    List<School_M_Section> secname = new List<School_M_Section>();
                //    secname = _ActivateDeactivateContext.masterSection.Where(s => s.MI_Id == data.MI_Id && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                //    data.AllSection = secname.ToArray();
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }

}
