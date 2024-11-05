(function () {
    'use strict';
    angular
        .module('app')
        .controller('QuestionController', QuestionController)

    QuestionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function QuestionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        //$scope.HRMFQNS_QuestionForFlg = 'Trainer';
        $scope.sortKey = 'HRMFQNS_Id';
        $scope.sortReverse = true;
        var paginationformasters = 10;
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_question", pageid)
                .then(function (promise) {
                    $scope.question_list = promise.question_list;
                    $scope.presentCountgrid = $scope.question_list.length;
                });
        };

        // edit
        $scope.edibl = {};
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmfqnS_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_question", pageid).
                then(function (promise) {
                    $scope.Id = promise.question_details_list[0].hrmfqnS_Id;
                    $scope.HRMFQNS_QuestionName = promise.question_details_list[0].hrmfqnS_QuestionName;
                    if (promise.question_details_list[0].hrmfqnS_QuestionTypeFlg == "Subjective") {
                        $scope.HRMFQNS_QuestionTypeFlg = true;
                    }
                    else {
                        $scope.HRMFQNS_QuestionTypeFlg = false;
                    }
                    $scope.HRMFQNS_QuestionOrder = promise.question_details_list[0].hrmfqnS_QuestionOrder;
                });
        };


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {
            var flag = "";
            if ($scope.HRMFQNS_QuestionTypeFlg == true) {
                flag = "Subjective";
            }
            else {
                flag = "Objective";
            }
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMFQNS_QuestionName": $scope.HRMFQNS_QuestionName,
                    "HRMFQNS_QuestionTypeFlg": flag,
                    "HRMFQNS_QuestionOrder": $scope.HRMFQNS_QuestionOrder,
                    "HRMFQNS_Id": $scope.Id,
                    "HRMFQNS_QuestionForFlg": $scope.HRMFQNS_QuestionForFlg
                };
                apiService.create("Training_Master/SaveEdit_question", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {
                            if (promise.returnvalue == "Update") {
                                swal('Record Updated Successfully');
                            }
                            else if (promise.returnvalue == "Add") {
                                swal('Record Saved Successfully');
                            }
                            else if (promise.returnvalue == "Exist") {
                                swal('Record Already Exist!!!');
                            }
                            else if (promise.returnvalue == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvalue === "Duplicate") {
                                swal('Category Already Exist');
                            }
                            $state.reload();
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        //deactive
        $scope.deactive = function (bL, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.hrmfqnS_ActiveFlg === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Question ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Master/deactivate_question", bL).
                            then(function (promise) {

                                if (promise.hrmfqnS_ActiveFlg === true) {
                                    if (promise.returnval === false) {
                                        swal('Master Question  Deactivated Successfully');
                                    }
                                }
                                else if (promise.hrmfqnS_ActiveFlg === false) {
                                    swal('Master Question Activated Successfully');
                                }
                                //   }

                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }
})();



