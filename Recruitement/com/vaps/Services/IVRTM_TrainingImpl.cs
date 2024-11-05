using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class IVRTM_TrainingImpl  : Interfaces.IVRTM_TrainingInterface
    {
        public VMSContext _context;
        public DomainModelMsSqlServerContext _db;
        public static string connectionstring = "Data Source=172.16.32.20;Initial Catalog=VMS_Test_New;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
        // public static string connectionstring = "Data Source=dcampus.database.windows.net,1433;Initial Catalog=VMS;Persist Security Info=False;User ID=decampus;Password=Digit@lc@mpu$@1;Connection Timeout=30;";
       
        public IVRTM_TrainingImpl(VMSContext _con, DomainModelMsSqlServerContext Context)
        {
            _context = _con;
            _db = Context;
        }

        //Training Request
        public IVRTM_TrainingDTO onloaddata(IVRTM_TrainingDTO data)
        {
            try
            {
                var hrmeid = _context.IVRM_Staff_User_Login.Where(a => a.Id == data.Userid).Select(t => t.Emp_Code).FirstOrDefault();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = hrmeid });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.emp_deatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "IVRM_Training_TransactionGrid";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = hrmeid });
                            cmd.Parameters.Add(new SqlParameter("@Userid", SqlDbType.BigInt) { Value = data.Userid });
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
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.getloaddetails = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }


                //data.getloaddetails = _context.IVRM_Training_TransactionDMO.Where(t => t.IVRMTT_EmployeeId == hrmeid).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRTM_TrainingDTO saverecord(IVRTM_TrainingDTO data)
        {
            try
            {
                
                long HRMEID = _context.Staff_User_Login.Where(t => t.Id == data.Userid).Select(t => t.Emp_Code).FirstOrDefault();
                data.institutionname = _context.Institution.Where(t => t.MI_Id == data.MI_Id).Select(t => t.MI_Name).FirstOrDefault();
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "IVRM_Training_Details_save";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@EndUserName", SqlDbType.VarChar) { Value = data.IVRMTT_EmployeeName });
                //    cmd.Parameters.Add(new SqlParameter("@ClientHRME_Id", SqlDbType.BigInt) { Value = HRMEID });
                //    cmd.Parameters.Add(new SqlParameter("@RoleID", SqlDbType.BigInt) { Value = data.roleid });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTT_EmployeeEmail", SqlDbType.VarChar) { Value = data.IVRMTT_EmployeeEmail });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTT_EmployeePhone", SqlDbType.VarChar) { Value = data.IVRMTT_EmployeePhone });
                //    cmd.Parameters.Add(new SqlParameter("@clientMI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTT_ClientName", SqlDbType.VarChar) { Value = data.institutionname });
                //    cmd.Parameters.Add(new SqlParameter("@TrainingDate", SqlDbType.DateTime) { Value = data.TrainingDate });
                //    cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveStartTime });
                //    cmd.Parameters.Add(new SqlParameter("@EndTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveEndTime });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTT_TrainingMode", SqlDbType.VarChar) { Value = data.IVRMTT_TrainingMode });
                //    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = "Pending" });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTT_ClientURL", SqlDbType.VarChar) { Value = data.ClientURL });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTT_Remarks", SqlDbType.VarChar) { Value = data.IVRMTT_Remarks });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.BigInt) { Value = data.IVRMTT_Id });
                //    cmd.Parameters.Add(new SqlParameter("@Userid", SqlDbType.BigInt) { Value = data.Userid });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }                        
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "IVRM_Training_Details_save";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EndUserName", SqlDbType.VarChar) { Value = data.IVRMTT_EmployeeName });
                            cmd.Parameters.Add(new SqlParameter("@ClientHRME_Id", SqlDbType.BigInt) { Value = HRMEID });
                            cmd.Parameters.Add(new SqlParameter("@RoleID", SqlDbType.BigInt) { Value = data.roleid });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_EmployeeEmail", SqlDbType.VarChar) { Value = data.IVRMTT_EmployeeEmail });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_EmployeePhone", SqlDbType.VarChar) { Value = data.IVRMTT_EmployeePhone });
                            cmd.Parameters.Add(new SqlParameter("@clientMI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_ClientName", SqlDbType.VarChar) { Value = data.institutionname });
                            cmd.Parameters.Add(new SqlParameter("@TrainingDate", SqlDbType.DateTime) { Value = data.TrainingDate });
                            cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveStartTime });
                            cmd.Parameters.Add(new SqlParameter("@EndTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveEndTime });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_TrainingMode", SqlDbType.VarChar) { Value = data.IVRMTT_TrainingMode });
                            cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = "Pending" });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_ClientURL", SqlDbType.VarChar) { Value = data.ClientURL });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_Remarks", SqlDbType.VarChar) { Value = data.IVRMTT_Remarks });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.BigInt) { Value = data.IVRMTT_Id });
                            cmd.Parameters.Add(new SqlParameter("@Userid", SqlDbType.BigInt) { Value = data.Userid });
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
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (data.IVRMTMT_Id > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "Update";

                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRTM_TrainingDTO Edit(IVRTM_TrainingDTO dto)
        {
            //try
            //{
            //    dto.editdata = _context.External_TrainingDMO.Where(t => t.MI_Id == dto.MI_Id && t.HREXTTRN_Id == dto.HREXTTRN_Id).ToArray();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return dto;
        }
        public IVRTM_TrainingDTO gettrainer(IVRTM_TrainingDTO dto)
        {
            try
            {
                //dto.gettrainer = _context.IVRM_Training_MasterTrainerDMO.Where(t =>t.IVRMTT_Id == dto.IVRMTT_Id).ToArray();


                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "IVRM_Training_MasterTrainerDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.VarChar) { Value = dto.IVRMTT_Id });
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
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                dto.gettrainer = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public IVRTM_TrainingDTO deactiveY(IVRTM_TrainingDTO data)
        {
            try
            {

                long hrmeid = _context.IVRM_Staff_User_Login.Where(a => a.Id == data.Userid).Select(t => t.Emp_Code).FirstOrDefault();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Training_Update_Status";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = hrmeid });
                    cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.BigInt) { Value = data.IVRMTT_Id });
                    cmd.Parameters.Add(new SqlParameter("@TrainerRemarks", SqlDbType.VarChar) { Value = data.IVRMTT_Remarks });
                    cmd.Parameters.Add(new SqlParameter("@TrainingDate", SqlDbType.DateTime) { Value = data.TrainingDate });
                    cmd.Parameters.Add(new SqlParameter("@IVRMTT_TentetiveStartTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveStartTime });
                    cmd.Parameters.Add(new SqlParameter("@IVRMTT_TentetiveEndTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveEndTime });
                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = data.status });
                    cmd.Parameters.Add(new SqlParameter("@TrainingMode", SqlDbType.VarChar) { Value = data.IVRMTT_TrainingMode });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "TrainingActivateDeactivate" });
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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandTimeout = 300;
                            cmd.CommandText = "IVRM_Training_Update_Status";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = hrmeid });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.BigInt) { Value = data.IVRMTT_Id });
                            cmd.Parameters.Add(new SqlParameter("@TrainerRemarks", SqlDbType.VarChar) { Value = data.IVRMTT_Remarks });
                            cmd.Parameters.Add(new SqlParameter("@TrainingDate", SqlDbType.DateTime) { Value = data.TrainingDate });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_TentetiveStartTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveStartTime });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_TentetiveEndTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveEndTime });
                            cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = data.status });
                            cmd.Parameters.Add(new SqlParameter("@TrainingMode", SqlDbType.VarChar) { Value = data.IVRMTT_TrainingMode });
                            cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "TrainingActivateDeactivate" });
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
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.aprovedlist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (data.aprovedlist.Length > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "Update";

                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }


                if (data.aprovedlist.Length > 0)
                {
                    data.message = "Add";
                }
                else
                {
                    data.message = "Update";

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRTM_TrainingDTO trainerfeedback(IVRTM_TrainingDTO data)
        {
            try
            {

                if (data.Trainerfeedback1.Length>0)
                {
                    for (var i=0;i<= data.Trainerfeedback1.Length;i++)
                    {

                        using (SqlConnection connection = new SqlConnection(connectionstring))
                        {
                            try
                            {
                                using (var cmd = connection.CreateCommand())
                                {
                                    cmd.CommandText = "IVRM_Trainer_feedback_save";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@IVRMTMT_Id", SqlDbType.VarChar) { Value = data.Trainerfeedback1[i].IVRMTMT_Id });
                                    cmd.Parameters.Add(new SqlParameter("@feedback", SqlDbType.VarChar) { Value = data.Trainerfeedback1[i].IVRMTMT_Feedback });
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
                                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                                {
                                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                                }
                                                retObject.Add((ExpandoObject)dataRow);
                                            }
                                        }
                                        data.gettrainer = retObject.ToArray();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.Message);
                            }
                        }


                        //var panchdate = _context.Database.ExecuteSqlCommand("IVRM_Trainer_feedback_save @p0,@p1", data.Trainerfeedback1[i].IVRMTMT_Id, data.Trainerfeedback1[i].IVRMTMT_Feedback);
                        if (data.gettrainer.Length > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "notUpdated";
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
        //Training Request END

        //Training Approval
        public IVRTM_TrainingDTO onloaddataRequest(IVRTM_TrainingDTO data)
        {
            try
            {
                var hrmeid = _context.IVRM_Staff_User_Login.Where(a => a.Id == data.Userid).Select(t => t.Emp_Code).FirstOrDefault();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Training_Update_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = hrmeid });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Training_Approval_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = hrmeid });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.aprovedlist = retObject.ToArray();
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
        public IVRTM_TrainingDTO saveData(IVRTM_TrainingDTO data)
        {
            try
            {
                long hrmeid = _context.IVRM_Staff_User_Login.Where(a => a.Id == data.Userid).Select(t => t.Emp_Code).FirstOrDefault();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Training_Update_Status";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = hrmeid });
                    cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.BigInt) { Value = data.IVRMTT_Id });
                    cmd.Parameters.Add(new SqlParameter("@TrainerRemarks", SqlDbType.VarChar) { Value = data.IVRMTT_Remarks });
                    cmd.Parameters.Add(new SqlParameter("@TrainingDate", SqlDbType.DateTime) { Value = data.TrainingDate });
                    cmd.Parameters.Add(new SqlParameter("@IVRMTT_TentetiveStartTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveStartTime });
                    cmd.Parameters.Add(new SqlParameter("@IVRMTT_TentetiveEndTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveEndTime });
                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = data.status });
                    cmd.Parameters.Add(new SqlParameter("@TrainingMode", SqlDbType.VarChar) { Value = data.IVRMTT_TrainingMode });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "TrainingApproval" });
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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandTimeout = 300;
                            cmd.CommandText = "IVRM_Training_Update_Status";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = hrmeid });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.BigInt) { Value = data.IVRMTT_Id });
                            cmd.Parameters.Add(new SqlParameter("@TrainerRemarks", SqlDbType.VarChar) { Value = data.IVRMTT_Remarks });
                            cmd.Parameters.Add(new SqlParameter("@TrainingDate", SqlDbType.DateTime) { Value = data.TrainingDate });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_TentetiveStartTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveStartTime });
                            cmd.Parameters.Add(new SqlParameter("@IVRMTT_TentetiveEndTime", SqlDbType.VarChar) { Value = data.IVRMTT_TentetiveEndTime });
                            cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = data.status });
                            cmd.Parameters.Add(new SqlParameter("@TrainingMode", SqlDbType.VarChar) { Value = data.IVRMTT_TrainingMode });
                            cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "TrainingApproval" });
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
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.aprovedlist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (data.aprovedlist.Length > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "Update";

                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //Training Approval END



                    /////////////////////////////IVRM_Training_Assigning/////////////////////////////////////////////////////////////////////////////
        public IVRTM_TrainingDTO assignonload(IVRTM_TrainingDTO data)
        {
            try
            {
                data.emp_deatils = (from a in _context.HR_Master_Employee_DMO
                                    from b in _context.Multiple_Mobile_DMO
                                    from c in _context.Multiple_Email_DMO
                                    where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id)
                                    select new IVRTM_TrainingDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                        HRMEM_EmailId = c.HRMEM_EmailId,
                                        HRMEMNO_MobileNo = b.HRMEMNO_MobileNo
                                    }).ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Training_RequestTransaction";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getloaddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Training_assigned_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.griddata = retObject.ToArray();
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

        public IVRTM_TrainingDTO EditDetails(IVRTM_TrainingDTO dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Training_Transaction_Edit";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.BigInt) { Value = dto.IVRMTT_Id });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.trainingdetails = retObject.ToArray();
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

        public IVRTM_TrainingDTO saveassign(IVRTM_TrainingDTO data)
        {
            try
            {
                var panchdate = _context.Database.ExecuteSqlCommand("IVRM_Trainer_Details_save @p0,@p1,@p2,@p3,@p4,@p5,@p6", data.IVRMTMT_Id, data.IVRMTT_Id, data.IVRMTMT_TrainerName, data.IVRMTMT_TrainerId, data.IVRMTMT_TrainerEmail, data.IVRMTT_TrainerPhone, data.IVRMTMT_Status);
                if (panchdate > 0)
                {
                    data.message = "Add";
                }
                else
                {
                    data.message = "notUpdated";
                }
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "IVRM_Trainer_Details_save";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@IVRMTMT_Id", SqlDbType.BigInt) { Value = data.IVRMTMT_Id });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTT_Id", SqlDbType.BigInt) { Value = data.IVRMTT_Id });                    
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTMT_TrainerName", SqlDbType.VarChar) { Value = data.IVRMTMT_TrainerName });
                //    cmd.Parameters.Add(new SqlParameter("@TrainerHRME_Id", SqlDbType.BigInt) { Value = data.IVRMTMT_TrainerId });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTMT_TrainerEmail", SqlDbType.VarChar) { Value = data.IVRMTMT_TrainerEmail });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTMT_TrainerPhone", SqlDbType.VarChar) { Value = data.IVRMTT_TrainerPhone });
                //    cmd.Parameters.Add(new SqlParameter("@IVRMTMT_Status", SqlDbType.VarChar) { Value = data.IVRMTMT_Status });                   

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();

                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {

                //        }                        
                //        if (retObject.Count == 0)
                //        {
                //            data.message = "Add";
                //        }
                //        else
                //        {
                //            data.message = "false";
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //=================================report==========================////////////////////////

        public IVRTM_TrainingDTO onloaddatareport(IVRTM_TrainingDTO data)
        {
            try
            {
                //data.getloaddetails = _context.IVRTM_TrainerDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();


                data.emp_deatils = (from a in _context.IVRM_Training_TransactionDMO
                                    from d in _context.HR_Master_Employee_DMO
                                        where (a.IVRMTT_EmployeeId == d.HRME_Id)
                                        select new IVRTM_TrainingDTO
                                        {
                                            HRME_EmployeeFirstName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                            HRME_Id = d.HRME_Id
                                        }).Distinct().ToArray();

                data.trainingdetails = _context.IVRM_Training_TransactionDMO.Where(a => a.IVRMTT_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRTM_TrainingDTO getreport(IVRTM_TrainingDTO data)
        {
            try
            {

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";
                fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Training_Report ";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@from_date", SqlDbType.DateTime)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = contodate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Mode", SqlDbType.VarChar)
                    {
                        Value = data.IVRMTT_TrainingMode
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = data.status
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
                        data.trainingdetails = retObject.ToArray();
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




        public string sendEmail(long MI_Id, string Template, long HRME_ID, string email, string ccemail)        {            try            {                Dictionary<string, string> val = new Dictionary<string, string>();                var template = _context.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template                && e.ISES_MailActiveFlag == true).ToList();                if (template.Count == 0)                {                    return "Email Template not Mapped to the selected Institution";                }                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Resultsms = Mailcontent;                string result = Mailmsg;                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_Email_Prametars_Replace";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",                  SqlDbType.VarChar)                    {                        Value = MI_Id                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",
                SqlDbType.VarChar)                    {                        Value = HRME_ID                    });                    cmd.Parameters.Add(new SqlParameter("@Type",                  SqlDbType.VarChar)                    {                        Value = Template                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                    var datatype = dataReader.GetFieldType(iFiled);                                    if (datatype.Name == "DateTime")                                    {                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else if (datatype.Name == "Decimal")                                    {                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else                                    {                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());                                    }                                }                            }                        }                    }                    catch (Exception ex)                    {                        return ex.Message;                    }                }                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailmsg = result;                        }                    }                }                Mailmsg = result;                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {


                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailcontent = Resultsms;                        }                    }                }                Mailcontent = Resultsms;                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();                string Attechement = "";                if (alldetails.Count > 0)                {                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);                    string Subject = template[0].ISES_MailSubject.ToString();                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();                    smstpdetails = _context.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();                    string mailcc = "";                    string mailbcc = "";

                    //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    //{
                    //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                    //    mailcc = ccmail[0].ToString();

                    //    if (ccmail.Length > 1)
                    //    {
                    //        if (ccmail[1] != null || ccmail[1] != "")
                    //        {
                    //            mailbcc = ccmail[1].ToString();
                    //        }
                    //    }

                    //}
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")                    {                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();                    }                    var message = new SendGridMessage();                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);                    message.Subject = Subject;                    message.AddTo(email);




                    //****************** EMAIL CC DETAILS ***************//

                    if (ccemail != null && ccemail != "")                    {                        string[] ccmaildetails = ccemail.Split(',');                        foreach (var c in ccmaildetails)                        {                            message.AddCc(c);                        }                    }                    //if (mailcc != null && mailcc != "")                    //{                    //    string[] ccmaildetails = mailcc.Split(',');                    //    foreach (var c in ccmaildetails)                    //    {                    //        message.AddCc(c);                    //    }                    //}

                    //if (ccemail != null && ccemail != "")
                    //{
                    //    message.AddBcc(ccemail);
                    //}

                    //****************** EMAIL BCC DETAILS ***************//




                    if (mailcc != null && mailcc != "")                    {
                        //message.AddCc(mailcc);
                    }                    if (mailbcc != null && mailbcc != "")                    {                        message.AddBcc(mailbcc);                    }                    message.HtmlContent = Mailmsg;                    var client = new SendGridClient(sengridkey);                    try                    {                        client.SendEmailAsync(message).Wait();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())                    {                        var template1010 = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();                        cmd.CommandText = "IVRM_Email_Outgoing";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@EmailId",                            SqlDbType.NVarChar)                        {                            Value = email                        });                        cmd.Parameters.Add(new SqlParameter("@Message",                           SqlDbType.NVarChar)                        {                            Value = Mailcontent                        });                        cmd.Parameters.Add(new SqlParameter("@module",                        SqlDbType.VarChar)                        {                            Value = modulename[0]                        });                        cmd.Parameters.Add(new SqlParameter("@MI_Id",                       SqlDbType.BigInt)                        {                            Value = MI_Id                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        try                        {                            using (var dataReader = cmd.ExecuteReader())                            {                            }                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return "success";        }


    }
}
