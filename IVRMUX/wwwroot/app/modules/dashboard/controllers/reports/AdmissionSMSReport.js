(function () {
    'use strict';
    angular
        .module('app')
        .controller('AdmissionSMSReportController', AdmissionSMSReportController)

    AdmissionSMSReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function AdmissionSMSReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {


        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.show1 = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.obj.dailydate = new Date();
        $scope.obj.fromdate = new Date();
        $scope.obj.todate = new Date();


        $scope.Toggle_header = function () {

            var toggleStatus1 = $scope.all2;
            angular.forEach($scope.moduledata, function (itm) {
                itm.selected = toggleStatus1;

            });
        }
        $scope.addColumn = function (role) {

            $scope.all2 = $scope.moduledata.every(function (itm) { return itm.selected; });
        };

        $scope.loaddata = function () {

            // $scope.moduledata = promise.moduledetails;
            $scope.columnsTest = [];
            $scope.todate3 = new Date();
            $scope.maxDateftodate = new Date(
                $scope.todate3.getFullYear(),
                $scope.todate3.getMonth(),
                $scope.todate3.getDate());
            $scope.currentPage = 1;
            //$scope.itemsPerPage = 5
            var pageid = 0;
            apiService.getURI("AdmissionSMSReport/getalldetails/", pageid).
                then(function (promise) {
                    $scope.moduledata = promise.moduledetails;
                    console.log($scope.moduledata);

                });
        }

        $scope.Toggle_header = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.moduledata, function (itm) {
                itm.selected = toggleStatus;
            });
        }


        $scope.betweendates = true;
        $scope.daily = true;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
            if ($scope.students.length === 0 && $scope.printstudents.length > 0) {
                angular.forEach($scope.printstudents, function (itm) {
                    $scope.printstudents.splice(itm);
                });
            }
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }
        //all or individual
        $scope.onclickloaddata = function () {
            if ($scope.allorindiv == "All") {
                $scope.submitted = false;
                $scope.show1 = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

                $scope.Toggle_header();

            }
            else if ($scope.allorindiv === "indi") {
                $scope.submitted = false;
                $scope.show1 = true;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;
            }
            $scope.students = [];
        };

        //datewise or between dates
        $scope.onclickdates = function () {
            if ($scope.dailybtedates == "daily") {
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.betweendates = true;
                $scope.daily = false;
                $scope.reporsmart = false;
            }
            else if ($scope.dailybtedates === "btwdates") {
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.todatef = true;
                $scope.betweendates = false;
                $scope.daily = true;
                $scope.reporsmart = false;

            }
        };

        //modules radio button
        $scope.onclickmodule = function () {
            if ($scope.regorname == "email") {
                $scope.moduleallorind = true;
                $scope.reporsmart = false;
            }
            else if ($scope.regorname === "sms") {
                $scope.moduleallorind = false;
                $scope.reporsmart = false;
            }
        };

        //reg or name wise binding 
        $scope.onclickregnoname = function (obj) {
            var data = {
                "regornamedetails": obj.regorname
            }
            apiService.create("AdmissionSMSReport/getnameregno", data).
                then(function (promise) {
                    $scope.studentlst = promise.studentlist;
                    $scope.reporsmart = false;
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.submitted = false;

        //report
        $scope.getreport = function (obj) {

            $scope.all = "";
            //  $scope.toggleAll();
            $scope.students = [];
            $scope.albumNameArray1 = [];
            if ($scope.myForm.$valid) {
                if ($scope.allorindiv == null || $scope.allorindiv == undefined || $scope.dailybtedates == null || $scope.dailybtedates == undefined) {
                    swal("Kindly Select The Options !!");
                }
                else {

                    angular.forEach($scope.moduledata, function (role) {
                        if (role.selected == true) {
                            $scope.albumNameArray1.push(role.ivrmM_ModuleName);
                        }
                    })

                    var dailydate1 = "";
                    var fromdate1 = "";
                    var todate1 = "";

                    if ($scope.dailybtedates == "daily") {
                        fromdate1 = new Date().toDateString();
                        todate1 = new Date().toDateString();
                        dailydate1 = new Date($scope.obj.dailydate).toDateString();
                    }
                    else {
                        fromdate1 = new Date($scope.obj.fromdate).toDateString();
                        todate1 = new Date($scope.obj.todate).toDateString();
                        dailydate1 = new Date().toDateString();
                    }

                    var data = {
                        "onclickloaddata": $scope.allorindiv,
                        "regorname": $scope.obj.regorname,
                        "dailydate": dailydate1,
                        "dailybtedates": $scope.dailybtedates,
                        "fromdate": fromdate1,
                        "todate": todate1,
                        "mdata": $scope.albumNameArray1
                    }

                    if ($scope.allorindiv == "All") {

                        if ($scope.dailybtedates == "daily") {
                            if ($scope.obj.dailydate == null || $scope.obj.dailydate == undefined) {
                                swal("Select The Required Fields !!");
                                $scope.reporsmart = false;
                            }
                            else {
                                apiService.create("AdmissionSMSReport/Getreportdetails", data).
                                    then(function (promise) {
                                        if (promise.messagelist.length == 0) {
                                            $scope.reporsmart = false;
                                            swal("Record Not Found !!");
                                            $state.reload();

                                        }
                                        else {
                                            $scope.students = promise.messagelist;
                                            $scope.presentCountgrid = $scope.students.length;
                                            $scope.reporsmart = true;
                                            $scope.exp_excel_flag = false;
                                            $scope.print_flag = false;
                                            $scope.name_display = $scope.obj.regorname;
                                        }
                                    })
                            }
                        }

                        else if ($scope.dailybtedates === "btwdates") {

                            if ($scope.obj.todate == null || $scope.obj.todate == undefined || $scope.obj.fromdate == null || $scope.obj.fromdate == undefined) {
                                $scope.reporsmart = false;
                                swal("Select The Required Fields !!");
                            }
                            else {
                                apiService.create("AdmissionSMSReport/Getreportdetails", data).
                                    then(function (promise) {
                                        if (promise.messagelist.length > 0) {
                                            $scope.reporsmart = true;
                                            $scope.students = promise.messagelist;
                                            $scope.presentCountgrid = $scope.students.length;
                                            $scope.exp_excel_flag = false;
                                            $scope.print_flag = false;
                                        }
                                        else {
                                            $scope.reporsmart = false;
                                            swal("Record Not Found !!");
                                            $state.reload();
                                        }
                                    })
                            }
                        }
                    }

                    else if ($scope.allorindiv === "indi") {

                        if ($scope.dailybtedates == "daily") {

                            if ($scope.obj.dailydate == null || $scope.obj.dailydate == undefined) {
                                swal("Select The Required Fields !!");
                                $scope.reporsmart = false;
                            }
                            else {
                                apiService.create("AdmissionSMSReport/Getreportdetails", data).
                                    then(function (promise) {
                                        if (promise.messagelist.length > 0) {
                                            $scope.reporsmart = true;
                                            $scope.students = promise.messagelist;
                                            $scope.presentCountgrid = $scope.students.length;
                                            $scope.exp_excel_flag = false;
                                            $scope.print_flag = false;
                                        }
                                        else {
                                            swal("Record Not Found !!");
                                            $state.reload();
                                            $scope.reporsmart = false;
                                        }
                                    })
                            }
                        }


                        else if ($scope.dailybtedates === "btwdates") {

                            if ($scope.obj.todate == null || $scope.obj.todate == undefined || $scope.obj.fromdate == null || $scope.obj.fromdate == undefined) {
                                swal("Select The Required Fields !!");
                                $scope.reporsmart = false;
                            }
                            else {
                                apiService.create("AdmissionSMSReport/Getreportdetails", data).
                                    then(function (promise) {

                                        if (promise.messagelist.length > 0) {
                                            $scope.reporsmart = true;
                                            $scope.students = promise.messagelist;
                                            $scope.presentCountgrid = $scope.students.length;
                                            $scope.exp_excel_flag = false;
                                            $scope.print_flag = false;
                                        }
                                        else {
                                            swal("Record Not Found !!");
                                            $state.reload();
                                            $scope.reporsmart = false;
                                        }
                                    })
                            }

                        }

                        else {
                            swal("Record Not Found !!");
                            $scope.reporsmart = false;
                            $state.reload();
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                        }
                    }
                    else {
                        $scope.reporsmart = false;
                        $state.reload();
                        swal("Record Not Found !!");
                    }
                }
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.Toggle_header = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.moduledata, function (itm) {
                itm.selected = toggleStatus;
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        //fromdate to date validation
        $scope.validatetodate = function (data1) {
            $scope.todatef = false;
            $scope.todate1 = data1.fromdate;
            $scope.minDatet = new Date(
                $scope.todate1.getFullYear(),
                $scope.todate1.getMonth(),
                $scope.todate1.getDate() + 1);

            $scope.maxDatet = new Date(
                $scope.todate1.getFullYear(),
                $scope.todate1.getMonth(),
                $scope.todate1.getDate() + 1);

        }

        $scope.validatetodatetoo = function (datato) {

            $scope.todate2 = datato.todate;
            $scope.minDatet = new Date(
                $scope.todate2.getFullYear(),
                $scope.todate2.getMonth(),
                $scope.todate2.getDate() + 1);
        }

        $scope.validatetoday = function (datatodate) {
            $scope.todate3 = datatodate.dailydate;
            $scope.maxDateftodate = new Date(
                $scope.todate3.getFullYear(),
                $scope.todate3.getMonth(),
                $scope.todate3.getDate() + 1);
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.Name).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.Class).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.Section).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.Modulename).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                ($filter('date')(obj.Mdate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0);
        }

        //$scope.printData = function (printSectionId) {
        //    if ($scope.printstudents !== null && $scope.students.length > 0) {
        //        var innerContents = document.getElementById("printSectionId").innerHTML;
        //        var popupWinindow = window.open('');
        //        popupWinindow.document.open();
        //        popupWinindow.document.write('<html><head>' +
        //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //  '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //   '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
        //  '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //  );
        //        popupWinindow.document.close();
        //    }
        //    else {
        //        swal("Please Select Records to be Printed");
        //    }
        //}



        //$scope.printData = function (printEmailId) {
        //    if ($scope.printstudents !== null && $scope.students.length > 0) {
        //        var innerContents = document.getElementById("printEmailId").innerHTML;
        //        var popupWinindow = window.open('');
        //        popupWinindow.document.open();
        //        popupWinindow.document.write('<html><head>' +
        //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //  '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //   '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
        //  '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //  );
        //        popupWinindow.document.close();
        //    }
        //    else {
        //        swal("Please Select Records to be Printed");
        //    }
        //}



        $scope.printData = function () {
            if ($scope.printstudents !== null && $scope.students.length > 0) {
                var innerContents = "";
                if ($scope.allorindiv == "All" && $scope.obj.regorname == "sms") {
                    innerContents = document.getElementById("printSMSId").innerHTML;
                }
                else if ($scope.allorindiv == "All" && $scope.obj.regorname == "email") {
                    innerContents = document.getElementById("printEmailId").innerHTML;
                }
                else if ($scope.allorindiv == "indi" && $scope.obj.regorname == "sms") {
                    innerContents = document.getElementById("printSectionId").innerHTML;
                }
                else if ($scope.allorindiv == "indi" && $scope.obj.regorname == "email") {
                    innerContents = document.getElementById("printSectionId").innerHTML;
                }
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
            else {
                swal("Please Select Records to be Printed");
            }
        }



        //$scope.printData1 = function () {

        //    var printSectionId = '';
        //    if ($scope.allorindiv == 'indi' && $scope.students.length > 0) {
        //        printSectionId = '#printSectionId';
        //    }
        //    else if ($scope.allorindiv == 'All' && $scope.obj.regorname == 'sms' && $scope.students.length > 0) {
        //        printSectionId = '#printEmailId';
        //    }
        //    else if ($scope.allorindiv == 'All' && $scope.obj.regorname == 'email' && $scope.students.length > 0) {
        //        printSectionId = '#printSMSId';
        //    }
        //    if ($scope.printstudents !== null && $scope.students.length > 0) {
        //        var innerContents = document.getElementById("printSectionId").innerHTML;
        //        var popupWinindow = window.open('');
        //        popupWinindow.document.open();
        //        popupWinindow.document.write('<html><head>' +
        //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //  '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //   '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
        //  '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //  );
        //        popupWinindow.document.close();
        //    }
        //    else {
        //        swal("Please Select Records to be Printed");
        //    }
        //}










        $scope.exportToExcel = function () {
            var printSectionId = '';
            if ($scope.allorindiv == 'indi' && $scope.students.length > 0) {
                printSectionId = '#table1';
            }
            else if ($scope.allorindiv == 'All' && $scope.obj.regorname == 'sms' && $scope.students.length > 0) {
                printSectionId = '#table2';
            }
            else if ($scope.allorindiv == 'All' && $scope.obj.regorname == 'email' && $scope.students.length > 0) {
                printSectionId = '#table3';
            }
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        //$scope.exportToExcel = function (printEmailId) {
        //    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
        //        var exportHref = Excel.tableToExcel(printEmailId, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //        //$state.reload();
        //    }
        //    else {
        //        swal("Please Select Records to be Exported");
        //    }

        //}


        //$scope.exportToExcel = function (printSMSId) {
        //    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
        //        var exportHref = Excel.tableToExcel(printSMSId, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //        //$state.reload();
        //    }
        //    else {
        //        swal("Please Select Records to be Exported");
        //    }

        //}



        $scope.validreport = function () {
            //  if($scope.obj.regorname=='email')
            // {
            $scope.students = [];
            //  }
        }
    }

}
)();













//        $scope.searchValue = '';

//        $scope.loaddata = function () {
//            $scope.currentPage = 1;
//            // $scope.itemsPerPage = 10;
//            $scope.printstudents = [];

//            apiService.getURI("AdmissionSMSReport/getalldetails/", pageid).then(function (promise) {

//                $scope.moduledata = promise.moduledetails;
//                //$scope.yearDropdown = promise.academicList;
//                //$scope.classDropdown = promise.classlist;
//                //$scope.sectionDropdown = promise.sectionList;


//            });
//        }
//        $scope.Toggle_header = function () {
//            
//            var toggleStatus = $scope.all2;
//            angular.forEach($scope.moduledata, function (itm) {
//                itm.selected = toggleStatus;
//            });
//        }


//        $scope.userPrivileges = "";
//        var pageid = $stateParams.pageId;

//        $scope.ddate = {};
//        $scope.ddate = new Date();

//        $scope.usrname = localStorage.getItem('username');
//        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

//        var paginationformasters;
//        var copty;
//        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
//        if (ivrmcofigsettings.length > 0) {
//            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
//            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
//        }

//        $scope.itemsPerPage = paginationformasters;
//        $scope.coptyright = copty;

//        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
//        if (admfigsettings.length > 0) {
//            var logopath = admfigsettings[0].asC_Logo_Path;
//        }

//        $scope.imgname = logopath;



//        $scope.onclickloaddata = function () {

//        };

//        var privlgs = JSON.parse(localStorage.getItem("privileges"));
//        for (var i = 0; i < privlgs.length; i++) {
//            if (privlgs[i].pageId == pageid) {
//                $scope.userPrivileges = privlgs[i];
//            }
//        }

//        $scope.catreport = true;
//        $scope.submitted = false;

//        $scope.columnTotal = [];
//        $scope.angularData =
//          {
//              'nameList': []
//          };

//        $scope.vals = [];
//        $scope.getreport = function () {
//            $scope.printstudents = [];
//            $scope.searchValue = '';
//            if ($scope.myForm.$valid) {
//                var data = {
//                    "IVRMM_Id": $scope.ivrmM_Id,
//                     "IVRM_SSB_ID":$scope.ivrM_SSB_ID,
//                      "IVRMESB_ID":$scope.ivrMESB_ID,

//                }
//                apiService.create("AdmissionSMSReport/getAttendetails", data)
//                    .then(function (promise) {
//                        
//                        $scope.students = promise.messagelist;
//                        $scope.presentCountgrid = $scope.students.length;
//                        for (var i = 0; i < promise.messagelist.length; i++) {
//                            var email = promise.messagelist[i].EmailId;
//                            $scope.vals.push(email);
//                        }
//                        //angular.forEach($scope.vals, function (v, k) {
//                        //    $scope.angularData.nameList.push({
//                        //        'fullname': v
//                        //    });
//                        //});
//                        //var j = 0;
//                        //angular.forEach($scope.students, function (obj) {
//                        //    //Using bracket notation
//                        //    obj["fullname"] = $scope.angularData.nameList[j].fullname;
//                        //    j++;
//                        //});
//                        //angular.forEach($scope.students, function (objectt) {
//                        //    if (objectt.fullname.length > 0) {
//                        //        var string = objectt.fullname
//                        //        objectt.namme = string.replace(/  +/g, ' ');
//                        //    }
//                        //})
//                        //angular.forEach($scope.columnsTest, function (value1, i) {
//                        //    var clmttl = $scope.columnsTest[i].TOTAL_classheld;
//                        //    $scope.columnTotal.push(clmttl);
//                        //});
//                        //angular.forEach($scope.students, function (value1, i) {
//                        //    $scope.students[i].totalval = 0;
//                        //});

//                        //$scope.totalList = [];
//                        

//                        if (promise.messagelist == null || promise.messagelist.length == 0) {
//                            swal('No Records Found!');
//                            $scope.catreport = true;
//                        }
//                        else {
//                            $scope.catreport = false;
//                            angular.forEach($scope.totalList, function (value1, i) {
//                                $scope.students[i].totalval = $scope.totalList[i];
//                            });
//                            console.log($scope.students);
//                        }
//                    })
//            } else {
//                $scope.submitted = true;
//            }
//        }

//        $scope.toggleAll = function () {
//            
//            var toggleStatus = $scope.all;
//            angular.forEach($scope.filterValue1, function (itm) {
//                itm.selected = toggleStatus;
//                if ($scope.all == true) {
//                    if ($scope.printstudents.indexOf(itm) === -1) {
//                        $scope.printstudents.push(itm);
//                    }
//                }
//                else {
//                    $scope.printstudents.splice(itm);
//                }
//            });
//        }

//        //Radio button switching function
//        $scope.onclickdates = function () {
//            if ($scope.dailybtedates === 'daily') {
//                $scope.ASMAY = "";
//                $scope.frdatetodate = true;
//                $scope.print_flag = false;
//                $scope.sms_flag = false;
//                $scope.mail_flag = false;
//                $scope.export_flag = false;
//                $scope.IsHiddendown = false;
//                $scope.submitted = false;

//            }
//            else if ($scope.dailybtedates === 'btwdates') {

//                $scope.errMessage = "";
//                $scope.obj.FromDate = "";
//                $scope.obj.ToDate = "";
//                $scope.frdatetodate = false;
//                $scope.print_flag = false;
//                $scope.sms_flag = false;
//                $scope.mail_flag = false;
//                $scope.export_flag = false;
//                $scope.submitted = false;
//                $scope.IsHiddendown = false;
//            }
//        };


//        $scope.optionToggled = function (SelectedStudentRecord, index) {
//            
//            $scope.all = $scope.students.every(function (itm)
//            { return itm.selected; });
//            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
//                $scope.printstudents.push(SelectedStudentRecord);
//            }
//            else {
//                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
//            }
//        }

//        $scope.sort = function (keyname) {
//            $scope.sortKey = keyname;   //set the sortKey to the param passed
//            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
//        }

//        $scope.Clearid = function () {
//            $scope.asmaY_Id = "";
//            $scope.asmcL_Id = "";
//            $scope.asmC_Id = "";
//            $scope.submitted = false;
//            $scope.catreport = true;
//            $scope.myForm.$setPristine();
//            $scope.myForm.$setUntouched();
//        }

//        $scope.interacted = function (field) {
//            return $scope.submitted;
//        };

//        $scope.printData = function (printSectionId) {
//            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
//                var innerContents = document.getElementById("printSectionId").innerHTML;
//                var popupWinindow = window.open('');
//                popupWinindow.document.open();
//                popupWinindow.document.write('<html><head>' +
//                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
//            '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
//             '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
//            '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
//            );
//                popupWinindow.document.close();
//            }
//            else {
//                swal("Please Select Records to be Printed");
//            }
//        }

//        $scope.exportToExcel = function (export_id) {
//            
//            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
//                var exportHref = Excel.tableToExcel(export_id, 'WireWorkbenchDataExport');

//                $timeout(function () {
//                    location.href = exportHref;
//                }, 100);
//            }
//            else {
//                swal("Please Select Records to be Printed");
//            }
//        }
//    }
//})();
