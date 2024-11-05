
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('FEESTodayCollectionController', FEESTodayCollectionController)

         FEESTodayCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function FEESTodayCollectionController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


             $scope.showgrid = false;
             $scope.booktype = 'C';
    
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
        apiService.create("FEESTodayCollection/Getdetails",data).
      then(function (promise) {
          
         
          
          $scope.groupclass = promise.fillgroupfee;
          if ($scope.booktype=='C') {
              $scope.fillhead = promise.fillhead;
              if ($scope.groupclass != null) {
                  $scope.loadcharts();

                  $scope.showgrid = true;
              }
              else {
                  swal("No Record Found");
              }
          }
          else {
              if ($scope.groupclass != null) {
                  $scope.showgrid = true;
                  angular.forEach($scope.groupclass, function (ff) {

                      $scope.cccc = $scope.cccc + ff.amount;
                      $scope.rcccc = $scope.rcccc + ff.recept;



                  })
              }
              else {
                  swal("No Record Found");
              }
          }

         
      
        





      })

    }

    $scope.showgroupGrid = function (classid) {
        $scope.sectionpop = [];
        var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
        var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
        var data = {
            "fromdate": frmdate,
            "todate": todate,
            "classid": classid,

        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("FEESTodayCollection/Getsectionpop", data).
      then(function (promise) {

          

          $scope.sectionarray = promise.sectionarray;





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
        if ($scope.groupclass != null) {

            for (var i = 0; i < $scope.groupclass.length; i++) {
                $scope.datagraph.push({ label: $scope.groupclass[i].class_Name, "y": $scope.groupclass[i].paid })
            }
        }
        


      




       
        var chart = new CanvasJS.Chart("rangeBarChat", {
            width: 1060,
            height:340,
            axisX: {
                labelFontSize: 12,
                interval: 1,
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

     

      


      

    }


             $scope.cccc = 0;
             $scope.rcccc = 0;

    $scope.onfromdatechange = function () {
        $scope.cccc = 0;
        $scope.rcccc = 0;
     
        $scope.showgrid=false;
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
            $scope.fields();
            var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
            var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
            var data = {
                "fromdate": frmdate,
                "todate": todate,
                "eventName": $scope.booktype,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FEESTodayCollection/ondatechange", data).
          then(function (promise) {
              
              $scope.groupclass = promise.fillgroupfee;

              if ($scope.booktype=='C') {
                  if ($scope.groupclass != null) {
                      $scope.showgrid = true;
                  }
                  else {
                      swal("No Record Found");
                  }




                  $scope.loadcharts();
              }
              else {
                  if ($scope.groupclass != null) {
                      $scope.showgrid = true;

                      angular.forEach($scope.groupclass, function (ff) {

                          $scope.cccc = $scope.cccc + ff.amount;
                          $scope.rcccc = $scope.rcccc + ff.recept;



                      })

                  }
                  else {
                      swal("No Record Found");
                  }
              }
            
          })
        } else {
            $scope.submitted = true;

        }

    }



   
         };
     })();