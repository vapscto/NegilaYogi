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
    public class StaffRequestImpl : Interface.StaffRequestInterface
    {
        public HostelContext _context;
        public DomainModelMsSqlServerContext _domainContext;
        public StaffRequestImpl(HostelContext context, DomainModelMsSqlServerContext para)
        {
            _context = context;
            _domainContext = para;
        }
        public async Task<StaffRequestDTO> loaddata(StaffRequestDTO data)
        {
            try
            {
                data.hostel_list = _context.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_ActiveFlag == true).Distinct().ToArray();

                data.room_list = _context.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_ActiveFlag == true).Distinct().ToArray();

                data.HRME_Id = _domainContext.Staff_User_Login.Where(t => t.Id == data.UserId && t.MI_Id == data.MI_Id).Single().Emp_Code;

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_SINGLE_STAFF_DETAILS";
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
                        data.staff_wisedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (data.staff_wisedata.Length > 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HOSTEL_STAFF_DETAILS_FOR_GRID";
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
                            data.all_requestdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StaffRequestDTO save(StaffRequestDTO data)
        {
            try
            {
                var empcode = _domainContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Single().Emp_Code;
                if (data.HLHSTREQ_Id == 0)
                {

                    var duplicate = _context.HL_Hostel_Staff_Request_DMO.Where(t => t.HLMH_Id == data.HLMH_Id && t.HLMRCA_Id == data.HLMRCA_Id && t.HLHSTREQ_Id != 0  && t.MI_Id == data.MI_Id && t.HRME_Id == empcode && t.HLHSTREQ_BookingStatus == "Waiting").Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Hostel_Staff_Request_DMO obj1 = new HL_Hostel_Staff_Request_DMO();

                        obj1.HLMH_Id = data.HLMH_Id;
                        obj1.HLMRCA_Id = data.HLMRCA_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.HRME_Id = empcode;
                        obj1.HLHSTREQ_ActiveFlag = true;
                        obj1.HLHSTREQ_ACRoomReqdFlg = data.HLHSTREQ_ACRoomFlg;
                        obj1.HLHSTREQ_BookingStatus = "Waiting";
                        obj1.HLHSTREQ_NonVegMessReqdFlg = data.HLHSTREQ_NonVegMessFlg;
                        obj1.HLHSTREQ_VegMessReqdFlg = data.HLHSTREQ_VegMessFlg;
                        obj1.HLHSTREQ_RequestDate = data.HLHSTREQ_RequestDate;
                        obj1.HLHSTREQ_Remarks = data.HLHSTREQ_Remarks;
                        obj1.HLHSTREQ_CreatedDate = DateTime.Now;
                        obj1.HLHSTREQ_UpdatedDate = DateTime.Now;
                        obj1.HLHSTREQ_CreatedBy = data.UserId;
                        obj1.HLHSTREQ_UpdatedBy = data.UserId;

                        _context.Add(obj1);

                        HL_Hostel_Staff_Request_Confirm_DMO obj = new HL_Hostel_Staff_Request_Confirm_DMO();

                        //obj.HLHSREQC_Id = data.HLHSREQC_Id;
                        obj.HLHSTREQ_Id = obj1.HLHSTREQ_Id;
                        obj.HLHSTREQC_RequestDate = DateTime.Now;
                        obj.HRME_Id = empcode;
                        obj.HLMH_Id = data.HLMH_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HLMRCA_Id = data.HLMRCA_Id;
                        obj.HLHSTREQC_ACRoomFlg = data.HLHSTREQC_ACRoomFlg;
                        obj.HLHSTREQC_SingleRoomFlg = data.HLHSTREQC_SingleRoomFlg;
                        obj.HLHSTREQC_VegMessFlg = data.HLHSTREQC_VegMessFlg;
                        obj.HLHSTREQC_NonVegMessFlg = data.HLHSTREQ_NonVegMessFlg;
                        obj.HLHSTREQC_Remarks = data.HLHSTREQ_Remarks;
                        obj.HLHSTREQC_BookingStatus = "Waiting";
                        obj.HLHSTREQC_ActiveFlag = true;
                        obj.HLHSTREQC_CreatedDate = DateTime.Now;
                        obj.HLHSTREQC_UpdatedDate = DateTime.Now;
                        obj.HLHSTREQC_CreatedBy = data.UserId;
                        obj.HLHSTREQC_UpdatedBy = data.UserId;

                        _context.Add(obj);

                        var w = _context.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "Saved";
                        }
                        else
                        {
                            data.msg = "Not Saved";
                        }
                    }
                }
                else if (data.HLHSTREQ_Id > 0)
                {
                    var duplicate = _context.HL_Hostel_Staff_Request_DMO.Where(t => t.HLHSTREQ_Id != data.HLHSTREQ_Id && t.HLMH_Id == data.HLMH_Id && t.HLMRCA_Id == data.HLMRCA_Id && t.HRME_Id == empcode && t.HLHSTREQ_RequestDate == data.HLHSTREQ_RequestDate).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var j = _context.HL_Hostel_Staff_Request_DMO.Where(t => t.HLHSTREQ_Id == data.HLHSTREQ_Id).SingleOrDefault();

                        j.HLMH_Id = data.HLMH_Id;
                        j.HLMRCA_Id = data.HLMRCA_Id;
                        j.MI_Id = data.MI_Id;
                        //j.HRME_Id = empcode;
                        j.HLHSTREQ_ACRoomReqdFlg = data.HLHSTREQ_ACRoomFlg;
                        j.HLHSTREQ_BookingStatus = "Waiting";
                        j.HLHSTREQ_NonVegMessReqdFlg = data.HLHSTREQ_NonVegMessFlg;
                        j.HLHSTREQ_VegMessReqdFlg = data.HLHSTREQ_VegMessFlg;
                        j.HLHSTREQ_RequestDate = data.HLHSTREQ_RequestDate;
                        j.HLHSTREQ_UpdatedDate = DateTime.Now;
                        j.HLHSTREQ_UpdatedBy = data.UserId;

                        _context.Update(j);
                    }
                    var updaterequest = _context.HL_Hostel_Staff_Request_Confirm_DMO.Where(t => t.HLHSTREQ_Id == data.HLHSTREQ_Id).Single();

                    //obj.HLHSREQC_Id = data.HLHSREQC_Id;
                    updaterequest.HLHSTREQ_Id = data.HLHSTREQ_Id;
                    updaterequest.HLHSTREQC_RequestDate = data.HLHSTREQ_RequestDate;

                    updaterequest.HLMH_Id = data.HLMH_Id;
                    updaterequest.HLMRCA_Id = data.HLMRCA_Id;
                    updaterequest.HLHSTREQC_ACRoomFlg = data.HLHSTREQ_ACRoomFlg;
                    updaterequest.HLHSTREQC_SingleRoomFlg = data.HLHSTREQ_SingleRoomFlg;
                    updaterequest.HLHSTREQC_VegMessFlg = data.HLHSTREQ_VegMessFlg;
                    updaterequest.HLHSTREQC_NonVegMessFlg = data.HLHSTREQ_NonVegMessFlg;
                    updaterequest.HLHSTREQC_Remarks = data.HLHSTREQ_Remarks;
                    updaterequest.HLHSTREQC_BookingStatus = "Waiting";
                    updaterequest.HLHSTREQC_UpdatedDate = DateTime.Now;
                    updaterequest.HLHSTREQC_UpdatedBy = data.UserId;

                    _context.Update(updaterequest);
                    var r = _context.SaveChanges();
                    if (r > 0)
                    {
                        data.msg = "Updated";
                    }
                    else
                    {
                        data.msg = "Not Updated";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StaffRequestDTO edittab1(StaffRequestDTO data)
        {
            try
            {
                var edit = (from d in _context.HL_Hostel_Staff_Request_DMO
                            where (d.HLHSTREQ_Id == data.HLHSTREQ_Id && d.MI_Id == data.MI_Id)
                            select d).Distinct().ToList();
                data.Editlist = edit.ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StaffRequestDTO deactive(StaffRequestDTO data)
        {
            try
            {
                var g = _context.HL_Hostel_Staff_Request_DMO.Where(t => t.HLHSTREQ_Id == data.HLHSTREQ_Id).SingleOrDefault();
                if (g.HLHSTREQ_ActiveFlag == true)
                {
                    g.HLHSTREQ_ActiveFlag = false;
                }
                else if (g.HLHSTREQ_ActiveFlag == false)
                {
                    g.HLHSTREQ_ActiveFlag = true;
                }

                g.MI_Id = data.MI_Id;
                _context.Update(g);
                int s = _context.SaveChanges();
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

    }
}
