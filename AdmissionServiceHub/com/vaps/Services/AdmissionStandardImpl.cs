using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AdmissionStandardImpl: Interfaces.AdmissionStandardInterface
    {
        private static ConcurrentDictionary<string, AdmissionStandardDTO> _login =
            new ConcurrentDictionary<string, AdmissionStandardDTO>();

        public AdmissionStandardContext _AdmissionStandardContext;
        public AdmissionStandardImpl(AdmissionStandardContext AdmissionStandardContext)
        {
            _AdmissionStandardContext = AdmissionStandardContext;
        }      

        public AdmissionStandardDTO getlistdata(int id)
        {
            AdmissionStandardDTO acdmc = new AdmissionStandardDTO();
            try
            {
                List<AdmissionStandardDMO> allconfigurations = new List<AdmissionStandardDMO>();
                allconfigurations = _AdmissionStandardContext.AdmissionStandardDMO.Where(t=>t.MI_Id==id).ToList();
                acdmc.fillconfig = allconfigurations.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }
        public AdmissionStandardDTO getlisttwo(AdmissionStandardDTO stu)
        {
            try
            {
                if(stu.ASC_Id > 0)
                {
                    var result = _AdmissionStandardContext.AdmissionStandardDMO.Single(t => t.ASC_Id == stu.ASC_Id);
                    result.MI_Id = stu.MI_Id;
                    // admiss.ASC_Id = stu.ASC_Id;
                    result.ASC_Adm_AddFieldsFlag = stu.ASC_Adm_AddFieldsFlag;
                    result.ASC_TC_AddFieldsFlag = stu.ASC_TC_AddFieldsFlag;
                    result.ASC_Att_DefaultEntry_Type = stu.ASC_Att_DefaultEntry_Type;
                    result.ASC_MaxAgeApl_Flag = stu.ASC_MaxAgeApl_Flag;
                    result.ASC_MinAgeApl_Flag = stu.ASC_MinAgeApl_Flag;
                    result.ASC_AdmNo_RegNo_RollNo_DefaultFlag = stu.ASC_AdmNo_RegNo_RollNo_DefaultFlag;
                    result.ASC_DefaultDisplay_Flag = stu.ASC_DefaultDisplay_Flag;
                    result.ASC_Default_Gender = stu.ASC_Default_Gender;
                    result.ASC_ParentsAnnualIncome_Flag = stu.ASC_ParentsAnnualIncome_Flag;
                    result.ASC_ParentsMonthlyIncome_Flag = stu.ASC_ParentsMonthlyIncome_Flag;
                    result.ASC_DefaultSMS_Flag = stu.ASC_DefaultSMS_Flag;
                    result.ASC_School_Address = stu.ASC_School_Address;
                    result.ASC_Category_Address = stu.ASC_Category_Address;
                    result.ASC_DefaultPhotoUpload = stu.ASC_DefaultPhotoUpload;
                    result.ASC_Stu_Photo_Path = stu.ASC_Stu_Photo_Path;
                    result.ASC_Staff_Photo_Path = stu.ASC_Staff_Photo_Path;
                    result.ASC_Logo_Path = stu.ASC_Logo_Path;
                    result.ASC_Doc_Path = stu.ASC_Doc_Path;
                    result.ASC_Att_Default_OrderFlag = stu.ASC_Att_Default_OrderFlag;
                    result.ASC_Default_Clm__Adm_Flag = stu.ASC_Default_Clm__Adm_Flag;
                    result.ASC_Default_Clm__Rol_Flag = stu.ASC_Default_Clm__Rol_Flag;
                    result.ASC_Default_Clm__Reg_Flag = stu.ASC_Default_Clm__Reg_Flag;
                    result.ASC_DefaultSMS_Flag = stu.ASC_DefaultSMS_Flag;
                    result.ADMC_TCAllowBalanceFlg = stu.ASC_TC_Payment;
                  result.ASC_LibraryAllowBalanceFlg = stu.ASC_TC_Library;
                    result.ASC_ECS_Flag = stu.ASC_ECS_Flag;
                    result.ASC_Att_Scheduler_Flag = stu.ASC_Att_Scheduler_Flag;
                    result.CreatedDate = result.CreatedDate;
                    result.UpdatedDate = DateTime.Now;
                    _AdmissionStandardContext.Update(result);
                   int contactExists=_AdmissionStandardContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        stu.returnval = true;                        
                    }
                    else
                    {
                        stu.returnval = false;                       
                    }
                }
                else
                {
                    var duplicate = _AdmissionStandardContext.AdmissionStandardDMO.Where(d => d.ASC_AdmNo_RegNo_RollNo_DefaultFlag == stu.ASC_AdmNo_RegNo_RollNo_DefaultFlag && d.ASC_Adm_AddFieldsFlag == stu.ASC_Adm_AddFieldsFlag && d.ASC_Att_DefaultEntry_Type == stu.ASC_Att_DefaultEntry_Type
                      && d.ASC_Att_Default_OrderFlag == stu.ASC_Att_Default_OrderFlag && d.ASC_Category_Address == stu.ASC_Category_Address
                      && d.ASC_DefaultDisplay_Flag == stu.ASC_DefaultDisplay_Flag && d.ASC_DefaultPhotoUpload == stu.ASC_DefaultPhotoUpload
                      && d.ASC_DefaultSMS_Flag == stu.ASC_DefaultSMS_Flag && d.ASC_Default_Clm__Adm_Flag == stu.ASC_Default_Clm__Adm_Flag
                      && d.ASC_Default_Clm__Reg_Flag == stu.ASC_Default_Clm__Reg_Flag && d.ASC_Default_Clm__Rol_Flag == stu.ASC_Default_Clm__Rol_Flag && d.ASC_Default_Gender == stu.ASC_Default_Gender &&
                      d.ASC_Doc_Path == stu.ASC_Doc_Path && d.ASC_Logo_Path == stu.ASC_Logo_Path && d.ASC_MaxAgeApl_Flag == stu.ASC_MaxAgeApl_Flag
                      && d.ASC_MinAgeApl_Flag == stu.ASC_MinAgeApl_Flag && d.ASC_ParentsAnnualIncome_Flag == stu.ASC_ParentsAnnualIncome_Flag && d.ASC_ParentsMonthlyIncome_Flag == stu.ASC_ParentsMonthlyIncome_Flag && d.ASC_School_Address == stu.ASC_School_Address && d.ASC_Staff_Photo_Path == stu.ASC_Staff_Photo_Path && d.ASC_Stu_Photo_Path == stu.ASC_Stu_Photo_Path && d.ASC_TC_AddFieldsFlag == stu.ASC_TC_AddFieldsFlag && d.ASC_Id==stu.ASC_Id && d.ADMC_TCAllowBalanceFlg == stu.ASC_TC_Payment && d.ASC_ECS_Flag==stu.ASC_ECS_Flag && d.ASC_Att_Scheduler_Flag == stu.ASC_Att_Scheduler_Flag).ToList();
                    if(duplicate.Count == 0)
                    {
                        AdmissionStandardDMO admiss = Mapper.Map<AdmissionStandardDMO>(stu);                      

                        admiss.MI_Id =stu.MI_Id ;
                        // admiss.ASC_Id = stu.ASC_Id;
                        admiss.ASC_Adm_AddFieldsFlag = stu.ASC_Adm_AddFieldsFlag;
                        admiss.ASC_TC_AddFieldsFlag = stu.ASC_TC_AddFieldsFlag;
                        admiss.ASC_Att_DefaultEntry_Type = stu.ASC_Att_DefaultEntry_Type;
                        admiss.ASC_MaxAgeApl_Flag = stu.ASC_MaxAgeApl_Flag;
                        admiss.ASC_MinAgeApl_Flag = stu.ASC_MinAgeApl_Flag;
                        admiss.ASC_AdmNo_RegNo_RollNo_DefaultFlag = stu.ASC_AdmNo_RegNo_RollNo_DefaultFlag;
                        admiss.ASC_DefaultDisplay_Flag = stu.ASC_DefaultDisplay_Flag;
                        admiss.ASC_Default_Gender = stu.ASC_Default_Gender;
                        admiss.ASC_ParentsAnnualIncome_Flag = stu.ASC_ParentsAnnualIncome_Flag;
                        admiss.ASC_ParentsMonthlyIncome_Flag = stu.ASC_ParentsMonthlyIncome_Flag;
                        admiss.ASC_DefaultSMS_Flag = stu.ASC_DefaultSMS_Flag;
                        admiss.ASC_School_Address = stu.ASC_School_Address;
                        admiss.ASC_Category_Address = stu.ASC_Category_Address;
                        admiss.ASC_DefaultPhotoUpload = stu.ASC_DefaultPhotoUpload;
                        admiss.ASC_Stu_Photo_Path = stu.ASC_Stu_Photo_Path;
                        admiss.ASC_Staff_Photo_Path = stu.ASC_Staff_Photo_Path;
                        admiss.ASC_Logo_Path = stu.ASC_Logo_Path;
                        admiss.ASC_Doc_Path = stu.ASC_Doc_Path;
                        admiss.ASC_Att_Default_OrderFlag = stu.ASC_Att_Default_OrderFlag;
                        admiss.ASC_Default_Clm__Adm_Flag = stu.ASC_Default_Clm__Adm_Flag;
                        admiss.ASC_Default_Clm__Rol_Flag = stu.ASC_Default_Clm__Rol_Flag;
                        admiss.ASC_Default_Clm__Reg_Flag = stu.ASC_Default_Clm__Reg_Flag;
                        admiss.ASC_DefaultSMS_Flag = stu.ASC_DefaultSMS_Flag;
                        admiss.ADMC_TCAllowBalanceFlg = stu.ASC_TC_Payment;
                        admiss.ASC_ECS_Flag = stu.ASC_ECS_Flag;
                        admiss.ASC_Att_Scheduler_Flag = stu.ASC_Att_Scheduler_Flag;
                        admiss.CreatedDate = DateTime.Now;
                        admiss.UpdatedDate = DateTime.Now;
                        _AdmissionStandardContext.Add(admiss);
                        int contactExists = _AdmissionStandardContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            stu.returnval = true;
                        }
                        else
                        {
                            stu.returnval = false;
                        }
                    }
                    else
                    {
                        stu.message = "Entries Already Exists With Provided Inputs";
                    }
                }              
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }

        // Admission Cancel Configuration 
        public AdmissionStandardDTO CancelConfigLoad(AdmissionStandardDTO data)
        {
            try
            {
                data.getdetails = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionStandardDTO SaveCancelConfigData(AdmissionStandardDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.AACC_Id > 0)
                {
                    var checkduplicate = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.AACC_Id != data.AACC_Id
                     && a.AACC_ToDays > data.AACC_FromDays).ToList();

                    var checkduplicate1 = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.AACC_Id != data.AACC_Id
                     && a.AACC_FromDays >= data.AACC_ToDays && a.AACC_ToDays <= data.AACC_ToDays).ToList();
                    
                    var result = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Single(a => a.MI_Id == data.MI_Id && a.AACC_Id == data.AACC_Id);
                    result.AACC_FromDays = data.AACC_FromDays;
                    result.AACC_ToDays = data.AACC_ToDays;
                     
                    result.AACC_CancellationPer = data.AACC_CancellationPer;
                    result.AACC_UpdatedDate = indiantime0;
                    result.AACC_UpdatedBy = data.UserId;

                    _AdmissionStandardContext.Update(result);
                    var i = _AdmissionStandardContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Update";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Update";
                        data.returnval = false;
                    }
                   
                }
                else
                {
                    var checkduplicate = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Where(a => a.MI_Id == data.MI_Id
                   && a.AACC_ToDays > data.AACC_FromDays).ToList();

                    var checkduplicate1 = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Where(a => a.MI_Id == data.MI_Id
                     && a.AACC_FromDays > data.AACC_ToDays && a.AACC_ToDays <= data.AACC_ToDays).ToList();

                    
                    Adm_AdmissionCancel_ConfigDMO result = new Adm_AdmissionCancel_ConfigDMO();
                    result.MI_Id = data.MI_Id;
                    result.AACC_DOAFlg = data.AACC_DOAFlg;
                    result.AACC_FromDays = data.AACC_FromDays;
                    result.AACC_ToDays = data.AACC_ToDays;
                    
                    result.AACC_CancellationPer = data.AACC_CancellationPer;
                    result.AACC_UpdatedDate = indiantime0;
                    result.AACC_CreatedDate = indiantime0;
                    result.AACC_UpdatedBy = data.UserId;
                    result.AACC_CreatedBy = data.UserId;
                    result.AACC_ActiveFlag = true;
                    _AdmissionStandardContext.Add(result);
                    var i = _AdmissionStandardContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionStandardDTO EditCancelConfig(AdmissionStandardDTO data)
        {
            try
            {
                data.editdetails = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.AACC_Id == data.AACC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionStandardDTO ActiveDeactiveCancelConfig(AdmissionStandardDTO data)
        {
            try
            {
                if (data.AACC_Id > 0)
                {
                    var result = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.AACC_Id == data.AACC_Id).ToList();
                    if (result.Count() > 0)
                    {
                        var result1 = _AdmissionStandardContext.Adm_AdmissionCancel_ConfigDMO.Single(a => a.MI_Id == data.MI_Id && a.AACC_Id == data.AACC_Id);
                        _AdmissionStandardContext.Remove(result1);

                        int i = _AdmissionStandardContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
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
