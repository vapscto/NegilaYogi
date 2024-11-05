
(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRSRechargeController', IVRSRechargeController)

    IVRSRechargeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function IVRSRechargeController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.BindData = function () {
            apiService.getDATA("IVRSRecharge/getalldetails").
                then(function (promise) {
                    $scope.institutiondropdown = promise.institute;
                    $scope.year_list = promise.yearlist;
                    $scope.month_name = promise.monthlist;
                    $scope.gridOptions.data = promise.maindata;
                    $scope.iacrE_Id = 0;
                });
        } 

        $scope.get_academics = function (iivrsC_Id) {
            angular.forEach($scope.institutiondropdown, function (role) {
                if (Number(iivrsC_Id) === role.iivrsC_Id) {
                    $scope.iivrsC_VirtualNo = role.iivrsC_VirtualNo;
                    $scope.iivrsC_SchoolName = role.iivrsC_SchoolName;
                    $scope.mI_Id = role.iivrsC_MI_Id
                }
            });
        }

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'iacrE_VirtualNo', displayName: 'VirtualNo' },
                { name: 'aA_SchoolName', displayName: 'SchoolName' },
                { name: 'iacrE_Year', displayName: 'Year' },
                { name: 'iacrE_Month', displayName: 'Month' },
                { name: 'iacrE_RechargeAmt', displayName: 'Recharge Amount' },
                { name: 'iacrE_PaymentMode', displayName: 'PaymentMode' },
                { name: 'iacrE_ReferneceNo', displayName: 'Reference No' },
                { name: 'iacrE_NoOfCalls', displayName: 'NoOfCalls' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.iacrE_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.iacrE_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ]
        };    
        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {   
                var data = {
                    "MI_Id": $scope.mI_Id,
                    "iacrE_Id": $scope.iacrE_Id,
                    "iacrE_VirtualNo": $scope.iivrsC_VirtualNo,
                    "iacrE_Year": $scope.asmaY_Year,
                    "iacrE_Month": $scope.ivrM_Month_Name,
                    "iacrE_RechargeAmt": Number($scope.iacrE_RechargeAmt),
                    "iacrE_PaymentMode": $scope.iacrE_PaymentMode,
                    "iacrE_ReferneceNo": $scope.iacrE_ReferneceNo,
                    "iacrE_NoOfCalls": $scope.iacrE_NoOfCalls                
                }
                apiService.create("IVRSRecharge/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Saved Successfully');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                        else if (promise.returnduplicatestatus === "Duplicate") {
                            swal('Record Already Exist');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                        else if (promise.returnval === false) {
                            swal('Record Not Saved');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                    })
            }
        };

        //TO clear  data
        $scope.cleardata = function () {
            $scope.iacrE_Id = 0;
            $scope.iacrE_VirtualNo = "";
            $scope.iivrsC_SchoolName = "";
            $scope.mI_Id = "";
            $scope.iivrsC_Id = "";
            $scope.iacrE_Year = "";
            $scope.ivrM_Month_Name = "";
            $scope.asmaY_Year = "";
            $scope.iacrE_Month = "";
            $scope.iacrE_RechargeAmt = "";
            $scope.iacrE_PaymentMode = "";
            $scope.iacrE_ReferneceNo = "";
            $scope.iacrE_NoOfCalls = ""; 
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.iacrE_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("IVRSRecharge/getdetails_page", pageid).
                then(function (promise) {
                    $scope.iacrE_Id = promise.maindata_grid[0].iacrE_Id;
                    $scope.mI_Id = promise.maindata_grid[0].mI_Id;
                    $scope.iivrsC_VirtualNo = promise.maindata_grid[0].iacrE_VirtualNo;
                    angular.forEach($scope.institutiondropdown, function (role) {
                        if (promise.maindata_grid[0].iacrE_VirtualNo === role.iivrsC_VirtualNo) {                        
                            $scope.iivrsC_Id = role.iivrsC_Id
                        }
                    });
                    $scope.iacrE_Year = promise.maindata_grid[0].iacrE_Year;
                    angular.forEach($scope.year_list, function (role) {
                        if (promise.maindata_grid[0].iacrE_Year === role.asmaY_Year) {
                            $scope.asmaY_Year = role.asmaY_Year
                        }
                    });
                   
                    $scope.iacrE_Month = promise.maindata_grid[0].iacrE_Month;
                    angular.forEach($scope.month_name, function (role) {
                        if (promise.maindata_grid[0].iacrE_Month === role.ivrM_Month_Name) {
                            $scope.ivrM_Month_Name = $scope.iacrE_Month
                        }
                    });
                    $scope.iacrE_RechargeAmt = promise.maindata_grid[0].iacrE_RechargeAmt;
                    $scope.iacrE_PaymentMode = promise.maindata_grid[0].iacrE_PaymentMode;
                    $scope.iacrE_ReferneceNo = promise.maindata_grid[0].iacrE_ReferneceNo;
                    $scope.iacrE_NoOfCalls = promise.maindata_grid[0].iacrE_NoOfCalls;
                    angular.forEach($scope.institutiondropdown, function (role) {
                        if ($scope.imlA_SchoolName === role.iivrsC_SchoolName) {
                            $scope.iivrsC_Id = role.iivrsC_Id;
                        }
                    });
               })
        }
        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.iacrE_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("IVRSRecharge/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.BindData();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }









    }

})();