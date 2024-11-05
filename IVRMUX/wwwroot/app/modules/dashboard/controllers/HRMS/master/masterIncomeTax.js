(function () {
    'use strict';
    angular
.module('app')
.controller('masterIncomeTaxController', masterIncomeTaxController)

    masterIncomeTaxController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterIncomeTaxController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.IncTax = {};

        $scope.incometaxDetails = [{ id: 'incometax' }];

        $scope.incometaxDetails[0].cess = false;

        $scope.cessNameDis = true;

        $scope.enableDisable = function (da) {




            if (da == true) {
                $scope.cessNameDis = false;
            } else {
                $scope.cessNameDis = true;
            }

        }

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                        {
                            name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>'
                        },
                        {
                            name: 'financilYear.imfY_FinancialYear', displayName: 'Financial Year', enableHiding: false
                        },
                        {
                            name: 'gendername.ivrmmG_GenderName', displayName: 'Gender', enableHiding: false
                        },
                        {
                            name: 'hrmiT_AgeFlag', displayName: 'Age', enableHiding: false
                        },
                        {
                            name: 'hrmiT_FromAge', displayName: 'From Age', enableHiding: false
                        },
                        {
                            name: 'hrmiT_ToAge', displayName: 'To Age', enableHiding: false
                        },
                        {
                            field: 'id', name: '',
                            displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                            '<div class="grid-action-cell">' +
                            '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                            '<a ng-if="row.entity.hrmiT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                            '<span ng-if="row.entity.hrmiT_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                            '</div>'
                        }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };


        //compare both Amount
        $scope.checkErr = function (FromAmount, ToAmount) {
            if (parseFloat(FromAmount) > parseFloat(ToAmount)) {
                swal("Amount From should not be greater than Amount To", 'Please Change Your amount!');
                $scope.IncTaxDetail.hrmitD_AmountFrom = "";
                return false;
            }
        };

        $scope.checkErr1 = function (FromAmount, ToAmount) {
            if (parseFloat(FromAmount) > parseFloat(ToAmount)) {
                swal("Amount To should be greater than Amount From", 'Please Change Your amount!');
                $scope.IncTaxDetail.hrmitD_AmountTo = "";
                return false;
            }
        };

        //Validate Age

        $scope.validateFromAge = function (FromAge, ToAge) {
            if ($scope.IncTax.hrmiT_AgeFlag == 'LessThan60') {
                if (parseFloat(FromAge) >= 60) {
                    swal("From Age should Less than 60", 'Please Change Age!');
                    $scope.IncTax.hrmiT_FromAge = "";
                    return false;
                }
            }
            else if ($scope.IncTax.hrmiT_AgeFlag == 'Between6080') {
                if (parseFloat(FromAge) <= 60) {
                    swal("From Age should More than 60", 'Please Change Age!');
                    $scope.IncTax.hrmiT_FromAge = "";
                    return false;
                }
                else if (parseFloat(FromAge) >= 80) {
                    swal("From Age should Less than 80", 'Please Change Age!');
                    $scope.IncTax.hrmiT_FromAge = "";
                    $scope.IncTax.hrmiT_ToAge = "";
                    return false;
                }


                else if (parseFloat(ToAge) >= 80) {
                    swal("From Age should Less than 80", 'Please Change Age!');
                    $scope.IncTax.hrmiT_FromAge = "";
                    $scope.IncTax.hrmiT_ToAge = "";
                    return false;
                }
            }

            else if ($scope.IncTax.hrmiT_AgeFlag == 'Above80') {
                if (parseFloat(FromAge) <= 80) {
                    swal("From Age should More than 80", 'Please Change Age!');
                    $scope.IncTax.hrmiT_FromAge = "";
                    return false;
                }
            }




            if (parseFloat(FromAge) > parseFloat(ToAge)) {
                swal("From Age should not be greater than To Age", 'Please Change Age!');
                $scope.IncTax.hrmiT_FromAge = "";
                return false;
            }
        };

        $scope.validateToAge = function (FromAge, ToAge) {
            if (parseFloat(FromAge) > parseFloat(ToAge)) {
                swal("To Age should be greater than From Age", 'Please Change Age!');
                $scope.IncTax.hrmiT_ToAge = "";
                return false;
            }
        };


       


        $scope.addNewIncometax = function () {
            var newItemNo = $scope.incometaxDetails.length + 1;

            if (newItemNo <= 10) {
                $scope.incometaxDetails.push({ 'id': 'incometax' + newItemNo });

                $scope.incometaxDetails[newItemNo - 1].cess = false;

                // incometax.exitDateDis
            }
        };




        $scope.removeNewIncometax = function (index, data) {
            var newItemNo = $scope.incometaxDetails.length - 1;
            $scope.incometaxDetails.splice(index, 1);

            //if (data.hrmeE_Id > 0) {
            //    $scope.DeleteIncometaxData(data);
            //}

            //if ($scope.incometaxDetails.length === 0) {
            //}
        };



        // Get form Details at onload 
        $scope.onloadGetData = function () {

            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("MasterIncomeTax/getalldetails", pageid).then(function (promise) {

                //incomeTaxList
                if (promise.incomeTaxList !== null && promise.incomeTaxList.length > 0) {
                    //$scope.gridOptions.data = promise.incomeTaxList;
                    $scope.gridOptions = promise.incomeTaxList;
                }

                //genderdropdown

                if (promise.genderdropdown !== null && promise.genderdropdown.length > 0) {
                    $scope.genderdropdown = promise.genderdropdown;
                }
                //financialYeardropdown

                if (promise.financialYeardropdown !== null && promise.financialYeardropdown.length > 0) {
                    $scope.financialYeardropdown = promise.financialYeardropdown;
                }

                if (promise.incomeTaxCessdropdown !== null && promise.incomeTaxCessdropdown.length > 0) {
                    $scope.incomeTaxCessdropdown = promise.incomeTaxCessdropdown;
                }


               
            })
        }
        // clear form data
        $scope.cancel = function () {
            $scope.IncTax = {};
            $scope.submitted = false;
            $scope.incometaxDetails = [{ id: 'incometax' }];
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        }
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var IncTaxDetail = $scope.incometaxDetails;

                $scope.IncTax.incTaxDetail = IncTaxDetail;

                var data = $scope.IncTax;

                apiService.create("MasterIncomeTax/", data).
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

                        if (promise.incomeTaxList.length !== null && promise.incomeTaxList.length > 0) {

                            $scope.gridOptions.data = promise.incomeTaxList;
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

            var id = record.hrmiT_Id;
            //var ids=record.hrmitD_Id
            apiService.getURI("MasterIncomeTax/editRecord", id).
                then(function (promise) {

                    if (promise.incomeTaxList !== null && promise.incomeTaxList.length > 0) {
                        $scope.IncTax = promise.incomeTaxList[0];
                       // $scope.IncTaxDetail = promise.incomeTaxDetailsList;
                    }
                    if (promise.incomeTaxDetailsList !== null && promise.incomeTaxDetailsList.length > 0) {
                        $scope.IncTaxDetail = promise.incomeTaxDetailsList[0];
                        console.log($scope.IncTaxDetail);
                    }
                })
        }
        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            if (data.hrmiT_ActiveFlag == false) {
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
                   apiService.DeleteURI("MasterIncomeTax/ActiveDeactiveRecord", data.hrmiT_Id).
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

                           if (promise.incomeTaxList.length !== null && promise.incomeTaxList.length > 0) {

                               $scope.gridOptions.data = promise.incomeTaxList;
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