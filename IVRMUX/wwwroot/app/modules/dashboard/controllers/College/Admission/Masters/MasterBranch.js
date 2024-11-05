(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgMasterbranchController', ClgMasterbranchController)

    ClgMasterbranchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ClgMasterbranchController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.searc_button = true;
        $scope.sortKey = 'asmcL_Id';
        $scope.sortReverse = true;
        $scope.searchValue = "";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.itemsPerPage = paginationformasters;

        $scope.imgname = logopath;
        $scope.dattt = false;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.currentPage = 1;
        $scope.submitted = false;

        $scope.clgmasterbranchload = function () {
            var pageid = 2;
            apiService.getURI("ClgMasterBranch/getalldetails", pageid).then(function (promise) {
                if (promise.getdetails.length > 0) {
                    $scope.getdetails = promise.getdetails;
                    $scope.presentCountgrid = $scope.getdetails.length;
                    $scope.dattt = true;
                } else {
                    swal("No Record Found");
                    $scope.dattt = false;
                }
            });
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cleardata = function () {
            $state.reload();
        };


        $scope.savebranch = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "AMB_Id": $scope.AMB_Id,
                    "AMB_BranchName": $scope.AMB_BranchName,
                    "AMB_BranchCode": $scope.AMB_BranchCode,
                    "AMB_BranchInfo": $scope.AMB_BranchInfo,
                    "AMB_BranchType": $scope.AMB_BranchType,
                    "AMB_Order": $scope.AMB_Order,
                    "AMB_StudentCapacity": $scope.AMB_StudentCapacity,
                    "AMB_AidedUnAided": $scope.AMB_AidedUnAided
                }
                apiService.create("ClgMasterBranch/savebranch", data).then(function (promise) {
                    if (promise.message == "Duplicate") {
                        swal("Record Already Exists")
                    } else {
                        if (promise.returnval == true) {
                            if (promise.message == "Add") {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Record Updated Successfully");
                            }
                        }
                        else if (promise.returnval == false) {
                            if (promise.message == "Update") {
                                swal("Failed To Update Record");
                            } else {
                                swal("Failed To Save Record");
                            }
                        }
                        else {
                            swal("Something Went Wrong Please Contact Administrator");
                        }
                    }

                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.editbranch = function (id) {
            var data = {
                "AMB_Id": id
            }

            apiService.create("ClgMasterBranch/editbranch", data).then(function (promise) {
                if (promise.editdetails.length > 0) {
                    $scope.AMB_BranchName = promise.editdetails[0].amB_BranchName;
                    $scope.AMB_BranchCode = promise.editdetails[0].amB_BranchCode;
                    $scope.AMB_BranchInfo = promise.editdetails[0].amB_BranchInfo;
                    $scope.AMB_BranchType = promise.editdetails[0].amB_BranchType;
                    $scope.AMB_Order = promise.editdetails[0].amB_Order;
                    $scope.AMB_Id = promise.editdetails[0].amB_Id;
                    $scope.AMB_AidedUnAided = promise.editdetails[0].amB_AidedUnAided;
                    $scope.AMB_StudentCapacity = promise.editdetails[0].amB_StudentCapacity;
                }
            });
        };
        
        $scope.reset = function () {
            $scope.School_M_ClassDropdownList();
            $scope.searc_button = true;
            $scope.searchValue = "";
            $scope.searchColumn = 0;
        };

        $scope.activedeactivebranch = function (data, SweetAlert) {
            var mgs = "";

            if (data.amB_ActiveFlag == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Branch?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgMasterBranch/activedeactivebranch", data).then(function (promise) {

                            if (promise.message != null) {
                                swal(promise.message);
                            }
                            else {
                                if (promise.returnval === "true") {
                                    swal('Branch De-Activated Successfully');
                                }
                                else {
                                    swal('Branch Activated Successfully');
                                }
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        };

        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        };

        //class order
        $scope.getclassorder = function () {

            var pageid = 2;
            apiService.getURI("ClgMasterBranch/getalldetails", pageid).then(function (promosie) {
                if (promosie != null) {
                    $scope.grouptypeListOrder = promosie.getdetails
                }
                else {
                    swal("No Records Found");
                }
            });
        };

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };

        $scope.init = function () {
            $scope.resetLists();
        };
        $scope.init();

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].amB_Order = Number(index) + 1;
                }
            }
        };

        $scope.saveorder = function (newuser3) {
            var data = {
                "orderlistbranch": $scope.grouptypeListOrder,
            }
            apiService.create("ClgMasterBranch/saveorder/", data).then(function (promoise) {
                if (promoise != null) {
                    if (promoise.returnval == true) {
                        swal("Records Updated Sucessfully");
                    }
                    else {
                        swal("Failed to Update the Record");
                    }
                }
                else {
                    swal("No Records Updated");
                }

                $scope.AMB_BranchName = "";
                $scope.AMB_BranchCode = "";
                $scope.AMB_BranchInfo = "";
                $scope.AMB_BranchType = "";
                $scope.AMB_Order = "";
                $scope.AMB_Id = 0;
                $scope.AMB_AidedUnAided = "";
                $scope.AMB_StudentCapacity = "";
            });
        };

        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amB_BranchCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.amB_StudentCapacity)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.amB_Order)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.amB_BranchType)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amB_BranchInfo)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amB_AidedUnAided)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };
    }

})();