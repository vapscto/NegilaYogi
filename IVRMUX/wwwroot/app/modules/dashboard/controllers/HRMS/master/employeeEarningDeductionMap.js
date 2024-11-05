(function () {
    'use strict';
    angular
.module('app')
.controller('employeeEarningDeductionMapController', employeeEarningDeductionMapController)

    employeeEarningDeductionMapController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter']
    function employeeEarningDeductionMapController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        // form Object
        $scope.EarningDet = {};


        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  //{ name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'hrmpeD_ED_Name', displayName: 'Earning' },
               { name: 'hrmpeD_PC_Amount_Flag', displayName: 'Amount/Per' },
                 { name: 'hreeD_ED_Amount', displayName: 'Amount' },
                  { name: 'hreeD_Percentage', displayName: 'Percentage' },
              { name: 'hreeD_PercentageOf_Name', displayName: '% of' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hreeD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hreeD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                 '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };



        //Deduction datatable list

        $scope.deductiongridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  //{ name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'hrmpeD_ED_Name', displayName: 'Deductions' },
               { name: 'hrmpeD_PC_Amount_Flag', displayName: 'Amount/Per' },
                 { name: 'hreeD_ED_Amount', displayName: 'Amount' },
                   { name: 'hreeD_Percentage', displayName: 'Percentage' },
              { name: 'hreeD_PercentageOf_Name', displayName: '% of' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hreeD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hreeD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
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
            apiService.getURI("EmployeeEarningDeductionMap/getalldetails", pageid).then(function (promise) {

                if (promise.listData !== null && promise.listData.length > 0) {
                    //head list
                    $scope.listData = promise.listData;
                }

                if (promise.employeedetailList != null && promise.employeedetailList.length > 0) {
                    //employee list
                    $scope.employeedetailList = promise.employeedetailList;
                }

                //earning list
                if (promise.earningList != null && promise.earningList.length > 0) {
                    $scope.gridOptions.data = promise.earningList;
                }
               
                if (promise.detectionList != null && promise.detectionList.length > 0) {
                    //deduction
                    $scope.deductiongridOptions.data = promise.detectionList;
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
            $scope.AmountPercent = "";
            $scope.listData = [];
            $scope.headList = [];
            $scope.sectioncheckboxchcked = [];
            $scope.listDataDis = false;
            $scope.AmountPercentDis = true;
            $scope.AmountPertcentLabel = "Amount/Percentage";
            $scope.EarningDet = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.onLoadGetData();
        }
        //

        //$scope.chckedIndexs = [];
        //$scope.test = function (data) {

        //    console.log(data.Selected);
        //    if ($scope.chckedIndexs.indexOf(data) === -1) {
        //        if (data.Selected) $scope.chckedIndexs.push(data);
        //    }
        //    else {
        //        $scope.chckedIndexs.splice($scope.chckedIndexs.indexOf(data), 1);
        //    }
        //}


        $scope.GetEarningDeductionList = function (hrmpeD_ED_Flag) {

            $scope.headList = [];

            var dataED = {
                "HRMPED_ED_Flag": hrmpeD_ED_Flag,
                "Type": "EarningDeductionList"
            }

            apiService.create("EmployeeEarningDeductionMap/", dataED).
                then(function (promise) {

                    //earning/deduction list
                    if (promise.earningdetectionList != null && promise.earningdetectionList.length > 0) {
                        //deduction
                        $scope.headList = promise.earningdetectionList;
                    }

                })
        }

        $scope.sectioncheckboxchcked = [];

        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function (data) {


            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                data.perc_OfDTO = [];

                angular.forEach($scope.listData, function (data, key) {
                    if (data.Selected) {
                        $scope.sectioncheckboxchcked.push(data);
                        console.log(data);
                    }
                })

                if (data.hrmpeD_PC_Amount_Flag === "Amount") {
                    data.hreeD_ED_Amount = $scope.AmountPercent;
                    data.hreeD_Percentage = "";
                }
                else if (data.hrmpeD_PC_Amount_Flag === "Percentage") {
                    data.hreeD_Percentage = $scope.AmountPercent;
                    data.hreeD_ED_Amount = "";

                    if ($scope.sectioncheckboxchcked == null || $scope.sectioncheckboxchcked.length == 0) {
                        swal('Kindly select atleast one record', 'Select Percentage Of')
                        return;
                    }
                }

                data.perc_OfDTO = $scope.sectioncheckboxchcked;

                apiService.create("EmployeeEarningDeductionMap/", data).
                then(function (promise) {
                    if (promise.retrunMsg !== "") {

                        if (promise.retrunMsg == "Duplicate") {
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
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }

                        //if (promise.headList !== null && promise.headList.length > 0) {
                        //    //head list
                        //    $scope.headList = promise.headList;
                        //}

                        //if (promise.employeedetailList != null && promise.employeedetailList.length > 0) {
                        //    //employee list
                        //    $scope.employeedetailList = promise.employeedetailList;
                        //}

                        ////earning list
                        //if (promise.earningList != null && promise.earningList.length > 0) {
                        //    $scope.gridOptions.data = promise.earningList;
                        //}

                        //if (promise.detectionList != null && promise.detectionList.length > 0) {
                        //    //deduction
                        //    $scope.deductiongridOptions.data = promise.detectionList;
                        //}

                        ////earning/deduction list
                        //if (promise.earningdetectionList != null && promise.earningdetectionList.length > 0) {
                        //    //deduction
                        //    $scope.listData = promise.earningdetectionList;
                        //}
                        $scope.cancel();
                    }
                })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {

            for (var i = 0; i < $scope.listData.length; i++) {
                $scope.listData[i].Selected = false;
            }

            $scope.EarningDet = {};
            $scope.headList = [];
            $scope.listDataDis = false;
            var id = record.hreedM_Id;
            apiService.getURI("EmployeeEarningDeductionMap/editRecord", id).
                then(function (promise) {

                    if (promise.getselectedearningdetectionList != null && promise.getselectedearningdetectionList.length > 0) {

                        $scope.SetEarningDeductionListByID(promise.getselectedearningdetectionList[0].hrmpeD_ED_Flag,promise.getselectedearningdetectionList[0].hrmpeD_Id);

                       $scope.EarningDet = promise.getselectedearningdetectionList[0];


                        $scope.setAmountPercentLable(promise.getselectedearningdetectionList[0].hrmpeD_PC_Amount_Flag);

                        if (promise.getselectedearningdetectionList[0].hrmpeD_PC_Amount_Flag == "Amount") {
                            $scope.AmountPercent = promise.getselectedearningdetectionList[0].hreeD_ED_Amount;

                        }
                        else if (promise.getselectedearningdetectionList[0].hrmpeD_PC_Amount_Flag == "Percentage") {

                            $scope.AmountPercent = promise.getselectedearningdetectionList[0].hreeD_Percentage;
                        }
                    }


                    if (promise.getselectedlistData != null && promise.getselectedlistData.length > 0) {

                       // $scope.listData = promise.getselectedlistData;
                        $scope.listDataDis = true;
                        angular.forEach(promise.getselectedlistData, function (value, key) {

                            var hrmpeD_Id = promise.getselectedlistData[key].hrmpeD_Id

                            for (var i = 0; i < $scope.listData.length; i++) {
                                if ($scope.listData[i].hrmpeD_Id == hrmpeD_Id) {
                                    $scope.listData[i].Selected = true;
                                }
                            }
                           
                        })
                    }
                })
        }

        $scope.SetEarningDeductionListByID = function (hrmpeD_ED_Flag, hrmpeD_Id) {

            $scope.headList = [];

            var dataED = {
                "HRMPED_ED_Flag": hrmpeD_ED_Flag,
                "Type": "EarningDeductionList"
            }

            apiService.create("EmployeeEarningDeductionMap/", dataED).
                then(function (promise) {

                    //earning/deduction list
                    if (promise.earningdetectionList != null && promise.earningdetectionList.length > 0) {
                        //deduction
                        $scope.headList = promise.earningdetectionList;
                        angular.forEach($scope.headList, function (head, key) {
                            if (head.hrmpeD_Id == hrmpeD_Id) {
                                head.Selected = true;
                               // console.log(head);
                                $scope.EarningDet.hrmpeD_Id = head.hrmpeD_Id.toString();
                            }

                        })
                    }

                })
        }

        $scope.headList = [];
        $scope.SetHeadName = function (hrmpeD_Id) {
            $scope.EarningDet.hrmpeD_ED_Name = "";
            if (hrmpeD_Id != "" && hrmpeD_Id != undefined) {

                angular.forEach($scope.headList, function (value, key) {
                    if (value.hrmpeD_Id === parseInt(hrmpeD_Id)) {

                        $scope.EarningDet.hrmpeD_ED_Name = value.hrmpeD_ED_Name;
                    }
                })
            }
        }

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hreeD_ActiveFlag == false) {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Earning and Deduction..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.DeleteURI("EmployeeEarningDeductionMap/ActiveDeactiveRecord", data.hrmeD_Id).
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

                           $scope.cancel();
                       }

                   })
               }
               else {
                   swal(" Cancelled", "Ok");
               }
           }

           );
        }

        $scope.listDataDis = false;
        $scope.AmountPercentDis = true;
        $scope.AmountPertcentLabel = "Amount/Percentage";
        $scope.setAmountPercentLable = function (da) {

            $scope.AmountPercentDis = false;
            if (da == "Amount") {
                $scope.AmountPertcentLabel = "Amount";
                $scope.listDataDis = false;
            }
            else if (da == "Percentage") {
                $scope.AmountPertcentLabel = "Percentage";
                $scope.listDataDis = true;
            }


        }
    }

})();