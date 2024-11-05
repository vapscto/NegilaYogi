(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeeLateInEarlyOutReportController', EmployeeLateInEarlyOutReportController)

    EmployeeLateInEarlyOutReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function EmployeeLateInEarlyOutReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.maxDatemf = new Date();
        //loading start


        $scope.loadData = function () {

            var id = 2;
            
            apiService.getURI("EmployeeLateInEarlyOutReport/getalldetails", id).
        then(function (promise) {
            
            $scope.staff_types = promise.filltypes;
            $scope.All_Individual();
            $scope.grid_view = false;
        })
        };
        //loading end
        $scope.gettodate = function () {
            
            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };
        //validation start
        $scope.interacted = function (field) {

            // return $scope.submitted || field.$dirty;
            return $scope.submitted;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.staff_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.Department_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired2 = function () {
            return !$scope.Designation_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //validation end
        //all-individual start
        $scope.All_Individual = function () {
            
            if ($scope.allind == 'Indi')
                $scope.disabledata = false;
            else
                // $scope.hrmE_Id = "";
                $scope.disabledata = true;

        }
        //all-individual end    

        //all check button start
        $scope.all_check = function () {
            
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_departments();
        }
        $scope.all_checkdep = function () {
            
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        }
        $scope.all_checkdesg = function () {
            
            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        }
        //all-check button end
        // fill dep start
        $scope.get_departments = function () {
            $scope.usercheck = $scope.staff_types.every(function (options) {
                return options.selected;
            });
            $scope.deptcheck = "";
            $scope.desgcheck = "";
            
            var groupidss;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                if ($scope.staff_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.staff_types[i].hrmgT_Id;
                    else
                        groupidss = groupidss + "," + $scope.staff_types[i].hrmgT_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipletype": groupidss,
                }
                apiService.create("EmployeeLateInEarlyOutReport/get_departments", data).
                then(function (promise) {
                    
                    $scope.Department_types = promise.filldepartment;
                    if ($scope.Department_types.length > 0) {
                        // $scope.Department_types[0].selected = true;
                        $scope.get_designation();
                    }

                })
            }
            else {
                $scope.Department_types = "";
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }
        //fill department end
        //fill desg start
        $scope.get_designation = function () {
            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.selected;
            });
            
            $scope.get_designationnew();
        }
        $scope.get_designationnew = function () {
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.Department_types.length; i++) {
                if ($scope.Department_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.Department_types[i].hrmD_Id;
                    else
                        groupidss = groupidss + "," + $scope.Department_types[i].hrmD_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledep": groupidss,
                }
                apiService.create("EmployeeLateInEarlyOutReport/get_designation", data).
                then(function (promise) {
                    
                    $scope.Designation_types = promise.filldesignation;
                    if ($scope.Designation_types.length > 0) {
                        //  $scope.Designation_types[0].selected = true;
                        $scope.get_employee();
                    }
                })
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }
        //fill desg end
        //fill employee start
        $scope.get_employee = function () {
            $scope.desgcheck = $scope.Designation_types.every(function (options) {

                return options.selected;
            });
            
            $scope.get_employeenew();
        }
        $scope.get_employeenew = function () {

            var typeIds;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                if ($scope.staff_types[i].selected == true) {

                    if (typeIds == undefined)
                        typeIds = $scope.staff_types[i].hrmgT_Id;
                    else
                        typeIds = typeIds + "," + $scope.staff_types[i].hrmgT_Id;
                }
            }

            var deptIds;
            for (var i = 0; i < $scope.Department_types.length; i++) {
                if ($scope.Department_types[i].selected == true) {

                    if (deptIds == undefined)
                        deptIds = $scope.Department_types[i].hrmD_Id;
                    else
                        deptIds = deptIds + "," + $scope.Department_types[i].hrmD_Id;
                }
            }

            var groupidss;
            for (var i = 0; i < $scope.Designation_types.length; i++) {
                if ($scope.Designation_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.Designation_types[i].hrmdeS_Id;
                    else
                        groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledes": groupidss,
                    "multipletype": typeIds,
                    "multipledep": deptIds
                }
                apiService.create("EmployeeLateInEarlyOutReport/get_employee", data).
                then(function (promise) {
                    
                    $scope.Employeelst = promise.fillemployee;
                    //if ($scope.Employeelst.length > 0) {
                    //   // $scope.hideempse = false;
                    //   // $scope.Employeelst[0].Selected = true;
                    //    $scope.hrmE_Id = $scope.Employeelst[0].hrmE_Id;
                    //}
                })
            }
            else {
                $scope.Employeelst = "";
            }
        }
        //fill employee end
        //get report start
        $scope.submitted = false;
        $scope.GetReport = function () {
            
            if ($scope.myForm.$valid) {
                var groupidss;
                if ($scope.allind == "All") {
                    for (var i = 0; i < $scope.Employeelst.length; i++) {
                        if (groupidss == undefined)
                            groupidss = $scope.Employeelst[i].hrmE_Id;
                        else
                            groupidss = groupidss + "," + $scope.Employeelst[i].hrmE_Id;
                    }
                }
                else {
                    groupidss = $scope.hrmE_Id;
                }
                if (groupidss != undefined) {
                    var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");
                    var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
                    var data = {

                        "fromdate": fromdate1,
                        "todate": todate1,
                        "multiplehrmeid": groupidss,
                        "punchtype": $scope.rdopunch
                    }
                    apiService.create("EmployeeLateInEarlyOutReport/getrpt", data).
                        then(function (promise) {
                          
                        if (promise.filldata.length > 0) {
                            $scope.grid_view = true;
                            if (promise.punchtype == "late")
                                $scope.rhead = "Late By";
                            else
                                $scope.rhead = "Early Out";

                            $scope.columnnames = promise.columnnames;
                            $scope.columnnum = $scope.columnnames.length + 4;
                           
                            if ($scope.rdopunch == 'LIEO')
                            {
                              
                                var temp_array1 = [];
                                var temp_array = [];
                                angular.forEach(promise.filldata, function (re) {
                                    var hrme_id = re.HRME_Id;                                  
                                    for (var i = 0; i < promise.filldata.length; i++) {
                                       
                                        if (promise.filldata[i].HRME_Id == hrme_id) {
                                            var newCol = { punchdate: promise.filldata[i].punchdate, punchtime: promise.filldata[i].punchtime, flag: promise.filldata[i].FOEPD_InOutFlg }
                                            if (temp_array.length == 0) {
                                                temp_array.push(newCol);
                                            }
                                            else if (temp_array.length > 0) {
                                                var alrdy_cnt1 = 0;
                                                angular.forEach(temp_array, function (tet1) {
                                                    if (tet1.punchdate == promise.filldata[i].punchdate && tet1.flag == promise.filldata[i].FOEPD_InOutFlg) {
                                                        alrdy_cnt1 += 1;
                                                    }
                                                })
                                                if (alrdy_cnt1 == 0) {
                                                    temp_array.push(newCol);
                                                }
                                            }

                                        }
                                    }
                                    var newCol1 = { pdate: temp_array, hrmE_Id: re.HRME_Id, ecode: re.ecode, ename: re.ename, HRMDES_DesignationName: re.desgname }
                                    if (temp_array1.length == 0) {
                                        temp_array1.push(newCol1);
                                    }
                                    else if (temp_array1.length > 0) {
                                        var alrdy_cnt = 0;
                                        angular.forEach(temp_array1, function (tet) {
                                            if (tet.hrmE_Id == hrme_id) {
                                                alrdy_cnt += 1;
                                            }
                                        })
                                        if (alrdy_cnt == 0) {
                                            temp_array1.push(newCol1);
                                        }
                                    }

                                    temp_array = [];
                                })
                                $scope.report_list = temp_array1;
                                console.log(temp_array1);
                            }
                            else {
                                var temp_array = [];
                                var temp_array1 = [];
                                angular.forEach(promise.filldata, function (re) {
                                    var hrme_id = re.HRME_Id;
                                    for (var i = 0; i < promise.filldata.length; i++) {
                                        
                                        if (promise.filldata[i].HRME_Id == hrme_id) {
                                            if (promise.punchtype == "late")
                                                var newCol = { punchdate: promise.filldata[i].punchdate, intime: promise.filldata[i].intime }
                                            else
                                                var newCol = { punchdate: promise.filldata[i].punchdate, intime: promise.filldata[i].outtime }

                                            if (temp_array.length == 0) {
                                                temp_array.push(newCol);
                                            }
                                            else if (temp_array.length > 0) {
                                                var alrdy_cnt1 = 0;
                                                angular.forEach(temp_array, function (tet1) {
                                                    if (tet1.punchdate == promise.filldata[i].punchdate) {
                                                        alrdy_cnt1 += 1;
                                                    }
                                                })
                                                if (alrdy_cnt1 == 0) {
                                                    temp_array.push(newCol);
                                                }
                                            }

                                        }

                                        //if (i < promise.filldata.length - 1) {
                                        //    if (promise.filldata[i].HRME_Id == promise.filldata[i + 1].HRME_Id)
                                        //        continue;
                                        //}

                                    }
                                    var newCol1 = { pdate: temp_array, hrmE_Id: re.HRME_Id, ecode: re.ecode, ename: re.ename, HRMDES_DesignationName: re.desgname }
                                    if (temp_array1.length == 0) {
                                        temp_array1.push(newCol1);
                                    }
                                    else if (temp_array1.length > 0) {
                                        var alrdy_cnt = 0;
                                        angular.forEach(temp_array1, function (tet) {
                                            if (tet.hrmE_Id == hrme_id) {
                                                alrdy_cnt += 1;
                                            }
                                        })
                                        if (alrdy_cnt == 0) {
                                            temp_array1.push(newCol1);
                                        }
                                    }

                                    temp_array = [];
                                })
                                $scope.yearlyemprep = temp_array1;
                            }
                          
                         
                         
                        }
                        else {
                            $scope.grid_view = false;
                            swal("Record not found.");

                        }
                    })
                    }
            }
            else
            {
                $scope.submitted = true;
            }
        };
            //get report end
            //print start
            $scope.printData = function () {
                
                var divToPrint;
                if ($scope.rdopunch == 'LIEO')
                {
                     divToPrint = document.getElementById("table_n");
                }
                else {
                     divToPrint = document.getElementById("table");
                }
              //  var divToPrint = document.getElementById("table");
                var newWin = window.open("");
                newWin.document.write(divToPrint.outerHTML);
                newWin.print();
                newWin.close();
            }
            //print end
            //export start

            $scope.exptoex = function () {
                var divToPrint;
                if ($scope.rdopunch == 'LIEO') {
                    divToPrint = document.getElementById("table_n");
                }
                else {
                    divToPrint = document.getElementById("table");
                }

              //  var divToPrint = document.getElementById("table");
                var blob = new Blob([divToPrint.outerHTML], {
                    type: "application/vnd.ms-excel;charset=utf-8"
                });
                saveAs(blob, "Report.xls");
            };
            //export end
            //TO clear  data
            $scope.clearid = function () {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.submitted = false;
                $scope.Department_types = "";
                $scope.Designation_types = "";
                $scope.Employeelst = "";
                $scope.hrmE_Id = "";
                $scope.usercheck = "";
                $scope.deptcheck = "";
                $scope.desgcheck = "";
                $scope.allind = "All";
                $scope.fromdate = "";
                $scope.todate = "";
             
                $scope.disabledata = true;
                $scope.grid_view = false;
                for (var i = 0; i < $scope.staff_types.length; i++) {
                    $scope.staff_types[i].selected = false;
                }
            };
            //clear end
       
    }
})();