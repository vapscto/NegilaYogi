(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingquestionController', TrainingquestionController)

    TrainingquestionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TrainingquestionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'HRTRQNS_Id';
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
            angular.forEach($scope.question_list, function (itm) {
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
        $scope.question_view = function (ev) {

            $scope.eve = ev.hrtcR_Id;
            var pageid = $scope.eve;
            apiService.getURI("Training_Master/option_view_TQM", pageid).then(function (promise) {

                $scope.question_view_list = promise.question_view_list;

            });

        };
        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_TQM", pageid)
                .then(function (promise) {
                    $scope.question = false;
                    $scope.training_list = promise.training_list;
                    $scope.question_list = promise.question_list;
                    $scope.training_question_mapping_list = promise.training_question_mapping_list;
                    $scope.presentCountgrid = $scope.training_question_mapping_list.length;

                });
        };




        // edit
        $scope.edibl = {};
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrtcR_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_TQM", pageid).
                then(function (promise) {
                    $scope.question_list1 = [];
                    $scope.question = true;

                    $scope.hrtcR_PrgogramName = promise.training_question_details_list[0].hrtcR_PrgogramName;
                    $scope.Id = promise.training_question_details_list[0].hrtcR_Id;
                    $scope.HRTCR_Id = promise.training_question_details_list[0].hrtcR_Id;
                    $scope.question_list1 = promise.question_details_list;
                    $scope.question_list = promise.question_list;

                    if ($scope.question_list1.length > 0) {
                        angular.forEach($scope.question_list, function (ss) {
                            angular.forEach($scope.question_list1, function (qq) {
                                if (ss.hrmfqnS_Id == qq.hrmfqnS_Id)
                                    ss.selectedd2 = true;
                            });


                        });
                    }
                });
        };


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.question_Option = [];
            angular.forEach($scope.question_list, function (cls) {
                if (cls.selectedd2 === true) {
                    $scope.question_Option.push(cls);
                }
            });
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRTCR_Id": $scope.HRTCR_Id,
                    question_Option: $scope.question_Option,
                    "hqn_id": $scope.Id
                };
                apiService.create("Training_Master/SaveEdit_TQM", data).
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
            if (bL.hrtrqnS_ActiveFlg === false) {
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
                        apiService.create("Training_Master/deactivate_TQM", bL).
                            then(function (promise) {


                                if (promise.returnval === false) {
                                    swal('Master Training Question  Deactivated Successfully');
                                    $state.reload();
                                }


                                else if (promise.returnval === true) {
                                    swal('Master Training Question Activated Successfully');
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



