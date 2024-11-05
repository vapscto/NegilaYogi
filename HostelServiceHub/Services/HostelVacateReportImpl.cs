using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Services
{
    public class HostelVacateReportImpl : Interface.HostelVacateReportInterface
    {
        public HostelContext _context;
        public AdmissionFormContext _admcontext;

        public HostelVacateReportImpl(HostelContext context1, AdmissionFormContext context2)
        {
            _context = context1;
            _admcontext = context2;
        }

        public HostelVacateReportDTO loaddata(HostelVacateReportDTO data)
        {
            try
            {
                data.yerlist = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).Distinct().ToArray();
                data.hostelist = _context.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_ActiveFlag == true).Distinct().ToArray();
                data.guestlist = _context.HL_Hostel_Guest_Allot_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHGSTALT_ActiveFlag == true).Distinct().ToArray();

                data.stafflist = (from a in _context.HL_Hostel_Staff_Allot_DMO
                                  from b in _context.HR_Master_Employee_DMO
                                  where (b.MI_Id == data.MI_Id && b.HRME_Id == a.HRME_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && b.MI_Id == a.MI_Id)
                                  select new HostelVacateReportDTO
                                  {
                                      HRME_Id = b.HRME_Id,
                                      HRME_EmployeeFirstName = b.HRME_EmployeeFirstName
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HostelVacateReportDTO get_Studentlist(HostelVacateReportDTO data)
        {
            try
            {
                data.studentlist = (from a in _context.HL_Hostel_Student_Allot_DMO
                                    from b in _context.Adm_M_Student
                                    from c in _context.SchoolYearWiseStudent
                                    where (c.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && c.AMST_Id == b.AMST_Id && c.AMST_Id == a.AMST_Id && b.AMST_SOL == "S" && b.MI_Id == a.MI_Id)
                                    select new StudentVacantDTO
                                    {
                                        studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                        AMST_Id = b.AMST_Id,
                                        AMST_AdmNo = b.AMST_AdmNo
                                    }).Distinct().ToArray();
            }
            catch (Exception xx)
            {
                Console.WriteLine(xx.Message);
            }
            return data;
        }
        public async Task<HostelVacateReportDTO> get_report(HostelVacateReportDTO data)
        {
            try
            {
                string hostel_ids = "0";
                string student_idss = "0";
                string staff_id = "0";
                string guest_id = "0";


                List<long> hstl_ids = new List<long>();
                List<long> stud_ids = new List<long>();
                List<long> staf_id = new List<long>();
                List<long> gust_id = new List<long>();

                if (data.type2 == "Hostel")
                {
                    foreach (var item in data.SelectedHostellist)
                    {
                        hstl_ids.Add(item.HLMH_Id);
                    }
                    for (int s = 0; s < hstl_ids.Count(); s++)
                    {
                        hostel_ids = hostel_ids + ',' + hstl_ids[s].ToString();
                    }
                }
                else if (data.type2 == "individual")
                {
                    if (data.type == "student")
                    {
                        foreach (var item in data.SelectedStudentlist)
                        {
                            stud_ids.Add(item.AMST_Id);
                        }
                        for (int s = 0; s < stud_ids.Count(); s++)
                        {
                            student_idss = student_idss + ',' + stud_ids[s].ToString();
                        }
                    }
                    else if (data.type == "staff")
                    {
                        foreach (var item in data.Selectedstafflist)
                        {
                            staf_id.Add(item.HRME_Id);
                        }
                        for (int d = 0; d < staf_id.Count(); d++)
                        {
                            staff_id = staff_id + ',' + staf_id[d].ToString();
                        }

                    }
                    else if (data.type == "guest")
                    {
                        foreach (var item in data.Selectedguestlist)
                        {
                            gust_id.Add(item.HLHGSTALT_Id);
                        }
                        for (int d = 0; d < gust_id.Count(); d++)
                        {
                            guest_id = guest_id + ',' + gust_id[d].ToString();
                        }
                    }
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_VACATE_REPORT_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = data.type
                    });
                    cmd.Parameters.Add(new SqlParameter("@type2",
                        SqlDbType.VarChar)
                    {
                        Value = data.type2
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",
                        SqlDbType.DateTime)
                    {
                        Value = data.Fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                        SqlDbType.DateTime)
                    {
                        Value = data.ToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id",
                        SqlDbType.VarChar)
                    {
                        Value = hostel_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                        SqlDbType.VarChar)
                    {
                        Value = student_idss
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.VarChar)
                    {
                        Value = staff_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HLHGSTALT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = guest_id
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
                        data.gridlistdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return data;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
    }
}
