(function () {
    'use strict'; angular.module('app')
        .controller('DepartmentwisegendercountController', DepartmentwisegendercountController)
    DepartmentwisegendercountController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function DepartmentwisegendercountController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {


   



        $scope.gridshow = true;

        var Male = new Array(55, 5, 88,  5, 18);

        for (var i = 0; i < Male.length; i++) {
            Male[i];
        }

        var Famale = new Array(88, 8, 2, 5, 77);

        for (var i = 0; i < Famale.length; i++) {
            Famale[i];
        }
        var Other = new Array(12, 8,  8, 23, 5);

        for (var i = 0; i < Other.length; i++) {
            Other[i];
        }
        $("#chart123").kendoChart({
            title: {
                text: "Department Wise Gender Count"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "Male",
                data: Male
            }, {
                name: "Female",
                data: Famale
            }, {
                name: "Other",
                data: Other
            },],
            valueAxis: {
                labels: {
                    format: "{0}"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: 0
            },
            categoryAxis: {
                // categories: [Software, Accounts, ManagementAdmin, Admin, HumanResourse, HumanResourse],
                categories: ["HIGH SCHOOL", "MIDDLE SCHOOL", "PRIMARY SCHOOL", "PE DEPT	", "NCC"],
                // categories: ids1,
                line: {
                    visible: false
                },
                labels: {
                    padding: { top: 35 }
                }
            },
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value #"
            }
        });





        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("HRMSReportGraphs/getalldetails", pageid).then(function (promise) {
                //dropdown list
                $scope.coutrylist = promise.coutrylist;
                $scope.monthlist = promise.monthlist;
                $scope.yearlist = promise.yearlist;
                $scope.departmentdropdown = promise.departmentdropdown;
                $scope.yearvalue = "2020";

                angular.forEach($scope.coutrylist, function (grp_t) {
                    if (grp_t.ivrmmC_Default === 1) {
                        $scope.ivrmmC_Id = grp_t.ivrmmC_Id;
                    }
                })

               






                $scope.statelist();

            })
        }


        $scope.statelist = function () {
            var data = {
                "IVRMMC_Id": $scope.ivrmmC_Id
            }
            apiService.create("HRMSReportGraphs/statelist", data).then(function (promise) {
                if (promise.statedropdown !== null && promise.statedropdown.length > 0) {
                    $scope.statedropdown = promise.statedropdown;
                    //$scope.dprtmetall = true;
                    //$scope.alldepartment();
                }
            })
        };





        $scope.get_depts = function () {
            var data = {
                MI_Idlist: ids
            }
            apiService.create("HRMSReportGraphs/get_depts", data).then(function (promise) {
                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    //$scope.dprtmetall = true;
                    //$scope.alldepartment();
                }
            })
        };


        $scope.all_check = function () {
            var toggleStatus = $scope.Allmi_id;
            angular.forEach($scope.statedropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_depts();
        }

        $scope.alldepartment = function (dprtmetall) {
            var toggleStatus1 = dprtmetall;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected1 = toggleStatus1;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.get_mi_list.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.departmentdropdown.some(function (options) {
                return options.selected;
            });
        }
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };


        $scope.Getreport123 = function () {
            // if ($scope.myForm.$valid) {
            var ids1 = [];
            var ids = [];
            $scope.getReportvalue = [];
            var total = 0;
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected1) {
                    ids1.push(grp_t.hrmD_Id);
                }
            })

            angular.forEach($scope.statedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.ivrmmS_Id);
                }
            })
            var data = {
                hrmD_IdList: ids1,
                ivrmmS_IdList: ids
            }
            $scope.getReportvalue.push({
                "year": 45, "year": 45, "year": 45, "year": 45, "year": 45, "year": 45, "year": 45, "year": 45
            })
            apiService.create("HRMSReportGraphs/Getreport", data).then(function (promise) {
               // if (promise.getReportvalue !== null && promise.getReportvalue.length > 0) {
                    //$scope.getReportvalue 

                $scope.departmentgraph = [];
                angular.forEach($scope.getReportvalue, function (dd) {
                    $scope.departmentgraph.push({
                        label: dd.HRMD_DepartmentName,
                        // label: dd.IVRMMS_Name,
                        y: dd.Count,
                    })
                })


                //////////////"column
                $("#chart123").kendoChart({
                    title: {
                        text: "Department Wise Gender Count"
                    },
                    legend: {
                        position: "top"
                    },
                    seriesDefaults: {
                        type: "column"
                    },
                    series: [{
                        name: "Male",
                        data: [25, 4, 7,  5, 18]
                    }, {
                        name: "Famale",
                            data: [12, 8,  28, 5, 18]
                    }, {
                        name: "Other",
                        data:  [12, 8, 9,  5, 18]
                    },],
                    valueAxis: {
                        labels: {
                            format: "{0}%"
                        },
                        line: {
                            visible: false
                        },
                        axisCrossingValue: 0
                    },
                    categoryAxis: {
                        categories: [Software, Accounts, ManagementAdmin, Admin, HumanResourse],
                        line: {
                            visible: false
                        },
                        labels: {
                            padding: { top: 135 }
                        }
                    },
                    tooltip: {
                        visible: true,
                        format: "{0}%",
                        template: "#= series.name #: #= value #"
                    }
                });






            });
            //}
            //else {
            //    $scope.submitted = true;
            //}
        }




        $scope.Getreport = function () {
            var ids1 = [];
            $scope.gridshow = true;
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected1) {
                    ids1.push(grp_t.hrmD_DepartmentName);
                }
            })

            

 

            var Male = new Array(55, 5, 88,  5, 18);

            for (var i = 0; i < Male.length; i++) {
                Male[i];
            }

            var Famale = new Array(88, 8,  2, 5,77);

            for (var i = 0; i < Famale.length; i++) {
                Famale[i];
            }
            var Other = new Array(12, 8, 9, 23,5);

            for (var i = 0; i < Other.length; i++) {
                Other[i];
            }
                $("#chart123").kendoChart({
                    title: {
                        text: "Department Wise Gender Count"
                    },
                    legend: {
                        position: "top"
                    },
                    seriesDefaults: {
                        type: "column"
                    },
                    series: [{
                        name: "Male",
                        data: Male
                    }, {
                        name: "Famale",
                            data: Famale
                    }, {
                        name: "Other",
                            data: Other
                    },],
                    valueAxis: {
                        labels: {
                            format: "{0}"
                        },
                        line: {
                            visible: false
                        },
                        axisCrossingValue: 0
                    },
                    categoryAxis: {
                        // categories: [Software, Accounts, ManagementAdmin, Admin, HumanResourse, HumanResourse],
                        categories: ["HIGH SCHOOL", "MIDDLE SCHOOL",  "PRIMARY SCHOOL", "PE DEPT	", "NCC"],
                       // categories: ids1,
                        line: {
                            visible: false
                        },
                        labels: {
                            padding: { top: 35 }
                        }
                    },
                    tooltip: {
                        visible: true,
                        format: "{0}%",
                        template: "#= series.name #: #= value #"
                    }
                });
            

        



        }

        $scope.Clearall = function () {
            $state.reload();
        }
    }
})();



