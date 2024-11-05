using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class NAAC_CommonDetails
    {
        public GeneralContext _context;
        public NAAC_CommonDetails(GeneralContext db)
        {
            _context = db;
        }
        public Array get_cycle_list(long MI_Id, long Userid)
        {
            List<NaacConsolidatProcessDTO> getinstitutioncycle = new List<NaacConsolidatProcessDTO>();

            var getinstitution = _context.Institution.Where(a => a.MI_Id == MI_Id).ToList();

            string NAACSL_InstitutionTypeFlg = "";
            string NAACSL_SchoolClgUniversity = "";

            if (getinstitution.Count() > 0)
            {
                NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg.ToUpper();
                NAACSL_SchoolClgUniversity = getinstitution.FirstOrDefault().MI_SchoolCollegeFlag.ToUpper();
            }


            if (NAACSL_SchoolClgUniversity.ToUpper() == "U")
            {
                var getorganization = _context.Institution.Where(a => a.MI_Id == MI_Id && a.MI_ActiveFlag == 1).Distinct().ToList();

                getinstitutioncycle = (from a in _context.NAAC_Master_Trust_CycleDMO
                                       where (a.MO_Id == getorganization.FirstOrDefault().MO_Id && a.NCMATC_ActiveFlg == true)
                                       select new NaacConsolidatProcessDTO
                                       {
                                           cycleid = a.NCMATC_Id,
                                           cyclename = a.NCMATC_NAACCycle,
                                           cycleorder = a.NCMATC_Order
                                       }).Distinct().OrderByDescending(a => a.cycleorder).ToList();

            }
            else
            {

                List<long> miid = new List<long>();

                var getmiid = (from a in _context.NAAC_User_PrivilegeDMO
                               from b in _context.NAAC_User_Privilege_InstitutionDMO
                               where (a.NAACUPRI_Id == b.NAACUPRI_Id && a.IVRMUL_Id == Userid && a.NAACUPRI_ActiveFlag == true && b.NAACUPRIIN_ActiveFlag == true)
                               select new NaacConsolidatProcessDTO
                               {
                                   MI_Id = b.MI_Id

                               }).Distinct().ToList();

                foreach (var c in getmiid)
                {
                    miid.Add(c.MI_Id);
                }


                getinstitutioncycle = (from a in _context.NAAC_Master_CycleDMO
                                       where (a.MI_Id == MI_Id && a.NCMACY_ActiveFlg == true)
                                       select new NaacConsolidatProcessDTO
                                       {
                                           cycleid = a.NCMACY_Id,
                                           cyclename = a.NCMACY_NAACCycle,
                                           cycleorder = a.NCMACY_Order
                                       }).Distinct().OrderByDescending(a => a.cycleorder).ToList();
            }

            return getinstitutioncycle.ToArray();
        }
        public Array get_Institution_list(long MI_Id, long Userid)
        {
            List<Institution> isnt = new List<Institution>();

            var miid = _context.UserRoleWithInstituteDMO.Where(t => t.Id == Userid && t.Activeflag == 1).ToList();

            List<long> miids = new List<long>();

            foreach (var item in miid)
            {
                miids.Add(item.MI_Id);
            }

            isnt = _context.Institution.Where(a => miids.Contains(a.MI_Id)).ToList();

            return isnt.ToArray();
        }
        public string get_Year_list(long MI_Id, long Userid, long cycleid)
        {
            string yearids = "0";
            var getinstitution = _context.Institution.Where(a => a.MI_Id == MI_Id).ToList();

            string NAACSL_InstitutionTypeFlg = "";
            string NAACSL_SchoolClgUniversity = "";

            if (getinstitution.Count() > 0)
            {
                NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg.ToUpper();
                NAACSL_SchoolClgUniversity = getinstitution.FirstOrDefault().MI_SchoolCollegeFlag.ToUpper();
            }

            List<NaacConsolidatProcessDTO> dto = new List<NaacConsolidatProcessDTO>();

            if (NAACSL_SchoolClgUniversity.ToUpper() == "U")
            {

                dto = (from a in _context.NAAC_Master_CycleDMO
                       from b in _context.NAAC_Master_Cycle_YearDMO
                       from c in _context.NAAC_Master_Trust_CycleDMO
                       from d in _context.NAAC_Master_Trust_Cycle_MappingDMO
                       where (a.NCMACY_Id == b.NCMACY_Id && c.NCMATC_Id == d.NCMATC_Id && d.NCMACY_Id == a.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && c.NCMATC_ActiveFlg == true && c.NCMATC_Id == cycleid)
                       select new NaacConsolidatProcessDTO
                       {
                           ASMAY_Id = b.ASMAY_Id
                       }).Distinct().ToList();
            }
            else
            {
                dto = (from a in _context.NAAC_Master_CycleDMO
                       from b in _context.NAAC_Master_Cycle_YearDMO
                       where (a.NCMACY_Id == b.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && a.NCMACY_Id == cycleid)
                       select new NaacConsolidatProcessDTO
                       {
                           ASMAY_Id = b.ASMAY_Id
                       }).Distinct().ToList();
            }

            foreach (var c in dto)
            {
                yearids = yearids + "," + c.ASMAY_Id;
            }

            return yearids;
        }
        public string get_mi_id_list(long MI_Id, long Userid, long cycleid)
        {
            string miids = "0";

            List<long> miid = new List<long>();

            var getmiid = (from a in _context.NAAC_User_PrivilegeDMO
                           from b in _context.NAAC_User_Privilege_InstitutionDMO
                           where (a.NAACUPRI_Id == b.NAACUPRI_Id && a.IVRMUL_Id == Userid && a.NAACUPRI_ActiveFlag == true && b.NAACUPRIIN_ActiveFlag == true)
                           select new NaacConsolidatProcessDTO
                           {
                               MI_Id = b.MI_Id

                           }).Distinct().ToList();

            foreach (var c in getmiid)
            {
                miids = miids + "," + c.MI_Id;
                miid.Add(c.MI_Id);
            }

            return miids;

        }
        public List<long> get_Institution_User_MI_Id_list(long MI_Id, long Userid)
        {
            var getmiid = (from a in _context.NAAC_User_PrivilegeDMO
                           from b in _context.NAAC_User_Privilege_InstitutionDMO
                           where (a.NAACUPRI_Id == b.NAACUPRI_Id && a.IVRMUL_Id == Userid && a.NAACUPRI_ActiveFlag == true && b.NAACUPRIIN_ActiveFlag == true)
                           select new NaacConsolidatProcessDTO
                           {
                               MI_Id = b.MI_Id

                           }).Distinct().ToList();

            List<long> miids = new List<long>();

            foreach (var item in getmiid)
            {
                miids.Add(item.MI_Id);
            }

            return miids;
        }
        public Array get_Institution_User_list(long MI_Id, long Userid)
        {
            List<Institution> isnt = new List<Institution>();

            var getmiid = (from a in _context.NAAC_User_PrivilegeDMO
                           from b in _context.NAAC_User_Privilege_InstitutionDMO
                           where (a.NAACUPRI_Id == b.NAACUPRI_Id && a.IVRMUL_Id == Userid && a.NAACUPRI_ActiveFlag == true && b.NAACUPRIIN_ActiveFlag == true)
                           select new NaacConsolidatProcessDTO
                           {
                               MI_Id = b.MI_Id

                           }).Distinct().ToList();

            List<long> miids = new List<long>();

            foreach (var item in getmiid)
            {
                miids.Add(item.MI_Id);
            }

            isnt = _context.Institution.Where(a => miids.Contains(a.MI_Id)).ToList();

            return isnt.ToArray();
        }
        public List<long> get_Year_listLong(long MI_Id, long Userid, long cycleid)
        {
            List<long> yearids = new List<long>();
            var getinstitution = _context.Institution.Where(a => a.MI_Id == MI_Id).ToList();

            string NAACSL_InstitutionTypeFlg = "";
            string NAACSL_SchoolClgUniversity = "";

            if (getinstitution.Count() > 0)
            {
                NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg.ToUpper();
                NAACSL_SchoolClgUniversity = getinstitution.FirstOrDefault().MI_SchoolCollegeFlag.ToUpper();
            }

            List<NaacConsolidatProcessDTO> dto = new List<NaacConsolidatProcessDTO>();

            if (NAACSL_SchoolClgUniversity.ToUpper() == "U")
            {

                dto = (from a in _context.NAAC_Master_CycleDMO
                       from b in _context.NAAC_Master_Cycle_YearDMO
                       from c in _context.NAAC_Master_Trust_CycleDMO
                       from d in _context.NAAC_Master_Trust_Cycle_MappingDMO
                       where (a.NCMACY_Id == b.NCMACY_Id && c.NCMATC_Id == d.NCMATC_Id && d.NCMACY_Id == a.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && c.NCMATC_ActiveFlg == true && c.NCMATC_Id == cycleid)
                       select new NaacConsolidatProcessDTO
                       {
                           ASMAY_Id = b.ASMAY_Id
                       }).Distinct().ToList();
            }
            else
            {
                dto = (from a in _context.NAAC_Master_CycleDMO
                       from b in _context.NAAC_Master_Cycle_YearDMO
                       where (a.NCMACY_Id == b.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && a.NCMACY_Id == cycleid)
                       select new NaacConsolidatProcessDTO
                       {
                           ASMAY_Id = b.ASMAY_Id
                       }).Distinct().ToList();
            }

            foreach (var c in dto)
            {
                yearids.Add(c.ASMAY_Id);
            }

            return yearids;
        }
    }
}
