(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedbackmasterquestionsController', FeedbackmasterquestionsController)

    FeedbackmasterquestionsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedbackmasterquestionsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.report = false;
        $scope.catreport = false;
        $scope.FMQE_ManualEntryFlg = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.BindData = function () {

            var data = {
                "MI_Id": 4
            };

            apiService.create("Feedbackmastertype/getquestiondetails", data).
                then(function (promise) {
                    $scope.getdetails = promise.getdetails;
                    $scope.grouptypeListOrder = promise.getdetails;
                    if ($scope.getdetails.length > 0) {
                        $scope.catreport = true;
                    }
                });
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchValue = '';

        $scope.students = [];
        $scope.catreport = false;
        $scope.submitted = false;

        $scope.onreport = function (obj) {
            $scope.albumNameArraycolumn = [];
            $scope.all = false;
            if ($scope.myForm.$valid) {

                var data = {
                    "FMQE_Id": $scope.FMQE_Id,
                    "FMQE_FeedbackQuestions": $scope.FMQE_FeedbackQuestions,
                    "FMQE_FeedbackQRemarks": $scope.FMQE_FeedbackQRemarks,
                    "FMQE_ManualEntryFlg": $scope.FMQE_ManualEntryFlg
                };

                apiService.create("Feedbackmastertype/questionssavedata", data).
                    then(function (promise) {

                        if (promise !== null) {
                            if (promise.message === "Add") {
                                if (promise.returnval === true) {
                                    swal("Record Saved Successfully");
                                } else {
                                    swal("Failed To Save Record");
                                }
                            } else if (promise.message === "Update") {
                                if (promise.returnval === true) {
                                    swal("Record Updated Successfully");
                                } else {
                                    swal("Failed To Update Record");
                                }
                            } else if (promise.message === "Duplicate") {
                                swal("Record Already Exists");
                            } else {
                                swal("Something Went Wrong Kindly Contact Administrator");
                            }


                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                        }
                        $state.reload();

                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.editdata = function (user) {

            var data = user;


            apiService.create("Feedbackmastertype/questionseditdata", data).then(function (promise) {

                $scope.editdetails = promise.editdetails;

                if ($scope.editdetails.length > 0) {
                    $scope.edit = true;
                    $scope.FMQE_FeedbackQuestions = promise.editdetails[0].fmqE_FeedbackQuestions;
                    $scope.FMQE_FeedbackQRemarks = promise.editdetails[0].fmqE_FeedbackQRemarks;
                    $scope.FMQE_Id = promise.editdetails[0].fmqE_Id;
                    $scope.FMQE_ManualEntryFlg = promise.editdetails[0].fmqE_ManualEntryFlg;


                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };


        $scope.deactiveY = function (item, SweetAlert) {

            $scope.FMQE_Id = item.fmqE_Id;

            var dystring = "";
            if (item.fmqE_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (item.fmqE_ActiveFlag === false) {
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
                        apiService.create("Feedbackmastertype/questionsactivedeactive", item).
                            then(function (promise) {
                                if (promise.message === "Mapped") {
                                    swal("You Can Not Deactivate The Record Its Already Mapped");
                                } else {
                                    if (promise.returnval === true) {
                                        swal("Record " + dystring + "Successfully");
                                    }
                                    else {
                                        swal("Record Not " + dystring + "Successfully");
                                    }
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Cancel");
                    }
                });
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                "Questions_Master_TempDTO": orderarray
            };

            apiService.create("Feedbackmastertype/questionsgetOrder", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Record Updated Successfully");
                    } else {
                        swal("Failed To Update Record");
                    }
                    $state.reload();
                });
        };


        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].fmqE_FQOrder = Number(index) + 1;

                }
            }
        };
    }

})();