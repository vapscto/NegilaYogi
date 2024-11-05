


(function () {
    'use strict';
    angular
        .module('app')
        .controller('totalcountJSHSController', totalcountJSHSController)

    totalcountJSHSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function totalcountJSHSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        //Date:23-12-2016 for displaying privileges.        
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.classid = 0;
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sortKey = "User_Name";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.tadprint = false;
        $scope.printdatatable = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.userPrivileges = "";
        $scope.tcreport = false;
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        //export start

        //$scope.exportToExcel = function (tableId) {
        //    if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
        //        var exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //    }
        //    else {
        //        swal("Please select records to be Exported");
        //    }

        //}

        $scope.exportToExcel = function () {


           

            //if ($scope.enq.ReportType == 1) {
            //    $scope.routename = 'Registered List';
            //}
            //else if ($scope.enq.ReportType == 2) {
            //    $scope.routename = 'Registered but not filled Application form';
            //}
            //else if ($scope.enq.ReportType == 3) {
            //    $scope.routename = 'Registered and filled Application form';
            //}
            //else if ($scope.enq.ReportType == 4) {
            //    $scope.routename = 'Registration payment Done List';
            //}
            //else if ($scope.enq.ReportType == 6) {
            //    $scope.routename = 'Registration payment Not Done List';
            //}
            //else if ($scope.enq.ReportType == 5) {
            //    $scope.routename = 'Transfer student(Admission Confirm)';
            //}

            //var excelname = $scope.routename.toLowerCase();
            $scope.excelname = $scope.excelname.toUpperCase();
            var printSectionId = '#table22';
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'Total Count Report');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = $scope.excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }        

        $scope.printData = function () {
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


        //print end
        $scope.toggleAll = function () {
            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.reportdetails, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }

            if ($scope.searchValue != '') {
                 toggleStatus = $scope.all;
                angular.forEach($scope.filterValue1, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }

        }

        $scope.toggleAlltrue = function () {          
                $scope.all = true;
                var toggleStatus = $scope.all;
                angular.forEach($scope.reportdetails, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });            
        };

        $scope.pageChanged = function (newPage) {
            if (newPage > 0) {
                $scope.newPage = newPage;


            }
        };

        $scope.selected = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.reportdetails.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }

            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }

        }

        $scope.setTodate = function (data) {
            console.log(data);

            $scope.enq.ToDate = data;
            $scope.enq.ToDate = data;
            $scope.minDate = new Date(
                $scope.enq.ToDate.getFullYear(),
                $scope.enq.ToDate.getMonth(),
                $scope.enq.ToDate.getDate());

        }


        $scope.checkErr = function (FromDate, ToDate) {
            $scope.errMessage = '';
            var curDate = new Date();

            if (new Date(FromDate) > new Date(ToDate)) {
                $scope.errMessage = 'To Date should be greater than from date';
                return false;
            }
            //if (new Date(FromDate) < curDate) {
            //    $scope.errMessage = 'from date should not be before today.';
            //    return false;
            //}
        };


        $scope.loaddata = function () {
            apiService.getURI("TotalCountJSHSReport/get_intial_data", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.fillyear;
                    $scope.classlist = promise.fillclass;
                    $scope.totalcount = promise.totalcountDetails;
                    if ($scope.totalcount != null) {
                        for (var a = 0; a < $scope.totalcount.length; a++) {
                            $scope.reg = $scope.totalcount[a].reg;
                            $scope.notappform = $scope.totalcount[a].notapplicationform;
                            $scope.appform = $scope.totalcount[a].applicationform;
                            $scope.pay = $scope.totalcount[a].payment;
                            $scope.notpay = $scope.totalcount[a].notpayment;
                            $scope.transferstu = $scope.totalcount[a].transferstu;
                        }
                    }
                })
        }

        $scope.interacted = function (field) {
            // alert(field);

            return $scope.submitted || field.$dirty;
        };
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        //$('#print1').on('click', function () {
        //    
        //    printData();
        //})

        //$scope.exportToExcel = function (tableId) { // ex: '#my-table'
        //    $scope.exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //    $timeout(function () { location.href = $scope.fileData.exportHref; }, 100); // trigger download
        //}
        //$scope.exportToExcel = function (tableId) { // ex: '#my-table'
        //    

        //    $scope.exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //    $timeout(function () { location.href = $scope.exportHref; }, 100); // trigger download
        //}


        $scope.submitted = false;
        $scope.ShowReport = function (enq,ASMCL) {


            //Form validation
            if ($scope.myForm.$valid) {

                var excelname = "";
                var excelname1 = "";
                var exceldate = "";
                if ($scope.enq.ReportType === "1" || $scope.enq.ReportType === 1) {
                    excelname = "Registered list";
                    excelname1 = "Registered list";
                }
                else if ($scope.enq.ReportType === "2" || $scope.enq.ReportType === 2) {
                    excelname = "not filled application form";
                    excelname1 = "Registered but not filled application form";
                }
                else if ($scope.enq.ReportType === "3" || $scope.enq.ReportType === 3) {
                    excelname = "filled application form";
                    excelname1 = "Registered and filled application form";
                }
                else if ($scope.enq.ReportType === "4" || $scope.enq.ReportType === 4) {
                    excelname = "payment done(Application datewise)";
                    excelname1 = "Registration payment done(Application datewise)";
                }
                else if ($scope.enq.ReportType === "5" || $scope.enq.ReportType === 5) {
                    excelname = "transferred student";
                    excelname1 = "transferred student";
                }
                else if ($scope.enq.ReportType === "6" || $scope.enq.ReportType === 6) {
                    excelname = "payment not done";
                    excelname1 = "Registration payment not done";
                }
                else if ($scope.enq.ReportType === "7" || $scope.enq.ReportType === 7) {
                    excelname = "payment done(Payment datewise)";
                    excelname1 = "Registration payment done(Payment datewise)";
                }
                else if ($scope.enq.ReportType === "8" || $scope.enq.ReportType === 8) {
                    excelname = "Sibling students list(Paid Students)";
                    excelname1 = "Sibling students list(Paid Students)";
                }
                else if ($scope.enq.ReportType === "9" || $scope.enq.ReportType === 9) {
                    excelname = "Partial/Admission payment done list";
                    excelname1 = "Partial/Admission payment done list";
                }

                if ($scope.enq.yearwiseorbtwdates === "btwdates") {
                    var fdate = new Date($scope.enq.FromDate);
                    fdate = $filter('date')(fdate, 'dd-MMM-yyyy');
                    var tdate = new Date($scope.enq.ToDate);
                    tdate = $filter('date')(tdate, 'dd-MMM-yyyy');
                    exceldate = " (" + fdate + " To " + tdate + ")";
                    excelname = excelname + exceldate;
                    excelname1 = excelname1 + exceldate;
                }
                else {
                    angular.forEach($scope.yearlst, function (itm) {
                        if (itm.asmaY_Id === Number($scope.enq.ASMAY)) {
                            exceldate = itm.asmaY_Year;
                            excelname = excelname + "-" + exceldate;
                            excelname1 = excelname1 + "-" + exceldate;
                        }
                    });
                };

                $scope.excelname = excelname.toUpperCase();
                if (enq.ReportType == 1 || enq.ReportType == 2) {
                    $scope.ASMCLId = 0;
                }
                else {
                    $scope.ASMCLId = ASMCL;
                }
                $scope.all = false;
                var toggleStatus = $scope.all;
                angular.forEach($scope.reportdetails, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
                if ($scope.enq.yearwiseorbtwdates === "yearwise") {
                    $scope.enq.FromDate = new Date();
                    $scope.enq.ToDate = new Date();

                    $scope.enq.ASMAY = $scope.enq.ASMAY;
                }
                else if ($scope.enq.yearwiseorbtwdates === "btwdates") {
                    $scope.enq.FromDate = new Date($scope.enq.FromDate).toDateString(),

                        $scope.enq.ToDate = new Date($scope.enq.ToDate).toDateString(),
                    $scope.enq.ASMAY = 0;
                }

                var data = {

                    "From_Date": $scope.enq.FromDate,
                    "To_Date": $scope.enq.ToDate,
                    "ASMAY_Id": $scope.enq.ASMAY,
                    "ReportType": $scope.enq.ReportType,
                    "type": $scope.enq.yearwiseorbtwdates,
                    "ASMCL_Id": $scope.ASMCLId
                };
                apiService.create("TotalCountJSHSReport/Getdetails", data).
                    then(function (promise) {


                        if (promise.searchstudentDetails != null && promise.searchstudentDetails.length > 0) {
                            $scope.reportdetails = promise.searchstudentDetails;
                            angular.forEach(promise.searchstudentDetails, function (xy) {
                                xy.sib = [];
                            })
                            if (promise.searchstudentDetails.length > 0 && promise.siblinglist.length>0) {                          
                                for (var i = 0; i < promise.siblinglist.length; i++) {
                                    for (var j = 0; j < promise.searchstudentDetails.length; j++) {                                        
                                        if (promise.siblinglist[i].pasR_Id == promise.searchstudentDetails[j].PASR_Id) {                                           
                                            promise.searchstudentDetails[j].sib.push({ siblingname: promise.siblinglist[i].siblingname, siblingclass: promise.siblinglist[i].siblingclass, siblingadmno: promise.siblinglist[i].siblingadmno });
                                        }
                                    }
                                }
                            }

                            angular.forEach($scope.reportdetails, function (obj) {
                                if (obj.ScheduleTime != "" && obj.ScheduleTime != undefined) {
                                if (obj.ScheduleTime.substring(0, 2) > 12) {
                                    obj.ScheduleTimeonly = (Number(obj.ScheduleTime.substring(0, 2)) - 12).toString();
                                    if (obj.ScheduleTimeonly.length == 1) {
                                        obj.ScheduleTimeoly = '0' + obj.ScheduleTimeonly;
                                        obj.ScheduleTime = obj.ScheduleTimeoly + ':' + obj.ScheduleTime.substring(3, 5);
                                        obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                    }
                                    else if (obj.ScheduleTimeonly.length > 1) {
                                        obj.ScheduleTime = obj.ScheduleTimeonly + ':' + obj.ScheduleTime.substring(3, 5);
                                        obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                    }
                                }
                                else if (obj.ScheduleTime.substring(0, 2) == 12) {
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                                else {
                                    if (obj.ScheduleTime.length == 1) {
                                        obj.ScheduleTime = '0' + obj.ScheduleTime;
                                        obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                    }
                                    else {
                                        obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                    }
                                    }
                                }
                            })

                            angular.forEach($scope.reportdetails, function (obj) {
                                if (obj.ScheduleTimeTo != "" && obj.ScheduleTimeTo != undefined) {
                                    if (obj.ScheduleTimeTo.substring(0, 2) > 12) {
                                        obj.ScheduleTimeToonly = (Number(obj.ScheduleTimeTo.substring(0, 2)) - 12).toString();
                                        if (obj.ScheduleTimeToonly.length == 1) {
                                            obj.ScheduleTimeTooly = '0' + obj.ScheduleTimeToonly;
                                            obj.ScheduleTimeTo = obj.ScheduleTimeTooly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                            obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                        }
                                        else if (obj.ScheduleTimeToonly.length > 1) {
                                            obj.ScheduleTimeTo = obj.ScheduleTimeToonly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                            obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                        }
                                    }
                                    else if (obj.ScheduleTimeTo.substring(0, 2) == 12) {
                                        obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                    }
                                    else {

                                        if (obj.ScheduleTimeTo.length == 1) {
                                            obj.ScheduleTimeTo = '0' + obj.ScheduleTimeTo;
                                            obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                        }
                                        else {
                                            obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                        }
                                    }
                                }
                           
                            })

                            $scope.siblinglist = promise.siblinglist;
                            $scope.presentCountgrid = promise.searchstudentDetails.length;
                            $scope.searchstudenthead = promise.searchstudentheader;
                            $scope.header = $scope.searchstudenthead;
                            //$scope.head = [];
                            //angular.forEach($scope.header, function (key) {
                            //    $scope.head.push($scope.header[0]);
                            //});


                            //for (var a = 0; a < $scope.searchstudenthead.length; a++) {
                            //    $scope.head.push($scope.searchstudenthead[a])
                            //}
                            $scope.tcreport = true;
                            $scope.pagination = true;
                            $scope.print_flag = true;
                            $scope.export_flag = true;
                            $scope.toggleAlltrue();
                        }
                        else {
                            $scope.reportdetails = [];
                            swal("No Records Found")
                            $scope.pagination = false;
                            $scope.tcreport = false;
                            $scope.print_flag = false;
                            $scope.sms_flag = false;
                            $scope.mail_flag = false;
                            $scope.export_flag = false;
                        }

                    })

            }
            else {
                $scope.submitted = true;

            }
        };

        $scope.ShowReportkendo = function (enq, ASMCL) {


            //Form validation
            if ($scope.myForm.$valid) {



                if (enq.ReportType == 1 || enq.ReportType == 2) {
                    $scope.ASMCLId = 0;
                }
                else {
                    $scope.ASMCLId = ASMCL;
                }
                $scope.all = false;
                var toggleStatus = $scope.all;
                angular.forEach($scope.reportdetails, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
                if ($scope.enq.yearwiseorbtwdates === "yearwise") {
                    $scope.enq.FromDate = new Date();
                    $scope.enq.ToDate = new Date();

                    $scope.enq.ASMAY = $scope.enq.ASMAY;
                }
                else if ($scope.enq.yearwiseorbtwdates === "btwdates") {
                    $scope.enq.FromDate = new Date($scope.enq.FromDate).toDateString(),

                        $scope.enq.ToDate = new Date($scope.enq.ToDate).toDateString(),
                        $scope.enq.ASMAY = 0;
                }

                var data = {

                    "From_Date": $scope.enq.FromDate,
                    "To_Date": $scope.enq.ToDate,
                    "ASMAY_Id": $scope.enq.ASMAY,
                    "ReportType": $scope.enq.ReportType,
                    "type": $scope.enq.yearwiseorbtwdates,
                    "ASMCL_Id": $scope.ASMCLId
                };
                apiService.create("TotalCountReport/Getdetails", data).
                    then(function (promise) {


                        if (promise.searchstudentDetails != null && promise.searchstudentDetails.length > 0) {
                            $scope.reportdetails = promise.searchstudentDetails;
                           
                            $scope.presentCountgrid = promise.searchstudentDetails.length;
                            $scope.searchstudenthead = promise.searchstudentheader;
                            $scope.header = $scope.searchstudenthead;

                            //================================= UI Grid
                            if (enq.ReportType != 1 && enq.ReportType != 2) {
                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'FirstName', field: 'FirstName', title: 'STUDENT NAME', width: "200Px"
                                },
                                {
                                    name: 'EmailID', field: 'EmailID', title: 'EMAIL-ID', width: "220Px"
                                },
                                {
                                    name: 'Mobileno', field: 'Mobileno', title: 'MOBILE NUMBER', width: "200Px"
                                },
                                //{
                                //    name: 'RegDate', field: 'RegDate', title: 'REG DATE', width: "100px", template: "#= kendo.toString(kendo.parseDate(RegDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                //},
                                {
                                    name: 'Class', field: 'Class', title: 'CLASS', width: "150px"
                                },
                                {
                                    name: 'Bloodgroup', field: 'Bloodgroup', title: 'Blood Group', width: "100px"
                                },
                                {
                                    name: 'RegNo', field: 'RegNo', title: 'REG-NO', width: "100px"
                                },
                                {
                                    name: 'status', field: 'status', title: 'STATUS', width: "100px"
                                },
                                {
                                    name: 'ReligionName', field: 'ReligionName', title: 'RELIGION', width: "120px"
                                },
                                {
                                    name: 'State', field: 'State', title: 'STATE', width: "100px"
                                },
                                {
                                    name: 'PermanentCity', field: 'PermanentCity', title: 'ADDRESS', width: "100px"
                                },
                                {
                                    name: 'Gender', field: 'Gender', title: 'GENDER', width: "100px"
                                },
                                {
                                    name: 'DOB', field: 'DOB', title: 'DOB', width: "100px"
                                },
                                {
                                    name: 'DOBinwords', field: 'DOBinwords', title: 'DOB IN WORDS', width: "220Px"
                                },
                                {
                                    name: 'MotherTounge', field: 'MotherTounge', title: 'MOTHER TOUNGE', width: "200Px"
                                },
                                {
                                    name: 'CasteName', field: 'CasteName', title: 'CASTE NAME', width: "150px"
                                },
                                {
                                    name: 'PermanentStreet', field: 'PermanentStreet', title: 'PERMANENT STREET', width: "200Px"
                                },
                                {
                                    name: 'PermanentArea', field: 'PermanentArea', title: 'PERMANENT AREA', width: "200Px"
                                },
                                {
                                    name: 'PermanentCity', field: 'PermanentCity', title: 'PERMANENT CITY', width: "200Px"
                                },
                                {
                                    name: 'PermanentPincode', field: 'PermanentPincode', title: 'PERMANENT PINCODE', width: "200Px"
                                },
                                {
                                    name: 'CurrentStreet', field: 'CurrentStreet', title: 'CURRENT STREET', width: "200Px"
                                },
                                {
                                    name: 'CurrentArea', field: 'CurrentArea', title: 'CURRENT AREA', width: "200Px"
                                },
                                {
                                    name: 'Country', field: 'Country', title: 'COUNTRY', width: "100px"
                                },
                                {
                                    name: 'CurrentPincode', field: 'CurrentPincode', title: 'CURRENT PINCODE', width: "200Px"
                                },
                                {
                                    name: 'AadharNo', field: 'AadharNo', title: 'AADHAR No', width: "120px"
                                },
                                {
                                    name: 'FatherName', field: 'FatherName', title: 'FATHER NAME', width: "120px"
                                },
                                {
                                    name: 'FatherEducation', field: 'FatherEducation', title: 'FATHEREDUCATION', width: "200Px"
                                },
                                {
                                    name: 'FatherOccupation', field: 'FatherOccupation', title: 'FATHER OCCUPATION', width: "200Px"
                                },
                                {
                                    name: 'FatherDesignation', field: 'FatherDesignation', title: 'FATHER DESIGNATION', width: "200Px"
                                },
                                {
                                    name: 'FatherIncome', field: 'FatherIncome', title: 'FATHER INCOME', width: "120px"
                                },
                                {
                                    name: 'FatherMobileno', field: 'FatherMobileno', title: 'FATHER MOBILE No', width: "150px"
                                },
                                {
                                    name: 'MotherName', field: 'MotherName', title: 'MOTHER NAME', width: "120px"
                                },
                                {
                                    name: 'MotherEducation', field: 'MotherEducation', title: 'MOTHER EDUCATION', width: "200Px"
                                },
                                {
                                    name: 'MotherOccupation', field: 'MotherOccupation', title: 'MOTHER OCCUPATION', width: "200Px"
                                },
                                {
                                    name: 'MotherDesignation', field: 'MotherDesignation', title: 'MOTHER DESIGNATION', width: "200Px"
                                },
                                {
                                    name: 'MotherIncome', field: 'MotherIncome', title: 'MOTHER INCOME', width: "150px"
                                },
                                {
                                    name: 'MotherMobileno', field: 'MotherMobileno', title: 'MOTHER MOBILE No', width: "200Px"
                                },
                                {
                                    name: 'BirthPlace', field: 'BirthPlace', title: 'BIRTH PLACE', width: "120Px"
                                },
                                {
                                    name: 'LastClassAttendance', field: 'LastClassAttendance', title: 'LAST CLASS ATTENDANCE', width: "200Px"
                                },
                                {
                                    name: 'ExtraActivity', field: 'ExtraActivity', title: 'EXTRA ACTIVITY', width: "200Px"
                                },
                                {
                                    name: 'FatherOfficeAddress', field: 'FatherOfficeAddress', title: 'FATHER OFFICE ADDRESS', width: "220Px"
                                },
                                {
                                    name: 'MotherOfficeAddress', field: 'MotherOfficeAddress', title: 'MOTHER OFFICE ADDRESS', width: "220Px"
                                },
                                {
                                    name: 'Contactno', field: 'Contactno', title: 'CONTACT No.', width: "150px"
                                },
                                {
                                    name: 'ContactEmail', field: 'ContactEmail', title: 'CONTACT EMAIL', width: "150px",
                                }];
                            }
                            else {
                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'FatherName', field: 'FatherName', title: 'STUDENT NAME', width: "200Px"
                                },
                                {
                                    name: 'EmailID', field: 'EmailID', title: 'EMAIL-ID', width: "220Px"
                                },
                                {
                                    name: 'Mobileno', field: 'Mobileno', title: 'MOBILE NUMBER', width: "200Px"
                                },
                                {
                                    name: 'RegDate', field: 'RegDate', title: 'REG DATE', width: "100px"
                                }];
                            }

                            $(document).ready(function () {
                                initGridall();
                            });
                            function initGridall() {
                                $('#gridall').empty();

                                $("#gridall").kendoGrid({
                                    toolbar: ["excel"],
                                    excel: {
                                        fileName: "TotalCountReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: "TotalCountReport.pdf",
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.reportdetails,
                                        pageSize: 10
                                    },
                                    sortable: true,
                                    pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,
                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }

                                }).data("kendoGrid");
                            }

                            $scope.tcreport = false;
                          
                        }
                      

                    })

            }
            else {
                $scope.submitted = true;

            }
        };

        $scope.onclickloaddata = function () {
            if ($scope.enq.yearwiseorbtwdates === 'yearwise') {
                $scope.enq.ASMAY = "";
                $scope.frdatetodate = true;
                $scope.print_flag = false;
                $scope.sms_flag = false;
                $scope.mail_flag = false;
                $scope.export_flag = false;
                $scope.tcreport = false;
                $scope.submitted = false;

            }
            else if ($scope.enq.yearwiseorbtwdates === 'btwdates') {

                $scope.errMessage = "";
                $scope.enq.FromDate = "";
                $scope.enq.ToDate = "";
                $scope.frdatetodate = false;
                $scope.print_flag = false;
                $scope.sms_flag = false;
                $scope.mail_flag = false;
                $scope.export_flag = false;
                $scope.submitted = false;
                $scope.tcreport = false;
            }
        };


        $scope.SendMAIL = function (Text) {

            $scope.albumNameArray = [];
            angular.forEach($scope.reportdetails, function (user) {
                if (!!user.selected) $scope.albumNameArray.push(user);
            })

            if ($scope.albumNameArray.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send MAIL?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                SmsMailStudentDetailst: $scope.albumNameArray,
                                "SmsMailText": Text
                            };
                            apiService.create("TotalCountJSHSReport/SendMail", data)
                            $scope.$apply();
                            $scope.PostDataResponse = data;
                            swal('MAIL Sent Successfully');
                            $state.reload();
                        }
                        else {
                            swal("MAIL Sending Cancelled");

                        }
                    });

            } else {
                swal("Kindly select atleast a record to procced..!");
                return;
            }


        }

        $scope.SendMSG = function (Text) {

            $scope.albumNameArray = [];
            angular.forEach($scope.reportdetails, function (user) {
                if (!!user.selected) $scope.albumNameArray.push(user);
            })

            if ($scope.albumNameArray.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data = {
                                SmsMailStudentDetailst: $scope.albumNameArray,
                                "SmsMailText": Text
                            };
                            apiService.create("TotalCountJSHSReport/SendSms", data)
                            $scope.$apply();
                            $scope.PostDataResponse = data;
                            swal('SMS Sent Successfully')
                            // $scope.saved = "MAIL Sent Successfully";
                            $state.reload();
                        }
                        else {
                            swal("SMS Sending Cancelled");
                        }
                    });

            } else {
                swal("Kindly select atleast a record to procced..!");
                return;
            }


        }

        $scope.cancel = function () {
            //$scope.FromDate = "";
            //$scope.ToDate = "";
            //$scope.ReportType = 1;
            //$scope.tcreport = false;
            //$scope.reportdetails = "";
            $state.reload();
        }

        //search
        //search
        //$scope.searchValue = '';
        //$scope.filterValue = function (obj) {
        //    return ($filter('date')(obj.Entry_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (obj.Email).indexOf($scope.searchValue) >= 0 || (obj.PhoneNumber).indexOf($scope.searchValue) >= 0;
        //}

        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });

        $scope.rdochange = function () {
            $scope.reportdetails = [];
            $scope.printdatatable = [];
            $scope.tcreport = false;
            $scope.export_flag = false;
            $scope.print_flag = false;
        };
    }

})();