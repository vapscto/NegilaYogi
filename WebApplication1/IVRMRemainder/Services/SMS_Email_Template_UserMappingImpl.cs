using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.IVRMRemainder.Services
{
    public class SMS_Email_Template_UserMappingImpl : Interface.SMS_Email_Template_UserMappingInterface
    {
        public DomainModelMsSqlServerContext _context;

        public SMS_Email_Template_UserMappingImpl(DomainModelMsSqlServerContext _cont)
        {
            _context = _cont;
        }

        public IVRM_RemaindersDTO OnChangeOfInstitution(IVRM_RemaindersDTO data)
        {
            try
            {
                var checktemplate = _context.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == data.ISES_Template_Name).ToList();

                if (checktemplate.Count > 0)
                {
                    data.GetTemplateDetails = checktemplate.ToArray();
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Remainders_InstitutionWise_UserDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TemplateName", SqlDbType.VarChar) { Value = "" });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "1" });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetUserDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Remainders_InstitutionWise_UserDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TemplateName", SqlDbType.VarChar) { Value = data.ISES_Template_Name });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "2" });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetMappedUserDetails = retObject.ToArray();
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
        public IVRM_RemaindersDTO OnSaveUserMapping(IVRM_RemaindersDTO data)
        {
            try
            {
                if(data.ISES_Id > 0)
                {
                    var UpdateResult = _context.smsEmailSetting.Single(a => a.ISES_Id == data.ISES_Id);
                    UpdateResult.ISES_SMSMessage = data.ISES_TemplateContent;
                    UpdateResult.ISES_Mail_Message = data.ISES_TemplateContent;
                    UpdateResult.ISES_MailBody = data.ISES_TemplateContent;
                    UpdateResult.ISES_MailHTMLTemplate = data.ISES_TemplateContent;
                    UpdateResult.UpdatedDate = DateTime.Now;
                    UpdateResult.ISES_IVRSTextMsg = data.ISES_TemplateContent;
                    _context.Update(UpdateResult);
                }
                else
                {
                    var GetInstitutionmoduleId = (from a in _context.masterModule
                                                  from b in _context.Institution_Module
                                                  where (a.IVRMM_Id == b.IVRMM_Id && b.MI_Id == data.MI_Id && a.IVRMM_ModuleName.ToUpper().Contains("FEE"))
                                                  select b).Distinct().ToList();

                    if (GetInstitutionmoduleId.Count > 0)
                    {
                        SMSEmailSetting sMSEmailSetting = new SMSEmailSetting();

                        sMSEmailSetting.MI_Id = data.MI_Id;
                        sMSEmailSetting.IVRMIM_Id = GetInstitutionmoduleId.FirstOrDefault().IVRMIM_Id;
                        sMSEmailSetting.ISES_Template_Name = data.ISES_Template_Name;
                        sMSEmailSetting.ISES_SMSMessage = data.ISES_TemplateContent;
                        sMSEmailSetting.ISES_SMSActiveFlag = true;
                        sMSEmailSetting.ISES_MailSubject = data.ISES_Template_Name;
                        sMSEmailSetting.ISES_MailBody = data.ISES_TemplateContent;
                        sMSEmailSetting.ISES_MailFooter = "Thank You";
                        sMSEmailSetting.ISES_Mail_Message = data.ISES_TemplateContent;
                        sMSEmailSetting.ISES_MailHTMLTemplate = data.ISES_TemplateContent;
                        sMSEmailSetting.ISES_MailActiveFlag = true;
                        sMSEmailSetting.IVRMSTAUL_Id = 3;
                        sMSEmailSetting.IVRMIMP_Id = 3;
                        sMSEmailSetting.CreatedDate = DateTime.Now;
                        sMSEmailSetting.UpdatedDate = DateTime.Now;
                        sMSEmailSetting.ISES_IVRSTextMsg = data.ISES_TemplateContent;
                        sMSEmailSetting.ISES_EnableSMSCCFlg = false;
                        sMSEmailSetting.ISES_SMSCCMobileNo = null;
                        sMSEmailSetting.ISES_EnableMailCCFlg = false;
                        sMSEmailSetting.ISES_EnableMailBCCFlg = false;
                        _context.Add(sMSEmailSetting);
                        data.ISES_Id = sMSEmailSetting.ISES_Id;
                    }
                    else
                    {
                        data.returnvalue = "ModulesNotMapped";
                        return data;
                    }                    
                }

                if (data.Temp_UserMapping_Add != null && data.Temp_UserMapping_Add.Length > 0)
                {
                    foreach (var d in data.Temp_UserMapping_Add)
                    {
                        SMSEmailSettingUserMapping sMSEmailSettingUserMapping = new SMSEmailSettingUserMapping();
                        sMSEmailSettingUserMapping.ISES_Id = data.ISES_Id;
                        sMSEmailSettingUserMapping.UserId = d.UserId;
                        sMSEmailSettingUserMapping.ISESUSR_ActiveFlg = true;
                        sMSEmailSettingUserMapping.ISESUSR_CreatedDate = DateTime.Now;
                        sMSEmailSettingUserMapping.ISESUSR_UpdatedDate = DateTime.Now;
                        sMSEmailSettingUserMapping.ISESUSR_CreatedBy = 3;
                        sMSEmailSettingUserMapping.ISESUSR_UpdatedBy = 3;
                        _context.Add(sMSEmailSettingUserMapping);
                    }
                }

                if (data.Temp_UserMapping_Remove != null && data.Temp_UserMapping_Remove.Length > 0)
                {
                    List<long> temparr1 = new List<long>();

                    foreach (Temp_UserMapping_Remove ph in data.Temp_UserMapping_Remove)
                    {
                        temparr1.Add(ph.ISESUSR_Id);
                    }

                    Array Phone_Noresultremove = _context.SMSEmailSettingUserMapping.Where(t => temparr1.Contains(t.ISESUSR_Id)
                    && t.ISES_Id == data.ISES_Id).ToArray();

                    foreach (SMSEmailSettingUserMapping ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }
                }

                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnvalue = "Save";
                }
                else
                {
                    data.returnvalue = "Failed";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}