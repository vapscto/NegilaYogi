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
    public class StudentVacantImpl : Interface.StudentVacantInterface
    {
        public HostelContext _context;
        public AdmissionFormContext _admcontext;

        public StudentVacantImpl(HostelContext context1, AdmissionFormContext context2)
        {
            _context = context1;
            _admcontext = context2;
        }
        public async Task<StudentVacantDTO> loaddata(StudentVacantDTO data)
        {
            try
            {
                data.studentdata = (from a in _context.HL_Hostel_Student_Allot_DMO
                                    from b in _context.Adm_M_Student
                                    from c in _context.SchoolYearWiseStudent
                                    where ( b.MI_Id == data.MI_Id && c.AMST_Id == b.AMST_Id && c.AMST_Id == a.AMST_Id && b.AMST_SOL == "S" && b.MI_Id == a.MI_Id)
                                    select new StudentVacantDTO
                                    {
                                        studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                        AMST_Id = b.AMST_Id,
                                        AMST_AdmNo = b.AMST_AdmNo,
                                        HLHSALT_AllotmentDate = a.HLHSALT_AllotmentDate,
                                    }).Distinct().ToArray();

                data.staffdata = (from a in _context.HL_Hostel_Staff_Allot_DMO
                                  from b in _context.HR_Master_Employee_DMO
                                  where (b.MI_Id == data.MI_Id && b.HRME_Id == a.HRME_Id && b.HRME_LeftFlag == false && b.HRME_ActiveFlag == true && b.MI_Id == a.MI_Id)
                                  select new StudentVacantDTO
                                  {
                                      staffname = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                      HRME_Id = b.HRME_Id,
                                      HLHSALT_AllotmentDate = a.HLHSTALT_AllotmentDate,
                                  }).Distinct().ToArray();
                data.guestdata = (from a in _context.HL_Hostel_Guest_Allot_DMO
                                  where a.MI_Id == data.MI_Id && a.HLHGSTALT_ActiveFlag == true && a.HLHGSTALT_VacateFlg==false
                                  select new StudentVacantDTO
                                  {
                                      HLHGSTALT_GuestName = a.HLHGSTALT_GuestName,
                                      HLHGSTALT_Id=a.HLHGSTALT_Id,
                                  }).Distinct().ToArray();
               


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_VACANT_GRID_LIST";
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public StudentVacantDTO edittab1(StudentVacantDTO data)
        {
            try
            {
                if (data.type == "student")
                {
                    var updateVacatedata = _context.HL_Hostel_Student_Allot_DMO.Where(t => t.AMST_Id == data.AMST_Id && t.MI_Id == data.MI_Id).Single();

                    updateVacatedata.HLHSALT_VacateFlg = true;
                   // updateVacatedata.HLHSALT_AllotmentDate = data.HLHSALT_AllotmentDate;
                    updateVacatedata.HLHSALT_VacatedDate = data.HLHSALT_VacatedDate;
                    updateVacatedata.HLHSALT_VacateRemarks = data.HLHSALT_VacateRemarks;
                    updateVacatedata.HLHSALT_UpdatedDate = DateTime.Now;
                    updateVacatedata.HLHSALT_UpdatedBy = data.UserId;

                    _context.Update(updateVacatedata);

                    var r = _context.SaveChanges();
                    if (r > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "failed";
                    }
                }
                else if (data.type == "staff")
                {
                    var updateVacatedata2 = _context.HL_Hostel_Staff_Allot_DMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id).Single();

                    updateVacatedata2.HLHSTALT_VacateFlg = true;
                   
                    updateVacatedata2.HLHSTALT_VacatedDate = data.HLHSTALT_VacatedDate;
                    updateVacatedata2.HLHSTALT_VacateRemarks = data.HLHSTALT_VacateRemarks;
                    updateVacatedata2.HLHSTALT_UpdatedDate = DateTime.Now;
                    updateVacatedata2.HLHSTALT_UpdatedBy = data.UserId;

                    _context.Update(updateVacatedata2);

                    var r = _context.SaveChanges();
                    if (r > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "failed";
                    }
                }
                else if (data.type == "guest")
                {
                    var updateVacatedata3 = _context.HL_Hostel_Guest_Allot_DMO.Where(t => t.HLHGSTALT_Id == data.HLHGSTALT_Id && t.MI_Id == data.MI_Id).Single();

                    updateVacatedata3.HLHGSTALT_VacateFlg = true;

                    updateVacatedata3.HLHGSTALT_VacatedDate = data.HLHGSTALT_VacatedDate;
                    updateVacatedata3.HLHGSTALT_VacateRemarks = data.HLHGSTALT_VacateRemarks;
                    updateVacatedata3.HLHGSTALT_UpdatedDate = DateTime.Now;
                    updateVacatedata3.HLHGSTALT_UpdatedBy = data.UserId;

                    _context.Update(updateVacatedata3);

                    var r = _context.SaveChanges();
                    if (r > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "failed";
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<StudentVacantDTO> getalldetailsOnselectiontype(StudentVacantDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_VACANT_GRID_LIST";
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public async Task<StudentVacantDTO> get_studentDetail(StudentVacantDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_STUDENT_SELECTION_DATA";
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
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public async Task<StudentVacantDTO> get_staffDetail(StudentVacantDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_STAFF_SELECTION_DATA";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.staffdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public StudentVacantDTO get_guestDetail(StudentVacantDTO data)
        {
            try
            {
                data.guest_details = (from a in _context.HL_Hostel_Guest_Allot_DMO
                                  from b in _context.HL_Master_Hostel_DMO
                                  from c in _context.HR_Master_Room_DMO
                                  where (a.MI_Id == data.MI_Id && a.HLHGSTALT_Id == data.HLHGSTALT_Id && b.HLMH_Id==a.HLMH_Id && c.HRMRM_Id==a.HRMRM_Id)
                                  select new StudentVacantDTO
                                  {
                                      HLHGSTALT_Id= a.HLHGSTALT_Id,
                                      HLHGSTALT_AllotmentDate = a.HLHGSTALT_AllotmentDate,
                                      HLMH_Name = b.HLMH_Name,
                                      HRMRM_RoomNo = c.HRMRM_RoomNo,

                                  }).Distinct().ToArray(); 


               
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return data;
        }

    }
}
