(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterquestionoptionController', MasterquestionoptionController)

    MasterquestionoptionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterquestionoptionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'HRMQNOP_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        $scope.all_check = function () {
            var checkStatus2 = $scope.usercheck;
            angular.forEach($scope.option_list, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.isOptionsRequired = function () {

            return !$scope.option_list.some(function (sec) {
                return sec.selectedd2;
            });
        };
        $scope.togchkbx = function () {

            $scope.usercheck = $scope.option_list.every(function (role) {
                return role.selectedd2;
            });
        };
        $scope.option_view = function (ev) {

            $scope.eve = ev.hrmfqnS_Id;
            var pageid = $scope.eve;
            apiService.getURI("Training_Master/option_view_MQO", pageid).then(function (promise) {

                $scope.option_view_list = promise.option_view_list;

            });

        };
        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_MQO", pageid)
                .then(function (promise) {
                    $scope.question = false;
                    $scope.question_list = promise.question_list;
                    $scope.option_list = promise.option_list;
                    $scope.question_option_list = promise.question_option_list;
                    $scope.presentCountgrid = $scope.question_option_list.length;

                });
        };




        // edit
        $scope.edibl = {};
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmfqnS_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_MQO", pageid).
                then(function (promise) {
                    $scope.option_list1 = [];
                    $scope.question = true;
                    //$scope.question_list = promise.question_details_list;
                    $scope.hrmfqnS_QuestionName = promise.question_details_list[0].hrmfqnS_QuestionName;
                    $scope.Id = promise.question_details_list[0].hrmfqnS_Id;
                    $scope.HRMFQNS_Id = promise.question_details_list[0].hrmfqnS_Id;
                    $scope.option_list1 = promise.option_details_list;
                    $scope.option_list = promise.option_list;

                    if ($scope.option_list1.length > 0) {
                        angular.forEach($scope.option_list, function (ss) {
                            angular.forEach($scope.option_list1, function (qq) {
                                if (ss.hrmfopT_Id == qq.hrmfopT_Id)
                                    ss.selectedd2 = true;
                            });


                        });
                    }
                });
        };


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.Feedback_Option = [];
            angular.forEach($scope.option_list, function (cls) {
                if (cls.selectedd2 === true) {
                    $scope.Feedback_Option.push(cls);
                }
            });
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMFQNS_Id": $scope.HRMFQNS_Id,
                    Feedback_Option: $scope.Feedback_Option,
                    "hqn_id": $scope.Id
                };
                apiService.create("Training_Master/SaveEdit_MQO", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue == "Update") {
                                swal('Record Updated Successfully');

                            }
                            else if (promise.returnvalue == "Add") {
                                swal('Record Saved Successfully');

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
            if (bL.hrmqnoP_ActiveFlg === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Question Option ?",
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
                        apiService.create("Training_Master/deactivate_MQO", bL).
                            then(function (promise) {

                               
                                    if (promise.returnval === false) {
                                        swal('Master Feedback Option  Deactivated Successfully');
                                        $state.reload();
                                    }
                                   
                               
                                    else if (promise.returnval === true) {
                                    swal('Master Feedback Option Activated Successfully');
                                    $state.reload();
                                }
                             

                              
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



