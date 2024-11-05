using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class EmployeeDataImportIMP : Interfaces.EmployeeDataImportInterface
    {
        public HRMSContext _hrmsContext;
        public EmployeeDataImportIMP(HRMSContext context)
        {
            _hrmsContext = context;
        }

        public EmployeeDataImportDTO Savedata(EmployeeDataImportDTO data)
        {
            try
            {



                if (data.empDataimport != null)
                {
                    if (data.empDataimport.Length > 0)
                    {

                        foreach (var item in data.empDataimport)
                        {
                            var empcode = _hrmsContext.MasterEmployee.Where(t => t.HRME_EmployeeCode.Trim() == item.EmployeeCode && t.MI_Id==data.MI_Id).ToList();
                            if (empcode.Count>0)
                            {
                                data.message = "Exist";
                            }
                            else
                            {
                                MasterEmployee obj = new MasterEmployee();

                                obj.HRME_MobileNo = item.MobileNo;
                                obj.HRME_EmailId = item.EmailID;
                                obj.HRME_EmployeeFirstName = item.EmployeeFirstName;
                                obj.HRME_EmployeeMiddleName = item.EmployeeMiddleName;
                                obj.HRME_EmployeeLastName = item.EmployeeLastName;
                                obj.HRME_EmployeeCode = item.EmployeeCode;
                                obj.HRME_DOJ = item.employeeDOJ;                     
                                obj.HRME_DOB = item.employeeDOB;
                                obj.HRME_PerPincode = item.pincode;
                                obj.HRME_LocStreet = item.employeeaddress1;
                                obj.MI_Id = data.MI_Id;
                                
                                obj.HRME_ActiveFlag = true;
                                obj.HRME_LeftFlag = false;
                               






                                var emptype = _hrmsContext.HR_Master_EmployeeType.Where(a => a.HRMET_EmployeeType.Trim() == item.EmployeeType.Trim() && a.MI_Id == data.MI_Id).ToList();
                                if (emptype.Count > 0)
                                {
                                    obj.HRMET_Id = emptype[0].HRMET_Id;
                                }
                                var empgrouptype = _hrmsContext.HR_Master_GroupType.Where(t => t.HRMGT_EmployeeGroupType.Trim() == item.EmployeeGroupType && t.MI_Id == data.MI_Id).ToList();
                                if (empgrouptype.Count > 0)
                                {
                                    obj.HRMGT_Id = empgrouptype[0].HRMGT_Id;
                                }
                                var deptname = _hrmsContext.HR_Master_Department.Where(t => t.HRMD_DepartmentName.Trim() == item.DepartmentName && t.MI_Id == data.MI_Id).ToList();
                                if (deptname.Count > 0)
                                {
                                    obj.HRMD_Id = deptname[0].HRMD_Id;
                                }
                                var desname = _hrmsContext.HR_Master_Designation.Where(t => t.HRMDES_DesignationName.Trim() == item.DesignationName && t.MI_Id == data.MI_Id).ToList();
                                if (desname.Count > 0)
                                {
                                    obj.HRMDES_Id = desname[0].HRMDES_Id;
                                }
                                var gradname = _hrmsContext.HR_Master_Grade.Where(t => t.HRMG_GradeName.Trim() == item.GradeName && t.MI_Id == data.MI_Id).ToList();
                                if (gradname.Count > 0)
                                {
                                    obj.HRMG_Id = gradname[0].HRMG_Id;
                                }
                                var maritalstus = _hrmsContext.IVRM_Master_Marital_Status.Where(t => t.IVRMMMS_MaritalStatus.Trim() == item.Marital_Status && t.MI_Id == data.MI_Id).ToList();
                                if (maritalstus.Count > 0)
                                {
                                    obj.IVRMMMS_Id = maritalstus[0].IVRMMMS_Id;
                                }
                                var GenderName = _hrmsContext.IVRM_Master_Gender.Where(t => t.IVRMMG_GenderName.Trim() == item.Gender_Name && t.MI_Id == data.MI_Id).ToList();
                                if (GenderName.Count > 0)
                                {
                                    obj.IVRMMG_Id = GenderName[0].IVRMMG_Id;
                                }
                                var castcatname = _hrmsContext.CasteCategory.Where(t => t.IMCC_CategoryName.Trim() == item.CasteCategory_Name ).ToList();
                                if (castcatname.Count > 0)
                                {
                                    obj.CasteCategoryId = castcatname[0].IMCC_Id;
                                }
                                var castname = _hrmsContext.Caste.Where(t => t.IMC_CasteName.Trim() == item.Caste_Name && t.MI_Id == data.MI_Id).ToList();
                                if (castname.Count > 0)
                                {
                                    obj.CasteId = castname[0].IMC_Id;
                                }
                                var ReligionName = _hrmsContext.Religion.Where(t => t.IVRMMR_Name.Trim() == item.Religion_Name).ToList();
                                if (ReligionName.Count > 0)
                                {
                                    obj.ReligionId = ReligionName[0].IVRMMR_Id;
                                }

                                _hrmsContext.Add(obj);
                                 _hrmsContext.SaveChanges();

                                Multiple_Email_DMO obj1 = new Multiple_Email_DMO();
                                obj1.HRME_Id = obj.HRME_Id;
                                obj1.HRMEM_EmailId = item.EmailID;
                                obj1.HRMEM_DeFaultFlag = "default";
                                _hrmsContext.Add(obj1);
                                Multiple_Mobile_DMO obj2 = new Multiple_Mobile_DMO();
                                obj2.HRME_Id = obj.HRME_Id;
                                obj2.HRMEMNO_MobileNo = item.MobileNo;
                                obj2.HRMEMNO_DeFaultFlag = "default";
                                _hrmsContext.Add(obj2);

                            }

                        }


                        var n = _hrmsContext.SaveChanges();


                        if (n > 0)
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
                data.failcnt += 1;
                data.returnval = false;
            }
            return data;
        }

        public EmployeeDataImportDTO getdetails(int id)
        {
            EmployeeDataImportDTO data = new EmployeeDataImportDTO();
            try
            {
                data.employeedetails = _hrmsContext.MasterEmployee.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public EmployeeDataImportDTO deactiveY(EmployeeDataImportDTO data)
        {
            try
            {
                var result = _hrmsContext.MasterEmployee.Single(t => t.HRME_Id == data.HRME_Id);

                if (result.HRME_ActiveFlag == true)
                {
                    result.HRME_ActiveFlag = false;
                }
                else if (result.HRME_ActiveFlag == false)
                {
                    result.HRME_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _hrmsContext.Update(result);
                int rowAffected = _hrmsContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}


