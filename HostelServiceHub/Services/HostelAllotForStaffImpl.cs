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
    public class HostelAllotForStaffImpl : Interface.HostelAllotForStaffInterface
    {

        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public HostelAllotForStaffImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public async Task<HostelAllotForStaff_DTO> loaddata(HostelAllotForStaff_DTO data)
        {
            try
            {
                data.desglist = (from a in _dbcontext.HR_Master_Designation
                                 from b in _dbcontext.HR_Master_Employee_DMO
                                 where (a.HRMDES_Id == b.HRMDES_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true
                                 && b.HRME_LeftFlag == false)
                                 select new HostelAllotForStaff_DTO
                                 {
                                     HRMDES_Id = a.HRMDES_Id,
                                     HRMDES_DesignationName = a.HRMDES_DesignationName,
                                 }).Distinct().ToArray();

                data.hostel_list = (from a in _HostelContext.HL_Master_Hostel_DMO
                                    from b in _HostelContext.HL_Hostel_Staff_Request_Confirm_DMO
                                    where (a.MI_Id == data.MI_Id && b.HLMH_Id == a.HLMH_Id && a.HLMH_ActiveFlag == true && b.HLHSTREQC_ActiveFlag == true && b.HLHSTREQC_BookingStatus == "Approved")
                                    select new HostelAllotForStaff_DTO { HLMH_Id = a.HLMH_Id, HLMH_Name = a.HLMH_Name }).Distinct().ToArray();

                data.deptlist = (from a in _dbcontext.HR_Master_Department
                                 from b in _dbcontext.HR_Master_Employee_DMO
                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                 select new HostelAllotForStaff_DTO
                                 {
                                     HRMD_Id = a.HRMD_Id,
                                     HRMD_DepartmentName = a.HRMD_DepartmentName,
                                 }).Distinct().ToArray();

                data.roomcatgry_list = _HostelContext.HL_Master_Room_Category_DMO.Where(a => a.MI_Id == data.MI_Id && a.HLMRCA_ActiveFlag == true).Distinct().ToArray();

                data.room_list = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_ActiveFlag == true).Distinct().ToArray();
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_ALLOT_FOR_STAFF";
                    cmd.CommandType = CommandType.StoredProcedure;

                     cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
        public HostelAllotForStaff_DTO savedata(HostelAllotForStaff_DTO data)
        {
            try
            {
                if (data.HLHSTALT_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Hostel_Staff_Allot_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id == data.HLMRCA_Id && t.HRME_Id == data.HRME_Id && t.HLMH_Id==data.HLMH_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Hostel_Staff_Allot_DMO obj = new HL_Hostel_Staff_Allot_DMO();
                        obj.HLHSTALT_Id = data.HLHSTALT_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HLHSTALT_AllotmentDate = data.HLHSTALT_AllotmentDate;
                        obj.HLMH_Id = data.HLMH_Id;
                        obj.HLMRCA_Id = data.HLMRCA_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.HRMRM_Id = data.HRMRM_Id;
                        //obj.HLHSTALT_NoOfBeds = data.HLHSTALT_NoOfBeds;
                        obj.HLHSTALT_AllotRemarks = data.HLHSTALT_AllotRemarks;
                        obj.HLHSTALT_VacateFlg = false;
                        obj.HLHSTALT_VacatedDate = DateTime.Now;
                        obj.HLHSTALT_VacateRemarks = "";
                        obj.HLHSTALT_ActiveFlag = true;
                        obj.HLHSTALT_CreatedDate = DateTime.Now;
                        obj.HLHSTALT_UpdatedDate = DateTime.Now;
                        obj.HLHSTALT_UpdatedBy = data.UserId;
                        obj.HLHSTALT_CreatedBy = data.UserId;

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
                    var duplicate = _HostelContext.HL_Hostel_Staff_Allot_DMO.Where(t => t.HLHSTALT_Id != data.HLHSTALT_Id && t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Hostel_Staff_Allot_DMO.Where(t => t.HLHSTALT_Id == data.HLHSTALT_Id && t.MI_Id == data.MI_Id).Single();
                        update.HLHSTALT_AllotmentDate = data.HLHSTALT_AllotmentDate;
                        update.HLMH_Id = data.HLMH_Id;
                        update.HLMRCA_Id = data.HLMRCA_Id;
                        update.HRME_Id = data.HRME_Id;
                        update.HRMRM_Id = data.HRMRM_Id;
                       //update.HLHSTALT_NoOfBeds = data.HLHSTALT_NoOfBeds;
                        update.HLHSTALT_AllotRemarks = data.HLHSTALT_AllotRemarks;
                        update.HLHSTALT_VacateFlg = false;
                        update.HLHSTALT_VacatedDate = DateTime.Now;
                        update.HLHSTALT_VacateRemarks = "";
                        update.HLHSTALT_UpdatedDate = DateTime.Now;
                        update.HLHSTALT_UpdatedBy = data.UserId;
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
        public async Task<HostelAllotForStaff_DTO> get_studInfo(HostelAllotForStaff_DTO data)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_HOUSE_WISE_STAFF_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
        public HostelAllotForStaff_DTO get_roomdetails(HostelAllotForStaff_DTO data)
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
        public async Task<HostelAllotForStaff_DTO> editdata(HostelAllotForStaff_DTO data)
        {
            try
            {
                var edit = _HostelContext.HL_Hostel_Staff_Allot_DMO.Where(a => a.HLHSTALT_Id == data.HLHSTALT_Id).ToList();

                data.editlist = edit.ToArray();

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_HOUSE_WISE_STAFF_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
        public HostelAllotForStaff_DTO deactivYTab1(HostelAllotForStaff_DTO data)
        {
            try
            {
                var g = _HostelContext.HL_Hostel_Staff_Allot_DMO.Where(t => t.HLHSTALT_Id == data.HLHSTALT_Id).SingleOrDefault();
                if (g.HLHSTALT_ActiveFlag == true)
                {
                    g.HLHSTALT_ActiveFlag = false;
                }
                else
                {
                    g.HLHSTALT_ActiveFlag = true;
                }
                g.HLHSTALT_UpdatedDate = DateTime.Now;
                g.HLHSTALT_UpdatedBy = data.UserId;
                g.MI_Id = data.MI_Id;
                _HostelContext.Update(g);
                int s = _HostelContext.SaveChanges();
                if (s > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HostelAllotForStaff_DTO getdesg(HostelAllotForStaff_DTO data)
        {
            try
            {
                data.desglist = (from a in _dbcontext.HR_Master_Designation
                                 from b in _dbcontext.HR_Master_Employee_DMO
                                 where (a.HRMDES_Id == b.HRMDES_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true
                                 && b.HRME_LeftFlag == false)
                                 select new HostelAllotForStaff_DTO
                                 {
                                     HRMDES_Id = a.HRMDES_Id,
                                     HRMDES_DesignationName = a.HRMDES_DesignationName,
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
