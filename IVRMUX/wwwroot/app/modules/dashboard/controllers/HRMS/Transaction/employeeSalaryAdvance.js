(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeeSalaryAdvanceController', employeeSalaryAdvanceController)
    employeeSalaryAdvanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function employeeSalaryAdvanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        // form Object
        $scope.saladvance = {};

        $scope.yearDis = true;
        $scope.monthDis = true;
        $scope.configurationDetails = {};

        $scope.empGrossSalDis = true;


        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'masterEmployee.hrmE_EmployeeFirstName', displayName: 'Employee Name', enableHiding: false },
                { name: 'hresA_EntryDate', displayName: 'Entry Date', enableHiding: false },
                { name: 'hresA_AdvYear', displayName: 'Advance Year', enableHiding: false },
                { name: 'hresA_AdvMonth', displayName: 'Advance Month', enableHiding: false },
                { name: 'hresA_AppliedAmount', displayName: 'Advance Amount', enableHiding: false },
                //{ name: 'hresA_AdvStatus', displayName: 'Status', enableHiding: false },

                { name: 'hresA_Remarks', displayName: 'Remarks', enableHiding: false },

                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hresA_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity)";data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hresA_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity); " data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }

                //{
                //    field: 'id', name: '',
                //    displayName: 'Actions', enableFiltering: false, enableSorting: false,
                //    cellTemplate:
                //        '<div class="grid-action-cell">' +
                //        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" data-toggle="tooltip" title="Edit" > <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                //        '<a href="javascript:void(0)" ng-click="grid.appScope.deletedataY(row.entity);" data-placement="bottom" data-toggle="tooltip" title="Delete"> <i class="fa fa-trash"></i></a> &nbsp; &nbsp;' +
                //        '<a ng-if="row.entity.HRML_LeaveCreditFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                //        '<span ng-if="row.entity.HRML_LeaveCreditFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                //        '</div>'
                //}






            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        // Get form Details at onload 
        $scope.onLoadGetData = function () {

            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("HREmpSalaryAdvance/getalldetails", pageid).then(function (promise) {

                if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                    $scope.gridOptions.data = promise.empadvaList;
                    $scope.gridOptions = promise.empadvaList;
                }

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }
                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }
                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

                if (promise.modeOfPaymentdropdown !== null && promise.modeOfPaymentdropdown.length > 0) {
                    $scope.modeOfPaymentdropdown = promise.modeOfPaymentdropdown;
                }

                angular.forEach($scope.gridOptions.data, function (value, key) {
                    var fdate = value.hresA_EntryDate.split('T');
                    value.hresA_EntryDate = fdate[0];
                });

                //
                if (promise.configurationDetails !== null) {

                    $scope.configurationDetails = promise.configurationDetails;

                    //var montha = new Date();
                    var date = new Date();
                    var year = date.getFullYear();
                    var montha = date.getMonth();

                    var out = $scope.monthdropdown.filter(function (x) {
                        var match = x.ivrM_Month_Id == montha;

                        return match;
                    });


                    var yearId = parseInt(year);
                    var monthId = parseInt(out[0].ivrM_Month_Id);
                    var startDay = parseInt($scope.configurationDetails.hrC_SalaryFromDay);
                    var endDay = parseInt($scope.configurationDetails.hrC_SalaryToDay);


                    if (startDay > 1 && monthId < 12) {

                        $scope.minDatedoi = new Date(
                            yearId,
                            monthId - 1,
                            startDay);

                        $scope.maxDatedoi = new Date(
                            yearId,
                            monthId,
                            endDay);


                    } else if (startDay > 1 && monthId == 12) {

                        $scope.minDatedoi = new Date(
                            yearId,
                            monthId - 1,
                            startDay);

                        $scope.maxDatedoi = new Date(
                            yearId + 1,
                            0,
                            endDay);


                    } else {

                        var days = getNumberOfDays(yearId, monthId);

                        $scope.minDatedoi = new Date(
                            yearId,
                            monthId - 1,
                            startDay);

                        $scope.maxDatedoi = new Date(
                            yearId,
                            monthId - 1,
                            days);

                    }


                }




            })
        }

        function getNumberOfDays(year, month) {
            var isLeap = ((year % 4) == 0 && ((year % 100) != 0 || (year % 400) == 0));
            return [0, 31, (isLeap ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
        }


        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $scope.obj.hrmE_Id = "";
           
            $scope.saladvance = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.editflg = false;
            $scope.gridApi.grid.clearAllFilters();
          
        }
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                if ($scope.saladvance.hresA_EntryDate != undefined && $scope.saladvance.hresA_EntryDate != "") {
                    $scope.saladvance.hresA_EntryDate = new Date($scope.saladvance.hresA_EntryDate).toDateString();
                }
                else {
                    $scope.saladvance.hresA_EntryDate = null;
                }

                var data = $scope.saladvance;
                apiService.create("HREmpSalaryAdvance/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg == "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully..");
                            }
                            else if (promise.retrunMsg == "acc") {
                                swal("Account No. is already exist..");
                                return;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                            if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                                $scope.gridOptions.data = promise.empadvaList;
                                angular.forEach($scope.gridOptions.data, function (value, key) {
                                    var fdate = value.hresA_EntryDate.split('T');
                                    value.hresA_EntryDate = fdate[0];

                                });
                            }
                            $scope.cancel();
                        }
                    })
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.setMonthAndyear = function (date) {

            $scope.saladvance.hresA_AdvYear = "";
            $scope.saladvance.hresA_AdvMonth = "";

            // var milis = Date.parse(date);
            // var d = new Date(date)

            var year = date.getFullYear();
            var montha = date.getMonth() + 1;

            var month = $filter('filter')($scope.monthdropdown, function (a) {
                return a.ivrM_Month_Id === montha;
            })[0].ivrM_Month_Name;



            $scope.saladvance.hresA_AdvYear = year;
            $scope.saladvance.hresA_AdvMonth = month;

            $scope.GetDetailsByEmployee();
        }

        $scope.editflg = false;
        // Edit Single Record
        $scope.lablenamme = {};
        $scope.EditData = function (record) {
            debugger
            $scope.saladvance = {};
            $scope.editflg = true;
            var id = record.hresA_Id;
            apiService.getURI("HREmpSalaryAdvance/editRecord", id).
                then(function (promise) {
                    if (promise.empadvaList != null && promise.empadvaList.length > 0) {
                        $scope.saladvance = promise.empadvaList[0];
                        $scope.lablenamme1 = promise.empadvaList[0].hrmE_Id;
                        $scope.saladvance.hresA_EntryDate = new Date(promise.empadvaList[0].hresA_EntryDate);

                        $scope.saladvance.empGrossSal = promise.empGrossSal;
                        $scope.saladvance.totalAppliedAmount = promise.totalAppliedAmount;
                        angular.forEach($scope.employeedropdown, function (wer) {
                            if (wer.hrmE_Id == $scope.lablenamme1) {
                                $scope.lablenamme = wer.hrmE_EmployeeFirstName;
                            }
                        })
                    }
                })
        }
        $scope.obj = {};
        $scope.GetDetailsByEmployee = function (obj) {

            $scope.saladvance.empGrossSal = "";
            $scope.saladvance.hrmE_Id = $scope.obj.hrmE_Id.hrmE_Id;
            if ($scope.saladvance.hrmE_Id != "" && $scope.saladvance.hresA_EntryDate != null && $scope.saladvance.hresA_EntryDate != "") {
                if ($scope.saladvance.hresA_EntryDate != undefined && $scope.saladvance.hresA_EntryDate != "") {
                    $scope.saladvance.hresA_EntryDate = new Date($scope.saladvance.hresA_EntryDate).toDateString();
                }
                else {
                    $scope.saladvance.hresA_EntryDate = null;
                }
                var data = $scope.saladvance;
                // $scope.saladvance.hrmE_Id;
                apiService.create("HREmpSalaryAdvance/getDetailsByEmployee", data).
                    then(function (promise) {
                        if (promise.empGrossSal != 0 && promise.empGrossSal != null) {
                            $scope.saladvance.empGrossSal = promise.empGrossSal;
                            $scope.saladvance.totalAppliedAmount = promise.totalAppliedAmount;
                        }
                    })
            }

        }

        $scope.validateAdvanceAmount = function () {

            if ($scope.saladvance.hresA_AppliedAmount != "" && $scope.saladvance.hresA_AppliedAmount != undefined) {

                if ($scope.saladvance.hresA_AppliedAmount > $scope.saladvance.empGrossSal) {

                    swal('Advance Amount Should Be Less than Gross Salary ');
                    $scope.saladvance.hresA_AppliedAmount = "";
                } else {
                    var maxminAmout = parseFloat($scope.saladvance.empGrossSal).toFixed(2) - parseFloat($scope.saladvance.totalAppliedAmount).toFixed(2);
                    if (maxminAmout > 0) {
                        if ($scope.saladvance.hresA_AppliedAmount > maxminAmout) {

                            swal('Already Issued Advance of ' + $scope.saladvance.totalAppliedAmount + ' Rupees. So Advance Amount Should Be Less than ' + maxminAmout + ' Rupees. ');
                            $scope.saladvance.hresA_AppliedAmount = "";
                        }
                    } else {
                        swal('Already Issued Advance of ' + $scope.saladvance.totalAppliedAmount + ' Rupees. So Advance Amount Should Be Less than ' + $scope.saladvance.empGrossSal + ' Rupees. ');
                        $scope.saladvance.hresA_AppliedAmount = "";
                    }

                }



            }
        }



        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hresA_ActiveFlag == false) {
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
                        apiService.DeleteURI("HREmpSalaryAdvance/ActiveDeactiveRecord", data.hresA_Id).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }
                                    if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                                        $scope.gridOptions.data = promise.empadvaList;
                                        angular.forEach($scope.gridOptions.data, function (value, key) {
                                            var fdate = value.hresA_EntryDate.split('T');
                                            value.hresA_EntryDate = fdate[0];

                                        });
                                    }
                                }

                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        }



        //search filter
        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '3') {
                $scope.studentlst = "";
                var data = {
                    "searchfilter": objj.search,
                }
                var data = $scope.saladvance;
                apiService.create("HREmpSalaryAdvance/searchfilter", data).
                    then(function (promise) {
                        if (promise.employeedropdown != null || promise.employeedropdown.length > 0) {
                            $scope.employeedropdown = promise.employeedropdown;
                            // $scope.saladvance.empGrossSal = promise.empGrossSal;
                        } else {
                            $scope.saladvance = "";
                            swal("No Staff are found for your search");
                        }
                    })
            }
        };



    }
})();