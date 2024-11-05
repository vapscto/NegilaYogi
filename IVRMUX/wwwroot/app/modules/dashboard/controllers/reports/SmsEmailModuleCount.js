
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SmsEmailModuleCountController', SmsEmailModuleCountController)
    SmsEmailModuleCountController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function SmsEmailModuleCountController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {
        $scope.getlist = false;
        $scope.obj = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.modulearray = [];
        $scope.modulearraytwo = [];
        $scope.BindData = function () {
            var pageid = 2;         
            apiService.getURI("SmsEmailModuleCount/getalldetails", pageid).
                then(function (promise) {
                    $scope.acayyearbind = promise.acayear;
                    $scope.month_name = promise.fillmonth;
                    $scope.Modulelist = promise.modulelist;

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
        
        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.rptmonth = [];
                if ($scope.month_name != null && $scope.month_name.length > 0) {
                    angular.forEach($scope.month_name, function (qq) {
                        if (qq.selected == true) {
                            $scope.rptmonth.push({
                                ivrM_Month_Name: qq.ivrM_Month_Name,
                                ivrM_Month_Id: qq.ivrM_Month_Id
                            })
                        }
                    })
                }
               

                var data = {
                    "ASMAY_Id": $scope.academicyr,
                    "rptmonth": $scope.rptmonth,
                    "year": $scope.yearmodel,
                    "radioption": $scope.radioption
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SmsEmailModuleCount/Getreportdetails", data).
                    then(function (promise) {
                       
                        $scope.calculateemail = [];
                        
                        $scope.Smscount = promise.smscount;
                        $scope.Emailcount = promise.emailcount;
                       

                    })
            }
         

        };
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.toggleAlltwo = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.Modulelist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.modulearray.indexOf(itm) === -1) {
                        $scope.modulearray.push({ IVRMM_ModuleName: itm.IVRMM_ModuleName });
                    }
                }
                else {
                    $scope.modulearray.splice({ IVRMM_ModuleName: itm.IVRMM_ModuleName });

                }
            });
        }
        $scope.optionToggledtwo = function (SelectedStudentRecord, index) {
            $scope.all = $scope.classlist.every(function (itm) { return itm.selected; });
            if ($scope.modulearray.indexOf(SelectedStudentRecord) === -1) {
                $scope.modulearray.push(SelectedStudentRecord);
            }
            else {
                $scope.modulearray.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);

            }
        }
        //extra task
        $scope.toggleAll = function () {
            var toggleStatus = $scope.allone;
            angular.forEach($scope.month_name, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.modulearraytwo.indexOf(itm) === -1) {
                        $scope.modulearraytwo.push({ ivrM_Month_Id: itm.ivrM_Month_Id });
                    }
                }
                else {
                    $scope.modulearraytwo.splice({ ivrM_Month_Id: itm.ivrM_Month_Id });

                }
            });
        }
        //
        $scope.optionToggledtwo = function (SelectedStudentRecord, index) {
            $scope.all = $scope.classlisttwo.every(function (itm) { return itm.selected; });
            if ($scope.modulearraytwo.indexOf(SelectedStudentRecord) === -1) {
                $scope.modulearraytwo.push(SelectedStudentRecord);
            }
            else {
                $scope.modulearraytwo.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);

            }
        }
    }

})();