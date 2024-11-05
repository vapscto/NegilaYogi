(function () {
    'use strict';
    angular
.module('app')
.controller('SpecialFeeHeadClgController', SpecialFeeHeadClgController)

    SpecialFeeHeadClgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function SpecialFeeHeadClgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {

            return angular.lowercase(obj.fmH_FeeName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.search = "";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "fmsfhfH_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("SpecialFeeHeadClg/getalldetailsY", pageid).
        then(function (promise) {
            $scope.arrlistchk = promise.groupHeadData;
            $scope.students = promise.newarydatah;

            $scope.totcountfirst = $scope.students.length;

        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        var name = "";
        var roleflag = "";
        $scope.cance = function (arrlistchk) {
            $scope.search = "";
            $scope.FMSFH_Name = "";
            $scope.searchchkbx = "";
            for (var i = 0; i < arrlistchk.length; i++) {

                arrlistchk[i].selected = false;

            }
            $scope.fmsfhfH_Id = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        }
        $scope.isOptionsRequired_1 = function () {

            return !$scope.arrlistchk.some(function (options) {
                return options.selected;
            });
        }
        $scope.submitted = false;
        $scope.saveYearlyGroupdata = function (arrlistchk) {
            
            if ($scope.myForm.$valid) {

                $scope.albumNameArray = [];
                angular.forEach($scope.arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })
                if ($scope.albumNameArray.length === 0) {
                    swal('Please Select Fee Head From List Atleast One....!')
                }
                else {
                    var data = {
                        "FMSFHFH_Id": $scope.fmsfhfH_Id,
                        "FMSFH_Name": $scope.FMSFH_Name,
                        "FMSFH_ActiceFlag": true,
                        "IVRMSTAUL_Id": 787926,
                        "TempararyArrayList": $scope.albumNameArray,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("SpecialFeeHeadClg/savedetailY", data).
                    then(function (promise) {
                        
                        if (promise.returnduplicatestatus === 'Duplicate' && promise.returnval === false) {
                            swal('Record Already Exist');
                        }
                        else if (promise.returnval === true) {
                            $scope.students = promise.newarydatah;
                            swal('Record ' + promise.returnduplicatestatus + ' Sucessfully');
                            $state.reload();
                            $scope.cance($scope.arrlistchk);
                        }
                        else {
                            swal('Record Not Saved/Updated Successfully');
                        }

                    })

                }
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.searchsourceY = function () {
            var entereddata = $scope.search;
            var data = {
                "FMSFH_Name": $scope.search,
                "FMSFH_ActiceFlag": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("SpecialFeeHeadClg/1", data).
        then(function (promise) {
          //  $scope.loaddata();
            //   $scope.students = promise.retrivefeeHeadData;
            $scope.students = promise.newarydatah;
            swal("searched Successfully");
            $state.reload();
        })
        }
        $scope.editEmployeeY = {}
        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fmsfhfH_Id;
            var pageid = $scope.editEmployeeY;
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
                    apiService.DeleteURI("SpecialFeeHeadClg/deletepagesY", pageid).
                    then(function (promise) {
                        $scope.students = promise.newarydatah;
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                            $state.reload();
                        }
                        else {
                            swal('Record Not Deleted');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalueY = function (employee, arrlistchk) {
            $scope.editEmployeeY = employee.fmsfhfH_Id;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("SpecialFeeHeadClg/getdetailsY", pageid).
            then(function (promise) {
                $scope.FMSFH_Name = promise.fmsfH_Name;
                for (var i = 0; i < arrlistchk.length; i++) {
                    if (arrlistchk[i].fmH_Id == promise.editidh) {
                        arrlistchk[i].selected = true
                    }
                }
                $scope.fmsfhfH_Id = promise.fmsfhfH_Id;
            })
        }
        // for deactive avtive 
        $scope.deactiveY = function (newarydatah) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            //if (newarydatah.fmsfH_ActiceFlag ==true) {
            if (newarydatah.fmsfhfH_ActiceFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("SpecialFeeHeadClg/deactivateY", newarydatah).
                then(function (promise) {

                    if (promise.returnduplicatestatus == "used") {
                        swal("Record already Used");
                    }
                    else {
                        if (promise.returnval == true) {
                            swal("Record " + confirmmgs + " " + " Successfully.");
                            $state.reload();
                        }
                        else {
                            swal("Record Not Activated ");
                            $state.reload();
                        }
                       
                    }
                  

                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        })
        }
    };
})();


