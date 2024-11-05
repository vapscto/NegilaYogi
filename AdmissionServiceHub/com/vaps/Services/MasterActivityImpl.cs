using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;



namespace AdmissionServiceHub.com.vaps.Services
{
    public class MasterActivityImpl : Interfaces.MasterActivityInterface
    {
        private static ConcurrentDictionary<string, MasterActivityDTO> _login =
        new ConcurrentDictionary<string, MasterActivityDTO>();

        private readonly MasterActivityContext _MasterActivityContext;


        public MasterActivityImpl(MasterActivityContext MasterActivityContext)
        {
            _MasterActivityContext = MasterActivityContext;
        }

        public MasterActivityDTO GetMasterActivityData(MasterActivityDTO MasterActivityDTO)//int IVRMM_Id
        {

            Array[] showdata = new Array[50];
            List<MasterActivityDMO> Allname = new List<MasterActivityDMO>();
            Allname = _MasterActivityContext.MasterActivityDMO.OrderByDescending(a=>a.CreatedDate).ToList();
            MasterActivityDTO.MasterActivityname = Allname.ToArray();
            return MasterActivityDTO;
        }

        public MasterActivityDTO GetSelectedRowDetails(int ID)
        {
            MasterActivityDTO MasterActivityDTO = new MasterActivityDTO();
            List<MasterActivityDMO> lorg = new List<MasterActivityDMO>();
            lorg = _MasterActivityContext.MasterActivityDMO.Where(t => t.AMA_Id.Equals(ID)).ToList();
            MasterActivityDTO.MasterActivityname = lorg.ToArray();
            return MasterActivityDTO;
        }

        public MasterActivityDTO MasterDeleteModulesData(int ID)
        {
            MasterActivityDTO MasterActivityDTO = new MasterActivityDTO();
            List<MasterActivityDMO> masters = new List<MasterActivityDMO>();
            try
            {
                var check_activty_used = _MasterActivityContext.StudentActitvityDMO.Where(t => t.AMA_Id == ID).ToList();
                if (check_activty_used.Count == 0)
                {
                    masters = _MasterActivityContext.MasterActivityDMO.Where(t => t.AMA_Id.Equals(ID)).ToList();
                    if (masters.Any())
                    {
                        _MasterActivityContext.Remove(masters.ElementAt(0));
                        _MasterActivityContext.SaveChanges();
                    }
                    else
                    {

                    }
                }
                else
                {
                    MasterActivityDTO.message = "Delete";
                }
            }catch(Exception ex)
            {
                MasterActivityDTO.message = "Delete";
                Console.WriteLine(ex.Message);
            }
            return MasterActivityDTO;
        }


        public MasterActivityDTO MasterActivityData(MasterActivityDTO mas)
        {

            MasterActivityDMO MM = Mapper.Map<MasterActivityDMO>(mas);
            if (mas.AMA_Id != 0)
            {
                var checkDuplicates1 = _MasterActivityContext.MasterActivityDMO.Where(t => t.AMA_Activity.Equals(mas.AMA_Activity) && t.AMA_Id != mas.AMA_Id).ToList();

                if (checkDuplicates1.Count > 0)
                {
                    mas.message = "Record Already Exist";
                }
                else
                {

                    var result = _MasterActivityContext.MasterActivityDMO.Single(t => t.AMA_Id == mas.AMA_Id);
                    result.AMA_Activity = mas.AMA_Activity;
                    result.AMA_Activity_Desc = mas.AMA_Activity_Desc;
                    result.UpdatedDate = DateTime.Now;
                    result.CreatedDate = result.CreatedDate;
                    result.MI_Id = mas.MI_Id;
                    _MasterActivityContext.Update(result);
                    int n = _MasterActivityContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.returnval = true;
                        mas.messageupdate = "Update";
                    }
                    else
                    {
                        mas.returnval = false;
                        mas.messageupdate = "Update";
                    }
                }
            }
            else
            {
                MasterActivityDMO MM3 = new MasterActivityDMO();
                MM.CreatedDate = DateTime.Now;
                MM.UpdatedDate = DateTime.Now;
                Array[] showdata1 = new Array[1];

                var checkDuplicates = _MasterActivityContext.MasterActivityDMO.Where(t => t.AMA_Activity.Equals(mas.AMA_Activity)).ToList();

                if (checkDuplicates.Count > 0)
                {
                    mas.message = "Record Already Exist";
                }
                else
                {
                    MM3.AMA_Activity = mas.AMA_Activity;
                    MM3.AMA_Activity_Desc = mas.AMA_Activity_Desc;
                    MM3.MI_Id = mas.MI_Id;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.UpdatedDate = DateTime.Now;
                    _MasterActivityContext.Add(MM3);
                    int n = _MasterActivityContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.returnval = true;
                        mas.messageupdate = "Save";
                    }
                    else
                    {
                        mas.returnval = false;
                        mas.messageupdate = "Save";
                    }
                }


            }
            return mas;
        }
    }
}
