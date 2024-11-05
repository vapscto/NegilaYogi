(function () {
    'use strict';
    angular
.module('app')
.controller('FeeThirdPartyTransactionsController', FeeThirdPartyTransactionsController)

    FeeThirdPartyTransactionsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FeeThirdPartyTransactionsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};
        $scope.search = "";
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid=20;
            apiService.getURI("FeeChequeBounce/getalldetails", pageid).
            then(function (promise) {
                $scope.yearlst = promise.fillyear;
                $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
                $scope.studentlst = promise.fillstudent;
                $scope.totcountfirst = promise.fillstudent.length;
                $scope.receiptlst = promise.fillreceipt;
                $scope.students = promise.alldata;
                $scope.FMCB_DATE = new Date();

            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };

        $scope.onselectacademic = function (yearlst) {
            var academicyearid = $scope.cfg.ASMAY_Id;
            apiService.getURI("FeeChequeBounce/getacademicyear", academicyearid).
       then(function (promise) {
           $scope.studentlst = promise.fillstudent;
           $scope.totcountfirst = promise.fillstudent.length;
       })
        };

        $scope.onselectstudent = function (studentlst) {
            var studid = $scope.Amst_Id;
            apiService.getURI("FeeChequeBounce/getstudlistgroup", studid).
       then(function (promise) {
           $scope.receiptlst = promise.fillreceipt;
       })
        };

        $scope.cleardata = function () {
            //$scope.cfg.ASMAY_Id = "";
            //$scope.Amst_Id = "";
            //$scope.FYP_ID = "";
            //$scope.FMCB_DATE = "";
            //$scope.FMCB_Remarks = "";
            $state.reload();
        }

        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmcB_Id;
            var feechequebounceid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeeChequeBounce/Deletedetails", feechequebounceid).
                   then(function (promise) {

                       $scope.students = promise.fillstudent;
                       $scope.totcountfirst = promise.fillstudent.length;
                       if (promise.returnval == true) {

                           $scope.masterse = promise.masterSectionData;

                           swal('Record Deleted Successfully');
                       }
                       else {
                           swal('Record Not Deleted Successfully');
                       }
                   })
               }
               else {
                   swal("Record Deletion Cancelled");
               }
           });


            //})
        }

        $scope.edit = function (employee) {
            $scope.editEmployee = employee.fmcB_Id;
            var templId = $scope.editEmployee;

            apiService.getURI("FeeChequeBounce/getSchoolTypedetails", templId).
            then(function (promise) {

                $scope.cfg.ASMAY_Id = promise.fillstudent[0].asmaY_ID;
                $scope.FYP_ID = promise.fillstudent[0].fyP_ID;
                $scope.Amst_Id = promise.fillstudent[0].amsT_Id;
                $scope.FMCB_DATE = promise.fillstudent[0].fmcB_DATE;
                $scope.FMCB_Remarks = promise.fillstudent[0].fmcB_Remarks;

            })
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;
        $scope.savedata = function (studentlst) {
          
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_ID": $scope.cfg.ASMAY_Id,
                    "AMST_Id": $scope.Amst_Id,
                    "FYP_ID": $scope.FYP_ID,
                    "FMCB_DATE": $scope.FMCB_DATE,
                    "FMCB_Remarks": $scope.FMCB_Remarks
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeChequeBounce/", data).
           then(function (promise) {
               $scope.students = promise.fillstudent;
               $scope.totcountfirst = promise.fillstudent.length;
               swal("Record Saved/Updated Successfully")
               $state.reload();

           })

            }
            else
            {
                $scope.submitted = true;
            }
            
        };


    }

})();