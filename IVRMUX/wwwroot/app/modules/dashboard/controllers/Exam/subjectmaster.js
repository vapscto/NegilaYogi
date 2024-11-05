

//dashboard.controller("attendanceEntryTypeController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash) {

//}]);


(function () {
    'use strict';
    angular
.module('app')
.controller('subjectmasterController', subjectmasterController)

    subjectmasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function subjectmasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

      //TO  GEt The Values iN Grid

        $scope.BindData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;

            apiService.getDATA("subjectmaster/Getdetails").
       then(function (promise) {
           if (promise.count > 0)
           {
               $scope.newuser = promise.subjectmastername;
           }
           else {
               swal("No Records Found.....!!");
           }
       })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }


        };

        $scope.Deletesubjectmasterdata = function (DeleteRecord) {
            $scope.deleteId = DeleteRecord.ismS_Id;
            var MdeleteId = $scope.deleteId;
            var mgs = "";
            var confirmmgs = "";
            if (DeleteRecord.ismS_ActiveFlag ==true) {
                mgs = "Disable";
                confirmmgs = "Disable";

            }
            else {
                mgs = "Enable";
                confirmmgs = "Enable";

            }

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("subjectmaster/MasterDeleteModulesDTO", MdeleteId).
                    then(function (promise) {

                        swal(promise.msg);
                        $state.reload();
                    })
                }
                else {
                    swal("Record " + mgs + " Cancelled");
                }
            });
        }

     
        //$scope.Deletesubjectmasterdata = function (DeleteRecord) {
        //    $scope.deleteId = DeleteRecord.amsU_Id;
        //    var MdeleteId = $scope.deleteId;
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do you want to Enable/Disable record !!!!!!!!",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
        //        cancelButtonText: "Cancel!!!!!!",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //    function (isConfirm) {
        //        if (isConfirm) {
        //            apiService.DeleteURI("subjectmaster/MasterDeleteModulesDTO", MdeleteId).
        //            then(function (promise) {

        //                if (promise.returnval === "true") {
        //                    swal('Record Deleted Successfully');
        //                }
        //                else if (promise.returnval === "false") {
        //                    swal('Record Not Deleted');
        //                }

        //                $state.reload();
        //            })
        //        }
        //        else {
        //            swal("Record Deletion Cancelled");
        //        }
        //    });
        //}



        // to Edit Data
        $scope.Editsubjectmasterdata = function (EditRecord) {
         
            $scope.EditId = EditRecord.ismS_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("subjectmaster/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.newuser.ismS_Id = promise.subjectmastername[0].ismS_Id;
                $scope.code = promise.subjectmastername[0].ismS_SubjectCode;
                $scope.name = promise.subjectmastername[0].ismS_SubjectName;
                $scope.order = promise.subjectmastername[0].ismS_OrderFlag;
                $scope.admissionprocedure = promise.subjectmastername[0].ismS_PreadmFlag;
                $scope.batchwise = promise.subjectmastername[0].ismS_BatchAppl;
                $scope.ISMS_Id = promise.subjectmastername[0].ismS_Id;
               // $scope.timetableFlag = promise.subjectmastername[0].timeTable_flag;

                if (promise.subjectmastername[0].ismS_BatchAppl === false) {

                    $scope.batchwise = false;
                }
                else {

                    $scope.batchwise = true;
                }

                if (promise.subjectmastername[0].ismS_PreadmFlag === true) {

                    $scope.admissionprocedure = true;
                }
                else {

                    $scope.admissionprocedure = false;
                }

                //if (promise.subjectmastername[0].timeTable_flag === "Y") {

                //    $scope.timetableFlag = true;
                //}
                //else {

                //    $scope.timetableFlag = false;
                //}

            })
        };

            $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }        


        // TO Save The Data
            
            $scope.savesubjectmasterdata = function () {

                if ($scope.myForm.$valid) {
             var batchflag = $scope.batchwise;
             var admproflag = $scope.admissionprocedure;               
            // var TTflag = $scope.timetableFlag;
                 
                    if (batchflag == true) {
                        batchflag = true;
                    }
                    else {
                        batchflag =false;
                    }
                  
                    if (admproflag == true) {
                        admproflag = true;
                    }
                    else {
                        admproflag = false;
                    }

                    //if (TTflag == true) {
                    //    TTflag = "Y";
                    //}
                    //else {
                    //    TTflag = "N";
                    //}

                var data = {
                    "ISMS_Id": $scope.ISMS_Id,
                    "ISMS_SubjectCode": $scope.code,
                    "ISMS_SubjectName": $scope.name,
                    "ISMS_PreadmFlag": admproflag,
                    "ISMS_OrderFlag": $scope.order,
                    "ISMS_BatchAppl": batchflag,
                   // "TimeTable_flag": TTflag
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                    apiService.create("subjectmaster/", data).
                             then(function (promise) {
                                 if (promise.msg == "orderduplicate")
                                 {
                                     swal("Entered order is already mapped.Please enter different one.");
                                     return;
                                 }
                                else if (promise.msg != null && promise.msg != "") {
                                    swal(promise.msg);
                                    return;
                                 }
                                 else if(promise.returnval==true)
                                 {
                                     swal("Record saved/Updated Successfully");
                                     $state.reload();
                                 }
                                 else if(promise.returnval==false)
                                 {
                                     swal("Failed to saved/Update");
                                 }
                                 
                             })
                } else {
                    $scope.submitted = true;
                }

            };
           
            $scope.interacted = function (field) {

                return $scope.submitted || field.$dirty;
            };

            $scope.cancel = function () {
             $scope.ISMS_Id = "";
            $scope.code = "";
            $scope.name = "";
            $scope.order = "";
            $state.reload();
        }      
           
    }

})();