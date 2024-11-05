using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class staffwisereportIMPL : Interfaces.staffwisereportInterface
    {
        public VMSContext _context;
        public staffwisereportIMPL(VMSContext _con)
        {
            _context = _con;
        }
        public staffwisereportDTO onloaddata(staffwisereportDTO data)
        {
            try
            {
                //data.getloaddetails = _context.Master_External_TrainingTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                //data.getloaddetails = (from a in _context.External_TrainingDMO
                //                       from d in _context.HR_Master_Employee_DMO
                //                       where (a.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.HREXTTRN_ActiveFlag==true)
                //                       select new staffwisereportDTO
                //                       {
                //                           EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                //                            HRME_Id = d.HRME_Id
                //                       }).Distinct().ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_External_Training_Request";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.getloaddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }



                //data.stafftrainingreport = (from a in _context.External_TrainingDMO
                //                            from b in _context.Master_External_TrainingCentersDMO
                //                            from e in _context.Master_External_TrainingTypeDMO
                //                            from f in _context.Staff_User_Login
                //                            where (f.Id == data.Userid && a.HRME_Id == f.Emp_Code && a.HREXTTRN_ApprovedFlg == "Rejected" && a.HRMETRCEN_Id == b.HRMETRCEN_Id && a.HRMETRTY_Id == e.HRMETRTY_Id)
                //                            select new staffwisereportDTO
                //                            {
                //                                HREXTTRN_TotalHrs = a.HREXTTRN_TotalHrs,
                //                                HRMETRCEN_CenterAddress = b.HRMETRCEN_CenterAddress,
                //                                HREXTTRN_CertificateFileName = a.HREXTTRN_CertificateFileName,
                //                                HRMETRTY_ExternalTrainingType = e.HRMETRTY_ExternalTrainingType,
                //                                HREXTTRN_TrainingTopic = a.HREXTTRN_TrainingTopic,
                //                                HREXTTRN_StartDate = a.HREXTTRN_StartDate,
                //                                HREXTTRN_EndDate = a.HREXTTRN_EndDate,
                //                                HREXTTRN_StartTime = a.HREXTTRN_StartTime,
                //                                HREXTTRN_EndTime = a.HREXTTRN_EndTime
                //                            }).Distinct().ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_External_Training_stafftrainingreport ";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt)
                    {
                        Value = data.Userid
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
                        data.stafftrainingreport = retObject.ToArray();
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
        public staffwisereportDTO getreport(staffwisereportDTO data)
        {
            try
            {
               

                if (data.HRME_Id > 0)
                {
                    data.staffwisetrainingreport = (from a in _context.External_TrainingDMO
                                                    from b in _context.Master_External_TrainingCentersDMO
                                                    from e in _context.Master_External_TrainingTypeDMO
                                                    where (a.HRME_Id == data.HRME_Id && a.HREXTTRN_ApprovedFlg == "Approved" && a.HRMETRCEN_Id == b.HRMETRCEN_Id && a.HRMETRTY_Id == e.HRMETRTY_Id)
                                                    select new staffwisereportDTO
                                                    {
                                                        HREXTTRN_TotalHrs = a.HREXTTRN_TotalHrs,
                                                        HRMETRCEN_CenterAddress = b.HRMETRCEN_CenterAddress,
                                                        HREXTTRN_CertificateFilePath = a.HREXTTRN_CertificateFilePath,
                                                        HRMETRTY_ExternalTrainingType = e.HRMETRTY_ExternalTrainingType,
                                                        HREXTTRN_TrainingTopic = a.HREXTTRN_TrainingTopic,
                                                        HREXTTRN_StartDate = a.HREXTTRN_StartDate,
                                                        HREXTTRN_EndDate = a.HREXTTRN_EndDate,
                                                        HREXTTRN_StartTime = a.HREXTTRN_StartTime,
                                                        HREXTTRN_EndTime = a.HREXTTRN_EndTime,
                                                        HREXTTRN_ApprovedFlg = a.HREXTTRN_ApprovedFlg,
                                                    }).Distinct().ToArray();

                }

            }
            catch (Exception ex)
            {
                //data.returnval = false;
                //data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}

