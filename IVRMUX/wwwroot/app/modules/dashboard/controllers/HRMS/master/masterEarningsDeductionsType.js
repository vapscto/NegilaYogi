(function () {
    'use strict';
    angular
.module('app')
.controller('masterEarningsDeductionsTypeController', masterEarningsDeductionsTypeController)

    masterEarningsDeductionsTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter']
    function masterEarningsDeductionsTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {



        //#region Master Earning Deduction Type Form Data starts
        $scope.earnedtype = {};

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("MasterEarningsDeductions/getalldetails", pageid).then(function (promise) {


                if (promise.eardettypelist !== null && promise.eardettypelist.length > 0) {
                    $scope.deductiontypegridOptions.data = promise.eardettypelist;
                }

            })


        }


        $scope.deductiontypegridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableHiding: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'hrmedT_EarnDedType', displayName: 'Earning / Deductions Type', enableHiding: false },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditDataType(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hrmedT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecordType(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hrmedT_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecordType(row.entity);">  Deactivate</a><span>' +
                 '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.deductiontygridApi = gridApi;
            }

        };



        $scope.cancelType = function () {
            // $scope.search = "";
            $scope.earnedtype = {};
            $scope.submittedType = false;
            $scope.myFormType.$setPristine();
            $scope.myFormType.$setUntouched();
            $scope.deductiontygridApi.grid.clearAllFilters();
        }

        //saving/updating Record


        $scope.submittedType = false;
        $scope.saveDataType = function () {
            $scope.submittedType = true;
            if ($scope.myFormType.$valid) {
                var data = $scope.earnedtype;

                apiService.create("MasterEarningsDeductions/savetype", data).
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
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }

                        $scope.onLoadGetData();
                        $scope.cancelType();
                    }
                })
            }

        };

        $scope.interactedType = function (field) {
            return $scope.submittedType || field.$dirty;
        };
        // Edit Single Record
        $scope.EditDataType = function (record) {

            var id = record.hrmedT_Id;
            apiService.getURI("MasterEarningsDeductions/editRecordtype", id).
                then(function (promise) {

                    if (promise.eardettypelist != null && promise.eardettypelist.length > 0) {
                        $scope.earnedtype = promise.eardettypelist[0];
                    }

                   
                })
        }

        //deactivate record
        $scope.DeletRecordType = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmedT_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.DeleteURI("MasterEarningsDeductions/ActiveDeactiveRecordtype", data.hrmedT_Id).
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

                           $scope.onLoadGetData();
                       }

                   })
               }
               else {
                   swal(" Cancelled", "Ok");
               }
           }

           );
        }
        //#endregion Master Earning Deduction Type Form Data Ends


    }

})();