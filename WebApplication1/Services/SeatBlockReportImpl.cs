using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class SeatBlockReportImpl : Interfaces.SeatBlockReportInterface
    {
        public ScheduleReportContext _ScheduleReportContext;
        public SeatBlockReportImpl(ScheduleReportContext DomainModelContext)
        {
            _ScheduleReportContext = DomainModelContext;
        }
        public SeatBlockReportDTO getdetails(SeatBlockReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _ScheduleReportContext.AcademicYear.Where(t => t.MI_Id == data.mid && t.Is_Active == true).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _ScheduleReportContext.admissioncls.Where(t => t.MI_Id == data.mid && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = classname.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //public SeatBlockReportDTO getstuddetails(SeatBlockReportDTO data)
        //{
        //    try
        //    {
        //        if (data.regornamedetails == "regno")
        //        {
        //            data.studentlist = (from a in _ScheduleReportContext.AdmissionStudentDMO
        //                                from b in _ScheduleReportContext.School_Adm_Y_StudentDMO
                                      
        //                                where (a.AMST_Id == b.AMST_Id && b.AMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.ASMCL_Id == data.ASMCL_Id )
        //                                select new SeatBlockReportDTO
        //                                {
        //                                    AMST_Id = a.AMST_Id,
        //                                    AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName,
        //                                    AMST_MiddleName = a.AMST_MiddleName,
        //                                    AMST_LastName = a.AMST_LastName,

        //                                }
        //     ).ToArray();

        //        }

        //        else if (data.regornamedetails == "stdname")
        //        {
        //            data.studentlist = (from a in _ScheduleReportContext.AdmissionStudentDMO
        //                                from b in _ScheduleReportContext.School_Adm_Y_StudentDMO
                                      
        //                                where (a.AMST_Id == b.AMST_Id && b.AMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.ASMCL_Id == data.ASMCL_Id )
        //                                select new SeatBlockReportDTO
        //                                {
        //                                    AMST_Id = a.AMST_Id,
        //                                    AMST_FirstName = a.AMST_FirstName,
        //                                    AMST_MiddleName = a.AMST_MiddleName,
        //                                    AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,

        //                                }
        //      ).ToArray();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);

        //    }
        //    return data;
        //}
        public SeatBlockReportDTO Getstudlist(SeatBlockReportDTO data)
        {
            try
            {
                if (data.stdorregnoflag == "stdnamewise")
                {
                    data.studentlist = (from a in _ScheduleReportContext.student_registration
                                        select new SeatBlockReportDTO
                                        {
                                            PASR_FirstName = a.PASR_FirstName,
                                            PASR_MiddleName = a.PASR_MiddleName,
                                            PASR_LastName = a.PASR_LastName + '_' + a.PASR_RegistrationNo,
                                            PASR_RegistrationNo = a.PASR_RegistrationNo,
                                            pasr_id = a.pasr_id,

                                        }).ToArray();
                }
                else if (data.stdorregnoflag == "regnowise")
                {
                    data.studentlist = (from a in _ScheduleReportContext.student_registration
                                        select new SeatBlockReportDTO
                                        {
                                            PASR_FirstName = a.PASR_RegistrationNo + '_' + a.PASR_FirstName,
                                            PASR_MiddleName = a.PASR_MiddleName,
                                            PASR_LastName = a.PASR_LastName,

                                            PASR_RegistrationNo = a.PASR_RegistrationNo,
                                            pasr_id = a.pasr_id,

                                        }).ToArray();
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<SeatBlockReportDTO> Getreportdetails(SeatBlockReportDTO data)
        {
            try
            {
                string name = "";
                string IVRM_CLM_coloumn = "";

                using (var cmd = _ScheduleReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_Seat_Blocked_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@year",
                             SqlDbType.VarChar)
                    {
                        Value = data.asmayid
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                            SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });

                    cmd.Parameters.Add(new SqlParameter("@classid",
                        SqlDbType.VarChar)
                    {
                        Value = data.asmclid
                    });
                    

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.allreports = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
