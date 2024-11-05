
   
    (function () {
        'use strict';
        angular
    .module('app')
            .controller('RFIDDashboardController', RFIDDashboardController)

        RFIDDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
        function RFIDDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache,$window) {

            $scope.searchValue = "";

            $scope.itemsPerPage1 = 1;
            $scope.currentPage1 = 1;
            var paginationformasters;
            var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            }
            $scope.masterlist = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = "";
            if ($scope.itemsPerPage == undefined) {
                $scope.itemsPerPage = 15
            }
            $scope.Todaydate = new Date();
            $scope.tcnt = 0;
            $scope.pcnt = 0;
            $scope.acnt = 0;
            $scope.hidecoe = false;
            $scope.totalstudent = 0;
            $scope.totalcollected = 0;
            $scope.studentlist = [];

            $scope.studentlist = [];
            $scope.totalstudentlist = [];
            $scope.presentstudentlist = [];


            $scope.studentlist = [];


            $scope.loadbasicdata = function () {
                $scope.studentlist = [];
                var data = {
                    "adate": $scope.Todaydate,
                }
              
                apiService.create("RFIDDashboard/Getdetails", data).
                    then(function (promise) {


                        $scope.studentlist = promise.studentlist;
                        $scope.totalstudentlist = promise.totalstudentlist;
                        $scope.presentstudentlist = promise.presentstudentlist;

                        if ($scope.totalstudentlist.length> 0) {
                            $scope.tcnt = $scope.totalstudentlist[0].STDCOUNT;
                           
                        }
                        else {
                            $scope.tcnt = 0;
                        }
                        if ($scope.presentstudentlist.length > 0) {
                            $scope.pcnt = $scope.presentstudentlist[0].PCOUNT;

                        }
                        else {
                            $scope.pcnt = 0;
                        }


                        $scope.acnt = $scope.tcnt - $scope.pcnt;

                  
                  //if (promise.smscount!=null) {
                  //    $scope.sms = promise.smscount.length;
                  //}
                  //if (promise.emailcount != null) {
                  //    $scope.email = promise.emailcount.length;
                  //}
          
          
                  //$scope.studentstrenth = promise.fillstudentstrenth;
                  //$scope.yearlt = promise.yearlist;
                  //// $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
                  //$scope.absentgraph = promise.fillabsent;
                  //$scope.asmaY_Id = promise.asmaY_Id;
                  //$scope.feedetailsgraph = promise.fillfee;
                  //$scope.year= promise.yearlist[0].asmaY_Year;

                  //$scope.coedata = promise.coedata;
                  //if ($scope.coedata != "" && $scope.coedata != null) {
                  //    if ($scope.coedata.length > 0) {
                  //        $scope.hidecoe = true;
                  //    }
                  //}
                  //else {
                  //    $scope.hidecoe = false;
                  //}
               
                  //var total = 0;
          
                  //if ($scope.studentstrenth.length > 0 ) {
              
                  //    for (var i = 0; i < $scope.studentstrenth.length; i++) {
                  //        total = total + $scope.studentstrenth[i].stud_count;
                  //    }
                  //}
             
                  //$scope.totalstudent = total;
            

                  //// alert(total);

                  //$scope.datagraph = [];
                  //if ($scope.studentstrenth != null) {
             
                  //    for (var i = 0; i < $scope.studentstrenth.length; i++) {
                  //        $scope.datagraph.push({  label: $scope.studentstrenth[i].class_Name, "y": $scope.studentstrenth[i].stud_count })
                  //    }
                  //}
                  //console.log($scope.datagraph);

                  //$scope.dataabsentgraph = [];
                  //if ($scope.absentgraph != null) {

                  //    for (var i = 0; i < $scope.absentgraph.length; i++) {
                  //        $scope.dataabsentgraph.push({ label: $scope.absentgraph[i].nameOfDesig, "y": $scope.absentgraph[i].absentee })
                  //    }
                  //}
                  //console.log($scope.dataabsentgraph);



                  //$scope.feegraphseries1 = [];
                  //if ($scope.feedetailsgraph != null) {

                  //    for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                  //        $scope.feegraphseries1.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].paid })
                  //    }
                  //}
                  //console.log($scope.feegraphseries1);
                  //var totalcollected = 0;
                  //$scope.feegraphseries2 = [];
                  //if ($scope.feedetailsgraph != null) {

                  //    for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                  //        totalcollected = totalcollected + $scope.feedetailsgraph[i].paid;
                  //        $scope.feegraphseries2.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].recived })
                  //    }
                  //}
                  //console.log($scope.feegraphseries2);
                  //$scope.totalcollected = totalcollected;
                  //$scope.feegraphseries3 = [];
                  //if ($scope.feedetailsgraph != null) {

                  //    for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                  //        $scope.feegraphseries3.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].ballance })
                  //    }
                  //}
                  //console.log($scope.feegraphseries3);

        
                  ////columnchart
                  //var chart = new CanvasJS.Chart("columnchart", {

                  //    axisX: {
                  //        labelFontSize: 10,
                  //        interval: 1,
                  //        labelAngle: -20, 
                  //         labelFontColor: "black",
                  //        labelFontWeight: "bold", 
                  //        // title:"Class",
                  //    },
                  //    axisY: {
                  //        labelFontSize: 12,
                  //        //  title: "Students",
                  //    },
              
                  //    data: [
                  //    {
                  //        type: "column",
                  //        showInLegend: true,
                  //        dataPoints: $scope.datagraph
                
                  //    }
                  //    ]
               
                  //});

                  //chart.render();
         
                  //var chart = new CanvasJS.Chart("areachart",
                  //{


                      
                  //    axisX: {
                  //        labelFontSize:8,
                  //        interval: 1,
                  //        labelAngle: -20,
                  //        labelFontColor: "black",
                  //        labelFontWeight: "bold",    
                         
                  //        //title: "Designation",
                  //    },
                  //    axisY: {
                  //        labelFontColor: "black",        
                  //        labelFontSize: 12,
                       
                  //        // title: "No.of. Staffs",

                  //    },

                  //    data: [
                  //  {
                  //      type: "column",
                  //      showInLegend: true,
                  //      dataPoints: $scope.dataabsentgraph
                  //  }
                  //    ]
                  //});

                  //chart.render();



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

            $scope.showstudentGrid = function (obj) {
                //var data = {
                //    "adate": $scope.Todaydate,
                //}

                apiService.create("RFIDDashboard/showstudentGrid", obj).
                    then(function (promise) {


                        $scope.inlist = promise.inlist;
                        $scope.outlist = promise.outlist;


                    })


            }
            $scope.cleardata = function () {
                $scope.studentlist = [];
                var data = {
                    "adate": $scope.Todaydate,
                }

                apiService.create("RFIDDashboard/cleardata", data).
                    then(function (promise) {


                        if (promise.returnval == true) {
                            swal('Records Deleted Successfully');
                        }
                        else {
                            swal('Records Not Deleted');
                        }
                        $state.reload();

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