(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedbackmastertypeController', FeedbackmastertypeController)

    FeedbackmastertypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedbackmastertypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

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
            apiService.getDATA("Feedbackmastertype/getdetails").then(function (promise) {
                $scope.getdetails = promise.getdetails;
                $scope.grouptypeListOrder = promise.getdetails;
                $scope.nooftimesfeedback = 1;
                angular.forEach($scope.getdetails, function (dd) {
                    if (dd.fmtY_StakeHolderFlag === 'Student') {
                        dd.nooftimesbyuser = dd.fmtY_NOFPerYearByStudent;
                    } else if (dd.fmtY_StakeHolderFlag === 'Parents') {
                        dd.nooftimesbyuser = dd.fmtY_NOFPerYearByParent;
                    } else if (dd.fmtY_StakeHolderFlag === 'Alumni') {
                        dd.nooftimesbyuser = dd.fmtY_NOFPerYearByAlumni;
                    } else if (dd.fmtY_StakeHolderFlag === 'Staff') {
                        dd.nooftimesbyuser = dd.fmtY_NOFPerYearByStaff;
                    }
                });

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

                if ($scope.SubjectSpecificFlag === "1") {
                    $scope.FMTY_SubjectSpecificFlag = true;
                } else {
                    $scope.FMTY_SubjectSpecificFlag = false;
                }
                var questionflag = false;
                if ($scope.FMTY_QuestionwiseOptionFlg === true) {
                    questionflag = true;
                }
                else {
                    questionflag = false;
                }


                var data = {
                    "FMTY_Id": $scope.FMTY_Id,
                    "FMTY_StakeHolderFlag": $scope.FMTY_StakeHolderFlag,
                    "FMTY_FeedbackTypeName": $scope.FMTY_FeedbackTypeName,
                    "FMTY_FeedbackTypeRemarks": $scope.FMTY_FeedbackTypeRemarks,
                    "FMTY_SubjectSpecificFlag": $scope.FMTY_SubjectSpecificFlag,
                    "FMTY_QuestionwiseOptionFlg": questionflag,
                    "nooftimesfeedback": $scope.nooftimesfeedback
                };

                apiService.create("Feedbackmastertype/savedata", data).then(function (promise) {

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
            apiService.create("Feedbackmastertype/editdata", data).then(function (promise) {
                $scope.editdetails = promise.editdetails;
                if ($scope.editdetails.length > 0) {
                    $scope.edit = true;
                    $scope.FMTY_StakeHolderFlag = promise.editdetails[0].fmtY_StakeHolderFlag;
                    $scope.FMTY_FeedbackTypeName = promise.editdetails[0].fmtY_FeedbackTypeName;
                    $scope.FMTY_FeedbackTypeRemarks = promise.editdetails[0].fmtY_FeedbackTypeRemarks;
                    $scope.FMTY_SubjectSpecificFlag = promise.editdetails[0].fmtY_SubjectSpecificFlag;
                    $scope.FMTY_QuestionwiseOptionFlg = promise.editdetails[0].fmtY_QuestionwiseOptionFlg;
                    $scope.FMTY_Id = promise.editdetails[0].fmtY_Id;

                    if ($scope.FMTY_StakeHolderFlag === 'Student') {
                        $scope.nooftimesfeedback = promise.editdetails[0].fmtY_NOFPerYearByStudent;
                    } else if ($scope.FMTY_StakeHolderFlag === 'Parents') {
                        $scope.nooftimesfeedback = promise.editdetails[0].fmtY_NOFPerYearByParent;
                    } else if ($scope.FMTY_StakeHolderFlag === 'Alumni') {
                        $scope.nooftimesfeedback = promise.editdetails[0].fmtY_NOFPerYearByAlumni;
                    } else if ($scope.FMTY_StakeHolderFlag === 'Staff') {
                        $scope.nooftimesfeedback = promise.editdetails[0].fmtY_NOFPerYearByStaff;
                    }

                    if ($scope.FMTY_SubjectSpecificFlag === true) {
                        $scope.SubjectSpecificFlag = "1";
                    } else {
                        $scope.SubjectSpecificFlag = "0";
                    }

                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };


        $scope.deactiveY = function (item, SweetAlert) {

            $scope.FMTY_Id = item.fmtY_Id;

            var dystring = "";
            var dystring1 = "";
            if (item.fmtY_ActiveFlag === true) {
                dystring = "Deactivate";
                dystring1 = "Deactivated";
            }
            else if (item.fmtY_ActiveFlag === false) {
                dystring = "Activate";
                dystring1 = "Activated";
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
                        apiService.create("Feedbackmastertype/activedeactive", item).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("You Can Not Deactivate The Record Its Already Mapped");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring1 + " Successfully");
                                }
                                else {
                                    swal("Record Not " + dystring1 + " Successfully");
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
                "Type_Master_TempDTO": orderarray
            };

            apiService.create("Feedbackmastertype/getOrder", data).
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
                    $scope.grouptypeListOrder[index].fmtY_FTOrder = Number(index) + 1;

                }
            }
        };
    }

})();