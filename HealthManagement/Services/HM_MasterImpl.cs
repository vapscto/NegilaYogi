using DataAccessMsSqlServerProvider.HealthManagement;
using DomainModel.Model.HealthManagement;
using PreadmissionDTOs.HealthManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthManagement.Services
{
    public class HM_MasterImpl : Interfaces.HM_MasterInterfaces
    {
        HealthManagenentMasterContext _context;
        public HM_MasterImpl(HealthManagenentMasterContext con)
        {
            _context = con;
        }

        // Master Behaviour
        public Master_HealthManagementDTO load_MB(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.behaviourlist = _context.HM_M_BehaviourDMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Save_MB(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMBEH_Id > 0)
                {
                    var mb = _context.HM_M_BehaviourDMO_con.Single(a => a.HMMBEH_Id == dto.HMMBEH_Id && a.MI_Id == dto.MI_Id);
                    mb.HMMBEH_BehaviourName = dto.HMMBEH_BehaviourName;
                    mb.HMMBEH_BehaviourDesc = dto.HMMBEH_BehaviourDesc;
                    mb.HMMBEH_UpdatedBy = dto.UserId;
                    mb.HMMBEH_UpdatedDate = DateTime.Today;
                    _context.Update(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    HM_M_BehaviourDMO mb = new HM_M_BehaviourDMO();
                    mb.MI_Id = dto.MI_Id;
                    mb.HMMBEH_BehaviourName = dto.HMMBEH_BehaviourName;
                    mb.HMMBEH_BehaviourDesc = dto.HMMBEH_BehaviourDesc;
                    mb.HMMBEH_ActiveFlg = true;
                    mb.HMMBEH_CreatedBy = dto.UserId;
                    mb.HMMBEH_CreatedDate = DateTime.Today;
                    _context.Add(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Edit_MB(Master_HealthManagementDTO dto)
        {
            try
            {
                var mb = _context.HM_M_BehaviourDMO_con.Where(a => a.HMMBEH_Id == dto.HMMBEH_Id && a.MI_Id == dto.MI_Id).ToArray();
                if (mb.Length > 0)
                {
                    dto.behaviour_edit = mb;
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO ActiveDeactive_MB(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMBEH_ActiveFlg == true)
                {
                    var result = _context.HM_M_BehaviourDMO_con.Single(a => a.HMMBEH_Id == dto.HMMBEH_Id && a.MI_Id == dto.MI_Id);
                    result.HMMBEH_ActiveFlg = false;
                    result.HMMBEH_UpdatedBy = dto.UserId;
                    result.HMMBEH_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {

                    var result = _context.HM_M_BehaviourDMO_con.Single(a => a.HMMBEH_Id == dto.HMMBEH_Id && a.MI_Id == dto.MI_Id);
                    result.HMMBEH_ActiveFlg = true;
                    result.HMMBEH_UpdatedBy = dto.UserId;
                    result.HMMBEH_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        // Master Cleanness
        public Master_HealthManagementDTO load_CL(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.clennesslist = _context.HM_M_CleannessDMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Save_CL(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMCLN_Id > 0)
                {
                    var mb = _context.HM_M_CleannessDMO_con.Single(a => a.HMMCLN_Id == dto.HMMCLN_Id && a.MI_Id == dto.MI_Id);
                    mb.HMMCLN_CleannessName = dto.HMMCLN_CleannessName;
                    mb.HMMCLN_CleannessDesc = dto.HMMCLN_CleannessDesc;
                    mb.HMMCLN_UpdatedBy = dto.UserId;
                    mb.HMMCLN_UpdatedDate = DateTime.Today;
                    _context.Update(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    HM_M_CleannessDMO mb = new HM_M_CleannessDMO();
                    mb.MI_Id = dto.MI_Id;
                    mb.HMMCLN_CleannessName = dto.HMMCLN_CleannessName;
                    mb.HMMCLN_CleannessDesc = dto.HMMCLN_CleannessDesc;
                    mb.HMMCLN_ActiveFlg = true;
                    mb.HMMCLN_CreatedBy = dto.UserId;
                    mb.HMMCLN_CreatedDate = DateTime.Today;
                    _context.Add(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Edit_CL(Master_HealthManagementDTO dto)
        {
            try
            {
                var mb = _context.HM_M_CleannessDMO_con.Where(a => a.HMMCLN_Id == dto.HMMCLN_Id && a.MI_Id == dto.MI_Id).ToArray();
                if (mb.Length > 0)
                {
                    dto.clenness_edit = mb;
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO ActiveDeactive_CL(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMCLN_ActiveFlg == true)
                {
                    var result = _context.HM_M_CleannessDMO_con.Single(a => a.HMMCLN_Id == dto.HMMCLN_Id && a.MI_Id == dto.MI_Id);
                    result.HMMCLN_ActiveFlg = false;
                    result.HMMCLN_UpdatedBy = dto.UserId;
                    result.HMMCLN_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {

                    var result = _context.HM_M_CleannessDMO_con.Single(a => a.HMMCLN_Id == dto.HMMCLN_Id && a.MI_Id == dto.MI_Id);
                    result.HMMCLN_ActiveFlg = true;
                    result.HMMCLN_UpdatedBy = dto.UserId;
                    result.HMMCLN_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        // Master Doctor
        public Master_HealthManagementDTO load_DC(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.doctorlist = _context.HM_M_DoctorDMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Save_DC(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMDOC_Id > 0)
                {
                    var mb = _context.HM_M_DoctorDMO_con.Single(a => a.HMMDOC_Id == dto.HMMDOC_Id && a.MI_Id == dto.MI_Id);
                    mb.HMMDOC_DoctorName = dto.HMMDOC_DoctorName;
                    mb.HMMDOC_DoctorQualification = dto.HMMDOC_DoctorQualification;
                    mb.HMMDOC_Specialisation = dto.HMMDOC_Specialisation;
                    mb.HMMDOC_Address = dto.HMMDOC_Address;
                    mb.HMMDOC_Phoneno = dto.HMMDOC_Phoneno;
                    mb.HMMDOC_EmailId = dto.HMMDOC_EmailId;
                    mb.HMMDOC_BloodGroup = dto.HMMDOC_BloodGroup;
                    mb.HMMDOC_UpdatedBy = dto.UserId;
                    mb.HMMDOC_UpdatedDate = DateTime.Today;
                    _context.Update(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    HM_M_DoctorDMO mb = new HM_M_DoctorDMO();
                    mb.MI_Id = dto.MI_Id;
                    mb.HMMDOC_DoctorName = dto.HMMDOC_DoctorName;
                    mb.HMMDOC_DoctorQualification = dto.HMMDOC_DoctorQualification;
                    mb.HMMDOC_Specialisation = dto.HMMDOC_Specialisation;
                    mb.HMMDOC_Address = dto.HMMDOC_Address;
                    mb.HMMDOC_Phoneno = dto.HMMDOC_Phoneno;
                    mb.HMMDOC_EmailId = dto.HMMDOC_EmailId;
                    mb.HMMDOC_ActiveFlg = true;
                    mb.HMMDOC_BloodGroup = dto.HMMDOC_BloodGroup;
                    mb.HMMDOC_CreatedBy = dto.UserId;
                    mb.HMMDOC_CreatedDate = DateTime.Today;
                    _context.Add(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Edit_DC(Master_HealthManagementDTO dto)
        {
            try
            {
                var mb = _context.HM_M_DoctorDMO_con.Where(a => a.HMMDOC_Id == dto.HMMDOC_Id && a.MI_Id == dto.MI_Id).ToArray();
                if (mb.Length > 0)
                {
                    dto.doctor_edit = mb;
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO ActiveDeactive_DC(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMDOC_ActiveFlg == true)
                {
                    var result = _context.HM_M_DoctorDMO_con.Single(a => a.HMMDOC_Id == dto.HMMDOC_Id && a.MI_Id == dto.MI_Id);
                    result.HMMDOC_ActiveFlg = false;
                    result.HMMDOC_UpdatedBy = dto.UserId;
                    result.HMMDOC_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {

                    var result = _context.HM_M_DoctorDMO_con.Single(a => a.HMMDOC_Id == dto.HMMDOC_Id && a.MI_Id == dto.MI_Id);
                    result.HMMDOC_ActiveFlg = true;
                    result.HMMDOC_UpdatedBy = dto.UserId;
                    result.HMMDOC_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        // Master Examination
        public Master_HealthManagementDTO load_EX(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.examinationlist = _context.HM_M_ExaminationDMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Save_EX(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMEXM_Id > 0)
                {
                    var mb = _context.HM_M_ExaminationDMO_con.Single(a => a.HMMEXM_Id == dto.HMMEXM_Id && a.MI_Id == dto.MI_Id);
                    mb.HMMEXM_ExaminationName = dto.HMMEXM_ExaminationName;
                    mb.HMMEXM_ExamDesc = dto.HMMEXM_ExamDesc;
                    mb.HMMEXM_UpdatedBy = dto.UserId;
                    mb.HMMEXM_UpdatedDate = DateTime.Today;
                    _context.Update(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    HM_M_ExaminationDMO mb = new HM_M_ExaminationDMO();
                    mb.MI_Id = dto.MI_Id;
                    mb.HMMEXM_ExaminationName = dto.HMMEXM_ExaminationName;
                    mb.HMMEXM_ExamDesc = dto.HMMEXM_ExamDesc;
                    mb.HMMEXM_ActiveFlg = true;
                    mb.HMMEXM_CreatedBy = dto.UserId;
                    mb.HMMEXM_CreatedDate = DateTime.Today;
                    _context.Add(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Edit_EX(Master_HealthManagementDTO dto)
        {
            try
            {
                var mb = _context.HM_M_ExaminationDMO_con.Where(a => a.HMMEXM_Id == dto.HMMEXM_Id && a.MI_Id == dto.MI_Id).ToArray();
                if (mb.Length > 0)
                {
                    dto.examination_edit = mb;
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO ActiveDeactive_EX(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMEXM_ActiveFlg == true)
                {
                    var result = _context.HM_M_ExaminationDMO_con.Single(a => a.HMMEXM_Id == dto.HMMEXM_Id && a.MI_Id == dto.MI_Id);
                    result.HMMEXM_ActiveFlg = false;
                    result.HMMEXM_UpdatedBy = dto.UserId;
                    result.HMMEXM_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {

                    var result = _context.HM_M_ExaminationDMO_con.Single(a => a.HMMEXM_Id == dto.HMMEXM_Id && a.MI_Id == dto.MI_Id);
                    result.HMMEXM_ActiveFlg = true;
                    result.HMMEXM_UpdatedBy = dto.UserId;
                    result.HMMEXM_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        // Master Observation
        public Master_HealthManagementDTO load_OB(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.observationlist = _context.HM_M_Observation_DMO_con.Where(a => a.MI_Id == dto.MI_Id).OrderByDescending(a => a.HMMOBS_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Save_OB(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMOBS_Id > 0)
                {
                    var mb = _context.HM_M_Observation_DMO_con.Single(a => a.HMMOBS_Id == dto.HMMOBS_Id && a.MI_Id == dto.MI_Id);
                    mb.HMMOBS_Observation = dto.HMMOBS_Observation;
                    mb.HMMOBS_ObservationDesc = dto.HMMOBS_ObservationDesc;
                    mb.HMMOBS_UpdatedBy = dto.UserId;
                    mb.HMMOBS_UpdatedDate = DateTime.Today;
                    _context.Update(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    HM_M_Observation_DMO mb = new HM_M_Observation_DMO();
                    mb.MI_Id = dto.MI_Id;
                    mb.HMMOBS_Observation = dto.HMMOBS_Observation;
                    mb.HMMOBS_ObservationDesc = dto.HMMOBS_ObservationDesc;
                    mb.HMMOBS_ActiveFlg = true;
                    mb.HMMOBS_CreatedBy = dto.UserId;
                    mb.HMMOBS_CreatedDate = DateTime.Today;
                    _context.Add(mb);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Edit_OB(Master_HealthManagementDTO dto)
        {
            try
            {
                var mb = _context.HM_M_Observation_DMO_con.Where(a => a.HMMOBS_Id == dto.HMMOBS_Id && a.MI_Id == dto.MI_Id).ToArray();
                if (mb.Length > 0)
                {
                    dto.observation_edit = mb;
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO ActiveDeactive_OB(Master_HealthManagementDTO dto)
        {
            try
            {
                if (dto.HMMOBS_ActiveFlg == true)
                {
                    var result = _context.HM_M_Observation_DMO_con.Single(a => a.HMMOBS_Id == dto.HMMOBS_Id && a.MI_Id == dto.MI_Id);
                    result.HMMOBS_ActiveFlg = false;
                    result.HMMOBS_UpdatedBy = dto.UserId;
                    result.HMMOBS_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {

                    var result = _context.HM_M_Observation_DMO_con.Single(a => a.HMMOBS_Id == dto.HMMOBS_Id && a.MI_Id == dto.MI_Id);
                    result.HMMOBS_ActiveFlg = true;
                    result.HMMOBS_UpdatedBy = dto.UserId;
                    result.HMMOBS_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        // Master Illness
        public Master_HealthManagementDTO Load_illness(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.GetIllnessLoadDataList = _context.HM_M_IllnessDMO.Where(a => a.MI_Id == dto.MI_Id).OrderByDescending(a => a.HMMILL_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Save_illness(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.Returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (dto.HMMILL_Id > 0)
                {
                    dto.message = "Update";
                    var check_duplicate = _context.HM_M_IllnessDMO.Where(a => a.MI_Id == dto.MI_Id && a.HMMILL_IllnessName.Equals(dto.HMMILL_IllnessName)
                    && a.HMMILL_Id != dto.HMMILL_Id).Count();

                    if (check_duplicate > 0)
                    {
                        dto.message = "Duplicate";
                    }
                    else
                    {
                        var check_result = _context.HM_M_IllnessDMO.Single(a => a.MI_Id == dto.MI_Id && a.HMMILL_Id == dto.HMMILL_Id);
                        check_result.HMMILL_IllnessName = dto.HMMILL_IllnessName;
                        check_result.HMMILL_IllnessDesc = dto.HMMILL_IllnessDesc;
                        check_result.HMMILL_UpdatedBy = dto.UserId;
                        check_result.HMMILL_UpdatedDate = indiantime0;
                        _context.Update(check_result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            dto.Returnval = true;
                        }
                    }
                }
                else
                {
                    dto.message = "Add";

                    var check_duplicate = _context.HM_M_IllnessDMO.Where(a => a.MI_Id == dto.MI_Id && a.HMMILL_IllnessName.Equals(dto.HMMILL_IllnessName)).Count();

                    if (check_duplicate > 0)
                    {
                        dto.message = "Duplicate";
                    }
                    else
                    {
                        HM_M_IllnessDMO hM_M_IllnessDMO = new HM_M_IllnessDMO();
                        hM_M_IllnessDMO.MI_Id = dto.MI_Id;
                        hM_M_IllnessDMO.HMMILL_IllnessName = dto.HMMILL_IllnessName;
                        hM_M_IllnessDMO.HMMILL_IllnessDesc = dto.HMMILL_IllnessDesc;
                        hM_M_IllnessDMO.HMMILL_ActiveFlg = true;
                        hM_M_IllnessDMO.HMMILL_CreatedDate = indiantime0;
                        hM_M_IllnessDMO.HMMILL_UpdatedDate = indiantime0;
                        hM_M_IllnessDMO.HMMILL_CreatedBy = dto.UserId;
                        hM_M_IllnessDMO.HMMILL_UpdatedBy = dto.UserId;
                        _context.Add(hM_M_IllnessDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            dto.Returnval = true;
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                dto.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO Edit_illness(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.EditIllnessDataList = _context.HM_M_IllnessDMO.Where(a => a.MI_Id == dto.MI_Id && a.HMMILL_Id == dto.HMMILL_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public Master_HealthManagementDTO ActiveDeactive_illness(Master_HealthManagementDTO dto)
        {
            try
            {
                dto.Returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _context.HM_M_IllnessDMO.Single(a => a.MI_Id == dto.MI_Id && a.HMMILL_Id == dto.HMMILL_Id);
                result.HMMILL_ActiveFlg = result.HMMILL_ActiveFlg == true ? false : true;
                result.HMMILL_UpdatedBy = dto.UserId;
                result.HMMILL_UpdatedDate = indiantime0;
                _context.Update(result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    dto.Returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

    }
}
