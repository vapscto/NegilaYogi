
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ManagementDashboardMonthlyController', ManagementDashboardMonthlyController)

    ManagementDashboardMonthlyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ManagementDashboardMonthlyController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {
        $scope.getlist = false;
        $scope.obj = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.BindData = function () {
            apiService.getDATA("ManagementDashboardMonthly/Getdetails").
                then(function (promise) {
                    $scope.acayyearbind = promise.acayear;
                    $scope.month_name = promise.fillmonth;

                })
        };
        var temp = [];
        var year = "";

        $scope.get_years = function () {
            temp = [];
            angular.forEach($scope.acayyearbind, function (itm) {
                if (itm.asmaY_Id == $scope.academicyr) {
                    year = itm.asmaY_Year
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            $scope.years = temp;
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.emcA_Id = '';
            $scope.emE_Id = '';
            $scope.asmcL_Id = '';
            $scope.fillcategory = [];
            $scope.classlist = [];
            $scope.exmstdlist = [];
            apiService.getURI("ManagementDashboardMonthly/getcategory", asmaY_Id).
                then(function (promise) {
                    if (promise.fillcategory.length > 0) {
                        $scope.fillcategory = promise.fillcategory;
                    }
                });

        }


        // TO Save The Data
        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.submitted = true;
            // if ($scope.myForm.$valid) {

            var data = {
                "ASMAY_Id": $scope.academicyr,
                "Month": $scope.monthmodel,
                "year": $scope.yearmodel
                }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ManagementDashboardMonthly/getreport", data).
                then(function (promise) {
                   $scope.feecolllection = [];
                    if (promise.mI_Address != null && promise.mI_Address.length > 0) {
                        $scope.getlist = true;
                        $scope.MI_Name = promise.mI_Address[0].mI_Name;
                        $scope.AddresOne = promise.mI_Address[0].mI_Address1;
                        $scope.Addresstwo = promise.mI_Address[0].mI_Address2;
                        $scope.Addressthree = promise.mI_Address[0].mI_Address3;
                        $scope.Smscount = promise.smscount.length;
                        $scope.Emailcount = promise.emailcount.length;
                        $scope.Totalcount = (Number($scope.Smscount) + ($scope.Emailcount));
                        $scope.Birthdaylist = promise.birthdaylist.length;
                        $scope.totalstudent = promise.totalstudent.length;
                        $scope.BoniFiedCertificate = promise.boniFiedCertificate.length;
                        $scope.Tctaken = promise.tctaken.length;
                        $scope.empstrenth = promise.empstrenth.length;
                        $scope.empleft = promise.empleft.length;
                        $scope.empsalary = promise.empsalary.length;
                        $scope.preadmision = promise.preadmision.length;
                       // $scope.Defaluter = promise.defaluter[0].defaulters;
                        $scope.feecolllection = promise.feecolllection;
                        var ds = 0;
                        var a = 0;
                        if ($scope.feecolllection != null && $scope.feecolllection.length > 0) {                                                  
                            for (var i = 0; i < $scope.feecolllection.length; i++) {
                                ds += parseFloat($scope.feecolllection[i].fyP_Tot_Amount)
                                a = ds.toFixed(2);
                            }
                          
                            $scope.feecollections = a;
                        }
                        else {
                            $scope.feecollections = a;
                        }
                        
                    }
                })
           
        };
        $scope.cancel = function () {
            $state.reload();
        }
    }

})();