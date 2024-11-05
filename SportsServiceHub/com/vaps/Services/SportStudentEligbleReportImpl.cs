﻿using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportStudentEligbleReportImpl : Interfaces.SportStudentEligbleReportInterface
    {
        private static ConcurrentDictionary<string, SportStudentParticipationReportDTO> _login =
   new ConcurrentDictionary<string, SportStudentParticipationReportDTO>();

        private readonly SportsContext _sportcontext;
        public StudentAttendanceReportContext _db;
        ILogger<SportStudentEligbleReportImpl> _acdimpl;
        public SportStudentEligbleReportImpl(SportsContext sportcontext, StudentAttendanceReportContext db)
        {
            _sportcontext = sportcontext;
            _db = db;
        }


        public SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data)//int IVRMM_Id
        {

            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }

        public SportStudentParticipationReportDTO showdetails(SportStudentParticipationReportDTO dto)
        {

            try
            {
                List<long> amstids = new List<long>();
                foreach (SportStudentParticipationReportDTO item in dto.StudentList)
                {
                    amstids.Add(item.AMST_Id);
                }

                dto.viewlist = (from a in _sportcontext.admissionyearstudent
                                from b in _sportcontext.admissionStduent
                                from c in _sportcontext.admissionClass
                                from d in _db.masterSection

                                where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && c.ASMCL_ActiveFlag == true && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == d.ASMS_Id && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && b.AMST_SOL=="S" && amstids.Contains(a.AMST_Id))
                                select new SportStudentParticipationReportDTO
                                {
                                    AMST_Id = b.AMST_Id,
                                    adm_no = b.AMST_AdmNo,
                                    studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                    classname = c.ASMCL_ClassName,
                                    sectionname = d.ASMC_SectionName,
                                    SPCCE_StartDate = b.AMST_DOB,
                                    AMST_MotherName = b.AMST_MotherName,
                                    AMST_FatherName = b.AMST_FatherName,
                                    Address = b.AMST_PerStreet + (string.IsNullOrEmpty(b.AMST_PerArea) || b.AMST_PerArea == "0" ? "" : ' ' + b.AMST_PerArea) + (string.IsNullOrEmpty(b.AMST_PerAdd3) || b.AMST_PerAdd3 == "0" ? "" : ' ' + b.AMST_PerAdd3),
                                    PASR_Age = b.PASR_Age,

                                }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return dto;
        }

        public SportStudentParticipationReportDTO get_class(SportStudentParticipationReportDTO dto)
        {
            try
            {
                dto.ClassList = (from a in _db.admissionyearstudent
                                 from b in _db.admissionClass
                                 where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id)
                                 select b).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public SportStudentParticipationReportDTO get_section(SportStudentParticipationReportDTO dto)
        {
            try
            {
                dto.SectionList = (from a in _db.admissionyearstudent
                                   from b in _db.masterSection

                                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                   select b).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public SportStudentParticipationReportDTO get_student(SportStudentParticipationReportDTO dto)
        {

            try
            {
                dto.StudentList1 = (from a in _sportcontext.admissionyearstudent
                                    from b in _sportcontext.admissionStduent

                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && b.AMST_SOL=="S" && a.AMAY_ActiveFlag==1)
                                    select new SportStudentParticipationReportDTO.Ostudent
                                    {
                                        AMST_Id = b.AMST_Id,
                                        studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),

                                    }
                                   ).Distinct().OrderBy(b => b.studentname).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
    }
}