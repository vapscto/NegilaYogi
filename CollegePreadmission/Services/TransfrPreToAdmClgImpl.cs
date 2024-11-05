using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CollegePreadmission.Services
{
    public class TransfrPreToAdmClgImpl : Interfaces.TransfrPreToAdmClgInterface
    {
        public ClgAdmissionContext _context;
        CollegepreadmissionContext _precontext;
        private readonly DomainModelMsSqlServerContext _db;
        private readonly UserManager<ApplicationUser> _UserManager;
        ILogger<TransfrPreToAdmClgImpl> _log;

        public TransfrPreToAdmClgImpl(CollegepreadmissionContext precontext, DomainModelMsSqlServerContext db, ClgAdmissionContext context, UserManager<ApplicationUser> UserManager)
        {
            _precontext = precontext;
            _context = context;
            _db = db;
            _UserManager = UserManager;
        }
        public TransfrPreToAdmDTO onloadgetdetails(TransfrPreToAdmDTO dto)
        {

            //List<MasterCourseDMO> courselist = new List<MasterCourseDMO>();
            //List<ClgMasterBranchDMO> branchlist = new List<ClgMasterBranchDMO>();
            //List<CLG_Adm_Master_SemesterDMO> semesterlist = new List<CLG_Adm_Master_SemesterDMO>();


            try
            {
                //GroupTypelist
                dto.courselist = _precontext.MasterCourseDMO.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();
                dto.yearlist = _precontext.AcademicYear.Where(y => y.MI_Id == dto.MI_Id).OrderByDescending(d => d.ASMAY_Order).ToArray();
                //courselist = _precontext.MasterCourseDMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                //dto.groupTypedropdown = GroupTypelist.ToArray();

                ////Departmentlist
                //branchlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                //dto.departmentdropdown = Departmentlist.ToArray();

                ////Designationlist
                //semesterlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                //dto.designationdropdown = Designationlist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public TransfrPreToAdmDTO get_branchs(TransfrPreToAdmDTO data)
        {
            try
            {

                List<long> courseids = new List<long>();
                foreach (var item in data.courselistarray)
                {
                    courseids.Add(item.AMCO_Id);
                }
                data.branchlist = (from a in _precontext.Adm_Course_Branch_MappingDMO
                                   from b in _precontext.ClgMasterBranchDMO
                                   where (a.AMB_Id == b.AMB_Id && a.MI_Id == data.MI_Id && b.AMB_ActiveFlag == true && courseids.Contains(a.AMCO_Id))
                                   select new TransfrPreToAdmDTO
                                   {
                                       AMB_Id = b.AMB_Id,
                                       AMB_BranchName = b.AMB_BranchName
                                   }).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public TransfrPreToAdmDTO get_semester(TransfrPreToAdmDTO data)
        {
            try
            {

                List<long> courseids = new List<long>();
                foreach (var item in data.courselistarray)
                {
                    courseids.Add(item.AMCO_Id);
                }
                List<long> branchids = new List<long>();
                foreach (var item in data.branchlistarray)
                {
                    branchids.Add(item.AMB_Id);
                }
                data.semesterlist = (from a in _precontext.Adm_Course_Branch_MappingDMO
                                     from c in _precontext.AdmCourseBranchSemesterMappingDMO
                                     from b in _precontext.CLG_Adm_Master_SemesterDMO
                                     where (a.MI_Id == data.MI_Id 
                                     && a.AMCOBM_Id==c.AMCOBM_Id
                                     && b.AMSE_ActiveFlg == true && courseids.Contains(a.AMCO_Id) 
                                     && branchids.Contains(a.AMB_Id) && c.AMSE_Id == b.AMSE_Id)
                                     select new TransfrPreToAdmDTO
                                     {
                                         AMSE_Id = b.AMSE_Id,
                                         AMSE_SEMName = b.AMSE_SEMName
                                     }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public TransfrPreToAdmDTO getserdata(TransfrPreToAdmDTO data)
        {
            try
            {
                List<long> courseids = new List<long>();
                foreach (var item in data.courselistarray)
                {
                    courseids.Add(item.AMCO_Id);
                }
                List<long> branchids = new List<long>();
                foreach (var item in data.branchlistarray)
                {
                    branchids.Add(item.AMB_Id);
                }
                List<long> semesterds = new List<long>();
                foreach (var item in data.semesterlistarray)
                {
                    semesterds.Add(item.AMSE_Id);
                }
                //List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                var  mstConfig = _precontext.masterConfig.Where(d => d.MI_Id.Equals(data.MI_Id) && d.ASMAY_Id.Equals(data.ASMAY_Id)).ToList();
               // mstConfig = mstConfig.Where(a => a.MI_Id.Equals(data.MI_Id) && data.ASMAY_Id.Equals(data.ASMAY_Id)).ToList();

                if (data.courselistarray.Length > 0 && data.semesterlistarray.Length > 0 && data.branchlistarray.Length > 0)
                {
                    if (mstConfig.FirstOrDefault().ISPAC_ApplFeeFlag == 1)
                    {
                        if (mstConfig.FirstOrDefault().ISPAC_AdmissionTransfer == 1)
                        {

                            data.preAdmtoAdmStuList = (from a in _db.PA_College_Application
                                                       from b in _db.AdmissionStatus
                                                       from c in _db.MasterCourseDMO
                                                       from d in _db.ClgMasterBranchDMO
                                                       from e in _db.CLG_Adm_Master_SemesterDMO
                                                       from f in _db.Fee_Y_Payment_PA_Application
                                                       where (a.PACA_ActiveFlag == true && f.PACA_Id == a.PACA_Id && a.PACA_AdmStatus == b.PAMST_Id && a.AMCO_Id == c.AMCO_Id && d.AMB_Id == a.AMB_Id
                                                       && e.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && b.PAMST_StatusFlag == "CNF" && f.FYPPA_Type == "R" && a.ASMAY_Id == data.ASMAY_Id && courseids.Contains(a.AMCO_Id) && branchids.Contains(a.AMB_Id) && semesterds.Contains(a.AMSE_Id))
                                                       select new TransfrPreToAdmDTO
                                                       {
                                                           PACA_Id = a.PACA_Id,
                                                           studentname = a.PACA_FirstName,
                                                           AMB_BranchName = d.AMB_BranchName,
                                                           AMSE_SEMName = e.AMSE_SEMName,
                                                           AMCO_CourseName = c.AMCO_CourseName
                                                       }
                        ).ToArray();

                            // data.preAdmtoAdmStuList = (from a in _db.PA_College_Application
                            //                            from b in _db.AdmissionStatus
                            //                            from c in _db.MasterCourseDMO
                            //                            from d in _db.ClgMasterBranchDMO
                            //                            from e in _db.CLG_Adm_Master_SemesterDMO

                            //                            where (a.PACA_ActiveFlag == true && a.PACA_AdmStatus == b.PAMST_Id && a.AMCO_Id == c.AMCO_Id && d.AMB_Id == a.AMB_Id
                            //                            && e.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && b.PAMST_StatusFlag == "CNF" && a.ASMAY_Id == data.ASMAY_Id && courseids.Contains(a.AMCO_Id) && branchids.Contains(a.AMB_Id) && semesterds.Contains(a.AMSE_Id))
                            //                            select new TransfrPreToAdmDTO
                            //                            {
                            //                                PACA_Id = a.PACA_Id,
                            //                                studentname = a.PACA_FirstName,
                            //                                AMB_BranchName = d.AMB_BranchName,
                            //                                AMSE_SEMName = e.AMSE_SEMName,
                            //                                AMCO_CourseName = c.AMCO_CourseName
                            //                            }
                            //).ToArray();
                        }
                        else
                        {
                            data.preAdmtoAdmStuList = (from a in _db.PA_College_Application
                                                       from b in _db.AdmissionStatus
                                                       from c in _db.MasterCourseDMO
                                                       from d in _db.ClgMasterBranchDMO
                                                       from e in _db.CLG_Adm_Master_SemesterDMO
                                                       where (a.PACA_ActiveFlag == true && a.PACA_AdmStatus == b.PAMST_Id && a.AMCO_Id == c.AMCO_Id && d.AMB_Id == a.AMB_Id
                                                       && e.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && b.PAMST_StatusFlag == "CNF" && a.ASMAY_Id == data.ASMAY_Id && courseids.Contains(a.AMCO_Id) && branchids.Contains(a.AMB_Id) && semesterds.Contains(a.AMSE_Id))
                                                       select new TransfrPreToAdmDTO
                                                       {
                                                           PACA_Id = a.PACA_Id,
                                                           studentname = a.PACA_FirstName,
                                                           AMB_BranchName = d.AMB_BranchName,
                                                           AMSE_SEMName = e.AMSE_SEMName,
                                                           AMCO_CourseName = c.AMCO_CourseName
                                                       }
                              ).ToArray();
                        }
                    }
                    else if (mstConfig.FirstOrDefault().ISPAC_ApplFeeFlag == 0)
                    {

                        if (mstConfig.FirstOrDefault().ISPAC_AdmissionTransfer == 0)
                        {

                            data.preAdmtoAdmStuList = (from a in _db.PA_College_Application
                                                       from b in _db.AdmissionStatus
                                                       from c in _db.MasterCourseDMO
                                                       from d in _db.ClgMasterBranchDMO
                                                       from e in _db.CLG_Adm_Master_SemesterDMO
                                                       where (a.PACA_ActiveFlag == true && a.PACA_AdmStatus == b.PAMST_Id && a.AMCO_Id == c.AMCO_Id && d.AMB_Id == a.AMB_Id
                                                       && e.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && b.PAMST_StatusFlag == "CNF" && a.ASMAY_Id == data.ASMAY_Id && courseids.Contains(a.AMCO_Id) && branchids.Contains(a.AMB_Id) && semesterds.Contains(a.AMSE_Id))
                                                       select new TransfrPreToAdmDTO
                                                       {
                                                           PACA_Id = a.PACA_Id,
                                                           studentname = a.PACA_FirstName,
                                                           AMB_BranchName = d.AMB_BranchName,
                                                           AMSE_SEMName = e.AMSE_SEMName,
                                                           AMCO_CourseName = c.AMCO_CourseName
                                                       }
                            ).ToArray();
                        }
                        else
                        {
                            data.preAdmtoAdmStuList = (from a in _db.PA_College_Application
                                                       from b in _db.AdmissionStatus
                                                       from c in _db.MasterCourseDMO
                                                       from d in _db.ClgMasterBranchDMO
                                                       from e in _db.CLG_Adm_Master_SemesterDMO
                                                       where (a.PACA_ActiveFlag == true && a.PACA_AdmStatus == b.PAMST_Id && a.AMCO_Id == c.AMCO_Id && d.AMB_Id == a.AMB_Id
                                                       && e.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && b.PAMST_StatusFlag == "CNF" && a.ASMAY_Id == data.ASMAY_Id && courseids.Contains(a.AMCO_Id) && branchids.Contains(a.AMB_Id) && semesterds.Contains(a.AMSE_Id))
                                                       select new TransfrPreToAdmDTO
                                                       {
                                                           PACA_Id = a.PACA_Id,
                                                           studentname = a.PACA_FirstName,
                                                           AMB_BranchName = d.AMB_BranchName,
                                                           AMSE_SEMName = e.AMSE_SEMName,
                                                           AMCO_CourseName = c.AMCO_CourseName
                                                       }
                           ).ToArray();
                        }


                    }

                }
               
                // }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public async Task<TransfrPreToAdmDTO> expoadmi(TransfrPreToAdmDTO data)
        // public Adm_M_StudentDTO expoadmi(Adm_M_StudentDTO data)
        {
            int j = 0;
            data.returnMsg = "";
            try
            {
                if (data.studentdetails.Count() > 0)
                {
                    data.studentdetails = data.studentdetails.OrderBy(t => t.Name).ToList();
                }
                while (j < data.studentdetails.Count())
                {

                    if (data.configurationsettings.ISPAC_ApplFeeFlag == 1)
                    {
                        if (data.configurationsettings.ISPAC_AdmissionTransfer == 1)
                        {

                            var studDet = _db.PA_College_Application.Where(t => t.MI_Id == data.MI_Id && t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();

                            var usr = data.userid;
                            var confirmstatusadmission = _db.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission_college @p0,@p1,@p2,@p3", data.studentdetails[j].PACA_Id, Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id), data.MI_Id, data.userid);


                            if (confirmstatusadmission > 0)
                            {
                                var getstudentamstid = _context.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();
                                if (getstudentamstid.Count() > 0)
                                {
                                    //int kk = 0;
                                    //if (data.AMCST_IDarray != null && data.AMCST_IDarray.Length > 0)
                                    //{
                                    //    for (kk = 0; kk < data.AMCST_IDarray.Length; kk++)
                                    //    {
                                    //        data.AMCST_IDarray[kk].AMCST_Id = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();
                                    //    }

                                    //}
                                    //else
                                    //{
                                    //    data.AMCST_IDarray[kk].AMCST_Id = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();


                                    //}
                                    data.AMCST_IDuser = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();
                                    //data.returnval = true;
                                    //saveatt(data);
                                    data.returnval = true;

                                }
                                else
                                {
                                    data.returnval = false;
                                }

                                List<Adm_Master_College_StudentDMO> getcurr = new List<Adm_Master_College_StudentDMO>();
                                long mobileno = 0;
                                string MailId = "";
                                var getdetails = (from a in _db.PA_College_Application

                                                  where (a.PACA_Id == data.studentdetails[j].PACA_Id)
                                                  select new TransfrPreToAdmDTO
                                                  {
                                                      AMCST_MobileNo = a.PACA_MobileNo,
                                                      AMCST_emailId = a.PACA_emailId
                                                  }).ToList();

                                mobileno = getdetails.FirstOrDefault().AMCST_MobileNo;
                                MailId = getdetails.FirstOrDefault().AMCST_emailId;

                                var AdmstudDet = _context.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();

                                //SMS sms = new SMS(_db);
                                //string s = await sms.sendSms(data.MI_Id, mobileno, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMCST_Id));

                                //Email Email = new Email(_db);
                                //string m = Email.sendmail(data.MI_Id, MailId, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMCST_Id));
                            }
                            j++;
                        }
                        else if (data.configurationsettings.ISPAC_AdmissionTransfer == 0)
                        {
                            data.payementcheck = _precontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && t.PACA_Id == data.studentdetails[j].PACA_Id).Count();
                            if (data.payementcheck != 0)
                            {
                                var studDet = _db.PA_College_Application.Where(t => t.MI_Id == data.MI_Id && t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();

                                var confirmstatusadmission = _db.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission_college @p0,@p1,@p2", data.studentdetails[j].PACA_Id, Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id), data.MI_Id);
                                if (confirmstatusadmission > 0)
                                {
                                    // var confirmstatus = 0;
                                    var getstudentamstid = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();
                                    if (getstudentamstid.Count() > 0)
                                    {
                                        //int kk = 0;
                                        //if (data.AMCST_IDarray != null && data.AMCST_IDarray.Length > 0)
                                        //{
                                        //    for (kk = 0; kk < data.AMCST_IDarray.Length; kk++)
                                        //    {
                                        //        data.AMCST_IDarray[kk].AMCST_Id = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();
                                        //    }

                                        //}
                                        //else
                                        //{
                                        //    data.AMCST_IDarray[0].AMCST_Id = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();


                                        //}
                                        data.AMCST_IDuser = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();

                                       // saveatt(data);
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                    }


                                    List<Adm_Master_College_StudentDMO> getcurr = new List<Adm_Master_College_StudentDMO>();
                                    long mobileno = 0;
                                    string MailId = "";

                                    var getdetails = (from a in _db.PA_College_Application
                                                      where (a.PACA_Id == data.studentdetails[j].PACA_Id)
                                                      select new TransfrPreToAdmDTO
                                                      {
                                                          AMCST_MobileNo = a.PACA_MobileNo,
                                                          AMCST_emailId = a.PACA_emailId
                                                      }).ToList();

                                    mobileno = getdetails.FirstOrDefault().AMCST_MobileNo;
                                    MailId = getdetails.FirstOrDefault().AMCST_emailId;

                                    var AdmstudDet = _db.Adm_Master_Student_PA.Where(t => t.PASR_Id == data.studentdetails[j].PACA_Id).ToList();

                                    //SMS sms = new SMS(_db);
                                    //string s = await sms.sendSms(data.MI_Id, mobileno, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));

                                    //Email Email = new Email(_db);
                                    //string m = Email.sendmail(data.MI_Id, MailId, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));

                                }
                                j++;
                            }
                            else
                            {
                                data.returnMsg = "Student Payment is not done";
                                return data;
                            }
                        }
                    }
                    else
                    {
                        if (data.configurationsettings.ISPAC_AdmissionTransfer == 1)
                        {

                            var studDet = _db.PA_College_Application.Where(t => t.MI_Id == data.MI_Id && t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();

                            var usr = data.userid;
                            var confirmstatusadmission = _db.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission_college @p0,@p1,@p2,@p3", data.studentdetails[j].PACA_Id, Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id), data.MI_Id, data.userid);


                            if (confirmstatusadmission > 0)
                            {
                                var getstudentamstid = _context.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();
                                if (getstudentamstid.Count() > 0)
                                {
                                    //int kk = 0;
                                    //if (data.AMCST_IDarray != null && data.AMCST_IDarray.Length > 0)
                                    //{
                                    //    for (kk = 0; kk < data.AMCST_IDarray.Length; kk++)
                                    //    {
                                    //        data.AMCST_IDarray[kk].AMCST_Id = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();
                                    //    }

                                    //}
                                    //else
                                    //{
                                    //    data.AMCST_IDarray[kk].AMCST_Id = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();


                                    //}
                                    data.AMCST_IDuser = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();
                                    //data.returnval = true;
                                    //saveatt(data);
                                    data.returnval = true;

                                }
                                else
                                {
                                    data.returnval = false;
                                }

                                List<Adm_Master_College_StudentDMO> getcurr = new List<Adm_Master_College_StudentDMO>();
                                long mobileno = 0;
                                string MailId = "";
                                var getdetails = (from a in _db.PA_College_Application

                                                  where (a.PACA_Id == data.studentdetails[j].PACA_Id)
                                                  select new TransfrPreToAdmDTO
                                                  {
                                                      AMCST_MobileNo = a.PACA_MobileNo,
                                                      AMCST_emailId = a.PACA_emailId
                                                  }).ToList();

                                mobileno = getdetails.FirstOrDefault().AMCST_MobileNo;
                                MailId = getdetails.FirstOrDefault().AMCST_emailId;

                                var AdmstudDet = _context.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();

                                //SMS sms = new SMS(_db);
                                //string s = await sms.sendSms(data.MI_Id, mobileno, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMCST_Id));

                                //Email Email = new Email(_db);
                                //string m = Email.sendmail(data.MI_Id, MailId, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMCST_Id));
                            }
                            j++;
                        }
                        else if (data.configurationsettings.ISPAC_AdmissionTransfer == 0)
                        {
                            data.payementcheck = _precontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && t.PACA_Id == data.studentdetails[j].PACA_Id).Count();
                            if (data.payementcheck != 0)
                            {
                                var studDet = _db.PA_College_Application.Where(t => t.MI_Id == data.MI_Id && t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();

                                var confirmstatusadmission = _db.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission_college @p0,@p1,@p2", data.studentdetails[j].PACA_Id, Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id), data.MI_Id);
                                if (confirmstatusadmission > 0)
                                {
                                    // var confirmstatus = 0;
                                    var getstudentamstid = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();
                                    if (getstudentamstid.Count() > 0)
                                    {
                                        //int kk = 0;
                                        //if (data.AMCST_IDarray != null && data.AMCST_IDarray.Length > 0)
                                        //{
                                        //    for (kk = 0; kk < data.AMCST_IDarray.Length; kk++)
                                        //    {
                                        //        data.AMCST_IDarray[kk].AMCST_Id = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();
                                        //    }

                                        //}
                                        //else
                                        //{
                                        //    data.AMCST_IDarray[0].AMCST_Id = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();


                                        //}
                                        data.AMCST_IDuser = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).Select(a => a.AMCST_Id).FirstOrDefault();

                                       // saveatt(data);
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                    }


                                    List<Adm_Master_College_StudentDMO> getcurr = new List<Adm_Master_College_StudentDMO>();
                                    long mobileno = 0;
                                    string MailId = "";

                                    var getdetails = (from a in _db.PA_College_Application
                                                      where (a.PACA_Id == data.studentdetails[j].PACA_Id)
                                                      select new TransfrPreToAdmDTO
                                                      {
                                                          AMCST_MobileNo = a.PACA_MobileNo,
                                                          AMCST_emailId = a.PACA_emailId
                                                      }).ToList();

                                    mobileno = getdetails.FirstOrDefault().AMCST_MobileNo;
                                    MailId = getdetails.FirstOrDefault().AMCST_emailId;

                                    var AdmstudDet = _db.Adm_Master_Student_PA.Where(t => t.PASR_Id == data.studentdetails[j].PACA_Id).ToList();

                                    //SMS sms = new SMS(_db);
                                    //string s = await sms.sendSms(data.MI_Id, mobileno, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));

                                    //Email Email = new Email(_db);
                                    //string m = Email.sendmail(data.MI_Id, MailId, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));

                                }
                                j++;
                            }
                            else
                            {
                                data.returnMsg = "Student Payment is not done";
                                return data;
                            }
                        }
                    }

                }
                data.returnMsg = "true";
            }
            catch (Exception e)
            {
                data.returnMsg = "false";
                Console.WriteLine(e.Message);
                return data;
            }
            return data;
        }
        public TransfrPreToAdmDTO saveatt(TransfrPreToAdmDTO data)
        {
            int sucesscount = 0;
            int failcount = 0;
            string failedmsg = "";
            string otpadmno = "";
            string admno = "";

            try
            {
                var checkotporadm = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).ToList();

                var virtualcode = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).Select(t => t.ivrm_school_code).FirstOrDefault();

                otpadmno = checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO;
                //  var getstudentamstid = _db.Adm_Master_College_Student_PA_DMO.Where(t => t.PACA_Id == data.studentdetails[j].PACA_Id).ToList();
                data.Studenttype = "Student";
                //if (data.Temp_Student.Length > 0)
                if (data.AMCST_IDuser > 0)

                {
                    try
                    {
                        if (data.Studenttype == "Student")
                        {
                            if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "Admno")
                            {
                                // for (int kk = 0; kk < data.AMCST_IDuser; kk++)
                                // {
                                try
                                {
                                    var AMCST_Id = data.AMCST_IDuser;
                                    var checkstudent = (from a in _db.CollegeStudentlogin
                                                        where a.AMCST_Id == AMCST_Id
                                                        select new PreadmissionDTOs.com.vaps.College.Admission.CollegeUsernameCreationDTO
                                                        {
                                                            AMCST_Id = a.AMCST_Id
                                                        }).ToList();
                                    if (checkstudent.Count() == 0)
                                    {
                                        string studotp = otpadmno;
                                        var studDet = _db.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == AMCST_Id).ToList();
                                        admno = studDet.FirstOrDefault().AMCST_AdmNo;
                                        long stduserid = 0;
                                        long fatuserid = 0;
                                        long motuserid = 0;
                                        string res = "";

                                        Dictionary<string, long> temp = new Dictionary<string, long>();

                                        if (studDet.FirstOrDefault().AMCST_emailId != "" && studDet.FirstOrDefault().AMCST_emailId != null)
                                        {
                                            string StudentName = virtualcode + "S" + admno.ToString();

                                            CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMCST_MobileNo.ToString()).Result;
                                            stduserid = response.useridapp;
                                            res = response.resp;
                                            if (stduserid == 0)
                                            {
                                                StudentName = virtualcode + "S" + admno.ToString();

                                                CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMCST_MobileNo.ToString()).Result;
                                                stduserid = response1.useridapp;
                                                res = response1.resp;
                                                temp.Add("studentid", stduserid);
                                            }
                                            else
                                            {
                                                temp.Add("studentid", stduserid);

                                            }
                                            bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMCST_Id);
                                            if (res == "Success" && val == true)
                                            {
                                            }

                                        }
                                        else
                                        {
                                            temp.Add("studentid", 0);
                                        }

                                        if (temp.Count != 0)
                                        {
                                            long uid = 0;
                                            if (temp["studentid"] != 0)
                                            {
                                                uid = temp["studentid"];
                                            }
                                            bool vall = false;
                                          //  vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "S");
                                            if (vall == true)
                                            {
                                                sucesscount = sucesscount + 1;
                                            }
                                            else
                                            {
                                                failcount = failcount + 1;
                                            }
                                        }

                                        if (studDet.FirstOrDefault().AMCST_FatheremailId != "" && studDet.FirstOrDefault().AMCST_FatheremailId != null)
                                        {
                                            string fathrotp = admno;

                                            string fathName = virtualcode + "F" + fathrotp.ToString();

                                            fathName = Regex.Replace(fathName, @"\s+", "");
                                            if (studDet.FirstOrDefault().AMCST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMCST_FatherMobleNo.ToString() != "")
                                            {
                                                data.AMCST_FatherMobleNo = studDet.FirstOrDefault().AMCST_FatherMobleNo;
                                            }
                                            else
                                            {
                                                data.AMCST_FatherMobleNo = 0;
                                            }
                                            CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_FatherMobleNo.ToString()).Result;
                                            fatuserid = response.useridapp;
                                            res = response.resp;
                                            if (fatuserid == 0)
                                            {
                                                fathName = virtualcode + "F" + fathrotp.ToString();

                                                CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_FatherMobleNo.ToString()).Result;
                                                fatuserid = response1.useridapp;
                                                res = response1.resp;
                                                temp.Add("Fatherid", fatuserid);
                                            }
                                            else
                                            {
                                                temp.Add("Fatherid", fatuserid);
                                            }
                                            bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMCST_Id);
                                            if (res == "Success" && val == true)
                                            {
                                            }
                                            fathrotp = "";
                                        }
                                        else
                                        {
                                            temp.Add("Fatherid", 0);
                                        }

                                        if (temp.Count != 0)
                                        {
                                            long uid = 0;
                                            if (temp["Fatherid"] != 0)
                                            {
                                                uid = temp["Fatherid"];
                                            }
                                            bool vall = false;
                                            // vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "F");
                                            if (vall == true)
                                            {
                                                sucesscount = sucesscount + 1;
                                            }
                                            else
                                            {
                                                failcount = failcount + 1;
                                            }
                                        }



                                        if (studDet.FirstOrDefault().AMCST_MotheremailId != "" && studDet.FirstOrDefault().AMCST_MotheremailId != null)
                                        {
                                            string motherotp = admno;
                                            string MotherName = virtualcode + "M" + motherotp.ToString();
                                            MotherName = Regex.Replace(MotherName, @"\s+", "");
                                            if (studDet.FirstOrDefault().AMCST_MotherMobleNo.ToString() != null && studDet.FirstOrDefault().AMCST_MotherMobleNo.ToString() != "")
                                            {
                                                data.AMCST_MotherMobleNo = studDet.FirstOrDefault().AMCST_MotherMobleNo;
                                            }
                                            else
                                            {
                                                data.AMCST_MotherMobleNo = 0;
                                            }
                                            CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_MotheremailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_MotherMobleNo.ToString()).Result;
                                            motuserid = response.useridapp;
                                            res = response.resp;
                                            if (motuserid == 0)
                                            {
                                                MotherName = virtualcode + "M" + motherotp.ToString();
                                                CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_MotheremailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_MotherMobleNo.ToString()).Result;
                                                motuserid = response1.useridapp;
                                                res = response1.resp;
                                                temp.Add("motherid", motuserid);
                                            }
                                            else
                                            {
                                                temp.Add("motherid", motuserid);
                                            }
                                            bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMCST_Id);
                                            if (res == "Success" && val == true)
                                            {
                                            }
                                            motherotp = "";
                                        }
                                        else
                                        {
                                            temp.Add("motherid", 0);
                                        }

                                        if (temp.Count != 0)
                                        {
                                            long uid = 0;
                                            if (temp["motherid"] != 0)
                                            {
                                                uid = temp["motherid"];
                                            }
                                            bool vall = false;
                                            // vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "M");
                                            if (vall == true)
                                            {
                                                sucesscount = sucesscount + 1;
                                            }
                                            else
                                            {
                                                failcount = failcount + 1;
                                            }
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                                catch (Exception ex)
                                {
                                    failedmsg += "," + admno;
                                    failcount = failcount + 1;
                                    Console.WriteLine(ex.Message);
                                    // continue;
                                }
                                // }
                            }

                            else if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "OTP")
                            {
                                //  for (int kk = 0; kk < data.AMCST_IDarray.Length; kk++)
                                //   {
                                try
                                {
                                    var AMCST_Id = data.AMCST_IDuser;
                                    var checkstudent = (from a in _db.CollegeStudentlogin
                                                        where a.AMCST_Id == AMCST_Id
                                                        select new PreadmissionDTOs.com.vaps.College.Admission.CollegeUsernameCreationDTO
                                                        {
                                                            AMCST_Id = a.AMCST_Id
                                                        }).ToList();
                                    if (checkstudent.Count() == 0)
                                    {
                                        generateOTP otp = new generateOTP();
                                        string studotp = otp.GeneratePassword();

                                        var studDet = _db.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == AMCST_Id).ToList();
                                        admno = studDet.FirstOrDefault().AMCST_AdmNo;
                                        long stduserid = 0;
                                        long fatuserid = 0;
                                        long motuserid = 0;
                                        string res = "";
                                        Dictionary<string, long> temp = new Dictionary<string, long>();

                                        if (studDet.FirstOrDefault().AMCST_emailId != "" && studDet.FirstOrDefault().AMCST_emailId != null)
                                        {
                                            string StudentName = virtualcode + "S" + studotp.ToString();

                                            CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMCST_MobileNo.ToString()).Result;
                                            stduserid = response.useridapp;
                                            res = response.resp;
                                            if (stduserid == 0)
                                            {
                                                studotp = otp.GeneratePassword();
                                                StudentName = virtualcode + "S" + studotp.ToString();
                                                CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMCST_MobileNo.ToString()).Result;
                                                stduserid = response1.useridapp;
                                                res = response1.resp;
                                                temp.Add("studentid", stduserid);
                                            }
                                            else
                                            {
                                                temp.Add("studentid", stduserid);

                                            }
                                            bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMCST_Id);
                                            if (res == "Success" && val == true)
                                            {
                                            }

                                        }
                                        else
                                        {
                                            temp.Add("studentid", 0);
                                        }

                                        if (temp.Count != 0)
                                        {
                                            long uid = 0;
                                            if (temp["studentid"] != 0)
                                            {
                                                uid = temp["studentid"];
                                            }
                                            bool vall = false;
                                            // vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "S");
                                            if (vall == true)
                                            {
                                                sucesscount = sucesscount + 1;
                                            }
                                            else
                                            {
                                                failcount = failcount + 1;
                                            }
                                        }


                                        if (studDet.FirstOrDefault().AMCST_FatheremailId != "" && studDet.FirstOrDefault().AMCST_FatheremailId != null)
                                        {
                                            string fathrotp = studotp;
                                            string fathName = virtualcode + "F" + fathrotp.ToString();
                                            fathName = Regex.Replace(fathName, @"\s+", "");
                                            if (studDet.FirstOrDefault().AMCST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMCST_FatherMobleNo.ToString() != "")
                                            {
                                                data.AMCST_FatherMobleNo = studDet.FirstOrDefault().AMCST_FatherMobleNo;
                                            }
                                            else
                                            {
                                                data.AMCST_FatherMobleNo = 0;
                                            }
                                            CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_FatherMobleNo.ToString()).Result;
                                            fatuserid = response.useridapp;
                                            res = response.resp;
                                            if (fatuserid == 0)
                                            {
                                                fathrotp = otp.GeneratePassword();
                                                fathName = virtualcode + "F" + fathrotp.ToString();
                                                CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_FatherMobleNo.ToString()).Result;
                                                fatuserid = response1.useridapp;
                                                res = response1.resp;
                                                temp.Add("Fatherid", fatuserid);
                                            }
                                            else
                                            {
                                                temp.Add("Fatherid", fatuserid);
                                            }
                                            bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMCST_Id);
                                            if (res == "Success" && val == true)
                                            {
                                            }
                                            fathrotp = "";
                                        }
                                        else
                                        {
                                            temp.Add("Fatherid", 0);
                                        }

                                        if (temp.Count != 0)
                                        {
                                            long uid = 0;
                                            if (temp["Fatherid"] != 0)
                                            {
                                                uid = temp["Fatherid"];
                                            }
                                            bool vall = false;
                                            // vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "F");
                                            if (vall == true)
                                            {
                                                sucesscount = sucesscount + 1;
                                            }
                                            else
                                            {
                                                failcount = failcount + 1;
                                            }
                                        }

                                        if (studDet.FirstOrDefault().AMCST_MotheremailId != "" && studDet.FirstOrDefault().AMCST_MotheremailId != null)
                                        {
                                            string motherotp = studotp;
                                            string MotherName = virtualcode + "M" + motherotp.ToString();
                                            MotherName = System.Text.RegularExpressions.Regex.Replace(MotherName, @"\s+", "");
                                            if (studDet.FirstOrDefault().AMCST_MotherMobleNo.ToString() != null && studDet.FirstOrDefault().AMCST_MotherMobleNo.ToString() != "")
                                            {
                                                data.AMCST_MotherMobleNo = studDet.FirstOrDefault().AMCST_MotherMobleNo;
                                            }
                                            else
                                            {
                                                data.AMCST_MotherMobleNo = 0;
                                            }
                                            CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_MotheremailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_MotherMobleNo.ToString()).Result;
                                            motuserid = response.useridapp;
                                            res = response.resp;
                                            if (motuserid == 0)
                                            {
                                                motherotp = otp.GeneratePassword_Mother();
                                                MotherName = virtualcode + "M" + motherotp.ToString();
                                                CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_MotheremailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_MotherMobleNo.ToString()).Result;
                                                motuserid = response1.useridapp;
                                                res = response1.resp;
                                                temp.Add("motherid", motuserid);
                                            }
                                            else
                                            {
                                                temp.Add("motherid", motuserid);
                                            }
                                            bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMCST_Id);
                                            if (res == "Success" && val == true)
                                            {
                                            }
                                            motherotp = "";
                                        }
                                        else
                                        {
                                            temp.Add("motherid", 0);
                                        }
                                        if (temp.Count != 0)
                                        {
                                            long uid = 0;
                                            if (temp["motherid"] != 0)
                                            {
                                                uid = temp["motherid"];
                                            }
                                            bool vall = false;
                                            // vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "M");
                                            if (vall == true)
                                            {
                                                sucesscount = sucesscount + 1;
                                            }
                                            else
                                            {
                                                failcount = failcount + 1;
                                            }
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                                catch (Exception ex)
                                {
                                    failedmsg += "," + admno;
                                    failcount = failcount + 1;
                                    Console.WriteLine(ex.Message);
                                    //  continue;
                                }
                                //  }
                            }
                        }
                        else if (data.Studenttype == "Alumni")
                        {
                            //  for (int kk = 0; kk < data.AMCST_IDarray.Length; kk++)
                            //  {
                            try
                            {
                                var AMCST_Id = data.AMCST_IDuser;
                                var checkstudent = (from a in _db.CLGAlumniUserRegistrationDMO
                                                    where a.AMCST_Id == AMCST_Id
                                                    select new PreadmissionDTOs.com.vaps.College.Admission.CollegeUsernameCreationDTO
                                                    {
                                                        AMCST_Id = Convert.ToInt64(a.AMCST_Id)
                                                    }).ToList();
                                if (checkstudent.Count() == 0)
                                {
                                    generateOTP otp = new generateOTP();
                                    string studotp = otp.GeneratePassword();

                                    List<CLGAlumni_M_StudentDMO> studDet = new List<CLGAlumni_M_StudentDMO>();
                                    studDet = _db.CLGAlumni_M_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.ALCMST_Id == AMCST_Id).ToList();

                                    admno = studDet.FirstOrDefault().ALCMST_AdmNo;
                                    long stduserid = 0;
                                    //long fatuserid = 0;
                                    //long motuserid = 0;
                                    string res = "";
                                    Dictionary<string, long> temp = new Dictionary<string, long>();

                                    //if (studDet.FirstOrDefault().ALCMST_emailId != "" && studDet.FirstOrDefault().ALCMST_emailId != null)
                                    //{
                                    string StudentName = "ALUMNI" + studotp.ToString();

                                    CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().ALCMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALCMST_MobileNo.ToString()).Result;
                                    stduserid = response.useridapp;
                                    res = response.resp;
                                    if (stduserid == 0)
                                    {
                                        studotp = otp.GeneratePassword();
                                        StudentName = "ALUMNIA" + studotp.ToString();
                                        CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().ALCMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALCMST_MobileNo.ToString()).Result;
                                        stduserid = response1.useridapp;
                                        res = response1.resp;
                                        temp.Add("studentid", stduserid);
                                    }
                                    else
                                    {
                                        temp.Add("studentid", stduserid);

                                    }
                                    bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().ALCMST_Id);
                                    if (res == "Success" && val == true)
                                    {
                                    }

                                    //}
                                    //else
                                    //{
                                    //    temp.Add("studentid", 0);
                                    //}

                                    if (temp.Count != 0)
                                    {
                                        long uid = 0;
                                        if (temp["studentid"] != 0)
                                        {
                                            uid = temp["studentid"];
                                        }

                                        bool vall = AddAlumniStudentlogin(uid, studDet.FirstOrDefault().ALCMST_Id, "S", studDet);
                                        if (vall == true)
                                        {
                                            sucesscount = sucesscount + 1;
                                        }
                                        else
                                        {
                                            failcount = failcount + 1;
                                        }
                                    }

                                }
                                else
                                {

                                }
                            }
                            catch (Exception ex)
                            {
                                failedmsg += "," + admno;
                                failcount = failcount + 1;
                                Console.WriteLine(ex.Message);
                                // continue;
                            }
                            // }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        failcount = failcount + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("College User Creation : " + ex.Message);
            }
            if (sucesscount >0 && data.AMCST_IDuser>0)
            {
                data.msg = "Record Saved";
            }
            else
            {
                if (failedmsg != "")
                {
                    data.msg = "Failed Record " + admno + "";
                }
                else
                {
                    data.msg = "Record Saved";
                }
            }
            return data;
        }


        public bool AddStudentApplogin(long userid, long amstId, string appflag)
        {
            CollegeStudentlogin dmo = new CollegeStudentlogin();
            dmo.AMCST_Id = amstId;
            dmo.IVRMUL_Id = Convert.ToInt32(userid);
            dmo.IVRMULSPGC_ActiveFlag = true;
            dmo.IVRMULSPGC_Flag = appflag;
            _db.Add(dmo);
            var flag = _db.SaveChanges();
            if (flag > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddAlumniStudentlogin(long userid, long amstId, string appflag, List<CLGAlumni_M_StudentDMO> reg)
        {

            CLGAlumniUserRegistrationDMO Alumni = new CLGAlumniUserRegistrationDMO();

            Alumni.MI_Id = reg.FirstOrDefault().MI_Id;
            Alumni.AMCST_Id = amstId;
            Alumni.ALCSREG_Photo = reg.FirstOrDefault().ALCMST_StudentPhoto;
            Alumni.ALCSREG_ApprovedFlag = true;
            Alumni.ALCSREG_MemberName = reg.FirstOrDefault().ALCMST_FirstName;
            Alumni.ALCSREG_EmailId = reg.FirstOrDefault().ALCMST_emailId;
            Alumni.ALCSREG_MobileNo = Convert.ToInt64(reg.FirstOrDefault().ALCMST_MobileNo);
            Alumni.ALCSREG_AdmittedYear = Convert.ToInt64(reg.FirstOrDefault().ASMAY_Id_Join);
            Alumni.ALCSREG_LeftYear = Convert.ToInt64(reg.FirstOrDefault().ASMAY_Id_Left);
            Alumni.ALCSREG_LeftCourse = Convert.ToInt64(reg.FirstOrDefault().AMCO_Left_Id);
            Alumni.ALCSREG_AdmittedCourse = Convert.ToInt64(reg.FirstOrDefault().AMCO_JOIN_Id);
            Alumni.ALCSREG_AdmisstedBranch = Convert.ToInt64(reg.FirstOrDefault().AMB_JOIN_Id);
            Alumni.ALCSREG_LeftBranch = Convert.ToInt64(reg.FirstOrDefault().AMB_Id_Left);
            Alumni.ALCSREG_AdmittedSemester = Convert.ToInt64(reg.FirstOrDefault().AMSE_Id_Left);
            Alumni.ALCSREG_LeftSemester = Convert.ToInt64(reg.FirstOrDefault().AMSE_JOIN_Id);
            Alumni.ALCSREG_Date = DateTime.Now;
            Alumni.CreatedDate = DateTime.Now;
            Alumni.UpdatedDate = DateTime.Now;
            Alumni.ALCSREG_CreatedBy = userid;
            Alumni.ALCSREG_UpdatedBy = userid;
            Alumni.ALCSREG_ActiveFlg = true;
            _db.Add(Alumni);
            _db.SaveChanges();

            CLGAlumni_User_LoginDMO dmo = new CLGAlumni_User_LoginDMO();
            dmo.ALCSREG_Id = Alumni.ALCSREG_Id;
            dmo.IVRMUL_Id = Convert.ToInt32(userid);
            _db.Add(dmo);
            var flag = _db.SaveChanges();
            if (flag > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddStudentUserLogin(long mi_id, string username, long amstId)
        {
            //  StudentUserLoginDMO dmo = new StudentUserLoginDMO();
            //dmo.AMST_Id = amstId;
            //dmo.CreatedDate = DateTime.Now;
            //dmo.IVRMSTUUL_ActiveFlag = 1;
            //dmo.IVRMSTUUL_Password = "Password@123";
            //dmo.IVRMSTUUL_UserName = username;
            //dmo.MI_Id = mi_id;
            //dmo.UpdatedDate = DateTime.Now;
            //_db.Add(dmo);
            //var flag = _db.SaveChanges();
            //if (flag > 0)
            //{
            //    StudentUserLogin_Institutionwise inst = new StudentUserLogin_Institutionwise();
            //    inst.AMST_Id = amstId;
            //    inst.CreatedDate = DateTime.Now;
            //    inst.IVRMSTUULI_ActiveFlag = 1;
            //    inst.IVRMSTUUL_Id = dmo.IVRMSTUUL_Id;
            //    inst.UpdatedDate = DateTime.Now;
            //    _db.Add(inst);
            //    var flag1 = _db.SaveChanges();
            //    if (flag1 > 0)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        public async Task<CollegeImportStudentWrapDTO> Createlogins(string emailid, string name, long mi_id, string roles, string roletype, string mobile)
        {
            CollegeImportStudentWrapDTO respdto = new CollegeImportStudentWrapDTO();
            //string resp = ""; 
            //Creating Student and parents login as well as Sending user name and password code starts.
            try
            {
                ApplicationUser user = new ApplicationUser();

                user = await _UserManager.FindByNameAsync(name);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = name, Email = emailid, PhoneNumber = mobile };
                    user.Entry_Date = DateTime.Now;
                    user.EmailConfirmed = true;
                    var result = await _UserManager.CreateAsync(user, "Password@123");
                    if (result.Succeeded)
                    {
                        // Student Roles
                        string studentRole = roles;
                        var id = _db.applicationRole.Single(d => d.Name == studentRole);
                        //

                        // Student Role Type
                        string studentRoleType = roletype;
                        var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                        //

                        // Save role
                        var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
                        role.CreatedDate = DateTime.Now;
                        role.UpdatedDate = DateTime.Now;
                        _db.appUserRole.Add(role);
                        _db.SaveChanges();
                        respdto.useridapp = role.UserId;
                        UserRoleWithInstituteDMO mas1 = new UserRoleWithInstituteDMO();
                        mas1.Id = user.Id;
                        mas1.MI_Id = mi_id;
                        mas1.Activeflag = 1;
                        _db.Add(mas1);
                        var res = _db.SaveChanges();
                        if (res > 0)
                        {
                            respdto.resp = "Success";
                        }
                        else
                        {
                            respdto.resp = "";
                        }

                    }
                    else
                    {
                        respdto.resp = result.Errors.FirstOrDefault().Description.ToString();
                    }
                }

            }
            catch (Exception e)
            {
                _log.LogInformation("Student User Creation College form error");
                _log.LogDebug(e.Message);
            }
            return respdto;

            //Creating Student and parents login as well as Sending user name and password code Ends.
        }
    }
}
