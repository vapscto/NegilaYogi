(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedbackSchoolGenrealTransactionController', FeedbackSchoolGenrealTransactionController)

    FeedbackSchoolGenrealTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedbackSchoolGenrealTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

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

        $scope.typelist = [];
        $scope.mappedquestiondeetails = [];
        $scope.mappedoptiondeetails = [];

        $scope.temparray = [];
        $scope.tempquestionarray = [];
        $scope.tempoptionarray = [];

        $scope.BindData = function () {

            var data = {
                "MI_Id": 4,
                "Flag": 'Parents'
            };

            apiService.create("FeedbackSchoolGeneralTransaction/getdetails", data).then(function (promise) {
                $scope.count = promise.count;

                if (promise.flag !== "Alumni" && promise.flag !== "Staff") {
                    if (promise.getstudentdetails !== null && promise.getstudentdetails.length > 0) {
                        $scope.typelistload = promise.typelistload;
                    } else {
                        swal("Student Details Not Found");
                    }
                } else {
                    $scope.typelistload = promise.typelistload;
                }
            });
        };

        $scope.getfeedback = function () {

            var data = {
                "FMTY_Id": $scope.FMTY_Id
            };

            apiService.create("FeedbackSchoolGeneralTransaction/getfeedback", data).then(function (promise) {
                if (promise !== null) {
                    $scope.count = promise.count;

                    if ($scope.count > 0) {
                        $scope.temparray = [];
                        $scope.tempquestionarray = [];
                        $scope.tempoptionarray = [];
                        swal("Feedback Is Already Given");
                        return;
                    }

                    if ((promise.typelist !== null && promise.typelist.length > 0)
                        && (promise.mappedquestiondeetails !== null && promise.mappedquestiondeetails.length > 0)
                        && (promise.mappedoptiondeetails !== null && promise.mappedoptiondeetails.length > 0)) {

                        $scope.temparray = [];
                        $scope.tempquestionarray = [];
                        $scope.tempoptionarray = [];

                        $scope.typelist = promise.typelist;
                        $scope.mappedquestiondeetails = promise.mappedquestiondeetails;
                        $scope.mappedoptiondeetails = promise.mappedoptiondeetails;


                        if ($scope.typelist[0].fmtY_QuestionwiseOptionFlg === false) {
                            angular.forEach($scope.typelist, function (dd) {

                                $scope.tempquestionarray = [];
                                $scope.tempoptionarray = [];
                                angular.forEach($scope.mappedquestiondeetails, function (ddd) {

                                    if (dd.fmtY_Id === ddd.fmtY_Id && !ddd.fmqE_ManualEntryFlg) {
                                        $scope.tempoptionarray = [];
                                        angular.forEach($scope.mappedoptiondeetails, function (dddd) {
                                            if (dd.fmtY_Id === dddd.fmtY_Id) {
                                                $scope.tempoptionarray.push({ FMOP_Id: dddd.fmoP_Id, FMOP_FeedbackOptions: dddd.fmoP_FeedbackOptions });
                                            }
                                        });

                                        $scope.tempquestionarray.push({
                                            FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                            opt: $scope.tempoptionarray, manualflg: ddd.fmqE_ManualEntryFlg
                                        });
                                    } else {
                                        $scope.tempquestionarray.push({
                                            FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                            manualflg: ddd.fmqE_ManualEntryFlg
                                        });
                                    }
                                });
                                $scope.temparray.push({ FMTY_Id: dd.fmtY_Id, FMTY_FeedbackTypeName: dd.fmtY_FeedbackTypeName, ques: $scope.tempquestionarray });

                            });
                        } else {
                            angular.forEach($scope.typelist, function (dd) {

                                $scope.tempquestionarray = [];
                                $scope.tempoptionarray = [];
                                angular.forEach($scope.mappedquestiondeetails, function (ddd) {
                                    if (dd.fmtY_Id === ddd.fmtY_Id && !ddd.fmqE_ManualEntryFlg) {
                                        $scope.tempoptionarray = [];
                                        angular.forEach($scope.mappedoptiondeetails, function (dddd) {
                                            if (dd.fmtY_Id === dddd.fmtY_Id && ddd.fmqE_Id === dddd.fmqE_Id) {
                                                $scope.tempoptionarray.push({ FMOP_Id: dddd.fmoP_Id, FMOP_FeedbackOptions: dddd.fmoP_FeedbackOptions });
                                            }
                                        });

                                        $scope.tempquestionarray.push({
                                            FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                            opt: $scope.tempoptionarray, manualflg: ddd.fmqE_ManualEntryFlg
                                        });
                                    } else {
                                        $scope.tempquestionarray.push({
                                            FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                            manualflg: ddd.fmqE_ManualEntryFlg
                                        });
                                    }
                                });
                                $scope.temparray.push({ FMTY_Id: dd.fmtY_Id, FMTY_FeedbackTypeName: dd.fmtY_FeedbackTypeName, ques: $scope.tempquestionarray });
                            });
                        }

                        console.log($scope.temparray);

                    } else {
                        $scope.temparray = [];
                        $scope.tempquestionarray = [];
                        $scope.tempoptionarray = [];
                        swal("No Records Found");
                    }
                } else {
                    $scope.temparray = [];
                    $scope.tempquestionarray = [];
                    $scope.tempoptionarray = [];
                    swal("Something Went Wrong Kidndly Contact Administrator");
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

        // $scope.user = {};

        $scope.onreport = function (user) {
            if ($scope.myForm.$valid) {

                var data = {
                    temp: $scope.temparray,
                    "Flag": 'Parents'
                };
                if ($scope.count > 0) {

                    swal({
                        title: "For Today's Date Already Feedback Is Submitted?",
                        text: "Do You Want To Submit Again?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Submit It Again",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },

                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("FeedbackSchoolGeneralTransaction/savedata", data).
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
                                swal("Cancel");
                                $state.reload();
                            }
                        });

                } else {

                    apiService.create("FeedbackSchoolGeneralTransaction/savedata", data).
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
            } else {
                $scope.submitted = true;
            }
            //$scope.albumNameArraycolumn = [];
            //$scope.all = false;
            //if ($scope.myForm.$valid) {

            //    var data = {
            //        temp: $scope.temparray,
            //        "Flag": 'Parents'
            //    };

            //    apiService.create("FeedbackTransaction/savedata", data).
            //        then(function (promise) {

            //            if (promise != null) {
            //                if (promise.returnval === true) {
            //                    swal("Record Saved Successfully");
            //                } else {
            //                    swal("Failed To Save Record");
            //                }
            //            } else {
            //                swal("Something Went Wrong Kindly Contact Administrator");
            //            }
            //            $state.reload();
            //        });
            //}
            //else {
            //    $scope.submitted = true;
            //}
        };

        $scope.editdata = function (user) {

            var data = user;


            apiService.create("FeedbackTransactionController/optioneditdata", data).then(function (promise) {

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

            apiService.create("FeedbackTransactionController/optiongetOrder", data).
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
    }

})();