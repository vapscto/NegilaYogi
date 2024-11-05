using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Hostel;
using HostelServiceHub.Interface;
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
    public class StudentRequestImpl : Interface.StudentRequestInterface
    {

        public HostelContext _context;
        public StudentRequestImpl(HostelContext context)
        {

            _context = context;
        }
        public async Task<StudentRequestDTO> loaddata(StudentRequestDTO data)
        {
            try
            {

                data.hostel_list = _context.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_ActiveFlag == true).Distinct().ToArray();

                data.room_list = _context.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_ActiveFlag == true).Distinct().ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_SINGLE_STUDENT_DETAILS";
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
                        data.student_wisedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (data.student_wisedata.Length > 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HOSTEL_STUDENT_DETAILS_FOR_GRID";
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
        public StudentRequestDTO save(StudentRequestDTO data)
        {
            try
            {
                if (data.HLHSREQ_Id == 0)
                {

                    var duplicate = _context.HL_Hostel_Student_Request_DMO.Where(t => t.AMST_Id == data.AMST_Id && t.HLMH_Id == data.HLMH_Id && t.HLMRCA_Id == data.HLMRCA_Id && t.HLHSREQ_Id != 0 && t.HLHSREQ_BookingStatus == "Waiting").Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                        return data;
                    }
                    else
                    {
                        HL_Hostel_Student_Request_DMO obj1 = new HL_Hostel_Student_Request_DMO();

                        obj1.HLMH_Id = data.HLMH_Id;
                        obj1.HLMRCA_Id = data.HLMRCA_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.AMST_Id = data.AMST_Id;
                        obj1.HLHSREQ_ActiveFlag = true;
                        obj1.HLHSREQ_ACRoomFlg = data.HLHSREQ_ACRoomFlg;
                        obj1.HLHSREQ_BookingStatus = "Waiting";
                        obj1.HLHSREQ_NonVegMessFlg = data.HLHSREQ_NonVegMessFlg;
                        obj1.HLHSREQ_VegMessFlg = data.HLHSREQ_VegMessFlg;
                        obj1.HLHSREQ_RequestDate = data.HLHSREQ_RequestDate;
                        obj1.HLHSREQ_Remarks = data.HLHSREQ_Remarks;
                        obj1.HLHSREQ_CreatedDate = DateTime.Now;
                        obj1.HLHSREQ_UpdatedDate = DateTime.Now;
                        obj1.HLHSREQ_CreatedBy = data.UserId;
                        obj1.HLHSREQ_UpdatedBy = data.UserId;

                        _context.Add(obj1);

                        HL_Hostel_Student_Request_Confirm_DMO obj = new HL_Hostel_Student_Request_Confirm_DMO();

                        //obj.HLHSREQC_Id = data.HLHSREQC_Id;
                        obj.HLHSREQ_Id = obj1.HLHSREQ_Id;
                        obj.HLHSREQC_Date = DateTime.Now;
                        //obj.HRME_Id = 0;
                        obj.HLMH_Id = data.HLMH_Id;
                        obj.HLMRCA_Id = data.HLMRCA_Id;
                        //obj.AMST_Id = data.AMST_Id;
                        obj.HLHSREQC_ACRoomFlg = data.HLHSREQ_ACRoomFlg;
                        obj.HLHSREQC_SingleRoomFlg = data.HLHSREQ_SingleRoomFlg;
                        obj.HLHSREQC_VegMessFlg = data.HLHSREQ_VegMessFlg;
                        obj.HLHSREQC_NonVegMessFlg = data.HLHSREQ_NonVegMessFlg;
                        obj.HLHSREQC_Remarks = data.HLHSREQ_Remarks;
                        obj.HLHSREQC_BookingStatus = "Waiting";
                        obj.HLHSREQC_ActiveFlag = true;
                        obj.HLHSREQC_CreatedDate = DateTime.Now;
                        obj.HLHSREQC_UpdatedDate = DateTime.Now;
                        obj.HLHSREQC_CreatedBy = data.UserId;
                        obj.HLHSREQC_UpdatedBy = data.UserId;

                        _context.Add(obj);

                    }
                    var w = _context.SaveChanges();
                    if (w > 0)
                    {
                        data.msg = "Saved";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }

                }
                else if (data.HLHSREQ_Id > 0)
                {
                    var duplicate = _context.HL_Hostel_Student_Request_DMO.Where(t => t.HLHSREQ_Id != data.HLHSREQ_Id && t.HLMH_Id == data.HLMH_Id && t.HLMRCA_Id == data.HLMRCA_Id && t.HLHSREQ_BookingStatus == "Waiting").Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                        return data;
                    }
                    else
                    {

                        var j = _context.HL_Hostel_Student_Request_DMO.Where(t => t.HLHSREQ_Id == data.HLHSREQ_Id).SingleOrDefault();

                        j.HLMH_Id = data.HLMH_Id;
                        j.HLMRCA_Id = data.HLMRCA_Id;
                        j.MI_Id = data.MI_Id;
                        j.AMST_Id = data.AMST_Id;
                        j.HLHSREQ_ACRoomFlg = data.HLHSREQ_ACRoomFlg;
                        j.HLHSREQ_BookingStatus = "Waiting";
                        j.HLHSREQ_NonVegMessFlg = data.HLHSREQ_NonVegMessFlg;
                        j.HLHSREQ_VegMessFlg = data.HLHSREQ_VegMessFlg;
                        j.HLHSREQ_RequestDate = data.HLHSREQ_RequestDate;
                        j.HLHSREQ_Remarks = data.HLHSREQ_Remarks;
                        j.HLHSREQ_UpdatedDate = DateTime.Now;
                        j.HLHSREQ_UpdatedBy = data.UserId;

                        _context.Update(j);


                        var updaterequest = _context.HL_Hostel_Student_Request_Confirm_DMO.Where(t => t.HLHSREQ_Id == data.HLHSREQ_Id).Single();

                        //obj.HLHSREQC_Id = data.HLHSREQC_Id;
                        updaterequest.HLHSREQ_Id = data.HLHSREQ_Id;
                        updaterequest.HLHSREQC_Date = data.HLHSREQ_RequestDate;
                        //updaterequest.HRME_Id = 0;
                        updaterequest.HLMH_Id = data.HLMH_Id;
                        updaterequest.HLMRCA_Id = data.HLMRCA_Id;
                        //updaterequest.AMST_Id = data.AMST_Id;
                        updaterequest.HLHSREQC_ACRoomFlg = data.HLHSREQ_ACRoomFlg;
                        updaterequest.HLHSREQC_SingleRoomFlg = data.HLHSREQ_SingleRoomFlg;
                        updaterequest.HLHSREQC_VegMessFlg = data.HLHSREQ_VegMessFlg;
                        updaterequest.HLHSREQC_NonVegMessFlg = data.HLHSREQ_NonVegMessFlg;
                        updaterequest.HLHSREQC_Remarks = data.HLHSREQ_Remarks;
                        updaterequest.HLHSREQC_BookingStatus = "Waiting";
                        updaterequest.HLHSREQC_UpdatedDate = DateTime.Now;
                        updaterequest.HLHSREQC_UpdatedBy = data.UserId;

                        _context.Update(updaterequest);

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

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentRequestDTO edittab1(StudentRequestDTO data)
        {
            try
            {


                data.Editlist = (from d in _context.HL_Hostel_Student_Request_DMO
                                 where (d.HLHSREQ_Id == data.HLHSREQ_Id && d.MI_Id == data.MI_Id)
                                 select d).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentRequestDTO deactive(StudentRequestDTO data)
        {
            try
            {
                var g = _context.HL_Hostel_Student_Request_DMO.Where(t => t.HLHSREQ_Id == data.HLHSREQ_Id).SingleOrDefault();
                if (g.HLHSREQ_ActiveFlag == true)
                {
                    g.HLHSREQ_ActiveFlag = false;
                }
                else if (g.HLHSREQ_ActiveFlag == false)
                {
                    g.HLHSREQ_ActiveFlag = true;
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


