


(function () {
    'use strict';
    angular
        .module('app')
        .controller('PointsController', PointsController)

    PointsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function PointsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.itemsPerPages = paginationformasters;
        $scope.currentPages = 1;

        $scope.user = {};
        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("Points/getdetails", pageid).
                then(function (promise) {
                    $scope.yearlst = promise.yeardropDown;
                    $scope.classlist = promise.fillclass;
                    $scope.IsHiddendown = false;
                })
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.datapages, function (itm) {
                itm.isSelected = toggleStatus;
            });
        };

        $scope.selected = function (SelectedStudentRecord) {
            $scope.all = $scope.datapages.every(function (itm) { return itm.selected; });
        };

        $scope.submitted = false;
        $scope.BindGrid = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var data = {
                "ASMAY_Id": $scope.ASMAY,
                "ASMCL_Id": $scope.ASMCL,
                "configurationsettings": configsettings[0].ispaC_ApplFeeFlag,
            }
            apiService.create("Points/Getreportdetails", data).
                then(function (promise) {

                    $scope.datapages = promise.studentDetails;
                    if ($scope.datapages.length > 0 || $scope.datapages == null) {
                        $scope.IsHiddendown = true;
                    } else {
                        swal("No Records Found");
                        $scope.IsHiddendown = false;

                    }
                });

        };


        $scope.cancelAll = function () {
            $scope.loaddata();
        }
        $scope.cancel = function () {
            $scope.loaddata();
        }

        $scope.submitted1 = false;
        $scope.SaveGridData = function (datapages) {

            $scope.albumNameArray = [];
            angular.forEach($scope.datapages, function (role) {
                if (!!role.isSelected) $scope.albumNameArray.push(role);
            })
            if ($scope.albumNameArray.length > 0) {

                var data = {
                    "TempararyArrayList": $scope.albumNameArray,
                }
                apiService.create("Points/savedata", data).
                    then(function (promise) {
                        // $scope.datapages = promise.studentDetails;
                        if (promise.returnval == true) {
                            swal("Records Updated Successfully !!!");
                            $scope.IsHiddendown = false;
                            $scope.loaddata();

                        } else {
                            swal("Records Not Updated");
                            $scope.IsHiddendown = true;

                        }
                    });

            }
            else {
                swal("Atleast Select One Student !!");
            }
        };




        $scope.calculatetotal = function () {
            $scope.albumNameArray = [];
            $scope.albumNameArray = $scope.datapages;
            // $scope.user.pasrapS_ID = "787926";
            $scope.albumNameArray1 = [];
            for (var i = 0; i < $scope.albumNameArray.length; i++) {

                $scope.albumNameArray1.push({
                    pasR_FirstName: $scope.albumNameArray[i].pasR_FirstName,
                    pasR_MiddleName: $scope.albumNameArray[i].pasR_MiddleName,
                    pasR_LastName: $scope.albumNameArray[i].pasR_LastName,
                    pasR_Age: $scope.albumNameArray[i].pasR_Age,
                    pasR_FatherEducation: $scope.albumNameArray[i].pasR_FatherEducation,
                    pasR_MotherEducation: $scope.albumNameArray[i].pasR_MotherEducation,
                    pasR_TotalIncome: $scope.albumNameArray[i].pasR_TotalIncome,
                    pasR_ConArea: $scope.albumNameArray[i].pasR_ConArea,
                    caste_Name: $scope.albumNameArray[i].caste_Name,
                    pasaP_AGE: $scope.albumNameArray[i].pasaP_AGE,
                    pasaP_INCOME: $scope.albumNameArray[i].pasaP_INCOME,
                    pasaP_CASTE: $scope.albumNameArray[i].pasaP_CASTE,
                    pasaP_ADRESS: $scope.albumNameArray[i].pasaP_ADRESS,
                    pasaP_QA: $scope.albumNameArray[i].pasaP_QA,
                    pasaP_TOTAL: Number($scope.albumNameArray[i].pasaP_AGE) + Number($scope.albumNameArray[i].pasaP_INCOME) + Number($scope.albumNameArray[i].pasaP_CASTE) + Number($scope.albumNameArray[i].pasaP_ADRESS) + Number($scope.albumNameArray[i].pasaP_QA),
                    pamsT_Id: $scope.albumNameArray[i].pamsT_Id,
                    pasR_Id: $scope.albumNameArray[i].pasR_Id,
                    pasrapS_ID: $scope.albumNameArray[i].pasrapS_ID
                });

            }
            $scope.datapages = $scope.albumNameArray1;



        };


    }

})();