(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedbacktypequestionmappingController', FeedbacktypequestionmappingController)

    FeedbacktypequestionmappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedbacktypequestionmappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.report = false;
        $scope.catreport = false;
        $scope.typeadd = false;
        $scope.searchValue = '';
        $scope.students = [];
        $scope.catreport = false;
        $scope.submitted = false;
        $scope.temparraydetails = [];

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
            apiService.create("FeedbackTypeQuestionMapping/getdetails", data).then(function (promise) {
                $scope.getdetails = promise.getdetails;
                $scope.grouptypeListOrder = promise.getdetails;
                $scope.feedbacktype = promise.feedbacktype;
                if ($scope.getdetails.length > 0) {
                    $scope.catreport = true;
                }
            });
        };

        $scope.onchnagetype = function () {
            $scope.feedbackquestion1 = [];
            $scope.feedbackquestion = [];
            $scope.feedbackquestiond = [];

            $scope.getoptions = [];
            angular.forEach($scope.feedbacktype, function (dd) {
                if (dd.fmtY_Id === parseInt($scope.FMTY_Id)) {
                    $scope.questionflag = dd.fmtY_QuestionwiseOptionFlg;
                }
            });
            var data = {
                "FMTY_Id": $scope.FMTY_Id
            };

            apiService.create("FeedbackTypeQuestionMapping/onchnagetype", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.mappeddetailscount === "mapped") {
                        $scope.feedbackquestion1 = [];
                        $scope.feedbackquestion = [];
                        $scope.feedbackquestiond = [];
                        swal("You Can Not Map The Question For This Type , Already Feedback Submitted");
                    } else {
                        $scope.feedbackquestion1 = promise.feedbackquestions;
                        if ($scope.feedbackquestion1.length > 0) {
                            $scope.feedbackquestion = promise.feedbackquestions;
                            $scope.feedbackquestiond = promise.feedbackquestions;
                        } else {
                            swal("No Record Found");
                        }
                    }

                    
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        // ************** When Feedback Type Does Not Have Question Wise Options ********* //      

        $scope.save = function (obj) {
            $scope.albumNameArraycolumn = [];
            $scope.all = false;
            if ($scope.myForm.$valid) {
                var data = "";

                angular.forEach($scope.feedbackquestion, function (hi) {
                    if (hi.Selected) {
                        $scope.albumNameArraycolumn.push({ FMQE_Id: hi.fmqE_Id, FMQE_FeedbackQuestions: hi.fmqE_FeedbackQuestions });
                    }
                });

                data = {
                    "FMTY_Id": $scope.FMTY_Id,
                    "FeedbackTypeQuestionMappingTempDTO": $scope.albumNameArraycolumn
                };

                apiService.create("FeedbackTypeQuestionMapping/savedata", data).then(function (promise) {

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

        $scope.deactiveY = function (item, SweetAlert) {
            $scope.FMTQ_Id = item.fmtQ_Id;
            var dystring = "";
            if (item.fmtQ_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (item.fmtQ_ActiveFlag === false) {
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
                        apiService.create("FeedbackTypeQuestionMapping/activedeactive", item).
                            then(function (promise) {
                                if (promise.mappeddetailscount === "mapped") {
                                    swal("You Can Not Deactivate The Record Feeback Already Submitted");
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

        $scope.isOptionsRequired = function () {
            return !$scope.feedbackquestion.some(function (options) {
                return options.Selected;
            });
        };


        // ************** When Feedback Type Have Question Wise Options ********* //

        $scope.onchangequestion = function (obj) {
            $scope.getoptions = [];

            angular.forEach($scope.feedbackquestiond, function (dd) {
                if (dd.fmqE_Id === parseInt(obj.FMQE_Id_New)) {
                    $scope.FMQE_ManualEntryFlg = dd.fmqE_ManualEntryFlg;
                }
            });

            var data = {
                "FMTY_Id": $scope.FMTY_Id,
                "FMQE_Id": obj.FMQE_Id_New
            };

            apiService.create("FeedbackTypeQuestionMapping/onchangequestion", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getoptions = promise.getoptions;
                    if ($scope.getoptions !== null && $scope.getoptions.length > 0) {
                        $scope.feedbackquestionoption = $scope.getoptions;
                    } else {
                        swal("No Feedback Options Found");
                    }
                }
            });
        };

        $scope.getquestionwiseoption = function (item) {
            $scope.getquestionsoptions = [];
            var data = item;
            apiService.create("FeedbackTypeQuestionMapping/getquestionwiseoption", data).then(function (promise) {
                if (promise !== null) {
                    $scope.feedbacktype = item.fmtY_FeedbackTypeName;
                    $scope.question = item.fmqE_FeedbackQuestions;
                    $scope.getquestionsoptions = promise.getquestionsoptions;
                }
            });
        };

        $scope.addtocart = function (obj) {

            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn = [];
                $scope.optionarray = [];

                angular.forEach($scope.feedbackquestionoption, function (hi) {
                    if (hi.Selected) {
                        $scope.albumNameArraycolumn.push({ FMOP_Id: hi.fmoP_Id, FMOP_FeedbackOptions: hi.fmoP_FeedbackOptions });
                    }
                });

                $scope.questionid = obj.FMQE_Id_New;
                $scope.typeid = $scope.FMTY_Id;

                angular.forEach($scope.feedbacktype, function (type) {
                    if (type.fmtY_Id === parseInt($scope.typeid)) {
                        $scope.typename = type.fmtY_FeedbackTypeName;
                    }
                });

                angular.forEach($scope.feedbackquestiond, function (ques) {
                    if (ques.fmqE_Id === parseInt($scope.questionid)) {
                        $scope.questionname = ques.fmqE_FeedbackQuestions;
                        $scope.FMQEManualEntryFlg = ques.fmqE_ManualEntryFlg;
                    }
                });

                var count = 0;
                $scope.typeadd = true;
                obj.FMQE_Id_New = "";
                if ($scope.temparraydetails.length === 0) {
                    $scope.temparraydetails.push({
                        FMQE_Id: $scope.questionid, feedbackquestionname: $scope.questionname,
                        FMTY_Id: $scope.typeid, feedbacktypename: $scope.typename, FMQE_ManualEntryFlg: $scope.FMQEManualEntryFlg,
                        optiondetails: $scope.albumNameArraycolumn
                    });
                } else if ($scope.temparraydetails.length > 0) {
                    angular.forEach($scope.temparraydetails, function (dd) {
                        if (parseInt(dd.FMQE_Id) === parseInt($scope.questionid)) {
                            count += 1;
                        }
                    });

                    if (count === 0) {
                        $scope.temparraydetails.push({
                            FMQE_Id: $scope.questionid, feedbackquestionname: $scope.questionname,
                            FMTY_Id: $scope.typeid, feedbacktypename: $scope.typename, FMQE_ManualEntryFlg: $scope.FMQEManualEntryFlg,
                            optiondetails: $scope.albumNameArraycolumn
                        });
                    } else {
                        $scope.typeadd = false;
                        swal("Question Already Added To Cart");
                    }
                    angular.forEach($scope.feedbackquestionoption, function (hi) {
                        hi.Selected = false;
                    });

                }
            } else {
                $scope.submitted = true;
            }
        };

        $scope.deletecart = function (obj, index) {
            angular.forEach($scope.temparraydetails, function (dd, i) {
                if (parseInt(i) === parseInt(index)) {
                    $scope.temparraydetails.splice(i, 1);
                }
            });
        };

        $scope.savemultiple = function (obj) {
            $scope.albumNameArraycolumn = [];
            $scope.all = false;
            var data = {
                "FMTY_Id": $scope.FMTY_Id,
                "temp_question": $scope.temparraydetails
            };

            apiService.create("FeedbackTypeQuestionMapping/savedatanew", data).then(function (promise) {
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
        };

        $scope.deactiveoption = function (item, SweetAlert) {
            var dystring = "";
            if (item.fmtqO_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (item.fmtqO_ActiveFlag === false) {
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
                        apiService.create("FeedbackTypeQuestionMapping/deactiveoption", item).
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
                                $('#myModal35').modal('hide');                                
                                $scope.BindData();
                            });
                    }
                    else {
                        swal("Cancel");
                    }
                });
        };

        $scope.isOptionsRequiredoptions = function () {
            return !$scope.feedbackquestionoption.some(function (options) {
                return options.Selected;
            });
        };

        $scope.getOrdernew = function (orderarray) {
            var data = {
                "temp_question_order_TemporderDTO": orderarray
            };

            apiService.create("FeedbackTypeQuestionMapping/getordernew", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Record Updated Successfully");
                    } else {
                        swal("Failed To Update Record");
                    }
                    $scope.BindData();
                });
        };

        // Common Functions

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

        $scope.getOrder = function (orderarray) {
            var data = {
                "FeedbackTypeQuestionMappingTemporderDTO": orderarray
            };

            apiService.create("FeedbackTypeQuestionMapping/getorder", data).
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

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
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

        $scope.sortableOptionsnew = {
            stop: function (e, ui) {
                for (var index in $scope.getquestionsoptions) {
                    $scope.getquestionsoptions[index].fmtqO_TQOOrder = Number(index) + 1;
                }
            }
        };
    }

})();