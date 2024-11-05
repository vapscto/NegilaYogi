     (function () {
         'use strict';
         angular
     .module('app')
             .controller('CLGCHSubjectDetailsController', CLGCHSubjectDetailsController)

         CLGCHSubjectDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window','uiGridGroupingConstants']
         function CLGCHSubjectDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants) {

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
        
                 apiService.getDATA("CLGCHSubjectDetails/Getdetails").
      then(function (promise) {
          $scope.yearlt = promise.yearlist;
          $scope.courselist = promise.courselist;
          $scope.asmaY_Id = promise.asmaY_Id;
          $scope.AMCO_Id = promise.amcO_Id;
          $scope.studentstrenth = promise.fillstudentstrenth;

          console.log($scope.studentstrenth);
          $scope.gridOptions.data = $scope.studentstrenth;


          if ($scope.studentstrenth.length > 0) {
              $scope.studentstrenthgr = true;

             
             
              $scope.regular = promise.sectionwisestrenth;

         


             
              $scope.getgraphdata();

              $scope.loadcharts();

          }
          else {
              swal("No Record Found")
          }
       



      })

             }


             $scope.gridOptions = {
                 showGridFooter: true,
                 showColumnFooter: true,
                 enableFiltering: true,
                 enableGridMenu: false,
                 enableColumnMenus: false,
                 treeRowHeaderAlwaysVisible: false,
                 columnDefs: [
                     { name: 'AMB_BranchName', displayName: 'BRANCH', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, width: '18%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                     { name: 'AMSE_SEMName', displayName: 'SEM', grouping: { groupPriority: 2 }, sort: { priority: 2, direction: 'asc' }, width: '10%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                     { name: 'ACMS_SectionName', displayName: 'SECTION', grouping: { groupPriority: 3 }, sort: { priority: 3, direction: 'asc' }, width: '6%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                     { name: 'EME_ExamName', displayName: 'EXAM', grouping: { groupPriority: 4 }, sort: { priority: 4, direction: 'asc' }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                     { name: 'ISMS_SubjectName', displayName: 'SUBJECT', grouping: { groupPriority: 5 }, sort: { priority: 5, direction: 'asc' }, width: '22%' },
                     {
                         name: 'total', displayName: 'Total', width: '6%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                             aggregation.rendered = aggregation.value;
                         }
                     },
                     {
                         name: 'Pass', displayName: 'Pass', width: '6%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                             aggregation.rendered = aggregation.value;
                         }
                     },
                     {
                         name: 'Fail', displayName: 'Fail', width: '6%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                             aggregation.rendered = aggregation.value;
                         }
                     },
                 ],
                 exporterMenuPdf: false,

                 onRegisterApi: function (gridApi) {
                     $scope.gridApi = gridApi;
                 },
                 gridMenuCustomItems: [{
                     title: 'class year status report',
                     action: function ($event) {
                         exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                     },
                     order: 110
                 },
                 {
                     title: 'Export visible data as EXCEL',
                     action: function ($event) {
                         exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                     },
                     order: 111
                 }
                 ]
             };

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
                 apiService.create("CLGCHSubjectDetails/Getsectioncount", data).
               then(function (promise) {

                   
                   
                   $scope.fillsectioncount = promise.fillsectioncount;
                  




               })

             }
   
             $scope.loadcharts = function () {
                 var total = 0;
                 var total1 = 0;


                 $scope.feegraphseries1 = [];
                 if ($scope.grapharray != null) {

                     for (var i = 0; i < $scope.grapharray.length; i++) {
                         $scope.feegraphseries1.push({ label: $scope.grapharray[i].AMB_BranchName, "y": $scope.grapharray[i].totalcnt })
                     }
                 }

                 var totalcollected = 0;
                 $scope.feegraphseries2 = [];
                 if ($scope.grapharray != null) {

                     for (var i = 0; i < $scope.grapharray.length; i++) {
                         totalcollected = totalcollected + $scope.grapharray[i].paid;
                         $scope.feegraphseries2.push({ label: $scope.grapharray[i].AMB_BranchName, "y": $scope.grapharray[i].passcnt })
                     }
                 }

                 $scope.totalcollected = totalcollected;
                 $scope.feegraphseries3 = [];
                 if ($scope.grapharray != null) {

                     for (var i = 0; i < $scope.grapharray.length; i++) {
                         $scope.feegraphseries3.push({ label: $scope.grapharray[i].AMB_BranchName, "y": $scope.grapharray[i].failcnt })
                     }
                 }







                 var chart = new CanvasJS.Chart("rangeBarChat");
                 chart.options.width = 1060;
                 chart.options.height = 350;
                 chart.options.axisX = {
                     interval: 1, labelFontSize: 10, labelAngle: -20, labelFontColor: "black",
                     labelFontWeight: "bold"
                 };
                 chart.options.axisY = { labelFontSize: 12 };
                 // chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                 var series1 = { //dataSeries - first quarter
                     type: "column",
                     name: "TOTAL",
                     color: "Gray",
                     showInLegend: true
                 };



                 var series2 = { //dataSeries - second quarter
                     type: "column",
                     name: "PASS",
                     color: "Green",
                     showInLegend: true
                 };

                 var series3 = { //dataSeries - second quarter
                     type: "column",
                     name: "FAIL",
                     color:"red",
                     showInLegend: true
                 };


                 chart.options.data = [];
                 chart.options.data.push(series1);
                 chart.options.data.push(series2);
                 chart.options.data.push(series3);

                 series1.dataPoints = $scope.feegraphseries1;
                 series2.dataPoints = $scope.feegraphseries2;
                 series3.dataPoints = $scope.feegraphseries3;

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
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("CLGCHSubjectDetails/yrchange", data).
      then(function (promise) {
          $scope.studentstrenth = promise.fillstudentstrenth;
          $scope.gridOptions.data = $scope.studentstrenth;

          if ($scope.studentstrenth.length > 0 ) {
    
        
          $scope.studentstrenthgr = true;
        
          // $scope.yearlt = promise.yearlist;
          $scope.regular = promise.sectionwisestrenth;

          $scope.asmaY_Id = promise.asmaY_Id;

              $scope.getgraphdata();

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

             $scope.getgraphdata = function () {


                 //Graph Data

                 $scope.exam_list = [];
                 $scope.overalltotalmax = 0;
                 angular.forEach($scope.studentstrenth, function (st) {
                     if ($scope.exam_list.length == 0) {
                         $scope.exam_list.push({AMB_Id: st.AMB_Id, AMB_BranchName: st.AMB_BranchName });
                     }
                     else if ($scope.exam_list.length > 0) {
                         var al_exm_cnt = 0;
                         angular.forEach($scope.exam_list, function (exm) {
                             if (exm.AMCO_Id == st.AMCO_Id && exm.AMB_Id == st.AMB_Id) {
                                 al_exm_cnt += 1;
                             }
                         })
                         if (al_exm_cnt == 0) {

                             $scope.exam_list.push({  AMB_Id: st.AMB_Id, AMB_BranchName: st.AMB_BranchName });
                         }
                     }
                 })


                 $scope.grapharray = [];
                 angular.forEach($scope.exam_list, function (kk) {
                     $scope.totapasscnt = 0;
                     $scope.totalfailcnt = 0;
                     $scope.totalstdcnt = 0;
                     angular.forEach($scope.studentstrenth, function (xx) {
                         if (kk.AMCO_Id == xx.AMCO_Id && kk.AMB_Id == xx.AMB_Id) {
                             $scope.totapasscnt += xx.Pass;
                             $scope.totalfailcnt += xx.Fail;
                             $scope.totalstdcnt += xx.total;
                         }

                     })
                     $scope.grapharray.push({ AMB_Id: kk.AMB_Id, AMB_BranchName: kk.AMB_BranchName, totalcnt: $scope.totalstdcnt, passcnt: $scope.totapasscnt, failcnt: $scope.totalfailcnt })
                 })


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
        apiService.create("CLGCHSubjectDetails/Getsection", data).
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