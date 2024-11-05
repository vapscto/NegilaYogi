(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineExamConfigController', LP_OnlineExamConfigController)

    LP_OnlineExamConfigController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function LP_OnlineExamConfigController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.getQuestion = [];

        $scope.loaddata = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getURI("LP_OnlineExam/getconfigloaddata", pageid).then(function (promise) {
                $scope.getconfigloaddataarray = promise.getconfigloaddataarray;
                if ($scope.getconfigloaddataarray !== null && $scope.getconfigloaddataarray.length > 0) {
                    $scope.LPMOES_Id = $scope.getconfigloaddataarray[0].lpmoelpmoeS_Id;
                    $scope.LPMOES_NoofQns = $scope.getconfigloaddataarray[0].lpmoeS_NoofQns;
                    $scope.LPMOES_TotalMarks = $scope.getconfigloaddataarray[0].lpmoeS_TotalMarks;
                    $scope.LPMOES_TotalDuration = $scope.getconfigloaddataarray[0].lpmoeS_TotalDuration;
                    $scope.LPMOES_EachQnsMarks = $scope.getconfigloaddataarray[0].lpmoeS_EachQnsMarks;
                    $scope.LPMOES_EachQnsDuration = $scope.getconfigloaddataarray[0].lpmoeS_EachQnsDuration;
                    $scope.LPMOES_NoOfOptions = $scope.getconfigloaddataarray[0].lpmoeS_NoOfOptions;                 
                    
                } else {
                    $scope.LPMOES_Id = 0;
                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "LPMOES_Id": $scope.LPMOES_Id,
                    "LPMOES_NoofQns": $scope.LPMOES_NoofQns,
                    "LPMOES_TotalMarks": $scope.LPMOES_TotalMarks,
                    "LPMOES_TotalDuration": $scope.LPMOES_TotalDuration,
                    "LPMOES_EachQnsMarks": $scope.LPMOES_EachQnsMarks,
                    "LPMOES_EachQnsDuration": $scope.LPMOES_EachQnsDuration,
                    "LPMOES_NoOfOptions": $scope.LPMOES_NoOfOptions
                };

                apiService.create("LP_OnlineExam/saveconfigdata", data).then(function (promise) {
                    if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal('Record Updated Successfully');
                        }
                        else {
                            swal('Record Failed To Update');
                        }
                    } else if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal('Record Saved Successfully');
                        }
                        else {
                            swal('Record Failed To Save');
                        }
                    } else {
                        swal('Something Went Wrong Contact Administrator');
                    }

                    $state.reload();
                });
            } else {
                $scope.submitted = true;
            }
        };    

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();