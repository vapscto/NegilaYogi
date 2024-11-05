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
    public class SummaryreportsIMPL : Interfaces.SummaryreportsInterface
    {
        public VMSContext _context;
        public SummaryreportsIMPL(VMSContext _con)
        {
            _context = _con;
        }
        public SummaryreportsDTO onloaddata(SummaryreportsDTO data)
        {
            try
            {
                data.getloaddetails = _context.Master_External_TrainingTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                //data.employeedetails = _context.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.employeedetails = (from a in _context.External_TrainingDMO
                                        from d in _context.HR_Master_Employee_DMO
                                        where (a.HRME_Id == d.HRME_Id && a.MI_Id == d.MI_Id)
                                        select new SummaryreportsDTO
                                        {
                                            EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                            HRME_Id = d.HRME_Id
                                        }).Distinct().ToArray();


                //inhouse
                data.internalemployeedetails = (from a in _context.HR_Training_Create_Participants_DMO_con
                                                from b in _context.HR_Training_Create_DMO_con
                                                from d in _context.HR_Master_Employee_DMO
                                                where (a.HRME_Id == d.HRME_Id && b.MI_Id == d.MI_Id && a.HRTCR_Id == b.HRTCR_Id)
                                                select new SummaryreportsDTO
                                                {
                                                    EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                                    HRME_Id = d.HRME_Id
                                                }).Distinct().ToArray();

                data.programname = (from a in _context.HR_Training_Create_DMO_con
                                    where (a.MI_Id == data.MI_Id)
                                    select new SummaryreportsDTO
                                    {
                                        HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                        HRTCR_Id = a.HRTCR_Id
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public SummaryreportsDTO getreport(SummaryreportsDTO data)
        {
            try
            {
                //if (data.Rvalue == 0)
                //{
                //    data.summaryreport = (from a in _context.External_TrainingDMO
                //                          from b in _context.External_Training_ApprovalDMO
                //                          from c in _context.Master_External_TrainingTypeDMO
                //                          where (a.HREXTTRN_ApprovedFlg == "Approved" && a.HRMETRTY_Id == c.HRMETRTY_Id)
                //                          select new SummaryreportsDTO
                //                          {
                //                              HRMETRTY_ExternalTrainingType = c.HRMETRTY_ExternalTrainingType,
                //                              HRMETRTY_MinimumTrainingHrs = c.HRMETRTY_MinimumTrainingHrs,
                //                              HREXTTRN_TotalHrs = a.HREXTTRN_TotalHrs                                             

                //                          }).Distinct().ToArray();
                //}
                //else
                //{
                //    data.summaryreport = (from a in _context.External_TrainingDMO
                //                          from d in _context.HR_Master_Employee_DMO
                //                          from c in _context.Master_External_TrainingTypeDMO
                //                          where (a.HRME_Id == d.HRME_Id && a.HREXTTRN_ApprovedFlg == "Approved" && a.HRMETRTY_Id == c.HRMETRTY_Id)
                //                          select new SummaryreportsDTO
                //                          {
                //                              EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                //                              HRMETRTY_MinimumTrainingHrs = c.HRMETRTY_MinimumTrainingHrs,
                //                              HRMETRTY_ExternalTrainingType = c.HRMETRTY_ExternalTrainingType,
                //                              HREXTTRN_TotalHrs = a.HREXTTRN_TotalHrs,
                //                              HRME_Id = a.HRME_Id
                //                          }).Distinct().ToArray();
                //}
                if (data.Rvalue == 0)
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
                        cmd.CommandText = "HR_Overall_Training_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)
                        {
                            Value = contodate
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
                            data.summaryreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                else
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
                        cmd.CommandText = "HR_Training_Calculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)
                        {
                            Value = contodate
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
                            data.summaryreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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

        public SummaryreportsDTO inhouseReportreport(SummaryreportsDTO data)
        {
            try
            {
                if (data.HRME_Id > 0)
                {
                    data.internalreport = (from a in _context.HR_Training_Create_Participants_DMO_con
                                           from b in _context.HR_Training_Create_DMO_con
                                           from d in _context.HR_Master_Employee_DMO
                                           where (a.HRME_Id == data.HRME_Id && a.HRME_Id == d.HRME_Id && b.MI_Id == d.MI_Id && a.HRTCR_Id == b.HRTCR_Id)
                                           select new SummaryreportsDTO
                                           {
                                               //  EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                               HRME_Id = d.HRME_Id,
                                               HRTCR_Id = b.HRTCR_Id,
                                               HRTCR_PrgogramName = b.HRTCR_PrgogramName,
                                               HRTCR_StatusFlg = b.HRTCR_StatusFlg,
                                               HRTCR_StartDate = b.HRTCR_StartDate,
                                               HRTCR_EndDate = b.HRTCR_EndDate
                                           }).Distinct().ToArray();
                }
                else if (data.HRTCR_Id > 0)
                {
                    data.internalreport = (from a in _context.HR_Training_Create_Participants_DMO_con
                                           from b in _context.HR_Training_Create_DMO_con
                                           from d in _context.HR_Master_Employee_DMO
                                           where (a.HRTCR_Id == data.HRTCR_Id && a.HRME_Id == d.HRME_Id && b.MI_Id == d.MI_Id && a.HRTCR_Id == b.HRTCR_Id)
                                           select new SummaryreportsDTO
                                           {
                                               EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                               HRME_Id = d.HRME_Id,
                                               HRTCR_Id = b.HRTCR_Id,
                                               HRTCR_PrgogramName = b.HRTCR_PrgogramName,
                                               HRTCR_StatusFlg = b.HRTCR_StatusFlg,
                                               HRTCR_StartDate = b.HRTCR_StartDate,
                                               HRTCR_EndDate = b.HRTCR_EndDate
                                           }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SummaryreportsDTO trainingcertificate(SummaryreportsDTO data)
        {
            try
            {

                //var result = _context.IVRM_PrincipalDMO.Where(R => R.MI_Id == data.MI_Id).FirstOrDefault();

                //data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == result.IVRMUL_Id).Select(t => t.Emp_Code).FirstOrDefault();


                //if (data.HRME_Id > 0)
                //{
                data.certificatedetails = (from a in _context.HR_Training_Create_Participants_DMO_con
                                           from b in _context.HR_Training_Create_DMO_con
                                           from c in _context.Institution
                                           from d in _context.HR_Master_Employee_DMO
                                           where (c.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && a.HRME_Id == d.HRME_Id && b.MI_Id == d.MI_Id && a.HRTCR_Id == b.HRTCR_Id)
                                           select new SummaryreportsDTO
                                           {
                                               EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                               HRME_Id = d.HRME_Id,
                                               HRTCR_Id = b.HRTCR_Id,
                                               HRTCR_PrgogramName = b.HRTCR_PrgogramName,
                                               HRTCR_StatusFlg = b.HRTCR_StatusFlg,
                                               HRTCR_StartDate = b.HRTCR_StartDate,
                                               HRTCR_EndDate = b.HRTCR_EndDate,
                                               MI_Name=c.MI_Name,
                                           }).Distinct().ToArray();
                //}
                //else if (data.HRTCR_Id > 0)
                //{
                //    data.certificatedetails = (from a in _context.HR_Training_Create_Participants_DMO_con
                //                           from b in _context.HR_Training_Create_DMO_con
                //                           from c in _context.Institution
                //                               from d in _context.HR_Master_Employee_DMO
                //                           where (c.MI_Id==data.MI_Id &&  a.HRTCR_Id == data.HRTCR_Id && a.HRME_Id == d.HRME_Id && b.MI_Id == d.MI_Id && a.HRTCR_Id == b.HRTCR_Id)
                //                           select new SummaryreportsDTO
                //                           {
                //                               EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                //                               HRME_Id = d.HRME_Id,
                //                               HRTCR_Id = b.HRTCR_Id,
                //                               HRTCR_PrgogramName = b.HRTCR_PrgogramName,
                //                               HRTCR_StatusFlg = b.HRTCR_StatusFlg,
                //                               HRTCR_StartDate = b.HRTCR_StartDate,
                //                               HRTCR_EndDate = b.HRTCR_EndDate
                //                           }).Distinct().ToArray();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }






    }
}
