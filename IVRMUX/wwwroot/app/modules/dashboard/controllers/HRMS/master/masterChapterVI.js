(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterChapterVIController', masterChapterVIController)

    masterChapterVIController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterChapterVIController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Bank = {};
        var hrmcvia_idvalue = 0;

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmcviA_SectionName', displayName: 'Section Name', enableHiding: false },
                { name: 'hrmcviA_SectionCode', displayName: 'Section Code', enableHiding: false },
                { name: 'hrmcviA_MaxLimit', displayName: 'Max Limit', enableHiding: false },
                //{ name: 'hrmbD_BranchName', displayName: 'Branch Name', enableHiding: false },
                //{ name: 'hrmbD_IFSCCode', displayName: 'IFSC Code', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"  data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmcviA_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmcviA_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
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
            apiService.getURI("MasterChapterVI/getalldetails", pageid).then(function (promise) {


                if (promise.emploanList !== null && promise.emploanList.length > 0) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 10;
                    $scope.gridOptions = promise.emploanList;
                    //$scope.gridOptions.data = promise.emploanList;
                    $scope.emploanList = promise.ordrlist;
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
                    "HRMCVIA_SectionName": $scope.Bank.hrmcviA_SectionName,
                    "HRMCVIA_SectionCode": $scope.Bank.hrmcviA_SectionCode,
                    "HRMCVIA_SubSectionAplFlg": $scope.Bank.hrmcviA_SubSectionAplFlg,
                    "HRMCVIA_MaxLimit": $scope.Bank.hrmcviA_MaxLimit,
                    "HRMCVIA_MaxLimitAplFlg": $scope.Bank.hrmcviA_MaxLimitAplFlg,
                    "HRMCVIA_PartFlg": $scope.Bank.hrmcviA_PartFlg,
                    "HRMCVIA_Id": hrmcvia_idvalue
                }
                apiService.create("MasterChapterVI/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {


                            if (promise.retrunMsg == "AllDuplicate") {
                                swal("Record already exist..!!");
                                return;
                            } else if (promise.retrunMsg == "Duplicate") {
                                swal("Section Name already exist..!!");
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

                            if (promise.emploanList !== null && promise.emploanList.length > 0) {

                                $scope.gridOptions.data = promise.emploanList;
                            }
                            $scope.cancel();
                        }
                    })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.resetLists = function () {
            $scope.configA = {
                onUpdate: function (evt) {
                    var itemEl = evt.item;
                }
            };
        };

        // Edit Single Record
        $scope.EditData = function (record) {

            var id = record.hrmcviA_Id;
            hrmcvia_idvalue = record.hrmcviA_Id;
            apiService.getURI("MasterChapterVI/editRecord", id).
                then(function (promise) {

                    if (promise.emploanList != null && promise.emploanList.length > 0) {
                        //$scope.Bank = promise.emploanList[0];


                        $scope.Bank.hrmcviA_SectionName = promise.emploanList[0].hrmcviA_SectionName;
                        $scope.Bank.hrmcviA_SectionCode = promise.emploanList[0].hrmcviA_SectionCode;
                        $scope.Bank.hrmcviA_SubSectionAplFlg = promise.emploanList[0].hrmcviA_SubSectionAplFlg;
                        $scope.Bank.hrmcviA_MaxLimitAplFlg = promise.emploanList[0].hrmcviA_MaxLimitAplFlg;


                        $scope.Bank.hrmcviA_MaxLimit = promise.emploanList[0].hrmcviA_MaxLimit;
                        $scope.Bank.hrmcviA_PartFlg = promise.emploanList[0].hrmcviA_PartFlg;
                        //$scope.Bank.hrmcviA_MaxLimit = promise.emploanList[0].hrmcviA_MaxLimit;

                    }


                })
        }
        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmcviA_Id !== 0) {
                    orderarray[key].hrmcviA_Order = key + 1;
                }
            });
            var data = {
                MasterChapterVIDTOO: orderarray,
            }
            apiService.create("MasterChapterVI/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.onLoadGetData();
                    }
                });
        }

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmcviA_ActiveFlg == false) {
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
                        apiService.DeleteURI("MasterChapterVI/ActiveDeactiveRecord", data.hrmcviA_Id).
                            then(function (promise) {


                                if (promise.retrunMsg !== "") {

                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                        $state.reload();
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }

                                    if (promise.emploanList !== null && promise.emploanList.length > 0) {
                                        // $scope.currentPage = 1;
                                        // $scope.itemsPerPage = 10;

                                        // $scope.employeeTypeList = promise.employeeTypeList;
                                        $scope.gridOptions.data = promise.emploanList;
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