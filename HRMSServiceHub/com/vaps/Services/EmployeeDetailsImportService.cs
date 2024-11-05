using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeDetailsImportService : Interfaces.EmployeeDetailsImportInterface
        {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;

        public AdmissionRegisterContext adm_reg_context;
        public EmployeeDetailsImportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
            {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;
            }
        public async Task<MasterEmployeeImportDTO> save_excel_data(MasterEmployeeImportDTO stu)
            {

            int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            stu.Emp_type = "";
            try
                {
                List<MasterEmployeeImportDTO> failedlist = new List<MasterEmployeeImportDTO>();


                for (int i = 0; i < stu.newlstget.Length; i++)
                    {
                    try
                        {

                        MasterEmployee enq = new MasterEmployee();

                        stu.newlstget[i].MI_Id = stu.MI_Id;

                        if ((Convert.ToString(stu.newlstget[i].EmployeeCode) != null))
                            {
                            if ((Regex.IsMatch(Convert.ToString(stu.newlstget[i].EmployeeCode), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(stu.newlstget[i].EmployeeCode).Length <= 20))
                                {
                                //  emp_code.HRME_EmployeeCode = stu.newlstget[i].EmployeeCode;
                                }

                            else
                                {
                                stu.Emp_type = "Employee Code Name is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is Twenty";
                                return stu;
                                }
                            }
                        else
                            {
                            stu.Emp_type = "Employee Code Name can not be null";
                            return stu;
                            }


                        if ((stu.newlstget[i].EmployeeFirstName != null) && (stu.newlstget[i].EmployeeFirstName != ""))
                            {
                            if ((Regex.IsMatch(stu.newlstget[i].EmployeeFirstName, @"^[a-zA-Z]+$")) && ((stu.newlstget[i].EmployeeFirstName).Length <= 20))
                                {
                                //  emp_code.HRME_EmployeeFirstName = stu.newlstget[i].EmployeeFirstName;
                                }

                            else
                                {
                                stu.Emp_type = "Employee First Name is not Valid for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                                return stu;
                                }
                            }
                        else
                            {
                            stu.Emp_type = "Employee First Name can not be null for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }


                        if (((Regex.IsMatch(stu.newlstget[i].EmployeeMiddleName, @"^[a-zA-Z]+$")) && ((stu.newlstget[i].EmployeeMiddleName).Length <= 20)) || (stu.newlstget[i].EmployeeMiddleName == null) || (stu.newlstget[i].EmployeeMiddleName == ""))
                            {
                            // emp_code.HRME_EmployeeMiddleName = stu.newlstget[i].EmployeeMiddleName;
                            }

                        else
                            {
                            stu.Emp_type = "Employee Middle Name is not Valid for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }


                        if (((Regex.IsMatch(stu.newlstget[i].EmployeeLastName, @"^[a-zA-Z]+$")) && ((stu.newlstget[i].EmployeeLastName).Length <= 20)) || (stu.newlstget[i].EmployeeLastName == null) || (stu.newlstget[i].EmployeeLastName == ""))
                            {
                            //emp_code.HRME_EmployeeLastName = stu.newlstget[i].EmployeeLastName;
                            }

                        else
                            {
                            stu.Emp_type = "Employee Last Name is not Valid for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }

                        var empt_id_count = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id == stu.MI_Id && t.HRMET_EmployeeType.ToLower() == stu.newlstget[i].EmployeeType.ToLower()).ToList();
                        if (empt_id_count.Count > 0)
                            {
                            var empt_id = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id == stu.MI_Id && t.HRMET_EmployeeType.ToLower() == stu.newlstget[i].EmployeeType.ToLower()).FirstOrDefault();
                            stu.newlstget[i].HRMET_Id = empt_id.HRMET_Id;
                            }

                        else
                            {
                            stu.Emp_type = "Employe Type is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }

                        var emp_gr_type_count = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id == stu.MI_Id && t.HRMGT_EmployeeGroupType.ToLower() == stu.newlstget[i].EmployeeGroupType.ToLower()).ToList();
                        if (emp_gr_type_count.Count > 0)
                            {
                            var emp_gr_type = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id == stu.MI_Id && t.HRMGT_EmployeeGroupType.ToLower() == stu.newlstget[i].EmployeeGroupType.ToLower()).FirstOrDefault();
                            stu.newlstget[i].HRMGT_Id = emp_gr_type.HRMGT_Id;
                            }
                        else
                            {
                            stu.Emp_type = "Employee Group Type is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }
                        var emp_dep_name_count = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id == stu.MI_Id && t.HRMD_DepartmentName.ToLower() == stu.newlstget[i].DepartmentName.ToLower()).ToList();

                        if (emp_dep_name_count.Count > 0)
                            {
                            var emp_dep_name = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id == stu.MI_Id && t.HRMD_DepartmentName.ToLower() == stu.newlstget[i].DepartmentName.ToLower()).FirstOrDefault();
                            stu.newlstget[i].HRMD_Id = emp_dep_name.HRMD_Id;
                            }
                        else
                            {
                            stu.Emp_type = "Department Name is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }

                        var emp_desg_count = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id == stu.MI_Id && t.HRMDES_DesignationName.ToLower() == stu.newlstget[i].DesignationName.ToLower()).ToList();
                        if (emp_desg_count.Count > 0)
                            {
                            var emp_desg = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id == stu.MI_Id && t.HRMDES_DesignationName.ToLower() == stu.newlstget[i].DesignationName.ToLower()).FirstOrDefault();
                            stu.newlstget[i].HRMDES_Id = emp_desg.HRMDES_Id;
                            }
                        else
                            {
                            stu.Emp_type = "Designation Name is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }

                        var emp_grade_count = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id == stu.MI_Id && t.HRMG_GradeName.ToLower() == stu.newlstget[i].GradeName.ToLower()).ToList();
                        if (emp_grade_count.Count > 0)
                            {
                            var emp_grade = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id == stu.MI_Id && t.HRMG_GradeName.ToLower() == stu.newlstget[i].GradeName.ToLower()).FirstOrDefault();
                            stu.newlstget[i].HRMG_Id = emp_grade.HRMG_Id;

                            }
                        else
                            {
                            stu.Emp_type = "Grade Name is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }



                        var emp_marital_status_count = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.IVRMMMS_MaritalStatus.ToLower() == stu.newlstget[i].Marital_Status.ToLower()).ToList();
                        if (emp_marital_status_count.Count > 0)
                            {
                            var emp_marital_status = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.IVRMMMS_MaritalStatus.ToLower() == stu.newlstget[i].Marital_Status.ToLower()).FirstOrDefault();
                            stu.newlstget[i].IVRMMMS_Id = emp_marital_status.IVRMMMS_Id;
                            }
                        else
                            {
                            stu.Emp_type = "Marital Status is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }

                        var emp_master_gender_count = _HRMSContext.IVRM_Master_Gender.Where(t => t.IVRMMG_GenderName.ToLower() == stu.newlstget[i].Gender_Name.ToLower()).ToList();

                        if (emp_master_gender_count.Count > 0)
                            {
                            var emp_master_gender = _HRMSContext.IVRM_Master_Gender.Where(t => t.IVRMMG_GenderName.ToLower() == stu.newlstget[i].Gender_Name.ToLower()).FirstOrDefault();
                            stu.newlstget[i].IVRMMG_Id = emp_master_gender.IVRMMG_Id;
                            }
                        else
                            {
                            stu.Emp_type = "Gender Name is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }


                        var emp_caste_count = _HRMSContext.Caste.Where(t => t.IMC_CasteName.ToLower() == stu.newlstget[i].Caste_Name.ToLower()).ToList();
                        if (emp_caste_count.Count > 0)
                            {
                            var emp_caste = _HRMSContext.Caste.Where(t => t.IMC_CasteName.ToLower() == stu.newlstget[i].Caste_Name.ToLower()).FirstOrDefault();
                            stu.newlstget[i].CasteId = emp_caste.IMC_Id;
                            }

                        else
                            {
                            stu.Emp_type = "Caste Name is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }

                        var emp_caste_category_count = _HRMSContext.CasteCategory.Where(t => t.IMCC_CategoryName.ToLower() == stu.newlstget[i].CasteCategory_Name.ToLower()).ToList();
                        if (emp_caste_category_count.Count > 0)
                            {
                            var emp_caste_category = _HRMSContext.CasteCategory.Where(t => t.IMCC_CategoryName.ToLower() == stu.newlstget[i].CasteCategory_Name.ToLower()).FirstOrDefault();
                            stu.newlstget[i].CasteCategoryId = emp_caste_category.IMCC_Id;
                            }
                        else
                            {
                            stu.Emp_type = "Caste Category Name is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                            return stu;
                            }


                        var emp_master_relg_count = _HRMSContext.Religion.Where(t => t.IVRMMR_Name.ToLower() == stu.newlstget[i].Religion_Name.ToLower()).ToList();
                        if (emp_master_relg_count.Count > 0)
                        {
                        var emp_master_relg = _HRMSContext.Religion.Where(t => t.IVRMMR_Name.ToLower() == stu.newlstget[i].Religion_Name.ToLower()).FirstOrDefault();
                        stu.newlstget[i].ReligionId = emp_master_relg.IVRMMR_Id;
                        }
                        else
                        {
                        stu.Emp_type = "Religion is Not Available for" + " " + stu.newlstget[i].EmployeeCode + " " + "Employee code";
                        return stu;
                        }

                        var emp_code_count = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == stu.MI_Id && t.HRME_EmployeeCode.ToLower() == stu.newlstget[i].EmployeeCode.ToLower()).ToList();

                        if (emp_code_count.Count > 0)
                            {

                            //Updating the Existing Records--------------------------------------------------------------------------------------------------------------------------------------
                            var emp_code = _HRMSContext.MasterEmployee.Single(t => t.MI_Id == stu.MI_Id && t.HRME_EmployeeCode.ToLower() == stu.newlstget[i].EmployeeCode.ToLower());

                            stu.newlstget[i].CreatedDate = emp_code.CreatedDate;
                            stu.newlstget[i].UpdatedDate = DateTime.Now;
                            stu.newlstget[i].HRME_ActiveFlag = true;
                            Mapper.Map(stu.newlstget[i], emp_code);
                            _HRMSContext.Update(emp_code);
                            var flag = await _HRMSContext.SaveChangesAsync();
                            if (flag >= 1)
                                {
                                stu.stuStatus = "true";
                                sucesscount = sucesscount + 1;
                                }

                            else
                                {
                                stu.stuStatus = "false";
                                failcount = failcount + 1;
                                string name = failnames;
                                finalnames += name + ",";


                                failedlist.Add(stu.newlstget[i]);

                                //stu.failedlist[i] = stu.newlstget[i];
                                }
                            // stu.returnMsg += "Total Records Updated :'" + sucesscount + "'  , Total Records Failed : '" + failcount;
                            }


                        //Adding new Records---------------------------------------------------------------------------------------------------------------------------------------------------------------------


                        else
                        {
                            enq = Mapper.Map<MasterEmployee>(stu.newlstget[i]);
                            
                            enq.CreatedDate = DateTime.Now;
                            enq.UpdatedDate = DateTime.Now;

                            _HRMSContext.Add(enq);
                            var flag = await _HRMSContext.SaveChangesAsync();
                            if (flag >= 1)
                                {

                                MasterEmployeeDTO DTO = Mapper.Map<MasterEmployeeDTO>(enq);
                                DTO.HRME_ActiveFlag = true;
                                DTO.HRME_EmployeeOrder = Convert.ToInt32(DTO.HRME_Id);
                                var resultd = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == DTO.HRME_Id);
                                Mapper.Map(DTO, resultd);
                                _HRMSContext.Update(resultd);
                                _HRMSContext.SaveChanges();

                                Multiple_Mobile_DMO mob = new Multiple_Mobile_DMO();
                                mob.HRME_Id = enq.HRME_Id;
                                mob.HRMEMNO_MobileNo = stu.newlstget[i].MobileNo;
                                mob.HRMEMNO_DeFaultFlag = "default";
                                mob.CreatedDate = DateTime.Now;
                                mob.UpdatedDate = DateTime.Now;
                                _HRMSContext.Add(mob);
                                _HRMSContext.SaveChanges();

                                Multiple_Email_DMO mail = new Multiple_Email_DMO();
                                mail.HRME_Id = enq.HRME_Id;
                                mail.HRMEM_EmailId = stu.newlstget[i].EmailId;
                                mail.HRMEM_DeFaultFlag = "default";
                                mail.CreatedDate = DateTime.Now;
                                mail.UpdatedDate = DateTime.Now;
                                _HRMSContext.Add(mail);
                                _HRMSContext.SaveChanges();

                                stu.stuStatus = "true";
                                sucesscount = sucesscount + 1;
                            }
                            else
                            {
                                stu.stuStatus = "false";
                                failcount = failcount + 1;
                                string name = failnames;
                                finalnames += name + ",";


                                failedlist.Add(stu.newlstget[i]);
                                }
                            }
                        }
                    catch (Exception ex)
                        {
                        Console.Write(ex.Message);
                        stu.stuStatus = "false";
                        failcount = failcount + 1;
                        string name = failnames;
                        finalnames += name + ",";
                        //failnames = failnames + "," + failnames;
                        failedlist.Add(stu.newlstget[i]);
                        // stu.returnMsg += "Total Records Inserted :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "'  , Please Export the Excel Sheet to See the Failed Records:'";
                        //stu.failedlist[i] = stu.newlstget[i];
                        continue;

                        }


                    }
                stu.failedlist = failedlist.ToArray();
                }
            catch (Exception ex)
                {
                Console.Write(ex.Message);
                stu.returnMsg = "Total Records Inserted :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "'  , Please Export the Excel Sheet to See the Failed Records:'";
                }

            stu.returnMsg = "Total Records Inserted :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "'  , Please Export the Excel Sheet to See the Failed Records:'";


            return stu;
            }

        }

}
