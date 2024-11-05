
(function () {
    'use strict';
    angular
.module('app')
.controller('FeeGroupsGroupingController', fee1)

    fee1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function fee1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$stateParams) {
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {

            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.savedisable = true;

        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }
        $scope.sortKey = "fmgG_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.search = "";
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeGroupsGrouping/getalldetailsY", pageid).
        then(function (promise) {
            $scope.arrlistchk = promise.groupData;
            $scope.students = promise.newarydata;

            $scope.totcountfirst = $scope.students.length;
            //$scope.user = {
            //    arrlistchk: [$scope.arrlistchk[1]]
            //};
            //$scope.checkAll = function () {
            //    $scope.user.arrlistchk = angular.copy($scope.arrlistchk);
            //};
            //$scope.uncheckAll = function () {
            //    $scope.user.arrlistchk = [];
            //};
            //$scope.checkFirst = function () {
            //    $scope.user.arrlistchk.splice(0, $scope.user.arrlistchk.length);
            //    $scope.user.arrlistchk.push($scope.arrlistchk[0]);
            //};
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        $scope.cance = function () {

            //$scope.fmgG_Id = 0;
            //$scope.FMGG_GroupName = "";
            //$scope.FMGG_GroupCode = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();

            $state.reload();
        }
        $scope.submitted = false;
        $scope.saveYearlyGroupdata = function (arrlistchk) {
            if ($scope.myForm.$valid) {
                $scope.albumNameArray = [];
                angular.forEach($scope.arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })

                if ($scope.albumNameArray.length > 0) {
                    var data = {
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMGG_GroupCode": $scope.FMGG_GroupCode,
                        "FMGG_GroupName": $scope.FMGG_GroupName,
                        "FMGG_ActiveFlag": true,
                        "TempararyArrayList": $scope.albumNameArray,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeGroupsGrouping/savedetailY", data).
                        then(function (promise) {
                            swal('Record  Saved/Updated Successfully');
                            $scope.loaddata();
                        //if (promise.returnduplicatestatus !== 'NULL' && promise.returnduplicatestatus === 'Duplicate') {
                        //    swal('Record Already Exist');
                        //}
                        //else if (promise.returnval === true) {
                        //    swal('Record ' + promise.returnduplicatestatus + ' Successfully');
                        //    $scope.cance();
                        //    $scope.loaddata();
                        //}
                        //else {
                        //    swal('Record Not Saved/Updated Successfully');
                        //}

                    })

                }
                else {
                    swal('Select Atleast one Group');
                }
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.searchsourceY = function () {
            var entereddata = $scope.search;
            var data = {
                "FMGG_GroupName": $scope.search,
                "FMGG_GroupCode": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeGroupsGrouping/1", data).
        then(function (promise) {
            $scope.loaddata();
            $scope.students = promise.newarydata;
            swal("searched Successfully");
        })
        }
        var name = "";
        $scope.editEmployeeY = {}
        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fmgG_Id;
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
                    apiService.DeleteURI("FeeGroupsGrouping/deletepagesY", pageid).
                    then(function (promise) {
                        $scope.loaddata();
                        $scope.students = promise.newarydata;

                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully', 'Kindly Contact Administrator');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalueY = function (employee, arrlistchk) {
            $scope.editEmployeeY = employee.fmggG_Id;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("FeeGroupsGrouping/getdetailsY", pageid).
            then(function (promise) {

                $scope.fmgG_Id = promise.groupGroupingData[0].fmgG_Id;
                $scope.FMGG_GroupName = promise.groupGroupingData[0].fmgG_GroupName;
                $scope.FMGG_GroupCode = promise.groupGroupingData[0].fmgG_GroupCode;

                for (var i = 0; i < arrlistchk.length; i++) {
                    if (arrlistchk[i].fmG_Id == promise.editid) {
                        arrlistchk[i].selected = true
                    }
                    else {
                        arrlistchk[i].selected = false
                    }
                }
            })
        }
        //for deactive avtive 

        $scope.deactiveY = function (det) {
            var id = {
                "FMGGG_Id": det.fmggG_Id,
            }
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
                        apiService.create("FeeGroupsGrouping/deactivateY/", id).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                    return;
                                }
                                else if (promise.returnval === false) {
                                    swal('Record Not Deleted');
                                    return;
                                }
                                else if (promise.returnval === "Mapped") {
                                    swal("Sorry.....You can not delete this record.Because it is already mapped");
                                }
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }

        //$scope.deactiveY = function (newarydata) {

        //    var mgs = "";
        //    var confirmmgs = "";
        //    var config = {
        //        headers: {
        //            'Content-Type': 'application/json;'
        //        }
        //    }
        //    if (newarydata.fmgG_ActiveFlag == 1) {
        //        mgs = "Deactivate";
        //        confirmmgs = "Deactivated";

        //    }
        //    else {
        //        mgs = "Activate";
        //        confirmmgs = "Activated";

        //    }
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do You Want To " + mgs + " Record?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //  function (isConfirm) {
        //      if (isConfirm) {                 
        //        apiService.create("FeeGroupsGrouping/deactivateY", newarydata).
        //        then(function (promise) {
        //             if (promise.returnval === true) {
        //                 swal( "Record "+confirmmgs + " Successfully");
        //                 $scope.loaddata();
        //             }
        //             else {
        //                 swal("Record " + confirmmgs + " Successfully");
        //             }

        //         })
        //      }
        //      else {
        //          swal("Record " + mgs + " Cancelled");
        //      }
        //  });

        //}

        $scope.isOptionsRequired_1 = function () {

            return !$scope.arrlistchk.some(function (options) {
                return options.selected;
            });
        }
    }
})();

