
     (function () {
         'use strict';
         angular
     .module('app')
             .controller('SMSApprovalTransactionController', SMSApprovalTransactionController)
         SMSApprovalTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$timeout']
         function SMSApprovalTransactionController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $timeout) {
             $scope.showgrid = false;
             var paginationformasters;
             var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
             if (ivrmcofigsettings.length > 0) {
                 paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
             }
             $scope.masterlist = false;
             $scope.currentPage = 1;
             $scope.itemsPerPage = paginationformasters;
             $scope.searchValue = "";
             if ($scope.itemsPerPage == undefined) {
                 $scope.itemsPerPage = 15
             }
           
             $scope.smsemailaplist = [];
             //$scope.studentdrp = false;
             $scope.maxDatemf = new Date();
             $scope.gettodate = function () {

                 $scope.minDatemf = new Date($scope.ASA_FromDate);
                 $scope.maxDatemf = new Date();
             };

             $scope.Binddata = function () {
               
       
      debugger;
                 apiService.getDATA("SMSApprovalTransaction/Getdetails").
      then(function (promise) {
         // debugger;
       
             
              $scope.headernamelist = promise.headernamelist;
        
        

      })

    }

   
             $scope.savedetails = function () {
                 $scope.smsdetailslist = [];
                 $scope.mainlist = [];
                 $scope.finalarray = [];
                 $scope.submitted = false;
                 if ($scope.myForm.$valid) {
                    
                     var fromdate1 = $scope.ASA_FromDate == null ? "" : $filter('date')($scope.ASA_FromDate, "yyyy-MM-dd");
                     var todate1 = $scope.ASA_ToDate == null ? "" : $filter('date')($scope.ASA_ToDate, "yyyy-MM-dd");

                     var data = {
                         "ASA_FromDate": fromdate1,
                         "ASA_ToDate": todate1,
                         "snd_sms": $scope.snd_sms,
                         "snd_email": $scope.snd_email,
                         "snd_call": $scope.snd_call,
                         "headername": $scope.iseS_Template_Name.trim(),
                     } 
                     apiService.create("SMSApprovalTransaction/savedetails", data).
                         then(function (promise) {
                             $scope.smsdetailslist = promise.smsdetailslist;
                             $scope.mainlist = promise.mainlist;



                             if (promise.mainlist != null &&  promise.mainlist.length > 0) {
                                 $scope.showgrid = true;

                                 angular.forEach($scope.mainlist, function (dd) {
                                     var list = [];
                                     var messg = '';
                                     angular.forEach($scope.smsdetailslist, function (ff) {
                                         if (ff.ssD_Id == dd.ssD_Id) {
                                             list.push(ff);
                                             messg = ff.ssdN_SMSMessage;
                                         }

                                     })


                                     $scope.finalarray.push({ SSD_Id: dd.ssD_Id, headername: dd.headername, SSD_TransactionId: dd.ssD_TransactionId, SSD_SentDate: dd.ssD_SentDate, dlist: list, SSDN_SMSMessage: messg })
                                 })
                                
             }
                             else {
                                 swal('No Record Found')
                             }

            
         })
                   

                 }
                 else {
                     $scope.submitted = true;
                 }

             }


             $scope.saveapprove = function () {

                 $scope.selectedlist = [];
                
                 angular.forEach($scope.finalarray, function (gg) {
                     if (gg.checkedvalue == true) {
                         $scope.selectedlist.push({ SSD_Id: gg.SSD_Id, headername: gg.headername, SSD_TransactionId: gg.SSD_TransactionId});
                     }
                     
                 })
                 if ($scope.selectedlist.length>0) {
                     var data = {
                         listdata: $scope.selectedlist,
                     }
                     apiService.create("SMSApprovalTransaction/saveapprove", data).
                         then(function (promise) {
                             if (promise.retmsg=='APR') {
                                 swal('Approved Successfully');
                             }
                             $state.reload();

                         })
                 }
                 else {
                     swal('Select Atleast One Record');
                 }
                
                   

                

             }

             $scope.rejectsms = function () {

                 $scope.selectedlist = [];
                
                 angular.forEach($scope.finalarray, function (gg) {
                     if (gg.checkedvalue == true) {
                         $scope.selectedlist.push({ SSD_Id: gg.SSD_Id, headername: gg.headername, SSD_TransactionId: gg.SSD_TransactionId});
                     }
                     
                 })
                 if ($scope.selectedlist.length>0) {
                     var data = {
                         listdata: $scope.selectedlist,
                     }
                     apiService.create("SMSApprovalTransaction/rejectsms", data).
                         then(function (promise) {
                             if (promise.retmsg=='APR') {
                                 swal('Approved Successfully');
                             }


                         })
                 }
                 else {
                     swal('Select Atleast One Record');
                 }
                
                   

                

             }


             $scope.toggleAll = function () {
                 var toggleStatus = $scope.all;
                 angular.forEach($scope.finalarray, function (itm) {
                     itm.checkedvalue = toggleStatus;
                 });
             }
             $scope.optionToggled = function (user) {
                 $scope.all = $scope.finalarray.every(function (itm) { return itm.checkedvalue; })
             }

    $scope.interacted = function (field) {
        return $scope.submitted;
    };

   
    $scope.editdata = function (user) {
        debugger;
       // var id = user.smA_Id;
        apiService.getURI("SMSMasterApproval/editdata", user.smA_Id).
     then(function (promise) {
         $scope.editdata1 = promise.editdata;
         debugger;
         $scope.smA_Id = $scope.editdata1[0].smA_Id;
         $scope.levelid = $scope.editdata1[0].smA_Level;
         $scope.type = $scope.editdata1[0].smA_SMSMailCallFlag;
         $scope.ivrmstauL_Id = $scope.editdata1[0].ivrmuL_Id;
         $scope.iseS_Template_Name = $scope.editdata1[0].smA_HeaderName;
     })

    }
    

    $scope.deactive = function (deactiveRecord) {

        debugger;
        var mgs = "";
        var confirmmgs = "";
        if (deactiveRecord.smA_ActiveFlag == true) {
            //mgs = "Deactive";
            mgs = "Deactivate";
            confirmmgs = "De-activated";

        }
        else {
            // mgs = "Active";
            mgs = "Activate";
            confirmmgs = "Activated";

        }
        swal({
            title: "Are you sure",
            text: "Do you want to " + mgs + " record??????",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: false
        },
       function (isConfirm) {
           if (isConfirm) {

               var config = {
                   headers: {
                       'Content-Type': 'application/json;'
                   }
               }

               apiService.create("SMSMasterApproval/deactivate", deactiveRecord).
                   then(function (promise) {
                       if (promise.already_cnt == true) {
                           swal("You Can Not Deactivate This Record,It Has Dependency");
                       }
                       else {
                           if (promise.returnval == true) {
                               swal("Record " + confirmmgs + " " + "successfully");
                           }
                           else {
                              
                               swal("Record " + mgs + " Failed");
                           }
                       }
                     
                      
                       $scope.Binddata();
                       $scope.cancelrpt();
                       
                   })
           }
           else {
               swal("Record " + mgs + " Cancelled");
           }
       });

    };





    $scope.sort = function (propertyName) {
       
        $scope.sortKey = propertyName;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }


             $scope.cancelrpt = function () {
                 $state.reload();

       

    }

  


         }
     })();