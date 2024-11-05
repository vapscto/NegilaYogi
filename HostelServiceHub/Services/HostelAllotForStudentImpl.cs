using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Hostel;
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
    public class HostelAllotForStudentImpl : Interface.HostelAllotForStudentInterface
    {
        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public HostelAllotForStudentImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public async Task<HostelAllotForStudent_DTO> loaddata(HostelAllotForStudent_DTO data)
        {
            try
            {
                var yeardata = (from a in _dbcontext.AcademicYear
                                where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                select new HostelAllotForStudent_DTO { ASMAY_Id = a.ASMAY_Id, ASMAY_Year = a.ASMAY_Year }).Distinct().ToList();
                if (yeardata.Count > 0)
                {
                    data.yearlist = yeardata.ToArray();
                    data.ASMAY_Id = data.ASMAY_Id;
                }

                data.hostel_list = (from a in _HostelContext.HL_Master_Hostel_DMO
                                    from b in _HostelContext.HL_Hostel_Student_Request_Confirm_DMO
                                    where (a.MI_Id == data.MI_Id && b.HLMH_Id == a.HLMH_Id && a.HLMH_ActiveFlag == true && b.HLHSREQC_ActiveFlag == true && b.HLHSREQC_BookingStatus == "Approved")
                                    select new HostelAllotForStudent_DTO { HLMH_Id = a.HLMH_Id, HLMH_Name = a.HLMH_Name }).Distinct().ToArray();

                data.classlist = (from a in _dbcontext.School_Adm_Y_StudentDMO
                                  from b in _dbcontext.School_M_Class
                                  where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)
                                  select new HostelAllotForStudent_DTO
                                  {
                                      ASMCL_Id = b.ASMCL_Id,
                                      ASMCL_ClassName = b.ASMCL_ClassName
                                  }).Distinct().ToArray();

                data.sectionlist = (from a in _dbcontext.School_Adm_Y_StudentDMO
                                    from b in _dbcontext.School_M_Section
                                    where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                    select new HostelAllotForStudent_DTO
                                    {
                                        ASMS_Id = b.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName
                                    }).Distinct().ToArray();

                data.roomcatgry_list = _HostelContext.HL_Master_Room_Category_DMO.Where(a => a.MI_Id == data.MI_Id && a.HLMRCA_ActiveFlag == true).Distinct().ToArray();

                data.room_list = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_ActiveFlag == true).Distinct().ToArray();

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_ALLOT_FOR_STUDENT";
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
                        data.student_allotlist = retObject.ToArray();
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
        public HostelAllotForStudent_DTO savedata(HostelAllotForStudent_DTO data)
        {
            try
            {
                if (data.HLHSALT_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Hostel_Student_Allot_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.AMST_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Hostel_Student_Allot_DMO obj = new HL_Hostel_Student_Allot_DMO();

                        obj.HLHSALT_Id = data.HLHSALT_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HLHSALT_AllotmentDate = data.HLHSALT_AllotmentDate;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.HLMH_Id = data.HLMH_Id;
                        obj.HLMRCA_Id = data.HLMRCA_Id;
                        obj.AMST_Id = data.AMST_Id;
                        obj.ASMCL_Id = data.ASMCL_Id;
                        obj.ASMS_Id = data.ASMS_Id;
                        obj.HRMRM_Id = data.HRMRM_Id;
                        obj.HLHSALT_NoOfBeds = data.HLHSALT_NoOfBeds;
                        obj.HLHSALT_AllotRemarks = data.HLHSALT_AllotRemarks;
                        obj.HLHSALT_VacateFlg = data.HLHSALT_VacateFlg;
                        obj.HLHSALT_VacatedDate = data.HLHSALT_VacatedDate;
                        obj.HLHSALT_VacateRemarks = data.HLHSALT_VacateRemarks;
                        obj.HLHSALT_ActiveFlag = true;
                        obj.HLHSALT_CreatedDate = DateTime.Now;
                        obj.HLHSALT_UpdatedDate = DateTime.Now;
                        obj.HLHSALT_UpdatedBy = data.UserId;
                        obj.HLHSALT_CreatedBy = data.UserId;
                        _HostelContext.Add(obj);
                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var duplicate = _HostelContext.HL_Hostel_Student_Allot_DMO.Where(t => t.HLHSALT_Id != data.HLHSALT_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.AMST_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Hostel_Student_Allot_DMO.Where(t => t.HLHSALT_Id == data.HLHSALT_Id && t.MI_Id == data.MI_Id).Single();
                        update.HLHSALT_AllotmentDate = data.HLHSALT_AllotmentDate;
                        update.ASMAY_Id = data.ASMAY_Id;
                        update.HLMH_Id = data.HLMH_Id;
                        update.HLMRCA_Id = data.HLMRCA_Id;
                        update.AMST_Id = data.AMST_Id;
                        update.ASMCL_Id = data.ASMCL_Id;
                        update.ASMS_Id = data.ASMS_Id;
                        update.HRMRM_Id = data.HRMRM_Id;
                        update.HLHSALT_NoOfBeds = data.HLHSALT_NoOfBeds;
                        update.HLHSALT_AllotRemarks = data.HLHSALT_AllotRemarks;
                        update.HLHSALT_VacateFlg = data.HLHSALT_VacateFlg;
                        update.HLHSALT_VacatedDate = data.HLHSALT_VacatedDate;
                        update.HLHSALT_VacateRemarks = data.HLHSALT_VacateRemarks;
                        update.HLHSALT_UpdatedDate = DateTime.Now;
                        update.HLHSALT_UpdatedBy = data.UserId;
                        _HostelContext.Update(update);
                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<HostelAllotForStudent_DTO> get_studInfo(HostelAllotForStudent_DTO data)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_HOUSE_WISE_STUDENT_LIST";
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
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HLMH_Id
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
                        data.housewise_studentList = retObject.ToArray();
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
        public HostelAllotForStudent_DTO get_roomdetails(HostelAllotForStudent_DTO data)
        {
            try
            {
                data.HRMRM_BedCapacity = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id == data.HRMRM_Id).Single().HRMRM_BedCapacity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<HostelAllotForStudent_DTO> editdata(HostelAllotForStudent_DTO data)
        {
            try
            {
                var edit = _HostelContext.HL_Hostel_Student_Allot_DMO.Where(a => a.HLHSALT_Id == data.HLHSALT_Id).ToList();

                data.editlist = edit.ToArray();

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_HOUSE_WISE_STUDENT_LIST";
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
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HLMH_Id
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
                        data.housewise_studentList = retObject.ToArray();
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
