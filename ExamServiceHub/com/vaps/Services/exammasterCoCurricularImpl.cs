
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
    public class exammasterCoCurricularImpl : Interfaces.exammasterCoCurricularInterface
    {
        private static ConcurrentDictionary<string, exammasterCoCurricularDTO> _login =
         new ConcurrentDictionary<string, exammasterCoCurricularDTO>();

        private readonly ExamContext _examContext;

        public exammasterCoCurricularImpl(ExamContext examContext)
        {
            _examContext = examContext;
        }

        public exammasterCoCurricularDTO Getdetails(exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            exammasterCoCurricularDTO getdata = new exammasterCoCurricularDTO();
            try
            {
                List<exammasterCoCulrricularDMO> list = new List<exammasterCoCulrricularDMO>();
                list = _examContext.exammasterCoCulrricularDMO.Where(t => t.MI_Id == data.MI_Id).OrderBy(t => t.ECC_CoCurricularOrder).ToList();
                getdata.exammasterCoCurricularname = list.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
        }

        public exammasterCoCurricularDTO validateordernumber(exammasterCoCurricularDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.examCoCurricularDTO.Count() > 0)
                {
                    foreach (exammasterCoCurricularDTO mob in dto.examCoCurricularDTO)
                    {
                        if (mob.ECC_Id > 0)
                        {
                            var result = _examContext.exammasterCoCulrricularDMO.Single(t => t.ECC_Id.Equals(mob.ECC_Id));
                            Mapper.Map(mob, result);
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                            _examContext.SaveChanges();
                        }
                    }
                    dto.retrunMsg = "Order Updated Successfully";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured"; ;
            }
            return dto;
        }

        public exammasterCoCurricularDTO savedetails(exammasterCoCurricularDTO data)
        {

            if (data.ECC_Id != 0)
            {
                var res = _examContext.exammasterCoCulrricularDMO.Where(t => t.MI_Id == data.MI_Id && (t.ECC_CoCurricularName == data.ECC_CoCurricularName || t.ECC_CoCurricularCode == data.ECC_CoCurricularCode) && t.ECC_Id != data.ECC_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var result = _examContext.exammasterCoCulrricularDMO.Single(t => t.MI_Id == data.MI_Id && t.ECC_Id == data.ECC_Id);
                    result.ECC_CoCurricularName = data.ECC_CoCurricularName;
                    result.ECC_CoCurricularCode = data.ECC_CoCurricularCode;
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
                var res = _examContext.exammasterCoCulrricularDMO.Where(t => t.MI_Id == data.MI_Id && (t.ECC_CoCurricularName == data.ECC_CoCurricularName || t.ECC_CoCurricularCode == data.ECC_CoCurricularCode)).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var row_cnt = _examContext.exammasterCoCulrricularDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                    exammasterCoCulrricularDMO exm = new exammasterCoCulrricularDMO();
                    exm.ECC_CoCurricularName = data.ECC_CoCurricularName;
                    exm.ECC_CoCurricularCode = data.ECC_CoCurricularCode;
                    exm.ECC_CoCurricularOrder = row_cnt + 1;
                    exm.MI_Id = data.MI_Id;
                    exm.ECC_ActiveFlag = true;

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

            return data;
        }

        public exammasterCoCurricularDTO editdetails(int ID)
        {
            exammasterCoCurricularDTO editlt = new exammasterCoCurricularDTO();
            try
            {
                List<exammasterCoCulrricularDMO> list = new List<exammasterCoCulrricularDMO>();
                list = _examContext.exammasterCoCulrricularDMO.Where(t => t.ECC_Id == ID).ToList();
                editlt.exammCoCurricularname = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }

        public exammasterCoCurricularDTO deactivate(exammasterCoCurricularDTO data)
        {
            data.already_cnt = false;
            if (data.ECC_Id > 0)
            {
                var checkused = _examContext.Exm_Student_CoCurricularDMO.Where(a => a.MI_Id == data.MI_Id && a.ECC_Id == data.ECC_Id).ToList();
                if (checkused.Count() > 0)
                {
                    data.retrunMsg = "Cannot";
                }
                else
                {
                    var result = _examContext.exammasterCoCulrricularDMO.Single(t => t.ECC_Id == data.ECC_Id);
                    if (result.ECC_ActiveFlag == true)
                    {
                        result.ECC_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        _examContext.Update(result);
                    }
                    else
                    {
                        result.ECC_ActiveFlag = true;
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
            return data;
        }

        // Student Personlaiy Mapping 

        public exammasterCoCurricularDTO studentdataload(exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            try
            {
                List<long> classid = new List<long>();
                List<long> sectionid = new List<long>();

                var getuserid = _examContext.ApplUser.Where(a => a.UserName.Equals(data.UserName.Trim())).Select(a => a.Id);

                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.examlist= _examContext.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderByDescending(a => a.EME_ExamOrder).ToArray();

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

                            data.loaddata = (from a in _examContext.Exm_Student_CoCurricularDMO
                                             from b in _examContext.exammasterCoCulrricularDMO
                                             from c in _examContext.exammasterDMO
                                             from d in _examContext.AcademicYear
                                             from e in _examContext.AdmissionClass
                                             from f in _examContext.School_M_Section
                                             where (a.ECC_Id == b.ECC_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id 
                                             && a.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && classid.Contains(a.ASMCL_Id) && sectionid.Contains(a.ASMS_Id))
                                             select new exammasterCoCurricularDTO
                                             {
                                                 ECC_CoCurricularName = b.ECC_CoCurricularName,
                                                 ECC_CoCurricularCode = b.ECC_CoCurricularCode,
                                                 monthname = c.EME_ExamName,
                                                 ASMCL_ClassName = e.ASMCL_ClassName,
                                                 ASMC_SectionName = f.ASMC_SectionName,
                                                 ASMAY_Year = d.ASMAY_Year,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                                 ECC_Id = a.ECC_Id,
                                                 EME_Id = a.EME_Id
                                             }).Distinct().OrderBy(a => a.ECC_CoCurricularCode).ToArray();
                        }
                    }
                }
                else
                {
                    data.loaddata = (from a in _examContext.Exm_Student_CoCurricularDMO
                                     from b in _examContext.exammasterCoCulrricularDMO
                                     from c in _examContext.exammasterDMO
                                     from d in _examContext.AcademicYear
                                     from e in _examContext.AdmissionClass
                                     from f in _examContext.School_M_Section
                                     where (a.ECC_Id == b.ECC_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id 
                                     && a.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id)
                                     select new exammasterCoCurricularDTO
                                     {
                                         ECC_CoCurricularName = b.ECC_CoCurricularName,
                                         ECC_CoCurricularCode = b.ECC_CoCurricularCode,
                                         monthname = c.EME_ExamName,
                                         ASMCL_ClassName = e.ASMCL_ClassName,
                                         ASMC_SectionName = f.ASMC_SectionName,
                                         ASMAY_Year = d.ASMAY_Year,
                                         ASMCL_Id = a.ASMCL_Id,
                                         ASMS_Id = a.ASMS_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         ECC_Id = a.ECC_Id,
                                         EME_Id = a.EME_Id
                                     }).Distinct().OrderBy(a => a.ECC_CoCurricularCode).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public exammasterCoCurricularDTO onchangeyear(exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            try
            {
                var getuserid = _examContext.ApplUser.Where(a => a.UserName.Equals(data.UserName.Trim())).Select(a => a.Id);

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
                    var check_classteacher = _examContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {
                        data.classlist = (from a in _examContext.ClassTeacherMappingDMO
                                          from b in _examContext.AdmissionClass
                                          from c in _examContext.AcademicYear
                                          where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                          && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                          select new exammasterCoCurricularDTO
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
        public exammasterCoCurricularDTO onchangeclass(exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            try
            {
                var getuserid = _examContext.ApplUser.Where(a => a.UserName.Equals(data.UserName.Trim())).Select(a => a.Id);

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
                    var check_classteacher = _examContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {
                        data.sectionlist = (from a in _examContext.ClassTeacherMappingDMO
                                            from b in _examContext.AdmissionClass
                                            from d in _examContext.School_M_Section
                                            from c in _examContext.AcademicYear
                                            where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                            && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                            select new exammasterCoCurricularDTO
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
        public exammasterCoCurricularDTO onchangesection(exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            try
            {
                data.monthlist = _examContext.IVRM_Month_DMO.ToArray();

                data.personlaitylist = _examContext.exammasterCoCulrricularDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ECC_CoCurricularOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterCoCurricularDTO searchdata(exammasterCoCurricularDTO data)//int IVRMM_Id
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

                List<exammasterCoCurricularDTO> studentList = new List<exammasterCoCurricularDTO>();

                studentList = (from a in _examContext.School_Adm_Y_StudentDMO
                               from b in _examContext.Adm_M_Student
                               from c in _examContext.AcademicYear
                               from d in _examContext.AdmissionClass
                               from e in _examContext.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                               && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id 
                               && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                               select new exammasterCoCurricularDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                   AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                   AMST_RegistrationNo = b.AMST_RegistrationNo,
                                   AMAY_RollNo = a.AMAY_RollNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(exammasterCoCurricularDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studentList = studentList.ToArray();


                List<exammasterCoCurricularDTO> savedstudentList = new List<exammasterCoCurricularDTO>();

                savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                    from b in _examContext.Adm_M_Student
                                    from c in _examContext.AcademicYear
                                    from d in _examContext.AdmissionClass
                                    from e in _examContext.School_M_Section
                                    from f in _examContext.Exm_Student_CoCurricularDMO
                                    from g in _examContext.exammasterCoCulrricularDMO
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                    && f.ECC_Id == g.ECC_Id && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id
                                    && f.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                    && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                    && f.EME_Id == data.EME_Id && f.ECC_Id == data.ECC_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S"
                                    && b.AMST_ActiveFlag == 1 && f.ESCOM_ActiveFlag == true)
                                    select new exammasterCoCurricularDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                        AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                        AMST_RegistrationNo = b.AMST_RegistrationNo,
                                        AMAY_RollNo = a.AMAY_RollNo,
                                        ECC_CoCurricularName = g.ECC_CoCurricularName,
                                        ESCOM_Remarks = f.ESCOM_Remarks
                                    }).Distinct().ToList();

                var propertyInfo1 = typeof(exammasterCoCurricularDTO).GetProperty(order);
                savedstudentList = savedstudentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();
                data.savedata = savedstudentList.ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterCoCurricularDTO savemapping(exammasterCoCurricularDTO data)
        {
            try
            {
                data.returnval = false;

                if (data.Temp_studentdata.Length > 0)
                {
                    for (int i = 0; i < data.Temp_studentdata.Length; i++)
                    {
                        var check_stdid = _examContext.Exm_Student_CoCurricularDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.Monthid && a.ECC_Id == data.ECC_Id && a.AMST_Id == data.Temp_studentdata[i].AMST_Id).ToList();

                        if (check_stdid.Count() > 0)
                        {
                            var result = _examContext.Exm_Student_CoCurricularDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ECC_Id == data.ECC_Id && a.AMST_Id == data.Temp_studentdata[i].AMST_Id);
                            result.ESCOM_Remarks = data.Temp_studentdata[i].ESCO_Remarks;
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                        }
                        else
                        {
                            Exm_Student_CoCurricularDMO map = new Exm_Student_CoCurricularDMO();
                            map.AMST_Id = data.Temp_studentdata[i].AMST_Id;
                            map.ASMAY_Id = data.ASMAY_Id;
                            map.ASMCL_Id = data.ASMCL_Id;
                            map.ASMS_Id = data.ASMS_Id;
                            map.ECC_Id = data.ECC_Id;
                            map.MI_Id = data.MI_Id;
                            map.ESCOM_Remarks = data.Temp_studentdata[i].ESCO_Remarks;
                            map.ESCOM_ActiveFlag = true;
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
        public exammasterCoCurricularDTO editmappingdetails(exammasterCoCurricularDTO data)
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

                List<exammasterCoCurricularDTO> studentList = new List<exammasterCoCurricularDTO>();

                studentList = (from a in _examContext.School_Adm_Y_StudentDMO
                               from b in _examContext.Adm_M_Student
                               from c in _examContext.AcademicYear
                               from d in _examContext.AdmissionClass
                               from e in _examContext.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                               && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                               select new exammasterCoCurricularDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                   AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                   AMST_RegistrationNo = b.AMST_RegistrationNo,
                                   AMAY_RollNo = a.AMAY_RollNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(exammasterCoCurricularDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studentList = studentList.ToArray();


                List<exammasterCoCurricularDTO> savedstudentList = new List<exammasterCoCurricularDTO>();

                savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                    from b in _examContext.Adm_M_Student
                                    from c in _examContext.AcademicYear
                                    from d in _examContext.AdmissionClass
                                    from e in _examContext.School_M_Section
                                    from f in _examContext.Exm_Student_CoCurricularDMO
                                    from g in _examContext.exammasterCoCulrricularDMO
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                    && f.ECC_Id == g.ECC_Id && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id
                                    && f.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                    && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                    && f.EME_Id == data.EME_Id && f.ECC_Id == data.ECC_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S"
                                    && b.AMST_ActiveFlag == 1 && f.ESCOM_ActiveFlag == true)
                                    select new exammasterCoCurricularDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                        AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                        AMST_RegistrationNo = b.AMST_RegistrationNo,
                                        AMAY_RollNo = a.AMAY_RollNo,
                                        ECC_CoCurricularName = g.ECC_CoCurricularName,
                                        ESCOM_Remarks = f.ESCOM_Remarks
                                    }).Distinct().ToList();

                var propertyInfo1 = typeof(exammasterCoCurricularDTO).GetProperty(order);
                savedstudentList = savedstudentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();
                data.savedata = savedstudentList.ToArray();

                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();
                data.classlist = _examContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id).ToArray();
                data.sectionlist = _examContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMS_Id == data.ASMS_Id).ToArray();
                data.personlaitylist = _examContext.exammasterCoCulrricularDMO.Where(a => a.MI_Id == data.MI_Id && a.ECC_Id == data.ECC_Id).ToArray();
                data.examlist = _examContext.masterexam.Where(a => a.EME_Id == data.EME_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

    }
}
