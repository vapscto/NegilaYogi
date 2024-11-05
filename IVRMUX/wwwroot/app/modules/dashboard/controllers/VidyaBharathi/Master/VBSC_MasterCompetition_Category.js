(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterCompetition_CategoryController', MasterCompetition_CategoryController)
    MasterCompetition_CategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function MasterCompetition_CategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.obj = {};
        $scope.obj.VBSCMCC_CCAgeFlag = true;
        $scope.obj.VBSCMCC_CCWeightFlag = true;
        $scope.obj.VBSCMCC_CCClassFlg = true;
        $scope.searchValue = "";
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("VBSC_MasterCompetition_Category/loaddata", pageid).then(function (promise) {
                $scope.Master_trust = promise.master_trust;
                $scope.Year = promise.year;
                $scope.ToYear = promise.year;
                $scope.Month = promise.month;
                $scope.getreport = promise.getReport;
                $scope.obj.MT_Id = promise.mT_Id;

            })
        };
        $scope.get_fromage = function () {
            debugger;
            if (($scope.obj.VBSCMCC_CCAgeFromYear == 0) || ($scope.obj.VBSCMCC_CCAgeFromYear < 0)) {
                $scope.obj.VBSCMCC_CCAgeFromYear = '';
            }
        }
        $scope.get_frommonth = function () {
            if (($scope.obj.VBSCMCC_CCAgeFromMonth == 0) || ($scope.obj.VBSCMCC_CCAgeFromMonth < 0)) {
                $scope.obj.VBSCMCC_CCAgeFromMonth = '';
            }
        }
        $scope.get_fromday = function () {
            if (($scope.obj.VBSCMCC_CCAgeFromDays == 0) || ($scope.obj.VBSCMCC_CCAgeFromDays < 0)) {
                $scope.obj.VBSCMCC_CCAgeFromDays = '';
            }
        }
        $scope.get_toage = function () {
            if (($scope.obj.VBSCMCC_CCAgeToYear == 0) || ($scope.obj.VBSCMCC_CCAgeToYear < 0)) {
                $scope.obj.VBSCMCC_CCAgeToYear = '';
            }
        }
        $scope.get_tomonth = function () {
            if (($scope.obj.VBSCMCC_CCAgeToMonth == 0) || ($scope.obj.VBSCMCC_CCAgeToMonth < 0)) {
                $scope.obj.VBSCMCC_CCAgeToMonth = '';
            }
        }
        $scope.get_todays = function () {
            if (($scope.obj.VBSCMCC_CCAgeToDays == 0) || ($scope.obj.VBSCMCC_CCAgeToDays < 0)) {
                $scope.obj.VBSCMCC_CCAgeToDays = '';
            }
        }
        $scope.get_change = function () {
            debugger
            var fromage = Number($scope.obj.VBSCMCC_CCAgeFromYear);
            var todage = Number($scope.obj.VBSCMCC_CCAgeToYear);

            if ((fromage != undefined) || (todage != null)) {
                if ((todage < fromage) || (todage == fromage)) {
                    $scope.obj.VBSCMCC_CCAgeToYear = "";
                    swal('To age is always Greater .');
                }
            }
        }
        $scope.get_weight = function () {
            if (($scope.obj.VBSCMCC_CCFromWeight == 0) || ($scope.obj.VBSCMCC_CCFromWeight < 0)) {
                $scope.obj.VBSCMCC_CCFromWeight = '';
            }
        }
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VBSCMCC_Id = 0;

                var VBSCMCC_CCAgeFromYear = 0;
                var VBSCMCC_CCAgeFromMonth = 0;
                var VBSCMCC_CCAgeFromDays = 0;
                var VBSCMCC_CCAgeToYear = 0;
                var VBSCMCC_CCAgeToMonth = 0;
                var VBSCMCC_CCAgeToDays = 0;
                var VBSCMCC_CCFromWeight = 0;
                var VBSCMCC_CCToWeight = 0;
                if ($scope.obj.VBSCMCC_Id > 0) {
                    VBSCMCC_Id = $scope.obj.VBSCMCC_Id;
                }
                if ($scope.obj.VBSCMCC_CCAgeFlag == true) {
                    VBSCMCC_CCAgeFromYear = $scope.obj.VBSCMCC_CCAgeFromYear;
                    VBSCMCC_CCAgeFromMonth = $scope.obj.VBSCMCC_CCAgeFromMonth;
                    VBSCMCC_CCAgeFromDays = $scope.obj.VBSCMCC_CCAgeFromDays;
                    VBSCMCC_CCAgeToYear = $scope.obj.VBSCMCC_CCAgeToYear;
                    VBSCMCC_CCAgeToMonth = $scope.obj.VBSCMCC_CCAgeToMonth;
                    VBSCMCC_CCAgeToDays = $scope.obj.VBSCMCC_CCAgeToDays;
                }
                if ($scope.obj.VBSCMCC_CCWeightFlag == true) {
                    VBSCMCC_CCFromWeight = $scope.obj.VBSCMCC_CCFromWeight;
                    VBSCMCC_CCToWeight = $scope.obj.VBSCMCC_CCToWeight;
                }
                var data = {
                    "VBSCMCC_Id": VBSCMCC_Id,
                    "MT_Id": $scope.obj.MT_Id,
                    "VBSCMCC_CompetitionCategory": $scope.obj.VBSCMCC_CompetitionCategory,
                    "VBSCMCC_CCDesc": $scope.obj.VBSCMCC_CCDesc,
                    "VBSCMCC_CCAgeFlag": $scope.obj.VBSCMCC_CCAgeFlag,
                    "VBSCMCC_CCAgeFromYear": VBSCMCC_CCAgeFromYear,
                    "VBSCMCC_CCAgeFromMonth": VBSCMCC_CCAgeFromMonth,
                    "VBSCMCC_CCAgeFromDays": VBSCMCC_CCAgeFromDays,
                    "VBSCMCC_CCAgeToYear": VBSCMCC_CCAgeToYear,
                    "VBSCMCC_CCAgeToMonth": VBSCMCC_CCAgeToMonth,
                    "VBSCMCC_CCAgeToDays": VBSCMCC_CCAgeToDays,
                    "VBSCMCC_CCWeightFlag": $scope.obj.VBSCMCC_CCWeightFlag,
                    "VBSCMCC_CCFromWeight": VBSCMCC_CCFromWeight,
                    "VBSCMCC_CCToWeight": VBSCMCC_CCToWeight,
                    "VBSCMCC_CCClassFlg": $scope.obj.VBSCMCC_CCClassFlg,

                }
                apiService.create("VBSC_MasterCompetition_Category/savedata", data).
                    then(function (promise) {

                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');

                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');

                        }
                        else if (promise.returnval == "dublicate") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                        $state.reload();




                    })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {

            var data = {

                "VBSCMCC_Id": item.VBSCMCC_Id
            }
            var dystring = "";
            if (item.VBSCMCC_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.VBSCMCC_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("VBSC_MasterCompetition_Category/Deactivate", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (user) {
            $scope.obj.MT_Id = user.MT_Id;
            $scope.obj.VBSCMCC_Id = user.VBSCMCC_Id;
            $scope.obj.VBSCMCC_CompetitionCategory = user.VBSCMCC_CompetitionCategory;
            $scope.obj.VBSCMCC_CCDesc = user.VBSCMCC_CCDesc;
            $scope.obj.VBSCMCC_CCAgeFlag = user.VBSCMCC_CCAgeFlag;
            $scope.obj.VBSCMCC_CCClassFlg = user.VBSCMCC_CCClassFlg;
            $scope.obj.VBSCMCC_CCWeightFlag = user.VBSCMCC_CCWeightFlag;
            if ($scope.obj.VBSCMCC_CCAgeFlag == true) {
                $scope.obj.VBSCMCC_CCAgeFromYear = user.VBSCMCC_CCAgeFromYear;
                $scope.obj.VBSCMCC_CCAgeFromMonth = user.VBSCMCC_CCAgeFromMonth;
                $scope.obj.VBSCMCC_CCAgeFromDays = user.VBSCMCC_CCAgeFromDays;
                $scope.obj.VBSCMCC_CCAgeToYear = user.VBSCMCC_CCAgeToYear;
                $scope.obj.VBSCMCC_CCAgeToMonth = user.VBSCMCC_CCAgeToMonth;
                $scope.obj.VBSCMCC_CCAgeToDays = user.VBSCMCC_CCAgeToDays;

            }
            else {
                $scope.obj.VBSCMCC_CCAgeFromYear = "";
                $scope.obj.VBSCMCC_CCAgeFromMonth = "";
                $scope.obj.VBSCMCC_CCAgeFromDays = "";
                $scope.obj.VBSCMCC_CCAgeToYear = "";
                $scope.obj.VBSCMCC_CCAgeToMonth = "";
                $scope.obj.VBSCMCC_CCAgeToDays = "";
            }

            if ($scope.obj.VBSCMCC_CCWeightFlag == true) {
                $scope.obj.VBSCMCC_CCFromWeight = user.VBSCMCC_CCFromWeight;
                $scope.obj.VBSCMCC_CCToWeight = user.VBSCMCC_CCToWeight;

            }
            else {
                $scope.obj.VBSCMCC_CCFromWeight = "";
                $scope.obj.VBSCMCC_CCToWeight = "";
            }
        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();