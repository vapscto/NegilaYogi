
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExmSectionPerformController', ExmSectionPerformController)

    ExmSectionPerformController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ExmSectionPerformController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.graph = false;
        $scope.sectiongrid = false;
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.graphstudlist = [];

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("ExmSectionPerform/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;

                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.emcA_Id = '';
            $scope.fillcategory = [];
            apiService.getURI("ExmSectionPerform/getcategory", asmaY_Id).
                then(function (promise) {
                    if (promise.fillcategory.length > 0) {
                        $scope.fillcategory = promise.fillcategory;

                    }
                    else {
                        swal("Category not found")
                    }

                });

        }


        $scope.onexamchange = function (emE_Id) {
            $scope.ismS_Id = '';
            var data = {
                "EME_Id": emE_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "EMCA_Id": $scope.emcA_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExmSectionPerform/getsubject", data).
                then(function (promise) {

                    //if (promise.subjlist.length > 0) {

                    $scope.subjlist = promise.subjlist;

                    //}
                    //else {
                    //    swal('No record Found');
                    //}
                })

        }
        $scope.onselectcategory = function (emcA_Id) {
            $scope.asmcL_Id = '';
            $scope.emE_Id = '';
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "EMCA_Id": emcA_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExmSectionPerform/getclassexam", data).
                then(function (promise) {

                    if (promise.classlist.length > 0) {
                        $scope.classlist = promise.classlist

                    }

                    else {
                        swal('No Class Found');
                    }

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
            $scope.graph = false;
            $scope.sectiongrid = false;

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "EMCA_Id": $scope.emcA_Id,
                    "ISMS_Id": $scope.ismS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ExmSectionPerform/showreport", data).
                    then(function (promise) {
                        $scope.seclist = promise.seclist;
                        if (promise.seclist != null) {

                            $scope.seclist = promise.seclist;
                            $scope.graph = true;
                            $scope.sectiongrid = true;

                            $scope.loadchart();
                        }
                        else {
                            swal('No record Found');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

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
            apiService.create("ExmSectionPerform/showsectioncount", data).
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
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }


        $scope.loadchart = function () {


            $scope.stdgraphseries3 = [];
            if ($scope.seclist != null) {

                for (var i = 0; i < $scope.seclist.length; i++) {
                    $scope.stdgraphseries3.push({ label: $scope.seclist[i].asmC_SectionName, "y": $scope.seclist[i].estmpS_SectionAverage })
                }
            }
            console.log($scope.stdgraphseries3);



            var chart = new CanvasJS.Chart("rangeBarChat",
                {
                    width: 1083,
                    height: 350,
                    axisY: {
                        labelFontSize: 12
                    },
                    axisX: {
                        interval: 1,
                        labelFontSize: 12
                    },

                    data: [
                        {
                            type: "column",

                            color: "#1f1f7a",

                            showInLegend: true,
                            dataPoints: $scope.stdgraphseries3
                        }

                    ]
                });

            chart.render();



        }



    }

})();