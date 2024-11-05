using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportMasterHouseCommitteImpl : Interfaces.SportMasterHouseCommitteInterface
    {
        private static ConcurrentDictionary<string, HouseCommitte_DTO> _login =
    new ConcurrentDictionary<string, HouseCommitte_DTO>();

        private readonly SportsContext _sportcontext;
        //private readonly castecategoryContext _castecategoryContext;

        //private readonly ILogger<SportMasterHouseCommitteImpl> _log;


        public SportMasterHouseCommitteImpl(SportsContext sportcontext/*, ILogger<SportMasterHouseCommitteImpl> log*/)
        {
            _sportcontext = sportcontext;
            //_log = log;

        }

        public HouseCommitte_DTO GetmastercasteData(HouseCommitte_DTO data)//int IVRMM_Id
        {


            try
            {

                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.DesignationList = (from a in _sportcontext.SportMasterHouseDessignationDMO
                                        where (a.MI_Id == data.MI_Id && a.SPCCMHD_ActiveFlag == true)
                                        select new HouseCommitte_DTO
                                        {
                                            SPCCMHD_Id = a.SPCCMHD_Id,
                                            SPCCMHD_DesignationName = a.SPCCMHD_DesignationName,

                                        }).Distinct().ToArray();
                //data.houseList = (from a in _sportcontext.SportMasterHouseDMO
                //                  where (a.MI_Id == data.MI_Id && a.SPCCMH_ActiveFlag == true)
                //                        select new HouseCommitte_DTO
                //                        {
                //                            SPCCMH_Id = a.SPCCMH_Id,
                //                            SPCCMH_HouseName = a.SPCCMH_HouseName,

                //                        }).Distinct().ToArray();
                data.ClassList = (from b in _sportcontext.admissionClass
                                  from c in _sportcontext.SportStudentHouseDivisionDMO
                                  where (b.ASMCL_Id == c.ASMCL_Id && c.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && c.SPCCMH_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                  select new HouseCommitte_DTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                  }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

                data.GridviewDetails = (from a in _sportcontext.SportMasterHouseCommitteDMO
                                        from b in _sportcontext.admissionStduent
                                        from c in _sportcontext.SportMasterHouseDessignationDMO
                                        from d in _sportcontext.SportMasterHouseDMO
                                        from e in _sportcontext.SportStudentHouseDivisionDMO
                                        from g in _sportcontext.AcademicYear
                                        where (a.AMST_Id == b.AMST_Id && e.SPCCMH_Id == a.SPCCMH_Id && e.SPCCMH_Id == d.SPCCMH_Id && a.AMST_Id == e.AMST_Id && a.SPCCMHD_Id == c.SPCCMHD_Id && a.MI_Id == data.MI_Id && e.ASMAY_Id == g.ASMAY_Id)//&& e.ASMAY_Id==data.ASMAY_Id 
                                        select new HouseCommitte_DTO
                                        {
                                            SPCCMHC_Id = a.SPCCMHC_Id,
                                            SPCCMHC_ContactNo = a.SPCCMHC_ContactNo,
                                            SPCCMHC_EmailId = a.SPCCMHC_EmailId,
                                            SPCCMHD_DesignationName = c.SPCCMHD_DesignationName,
                                            SPCCMH_HouseName = d.SPCCMH_HouseName,
                                            studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                            SPCCMHD_ActiveFlag = a.SPCCMHD_ActiveFlag,
                                            ASMAY_Id = e.ASMAY_Id,
                                            ASMAY_Year = g.ASMAY_Year
                                        }).Distinct().ToArray();

                if (data.GridviewDetails.Length > 0)
                {
                    data.count = data.GridviewDetails.Length;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HouseCommitte_DTO get_section(HouseCommitte_DTO dto)
        {
            try
            {

                dto.SectionList = (from b in _sportcontext.masterSection
                                   from c in _sportcontext.SportStudentHouseDivisionDMO
                                   where (c.ASMS_Id == b.ASMS_Id && c.MI_Id == b.MI_Id && c.SPCCMH_ActiveFlag == true && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id)
                                   select new HouseCommitte_DTO
                                   {
                                       ASMS_Id = c.ASMS_Id,
                                       ASMC_SectionName = b.ASMC_SectionName,
                                   }).Distinct().OrderBy(t => t.ASMC_Order).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public HouseCommitte_DTO get_student(HouseCommitte_DTO dto)
        {
            try
            {
                dto.studentList = (from a in _sportcontext.admissionyearstudent
                                   from b in _sportcontext.admissionStduent
                                   where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                   select new HouseCommitte_DTO
                                   {
                                       AMST_Id = b.AMST_Id,
                                       studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),

                                   }
                              ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public HouseCommitte_DTO GetSelectedRowDetails(HouseCommitte_DTO HouseCommitte_DTO)
        {
            try
            {

                //var asmay_ids = _sportcontext.AcademicYear.Where(t => t.MI_Id == HouseCommitte_DTO.MI_Id && t.ASMAY_From_Date < DateTime.Now && t.ASMAY_To_Date > DateTime.Now).Distinct().Select(i => i.ASMAY_Id).ToList();

                HouseCommitte_DTO.studentList = (from a in _sportcontext.SportMasterHouseCommitteDMO
                                                 from b in _sportcontext.admissionStduent
                                                 from c in _sportcontext.SportMasterHouseDessignationDMO
                                                 from d in _sportcontext.SportMasterHouseDMO
                                                 from e in _sportcontext.admissionyearstudent
                                                 from f in _sportcontext.SportStudentHouseDivisionDMO

                                                 where (a.AMST_Id == b.AMST_Id && a.SPCCMHD_Id == c.SPCCMHD_Id && a.SPCCMH_Id == d.SPCCMH_Id && a.SPCCMHC_Id == HouseCommitte_DTO.SPCCMHC_Id && b.AMST_Id == e.AMST_Id && f.AMST_Id == a.AMST_Id && f.SPCCMH_Id == a.SPCCMH_Id && f.ASMAY_Id == HouseCommitte_DTO.ASMAY_Id && a.MI_Id == HouseCommitte_DTO.MI_Id) //&& asmay_ids.Contains(f.ASMAY_Id)
                                                 select new HouseCommitte_DTO
                                                 {
                                                     SPCCMHC_Id = a.SPCCMHC_Id,
                                                     SPCCMHC_ContactNo = a.SPCCMHC_ContactNo,
                                                     SPCCMHC_EmailId = a.SPCCMHC_EmailId,
                                                     SPCCMHD_Id = c.SPCCMHD_Id,
                                                     SPCCMH_Id = d.SPCCMH_Id,
                                                     AMST_Id = b.AMST_Id,
                                                     SPCCMHD_ActiveFlag = a.SPCCMHD_ActiveFlag,
                                                     ASMAY_Id = f.ASMAY_Id,
                                                 }).Distinct().ToArray();

                HouseCommitte_DTO.DesignationList = _sportcontext.SportMasterHouseDessignationDMO.Where(t => t.MI_Id == HouseCommitte_DTO.MI_Id && t.SPCCMHD_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMHD_Id).ToArray();

                //HouseCommitte_DTO.houseList = _sportcontext.SportMasterHouseDMO.Where(t => t.MI_Id == HouseCommitte_DTO.MI_Id && t.SPCCMH_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return HouseCommitte_DTO;
        }

        public HouseCommitte_DTO deactivate(HouseCommitte_DTO dto)
        {
            try
            {

                var result = _sportcontext.SportMasterHouseCommitteDMO.Single(t => t.SPCCMHC_Id == dto.SPCCMHC_Id && t.MI_Id == dto.MI_Id);
                if (result.SPCCMHD_ActiveFlag == true)
                {
                    result.SPCCMHD_ActiveFlag = false;
                }
                else
                {
                    result.SPCCMHD_ActiveFlag = true;
                }
                result.CreatedDate = result.CreatedDate;
                result.UpdatedDate = DateTime.Now;
                _sportcontext.Update(result);
                var flag = _sportcontext.SaveChanges();
                if (flag == 1)
                {
                    dto.returnVal = true;

                    if (result.SPCCMHD_ActiveFlag == true)
                    {
                        dto.msg = "House Committee Activated Successfully.";
                    }
                    else if (result.SPCCMHD_ActiveFlag == false)
                    {
                        dto.msg = "House Committee Deactivated Successfully.";
                    }
                }
                else
                {
                    dto.returnVal = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HouseCommitte_DTO mastercasteData(HouseCommitte_DTO mas)
        {

            try
            {

                if (mas.SPCCMHC_Id != 0)
                {

                    var duplicate = _sportcontext.SportMasterHouseCommitteDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMHC_Id != mas.SPCCMHC_Id && (t.AMST_Id == mas.AMST_Id && t.SPCCMHD_Id == mas.SPCCMHD_Id && t.SPCCMH_Id == mas.SPCCMH_Id) || (t.AMST_Id != mas.AMST_Id && t.SPCCMHD_Id == mas.SPCCMHD_Id && t.SPCCMH_Id == mas.SPCCMH_Id)).ToList();



                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {

                        var result = _sportcontext.SportMasterHouseCommitteDMO.Single(t => t.MI_Id == mas.MI_Id && t.SPCCMHC_Id == mas.SPCCMHC_Id);

                        result.SPCCMHC_ContactNo = mas.SPCCMHC_ContactNo;
                        result.SPCCMHC_EmailId = mas.SPCCMHC_EmailId;
                        result.SPCCMHC_Id = mas.SPCCMHC_Id;
                        result.SPCCMHD_Id = mas.SPCCMHD_Id;
                        result.SPCCMH_Id = mas.SPCCMH_Id;
                        result.AMST_Id = mas.AMST_Id;
                        result.MI_Id = mas.MI_Id;

                        result.UpdatedDate = DateTime.Now;
                        result.CreatedDate = result.CreatedDate;
                        _sportcontext.Update(result);
                        var flag = _sportcontext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.returnVal_update = true;
                        }
                        else
                        {
                            mas.returnVal_update = false;
                        }

                    }

                }
                else
                {

                    var duplicate_caste_name = _sportcontext.SportMasterHouseCommitteDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMHC_Id != mas.SPCCMHC_Id && (t.AMST_Id == mas.AMST_Id && t.SPCCMHD_Id == mas.SPCCMHD_Id && t.SPCCMH_Id == mas.SPCCMH_Id) || (t.AMST_Id != mas.AMST_Id && t.SPCCMHD_Id == mas.SPCCMHD_Id && t.SPCCMH_Id == mas.SPCCMH_Id)).ToList();


                    if (duplicate_caste_name.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }

                    else
                    {
                        SportMasterHouseCommitteDMO MM3 = new SportMasterHouseCommitteDMO();
                        MM3.SPCCMHC_ContactNo = mas.SPCCMHC_ContactNo;
                        MM3.SPCCMHC_EmailId = mas.SPCCMHC_EmailId;
                        MM3.SPCCMHC_Id = mas.SPCCMHC_Id;
                        MM3.SPCCMHD_Id = mas.SPCCMHD_Id;
                        MM3.SPCCMH_Id = mas.SPCCMH_Id;
                        MM3.AMST_Id = mas.AMST_Id;

                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        MM3.SPCCMHD_ActiveFlag = true;
                        MM3.MI_Id = mas.MI_Id;
                        _sportcontext.Add(MM3);
                        var flag = _sportcontext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.SPCCMHC_Id = MM3.SPCCMHC_Id;
                            mas.returnVal = true;
                        }
                        else
                        {
                            mas.returnVal = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mas;
        }


        public HouseCommitte_DTO onhousechage(HouseCommitte_DTO obj)
        {
            try
            {
                var student = (from h1 in _sportcontext.SportMasterHouseDMO
                               from h in _sportcontext.SportStudentHouseDivisionDMO
                               from y in _sportcontext.admissionyearstudent
                               from st in _sportcontext.Adm_M_Student
                               from c in _sportcontext.admissionClass
                               from s in _sportcontext.masterSection
                               where (h1.SPCCMH_Id == h.SPCCMH_Id && y.ASMAY_Id == h.ASMAY_Id && y.ASMCL_Id == h.ASMCL_Id && y.ASMS_Id == h.ASMS_Id && y.AMST_Id == h.AMST_Id && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id && h.AMST_Id == st.AMST_Id && st.AMST_SOL.Equals("S") && y.AMAY_ActiveFlag == 1 && h.SPCCMH_ActiveFlag == true && h.MI_Id == obj.MI_Id && y.ASMAY_Id == obj.ASMAY_Id && h.SPCCMH_Id == obj.SPCCMH_Id)
                               select new HouseCommitte_DTO
                               {
                                   AMST_Id = st.AMST_Id,
                                   studentname = st.AMST_FirstName + (string.IsNullOrEmpty(st.AMST_MiddleName) ? "" : ' ' + st.AMST_MiddleName) + (string.IsNullOrEmpty(st.AMST_LastName) ? "" : ' ' + st.AMST_LastName),
                                   AMST_AdmNo = st.AMST_AdmNo,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMC_SectionName = s.ASMC_SectionName,

                               }).Distinct().OrderBy(n => n.studentname).ToList();
                if (student.Count > 0)
                {
                    obj.studentList = student.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public HouseCommitte_DTO get_House(HouseCommitte_DTO data)
        {
            try
            {
                data.houseList = (from t in _sportcontext.SportMasterHouseDMO
                                  from b in _sportcontext.SportStudentHouseDivisionDMO
                                  where (t.MI_Id == b.MI_Id && t.SPCCMH_Id == b.SPCCMH_Id && t.MI_Id == data.MI_Id && t.SPCCMH_ActiveFlag == true && b.SPCCMH_ActiveFlag == true)
                                  select t
                                  ).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
