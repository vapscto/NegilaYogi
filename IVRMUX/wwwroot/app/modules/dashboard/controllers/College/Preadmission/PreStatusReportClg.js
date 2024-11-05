
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TotalCountClgController', TotalCountClgController)

    TotalCountClgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'Excel', '$compile', '$timeout', 'superCache']
    function TotalCountClgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, Excel, $compile, $timeout, superCache) {

        // Load initial data

        $scope.detialsGrid = false;
        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#34495E",
                "#85C1E9",
                "#DAF7A6",
                "#FFC300",
                "#FF5733",
            ]);

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        $scope.obj = {};
        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (configsettings !== null && configsettings.length > 0) {
            var emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            var mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;

            $scope.emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            $scope.mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;

            if ($scope.emailotp == 1) {
                $scope.emailotpshow = true;
            }
            else {
                $scope.emailotpshow = false;
            }

            if ($scope.mobileotp == 1) {
                $scope.mobileotpshow = true;
            }
            else {
                $scope.mobileotpshow = false;
            }

            $scope.obj.email = false;
            $scope.obj.sms = false;
            var mobilenofull = configsettings[0].ivrmgC_OTPMobileNo.toString();
            if (mobilenofull != '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }
            var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
            if (emailidforotp != null || emailidforotp != undefined) {
                $scope.emailid = emailidforotp.substring(0, 4);
            }


            if ($scope.emailotp == 1 || $scope.mobileotp == 1) {
                $scope.otpcheck = true;

            }
            else {
                $scope.otpcheck = false;
            }
        }
        else {
            var emailotp = 0;
            var mobileotp = 0;

        }
        $scope.printDataadd = function () {
            var innerContents = document.getElementById("SRKVSStudentAddressBook").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet"  href="css/print/SRKVSStudentAddressBook/SRKVSStudentAddressBookPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.otpmobile = false;
        $scope.otpemail = false;
        $scope.buttonotp = false;
        $scope.smsshow = false;
        $scope.emailshow = false;
        $scope.ondropdown = false;
        $scope.resendotpbutton = false;
        $scope.page2 = "pag2";
        $scope.page3 = "pag3";

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 5;

        $scope.user = {};
        $scope.remarks = [];
        $scope.rptStatus = false;
        $scope.angularData = {
            'nameList': []
        };

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sortKey = "pacA_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.IsHidden = true;
        // $scope.IsHidden1 = false;

        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        //  $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.printdatatable = [];
        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all2;
            angular.forEach($scope.studentlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    if ($scope.printdatatable.indexOf(itm) === -1) {
                        $scope.printdatatable.push(itm);
                    }
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });

        }

        $scope.selected = function (SelectedStudentRecord, index) {


            $scope.all2 = $scope.studentlist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        }

        $scope.loadbasicdata = function () {

            apiService.get("TotalCountClg/getstatusdata/").then(function (promise) {
                if (promise.academicList != undefined && promise.academicList.length > 0) {
                    $scope.academiclist = promise.academicList;
                    $scope.yearid = $scope.academiclist[0].asmaY_Id;
                    $scope.courselist = promise.courselist;
                    $scope.statuslist = promise.statuslist;

                    $scope.onclickloaddata();

                }
                else if (promise.isSessionExpired == true) {
                    $cookieStore.remove('pagetitle');
                    $cookieStore.put("pagetitle", "Home");
                    $cookieStore.remove('IsLogged');
                    apiService.getURI("Login/clearsession", 1).
                        then(function (promise) {
                            //$state.go("login");
                            if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                                window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                            }
                            else {
                                window.location.href = "http://localhost:57606/#/login/";
                            }

                        })
                    swal("Your Session has been Expired.....Please Re-login");

                }
            });
        }

       




        $scope.search = '';
        $scope.filterOnLocation = function (user) {
            if ($scope.search != "") {
                $scope.ondropdown = true;
            }
            else {
                $scope.ondropdown = false;
            }
            (angular.lowercase(obj.User_Name)).indexOf(angular.lowercase($scope.searchValue))
            return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(user.statusName)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.pacA_RegistrationNo)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.remark)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.pacA_Sex)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.className)).indexOf(angular.lowercase($scope.search)) >= 0;

        };

        // search student based on year, class and status
        $scope.submitted = false;

        $scope.searchuser = function () {
            $scope.all2 = "";
            $scope.search = "";
            $scope.yearid = $scope.academiclist[0].asmaY_Id;

            if ($scope.myForm.$valid) {
                $scope.currentPage = 1;
                //$scope.itemsPerPage = paginationformasters;
                $scope.itemsPerPage = 5;
                if ($scope.asmaY_Id == "") {
                    $scope.asmaY_Id = 0;
                }
                if ($scope.pamsT_Id == "" || $scope.pamsT_Id == undefined) {
                    $scope.pamsT_Id = 0;
                }
                if ($scope.asmcL_Id == "") {
                    $scope.asmcL_Id = 0;
                }
                var statusall = "";
                if ($scope.pamsT_Id == '0') {
                    statusall = 'ALL';
                } else {
                    statusall = $scope.pamsT_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "PAMST_Id": $scope.pamsT_Id,
                    "status_type": $scope.status_type,
                    "status_all": statusall
                }

                apiService.create("TotalCountClg/SearchData", data).then(function (promise) {

                    if (promise.studentlistreport != null && promise.studentlistreport != undefined) {


                        if (configsettings != null) {
                            for (var i = 0; i < configsettings.length; i++) {
                                if (configsettings.length > 0) {
                                    $scope.configurationsettings = configsettings[i];
                                    if ($scope.configurationsettings.ispaC_ApplFeeFlag === 1) {

                                        $scope.ispaC_ApplFeeFlag = $scope.configurationsettings.ispaC_ApplFeeFlag;
                                        $scope.prosH = true;
                                        $scope.prosL = true;
                                    }
                                    else {
                                        $scope.ispaC_ApplFeeFlag = 0;
                                        $scope.prosH = true;
                                        $scope.prosL = true;
                                    }
                                }
                            }
                        }

                        $scope.studentlist = promise.studentlistreport;
                        $scope.prospectusPaymentlist = promise.prospectusPaymentlist;



                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < $scope.studentlist.length; i++) {
                            if ($scope.studentlist[i].pacA_FirstName != '') {
                                if ($scope.studentlist[i].pacA_MiddleName != null && $scope.studentlist[i].pacA_MiddleName != '' && $scope.studentlist[i].pacA_MiddleName != "") {
                                    if ($scope.studentlist[i].pacA_LastName != null && $scope.studentlist[i].pacA_LastName != '' && $scope.studentlist[i].pacA_LastName != "") {

                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pacA_FirstName + " " + $scope.studentlist[i].pacA_MiddleName + " " + $scope.studentlist[i].pacA_LastName, pacA_Id: $scope.studentlist[i].pacAId });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pacA_FirstName + " " + $scope.studentlist[i].pacA_MiddleName, pacA_Id: $scope.studentlist[i].pacA_Id });
                                    }
                                }
                                else {
                                    if ($scope.studentlist[i].pacA_LastName != null && $scope.studentlist[i].pacA_LastName != '' && $scope.studentlist[i].pacA_LastName != "") {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pacA_FirstName + " " + $scope.studentlist[i].pacA_LastName, pacA_Id: $scope.studentlist[i].pacA_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pacA_FirstName, pacA_Id: $scope.studentlist[i].pacA_Id });
                                    }
                                }

                                $scope.studentlist[i].name = $scope.albumNameArray1[i].name;

                                var str = $scope.studentlist[i].pacA_ConStreet;
                                var strReplacedWith = " ";
                                if (str != null && str != '') {
                                    var currentIndex = str.lastIndexOf(",");
                                    str = str.substring(0, currentIndex) + strReplacedWith + str.substring(currentIndex + 1, str.length);
                                    $scope.studentlist[i].pacA_ConStreet = str;
                                }

                                var str1 = $scope.studentlist[i].pacA_ConArea;
                                var strReplacedWith1 = " ";
                                if (str1 != null && str1 != '') {
                                    var currentIndex1 = str1.lastIndexOf(",");
                                    str1 = str1.substring(0, currentIndex1) + strReplacedWith1 + str1.substring(currentIndex1 + 1, str1.length);
                                    $scope.studentlist[i].pacA_ConArea = str1;
                                }

                            }
                        }

                        $scope.presentCountgrid = promise.studentlistreport.length;
                        $scope.showPrintB = true;
                        $scope.showExportB = true;
                        if ($scope.pamsT_Id == "0" && $scope.status_type === 'Appsts') {
                            $scope.abc = "787926";
                        }
                        else if ($scope.pamsT_Id == "0" && $scope.status_type === 'admsts') {
                            $scope.abc = "1";
                        }
                        else {
                            $scope.abc = $scope.pamsT_Id;
                        }
                        $scope.IsHidden1 = true;
                        $scope.doclist = [];
                        $scope.doclists = [];
                        $scope.doclist = promise.ddoc;


                     
                        if ($scope.status_type === 'Appsts') {
                            $scope.Student_status = 787926;
                        
                            $scope.Student_status = 786;
                          
                        }
                        $scope.studentcountreport = [];
                        // $scope.Fee_grid = true;
                        $scope.studentcountreport = promise.studentcountreport;
                        $scope.detialsGrid = true;
                        if ($scope.status_type === 'Appsts') {
                            if (($scope.studentcountreport != null && $scope.studentcountreport.length > 0)) {
                               
                                $scope.head = ["WATING", "REJECTED", "ACCEPTED"]                                $scope.overallgraph = [];                                $scope.overallgraph1 = [];                                var waiting = parseInt($scope.studentcountreport[0].applWaiting);                                var rejected = parseInt($scope.studentcountreport[0].applRejected);                                var accepted = parseInt($scope.studentcountreport[0].applAccepted);                                                           angular.forEach($scope.studentcountreport, function (o) {                                    $scope.overallgraph.push({ label: $scope.head[0], y: parseInt(o.applWaiting), name: "WATING" });                                    $scope.overallgraph1.push({ indexLabel: $scope.head[0], y: parseInt(o.applWaiting), name: "WATING" })                                    $scope.overallgraph.push({ label: $scope.head[1], y: parseInt(o.applRejected), name: "REJECTED" });                                    $scope.overallgraph1.push({ indexLabel: $scope.head[1], y: parseInt(o.applRejected), name: "REJECTED" });                                    $scope.overallgraph.push({ label: $scope.head[2], y: parseInt(o.applAccepted), name: "ACCEPTED" });                                    $scope.overallgraph1.push({ indexLabel: $scope.head[2], y: parseInt(o.applAccepted), name: "ACCEPTED" });                                    //$scope.overallgraph.push({ label: $scope.head[3], y: o.Adjusted, name: "ADJUSTMENT" });                                    //$scope.overallgraph1.push({ indexLabel: $scope.head[3], y: o.Adjusted, name: "ADJUSTMENT" });                                    //$scope.overallgraph.push({ label: $scope.head[4], y: o.Balance, name: "BALANCE" });                                    //$scope.overallgraph1.push({ indexLabel: $scope.head[4], y: o.Balance, name: "BALANCE" });                                })                                var chart = new CanvasJS.Chart("columnchart", {                                    animationEnabled: true,                                    animationDuration: 3000,                                    height: 350,                                    colorSet: "graphcolor",                                    axisX: {                                        labelFontSize: 12,                                    },                                    axisY: {                                        labelFontSize: 12,                                    },                                    toolTip: {                                        shared: true                                    },                                    data: [                                        {                                            type: "column",                                            showInLegend: false,                                            dataPoints: $scope.overallgraph                                        }                                    ]                                });                                chart.render();                                var chart = new CanvasJS.Chart("chartContainer", {                                    animationEnabled: true,                                    animationDuration: 3000,                                    height: 350,                                    colorSet: "graphcolor",                                    axisX: {                                        interval: 1,                                        labelFontSize: 12,                                    },                                    axisY: {                                        labelFontSize: 12,                                    },                                    data: [                                        {                                            type: "doughnut",                                            innerRadius: 90,                                            showInLegend: true,                                            dataPoints: $scope.overallgraph1                                        }                                    ]                                });                                chart.render();
                            
                            }
                            else {
                                swal("No Record found....!!")
                                $scope.detialsGrid = false;
                            }

                        }
                        else if ($scope.status_type === 'admsts')
                        {
                            if (($scope.studentcountreport != null && $scope.studentcountreport != "NULL")) {

                            
                                //==============================Overall Graph

                                $scope.head = ["INPROGRESS", "SELECTED", "WAITING","REJECTED","CONFIRM"]
                                $scope.overalladmgraph = [];
                                $scope.overalladmgraph1 = [];

                                angular.forEach($scope.studentcountreport, function (o) {                                    $scope.overalladmgraph.push({ label: $scope.head[0], y: parseInt(o.admInprogress), name: "INPROGRESS" });                                    $scope.overalladmgraph1.push({ indexLabel: $scope.head[0], y: parseInt(o.admInprogress), name: "INPROGRESS" })                                    $scope.overalladmgraph.push({ label: $scope.head[1], y: parseInt(o.admRejected), name: "SELECTED" });                                    $scope.overalladmgraph1.push({ indexLabel: $scope.head[1], y: parseInt(o.admRejected), name: "SELECTED" });                                    $scope.overalladmgraph.push({ label: $scope.head[2], y: parseInt(o.admWaiting), name: "WAITING" });                                    $scope.overalladmgraph1.push({ indexLabel: $scope.head[2], y: parseInt(o.admWaiting), name: "WAITING" });                                    $scope.overalladmgraph.push({ label: $scope.head[3], y: parseInt(o.admRejected), name: "REJECTED" });                                    $scope.overalladmgraph1.push({ indexLabel: $scope.head[3], y: o.admRejected, name: "REJECTED" });                                    $scope.overalladmgraph.push({ label: $scope.head[4], y: parseInt(o.admConfirm), name: "CONFIRM" });                                    $scope.overalladmgraph1.push({ indexLabel: $scope.head[4], y: o.admConfirm, name: "CONFIRM" });                                })
                                var chart = new CanvasJS.Chart("admcolumnchart", {                                    animationEnabled: true,                                    animationDuration: 3000,                                    height: 350,                                    colorSet: "graphcolor",                                    axisX: {                                        labelFontSize: 12,                                    },                                    axisY: {                                        labelFontSize: 12,                                    },                                    toolTip: {                                        shared: true                                    },                                    data: [                                        {                                            type: "column",                                            showInLegend: false,                                            dataPoints: $scope.overalladmgraph                                        }                                    ]                                });                                chart.render();                                var chart = new CanvasJS.Chart("admchartContainer", {                                    animationEnabled: true,                                    animationDuration: 3000,                                    height: 350,                                    colorSet: "graphcolor",                                    axisX: {                                        interval: 1,                                        labelFontSize: 12,                                    },                                    axisY: {                                        labelFontSize: 12,                                    },                                    data: [                                        {                                            type: "doughnut",                                            innerRadius: 90,                                            showInLegend: true,                                            dataPoints: $scope.overalladmgraph1                                        }                                    ]                                });                                chart.render();
                                function explodePie(e) {
                                    if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                                        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
                                    } else {
                                        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
                                    }
                                    e.chart.render();
                                }
                            }
                            else {
                                swal("No Record found....!!")
                                $scope.detialsGrid = false;
                            }
                        }
                       


                    }
                    else {
                        $scope.IsHidden1 = false;
                        $scope.showPrintB = false;
                        $scope.showExportB = false;
                        swal("No records Found");
                    }

                });
                $scope.sort = function (keyname) {

                    $scope.sortKey = keyname;   //set the sortKey to the param passed
                    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
                }
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interactedSM = function (field) {
            return $scope.submitted1 || field.$dirty1;
        };
        // Empty scope variable
        $scope.checkboxselected = [];


        $scope.clearfieldspage = function () {

            $state.reload();
        }


        //view docs//

        $scope.Clgapplicationstudocs = function (studentid) {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "PACA_Id": studentid,
                "status_type": $scope.status_type
            }
            apiService.create("TotalCountClg/Clgapplicationstudocs/", data).then(function (promise) {

                if (promise.doclist.length > 0 && promise.doclist != null) {
                    $scope.pages = promise.doclist;
                    if (promise.doclist.length > 0) {
                        angular.forEach($scope.pages, function (obj1) {
                            $('#').attr('src', obj1.docpath);
                            var img = obj1.docpath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            obj1.filetype = lastelement;
                        });
                    }
                    $('#myModaldoc').modal('show');
                }
                else {
                    swal("No Record Found!");
                }





            });
        }


        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.docpath);
        };

        var studentreg = "";
        var imagedownload = "";
        var docname = "";
        $scope.downloaddirect = function (data, idd) {
            if ($scope.pages.length > 0) {
                for (var i = 0; i < $scope.pages.length; i++) {
                    if ($scope.pages[i].paca_id == idd) {
                        studentreg = $scope.pages[i].PACA_RegistrationNo;
                    }
                }
            }

            $scope.imagedownload = data.docpath;
            imagedownload = data.docpath;
            docname = data.docname;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '-' + docname + '.jpg'
                    })[0].click();
                })
        }

        $scope.downloaddirectimage = function (data, idd, typeofphoto) {
            if ($scope.pages.length > 0) {
                for (var i = 0; i < $scope.pages.length; i++) {
                    if ($scope.pages[i].paca_id == idd) {
                        studentreg = $scope.pages[i].pacA_RegistrationNo;
                    }
                }
            }

            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '-' + docname + '.jpg'
                    })[0].click();
                })
        }


        $scope.downloadpdf = function (data) {

            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = data.document_Pathtemp;

            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        }
        //
        $scope.clearfields = function () {
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.pamsT_Id = "";
            $scope.IsHidden1 = false;
            //  $state.reload();
            $scope.Student_status = "786";
            $scope.showPrintB = false;
            $scope.showExportB = false;
            $scope.search = "";
        }
        $scope.Clearid = function () {
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.changedegault = function () {
            if ($scope.default == true) {
                $scope.sms = false;
                $scope.email = false;
                $scope.smsshow = false;
                $scope.emailshow = false;
                $scope.MailSubject = "";
                $scope.MailHeader = "";
                $scope.Parameter_email = "";
                $scope.MailFooter = "";
                $scope.smscontent = "";
            }
        };



        $scope.otponclickloaddata = function () {
            $scope.buttonotp = true;
            if ($scope.otptype == 'M') {
                $scope.otpmobile = true;
                $scope.otpemail = false;
            }
            else if ($scope.otptype == 'E') {
                $scope.otpmobile = false;
                $scope.otpemail = true;
            }
        }


        //added by suryan 

        $scope.onclickloaddata = function () {

            $scope.statuslist = [];
            //  $scope.courselist = [];
            $scope.asmaY_Id = "";
            $scope.amcO_Id = "";
            $scope.pamsT_Id = "";
            $scope.studentlist = "";
            $scope.showPrintB = false;
            $scope.showExportB = false;
            $scope.search = "";
            //   $scope.Student_status = "786";
            if ($scope.status_type === 'Appsts') {
                $scope.statuslist = [{ pamsT_Id: 787926, pamsT_Status: "APP WAITING" }, { pamsT_Id: 787927, pamsT_Status: "APP REJECTED" }, { pamsT_Id: 787928, pamsT_Status: "APP ACCEPTED" }];
            }
            else if ($scope.status_type === 'admsts') {
                apiService.get("TotalCountClg/getstatusdata/").then(function (promise) {
                    if (promise != "" && promise.statuslist.length > 0) {
                        $scope.statuslist = promise.statuslist;
                    }
                });
            }
            $scope.IsHidden1 = false;
            $scope.Student_status = "786";
          
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };



 

        //for Print
        $scope.printData = function (print_data) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();

            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        //for Export excel
        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };


        $scope.Clgapplicationdata = function (studentid) {


            apiService.getURI("ApplicationForm/Clgapplicationdata/", studentid).then(function (promise) {
                if (promise.studentList != null && promise.studentList.length > 0) {

                    //$scope.documentList = promise.documentList;
                    //$scope.DOB = false;
                    //$scope.mi_id = promise.mI_Id;


                    if (promise.prevSchoolDetails != null) {
                        if (promise.prevSchoolDetails.length > 0) {

                            $scope.prevSchoolDetails = promise.prevSchoolDetails;
                            $scope.prevschlcount = promise.prevSchoolDetails.length;

                            for (var i = 0; i < promise.prevSchoolDetails.length; i++) {
                                $scope.pacstpS_PreSchoolBoard = promise.prevSchoolDetails[i].pacstpS_PreSchoolBoard;
                                $scope.pacstpS_PreviousExamPassed = promise.prevSchoolDetails[i].pacstpS_PreviousExamPassed;
                                $scope.pacstpS_PreSchoolType = promise.prevSchoolDetails[i].pacstpS_PreSchoolType;

                                $scope.pacstpS_PreviousMarks = promise.prevSchoolDetails[i].pacstpS_PreviousMarks;
                                $scope.pacstpS_PreviousMarksObtained = promise.prevSchoolDetails[i].pacstpS_PreviousMarksObtained;
                                $scope.pacstpS_PreviousGrade = promise.prevSchoolDetails[i].pacstpS_PreviousGrade;

                                $scope.pacstpS_PrvSchoolName = promise.prevSchoolDetails[i].pacstpS_PrvSchoolName;
                                $scope.pacstpS_PreviousExamPassed = promise.prevSchoolDetails[i].pacstpS_PreviousExamPassed;
                                $scope.pacstpS_Urbanrural = promise.prevSchoolDetails[i].pacstpS_Urbanrural;
                                $scope.pacstpS_Attempts = promise.prevSchoolDetails[i].pacstpS_Attempts;
                                $scope.pacstpS_PreviousClass = promise.prevSchoolDetails[i].pacstpS_PreviousClass;
                                $scope.pacstpS_PreviousTCNo = promise.prevSchoolDetails[i].pacstpS_PreviousTCNo;
                                $scope.pacstpS_PreviousRegNo = promise.prevSchoolDetails[i].pacstpS_PreviousRegNo;
                                $scope.pacstpS_PreviousBranch = promise.prevSchoolDetails[i].pacstpS_PreviousBranch;
                                $scope.pacstpS_MediumOfInst = promise.prevSchoolDetails[i].pacstpS_MediumOfInst;
                                $scope.pacstpS_PasssedMonthYear = promise.prevSchoolDetails[i].pacstpS_PasssedMonthYear;
                                $scope.pacstpS_LanguagesTaken = promise.prevSchoolDetails[i].pacstpS_LanguagesTaken;
                                $scope.pacstpS_TCDate = new Date(promise.prevSchoolDetails[i].pacstpS_TCDate);
                                $scope.pacstpS_LeftYear = promise.prevSchoolDetails[i].pacstpS_LeftYear;
                                $scope.studentpreviousstate = promise.studentpreviousstate.length > 0 ? promise.studentpreviousstate[0].studpreviousstate : "";

                                var doobTc = promise.prevSchoolDetails[i].pacstpS_TCDate;

                                var doobyrtc = doobTc.substring(0, 4);
                                var doobmnthtc = doobTc.substring(5, 7);
                                var doobdaystc = doobTc.substring(8, 10);

                                $scope.b1tc = doobdaystc.substring(0, 1);
                                $scope.b2tc = doobdaystc.substring(1, 2);
                                $scope.BTC1 = $scope.b1tc + $scope.b2tc;

                                $scope.b3tc = doobmnthtc.substring(0, 1);
                                $scope.b4tc = doobmnthtc.substring(1, 2);
                                $scope.BTC2 = $scope.b3tc + $scope.b4tc;

                                $scope.b5tc = doobyrtc.substring(0, 1);
                                $scope.b6tc = doobyrtc.substring(1, 2);
                                $scope.b7tc = doobyrtc.substring(2, 3);
                                $scope.b8tc = doobyrtc.substring(3, 4);
                                $scope.BTC3 = $scope.b5tc + $scope.b6tc + $scope.b7tc + $scope.b8tc;

                                //$scope.pacstpS_PreSchoolCountry = promise.prevSchoolDetails[i].pacstpS_PreSchoolCountry;
                                //  getPrevGetState(promise.prevSchoolDetails[i].pacstpS_PreSchoolCountry, promise.prevSchoolDetails[i].pacstpS_PreSchoolState);
                                //$scope.onselectprevCountry($scope.pacstpS_PreSchoolCountry);
                                $scope.pacstpS_PreSchoolState = promise.prevSchoolDetails[i].pacstpS_PreSchoolState;
                            }
                        }
                    }
                    else {
                        $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
                        $scope.prevschlcount = 0;
                        $scope.pacstpS_Attempts = 1;
                    }
                    if (promise.studentGuardianDetails != null) {
                        if (promise.studentGuardianDetails.length > 0) {
                            $scope.studentGuardianDetails = promise.studentGuardianDetails;
                            $scope.grddetcount = promise.studentGuardianDetails.length;
                        }
                    }

                    else {
                        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
                        $scope.grddetcount = 0;
                    }

                    if (promise.studentsubjectmarksarry != null) {
                        if (promise.studentsubjectmarksarry.length > 0) {

                            $scope.prevexammarksdetailsprint = promise.studentsubjectmarksarry;
                            $scope.prevexammarksdetailscountprint = promise.studentsubjectmarksarry.length;
                        }
                    }

                    //documnets
                    if (promise.documentList != null) {
                        if (promise.documentList.length > 0) {
                            $scope.document = {};
                            $scope.documentList = promise.documentList;
                            angular.forEach(promise.documentList, function (value, key) {
                                $('#' + value.amsmD_Id).attr('src', value.document_Path);
                            })
                        }
                    }




                    $('#blah').attr('src', promise.studentList[0].pacA_StudentPhoto);



                    $scope.fatherphoto = promise.studentList[0].pacA_FatherPhoto;
                    $scope.fatherSign = promise.studentList[0].pacA_FatherSign;
                    $scope.fatherFingerprint = promise.studentList[0].pacA_FatherFingerprint;
                    $scope.motherphoto = promise.studentList[0].pacA_MotherPhoto;
                    $scope.mothersign = promise.studentList[0].pacA_MotherSign;
                    $scope.motherfingerprint = promise.studentList[0].pacA_MotherFingerprint;
                    $scope.image = promise.studentList[0].pacA_StudentPhoto;
                    $('#blahnewa').attr('src', promise.studentList[0].pacA_StudentPhoto);



                    if (promise.studentList != null && promise.studentList.length > 0) {
                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < promise.studentList.length; i++) {
                            if (promise.studentList[i].pacA_FirstName != '') {
                                if (promise.studentList[i].pacA_MiddleName !== null) {
                                    if (promise.studentList[i].pacA_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName + " " + promise.studentList[i].pacA_MiddleName + " " + promise.studentList[i].pacA_LastName, pacA_Id: promise.studentList[i].pacA_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName + " " + promise.studentList[i].pacA_MiddleName, pacA_Id: promise.studentList[i].pacA_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName, pacA_Id: promise.studentList[i].pacA_Id });
                                }

                                promise.studentList[i].pacA_FirstName = $scope.albumNameArray1[i].name;
                            }
                        }
                    }


                    $scope.PACA_FirstName = promise.studentList[0].pacA_FirstName;
                    $scope.PACA_MiddleName = promise.studentList[0].pacA_MiddleName;
                    $scope.PACA_LastName = promise.studentList[0].pacA_LastName;
                    $scope.PACA_Date = new Date(promise.studentList[0].pacA_Date);
                    $scope.PACA_RegistrationNo = promise.studentList[0].pacA_RegistrationNo;
                    $scope.PACA_AdmNo = promise.studentList[0].pacA_AdmNo;
                    $scope.PACA_StudentSubCaste = promise.studentList[0].pacA_StudentSubCaste;
                    $scope.ASMAY_Id = promise.studentList[0].asmaY_Id;
                    $scope.AMCO_Id = promise.studentList[0].amcO_Id;
                    $scope.AMB_Id = promise.studentList[0].amB_Id;
                    $scope.AMSE_Id = promise.studentList[0].amsE_Id;
                    $scope.ACMB_Id = promise.studentList[0].acmB_Id;
                    $scope.ACSS_Id = promise.studentList[0].acsS_Id;
                    $scope.ACST_Id = promise.studentList[0].acsT_Id;
                    if (promise.studentCategory.length > 0) {
                        $scope.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                    }
                    $scope.PACA_Sex = promise.studentList[0].pacA_Sex;
                    $scope.PACA_DOB = new Date(promise.studentList[0].pacA_DOB);
                    var doob = promise.studentList[0].pacA_DOB;

                    var doobyr = doob.substring(0, 4);
                    var doobmnth = doob.substring(5, 7);
                    var doobdays = doob.substring(8, 10);

                    $scope.b1 = doobdays.substring(0, 1);
                    $scope.b2 = doobdays.substring(1, 2);
                    $scope.BB1 = $scope.b1 + $scope.b2;

                    $scope.b3 = doobmnth.substring(0, 1);
                    $scope.b4 = doobmnth.substring(1, 2);
                    $scope.BB2 = $scope.b3 + $scope.b4;

                    $scope.b5 = doobyr.substring(0, 1);
                    $scope.b6 = doobyr.substring(1, 2);
                    $scope.b7 = doobyr.substring(2, 3);
                    $scope.b8 = doobyr.substring(3, 4);
                    $scope.BB3 = $scope.b5 + $scope.b6 + $scope.b7 + $scope.b8;

                    $scope.PACA_DOB_inwords = promise.studentList[0].pacA_DOB_inwords;
                    $scope.PACA_Age = promise.studentList[0].pacA_Age;
                    $scope.PACA_BloodGroup = promise.studentList[0].pacA_BloodGroup;
                    $scope.PACA_MotherTongue = promise.studentList[0].pacA_MotherTongue;
                    $scope.PACA_BirthPlace = promise.studentList[0].pacA_BirthPlace;
                    $scope.PACA_BirthCertNo = promise.studentList[0].pacA_BirthCertNo;
                    $scope.IVRMMR_Id = promise.studentList[0].ivrmmR_Id;
                    $scope.PACA_StudentSubCaste = promise.studentList[0].pacA_StudentSubCaste;
                    $scope.PACA_TPINNO = promise.studentList[0].pacA_TPINNO;

                    $scope.PACA_Village = promise.studentList[0].pacA_Village;
                    $scope.PACA_Taluk = promise.studentList[0].pacA_Taluk;
                    $scope.PACA_District = promise.studentList[0].pacA_District;
                    $scope.PACA_Urban_Rural = promise.studentList[0].pacA_Urban_Rural;

                    //$scope.IMCC_Id = promise.studentList[0].imcC_Id;
                    //for (var i = 0; i < $scope.allCaste.length; i++) {
                    //    $scope.allCaste[i].Selected = false;
                    //    $scope.IMC_Id = "";
                    //}


                    //if (promise.allCaste.length > 0) {
                    //    for (var i = 0; i < promise.allCaste.length; i++) {
                    //        if (promise.studentList[0].imC_Id == promise.allCaste[i].imC_Id) {
                    //            $scope.allCaste[i].Selected = true;
                    //            $scope.IMC_Id = promise.studentList[0].imC_Id;
                    //        }
                    //    }
                    //}
                    //else {
                    //    swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                    //}




                    $scope.PACA_Nationality = promise.studentList[0].pacA_Nationality;

                    $scope.IVRMMC_Id = promise.studentList[0].ivrmmC_Id;



                    // getSelectGetState($scope.IVRMMC_Id, promise.studentList[0].pacA_PerState);

                    $scope.PACA_PerState = promise.studentList[0].pacA_PerState;


                    $scope.PACA_PerStreet = promise.studentList[0].pacA_PerStreet;
                    $scope.PACA_PerArea = promise.studentList[0].pacA_PerArea;
                    $scope.PACA_PerCity = promise.studentList[0].pacA_PerCity;

                    $scope.PACA_PerPincode = promise.studentList[0].pacA_PerPincode;


                    $scope.PACA_StuBankAccNo = promise.studentList[0].pacA_StuBankAccNo;
                    $scope.PACA_StuBankIFSCCode = promise.studentList[0].pacA_StuBankIFSCCode;
                    $scope.PACA_AadharNo = promise.studentList[0].pacA_AadharNo;
                    $scope.PACA_BirthPlace = promise.studentList[0].pacA_BirthPlace;
                    $scope.PACA_StuCasteCertiNo = promise.studentList[0].pacA_StuCasteCertiNo;
                    $scope.PACA_MobileNo = promise.studentList[0].pacA_MobileNo;
                    $scope.PACA_emailId = promise.studentList[0].pacA_emailId;

                    $scope.PACA_PerStreet = promise.studentList[0].pacA_PerStreet;
                    $scope.PACA_ConPincode = promise.studentList[0].pacA_ConPincode;
                    $scope.PACA_ConArea = promise.studentList[0].pacA_ConArea;
                    $scope.PACA_ConStreet = promise.studentList[0].pacA_ConStreet;
                    $scope.PACA_ConCity = promise.studentList[0].pacA_ConCity;
                    $scope.PACA_ConCountryId = promise.studentList[0].pacA_ConCountryId;


                    $scope.studcourse = promise.studentcourse.length > 0 ? promise.studentcourse[0].amcO_CourseName : "";
                    $scope.studReligion = promise.studentReligion.length > 0 ? promise.studentReligion[0].ivrmmR_Name : "";
                    $scope.CasteName = promise.studentcastecate.length > 0 ? promise.studentcastecate[0].imC_CasteName : "";
                    $scope.studperstate = promise.studentperstate.length > 0 ? promise.studentperstate[0].studperstate : "";
                    $scope.studconstate = promise.studentconstate.length > 0 ? promise.studentconstate[0].studconstate : "";
                    $scope.studconcountry = promise.studentconcountry.length > 0 ? promise.studentconcountry[0].studconcountry : "";
                    $scope.studpercountry = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].studpercountry : "";

                    $scope.countrycode = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].countrycode : "";
                    $scope.statecode = promise.studentperstate.length > 0 ? promise.studentperstate[0].statecode : "";
                    $scope.CategoryName = promise.casteCategoryName.length > 0 ? promise.casteCategoryName[0].categoryName : "";


                    // getSelectGetState2($scope.PACA_ConCountryId, promise.studentList[0].pacA_ConState);

                    $scope.PACA_ConState = promise.studentList[0].pacA_ConState;

                    $scope.PACA_FatherAliveFlag = promise.studentList[0].pacA_FatherAliveFlag;
                    if (promise.studentList[0].pacA_FatherSurname != null) {
                        $scope.PACA_FatherName = promise.studentList[0].pacA_FatherName + ' ' + promise.studentList[0].pacA_FatherSurname;
                    }
                    else {
                        $scope.PACA_FatherName = promise.studentList[0].pacA_FatherName;
                    }

                    $scope.PACA_FatherSurname = promise.studentList[0].pacA_FatherSurname;
                    $scope.PACA_FatherAadharNo = promise.studentList[0].pacA_FatherAadharNo;
                    $scope.PACA_FatherEducation = promise.studentList[0].pacA_FatherEducation;
                    $scope.PACA_FatherOfficeAdd = promise.studentList[0].pacA_FatherOfficeAdd;
                    $scope.PACA_FatherOccupation = promise.studentList[0].pacA_FatherOccupation;
                    $scope.PACA_FatherDesignation = promise.studentList[0].pacA_FatherDesignation;
                    $scope.PACA_FatherBankAccNo = promise.studentList[0].pacA_FatherBankAccNo;
                    $scope.PACA_FatherBankIFSCCode = promise.studentList[0].pacA_FatherBankIFSCCode;
                    $scope.PACA_FatherCasteCertiNo = promise.studentList[0].pacA_FatherCasteCertiNo;
                    $scope.PACA_FatherNationality = promise.studentList[0].pacA_FatherNationality;
                    $scope.PACA_FatherMonIncome = promise.studentList[0].pacA_FatherMonIncome;
                    $scope.PACA_FatherAnnIncome = promise.studentList[0].pacA_FatherAnnIncome;
                    $scope.PACA_FatherMobleNo = promise.studentList[0].pacA_FatherMobleNo;
                    $scope.PACA_FatherEmailId = promise.studentList[0].pacA_FatherEmailId;
                    $scope.PACA_FatherReligion = promise.studentList[0].pacA_FatherReligion;
                    $scope.PACA_FatherCaste = promise.studentList[0].pacA_FatherCaste;
                    $scope.PACA_FatherSubCaste = promise.studentList[0].pacA_FatherSubCaste;

                    $scope.PACA_MotherAliveFlag = promise.studentList[0].pacA_MotherAliveFlag;
                    if (promise.studentList[0].pacA_MotherSurname != null) {
                        $scope.PACA_MotherName = promise.studentList[0].pacA_MotherName + ' ' + promise.studentList[0].pacA_MotherSurname;
                    }
                    else {
                        $scope.PACA_MotherName = promise.studentList[0].pacA_MotherName;
                    }

                    $scope.PACA_MotherSurname = promise.studentList[0].pacA_MotherSurname;
                    $scope.PACA_MotherAadharNo = promise.studentList[0].pacA_MotherAadharNo;
                    $scope.PACA_MotherEducation = promise.studentList[0].pacA_MotherEducation;
                    $scope.PACA_MotherOfficeAdd = promise.studentList[0].pacA_MotherOfficeAdd;
                    $scope.PACA_MotherOccupation = promise.studentList[0].pacA_MotherOccupation;
                    $scope.PACA_MotherDesignation = promise.studentList[0].pacA_MotherDesignation;
                    $scope.PACA_MotherBankAccNo = promise.studentList[0].pacA_MotherBankAccNo;
                    $scope.PACA_MotherBankIFSCCode = promise.studentList[0].pacA_MotherBankIFSCCode;
                    $scope.PACA_MotherCasteCertiNo = promise.studentList[0].pacA_MotherCasteCertiNo;
                    $scope.PACA_MotherNationality = promise.studentList[0].pacA_MotherNationality;
                    $scope.PACA_MotherMonIncome = promise.studentList[0].pacA_MotherMonIncome;
                    $scope.PACA_MotherAnnIncome = promise.studentList[0].pacA_MotherAnnIncome;
                    $scope.PACA_MotherMobleNo = promise.studentList[0].pacA_MotherMobleNo;
                    $scope.PACA_MotherEmailId = promise.studentList[0].pacA_MotherEmailId;

                    $scope.PACA_MotherReligion = promise.studentList[0].pacA_MotherReligion;
                    $scope.PACA_MotherCaste = promise.studentList[0].pacA_MotherCaste;
                    $scope.PACA_MotherSubCaste = promise.studentList[0].pacA_MotherSubCaste;

                    $scope.PACA_PassportNo = promise.studentList[0].pacA_PassportNo;

                    $scope.PACA_PassportIssuedAt = promise.studentList[0].pacA_PassportIssuedAt;
                    if (promise.studentList[0].pacA_PassportIssueDate != null) {


                        $scope.PACA_PassportIssueDate = new Date(promise.studentList[0].pacA_PassportIssueDate);

                        var do0ob = promise.studentList[0].pacA_PassportIssueDate;

                        var doobyrv = do0ob.substring(0, 4);
                        var doobmnthv = do0ob.substring(5, 7);
                        var doobdaysv = do0ob.substring(8, 10);

                        $scope.b1v = doobdaysv.substring(0, 1);
                        $scope.b2v = doobdaysv.substring(1, 2);
                        $scope.BV1 = $scope.b1v + $scope.b2v;

                        $scope.b3v = doobmnthv.substring(0, 1);
                        $scope.b4v = doobmnthv.substring(1, 2);
                        $scope.BV2 = $scope.b3v + $scope.b4v;

                        $scope.b5v = doobyrv.substring(0, 1);
                        $scope.b6v = doobyrv.substring(1, 2);
                        $scope.b7v = doobyrv.substring(2, 3);
                        $scope.b8v = doobyrv.substring(3, 4);
                        $scope.BV3 = $scope.b5v + $scope.b6v + $scope.b7v + $scope.b8v;
                    }
                    $scope.PACA_PassportIssuedCounrty = promise.studentList[0].pacA_PassportIssuedCounrty;
                    $scope.PACA_PassportIssuedPlace = promise.studentList[0].pacA_PassportIssuedPlace;
                    $scope.PACA_PassportExpiryDate = promise.studentList[0].pacA_PassportExpiryDate;

                    $scope.PACA_VISAIssuedBy = promise.studentList[0].pacA_VISAIssuedBy;
                    $scope.PACA_VISAValidFrom = promise.studentList[0].pacA_VISAValidFrom;
                    $scope.PACA_VISAValidTo = promise.studentList[0].pacA_VISAValidTo;

                    if (promise.studentpreffredbranch != null && promise.studentpreffredbranch.length > 0) {
                        $scope.studentpreffredbranch = promise.studentpreffredbranch;
                    }

                    if (promise.studentcurrenrtbranch != null && promise.studentcurrenrtbranch.length > 0) {
                        $scope.studentcurrenrtbranch = promise.studentcurrenrtbranch[0].studentbranchname;
                    }


                    //comp marks and exam details
                    $scope.compexamstudetails = [];
                    $scope.compexammarksdetails = [];
                    $scope.compexammarksdetails = promise.studentCompSubDetails;
                    $scope.compexamstudetails = promise.studentCompDetails;

                    if ($scope.compexamstudetails.length > 0) {
                        $scope.editflg = true;
                        for (var i = 0; i < $scope.compexamstudetails.length; i++) {

                            $scope.compexamstudetails[i].pamcexM_Id = $scope.compexamstudetails[0].pamcexM_Id;
                            $scope.compexamstudetails[i].pacstceM_RollNo = $scope.compexamstudetails[0].pacstceM_RollNo;
                            $scope.compexamstudetails[i].pacstceM_MeritNo = $scope.compexamstudetails[0].pacstceM_MeritNo;
                            $scope.compexamstudetails[i].PACSTCEM_RegistrationId = $scope.compexamstudetails[0].pacstceM_RegistrationId;
                            $scope.compexamstudetails[i].PACSTCEM_TotalMaxMarks = $scope.compexamstudetails[0].pacstceM_TotalMaxMarks;
                            $scope.compexamstudetails[i].PACSTCEM_ObtdMarks = $scope.compexamstudetails[0].pacstceM_ObtdMarks;
                            $scope.compexamstudetails[i].PACSTCEM_ALLIndiaRank = $scope.compexamstudetails[0].pacstceM_ALLIndiaRank;
                            $scope.compexamstudetails[i].PACSTCEM_CategoryRank = $scope.compexamstudetails[0].pacstceM_CategoryRank;
                            $scope.compexamstudetails[i].PACSTCEM_Percentage = $scope.compexamstudetails[0].pacstceM_Percentage;
                            $scope.compexamstudetails[i].PACSTCEM_Percentile = $scope.compexamstudetails[0].pacstceM_Percentile;
                            $scope.compexamstudetails[i].PAMCEXM_CompetitiveExams = $scope.compexamstudetails[0].pamcexM_CompetitiveExams;

                        }


                    }
                    //else {

                    //    $scope.compexamstudetails = {};

                    //    $scope.compexamstudetails = [{ id: 'compExamStudetails1' }];
                    //    $scope.compexamstudetailscount = 0;

                    //    $scope.editflg = false;
                    //}

                    if ($scope.compexammarksdetails.length > 0) {
                        $scope.editflg = true;
                        for (var i = 0; i < $scope.compexammarksdetails.length; i++) {
                            $scope.compexammarksdetails[i].pamcexmsuB_Id = $scope.compexammarksdetails[0].pamcexmsuB_Id;
                            $scope.compexammarksdetails[i].pamcexmsuB_MaxMarks = $scope.compexammarksdetails[0].pamcexmsuB_MaxMarks;
                            $scope.compexammarksdetails[i].pacstcemS_SubjectMarks = $scope.compexammarksdetails[0].pacstcemS_SubjectMarks;
                            $scope.compexammarksdetails[i].PAMCEXM_CompetitiveExams = $scope.compexammarksdetails[0].pamcexM_CompetitiveExams;
                            $scope.compexammarksdetails[i].PAMCEXMSUB_SubjectName = $scope.compexammarksdetails[0].pamcexmsuB_SubjectName;

                        }
                    }
                    //else {

                    //    $scope.compexammarksdetails = {};
                    //    $scope.compexammarksdetails = [{ id: 'compExamdetails1' }];
                    //    $scope.compexammarksdetailscount = 0;

                    //}

                    //
                    //var e1 = angular.element(document.getElementById("test"));
                    //$compile(e1.html(promise.htmldata))(($scope));
                }
            });
        }


        $scope.TERESIANAPP = function () {

            var innerContents = document.getElementById("TeresianID1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Teresian/PreAdmission/Teresian_Admission_Pdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

    }

})();