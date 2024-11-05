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
    public class StudentDetailsImpl : Interfaces.StudentDetailsInterface
    {
        public ScheduleReportContext _SReportContext;
        public DomainModelMsSqlServerContext _SSReportContext;
        public StudentApplicationContext _StudentApplicationContext;

        public StudentDetailsImpl(StudentApplicationContext StudentApplicationContext, ScheduleReportContext DomainModelContext, DomainModelMsSqlServerContext DomainModelContext1)
        {
            _StudentApplicationContext = StudentApplicationContext;
            _SReportContext = DomainModelContext;
            _SSReportContext = DomainModelContext1;
        }

        public PointsReportDTO getdetails(PointsReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _SReportContext.AcademicYear.Where(t => t.MI_Id == data.mid && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.yeardropDown = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _SReportContext.admissioncls.Where(t => t.MI_Id == data.mid && t.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToList();
                data.fillclass = classname.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<PointsReportDTO> Getreportdetails(PointsReportDTO data)
        {
            var siblinglist = (from a in _SReportContext.StudentApplication
                               from b in _SReportContext.StudentSibling
                               where (a.pasr_id == b.PASR_Id && a.MI_Id == data.mid && a.ASMAY_Id==data.ASMAY_Id && b.PASRS_SiblingsName != "" && b.PASRS_SiblingsName != null && b.PASRS_SiblingsName != "nil" && b.PASRS_SiblingsName != "na" && b.PASRS_SiblingsName != "nan" && b.PASRS_SiblingsName != "no")
                               select new WrittenTestMarksBindDataDTO
                               {
                                   PASR_Id = a.pasr_id,
                                   siblingname = b.PASRS_SiblingsName,
                                   siblingclass = b.PASRS_SiblingsClass,
                                   siblingadmno = b.PASRS_SiblingsAdmissionNo,
                                   siblingsec = b.PASRS_SiblingsSection
                               }
                              ).ToList();

            data.siblinglist = siblinglist.ToArray();

            using (var cmd = _SReportContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "PREADMISSION_DETAILS";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@year",
                SqlDbType.BigInt)
                {
                    Value = data.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.BigInt)
                {
                    Value = data.ASMCL_Id
                });
                cmd.Parameters.Add(new SqlParameter("@type",
                SqlDbType.VarChar)
                {
                    Value = data.type
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
               SqlDbType.BigInt)
                {
                    Value = data.mid
                });
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
                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentDetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
    }
}
