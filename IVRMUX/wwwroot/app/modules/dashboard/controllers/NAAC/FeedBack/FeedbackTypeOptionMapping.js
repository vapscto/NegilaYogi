(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedbacktypeoptionmappingController', FeedbacktypeoptionmappingController)

    FeedbacktypeoptionmappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedbacktypeoptionmappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

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

            apiService.create("FeedbackTypeQuestionMapping/optiongetdetails", data).
                then(function (promise) {
                    $scope.getdetails = promise.getdetails;
                    $scope.grouptypeListOrder = promise.getdetails;
                    $scope.feedbacktype = promise.feedbacktype;
                    if ($scope.getdetails.length > 0) {
                        $scope.catreport = true;
                    }
                });
        };

        $scope.onchnagetype = function () {
            var data = {
                "FMTY_Id": $scope.FMTY_Id
            };
            apiService.create("FeedbackTypeQuestionMapping/optiononchnagetype", data).then(function (promise) {
                if (promise !== null) {
                    $scope.feedbackoptions = [];
                    $scope.feedbackquestion1 = [];
                    if (promise.mappeddetailscount === "mapped") {
                        swal("You Can Not Map The Options For This Type , Already Feedabck Is Submitted")
                    } else {
                        $scope.feedbackquestion1 = promise.feedbackoptions;
                        if ($scope.feedbackquestion1.length > 0) {
                            $scope.feedbackoptions = promise.feedbackoptions;
                        } else {
                            swal("No Record Found");
                        }
                    }
                    
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
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

                angular.forEach($scope.feedbackoptions, function (hi) {
                    if (hi.Selected) {
                        $scope.albumNameArraycolumn.push({ FMOP_Id: hi.fmoP_Id, FMOP_FeedbackOptions: hi.fmoP_FeedbackOptions });
                    }
                });

                var data = {
                    "FMTY_Id": $scope.FMTY_Id,
                    "FeedbackTypeOptionMappingTempDTO": $scope.albumNameArraycolumn
                };

                apiService.create("FeedbackTypeQuestionMapping/optionsavedata", data).
                    then(function (promise) {

                        if (promise !== null) {

                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
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
            apiService.create("FeedbackTypeQuestionMapping/editdata", data).then(function (promise) {

                $scope.editdetails = promise.editdetails;

                if ($scope.editdetails.length > 0) {
                    $scope.edit = true;
                    $scope.FMTY_Id = promise.editdetails[0].fmtY_Id;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.deactiveY = function (item, SweetAlert) {

            $scope.FMTO_Id = item.fmtO_Id;

            var dystring = "";
            if (item.fmtO_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (item.fmtO_ActiveFlag === false) {
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
                        apiService.create("FeedbackTypeQuestionMapping/optionactivedeactive", item).
                            then(function (promise) {
                                if (promise.mappeddetailscount === "mapped") {
                                    swal("You Can Not Deactivate The Record Already Feedback Submitted");
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
                "FeedbackTypeOptionMappingTemporderDTO": orderarray
            };

            apiService.create("FeedbackTypeQuestionMapping/optiongetorder", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Record Updated Successfully");
                    } else {
                        swal("Failed To Update Record");
                    }
                    $state.reload();
                });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.feedbackoptions.some(function (options) {
                return options.Selected;
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