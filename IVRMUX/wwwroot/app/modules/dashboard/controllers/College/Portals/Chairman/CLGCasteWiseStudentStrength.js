
     (function () {
         'use strict';
         angular
     .module('app')
             .controller('CLGCasteWiseStudentStrengthController', CLGCasteWiseStudentStrengthController)

         CLGCasteWiseStudentStrengthController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window','uiGridGroupingConstants']
         function CLGCasteWiseStudentStrengthController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants) {
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
        
                 apiService.getDATA("CLGCasteWiseStudentStrength/Getdetails").
      then(function (promise) {
          
          $scope.studentstrenth = promise.fillstudentstrenth;

          console.log($scope.studentstrenth);
          $scope.gridOptions.data = $scope.studentstrenth;


          if ($scope.studentstrenth.length > 0) {
              $scope.studentstrenthgr = true;

             
              $scope.yearlt = promise.yearlist;
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


             $scope.gridOptions = {
                 showGridFooter: true,
                 showColumnFooter: true,
                 enableFiltering: true,
                 enableGridMenu: false,
                 enableColumnMenus: false,
                 treeRowHeaderAlwaysVisible: false,
                 columnDefs: [
                     { name: 'IMCC_CategoryName', displayName: 'Category Name', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                     { name: 'IMC_CasteName', displayName: 'Caste Name', grouping: { groupPriority:1 }, sort: { priority: 1, direction: 'asc' }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                     { name: 'AMCO_CourseName', displayName: 'Course Name', grouping: { groupPriority: 2 }, sort: { priority: 2, direction: 'asc' }, width: '11%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                     { name: 'AMB_BranchName', displayName: 'Branch Name', grouping: { groupPriority: 3 }, sort: { priority: 3, direction: 'asc' }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                     { name: 'AMSE_SEMName', displayName: 'Semester', grouping: { groupPriority: 4 }, sort: { priority: 4, direction: 'asc' }, width: '10%' },
                     {
                         name: 'stud_count', displayName: 'Student count', width: '10%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
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
                 apiService.create("CLGCasteWiseStudentStrength/Getsectioncount", data).
               then(function (promise) {

                   
                   
                   $scope.fillsectioncount = promise.fillsectioncount;
                  




               })

             }
   
    $scope.loadcharts = function () {
        var total = 0;
        var total1 = 0;
        

        if ($scope.grapharray != null) {

            for (var i = 0; i < $scope.grapharray.length; i++) {
                $scope.regularstdtotal.push({ label: $scope.grapharray[i].IMCC_CategoryName, "y": $scope.grapharray[i].stud_count })
            }
        }

        
      


        var chart = new CanvasJS.Chart("areachart",
        {
            width: 1070,
            height:348,
            axisX: {
                labelFontSize:10,
                interval: 1,
                labelFontColor: "black",
                labelAngle: -20 
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

            apiService.getURI("CLGCasteWiseStudentStrength/yrchange", asmaY_Id).
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
                         $scope.exam_list.push({ IMCC_Id: st.IMCC_Id, IMCC_CategoryName: st.IMCC_CategoryName });
                     }
                     else if ($scope.exam_list.length > 0) {
                         var al_exm_cnt = 0;
                         angular.forEach($scope.exam_list, function (exm) {
                             if (exm.IMCC_Id == st.IMCC_Id) {
                                 al_exm_cnt += 1;
                             }
                         })
                         if (al_exm_cnt == 0) {

                             $scope.exam_list.push({ IMCC_Id: st.IMCC_Id, IMCC_CategoryName: st.IMCC_CategoryName });
                         }
                     }
                 })


                 $scope.grapharray = [];
                 angular.forEach($scope.exam_list, function (kk) {
                     $scope.branchcnt = 0;
                     angular.forEach($scope.studentstrenth, function (xx) {
                         if (kk.IMCC_Id == xx.IMCC_Id) {
                             $scope.branchcnt += xx.stud_count;
                         }

                     })
                     $scope.grapharray.push({ IMCC_Id: kk.IMCC_Id, IMCC_CategoryName: kk.IMCC_CategoryName, stud_count: $scope.branchcnt })
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
        apiService.create("ADMClassSectionStrength/Getsection", data).
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