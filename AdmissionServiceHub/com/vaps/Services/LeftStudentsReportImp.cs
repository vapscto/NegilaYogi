using DataAccessMsSqlServerProvider;
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
    public class LeftStudentsReportImp : Interfaces.LeftStudentsReportInterface
    {
        public AdmissionFormContext _context;
        public LeftStudentsReportImp(AdmissionFormContext _cont)
        {
            _context = _cont;
        }
        public LeftStudentsReportDTO loaddata(LeftStudentsReportDTO data)
        {
            try
            {


                data.academic = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //getCategory
        public LeftStudentsReportDTO getCategory(LeftStudentsReportDTO data)
        {
            try
            {
                List<long> cateid = new List<long>();
                foreach (var e in data.academicyears)
                {
                    cateid.Add(e.ASMAY_Id);
                }


                data.category = (from c in _context.AcademicYear
                                 from e in _context.Masterclasscategory
                                 from d in _context.Adm_M_Stu_Cat
                                 where (e.ASMAY_Id == c.ASMAY_Id && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && e.AMC_Id == d.AMC_Id && cateid.Contains(c.ASMAY_Id) 
                                 && e.Is_Active == true && d.AMC_ActiveFlag == 1)
                                 select new LeftStudentsReportDTO
                                 {
                                     AMC_Id = d.AMC_Id,
                                     AMC_Name = d.AMC_Name
                                 }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //getClass
        public LeftStudentsReportDTO getClass(LeftStudentsReportDTO data)
        {
            try
            {

                List<long> cateid = new List<long>();
                foreach (var e in data.academicyears)
                {
                    cateid.Add(e.ASMAY_Id);
                }

                List<long> classid = new List<long>();
                foreach (var e in data.categorylists)
                {
                    classid.Add(e.AMC_Id);
                }


                
                data.classlist = (from c in _context.AcademicYear
                                 from e in _context.Masterclasscategory
                                 from d in _context.Adm_M_Stu_Cat
                                 from f in _context.School_M_Class
                                  where (e.ASMAY_Id == c.ASMAY_Id && e.ASMCL_Id == f.ASMCL_Id  && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && e.AMC_Id == d.AMC_Id && cateid.Contains(c.ASMAY_Id) && classid.Contains(d.AMC_Id)
                                   && e.Is_Active == true && d.AMC_ActiveFlag == 1 && f.ASMCL_ActiveFlag == true)
                                  select f
                                 ).Distinct().OrderBy(g => g.ASMCL_Order).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //getsection
        public LeftStudentsReportDTO getsection(LeftStudentsReportDTO data)
        {
            try
            {

                List<long> cateid = new List<long>();
                foreach (var e in data.academicyears)
                {
                    cateid.Add(e.ASMAY_Id);
                }

                List<long> classid = new List<long>();
                foreach (var e in data.categorylists)
                {
                    classid.Add(e.AMC_Id);
                }

                List<long> Sectionid = new List<long>();
                foreach (var e in data.classlsttwo)
                {
                    Sectionid.Add(e.ASMCL_Id);
                }

               


                data.sectionlist = (from a in _context.School_M_Class
                                   from b in _context.AdmSection
                                   from c in _context.AcademicYear
                                   from d in _context.Masterclasscategory
                                   from e in _context.AdmSchoolMasterClassCatSec
                                   where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                                   && d.MI_Id == data.MI_Id && cateid.Contains(c.ASMAY_Id) && classid.Contains(d.AMC_Id) && Sectionid.Contains(d.ASMCL_Id)
                                   && d.Is_Active == true && e.ASMCCS_ActiveFlg == true && a.ASMCL_ActiveFlag == true)
                                   select b
                                 ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LeftStudentsReportDTO report(LeftStudentsReportDTO data)
        {
            try
            {

                string Academicyear_Id = "0";
                foreach (var e in data.academicyears)
                {
                    Academicyear_Id = Academicyear_Id + "," + e.ASMAY_Id;   //cateid.Add(e.ASMAY_Id);
                }

                string Categary_Id = "0";
                foreach (var e in data.categorylists)
                {
                    Categary_Id = Categary_Id + "," + e.AMC_Id; // classid.Add(e.AMC_Id);
                }

                string Class_Id = "0";
                foreach (var e in data.classlsttwo)
                {
                    Class_Id = Class_Id + "," + e.ASMCL_Id; // Sectionid.Add(e.ASMCL_Id);
                }

                string Section_Id = "0";
                foreach (var e in data.sectionlistarray)
                {
                    Section_Id = Section_Id + "," + e.ASMS_Id; // Sectionids.Add(e.ASMS_Id);
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LEFT_StudentDetails_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        Value = Academicyear_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar)
                    {
                        Value = Categary_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = Class_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Section_Id
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
                        data.viewlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.viewlist = (from a in _context.SchoolYearWiseStudent
                //                 from b in _context.Adm_M_Student
                //                 from c in _context.AcademicYear
                //                 from d in _context.Adm_M_Stu_Cat
                //                 from e in _context.School_M_Class
                //                 from f in _context.AdmSection
                //                 where(a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == c.ASMAY_Id && b.ASMAY_Id == c.ASMAY_Id && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && b.AMC_Id == d.AMC_Id && cateid.Contains(b.ASMAY_Id) && classid.Contains(d.AMC_Id) && Sectionid.Contains(e.ASMCL_Id) && Sectionids.Contains(f.ASMS_Id) && b.AMST_SOL == "L")
                //                 select new LeftStudentsReportDTO
                //                 {
                //                        AMST_FirstName = b.AMST_FirstName,
                //                        AMST_SOL = b.AMST_SOL,
                //                        AMST_AdmNo = b.AMST_AdmNo,
                //                        AMST_MobileNo = b.AMST_MobileNo,
                //                        ASMAY_Year = c.ASMAY_Year

                //                 }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
