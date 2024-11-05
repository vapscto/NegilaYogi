using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class ConsolidatesRankReportImp : Interfaces.ConsolidatesRankReportInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
         new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly WrittenTestMarksEntryContext _WrittenTestMarksEntryContext;
        public DomainModelMsSqlServerContext _db;

        public ConsolidatesRankReportImp(WrittenTestMarksEntryContext WrittenTestMarksEntryContext, DomainModelMsSqlServerContext db)
        {
            _WrittenTestMarksEntryContext = WrittenTestMarksEntryContext;
            _db = db;
        }

        public WrittenTestMarksBindDataDTO Getdetails(WrittenTestMarksBindDataDTO data)
        {
            List<MasterAcademic> list = new List<MasterAcademic>();
            list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(d=>d.ASMAY_Order).ToList();
            data.fillyear = list.ToArray();

            data.classlist = (from b in _db.admissioncls
                              where (b.MI_Id == data.MI_Id && b.ASMCL_ActiveFlag == true && b.ASMCL_PreadmFlag==1)
                              select new WrittenTestMarksBindDataDTO
                              {
                                  classname = b.ASMCL_ClassName,
                                  ASMCL_Id = b.ASMCL_Id
                              }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

            List<CasteCategory> allcc = new List<CasteCategory>();
            allcc = _db.castecategory.ToList();
            data.admissioncatdrpall = allcc.ToArray();

            return data;

        }
        public WrittenTestMarksBindDataDTO getclass(WrittenTestMarksBindDataDTO data)
        {
            data.classlist = (from a in _db.School_Adm_Y_StudentDMO
                              from b in _db.admissioncls
                              where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)
                              select new WrittenTestMarksBindDataDTO
                              {
                                  classname = b.ASMCL_ClassName,
                                  ASMCL_Id = b.ASMCL_Id
                              }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

            return data;
        }
        public WrittenTestMarksBindDataDTO Getreport(WrittenTestMarksBindDataDTO data)
        {
          

            try
            {
                //         var query = (from a in _WrittenTestMarksEntryContext.AcademicYear
                //                      from b in _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO
                //                      from c in _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO
                //                      from d in _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO
                //                      from e in _WrittenTestMarksEntryContext.StudentDetailsDMO
                //                      from f in _WrittenTestMarksEntryContext.AdmissionClass
                //                      from g in _WrittenTestMarksEntryContext.allSubject
                //                      where (a.ASMAY_Id == b.ASMAY_Id && b.PASWM_Id == c.PASWM_Id && b.PASWM_Id == d.PASWM_Id && d.PASR_Id == e.PASR_Id && e.ASMCL_Id == f.ASMCL_Id && b.ISMS_ID == g.ISMS_Id && a.MI_Id == 5)
                //                      group new { a, b, c, d, e, f, g }
                //by new { e.PASR_FirstName} into h
                //                      select new WrittenTestMarksBindDataDTO
                //                      {
                //                          totalMarks = h.Sum(m => m.g.ISMS_Max_Marks),
                //                          totalObtain = h.Sum(m => m.d.PASWMS_MarksScored),
                //                          PASR_FirstName = h.FirstOrDefault().e.PASR_FirstName

                //                      }).Distinct().ToList();

                //         data.WirettenTestSubjectWiseStudentMarks = query.ToArray();
                List<WrittenTestMarksBindDataDTO> obj = new List<WrittenTestMarksBindDataDTO>();
                //using (var command = _WrittenTestMarksEntryContext.Database.GetDbConnection().CreateCommand())
                //{
                //    command.CommandText = " select distinct e.PASR_RegistrationNo,sum(g.isms_max_marks) as total_marks,e.PASR_Id, e.PASR_FirstName,sum(d.PASWMS_MarksScored) as MarksScored" +
                //                          ",rank()over(order by sum(d.PASWMS_MarksScored) desc)RNK  " +
                //                          " from Adm_School_M_Academic_Year a inner join Preadmission_Subjectwise_Written_Marks b on a.ASMAY_Id=b.ASMAY_Id " +
                //                          "inner join Preadmission_Schedule_WrritenTest_Marks c on b.PASWM_Id =c.PASWM_Id  " +
                //                          "inner join Preadmission_Subjectwise_Written_Marks_Students d on b.PASWM_Id=d.PASWM_Id " +
                //                          " inner join Preadmission_School_Registration e on d.PASR_Id =e.PASR_Id " +
                //                          " inner join Adm_School_M_Class f on e.ASMCL_Id = f.ASMCL_Id " +
                //                          "inner join IVRM_Master_Subjects g on b.ISMS_Id = g.ISMS_Id " +
                //                          " group by e.PASR_id,e.PASR_FirstName,e.PASR_RegistrationNo";
                //    //"where a.MI_Id = 5  group by e.PASR_id,e.PASR_FirstName,e.PASR_RegistrationNo";
                //    //" + ordderby + "
                //    _WrittenTestMarksEntryContext.Database.OpenConnection();
                //    using (var result12 = command.ExecuteReader())
                //    {
                //        while (result12.Read())
                //        {
                //            obj.Add(new WrittenTestMarksBindDataDTO
                //            {
                //                totalMarks = Convert.ToDecimal(result12["total_marks"]),
                //                totalObtain = Convert.ToDecimal(result12["MarksScored"]),
                //                PASR_FirstName = result12["PASR_FirstName"].ToString(),
                //                rank = Convert.ToInt32(result12["RNK"]),
                //                PASR_RegistrationNo = result12["PASR_RegistrationNo"].ToString()
                //            });
                //        }


                //    }
                //}
                //data.WirettenTestSubjectWiseStudentMarks = obj.ToArray();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_StudentRnk";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@OrderType",
                     SqlDbType.VarChar)
                    {
                        Value = data.order_type
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@CasteCategory_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.CasteCategory_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader =  cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow1);
                            }
                            //data.savelist = retObject.ToArray();

                        }
                        data.WirettenTestSubjectWiseStudentMarks = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

    }
}
