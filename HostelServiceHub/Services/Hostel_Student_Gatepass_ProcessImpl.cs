using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
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
    public class Hostel_Student_Gatepass_ProcessImpl : Interface.Hostel_Student_Gatepass_ProcessInterface
    {

        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public Hostel_Student_Gatepass_ProcessImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;

        }
        public Hostel_Student_GatePassDTO getBasicData(Hostel_Student_GatePassDTO dto)
        {
            dto.retrunMsg = "";
            try
            {

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HL_Hostel_Student_Request_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                    {
                        Value = dto.LogInUserId
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.gridlistdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HL_Hostel_Student_Approval_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.approved = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }


        public Hostel_Student_GatePassDTO empdetails(Hostel_Student_GatePassDTO dto)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_student_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar)
                    {
                        Value = dto.AMCST_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.griddisplay = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;

        }


        public Hostel_Student_GatePassDTO approvedrecord(Hostel_Student_GatePassDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HLHSTGP_Id > 0)
                {
                    var result = _HostelContext.HL_Hostel_GatePass_DMO.Single(a => a.HLHSTGP_Id == dto.HLHSTGP_Id);
                    result.HLHSTGP_ApprovedFlg = true;
                    result.HLHSTGP_ComingBackDate = dto.HLHSTGP_ComingBackDate;
                    result.HLHSTGP_ComingBackTime = dto.HLHSTGP_ComingBackTime;
                    result.HLHSTGP_UpdatedDate = DateTime.Now;
                    result.HLHSTGP_CreatedDate = DateTime.Now;
                    result.HLHSTGP_CreatedBy = dto.LogInUserId;
                    result.HLHSTGP_UpdatedBy = dto.LogInUserId;
                    _HostelContext.Update(result);
                    var suc = _HostelContext.SaveChanges();
                    if (suc > 0)
                    {
                        dto.retrunMsg = "Update";
                    }
                    else
                    {
                        dto.retrunMsg = "False";
                    }
                }

                var result1 = _HostelContext.Hostel_Student_GatePass_ApprovalDMO.Where(a => a.HLHSTGPAPP_Id == dto.HLHSTGPAPP_Id).ToList();
                if (result1.Count == 0)
                {
                    Hostel_Student_GatePass_ApprovalDMO passapproval = new Hostel_Student_GatePass_ApprovalDMO();
                    passapproval.HLHSTGPAPP_ActiveFlg = true;
                    passapproval.HLHSTGP_Id = dto.HLHSTGP_Id;
                    passapproval.HLHSTGPAPP_Remarks = dto.HLHSTGPAPP_Remarks;
                    passapproval.Id = dto.LogInUserId;
                    passapproval.HLHSTGPAPP_CreatedDate = DateTime.Now;
                    passapproval.HLHSTGPAPP_UpdatedDate = DateTime.Now;
                    passapproval.HLHSTGPAPP_CreatedBy = dto.LogInUserId;
                    passapproval.HLHSTGPAPP_UpdatedBy = dto.LogInUserId;
                    passapproval.HLHSTGPAPP_Status = dto.HLHSTGPAPP_Status;
                    _HostelContext.Add(passapproval);
                    var scc = _HostelContext.SaveChanges();
                    if (scc > 0)
                    {
                        dto.retrunMsg = "Update";
                    }
                    else
                    {
                        dto.retrunMsg = "False";
                    }
                }
                   
                

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }


        ////------------------  Approval Report------------------------------
        public Hostel_Student_GatePassDTO Onload(Hostel_Student_GatePassDTO dto)
        {
            try
            {

                var employees = _HostelContext.HL_Hostel_GatePass_DMO.Where(R => R.HLHSTGP_ActiveFlg == true).Distinct().ToList();
                List<long> AMCST_Id = new List<long>();
                if (employees.Count > 0)
                {
                    foreach (var d in employees)
                    {
                        AMCST_Id.Add(d.AMCST_Id);
                    }
                }
                dto.employees = _HostelContext.Adm_Master_College_StudentDMO.Where(R => R.AMCST_ActiveFlag == true && AMCST_Id.Contains(R.AMCST_Id)).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public Hostel_Student_GatePassDTO getapprovalreport(Hostel_Student_GatePassDTO dto)
        {
            try
            {
                List<long> AMCST_Id = new List<long>();

                for (var i = 0; i < dto.AMCSTId.Length; i++)
                {
                    AMCST_Id.Add(dto.AMCSTId[i].AMCST_Id);
                }
                string employeeid = string.Join(",", AMCST_Id);

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HL_HOSTEL_GATEPASS_APPROVAL_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar)
                    { Value = employeeid });
                    cmd.Parameters.Add(value: new SqlParameter("@FROMDATE",
              SqlDbType.VarChar)
                    {
                        Value = dto.fromdate.Date.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                  SqlDbType.VarChar)
                    {
                        Value = dto.todate.Date.ToString("yyyy-MM-dd")
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(dtoReader.GetName(iFiled), dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.approvalReport = retObject.ToArray();

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
            return dto;
        }


        //GatePass Admin Apply
        public Hostel_Student_GatePassDTO getGatePassAdminApplyOnload(Hostel_Student_GatePassDTO data)
        {



            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "HL_Hostel_Student_Request_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.admingatepassapplylist = retObject1.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "HL_GetStudent_List";   // Existing 
                    cmd.CommandText = "HL_GetStudent_List_Test"; // Testing
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "collegestudents"
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent = retObject1.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return data;
        }


        public Hostel_Student_GatePassDTO SaveUpdate(Hostel_Student_GatePassDTO dto)        {            dto.retrunMsg = "";            try            {                var contactExistsP = _HostelContext.Database.ExecuteSqlCommand("HL_Hostel_Student_Admin_GatePass_INSERT @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", dto.AMCST_Id, dto.MI_Id, dto.HLHSTGP_Id,                       dto.HLHSTGP_TypeFlg, dto.HLHSTGP_GoingOutDate, dto.HLHSTGP_GoingOutTime, dto.HLHSTGP_Reason, dto.HLHSTGP_ComingBackDate, dto.LogInUserId, dto.HLHSTGP_ActiveFlg);                if (contactExistsP > 0)                {                    dto.retrunMsg = "Add";                }                else                {                    dto.retrunMsg = "false";                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);                dto.retrunMsg = "Error occured";            }            return dto;        }
        public Hostel_Student_GatePassDTO UpdateStatus(Hostel_Student_GatePassDTO dto)        {            dto.retrunMsg = "";            try            {                var contactExistsP = _HostelContext.Database.ExecuteSqlCommand("HL_Hostel_Student_Admin_GatePass_TimeUpdate @p0,@p1,@p2", dto.HLHSTGP_Id, dto.HLHSTGP_CameBackDate, dto.HLHSTGP_CameBackTime);                if (contactExistsP > 0)                {                    dto.retrunMsg = "Add";                }                else                {                    dto.retrunMsg = "false";                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);                dto.retrunMsg = "Error occured";            }            return dto;        }

        public Hostel_Student_GatePassDTO Edit(Hostel_Student_GatePassDTO dto)
        {
            try
            {
                dto.editdata = _HostelContext.HL_Hostel_GatePass_DMO.Where(t => t.MI_Id == dto.MI_Id && t.HLHSTGP_Id == dto.HLHSTGP_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public Hostel_Student_GatePassDTO deactivate(Hostel_Student_GatePassDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //var HLHSTGP_Id = _HostelContext.HL_Hostel_GatePass_DMO.Where(d => d.HLHSTGP_Id == dto.HLHSTGP_Id).ToList();
                if (dto.HLHSTGP_Id > 0)
                {
                    var result = _HostelContext.HL_Hostel_GatePass_DMO.Single(t => t.HLHSTGP_Id == dto.HLHSTGP_Id);

                    if (result.HLHSTGP_ActiveFlg == true)
                    {
                        result.HLHSTGP_ActiveFlg = false;
                    }
                    else if (result.HLHSTGP_ActiveFlg == false)
                    {
                        result.HLHSTGP_ActiveFlg = true;
                    }
                    _HostelContext.Update(result);
                    var flag = _HostelContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HLHSTGP_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }


    }

}
 





