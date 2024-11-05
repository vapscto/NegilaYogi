(function () {
    'use strict';
    angular.module('app').controller('PAOnlineExamConfigController', PAOnlineExamConfigController)

    PAOnlineExamConfigController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function PAOnlineExamConfigController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

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
            apiService.getURI("PAOnlineExamConfig/getloaddata", pageid).then(function (promise) {
                $scope.getQuestion = promise.getQdetails;
            });
        };


        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pamoeS_Id;

            var data = {
                "PAMOES_Id": $scope.editEmployee
            };

            apiService.create("PAOnlineExamConfig/editQuestion", data).then(function (promise) {
                $scope.PAMOES_Id = promise.editQus[0].pamoeS_Id;
                $scope.Noofques = promise.editQus[0].pamoeS_NoofQns;
                $scope.totmrks = promise.editQus[0].pamoeS_TotalMarks;
                $scope.totdur = promise.editQus[0].pamoeS_TotalDuration;
                $scope.echmrkques = promise.editQus[0].pamoeS_EachQnsMarks;
                $scope.echquesdur = promise.editQus[0].pamoeS_EachQnsDuration;
                $scope.noopt = promise.editQus[0].pamoeS_NoOfOptions;

            });
        };




        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "PAMOES_Id": $scope.PAMOES_Id,
                    "Noofques": $scope.Noofques,
                    "totmrks": $scope.totmrks,
                    "totdur": $scope.totdur,
                    "echmrkques": $scope.echmrkques,
                    "echquesdur": $scope.echquesdur,
                    "noopt": $scope.noopt
                };

                apiService.create("PAOnlineExamConfig/savedata", data).then(function (promise) {
                    $scope.getQuestion = promise.getQuestion;
                    if (promise.returnval === true) {
                        swal('Record Saved/Updated Successfully');
                    }

                    else {
                        swal('Record Failed To Save / Update');
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


        $scope.DeletRecord = function (pamoeS_Id) {
            var data = {
                "PAMOES_Id": pamoeS_Id
            };

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("PAOnlineExamConfig/Deletedetails/", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Record Deleted Successfully');
                            }
                            else if (promise.returnval === false) {
                                swal('Failed To Delete Record Contact Administrator');
                            }
                            else if (promise.returnval === "RecordExists") {
                                swal('Data has already been used So record cannot be deleted');
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                    $state.reload();
                });
        };
    }
})();