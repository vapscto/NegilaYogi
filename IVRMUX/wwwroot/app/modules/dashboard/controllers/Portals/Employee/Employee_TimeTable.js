(function () {
    'use strict';
    angular
.module('app')
.controller('Employee_TimetableController', Employee_TimetableController)

    Employee_TimetableController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    //dashboard.controller("EmployeeDashboardController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache',
    function Employee_TimetableController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {



        $scope.LoadData = function () {
            
            var TT_list = [];
            
            $scope.showweekly = false;
            $scope.showday_d = true;
            apiService.getDATA("Employee_Timetable/getalldetails")
                .then(function (promise) {
                    
                    $scope.tt_final = promise.tT_final_generation;
                    $scope.AllPeriod = promise.allperiods;
                  
                    $scope.showdaily = false;
                    $scope.showweekly = false;
                    //for weekly
                    if ($scope.tt_final != null && $scope.AllPeriod != null) {
                        for (var i = 0; i < $scope.AllPeriod.length; i++) {
                            for (var j = 0; j < $scope.tt_final.length; j++) {
                                if ($scope.tt_final[j].dayName == $scope.AllPeriod[i].ttmD_DayName) {
                                    TT_list.push({ label: $scope.AllPeriod[i].ttmD_DayName, "y": $scope.tt_final[j].periodCount })
                                } 

                            }
                        }
                    }
                   
                    var chart = new CanvasJS.Chart("columnchart", {
                        height: 260,
                        width: 320,
                        axisX: {
                            labelFontSize: 12,
                        },
                        axisY: {
                            labelFontSize: 12,
                        },

                        data: [
                        {
                            type: "pie",
                            showInLegend: true,
                            dataPoints: TT_list
                        }
                        ]
                    });

                    chart.render();

                  
               
                   
                    //for weekly
                    console.log(promise.getdata);
                    $scope.ClassSection = promise.class_sectons;
                    $scope.Period = promise.periods;

                    //var breakname = { ttmP_PeriodName: "Break Period" };
                    //var afterperiod = $scope.ClassSection[0].ttmB_AfterPeriod;
                    //var index = afterperiod;
                    //$scope.Period.splice(index, 0, breakname);

                      


                })

            $scope.changeradio = function (abcc) {

                if (abcc == 'daily') {
                    $scope.showdaily = false;
                    $scope.showweekly = false;
                    $scope.showday_d = true;
                }
                else if (abcc == 'weekly') {
                    if ($scope.Period.length > 0 && $scope.Period !== null) {
                        $scope.showweekly = true;
                        $scope.showdaily = false;
                        $scope.showday_d = false;
                    }
                    else {
                        $scope.showweekly = false;
                        $scope.showdaily = false;
                        $scope.showday_d = false;
                        swal("No Record Found")
                    }
                   
                }
            }

            $scope.onselectgroup = function (data) {
                
                var TT_list_d = [];
               
                $scope.showday_d = true;
                apiService.getURI("Employee_Timetable/getdaily_data", data).
           then(function (promise) {

               var amountentryarray = [];

               //$scope.tt_final = promise.tT_final_generation;
               $scope.ClassS = promise.class_sectons;
               $scope.showdaily = true;
               $scope.showweekly = false;
              
               //for daily
               if ($scope.ClassS != null) {
                   $scope.showdaily = true;
                   $scope.showweekly = false;
                   for (var i = 0; i < $scope.ClassS.length; i++) {
                       TT_list_d.push({ label: $scope.ClassS[i].asmcL_ClassName + "-" + $scope.ClassS[i].asmC_SectionName, "y": parseInt($scope.ClassS[i].period) })
                           
                   }
               }
              
               var chart = new CanvasJS.Chart("columnchart_D", {
                   height: 260,
                   width: 320,
                   axisX: {
                       labelFontSize: 12,
                   },
                   axisY: {
                       labelFontSize: 12,
                   },

                   data: [
                   {
                       type: "pie",
                       showInLegend: true,
                       dataPoints: TT_list_d
                   }
                   ]
               });

               chart.render();
              
           })

            };


        };

   

    };
})();