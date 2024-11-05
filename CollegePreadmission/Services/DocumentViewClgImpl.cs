using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Preadmission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Services
{
    public class DocumentViewClgImpl:Interfaces.DocumentViewClgInterface
    {
        private static ConcurrentDictionary<string, CollegePreadmissionstudnetDto> _login =
      new ConcurrentDictionary<string, CollegePreadmissionstudnetDto>();
        public CollegepreadmissionContext _StudentApplicationContext;
        public ClgAdmissionContext _AcademicContext;
        ILogger<DocumentViewClgImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public CollFeeGroupContext _feecontext;
        public DocumentViewClgImpl(CollegepreadmissionContext StudentApplicationContext, CollFeeGroupContext feecontext, ClgAdmissionContext academiccontext, ILogger<DocumentViewClgImpl> acdimpl, DomainModelMsSqlServerContext db)
        {
            _StudentApplicationContext = StudentApplicationContext;
            _AcademicContext = academiccontext;
            _acdimpl = acdimpl;
            _db = db;
            _feecontext = feecontext;
        }
        public CollegePreadmissionstudnetDto getInitailData(CollegePreadmissionstudnetDto miid)
        {
            CollegePreadmissionstudnetDto ctdo = new CollegePreadmissionstudnetDto();
            try
            {
                ctdo.fillyear = _db.AcademicYear.Where(t => t.MI_Id == miid.MI_Id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToArray();

                ctdo.allcourse = _StudentApplicationContext.MasterCourseDMO.Where(c => c.MI_Id == miid.MI_Id && c.AMCO_ActiveFlag == true).ToArray();

            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public CollegePreadmissionstudnetDto getclgstudata(CollegePreadmissionstudnetDto miid)
        {
            CollegePreadmissionstudnetDto ctdo = new CollegePreadmissionstudnetDto();
            List<CollegePreadmissionstudnetDto> alldocList = new List<CollegePreadmissionstudnetDto>();
            List<CollegePreadmissionstudnetDto> alldocListmain = new List<CollegePreadmissionstudnetDto>();
            try
            {


                //
                List<long> amb_ids = new List<long>();
              
                if (miid.branchlisttwo.Length > 0)
                {
                    foreach (var ue in miid.branchlisttwo)
                    {
                        amb_ids.Add(ue.AMB_Id);
                    }

                }

                List<long> amco_ids = new List<long>();
                if (miid.courselsttwo.Length > 0)
                {
                    foreach (var ue in miid.courselsttwo)
                    {
                        amco_ids.Add(ue.AMCO_Id);
                    }

                }

                List<long> amse_ids = new List<long>();
                if (miid.semesterlisttwo.Length > 0)
                {
                    foreach (var ue in miid.semesterlisttwo)
                    {
                        amse_ids.Add(ue.AMSE_Id);
                    }

                }
                //

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _db.AcademicYear.Where(t => t.MI_Id == miid.MI_Id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                ctdo.fillyear = year.ToArray();

                ctdo.admissioncatdrp = _StudentApplicationContext.MasterCourseDMO.Where(c => c.MI_Id == miid.MI_Id && c.AMCO_ActiveFlag == true).ToArray();

                ctdo.admissioncatdrpall = _StudentApplicationContext.MasterCourseDMO.Where(c => c.MI_Id == miid.MI_Id && c.AMCO_ActiveFlag == true).ToArray();


                List<long> temparr = new List<long>();
                // flag ISPAC_ApplFeeFlag
                if (miid.configurationsettings1 == 1)
                {
                    List<PA_College_Application> allRegStudentmain = new List<PA_College_Application>();
                    List<PA_College_Application> allRegStudent = new List<PA_College_Application>();
                    //if (miid.AMCO_Id == 0)
                    //{

                    //    allRegStudent = _StudentApplicationContext.PA_College_Application.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id).ToList();
                    //}
                    //else
                    //{
                    //    allRegStudent = _StudentApplicationContext.PA_College_Application.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id && d.AMCO_Id == miid.AMCO_Id).ToList();
                    //}

                    //added
                    if (amco_ids.Count==0 && amb_ids.Count==0 && amse_ids.Count==0)
                    {

                        allRegStudent = _StudentApplicationContext.PA_College_Application.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id).ToList();
                    }
                    else
                    {
                        allRegStudent = _StudentApplicationContext.PA_College_Application.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id && amco_ids.Contains(d.AMCO_Id) && amb_ids.Contains(d.AMB_Id) && amse_ids.Contains(d.AMSE_Id)).ToList();
                    }

                    //

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id.Equals(miid.ASMAY_Id)).ToList();
                    for (int i = 0; i < allRegStudent.Count; i++)
                    {
                        temparr.Add(allRegStudent[i].PACA_Id);
                    }
                    if (mstConfig.FirstOrDefault().ISPAC_ApplFeeFlag == 1)
                    {
                        ctdo.studentDetailsHelth = (from b in _StudentApplicationContext.PA_College_Application
                                                    from c in _feecontext.Fee_Y_Payment_PA_Application
                                                    where (b.PACA_Id == c.PACA_Id && b.MI_Id == miid.MI_Id && b.ASMAY_Id == miid.ASMAY_Id && c.FYPPA_Type == "R" && temparr.Contains(b.PACA_Id))
                                                    select new CollegePreadmissionstudnetDto
                                                    {
                                                        PACA_Id = b.PACA_Id,
                                                        PACA_FirstName = b.PACA_FirstName,
                                                        PACA_MiddleName = b.PACA_MiddleName,
                                                        PACA_LastName = b.PACA_LastName,
                                                        PACA_RegistrationNo = b.PACA_RegistrationNo,
                                                        PACA_StudentPhoto = b.PACA_StudentPhoto,
                                                        AMCO_Id = b.AMCO_Id,
                                                        PACA_Date = b.PACA_Date

                                                    }
       ).Distinct().ToList().ToArray();
                        ctdo.registrationList = ctdo.studentDetailsHelth;
                        ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PACA_Id)).ToArray();
                    }
                    else
                    {
                        ctdo.studentDetailsHelth = (from b in _StudentApplicationContext.PA_College_Application
                                                    where ( b.MI_Id == miid.MI_Id && b.ASMAY_Id == miid.ASMAY_Id && temparr.Contains(b.PACA_Id))
                                                    select new CollegePreadmissionstudnetDto
                                                    {
                                                        PACA_Id = b.PACA_Id,
                                                        PACA_FirstName = b.PACA_FirstName,
                                                        PACA_MiddleName = b.PACA_MiddleName,
                                                        PACA_LastName = b.PACA_LastName,
                                                        PACA_RegistrationNo = b.PACA_RegistrationNo,
                                                        PACA_StudentPhoto = b.PACA_StudentPhoto,
                                                        AMCO_Id = b.AMCO_Id,
                                                        PACA_Date = b.PACA_Date

                                                    }
     ).Distinct().ToList().ToArray();
                        ctdo.registrationList = ctdo.studentDetailsHelth;
                        //ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PACA_Id)).ToArray();

                    }
        
                }
                else
                {
                    List<PA_College_Application> allRegStudentmain = new List<PA_College_Application>();
                    //if (miid.AMCO_Id == 0)
                    //{
                    //    allRegStudentmain = _StudentApplicationContext.PA_College_Application.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id).ToList();
                    //}
                    //else
                    //{
                    //    allRegStudentmain = _StudentApplicationContext.PA_College_Application.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id && d.AMCO_Id == miid.AMCO_Id).ToList();
                    //}
                    //added
                    if (amco_ids.Count == 0 && amb_ids.Count == 0 && amse_ids.Count == 0)
                    {

                        allRegStudentmain = _StudentApplicationContext.PA_College_Application.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id).ToList();
                    }
                    else
                    {
                        allRegStudentmain = _StudentApplicationContext.PA_College_Application.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id && amco_ids.Contains(d.AMCO_Id) && amb_ids.Contains(d.AMB_Id) && amse_ids.Contains(d.AMSE_Id)).ToList();
                    }
                    //
                    for (int i = 0; i < allRegStudentmain.Count; i++)
                    {
                        temparr.Add(allRegStudentmain[i].PACA_Id);
                    }

                    ctdo.registrationList = allRegStudentmain.ToArray();
                    ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PACA_Id)).ToArray();
                }

                ctdo.ddoc = (from a in _StudentApplicationContext.PA_College_Student_Documents
                             from b in _StudentApplicationContext.MasterDocumentDMO
                             from c in _StudentApplicationContext.PA_College_Application
                             where (a.AMSMD_Id == b.AMSMD_Id && a.PACA_Id == c.PACA_Id && c.ASMAY_Id == miid.ASMAY_Id && c.MI_Id == miid.MI_Id && temparr.Contains(c.PACA_Id))
                             select new CollegePreadmissionstudnetDto
                             {
                                 PACA_Id = c.PACA_Id,
                                 PACSTD_Id = a.PACSTD_Id,
                                 ACSTD_Doc_Path = a.ACSTD_Doc_Path,
                                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                                 AMSMD_Id = a.AMSMD_Id
                             }
                      ).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public CollegePreadmissionstudnetDto getdocksonly(CollegePreadmissionstudnetDto miid)
        {
            try
            {
                miid.ddoc = (from a in _StudentApplicationContext.PA_College_Student_Documents
                             from b in _StudentApplicationContext.MasterDocumentDMO
                             from c in _StudentApplicationContext.PA_College_Application
                             where (a.AMSMD_Id == b.AMSMD_Id && a.PACA_Id == c.PACA_Id && c.PACA_Id == miid.PACA_Id && a.PACA_Id == miid.PACA_Id)
                             select new CollegePreadmissionstudnetDto
                             {

                                 PACA_Id = c.PACA_Id,
                                 PACSTD_Id = a.PACSTD_Id,
                                 ACSTD_Doc_Path = a.ACSTD_Doc_Path,
                                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                                 AMSMD_Id = a.AMSMD_Id,
                                 PACA_StudentPhoto = c.PACA_StudentPhoto,
                                 PACA_FirstName = c.PACA_FirstName,
                                 PACA_MiddleName = c.PACA_MiddleName,
                                 PACA_LastName = c.PACA_LastName
                             }
                      ).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return miid;
        }


        public CollegePreadmissionstudnetDto getbranch(CollegePreadmissionstudnetDto dto)
        {
            try
            {

                List<long> amco_ids = new List<long>();
               // var amco_ids = "0";
                if (dto.courselsttwo.Length > 0)
                {
                    foreach (var ue in dto.courselsttwo)
                    {
                        //amco_ids = amco_ids + "," + ue.AMCO_Id;
                        amco_ids.Add(ue.AMCO_Id);
                    }

                }

                dto.branchlist = (from a in _StudentApplicationContext.ClgMasterBranchDMO
                                  from b in _StudentApplicationContext.Adm_Course_Branch_MappingDMO
                                  where (a.MI_Id == dto.MI_Id && a.AMB_Id == b.AMB_Id && amco_ids.Contains(b.AMCO_Id))
                                  select new CollegePreadmissionstudnetDto
                                  {
                                      AMB_Id = b.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName
                                  }).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


        public CollegePreadmissionstudnetDto getsemester(CollegePreadmissionstudnetDto dto)
        {
            try
            {

                List<long> amb_ids = new List<long>();
                // var amco_ids = "0";
                if (dto.branchlisttwo.Length > 0)
                {
                    foreach (var ue in dto.branchlisttwo)
                    {
                        //amco_ids = amco_ids + "," + ue.AMCO_Id;
                        amb_ids.Add(ue.AMB_Id);
                    }

                }

                List<long> amco_ids = new List<long>();
                // var amco_ids = "0";
                if (dto.courselsttwo.Length > 0)
                {
                    foreach (var ue in dto.courselsttwo)
                    {
                        //amco_ids = amco_ids + "," + ue.AMCO_Id;
                        amco_ids.Add(ue.AMCO_Id);
                    }

                }

                dto.semesterlist = (from a in _StudentApplicationContext.CLG_Adm_Master_SemesterDMO
                                  from b in _StudentApplicationContext.AdmCourseBranchSemesterMappingDMO
                                  from c in _StudentApplicationContext.Adm_Course_Branch_MappingDMO
                                      
                                    where (a.MI_Id == dto.MI_Id && a.AMSE_Id == b.AMSE_Id && amb_ids.Contains(c.AMB_Id) && amco_ids.Contains(c.AMCO_Id) && b.AMCOBM_Id == c.AMCOBM_Id)
                                  select new CollegePreadmissionstudnetDto
                                  {
                                      AMSE_Id = b.AMSE_Id,
                                      AMSE_SEMName = a.AMSE_SEMName
                                  }).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}
