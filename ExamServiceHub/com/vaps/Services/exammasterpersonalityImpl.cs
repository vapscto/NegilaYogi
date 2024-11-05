
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class exammasterpersonalityImpl : Interfaces.exammasterPersonalityInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _examContext;

        public exammasterpersonalityImpl(ExamContext examContext)
        {
            _examContext = examContext;
        }

        // Master Personlaity
        public exammasterpersonalityDTO Getdetails(exammasterpersonalityDTO data)//int IVRMM_Id
        {
            exammasterpersonalityDTO getdata = new exammasterpersonalityDTO();
            try
            {
                List<Exm_PersonalityDMO> list = new List<Exm_PersonalityDMO>();
                list = _examContext.Exm_PersonalityDMO.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t => t.EP_PersonlaityOrder).ToList();
                getdata.exammasterpersonalityname = list.ToArray();

                getdata.personalityorder = _examContext.Exm_PersonalityDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.EP_PersonlaityOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;

        }

        //Onchange 
        //Added by Ramesh
        public exammasterpersonalityDTO validateordernumber(exammasterpersonalityDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.examPersonlityDTO.Count() > 0)
                {
                    foreach (exammasterpersonalityDTO mob in dto.examPersonlityDTO)
                    {
                        if (mob.EP_Id > 0)
                        {
                            var result = _examContext.Exm_PersonalityDMO.Single(t => t.EP_Id.Equals(mob.EP_Id));
                            result.EP_PersonlaityOrder = mob.EP_PersonlaityOrder;
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                        }
                    }
                    var i = _examContext.SaveChanges();
                    if (i > 0)
                    {
                        dto.retrunMsg = "Order Updated Successfully";
                    }
                    else
                    {
                        dto.retrunMsg = "Order Updated Failed";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Order Updated Failed";
            }
            return dto;
        }

        public exammasterpersonalityDTO savedetails(exammasterpersonalityDTO data)
        {
            try
            {
                if (data.EP_Id != 0)
                {
                    var res = _examContext.Exm_PersonalityDMO.Where(t => t.MI_Id == data.MI_Id && (t.EP_PersonlaityName == data.EP_PersonlaityName || t.EP_PersonlaityCode == data.EP_PersonlaityCode) && t.EP_Id != data.EP_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _examContext.Exm_PersonalityDMO.Single(t => t.MI_Id == data.MI_Id && t.EP_Id == data.EP_Id);
                        result.EP_PersonlaityName = data.EP_PersonlaityName;
                        result.EP_PersonlaityCode = data.EP_PersonlaityCode;
                        result.UpdatedDate = DateTime.Now;
                        _examContext.Update(result);
                        var contactExists = _examContext.SaveChanges();
                        if (contactExists == 1)
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
                    var res = _examContext.Exm_PersonalityDMO.Where(t => t.MI_Id == data.MI_Id && (t.EP_PersonlaityName == data.EP_PersonlaityName || t.EP_PersonlaityCode == data.EP_PersonlaityCode)).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _examContext.Exm_PersonalityDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                        Exm_PersonalityDMO exm = new Exm_PersonalityDMO();
                        exm.EP_PersonlaityName = data.EP_PersonlaityName;
                        exm.EP_PersonlaityCode = data.EP_PersonlaityCode;
                        exm.EP_PersonlaityOrder = row_cnt + 1;
                        exm.MI_Id = data.MI_Id;
                        exm.EP_ActiveFlag = true;
                        exm.CreatedDate = DateTime.Now;
                        exm.UpdatedDate = DateTime.Now;
                        _examContext.Add(exm);
                        var contactExists = _examContext.SaveChanges();
                        if (contactExists == 1)
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
                data.retrunMsg = "Error occured";
            }

            return data;
        }

        public exammasterpersonalityDTO editdetails(int ID)
        {
            exammasterpersonalityDTO editlt = new exammasterpersonalityDTO();
            try
            {
                List<Exm_PersonalityDMO> list = new List<Exm_PersonalityDMO>();
                list = _examContext.Exm_PersonalityDMO.Where(t => t.EP_Id == ID).ToList();
                editlt.exammpersonalityname = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }

        public exammasterpersonalityDTO deactivate(exammasterpersonalityDTO data)
        {
            try
            {
                data.already_cnt = false;
                var checkusedoronot = _examContext.Exm_Student_PersonalityDMO.Where(a => a.MI_Id == data.MI_Id && a.EP_Id == data.EP_Id && a.ESPM_ActiveFlag == true).ToList();

                if (checkusedoronot.Count() == 0)
                {
                    if (data.EP_Id > 0)
                    {
                        var result = _examContext.Exm_PersonalityDMO.Single(t => t.EP_Id == data.EP_Id);
                        if (result.EP_ActiveFlag == true)
                        {
                            result.EP_ActiveFlag = false;
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                        }
                        else
                        {
                            result.EP_ActiveFlag = true;
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                        }

                        var flag = _examContext.SaveChanges();
                        if (flag == 1)
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
                    data.already_cnt = true;
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }

        // Student Personlaiy Mapping 

        public exammasterpersonalityDTO studentdataload(exammasterpersonalityDTO data)//int IVRMM_Id
        {
            try
            {
                var getuserid = _examContext.ApplUser.Where(a => a.UserName.Equals(data.UserName.Trim())).Select(a => a.Id);
                List<long> classid = new List<long>();
                List<long> sectionid = new List<long>();

                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.examlist = _examContext.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderByDescending(a => a.EME_ExamOrder).ToArray();
                data.remarkslist = _examContext.exammasterRemarkDMO.Where(a => a.MI_Id == data.MI_Id && a.EPCR_ActiveFlag == true).OrderBy(a => a.EPCR_RemarksOrder).ToArray();

                var check_rolename = (from a in _examContext.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleid)
                                      select new exammasterCoCurricularDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                ).ToList();

                var empcode_check = (from a in _examContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(getuserid))
                                     select new exammasterCoCurricularDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _examContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {
                        for (int i = 0; i < check_classteacher.Count; i++)
                        {
                            classid.Add(check_classteacher[i].ASMCL_Id);
                            sectionid.Add(check_classteacher[i].ASMS_Id);

                            data.loaddata = (from a in _examContext.Exm_Student_PersonalityDMO
                                             from b in _examContext.Exm_PersonalityDMO
                                             from c in _examContext.exammasterDMO
                                             from d in _examContext.AcademicYear
                                             from e in _examContext.AdmissionClass
                                             from f in _examContext.School_M_Section
                                             where (a.EP_Id == b.EP_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id
                                             && a.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && classid.Contains(a.ASMCL_Id) && sectionid.Contains(a.ASMS_Id))
                                             select new exammasterpersonalityDTO
                                             {
                                                 EP_PersonlaityName = b.EP_PersonlaityName,
                                                 EP_PersonlaityCode = b.EP_PersonlaityCode,
                                                 monthname = c.EME_ExamName,
                                                 ASMCL_ClassName = e.ASMCL_ClassName,
                                                 ASMC_SectionName = f.ASMC_SectionName,
                                                 ASMAY_Year = d.ASMAY_Year,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                                 EP_Id = a.EP_Id,
                                                 EME_Id = a.EME_Id
                                             }).Distinct().OrderBy(a => a.EP_PersonlaityCode).ToArray();
                        }
                    }

                }
                else
                {
                    data.loaddata = (from a in _examContext.Exm_Student_PersonalityDMO
                                     from b in _examContext.Exm_PersonalityDMO
                                     from c in _examContext.exammasterDMO
                                     from d in _examContext.AcademicYear
                                     from e in _examContext.AdmissionClass
                                     from f in _examContext.School_M_Section
                                     where (a.EP_Id == b.EP_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id 
                                     && a.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id)
                                     select new exammasterpersonalityDTO
                                     {
                                         EP_PersonlaityName = b.EP_PersonlaityName,
                                         EP_PersonlaityCode = b.EP_PersonlaityCode,
                                         monthname = c.EME_ExamName,
                                         ASMCL_ClassName = e.ASMCL_ClassName,
                                         ASMC_SectionName = f.ASMC_SectionName,
                                         ASMAY_Year = d.ASMAY_Year,
                                         ASMCL_Id = a.ASMCL_Id,
                                         ASMS_Id = a.ASMS_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         EP_Id = a.EP_Id,
                                         EME_Id = a.EME_Id
                                     }).Distinct().OrderBy(a => a.EP_PersonlaityCode).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public exammasterpersonalityDTO onchangeyear(exammasterpersonalityDTO data)//int IVRMM_Id
        {
            try
            {
                var getuserid = _examContext.ApplUser.Where(a => a.UserName.Equals(data.UserName.Trim())).Select(a => a.Id);

                var check_rolename = (from a in _examContext.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleid)
                                      select new exammasterpersonalityDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                 ).ToList();

                var empcode_check = (from a in _examContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(getuserid))
                                     select new exammasterpersonalityDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _examContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {
                        data.classlist = (from a in _examContext.ClassTeacherMappingDMO
                                          from b in _examContext.AdmissionClass
                                          from c in _examContext.AcademicYear
                                          where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                          && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                          select new exammasterpersonalityDTO
                                          {
                                              ASMCL_Id = b.ASMCL_Id,
                                              ASMCL_ClassName = b.ASMCL_ClassName,
                                              ASMCL_ClassOrder = b.ASMCL_Order
                                          }).Distinct().OrderBy(a => a.ASMCL_ClassOrder).ToArray();
                    }
                }
                else
                {
                    data.classlist = _examContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public exammasterpersonalityDTO onchangeclass(exammasterpersonalityDTO data)//int IVRMM_Id
        {
            try
            {
                var getuserid = _examContext.ApplUser.Where(a => a.UserName.Equals(data.UserName.Trim())).Select(a => a.Id);

                var check_rolename = (from a in _examContext.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleid)
                                      select new exammasterpersonalityDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                 ).ToList();

                var empcode_check = (from a in _examContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(getuserid))
                                     select new exammasterpersonalityDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _examContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {
                        data.sectionlist = (from a in _examContext.ClassTeacherMappingDMO
                                            from b in _examContext.AdmissionClass
                                            from d in _examContext.School_M_Section
                                            from c in _examContext.AcademicYear
                                            where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                            && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                            select new exammasterpersonalityDTO
                                            {
                                                ASMS_Id = d.ASMS_Id,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                ASMC_Order = d.ASMC_Order
                                            }).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                    }
                }
                else
                {
                    data.sectionlist = _examContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public exammasterpersonalityDTO onchangesection(exammasterpersonalityDTO data)//int IVRMM_Id
        {
            try
            {
                data.monthlist = _examContext.IVRM_Month_DMO.ToArray();

                data.personlaitylist = _examContext.Exm_PersonalityDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.EP_PersonlaityOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterpersonalityDTO searchdata(exammasterpersonalityDTO data)//int IVRMM_Id
        {
            try
            {
                string order = "";
                var get_configuration = _examContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "studentname";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "studentname";
                }

                List<exammasterpersonalityDTO> studentList = new List<exammasterpersonalityDTO>();

                studentList = (from a in _examContext.School_Adm_Y_StudentDMO
                               from b in _examContext.Adm_M_Student
                               from c in _examContext.AcademicYear
                               from d in _examContext.AdmissionClass
                               from e in _examContext.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                               && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                               select new exammasterpersonalityDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                   AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                   AMST_RegistrationNo = b.AMST_RegistrationNo,
                                   AMAY_RollNo = a.AMAY_RollNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(exammasterpersonalityDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studentList = studentList.ToArray();


                List<exammasterpersonalityDTO> savedstudentList = new List<exammasterpersonalityDTO>();

                savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                    from b in _examContext.Adm_M_Student
                                    from c in _examContext.AcademicYear
                                    from d in _examContext.AdmissionClass
                                    from e in _examContext.School_M_Section
                                    from f in _examContext.Exm_Student_PersonalityDMO
                                    from g in _examContext.Exm_PersonalityDMO
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                    && f.EP_Id == g.EP_Id && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id
                                    && f.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                    && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                    && f.EME_Id == data.EME_Id && f.EP_Id == data.EP_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S"
                                    && b.AMST_ActiveFlag == 1 && f.ESPM_ActiveFlag == true)
                                    select new exammasterpersonalityDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                        AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                        AMST_RegistrationNo = b.AMST_RegistrationNo,
                                        AMAY_RollNo = a.AMAY_RollNo,
                                        EP_PersonlaityName = g.EP_PersonlaityName,
                                        ESPM_Remarks = f.ESPM_Remarks
                                    }).Distinct().ToList();

                var propertyInfo1 = typeof(exammasterpersonalityDTO).GetProperty(order);
                savedstudentList = savedstudentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();
                data.savedata = savedstudentList.ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterpersonalityDTO savemapping(exammasterpersonalityDTO data)
        {
            try
            {
                data.returnval = false;

                if (data.Temp_studentdata.Length > 0)
                {
                    for (int i = 0; i < data.Temp_studentdata.Length; i++)
                    {
                        var check_stdid = _examContext.Exm_Student_PersonalityDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.EP_Id == data.EP_Id && a.AMST_Id == data.Temp_studentdata[i].AMST_Id).ToList();

                        if (check_stdid.Count() > 0)
                        {
                            var result = _examContext.Exm_Student_PersonalityDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.EP_Id == data.EP_Id && a.AMST_Id == data.Temp_studentdata[i].AMST_Id);
                            result.ESPM_Remarks = data.Temp_studentdata[i].ESP_Remarks;
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                        }
                        else
                        {
                            Exm_Student_PersonalityDMO map = new Exm_Student_PersonalityDMO();
                            map.AMST_Id = data.Temp_studentdata[i].AMST_Id;
                            map.ASMAY_Id = data.ASMAY_Id;
                            map.ASMCL_Id = data.ASMCL_Id;
                            map.ASMS_Id = data.ASMS_Id;
                            map.EP_Id = data.EP_Id;
                            map.MI_Id = data.MI_Id;
                            map.ESPM_Remarks = data.Temp_studentdata[i].ESP_Remarks;
                            map.ESPM_ActiveFlag = true;
                            map.CreatedDate = DateTime.Now;
                            map.UpdatedDate = DateTime.Now;
                            map.EME_Id = data.EME_Id;
                            _examContext.Add(map);
                        }
                    }
                    if (data.Temp_studentdata.Length > 0)
                    {
                        var d = _examContext.SaveChanges();
                        if (d > 0)
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
                data.returnval = false;
            }
            return data;
        }
        public exammasterpersonalityDTO editmappingdetails(exammasterpersonalityDTO data)
        {
            try
            {
                string order = "";
                var get_configuration = _examContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "studentname";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "studentname";
                }

                List<exammasterpersonalityDTO> studentList = new List<exammasterpersonalityDTO>();

                studentList = (from a in _examContext.School_Adm_Y_StudentDMO
                               from b in _examContext.Adm_M_Student
                               from c in _examContext.AcademicYear
                               from d in _examContext.AdmissionClass
                               from e in _examContext.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                               && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                               select new exammasterpersonalityDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                   AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                   AMST_RegistrationNo = b.AMST_RegistrationNo,
                                   AMAY_RollNo = a.AMAY_RollNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(exammasterpersonalityDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studentList = studentList.ToArray();


                List<exammasterpersonalityDTO> savedstudentList = new List<exammasterpersonalityDTO>();

                savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                    from b in _examContext.Adm_M_Student
                                    from c in _examContext.AcademicYear
                                    from d in _examContext.AdmissionClass
                                    from e in _examContext.School_M_Section
                                    from f in _examContext.Exm_Student_PersonalityDMO
                                    from g in _examContext.Exm_PersonalityDMO
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                    && f.EP_Id == g.EP_Id && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id
                                    && f.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                    && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                    && f.EME_Id == data.EME_Id && f.EP_Id == data.EP_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S"
                                    && b.AMST_ActiveFlag == 1 && f.ESPM_ActiveFlag == true)
                                    select new exammasterpersonalityDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                        AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                        AMST_RegistrationNo = b.AMST_RegistrationNo,
                                        AMAY_RollNo = a.AMAY_RollNo,
                                        EP_PersonlaityName = g.EP_PersonlaityName,
                                        ESPM_Remarks = f.ESPM_Remarks
                                    }).Distinct().ToList();

                var propertyInfo1 = typeof(exammasterpersonalityDTO).GetProperty(order);
                savedstudentList = savedstudentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();
                data.savedata = savedstudentList.ToArray();

                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();
                data.classlist = _examContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id).ToArray();
                data.sectionlist = _examContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMS_Id == data.ASMS_Id).ToArray();
                data.personlaitylist = _examContext.Exm_PersonalityDMO.Where(a => a.MI_Id == data.MI_Id && a.EP_Id == data.EP_Id).ToArray();
                data.examlist = _examContext.exammasterDMO.Where(a => a.EME_Id == data.EME_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }


    }
}
