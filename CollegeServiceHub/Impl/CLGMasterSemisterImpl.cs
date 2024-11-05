using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;

namespace CollegeServiceHub.Impl
{
    public class CLGMasterSemisterImpl : Interface.CLGMasterSemisterInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        public CLGMasterSemisterImpl(ClgAdmissionContext ClgAdmissionContext)
        {
            _ClgAdmissionContext = ClgAdmissionContext;

        }
        public CLGMasterSemisterDTO activedeactivesem(CLGMasterSemisterDTO data)
        {
            try
            {
                var check_sem_used = (from a in _ClgAdmissionContext.AdmCourseBranchSemesterMappingDMO
                                      from b in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                      where (a.AMSE_Id == b.AMSE_Id && a.MI_Id == data.MI_Id && a.AMCOBMS_ActiveFlg == true)
                                      select new CLGMasterSemisterDTO
                                      {
                                          AMSE_Id = a.AMSE_Id
                                      }).ToList();

                var check_sem_used1 = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                       from b in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                       where (a.AMSE_Id == b.AMSE_Id && b.MI_Id == data.MI_Id && a.ACALD_ActiveFlag == true)
                                       select new CLGMasterSemisterDTO
                                       {
                                           AMSE_Id = a.AMSE_Id
                                       }).ToList();


                var check_sem_used2 = (from a in _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO
                                       from b in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                       where (a.AMSE_Id == b.AMSE_Id && b.MI_Id == data.MI_Id && a.ACASMP_ActiveFlag == true)
                                       select new CLGMasterSemisterDTO
                                       {
                                           AMSE_Id = a.AMSE_Id
                                       }).ToList();

                var check_sem_used3 = (from a in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                       from b in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                       where (a.AMSE_Id == b.AMSE_Id && b.MI_Id == data.MI_Id && a.ACAYCBS_ActiveFlag == true)
                                       select new CLGMasterSemisterDTO
                                       {
                                           AMSE_Id = a.AMSE_Id
                                       }).ToList();

                if (check_sem_used.Count == 0 && check_sem_used1.Count == 0 && check_sem_used2.Count == 0 && check_sem_used3.Count == 0)
                {
                    var query = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Single(t => t.MI_Id == data.MI_Id && t.AMSE_Id == data.AMSE_Id);
                    if (query.AMSE_ActiveFlg == true)
                    {
                        query.AMSE_ActiveFlg = false;
                    }
                    else
                    {
                        query.AMSE_ActiveFlg = true;
                    }
                    query.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Update(query);
                    var i = _ClgAdmissionContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    data.message = "Mapped";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }

        public CLGMasterSemisterDTO editsem(CLGMasterSemisterDTO data)
        {
            try
            {
                var query = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(f => f.AMSE_Id == data.AMSE_Id && f.MI_Id == data.MI_Id).ToArray();
                data.editdetails = query;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGMasterSemisterDTO savesem(CLGMasterSemisterDTO data)
        {
            try
            {
                if (data.AMSE_Id > 0)
                {
                    var check_duplicate = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_SEMName.Equals(data.AMSE_SEMName) && a.AMSE_Id != data.AMSE_Id).ToList();

                    var check_duplicate1 = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_SEMCode.Equals(data.AMSE_SEMCode) && a.AMSE_Id != data.AMSE_Id).ToList();

                    if (check_duplicate.Count == 0 && check_duplicate1.Count == 0)
                    {
                        var query = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Single(d => d.AMSE_Id == data.AMSE_Id);

                        query.AMSE_EvenOdd = data.AMSE_EvenOdd;
                        query.AMSE_SEMCode = data.AMSE_SEMCode;
                        query.AMSE_SEMInfo = data.AMSE_SEMInfo;
                        query.AMSE_SEMName = data.AMSE_SEMName;
                        query.AMSE_SEMOrder = data.AMSE_SEMOrder;
                        query.AMSE_SEMTypeFlag = data.AMSE_SEMTypeFlag;
                        query.AMSE_Year = data.AMSE_Year;
                        query.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(query);
                        var flag = _ClgAdmissionContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "";
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var check_duplicate = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_SEMName.Equals(data.AMSE_SEMName)).ToList();

                    var check_duplicate1 = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_SEMCode.Equals(data.AMSE_SEMCode)).ToList();

                    if (check_duplicate.Count == 0 && check_duplicate1.Count == 0)
                    {
                        CLG_Adm_Master_SemesterDMO obj = AutoMapper.Mapper.Map<CLG_Adm_Master_SemesterDMO>(data);
                        obj.AMSE_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(obj);
                        var i = _ClgAdmissionContext.SaveChanges();
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
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGMasterSemisterDTO getdata(CLGMasterSemisterDTO data)
        {
            try
            {
                var query02 = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(w => w.MI_Id == data.MI_Id).ToArray();
                var query021 = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(w => w.MI_Id == data.MI_Id).OrderBy(a=>a.AMSE_SEMOrder).ToArray();
                if (query02.Length > 0)
                {
                    data.semlist = query02;
                    data.semlistorder = query021;
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

        public CLGMasterSemisterDTO getOrder(CLGMasterSemisterDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.sem_temp.Count(); i++)
                {
                    id = id + 1;
                    var result = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Single(a => a.MI_Id == data.MI_Id && a.AMSE_Id == data.sem_temp[i].AMSE_Id);
                    result.AMSE_SEMOrder = id;
                    _ClgAdmissionContext.Update(result);
                }
                var idd = _ClgAdmissionContext.SaveChanges();
                if (idd > 0)
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
                data.returnval = false;
            }
            return data;
        }

    }
}
