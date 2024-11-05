using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class PFTransactionIMPL : Interfaces.PFTransactionInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public PFTransactionIMPL(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public PFReportsDTO getBasicData(PFReportsDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public PFReportsDTO GetAllDropdownAndDatatableDetails(PFReportsDTO dto)
        {
            try
            {

                //employee  
                //  dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true ).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                dto.employeedropdown = (from b in _HRMSContext.MasterEmployee
                                    where (b.MI_Id == dto.MI_Id  && b.HRME_ActiveFlag == true)
                                    select new PFReportsDTO
                                    {
                                        HRME_Id = b.HRME_Id,
                                        HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim()

                                    }).ToArray();
                //leave year
                dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.OrderBy(t => t.IMFY_OrderBy).ToArray();

               // dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "VPF_TransactionGrid";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)                    {                        Value = dto.UserId                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        dto.getpfgriddata = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }




                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "PF_TransactionGrid";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)                    {                        Value = dto.UserId                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        dto.pfreport = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }




                }
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_PFVPF_Interest_Grid";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)                    {                        Value = dto.MI_Id                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        dto.get_store = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }




                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public PFReportsDTO SavePFData(PFReportsDTO dto)
        {
            try
            {
                if (dto.PFVPFflag== "VPF")
                {
                    var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_VPFTransactionSave @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", dto.IMFY_Id, dto.IVRM_Month_Id, dto.MI_Id, dto.HRME_Id, dto.UserId, dto.TransAmount, dto.HeadType, dto.DepositWithdrow, dto.Remark,dto.HRME_PFDate);
                    if (contactExistsP > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "notAdded";
                    }
                }
                else if (dto.PFVPFflag == "PF")
                {
                    var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_PFTransactionSave @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10", dto.IMFY_Id, dto.IVRM_Month_Id, dto.MI_Id, dto.HRME_Id, dto.UserId, dto.TransAmount,dto.Schoolamount, dto.HeadType, dto.DepositWithdrow, dto.Remark, dto.HRME_PFDate);
                    if (contactExistsP > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "notAdded";
                    }
                }

                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }


        public PFReportsDTO getReport(PFReportsDTO dto)        {            try            {                List<long> HRMEID = new List<long>();                string HRME_Id = "0";                if (dto.employee != null && dto.employee.Length > 0)                {                    foreach (var d in dto.employee)                    {                        HRME_Id = HRME_Id + ',' + d.HRME_Id;                    }                }                List<PFReportsDTO> Tasklist = new List<PFReportsDTO>();                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {
                    //cmd.CommandText = "PFYearlyReport";
                    cmd.CommandText = dto.Procedure;                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.CommandTimeout = 0;                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar) { Value = dto.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value = HRME_Id });                    cmd.Parameters.Add(new SqlParameter("@IMFY_ID", SqlDbType.VarChar) { Value = dto.IMFY_Id });                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = dto.Flag });                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(                                        dataReader.GetName(iFiled1),                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow1);                                if (dto.Flag != "PFGrandTotalReport" && dto.Flag != "VPFGrandTotalReport")                                {                                    Tasklist.Add(new PFReportsDTO                                    {                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),                                    });                                }

                            }                        }                        dto.EmployeePFreportDetails = retObject.ToArray();                    }                    catch (Exception ee)                    {                        Console.WriteLine(ee.Message);                    }                }                if (Tasklist != null && Tasklist.Count > 0)                {                    foreach (var d in Tasklist)                    {                        HRMEID.Add(d.HRME_Id);                    }                    dto.employeeDetails = (from a in _HRMSContext.MasterEmployee                                           from b in _HRMSContext.Institution                                           where (a.MI_Id == b.MI_Id && HRMEID.Contains(a.HRME_Id))                                           select new PFReportsDTO                                           {                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)                                         + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),                                               MI_Name = b.MI_Name,                                               HRME_PFAccNo = a.HRME_PFAccNo,                                               HRME_EmployeeCode = a.HRME_EmployeeCode,                                               HRME_Id = a.HRME_Id                                           }).Distinct().ToArray();                }            }            catch (Exception ex)            {                Console.WriteLine(ex);            }            return dto;        }

        public PFReportsDTO getEmployeedetailsBySelectionStJames(PFReportsDTO dto)
        {
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            Institution InstitutionDetails = new Institution();
            List<HR_Employee_SalaryDTO> employeeSalaryDetailsDTO = new List<HR_Employee_SalaryDTO>();
            List<PFReportsDTO> alldata = new List<PFReportsDTO>();

            try
            {

                List<HR_Employee_Salary> employeeSalaryDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Month.Equals(dto.HRES_Month) && t.HRES_Year.Equals(dto.HRES_Year) && t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                foreach (var empSalary in employeeSalaryDetails)
                {
                    PFReportsDTO ss = new PFReportsDTO();

                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empSalary.HRME_Id && t.HRME_PFAccNo != null).ToList();
                    if (employeeDetails.Count > 0)
                    {

                        // var agefac = employeeDetails.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year);
                        var agefactor = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).Select(t => Convert.ToInt32(t.HRC_RetirementYrs));

                        string departmentname = _HRMSContext.HR_Master_Department.Where(t => t.HRMD_Id == employeeDetails[0].HRMD_Id).Select(t => t.HRMD_DepartmentName).FirstOrDefault();

                        dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();
                        dto.HRC_RetirementYrs = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).Select(t => t.HRC_RetirementYrs).FirstOrDefault();

                        //   int hhhh = _HRMSContext.MasterEmployee.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year && t.);

                        var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                          from mas in _HRMSContext.HR_Master_EarningsDeductions
                                          where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == empSalary.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id)
                                          select new PFReportsDTO
                                          {
                                              HRESD_Amount = emp.HREED_Amount
                                          }).ToList();

                        dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));

                        var emptotDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                         from mas in _HRMSContext.HR_Master_EarningsDeductions
                                         where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == empSalary.HRME_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.MI_Id == dto.MI_Id)
                                         select new PFReportsDTO
                                         {
                                             HRESD_Amount = emp.HREED_Amount
                                         }).ToList();

                        dto.emptotdedSal = Convert.ToDecimal(emptotDed.Sum(t => t.HRESD_Amount));

                        var birthdays = (from emp in _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empSalary.HRME_Id && t.MI_Id == dto.MI_Id)
                                         select new PFReportsDTO
                                         {
                                             H_DOB = DateTime.Now.Year - emp.HRME_DOB.Value.Date.Year
                                         }).ToList();

                        dto.abc = Convert.ToInt32(birthdays.Sum(t => t.H_DOB));
                        ss.HRME_Age = DateTime.Now.Year - employeeDetails.FirstOrDefault().HRME_DOB.Value.Date.Year;
                        //if (dto.abc <= 58)
                        if (dto.abc <= dto.HRC_RetirementYrs)
                        {
                            var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                               from HRES in _HRMSContext.HR_Employee_Salary
                                               from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                               where (HRESD.HRES_Id == HRES.HRES_Id && HRESD.HRMED_Id == HRMED.HRMED_Id && HRESD.HRES_Id == empSalary.HRES_Id) //checking condition
                                               select new PFReportsDTO
                                               {
                                                   HRESD_Id = HRESD.HRESD_Id,
                                                   HRES_Id = HRES.HRES_Id,
                                                   HRMED_Id = HRESD.HRMED_Id,
                                                   HRMED_Name = HRMED.HRMED_Name,
                                                   HRESD_Amount = HRESD.HRESD_Amount
                                               }).ToList();

                            if (currentdata.Count() > 0)
                            {
                                decimal? BasicPayHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("Basic Pay")).Sum(t => t.HRESD_Amount);
                                decimal? DAHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("DA")).Sum(t => t.HRESD_Amount);
                                decimal? OTHERS_Amount = currentdata.Where(t => t.HRMED_Name.Equals("PERSONAL PAY")).Sum(t => t.HRESD_Amount);
                                decimal? CLPAY = currentdata.Where(t => t.HRMED_Name.Equals("CL AMT")).Sum(t => t.HRESD_Amount);
                                dto.empGrossSal = BasicPayHRESD_Amount + DAHRESD_Amount + OTHERS_Amount + CLPAY;
                                long netsalary = Convert.ToInt64(dto.empGrossSal);
                                long PFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("P F")).Sum(t => t.HRESD_Amount));
                                long VPFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("V PF")).Sum(t => t.HRESD_Amount));

                                ss.basicamount = Convert.ToInt64(BasicPayHRESD_Amount);
                                ss.DAamount = Convert.ToInt64(DAHRESD_Amount);
                                ss.Othersamount = Convert.ToInt64(OTHERS_Amount + CLPAY);
                                ss.VPFAmount = VPFAmount;

                                if (netsalary > 15000) { ss.PFAmount = 15000; }
                                else { ss.PFAmount = netsalary; }

                                if (PFAmount >= 0)
                                {
                                    long AmountofWages = Convert.ToInt64(BasicPayHRESD_Amount + DAHRESD_Amount);
                                    ss.HRME_PFuAN = employeeDetails.FirstOrDefault().HRME_UINumber;
                                    ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                    ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                    ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                    ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                    ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;
                                    ss.departmentname = departmentname;
                                    ss.netsalary = dto.empGrossSal;
                                    ss.emptotdedSal = dto.emptotdedSal;
                                    ss.AmountofWages = AmountofWages;
                                    ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                    ss.HRME_DOJ = employeeDetails.FirstOrDefault().HRME_DOJ;
                                    ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                    if (ss.AmountofWages <= 15000)
                                    {
                                        var adg = 0.01;
                                        ss.HRES_Ac5 = ss.AmountofWages * Convert.ToDecimal(adg);
                                    }


                                    else if (ss.AmountofWages > 15000)
                                    {
                                        //var adg = 0.1;
                                        ss.HRES_Ac5 = 150;
                                    }
                                    ss.STJOwnPF = PFAmount;
                                    ss.HRES_EPF = (empSalary.HRES_EPF);
                                    ss.HRES_FPF = (empSalary.HRES_FPF);
                                    ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                                    ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                                    ss.FatherHusbandName = dto.abc.ToString();
                                    // ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                                    alldata.Add(ss);


                                }
                                else { }
                            }
                            else
                            {
                                ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                ss.HRME_DOJ = employeeDetails.FirstOrDefault().HRME_DOJ;
                                ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;
                                ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                ss.netsalary = dto.empGrossSal;
                                ss.AmountofWages = 0;
                                ss.PFAmount = 0;
                                ss.HRES_EPF = (empSalary.HRES_EPF);
                                ss.HRES_FPF = (empSalary.HRES_FPF);
                                ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                                ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                                ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                                ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                alldata.Add(ss);

                            }
                        }
                        else
                        {
                            var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                               from HRES in _HRMSContext.HR_Employee_Salary
                                               from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                               where (HRESD.HRES_Id == HRES.HRES_Id &&
                                               HRESD.HRMED_Id == HRMED.HRMED_Id
                                             && HRESD.HRES_Id == empSalary.HRES_Id) //checking condition
                                               select new PFReportsDTO
                                               {
                                                   HRESD_Id = HRESD.HRESD_Id,
                                                   HRES_Id = HRES.HRES_Id,
                                                   HRMED_Id = HRESD.HRMED_Id,
                                                   HRMED_Name = HRMED.HRMED_Name,
                                                   HRESD_Amount = HRESD.HRESD_Amount,
                                               }).ToList();
                            if (currentdata.Count() > 0)
                            {
                                decimal? BasicPayHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("Basic Pay")).Sum(t => t.HRESD_Amount);
                                decimal? DAHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("DA")).Sum(t => t.HRESD_Amount);
                                decimal? OTHERS_Amount = currentdata.Where(t => t.HRMED_Name.Equals("PERSONAL PAY")).Sum(t => t.HRESD_Amount);
                                decimal? CLPAY = currentdata.Where(t => t.HRMED_Name.Equals("CL AMT")).Sum(t => t.HRESD_Amount);
                                dto.empGrossSal = BasicPayHRESD_Amount + DAHRESD_Amount + OTHERS_Amount + CLPAY;
                                long netsalary = Convert.ToInt64(dto.empGrossSal);
                                long PFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("P F")).Sum(t => t.HRESD_Amount));
                                long VPFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("V PF")).Sum(t => t.HRESD_Amount));

                                ss.basicamount = Convert.ToInt64(BasicPayHRESD_Amount);
                                ss.DAamount = Convert.ToInt64(DAHRESD_Amount);
                                ss.Othersamount = Convert.ToInt64(OTHERS_Amount + CLPAY);
                                ss.VPFAmount = VPFAmount;
                                ss.STJOwnPF = PFAmount;
                                ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;

                                if (netsalary > 15000) { ss.PFAmount = 15000; }
                                else { ss.PFAmount = netsalary; }

                                if (PFAmount > 0)
                                {

                                    long AmountofWages = Convert.ToInt64(BasicPayHRESD_Amount + DAHRESD_Amount);

                                    ss.HRME_PFuAN = employeeDetails.FirstOrDefault().HRME_UINumber;
                                    ss.HRME_DOJ = employeeDetails.FirstOrDefault().HRME_DOJ;
                                    ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                    ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                    ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                    ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                    ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;
                                    ss.netsalary = dto.empGrossSal;
                                    ss.AmountofWages = AmountofWages;
                                    ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                    ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                    ss.VPFAmount = VPFAmount;

                                    if (ss.AmountofWages <= 15000)
                                    {
                                        var adg = 0.01;
                                        ss.HRES_Ac5 = ss.AmountofWages * Convert.ToDecimal(adg);
                                    }

                                    else if (ss.AmountofWages > 15000)
                                    {
                                        //var adg = 0.1;
                                        ss.HRES_Ac5 = 150;
                                    }
                                    //ss.PFAmount = PFAmount;
                                    ss.HRES_EPF = (empSalary.HRES_EPF);
                                    ss.HRES_FPF = (empSalary.HRES_FPF);
                                    //ss.HRES_EPF = PFAmount;
                                    //ss.HRES_FPF = PFAmount;
                                    ss.HRES_Ac21 = 0;
                                    ss.HRES_Ac22 = 0;
                                    //ss.HRES_Ac5 = 0;
                                    ss.FatherHusbandName = dto.abc.ToString();
                                    alldata.Add(ss);

                                }
                                else { }
                            }
                            else
                            {
                                ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;
                                ss.HRME_DOJ = employeeDetails.FirstOrDefault().HRME_DOJ;
                                ss.netsalary = dto.empGrossSal;
                                ss.AmountofWages = 0;
                                ss.PFAmount = 0;
                                ss.HRES_EPF = (empSalary.HRES_EPF);
                                ss.HRES_FPF = (empSalary.HRES_EPF);
                                ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                                ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                                ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                                ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                alldata.Add(ss);
                            }
                        }
                    }
                }

                dto.pfreport = alldata.OrderBy(t => t.departmentname).ToArray();

                // dto.FatherHusbandName = 
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public PFReportsDTO DeleteRecord(PFReportsDTO dto)
        {
            

            try
            {
                
                var contactExistsP = _Context.Database.ExecuteSqlCommand("HR_PFVPFTransaction_Delete @p0,@p1", dto.TransactionID,dto.PFVPFflag);
                if (contactExistsP > 0)
                {
                    dto.retrunMsg = "updated";
                }
                else
                {
                    dto.retrunMsg = "notUpdated";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }
        public PFReportsDTO editdata(PFReportsDTO dto)
        {
            
            try
            {
                if (dto.TransactionID > 0)
                {
                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PFVPF_EditDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TransactionID", SqlDbType.BigInt)
                        {
                            Value = dto.TransactionID
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = dto.Flag
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
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                                                   );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            dto.PayrollStandard = retObject.ToArray();
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
                Console.WriteLine(ex);
            }
            return dto;
        }

        public PFReportsDTO getloaddata(PFReportsDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public PFReportsDTO savedetails(PFReportsDTO data)
        {
            try
            {
                if (data.HRMPFVPFINT_Id != 0)
                {
                    var res = _HRMSContext.HR_Master_PFVPF_InterestDMO.Where(t => t.HRMPFVPFINT_PFInterestRate == data.HRMPFVPFINT_PFInterestRate && t.HRMPFVPFINT_VPFInterestRate == data.HRMPFVPFINT_VPFInterestRate && t.MI_Id == data.MI_Id && t.HRMPFVPFINT_Id != data.HRMPFVPFINT_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _HRMSContext.HR_Master_PFVPF_InterestDMO.Single(t => t.HRMPFVPFINT_Id == data.HRMPFVPFINT_Id);
                        result.MI_Id = data.MI_Id;
                        result.IMFY_Id = data.IMFY_Id;
                        result.HRMPFVPFINT_PFInterestRate = data.HRMPFVPFINT_PFInterestRate;
                        result.HRMPFVPFINT_VPFInterestRate = data.HRMPFVPFINT_VPFInterestRate;

                        result.HRMPFVPFINT_ActiveFlg = true;
                        result.HRMPFVPFINT_UpdatedDate = DateTime.Now;
                        result.HRMPFVPFINT_UpdatedBy = data.UserId;
                        _HRMSContext.Update(result);

                        var contactExists = _HRMSContext.SaveChanges();
                        if (contactExists > 0)
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
                    var res = _HRMSContext.HR_Master_PFVPF_InterestDMO.Where(t => t.HRMPFVPFINT_PFInterestRate == data.HRMPFVPFINT_PFInterestRate && t.HRMPFVPFINT_VPFInterestRate == data.HRMPFVPFINT_VPFInterestRate && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        HR_Master_PFVPF_InterestDMO store = new HR_Master_PFVPF_InterestDMO();
                        store.MI_Id = data.MI_Id;
                        store.HRMPFVPFINT_PFInterestRate = data.HRMPFVPFINT_PFInterestRate;
                        store.HRMPFVPFINT_VPFInterestRate = data.HRMPFVPFINT_VPFInterestRate;
                        store.IMFY_Id = data.IMFY_Id;
                        store.HRMPFVPFINT_ActiveFlg = true;
                        store.HRMPFVPFINT_CreatedDate = DateTime.Now;
                        store.HRMPFVPFINT_UpdatedDate = DateTime.Now;
                        store.HRMPFVPFINT_CreatedBy = data.UserId;

                        _HRMSContext.Add(store);

                        var contactExists = _HRMSContext.SaveChanges();
                        if (contactExists > 0)
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

        public PFReportsDTO deactive(PFReportsDTO data)
        {
            try
            {

                var result = _HRMSContext.HR_Master_PFVPF_InterestDMO.Single(t => t.HRMPFVPFINT_Id == data.HRMPFVPFINT_Id);
                if (result.HRMPFVPFINT_ActiveFlg == true)
                {
                    result.HRMPFVPFINT_ActiveFlg = false;
                }
                else if (result.HRMPFVPFINT_ActiveFlg == false)
                {
                    result.HRMPFVPFINT_ActiveFlg = true;
                }
                result.HRMPFVPFINT_UpdatedDate = DateTime.Now;
                _HRMSContext.Update(result);
                int returnval = _HRMSContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public PFReportsDTO PFBlurcalculation(PFReportsDTO data)
        {
            try
            {

                if (data.PFVPFflag=="VPF")
                {

                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_VPF_BlurCalculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HREVPFST_Id", SqlDbType.BigInt) { Value = data.vpfTranaction[0].HREVPFST_Id });
                        cmd.Parameters.Add(new SqlParameter("@HREVPFST_VOBAmount", SqlDbType.Decimal) { Value = data.vpfTranaction[0].HREVPFST_VOBAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREVPFST_TransferAmount", SqlDbType.Decimal) { Value = data.vpfTranaction[0].HREVPFST_TransferAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREVPFST_WithdrawnAmount", SqlDbType.Decimal) { Value = data.vpfTranaction[0].HREVPFST_WithdrawnAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREVPFST_SettledAmount", SqlDbType.Decimal) { Value = data.vpfTranaction[0].HREVPFST_SettledAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREVPFST_DepositAdjustmentAmount", SqlDbType.Decimal) { Value = data.vpfTranaction[0].HREVPFST_DepositAdjustmentAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREVPFST_WithsrawAdjustmentAmount", SqlDbType.Decimal) { Value = data.vpfTranaction[0].HREVPFST_WithsrawAdjustmentAmount });
                        cmd.Parameters.Add(new SqlParameter("@Headdate", SqlDbType.Date) { Value = data.HRME_PFDate });

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
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                                                   );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.PayrollStandard = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                else
                {

                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_PF_BlurCalculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_Id", SqlDbType.BigInt) { Value = data.pfTranaction[0].HREPFST_Id });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_OBOwnAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_OBOwnAmount });
                        cmd.Parameters.Add(new SqlParameter("@HHREPFST_OBInstituteAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_OBInstituteAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_OwnTransferAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_OwnTransferAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_InstituteTransferAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_InstituteTransferAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_OwnWithdrwanAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_OwnWithdrwanAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_InstituteWithdrawnAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_InstituteWithdrawnAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_OwnSettlementAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_OwnSettlementAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_InstituteLSettlementAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_InstituteLSettlementAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_OwnDepositAdjustmentAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_OwnDepositAdjustmentAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_InstituteDepositAdjustmentAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_InstituteDepositAdjustmentAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_OwnWithdrawAdjustmentAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_OwnWithdrawAdjustmentAmount });
                        cmd.Parameters.Add(new SqlParameter("@HREPFST_InstituteWithdrawAdjustmentAmount", SqlDbType.Decimal) { Value = data.pfTranaction[0].HREPFST_InstituteWithdrawAdjustmentAmount });
                        cmd.Parameters.Add(new SqlParameter("@Headdate", SqlDbType.Date) { Value = data.HRME_PFDate });

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
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                                                   );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.PayrollStandard = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }

                


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public PFReportsDTO EditSave(PFReportsDTO data)
        {
            try
            {

                if (data.PFVPFflag=="VPF")
                {

                    for (int i = 0; i < data.vpfTranaction.Length; i++)
                    {
                        var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_VPF_BlurCalculation_Save @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.vpfTranaction[i].HREVPFST_Id, data.vpfTranaction[i].HREVPFST_VOBAmount, data.vpfTranaction[i].HREVPFST_Contribution, data.vpfTranaction[i].HREVPFST_Intersest,data.vpfTranaction[i].HREVPFST_TransferAmount, data.vpfTranaction[i].HREVPFST_WithdrawnAmount, data.vpfTranaction[i].HREVPFST_SettledAmount, data.vpfTranaction[i].HREVPFST_DepositAdjustmentAmount, data.vpfTranaction[i].HREVPFST_WithsrawAdjustmentAmount, data.vpfTranaction[i].HREVPFST_ClosingBalance);
                        if (contactExistsP > 0)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "notAdded";
                        }
                    }


                }
                else
                {
                    for (int i = 0; i < data.pfTranaction.Length; i++)
                    {
                        var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_PF_BlurCalculation_save @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18", data.pfTranaction[i].HREPFST_Id, data.pfTranaction[i].HREPFST_OBOwnAmount, data.pfTranaction[i].HREPFST_OBInstituteAmount, data.pfTranaction[i].HREPFST_OwnContribution, data.pfTranaction[i].HREPFST_IntstituteContribution, data.pfTranaction[i].HREPFST_OwnInterest, data.pfTranaction[i].HREPFST_InstituteInterest, data.pfTranaction[i].HREPFST_OwnTransferAmount, data.pfTranaction[i].HREPFST_InstituteTransferAmount, data.pfTranaction[i].HREPFST_OwnWithdrwanAmount, data.pfTranaction[i].HREPFST_InstituteWithdrawnAmount, data.pfTranaction[i].HREPFST_OwnSettlementAmount, data.pfTranaction[i].HREPFST_InstituteLSettlementAmount, data.pfTranaction[i].HREPFST_OwnDepositAdjustmentAmount, data.pfTranaction[i].HREPFST_InstituteDepositAdjustmentAmount, data.pfTranaction[i].HREPFST_OwnWithdrawAdjustmentAmount, data.pfTranaction[i].HREPFST_InstituteWithdrawAdjustmentAmount, data.pfTranaction[i].HREPFST_OwnClosingBalance, data.pfTranaction[i].HREPFST_InstituteClosingBalance);
                        if (contactExistsP > 0)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "notAdded";
                        }
                    }



                }

                


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public PFReportsDTO finalverify(PFReportsDTO data)
        {
            try
            {

                if (data.PFVPFflag=="VPF")
                {

                    for (int i = 0; i < data.employee.Length; i++)
                    {
                        var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_VPF_finalverify @p0,@p1", data.employee[i].HRME_Id, data.IMFY_Id);
                        if (contactExistsP > 0)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "notAdded";
                        }
                    }


                }
                else
                {
                    for (int i = 0; i < data.employee.Length; i++)
                    {
                        var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_PF_finalverify @p0,@p1", data.employee[i].HRME_Id, data.IMFY_Id);
                        if (contactExistsP > 0)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "notAdded";
                        }
                    }



                }

                


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


    }
}
