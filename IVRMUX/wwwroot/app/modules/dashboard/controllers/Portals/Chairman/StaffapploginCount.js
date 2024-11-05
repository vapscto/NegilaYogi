     (function () {
         'use strict';
         angular
     .module('app')
             .controller('StaffapploginCountController', StaffapploginCountController)

         StaffapploginCountController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function StaffapploginCountController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


             $scope.showgrid = false;
             $scope.itemsPerPage = 15;
             $scope.currentPage = 1;
             $scope.booktype = 'C';
             $scope.searchValue = '';
    
             $scope.totalregstudent = 0;

             $scope.totalnewstudent = 0;
             $scope.sms = 0;
             $scope.email = 0;
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
             $scope.maxDatemf = new Date();
             //loading start
             $scope.loadbasicdata = function () {
                 $scope.fields();
                 $scope.cccc = 0;
                 $scope.rcccc = 0;
                 $scope.classfee = [];

                 $scope.FMCB_fromDATE = new Date();
                 $scope.FMCB_toDATE = new Date();
                 var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
                 var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");

                 var data = {
                     //  "fromdate": $scope.FMCB_fromDATE,
                     //  "todate": $scope.FMCB_toDATE,   
                     "fromdate": frmdate,
                     "todate": todate,
                     "eventName": $scope.booktype,

                 }
                 var config = {
                     headers: {
                         'Content-Type': 'application/json;'
                     }
                 }
                 apiService.create("ChairmanloginCount/getstaffappcount", data).
                     then(function (promise) {
                         $scope.totalyearfees = promise.fillgroupfee;
                         $scope.fillnewadmstd = promise.fillnewadmstd;
                         $scope.instarray = promise.instarray;
                         $scope.fillhead = promise.fillhead;

                         if ((promise.instarray !== null && promise.instarray.length>0)) {
                             $scope.showgrid = true;

                             angular.forEach($scope.instarray, function (ee) {
                                
                                 angular.forEach($scope.totalyearfees, function (xx) {
                                     if (ee.MI_Id === xx.MI_Id) {
                                         ee.LCNT = xx.CNT;
                                     }
                                 });
                                 
                             });


                             angular.forEach($scope.instarray, function (ee) {

                                 angular.forEach($scope.fillnewadmstd, function (xx) {
                                     if (ee.MI_Id === xx.MI_Id) {
                                         ee.DCNT = xx.CNT;
                                     }
                                 });

                             });
                             angular.forEach($scope.instarray, function (ee) {

                                 angular.forEach($scope.fillhead, function (xx) {
                                     if (ee.MI_Id === xx.MI_Id) {
                                         ee.RCNT = xx.CNT;
                                     }
                                 });

                             });

                             $scope.loadcharts();
                             console.log($scope.instarray);
                         }
                         else {
                             swal('NO RECORD FOUND');
                         }
                         
                     });

             };

             $scope.showgroupGrid = function (classid, tp) {
                 $scope.currentPage = 1;
                 $scope.type1 = tp;
        $scope.sectionarray = [];
        $scope.sectionpop = [];
        var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
        var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
        var data = {
            "fromdate": frmdate,
            "todate": todate,
            "MI_Id": classid,
            "type": tp

        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ChairmanloginCount/GetpopupDetails", data).
      then(function (promise) {


          if (promise.fillgroupfee !== null && promise.fillgroupfee.length>0) {
              $scope.sectionarray = promise.fillgroupfee;
              $('#myModal3').modal('show');
          }
          else {
              swal('NO RECORD FOUND');
          }
          

      })
    
}

    $scope.gettodate = function () {
        
        $scope.minDatemf = new Date($scope.fromdate);
        $scope.maxDatemf = new Date();
    };
   
             $scope.loadcharts = function () {
                 var total = 0;
                 var total1 = 0;

                 $scope.totalregstudent = total;
                 $scope.totalnewstudent = total1;

                 $scope.datagraph = [];
                 if ($scope.instarray !== null) {

                     for (var i = 0; i < $scope.instarray.length; i++) {
                         $scope.datagraph.push({ label: $scope.instarray[i].MI_Name, "y": $scope.instarray[i].LCNT })
                     }
                 }
                 $scope.datagraph1 = [];
                 if ($scope.instarray !== null) {

                     for (var j = 0; j < $scope.instarray.length; j++) {
                         $scope.datagraph1.push({ label: $scope.instarray[j].MI_Name, "y": $scope.instarray[j].DCNT });
                     }
                 }

                 var chart = new CanvasJS.Chart("rangeBarChat", {

                     height: 340,
                     axisX: {
                         labelFontSize: 12,
                         interval: 1,
                         labelWrap: true
                         // title:"Class",
                     },
                     axisY: {
                         labelFontSize: 12,
                          
                         //  title: "Students",
                     },

                     data: [
                         {
                             type: "column",
                             showInLegend: true,
                             dataPoints: $scope.datagraph

                         }
                     ]

                 });

                 chart.render();


                 var chart1 = new CanvasJS.Chart("chartContainer", {

                     height: 340,
                     axisX: {
                         labelFontSize: 12,
                         interval: 1
                         // title:"Class",
                     },
                     axisY: {
                         labelFontSize: 12
                         //  title: "Students",
                     },

                     data: [
                         {
                             type: "column",
                             showInLegend: true,
                             dataPoints: $scope.datagraph1

                         }
                     ]

                 });

                 chart1.render();

             };


             $scope.cccc = 0;
             $scope.rcccc = 0;

             $scope.onfromdatechange = function () {
                 $scope.cccc = 0;
                 $scope.rcccc = 0;

                 $scope.showgrid = false;
                 $scope.submitted = true;
                 if ($scope.myForm.$valid) {
                     $scope.fields();
                     var frmdate = $scope.FMCB_fromDATE === null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
                     var todate = $scope.FMCB_toDATE === null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
                     var data = {
                         "fromdate": frmdate,
                         "todate": todate,
                         "eventName": $scope.booktype

                     };
                     var config = {
                         headers: {
                             'Content-Type': 'application/json;'
                         }
                     };

                     apiService.create("ChairmanloginCount/onstaffappchange", data).
                         then(function (promise) {

                             $scope.totalyearfees = promise.fillgroupfee;
                             $scope.fillnewadmstd = promise.fillnewadmstd;
                             $scope.instarray = promise.instarray;
                             $scope.fillhead = promise.fillhead;

                             if ((promise.instarray !== null && promise.instarray.length > 0)) {
                                 $scope.showgrid = true;

                                 angular.forEach($scope.instarray, function (ee) {

                                     angular.forEach($scope.totalyearfees, function (xx) {
                                         if (ee.MI_Id === xx.MI_Id) {
                                             ee.LCNT = xx.CNT;
                                         }
                                     });

                                 });


                                 angular.forEach($scope.instarray, function (ee) {

                                     angular.forEach($scope.fillnewadmstd, function (xx) {
                                         if (ee.MI_Id === xx.MI_Id) {
                                             ee.DCNT = xx.CNT
                                         }
                                     });

                                 });

                                 angular.forEach($scope.instarray, function (ee) {

                                     angular.forEach($scope.fillhead, function (xx) {
                                         if (ee.MI_Id === xx.MI_Id) {
                                             ee.RCNT = xx.CNT;
                                         }
                                     });

                                 });

                                 $scope.loadcharts();
                             }
                             else {
                                 swal('NO RECORD FOUND');
                             }

                         });
                 } else {
                     $scope.submitted = true;

                 }

             };



   
         };
     })();