using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Interfaces
{
    public interface FooditemtaxInterface
    {
        FooditemtaxDTO loaddata(FooditemtaxDTO data);
        FooditemtaxDTO savedata(FooditemtaxDTO data);
        FooditemtaxDTO deactivate(FooditemtaxDTO data);
    }

}
