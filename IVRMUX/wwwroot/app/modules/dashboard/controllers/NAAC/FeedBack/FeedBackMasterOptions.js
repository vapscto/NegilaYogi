(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedbackmasteroptionsController', FeedbackmasteroptionsController)

    FeedbackmasteroptionsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedbackmasteroptionsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.report = false;
        $scope.catreport = false;

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

            apiService.create("Feedbackmastertype/getoptiondetails", data).
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
                    "FMOP_Id": $scope.FMOP_Id,
                    "FMOP_FeedbackOptions": $scope.FMOP_FeedbackOptions,
                    "FMOP_OptionsValue": $scope.FMOP_OptionsValue,
                    "FMOP_FeedbackORemarks": $scope.FMOP_FeedbackORemarks
                };

                apiService.create("Feedbackmastertype/optionsavedata", data).
                    then(function (promise) {

                        if (promise != null) {
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


            apiService.create("Feedbackmastertype/optioneditdata", data).then(function (promise) {

                $scope.editdetails = promise.editdetails;

                if ($scope.editdetails.length > 0) {
                    $scope.edit = true;
                    $scope.FMOP_FeedbackOptions = promise.editdetails[0].fmoP_FeedbackOptions;
                    $scope.FMOP_OptionsValue = promise.editdetails[0].fmoP_OptionsValue;                    
                    $scope.FMOP_Id = promise.editdetails[0].fmoP_Id;
                    $scope.FMOP_FeedbackORemarks = promise.editdetails[0].fmoP_FeedbackORemarks;
                   

                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };


        $scope.deactiveY = function (item, SweetAlert) {

            $scope.FMOP_Id = item.fmoP_Id;

            var dystring = "";
            if (item.fmoP_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (item.fmoP_ActiveFlag === false) {
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
                        apiService.create("Feedbackmastertype/optionactivedeactive", item).
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
                "Option_Master_TempDTO": orderarray
            };

            apiService.create("Feedbackmastertype/optiongetOrder", data).
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
                    $scope.grouptypeListOrder[index].fmoP_FOOrder = Number(index) + 1;

                }
            }
        };
    }

})();