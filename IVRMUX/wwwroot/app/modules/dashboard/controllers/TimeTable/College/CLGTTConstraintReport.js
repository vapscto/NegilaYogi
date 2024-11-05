(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGTTConstraintReportController', CLGTTConstraintReportController)

    CLGTTConstraintReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function CLGTTConstraintReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.submitted = false;
        $scope.LoadData = function () {
       
            apiService.getDATA("CLGTTConstraintReport/getalldetails").
                then(function (promise) {
                  
                    $scope.year_list = promise.acayear;
                })
        };

        $scope.ttconstrep = "F";
        $scope.fixing1 = "DP";


        $scope.onclickrdo = function () {
            $scope.alldata1 = [];
            $scope.alldata2 = [];
            $scope.alldata3 = [];
            $scope.alldata4 = [];
            $scope.alldata5 = [];
            $scope.alldata6 = [];
            $scope.alldata7 = [];
            $scope.alldata8 = [];
            $scope.alldata9 = [];
            $scope.alldata10 = [];
            $scope.alldata11 = [];
            $scope.alldata12 = [];
            $scope.alldata13 = [];
        }




        $scope.exportToExcel = function () {
        
            var table = '';
            if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'DP') {
                table = '#A';
            }
            else if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'DS'){
                table = '#B';
            }
            else if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'DSUB') {
                table = '#C';
            }
            else if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'PS') {
                table = '#D';
            }
            else if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'PSUB') {
                table = '#E';
            }
            else if ($scope.ttconstrep === 'R' && $scope.fixing1 === 'DP') {
                table = '#F';
            }
            else if ($scope.ttconstrep === 'R' && $scope.fixing1 === 'DS') {
                table = '#G';
            }
            else if ($scope.ttconstrep === 'R' && $scope.fixing1 === 'DSUB') {
                table = '#H';
            }
            else if ($scope.ttconstrep === 'R' && $scope.fixing1 === 'PS') {
                table = '#I';
            }
            else if ($scope.ttconstrep === 'R' && $scope.fixing1 === 'PSUB') {
                table = '#J';
            }
            else if ($scope.ttconstrep === 'C') {
                table='#K'
            }
            else if ($scope.ttconstrep === 'B') {
                table='#L'
            }
            else if ($scope.ttconstrep === 'L') {
                table='#M'
            }
          
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }




        //$scope.printData = function () {

        //    var innerContents = '';
        //    if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'DP') {
        //        innerContents = document.getElementById("A").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'DS'){
        //        innerContents = document.getElementById("B").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'DSUB'){
        //        innerContents = document.getElementById("C").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'PS') {
        //        innerContents = document.getElementById("D").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'F' && $scope.fixing1 === 'PSUB'){
        //        innerContents = document.getElementById("E").innerHTML;
        //    }
        //    else if($scope.ttconstrep === 'R' && $scope.fixing1 === 'DP') {
        //        innerContents = document.getElementById("F").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'R' && $scope.fixing1 === 'DS') {
        //        innerContents = document.getElementById("G").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'R' && $scope.fixing1 === 'DSUB') {
        //        innerContents = document.getElementById("H").innerHTML;
        //    }
        //    else if($scope.ttconstrep === 'R' && $scope.fixing1 === 'PS') {
        //        innerContents = document.getElementById("I").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'R' && $scope.fixing1 === 'PSUB') {
        //        innerContents = document.getElementById("J").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'C' ) {
        //        innerContents = document.getElementById("K").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'B' ) {
        //        innerContents = document.getElementById("L").innerHTML;
        //    }
        //    else if ($scope.ttconstrep === 'L') {
        //        innerContents = document.getElementById("M").innerHTML;
        //    }



           

            


        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //        '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
        //        '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
        //    );
        //    popupWinindow.document.close();

        //}




        $scope.GetReport = function () {
      
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "constype": $scope.ttconstrep,
                    "periodtype": $scope.fixing1,
                }
                apiService.create("CLGTTConstraintReport/getpagedetails", data).
                    then(function (promise) {
                        
                        if (promise.getreportdata.length > 0) {
                            debugger;
                            if ($scope.ttconstrep === "F") {
                                if ($scope.fixing1 === "DP") {
                                    $scope.alldata1 = promise.getreportdata;

                                }
                                else if ($scope.fixing1 === "DS") {
                                    $scope.alldata2 = promise.getreportdata;
                                }
                                else if ($scope.fixing1 === "DSUB") {
                                    $scope.alldata3 = promise.getreportdata;
                                }
                                else if ($scope.fixing1 === "PS") {
                                    $scope.alldata4 = promise.getreportdata;
                                }
                                else if ($scope.fixing1 === "PSUB") {
                                    $scope.alldata5 = promise.getreportdata;
                                }
                            }
                            else if ($scope.ttconstrep === "R") {
                                if ($scope.fixing1 === "DP") {
                                    $scope.alldata6 = promise.getreportdata;
                                }
                                else if ($scope.fixing1 === "DS") {
                                    $scope.alldata7 = promise.getreportdata;
                                }
                                else if ($scope.fixing1 === "DSUB") {
                                    $scope.alldata8 = promise.getreportdata;
                                }
                                else if ($scope.fixing1 === "PS") {
                                    $scope.alldata9 = promise.getreportdata;
                                }
                                else if ($scope.fixing1 === "PSUB") {
                                    $scope.alldata10 = promise.getreportdata;
                                }
                            }
                            else if ($scope.ttconstrep === "C") {
                                $scope.alldata11 = promise.getreportdata;
                            }
                            else if ($scope.ttconstrep === "B") {
                                $scope.alldata12 = promise.getreportdata;
                            }
                            else if ($scope.ttconstrep === "L") {
                                
                                $scope.alldata13 = promise.getreportdata;
                            }
                        }
                        else {
                            swal("Record Not found");
                        }

                        //if ($scope.ttconstrep === "fix") {

                        //   if (promise.fix_day_period_list.length > 0)
                        //   {
                        //       $scope.header_display = false;
                        //       $scope.name = "Fixing";
                        //       $scope.gridOptions.data = promise.fix_day_period_list;
                        //       $scope.datafix = promise.fix_day_period_list;
                        //       $scope.fix_res = false;
                        //       $scope.conscon = true;
                        //       $scope.bifcon = true;
                        //       $scope.labcon = true;
                        //   }
                        //   else {
                        //       swal("No Data Found!!!");
                        //       $scope.clearid();
                        //   }                       
                        //}
                        //else if ($scope.ttconstrep === "restrict") {

                        //    if (promise.restrict_day_period_list.length > 0) {
                        //        $scope.header_display = false;
                        //        $scope.name = "Restrict";
                        //        $scope.gridOptions.data = promise.restrict_day_period_list;
                        //        $scope.datafix = promise.restrict_day_period_list;
                        //        $scope.fix_res = false;
                        //        $scope.conscon = true;
                        //        $scope.bifcon = true;
                        //        $scope.labcon = true;
                        //    }
                        //    else {
                        //        swal("No Data Found!!!");
                        //        $scope.clearid();

                        //    }
                        //}
                        //else if ($scope.ttconstrep === "cons") {

                        //    if (promise.consecutivelst.length > 0) {
                        //        $scope.header_display = false;
                        //        $scope.name = "Consecutive";
                        //        $scope.gridOptions_cons.data = promise.consecutivelst;
                        //        $scope.data_con = promise.consecutivelst;
                        //        $scope.conscon = false;
                        //        $scope.fix_res = true;
                        //        $scope.bifcon = true;
                        //        $scope.labcon = true;
                        //    }
                        //    else {
                        //        swal("No Data Found!!!");
                        //        $scope.clearid();

                        //    }
                        //}
                        //else if ($scope.ttconstrep === "bif") {

                        //    if (promise.bif_detailslist.length > 0) {
                        //        $scope.header_display = false;
                        //        $scope.name = "Bifurcation";
                        //        $scope.gridOptions_bif.data = promise.bif_detailslist;
                        //        $scope.data_bifer = promise.bif_detailslist;
                        //        $scope.bifcon = false;
                        //        $scope.fix_res = true;
                        //        $scope.labcon = true;
                        //        $scope.conscon = true;
                        //    }
                        //    else {
                        //        swal("No Data Found!!!");
                        //        $scope.clearid();

                        //    }
                        //}
                        //else if ($scope.ttconstrep === "lab") {

                        //    if (promise.labdetailsarray.length > 0) {
                        //        $scope.header_display = false;
                        //        $scope.name = "Lab";
                        //        $scope.gridOptions_lab.data = promise.labdetailsarray;
                        //        $scope.data_lab = promise.labdetailsarray;
                        //        $scope.labcon = false;
                        //        $scope.fix_res = true;
                        //        $scope.conscon = true;
                        //        $scope.bifcon = true;
                        //    }
                        //    else {
                        //        swal("No Data Found!!!");
                        //        $scope.clearid();

                        //    }
                        //}
                        //else {
                        //    swal("No Record found....!!")
                        //    $scope.submitted = false;
                        //}

                    });
            }
        }


    }
})();