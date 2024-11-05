(function () {
    'use strict';
    angular.module('app').controller('LP_MasterComplexitiesController', LP_MasterComplexitiesController)

    LP_MasterComplexitiesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce', '$window']
    function LP_MasterComplexitiesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $q, $sce, $window) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'LMSMOEQ_Id';
        $scope.sortReverse = true;
        $scope.LPMOEQ_SubjectiveFlg = false;
        $scope.edit = false;
        $scope.getexamhappenedcounttemp = false;

        $scope.answer = "";
        $scope.show_ansOption = false;
        $scope.obj = {};
        $scope.obj.searchValueddd = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.btn = false;

        $scope.itemsPerPage1 = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.itemsPerPage3 = paginationformasters;

        $scope.teacherdocuupload = {};
        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

        $scope.teacherdocuuploadopts = {};
        $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];

        $scope.totalgrid = [];
        $scope.LPMCOMP_DefaultFlg = false;


        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("LP_OnlineExam/getmastercompliexities", pageid).then(function (promise) {
                $scope.getMasterComplexitiesdetails = promise.getMasterComplexitiesdetails;
                var count = 0;
                angular.forEach($scope.getMasterComplexitiesdetails, function (dd) {
                    if (dd.lpmcomP_DefaultFlg) {
                        count = 1;
                    }
                });
                $scope.flag = count;

            });
        };
        $scope.obj.LPMCOMP_DefaultFlg = false;

        $scope.SaveMasterComplexity = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "LPMCOMP_Id": $scope.LPMCOMP_Id !== undefined && $scope.LPMCOMP_Id !== null && $scope.LPMCOMP_Id !== "" ? $scope.LPMCOMP_Id : 0,
                    "LPMCOMP_ComplexityName": $scope.LPMCOMP_ComplexityName,
                    "LPMCOMP_ComplexityDesc": $scope.LPMCOMP_ComplexityDesc,
                    "LPMCOMP_DefaultFlg": $scope.obj.LPMCOMP_DefaultFlg
                };
                apiService.create("LP_OnlineExam/SaveMasterComplexity", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {
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
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $scope.cleartabl1();
                    }
                });

            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.EditMasterComplexity = function (d) {
            angular.forEach($scope.getMasterComplexitiesdetails, function (dd) {
                if (parseInt(d) === dd.lpmcomP_Id) {
                    $scope.LPMCOMP_ComplexityName = dd.lpmcomP_ComplexityName;
                    $scope.LPMCOMP_ComplexityDesc = dd.lpmcomP_ComplexityDesc;
                    $scope.obj.LPMCOMP_DefaultFlg = dd.lpmcomP_DefaultFlg;
                    $scope.LPMCOMP_Id = dd.lpmcomP_Id;

                    if ($scope.obj.LPMCOMP_DefaultFlg === true) {
                        $scope.flag = 0;
                    }

                }
            });
        };

        $scope.DeactivateActivateComplexities = function (dd) {
            var mgs = "";
            var confirmmgs = "";
            if (dd.lpmcomP_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LPMCOMP_Id": dd.lpmcomP_Id,
            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("LP_OnlineExam/DeactivateActivateComplexities", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.cleartabl1();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.cleartabl1 = function () {
            $scope.LPMCOMP_ComplexityName = "";
            $scope.LPMCOMP_ComplexityDesc = "";
            $scope.LPMCOMP_Id = "";
            $scope.obj.LPMCOMP_DefaultFlg = false;
            $scope.getMasterComplexitiesdetails = [];
            $scope.loaddata();
        };

        $scope.searchValue1 = function (obj) {
            return (angular.lowercase(obj.lpmcomP_ComplexityName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.lpmcomP_ComplexityDesc)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0;
        };
    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });

})();