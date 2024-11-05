using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Hostel;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;


namespace HostelServiceHub.Services
{
    public class Hostel_Student_GatePassImpl : Interface.Hostel_Student_GatePassInterface
    {
        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public Hostel_Student_GatePassImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
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
                    cmd.CommandText = "HL_Hostel_Student_Gatepass_Request";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value =dto.MI_Id
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
            }
            catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                    dto.retrunMsg = "Error occured";
                }

                return dto;
            }

        public Hostel_Student_GatePassDTO SaveUpdate(Hostel_Student_GatePassDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                var contactExistsP = _HostelContext.Database.ExecuteSqlCommand("HL_Hostel_Student_GatePass_INSERT @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", dto.MI_Id, dto.HLHSTGP_Id,
                       dto.HLHSTGP_TypeFlg, dto.HLHSTGP_GoingOutDate, dto.HLHSTGP_GoingOutTime, dto.HLHSTGP_Reason, dto.HLHSTGP_ComingBackDate, dto.LogInUserId,dto.HLHSTGP_ActiveFlg);
                if (contactExistsP > 0)
                {
                    dto.retrunMsg = "Add";
                }

                else
                {
                    dto.retrunMsg = "false";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public Hostel_Student_GatePassDTO Edit(Hostel_Student_GatePassDTO dto)
        {
            try
            {
                dto.editdata = _HostelContext.HL_Hostel_GatePass_DMO.Where(t =>t.MI_Id==dto.MI_Id && t.HLHSTGP_Id == dto.HLHSTGP_Id).ToArray();             

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
