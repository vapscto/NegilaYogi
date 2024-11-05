(function () {
    'use strict';

    angular
        .module('app')
        .controller('HODStudentStrengthController', HODStudentStrengthController);

    HODStudentStrengthController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];

    function HODStudentStrengthController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.studentstrenthgr = false;
        $scope.totalregstudent = 0;

        $scope.totalnewstudent = 0;
        $scope.sms = 0;
        $scope.email = 0;
        $scope.regular = [];
        $scope.fields = function () {

            $scope.newadmissionstdtotal = [];
            $scope.datagraph = [];
            $scope.regularstdtotal = [];
            $scope.newadmstdgraphdta = [];


            $scope.Todaydate = new Date();
        }



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Binddata = function () {
            $scope.fields();
            $scope.studentstrenthgr = false;

            apiService.getDATA("HODStudentStrength/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.studentstrenth = promise.fillstudentstrenth;
                    if ($scope.studentstrenth.length > 0) {
                        $scope.studentstrenthgr = true;

                        $scope.regular = promise.sectionwisestrenth;

                        $scope.asmaY_Id = promise.asmaY_Id;
                        $scope.asmaY_Year = promise.asmaY_Year;

                        $scope.loadcharts();

                    }

                    else {
                        swal("No Record Found")
                    }




                })

        }

        $scope.showsectionGrid = function (classid) {

            //alert(asmayid)
            //alert(classid)


            var data = {
                "classid": classid,
                "ASMAY_Id": $scope.asmaY_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODStudentStrength/Getsectioncount", data).
                then(function (promise) {



                    $scope.fillsectioncount = promise.fillsectioncount;





                })

        }

        $scope.loadcharts = function () {
            var total = 0;
            var total1 = 0;


            if ($scope.regular != null) {

                for (var i = 0; i < $scope.regular.length; i++) {
                    $scope.regularstdtotal.push({ label: $scope.regular[i].class_Name + '-' + $scope.regular[i].sectionname, "y": $scope.regular[i].stud_count })
                }
            }








            if ($scope.sectioncount != null) {

                for (var i = 0; i < $scope.sectioncount.length; i++) {
                    $scope.newadmstdgraphdta.push({ label: $scope.sectioncount[i].section, "y": $scope.sectioncount[i].stud_count })
                }
            }




            var chart = new CanvasJS.Chart("areachart",
                {
                    width: 1070,
                    height: 348,
                    axisX: {
                        labelFontSize: 10,
                        interval: 1,
                        labelAngle: -45,
                        labelFontColor: "black",
                        labelFontWeight: "bold",
                        //title: "Designation",
                    },
                    axisY: {
                        labelFontSize: 12,
                        // title: "No.of. Staffs",

                    },

                    data: [
                        {
                            type: "column",
                            showInLegend: true,
                            dataPoints: $scope.regularstdtotal
                        }
                    ]
                });

            chart.render();



        }




        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.regularstdtotal = [];
            $scope.studentstrenthgr = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var a = $scope.asmaY_Id;
                // alert(asmaY_Id)
                $scope.fields();

                apiService.getURI("HODStudentStrength/getclass", asmaY_Id).
                    then(function (promise) {
                        $scope.studentstrenth = promise.fillstudentstrenth;
                        if ($scope.studentstrenth.length > 0) {


                            $scope.studentstrenthgr = true;

                            // $scope.yearlt = promise.yearlist;
                            $scope.regular = promise.sectionwisestrenth;

                            $scope.asmaY_Id = promise.asmaY_Id;



                            $scope.loadcharts();

                        }
                        else {
                            swal("No Record Found")
                        }

                    })
            }
            else {
                $scope.submitted = true;
                $scope.studentstrenthgr = false;
            }

        }
        $scope.OnClass = function (asmcL_Id) {

            var a = $scope.asmcL_Id;
            // alert(asmaY_Id)
            $scope.fields();
            var data = {
                "classid": asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODStudentStrength/Getsection", data).
                then(function (promise) {


                    // $scope.sectioncount = promise.fillstudentstrenth;
                    // $scope.regular = promise.sectionwisestrenth;
                    // alert($scope.regular)
                    // $scope.yearlt = promise.yearlist;
                    // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
                    //$scope.regstdtotal = promise.fillregstd;
                    //$scope.newadmstdtotal = promise.fillnewadmstd;
                    //$scope.asmaY_Id = promise.asmaY_Id;
                    //$scope.newadmstdgraph = promise.newadmstd;
                    //$scope.year = promise.yearlist[0].asmaY_Year;

                    //
                    //$scope.classarray = promise.classarray;
                    //$scope.sectionarray = promise.sectionarray;
                    //$scope.newadmit = promise.sectionwisestrenth;

                    $scope.loadcharts();

                })


        }




    };
})();
