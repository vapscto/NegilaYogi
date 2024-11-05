
//dashboard.controller("ChairmanDashboardController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache','$window',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache,$window) {
   
    (function () {
        'use strict';
        angular
    .module('app')
            .controller('ManagerDashboardController', ManagerDashboardController)

        ManagerDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
        function ManagerDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache,$window) {

            $scope.sms = 0;
            $scope.email = 0;
            $scope.hidecoe = false;
            $scope.totalstudent = 0;
            $scope.totalcollected = 0;
            $scope.loadbasicdata = function () {
                $scope.Todaydate = new Date();
              
                apiService.getDATA("ChairmanDashboard/Getdetails").
              then(function (promise) {
                  
                  if (promise.smscount!=null) {
                      $scope.sms = promise.smscount.length;
                  }
                  if (promise.emailcount != null) {
                      $scope.email = promise.emailcount.length;
                  }
          
          
                  $scope.studentstrenth = promise.fillstudentstrenth;
                  $scope.yearlt = promise.yearlist;
                  // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
                  $scope.absentgraph = promise.fillabsent;
                  $scope.asmaY_Id = promise.asmaY_Id;
                  $scope.feedetailsgraph = promise.fillfee;
                  $scope.year= promise.yearlist[0].asmaY_Year;

                  $scope.coedata = promise.coedata;
                  if ($scope.coedata != "" && $scope.coedata != null) {
                      if ($scope.coedata.length > 0) {
                          $scope.hidecoe = true;
                      }
                  }
                  else {
                      $scope.hidecoe = false;
                  }
               
                  var total = 0;
          
                  if ($scope.studentstrenth.length > 0 ) {
              
                      for (var i = 0; i < $scope.studentstrenth.length; i++) {
                          total = total + $scope.studentstrenth[i].stud_count;
                      }
                  }
             
                  $scope.totalstudent = total;
            

                  // alert(total);

                  $scope.datagraph = [];
                  if ($scope.studentstrenth != null) {
             
                      for (var i = 0; i < $scope.studentstrenth.length; i++) {
                          $scope.datagraph.push({  label: $scope.studentstrenth[i].class_Name, "y": $scope.studentstrenth[i].stud_count })
                      }
                  }
                  console.log($scope.datagraph);

                  $scope.dataabsentgraph = [];
                  if ($scope.absentgraph != null) {

                      for (var i = 0; i < $scope.absentgraph.length; i++) {
                          $scope.dataabsentgraph.push({ label: $scope.absentgraph[i].nameOfDesig, "y": $scope.absentgraph[i].absentee })
                      }
                  }
                  console.log($scope.dataabsentgraph);



                  $scope.feegraphseries1 = [];
                  if ($scope.feedetailsgraph != null) {

                      for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                          $scope.feegraphseries1.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].paid })
                      }
                  }
                  console.log($scope.feegraphseries1);
                  var totalcollected = 0;
                  $scope.feegraphseries2 = [];
                  if ($scope.feedetailsgraph != null) {

                      for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                          totalcollected = totalcollected + $scope.feedetailsgraph[i].paid;
                          $scope.feegraphseries2.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].recived })
                      }
                  }
                  console.log($scope.feegraphseries2);
                  $scope.totalcollected = totalcollected;
                  $scope.feegraphseries3 = [];
                  if ($scope.feedetailsgraph != null) {

                      for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                          $scope.feegraphseries3.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].ballance })
                      }
                  }
                  console.log($scope.feegraphseries3);

        
                  //columnchart
                  var chart = new CanvasJS.Chart("columnchart", {

                      axisX: {
                          labelFontSize: 10,
                          interval: 1,
                          labelAngle: -20, 
                           labelFontColor: "black",
                          labelFontWeight: "bold", 
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
         
                  var chart = new CanvasJS.Chart("areachart",
                  {


                      
                      axisX: {
                          labelFontSize:8,
                          interval: 1,
                          labelAngle: -20,
                          labelFontColor: "black",
                          labelFontWeight: "bold",    
                         
                          //title: "Designation",
                      },
                      axisY: {
                          labelFontColor: "black",        
                          labelFontSize: 12,
                       
                          // title: "No.of. Staffs",

                      },

                      data: [
                    {
                        type: "column",
                        showInLegend: true,
                        dataPoints: $scope.dataabsentgraph
                    }
                      ]
                  });

                  chart.render();



                  var chart = new CanvasJS.Chart("rangeBarChat",
                      {
                          responsive: true,

                          axisX: {
                              labelFontSize: 9,
                              interval: 1,
                              //title: "Department",
                          },
                          axisY: {
                              labelFontSize: 9,
                              // title: "No.of. Staffs",

                          },

                          legend: {
                              maxWidth: 300,
                              fontSize: 10,
                          },
                          


                          data: [
                              {
                                  type: "pie",
                                  showInLegend: true,
                                  indexLabel: "{label}-{y}",
                                  indexLabelFontSize: 12,
                                  indexLabelFontWeight: "bold",
                                  toolTipContent: "{y} - #percent %",
                                  legendText: "{label}",
                                  dataPoints: $scope.feegraphseries1
                              }
                          ]
                      });

                  chart.render();


                  //var chart = new CanvasJS.Chart("rangeBarChat");

                  //chart.options.axisX = {
                  //    interval: 1, labelFontSize: 10, labelAngle: -20, labelFontColor: "black",
                  //    labelFontWeight: "bold"  };
                  //chart.options.axisY = {  labelFontSize: 12 };
                  //// chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                  //var series1 = { //dataSeries - first quarter
                  //    type: "column",
                  //    name: "Collected",
                  //    showInLegend: true
                  //};



                  //var series2 = { //dataSeries - second quarter
                  //    type: "column",
                  //    name: "Receivable",
                  //    showInLegend: true
                  //};

                  //var series3 = { //dataSeries - second quarter
                  //    type: "column",
                  //    name: "Balance",
                  //    showInLegend: true
                  //};


                  //chart.options.data = [];
                  //chart.options.data.push(series1);
                  //chart.options.data.push(series2);
                  //chart.options.data.push(series3);


                  //series1.dataPoints = $scope.feegraphseries1;

                  //series2.dataPoints = $scope.feegraphseries2;
                  //series3.dataPoints = $scope.feegraphseries3;

                  //chart.render();




              })

            }

   
   
   
            var HostName = location.host;
            $scope.showStudent = function () {
                
        
                $window.location.href = 'http://' + HostName + '/#/app/ADMClassSectionStrength';

            };

            //var HostName = location.host;
            $scope.showStudent1 = function () {
                

                $window.location.href = 'http://' + HostName + '/#/app/FEESOverAllStatusSchool';

            };

            var HostName = location.host;
            $scope.showStudent3 = function () {
                

                $window.location.href = 'http://' + HostName + '/#/app/FEESOverAllStatusSchool';

            };

            var HostName = location.host;
            $scope.showStudent4 = function () {
                

                $window.location.href = 'http://' + HostName + '/#/app/FEESGroupHeadWiseDetailsSchool';

            };

   





         


        };
    })();