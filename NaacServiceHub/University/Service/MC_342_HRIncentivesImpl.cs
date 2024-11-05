using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class MC_342_HRIncentivesImpl:Interface.MC_342_HRIncentivesInterface
    {
        public GeneralContext _context;
        public MC_342_HRIncentivesImpl(GeneralContext y)
        {
            _context = y;
        }
        public MC_342_HRIncentivesDTO loaddata(MC_342_HRIncentivesDTO data)
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
                data.yearlist = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();
                data.alldata1 = (from a in _context.MC_342_HRIncentivesDMO
                                 from y in _context.Academic
                                   where (a.MI_Id == data.MI_Id && a.NCMCHRI342_Year==y.ASMAY_Id)
                                   select new MC_342_HRIncentivesDTO
                                   {
                                       ASMAY_Year = y.ASMAY_Year,
                                       NCMCHRI342_Year = a.NCMCHRI342_Year,
                                       NCMCHRI342_Id = a.NCMCHRI342_Id,
                                       NCMCHRI342_CareerAdvancement = a.NCMCHRI342_CareerAdvancement,
                                       NCMCHRI342_IncrementInSalary = a.NCMCHRI342_IncrementInSalary,
                                       NCMCHRI342_RecThroughWebsiteNotification = a.NCMCHRI342_RecThroughWebsiteNotification,
                                       NCMCHRI342_CommnCertAndCashaward = a.NCMCHRI342_CommnCertAndCashaward,
                                       NCMCHRI342_ActiveFlag=a.NCMCHRI342_ActiveFlag,
                                       MI_Id = data.MI_Id,
                                   }).Distinct().OrderByDescending(t => t.NCMCHRI342_Id).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public MC_342_HRIncentivesDTO savedata(MC_342_HRIncentivesDTO data)
        {
            try
            {
                if (data.NCMCHRI342_Id == 0)
                {
                    var duplicate = _context.MC_342_HRIncentivesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCHRI342_Year==data.ASMAY_Id && t.NCMCHRI342_Id!=0
                    && t.NCMCHRI342_CareerAdvancement == data.NCMCHRI342_CareerAdvancement && t.NCMCHRI342_IncrementInSalary == data.NCMCHRI342_IncrementInSalary && t.NCMCHRI342_RecThroughWebsiteNotification == data.NCMCHRI342_RecThroughWebsiteNotification && t.NCMCHRI342_CommnCertAndCashaward == data.NCMCHRI342_CommnCertAndCashaward).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MC_342_HRIncentivesDMO obj1 = new MC_342_HRIncentivesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMCHRI342_Year = data.ASMAY_Id;
                        obj1.NCMCHRI342_CareerAdvancement = data.NCMCHRI342_CareerAdvancement;
                        obj1.NCMCHRI342_IncrementInSalary = data.NCMCHRI342_IncrementInSalary;
                        obj1.NCMCHRI342_RecThroughWebsiteNotification = data.NCMCHRI342_RecThroughWebsiteNotification;
                        obj1.NCMCHRI342_CommnCertAndCashaward = data.NCMCHRI342_CommnCertAndCashaward;
                        obj1.NCMCHRI342_ActiveFlag = true;
                        obj1.NCMCHRI342_CreatedDate = DateTime.Now;
                        obj1.NCMCHRI342_UpdatedDate = DateTime.Now;
                        obj1.NCMCHRI342_CreatedBy = data.UserId;
                        obj1.NCMCHRI342_UpdatedBy = data.UserId;
                        _context.Add(obj1);                        
                        int row = _context.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCMCHRI342_Id > 0)
                {
                    var duplicate = _context.MC_342_HRIncentivesDMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMCHRI342_Id != data.NCMCHRI342_Id && t.NCMCHRI342_Year==data.ASMAY_Id
                    && t.NCMCHRI342_CareerAdvancement == data.NCMCHRI342_CareerAdvancement
                    && t.NCMCHRI342_IncrementInSalary == data.NCMCHRI342_IncrementInSalary
                    && t.NCMCHRI342_RecThroughWebsiteNotification == data.NCMCHRI342_RecThroughWebsiteNotification
                    && t.NCMCHRI342_CommnCertAndCashaward == data.NCMCHRI342_CommnCertAndCashaward).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _context.MC_342_HRIncentivesDMO.Where(t => t.NCMCHRI342_Id == data.NCMCHRI342_Id && t.MI_Id == data.MI_Id).Single();                        
                        update.NCMCHRI342_CareerAdvancement = data.NCMCHRI342_CareerAdvancement;
                        update.NCMCHRI342_IncrementInSalary = data.NCMCHRI342_IncrementInSalary;
                        update.NCMCHRI342_RecThroughWebsiteNotification = data.NCMCHRI342_RecThroughWebsiteNotification;
                        update.NCMCHRI342_CommnCertAndCashaward = data.NCMCHRI342_CommnCertAndCashaward;                update.NCMCHRI342_Year = data.ASMAY_Id;
                        update.MI_Id = data.MI_Id;
                        update.NCMCHRI342_UpdatedDate = DateTime.Now;
                        update.NCMCHRI342_UpdatedBy = data.UserId;
                        _context.Update(update);                        
                        int row = _context.SaveChanges();
                        if (row > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MC_342_HRIncentivesDTO editdata(MC_342_HRIncentivesDTO data)
        {
            try
            {
                data.editlist = (from a in _context.MC_342_HRIncentivesDMO
                                 from b in _context.Academic
                                 where (a.NCMCHRI342_Id == data.NCMCHRI342_Id && a.MI_Id == data.MI_Id && a.NCMCHRI342_Year==b.ASMAY_Id && a.MI_Id==b.MI_Id)
                                 select new MC_342_HRIncentivesDTO
                                 {
                                     NCMCHRI342_Id = a.NCMCHRI342_Id,
                                     MI_Id = a.MI_Id,
                                     NCMCHRI342_Year = a.NCMCHRI342_Year,
                                     ASMAY_Year = b.ASMAY_Year,
                                     NCMCHRI342_CareerAdvancement = a.NCMCHRI342_CareerAdvancement,
                                     NCMCHRI342_IncrementInSalary = a.NCMCHRI342_IncrementInSalary,
                                     NCMCHRI342_RecThroughWebsiteNotification = a.NCMCHRI342_RecThroughWebsiteNotification,
                                     NCMCHRI342_CommnCertAndCashaward = a.NCMCHRI342_CommnCertAndCashaward,                                     
                                 }).Distinct().ToArray();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
      
        public MC_342_HRIncentivesDTO deactive(MC_342_HRIncentivesDTO data)
        {
            try
            {
                var u = _context.MC_342_HRIncentivesDMO.Where(t => t.NCMCHRI342_Id == data.NCMCHRI342_Id).SingleOrDefault();
                if (u.NCMCHRI342_ActiveFlag == true)
                {
                    u.NCMCHRI342_ActiveFlag = false;
                }
                else if (u.NCMCHRI342_ActiveFlag == false)
                {
                    u.NCMCHRI342_ActiveFlag = true;
                }
                u.NCMCHRI342_UpdatedDate = DateTime.Now;
                u.NCMCHRI342_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
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
    }
}
