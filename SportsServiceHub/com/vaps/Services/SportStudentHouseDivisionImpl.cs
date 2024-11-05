using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportStudentHouseDivisionImpl : Interfaces.SportStudentHouseDivisionInterface
    {
        private static ConcurrentDictionary<string, SportMasterHouseDTO> _login =
    new ConcurrentDictionary<string, SportMasterHouseDTO>();

        private readonly SportsContext _sportcontext;
        private readonly castecategoryContext _castecategoryContext;
        public StudentAttendanceReportContext _db;
        private readonly ILogger<SportStudentHouseDivisionImpl> _log;


        public SportStudentHouseDivisionImpl(SportsContext sportcontext, ILogger<SportStudentHouseDivisionImpl> log, StudentAttendanceReportContext db)
        {
            _sportcontext = sportcontext;
            _log = log;
            _db = db;
        }

        public SportMasterHouseDTO GetmastercasteData(SportMasterHouseDTO data)//int IVRMM_Id
        {

            List<SportStudentHouseDivisionDMO> Allname = new List<SportStudentHouseDivisionDMO>();

            //try
            //{
            //    Allname = _sportcontext.SportStudentHouseDivisionDMO.ToList();
            //    data.masterhousename = Allname.ToArray();

            //    List<MasterAcademic> list = new List<MasterAcademic>();
            //    list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).OrderBy(t => t.ASMAY_Order).ToList();
            //    data.yearlist = list.ToArray();

            //    List<School_M_Class> classes = new List<School_M_Class>();
            //    classes = _db.admissionClass.Where(c => c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag == true).ToList();
            //    data.ClassList = classes.Distinct().ToArray();

            //    List<School_M_Section> sections = new List<School_M_Section>();
            //    sections = _sportcontext.masterSection.Where(c => c.MI_Id == data.MI_Id && c.ASMC_ActiveFlag == 1).ToList();
            //    data.SectionList = sections.Distinct().ToArray();


            //    data.DesignationList = (from a in _sportcontext.SportMasterDivisionDMO
            //                            where (a.MI_Id == data.MI_Id && a.SPCCMD_ActiveFlag == true)
            //                            select new SportMasterHouseDTO
            //                            {
            //                                SPCCMD_Id = a.SPCCMD_Id,
            //                                SPCCMD_DivisionName = a.SPCCMD_DivisionName,

            //                            }).Distinct().ToArray();
            //    data.HouseList = (from a in _sportcontext.SportMasterHouseDMO
            //                      where (a.MI_Id == data.MI_Id && a.SPCCMH_ActiveFlag == true)
            //                      select new SportMasterHouseDTO
            //                      {
            //                          SPCCMH_Id = a.SPCCMH_Id,
            //                          SPCCMH_HouseName = a.SPCCMH_HouseName,

            //                      }).Distinct().ToArray();

            //    data.GridviewDetails = (from a in _sportcontext.SportStudentHouseDivisionDMO
            //                            from b in _sportcontext.admissionStduent
            //                            from c in _sportcontext.SportMasterDivisionDMO
            //                            from d in _sportcontext.SportMasterHouseDMO
            //                            where (a.AMST_Id == b.AMST_Id && a.SPCCMD_Id == c.SPCCMD_Id && a.SPCCMH_Id == d.SPCCMH_Id && a.MI_Id == data.MI_Id)
            //                            select new SportMasterHouseDTO
            //                            {
            //                                SPCCMD_Id = c.SPCCMD_Id,
            //                                SPCCMH_Id = d.SPCCMH_Id,
            //                                SPCCSHD_Id = a.SPCCSHD_Id,
            //                                SPCCSHD_Height = a.SPCCSHD_Height,
            //                                SPCCSHD_Weight = a.SPCCSHD_Weight,
            //                                SPCCSHD_Age = a.SPCCSHD_Age,
            //                                SPCCMD_DivisionName = c.SPCCMD_DivisionName,
            //                                SPCCMH_HouseName = d.SPCCMH_HouseName,
            //                                studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
            //                                SPCCMHD_ActiveFlag = a.SPCCMHD_ActiveFlag,
            //                                SPCCMHD_BMI=a.SPCCMHD_BMI,
            //                                SPCCMHD_BMI_Remark=a.SPCCMHD_BMI_Remark,
            //                            }).Distinct().ToArray();

            //    if (data.GridviewDetails.Length > 0)
            //    {
            //        data.count = data.GridviewDetails.Length;
            //    }
            //    else
            //    {
            //        data.count = 0;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _log.LogInformation("Master Caste form error");
            //    _log.LogDebug(ex.Message);
            //}
            return data;
        }

        public SportMasterHouseDTO get_section(SportMasterHouseDTO dto)
        {
            //try
            //{
            //    dto.SectionList = (from a in _db.admissionyearstudent
            //                       from b in _db.masterSection
            //                       where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
            //                       select b).Distinct().ToArray();
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return dto;

        }
        public SportMasterHouseDTO get_student(SportMasterHouseDTO dto)
        {
            try
            {
                //var sec = dto.Section_List.Split(',');
                //List<long> list = new List<long>();
                //for (int i = 0; i < sec.Length; i++)
                //{
                //    list.Add(Convert.ToInt64(sec[i]));
                //}
                //for (int i = 0; i < dto.Section_List.Count(); i++)
                //{
                dto.StudentList = (from a in _sportcontext.admissionyearstudent
                                   from b in _sportcontext.admissionStduent
                                   where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                   select new SportStudentParticipationReportDTO.Ostudent
                                   {
                                       AMST_Id = b.AMST_Id,
                                       studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),

                                   }
                               ).Distinct().OrderBy(b => b.studentname).ToArray();
                //   }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public SportMasterHouseDTO GetSelectedRowDetails(SportMasterHouseDTO data)
        {
            //try
            //{
               
            //    var editData = _sportcontext.SportStudentHouseDivisionDMO.Where(d => d.SPCCSHD_Id == data.SPCCSHD_Id).ToList();
            //    data.GridviewDetails = editData.ToArray();

            //    data.StudentList = _sportcontext.Adm_M_Student.Where(d => d.AMST_Id == editData.FirstOrDefault().AMST_Id).Select(d => new SportMasterHouseDTO { AMST_Id = d.AMST_Id, studentname = d.AMST_FirstName + (string.IsNullOrEmpty(d.AMST_MiddleName) ? "" : ' ' + d.AMST_MiddleName) + (string.IsNullOrEmpty(d.AMST_LastName) ? "" : ' ' + d.AMST_LastName) }).ToArray();

            //}
            //catch (Exception ex)
            //{
            //    _log.LogInformation("Master Caste form error");
            //    _log.LogDebug(ex.Message);
            //}

            return data;
        }

        public SportMasterHouseDTO deactivate(SportMasterHouseDTO dto)
        {
            //try
            //{
            //    SportMasterHouseDTO enq = Mapper.Map<SportMasterHouseDTO>(dto);

            //    if (enq.SPCCSHD_Id > 0)
            //    {
            //        var check_religinassign = (from a in _sportcontext.SportStudentHouseDivisionDMO
            //                                   where (a.SPCCSHD_Id == dto.SPCCSHD_Id)
            //                                   select new SportMasterHouseDTO
            //                                   {
            //                                       SPCCSHD_Id = a.SPCCSHD_Id
            //                                   }
            //                                ).ToList();

            //        //if (check_religinassign.Count > 0)
            //        //{
            //        //    dto.returnVal = true;

            //        //    dto.msg = "You Can Not Deactivate This Record It Is Already Mapped With Student";
            //        //}
            //        //else
            //        //{
            //        var result = _sportcontext.SportStudentHouseDivisionDMO.Single(t => t.SPCCSHD_Id == enq.SPCCSHD_Id);
            //        if (result.SPCCMHD_ActiveFlag == true)
            //        {
            //            result.SPCCMHD_ActiveFlag = false;
            //        }
            //        else
            //        {
            //            result.SPCCMHD_ActiveFlag = true;
            //        }
            //        result.CreatedDate = result.CreatedDate;
            //        result.UpdatedDate = DateTime.Now;
            //        _sportcontext.Update(result);
            //        var flag = _sportcontext.SaveChanges();
            //        if (flag == 1)
            //        {
            //            dto.returnVal = true;

            //            if (result.SPCCMHD_ActiveFlag == true)
            //            {
            //                dto.msg = "House Division Activated Successfully.";
            //            }
            //            else if (result.SPCCMHD_ActiveFlag == false)
            //            {
            //                dto.msg = "House Division Deactivated Successfully.";
            //            }
            //        }
            //        else
            //        {
            //            dto.returnVal = false;
            //        }
            //        List<SportStudentHouseDivisionDMO> Allname = new List<SportStudentHouseDivisionDMO>();
            //        Allname = _sportcontext.SportStudentHouseDivisionDMO.ToList();
            //        dto.masterhousename = Allname.ToArray();
            //    }
            //    //  }
            //}
            //catch (Exception ee)
            //{
            //    _log.LogTrace(ee.Message);
            //    _log.LogDebug(ee.Message);
            //    _log.LogError(ee.Message);
            //}
            return dto;
        }

        public SportMasterHouseDTO mastercasteData(SportMasterHouseDTO obj)
        {
            //try
            //{
            //    SportStudentHouseDivisionDMO objj = Mapper.Map<SportStudentHouseDivisionDMO>(obj);
            //    if (obj.SPCCSHD_Id == 0)
            //    {
            //        for (int i = 0; i < obj.StudList.Count(); i++)
            //        {
            //            var std = obj.StudList[i].AMST_Id;
            //            var duplicate = _sportcontext.SportStudentHouseDivisionDMO.Where(t => t.MI_Id == obj.MI_Id && t.ASMAY_Id == obj.ASMAY_Id && t.ASMCL_Id == obj.ASMCL_Id && t.ASMS_Id == obj.ASMS_Id && t.AMST_Id == std && t.SPCCSHD_Age == obj.SPCCSHD_Age && t.SPCCSHD_Height == obj.SPCCSHD_Height && t.SPCCSHD_Weight == obj.SPCCSHD_Weight).ToList();

            //            if (duplicate.Count > 0)
            //            {
            //                obj.msg = "duplicate";
            //            }

            //            else
            //            {
            //                var mapp = Mapper.Map<SportStudentHouseDivisionDMO>(obj);
            //                mapp.AMST_Id = obj.StudList[i].AMST_Id;
            //                mapp.SPCCMHD_BMI = obj.SPCCMHD_BMI;
            //                mapp.SPCCMHD_BMI_Remark = obj.SPCCMHD_BMI_Remark;
            //                mapp.SPCCMHD_ActiveFlag = true;
            //                mapp.CreatedDate = DateTime.Now;
            //                mapp.UpdatedDate = DateTime.Now;
            //                _sportcontext.Add(mapp);
            //                int s = _sportcontext.SaveChanges();
            //                if (s > 0)
            //                {
            //                    obj.msg = "saved";
            //                }
            //                else
            //                {
            //                    obj.msg = "savingFailed";
            //                }
            //            }
            //        }

            //    }
            //    else if (obj.SPCCSHD_Id > 0)
            //    {
            //        for (int i = 0; i < obj.StudList.Count(); i++)
            //        {
            //            var std = obj.StudList[i].AMST_Id;
            //            var duplicate = _sportcontext.SportStudentHouseDivisionDMO.Where(d => d.MI_Id == obj.MI_Id && d.ASMAY_Id == obj.ASMAY_Id && d.ASMCL_Id == obj.ASMCL_Id && d.ASMS_Id == obj.ASMS_Id && d.AMST_Id == std && d.SPCCSHD_Age == obj.SPCCSHD_Age && d.SPCCSHD_Height == obj.SPCCSHD_Height && d.SPCCSHD_Weight == obj.SPCCSHD_Weight && d.SPCCSHD_Id != obj.SPCCSHD_Id).ToList();

            //            if (duplicate.Count > 0)
            //            {
            //                obj.msg = "duplicate";
            //            }
            //            else
            //            {
            //                var query = _sportcontext.SportStudentHouseDivisionDMO.Where(d => d.SPCCSHD_Id == obj.SPCCSHD_Id).ToList();
            //                if (query.Count > 0)
            //                {
            //                    var result = _sportcontext.SportStudentHouseDivisionDMO.Single(d => d.SPCCSHD_Id == obj.SPCCSHD_Id);

            //                    result.SPCCSHD_Id = obj.SPCCSHD_Id;
            //                    result.SPCCMD_Id = obj.SPCCMD_Id;
            //                    result.SPCCMH_Id = obj.SPCCMH_Id;
            //                    result.AMST_Id = obj.StudList[i].AMST_Id;
            //                    result.ASMS_Id = obj.ASMS_Id;
            //                    result.ASMCL_Id = obj.ASMCL_Id;
            //                    result.ASMAY_Id = obj.ASMAY_Id;
            //                    result.SPCCSHD_Age = obj.SPCCSHD_Age;
            //                    result.SPCCSHD_Height = obj.SPCCSHD_Height;
            //                    result.SPCCSHD_Weight = obj.SPCCSHD_Weight;

            //                    result.SPCCMHD_BMI = obj.SPCCMHD_BMI;
            //                    result.SPCCMHD_BMI_Remark = obj.SPCCMHD_BMI_Remark;

            //                    result.UpdatedDate = DateTime.Now;

            //                    _sportcontext.Update(result);
            //                    int s = _sportcontext.SaveChanges();
            //                    if (s > 0)
            //                    {
            //                        obj.msg = "updated";
            //                    }
            //                    else
            //                    {
            //                        obj.msg = "updateFailed";
            //                    }
            //                }
            //            }
            //        }
            //    }             
            //}
            //catch (Exception ex)
            //{
            //    _log.LogInformation("Master House Designation form error");
            //    _log.LogDebug(ex.Message);
            //}
            return obj;
        }
    }
}
