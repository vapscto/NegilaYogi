using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Hostel;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Services
{
    public class MasterMess_MessCategoryIMPL : Interface.MasterMess_MessCategoryInterface
    {
        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;


        public MasterMess_MessCategoryIMPL(HostelContext Para, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = Para;
            _dbcontext = para2;
        }
        public MasterMess_MessCategoryDTO get_Mmessdata(MasterMess_MessCategoryDTO data)
        {
            try
            {
                // = "NORTH INDIAN FOOD"

                data.get_messlistmapping = (from a in _HostelContext.HL_Master_Mess_MessCategoryDMO
                                            from b in _HostelContext.HL_Master_MessCategory_DMO
                                            from c in _HostelContext.HL_Master_Mess_DMO
                                            where (a.HLMMC_Id == b.HLMMC_Id && a.HLMM_Id == c.HLMM_Id && b.MI_Id == c.MI_Id && c.MI_Id == data.MI_Id)
                                            select new MasterMess_MessCategoryDTO
                                            {
                                                HLMM_Name = c.HLMM_Name,
                                                HLMMC_Name = b.HLMMC_Name,
                                                HLMMC_Id = a.HLMMC_Id,
                                                HLMM_Id = a.HLMM_Id,
                                                HLMMC_ActiveFlag = a.HLMMC_ActiveFlag,
                                                HLMMMC_Id = a.HLMMMC_Id
                                            }
                                   ).Distinct().ToArray();
                data.master_mess = _HostelContext.HL_Master_Mess_DMO.Where(R => R.HLMM_ActiveFlag == true && R.MI_Id == data.MI_Id).Distinct().ToArray();
                data.mess_category = _HostelContext.HL_Master_MessCategory_DMO.Where(R => R.HLMMC_ActiveFlag == true && R.MI_Id == data.MI_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Mess_DTO save_Mmessdata(HL_Master_Mess_DTO data)
        {
            try
            {
                if (data.MatserMessArray != null)
                {
                    int count = 0;
                    if (data.HLMMMC_Id > 0)
                    {

                        var updates = _HostelContext.HL_Master_Mess_MessCategoryDMO.Where(R => R.HLMMMC_Id == data.HLMMMC_Id).FirstOrDefault();

                        updates.HLMM_Id = data.HLMM_Id;
                        updates.HLMMC_UpdatedBy = data.UserId;
                        updates.HLMMC_UpdatedDate = DateTime.Now;
                        _HostelContext.Update(updates);

                        int t = _HostelContext.SaveChanges();
                        data.saverecord = t;
                        data.count = count;
                        if (t > 0)
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
                        foreach (var i in data.MatserMessArray)
                        {
                            var duplicate = _HostelContext.HL_Master_Mess_MessCategoryDMO.Where(R => R.HLMM_Id == data.HLMM_Id && R.HLMMC_Id == i.HLMMC_Id).ToList();
                            if (duplicate.Count > 0)
                            {
                                count += 1;
                            }
                            else
                            {
                                HL_Master_Mess_MessCategoryDMO obj = new HL_Master_Mess_MessCategoryDMO();
                                obj.HLMM_Id = data.HLMM_Id;
                                obj.HLMMC_Id = i.HLMMC_Id;
                                obj.HLMMC_ActiveFlag = true;
                                obj.HLMMC_CreatedDate = DateTime.Now;
                                obj.HLMMC_UpdatedDate = DateTime.Now;
                                obj.HLMMC_CreatedBy = data.UserId;
                                obj.HLMMC_UpdatedBy = data.UserId;
                                _HostelContext.Add(obj);
                            }

                        }

                        int t = _HostelContext.SaveChanges();
                        data.saverecord = t;
                        data.count = count;
                        if (t > 0)
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
        public MasterMess_MessCategoryDTO edit_Mmessdata(MasterMess_MessCategoryDTO data)
        {
            try
            {

                // HLMM_Name
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterMess_MessCategoryDTO deactiveY_Mmessdata(MasterMess_MessCategoryDTO data)
        {
            try
            {
                if (data.HLMMMC_Id > 0)
                {
                    var updates = _HostelContext.HL_Master_Mess_MessCategoryDMO.Where(R => R.HLMMMC_Id == data.HLMMMC_Id).FirstOrDefault();

                    if (updates.HLMMC_ActiveFlag == true)
                    {
                        updates.HLMMC_ActiveFlag = false;
                    }
                    else
                    {
                        updates.HLMMC_ActiveFlag = true;
                    }
                    updates.HLMMC_UpdatedBy = data.UserId;
                    updates.HLMMC_UpdatedDate = DateTime.Now;
                    _HostelContext.Update(updates);
                    int t = _HostelContext.SaveChanges();
                    if (t > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


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
