using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Interfaces
{
    public interface FoodtransactionInterface
    {
        FoodtransactionDTO loaddata(FoodtransactionDTO data);
        FoodtransactionDTO FoodItem(FoodtransactionDTO data);
        FoodtransactionDTO FoodItemtax(FoodtransactionDTO data);
        FoodtransactionDTO savedata(FoodtransactionDTO data);
        FoodtransactionDTO paymenthistory(FoodtransactionDTO data);
        FoodtransactionDTO deactivate(FoodtransactionDTO data);
        FoodtransactionDTO getstudent(FoodtransactionDTO data);
        FoodtransactionDTO savesmartData(FoodtransactionDTO data);
        FoodtransactionDTO trns_cancel(FoodtransactionDTO data);
        FoodtransactionDTO orderdeatils(FoodtransactionDTO data);
        FoodtransactionDTO foodreport(FoodtransactionDTO data);
        FoodtransactionDTO Month_Daywise_graph(FoodtransactionDTO data);
        FoodtransactionDTO YearWise_graph(FoodtransactionDTO data);
        FoodtransactionDTO paymenthistory_print(FoodtransactionDTO data);
        FoodtransactionDTO paymenthistory_print_onetime(FoodtransactionDTO data);
        

    }
    
}
