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
    public class CLGHostelVacantIMPL : Interface.CLGHostelVacantInterface
    {
        public HostelContext _context;
        public AdmissionFormContext _admcontext;

        public CLGHostelVacantIMPL(HostelContext context1, AdmissionFormContext context2)
        {
            _context = context1;
            _admcontext = context2;
        }
        public async Task<CLGHostelVacantDTO> loaddata(CLGHostelVacantDTO data)
        {
            try
            {
                data.studentdata = (from a in _context.HL_Hostel_Student_Allot_College_DMO
                                    from b in _context.Adm_Master_College_StudentDMO
                                    from c in _context.Adm_College_Yearly_StudentDMO
                                    where (c.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && c.AMCST_Id == b.AMCST_Id && c.AMCST_Id == a.AMCST_Id && b.AMCST_SOL == "S" && b.MI_Id == a.MI_Id && (a.HLHSALTC_VacateFlg == false || a.HLHSALTC_VacateFlg == null))
                                    select new CLGHostelVacantDTO
                                    {
                                        studentname = ((b.AMCST_FirstName == null ? " " : b.AMCST_FirstName) + " " + (b.AMCST_MiddleName == null ? " " : b.AMCST_MiddleName) + " " + (b.AMCST_LastName == null ? " " : b.AMCST_LastName)).Trim(),
                                        AMCST_Id = b.AMCST_Id,
                                        AMCST_AdmNo = b.AMCST_AdmNo,
                                        HLHSALTC_AllotmentDate = a.HLHSALTC_AllotmentDate,
                                    }).Distinct().ToArray();

                data.staffdata = (from a in _context.HL_Hostel_Staff_Allot_DMO
                                  from b in _context.HR_Master_Employee_DMO
                                  where (b.MI_Id == data.MI_Id && b.HRME_Id == a.HRME_Id && b.HRME_LeftFlag == false && b.HRME_ActiveFlag == true && b.MI_Id == a.MI_Id)
                                  select new CLGHostelVacantDTO
                                  {
                                      staffname = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                      HRME_Id = b.HRME_Id,
                                      HLHSALT_AllotmentDate = a.HLHSTALT_AllotmentDate,
                                  }).Distinct().ToArray();

                data.guestdata = (from a in _context.HL_Hostel_Guest_Allot_DMO
                                  where a.MI_Id == data.MI_Id && a.HLHGSTALT_ActiveFlag == true && a.HLHGSTALT_VacateFlg == false
                                  select new CLGHostelVacantDTO
                                  {
                                      HLHGSTALT_GuestName = a.HLHGSTALT_GuestName,
                                      HLHGSTALT_Id = a.HLHGSTALT_Id,
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

        public CLGHostelVacantDTO edittab1(CLGHostelVacantDTO data)
        {
            try
            {
                if (data.type == "student")
                {
                    var updateVacatedata = _context.HL_Hostel_Student_Allot_College_DMO.Where(t => t.AMCST_Id == data.AMCST_Id && t.MI_Id == data.MI_Id && t.HLHSALTC_VacateFlg==false).Single();
                    var Requesttable = _context.HL_Hostel_Student_Request_College_DMO.Where(t => t.AMCST_Id == data.AMCST_Id && t.MI_Id==data.MI_Id  && t.HLHSREQC_Id != 0 && t.HLHSREQC_ActiveFlag == true && t.HLHSREQC_BookingStatus == "Approved" ).Single();



                    Requesttable.HLHSREQC_BookingStatus = "Vacated";
                    updateVacatedata.HLHSALTC_VacateFlg = true;
                    // updateVacatedata.HLHSALT_AllotmentDate = data.HLHSALT_AllotmentDate;
                    updateVacatedata.HLHSALTC_VacatedDate = data.HLHSALTC_VacatedDate;
                    updateVacatedata.HLHSALTC_VacateRemarks = data.HLHSALTC_VacateRemarks;
                    updateVacatedata.HLHSALTC_UpdatedDate = DateTime.Now;
                    updateVacatedata.HLHSALTC_UpdatedBy = data.UserId;

                    _context.Update(updateVacatedata);
                    _context.Update(Requesttable);

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
        public async Task<CLGHostelVacantDTO> getalldetailsOnselectiontype(CLGHostelVacantDTO data)
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

        public async Task<CLGHostelVacantDTO> get_studentDetail(CLGHostelVacantDTO data)
        {
            try
            {
               

               
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_STUDENT_SELECTION_DATA";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
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
                  
                }


                List<CLGHostelVacantDTO> devicelist = new List<CLGHostelVacantDTO>();
                //var ddata = new List();
                var ddata = new { };
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "CLG_HOSTEL_STUDENT_SELECTION_DATA";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
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
                                devicelist.Add(new CLGHostelVacantDTO
                                {

                                    AMCST_Sex = Convert.ToString(dataReader["AMCST_Sex"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                CLGHostelVacantDTO dto = new CLGHostelVacantDTO();
                dto.devicelist12 = devicelist.ToList();
                var gender = "";
                if (dto.devicelist12.Count > 0)
                {
                    foreach (var r in dto.devicelist12)
                    {
                        if (r.AMCST_Sex == "Male")
                        {
                            gender = "Boys";
                        }
                        if (r.AMCST_Sex == "Female")
                        {
                            gender = "Girls";
                        }
                    }
                }

                data.hostel_list = _context.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_BoysGirlsFlg == gender && t.HLMH_ActiveFlag == true).Distinct().ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<CLGHostelVacantDTO> get_staffDetail(CLGHostelVacantDTO data)
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
        public CLGHostelVacantDTO get_guestDetail(CLGHostelVacantDTO data)
        {
            try
            {
                data.guest_details = (from a in _context.HL_Hostel_Guest_Allot_DMO
                                      from b in _context.HL_Master_Hostel_DMO
                                      from c in _context.HR_Master_Room_DMO
                                      where (a.MI_Id == data.MI_Id && a.HLHGSTALT_Id == data.HLHGSTALT_Id && b.HLMH_Id == a.HLMH_Id && c.HRMRM_Id == a.HRMRM_Id)
                                      select new StudentVacantDTO
                                      {
                                          HLHGSTALT_Id = a.HLHGSTALT_Id,
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

