(function () {
    'use strict';
    angular
.module('app')
.controller('masterEarningsDeductionsController', masterEarningsDeductionsController)

    masterEarningsDeductionsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter']
    function masterEarningsDeductionsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        //#region Master Earning Deduction Form Data starts

        // form Object
        $scope.EarningDet = {};
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;

       // $scope.EarningDet.hrmeD_AmountPercentFlag = 'Amount';

        $scope.GetErningDeductionDetails = function () {

            $scope.cancel();
            $scope.onLoadGetData();
        }


        // Datatable display
        $scope.gridOptionsEarning = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [

              { name: 'hrmeD_Name', displayName: 'Earning', enableHiding: false },
              { name: 'hrmeD_AmountPercent', displayName: 'Amount/Per', enableHiding: false },
              {
                  name: 'percentOff', displayName: '% of', enableHiding: false
              },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hrmeD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hrmeD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                 '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApiEarning = gridApi;
            }

        };

        //Deduction datatable list

        $scope.gridOptionsDeduction = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'hrmeD_Name', displayName: 'Deduction' },
              { name: 'hrmeD_AmountPercent', displayName: 'Amount/Per' },
              {
                  name: 'percentOff', displayName: '% of'
              },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hrmeD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hrmeD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                 '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApiDeduction = gridApi;
            }

        };

        $scope.gridOptionsArrear = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [

              { name: 'hrmeD_Name', displayName: 'Arrear', enableHiding: false },
              { name: 'hrmeD_AmountPercent', displayName: 'Amount/Per', enableHiding: false },
              {
                  name: 'percentOff', displayName: '% of', enableHiding: false
              },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hrmeD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hrmeD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                 '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApiArrear = gridApi;
            }

        };

        //Gross
        $scope.gridOptionsGross = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [

              { name: 'hrmeD_Name', displayName: 'Gross', enableHiding: false },
              { name: 'hrmeD_AmountPercent', displayName: 'Amount/Per', enableHiding: false },
              //{
              //    name: 'percentOff', displayName: '% of', enableHiding: false
              //},
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hrmeD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hrmeD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                 '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApiGross = gridApi;
            }

        };

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("MasterEarningsDeductions/getalldetails", pageid).then(function (promise) {


                if (promise.earningdetectionList !== null && promise.earningdetectionList.length > 0) {
                  

                    $scope.earningdetectionList = promise.earningdetectionList;
                }

                $scope.gridOptionsEarning.data = promise.earningList;

                //deduction
                $scope.gridOptionsDeduction.data = promise.detectionList;

                $scope.gridOptionsArrear.data = promise.arrearlist;

                $scope.gridOptionsGross.data = promise.grosslist;

                //type dropdown
                if (promise.eardettypeDropdown !== null && promise.eardettypeDropdown.length > 0) {
                    $scope.eardettypeDropdown = promise.eardettypeDropdown;
                }

                $scope.earningdetectionListOrder = promise.earningdetectionList;

            })


        }


        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $scope.listDataDis = true;
            $scope.AmountPercentDis = true;
            $scope.AmountPertcentLabel = "Amount/Percentage";
            $scope.EarningDet = {};
            $scope.EarningDet.hrmeD_AmountPercentFlag = 'Amount';
            $scope.setAmountPercentLable("Amount")

            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.sectioncheckboxchcked = [];
            $scope.gridApiEarning.grid.clearAllFilters();
            $scope.gridApiDeduction.grid.clearAllFilters();
            $scope.gridApiArrear.grid.clearAllFilters();
            $scope.gridApiGross.grid.clearAllFilters();
        }


        $scope.sectioncheckboxchcked = [];
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveDataErningDeduction = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.EarningDet.perc_OfDTO = [];

                if ($scope.EarningDet.hrmeD_AmountPercentFlag === "Amount") {

                    $scope.sectioncheckboxchcked = [];

                    angular.forEach($scope.earningdetectionList, function (data, key) {
                        if (data.Selected) {
                            $scope.earningdetectionList[key].Selected = false;
                        }
                    })
                }
                else if ($scope.EarningDet.hrmeD_AmountPercentFlag === "Percentage") {

                    angular.forEach($scope.earningdetectionList, function (data) {
                        if (data.Selected == true) {
                            data.hrmedP_HRMED_Id = data.hrmeD_Id;
                            data.hrmeD_Id = 0;
                            $scope.sectioncheckboxchcked.push(data);
                           
                            console.log(data);
                        }
                    })

                    if ($scope.sectioncheckboxchcked == null || $scope.sectioncheckboxchcked.length == 0) {
                        swal('Kindly select atleast one record', 'Select Percentage Of')
                        return;
                    }
                }

               
                $scope.EarningDet.perc_OfDTO = $scope.sectioncheckboxchcked;

                var data = $scope.EarningDet;

                apiService.create("MasterEarningsDeductions/", data).
                then(function (promise) {

                    $scope.EarningDet.perc_OfDTO = [];

                    if (promise.retrunMsg !== "") {

                        if (promise.retrunMsg == "Duplicate") {
                            swal("Name already exist..!!");
                            return;
                        } else if (promise.retrunMsg == "AllDuplicate") {
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
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }

                        if (promise.earningdetectionList !== null && promise.earningdetectionList.length > 0) {
                            //$scope.currentPage = 1;
                            //$scope.itemsPerPage = 10;

                            //list data 
                            $scope.earningdetectionList = promise.earningdetectionList;

                            $scope.gridOptionsEarning.data = promise.earningList;
                            //deductionlist
                            $scope.gridOptionsDeduction.data = promise.detectionList;
                            $scope.gridOptionsArrear.data = promise.arrearlist;
                            $scope.gridOptionsGross.data = promise.grosslist;


                            $scope.earningdetectionListOrder = promise.earningdetectionList;
                        }
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

            $scope.EarningDet = [];

            angular.forEach($scope.earningdetectionList, function (value, key) {

                $scope.earningdetectionList[key].Selected = false;
            });

            var id = record.hrmeD_Id;
            apiService.getURI("MasterEarningsDeductions/editRecord", id).
                then(function (promise) {

                    if (promise.earningdetectionList !=null && promise.earningdetectionList.length > 0) {
                        $scope.EarningDet = promise.earningdetectionList[0];

                        $scope.AmountPercentDis = false;

                        if ($scope.EarningDet.hrmeD_AmountPercentFlag == 'Amount') {
                            $scope.AmountPertcentLabel = "Amount";
                            $scope.listDataDis = false;
                        }
                        else if ($scope.EarningDet.hrmeD_AmountPercentFlag == 'Percentage') {
                            $scope.AmountPertcentLabel = "Percentage";
                            $scope.listDataDis = true;
                        }
                    }
                    if (promise.selectedearningdetectionList != null && promise.selectedearningdetectionList.length > 0) {


                        angular.forEach($scope.earningdetectionList, function (value, key) {

                            angular.forEach(promise.selectedearningdetectionList, function (value1, key1) {
                                
                                if (value.hrmeD_Id == value1.hrmedP_HRMED_Id) {
                                    $scope.earningdetectionList[key].Selected = true;
                                }
                            });

                        });
                    }
                       
                })
        }


        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmeD_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + "Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.getURI("MasterEarningsDeductions/ActiveDeactiveRecord", data.hrmeD_Id).
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

                           if (promise.earningdetectionList !== null && promise.earningdetectionList.length > 0) {
                               // $scope.currentPage = 1;
                               // $scope.itemsPerPage = 10;

                               //list data 
                                 $scope.earningdetectionList = promise.earningdetectionList;

                                $scope.gridOptionsEarning.data = promise.earningList;

                               //deductionlist
                               $scope.gridOptionsDeduction.data = promise.detectionList;

                               $scope.gridOptionsArrear.data = promise.arrearlist;
                               $scope.gridOptionsGross.data = promise.grosslist;

                               $scope.earningdetectionListOrder = promise.earningdetectionList;
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
        $scope.listDataDis = true;
        $scope.AmountPercentDis = true;
        $scope.AmountPertcentLabel = "Amount/Percentage";
        $scope.setAmountPercentLable = function (da) {

            $scope.AmountPercentDis = false;
            if (da == "Amount") {
                $scope.AmountPertcentLabel = "Amount";
                $scope.listDataDis = false;

                angular.forEach($scope.earningdetectionList, function (data, key) {
                    if (data.Selected) {
                        $scope.earningdetectionList[key].Selected = false;
                    }
                })
            }
            else if (da == "Percentage") {
                $scope.AmountPertcentLabel = "Percentage";
                $scope.listDataDis = true;
            }
        }





        //fix the order drag
        //ConfigA is an Items
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
        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmeD_Id !== 0) {
                    orderarray[key].hrmeD_Order = key + 1;
                }
            });
            var data = {
                EarningsDeductionsDTO: orderarray,
            }
            apiService.create("MasterEarningsDeductions/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.onLoadGetData();

                    }
                });
        }


        //#endregion Master Earning Deduction Form Data Ends

       



    }

})();