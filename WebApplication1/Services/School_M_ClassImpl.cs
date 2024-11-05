using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class School_M_ClassImpl : Interfaces.School_M_ClassInterface
    {
        private static ConcurrentDictionary<string, School_M_ClassDTO> _login =
          new ConcurrentDictionary<string, School_M_ClassDTO>();

        public DomainModelMsSqlServerContext _Context;

        public School_M_ClassImpl(DomainModelMsSqlServerContext DomainModelContext)
        {
            _Context = DomainModelContext;
        }

        public School_M_ClassDTO saveSchool_M_Class(School_M_ClassDTO School_MDTO)
        {
            List<School_M_Class> allSchool_M_ClassList = new List<School_M_Class>();
            try
            {
                School_M_Class Class = Mapper.Map<School_M_Class>(School_MDTO);

                var record_update = _Context.School_M_Class.Where(d => d.MI_Id == School_MDTO.MI_Id && d.ASMCL_Id == Class.ASMCL_Id).ToList();
                if (Class.ASMCL_Id > 0)
                {
                    //var check_assignclass = (from a in _Context.Adm_M_Student
                    //                         from b in _Context.School_M_Class
                    //                         where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == School_MDTO.MI_Id && a.ASMCL_Id == School_MDTO.ASMCL_Id)
                    //                         select new School_M_ClassDTO
                    //                         {
                    //                             ASMCL_Id = b.ASMCL_Id
                    //                         }
                    //                       ).ToList();
                    //var check_assignclass1 = (from a in _Context.School_Adm_Y_StudentDMO
                    //                          from b in _Context.School_M_Class
                    //                          where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == School_MDTO.MI_Id && a.ASMCL_Id == School_MDTO.ASMCL_Id)
                    //                          select new School_M_ClassDTO
                    //                          {
                    //                              ASMCL_Id = b.ASMCL_Id
                    //                          }
                    //                       ).ToList();

                    //var check_assignclass2 = (from a in _Context.StudentApplication
                    //                          from b in _Context.School_M_Class
                    //                          where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == School_MDTO.MI_Id && a.ASMCL_Id == School_MDTO.ASMCL_Id)
                    //                          select new School_M_ClassDTO
                    //                          {
                    //                              ASMCL_Id = b.ASMCL_Id
                    //                          }
                    //                  ).ToList();

                    //if (check_assignclass.Count > 0 || check_assignclass1.Count > 0 || check_assignclass2.Count > 0)
                    //{                      

                    //    School_MDTO.returnval = "Sorry You Can Not Edit This Record It Is Already Mapped..";
                    //    allSchool_M_ClassList = _Context.School_M_Class.ToList();
                    //    School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();

                    //    return School_MDTO;

                    //}

                    //else if (record_update.Count > 0)
                    //{
                    //    var result_update = _Context.School_M_Class.Single(t => t.ASMCL_Id == Class.ASMCL_Id);
                    //    result_update.MI_Id = Class.MI_Id;
                    //    result_update.ASMCL_Id = Class.ASMCL_Id;
                    //    result_update.ASMCL_Order = Class.ASMCL_Order;
                    //    result_update.ASMCL_MinAgeYear = Class.ASMCL_MinAgeYear;
                    //    result_update.ASMCL_MinAgeMonth = Class.ASMCL_MinAgeMonth;
                    //    result_update.ASMCL_MinAgeDays = Class.ASMCL_MinAgeDays;
                    //    result_update.ASMCL_MaxCapacity = Class.ASMCL_MaxCapacity;
                    //    result_update.ASMCL_MaxAgeYear = Class.ASMCL_MaxAgeYear;
                    //    result_update.ASMCL_MaxAgeMonth = Class.ASMCL_MaxAgeMonth;
                    //    result_update.ASMCL_MaxAgeDays = Class.ASMCL_MaxAgeDays;
                    //    result_update.ASMCL_ClassName = Class.ASMCL_ClassName;
                    //    result_update.ASMCL_ClassCode = Class.ASMCL_ClassCode;
                    //    result_update.ASMCL_PreadmFlag = Class.ASMCL_PreadmFlag;

                    //    if (result_update.ASMCL_ActiveFlag == false)
                    //    {
                    //        result_update.ASMCL_ActiveFlag = false;
                    //    }
                    //    else
                    //    {
                    //        result_update.ASMCL_ActiveFlag = true;
                    //    }


                    //    result_update.UpdatedDate = DateTime.Now;
                    //    result_update.ASMCL_ActiveFlag = true;
                    //    _Context.Update(result_update);
                    //    School_MDTO.returnval = "update";
                    //    _Context.SaveChanges();

                    //}


                    //else
                    //{
                    var isDuplicate_all = _Context.School_M_Class.Where(d => d.MI_Id == School_MDTO.MI_Id && d.ASMCL_ClassName == School_MDTO.ASMCL_ClassName && d.ASMCL_Id != Class.ASMCL_Id).ToList();

                    if (isDuplicate_all.Count > 0)
                    {
                        School_MDTO.returnval = "Record Already Exists";
                        allSchool_M_ClassList = _Context.School_M_Class.ToList();
                        School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();
                        return School_MDTO;
                    }

                    int ClassNameExistorder = _Context.School_M_Class.Where(t => t.ASMCL_Order == Class.ASMCL_Order && t.MI_Id == Class.MI_Id && t.ASMCL_Id != Class.ASMCL_Id).Count();
                    if (ClassNameExistorder > 0)
                    {
                        School_MDTO.returnval = "Record Already Exists";
                        allSchool_M_ClassList = _Context.School_M_Class.ToList();
                        School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();
                        return School_MDTO;
                    }
                    int ClassNameExistordercode = _Context.School_M_Class.Where(t => t.ASMCL_ClassCode == Class.ASMCL_ClassCode && t.ASMCL_ClassCode != null && t.ASMCL_ClassCode != "" && t.MI_Id == Class.MI_Id && t.ASMCL_Id != Class.ASMCL_Id).Count();
                    if (ClassNameExistordercode > 0)
                    {
                        School_MDTO.returnval = "Record Already Exists ";
                        allSchool_M_ClassList = _Context.School_M_Class.ToList();
                        School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();
                        return School_MDTO;
                    }

                    else
                    {
                        var result = _Context.School_M_Class.Single(t => t.ASMCL_Id == Class.ASMCL_Id);
                        result.MI_Id = Class.MI_Id;
                        result.ASMCL_Id = Class.ASMCL_Id;
                        result.ASMCL_Order = Class.ASMCL_Order;
                        result.ASMCL_MinAgeYear = Class.ASMCL_MinAgeYear;
                        result.ASMCL_MinAgeMonth = Class.ASMCL_MinAgeMonth;
                        result.ASMCL_MinAgeDays = Class.ASMCL_MinAgeDays;
                        result.ASMCL_MaxCapacity = Class.ASMCL_MaxCapacity;
                        result.ASMCL_MaxAgeYear = Class.ASMCL_MaxAgeYear;
                        result.ASMCL_MaxAgeMonth = Class.ASMCL_MaxAgeMonth;
                        result.ASMCL_MaxAgeDays = Class.ASMCL_MaxAgeDays;
                        result.ASMCL_ClassName = Class.ASMCL_ClassName;
                        result.ASMCL_ClassCode = Class.ASMCL_ClassCode;
                        result.ASMCL_PreadmFlag = Class.ASMCL_PreadmFlag;
                        if (Class.ASMCL_ActiveFlag == false)
                        {
                            result.ASMCL_ActiveFlag = false;
                        }
                        else
                        {
                            result.ASMCL_ActiveFlag = true;
                        }

                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _Context.Update(result);
                        School_MDTO.returnval = "update";
                        _Context.SaveChanges();
                    }
                }
                //}

                else
                {
                    int ClassNameExist = _Context.School_M_Class.Where(t => t.ASMCL_ClassName == Class.ASMCL_ClassName && t.MI_Id == Class.MI_Id && t.ASMCL_Id != Class.ASMCL_Id).Count();
                    if (ClassNameExist > 0)
                    {
                        School_MDTO.returnval = "Record Already Exists";
                        allSchool_M_ClassList = _Context.School_M_Class.ToList();
                        School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();
                        return School_MDTO;
                    }
                    int ClassNameExistorder = _Context.School_M_Class.Where(t => t.ASMCL_Order == Class.ASMCL_Order && t.MI_Id == Class.MI_Id).Count();
                    if (ClassNameExistorder > 0)
                    {
                        School_MDTO.returnval = "Record Already Exists";
                        allSchool_M_ClassList = _Context.School_M_Class.ToList();
                        School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();
                        return School_MDTO;
                    }

                    int ClassNameExistordercode = _Context.School_M_Class.Where(t => t.ASMCL_ClassCode == Class.ASMCL_ClassCode && t.ASMCL_ClassCode != null && t.ASMCL_ClassCode != "" && t.MI_Id == Class.MI_Id).Count();
                    if (ClassNameExistordercode > 0)
                    {
                        School_MDTO.returnval = "Record Already Exists";
                        allSchool_M_ClassList = _Context.School_M_Class.ToList();
                        School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();
                        return School_MDTO;
                    }


                    else
                    {

                        //added by 02/02/2017
                        Class.CreatedDate = DateTime.Now;
                        Class.UpdatedDate = DateTime.Now;
                        Class.ASMCL_ActiveFlag = true;
                        Class.ASMCL_PreadmFlag = Class.ASMCL_PreadmFlag;
                        _Context.Add(Class);
                        School_MDTO.returnval = "add";
                        _Context.SaveChanges();
                    }
                }

                allSchool_M_ClassList = _Context.School_M_Class.ToList();
                School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                allSchool_M_ClassList = _Context.School_M_Class.Where(t => t.MI_Id == School_MDTO.MI_Id).ToList();
                School_MDTO.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(d => d.CreatedDate).ToArray();
            }

            return School_MDTO;
        }
        //public EnqDTO countrydrp(EnqDTO stu)
        public School_M_ClassDTO AllDropdownList(School_M_ClassDTO stu)
        {
            try
            {
                List<School_M_Class> allSchool_M_ClassList = new List<School_M_Class>();
                allSchool_M_ClassList = _Context.School_M_Class.Where(c => c.MI_Id == stu.MI_Id).OrderByDescending(d => d.CreatedDate).ToList();
                stu.School_M_ClassList = allSchool_M_ClassList.Distinct().ToArray();

                stu.getclasslist = _Context.School_M_Class.Where(c => c.MI_Id == stu.MI_Id && c.ASMCL_ActiveFlag==true).OrderBy(d => d.ASMCL_Order).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }

        public School_M_ClassDTO deleterec(School_M_ClassDTO id)
        {
            School_M_ClassDTO stu = new School_M_ClassDTO();

            List<School_M_Class> lorg = new List<School_M_Class>();

            try
            {


                var check_assignclass = (from a in _Context.Adm_M_Student
                                         from b in _Context.School_M_Class
                                         where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == id.MI_Id && a.ASMCL_Id == id.ASMCL_Id)
                                         select new School_M_ClassDTO
                                         {
                                             ASMCL_Id = b.ASMCL_Id
                                         }
                                      ).ToList();
                var check_assignclass1 = (from a in _Context.School_Adm_Y_StudentDMO
                                          from b in _Context.School_M_Class
                                          where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == id.MI_Id && a.ASMCL_Id == id.ASMCL_Id)
                                          select new School_M_ClassDTO
                                          {
                                              ASMCL_Id = b.ASMCL_Id
                                          }
                                       ).ToList();

                var check_assignclass2 = (from a in _Context.StudentApplication
                                          from b in _Context.School_M_Class
                                          where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == id.MI_Id && a.ASMCL_Id == id.ASMCL_Id)
                                          select new School_M_ClassDTO
                                          {
                                              ASMCL_Id = b.ASMCL_Id
                                          }
                                  ).ToList();

                if (check_assignclass.Count > 0 || check_assignclass1.Count > 0 || check_assignclass2.Count > 0)
                {
                    id.returnval = "Class Name is Already Mapped, You Can Not Delete It.";
                    return id;
                }
                else
                {
                    var result = _Context.School_M_Class.Single(t => t.ASMCL_Id == id.ASMCL_Id);

                    if (result.ASMCL_ActiveFlag == true)
                    {
                        result.ASMCL_ActiveFlag = false;
                        //added by 02/02/2017


                        result.UpdatedDate = DateTime.Now;
                        _Context.Update(result);
                        _Context.SaveChanges();
                        stu.returnval = "true";
                    }
                    else
                    {

                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        result.ASMCL_ActiveFlag = true;
                        _Context.Update(result);
                        _Context.SaveChanges();
                        stu.returnval = "false";
                    }



                    List<School_M_Class> allSchool_M_ClassList = new List<School_M_Class>();
                    allSchool_M_ClassList = _Context.School_M_Class.Where(c => c.MI_Id == stu.MI_Id).ToList();
                    stu.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(d => d.CreatedDate).ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }
        //enable diable
        public School_M_ClassDTO deletedetails(School_M_ClassDTO id)
        {
            School_M_ClassDTO stu = new School_M_ClassDTO();

            List<School_M_Class> lorg = new List<School_M_Class>();

            try
            {


                var check_assignclass = (from a in _Context.Adm_M_Student
                                         from b in _Context.School_M_Class
                                         where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == id.MI_Id && a.ASMCL_Id == id.ASMCL_Id && b.ASMCL_ActiveFlag == true)
                                         select new School_M_ClassDTO
                                         {
                                             ASMCL_Id = b.ASMCL_Id
                                         }
                                      ).ToList();
                var check_assignclass1 = (from a in _Context.School_Adm_Y_StudentDMO
                                          from b in _Context.School_M_Class
                                          where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == id.MI_Id && a.ASMCL_Id == id.ASMCL_Id && b.ASMCL_ActiveFlag == true)
                                          select new School_M_ClassDTO
                                          {
                                              ASMCL_Id = b.ASMCL_Id
                                          }
                                       ).ToList();

                var check_assignclass2 = (from a in _Context.StudentApplication
                                          from b in _Context.School_M_Class
                                          where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == id.MI_Id && a.ASMCL_Id == id.ASMCL_Id && b.ASMCL_ActiveFlag == true)
                                          select new School_M_ClassDTO
                                          {
                                              ASMCL_Id = b.ASMCL_Id
                                          }
                                  ).ToList();

                if (check_assignclass.Count > 0 || check_assignclass1.Count > 0 || check_assignclass2.Count > 0)
                {
                    id.message = "Class Name is Already Mapped, You Can Not Disable It.";
                    return id;
                }
                else
                {
                    var result = _Context.School_M_Class.Single(t => t.ASMCL_Id == id.ASMCL_Id);

                    if (result.ASMCL_ActiveFlag == true)
                    {
                        result.ASMCL_ActiveFlag = false;
                        //added by 02/02/2017


                        result.UpdatedDate = DateTime.Now;
                        _Context.Update(result);
                        _Context.SaveChanges();
                        stu.returnval = "true";
                    }
                    else
                    {

                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        result.ASMCL_ActiveFlag = true;
                        _Context.Update(result);
                        _Context.SaveChanges();
                        stu.returnval = "false";
                    }



                    List<School_M_Class> allSchool_M_ClassList = new List<School_M_Class>();
                    allSchool_M_ClassList = _Context.School_M_Class.Where(c => c.MI_Id == stu.MI_Id).ToList();
                    stu.School_M_ClassList = allSchool_M_ClassList.OrderByDescending(c => c.CreatedDate).ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }

        public School_M_ClassDTO getdetails(int id)
        {
            School_M_ClassDTO org = new School_M_ClassDTO();
            // Institution_MobileDTO[] mobDTO = 0;
            try
            {
                List<School_M_Class> lorg = new List<School_M_Class>();

                lorg = _Context.School_M_Class.AsNoTracking().Where(t => t.ASMCL_Id.Equals(id)).ToList();
                org.School_M_ClassList = lorg.OrderByDescending(c => c.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }


        public School_M_ClassDTO searchByColumn(School_M_ClassDTO acd)
        {
            try
            {

                List<School_M_Class> allclass = new List<School_M_Class>();
                switch (acd.SearchColumn)
                {



                    case "1":





                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_ClassName.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;

                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "2":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_ClassCode.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "3":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_MaxCapacity.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "4":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_Order.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "5":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_MaxAgeYear.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "6":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_MaxAgeMonth.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "7":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_MaxAgeDays.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "8":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_MinAgeYear.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "9":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_MinAgeMonth.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "10":
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_MinAgeDays.ToString().ToLower().Contains(acd.EnteredData.ToLower()) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;
                    case "11":
                        if (acd.EnteredData.Equals("active", StringComparison.OrdinalIgnoreCase))

                        {
                            acd.EnteredData = "true";
                        }
                        else if (acd.EnteredData.Equals("inactive", StringComparison.OrdinalIgnoreCase))
                        {
                            acd.EnteredData = "false";
                        }
                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).Where(w => w.ASMCL_ActiveFlag == Convert.ToBoolean(acd.EnteredData) && w.MI_Id == acd.MI_Id).ToList();
                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;

                    case "0":


                        allclass = _Context.School_M_Class.OrderByDescending(d => d.CreatedDate).
                            Where(w => (w.ASMCL_ClassName.ToLower().Contains(acd.EnteredData.ToLower())
                           || ((w.ASMCL_ClassCode == null) ? false : (w.ASMCL_ClassCode.ToLower().ToString().Contains(acd.EnteredData.ToLower())))
                            || w.ASMCL_MaxCapacity.ToString().ToLower().Contains(acd.EnteredData.ToLower())
                            || w.ASMCL_Order.ToString().ToLower().Contains(acd.EnteredData.ToLower())
                            || w.ASMCL_MaxAgeYear.ToString().ToLower().Contains(acd.EnteredData.ToLower())
                            || w.ASMCL_MaxAgeMonth.ToString().ToLower().Contains(acd.EnteredData.ToLower())
                            || w.ASMCL_MaxAgeDays.ToString().ToLower().Contains(acd.EnteredData.ToLower())
                            || w.ASMCL_MinAgeYear.ToString().ToLower().Contains(acd.EnteredData.ToLower())
                            || w.ASMCL_MinAgeMonth.ToString().ToLower().Contains(acd.EnteredData.ToLower())
                            || w.ASMCL_MinAgeDays.ToString().ToLower().Contains(acd.EnteredData.ToLower())) && w.MI_Id == acd.MI_Id).ToList();


                        acd.School_M_ClassList = allclass.Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                        if (allclass.Count > 0)
                        {
                            acd.count = allclass.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return acd;
        }


    }
}
