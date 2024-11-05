(function () {
    'use strict';

    angular
        .module('app')
        .controller('Student_Settlement', Student_Settlement);

    Student_Settlement.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'exportUiGridService', 'Excel', '$timeout','$filter'];

    function Student_Settlement($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, exportUiGridService, Excel, $timeout, $filter) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Student_Settlement';

        activate();
        $scope.testgrd = false;
        function activate() { }
        $scope.onTabChanges = function (currentTabIndex) {
            debugger;
            var count = 0;
            if (currentTabIndex == 3) {
                $scope.testgrd = true;

                //$scope.submitted2 = false;
                //$scope.myForm3.$setPristine();
                //$scope.myForm3.$setUntouched();
            }
            else {
                $scope.testgrd = false;

            }

        };
        $scope.previousDate = new Date();
        $scope.previousDate.setDate($scope.previousDate.getDate() - 1);
        $scope.today = new Date();

        $scope.print_data = false;


        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings != null) {

            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
            else {
                copty = "";
                paginationformasters = 10;
            }
        }
        else {
            copty = "";
            paginationformasters = 10;
        }


        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.loadData = function () {
            $scope.currentPage = 1;
            $scope.searchValue = '';
            $scope.searchValue1 = '';
            apiService.getDATA("Student_Settlement/Getdetails").
                then(function (promise) {
                    debugger;
                    $scope.year_list = promise.yearlist;
                    //$scope.merchant_list = promise.merchantlist;
                    $scope.pages = promise.savedlist;
                    // $scope.route_list = promise.routelist;
                    $scope.ASMAY_Id = promise.asmaY_Id;
                    $scope.getdates($scope.ASMAY_Id);
                    //$scope.Selected_Date = new Date();
                    $scope.Selected_Date = $scope.previousDate;

                    $scope.paygatewaylist = promise.paymentgatewaydet;
                })
        }

        $scope.fillmerchants = function (impgid) {
            var data = {
                "IMPG_Id": impgid,
            }

            apiService.create("Student_Settlement/fillmerchants", data).
                then(function (promise) {
                    $scope.merchant_list = promise.merchantlist;
                })
        }

        $scope.getdates = function (yr_id) {

            if (yr_id != '') {


                var data = "";
                angular.forEach($scope.year_list, function (yr) {
                    if (yr.asmaY_Id == $scope.ASMAY_Id) {
                        data = yr.asmaY_Year;
                    }
                })

                if (data != null) {
                    debugger;
                    console.log(data);
                    var name, name1;
                    for (var i = 0; i < data.length; i++) {
                        if (i < 4) {
                            if (i == 0) {
                                name = data[i];
                            } else {
                                name += data[i];
                            }
                        }
                        if (i > 4) {
                            if (i == 5) {
                                name1 = data[5];
                            } else {
                                name1 += data[i];
                            }
                        }
                    }
                    $scope.fromDate = name;
                    $scope.toDate = name1;


                    $scope.frommon = "";
                    $scope.tomon = "";
                    $scope.fromDay = "";
                    $scope.toDay = "";
                    // For Academic From Date

                    $scope.minDatemf = new Date(
                        $scope.fromDate,
                        $scope.frommon,
                        $scope.fromDay + 1);

                    $scope.maxDatemf = new Date(
                        $scope.toDate,
                        $scope.tomon,
                        $scope.toDay + 365);

                    // $scope.today = new Date();
                    $scope.Selected_Date = null;
                    if ($scope.maxDatemf >= $scope.previousDate) {
                        $scope.maxDatemf = $scope.previousDate;
                    }
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Student_Settlement/getdates", data).
                    then(function (promise) {
                        $scope.date_list = promise.datelist;
                    })

            }
        }
        $scope.onlyWeekendsPredicate = function (date) {
            if ($scope.date_list != [] && $scope.date_list != undefined && $scope.date_list != null && $scope.date_list.length > 0) {
                //var day = date.getDay();
                //return day === 1 || day === 2 || day === 3 || day === 4 || day === 5 || day === 6;
                var already_cnt = 0;
                var dateb = new Date(date).toDateString();

                var accountlen = $scope.merchant_list.length;

                angular.forEach($scope.date_list, function (date1) {
                    var datea = new Date(date1).toDateString();
                    //if(new Date(date1)==new Date(date))
                    if (datea == dateb) {
                        // return false;
                        already_cnt += Number(accountlen);
                    }
                })
                if (already_cnt >= Number(accountlen)) {
                    return false;
                }
                else if (already_cnt < Number(accountlen)) {
                    return true;
                }
            }
            else {
                return true;
            }
        };

        $scope.interacted1 = function (field) {

            return $scope.submitted1;
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        };
        $scope.sort2 = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        };

        $scope.submitted1 = false;
        $scope.saveddata = function () {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                var data = {
                    //"EME_Id": $scope.EME_ID,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Selected_Date": new Date($scope.Selected_Date).toDateString(),
                    "FPGD_Id": $scope.FPGD_Id,
                    "IMPG_Id": $scope.IMPG_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Student_Settlement/savedata", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record saved successfully');
                        }
                        else {
                            if (!promise.settled_flag) {
                                swal('Settlement Not Done For Selected Date');
                            }
                            else {
                                swal('Failed To Save, Please Contact Administrator');
                            }
                        }
                        $scope.clear1();
                        $scope.loadData();
                    })
            }
            else {
                $scope.submitted1 = true;
            }
        };
        $scope.clear1 = function () {
            $scope.ASMAY_Id = "";
            $scope.Selected_Date = null;
            $scope.FPGD_Id = "";
            $scope.searchValue = '';
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        };
        $scope.clear2 = function () {
            $scope.ASMAY_Id1 = "";
            $scope.From_Date = null;
            $scope.To_Date = null;
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.TRMR_Id = '';
            $scope.searchValue1 = '';
            $scope.route_check = false;
            $scope.report_list = [];
            $scope.submitted2 = false;
            //$scope.myForm2.$setPristine();
            //$scope.myForm2.$setUntouched();
            $scope.section_check = false;
            $scope.class_check = false;
        };

        $scope.clear3 = function () {
            $state.reload();
        }


        $scope.searchValue = '';
        $scope.search1 = function (obj) {
            return ($filter('date')(obj.fyppsT_Settlement_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) ||
                (JSON.stringify(obj.fyppsT_Settlement_Amount)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.fyppsT_UTR_No)).indexOf($scope.searchValue) >= 0;
        }


        $scope.viewrecordspopup = function (employee) {
            debugger;
            $scope.editEmployee = employee.fyppsT_Id;
            var pageid = $scope.editEmployee;
            $scope.Settlement_Id = employee.fyppsT_Settlement_Id;
            apiService.create("Student_Settlement/viewrecords", employee).
                then(function (promise) {
                    debugger;
                    $scope.viewrecordspopupdisplay = promise.viewlist;

                })

        };


        $scope.get_classes = function () {
            debugger;
            $scope.report_list = [];
            if ($scope.ASMAY_Id1 != "" && $scope.ASMAY_Id1 != undefined) {

                var data = "";
                angular.forEach($scope.year_list, function (yr) {
                    if (yr.asmaY_Id == $scope.ASMAY_Id1) {
                        data = yr.asmaY_Year;
                    }
                })

                if (data != null) {
                    debugger;
                    console.log(data);
                    var name, name1;
                    for (var i = 0; i < data.length; i++) {
                        if (i < 4) {
                            if (i == 0) {
                                name = data[i];
                            } else {
                                name += data[i];
                            }
                        }
                        if (i > 4) {
                            if (i == 5) {
                                name1 = data[5];
                            } else {
                                name1 += data[i];
                            }
                        }
                    }
                    $scope.fromDate = name;
                    $scope.toDate = name1;
                    $scope.frommon = "";
                    $scope.tomon = "";
                    $scope.fromDay = "";
                    $scope.toDay = "";
                    // For Academic From Date
                    $scope.minDatemf1 = new Date(
                        $scope.fromDate,
                        $scope.frommon,
                        $scope.fromDay + 1);

                    $scope.maxDatemf1 = new Date(
                        $scope.toDate,
                        $scope.tomon,
                        $scope.toDay + 365);
                    // $scope.today = new Date();
                    if ($scope.maxDatemf1 >= $scope.previousDate) {
                        $scope.maxDatemf1 = $scope.previousDate;
                    }
                    $scope.From_Date = null;
                    $scope.To_Date = null;
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id1,

                }
                apiService.create("Student_Settlement/get_classes", data).
                    then(function (promise) {
                        $scope.class_list = promise.classlist;
                        $scope.ASMCL_Id = "";

                    })

            }
            else {
                // swal("Please Select Academic Year First !!!");
                $scope.class_list = [];
                $scope.ASMCL_Id = "";
            }
        }

        $scope.get_sections = function () {
            $scope.report_list = [];
            if ($scope.ASMCL_Id != "" && $scope.ASMCL_Id != undefined) {
                if ($scope.ASMAY_Id1 != "" && $scope.ASMAY_Id1 != undefined) {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id1,
                        "ASMCL_Id": $scope.ASMCL_Id
                    }
                    apiService.create("Student_Settlement/get_sections", data).
                        then(function (promise) {
                            $scope.section_list = promise.sectionlist;
                            $scope.ASMS_Id = "";

                        })

                }
                else {
                    swal("Please Select Academic Year And Class First !!!");
                    $scope.section_list = [];
                    $scope.ASMS_Id = "";
                    $scope.ASMCL_Id = "";
                }
            }
            else {
                $scope.section_list = [];
                $scope.ASMS_Id = "";
            }

        }

        $scope.get_routes = function () {
            $scope.report_list = [];
            if ($scope.ASMS_Id != "" && $scope.ASMS_Id != undefined) {
                if ($scope.ASMAY_Id1 != "" && $scope.ASMAY_Id1 != undefined && $scope.ASMCL_Id != "" && $scope.ASMCL_Id != undefined) {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id1,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id
                    }
                    apiService.create("Student_Settlement/get_routes", data).
                        then(function (promise) {
                            $scope.route_list = promise.routelist;
                            $scope.TRMR_Id = "";

                        })

                }
                else {
                    swal("Please Select Academic Year And Class First !!!");
                    $scope.route_list = [];
                    $scope.TRMR_Id = "";
                    $scope.ASMS_Id = "";
                }
            }
            else {
                $scope.route_list = [];
                $scope.TRMR_Id = "";
            }

        }

        $scope.section_check = false;
        $scope.class_check = false;

        $scope.submitted2 = false;
        $scope.getreport = function () {
            $scope.submitted2 = true;



            //if ($scope.myForm2.$valid) {

                if ($scope.class_check == false && $scope.section_check == false) {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id1,
                        "ASMCL_Id": 0,
                        "ASMS_Id": 0,
                        "From_Date": new Date($scope.From_Date).toDateString(),
                        "To_Date": new Date($scope.To_Date).toDateString(),
                        "allclass": $scope.class_check,
                        "allsection": $scope.section_check
                    }
                }

                if ($scope.class_check == true && $scope.section_check == true) {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id1,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "From_Date": new Date($scope.From_Date).toDateString(),
                        "To_Date": new Date($scope.To_Date).toDateString(),
                        "allclass": $scope.class_check,
                        "allsection": $scope.section_check
                    }
                }

                if ($scope.class_check == true && $scope.section_check == false) {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id1,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": 0,
                        "From_Date": new Date($scope.From_Date).toDateString(),
                        "To_Date": new Date($scope.To_Date).toDateString(),
                        "allclass": $scope.class_check,
                        "allsection": $scope.section_check
                    }
                }

                if ($scope.class_check == false && $scope.section_check == true) {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id1,
                        "ASMCL_Id": 0,
                        "ASMS_Id": $scope.ASMS_Id,
                        "From_Date": new Date($scope.From_Date).toDateString(),
                        "To_Date": new Date($scope.To_Date).toDateString(),
                        "allclass": $scope.class_check,
                        "allsection": $scope.section_check
                    }
                }

                if ($scope.route_check) {
                    data.TRMR_Id = $scope.TRMR_Id;
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Student_Settlement/getreport", data).
                    then(function (promise) {

                        $scope.report_list = promise.reportlist;
                    })
            //}
            //else {
            //    $scope.submitted2 = true;
            //}
        };






        $scope.gridOptions = {
            enableRowSelection: true,
            enableSelectAll: true,
            showGridFooter: false,
            showColumnFooter: false,
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [10, 20, 30],
            paginationPageSize: 20,
            enableGridMenu: false,
            columnDefs: $scope.colarray,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Export all data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
                }
            }
            ],
            getFooterValue: function () {
                return $scope.gridApi.grid.columns[2].getAggregationValue();
            }

        };

        $scope.gridOptionsall = {
            enableRowSelection: true,
            enableSelectAll: true,
            showGridFooter: false,
            showColumnFooter: true,
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [10, 20, 30],
            paginationPageSize: 20,
            enableGridMenu: false,
            columnDefs: $scope.colarrayall,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Export all data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
                }
            }
            ],
            getFooterValue: function () {
                return $scope.gridApi.grid.columns[2].getAggregationValue();
            }

        };
        $scope.remove_list = [];
        $scope.getreport1 = function () {

            // $scope.testgrd = true;
            $scope.submitted2 = true;
            $scope.arrlistchkhead = [];
            $scope.prsary = [];
            $scope.colarrayaggre = [];
            $scope.kengrdtotary = [];
            $scope.colarray = [];
            $scope.colarray = [{

                title: "SLNO",
                template: "<span class='row-numberind'></span>"

            }, { name: 'StudentName', field: 'StudentName', width: '100px', title: 'Student Name' }, { name: 'AdmNo', field: 'AdmNo', title: 'Adm No' },
            { name: 'ClassName', field: 'ClassName', width: '100px', title: 'Class' },
            { name: 'SectionName', field: 'SectionName', width: '100px', title: 'Section' },
            { name: 'UTR_No', field: 'UTR_No', width: '100px', title: 'ReceiptNo' },
            { name: 'Transactionid', field: 'Transactionid', width: '100px', title: 'Transaction No' },
                { name: 'PaymentId', field: 'PaymentId', width: '100px', title: 'Payment Id' },
                { name: 'FYPPST_Settlement_Date', field: 'FYPPST_Settlement_Date', width: '100px', title: 'Settlement Date' },
                { name: 'FYP_Date', field: 'FYP_Date', width: '100px', title: 'Transaction Date' }
            ];
            $scope.gridOptions.data = [];
            $scope.gridOptionsall.data = [];

            //if ($scope.myForm2.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id1,
                    "ASMCL_Id": 0,
                    "ASMS_Id": 0,
                    "From_Date": new Date($scope.From_Date).toDateString(),
                    "To_Date": new Date($scope.To_Date).toDateString(),

                }


                apiService.create("Student_Settlement/getreport1", data).
                    then(function (promise) {

                        //  $scope.report_list = promise.reportlist;
                        $scope.arrlistchkhead = promise.alldata;
                        $scope.temporary_list = promise.alllist;
                        $scope.student_list = promise.reportlist;

                        $scope.specialfeeheads = promise.studentlist;
                        $scope.temp_special_headers = [];
                        $scope.specialheadsdetails = promise.allgroupheaddata;
                        angular.forEach($scope.specialfeeheads, function (op1) {
                            var spe_hd_list = [];
                            angular.forEach($scope.specialheadsdetails, function (op2) {
                                if (op1.fmsfH_Id == op2.fmsfH_Id) {
                                    angular.forEach($scope.arrlistchkhead, function (op_m) {
                                        if (op_m.fmH_Id == op2.fmH_Id) {
                                            spe_hd_list.push(op_m);
                                            //   headtot += headtot + op2.ftP_Paid_Amt;
                                            $scope.remove_list.push(op_m);
                                        }
                                    })
                                }

                            })
                            if (spe_hd_list.length > 0) {
                                $scope.temp_special_headers.push({ sp_id: op1.fmsfH_Id, sp_name: op1.fmsfH_Name, spe_hd_list: spe_hd_list, fmH_FeeName: op1.fmsfH_Name });
                            }
                        })


                        angular.forEach($scope.student_list, function (stud) {
                            angular.forEach($scope.arrlistchkhead, function (sp_hd) {
                                if (sp_hd.special_flag) {
                                    var special_head_amoumt = 0;
                                    angular.forEach(sp_hd.spe_hd_list, function (hds) {
                                        special_head_amoumt += stud[hds.fmH_FeeName];
                                    })
                                    stud[sp_hd.fmH_FeeName] = parseInt(special_head_amoumt);
                                }
                            })
                        })
                        console.log($scope.student_list);

                        $scope.table_all = false;
                        $scope.table = true;
                        $scope.testary = [];
                        $scope.temp_array_total = [];


                        $scope.temp_array = [];
                        debugger;
                        angular.forEach($scope.arrlistchkhead, function (objj) {
                            var strstr = '["' + objj.fmH_FeeName + '"]';

                            $scope.colarrayaggre.push({ field: strstr, name: objj.fmH_FeeName, title: objj.fmH_FeeName, aggregate: "sum" });
                            $scope.colarray.push({
                                field: strstr, name: objj.fmH_FeeName, title: objj.fmH_FeeName, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                groupFooterTemplate: "Sum: #=sum#"
                            });

                            $scope.prsary.push({
                                field: strstr, name: objj.fmH_FeeName, title: objj.fmH_FeeName, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                groupFooterTemplate: "Sum: #=sum#"
                            });
                        })
                        $scope.all_grand_total = 0;
                        for (var z = 0; z < $scope.temp_array_total.length; z++) {
                            $scope.all_grand_total += $scope.temp_array_total[z].total;
                        }
                        $scope.colarray.push({
                            name: 'total_side1', field: 'total_side1', title: 'Total', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                            groupFooterTemplate: "Sum: #=sum#"
                        });

                        $scope.colarrayaggre.push({ name: 'total_side1', field: 'total_side1', title: 'Total', aggregate: "sum" });
                        angular.forEach($scope.colarray, function (widobj) {
                            widobj.width = 100;
                        })

                        for (var x = 0; x < $scope.student_list.length; x++) {
                            var total_y = 0;
                            for (var y = 0; y < $scope.arrlistchkhead.length; y++) {
                                var column = $scope.arrlistchkhead[y].fmH_FeeName;
                                total_y += Number($scope.student_list[x][column]);
                                $scope.student_list[x].total_side = total_y;
                            }
                            $scope.temp_array.push({ date: $scope.student_list[x].Date, array: $scope.student_list[x] });
                            $scope.gridOptions.data.push($scope.student_list[x]);

                        }
                        $scope.kengrdtotary = $scope.gridOptions.data;
                        debugger;
                        var obj = [];
                        var indi_totals = [];
                        for (var j = 0; j < $scope.arrlistchkhead.length; j++) {

                            var total_X = 0;
                            angular.forEach($scope.student_list, function (e) {
                                if (e[$scope.arrlistchkhead[j].fmH_FeeName] == null) {//added for checking null values
                                    e[$scope.arrlistchkhead[j].fmH_FeeName] = 0;
                                }
                                total_X += e[$scope.arrlistchkhead[j].fmH_FeeName];

                            });

                            var key = $scope.arrlistchkhead[j].fmH_FeeName;

                            obj = {
                                [key]: total_X
                            };
                            indi_totals.push(obj);
                            console.log(obj);

                        }

                        console.log($scope.colarray);
                        console.log($scope.kengrdtotary);
                        var gridind;

                        $(document).ready(function () {
                            initGridind();
                        });

                        function initGridind() {
                            $('#gridind').empty();
                            gridind = $("#gridind").kendoGrid({
                                toolbar: ["excel", "pdf"],
                                excel: {
                                    fileName: "inddExport.xlsx",
                                    filterable: true,
                                    allPages: true
                                },
                                pdf: {
                                    fileName: "inddExport.pdf",
                                    filterable: true
                                },
                                dataSource: {

                                    data: $scope.kengrdtotary,
                                    aggregate: $scope.colarrayaggre,
                                    pageSize: 100,
                                    schema: {

                                        parse: function (d) {

                                            $.each(d, function (idx, elem) {
                                                var z = 0;
                                                for (var j = 0; j < $scope.prsary.length; j++) {
                                                    z += elem[$scope.prsary[j].name];
                                                }
                                                elem.total_side1 = z;
                                            });
                                            return d;
                                        }
                                    }
                                },

                                sortable: true,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,
                                columns: $scope.colarray,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize()
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-numberind");
                                        $(rowLabel).html(index);
                                    });
                                }

                            }).data("kendoGrid");
                        }




                    })
            //}
            //else {
            //    $scope.submitted2 = true;
            //}
        };


        $scope.exportExcel = function () {

            var grid = $scope.gridApi.grid;
            var rowTypes = exportUiGridService.ALL;
            var colTypes = exportUiGridService.ALL;
            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
        };



        $scope.printData = function (printSectionId) {

            var year1 = "";
            angular.forEach($scope.year_list, function (dd) {
                if (dd.asmaY_Id == $scope.ASMAY_Id) {
                    year1 = dd.asmaY_Year;
                    $scope.year = year1;
                }
            })

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function (export_id) {

            $scope.exportarr = $scope.pages;

            var year1 = "";
            angular.forEach($scope.year_list, function (dd) {
                if (dd.asmaY_Id == $scope.ASMAY_Id) {
                    year1 = dd.asmaY_Year;
                    $scope.year = year1;
                }
            })

            var excelname = "Settlement Report  " + $scope.year + ".xls";
            var excelname1 = "Settlement Report  " + $scope.year;

            var exportHref = Excel.tableToExcel(export_id, excelname1);

            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        }


        $scope.exportToExcelmodel = function (export_id) {

            var year1 = "";
            angular.forEach($scope.year_list, function (dd) {
                if (dd.asmaY_Id == $scope.ASMAY_Id) {
                    year1 = dd.asmaY_Year;
                    $scope.year = year1;
                }
            })

            var excelname = "Settlement Report - Payment Details  " + $scope.year + ".xls";
            var excelname1 = "Settlement Report - Payment Details " + $scope.year;

            var exportHref = Excel.tableToExcel(export_id, excelname1);

            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        }


    }
})();

angular
    .module('app').factory('exportUiGridService', exportUiGridService);

exportUiGridService.inject = ['uiGridExporterService'];
function exportUiGridService(uiGridExporterService) {
    var service = {
        exportToExcel: exportToExcel
    };

    return service;

    function Workbook() {
        if (!(this instanceof Workbook)) return new Workbook();
        this.SheetNames = [];
        this.Sheets = {};
    }

    function exportToExcel(sheetName, gridApi, rowTypes, colTypes) {
        var columns = gridApi.grid.options.showHeader ? uiGridExporterService.getColumnHeaders(gridApi.grid, colTypes) : [];
        var data = uiGridExporterService.getData(gridApi.grid, rowTypes, colTypes);
        var fileName = gridApi.grid.options.exporterExcelFilename ? gridApi.grid.options.exporterExcelFilename : 'dailyfeecolreport';
        fileName += '.xlsx';
        var wb = new Workbook(),
            ws = sheetFromArrayUiGrid(data, columns);
        wb.SheetNames.push(sheetName);
        wb.Sheets[sheetName] = ws;
        var wbout = XLSX.write(wb, {
            bookType: 'xlsx',
            bookSST: true,
            type: 'binary'
        });
        saveAs(new Blob([s2ab(wbout)], {
            type: 'application/octet-stream'
        }), fileName);
    }

    function sheetFromArrayUiGrid(data, columns) {
        var ws = {};
        var range = {
            s: {
                c: 10000000,
                r: 10000000
            },
            e: {
                c: 0,
                r: 0
            }
        };
        var C = 0;
        columns.forEach(function (c) {
            var v = c.displayName || c.value || columns[i].name;
            addCell(range, v, 0, C, ws);
            C++;
        }, this);
        var R = 1;
        data.forEach(function (ds) {
            C = 0;
            ds.forEach(function (d) {
                var v = d.value;
                addCell(range, v, R, C, ws);
                C++;
            });
            R++;
        }, this);
        if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
        return ws;
    }
    /**
     * 
     * @param {*} data 
     * @param {*} columns 
     */

    function datenum(v, date1904) {
        if (date1904) v += 1462;
        var epoch = Date.parse(v);
        return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
    }

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    function addCell(range, value, row, col, ws) {
        if (range.s.r > row) range.s.r = row;
        if (range.s.c > col) range.s.c = col;
        if (range.e.r < row) range.e.r = row;
        if (range.e.c < col) range.e.c = col;
        var cell = {
            v: value
        };
        if (cell.v == null) cell.v = '';
        var cell_ref = XLSX.utils.encode_cell({
            c: col,
            r: row
        });

        if (typeof cell.v === 'number') cell.t = 'n';
        else if (typeof cell.v === 'boolean') cell.t = 'b';
        else if (cell.v instanceof Date) {
            cell.t = 'n';
            cell.z = XLSX.SSF._table[14];
            cell.v = datenum(cell.v);
        } else cell.t = 's';

        ws[cell_ref] = cell;
    }
}