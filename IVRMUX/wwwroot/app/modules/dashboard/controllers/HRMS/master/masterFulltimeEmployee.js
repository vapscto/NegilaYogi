(function () {
    'use strict';
    angular
        .module('app')
        .controller('hrmasterempFullTimeController', hrmasterempFullTimeController)

    hrmasterempFullTimeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function hrmasterempFullTimeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name', enableHiding: false },
                { name: 'asmaY_Year', displayName: 'Academic Year', enableHiding: false },
                { name: 'asmaY_From_Date', displayName: 'From Date', enableHiding: false },
                { name: 'asmaY_To_Date', displayName: 'To Date', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"  data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmepT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmepT_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("HRMasterEmpFullTime/getalldetails", pageid).then(function (promise) {
                $scope.employeedropdownlist = promise.employeelist;
                $scope.academicyearlist = promise.academicyearlist;

                if (promise.empdata.length > 0) {
                    angular.forEach(promise.empdata, function (data) {
                        if (data.asmaY_From_Date != undefined && data.asmaY_From_Date != "") {
                            data.asmaY_From_Date = $filter('date')(data.asmaY_From_Date, "dd/MM/yyyy");
                        }
                        else {
                            data.asmaY_From_Date = "";
                        }

                        if (data.asmaY_To_Date != undefined && data.asmaY_To_Date != "") {
                            data.asmaY_To_Date = $filter('date')(data.asmaY_To_Date, "dd/MM/yyyy");
                        }
                        else {
                            data.asmaY_To_Date = "";
                        }
                    });
                }
                $scope.gridOptions.data = promise.empdata;

            });
        };

        // clear form data
        $scope.cancel = function () {
            $scope.hrmE_Id = "";
            $scope.hrmepT_Year = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $state.reload();
        };

        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.selectedEmp = [];
            angular.forEach($scope.employeedropdownlist, function (emp) {
                if (emp.employee == true) {
                    $scope.selectedEmp.push({ HRME_Id: emp.hrmE_Id});
                }
            })

            if ($scope.myForm.$valid) {
                var data = {
                   // "HRME_Id": $scope.hrmE_Id,
                    "HRMEPT_Id": $scope.hrmepT_Id,
                    selectedEmp: $scope.selectedEmp,
                    "HRMEPT_Year": $scope.hrmepT_Year
                };

                apiService.create("HRMasterEmpFullTime/savedata", data).
                    then(function (promise) {
                        debugger;
                        if (promise.msg == 'saved') {
                            swal("Record Saved Successfully!");
                           
                            $state.reload();
                        }
                        else if (promise.msg == 'updated') {
                            swal("Record Updated Successfully!");                          
                            $state.reload();
                        }
                        else if (promise.msg == 'duplicate') {
                            swal("Record already exist");
                        }
                        else if (promise.msg == "savingFailed") {
                            swal("Failed to save record");
                        }
                        else if (promise.msg == "updateFailed") {
                            swal("Failed to update record");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }

                            if (promise.empdata !== null && promise.empdata.length > 0) {
                                if (promise.empdata.length > 0) {
                                    angular.forEach(promise.empdata, function (data) {
                                        if (data.asmaY_From_Date != undefined && data.asmaY_From_Date != "") {
                                            data.asmaY_From_Date = $filter('date')(data.asmaY_From_Date, "dd/MM/yyyy");
                                        }
                                        else {
                                            data.asmaY_From_Date = "";
                                        }

                                        if (data.asmaY_To_Date != undefined && data.asmaY_To_Date != "") {
                                            data.asmaY_To_Date = $filter('date')(data.asmaY_To_Date, "dd/MM/yyyy");
                                        }
                                        else {
                                            data.asmaY_To_Date = "";
                                        }
                                    });
                                }
                                $scope.gridOptions.data = promise.empdata;
                            }
                            $scope.cancel();                     
                    });
            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {
            var id = record.hrmepT_Id;
            apiService.getURI("HRMasterEmpFullTime/editRecord", id).
                then(function (promise) {
                    if (promise.fulltimedetailList != null && promise.fulltimedetailList.length > 0) {
                        $scope.hrmepT_Id = promise.fulltimedetailList[0].hrmepT_Id;
                        $scope.hrmE_Id = promise.fulltimedetailList[0].hrmE_Id;
                        $scope.hrmepT_Year = promise.fulltimedetailList[0].hrmepT_Year;

                        angular.forEach($scope.employeedropdownlist, function (emp) {
                            angular.forEach(promise.fulltimedetailList, function (ss) {
                                if (emp.hrmE_Id == ss.hrmE_Id) {
                                    emp.employee = true;
                                }
                            })
                        })
                    }
                });
        };

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmepT_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },

            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("HRMasterEmpFullTime/ActiveDeactiveRecord", data.hrmepT_Id).
                        then(function (promise) {
                            if (promise !== "") {

                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();


                                if (promise.empdata !== null && promise.empdata.length > 0) {
                                    if (promise.empdata.length > 0) {
                                        angular.forEach(promise.empdata, function (data) {
                                            if (data.asmaY_From_Date != undefined && data.asmaY_From_Date != "") {
                                                data.asmaY_From_Date = $filter('date')(data.asmaY_From_Date, "dd/MM/yyyy");
                                            }
                                            else {
                                                data.asmaY_From_Date = "";
                                            }

                                            if (data.asmaY_To_Date != undefined && data.asmaY_To_Date != "") {
                                                data.asmaY_To_Date = $filter('date')(data.asmaY_To_Date, "dd/MM/yyyy");
                                            }
                                            else {
                                                data.asmaY_To_Date = "";
                                            }
                                        });
                                    }
                                    $scope.gridOptions.data = promise.empdata;
                                }
                            }
                        });
                }
                else {
                    swal("Record " + mgs + " Cancelled");
                }
            });
        };


        $scope.togchkbx = function () {
            $scope.usercheck = $scope.employeedropdownlist.every(function (options) {
                return options.employee;
            });
        }

        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.employeedropdownlist, function (itm) {
                itm.employee = toggleStatus;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.employeedropdownlist.some(function (options) {
                return options.employee;
            });
        }


    }

})();