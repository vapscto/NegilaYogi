(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterQuarterController', masterQuarterController)

    masterQuarterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterQuarterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Bank = {};
        var hrmq_idvalue = 0;

        $scope.monthDis = true;
        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmQ_QuarterName', displayName: 'Quarter Name', enableHiding: false },
                { name: 'hrmQ_FromDay', displayName: 'From Date', enableHiding: false },
                { name: 'hrmQ_FromMonth', displayName: 'From Month', enableHiding: false },
                { name: 'hrmQ_ToDay', displayName: 'To Date', enableHiding: false },
                { name: 'hrmQ_ToMonth', displayName: 'To Month', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"  data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmQ_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmQ_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }


        };

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("MasterQuarter/getalldetails", pageid).then(function (promise) {
                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

                if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 10;
                    $scope.gridOptions = promise.bankdetailList;
                    //$scope.gridOptions.data = promise.bankdetailList;
                    angular.forEach($scope.gridOptions.data, function (value, key) {
                        var fdate = value.hrmQ_FromDay.split('T');
                        value.hrmQ_FromDay = fdate[0];
                        var tdate = value.hrmQ_ToDay.split('T');
                        value.hrmQ_ToDay = tdate[0];
                    });
                }
              
            })
        }

        //// Sort table data
        //$scope.sort = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}

        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $scope.Bank = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        }

        //saving/updating Record
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
               var data1 = $scope.Bank;
                var data = {
                   // "IMFY_Id": $scope.abc,
                    "HRMQ_QuarterName": $scope.Bank.hrmQ_QuarterName,
                    "HRMQ_FromDay": new Date($scope.Bank.hrmQ_FromDay).toDateString(),
                    "HRMQ_FromMonth": $scope.Bank.hrmQ_FromMonth,
                    "HRMQ_ToDay":new Date($scope.Bank.hrmQ_ToDay).toDateString(),
                    "HRMQ_ToMonth": $scope.Bank.hrmQ_ToMonth,
                    "HRMQ_Id": hrmq_idvalue
                }
                apiService.create("MasterQuarter/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {


                            if (promise.retrunMsg === "AllDuplicate") {
                                swal("Record already exist..!!");
                                return;
                            } else if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                            }
                            else if (promise.retrunMsg == "acc") {
                                swal("Account No. is already exist..");
                                return;
                            }
                            else if (promise.retrunMsg == "branch") {
                                swal("Branch Name already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg == "ifsc") {
                                swal("IFSC Code is already exist..");
                                return;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }

                            if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {

                                $scope.gridOptions.data = promise.bankdetailList;



                                angular.forEach($scope.gridOptions.data, function (value, key) {
                                    var fdate = value.hrmQ_FromDay.split('T');
                                    value.hrmQ_FromDay = fdate[0];
                                    var tdate = value.hrmQ_ToDay.split('T');
                                    value.hrmQ_ToDay = tdate[0];
                                });
                            }
                            $scope.cancel();
                        }
                    })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        $scope.setMonthAndyear = function (date) {

           // $scope.saladvance.hresA_AdvYear = "";
            $scope.Bank.hrmQ_FromMonth
 = "";

            // var milis = Date.parse(date);
            // var d = new Date(date)

            var year = date.getFullYear();
            var montha = date.getMonth() + 1;

            var month = $filter('filter')($scope.monthdropdown, function (a) {
                return a.ivrM_Month_Id === montha;
            })[0].ivrM_Month_Name;



            //$scope.saladvance.hresA_AdvYear = year;
            $scope.Bank.hrmQ_FromMonth = month;

          //  $scope.GetDetailsByEmployee();
        }


        $scope.setMonthAndyears = function (date) {

           /// $scope.saladvance.hresA_AdvYear = "";
            $scope.Bank.hrmQ_ToMonth= "";

            // var milis = Date.parse(date);
            // var d = new Date(date)

            var year = date.getFullYear();
            var montha = date.getMonth() + 1;

            var month = $filter('filter')($scope.monthdropdown, function (a) {
                return a.ivrM_Month_Id === montha;
            })[0].ivrM_Month_Name;



          //  $scope.saladvance.hresA_AdvYear = year;
            $scope.Bank.hrmQ_ToMonth = month;

            //  $scope.GetDetailsByEmployee();
        }

        // Edit Single Record
        $scope.EditData = function (record) {

            var id = record.hrmQ_Id;
            $scope.select = id;
            hrmq_idvalue = record.hrmQ_Id;
            apiService.getURI("MasterQuarter/editRecord", id).
                then(function (promise) {

                    if (promise.bankdetailList != null && promise.bankdetailList.length > 0) {
                        $scope.Bank.hrmQ_QuarterName = promise.bankdetailList[0].hrmQ_QuarterName;
                        $scope.Bank.hrmQ_FromDay = new Date(promise.bankdetailList[0].hrmQ_FromDay);
                        //$scope.Bank.hrmQ_FromDay = promise.bankdetailList[0].hrmQ_FromDay;
                        $scope.Bank.hrmQ_FromMonth = promise.bankdetailList[0].hrmQ_FromMonth;
                        $scope.Bank.hrmQ_ToDay = new Date(promise.bankdetailList[0].hrmQ_ToDay);
                       // $scope.Bank.hrmQ_ToDay = promise.bankdetailList[0].hrmQ_ToDay;
                        $scope.Bank.hrmQ_ToMonth= promise.bankdetailList[0].hrmQ_ToMonth;
                    }


                })
        }


        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmQ_ActiveFlg == false) {
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
                        apiService.DeleteURI("MasterQuarter/ActiveDeactiveRecord", data.hrmQ_Id).
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

                                    if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {
                                     
                                        $scope.gridOptions.data = promise.bankdetailList;
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
    }

})();