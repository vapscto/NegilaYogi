
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterRoleController', MasterRoleController)

    MasterRoleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window','superCache']
    function MasterRoleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/pagemapping/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/masterroletype/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            var pageid = 2;
            apiService.getURI("MasterRole/getalldetails", pageid).
        then(function (promise) {
            $scope.pages = promise.pagesdata;
            // $scope.IVRMR_Order = promise.maxcount;
            //$scope.totalItems = $scope.pages.length;
            //$scope.numPerPage = 5;
        })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        //$scope.predicate = 'sno';
        //$scope.reverse = false;
        //$scope.currentPage = 1;
        //$scope.order = function (predicate) {
        //    $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        //    $scope.predicate = predicate;
        //};

        //$scope.pages = promise.pagesdata;
        //$scope.totalItems = $scope.pages.length;
        //$scope.numPerPage = 5;

        //$scope.paginate = function (value) {
        //    var begin, end, index;
        //    begin = ($scope.currentPage - 1) * $scope.numPerPage;
        //    end = begin + $scope.numPerPage;
        //    index = $scope.pages.indexOf(value);
        //    return (begin <= index && index < end);
        //};

        $scope.searchsource = function () {
            var entereddata = $scope.search;

            var data = {
                "IVRMR_Role": $scope.search,
                "IVRMR_Role_desc": $scope.type
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterRole/1", data).
        then(function (promise) {
            $scope.pages = promise.pagesdata;
            if (promise.pagesdata.length>0)
                swal("Searched Successfully");
            else
                swal("No Data Found");
        })
        }

        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.id;
            var pageid = $scope.editEmployee;

            var msg;
            if (employee.activeFlag == 1) {
                msg = "Deactivate";
            }
            else {
                msg = "Activate";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + msg + " record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + msg + " it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterRole/deletepages", pageid).
                    then(function (promise) {
                        if (promise != null && promise != "") {
                            if (promise.returnval === "true") {
                                swal('Record Deactivated Successfully', 'success');
                                $state.reload();
                            }
                            else if (promise.returnval === "false") {
                                swal('Record Activated Successfully!', 'success');
                                $state.reload();
                            }
                            else {
                                swal('Record not  Activated/Deactivated.!', 'Failed');
                                return;
                            }

                        }
                        else {
                            swal('Record not Activated/Deactivated.!', 'Failed');
                            return;
                        }

                    })
                }
                else {
                    swal("Record Activation/Deactivation Cancelled");
                }
            });
        }


        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterRole/getdetails", pageid).
            then(function (promise) {

                $scope.Name = promise.pagesdata[0].name;
                $scope.NormalizedName = promise.pagesdata[0].normalizedName;
                //  $scope.IVRMR_Order = promise.pagesdata[0].ivrmR_Order;
                $scope.IVRMR_Id = promise.pagesdata[0].id;

            })
        }

        $scope.clearfields = function () {
            //$scope.Name = "";
            //$scope.NormalizedName = "";
            // $scope.IVRMR_Order = "";
            $state.reload();
        }
        $scope.submitted = false;
        $scope.savepages = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "Name": $scope.Name,
                    "NormalizedName": $scope.NormalizedName,
                    //"IVRMR_Order": $scope.IVRMR_Order
                    "IVRMR_Id": $scope.IVRMR_Id
                }
                apiService.create("MasterRole/", data).
                then(function (promise) {
                  
                    if (promise.returnval == "Save" || promise.returnval == "Update") {
                    if(promise.returnval == "Save")
                    {
                        swal('Record Saved Successfully', 'success');
                        $state.reload();
                    }
                    else if(promise.returnval == "Update")
                    {
                        swal('Record Updated Successfully', 'success');
                        $state.reload();
                    }
                }
                else if (promise.returnval == "NotSave" || promise.returnval == "NotUpdate") {
                    if (promise.returnval == "NotSave") {
                        swal('Record Not Saved');
                        $state.reload();
                    }
                    else if (promise.returnval == "NotUpdate") {
                        swal('Record Not Updated');
                        $state.reload();

                    }
                }
                else if (promise.returnduplicatestatus == "Duplicate") {
                    swal('Master Role Already Exist');

                }

                })
            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }

})();