

(function () {
    'use strict';
    angular
        .module('app')
        .controller('TotalStrengthController', TotalStrengthController)

    TotalStrengthController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function TotalStrengthController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.students = [];
        $scope.totstr = false;
        $scope.tadprint = false;
        $scope.excel_flag = true;
        $scope.objj = {};
        $scope.searchValue = '';
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;
        $scope.sortKey100 = 'ASMCL_Order';
        //if ($scope.sortKey100 == 'ASMCL_Order') {
        //    $scope.sortKey100 = 'ASMC_Order'
        //}



        $scope.ddate = {};
        $scope.objj = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        //   $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        $scope.sortReverse = false;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            //     $scope.itemsPerPage = 10;
            var pageid = 1;
            apiService.get("TotalStrength/getdetails", pageid).
                then(function (promise) {

                    $scope.yearlist = promise.allAcademicYear;
                    $scope.classList = promise.allClass;
                    $scope.categoryDropdown = promise.category_list;
                    $scope.categoryflag = promise.categoryflag;
                    //$scope.sectionList = promise.allSection;
                });
        };

        $scope.min = function (arr) {
            return $filter('min')
                ($filter('map')(arr, 'ASMCL_Order'));
        };

        $scope.getVolumeSumgirls = function (items) {
            return items
                .map(function (x) { return x.girls; })
                .reduce(function (a, b) { return a + b; });
        };
        //getVolumeSumtotalsubjects
        $scope.getVolumeSumtotalsubjects = function (items) {

            $scope.ASMCL_Id = items[0].ASMCL_Id;
            return items.ASMCL_Id;
            //.map(function (x) { return x.ASMCL_Id; })
            //.reduce(function (x) { return x.ASMCL_Id });
        };
        $scope.getVolumeSumboys = function (items) {

            return items
                .map(function (x) { return x.boys; })
                .reduce(function (a, b) { return a + b; });
        };

        $scope.getVolumeNewadmission = function (items) {

            return items
                .map(function (x) { return x.New_admission; })
                .reduce(function (a, b) { return a + b; });
        };

        $scope.getVolumeSumtotal = function (items) {

            return items
                .map(function (x) { return x.total; })
                .reduce(function (a, b) { return a + b; });
        };

        $scope.getVolumeSumpresent = function (items) {

            return items
                .map(function (x) { return x.present; })
                .reduce(function (a, b) { return a + b; });
        };

        $scope.getVolumeSumtctaken = function (items) {

            return items
                .map(function (x) { return x.tctaken; })
                .reduce(function (a, b) { return a + b; });
        };

        $scope.getVolumeSumnewadm = function (items) {

            return items
                .map(function (x) { return x.newadm; })
                .reduce(function (a, b) { return a + b; });
        };

        $scope.getVolumeSumntotal = function (items) {

            return items
                .map(function (x) { return x.total; })
                .reduce(function (a, b) { return a + b; });
        };

        $scope.toggleAll = function () {
            $scope.printstudents = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                    var tot_boy_print = 0;
                    for (var i = 0; i < $scope.printstudents.length; i++) {
                        tot_boy_print = tot_boy_print + $scope.printstudents[i].boys;

                    }
                    $scope.total_print_boy = tot_boy_print;



                    var total_girl_print = 0;
                    for (var i2 = 0; i2 < $scope.printstudents.length; i2++) {
                        total_girl_print = total_girl_print + $scope.printstudents[i2].girls;
                    }
                    $scope.total_print_girl = total_girl_print;


                    var sumtotoalboysandgirls_print = 0;
                    for (var i1 = 0; i1 < $scope.printstudents.length; i1++) {
                        sumtotoalboysandgirls_print = sumtotoalboysandgirls_print + $scope.printstudents[i1].boys + $scope.printstudents[i1].girls;

                    }
                    $scope.sumtotoalboysandgirls_export = sumtotoalboysandgirls_print;
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };

        $scope.exportToExcel = function () {
            var exportHref = "";
            if ($scope.ts.optstatus === "O") {
                exportHref = Excel.tableToExcel('#Table1', 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            } else {
                exportHref = Excel.tableToExcel('#Table', 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.ts.optstatus === "O") {
                innerContents = document.getElementById("Table1print").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();

            } else {

                innerContents = document.getElementById("printSectionId").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
        };

        $scope.radio_btn_function = function () {

            if ($scope.ts.optradio === 'all') {
                $scope.ts.asmaY_Id = "";
                $scope.cls_div = false;
                $scope.sec_div = false;
                $scope.totstr = false;
                $scope.submitted = false;
                //$scope.myform.$setPristine();
                //$scope.myform.$setUntouched();

            }
            else if ($scope.ts.optradio === 'indi') {
                $scope.cls_div = true;
                $scope.sec_div = true;
                $scope.totstr = false;
                $scope.ts.asmaY_Id = "";
                $scope.ts.asmcL_Id = "";
                $scope.ts.asmC_Id = "";
                $scope.submitted = false;
                //$scope.myform.$setPristine();
                //$scope.myform.$setUntouched();


            }
            else if ($scope.ts.optradio === 'indi') {
                $scope.cls_div = true;
                $scope.sec_div = true;
                $scope.totstr = false;
                $scope.ts.asmaY_Id = "";
                $scope.ts.asmcL_Id = "";
                $scope.ts.asmC_Id = "";
                $scope.submitted = false;
                //$scope.myForm.$setPristine();
                //$scope.myForm.$setUntouched();


            }
            else if ($scope.ts.optradio === 'indi') {
                $scope.cls_div = true;
                $scope.sec_div = true;
                $scope.totstr = false;
                $scope.ts.asmaY_Id = "";
                $scope.ts.asmcL_Id = "";
                $scope.ts.asmC_Id = "";
                $scope.submitted = false;
                //$scope.myForm.$setPristine();
                //$scope.myForm.$setUntouched();
            }
            else {
                swal("Oops!!Something Went Wrong");
            }
        };


        $scope.al_electives = function (all, ASMCL_Id) {

            $scope.arrseclist = [];

            $scope.electivelistarray = [];
            $scope.obj.usercheckCC = all;
            if ($scope.obj.usercheckC == true) {
                $scope.obj.usercheckC = false;
            }

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.electiveList, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.electiveList, function (qq) {
                if (qq.selected == true) {
                    $scope.electivelistarray.push({ ISMS_Id: qq.ISMS_Id })
                }
            });
        }


        $scope.getelectives = function () {
            $scope.electivelistarray = [];
            angular.forEach($scope.electiveList, function (qq) {
                if (qq.selected == true) {
                    $scope.electivelistarray.push({ ISMS_Id: qq.ISMS_Id })
                }
            });

        };

        $scope.submitted = false;
        $scope.Report = function (optradio) {

            $scope.printstudents = [];
            $scope.AllSubject = [];
            $scope.studentElectivelist = [];
            $scope.searchValue = '';
            //$scope: ASMAY_Id = $scope.asmaY_Id,
            if ($scope.myForm.$valid) {

                var acedamicId = $scope.ts.asmaY_Id;
                var amcId = $scope.ts.asmaY_Id;
                var sectionId = $scope.ts.asmC_Id;
                var classId = $scope.ts.asmcL_Id;
                var AMST_SOL = $scope.ts.optstatus;
                var withtc;
                var wtihdeactive;



                if (acedamicId === undefined || acedamicId === "") {
                    acedamicId = 0;
                }
                if (classId === undefined || classId === "") {
                    classId = 0;
                }
                if (sectionId === undefined || sectionId === "") {
                    sectionId = 0;
                }

                if ($scope.ts.withtc === true) {
                    withtc = 1;
                } else {
                    withtc = 0;
                }
                if ($scope.ts.withdeactive === true) {
                    wtihdeactive = 1;
                } else {
                    wtihdeactive = 0;
                }
                //electives

                $scope.Electivearray = [];
                angular.forEach($scope.electiveList, function (aa) {
                    if (aa.selected == true) {
                        $scope.Electivearray.push({ ISMS_Id: aa.ISMS_Id, ISMS_SubjectName: aa.ISMS_SubjectName })
                    }

                });

                var AMC_Id = 0
                if ($scope.objj.amC_Id != 0 && $scope.objj.amC_Id > 0) {
                    AMC_Id = $scope.objj.amC_Id
                }

    
                //var AMC_Id = 0
                //if ($scope.objj.amC_Id != 'All') {
                //    AMC_Id = $scope.objj.amC_Id
                //}

                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "Status_Flag": $scope.ts.optradio,
                    "AMST_SOL": AMST_SOL,
                    "withtc": withtc,
                    "withdeactive": wtihdeactive,
                    "AMC_Id": AMC_Id,
                    "Electivearray": $scope.Electivearray
                };

                apiService.create("TotalStrength/Studdetails", data)
                    .then(function (promise) {

                        if (promise.studentlist.length > 0) {
                            if ($scope.ts.optstatus !== "O") {
                                if (promise.gender1 === 'F') {
                                    $scope.girls1 = true;
                                    $scope.boys1 = false;
                                }
                                else if (promise.gender1 === 'M') {
                                    if (promise.totalgender == 1) {
                                        $scope.girls1 = false;
                                        $scope.boys1 = true;
                                    }
                                    else {
                                        $scope.girls1 = true;
                                        $scope.boys1 = true;
                                    }
                                }

                                $scope.students = promise.studentlist;
                                $scope.studentsele = promise.studentElectivelist;
                                $scope.presentCountgrid = $scope.students.length;
                                $scope.totoalboys();
                                $scope.totoalgirls();
                                $scope.totalboysandgirls();
                                $scope.totoalnewadmn();
                                $scope.excel_flag = false;
                                console.log(promise.studentlist);
                                $scope.totstr = true;

                            }
                            else {
                                $scope.students = promise.studentlist;
                                $scope.studentsele = promise.studentElectivelist;
                                $scope.presentCountgrid = $scope.students.length;
                                $scope.totoalpresent();
                                $scope.totoaltctaken();
                                $scope.totoalnewadmn();
                                $scope.overallsum();
                                $scope.totstr1 = true;
                            }


                            angular.forEach($scope.yearlist, function (dd) {

                                if (parseInt(dd.asmaY_Id) === parseInt($scope.ts.asmaY_Id)) {
                                    $scope.yearname = dd.asmaY_Year;
                                }
                            });

                            if (promise.AMC_logo != null) {
                                $scope.imgname = promise.AMC_logo[0].amC_FilePath;
                            }
                            else {
                                $scope.imgname = logopath;
                            }

                            //AllSubject
                            //students

                            $scope.ElectiveSum = [];
                            $scope.AllSubject = promise.allSubject;
                            $scope.studentElectivelist = promise.studentElectivelist;
                            $scope.ElectiveSum = promise.electiveSum;
                            //
                            $scope.studentDropdown23 = [];
                            angular.forEach($scope.students, function (dev) {
                                if ($scope.studentDropdown23.length === 0) {
                                    $scope.studentDropdown23.push(dev);
                                }

                                else if ($scope.studentDropdown23.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.studentDropdown23, function (emp) {
                                        if (emp.ASMCL_Id === dev.ASMCL_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.studentDropdown23.push(dev);
                                    }
                                }
                            });
                        }
                        else {
                            swal("Record Not Found");
                            $scope.students = null;
                            $scope.totstr = false;
                            $scope.excel_flag = true;
                        }
                    });
            }
            else {
                $scope.submitted = true;
                $scope.totstr = false;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
            $scope.ts.optradio = "";
            $scope.ts.asmaY_Id = "";
            $scope.ts.asmaY_Ids = "";
            $scope.ts.asmaY_Idl = "";
            $scope.ts.asmC_Id = "";
            $scope.ts.asmcL_Id = "";
            $scope.submitted = false;
            $scope.totstr = false;
            $scope.excel_flag = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort100 = function (key) {
            $scope.reverse100 = ($scope.sortKey100 === key) ? !$scope.reverse100 : $scope.reverse100;
            $scope.sortKey100 = key;
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);

                var tot_boy_print = 0;
                for (var i = 0; i < $scope.printstudents.length; i++) {
                    tot_boy_print = tot_boy_print + $scope.printstudents[i].boys;

                }
                $scope.total_print_boy = tot_boy_print;



                var total_girl_print = 0;
                for (var i1 = 0; i1 < $scope.printstudents.length; i1++) {
                    total_girl_print = total_girl_print + $scope.printstudents[i1].girls;
                }
                $scope.total_print_girl = total_girl_print;


                var sumtotoalboysandgirls_print = 0;
                for (var i2 = 0; i2 < $scope.printstudents.length; i2++) {
                    sumtotoalboysandgirls_print = sumtotoalboysandgirls_print + $scope.printstudents[i2].boys + $scope.printstudents[i2].girls;

                }
                $scope.sumtotoalboysandgirls_export = sumtotoalboysandgirls_print;
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

        //sum of boys
        $scope.totoalboys = function () {
            var sumtotoalboys = 0;
            for (var i = 0; i < $scope.students.length; i++) {
                sumtotoalboys = sumtotoalboys + $scope.students[i].boys;
            }
            $scope.total = sumtotoalboys;
        };

        //sum of girls
        $scope.totoalgirls = function () {
            var sumtotoalgirls = 0;
            for (var i = 0; i < $scope.students.length; i++) {
                sumtotoalgirls = sumtotoalgirls + $scope.students[i].girls;
            }
            $scope.total1 = sumtotoalgirls;
        };

        //sum of totalboysandgirls
        $scope.totalboysandgirls = function () {
            var sumtotoalboysandgirls = 0;
            for (var i = 0; i < $scope.students.length; i++) {
                sumtotoalboysandgirls = sumtotoalboysandgirls + $scope.students[i].boys + $scope.students[i].girls;
            }
            $scope.total2 = sumtotoalboysandgirls;
        };


        //sum of present
        $scope.totoalpresent = function () {
            var sumtotoalpresent = 0;
            for (var i = 0; i < $scope.students.length; i++) {
                sumtotoalpresent = sumtotoalpresent + $scope.students[i].present;
            }
            $scope.totalpresent = sumtotoalpresent;
        };

        //sum of tctaken
        $scope.totoaltctaken = function () {
            var sumtotoaltctaken = 0;
            for (var i = 0; i < $scope.students.length; i++) {
                sumtotoaltctaken = sumtotoaltctaken + $scope.students[i].tctaken;
            }
            $scope.totaltctaken = sumtotoaltctaken;
        };

        //sum of new admission
        $scope.totoalnewadmn = function () {
            var sumtotoalnewadm = 0;
            for (var i = 0; i < $scope.students.length; i++) {
                sumtotoalnewadm = sumtotoalnewadm + $scope.students[i].New_admission;
            }
            $scope.totalnewadm = sumtotoalnewadm;
        };

        //over all sum
        $scope.overallsum = function () {
            var sumtotoaloverall = 0;
            for (var i = 0; i < $scope.students.length; i++) {
                sumtotoaloverall = sumtotoaloverall + $scope.students[i].total;
            }
            $scope.overalltotal = sumtotoaloverall;
        };

        $scope.getelective = function () {
            var acedamicId = $scope.ts.asmaY_Id;
            var sectionId = $scope.ts.asmC_Id;
            var classId = $scope.ts.asmcL_Id;
            var AMST_SOL = $scope.ts.optstatus;
            var withtc;
            var wtihdeactive;


            var data = {
                "ASMAY_Id": acedamicId,

            };
            apiService.create("TotalStrength/getelective", data).then(function (promise) {
                $scope.electiveList = promise.allElectives;
            });
        };

        $scope.getsection = function () {
            var acedamicId = $scope.ts.asmaY_Id;
            var sectionId = $scope.ts.asmC_Id;
            var classId = 0;
            if ($scope.ts.asmcL_Id > 0) {
                classId = $scope.ts.asmcL_Id;
            }
            $scope.electiveList = [];
            var AMST_SOL = $scope.ts.optstatus;
            //var withtc;
            //var wtihdeactive;


            var data = {
                "ASMAY_Id": acedamicId,
                "ASMCL_Id": classId,
                "AMC_Id": $scope.objj.amC_Id
            };
            apiService.create("TotalStrength/getsection", data).then(function (promise) {
                $scope.sectionList = promise.allSection;
                $scope.electiveList = promise.allElectives;
            });
        };



        $scope.getclass = function () {
            var acedamicId = $scope.ts.asmaY_Id;



            var data = {
                "ASMAY_Id": acedamicId,
                "AMC_Id": $scope.objj.amC_Id
            };
            apiService.create("TotalStrength/getclass", data).then(function (promise) {
                $scope.classList = promise.allClass;
                $scope.electiveList = promise.allElectives;

            });
        };


        $scope.getelectiveSele = function (ASMCL_Id) {
            //added by roopa//
            $scope.electivelistarray = [];
            angular.forEach($scope.electiveList, function (aa) {
                if (aa.selected == true) {
                    $scope.electivelistarray.push({ ISMS_Id: aa.ISMS_Id })
                }

            });
        }

        $scope.al_checkelective = function (all, ASMCL_Id) {
            $scope.electivelistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.electiveList, function (role) {
                role.selected = toggleStatus;
            });

            $scope.electivelistarray = [];
            angular.forEach($scope.electiveList, function (qq) {
                if (qq.selected == true) {
                    $scope.electivelistarray.push({ ISMS_Id: qq.ISMS_Id })
                }
            });
        }
    }

})();