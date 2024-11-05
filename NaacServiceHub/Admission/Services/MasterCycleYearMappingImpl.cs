using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Services
{
    public class MasterCycleYearMappingImpl : Interface.MasterCycleYearMappingInterface
    {
        public GeneralContext _context;
        public MasterCycleYearMappingImpl(GeneralContext context)
        {
            _context = context;
        }

        public MasterCycleYearMappingDTO getalldetails(MasterCycleYearMappingDTO data)
        {
            try
            {
                data.getmastercycle = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getmastercyclemapping = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id && a.NCMACY_ActiveFlg == true).OrderBy(a => a.NCMACY_Order).ToArray();

                data.getmastercycleorder = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.NCMACY_Order).ToArray();

                data.getmastercyclemappingdetails = (from a in _context.NAAC_Master_CycleDMO
                                                     from b in _context.NAAC_Master_Cycle_YearDMO
                                                     from c in _context.Academic
                                                     where (a.NCMACY_Id == b.NCMACY_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                                     select new MasterCycleYearMappingDTO
                                                     {
                                                         NCMACY_NAACCycle = a.NCMACY_NAACCycle,
                                                         NCMACY_FromDate = a.NCMACY_FromDate,
                                                         NCMACY_TODate = a.NCMACY_TODate,
                                                         NCMACY_Id = a.NCMACY_Id,
                                                         NCMACYYR_Id = b.NCMACY_Id,
                                                         NCMACYYR_ActiveFlg = b.NCMACYYR_ActiveFlg

                                                     }).Distinct().ToArray();

                data.getyearlist = _context.Academic.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderBy(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterCycleYearMappingDTO savedetails(MasterCycleYearMappingDTO data)
        {
            try
            {
                if (data.NCMACY_Id > 0)
                {
                    var checkduplicate = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id && a.NCMACY_Id != data.NCMACY_Id
                    && a.NCMACY_NAACCycle == data.NCMACY_NAACCycle).Count();

                    if (checkduplicate > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _context.NAAC_Master_CycleDMO.Single(a => a.MI_Id == data.MI_Id && a.NCMACY_Id == data.NCMACY_Id);
                        result.NCMACY_NAACCycle = data.NCMACY_NAACCycle;
                        result.NCMACY_FromDate = data.NCMACY_FromDate;
                        result.NCMACY_TODate = data.NCMACY_TODate;
                        result.NCMACY_UpdatedBy = data.NCMACY_CreatedBy;
                        result.NCMACY_UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id && a.NCMACY_NAACCycle == data.NCMACY_NAACCycle).Count();

                    //var checkduplicate_fromdate = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id
                    // && (a.NCMACY_FromDate.Value.Date >= data.NCMACY_FromDate.Value.Date || a.NCMACY_TODate.Value.Date <= data.NCMACY_FromDate.Value.Date)).Count();

                    //var checkduplicate_todate = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id
                    //&& (a.NCMACY_FromDate.Value.Date >= data.NCMACY_TODate.Value.Date || a.NCMACY_TODate.Value.Date <= data.NCMACY_TODate.Value.Date)).Count();


                    if (checkduplicate > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var resultcount = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                        NAAC_Master_CycleDMO dmo = new NAAC_Master_CycleDMO();
                        dmo.NCMACY_NAACCycle = data.NCMACY_NAACCycle;
                        dmo.NCMACY_FromDate = data.NCMACY_FromDate;
                        dmo.MI_Id = data.MI_Id;
                        dmo.NCMACY_Order = resultcount + 1;
                        dmo.NCMACY_TODate = data.NCMACY_TODate;
                        dmo.NCMACY_UpdatedBy = data.NCMACY_CreatedBy;
                        dmo.NCMACY_UpdatedDate = DateTime.Now;
                        dmo.NCMACY_CreatedBy = data.NCMACY_CreatedBy;
                        dmo.NCMACY_ActiveFlg = true;
                        dmo.NCMACY_CreatedDate = DateTime.Now;
                        _context.Add(dmo);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Add";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterCycleYearMappingDTO activedeactivedetails(MasterCycleYearMappingDTO data)
        {
            try
            {

                var checkcycleused = _context.NAAC_Master_Cycle_YearDMO.Where(a => a.MI_Id == data.MI_Id && a.NCMACY_Id == data.NCMACY_Id).ToList();

                if (checkcycleused.Count > 0)
                {
                    data.message = "Mapped";
                }
                else
                {
                    var result = _context.NAAC_Master_CycleDMO.Single(a => a.MI_Id == data.MI_Id && a.NCMACY_Id == data.NCMACY_Id);
                    if (result.NCMACY_ActiveFlg == true)
                    {
                        result.NCMACY_ActiveFlg = false;
                    }
                    else
                    {
                        result.NCMACY_ActiveFlg = true;
                    }
                    result.NCMACY_UpdatedBy = data.NCMACY_CreatedBy;
                    result.NCMACY_UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    var i = _context.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                        data.message = "Update";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Update";
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterCycleYearMappingDTO editdetails(MasterCycleYearMappingDTO data)
        {
            try
            {
                data.geteditdetails = _context.NAAC_Master_CycleDMO.Where(a => a.MI_Id == data.MI_Id && a.NCMACY_Id == data.NCMACY_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterCycleYearMappingDTO getOrder(MasterCycleYearMappingDTO data)
        {
            try
            {
                int i = 0;
                for (int k = 0; k < data.order_temp.Length; k++)
                {
                    i = i + 1;
                    var result = _context.NAAC_Master_CycleDMO.Single(a => a.MI_Id == data.MI_Id && a.NCMACY_Id == data.order_temp[k].NCMACY_Id);
                    result.NCMACY_Order = i;
                    result.NCMACY_UpdatedBy = data.NCMACY_CreatedBy;                    
                    result.NCMACY_UpdatedDate = DateTime.Now;
                    _context.Update(result);
                }
                var i1 = _context.SaveChanges();
                if (i1 > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // Master Cycle Year Mapping
        public MasterCycleYearMappingDTO onchangecycle(MasterCycleYearMappingDTO data)
        {
            try
            {
                List<long> yearid = new List<long>();

                var getsavedyear = (from a in _context.NAAC_Master_CycleDMO
                                    from b in _context.NAAC_Master_Cycle_YearDMO
                                    where (a.NCMACY_Id == b.NCMACY_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                    select new MasterCycleYearMappingDTO
                                    {
                                        ASMAY_Id = b.ASMAY_Id
                                    }).Distinct().ToList();
                foreach (var c in getsavedyear)
                {
                    yearid.Add(c.ASMAY_Id);
                }

                data.getyearlist = _context.Academic.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && !yearid.Contains(a.ASMAY_Id)).Distinct().OrderBy(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterCycleYearMappingDTO savedetails1(MasterCycleYearMappingDTO data)
        {
            try
            {
                foreach (var c in data.temp_yeardto)
                {
                    NAAC_Master_Cycle_YearDMO dmonew = new NAAC_Master_Cycle_YearDMO();
                    dmonew.MI_Id = data.MI_Id;
                    dmonew.NCMACY_Id = data.NCMACY_Id;
                    dmonew.ASMAY_Id = c.ASMAY_Id;
                    dmonew.NCMACYYR_ActiveFlg = true;
                    dmonew.NCMACYYR_UpdatedBy = data.NCMACY_CreatedBy;
                    dmonew.NCMACYYR_UpdatedDate = DateTime.Now;
                    dmonew.NCMACYYR_CreatedBy = data.NCMACY_CreatedBy;
                    dmonew.NCMACYYR_CreatedDate = DateTime.Now;
                    _context.Add(dmonew);
                }
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.message = "Add";
                    data.returnval = true;
                }
                else
                {
                    data.message = "Add";
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterCycleYearMappingDTO viewdetails(MasterCycleYearMappingDTO data)
        {
            try
            {
                data.getviewdetails = (from a in _context.NAAC_Master_CycleDMO
                                       from b in _context.NAAC_Master_Cycle_YearDMO
                                       from c in _context.Academic
                                       where (a.NCMACY_Id == b.NCMACY_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                       && a.NCMACY_Id == data.NCMACY_Id)
                                       select new MasterCycleYearMappingDTO
                                       {
                                           NCMACY_NAACCycle = a.NCMACY_NAACCycle,
                                           NCMACY_Id = a.NCMACY_Id,
                                           NCMACYYR_Id = b.NCMACY_Id,
                                           NCMACYYR_ActiveFlg = b.NCMACYYR_ActiveFlg,
                                           ASMAY_Year = c.ASMAY_Year,
                                           ASMAY_Order = c.ASMAY_Order
                                       }).Distinct().OrderBy(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterCycleYearMappingDTO deactivesem(MasterCycleYearMappingDTO data)
        {
            try
            {
                var result = _context.NAAC_Master_Cycle_YearDMO.Single(a => a.NCMACYYR_Id == data.NCMACYYR_Id);
                if (result.NCMACYYR_ActiveFlg == true)
                {
                    result.NCMACYYR_ActiveFlg = false;
                }
                else
                {
                    result.NCMACYYR_ActiveFlg = true;
                }
                result.NCMACYYR_UpdatedBy = data.NCMACY_CreatedBy;
                result.NCMACYYR_UpdatedDate = DateTime.Now;
                _context.Update(result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterCycleYearMappingDTO delete(MasterCycleYearMappingDTO data)
        {
            try
            {
                var result = _context.NAAC_Master_Cycle_YearDMO.Single(a => a.NCMACYYR_Id == data.NCMACYYR_Id);
                _context.Remove(result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
