
     (function () {
         'use strict';
         angular
     .module('app')
             .controller('SMSMasterApprovalController', SMSMasterApprovalController)
         SMSMasterApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$timeout']
         function SMSMasterApprovalController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $timeout) {
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
             $scope.Binddata = function () {
      
       
      debugger;
                 apiService.getDATA("SMSMasterApproval/Getdetails").
      then(function (promise) {
         // debugger;
          if (promise.userNamelist !=null) {
              $scope.userNamelist = promise.userNamelist;
              $scope.headernamelist = promise.headernamelist;
          }
          else {
              swal(" User name Not found")
          }
        
          if (promise.smsemailaplist != null) {

              $scope.smsemailaplist = promise.smsemailaplist;
              if ($scope.smsemailaplist.length>0) {
                  $scope.showgrid = true;
              }
            
          }

      })

    }

   
             $scope.savedetails = function () {

                 $scope.submitted = false;
                 if ($scope.myForm.$valid) {
                     var data = {
                         "SMA_Id": $scope.smA_Id,
                         "IVRMSTAUL_Id": $scope.ivrmstauL_Id,
                         "SMA_SMSMailCallFlag": $scope.type.trim(),
                         "SMA_Level": $scope.levelid.trim(),
                         "headername": $scope.iseS_Template_Name.trim(),
                     } 
                     apiService.create("SMSMasterApproval/savedetails", data).
         then(function (promise) {

             if (promise.returnval==true) {
                 swal("Record Saved/Updated Successfully")
             }
             if (promise.returnval == false && promise.dup != 'Duplicate') {
                 swal("Not Saved/Updated")
             }
             if (promise.dup == 'Duplicate') {
                 swal("Duplicate data")
             }
             if (promise.dup == 'level') {
                 swal("level assigned for other user for same template")
             }
             $scope.Binddata();
             $scope.cancelrpt();
             

         })
                   

                 }
                 else {
                     $scope.submitted = true;
                 }

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


    $scope.cancelrpt=function(){
     //   $scope.smA_Id = '';
        $scope.levelid = '';
        $scope.type = '';
        $scope.ivrmstauL_Id = '';
        $scope.iseS_Template_Name = '';

       

    }

  


         }
     })();