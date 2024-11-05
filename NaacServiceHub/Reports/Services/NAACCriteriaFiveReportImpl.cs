using CommonLibrary;
using DataAccessMsSqlServerProvider.NAAC;
using DataAccessMsSqlServerProvider.NAAC.Documents;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Services
{
    public class NAACCriteriaFiveReportImpl: Interface.NAACCriteriaFiveReportInterface
    {
        public DocumentsContext _DocumentsContext;
        public GeneralContext _GeneralContext;
        public NAACCriteriaFiveReportImpl(DocumentsContext DocumentsContext, GeneralContext GeneralContext)
        {
            _DocumentsContext = DocumentsContext;
            _GeneralContext = GeneralContext;
        }
        public NAACCriteriaFiveReportDTO getdata(NAACCriteriaFiveReportDTO data)
        {
            try
            {
                var getinstitution = _GeneralContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                string NAACSL_InstitutionTypeFlg = "";
                List<long> miid = new List<long>();
                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                }

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                data.getinstitutioncycle = naaccomm.get_cycle_list(data.MI_Id, data.UserId);

                data.getinstitution = naaccomm.get_Institution_list(data.MI_Id, data.UserId);

                data.NAACSL_InstitutionTypeFlg = NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;

              


                data.yearlist = _DocumentsContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            return data;
        }


        public async Task<NAACCriteriaFiveReportDTO> HSU511(NAACCriteriaFiveReportDTO data)
        {
            try
            {


                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yearid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);


                //

                data.nngovtsclist = (from f in _GeneralContext.NAAC_HSU_511_NonGovScholarshipDMO
                                   from b in _GeneralContext.Academic
                                   where f.MI_Id == b.MI_Id && b.Is_Active == true && f.NCAC512NGSCH_ActiveFlg == true && mid.Contains(f.MI_Id) && f.NCAC512NGSCH_ActiveFlg == true && yearid.Contains(f.NCAC512NGSCH_Year) && f.NCAC512NGSCH_Year == b.ASMAY_Id
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC512NGSCH_Id = f.NCAC512NGSCH_Id,
                                       MI_Id = f.MI_Id,
                                       NCAC512NGSCH_SchemeName = f.NCAC512NGSCH_SchemeName,
                                       NCAC512NGSCH_NoOfStudents = f.NCAC512NGSCH_NoOfStudents,
                                       NCAC512NGSCH_Year = f.NCAC512NGSCH_Year,
                                       ASMAY_Year = b.ASMAY_Year.Trim()
                                   }).Distinct().ToArray();


                data.nngovtsclistfiles = (from c in _GeneralContext.Academic
                                        from a in _GeneralContext.NAAC_HSU_511_NonGovScholarshipDMO
                                          from b in _GeneralContext.NAAC_HSU_511_NonGovScholarship_FilesDMO
                                          where mid.Contains(a.MI_Id) && a.NCAC512NGSCH_ActiveFlg == true && a.NCAC512NGSCH_Id == b.NCAC512NGSCH_Id && yearid.Contains(a.NCAC512NGSCH_Year) && b.NCAC512NGSCHF_ActiveFlg==true
                                        select b).Distinct().ToArray();


                //
                data.govtsclist = (from f in _GeneralContext.NAAC_AC_511_GovScholarshipDMO
                                   from b in _GeneralContext.Academic
                                   where f.MI_Id == b.MI_Id && b.Is_Active == true && f.NCAC511GSCH_ActiveFlg == true && mid.Contains(f.MI_Id) && f.NCAC511GSCH_ActiveFlg == true && yearid.Contains(f.NCAC511GSCH_Year) && f.NCAC511GSCH_Year == b.ASMAY_Id
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC511GSCH_Id = f.NCAC511GSCH_Id,
                                       MI_Id = f.MI_Id,
                                       NCAC511GSCH_SchemeName = f.NCAC511GSCH_SchemeName,
                                       NCAC511GSCH_NoOfStudents = f.NCAC511GSCH_NoOfStudents,
                                       NCAC511GSCH_Year = f.NCAC511GSCH_Year,
                                       ASMAY_Year = b.ASMAY_Year.Trim()
                                   }).Distinct().ToArray();


                data.govtsclistfiles = (from c in _GeneralContext.Academic
                                        from a in _GeneralContext.NAAC_AC_511_GovScholarshipDMO
                                        from b in _GeneralContext.NAAC_AC_511_GovScholarshipFilesDMO
                                        where mid.Contains(a.MI_Id) && a.NCAC511GSCH_ActiveFlg == true && a.NCAC511GSCH_Id == b.NCAC511GSCH_Id && yearid.Contains(a.NCAC511GSCH_Year) && b.NCAC511GSCHF_ActiveFlg == true
                                        select b).Distinct().ToArray();


                data.instsclist = (from y in _GeneralContext.NAAC_AC_512_InstScholarshipDMO
                                   from b in _GeneralContext.Academic
                                   where y.MI_Id == b.MI_Id && b.Is_Active == true && y.NCAC512INSCH_ActiveFlg == true && mid.Contains(y.MI_Id) && b.Is_Active == true && yearid.Contains(y.NCAC512INSCH_Year) && y.NCAC512INSCH_Year == b.ASMAY_Id
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       MI_Id = y.MI_Id,
                                       NCAC512INSCH_Id = y.NCAC512INSCH_Id,
                                       NCAC512INSCH_SchemeName = y.NCAC512INSCH_SchemeName,
                                       NCAC512INSCH_NoOfStudents = y.NCAC512INSCH_NoOfStudents,
                                       NCAC512INSCH_Year = y.NCAC512INSCH_Year,
                                       ASMAY_Year = b.ASMAY_Year,
                                   }).Distinct().ToArray();


                data.instsclistfiles = (from c in _GeneralContext.Academic
                                        from a in _GeneralContext.NAAC_AC_512_InstScholarshipDMO
                                        from b in _GeneralContext.NAAC_AC_512_InstScholarshipFilesDMO
                                        where mid.Contains(a.MI_Id) && a.NCAC512INSCH_ActiveFlg == true && a.NCAC512INSCH_Id == b.NCAC512INSCH_Id && yearid.Contains(a.NCAC512INSCH_Year) && b.NCAC512INSCHF_ActiveFlg == true
                                        select b).Distinct().ToArray();

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report(NAACCriteriaFiveReportDTO data)
        {
            try
            {
              

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }
                  
                }
                List<long> yearid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.govtsclist = (from f in _GeneralContext.NAAC_AC_511_GovScholarshipDMO
                                  from b in _GeneralContext.Academic
                                  where f.MI_Id==b.MI_Id && b.Is_Active==true && f.NCAC511GSCH_ActiveFlg==true && mid.Contains(f.MI_Id) && f.NCAC511GSCH_ActiveFlg == true && yearid.Contains(f.NCAC511GSCH_Year) && f.NCAC511GSCH_Year==b.ASMAY_Id
                                   select new NAACCriteriaFiveReportDTO
                                  {
                                      NCAC511GSCH_Id = f.NCAC511GSCH_Id,
                                      MI_Id = f.MI_Id,
                                      NCAC511GSCH_SchemeName = f.NCAC511GSCH_SchemeName,
                                      NCAC511GSCH_NoOfStudents = f.NCAC511GSCH_NoOfStudents,
                                      NCAC511GSCH_Year = f.NCAC511GSCH_Year,
                                      ASMAY_Year = b.ASMAY_Year.Trim()
                                  }).Distinct().ToArray();


                data.govtsclistfiles = (from c in _GeneralContext.Academic
                                         from a in _GeneralContext.NAAC_AC_511_GovScholarshipDMO
                                        from b in _GeneralContext.NAAC_AC_511_GovScholarshipFilesDMO
                                        where mid.Contains(a.MI_Id) && a.NCAC511GSCH_ActiveFlg == true && a.NCAC511GSCH_Id == b.NCAC511GSCH_Id && yearid.Contains(a.NCAC511GSCH_Year) && b.NCAC511GSCHF_ActiveFlg == true select b).Distinct().ToArray();
                

                    data.instsclist =(from y in  _GeneralContext.NAAC_AC_512_InstScholarshipDMO
                                       from b in _GeneralContext.Academic
                                     where y.MI_Id == b.MI_Id && b.Is_Active == true && y.NCAC512INSCH_ActiveFlg == true && mid.Contains(y.MI_Id) && b.Is_Active == true && yearid.Contains(y.NCAC512INSCH_Year) && y.NCAC512INSCH_Year==b.ASMAY_Id
                                     select new NAACCriteriaFiveReportDTO
                                     {
                                         MI_Id = y.MI_Id,
                                         NCAC512INSCH_Id = y.NCAC512INSCH_Id,
                                         NCAC512INSCH_SchemeName = y.NCAC512INSCH_SchemeName,
                                         NCAC512INSCH_NoOfStudents = y.NCAC512INSCH_NoOfStudents,
                                         NCAC512INSCH_Year = y.NCAC512INSCH_Year,
                                         ASMAY_Year = b.ASMAY_Year,
                                     }).Distinct().ToArray();


                data.instsclistfiles = (from c in _GeneralContext.Academic
                                        from a in _GeneralContext.NAAC_AC_512_InstScholarshipDMO
                                        from b in _GeneralContext.NAAC_AC_512_InstScholarshipFilesDMO
                                        where mid.Contains(a.MI_Id) && a.NCAC512INSCH_ActiveFlg == true && a.NCAC512INSCH_Id == b.NCAC512INSCH_Id && yearid.Contains(a.NCAC512INSCH_Year) && b.NCAC512INSCHF_ActiveFlg == true
                                        select b).Distinct().ToArray();

                data.yearlist =( from  a in  _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();



            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

      

        public async Task<NAACCriteriaFiveReportDTO> get_report513(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.govtsclist = (from a in _GeneralContext.NAAC_AC_513_DevSchemesDMO
                                   from b in _GeneralContext.Academic
                                   where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC513INSCH_ImpYear == b.ASMAY_Id && a.NCAC513INSCH_ActiveFlg == true && b.Is_Active == true &&  yrid.Contains(a.NCAC513INSCH_ImpYear)
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC513INSCH_Id = a.NCAC513INSCH_Id,
                                       NCAC513INSCH_DevSchemeName = a.NCAC513INSCH_DevSchemeName,
                                       noofstd = a.NCAC513INSCH_NoOfStudents,
                                       NCAC513INSCH_AgencyDetails = a.NCAC513INSCH_AgencyDetails,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMAY_Order = b.ASMAY_Order
                                   }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray(); 




                 data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_513_DevSchemesDMO
                                         from b in _GeneralContext.NAAC_AC_513_DevSchemeFilesDMO
                                         where mid.Contains(a.MI_Id) && a.NCAC513INSCH_ActiveFlg == true && a.NCAC513INSCH_Id == b.NCAC513INSCH_Id && yrid.Contains(a.NCAC513INSCH_ImpYear) && b.NCAC513INSCHF_ActiveFlg == true
                                         select b).Distinct().ToArray();

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report514(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();


                data.govtsclist = (from a in _GeneralContext.NAAC_AC_514_CompExamsDMO
                                   from b in _GeneralContext.Academic
                                   where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC514CPEX_ImpYear == b.ASMAY_Id && a.NCAC514CPEX_ActiveFlg == true && b.Is_Active == true &&  yrid.Contains(a.NCAC514CPEX_ImpYear)
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC513INSCH_Id = a.NCAC514CPEX_Id,
                                       NCAC513INSCH_DevSchemeName = a.NCAC514CPEX_ExamSchemeName,
                                       noofstd = a.NCAC514CPEX_NoOfStudents,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMAY_Order = b.ASMAY_Order
                                   }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray(); 




                 data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_514_CompExamsDMO
                                         from b in _GeneralContext.NAAC_AC_514_CompExamsFilesDMO
                                         where mid.Contains(a.MI_Id) && a.NCAC514CPEX_ActiveFlg == true && a.NCAC514CPEX_Id == b.NCAC514CPEX_Id && yrid.Contains(a.NCAC514CPEX_ImpYear) && b.NCAC514CPEXF_ActiveFlg == true
                                         select b).Distinct().ToArray();

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<NAACCriteriaFiveReportDTO> get_report513med(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();


                data.govtsclist = (from a in _GeneralContext.NAAC_AC_514_CompExamsDMO
                                   from b in _GeneralContext.Academic
                                   where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC514CPEX_ImpYear == b.ASMAY_Id && a.NCAC514CPEX_ActiveFlg == true && b.Is_Active == true &&  yrid.Contains(a.NCAC514CPEX_ImpYear) 
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC513INSCH_Id = a.NCAC514CPEX_Id,
                                       NCAC513INSCH_DevSchemeName = a.NCAC514CPEX_ExamSchemeName,
                                       noofstd = a.NCAC514CPEX_NoOfStudents,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMAY_Order = b.ASMAY_Order
                                   }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray(); 




                 data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_514_CompExamsDMO
                                         from b in _GeneralContext.NAAC_AC_514_CompExamsFilesDMO
                                         where mid.Contains(a.MI_Id) && a.NCAC514CPEX_ActiveFlg == true && a.NCAC514CPEX_Id == b.NCAC514CPEX_Id && yrid.Contains(a.NCAC514CPEX_ImpYear) && b.NCAC514CPEXF_ActiveFlg == true
                                         select b).Distinct().ToArray();



                data.stdcntlist=(from a in _GeneralContext.Adm_Master_College_StudentDMO
                                 from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                 from c in _GeneralContext.Academic
                                 where a.AMCST_Id==b.AMCST_Id && b.ASMAY_Id==c.ASMAY_Id && a.MI_Id==c.MI_Id && mid.Contains(a.MI_Id)  && yrid.Contains(a.ASMAY_Id ) && a.AMCST_ActiveFlag==true
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = c.ASMAY_Year,
                                     stud_count = a.AMCST_Id
                                 }).Distinct().GroupBy(id => id.ASMAY_Year).Select(g => new NAACCriteriaFiveReportDTO { ASMAY_Year = g.Key, stud_count = g.Count() }).ToArray();



            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report516(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yearid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);


                data.govtsclist = (from a in _GeneralContext.NAAC_AC_516_GRIDMO
                                   from b in _GeneralContext.Academic
                                   where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id)  && a.NCAC516GRI_Year == b.ASMAY_Id && a.NCAC516GRI_ActiveFlg == true && b.Is_Active == true &&  yearid.Contains(a.NCAC516GRI_Year)
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC513INSCH_Id = a.NCAC516GRI_Id,
                                       NCAC516GRI_GRIAPP = a.NCAC516GRI_GRIAPP,
                                       NCAC516GRI_GRIRED = a.NCAC516GRI_GRIRED,
                                       NCAC516GRI_AvgTime = a.NCAC516GRI_AvgTime,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMAY_Order = b.ASMAY_Order
                                   }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray(); 




                 data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_516_GRIDMO
                                         from b in _GeneralContext.NAAC_AC_516_GRIFilesDMO
                                         where mid.Contains(a.MI_Id) && a.NCAC516GRI_ActiveFlg == true && a.NCAC516GRI_Id == b.NCAC516GRI_Id && yearid.Contains(a.NCAC516GRI_Year) && b.NCAC516GRIF_ActiveFlg == true
                                         select b).Distinct().ToArray();

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report515med(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yearid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);


                data.govtsclist = (from a in _GeneralContext.NAAC_AC_516_GRIDMO
                                       //from b in _GeneralContext.Academic
                                   where mid.Contains(a.MI_Id)
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC516GRI_AdpOfguidelinesofRegbodiesFlg = a.NCAC516GRI_AdpOfguidelinesofRegbodiesFlg,
                NCAC516GRI_StusgrvOnline_OR_offlineFlg = a.NCAC516GRI_StusgrvOnline_OR_offlineFlg,
                                       NCAC516GRI_CommitteewithminutesFlg = a.NCAC516GRI_CommitteewithminutesFlg,
                                       NCAC516GRI_RecordOfActionTakenFlg = a.NCAC516GRI_RecordOfActionTakenFlg
                                   }).Distinct().ToArray(); 




                 data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_516_GRIDMO
                                         from b in _GeneralContext.NAAC_AC_516_GRIFilesDMO
                                         where mid.Contains(a.MI_Id) && a.NCAC516GRI_ActiveFlg == true && a.NCAC516GRI_Id == b.NCAC516GRI_Id && yearid.Contains(a.NCAC516GRI_Year) && b.NCAC516GRIF_ActiveFlg == true
                                         select b).Distinct().ToArray();

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report521(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.govtsclist = (from a in _GeneralContext.NAAC_AC_521_PlacementDMO
                                   from b in _GeneralContext.Academic
                                   from c in _GeneralContext.MasterCourseDMO
                                   from d in _GeneralContext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC521PLA_Year == b.ASMAY_Id && a.NCAC521PLA_ActiveFlg == true && b.Is_Active == true &&  yrid.Contains(a.NCAC521PLA_Year) && a.MI_Id==c.MI_Id && c.AMCO_Id==a.NCAC521PLA_GradCourse && a.NCAC521PLA_GradBranch==d.AMB_Id && a.MI_Id==d.MI_Id
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC521PLA_Id = a.NCAC521PLA_Id,
                                       NCAC521PLA_EmployerName = a.NCAC521PLA_EmployerName,
                                       noofstd = a.NCAC521PLA_NoOfStudents,
                                       noofstdself = a.NCAC521PLA_NoOfstudentsselfemployed,
                                       amount = a.NCAC521PLA_Package,
                                       AMB_BranchName = d.AMB_BranchName,
                                       AMCO_CourseName = c.AMCO_CourseName,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMAY_Order = b.ASMAY_Order
                                   }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray(); 




                 data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_521_PlacementDMO
                                         from b in _GeneralContext.NAAC_AC_521_PlacementFilesDMO
                                         where mid.Contains(a.MI_Id) && a.NCAC521PLA_ActiveFlg == true && a.NCAC521PLA_Id == b.NCAC521PLA_Id && yrid.Contains(a.NCAC521PLA_Year) && b.NCAC521PLAF_ActiveFlg == true
                                         select b).Distinct().ToArray();

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report522(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yearid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.govtsclist = (from a in _GeneralContext.NAAC_AC_522_HrEducationDMO
                                   from b in _GeneralContext.Academic
                                   from c in _GeneralContext.MasterCourseDMO
                                   from d in _GeneralContext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC522HRED_Year == b.ASMAY_Id && a.NCAC522HRED_ActiveFlg == true && b.Is_Active == true &&  yearid.Contains(a.NCAC522HRED_Year) && a.MI_Id==c.MI_Id && c.AMCO_Id==a.NCAC522HRED_GraduatedProgram && a.NCAC522HRED_GraduatedDept==d.AMB_Id && a.MI_Id==d.MI_Id
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC522HRED_Id = a.NCAC522HRED_Id,
                                       atudentname = a.NCAC522HRED_HrEduEnrollStudentNo,
                                       department = a.NCAC522HRED_AdmittedDept,
                                       program = a.NCAC522HRED_AdmittedProgram,
                                       AMB_BranchName = d.AMB_BranchName,
                                       AMCO_CourseName = c.AMCO_CourseName,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMAY_Order = b.ASMAY_Order,
                                       institutionname = a.NCAC522HRED_InstitutionName
                                   }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray(); 




                 data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_522_HrEducationDMO
                                         from b in _GeneralContext.NAAC_AC_522_HrEducationFilesDMO
                                         where mid.Contains(a.MI_Id) && a.NCAC522HRED_ActiveFlg == true && a.NCAC522HRED_Id == b.NCAC522HRED_Id && yearid.Contains(a.NCAC522HRED_Year) && b.NCAC522HREDF_ActiveFlg == true
                                         select b).Distinct().ToArray();

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
         public async Task<NAACCriteriaFiveReportDTO> get_report531(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();


                data.govtsclist = (from a in _GeneralContext.NAAC_AC_531_SportsCADMO
                                   from b in _GeneralContext.Academic
                                   from c in _GeneralContext.NAAC_AC_531_SportsCA_StudentsDMO
                                   from d in _GeneralContext.Adm_Master_College_StudentDMO
                                   from e in _GeneralContext.Adm_College_Yearly_StudentDMO
                                   where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC531SPCA_Year == b.ASMAY_Id && a.NCAC531SPCA_ActiveFlg == true && b.Is_Active == true &&  yrid.Contains(a.NCAC531SPCA_Year) && a.MI_Id==c.MI_Id && c.NCAC531SPCA_Id==a.NCAC531SPCA_Id && c.NCAC531SPCAS_ActiveFlg==true && a.MI_Id==c.MI_Id && a.MI_Id==d.MI_Id && c.AMCST_Id==d.AMCST_Id && d.AMCST_Id==e.AMCST_Id && e.ASMAY_Id==a.NCAC531SPCA_Year && d.AMCST_SOL=="S" && d.AMCST_ActiveFlag==true  && e.ACYST_ActiveFlag==1
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       AMCST_Id = d.AMCST_Id,
                                       NCAC531SPCA_Id = a.NCAC531SPCA_Id,
                                       NCAC531SPCAS_Id = c.NCAC531SPCAS_Id,
                                       awardname = c.NCAC531SPCAS_AwardName,
                                       NCAC531SPCAS_NatOrInterNatFlg = c.NCAC531SPCAS_NatOrInterNatFlg,
                                       NCAC531SPCAS_SportsCAIEEEFlg = c.NCAC531SPCAS_SportsCAIEEEFlg,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMAY_Order = b.ASMAY_Order,
                                       AMCST_FirstName = ((d.AMCST_FirstName == null ? " " : d.AMCST_FirstName) + " " + (d.AMCST_MiddleName == null ? " " : d.AMCST_MiddleName) + " " + (d.AMCST_LastName == null ? " " : d.AMCST_LastName)).Trim(),
                                       AMCST_AadharNo = d.AMCST_AadharNo,
                                       AMCST_RegistrationNo = d.AMCST_RegistrationNo,

                                   }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray(); 




                 data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_531_SportsCADMO
                                         from b in _GeneralContext.NAAC_AC_531_SportsCAFilesDMO
                                         where mid.Contains(a.MI_Id) && a.NCAC531SPCA_ActiveFlg == true && a.NCAC531SPCA_Id == b.NCAC531SPCA_Id && yrid.Contains(a.NCAC531SPCA_Year) && b.NCAC531SPCAF_ActiveFlg == true
                                         select b).Distinct().ToArray();

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<NAACCriteriaFiveReportDTO> get_report533(NAACCriteriaFiveReportDTO data)
        {
            try
                {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.govtsclist = (from a in _GeneralContext.NAAC_AC_533_SportsCA_ActivitiesDMO
                                       from b in _GeneralContext.Academic
                                       where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC533SPCAA_Year == b.ASMAY_Id && a.NCAC533SPCAA_ActiveFlg == true && b.Is_Active == true && yrid.Contains(a.NCAC533SPCAA_Year)
                                       select new NAACCriteriaFiveReportDTO
                                       {
                                           NCAC533SPCAA_Id = a.NCAC533SPCAA_Id,
                                           NCAC533SPCAA_ActType = a.NCAC533SPCAA_ActType,
                                           NCAC533SPCAA_ActLevel = a.NCAC533SPCAA_ActLevel,
                                           program = a.NCAC533SPCAA_NameOfActivities,
                                           ASMAY_Year = b.ASMAY_Year,
                                           ASMAY_Order = b.ASMAY_Order
                                       }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();




                    data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_533_SportsCA_ActivitiesDMO
                                            from b in _GeneralContext.NAAC_AC_533_SportsCA_ActivitiesFilesDMO
                                            where mid.Contains(a.MI_Id) && a.NCAC533SPCAA_ActiveFlg == true && a.NCAC533SPCAA_Id == b.NCAC533SPCAA_Id && yrid.Contains(a.NCAC533SPCAA_Year) && b.NCAC533SPCAAF_ActiveFlg == true
                                            select b).Distinct().ToArray();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report542(NAACCriteriaFiveReportDTO data)
        {
            try
                {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yearid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.govtsclist = (from a in _GeneralContext.NAAC_AC_542_AlumniContDMO
                                       from b in _GeneralContext.Academic
                                       where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id)&& a.NCAC542ALMCON_ContributionYear == b.ASMAY_Id && a.NCAC542ALMCON_ActiveFlg == true && b.Is_Active == true && yearid.Contains(a.NCAC542ALMCON_ContributionYear)
                                       select new NAACCriteriaFiveReportDTO
                                       {
                                           NCAC542ALMCON_Id = a.NCAC542ALMCON_Id,
                                           awardname = a.NCAC542ALMCON_AlumnsName,
                                           amount = a.NCAC542ALMCON_ContriAmount.ToString(),
                                           aadharpan = a.NCAC542ALMCON_AadharPAN,
                                           ASMAY_Year = b.ASMAY_Year,
                                           ASMAY_Order = b.ASMAY_Order,
                                           NCAC531SPCAS_DonationOfBooksFlag = a.NCAC531SPCAS_DonationOfBooksFlag,
                                           NCAC531SPCAS_FinancialORKindFlag = a.NCAC531SPCAS_FinancialORKindFlag,
                                           NCAC531SPCAS_InstendowmentsFlag = a.NCAC531SPCAS_InstendowmentsFlag,
                                           NCAC531SPCAS_StudentexchangesFlag = a.NCAC531SPCAS_StudentexchangesFlag,
                                           NCAC531SPCAS_StudentsplacementFlag = a.NCAC531SPCAS_StudentsplacementFlag,
                                           ASMAY_Year1 = _GeneralContext.Academic.Where(w => w.ASMAY_Id == a.NCAC542ALMCON_GraduationYear && w.Is_Active).Select(e => e.ASMAY_Year).SingleOrDefault(),
                                       }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();




                    data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_542_AlumniContDMO
                                            from b in _GeneralContext.NAAC_AC_542_AlumniContFilesDMO
                                            where mid.Contains(a.MI_Id) && a.NCAC542ALMCON_ActiveFlg == true && a.NCAC542ALMCON_Id == b.NCAC542ALMCON_Id && yearid.Contains(a.NCAC542ALMCON_ContributionYear) && b.NCAC542ALMCONF_ActiveFlg == true
                                            select b).Distinct().ToArray();

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
            }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            return data;
        }

        public async Task<NAACCriteriaFiveReportDTO> get_report542HSU(NAACCriteriaFiveReportDTO data)
        {
            try
                {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yearid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.govtsclist = (from a in _GeneralContext.NAAC_AC_542_AlumniContDMO
                                       from b in _GeneralContext.Academic
                                       where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id)&& a.NCAC542ALMCON_ContributionYear == b.ASMAY_Id && a.NCAC542ALMCON_ActiveFlg == true && b.Is_Active == true && yearid.Contains(a.NCAC542ALMCON_ContributionYear)
                                       select new NAACCriteriaFiveReportDTO
                                       {

                                           MI_Id = b.MI_Id,
                                           NCAC542ALMCON_Id = a.NCAC542ALMCON_Id,
                                           ASMAY_Year = b.ASMAY_Year,
                                           NCAC531SPCAS_DonationOfBooksFlag = a.NCAC531SPCAS_DonationOfBooksFlag,
                                           NCAC531SPCAS_FinancialORKindFlag = a.NCAC531SPCAS_FinancialORKindFlag,
                                           NCAC531SPCAS_InstendowmentsFlag = a.NCAC531SPCAS_InstendowmentsFlag,
                                           NCAC531SPCAS_StudentexchangesFlag = a.NCAC531SPCAS_StudentexchangesFlag,
                                           NCAC531SPCAS_StudentsplacementFlag = a.NCAC531SPCAS_StudentsplacementFlag,
                                       }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();




                    data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_542_AlumniContDMO
                                            from b in _GeneralContext.NAAC_AC_542_AlumniContFilesDMO
                                            where mid.Contains(a.MI_Id) && a.NCAC542ALMCON_ActiveFlg == true && a.NCAC542ALMCON_Id == b.NCAC542ALMCON_Id && yearid.Contains(a.NCAC542ALMCON_ContributionYear) && b.NCAC542ALMCONF_ActiveFlg == true
                                            select b).Distinct().ToArray();

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
            }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report543(NAACCriteriaFiveReportDTO data)
        {
            try
                {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yearid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.govtsclist = (from a in _GeneralContext.NAAC_AC_543_AlumniMeetingsDMO
                                       from b in _GeneralContext.Academic
                                       where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC543ALMMET_MeetingYear == b.ASMAY_Id && a.NCAC543ALMMET_ActiveFlg == true && b.Is_Active == true && yearid.Contains(a.NCAC543ALMMET_MeetingYear)
                                       select new NAACCriteriaFiveReportDTO
                                       {
                                           NCAC543ALMMET_Id = a.NCAC543ALMMET_Id,
                                           NCAC543ALMMET_MeetingDate = a.NCAC543ALMMET_MeetingDate,
                                           NCAC543ALMMET_NoOfMeetings = a.NCAC543ALMMET_NoOfMeetings,
                                           NCAC543ALMMET_NoOfMemAttnd = a.NCAC543ALMMET_NoOfMemAttnd,
                                           NCAC543ALMMET_TotalAlumniCount = a.NCAC543ALMMET_TotalAlumniCount,
                                           ASMAY_Year = b.ASMAY_Year,
                                           ASMAY_Order = b.ASMAY_Order,
                                       }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();




                    data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_543_AlumniMeetingsDMO
                                            from b in _GeneralContext.NAAC_AC_543_AlumniMeetingsFilesDMO
                                            where  mid.Contains(a.MI_Id) && a.NCAC543ALMMET_ActiveFlg == true && a.NCAC543ALMMET_Id == b.NCAC543ALMMET_Id && yearid.Contains(a.NCAC543ALMMET_MeetingYear) && b.NCAC543ALMMETF_ActiveFlg == true
                                            select b).Distinct().ToArray();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report523(NAACCriteriaFiveReportDTO data)
        {
            try
                {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();


                data.examlist = (from a in _GeneralContext.NAAC_AC_523_QAMastersDMO
                                 from b in _GeneralContext.NAAC_AC_523_QualExamsDMO
                                 where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC523QAMA_ActiveFlg == true && yrid.Contains(b.NCAC523QE_Year) && b.NCAC523QE_ActiveFlg == true && a.NCAC523QAMA_Id == b.NCAC523QAMA_Id
                                 select a).Distinct().OrderBy(t => t.NCAC523QAMA_ExamName).ToArray();


                    data.govtsclist = (from a in _GeneralContext.NAAC_AC_523_QAMastersDMO
                                       from b in _GeneralContext.NAAC_AC_523_QualExamsDMO
                                       from c in _GeneralContext.Academic
                                       where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && b.NCAC523QE_Year == c.ASMAY_Id && a.NCAC523QAMA_ActiveFlg == true && c.Is_Active == true && yrid.Contains(b.NCAC523QE_Year) && b.NCAC523QE_ActiveFlg==true && a.NCAC523QAMA_Id == b.NCAC523QAMA_Id && a.MI_Id==c.MI_Id
                                       select new NAACCriteriaFiveReportDTO
                                       {
                                           NCAC523QAMA_Id = a.NCAC523QAMA_Id,
                                           NCAC523QE_NoOfStudentsappearing = b.NCAC523QE_NoOfStudentsappearing,
                                           NCAC523QE_Id = b.NCAC523QE_Id,
                                           noofstd = b.NCAC523QE_NoOfStudents,
                                           program = a.NCAC523QAMA_ExamName,
                                           ASMAY_Year = c.ASMAY_Year,
                                           ASMAY_Order = c.ASMAY_Order,
                                           ASMAY_Id = c.ASMAY_Id,
                                       }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();




                    data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_523_QAMastersDMO
                                            from b in _GeneralContext.NAAC_AC_523_QualExamsDMO
                                            from c in _GeneralContext.NAAC_AC_523_QualExamsFilesDMO
                                            where mid.Contains(a.MI_Id) && a.NCAC523QAMA_ActiveFlg == true && a.NCAC523QAMA_Id == b.NCAC523QAMA_Id && yrid.Contains(b.NCAC523QE_Year) && b.NCAC523QE_ActiveFlg==true && c.NCAC523QEF_ActiveFlg == true
                                            select c).Distinct().ToArray();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            return data;
        }
        public async Task<NAACCriteriaFiveReportDTO> get_report515(NAACCriteriaFiveReportDTO data)
        {
            try
            {

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }
                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                data.yearlist = (from a in _GeneralContext.Academic
                                 where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                 select new NAACCriteriaFiveReportDTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();


                data.govtsclist = (from a in _GeneralContext.NAAC_AC_515_VETDMO
                                   from b in _GeneralContext.Academic
                                   where a.MI_Id == b.MI_Id && mid.Contains(a.MI_Id) && a.NCAC515VET_Year == b.ASMAY_Id && a.NCAC515VET_ActiveFlg == true && b.Is_Active == true && yrid.Contains(a.NCAC515VET_Year)
                                   select new NAACCriteriaFiveReportDTO
                                   {
                                       NCAC515VET_Id = a.NCAC515VET_Id,
                                       program = a.NCAC515VET_VETProgramName,
                                       noofstd = a.NCAC515VET_NoOfStudents,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMAY_Order = b.ASMAY_Order
                                   }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();




                data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_515_VETDMO
                                        from b in _GeneralContext.NAAC_AC_515_VETFilesDMO
                                        where mid.Contains(a.MI_Id) && a.NCAC515VET_ActiveFlg == true && a.NCAC515VET_Id == b.NCAC515VET_Id && yrid.Contains(a.NCAC515VET_Year) && b.NCAC515VETF_ActiveFlg == true
                                        select b).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
