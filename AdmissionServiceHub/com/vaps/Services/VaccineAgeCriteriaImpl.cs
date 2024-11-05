using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class VaccineAgeCriteriaImpl : Interfaces.VaccineAgeCriteriaInterface
    {
        public AdmissionFormContext _context;

        public VaccineAgeCriteriaImpl(AdmissionFormContext _cont)
        {
            _context = _cont;
        }

        public VaccineAgeCriteriaDTO OnLoadVaccineAgeCriteriaDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.GetAgeCriteriaDetails = _context.VaccineAgeCriteriaDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO SaveVaccineAgeDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ASVAC_Id > 0)
                {
                    var result = _context.VaccineAgeCriteriaDMO.Where(a => a.MI_Id == data.MI_Id && a.ASVAC_Id == data.ASVAC_Id).ToList();

                    if (result.Count > 0)
                    {
                        var update_result = _context.VaccineAgeCriteriaDMO.Single(a => a.MI_Id == data.MI_Id && a.ASVAC_Id == data.ASVAC_Id);
                        update_result.ASVAC_AgeStartNo = data.ASVAC_AgeStartNo;
                        update_result.ASVAC_AgeEndNo = data.ASVAC_AgeEndNo;
                        update_result.ASVAC_UpdatedDate = indiantime0;
                        _context.Update(update_result);
                        AddUpdateVaccineAgeCriteriaDetails(data);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.ReturnValue = true;
                        }
                    }
                }
                else
                {
                    VaccineAgeCriteriaDMO vaccineAgeCriteriaDMO = new VaccineAgeCriteriaDMO();
                    vaccineAgeCriteriaDMO.MI_Id = data.MI_Id;
                    vaccineAgeCriteriaDMO.ASVAC_AgeStartNo = data.ASVAC_AgeStartNo;
                    vaccineAgeCriteriaDMO.ASVAC_AgeEndNo = data.ASVAC_AgeEndNo;
                    vaccineAgeCriteriaDMO.ASVAC_ActiveFlag = true;
                    vaccineAgeCriteriaDMO.ASVAC_CreatedDate = indiantime0;
                    vaccineAgeCriteriaDMO.ASVAC_UpdatedDate = indiantime0;
                    _context.Add(vaccineAgeCriteriaDMO);
                    data.ASVAC_Id = vaccineAgeCriteriaDMO.ASVAC_Id;
                    AddUpdateVaccineAgeCriteriaDetails(data);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.ReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO EditVaccineAgeDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.GetEditAgeCriteriaDetails = _context.VaccineAgeCriteriaDMO.Where(a => a.MI_Id == data.MI_Id && a.ASVAC_Id == data.ASVAC_Id).ToArray();

                data.GetEditViewVaccineDetails = _context.VaccineAgeCriteriaDetailsDMO.Where(a => a.ASVAC_Id == data.ASVAC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO ActiveDeactiveVaccineAgeDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _context.VaccineAgeCriteriaDMO.Where(a => a.MI_Id == data.MI_Id && a.ASVAC_Id == data.ASVAC_Id).ToList();
                if (result.Count > 0)
                {
                    var update_result = _context.VaccineAgeCriteriaDMO.Single(a => a.ASVAC_Id == data.ASVAC_Id);

                    update_result.ASVAC_ActiveFlag = update_result.ASVAC_ActiveFlag == true ? false : true;
                    update_result.ASVAC_UpdatedDate = indiantime0;
                    _context.Update(update_result);
                    var i = _context.SaveChanges();

                    if (i > 0)
                    {
                        data.ReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO OnClickViewDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.GetViewVaccineDetails = _context.VaccineAgeCriteriaDetailsDMO.Where(a => a.ASVAC_Id == data.ASVAC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO ActiveDeactiveVaccineDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _context.VaccineAgeCriteriaDetailsDMO.Where(a => a.ASVACD_Id == data.ASVACD_Id).ToList();
                if (result.Count > 0)
                {
                    var update_result = _context.VaccineAgeCriteriaDetailsDMO.Single(a => a.ASVACD_Id == data.ASVACD_Id);

                    update_result.ASVACD_ActiveFlag = update_result.ASVACD_ActiveFlag == true ? false : true;
                    update_result.ASVACD_UpdatedDate = indiantime0;
                    var i = _context.SaveChanges();

                    if (i > 0)
                    {
                        data.ReturnValue = true;
                    }
                }


                data.GetViewVaccineDetails = _context.VaccineAgeCriteriaDetailsDMO.Where(a => a.ASVAC_Id == data.ASVAC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Vaccine Student Details
        public VaccineAgeCriteriaDTO OnLoadVaccineStudentDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.GetAccademicYear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetAgeCriteriaStudentDetails = _context.VaccineAgeCriteriaDMO.Where(a => a.MI_Id == data.MI_Id && a.ASVAC_ActiveFlag == true).Distinct().ToArray();

                data.Getstudentvaccinedetails = (from a in _context.Adm_StudentWise_Vaccine_DetailsDMO
                                                 from b in _context.Adm_M_Student
                                                 from c in _context.VaccineAgeCriteriaDetailsDMO
                                                 from d in _context.VaccineAgeCriteriaDMO
                                                 where (a.AMST_Id == b.AMST_Id && a.ASVACD_Id == c.ASVACD_Id && c.ASVAC_Id == d.ASVAC_Id
                                                 && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                                 select new VaccineAgeCriteriaDTO
                                                 {
                                                     AMST_Id = a.AMST_Id,
                                                     StudentName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                 (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                 (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                     AMST_AdmNo = b.AMST_AdmNo,
                                                     vaccineagecriteria = d.ASVAC_AgeStartNo + " To " + d.ASVAC_AgeEndNo,
                                                     ASVAC_Id = d.ASVAC_Id
                                                 }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO GetStudentDetailsBySearch(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                data.GetStudentSearchList = (from a in _context.Adm_M_Student
                                             from b in _context.SchoolYearWiseStudent
                                             where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                             && b.AMAY_ActiveFlag == 1 && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                             select new VaccineAgeCriteriaDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                                 (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName) +
                                                 ':' + a.AMST_AdmNo).Trim(),
                                             }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO SearchVaccineStudentDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.GetAgeCriteriaVaccineDetails = _context.VaccineAgeCriteriaDetailsDMO.Where(a => a.ASVAC_Id == data.ASVAC_Id
                && a.ASVACD_ActiveFlag == true).ToArray();

                data.GetSavedStudentVaccineDetails = (from a in _context.Adm_StudentWise_Vaccine_DetailsDMO
                                                      from b in _context.Adm_M_Student
                                                      from c in _context.VaccineAgeCriteriaDetailsDMO
                                                      from d in _context.VaccineAgeCriteriaDMO
                                                      where (a.AMST_Id == b.AMST_Id && a.ASVACD_Id == c.ASVACD_Id && c.ASVAC_Id == d.ASVAC_Id
                                                      && d.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.ASVAC_Id == data.ASVAC_Id
                                                      && d.ASVAC_Id == data.ASVAC_Id && a.AMST_Id == data.AMST_Id)
                                                      select a).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO SaveStudentVaccineDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.SaveStudentVaccineDetails_Temp != null && data.SaveStudentVaccineDetails_Temp.Length > 0)
                {
                    foreach (var c in data.SaveStudentVaccineDetails_Temp)
                    {
                        if (c.ASWVD_Id > 0)
                        {
                            var result = _context.Adm_StudentWise_Vaccine_DetailsDMO.Where(a => a.ASWVD_Id == c.ASWVD_Id).ToList();
                            if (result.Count > 0)
                            {
                                var result_update = _context.Adm_StudentWise_Vaccine_DetailsDMO.Single(a => a.ASWVD_Id == c.ASWVD_Id);
                                result_update.ASWVD_NextDoseDate = c.ASWVD_NextDoseDate;
                                result_update.ASWVD_AdministeredBy = c.ASWVD_AdministeredBy;
                                if (c.ASWVD_DateGiven != null)
                                {
                                    result_update.ASWVD_DateGiven = c.ASWVD_DateGiven;
                                }

                                result_update.ASWVD_CreatedDate = indiantime0;
                                _context.Update(result_update);
                            }
                        }
                        else
                        {
                            Adm_StudentWise_Vaccine_DetailsDMO adm_StudentWise_Vaccine_DetailsDMO = new Adm_StudentWise_Vaccine_DetailsDMO();
                            adm_StudentWise_Vaccine_DetailsDMO.AMST_Id = data.AMST_Id;
                            adm_StudentWise_Vaccine_DetailsDMO.ASVACD_Id = c.ASVACD_Id;
                            adm_StudentWise_Vaccine_DetailsDMO.ASWVD_NextDoseDate = c.ASWVD_NextDoseDate;
                            adm_StudentWise_Vaccine_DetailsDMO.ASWVD_AdministeredBy = c.ASWVD_AdministeredBy;
                            if (c.ASWVD_DateGiven != null)
                            {
                                adm_StudentWise_Vaccine_DetailsDMO.ASWVD_DateGiven = c.ASWVD_DateGiven;
                            }
                            adm_StudentWise_Vaccine_DetailsDMO.ASWVD_ActiveFlag = true;
                            adm_StudentWise_Vaccine_DetailsDMO.ASWVD_CreatedDate = indiantime0;
                            adm_StudentWise_Vaccine_DetailsDMO.ASWVD_UpdatedDate = indiantime0;
                            _context.Add(adm_StudentWise_Vaccine_DetailsDMO);
                        }
                    }
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.ReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VaccineAgeCriteriaDTO OnClickViewStudentVaccineDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                data.GetViewstudentvaccinedetails = (from a in _context.Adm_StudentWise_Vaccine_DetailsDMO
                                                     from b in _context.Adm_M_Student
                                                     from c in _context.VaccineAgeCriteriaDetailsDMO
                                                     from d in _context.VaccineAgeCriteriaDMO
                                                     where (a.AMST_Id == b.AMST_Id && a.ASVACD_Id == c.ASVACD_Id && c.ASVAC_Id == d.ASVAC_Id
                                                     && d.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.ASVAC_Id == data.ASVAC_Id
                                                     && d.ASVAC_Id == data.ASVAC_Id && a.AMST_Id == data.AMST_Id)
                                                     select new VaccineAgeCriteriaDTO
                                                     {
                                                         ASVACD_VaccineName = c.ASVACD_VaccineName,
                                                         ASVACD_VaccineType = c.ASVACD_VaccineType,
                                                         ASVACD_Id = a.ASVACD_Id,
                                                         ASWVD_Id = a.ASWVD_Id,
                                                         ASWVD_DateGiven = a.ASWVD_DateGiven,
                                                         ASWVD_NextDoseDate = a.ASWVD_NextDoseDate,
                                                         ASWVD_AdministeredBy = a.ASWVD_AdministeredBy,
                                                         ASWVD_ActiveFlag = a.ASWVD_ActiveFlag
                                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<VaccineAgeCriteriaDTO> VaccineDueDateWebJobsApi(VaccineAgeCriteriaDTO data)
        {
            try
            {
                List<VaccineAgeCriteriaDTO> VaccineAgeCriteriaDTO = new List<VaccineAgeCriteriaDTO>();
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                string duedate = indiantime0.ToString("yyyy-MM-dd");

                var template = _context.SMSEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "Vaccine_Due_Date").ToList();

                if(template.FirstOrDefault().ISES_SMSActiveFlag==true || template.FirstOrDefault().ISES_MailActiveFlag == true)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Student_Vaccination_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@Currentdate", SqlDbType.VarChar) { Value = duedate });
                        cmd.Parameters.Add(new SqlParameter("@TemplateName", SqlDbType.VarChar) { Value = "Vaccine_Due_Date" });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var resultc = cmd.ExecuteReader())
                            {
                                while (resultc.Read())
                                {
                                    VaccineAgeCriteriaDTO.Add(new VaccineAgeCriteriaDTO
                                    {
                                        MobileNo = Convert.ToInt64(resultc["AMST_MobileNo"]),
                                        AMST_Id = Convert.ToInt64(resultc["AMST_Id"]),
                                        StudentName = Convert.ToString(resultc["studentname"]),
                                        EmailId = Convert.ToString(resultc["AMST_emailId"]),
                                        ASVACD_VaccineName = Convert.ToString(resultc["ASVACD_VaccineName"]),
                                        ASVACD_VaccineType = Convert.ToString(resultc["ASVACD_VaccineType"]),
                                        VaccinationDueDate = Convert.ToString(resultc["VaccinationDueDate"]),

                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    try
                    {
                        if (VaccineAgeCriteriaDTO.Count() > 0)
                        {
                            for (int i = 0; i < VaccineAgeCriteriaDTO.Count(); i++)
                            {
                                long MI_id = data.MI_Id;
                                long MobileNo = Convert.ToInt64(VaccineAgeCriteriaDTO[i].MobileNo);
                                long AMST_Id = VaccineAgeCriteriaDTO[i].AMST_Id;
                                string StudentName = VaccineAgeCriteriaDTO[i].StudentName;
                                string EmailId = VaccineAgeCriteriaDTO[i].EmailId;
                                string ASVACD_VaccineName = VaccineAgeCriteriaDTO[i].ASVACD_VaccineName;
                                string ASVACD_VaccineType = VaccineAgeCriteriaDTO[i].ASVACD_VaccineType;
                                string VaccinationDueDate = VaccineAgeCriteriaDTO[i].VaccinationDueDate;
                                if (template.FirstOrDefault().ISES_SMSActiveFlag == true)
                                {
                                    try
                                    {
                                        var d = await SendSms(MI_id, MobileNo, "Vaccine_Due_Date", AMST_Id, VaccinationDueDate, StudentName, ASVACD_VaccineName);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                if (template.FirstOrDefault().ISES_MailActiveFlag == true)
                                {
                                    try
                                    {
                                        string d =await SendEmail(MI_id, EmailId, "Vaccine_Due_Date", AMST_Id, VaccinationDueDate, StudentName, ASVACD_VaccineName);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
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
        public VaccineAgeCriteriaDTO OnLoadIllnessStudentDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Common Function
        public VaccineAgeCriteriaDTO AddUpdateVaccineAgeCriteriaDetails(VaccineAgeCriteriaDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.VaccineAgeCriteriaDetails != null && data.VaccineAgeCriteriaDetails.Length > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (VaccineAgeCriteriaDetails ph in data.VaccineAgeCriteriaDetails)
                    {
                        temparr.Add(ph.ASVACD_Id);
                    }

                    Array Phone_Noresultremove = _context.VaccineAgeCriteriaDetailsDMO.Where(t => !temparr.Contains(t.ASVACD_Id)
                    && t.ASVAC_Id == data.ASVAC_Id).ToArray();
                    foreach (VaccineAgeCriteriaDetailsDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    foreach (var d in data.VaccineAgeCriteriaDetails)
                    {
                        if (d.ASVACD_Id > 0)
                        {
                            var result = _context.VaccineAgeCriteriaDetailsDMO.Where(a => a.ASVACD_Id == d.ASVACD_Id && a.ASVAC_Id == data.ASVAC_Id).ToList();
                            if (result.Count > 0)
                            {
                                var result_update = _context.VaccineAgeCriteriaDetailsDMO.Single(a => a.ASVACD_Id == d.ASVACD_Id);
                                result_update.ASVACD_VaccineName = d.ASVACD_VaccineName;
                                result_update.ASVACD_VaccineType = d.ASVACD_VaccineType;
                                result_update.ASVACD_UpdatedDate = indiantime0;
                                _context.Update(result_update);
                            }
                        }
                        else
                        {
                            VaccineAgeCriteriaDetailsDMO vaccineAgeCriteriaDetailsDMO = new VaccineAgeCriteriaDetailsDMO();
                            vaccineAgeCriteriaDetailsDMO.ASVAC_Id = data.ASVAC_Id;
                            vaccineAgeCriteriaDetailsDMO.ASVACD_VaccineName = d.ASVACD_VaccineName;
                            vaccineAgeCriteriaDetailsDMO.ASVACD_VaccineType = d.ASVACD_VaccineType;
                            vaccineAgeCriteriaDetailsDMO.ASVACD_UpdatedDate = indiantime0;
                            vaccineAgeCriteriaDetailsDMO.ASVACD_CreatedDate = indiantime0;
                            vaccineAgeCriteriaDetailsDMO.ASVACD_ActiveFlag = true;
                            _context.Add(vaccineAgeCriteriaDetailsDMO);
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

        public async Task<string> SendSms(long MI_Id, long mobileNo, string Template, long AMST_Id, string VaccinationDueDate, string StudentName, 
            string ASVACD_VaccineName)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    sms = result;
                }
                else
                {
                    result = result.Replace("[NAME]", StudentName);
                    result = result.Replace("[VACCINENAME]", ASVACD_VaccineName);
                    result = result.Replace("[DATE]", VaccinationDueDate);
                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _context.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _context.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.NVarChar) { Value = PHNO });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar) { Value = sms });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar) { Value = modulename[0] });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = "Delivered" });
                        cmd.Parameters.Add(new SqlParameter("@Message_id", SqlDbType.VarChar) { Value = messageid });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public async Task<string> SendEmail(long MI_Id, string Email, string Template, long AMST_Id, string VaccinationDueDate, string StudentName,
          string ASVACD_VaccineName)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;

                string result = Mailmsg;                

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    result = result.Replace("[NAME]", StudentName);
                    result = result.Replace("[VACCINENAME]", ASVACD_VaccineName);
                    result = result.Replace("[DATE]", VaccinationDueDate);
                    Mailmsg = result;
                    Mailcontent = result;
                }
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();

                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }
                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }                         

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);

                        if (Attechement.Equals("1"))
                        {
                            var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

                        message.HtmlContent = Mailmsg;
                        
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }

                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;

                                if (Attechement.Equals("1"))
                                {
                                    var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }                                          
                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",SqlDbType.NVarChar){Value = Email});
                        cmd.Parameters.Add(new SqlParameter("@Message",SqlDbType.NVarChar){Value = Mailcontent});
                        cmd.Parameters.Add(new SqlParameter("@module",SqlDbType.VarChar){Value = modulename[0]});
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = MI_Id});

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}