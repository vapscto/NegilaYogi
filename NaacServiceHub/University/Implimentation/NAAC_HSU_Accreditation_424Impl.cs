using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Implimentation
{
    public class NAAC_HSU_Accreditation_424Impl : Interface.NAAC_HSU_Accreditation_424Interface
    {
        public GeneralContext _context;
        public NAAC_HSU_Accreditation_424Impl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_HSU_Accreditation_424_DTO loaddata(NAAC_HSU_Accreditation_424_DTO data)
        {
            try
            {

                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
                                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_Accreditation_424DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCHSUA424_Year)
                                 select new NAAC_HSU_Accreditation_424_DTO
                                 {
                                     NCHSUA424_Id = b.NCHSUA424_Id,

                                     NCHSUA424_Year = b.NCHSUA424_Year,
                                     NCHSUA424_NabhAcrFlag = b.NCHSUA424_NabhAcrFlag,
                                     NCHSUA424_NablAcrFlag = b.NCHSUA424_NablAcrFlag,
                                     NCHSUA424_IntAcrFlag = b.NCHSUA424_IntAcrFlag,
                                     NCHSUA424_ISOCertFlag = b.NCHSUA424_ISOCertFlag,
                                     NCHSUA424_GplorGcplFlag = b.NCHSUA424_GplorGcplFlag,
                                    
                                     ASMAY_Year = a.ASMAY_Year,
                                   
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_Accreditation_424_DTO save(NAAC_HSU_Accreditation_424_DTO data)
        {
            try
            {
                if (data.NCHSUA424_Id == 0)
                {
                    var duplicate = _context.NAAC_HSU_Accreditation_424DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSUA424_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_HSU_Accreditation_424DMO u = new NAAC_HSU_Accreditation_424DMO();
                        u.MI_Id = data.MI_Id;
                        if (data.flag == "a")
                        {
                            u.NCHSUA424_NabhAcrFlag = true;
                            u.NCHSUA424_NablAcrFlag = false;
                            u.NCHSUA424_IntAcrFlag = false;
                            u.NCHSUA424_ISOCertFlag = false;
                            u.NCHSUA424_GplorGcplFlag = false;
                        }
                        else if (data.flag == "b")
                        {
                            u.NCHSUA424_NabhAcrFlag = false;
                            u.NCHSUA424_NablAcrFlag = true;
                            u.NCHSUA424_IntAcrFlag = false;
                            u.NCHSUA424_ISOCertFlag = false;
                            u.NCHSUA424_GplorGcplFlag = false;

                        }
                        else if (data.flag == "c")
                        {
                            u.NCHSUA424_NabhAcrFlag = false;
                            u.NCHSUA424_NablAcrFlag = false;
                            u.NCHSUA424_IntAcrFlag = true;
                            u.NCHSUA424_ISOCertFlag = false;
                            u.NCHSUA424_GplorGcplFlag = false;

                        }
                        else if (data.flag == "d")
                        {
                            u.NCHSUA424_NabhAcrFlag = false;
                            u.NCHSUA424_NablAcrFlag = false;
                            u.NCHSUA424_IntAcrFlag = false;
                            u.NCHSUA424_ISOCertFlag = true;
                            u.NCHSUA424_GplorGcplFlag = false;

                        }
                        else if (data.flag == "e")
                        {
                            u.NCHSUA424_NabhAcrFlag = false;
                            u.NCHSUA424_NablAcrFlag = false;
                            u.NCHSUA424_IntAcrFlag = false;
                            u.NCHSUA424_ISOCertFlag = false;
                            u.NCHSUA424_GplorGcplFlag = true;
                        }

                      
                       
                        u.NCHSUA424_CreatedBy = data.UserId;
                        u.NCHSUA424_UpdatedBy = data.UserId;
                        u.NCHSUA424_CreatedDate = DateTime.Now;
                        u.NCHSUA424_UpdatedDate = DateTime.Now;
                        u.NCHSUA424_Year = data.ASMAY_Id;
                     
                        _context.Add(u);
                      
                        var w = _context.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }

                else if (data.NCHSUA424_Id > 0)
                {
                    var duplicate = _context.NAAC_HSU_Accreditation_424DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSUA424_Id != data.NCHSUA424_Id && t.NCHSUA424_Year == data.NCHSUA424_Year).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.NAAC_HSU_Accreditation_424DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSUA424_Id == data.NCHSUA424_Id).SingleOrDefault();



                        if (data.flag == "a")
                        {
                            j.NCHSUA424_NabhAcrFlag = true;
                            j.NCHSUA424_NablAcrFlag = false;
                            j.NCHSUA424_IntAcrFlag = false;
                            j.NCHSUA424_ISOCertFlag = false;
                            j.NCHSUA424_GplorGcplFlag = false;
                        }
                        else if (data.flag == "b")
                        {
                            j.NCHSUA424_NabhAcrFlag = false;
                            j.NCHSUA424_NablAcrFlag = true;
                            j.NCHSUA424_IntAcrFlag = false;
                            j.NCHSUA424_ISOCertFlag = false;
                            j.NCHSUA424_GplorGcplFlag = false;

                        }
                        else if (data.flag == "c")
                        {
                            j.NCHSUA424_NabhAcrFlag = false;
                            j.NCHSUA424_NablAcrFlag = false;
                            j.NCHSUA424_IntAcrFlag = true;
                            j.NCHSUA424_ISOCertFlag = false;
                            j.NCHSUA424_GplorGcplFlag = false;

                        }
                        else if (data.flag == "d")
                        {
                            j.NCHSUA424_NabhAcrFlag = false;
                            j.NCHSUA424_NablAcrFlag = false;
                            j.NCHSUA424_IntAcrFlag = false;
                            j.NCHSUA424_ISOCertFlag = true;
                            j.NCHSUA424_GplorGcplFlag = false;

                        }
                        else if (data.flag == "e")
                        {
                            j.NCHSUA424_NabhAcrFlag = false;
                            j.NCHSUA424_NablAcrFlag = false;
                            j.NCHSUA424_IntAcrFlag = false;
                            j.NCHSUA424_ISOCertFlag = false;
                            j.NCHSUA424_GplorGcplFlag = true;
                        }


                        j.NCHSUA424_Year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCHSUA424_UpdatedDate = DateTime.Now;
                        j.NCHSUA424_UpdatedBy = data.UserId;
                        _context.Update(j);
                        var r = _context.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
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
        public NAAC_HSU_Accreditation_424_DTO EditData(NAAC_HSU_Accreditation_424_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_Accreditation_424DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCHSUA424_Id == data.NCHSUA424_Id && a.ASMAY_Id == b.NCHSUA424_Year)
                                 select new NAAC_HSU_Accreditation_424_DTO
                                 {
                                     NCHSUA424_Id = b.NCHSUA424_Id,

                                     NCHSUA424_NabhAcrFlag = b.NCHSUA424_NabhAcrFlag,
                                     NCHSUA424_NablAcrFlag = b.NCHSUA424_NablAcrFlag,
                                     NCHSUA424_IntAcrFlag = b.NCHSUA424_IntAcrFlag,
                                     NCHSUA424_ISOCertFlag = b.NCHSUA424_ISOCertFlag,
                                     NCHSUA424_GplorGcplFlag = b.NCHSUA424_GplorGcplFlag,
                                     NCHSUA424_Year = b.NCHSUA424_Year,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
