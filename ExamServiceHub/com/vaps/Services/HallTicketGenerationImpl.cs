using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class HallTicketGenerationImpl : HallTicketGenerationInterface
    {
        public ExamContext _examctxt;
        public HallTicketGenerationImpl(ExamContext obj)
        {
            _examctxt = obj;
        }
        public HallTicketGenerationDTO getdetails(HallTicketGenerationDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.examlist = _examctxt.masterexam.Where(z => z.MI_Id == data.MI_Id && z.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToArray();

                List<AdmissionClass> classes = new List<AdmissionClass>();
                classes = _examctxt.AdmissionClass.Where(c => c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                data.ctlist = classes.Distinct().ToArray();

                List<School_M_Section> sections = new List<School_M_Section>();
                sections = _examctxt.School_M_Section.Where(c => c.MI_Id == data.MI_Id && c.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                data.seclist = sections.Distinct().ToArray();

                data.alldata = (from a in _examctxt.Exm_HallTicketDMO
                                from b in _examctxt.AcademicYear
                                from c in _examctxt.masterexam
                                from d in _examctxt.AdmissionClass
                                from e in _examctxt.School_M_Section
                                where (a.ASMAY_Id == b.ASMAY_Id && a.EME_Id == c.EME_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id)
                                select new HallTicketGenerationDTO
                                {
                                    ASMAY_Year = b.ASMAY_Year,
                                    EME_ExamName = c.EME_ExamName,
                                    ASMCL_ClassName = d.ASMCL_ClassName,
                                    ASMC_SectionName = e.ASMC_SectionName,
                                    ASMAY_Order = b.ASMAY_Order,
                                    ASMCL_Id = a.ASMCL_Id,
                                    ASMS_Id = a.ASMS_Id,
                                    ASMAY_Id = a.ASMAY_Id,
                                    EME_Id = a.EME_Id
                                }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public HallTicketGenerationDTO onselectAcdYear(HallTicketGenerationDTO data)
        {
            try
            {
                data.ctlist = (from a in _examctxt.AdmissionClass
                               from b in _examctxt.Exm_Category_ClassDMO
                               from c in _examctxt.AcademicYear
                               where (b.MI_Id == data.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true
                               && b.ASMAY_Id == data.ASMAY_Id)
                               select new ExamTTTransSettingsDTO
                               {
                                   ASMCL_Id = a.ASMCL_Id,
                                   ASMCL_ClassName = a.ASMCL_ClassName,
                                   ASMCL_Order = a.ASMCL_Order
                               }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HallTicketGenerationDTO onselectclass(HallTicketGenerationDTO data)
        {
            try
            {
                data.seclist = (from a in _examctxt.School_M_Section
                                from b in _examctxt.Exm_Category_ClassDMO
                                from c in _examctxt.AdmissionClass
                                from d in _examctxt.AcademicYear
                                where (a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == b.ASMS_Id
                                && c.ASMCL_Id == b.ASMCL_Id && d.ASMAY_Id == b.ASMAY_Id && a.ASMC_ActiveFlag == 1)
                                select new HallTicketGenerationDTO
                                {
                                    ASMS_Id = a.ASMS_Id,
                                    ASMC_SectionName = a.ASMC_SectionName,
                                    ASMC_Order = a.ASMC_Order
                                }
                                ).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HallTicketGenerationDTO onselectsection(HallTicketGenerationDTO data)
        {
            try
            {
                List<long> secids = new List<long>();

                foreach (var c in data.section_Array)
                {
                    secids.Add(c.ASMS_Id);
                }

                data.examlist = (from a in _examctxt.School_M_Section
                                 from b in _examctxt.Exm_Category_ClassDMO
                                 from c in _examctxt.AdmissionClass
                                 from d in _examctxt.AcademicYear
                                 from e in _examctxt.Exm_Yearly_CategoryDMO
                                 from g in _examctxt.Exm_Yearly_Category_ExamsDMO
                                 from h in _examctxt.exammasterDMO
                                 where (a.ASMS_Id == b.ASMS_Id && c.ASMCL_Id == b.ASMCL_Id && d.ASMAY_Id == b.ASMAY_Id && a.ASMC_ActiveFlag == 1
                                 && b.EMCA_Id == e.EMCA_Id && d.ASMAY_Id == e.ASMAY_Id && e.EYC_ActiveFlg == true && e.EYC_Id == g.EYC_Id && g.EYCE_ActiveFlg == true
                                 && g.EME_Id == h.EME_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                 && e.ASMAY_Id == data.ASMAY_Id && secids.Contains(b.ASMS_Id))
                                 select h).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public HallTicketGenerationDTO savedetail(HallTicketGenerationDTO data)
        {
            try
            {
                string secidget = "";
                for (int i = 0; i < data.section_Array.Length; i++)
                {

                    string Id = data.section_Array[i].ASMS_Id.ToString();
                    if (Id != "0" && Id != null)
                    {

                        secidget = Id + "," + secidget;
                    }
                }
                string coloumns = secidget.Remove(secidget.Length - 1);

                var contactExists = _examctxt.Database.ExecuteSqlCommand("Exam_HallTicket_Generation_New @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, coloumns, data.EME_Id, data.prefixstr, data.startno, data.increment, data.leadingzeros);
                if (contactExists > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HallTicketGenerationDTO ViewStudentDetails(HallTicketGenerationDTO data)
        {
            try
            {
                data.datareport = (from a in _examctxt.Exm_HallTicketDMO
                                   from b in _examctxt.Adm_M_Student
                                   from c in _examctxt.AdmissionClass
                                   from d in _examctxt.School_M_Section
                                   from g in _examctxt.School_Adm_Y_Student
                                   from e in _examctxt.AcademicYear
                                   from f in _examctxt.exammasterDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                   && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id
                                   && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id && a.EME_Id == f.EME_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                   && g.AMST_Id == b.AMST_Id && g.AMAY_ActiveFlag == 1 && g.ASMAY_Id == data.ASMAY_Id && g.ASMCL_Id == data.ASMCL_Id
                                   && g.ASMS_Id == data.ASMS_Id)
                                   select new HallTicketGenerationDTO
                                   {
                                       AMST_Id = g.AMST_Id,
                                       AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : " " + b.AMST_FirstName) +
                                       (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                       (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                       AMST_AdmNo = b.AMST_AdmNo,
                                       AMAY_Rollno = g.AMAY_RollNo,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMC_SectionName = d.ASMC_SectionName,
                                       EHT_HallTicketNo = a.EHT_HallTicketNo,
                                       EHT_Id = a.EHT_Id,
                                       AMST_Photoname = b.AMST_Photoname,
                                       AMST_FatherName = ((b.AMST_FatherName == null || b.AMST_FatherName == "" ? "" : " " + b.AMST_FatherName)
                                       + (b.AMST_FatherSurname == null || b.AMST_FatherSurname == "" || b.AMST_FatherSurname == "0" ? "" : " " + b.AMST_FatherSurname)).Trim(),
                                       EHT_PublishFlg = a.EHT_PublishFlg

                                   }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HallTicketGenerationDTO SaveStudentStatus(HallTicketGenerationDTO data)
        {
            try
            {
                if (data.selectedstudents.Length > 0)
                {
                    foreach (var c in data.selectedstudents)
                    {
                        var checkresult = _examctxt.Exm_HallTicketDMO.Where(a => a.AMST_Id == c.AMST_Id && a.EHT_Id == c.EHT_Id).ToList();

                        if (checkresult.Count > 0)
                        {
                            var result = _examctxt.Exm_HallTicketDMO.Single(a => a.AMST_Id == c.AMST_Id && a.EHT_Id == c.EHT_Id);

                            //result.EHT_PublishFlg = data.statusflag == "1" ? true : false;
                            result.EHT_PublishFlg = result.EHT_PublishFlg == true ? false : true;
                            result.UpdatedDate = DateTime.Now;
                            _examctxt.Update(result);
                        }
                    }

                    var i = _examctxt.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}