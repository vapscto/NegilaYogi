
(function () {
    'use strict';
    angular
.module('app')
        .controller('StaffPeriodTransformController', StaffPeriodTransformController)

    StaffPeriodTransformController = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function StaffPeriodTransformController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.classdetails = [];
        $scope.grid_view = false;
        $scope.gridOptions = {};
        $scope.datareport = false;
        $scope.submitted = false;

        $scope.perSDK = false;
        $scope.deleteflg = false;
        $scope.ismS_Id = 0;
        $scope.ASMS_Id = 0;
        $scope.ASMCL_Id = 0;
        $scope.clsperer = false;
        $scope.allper = false;
        $scope.subper = false;
        $scope.Delete_to_save = function () {
            debugger;
            $scope.resetflag = true;
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if ($scope.resetflag === true) {
                mgs = "Delete";
                confirmmgs = "Deleted";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " The Selected Periods??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "HRME_Id": $scope.HRME_Id.hrmE_Id,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            Time_Table: $scope.trantimetable
                        }
                        apiService.create("StaffPeriodTransform/deleteperiod", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Total" + "  " + promise.sscnt + "  " + "Periods Deleted  successfully !");
                            
                                    $state.reload();
                                }
                                else {
                                    swal("Total" + "  " + promise.ffcnt + " " + "Periods Not Transfered !");

                                }
                            })
                       
                    }
                    else {
                        swal("Periods " + mgs + " Cancelled");
                    }
                });
            
        };


        $scope.HRME_Id = '';
        $scope.gettimetable = function () {
            debugger;
            var data = {
                "HRME_Id": $scope.HRME_Id.hrmE_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ISMS_Id": $scope.ismS_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("StaffPeriodTransform/gettimetable", data).
                then(function (promise) {
                    if (promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "" && promise.time_Table != null && promise.time_Table != "") {
                        $scope.grid_view = true;
                        $scope.period_list = promise.periodslst;
                        $scope.day_list = promise.gridweeks;
                        $scope.tt_list = promise.time_Table;
                     
                        console.log($scope.period_list);
                   
                        console.log($scope.day_list);
                      
                        console.log($scope.tt_list);
                        var temp_array = [];
                        $scope.table_list = [];
                        for (var j = 0; j < $scope.day_list.length; j++) {
                            temp_array = [];
                            for (var i = 0; i < $scope.period_list.length; i++) {
                                var count = 0;
                                var newCol = "";
                                for (var k = 0; k < $scope.tt_list.length; k++) {

                                    if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id) {
                                        if (count == 0) {
                                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                            count += 1;
                                        }
                                        else if (count > 0) {
                                            newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                            newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
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
                            $scope.table_list.push(temp_array);
                        }
                        $scope.datareport = true;
                        var temp_table_list = [];
                        temp_table_list = $scope.table_list;
                        $scope.temp_grid = temp_table_list;
                    }
                    else {
                        swal("TimeTable is Not Generated For Selected Details !!!!");
                        $scope.grid_view = false;
                        $scope.datareport = false;
                    }

                })

        }

        $scope.GetReport = function () {
            debugger;
            $scope.classdetails = [];
           $scope.trantimetable = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.HRME_Id == "" || $scope.HRME_Id == undefined) {
                    swal("Select Staff Name");
                }
                else {
                var data = {
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("StaffPeriodTransform/getrpt", data).
                    then(function (promise) {
                        if (promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "" && promise.time_Table != null && promise.time_Table != "") {
                            $scope.grid_view = true;
                            $scope.period_list = promise.periodslst;
                            $scope.stafflstto = promise.staffDrpDwnto;

                        $scope.day_list = promise.gridweeks;
                            $scope.tt_list = promise.time_Table;
                            console.log("P");
                            console.log($scope.period_list);
                            console.log("d");
                            console.log($scope.day_list);
                            console.log("t");
                            console.log($scope.tt_list);
                        var temp_array=[];
                        $scope.table_list = [];
                        for (var j = 0; j < $scope.day_list.length; j++) {
                            temp_array = [];
                            for (var i = 0; i < $scope.period_list.length; i++)
                            {
                                var count = 0;
                                var newCol = "";
                                for (var k = 0; k < $scope.tt_list.length; k++) {

                                    if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id) {
                                        if (count == 0) {
                                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                            count += 1;
                                        }
                                        else if (count > 0) {
                                            newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                            newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
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
                            $scope.table_list.push(temp_array);
                        }
                        $scope.datareport = true;
                        var temp_table_list = [];
                        temp_table_list = $scope.table_list;
                            $scope.temp_grid = temp_table_list;

                            $scope.classdetails = promise.classdetails;
                            $scope.secdetails = promise.secdetails;
                            $scope.subjectdet = promise.subjectdet;
                            console.log("fffffffff");
                            console.log($scope.table_list);
                            $scope.ismS_Id = 0;
                            $scope.ASMS_Id = 0;
                            $scope.ASMCL_Id = 0;





                        }
                        else {
                            swal("TimeTable is Not Generated For Selected Details !!!!");
                            $scope.grid_view = false;
                            $scope.datareport = false;
                        }
                    })
            }
            }
            $scope.staffSDK = "0";
            $scope.subSDK = "0";
            $scope.conSDK = "0";
            $scope.from = "";
            $scope.to = "";
            $scope.day_to_Id = "";
            $scope.period_to_Id = "";
            $scope.day_from_Id = "";
            $scope.period_from_Id = "";
        };

        $scope.trantimetable = [];
        $scope.clickAllprd = function (flg) {

            $scope.table_list123 = $scope.tt_list;
            if (flg == true) {
                for (var i = 0; i < $scope.table_list.length; i++) {
                    for (var j = 0; j < $scope.table_list[i].length; j++) {
                        for (var k = 0; k < $scope.table_list123.length; k++) {
                            $scope.temp_color = $scope.table_list[i][j].color;
                            if ($scope.table_list[i][j].pedid == $scope.table_list123[k].ttmP_Id && $scope.table_list[i][j].dayid == $scope.table_list123[k].ttmD_Id) {
                                $scope.table_list[i][j].color = "white";
                                $scope.table_list[i][j].background = "Green";
                            }
                            else {
                                $scope.table_list[i][j].color = $scope.temp_color;
                            }
                        }
                    }
                }
                $scope.trantimetable = $scope.table_list123;

            }
            else {
                for (var i = 0; i < $scope.table_list.length; i++) {
                    for (var j = 0; j < $scope.table_list[i].length; j++) {
                        for (var k = 0; k < $scope.table_list123.length; k++) {
                            $scope.temp_color = $scope.table_list[i][j].color;
                            if ($scope.table_list[i][j].pedid == $scope.table_list123[k].ttmP_Id && $scope.table_list[i][j].dayid == $scope.table_list123[k].ttmD_Id) {
                                $scope.table_list[i][j].color = "black";
                                $scope.table_list[i][j].background = "White";
                            }
                            else {
                                $scope.table_list[i][j].color = $scope.temp_color;
                            }
                        }
                    }
                }
                $scope.trantimetable = [];
            }


        }


        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("StaffPeriodTransform/getalldetails").
       then(function (promise) {

           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylst = promise.catelist;
           $scope.stafflst = promise.staffDrpDwn;               
       })
        };
        //TO clear  data
        $scope.clearid = function () {

            $scope.ttmC_Id = "";
            $scope.ASMAY_Id = "";
            $scope.HRME_Id = "";
            $scope.grid_view = false;
            $scope.submitted = false;
            $scope.datareport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.from = "";
            $scope.to = "";
            $scope.staffSDK = 0;
            $scope.clsSDK = 0;
            $scope.conSDK = 0;
        };
        $scope.from = "";
        $scope.to = "";
        $scope.templistdata = [];
        $scope.cell_click = function (dayid, periodid,day,period,bccolor) {
           // $scope.table_list = $scope.temp_grid;
           // $scope.table_list = $scope.trantimetable;

            var cntt = 0;
           
        
              //  $scope.from = day + "Day & " + period + " period";
                $scope.day_from_Id = dayid;
                $scope.period_from_Id = periodid;
             //   $scope.table_list = $scope.temp_grid;
                for (var i = 0; i < $scope.table_list.length; i++) {
                    for (var j = 0; j < $scope.table_list[i].length; j++) {
                     //   $scope.temp_color = $scope.table_list[i][j].color;
                        if ($scope.table_list[i][j].pedid == periodid && $scope.table_list[i][j].dayid == dayid) {
                            if ($scope.table_list[i][j].color == "white" && $scope.table_list[i][j].background == "Green") {
                                $scope.table_list[i][j].color = "black";
                                $scope.table_list[i][j].background = "white";
                                cntt += 1;
                                
                            }
                            else {
                                $scope.table_list[i][j].color = "white";
                                $scope.table_list[i][j].background = "Green";
                              
                            }
                           
                        }
                        //else {
                        //    $scope.table_list[i][j].color = $scope.temp_color;
                        //}
                    }
                }

            $scope.templistdata=[];
       
                //if ($scope.trantimetable.length>0) {
                    if (bccolor == "white") {
                        angular.forEach($scope.trantimetable, function (tt) {
                            if (dayid == tt.ttmD_Id && periodid==tt.ttmP_Id) {
                                $scope.templistdata.push(tt);
                            }
                        })


                        $scope.templistdata1 = [];
                        angular.forEach($scope.trantimetable, function (tt) {

                            angular.forEach($scope.templistdata, function (rr) {



                                if (tt.ttfgD_Id != rr.ttfgD_Id) {
                                    $scope.templistdata1.push(tt);
                                }
                            })

                           
                        })


                        $scope.trantimetable = $scope.templistdata1;
                    }
                    if (bccolor == "black") {
                        $scope.templistdata2 = [];
                        angular.forEach($scope.tt_list, function (jj) {

                            if (dayid == jj.ttmD_Id && periodid == jj.ttmP_Id) {
                                $scope.templistdata2.push(jj);
                            }

                        })
                        angular.forEach($scope.templistdata2, function (tt) {
                          
                                $scope.trantimetable.push(tt);
                           
                        })
                    }
                //}
           

            
            console.log("lllll");
            console.log($scope.trantimetable);

            if (cntt>0) {
                $scope.allper = false;
            }
           
          

        }

        

        $scope.interacted = function (field) {

            return $scope.submitted //|| field.$dirty;
        };

        $scope.get_category = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("StaffPeriodTransform/get_catg", data).
         then(function (promise) {

             $scope.categorylst = promise.catelist;
             if (promise.catelist === "" || promise.catelist === null) {
                 swal("No Category Are Mapped To Selected Academic Year");
             }
         })
            }
        };

        $scope.replacement_to_save = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HRME_IdTO": $scope.hrmE_Idto,
                    Time_Table:$scope.trantimetable
                }
                apiService.create("StaffPeriodTransform/savedetail", data).
                  then(function (promise) {
                      if (promise.returnval === true) {
                          swal("Total"+"  " + promise.sscnt +"  "+ "Periods Transfered  successfully !");
                          $state.reload();
                      }
                      else {
                          swal("Total"+"  " + promise.ffcnt +" " + "Periods Not Transfered !");
                          
                      }
                  })
            }
            else {
                $scope.submitted = true;
            }
        };
    }
})();