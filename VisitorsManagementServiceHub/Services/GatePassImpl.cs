using AutoMapper;
using CommonLibrary;
//using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;

namespace VisitorsManagementServiceHub.Services
{
    public class GatePassImpl : GatePassInterface
    {
        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _db;
        public GatePassImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext _db123)
        {
            visctxt = context;
            _db = _db123;
        }
        public GatePassDTO getDetails(GatePassDTO data)
        {
            try
            {
                //data.studentlist = (from a in visctxt.Adm_M_Student
                //                    from b in visctxt.School_Adm_Y_StudentDMO
                //                    from c in visctxt.admissionClass
                //                    from d in visctxt.masterSection
                //                    where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.AMST_Id == a.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id==data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag==1 && c.ASMCL_ActiveFlag==true && d.ASMC_ActiveFlag==1)
                //                    select new GatePassDTO
                //                    {
                //                        AMST_Id = a.AMST_Id,
                //                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                //                        AMST_AdmNo = a.AMST_AdmNo,
                //                        ASMCL_ClassName = c.ASMCL_ClassName,
                //                        ASMC_SectionName = d.ASMC_SectionName

                //                    }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();



                data.studentlist = (from a in visctxt.Adm_M_Student
                                    from b in visctxt.School_Adm_Y_StudentDMO
                                    from c in visctxt.admissionClass
                                    from d in visctxt.masterSection
                                    where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.AMST_Id == a.AMST_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1)
                                    select new GatePassDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                        ASMC_SectionName = d.ASMC_SectionName

                                    }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();


                data.employeelist = (from a in visctxt.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new GatePassDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                                     }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public GatePassDTO saveData(GatePassDTO data)
        {
            try
            {
                if (data.AGPH_Id == 0)
                {
                    var mapp = Mapper.Map<GatePassDMO>(data);
                    if (data.radiotype == "studentgp")
                    {

                        mapp.AGPH_PassType = "Student";
                        mapp.AGPH_Idno = data.AMST_Id;
                    }
                    else if (data.radiotype == "employeegp")
                    {
                        mapp.AGPH_PassType = "Employee";
                        mapp.AGPH_Idno = data.HRME_Id;
                    }
                    //string date = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    mapp.AGPH_Dategiven = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    mapp.AGPH_Remark = data.AGPH_Remark;
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;
                    visctxt.Add(mapp);
                    int s = visctxt.SaveChanges();
                    if (s > 0)
                    {
                        var result123 = visctxt.GatePassDMO.Max(t => t.AGPH_Id);
                        if (data.radiotype == "studentgp")
                        {
                            var asmay_ids = visctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_From_Date < DateTime.Now
                            && t.ASMAY_To_Date > DateTime.Now).Distinct().Select(i => i.ASMAY_Id).ToList();

                            data.student = (from a in visctxt.Adm_M_Student
                                            from c in visctxt.admissionClass
                                            from y in visctxt.School_Adm_Y_StudentDMO
                                            from d in visctxt.GatePassDMO
                                            where (y.ASMCL_Id == c.ASMCL_Id && y.AMST_Id == a.AMST_Id && a.AMST_Id == d.AGPH_Idno && asmay_ids.Contains(y.ASMAY_Id) && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_Id == data.AMST_Id /*&&  a.ASMCL_Id == c.ASMCL_Id*/ && d.AGPH_Idno == data.AMST_Id && d.AGPH_Id == result123)
                                            select new GatePassDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                AMST_MobileNo = a.AMST_MobileNo,
                                                Address = a.AMST_PerStreet + (string.IsNullOrEmpty(a.AMST_PerArea) || a.AMST_PerArea == "0" ? "" : ' ' + a.AMST_PerArea) + (string.IsNullOrEmpty(a.AMST_PerAdd3) || a.AMST_PerAdd3 == "0" ? "" : ' ' + a.AMST_PerAdd3),
                                                date = d.AGPH_Dategiven,
                                                remarks = d.AGPH_Remark

                                            }).Distinct().Take(1).ToArray();
                        }

                        else if (data.radiotype == "employeegp")
                        {
                            data.employ = (from a in visctxt.MasterEmployee
                                           from b in visctxt.HR_Master_Department
                                           from c in visctxt.HR_Master_Designation
                                           from d in visctxt.GatePassDMO
                                           from e in visctxt.Emp_MobileNo
                                           where (a.MI_Id == data.MI_Id && a.HRME_Id == e.HRME_Id && e.HRMEMNO_DeFaultFlag == "default" && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_Id == data.HRME_Id && d.AGPH_Idno == data.HRME_Id && d.AGPH_Id == result123)
                                           select new GatePassDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               HRMDES_DesignationName = c.HRMDES_DesignationName,
                                               HRME_MobileNo = e.HRMEMNO_MobileNo,
                                               date = d.AGPH_Dategiven,
                                               remarks = d.AGPH_Remark

                                           }).Distinct().Take(1).ToArray();
                        }
                        data.returnVal = "saved";
                        data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();

                    }
                    else
                    {
                        data.returnVal = "savingFailed";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public GatePassDTO getStudentDetails(GatePassDTO data)
        {
            try
            {
                //data.studDetails = (from a in visctxt.Adm_M_Student
                //                    from b in visctxt.School_Adm_Y_StudentDMO
                //                    from c in visctxt.admissionClass
                //                    from d in visctxt.masterSection
                //                    where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && a.AMST_Id == data.AMST_Id && a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id)
                //                    select new GatePassDTO
                //                    {
                //                        AMST_Id = a.AMST_Id,
                //                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                //                        ASMCL_ClassName = c.ASMCL_ClassName,
                //                        ASMC_SectionName = d.ASMC_SectionName,
                //                        datetime = DateTime.UtcNow.ToLocalTime(),
                //                        Address = a.AMST_PerStreet + (string.IsNullOrEmpty(a.AMST_PerArea) || a.AMST_PerArea == "0" ? "" : ' ' + a.AMST_PerArea) + (string.IsNullOrEmpty(a.AMST_PerAdd3) || a.AMST_PerAdd3 == "0" ? "" : ' ' + a.AMST_PerAdd3),
                //                        AMST_MobileNo = a.AMST_MobileNo,
                //                        AMST_emailId = a.AMST_emailId

                //                    }).Distinct().Take(1).ToArray();

                var asmay_ids = visctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_From_Date < DateTime.Now
                          && t.ASMAY_To_Date > DateTime.Now).Distinct().Select(i => i.ASMAY_Id).ToList();
                data.studDetails = (from a in visctxt.Adm_M_Student
                                    from c in visctxt.admissionClass
                                    from y in visctxt.School_Adm_Y_StudentDMO
                                    from s in visctxt.masterSection
                                    //from d in visctxt.GatePassDMO
                                    where (y.ASMCL_Id == c.ASMCL_Id && y.AMST_Id == a.AMST_Id && y.ASMS_Id == s.ASMS_Id /*&& a.AMST_Id == d.AGPH_Idno */&& asmay_ids.Contains(y.ASMAY_Id) && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_Id == data.AMST_Id /*&&  a.ASMCL_Id == c.ASMCL_Id*/ /*&& d.AGPH_Idno == data.AMST_Id*/ /*&& d.AGPH_Id == result123*/)
                                    select new GatePassDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                        AMST_MobileNo = a.AMST_MobileNo,
                                        ASMC_SectionName = s.ASMC_SectionName,
                                        Address = a.AMST_PerStreet + (string.IsNullOrEmpty(a.AMST_PerArea) || a.AMST_PerArea == "0" ? "" : ' ' + a.AMST_PerArea) + (string.IsNullOrEmpty(a.AMST_PerAdd3) || a.AMST_PerAdd3 == "0" ? "" : ' ' + a.AMST_PerAdd3),
                                        //date = d.AGPH_Dategiven,
                                        //remarks = d.AGPH_Remark,
                                        AMST_emailId = a.AMST_emailId

                                    }).Distinct().Take(1).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public GatePassDTO sendmail(GatePassDTO data)
        {
            try
            {
                var contactno1 = visctxt.Adm_M_Student.Single(t => t.AMST_Id == data.AMST_Id).AMST_FatherMobleNo;
                var contactno = contactno1.Value;
                var mailId = visctxt.Adm_M_Student.Single(t => t.AMST_Id == data.AMST_Id).AMST_FatheremailId;
                var fname = visctxt.Adm_M_Student.Single(t => t.AMST_Id == data.AMST_Id).AMST_FirstName;
                var mname = visctxt.Adm_M_Student.Single(t => t.AMST_Id == data.AMST_Id).AMST_MiddleName;
                var lname = visctxt.Adm_M_Student.Single(t => t.AMST_Id == data.AMST_Id).AMST_LastName;
                var admno = visctxt.Adm_M_Student.Single(t => t.AMST_Id == data.AMST_Id).AMST_FatheremailId;
                var Template = "StudentLateIn";

                SMS sms = new SMS(_db);
                sms.sendSms(data.MI_Id, contactno, Template, data.AMST_Id);

                Email Email = new Email(_db);
                Email.sendmail(data.MI_Id, mailId, Template, data.AMST_Id);
                data.returnVal = "saved";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}