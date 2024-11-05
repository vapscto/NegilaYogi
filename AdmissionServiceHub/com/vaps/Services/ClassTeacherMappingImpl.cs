using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{

    public class ClassTeacherMappingImpl : Interfaces.ClassTeacherMappingInterface
    {
        public ClassTeacherMappingContext _db;

        public ClassTeacherMappingImpl(ClassTeacherMappingContext db)
        {
            _db = db;
        }
        public ClassTeacherMappingDTO getdetails(int id)
        {
            ClassTeacherMappingDTO data = new ClassTeacherMappingDTO();
            try
            {
                data.accyear = _db.year.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.accclass = _db.classdetails.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToArray();
                data.accsection = _db.sectiondetails.Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToArray();

                data.empdetails = (from e in _db.employee
                                   where (e.MI_Id == id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.HRME_TechNonTeachingFlg == "Teaching")
                                   select new ClassTeacherMappingDTO
                                   {
                                       hrmE_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : e.HRME_EmployeeFirstName)
                                       + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" ? "" : " " + e.HRME_EmployeeMiddleName)
                                       + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" ? "" : " " + e.HRME_EmployeeLastName)
                                       + (e.HRME_EmployeeCode == null || e.HRME_EmployeeCode == "" ? "" : " : " + e.HRME_EmployeeCode)).Trim(),
                                       HRME_Id = e.HRME_Id,
                                   }).Distinct().ToArray();

                data.getsavedetails = (from a in _db.ClassTeacherMappingDMO
                                       from b in _db.year
                                       from c in _db.classdetails
                                       from d in _db.sectiondetails
                                       from e in _db.employee
                                       where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.HRME_Id == e.HRME_Id && a.MI_Id == id)
                                       select new ClassTeacherMappingDTO
                                       {
                                           IMCT_Id = a.IMCT_Id,
                                           ASMAY_Year = b.ASMAY_Year,
                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                           ASMS_SectionName = d.ASMC_SectionName,
                                           hrmE_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : e.HRME_EmployeeFirstName)
                                           + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" ? "" : " " + e.HRME_EmployeeMiddleName)
                                           + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),
                                           imct_activeflag = a.IMCT_ActiveFlag,
                                           employeecode = e.HRME_EmployeeCode == null || e.HRME_EmployeeCode == "" ? "" : e.HRME_EmployeeCode
                                       }).OrderByDescending(a => a.IMCT_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //save data
        public ClassTeacherMappingDTO save1(ClassTeacherMappingDTO data)
        {
            try
            {
                if (data.IMCT_Id > 0)
                {
                    List<ClassTeacherMappingDMO> allname2 = new List<ClassTeacherMappingDMO>();
                    ClassTeacherMappingDMO mm3 = new ClassTeacherMappingDMO();
                    allname2 = _db.ClassTeacherMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.HRME_Id && t.IMCT_Id != data.IMCT_Id).ToList();
                    if (allname2.Count > 0)
                    {
                        data.message = "For This Employee Already Class Teacher Is Mapped"; //"For This Employee Already Class Teacher Is Mapped";
                    }
                    else
                    {

                    }
                }
                else
                {
                    List<ClassTeacherMappingDMO> Allname1 = new List<ClassTeacherMappingDMO>();
                    ClassTeacherMappingDMO MM2 = new ClassTeacherMappingDMO();
                    Allname1 = _db.ClassTeacherMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IMCT_ActiveFlag == true).ToList();
                    if (Allname1.Count > 0)
                    {
                        data.message = "For This Class And Section Already Class Teacher Is Mapped"; //"For This Class And Section Already Class Teacher Is Mapped";
                    }
                    else
                    {
                        Allname1 = _db.ClassTeacherMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.HRME_Id && t.IMCT_ActiveFlag == true).ToList();
                        if (Allname1.Count > 0)
                        {
                            data.message = "For This Employee Already Class Teacher Is Mapped";
                        }
                        else
                        {
                            MM2.MI_Id = data.MI_Id;
                            MM2.ASMAY_Id = data.ASMAY_Id;
                            MM2.ASMCL_Id = data.ASMCL_Id;
                            MM2.ASMS_Id = data.ASMS_Id;
                            MM2.HRME_Id = data.HRME_Id;
                            MM2.IMCT_ActiveFlag = true;
                            MM2.CreatedDate = DateTime.Now;
                            MM2.UpdatedDate = DateTime.Now;
                            _db.Add(MM2);
                            int flag = _db.SaveChanges();
                            if (flag > 0)
                            {
                                data.retruval = true;
                            }
                            else
                            {
                                data.retruval = false;
                            }

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

        public ClassTeacherMappingDTO save(ClassTeacherMappingDTO data)
        {
            try
            {
                if (data.IMCT_Id > 0)
                {
                    List<ClassTeacherMappingDMO> allname2 = new List<ClassTeacherMappingDMO>();
                    ClassTeacherMappingDMO mm3 = new ClassTeacherMappingDMO();
                    allname2 = _db.ClassTeacherMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.HRME_Id && t.IMCT_Id != data.IMCT_Id).ToList();
                    if (allname2.Count > 0)
                    {
                        data.message = "For This Employee Already Class Teacher Is Mapped"; //"For This Employee Already Class Teacher Is Mapped";
                    }
                    else
                    {

                    }
                }
                else
                {
                    ClassTeacherMappingDMO MM2 = new ClassTeacherMappingDMO();
                    var check = _db.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToList();
                    if (check.Count > 0)
                    {
                        data.message = "Already Mapped";
                    }
                    else
                    {
                        MM2.MI_Id = data.MI_Id;
                        MM2.ASMAY_Id = data.ASMAY_Id;
                        MM2.ASMCL_Id = data.ASMCL_Id;
                        MM2.ASMS_Id = data.ASMS_Id;
                        MM2.HRME_Id = data.HRME_Id;
                        MM2.IMCT_ActiveFlag = true;
                        MM2.CreatedDate = DateTime.Now;
                        MM2.UpdatedDate = DateTime.Now;
                        _db.Add(MM2);
                        int flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            data.retruval = true;
                        }
                        else
                        {
                            data.retruval = false;
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

        //edit record 
        public ClassTeacherMappingDTO GetSelectedRowDetails(ClassTeacherMappingDTO data)
        {
            List<ClassTeacherMappingDMO> classteacher = new List<ClassTeacherMappingDMO>();
            try
            {
                classteacher = _db.ClassTeacherMappingDMO.Where(t => t.IMCT_Id == data.IMCT_Id && t.MI_Id == data.MI_Id).ToList();
                data.geteditdetails = classteacher.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public ClassTeacherMappingDTO onchangestaff1(ClassTeacherMappingDTO data)
        {
            List<ClassTeacherMappingDMO> classteacher = new List<ClassTeacherMappingDMO>();
            try
            {

                data.onchangestaff = (from a in _db.ClassTeacherMappingDMO
                                      from b in _db.sectiondetails
                                      from c in _db.classdetails
                                      where (a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.HRME_Id == data.HRME_Id)
                                      select new ClassTeacherMappingDTO
                                      {
                                          classname = c.ASMCL_ClassName,
                                          secname = b.ASMC_SectionName,
                                          staffpk1 = a.IMCT_Id
                                      }).ToArray();

                data.empdetails = (from e in _db.employee
                                   where (e.MI_Id == data.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.HRME_Id != data.HRME_Id)
                                   select new ClassTeacherMappingDTO
                                   {
                                       hrmE_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : e.HRME_EmployeeFirstName)
                                           + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" ? "" : " " + e.HRME_EmployeeMiddleName)
                                           + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" ? "" : " " + e.HRME_EmployeeLastName)
                                            + (e.HRME_EmployeeCode == null || e.HRME_EmployeeCode == "" ? "" : " : " + e.HRME_EmployeeCode)).Trim(),
                                       HRME_Id = e.HRME_Id,
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClassTeacherMappingDTO onchangestaff2(ClassTeacherMappingDTO data)
        {
            List<ClassTeacherMappingDMO> classteacher = new List<ClassTeacherMappingDMO>();
            try
            {

                data.onchangestaff = (from a in _db.ClassTeacherMappingDMO
                                      from b in _db.sectiondetails
                                      from c in _db.classdetails
                                      where (a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.HRME_Id == data.HRME_Id)
                                      select new ClassTeacherMappingDTO
                                      {
                                          classname = c.ASMCL_ClassName,
                                          secname = b.ASMC_SectionName,
                                          staffpk2 = a.IMCT_Id
                                      }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //saving the exchange data
        public ClassTeacherMappingDTO exchangesave(ClassTeacherMappingDTO data)
        {
            try
            {
                //updating staff1 class teacher to staff2 class teacher
                var result = _db.ClassTeacherMappingDMO.Single(t => t.IMCT_Id == data.staffpk1 && t.MI_Id == data.MI_Id);
                result.HRME_Id = data.HRME_Id2;
                result.UpdatedDate = DateTime.Now;
                _db.Update(result);
                int n = _db.SaveChanges();
                if (n > 0)
                {
                    data.retruval = true;
                }
                else
                {
                    data.retruval = false;
                }
                //updating staff2 class teacher to staff1 class teacher
                var result2 = _db.ClassTeacherMappingDMO.Single(t => t.IMCT_Id == data.staffpk2 && t.MI_Id == data.MI_Id);
                result2.HRME_Id = data.HRME_Id1;
                result2.UpdatedDate = DateTime.Now;
                _db.Update(result2);
                int n1 = _db.SaveChanges();
                if (n1 > 0)
                {
                    data.retruval = true;
                }
                else
                {
                    data.retruval = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //deleteing the record 
        public ClassTeacherMappingDTO deleterecord(ClassTeacherMappingDTO data)
        {
            try
            {
                ClassTeacherMappingDMO dmmo = new ClassTeacherMappingDMO();
                var result = _db.ClassTeacherMappingDMO.Single(t => t.MI_Id == data.MI_Id && t.IMCT_Id == data.IMCT_Id);
                if (result.IMCT_ActiveFlag == true)
                {
                    result.IMCT_ActiveFlag = false;
                    result.UpdatedDate = DateTime.Now;
                    _db.Update(result);
                    int n = _db.SaveChanges();
                    if (n > 0)
                    {
                        data.retruval = true;
                    }
                    else
                    {
                        data.retruval = false;
                    }
                }
                else
                {
                    result.IMCT_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _db.Update(result);
                    int n = _db.SaveChanges();
                    if (n > 0)
                    {
                        data.retruval = true;
                    }
                    else
                    {
                        data.retruval = false;
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