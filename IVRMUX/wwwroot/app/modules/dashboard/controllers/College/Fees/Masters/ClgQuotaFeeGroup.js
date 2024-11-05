
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgQuotaFeeGroupController', ClgQuotaFeeGroupController)

    ClgQuotaFeeGroupController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ClgQuotaFeeGroupController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
            paginationformasters = 5;
        }

        $scope.sortKey = "fmG_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "fyG_Id";
        $scope.reverse1 = true;

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            if (obj.fmG_ActiceFlag == true) {
                $scope.test = "Active";
            } else if (obj.fmG_ActiceFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.fmG_Remarks).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.searchValue1 = "";
        $scope.filterValue1 = function (obj) {
            if (obj.fyG_ActiveFlag == true) {
                $scope.test = "Active";
            } else if (obj.fyG_ActiveFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.grpname).indexOf(angular.lowercase($scope.searchValue1)) >= 0 || angular.lowercase(obj.yearname).indexOf(angular.lowercase($scope.searchValue1)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue1)) >= 0;
        }
        $scope.isOptionsRequired = function () {
            return !$scope.feegroup.some(function (options) {
                return options.selected;
            });
        }

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            var pageid = 2;
            apiService.getURI("ClgQuotaFeeGroup/getalldetails", pageid).
                then(function (promise) {

                    $scope.feegroup = promise.feegroup;
                    $scope.pages = promise.groupData;
                    $scope.Category = promise.category;
                 
                 


                })
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }

        $scope.cance = function () {
            $state.reload();
        }

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {

            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.selectacademicyear = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ClgQuotaFeeGroup/selectacademicyear", data).
                then(function (promise) {

                    // $scope.arrlistchk = promise.fillmastergroup;
                })

        }

        $scope.submitted = false;
        $scope.saveGroupdata = function () {

            if ($scope.myForm.$valid) {

               
                $scope.albumNameArray = [];
                angular.forEach($scope.feegroup, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })
                var data = {
                    //  "ASMAY_Id": $scope.ASMAY_Id,
                    "FCQCFG_Id": $scope.fcqcfG_Id,
                    //"FMG_Id": $scope.FMG_Id,
                    "ACQC_Id": $scope.ACQC_Id,
                    "FCQCFG_CompulsoryFlg": $scope.FCQCFG_CompulsoryFlg,
                  
                    "FCQCFG_ActiveFlg": true,
                    "TempararyArrayList": $scope.albumNameArray,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ClgQuotaFeeGroup/", data).then(function (promise) {
                    if (promise.returnduplicatestatus == "Duplicate") {
                        swal('Record Already Exist');
                        // $scope.cance();
                    }

                    else if (promise.returnduplicatestatus == "Save") {

                        swal('Record Saved Successfully');
                        $state.reload();
                    }

                    else if (promise.returnduplicatestatus == "NotSave") {

                        swal('Record Not Saved');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == "Update") {

                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == "NotUpdate") {

                        swal('Record Not Updated');
                        $state.reload();
                    }


                    else {
                        if (promise.message != null) {
                            swal('Record Not Updated', 'success');
                        }
                        else {
                            swal('Record Not Saved', 'success');
                        }
                        $scope.cance();
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMG_GroupName": $scope.search,
                "FMG_Remarks": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ClgQuotaFeeGroup/1", data).
                then(function (promise) {
                    $scope.pages = promise.groupData;
                    swal("searched Successfully");
                })
        }
        $scope.editEmployee = {}
           $scope.getorgvalue = function (employee) {
               $scope.editEmployee = employee.fcqcfG_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ClgQuotaFeeGroup/getdetails", pageid).
                then(function (promise) {

                    $scope.ACQC_Id = promise.groupData[0].acqC_Id;
                    $scope.FCQCFG_CompulsoryFlg = promise.groupData[0].fcqcfG_CompulsoryFlg;
                    for (var i = 0; i < $scope.feegroup.length; i++) {

                        $scope.disablegroups = true;

                        name = $scope.feegroup[i].fmG_Id;
                        if (name == promise.groupData[0].fmG_Id) {
                            $scope.feegroup[i].selected = true
                            //$scope. arrlistchk[i].disablegroups = true;
                        }
                        else {
                            $scope.feegroup[i].selected = false;
                          
                        }

                    }
                   // $scope.FMG_Id = promise.groupData[0].fmG_Id;
                    //var activeflag = promise.groupData[0].fmG_CompulsoryFlag;

                    //if (promise.groupData[0].fmG_CompulsoryFlag == "1") {
                    //    $scope.FMG_CompulsoryFlag = true;

                    //}
                    //else {
                    //    $scope.FMG_CompulsoryFlag = false;
                    //}
                    $scope.fcqcfG_Id = promise.groupData[0].fcqcfG_Id;
                })
        }
        $scope.deactive = function (groupData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupData.fcqcfG_ActiveFlg == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("ClgQuotaFeeGroup/deactivate", groupData).
                            then(function (promise) {
                                if (promise.returnval == "true") {
                                    swal("Record " + confirmmgs + " Successfully");
                                }
                                else {
                                    swal("Request Failed!!!");
                                }
                                $scope.loaddata();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectAll;
            angular.forEach($scope.arrlistchk, function (role) { role.selected = toggleStatus; });

        }

        $scope.optionToggled = function (fmG_Id) {

            if ($scope.disablegroups == false) {
                $scope.selectAll = $scope.feegroup.every(function (role) { return role.selected; })
            }
            else {
                for (var i = 0; i < $scope.feegroup.length; i++) {

                    if (fmG_Id == $scope.feegroup[i].fmG_Id) {
                        $scope.feegroup[i].selected = true;
                    }
                    else {
                        $scope.feegroup[i].selected = false;
                    }
                }
            }
        }

        var name = "";
        var roleflag = "";
        $scope.submitted1 = false;
        $scope.saveYearlyGroupdata = function (arrlistchk) {
            if ($scope.myForm1.$valid) {
                $scope.albumNameArray = [];
                angular.forEach($scope.arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })

                if ($scope.albumNameArray.length === 0) {
                    swal('Please Select Group From List Atleast One....!')
                }
                else {

                    var data = {
                        //"MI_Id": 2,
                        "FYG_Id": $scope.fyG_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "FYG_ActiveFlag": true,
                        "TempararyArrayList": $scope.albumNameArray,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("ClgQuotaFeeGroup/savedetailY", data).
                        then(function (promise) {
                            if (promise.returnduplicatestatus == 'Duplicate') {
                                swal("Record Already Exist");
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }

                            else if (promise.returnduplicatestatus == "Save") {
                                $scope.students = promise.groupData;
                                swal('Record Saved Successfully');
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }

                            else if (promise.returnduplicatestatus == "NotSave") {

                                swal('Record Not Saved');
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }
                            else if (promise.returnduplicatestatus == "Update") {
                                $scope.students = promise.groupData;
                                swal('Record Updated Successfully');
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }
                            else if (promise.returnduplicatestatus == "NotUpdate") {

                                swal('Record Not Updated');
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }

                            else {


                                if (promise.message != null) {
                                    swal('Record Not Updated', 'success');
                                }
                                else {
                                    swal('Record Not Saved', 'success');
                                }
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }

                        })
                }
            }
            else {
                $scope.submitted1 = true;
            }

        };
  
        $scope.cance1 = function (arrlistchk) {
            $scope.ASMAY_Id = "";
            for (var i = 0; i < $scope.arrlistchk.length; i++) {
                name = $scope.arrlistchk[i].selected
                if (name == true) {
                    $scope.arrlistchk[i].selected = false;
                }
            }
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.searchValue1 = "";
            $scope.searchchkbx = "";
            $scope.disablegroups = false;

            //  $scope.arrlist6 = academicyrlst;
            //  $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

        }

        $scope.searchsourceY = function () {
            var entereddata = $scope.search;
            var data = {
                "Fee": $scope.search,
                "AcademicYear": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ClgQuotaFeeGroup/1", data).
                then(function (promise) {
                    $scope.loaddata();
                    $scope.students = promise.retriveYearlyGrpdata;

                    swal("searched Successfully");
                })
        }
        $scope.editEmployeeY = {}
        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fyG_Id;
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
                        apiService.DeleteURI("ClgQuotaFeeGroup/deletepagesY", pageid).
                            then(function (promise) {
                                $scope.loaddata();
                                $scope.students = promise.retriveYearlyGrpdata;
                                if (promise.returnval != true && promise.retvalue === true) {
                                    swal('Record Not Deleted. Group Already Mapped Yearly Group Head');
                                }
                                else if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully!');
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        $scope.disablegroups = false;

        $scope.getorgvalueY = function (employee, arrlistchk, arrlist6) {
            $scope.editEmployeeY = employee.fyG_Id;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("ClgQuotaFeeGroup/getdetailsY", pageid).
                then(function (promise) {

                    for (var i = 0; i < arrlistchk.length; i++) {

                        $scope.disablegroups = true;

                        name = arrlistchk[i].fmG_Id;
                        if (name == promise.groupYearData[0].fmG_Id) {
                            arrlistchk[i].selected = true
                            //$scope. arrlistchk[i].disablegroups = true;
                        }
                        else {
                            arrlistchk[i].selected = false;
                            // $scope.arrlistchk[i].disablegroups = true;
                        }

                    }
                    $scope.ASMAY_Id = promise.groupYearData[0].asmaY_Id;
                    $scope.fyG_Id = promise.groupYearData[0].fyG_Id;
                })
        }
      
    }
    function getIndependentDrpDwnss() {
        apiService.getURI("ClgQuotaFeeGroup/getdpforyear", 2).then(function (promise) {

            $scope.arrlist6 = promise.academicdrp;

            // for pagination 
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            $scope.pages = promise.academicdrp;
            // for pagination
        })
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }


})();

