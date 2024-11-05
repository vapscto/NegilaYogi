(function () {
    'use strict';
    angular
.module('app')
.controller('TimeTableController', TimeTableController)

    TimeTableController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    //dashboard.controller("EmployeeDashboardController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache',
    function TimeTableController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {

        $scope.LoadData = function () {
            
            var TT_list = [];

            $scope.showweekly = false;
            $scope.showEmplo = true;
            $scope.showday_d = true;


            var week = {
                "HRME_Id": $scope.HRME_Id,                
            }
            apiService.create("TimeTable/getalldetails",week)
                .then(function (promise) {
                    
                    $scope.AllEmployee = promise.stafflist;
                    $scope.tt_final = promise.tT_final_generation;
                    $scope.AllPeriod = promise.allperiods;
                    //$scope.showdaily = true;
                    //$scope.showweekly = false;
                    //for weekly

                    if ($scope.AllPeriod.length == "0") {
                        swal("No record found...")
                    }

                    if ($scope.tt_final != null && $scope.AllPeriod != null) {
                        for (var i = 0; i < $scope.AllPeriod.length; i++) {
                            for (var j = 0; j < $scope.tt_final.length; j++) {
                                if ($scope.tt_final[j].dayName == $scope.AllPeriod[i].ttmD_DayCode) {
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
                            labelAngle: -20 
                        },
                        axisY: {
                            labelFontSize: 12,
                        },

                        data: [
                        {
                            type: "column",
                            showInLegend: true,
                            dataPoints: TT_list
                        }
                        ]
                    });

                    chart.render();

                    //for weekly

                    //console.log(promise.getdata);
                    $scope.ClassSection = promise.class_sectons;
                    $scope.Period = promise.periods;
                    $scope.presentCountgrid = $scope.ClassSection.length;

                })
        };
        $scope.onselectgroup = function () {
            
            var TT_list_d = [];
            var TT_list = [];
            if ($scope.stuchk == "daily") {
                //$scope.showdaily = true;
                //$scope.showweekly = false;
                //$scope.showEmplo = true;
                //$scope.showday_d = true;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                if ($scope.HRME_Id != undefined && $scope.HRME_Id != "" && $scope.TTMD_Id != undefined && $scope.TTMD_Id != "")
                {
                    var data = {
                        "type":$scope.stuchk,
                        "HRME_Id": $scope.HRME_Id,
                        "TTMD_Id": $scope.TTMD_Id
                    }
                    apiService.create("TimeTable/getdaily_data", data).
               then(function (promise) {
                   
                   var amountentryarray = [];            
                   $scope.ClassS = promise.class_sectons;             
                   if ($scope.ClassS.length == "0") {
                       swal("No record found...")
                       $scope.showdaily = false;
                       $scope.showweekly = false;
                   }                 
                   //for daily
                   if ($scope.ClassS != null) {
                       $scope.showdaily = true;
                       $scope.showweekly = false;
                       $scope.showEmplo = true;
                       $scope.showday_d = true;

                       for (var i = 0; i < $scope.ClassS.length; i++) {
                           TT_list_d.push({ label: $scope.ClassS[i].asmcL_ClassName + "-" + $scope.ClassS[i].asmC_SectionName, "y": parseInt($scope.ClassS[i].period) })
                       }
                   }

                   var chart = new CanvasJS.Chart("columnchart_D", {
                       height: 260,
                       width: 320,
                       axisX: {
                           labelFontSize: 12,
                           labelAngle: -20 
                       },
                       axisY: {
                           labelFontSize: 12,
                       },

                       data: [
                       {
                           type: "column",
                           showInLegend: true,
                           dataPoints: TT_list_d
                       }
                       ]
                   });
                   chart.render();
               })
                } else {
                    TT_list_d = [];
                    $scope.showdaily = false;
                    $scope.showweekly = false;
                }
            }
            else if ($scope.stuchk == "weekly") {
               
                $scope.showdaily = false;
                $scope.showweekly = true;
                $scope.showEmplo = true;
                $scope.showday_d = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                if ($scope.HRME_Id != undefined && $scope.HRME_Id != "") {
                    var data = {
                        "type": $scope.stuchk,
                        "HRME_Id": $scope.HRME_Id,
                        // "TTMD_Id": $scope.TTMD_Id
                    }
                    apiService.create("TimeTable/getdaily_data", data).
               then(function (promise) {
                   
                   if (promise.tt != null && promise.tt != "" && promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "") {
                       $scope.table_list_sub_wise = [];
                           $scope.grid_view = true;
                           $scope.period_list = promise.periodslst;
                           $scope.day_list = promise.gridweeks;
                           $scope.tt_list = promise.tt;
                           var temp_array = [];
                           $scope.table_list = [];

                           var chart_list = [];
                           for (var j = 0; j < $scope.day_list.length; j++) {
                               temp_array = [];
                               for (var i = 0; i < $scope.period_list.length; i++) {
                                   var count = 0;
                                   var newCol = "";
                                   for (var k = 0; k < $scope.tt_list.length; k++) {

                                       if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id ) {
                                           
                                           if (count == 0) {
                                               newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value_: $scope.tt_list[k].ismS_SubjectName }
                                               count += 1;
                                           }
                                           else if (count > 0) {
                                               newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                               newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                               newCol.value2 = newCol.value2 + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
                                               count += 1;
                                           }
                                       }

                                   }
                                   if (newCol == "") {
                                       newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                                   }
                                   temp_array.push(newCol);
                                   newCol = "";
                                   count = 0;
                               }
                               var ped_count = 0;
                               angular.forEach(temp_array, function (uy) {
                                   if(uy.value!=" ")
                                   {
                                       ped_count += 1;
                                   }
                               })

                               chart_list.push({ label: $scope.day_list[j].ttmD_DayName ,y:ped_count})
                               $scope.table_list.push(temp_array);
                           }
                          // $scope.table_list_sub_wise.push({ array: $scope.table_list, header: $scope.albumNameArray1[a].staffName, id: $scope.albumNameArray1[a].hrmE_Id });
                           // $scope.table_headers.push($scope.table_list);
                           console.log($scope.table_list);
                           console.log($scope.tt_list);
                           console.log(chart_list);
                   }
                   else {
                       swal("TimeTable is Not Generated For Selected Details !!!!");
                       $scope.grid_view = false;
                   }

                   //for (var i = 0; i < $scope.table_list.length; i++) {

                   //}
                                                           

                   var chart = new CanvasJS.Chart("columnchart", {
                       
                       height: 260,
                       width: 320,
                       axisX: {
                           labelFontSize: 12,
                           labelAngle: -20 
                       },
                       axisY: {
                           labelFontSize: 12,
                       },

                       data: [
                       {
                           type: "column",
                           showInLegend: true,
                           dataPoints: chart_list
                       }
                       ]
                   });

                   chart.render();
                   //Mb


               })
                } else {
                    $scope.showweekly = false;
                  //  $scope.showEmplo = false;
                    TT_list = [];
                }
            }         
        };
        $scope.getdata = function () {
            if ($scope.TTMD_Id != null && $scope.TTMD_Id != "" && $scope.TTMD_Id != undefined) {
                $scope.onselectgroup();
            }
        }
        $scope.changeradio = function (abcc) {

            if (abcc == 'daily') {
                $scope.showdaily = false;
                $scope.showweekly = false;
                $scope.showday_d = true;
                $scope.showEmplo = true;
            }
            else if (abcc == 'weekly') {
                $scope.showweekly = false;
                $scope.showdaily = false;
                $scope.showday_d = false;
                $scope.showEmplo = true;
            }
            $scope.HRME_Id = "";
            $scope.TTMD_Id = "";
        }
    };
})();