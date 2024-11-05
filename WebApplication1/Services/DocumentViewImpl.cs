
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class DocumentViewImpl : Interfaces.DocumentViewInterface
    {
        //  private readonly AcademicContext _db;
        private static ConcurrentDictionary<string, DocumentViewDTO> _login =
       new ConcurrentDictionary<string, DocumentViewDTO>();
        public StudentApplicationContext _StudentApplicationContext;
        public AcademicContext _AcademicContext;
        ILogger<DocumentViewImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public FeeGroupContext _feecontext;
        public DocumentViewImpl(StudentApplicationContext StudentApplicationContext, FeeGroupContext feecontext, AcademicContext academiccontext, ILogger<DocumentViewImpl> acdimpl, DomainModelMsSqlServerContext db)
        {
            _StudentApplicationContext = StudentApplicationContext;
            _AcademicContext = academiccontext;
            _acdimpl = acdimpl;
            _db = db;
            _feecontext = feecontext;
        }

        public DocumentViewDTO getInitailData(DocumentViewDTO miid)
        {
            DocumentViewDTO ctdo = new DocumentViewDTO();
            try
            {
                ctdo.fillyear = _db.AcademicYear.Where(t => t.MI_Id == miid.mi_id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToArray();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                //*****MAXMINAGE****
                //   allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == miid.mi_id && t.ASMCL_ActiveFlag == true).ToList();
                ctdo.admissioncatdrp = allclass.ToArray();

            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public DocumentViewDTO getDpData(DocumentViewDTO miid)
        {
            DocumentViewDTO ctdo = new DocumentViewDTO();
            List<DocumentViewDTO> alldocList = new List<DocumentViewDTO>();
            List<DocumentViewDTO> alldocListmain = new List<DocumentViewDTO>();
            try
            {

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                //*****MAXMINAGE****
                //   allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == miid.mi_id && t.ASMCL_ActiveFlag == true).ToList();
                ctdo.admissioncatdrp = allclass.ToArray();

                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == miid.mi_id && t.ASMCL_ActiveFlag == true).ToList();
                ctdo.admissioncatdrpall = allclass.ToArray();
                List<long> temparr = new List<long>();
                // flag ISPAC_ApplFeeFlag
                if (miid.configurationsettings == 1)
                {
                    List<StudentApplication> allRegStudentmain = new List<StudentApplication>();
                    //   List<StudentTrnxDoc> alldocList = new List<StudentTrnxDoc>();
                    List<StudentApplication> allRegStudent = new List<StudentApplication>();
                    if (miid.ASMCL_Id == 0)
                    {

                        allRegStudent = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id == miid.asmay_id).ToList();
                    }
                    else
                    {
                        allRegStudent = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id == miid.asmay_id && d.ASMCL_Id == miid.ASMCL_Id).ToList();
                    }

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id.Equals(miid.asmay_id)).ToList();
                    for (int i = 0; i < allRegStudent.Count; i++)
                    {
                        temparr.Add(allRegStudent[i].pasr_id);
                    }
                    if (mstConfig.FirstOrDefault().ISPAC_Healthapp == 0)
                    {
                        ctdo.studentDetailsHelth = (from b in _StudentApplicationContext.Enq
                                                    from c in _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                    where (b.pasr_id == c.PASA_Id && b.MI_Id == miid.mi_id && b.ASMAY_Id == miid.asmay_id && c.FYPPA_Type == "R" && temparr.Contains(b.pasr_id))
                                                    select new StudentHelthcertificateDTO
                                                    {
                                                        pasr_id = b.pasr_id,
                                                        PASR_FirstName = b.PASR_FirstName,
                                                        PASR_MiddleName = b.PASR_MiddleName,
                                                        PASR_LastName = b.PASR_LastName,
                                                        pasR_RegistrationNo = b.PASR_RegistrationNo,
                                                        pasR_Student_Pic_Path = b.PASR_Student_Pic_Path,
                                                        ASMCL_Id = b.ASMCL_Id,
                                                        PASR_Date=b.PASR_Date

                                                    }
       ).Distinct().ToList().ToArray();
                        ctdo.registrationList = ctdo.studentDetailsHelth;
                        ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PASA_Id)).ToArray();
                    }
                    else
                    {
                        if (miid.ASMCL_Id == 0)
                        {
                            ctdo.studentDetailsHelth = (from a in _StudentApplicationContext.StudentHelthcertificate
                                                        from b in _StudentApplicationContext.Enq
                                                        from c in _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                        where (a.PASR_Id == b.pasr_id && b.pasr_id == c.PASA_Id && b.ASMAY_Id==miid.asmay_id && b.MI_Id == miid.mi_id && c.FYPPA_Type == "R")
                                                        select new StudentHelthcertificateDTO
                                                        {
                                                            pasr_id = b.pasr_id,
                                                            PASR_FirstName = b.PASR_FirstName,
                                                            PASR_MiddleName = b.PASR_MiddleName,
                                                            PASR_LastName = b.PASR_LastName,
                                                            PASHD_Id = a.PASHD_Id,
                                                            pasR_RegistrationNo = b.PASR_RegistrationNo,
                                                            pasR_Student_Pic_Path = b.PASR_Student_Pic_Path,
                                                            ASMCL_Id = b.ASMCL_Id,
                                                            PASR_Date = b.PASR_Date

                                                        }
        ).Distinct().ToList().ToArray();
                        }
                        else
                        {
                            ctdo.studentDetailsHelth = (from a in _StudentApplicationContext.StudentHelthcertificate
                                                        from b in _StudentApplicationContext.Enq
                                                        from c in _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                        where (a.PASR_Id == b.pasr_id && b.pasr_id == c.PASA_Id &&  b.ASMAY_Id == miid.asmay_id && b.MI_Id == miid.mi_id && b.ASMCL_Id == miid.ASMCL_Id && c.FYPPA_Type == "R")
                                                        select new StudentHelthcertificateDTO
                                                        {
                                                            pasr_id = b.pasr_id,
                                                            PASR_FirstName = b.PASR_FirstName,
                                                            PASR_MiddleName = b.PASR_MiddleName,
                                                            PASR_LastName = b.PASR_LastName,
                                                            PASHD_Id = a.PASHD_Id,
                                                            pasR_RegistrationNo = b.PASR_RegistrationNo,
                                                            pasR_Student_Pic_Path = b.PASR_Student_Pic_Path,
                                                            ASMCL_Id = b.ASMCL_Id,
                                                            PASR_Date = b.PASR_Date

                                                        }
        ).Distinct().ToList().ToArray();
                        }

                        ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PASA_Id)).ToArray();
                        ctdo.registrationList = ctdo.studentDetailsHelth;
                    }
                }
                else
                {
                    List<StudentApplication> allRegStudentmain = new List<StudentApplication>();
                    if (miid.ASMCL_Id == 0)
                    {
                        allRegStudentmain = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id == miid.asmay_id).ToList();
                    }
                    else
                    {
                        allRegStudentmain = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id == miid.asmay_id && d.ASMCL_Id == miid.ASMCL_Id).ToList();
                    }
                    for (int i = 0; i < allRegStudentmain.Count; i++)
                    {
                        temparr.Add(allRegStudentmain[i].pasr_id);
                    }

                    ctdo.registrationList = allRegStudentmain.ToArray();
                    ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PASA_Id)).ToArray();
                }

                ctdo.ddoc = (from a in _StudentApplicationContext.trxDoc
                             from b in _StudentApplicationContext.MasterDocumentDMO
                             from c in _StudentApplicationContext.Enq
                             where (a.AMSMD_Id == b.AMSMD_Id && a.PASR_Id == c.pasr_id && c.ASMAY_Id == miid.asmay_id && c.MI_Id == miid.mi_id && temparr.Contains(c.pasr_id))
                             select new DocumentViewDTO
                             {
                                 pasr_id = c.pasr_id,
                                 PASRD_Id = a.PASRD_Id,
                                 Document_Path = a.Document_Path,
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
        public DocumentViewDTO getdocksonly(DocumentViewDTO miid)
        {                   
            try
            {
                miid.ddoc = (from a in _StudentApplicationContext.trxDoc
                             from b in _StudentApplicationContext.MasterDocumentDMO
                             from c in _StudentApplicationContext.Enq
                             where (a.AMSMD_Id == b.AMSMD_Id && a.PASR_Id == c.pasr_id && c.pasr_id==miid.pasr_id && a.PASR_Id==miid.pasr_id)
                             select new DocumentViewDTO
                             {
                                 pasr_id = c.pasr_id,
                                 PASRD_Id = a.PASRD_Id,
                                 Document_Path = a.Document_Path,
                                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                                 AMSMD_Id = a.AMSMD_Id,
                                 pasR_Student_Pic_Path=c.PASR_Student_Pic_Path,
                                 PASR_FirstName=c.PASR_FirstName,
                                 PASR_MiddleName=c.PASR_MiddleName,
                                 PASR_LastName=c.PASR_LastName
                             }
                      ).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return miid;
        }


        public DocumentViewDTO StatusGetdetails(DocumentViewDTO masterDTO)//int IVRMM_Id
        {

            List<AdmissionStatus> Allname = new List<AdmissionStatus>();

            try
            {
                Allname = _StudentApplicationContext.AdmissionStatus.Where(m=>m.active==1 && m.MI_Id==masterDTO.mi_id).ToList();
                masterDTO.status_array = Allname.ToArray();
               // mastercasteDTO.getcastelist = _mastercasteContext.mastercasteDMO.Where(a => a.MI_Id == mastercasteDTO.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return masterDTO;
        }


        public DocumentViewDTO mastersaveDTO(DocumentViewDTO mas)
        {

            try
            {
                //AdmissionStatus MM = Mapper.Map<AdmissionStatus>(mas);
                if (mas.PAMST_Id != 0)
                {
                    //&& t.IMCC_Id != mas.IMCC_Id
                    var duplicate = _StudentApplicationContext.AdmissionStatus.Where(t => t.MI_Id == mas.mi_id && t.PAMST_Status == mas.PAMST_Status && t.PAMST_StatusFlag== mas.PAMST_StatusFlag).ToList();

                    // var result_update = _mastercasteContext.mastercasteDMO.Where(t => t.MI_Id == mas.MI_Id || t.IMC_CasteName != mas.IC_CasteName||t.IMC_CasteDesc!=mas.IC_CasteDesc||t.IMCC_Id!=mas.IMCC_Id).Count();

                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {
                        //if (duplicate.FirstOrDefault().IMC_CasteDesc != mas.IC_CasteDesc)
                        //{
                        var result = _StudentApplicationContext.AdmissionStatus.Single(t => t.MI_Id == mas.mi_id && t.PAMST_Id==mas.PAMST_Id);

                        result.PAMST_Status = mas.PAMST_Status;
                        result.PAMST_StatusFlag = mas.PAMST_StatusFlag;
                        result.active = 1;
                        result.MI_Id = mas.mi_id;

                        result.UpdatedDate = DateTime.Now;
                        result.CreatedDate = result.CreatedDate;
                        _StudentApplicationContext.Update(result);
                        var flag = _StudentApplicationContext.SaveChanges();
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

                    var duplicate_caste_name = _StudentApplicationContext.AdmissionStatus.Where(t => t.MI_Id == mas.mi_id && t.PAMST_Status == mas.PAMST_Status && t.PAMST_StatusFlag==mas.PAMST_StatusFlag).ToList();

                    var duplicatecountresult = _StudentApplicationContext.AdmissionStatus.Where(t => t.MI_Id == mas.mi_id && t.PAMST_Status == mas.PAMST_Status && t.PAMST_StatusFlag == mas.PAMST_StatusFlag).Count();



                    //if (duplicate_caste_name.Count > 0)
                    //{
                    //    mas.duplicate_caste_name_bool = true;
                    //    return mas;
                    //}

                    if (duplicatecountresult == 0)
                    {
                        AdmissionStatus MM3 = new AdmissionStatus();
                        MM3.PAMST_Status = mas.PAMST_Status;
                        MM3.PAMST_StatusFlag = mas.PAMST_StatusFlag;
                        MM3.active = 1;
                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        MM3.MI_Id = mas.mi_id;
                        _StudentApplicationContext.Add(MM3);
                        var flag = _StudentApplicationContext.SaveChanges();
                        if (flag > 0)
                        {
                         

                            mas.returnVal = true;
                        }
                        else
                        {
                            mas.returnVal = false;
                        }

                    }



                    else
                    {
                        mas.msg = "Record Already Exist";
                    }


                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return mas;
        }


        public DocumentViewDTO GetSelectedRowDetails(int ID)
        {
            DocumentViewDTO masterDTO = new DocumentViewDTO();

            try
            {
                List<AdmissionStatus> lorg = new List<AdmissionStatus>();
                lorg = _StudentApplicationContext.AdmissionStatus.Where(t => t.PAMST_Id == ID).ToList();
                masterDTO.GridviewDetails = lorg.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return masterDTO;
        }
        public DocumentViewDTO MasterDeleteModulesData(int ID)
        {
            DocumentViewDTO masterDTO = new DocumentViewDTO();
            List<AdmissionStatus> masters = new List<AdmissionStatus>();
            try
            {
                masters = _StudentApplicationContext.AdmissionStatus.Where(t => t.PAMST_Id == ID).ToList();
                if (masters.Any())
                {
                    _StudentApplicationContext.Remove(masters.ElementAt(0));
                        var flag = _StudentApplicationContext.SaveChanges();
                        if (flag > 0)
                        {
                            masterDTO.returnVal = true;
                        }
                        else
                        {
                            masterDTO.returnVal = false;
                        }
                    
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }


            return masterDTO;
        }
    }
}
