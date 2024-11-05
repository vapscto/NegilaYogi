(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineExamMasterQuestionDeactivateController', LP_OnlineExamMasterQuestionDeactivateController)

    LP_OnlineExamMasterQuestionDeactivateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce', '$window']
    function LP_OnlineExamMasterQuestionDeactivateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $q, $sce, $window) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'LMSMOEQ_Id';
        $scope.sortReverse = true;
        $scope.LPMOEQ_SubjectiveFlg = false;

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
        $scope.totalgrid = [];

        //***** MASTER QUESTION ******//
        $scope.loaddatadeactivate = function () {
            var pageid = 2;
            apiService.getURI("LP_OnlineExam/getmasterquestionloaddata", pageid).then(function (promise) {
                $scope.yearlist = promise.getyearlist;
                $scope.getMasterQuestiondetails = promise.getMasterQuestiondetails;
                $scope.getConfigurationSettings = promise.getConfigurationSettings;

                //$scope.btn = false;

                if (promise.getclasslist !== null && promise.getclasslist.length > 0) {
                    $scope.classlist = promise.getclasslist;
                }
            });
        };

        $scope.getclasslistdeactivate = function () {
            $scope.totalgrid = [];
            $scope.ASMCL_Id = "";
            $scope.classlist = [];
            $scope.ISMS_Id = "";
            $scope.getSubjects = [];
            $scope.LPMT_Id = "";
            $scope.gettopiclist = [];
            $scope.LPMOEQ_Question = "";
            $scope.LPMOEQ_QuestionDesc = "";
            $scope.LPMOEQ_Marks = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("LP_OnlineExam/getclasslistdeactivate", data).then(function (promise) {

                if (promise.getclasslist !== null && promise.getclasslist.length > 0) {
                    $scope.classlist = promise.getclasslist;
                }
            });

        };

        $scope.getsubjectlistdeactivate = function () {
            $scope.totalgrid = [];
            var data = {
                //"ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("LP_OnlineExam/getsubjectlistdeactivate", data).then(function (promise) {
                if (promise.getsubjectlist !== null && promise.getsubjectlist.length > 0) {
                    $scope.getSubjects = promise.getsubjectlist;
                }
            });

        };

        $scope.onchangefromdate = function () {
            $scope.ToDate = null;
        };

        $scope.GetQuestionList = function () {
            var data = {
                "ISMS_Id": $scope.ISMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "FromDate": new Date($scope.FromDate).toDateString(),
                "ToDate": new Date($scope.ToDate).toDateString()
            };
            apiService.create("LP_OnlineExam/GetQuestionList", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getquestionlist = promise.getquestionlist;
                }
            });
        };

        $scope.SaveDeactiveQuestionDetails = function () {
            if ($scope.myForm.$valid) {
                $scope.tempques = [];
                angular.forEach($scope.getquestionlist, function (dd) {
                    if (dd.checked) {
                        $scope.tempques.push({ LPMOEQ_Id: dd.lpmoeQ_Id });
                    }
                });
                if ($scope.tempques.length === 0) {
                    swal("Select Atleast One Question To Save Record");
                    return;
                }
                var data = {
                    "ISMS_Id": $scope.ISMS_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "FromDate": new Date($scope.FromDate).toDateString(),
                    "ToDate": new Date($scope.ToDate).toDateString(),
                    "tempquestiondto": $scope.tempques
                };
                apiService.create("LP_OnlineExam/SaveDeactiveQuestionDetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Records Deactivated Successfully");
                        } else {
                            swal("Records Failed To Deactivated");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.toggleAll_S = function (checkall) {
            var toggleStatus = checkall;
            angular.forEach($scope.getquestionlist, function (itm) {
                itm.checked = toggleStatus;
            });
        };

        $scope.optionToggled_S = function () {
            $scope.obj.checkall = $scope.getquestionlist.every(function (itm) { return itm.checked; });
        };

        $scope.cleartabl1 = function () {
            $state.reload();
        };

        $scope.searchValue1 = function (obj) {
            return (angular.lowercase(obj.lpmoeQ_Question)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.lpmoeQ_QuestionDesc)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.lpmT_TopicName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (JSON.stringify(obj.lpmoeQ_Marks)).indexOf($scope.searchValueddd) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
    }
})();