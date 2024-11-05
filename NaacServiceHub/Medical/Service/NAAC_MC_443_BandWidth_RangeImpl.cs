using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class NAAC_MC_443_BandWidth_RangeImpl:Interface.NAAC_MC_443_BandWidth_RangeInterface
    {
        public GeneralContext _context;
        public NAAC_MC_443_BandWidth_RangeImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_MC_443_BandWidth_Range_DTO loaddata(NAAC_MC_443_BandWidth_Range_DTO data)
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
                                 from b in _context.NAAC_MC_443_BandWidth_RangeDMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCMC443BWR_Year)
                                 select new NAAC_MC_443_BandWidth_Range_DTO
                                 {
                                     NCMC443BWR_Id = b.NCMC443BWR_Id,
                                     NCMC443BWR_Range = b.NCMC443BWR_Range,
                                     NCMC443BWR_Year = b.NCMC443BWR_Year,
                                     NCMC443BWR_OneOrMoreGBPS = b.NCMC443BWR_OneOrMoreGBPS,
                                     NCMC443BWR_500MBPSTo1GBPS = b.NCMC443BWR_500MBPSTo1GBPS,
                                     NCMC443BWR_250MBPSTo500MBPS = b.NCMC443BWR_250MBPSTo500MBPS,
                                     NCMC443BWR_50MBPSTo250MBPS = b.NCMC443BWR_50MBPSTo250MBPS,
                                     NCMC443BWR_LessThan50MBPS = b.NCMC443BWR_LessThan50MBPS,
                                    
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
        public NAAC_MC_443_BandWidth_Range_DTO save(NAAC_MC_443_BandWidth_Range_DTO data)
        {
            try
            {
                if (data.NCMC443BWR_Id == 0)
                {
                    var duplicate = _context.NAAC_MC_443_BandWidth_RangeDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC443BWR_Range == data.NCMC443BWR_Range&& t.NCMC443BWR_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_MC_443_BandWidth_RangeDMO u = new NAAC_MC_443_BandWidth_RangeDMO();
                        u.MI_Id = data.MI_Id;
                        if (data.flag == "a")
                        {
                            u.NCMC443BWR_OneOrMoreGBPS = true;
                            u.NCMC443BWR_500MBPSTo1GBPS = false;
                            u.NCMC443BWR_250MBPSTo500MBPS = false;
                            u.NCMC443BWR_50MBPSTo250MBPS = false;
                            u.NCMC443BWR_LessThan50MBPS = false;
                        }
                        else if (data.flag == "b")
                        {
                            u.NCMC443BWR_OneOrMoreGBPS = false;
                            u.NCMC443BWR_500MBPSTo1GBPS = true;
                            u.NCMC443BWR_250MBPSTo500MBPS = false;
                            u.NCMC443BWR_50MBPSTo250MBPS = false;
                            u.NCMC443BWR_LessThan50MBPS = false;

                        }
                        else if (data.flag == "c")
                        {
                            u.NCMC443BWR_OneOrMoreGBPS = false;
                            u.NCMC443BWR_500MBPSTo1GBPS = false;
                            u.NCMC443BWR_250MBPSTo500MBPS = true;
                            u.NCMC443BWR_50MBPSTo250MBPS = false;
                            u.NCMC443BWR_LessThan50MBPS = false;

                        }
                        else if (data.flag == "d")
                        {
                            u.NCMC443BWR_OneOrMoreGBPS = false;
                            u.NCMC443BWR_500MBPSTo1GBPS = false;
                            u.NCMC443BWR_250MBPSTo500MBPS = false;
                            u.NCMC443BWR_50MBPSTo250MBPS = true;
                            u.NCMC443BWR_LessThan50MBPS = false;

                        }
                        else if (data.flag == "e")
                        {
                            u.NCMC443BWR_OneOrMoreGBPS = false;
                            u.NCMC443BWR_500MBPSTo1GBPS = false;
                            u.NCMC443BWR_250MBPSTo500MBPS = false;
                            u.NCMC443BWR_50MBPSTo250MBPS = false;
                            u.NCMC443BWR_LessThan50MBPS = true;
                        }

                        u.NCMC443BWR_Range = data.NCMC443BWR_Range;
                       
                        u.NCMC443BWR_CreatedBy = data.UserId;
                        u.NCMC443BWR_UpdatedBy = data.UserId;
                        u.NCMC443BWR_CreatedDate = DateTime.Now;
                        u.NCMC443BWR_UpdatedDate = DateTime.Now;
                        u.NCMC443BWR_Year = data.ASMAY_Id;
                     
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

                else if (data.NCMC443BWR_Id > 0)
                {
                    var duplicate = _context.NAAC_MC_443_BandWidth_RangeDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC443BWR_Id != data.NCMC443BWR_Id && t.NCMC443BWR_Range == data.NCMC443BWR_Range && t.NCMC443BWR_Year == data.NCMC443BWR_Year).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.NAAC_MC_443_BandWidth_RangeDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC443BWR_Id == data.NCMC443BWR_Id).SingleOrDefault();
                        j.NCMC443BWR_Range = data.NCMC443BWR_Range;


                        if (data.flag == "a")
                        {
                            j.NCMC443BWR_OneOrMoreGBPS = true;
                            j.NCMC443BWR_500MBPSTo1GBPS = false;
                            j.NCMC443BWR_250MBPSTo500MBPS = false;
                            j.NCMC443BWR_50MBPSTo250MBPS = false;
                            j.NCMC443BWR_LessThan50MBPS = false;
                        }
                        else if (data.flag == "b")
                        {
                            j.NCMC443BWR_OneOrMoreGBPS = false;
                            j.NCMC443BWR_500MBPSTo1GBPS = true;
                            j.NCMC443BWR_250MBPSTo500MBPS = false;
                            j.NCMC443BWR_50MBPSTo250MBPS = false;
                            j.NCMC443BWR_LessThan50MBPS = false;

                        }
                        else if (data.flag == "c")
                        {
                            j.NCMC443BWR_OneOrMoreGBPS = false;
                            j.NCMC443BWR_500MBPSTo1GBPS = false;
                            j.NCMC443BWR_250MBPSTo500MBPS = true;
                            j.NCMC443BWR_50MBPSTo250MBPS = false;
                            j.NCMC443BWR_LessThan50MBPS = false;

                        }
                        else if (data.flag == "d")
                        {
                            j.NCMC443BWR_OneOrMoreGBPS = false;
                            j.NCMC443BWR_500MBPSTo1GBPS = false;
                            j.NCMC443BWR_250MBPSTo500MBPS = false;
                            j.NCMC443BWR_50MBPSTo250MBPS = true;
                            j.NCMC443BWR_LessThan50MBPS = false;

                        }
                        else if (data.flag == "e")
                        {
                            j.NCMC443BWR_OneOrMoreGBPS = false;
                            j.NCMC443BWR_500MBPSTo1GBPS = false;
                            j.NCMC443BWR_250MBPSTo500MBPS = false;
                            j.NCMC443BWR_50MBPSTo250MBPS = false;
                            j.NCMC443BWR_LessThan50MBPS = true;
                        }

                        j.NCMC443BWR_Year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCMC443BWR_UpdatedDate = DateTime.Now;
                        j.NCMC443BWR_UpdatedBy = data.UserId;
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
        public NAAC_MC_443_BandWidth_Range_DTO EditData(NAAC_MC_443_BandWidth_Range_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_MC_443_BandWidth_RangeDMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCMC443BWR_Id == data.NCMC443BWR_Id && a.ASMAY_Id == b.NCMC443BWR_Year)
                                 select new NAAC_MC_443_BandWidth_Range_DTO
                                 {
                                     NCMC443BWR_Id = b.NCMC443BWR_Id,
                                     NCMC443BWR_Range = b.NCMC443BWR_Range,
                                     NCMC443BWR_OneOrMoreGBPS = b.NCMC443BWR_OneOrMoreGBPS,
                                     NCMC443BWR_500MBPSTo1GBPS = b.NCMC443BWR_500MBPSTo1GBPS,
                                     NCMC443BWR_250MBPSTo500MBPS = b.NCMC443BWR_250MBPSTo500MBPS,
                                     NCMC443BWR_50MBPSTo250MBPS = b.NCMC443BWR_50MBPSTo250MBPS,
                                     NCMC443BWR_LessThan50MBPS = b.NCMC443BWR_LessThan50MBPS,
                                     NCMC443BWR_Year = b.NCMC443BWR_Year,
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
