using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vaps.admission;

namespace WebApplication1.Services
{
    public class MasterSectionImpl : MasterSectionInterface
    {
        public MasterSectionContext _MasterSectionContext;
        public MasterSectionImpl(MasterSectionContext masterSectionContext)
        {
            _MasterSectionContext = masterSectionContext;
        }

        public MasterSectionDTO DeleteMasterscetionDetails(MasterSectionDTO master)
        {
           
            List<School_M_Section> mastersect = new List<School_M_Section>(); // Mapper.Map<Organisation>(org);
            School_M_Section maspge = new School_M_Section();

            try
            {
                var check_assign = (from a in _MasterSectionContext.ystudent
                                    from b in _MasterSectionContext.masterSection
                                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == master.MI_Id && b.ASMS_Id == master.ASMC_Id && b.ASMC_ActiveFlag==1)
                                    select new MasterSectionDTO
                                    {
                                        ASMC_Id = b.ASMS_Id
                                    }).ToList();

                if (check_assign.Count > 0)
                {
                    master.message = "Section Can Not Be Disable It Is Already Mapped..";
                }
                else
                {
                    var result = _MasterSectionContext.masterSection.Single(t => t.ASMS_Id == master.ASMC_Id);

                    if (result.ASMC_ActiveFlag == 1)
                    {
                        result.ASMC_ActiveFlag = 0;
                        result.UpdatedDate = DateTime.Now;
                        result.CreatedDate = result.CreatedDate;
                        _MasterSectionContext.Update(result);
                        _MasterSectionContext.SaveChanges();
                        master.returnval = "true";
                    }
                    else
                    {
                        result.UpdatedDate = DateTime.Now;
                        result.CreatedDate = result.CreatedDate;
                        result.ASMC_ActiveFlag = 1;
                        _MasterSectionContext.Update(result);
                        _MasterSectionContext.SaveChanges();
                        master.returnval = "false";
                    }
                    List<School_M_Section> allmastersection = new List<School_M_Section>();
                    allmastersection = _MasterSectionContext.masterSection.Where(d => d.MI_Id == master.MI_Id).ToList();
                    master.MasterSectionData = allmastersection.OrderByDescending(a=>a.CreatedDate).ToArray();

                }
                
            
            //try
            //{
            //        mastersect = _MasterSectionContext.masterSection.Where(t => t.ASMC_Id.Equals(id)).ToList();

            //        if (mastersect.Any())
            //        {
            //            _MasterSectionContext.Remove(mastersect.ElementAt(0));
            //            var contactExists = _MasterSectionContext.SaveChanges();
            //            if (contactExists == 1)
            //            {
            //                master.returnval = "Record deleted successfully";
            //            }
            //            else
            //            {
            //                master.returnval = "No Record found to delete";
            //            }
            //        }


            //    List<School_M_Section> allmastersection = new List<School_M_Section>();
            //    allmastersection = _MasterSectionContext.masterSection.ToList();
            //    master.MasterSectionData = allmastersection.ToArray();
        }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                master.returnval = ee.Message;
            }

            return master;
        }


        public MasterSectionDTO SaveMasterscetionDetails(MasterSectionDTO master)
        {
            School_M_Section maspge = new School_M_Section();
            if (master.ASMC_Id > 0)
            {

                var check_assign = (from a in _MasterSectionContext.ystudent
                                    from b in _MasterSectionContext.masterSection
                                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == master.MI_Id && b.ASMS_Id == master.ASMC_Id)
                                    select new MasterSectionDTO
                                    {
                                        ASMC_Id = b.ASMS_Id
                                    }).ToList();

                if(check_assign.Count>0)
                {
                    master.returnval = "Section Can Not Be Edit It Is Already Mapped..";
                }
                else
                {
                    var duplicatecountresult = _MasterSectionContext.masterSection.Where(t => t.ASMC_SectionName == master.ASMC_SectionName && t.MI_Id == master.MI_Id && t.ASMS_Id != master.ASMC_Id).Count();
                    var duplicatecountresultorder = _MasterSectionContext.masterSection.Where(t => t.ASMC_Order == master.ASMC_Order && t.MI_Id == master.MI_Id && t.ASMS_Id != master.ASMC_Id).Count();
                    var duplicatecountresultcode = _MasterSectionContext.masterSection.Where(t => t.ASMC_SectionCode == master.ASMC_SectionCode && t.MI_Id == master.MI_Id && t.ASMS_Id != master.ASMC_Id).Count();
                    if (duplicatecountresult > 0)
                    {
                        master.returnval = "Master Section Record Already Exist";
                        return master;
                    }
                    if (duplicatecountresultorder > 0)
                    {
                        master.returnval = "Master Section Record Already Exist";
                        return master;
                    }
                    if (duplicatecountresultcode > 0)
                    {
                        master.returnval = "Master Section Record Already Exist";
                        return master;
                    }
                    else
                    {
                        var result = _MasterSectionContext.masterSection.Single(t => t.ASMS_Id == master.ASMC_Id);
                        // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);
                        result.MI_Id = master.MI_Id;
                        result.ASMC_SectionName = master.ASMC_SectionName;
                        result.ASMC_SectionCode = master.ASMC_SectionCode;
                        result.ASMC_Order = master.ASMC_Order;
                        result.ASMC_MaxCapacity = master.ASMC_MaxCapacity;
                        result.UpdatedDate = DateTime.Now;
                        _MasterSectionContext.Update(result);
                        var contactExists = _MasterSectionContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            master.returnval = "Record Updated Successfully";
                        }
                        else
                        {
                            master.returnval = "Record Not Updated";
                        }
                    }
                }                
            }
            else
            {
                School_M_Section mas = Mapper.Map<School_M_Section>(master);
                var duplicatecountresult = _MasterSectionContext.masterSection.Where(t => t.ASMC_SectionName == master.ASMC_SectionName && t.MI_Id==master.MI_Id).Count();
                var duplicatecountresultorder= _MasterSectionContext.masterSection.Where(t => t.ASMC_Order == master.ASMC_Order && t.MI_Id == master.MI_Id).Count();
               var duplicatecountresultcode = _MasterSectionContext.masterSection.Where(t => t.ASMC_SectionCode == master.ASMC_SectionCode && t.MI_Id == master.MI_Id).Count();
                if (duplicatecountresult == 0 && duplicatecountresultorder==0 && duplicatecountresultcode == 0)
                {
                    mas.CreatedDate = DateTime.Now;
                    mas.UpdatedDate = DateTime.Now;
                    mas.ASMC_ActiveFlag = 1;
                    _MasterSectionContext.Add(mas);
                    _MasterSectionContext.SaveChanges();

                    master.returnval = "Record Saved Successfully";
                }
                else if (duplicatecountresult > 0)
                {
                    master.returnval = "Master Section Record Already Exist";
                }
                else if (duplicatecountresultorder > 0)
                {
                    master.returnval = "Master Section Record Already Exist";
                }
                else if (duplicatecountresultcode > 0)
                {
                    master.returnval = "Master Section Record Already Exist";
                }
                else
                {
                    master.returnval = "Master Section Record Already Exist";
                }               
            }

            List<School_M_Section> allmastersection = new List<School_M_Section>();
            allmastersection = _MasterSectionContext.masterSection.Where(d=>d.MI_Id==master.MI_Id).ToList();
            master.MasterSectionData = allmastersection.OrderByDescending(a => a.CreatedDate).ToArray();

            return master;
        }


        public MasterSectionDTO EditMasterscetionDetails(int id)
        {
            MasterSectionDTO mast = new MasterSectionDTO();
            try
            {
                List<School_M_Section> mastersec = new List<School_M_Section>();
                mastersec = _MasterSectionContext.masterSection.AsNoTracking().Where(t => t.ASMS_Id.Equals(id)).ToList();

                mast.MasterSectionData = mastersec.OrderByDescending(a => a.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                mast.returnval = ee.Message;
            }
            return mast;
        }

        public MasterSectionDTO GetMasterscetionDetails(int mi_id)
        {

            MasterSectionDTO master = new MasterSectionDTO();
            try
            {
                List<School_M_Section> mastersec = new List<School_M_Section>();
                 mastersec = _MasterSectionContext.masterSection.Where(d=>d.MI_Id== mi_id).ToList();
                 master.MasterSectionData = mastersec.OrderByDescending(a => a.CreatedDate).ToArray();

               master.getsectionlist = _MasterSectionContext.masterSection.Where(d => d.MI_Id == mi_id && d.ASMC_ActiveFlag==1).OrderBy(a=>a.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return master;
        }

        public MasterSectionDTO getsearchdata(int id, MasterSectionDTO org)
        {
            //string filetype = "All";
            MasterSectionDTO pagedata = new MasterSectionDTO();
            try
            {
                List<School_M_Section> lorg = new List<School_M_Section>();
                if (org.ASMC_SectionCode == "Section Name")
                {
                    lorg = _MasterSectionContext.masterSection.Where(t => t.ASMC_SectionName.Contains(org.ASMC_SectionName) && t.ASMC_ActiveFlag==0).ToList();

                }
                if (org.ASMC_SectionCode == "Section Capacity")
                {
                    lorg = _MasterSectionContext.masterSection.Where(t => t.ASMC_MaxCapacity.Equals(org.ASMC_MaxCapacity) && t.ASMC_ActiveFlag==0).ToList();

                }
                if (org.ASMC_SectionCode == "All")
                {
                    lorg = _MasterSectionContext.masterSection.ToList();
                }
                org.MasterSectionData = lorg.OrderByDescending(a => a.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }
    }
}
         


        
    
 
