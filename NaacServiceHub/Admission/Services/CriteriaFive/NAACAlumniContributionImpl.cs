using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACAlumniContributionImpl : Interface.NAACAlumniContributionInterface
    {
        public GeneralContext _context;
        public NAACAlumniContributionImpl(GeneralContext w)
        {
            _context = w;
        }


        public NAACAlumniContributionDTO loaddatahsu(NAACAlumniContributionDTO data)
        {
            try
            {

                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
                                       where a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                /////////////////

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.alldatalist = (from a in _context.Academic
                                    from b in _context.NAAC_AC_542_AlumniContDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC542ALMCON_ContributionYear == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACAlumniContributionDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC542ALMCON_Id = b.NCAC542ALMCON_Id,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC531SPCAS_DonationOfBooksFlag = b.NCAC531SPCAS_DonationOfBooksFlag,
                                        NCAC531SPCAS_FinancialORKindFlag = b.NCAC531SPCAS_FinancialORKindFlag,
                                        NCAC531SPCAS_InstendowmentsFlag = b.NCAC531SPCAS_InstendowmentsFlag,
                                        NCAC531SPCAS_StudentexchangesFlag = b.NCAC531SPCAS_StudentexchangesFlag,
                                        NCAC531SPCAS_StudentsplacementFlag = b.NCAC531SPCAS_StudentsplacementFlag,
                                        NCAC542ALMCON_ActiveFlg = b.NCAC542ALMCON_ActiveFlg,
                                        NCAC542ALMCON_StatusFlg = b.NCAC542ALMCON_StatusFlg
                                    }).Distinct().OrderByDescending(t => t.NCAC542ALMCON_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAACAlumniContributionDTO loaddata(NAACAlumniContributionDTO data)
        {
            try
            {

                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
                                       where a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                /////////////////

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.alldatalist = (from a in _context.Academic
                                    from b in _context.NAAC_AC_542_AlumniContDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC542ALMCON_ContributionYear == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACAlumniContributionDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC542ALMCON_Id = b.NCAC542ALMCON_Id,
                                        NCAC542ALMCON_AlumnsName = b.NCAC542ALMCON_AlumnsName,
                                        NCAC542ALMCON_AadharPAN = b.NCAC542ALMCON_AadharPAN,
                                        NCAC542ALMCON_ActiveFlg = b.NCAC542ALMCON_ActiveFlg,
                                        NCAC542ALMCON_GraduationYear = b.NCAC542ALMCON_GraduationYear,
                                        NCAC542ALMCON_ContriAmount = b.NCAC542ALMCON_ContriAmount,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC542ALMCON_StatusFlg = b.NCAC542ALMCON_StatusFlg,
                                        ASMAY_Year1 = _context.Academic.Where(w => w.ASMAY_Id == b.NCAC542ALMCON_GraduationYear && w.Is_Active).Select(e => e.ASMAY_Year).SingleOrDefault(),
                                    }).Distinct().OrderByDescending(t => t.NCAC542ALMCON_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
       
        public NAACAlumniContributionDTO save(NAACAlumniContributionDTO data)
        {
            try
            {
                if (data.NCAC542ALMCON_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_542_AlumniContDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC542ALMCON_AlumnsName == data.NCAC542ALMCON_AlumnsName && t.NCAC542ALMCON_ContributionYear == data.NCAC542ALMCON_ContributionYear && t.NCAC542ALMCON_ContriAmount == data.NCAC542ALMCON_ContriAmount && t.NCAC542ALMCON_GraduationYear == data.NCAC542ALMCON_GraduationYear && t.NCAC542ALMCON_AadharPAN == data.NCAC542ALMCON_AadharPAN).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_542_AlumniContDMO obj1 = new NAAC_AC_542_AlumniContDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC542ALMCON_ContributionYear = data.NCAC542ALMCON_ContributionYear;
                        obj1.NCAC542ALMCON_AlumnsName = data.NCAC542ALMCON_AlumnsName;
                        obj1.NCAC542ALMCON_AadharPAN = data.NCAC542ALMCON_AadharPAN;
                        obj1.NCAC542ALMCON_GraduationYear = data.NCAC542ALMCON_GraduationYear;
                        obj1.NCAC542ALMCON_ContriAmount = data.NCAC542ALMCON_ContriAmount;
                        obj1.NCAC542ALMCON_AreaOfContribution = data.NCAC542ALMCON_AreaOfContribution;

                        obj1.NCAC542ALMCON_ActiveFlg = true;
                        obj1.NCAC542ALMCON_CreatedBy = data.UserId;
                        obj1.NCAC542ALMCON_UpdatedBy = data.UserId;
                        obj1.NCAC542ALMCON_CreatedDate = DateTime.Now;
                        obj1.NCAC542ALMCON_UpdatedDate = DateTime.Now;
                        obj1.NCAC531SPCAS_FinancialORKindFlag = data.NCAC531SPCAS_FinancialORKindFlag;
                        obj1.NCAC531SPCAS_DonationOfBooksFlag = data.NCAC531SPCAS_DonationOfBooksFlag;
                        obj1.NCAC531SPCAS_StudentsplacementFlag = data.NCAC531SPCAS_StudentsplacementFlag;
                        obj1.NCAC531SPCAS_StudentexchangesFlag = data.NCAC531SPCAS_StudentexchangesFlag;
                        obj1.NCAC531SPCAS_InstendowmentsFlag = data.NCAC531SPCAS_InstendowmentsFlag;
                        obj1.NCAC542ALMCON_StatusFlg ="";
                        _context.Add(obj1);


                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_542_AlumniContFilesDMO obb = new NAAC_AC_542_AlumniContFilesDMO();


                                    obb.NCAC542ALMCON_Id = obj1.NCAC542ALMCON_Id;
                                    obb.NCAC542ALMCONF_FileName = item.cfilename;
                                    obb.NCAC542ALMCONF_FilePath = item.cfilepath;
                                    obb.NCAC542ALMCONF_Filedesc = item.cfiledesc;
                                    obb.NCAC542ALMCONF_StatusFlg = "";
                                    obb.NCAC542ALMCONF_ActiveFlg = true;

                                    _context.Add(obb);
                                }


                            }
                        }
                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCAC542ALMCON_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_542_AlumniContDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC542ALMCON_AlumnsName == data.NCAC542ALMCON_AlumnsName && t.NCAC542ALMCON_ContributionYear == data.NCAC542ALMCON_ContributionYear && t.NCAC542ALMCON_ContriAmount == data.NCAC542ALMCON_ContriAmount && t.NCAC542ALMCON_GraduationYear == data.NCAC542ALMCON_GraduationYear && t.NCAC542ALMCON_Id != data.NCAC542ALMCON_Id && t.NCAC542ALMCON_AadharPAN == data.NCAC542ALMCON_AadharPAN).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        var update = _context.NAAC_AC_542_AlumniContDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC542ALMCON_ContributionYear = data.NCAC542ALMCON_ContributionYear;
                        update.NCAC542ALMCON_AlumnsName = data.NCAC542ALMCON_AlumnsName;
                        update.NCAC542ALMCON_AadharPAN = data.NCAC542ALMCON_AadharPAN;
                        update.NCAC542ALMCON_GraduationYear = data.NCAC542ALMCON_GraduationYear;
                        update.NCAC542ALMCON_ContriAmount = data.NCAC542ALMCON_ContriAmount;
                        update.NCAC542ALMCON_AreaOfContribution = data.NCAC542ALMCON_AreaOfContribution;
                        update.NCAC542ALMCON_ActiveFlg = true;
                        update.NCAC542ALMCON_UpdatedBy = data.UserId;
                        update.NCAC542ALMCON_UpdatedDate = DateTime.Now;
                        update.NCAC531SPCAS_FinancialORKindFlag = data.NCAC531SPCAS_FinancialORKindFlag;
                        update.NCAC531SPCAS_DonationOfBooksFlag = data.NCAC531SPCAS_DonationOfBooksFlag;
                        update.NCAC531SPCAS_StudentsplacementFlag = data.NCAC531SPCAS_StudentsplacementFlag;
                        update.NCAC531SPCAS_StudentexchangesFlag = data.NCAC531SPCAS_StudentexchangesFlag;
                        update.NCAC531SPCAS_InstendowmentsFlag = data.NCAC531SPCAS_InstendowmentsFlag;
                        _context.Update(update);
                        
                         if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_542_AlumniContFilesDMO.Where(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id && !Fid.Contains(t.NCAC542ALMCONF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_542_AlumniContFilesDMO.Single(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id && t.NCAC542ALMCONF_Id == item2.NCAC542ALMCONF_Id);
                                    deactfile.NCAC542ALMCONF_ActiveFlg = false;
                                    _context.Update(deactfile);

                                }

                            }



                            foreach (var item in data.filelist)
                            {
                                if (item.status == null)
                                {
                                    item.status = "";
                                }

                                if (item.cfileid > 0 && item.status.ToLower() != "approved")
                                {
                                    var filesdata = _context.NAAC_AC_542_AlumniContFilesDMO.Where(t => t.NCAC542ALMCONF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC542ALMCON_Id = data.NCAC542ALMCON_Id;
                                    filesdata.NCAC542ALMCONF_FileName = item.cfilename;
                                    filesdata.NCAC542ALMCONF_FilePath = item.cfilepath;
                                    filesdata.NCAC542ALMCONF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC542ALMCONF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_542_AlumniContFilesDMO obb = new NAAC_AC_542_AlumniContFilesDMO();
                                            obb.NCAC542ALMCON_Id = data.NCAC542ALMCON_Id;
                                            obb.NCAC542ALMCONF_FileName = item.cfilename;
                                            obb.NCAC542ALMCONF_FilePath = item.cfilepath;
                                            obb.NCAC542ALMCONF_Filedesc = item.cfiledesc;
                                            obb.NCAC542ALMCONF_ActiveFlg = true;
                                            obb.NCAC542ALMCONF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_542_AlumniContFilesDMO.Where(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_542_AlumniContFilesDMO.Single(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id && t.NCAC542ALMCONF_Id == item.NCAC542ALMCONF_Id);
                                    deactfile.NCAC542ALMCONF_ActiveFlg = false;
                                    _context.Update(removefile1);
                                }
                            }
                        }


                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }


        public NAACAlumniContributionDTO savehsu(NAACAlumniContributionDTO data)
        {
            try
            {
                if (data.NCAC542ALMCON_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_542_AlumniContDMO.Where(t => t.MI_Id == data.MI_Id &&   t.NCAC542ALMCON_ContributionYear == data.NCAC542ALMCON_ContributionYear).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_542_AlumniContDMO obj1 = new NAAC_AC_542_AlumniContDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC542ALMCON_ContributionYear = data.NCAC542ALMCON_ContributionYear;
                        obj1.NCAC542ALMCON_AlumnsName = data.NCAC542ALMCON_AlumnsName;
                        obj1.NCAC542ALMCON_AadharPAN = data.NCAC542ALMCON_AadharPAN;
                        obj1.NCAC542ALMCON_GraduationYear = data.NCAC542ALMCON_GraduationYear;
                        obj1.NCAC542ALMCON_ContriAmount = data.NCAC542ALMCON_ContriAmount;
                        obj1.NCAC542ALMCON_AreaOfContribution = data.NCAC542ALMCON_AreaOfContribution;

                        obj1.NCAC542ALMCON_ActiveFlg = true;
                        obj1.NCAC542ALMCON_CreatedBy = data.UserId;
                        obj1.NCAC542ALMCON_UpdatedBy = data.UserId;
                        obj1.NCAC542ALMCON_CreatedDate = DateTime.Now;
                        obj1.NCAC542ALMCON_UpdatedDate = DateTime.Now;
                        obj1.NCAC531SPCAS_FinancialORKindFlag = data.NCAC531SPCAS_FinancialORKindFlag;
                        obj1.NCAC531SPCAS_DonationOfBooksFlag = data.NCAC531SPCAS_DonationOfBooksFlag;
                        obj1.NCAC531SPCAS_StudentsplacementFlag = data.NCAC531SPCAS_StudentsplacementFlag;
                        obj1.NCAC531SPCAS_StudentexchangesFlag = data.NCAC531SPCAS_StudentexchangesFlag;
                        obj1.NCAC531SPCAS_InstendowmentsFlag = data.NCAC531SPCAS_InstendowmentsFlag;
                        obj1.NCAC542ALMCON_StatusFlg = "";
                        _context.Add(obj1);


                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_542_AlumniContFilesDMO obb = new NAAC_AC_542_AlumniContFilesDMO();


                                    obb.NCAC542ALMCON_Id = obj1.NCAC542ALMCON_Id;
                                    obb.NCAC542ALMCONF_FileName = item.cfilename;
                                    obb.NCAC542ALMCONF_FilePath = item.cfilepath;
                                    obb.NCAC542ALMCONF_Filedesc = item.cfiledesc;
                                    obb.NCAC542ALMCONF_StatusFlg = "";
                                    obb.NCAC542ALMCONF_ActiveFlg = true;

                                    _context.Add(obb);
                                }


                            }
                        }
                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCAC542ALMCON_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_542_AlumniContDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC542ALMCON_ContributionYear == data.NCAC542ALMCON_ContributionYear && t.NCAC542ALMCON_Id != data.NCAC542ALMCON_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        

                        var update = _context.NAAC_AC_542_AlumniContDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC542ALMCON_ContributionYear = data.NCAC542ALMCON_ContributionYear;
                        update.NCAC542ALMCON_AlumnsName = data.NCAC542ALMCON_AlumnsName;
                        update.NCAC542ALMCON_AadharPAN = data.NCAC542ALMCON_AadharPAN;
                        update.NCAC542ALMCON_GraduationYear = data.NCAC542ALMCON_GraduationYear;
                        update.NCAC542ALMCON_ContriAmount = data.NCAC542ALMCON_ContriAmount;
                        update.NCAC542ALMCON_AreaOfContribution = data.NCAC542ALMCON_AreaOfContribution;
                        update.NCAC542ALMCON_ActiveFlg = true;
                        update.NCAC542ALMCON_UpdatedBy = data.UserId;
                        update.NCAC542ALMCON_UpdatedDate = DateTime.Now;
                        update.NCAC531SPCAS_FinancialORKindFlag = data.NCAC531SPCAS_FinancialORKindFlag;
                        update.NCAC531SPCAS_DonationOfBooksFlag = data.NCAC531SPCAS_DonationOfBooksFlag;
                        update.NCAC531SPCAS_StudentsplacementFlag = data.NCAC531SPCAS_StudentsplacementFlag;
                        update.NCAC531SPCAS_StudentexchangesFlag = data.NCAC531SPCAS_StudentexchangesFlag;
                        update.NCAC531SPCAS_InstendowmentsFlag = data.NCAC531SPCAS_InstendowmentsFlag;
                        _context.Update(update);

                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_542_AlumniContFilesDMO.Where(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id && !Fid.Contains(t.NCAC542ALMCONF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_542_AlumniContFilesDMO.Single(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id && t.NCAC542ALMCONF_Id == item2.NCAC542ALMCONF_Id);
                                    deactfile.NCAC542ALMCONF_ActiveFlg = false;
                                    _context.Update(deactfile);

                                }

                            }



                            foreach (var item in data.filelist)
                            {
                                if (item.status == null)
                                {
                                    item.status = "";
                                }

                                if (item.cfileid > 0 && item.status.ToLower() != "approved")
                                {
                                    var filesdata = _context.NAAC_AC_542_AlumniContFilesDMO.Where(t => t.NCAC542ALMCONF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC542ALMCON_Id = data.NCAC542ALMCON_Id;
                                    filesdata.NCAC542ALMCONF_FileName = item.cfilename;
                                    filesdata.NCAC542ALMCONF_FilePath = item.cfilepath;
                                    filesdata.NCAC542ALMCONF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC542ALMCONF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_542_AlumniContFilesDMO obb = new NAAC_AC_542_AlumniContFilesDMO();
                                            obb.NCAC542ALMCON_Id = data.NCAC542ALMCON_Id;
                                            obb.NCAC542ALMCONF_FileName = item.cfilename;
                                            obb.NCAC542ALMCONF_FilePath = item.cfilepath;
                                            obb.NCAC542ALMCONF_Filedesc = item.cfiledesc;
                                            obb.NCAC542ALMCONF_ActiveFlg = true;
                                            obb.NCAC542ALMCONF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_542_AlumniContFilesDMO.Where(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_542_AlumniContFilesDMO.Single(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id && t.NCAC542ALMCONF_Id == item.NCAC542ALMCONF_Id);
                                    deactfile.NCAC542ALMCONF_ActiveFlg = false;
                                    _context.Update(removefile1);
                                }
                            }
                        }


                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACAlumniContributionDTO deactiveStudent(NAACAlumniContributionDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_542_AlumniContDMO.Where(t => t.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id).SingleOrDefault();
                if (data.NCAC542ALMCON_ActiveFlg == true)
                {
                    u.NCAC542ALMCON_ActiveFlg = false;
                }
                else if (u.NCAC542ALMCON_ActiveFlg == false)
                {
                    u.NCAC542ALMCON_ActiveFlg = true;
                }
                u.NCAC542ALMCON_UpdatedDate = DateTime.Now;
                u.NCAC542ALMCON_UpdatedBy = data.UserId;
                _context.Update(u);
                int o = _context.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAACAlumniContributionDTO EditData(NAACAlumniContributionDTO data)
        {
            try
            {
                data.editlist = (from b in _context.NAAC_AC_542_AlumniContDMO
                                 where (b.MI_Id == data.MI_Id && b.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id)
                                 select new NAACAlumniContributionDTO
                                 {
                                     NCAC542ALMCON_Id = b.NCAC542ALMCON_Id,
                                     NCAC542ALMCON_AlumnsName = b.NCAC542ALMCON_AlumnsName,
                                     NCAC542ALMCON_AadharPAN = b.NCAC542ALMCON_AadharPAN,
                                     NCAC542ALMCON_ActiveFlg = b.NCAC542ALMCON_ActiveFlg,
                                     NCAC542ALMCON_ContributionYear = b.NCAC542ALMCON_ContributionYear,
                                     NCAC542ALMCON_GraduationYear = b.NCAC542ALMCON_GraduationYear,
                                     NCAC542ALMCON_ContriAmount = b.NCAC542ALMCON_ContriAmount,
                                     NCAC542ALMCON_AreaOfContribution = b.NCAC542ALMCON_AreaOfContribution,
                                     NCAC531SPCAS_FinancialORKindFlag=b.  NCAC531SPCAS_FinancialORKindFlag,
                                     NCAC531SPCAS_DonationOfBooksFlag = b.NCAC531SPCAS_DonationOfBooksFlag,
                                     NCAC531SPCAS_StudentsplacementFlag = b.NCAC531SPCAS_StudentsplacementFlag,
                                     NCAC531SPCAS_StudentexchangesFlag = b.NCAC531SPCAS_StudentexchangesFlag,
                                     NCAC531SPCAS_InstendowmentsFlag = b.NCAC531SPCAS_InstendowmentsFlag,
                                     NCAC542ALMCON_StatusFlg = b.NCAC542ALMCON_StatusFlg 

                                 }).Distinct().ToArray();

                data.editfiles = (from a in _context.NAAC_AC_542_AlumniContFilesDMO

                                  where (a.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id && a.NCAC542ALMCONF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC542ALMCONF_FileName,
                                      cfilepath = a.NCAC542ALMCONF_FilePath,
                                      cfiledesc = a.NCAC542ALMCONF_Filedesc,
                                      status = a.NCAC542ALMCONF_StatusFlg,
                                      cfileid = a.NCAC542ALMCONF_Id,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniContributionDTO viewuploadflies(NAACAlumniContributionDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_542_AlumniContFilesDMO

                                  where (a.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id && a.NCAC542ALMCONF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC542ALMCON_Id,
                                      cfileid = a.NCAC542ALMCONF_Id,
                                      cfilename = a.NCAC542ALMCONF_FileName,
                                      cfilepath = a.NCAC542ALMCONF_FilePath,
                                      cfiledesc = a.NCAC542ALMCONF_Filedesc,
                                      status = a.NCAC542ALMCONF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniContributionDTO deleteuploadfile(NAACAlumniContributionDTO data)
        {
            try
            {
                if (data.NCAC542ALMCONF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_542_AlumniContFilesDMO.Where(e => e.NCAC542ALMCONF_Id == data.NCAC542ALMCONF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC542ALMCONF_ActiveFlg = false;
                            _context.Update(item);
                        }
                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniContributionDTO getcomment(NAACAlumniContributionDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_542_AlumniCont_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC542ALMCONC_RemarksBy == b.Id && a.NCAC542ALMCON_Id == data.NCAC542ALMCON_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC542ALMCONC_Remarks,
                                        commentid = a.NCAC542ALMCONC_Id,
                                        status = a.NCAC542ALMCONC_StatusFlg,
                                        createddate = a.NCAC542ALMCONC_CreatedDate,
                                        activeflag = a.NCAC542ALMCONC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniContributionDTO getfilecomment(NAACAlumniContributionDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_542_AlumniCont_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC542ALMCONFC_RemarksBy == b.Id && a.NCAC542ALMCONF_Id == data.NCAC542ALMCONF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC542ALMCONFC_Remarks,
                                        commentid = a.NCAC542ALMCONFC_Id,
                                        status = a.NCAC542ALMCONFC_StatusFlg,
                                        createddate = a.NCAC542ALMCONFC_CreatedDate,
                                        activeflag = a.NCAC542ALMCONFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniContributionDTO savemedicaldatawisecomments(NAACAlumniContributionDTO data)
        {
            try
            {
                NAAC_AC_542_AlumniCont_CommentsDMO cm = new NAAC_AC_542_AlumniCont_CommentsDMO();
                cm.NCAC542ALMCONC_Remarks = data.Remarks;
                cm.NCAC542ALMCONC_RemarksBy = data.UserId;
                cm.NCAC542ALMCONC_StatusFlg = "";
                cm.NCAC542ALMCONC_ActiveFlag = true;
                cm.NCAC542ALMCONC_CreatedBy = data.UserId;
                cm.NCAC542ALMCONC_CreatedDate = DateTime.Now;
                cm.NCAC542ALMCONC_UpdatedBy = data.UserId;
                cm.NCAC542ALMCONC_UpdatedDate = DateTime.Now;
                cm.NCAC542ALMCON_Id = data.filefkid;
                _context.Add(cm);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniContributionDTO savefilewisecomments(NAACAlumniContributionDTO data)
        {
            try
            {
                NAAC_AC_542_AlumniCont_File_CommentsDMO cm = new NAAC_AC_542_AlumniCont_File_CommentsDMO();
                cm.NCAC542ALMCONFC_Remarks = data.Remarks;
                cm.NCAC542ALMCONFC_RemarksBy = data.UserId;
                cm.NCAC542ALMCONFC_StatusFlg = "";
                cm.NCAC542ALMCONFC_ActiveFlag = true;
                cm.NCAC542ALMCONFC_CreatedBy = data.UserId;
                cm.NCAC542ALMCONFC_CreatedDate = DateTime.Now;
                cm.NCAC542ALMCONFC_UpdatedBy = data.UserId;
                cm.NCAC542ALMCONFC_UpdatedDate = DateTime.Now;
                cm.NCAC542ALMCONF_Id = data.filefkid;
                _context.Add(cm);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
    }
}
