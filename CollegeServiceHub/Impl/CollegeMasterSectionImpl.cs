using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeMasterSectionImpl : Interface.CollegeMasterSectionInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<CollegeMasterSectionImpl> _logbranch;
        public CollegeMasterSectionImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<CollegeMasterSectionImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logbranch = log;
        }


        public CollegeMasterSectionDTO getalldetails(int id)
        {
            CollegeMasterSectionDTO data = new CollegeMasterSectionDTO();
            try
            {
                data.getdetails = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id).ToArray();

                data.getdetails1 = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id).OrderBy(a=>a.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logbranch.LogInformation("College Master Section getalldetails :" + ex.Message);
            }
            return data;
        }
        public CollegeMasterSectionDTO saveMasterdata(CollegeMasterSectionDTO id)
        {
            try
            {
                if (id.ACMS_Id > 0)
                {
                    var check_duplicate = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id.MI_Id && a.ACMS_SectionName == id.ACMS_SectionName && a.ACMS_Id != id.ACMS_Id).ToList();

                    var check_duplicate1 = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id.MI_Id && a.ACMS_SectionCode == id.ACMS_SectionCode && a.ACMS_Id != id.ACMS_Id).ToList();

                    var check_duplicate2 = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id.MI_Id && a.ACMS_Order == id.ACMS_Order && a.ACMS_Id != id.ACMS_Id).ToList();

                    if (check_duplicate.Count == 0 && check_duplicate1.Count == 0 && check_duplicate2.Count == 0)
                    {
                        var result = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Single(a => a.MI_Id == id.MI_Id && a.ACMS_Id == id.ACMS_Id);
                        result.ACMS_SectionName = id.ACMS_SectionName;
                        result.ACMS_SectionCode = id.ACMS_SectionCode;
                        result.ACMS_Order = id.ACMS_Order;
                        result.ACMS_MaxCapacity = id.ACMS_MaxCapacity;
                        result.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                        int kk = _ClgAdmissionContext.SaveChanges();
                        if (kk > 0)
                        {
                            id.returnval = true;
                            id.message = "Update";
                        }
                        else
                        {
                            id.returnval = false;
                            id.message = "Update";
                        }
                    }
                    else
                    {
                        id.message = "Duplicate";
                    }

                }
                else
                {
                    var check_duplicate = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id.MI_Id && a.ACMS_SectionName == id.ACMS_SectionName).ToList();

                    var check_duplicate1 = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id.MI_Id && a.ACMS_SectionCode == id.ACMS_SectionCode).ToList();

                    var check_duplicate2 = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id.MI_Id && a.ACMS_Order == id.ACMS_Order).ToList();

                    if (check_duplicate.Count == 0 && check_duplicate1.Count == 0 && check_duplicate2.Count == 0)
                    {
                        Adm_College_Master_SectionDMO section = new Adm_College_Master_SectionDMO();
                        section.ACMS_SectionName = id.ACMS_SectionName;
                        section.ACMS_SectionCode = id.ACMS_SectionCode;
                        section.ACMS_Order = id.ACMS_Order;
                        section.ACMS_MaxCapacity = id.ACMS_MaxCapacity;
                        section.UpdatedDate = DateTime.Now;
                        section.CreatedDate = DateTime.Now;
                        section.MI_Id = id.MI_Id;
                        section.ACMS_ActiveFlag = true;
                        _ClgAdmissionContext.Add(section);
                        var ii = _ClgAdmissionContext.SaveChanges();
                        if (ii > 0)
                        {
                            id.returnval = true;
                            id.message = "Add";
                        }
                        else
                        {
                            id.returnval = false;
                            id.message = "Add";
                        }
                    }
                    else
                    {
                        id.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logbranch.LogInformation("College Master Section saveMasterdata :" + ex.Message);
            }
            return id;
        }
        public CollegeMasterSectionDTO Editdetails(CollegeMasterSectionDTO id)
        {
            try
            {
                id.editdetails = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id.MI_Id && a.ACMS_Id == id.ACMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logbranch.LogInformation("College Master Section Editdetails :" + ex.Message);
            }
            return id;
        }
        public CollegeMasterSectionDTO saveorder(CollegeMasterSectionDTO id)
        {
            try
            {
                int order = 0;
                for(int kk =0;kk< id.Master_Section_CLg_Temp.Count(); kk++)
                {
                    order = order + 1;
                    var result = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Single(a => a.MI_Id == id.MI_Id && a.ACMS_Id == id.Master_Section_CLg_Temp[kk].ACMS_Id);
                    result.ACMS_Order = order;
                    result.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Update(result);
                }
                var ii = _ClgAdmissionContext.SaveChanges();
                if (ii > 0)
                {
                    id.returnval = true;
                    id.message = "Update";
                }
                else
                {
                    id.returnval = false;
                    id.message = "Update";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                id.returnval = false;
                _logbranch.LogInformation("College Master Section saveorder :" + ex.Message);
            }
            return id;
        }
        public CollegeMasterSectionDTO Deletedetails(CollegeMasterSectionDTO id)
        {
            try
            {
                var check_section_used = (from a in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                          from b in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                          where (a.ACMS_Id == b.ACMS_Id && b.ACMS_ActiveFlag == true && a.ACMS_Id==id.ACMS_Id)
                                          select new CollegeMasterSectionDTO
                                          {
                                              ACMS_Id = a.ACMS_Id
                                          }).ToList();

                var check_section_used1 = (from a in _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO
                                          from b in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                          where (a.ACMS_Id == b.ACMS_Id && b.ACMS_ActiveFlag == true && a.ACASMP_ActiveFlag== true && a.ACMS_Id == id.ACMS_Id)
                                          select new CollegeMasterSectionDTO
                                          {
                                              ACMS_Id = a.ACMS_Id
                                          }).ToList();


                if(check_section_used.Count==0 && check_section_used1.Count == 0)
                {
                    var result = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Single(a => a.MI_Id == id.MI_Id && a.ACMS_Id == id.ACMS_Id);
                    if (result.ACMS_ActiveFlag == true)
                    {
                        result.ACMS_ActiveFlag = false;
                    }
                    else
                    {
                        result.ACMS_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Update(result);
                    var il = _ClgAdmissionContext.SaveChanges();
                    if (il > 0)
                    {
                        id.returnval = true;
                    }
                    else
                    {
                        id.returnval = false;
                    }
                }
                else
                {
                    id.message = "Mapped";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logbranch.LogInformation("College Master Section Deletedetails :" + ex.Message);
            }
            return id;
        }
    }
}
