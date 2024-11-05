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
    public class HostelFoodConveyanceReportImpl : Interfaces.HostelFoodConveyanceReportInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login =
            new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        public HostelFoodConveyanceReportImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
        }

        public Adm_M_StudentDTO getdetails(Adm_M_StudentDTO stu)
        {
            Adm_M_StudentDTO acdmc = new Adm_M_StudentDTO();
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(r=>r.Is_Active==true && r.MI_Id==stu.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToList();
                acdmc.AllAcademicYear = allacademic.ToArray();

                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _DomainModelMsSqlServerContext.School_M_Class.Where(r => r.ASMCL_ActiveFlag == true && r.MI_Id == stu.MI_Id).OrderBy(c => c.ASMCL_Order).ToList();
                acdmc.AllClass = classlist.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _DomainModelMsSqlServerContext.Section.Where(r => r.ASMC_ActiveFlag == 1 && r.MI_Id == stu.MI_Id).OrderBy(s => s.ASMC_Order).ToList();
                acdmc.AllSection = seclist.ToArray();

                //List<Adm_M_Student> adm_m_student = new List<Adm_M_Student>();
                //adm_m_student = _DomainModelMsSqlServerContext.Adm_M_Student.ToList();
                //acdmc.adm_m_student = adm_m_student.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }

        public async System.Threading.Tasks.Task<Adm_M_StudentDTO> getStudDetails(Adm_M_StudentDTO stuDTO)
        {
            //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
            List<Adm_M_StudentDTO> HFClist = new List<Adm_M_StudentDTO>();
            try
            {
                
                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Hostel_Food_Conveyance_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                    //cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = stuDTO.HFC_Flag });
                    cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.VarChar) { Value = stuDTO.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar) { Value = stuDTO.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.VarChar) { Value = stuDTO.ASMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = stuDTO.MI_Id });
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }


        //public ActivateDeactivateStudentDTO getlistone(int id)
        //{
        //    ActivateDeactivateStudentDTO acdmc = new ActivateDeactivateStudentDTO();
        //    try
        //    {
        //        List<AdmissionClass> classlist = new List<AdmissionClass>();
        //        acdmc.classfilllist = (from a in _ActivateDeactivateContext.masterclasscategory
        //                     from b in _ActivateDeactivateContext.admissionClass
        //                     where (a.ASMCL_Id==b.ASMCL_Id && a.ASMAY_Id==id)

        //                         select new ActivateDeactivateStudentDTO
        //                     {
        //                             asmcL_Id = b.ASMCL_Id,
        //                         asmcL_ClassName = b.ASMCL_ClassName
        //                     }
        //     ).ToArray();
        //    }

        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return acdmc;
        //}

        //public ActivateDeactivateStudentDTO getlistthree(ActivateDeactivateStudentDTO stu)
        //{
        //    ActivateDeactivateStudentDTO acdmc = new ActivateDeactivateStudentDTO();
        //    try
        //    {
        //        List<ActivateDeactivateStudentDTO> classlist = new List<ActivateDeactivateStudentDTO>();
        //        stu.studentlist = (from a in _ActivateDeactivateContext.school_Adm_Y_StudentDMO
        //                           from b in _ActivateDeactivateContext.ActivateDeactivateStudentDMO
        //                           where (a.AMST_Id == b.AMST_Id && a.AMAY_Id ==stu.yearid && a.ASMCL_Id==stu.asmcL_Id && a.ASMS_Id==stu.sectionid && b.AMST_SOL==stu.AMST_SOL)
        //                           select new  ActivateDeactivateStudentDTO
        //                           {
        //                               AMST_Id = a.AMST_Id,
        //                               stuFN = b.AMST_FirstName,
        //                               stuMN = b.AMST_MiddleName,
        //                               stuLN = b.AMST_LastName,
        //                               regno = b.AMST_RegistrationNo,
        //                               admno = b.AMST_AdmNo
        //                           }
        //     ).ToArray();

        //    }

        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return stu;
        //}

        //public ActivateDeactivateStudentDTO getlisttwo(ActivateDeactivateStudentDTO stu)
        //{

        //    ActivateDeactivateStudentDTO acdmc = new ActivateDeactivateStudentDTO();
        //    try
        //    {
        //        if (stu.savetmpdata.Count() > 0)
        //        {
        //            foreach (ActivateDeactivateStudentDTO ph in stu.savetmpdata)
        //            {
        //               if(ph.checkedvalue == true)
        //                {
        //                    var Phone_Noresult = _ActivateDeactivateContext.ActivateDeactivateStudentDMO.Single(t => t.AMST_Id == ph.AMST_Id);
        //                    Phone_Noresult.AMST_SOL = stu.AMST_SOL_activate;
        //                    _ActivateDeactivateContext.Update(Phone_Noresult);
        //                    var contactExists= _ActivateDeactivateContext.SaveChanges();

        //                    if(contactExists==1)
        //                    {
        //                        stu.returnval = true;
        //                    }
        //                    else
        //                    {
        //                        stu.returnval = false;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return stu;
        //}
    }
}
