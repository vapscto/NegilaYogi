
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HODExamTopperController', HODExamTopperController)

    HODExamTopperController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function HODExamTopperController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.graph = false;
        $scope.secgraph = false;

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.graphstudlist = [];

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("HODExamTopper/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;

                })
        };



        $scope.onclasschange = function (asmcL_Id) {
            $scope.asmS_Id = '';

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": asmcL_Id,
                "EMCA_Id": $scope.emcA_Id

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODExamTopper/getsection", data).
                then(function (promise) {

                    if (promise.seclist.length > 0) {
                        $scope.sectionlist = promise.seclist

                    }

                    //else {
                    //    swal('No Class Found For selected academic year');
                    //}


                })
        }


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.asmcL_Id = '';
            $scope.emE_Id = '';
            $scope.asmcL_Id = '';
            $scope.emE_Id = '';
            $scope.classlist = [];
            $scope.exmstdlist = [];
            $scope.sectionlist = [];
            $scope.emcA_Id = '';
            $scope.fillcategory = [];
            apiService.getURI("HODExamTopper/getcategory", asmaY_Id).
                then(function (promise) {
                    if (promise.fillcategory.length > 0) {
                        $scope.fillcategory = promise.fillcategory;

                    }
                    //else {
                    //    swal("No Record Found")
                    //}

                });

        }
        $scope.onselectcategory = function (emcA_Id) {
            $scope.asmcL_Id = '';
            $scope.emE_Id = '';
            $scope.asmS_Id = '';
            $scope.classlist = [];
            $scope.exmstdlist = [];
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "EMCA_Id": emcA_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODExamTopper/getclassexam", data).
                then(function (promise) {

                    if (promise.classlist.length > 0) {
                        $scope.classlist = promise.classlist

                    }

                    //else {
                    //                 swal('No Class Found');
                    //}

                    if (promise.exmstdlist.length > 0) {
                        $scope.exmstdlist = promise.exmstdlist

                    }

                    else {
                        swal('No Exam Found');
                    }
                })


        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.showreport = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.sectionranklist = [];
                $scope.classranklist = [];
                $scope.graph = false;
                $scope.secgraph = false;

                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "EMCA_Id": $scope.emcA_Id,
                    "ASMS_Id": $scope.asmS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HODExamTopper/showreport", data).
                    then(function (promise) {


                        $scope.loaddata();
                        if (promise.classranklist != null) {

                            $scope.classranklist = promise.classranklist;
                            $scope.graph = true;





                        }
                        else {
                            swal(' Classwise topper list Not found');
                        }

                        if (promise.sectionranklist != null) {
                            $scope.sectionranklist = promise.sectionranklist
                            $scope.secgraph = true;
                        }
                        else {
                            swal(' Sectionwise topper list not found');

                        }
                        if (promise.sectionranklist == null && promise.classranklist == null) {
                            swal('Both Class and Section topper list Not found');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.loaddata = function () {

            if ($scope.exmstdlist != null) {
                angular.forEach($scope.exmstdlist, function (ex) {
                    if (ex.emE_Id == $scope.emE_Id) {

                        $scope.examname = ex.emE_ExamName;

                    }

                })
            }
            if ($scope.yearlt != null) {
                angular.forEach($scope.yearlt, function (yr) {
                    if (yr.asmaY_Id == $scope.asmaY_Id) {

                        $scope.academicyear = yr.asmaY_Year;

                    }

                })
            }
            if ($scope.classlist != null) {
                angular.forEach($scope.classlist, function (cl) {
                    if (cl.asmcL_Id == $scope.asmcL_Id) {

                        $scope.classname = cl.asmcL_ClassName;

                    }

                })
            }

            if ($scope.sectionlist != null) {
                angular.forEach($scope.sectionlist, function (sc) {
                    if (sc.asmS_Id == $scope.asmS_Id) {
                        $scope.sectionname = sc.asmC_SectionName;

                    }

                })
            }

        }

        $scope.showstudentGrid = function () {
            var data = {
                "EME_Id": $scope.emE_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODExamTopper/showsectioncount", data).
                then(function (promise) {

                    if (promise.seclist.length > 0) {

                        $scope.seclist = promise.seclist;
                        $scope.sectionlist = [];
                        if ($scope.seclist != null) {

                            for (var i = 0; i < $scope.seclist.length; i++) {
                                $scope.sectionlist.push({ sec: $scope.seclist[i].asmC_SectionName, total: $scope.seclist[i].pass_Count + $scope.seclist[i].fail_Count, pass: $scope.seclist[i].pass_Count, fail: $scope.seclist[i].fail_Count })
                            }
                        }
                    }
                    else {
                        swal('No record Found');
                    }
                })
        }


        $scope.cancel = function () {
           
            $state.reload();
        }






    }

})();