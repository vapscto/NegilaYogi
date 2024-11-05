(function () {
    'use strict';
    angular.module('app').controller('OnlineExamConfigController', OnlineExamConfigController)

    OnlineExamConfigController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function OnlineExamConfigController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {


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


        $scope.loaddata = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getURI("OnlineExamConfig/getloaddata", pageid).then(function (promise) {
                $scope.getQuestion = promise.getQdetails;
            });
        };


        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.lmsmoeS_Id;

            var data = {
                "LMSMOES_Id": $scope.editEmployee
            };

            apiService.create("OnlineExamConfig/editQuestion", data).then(function (promise) {
                $scope.LMSMOES_Id = promise.editQus[0].lmsmoeS_Id;
                $scope.Noofques = promise.editQus[0].lmsmoeS_NoofQns;
                $scope.totmrks = promise.editQus[0].lmsmoeS_TotalMarks;
                $scope.totdur = promise.editQus[0].lmsmoeS_TotalDuration;
                $scope.echmrkques = promise.editQus[0].lmsmoeS_EachQnsMarks;
                $scope.echquesdur = promise.editQus[0].lmsmoeS_EachQnsDuration;
                $scope.noopt = promise.editQus[0].lmsmoeS_NoOfOptions;

            });
        };




        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMSMOES_Id": $scope.LMSMOES_Id,
                    "Noofques": $scope.Noofques,
                    "totmrks": $scope.totmrks,
                    "totdur": $scope.totdur,
                    "echmrkques": $scope.echmrkques,
                    "echquesdur": $scope.echquesdur,
                    "noopt": $scope.noopt
                };

                apiService.create("OnlineExamConfig/savedata", data).then(function (promise) {
                    $scope.getQuestion = promise.getQuestion;
                    if (promise.returnval === true) {
                        if (promise.returnval === true) {
                            if (promise.message != null) {
                                swal('Record Updated Successfully', 'success');
                                $state.reload();
                            }
                            else {
                                swal('Record Saved Successfully', 'success');
                                $state.reload();
                            }
                        }
                    }
                    else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record Already Exist');
                    }
                    else {
                        if (promise.message != null) {
                            swal('Record Not Updated', 'success');
                        }
                        else {
                            swal('Record Not Saved', 'success');
                        }
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


        $scope.DeletRecord = function (lmsmoeS_Id) {
            var data = {
                "LMSMOES_Id": lmsmoeS_Id
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
                        apiService.create("OnlineExamConfig/Deletedetails/", data).then(function (promise) {
                            if (promise.returnval == true) {
                                swal('Record Deleted Successfully');
                            }
                            else if (promise.returnval == false) {
                                swal('Contact Administrator');
                            }
                            else if (promise.returnval == "RecordExists") {
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