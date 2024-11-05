using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using AdmissionServiceHub.com.vaps.Interfaces;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SendSMSandMailsImpl: SendingSMSandMailsInterface
    {

        private readonly AdmissionRegisterContext _AdmissionRegisterContext;
        private readonly DomainModelMsSqlServerContext _db;

        public SendSMSandMailsImpl(AdmissionRegisterContext castecategoryContext, DomainModelMsSqlServerContext db)
        {
            _AdmissionRegisterContext = castecategoryContext;
            _db = db;
        }


        public CommonDTO getdetails(int mas)
        {
            CommonDTO data = new CommonDTO();
            try
            {
                List<AcademicYear> aya = new List<AcademicYear>();
                aya = _AdmissionRegisterContext.academicyr.ToList();
                data.yerlist = aya.ToArray();

                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _AdmissionRegisterContext.classs.ToList();
                data.clslist = aya1.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CommonDTO getdetailsstudentorstaff(CommonDTO mas)
        {
            try
            {
                if(mas.stustaffflag== "stu")
                {
                    mas.fillstudent = (from a in _AdmissionRegisterContext.Adm_M_Student
                                       where (a.MI_Id == mas.IVRM_MI_Id && a.ASMAY_Id == mas.acayearid && a.ASMCL_Id == mas.acaclsid && a.AMST_SOL == "S")
                                       select new CommonDTO
                                       {
                                           name = a.AMST_FirstName + a.AMST_MiddleName + a.AMST_LastName,
                                           smsmobileno = a.AMST_MobileNo,
                                           sendmailid = a.AMST_emailId
                                       }).ToArray();
                }
                else if (mas.stustaffflag == "staff")
                {
                    mas.fillstudent = (from a in _AdmissionRegisterContext.Employee
                                       where (a.MI_Id == mas.IVRM_MI_Id)
                                       select new CommonDTO
                                       {
                                           name = a.HRME_EmployeeFirstName,
                                           smsmobileno =Convert.ToInt32(a.HRME_MobileNo),
                                           sendmailid = a.HRME_EmailId
                                       }).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mas;
        }
    }
}