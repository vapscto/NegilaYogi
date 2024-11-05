using AutoMapper;
using DataAccessMsSqlServerProvider;
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
    public class EmployeeSalaryDetailsService : Interfaces.EmployeeSalaryDetailsInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeSalaryDetailsService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public HR_Employee_EarningsDeductionsDTO getBasicData(HR_Employee_EarningsDeductionsDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public HR_Employee_EarningsDeductionsDTO GetEmployeeDetailsBySelected(HR_Employee_EarningsDeductionsDTO dto)
        {

            List<MasterEmployee> employe = new List<MasterEmployee>();

            try
            {

                employe = (from a in _HRMSContext.MasterEmployee
                           from b in _HRMSContext.HR_Employee_EarningsDeductions
                           where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id)
                           && a.HRME_ActiveFlag == true)
                           select a).Distinct().ToList();
                if (employe.Count > 0)
                {

                    if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }

                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }

                }

                dto.employeedetailList = employe.ToArray();

              //  getEmployeeSalaryDetails(dto);
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HRMS_Employee_Increment_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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

                            dto.employeedropdowndetails = retObject.ToArray();
                        }
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


        public HR_Employee_EarningsDeductionsDTO SaveUpdate(HR_Employee_EarningsDeductionsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {

                if (dto.TabName.Equals("SalaryTab"))
                {
                    AddUpdateEmployeeEarningDetails(dto);
                    AddUpdateEmployeeDeductionDetails(dto);
                    AddUpdateEmployeeArrearDetails(dto);

                    AddIncrementdetails(dto);
                }

            }

            catch (Exception ee)
            {  
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }

            return dto;
        }

        public HR_Employee_EarningsDeductionsDTO AddIncrementdetails(HR_Employee_EarningsDeductionsDTO dto)
        {
            //add/update Documents details



            try
            {
                HR_Employee_Increment dmoObj = Mapper.Map<HR_Employee_Increment>(dto);

                var alldata = _HRMSContext.HR_Employee_IncrementDMO.Where(t => t.MI_Id == dto.MI_Id && t.HREIC_IncrementDate != dto.HREIC_IncrementDate && t.HREIC_IncrementDueDate != dto.HREIC_IncrementDueDate && t.HRME_Id == dto.HRME_Id && t.HREIC_LastIncrementDate != dto.HREIC_LastIncrementDate).Count();
                


                if (alldata == 0)
                {
                    if (dmoObj.HREIC_Id > 0)
                    {
                        var duplicateHRMBD_BankName = _HRMSContext.HR_Employee_IncrementDMO.Where(t => t.MI_Id == dto.MI_Id && t.HREIC_Id != dmoObj.HREIC_Id).Count();

                        if (duplicateHRMBD_BankName == 0)
                        {
                            var result = _HRMSContext.HR_Employee_IncrementDMO.Single(t => t.HREIC_Id == dmoObj.HREIC_Id);

                            dto.HREIC_ActiveFlag = true;
                            Mapper.Map(dto, result);
                            _HRMSContext.Update(result);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "AllDuplicate";
                            }
                        }
                    }



                    else
                    {
                        DateTime newDate = new DateTime();
                        var checkconfig = _HRMSContext.HR_Configuration.Where(a => a.MI_Id == dto.MI_Id).ToList();

                        if (checkconfig.Count() > 0)
                        {
                            if (checkconfig.FirstOrDefault().HRC_IncrementOnceInMonths > 0)
                            {
                                newDate = DateTime.Now.AddMonths(Convert.ToInt32(checkconfig.FirstOrDefault().HRC_IncrementOnceInMonths));
                            }
                            else
                            {
                                newDate = DateTime.Now.AddMonths(0);
                            }
                        }
                        else
                        {
                            newDate = DateTime.Now.AddMonths(0);
                        }



                        var duplicateHRMBD_BankName = _HRMSContext.HR_Employee_IncrementDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == dto.HRME_Id).Count();
                        if (duplicateHRMBD_BankName == 0)
                        {


                            dmoObj.HREIC_ActiveFlag = true;
                            dmoObj.HREIC_ArrearGivenFlg = dto.HREIC_ArrearGivenFlg;
                            dmoObj.HREIC_ArrearApplicableFlg = dto.HREIC_ArrearApplicableFlg;
                            dmoObj.HREIC_IncrementDate = DateTime.Now;
                            dmoObj.HREIC_IncrementDueDate = newDate;
                            dmoObj.HREIC_LastIncrementDate = DateTime.Now;
                            dmoObj.HREIC_UpdatedDate = DateTime.Now;
                            dmoObj.HREIC_CreatedDate = DateTime.Now;
                            dmoObj.HRME_Id = dto.HRME_Id;
                            // dmoObj.HRMOI_MaxLimitAplFlg = dto.HRMOI_MaxLimitAplFlg;
                            // dmoObj.HRMOI_MaxLimit = dto.HRMOI_MaxLimit;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRME_Id = dto.HRME_Id;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }



                            try
                            {
                                if (dto.EarningDTO.Count() > 0)
                                {
                                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.EarningDTO)
                                    {
                                        DocumentsDTO.MI_Id = dto.MI_Id;
                                        // DocumentsDTO.HRME_Id = dto.HRME_Id;
                                        HR_Employee_Increment_EDHeads Documents = Mapper.Map<HR_Employee_Increment_EDHeads>(DocumentsDTO);

                                        if (DocumentsDTO.HREICED_Id > 0)
                                        {
                                            var duplicateHRMBD_BankNames = _HRMSContext.HR_Employee_Increment_EDHeadsDMO.Where(t => t.MI_Id == dto.MI_Id && t.HREIC_Id == dto.HREIC_Id).Count();

                                            //Documents.HREICED_ActiveFlag = true;
                                            //Documents.HREICED_Percentage = dto.HREICED_Percentage;
                                            //Documents.HREICED_Amount = dto.HREICED_Amount;
                                            //Documents.HRMED_Id = dto.HRMED_Id;
                                            //Documents.HREIC_Id = dto.HREIC_Id;
                                            //Documents.MI_Id = dto.MI_Id;


                                            dmoObj.HRME_Id = dto.HRME_Id;
                                            // dmoObj.HRMOI_MaxLimitAplFlg = dto.HRMOI_MaxLimitAplFlg;
                                            // dmoObj.HRMOI_MaxLimit = dto.HRMOI_MaxLimit;
                                            dmoObj.MI_Id = dto.MI_Id;
                                            dmoObj.HRME_Id = dto.HRME_Id;
                                            _HRMSContext.Add(dmoObj);
                                            var flags = _HRMSContext.SaveChanges();
                                            if (flags == 1)
                                            {
                                                dto.retrunMsg = "Add";
                                            }
                                            else
                                            {
                                                dto.retrunMsg = "false";
                                            }

                                        }
                                        else
                                        {

                                            Documents.HREICED_ActiveFlag = true;
                                            Documents.HREICED_Percentage = DocumentsDTO.HREED_Percentage;
                                            // Documents.HREICED_Amount = DocumentsDTO.HREICED_Amount;
                                            Documents.HREICED_Amount = DocumentsDTO.HREED_Amount;
                                            Documents.HRMED_Id = DocumentsDTO.HRMED_Id;
                                            Documents.HREIC_Id = dmoObj.HREIC_Id;
                                            Documents.MI_Id = DocumentsDTO.MI_Id;

                                            // Documents.HREED_ActiveFlag = true;
                                            _HRMSContext.Add(Documents);
                                            var flags = _HRMSContext.SaveChanges();
                                            if (flags > 0)
                                            {
                                                dto.retrunMsg = "Add";
                                            }
                                            else
                                            {
                                                dto.retrunMsg = "false";
                                            }

                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.InnerException);
                                dto.retrunMsg = "Error occured";
                            }



                            //deduction part...
                            try
                            {
                                if (dto.EarningDTO.Count() > 0)
                                {
                                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.DeductionDTO)
                                    {
                                        DocumentsDTO.MI_Id = dto.MI_Id;
                                        // DocumentsDTO.HRME_Id = dto.HRME_Id;
                                        HR_Employee_Increment_EDHeads Documents = Mapper.Map<HR_Employee_Increment_EDHeads>(DocumentsDTO);

                                        if (DocumentsDTO.HREICED_Id > 0)
                                        {
                                            var duplicateHRMBD_BankNames = _HRMSContext.HR_Employee_Increment_EDHeadsDMO.Where(t => t.MI_Id == dto.MI_Id && t.HREIC_Id == dto.HREIC_Id).Count();



                                            dmoObj.HRME_Id = dto.HRME_Id;
                                            // dmoObj.HRMOI_MaxLimitAplFlg = dto.HRMOI_MaxLimitAplFlg;
                                            // dmoObj.HRMOI_MaxLimit = dto.HRMOI_MaxLimit;
                                            dmoObj.MI_Id = dto.MI_Id;
                                            dmoObj.HRME_Id = dto.HRME_Id;
                                            _HRMSContext.Add(dmoObj);
                                            var flags = _HRMSContext.SaveChanges();
                                            if (flags == 1)
                                            {
                                                dto.retrunMsg = "Add";
                                            }
                                            else
                                            {
                                                dto.retrunMsg = "false";
                                            }

                                        }
                                        else
                                        {

                                            Documents.HREICED_ActiveFlag = true;
                                            Documents.HREICED_Percentage = DocumentsDTO.HREED_Percentage;
                                            // Documents.HREICED_Amount = DocumentsDTO.HREICED_Amount;
                                            Documents.HREICED_Amount = DocumentsDTO.HREED_Amount;
                                            Documents.HRMED_Id = DocumentsDTO.HRMED_Id;
                                            Documents.HREIC_Id = dmoObj.HREIC_Id;
                                            Documents.MI_Id = DocumentsDTO.MI_Id;

                                            // Documents.HREED_ActiveFlag = true;
                                            _HRMSContext.Add(Documents);
                                            var flags = _HRMSContext.SaveChanges();
                                            if (flags > 0)
                                            {
                                                dto.retrunMsg = "Add";
                                            }
                                            else
                                            {
                                                dto.retrunMsg = "false";
                                            }

                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.InnerException);
                                dto.retrunMsg = "Error occured";
                            }










                        }


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





        //Employee Earning details
        public HR_Employee_EarningsDeductionsDTO AddUpdateEmployeeEarningDetails(HR_Employee_EarningsDeductionsDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EarningDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.EarningDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        // DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);

                        if (Documents.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);
                            //added by 20/05/2017  
                            // DocumentsDTO.HREED_ActiveFlag = true;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            // Documents.HREED_ActiveFlag = true;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
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

        //Employee Arrear details
        public HR_Employee_EarningsDeductionsDTO AddUpdateEmployeeArrearDetails(HR_Employee_EarningsDeductionsDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EarningDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.ArrearDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        // DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);

                        if (Documents.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);
                            //added by 20/05/2017  
                            //  DocumentsDTO.HREED_ActiveFlag = true;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            // Documents.HREED_ActiveFlag = true;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
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
        //Employee Deduction details
        public HR_Employee_EarningsDeductionsDTO AddUpdateEmployeeDeductionDetails(HR_Employee_EarningsDeductionsDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.DeductionDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.DeductionDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        // DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);

                        if (Documents.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);
                            //added by 20/05/2017   
                            // DocumentsDTO.HREED_ActiveFlag = true;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            //Documents.HREED_ActiveFlag = true;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
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

        public HR_Employee_EarningsDeductionsDTO GetAllDropdownAndDatatableDetails(HR_Employee_EarningsDeductionsDTO dto)
        {
            List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();

            List<HR_Master_EarningsDeductions> earningdeductiondatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistArrear = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductions> arreardatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductions> grossdatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistGross = new List<HR_Master_EarningsDeductionsDTO>();

            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();




            try
            {
                //  dto.LogInUserId = 362;

                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)


                               select pa
                          ).ToList();

                if (PROCESSList.Count() > 0)
                {

                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedetailList = EmployeeList.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();

                }
                else
                {

                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedetailList = EmployeeList.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();


                }

                //Employee List

                //EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                //dto.employeedetailList = EmployeeList.ToArray();


                ////emptype
                //EmployeeTypelist = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToList();
                //dto.employeeTypedropdownlist = EmployeeTypelist.ToArray();

                ////GroupTypelist
                //GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                //dto.groupTypedropdownlist = GroupTypelist.ToArray();

                ////Departmentlist
                //Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                //dto.departmentdropdownlist = Departmentlist.ToArray();

                ////Designationlist
                //Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                //dto.designationdropdownlist = Designationlist.ToArray();

                ////Gradelist
                //Gradelist = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMG_ActiveFlag == true).OrderBy(t => t.HRMG_Order).ToList();
                //dto.gradedropdownlist = Gradelist.ToArray();


                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;


                //Earning list
                earningdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning") && t.HRMED_ActiveFlag == true).ToList();

                if (earningdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistEarning.Add(phdto);

                    }

                }

                dto.earningList = DTOdatalistEarning.ToArray();

                //Deduction List
                deductiondatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Deduction") && t.HRMED_ActiveFlag == true).ToList();


                if (deductiondatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in deductiondatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistDeduction.Add(phdto);

                    }

                }

                dto.detectionList = DTOdatalistDeduction.ToArray();



                //Arrear list
                arreardatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Arrear") && t.HRMED_ActiveFlag == true).ToList();

                if (arreardatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in arreardatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistArrear.Add(phdto);

                    }

                }

                dto.arrearList = DTOdatalistArrear.ToArray();



                //Gross list
                grossdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Gross") && t.HRMED_ActiveFlag == true).ToList();

                if (grossdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in grossdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistGross.Add(phdto);

                    }

                }

                dto.grossList = DTOdatalistGross.ToArray();




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        //OnLoadSaveEmployeeSalaryHeads
        public HR_Employee_EarningsDeductionsDTO OnLoadSaveEmployeeSalaryHeads(HR_Employee_EarningsDeductionsDTO dto)
        {
            //Earning list
            List<HR_Master_EarningsDeductions> masterheadlist = new List<HR_Master_EarningsDeductions>();
            try
            {
                var EmployeeHRMED_Idlist = _HRMSContext.HR_Employee_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(dto.HRME_Id)).Select(t => t.HRMED_Id);
                if (EmployeeHRMED_Idlist.Count() > 0)
                {
                    masterheadlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && !EmployeeHRMED_Idlist.Contains(t.HRMED_Id) && t.HRMED_ActiveFlag == true).ToList();
                }
                else
                {
                    masterheadlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToList();
                }

                if (masterheadlist.Count > 0)
                {

                    foreach (HR_Master_EarningsDeductions ph in masterheadlist)
                    {
                        HR_Employee_EarningsDeductionsDTO EmployeeDetails = new HR_Employee_EarningsDeductionsDTO();
                        EmployeeDetails.HRMED_Id = ph.HRMED_Id;
                        EmployeeDetails.HRME_Id = dto.HRME_Id;
                        EmployeeDetails.MI_Id = dto.MI_Id;
                        if (ph.HRMED_AmountPercentFlag == "Amount")
                        {
                            EmployeeDetails.HREED_Amount = Convert.ToDecimal(ph.HRMED_AmountPercent);
                            EmployeeDetails.HREED_Percentage = "0";
                        }
                        else
                        {
                            EmployeeDetails.HREED_Percentage = ph.HRMED_AmountPercent;
                            EmployeeDetails.HREED_Amount = 0;
                        }
                        EmployeeDetails.HREED_ActiveFlag = true;
                        //EmployeeDetails. = dto.MI_Id;
                        //EmployeeDetails.MI_Id = dto.MI_Id;
                        HR_Employee_EarningsDeductions phdto = Mapper.Map<HR_Employee_EarningsDeductions>(EmployeeDetails);
                        _HRMSContext.Add(phdto);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            // dto.retrunMsg = "Add";
                        }
                        else
                        {
                            // dto.retrunMsg = "false";
                        }

                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return dto;
        }


        public HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetails(HR_Employee_EarningsDeductionsDTO dto)
        {
            List<HR_Master_EarningsDeductions> mstheads = new List<HR_Master_EarningsDeductions>();

            try
            {
                OnLoadSaveEmployeeSalaryHeads(dto);

                HR_Employee_EarningsDeductionsDTO phdto = new HR_Employee_EarningsDeductionsDTO();
                phdto.MI_Id = dto.MI_Id;
                phdto.HRME_Id = dto.HRME_Id;
                //Calculating the standard salarycalculation
                CalculateEmployeeEarnDeductionDetailsByHead(phdto);

                //Employee Earning & deduction details

                List<HR_Employee_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Employee_EarningsDeductionsDTO>();
                //EarningsDeductionsDetails = _HRMSContext.HR_Employee_EarningsDeductions.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();


                EarningsDeductionsDetails = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from med in _HRMSContext.HR_Master_EarningsDeductions
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true
                                             && emp.HRME_Id == dto.HRME_Id
                                             select new HR_Employee_EarningsDeductionsDTO
                                             {
                                                 HREED_Id = emp.HREED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRME_Id = emp.HRME_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HREED_Amount = emp.HREED_Amount,
                                                 HREED_Percentage = emp.HREED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HREED_ActiveFlag = emp.HREED_ActiveFlag,


                                             }
                                ).ToList();


                dto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.ToArray();


                var employees = (from dep in _HRMSContext.HR_Master_Department
                                 from mm in _HRMSContext.MasterEmployee
                                 from des in _HRMSContext.HR_Master_Designation
                                 from grad in _HRMSContext.HR_Master_Grade
                                 from grou in _HRMSContext.HR_Master_GroupType
                                 from emptypr in _HRMSContext.HR_Master_EmployeeType
                                // from ince in _HRMSContext.HR_Employee_IncrementDMO
                                 where mm.MI_Id.Equals(dto.MI_Id)
                                 && mm.HRMDES_Id == des.HRMDES_Id && mm.HRME_Id == dto.HRME_Id
                                 && mm.HRME_ActiveFlag == true
                                 && dep.HRMD_ActiveFlag == true && des.HRMDES_ActiveFlag == true && grad.HRMG_ActiveFlag == true && grou.HRMGT_ActiveFlag == true && emptypr.HRMET_ActiveFlag == true && grad.HRMG_Id == mm.HRMG_Id && grou.HRMGT_Id == mm.HRMGT_Id && dep.HRMD_Id == mm.HRMD_Id && emptypr.HRMET_Id == mm.HRMET_Id /*&& ince.HRME_Id==mm.HRME_Id*/

                                 select new HR_Employee_EarningsDeductionsDTO
                                 {
                                     HRME_Id = mm.HRME_Id,
                                     hrmE_EmployeeFirstName = ((mm.HRME_EmployeeFirstName == null ? " " : mm.HRME_EmployeeFirstName) + (mm.HRME_EmployeeMiddleName == null ? " " : mm.HRME_EmployeeMiddleName) + (mm.HRME_EmployeeLastName == null ? " " : mm.HRME_EmployeeLastName)).Trim(),
                                     HRME_EmployeeCode = mm.HRME_EmployeeCode,
                                     HRMG_GradeName = grad.HRMG_GradeName,
                                     HRMG_Id = grad.HRMG_Id,
                                     HRMDES_DesignationName = des.HRMDES_DesignationName,
                                     HRMDES_Id = des.HRMDES_Id,
                                     HRMD_DepartmentName = dep.HRMD_DepartmentName,
                                     HRMD_Id = dep.HRMD_Id,
                                     HRMET_EmployeeType = emptypr.HRMET_EmployeeType,
                                     HRMET_Id = emptypr.HRMET_Id,
                                     HRMGT_EmployeeGroupType = grou.HRMGT_EmployeeGroupType,
                                     HRMGT_Id = grou.HRMGT_Id,
                                     HRME_PHOTO = mm.HRME_Photo,
                                     HRME_DOB = mm.HRME_DOB,
                                     HRME_DOJ = mm.HRME_DOJ,
                                     HRME_DOC = mm.HRME_DOC,
                                     HRMG_PayScaleRange = grad.HRMG_PayScaleRange,
                                    // HREIC_Id=ince.HREIC_Id,
                                     //HREIC_IncrementDueDate= ince.HREIC_IncrementDueDate
                                 }).ToList();



                dto.dropdownvalus = employees.ToArray();

                dto.HREIC_IncrementDueDate = _HRMSContext.HR_Employee_IncrementDMO.Where(t => t.HRME_Id == dto.HRME_Id && t.MI_Id == dto.MI_Id).Select(t=>t.HREIC_IncrementDueDate).LastOrDefault();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetailsByHead(HR_Employee_EarningsDeductionsDTO dto)
        {
            HR_Employee_EarningsDeductionsDTO empdto = new HR_Employee_EarningsDeductionsDTO();
            try
            {
                if (dto.HREED_Id > 0)
                {
                    var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == dto.HREED_Id);
                    //added by 20/05/2017   
                    // DocumentsDTO.HREED_ActiveFlag = true;
                    Mapper.Map(dto, Documentsresult);
                    _HRMSContext.Update(Documentsresult);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        CalculateEmployeeEarnDeductionDetailsByHead(dto);
                    }
                    else
                    {
                        // dto.retrunMsg = "false";
                    }

                }


                List<HR_Employee_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Employee_EarningsDeductionsDTO>();

                EarningsDeductionsDetails = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from med in _HRMSContext.HR_Master_EarningsDeductions
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true
                                             && emp.HRME_Id == dto.HRME_Id
                                             select new HR_Employee_EarningsDeductionsDTO
                                             {
                                                 HREED_Id = emp.HREED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRME_Id = emp.HRME_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HREED_Amount = emp.HREED_Amount,
                                                 HREED_Percentage = emp.HREED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HREED_ActiveFlag = emp.HREED_ActiveFlag,


                                             }
                                ).ToList();


                empdto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return empdto;
        }

        public HR_Employee_EarningsDeductionsDTO CalculateEmployeeEarnDeductionDetailsByHead(HR_Employee_EarningsDeductionsDTO dto)
        {
            try
            {


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeSalaryDetailDynamicCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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


        public HR_Employee_EarningsDeductionsDTO get_depts(HR_Employee_EarningsDeductionsDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public HR_Employee_EarningsDeductionsDTO get_desig(HR_Employee_EarningsDeductionsDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
