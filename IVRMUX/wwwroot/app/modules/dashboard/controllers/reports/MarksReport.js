(function () {
    'use strict';
    angular.module('app')
    .controller('MarksReportController', MarksReportController)
    MarksReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function MarksReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];

        $scope.ddate = {};
        $scope.ddate = new Date();

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings!=null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
        }
     
        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }
        }
       

        $scope.imgname = logopath;

        $scope.loaddata = function () {
            $scope.screport = false;
            $scope.paginate1 = "paginate1";
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            var pageid = 2;
            apiService.getURI("MarksReport/getdetails", pageid).
            then(function (promise) {
                $scope.yearlst = promise.fillyear;
                $scope.classlist = promise.fillclass;
                $scope.namesubject = false;
                $scope.nameschedule = false;
            })
        };
        ////search
        //$scope.searchValue = '';
        //$scope.filterValue = function (obj) {
        //    return ($filter('date')(obj.ScheduleTime, 'HH:mm').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.ScheduleTimeTo, 'HH:mm').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.entrydate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || angular.lowercase(obj.regno).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.name).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.ScheduleState).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        //}

        //export start
        //$scope.exportToExcel = function (tableId) {
        //    if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
        //        var exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //    }
        //    else {
        //        swal("Please select records to be Exported");
        //    }

        //};

        $scope.exportToExcel = function (tableId) {

            var excelname = "MARKS REPORT";
            excelname = excelname.toUpperCase() + '.xls';
            var printSectionId = tableId;
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'Registration Report');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
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
        //export end

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
            '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
           '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
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
                angular.forEach($scope.students, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        if ($scope.printdatatable.indexOf(itm) === -1) {
                            $scope.printdatatable.push(itm);
                        }
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }

            if ($scope.searchValue != '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.filterValue1, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        if ($scope.printdatatable.indexOf(itm) === -1) {
                            $scope.printdatatable.push(itm);
                        }
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }
        };

        $scope.selected = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.students.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.setTodate = function (data) {
            $scope.obj.todate = data;
            $scope.minDate = new Date(
            $scope.obj.todate.getFullYear(),
            $scope.obj.todate.getMonth(),
            $scope.obj.todate.getDate());
        };

        $scope.oralwrittenschedule1 =
            function () {

            $scope.screport = false;
            $scope.students = [];
            $scope.nameschedule = true;
            if ($scope.obj.oralwrittenschedule == "oral") {
                $scope.namesubject = false;
            }
            else if ($scope.obj.oralwrittenschedule == "written") {
                $scope.namesubject = true;
            }
            $scope.obj.disid = "";
            var data = {
                "oralwrittenscheduleflag": $scope.obj.oralwrittenschedule
            }
            apiService.create("MarksReport/schedulelist", data).then(function (promise) {
                $scope.schelist = promise.writentestlist;               
                $scope.sublist = promise.fillsub;
                $scope.classlists = promise.fillclass;
                $scope.arrlist9 = promise.admissioncatdrpall;
                $scope.students = [];
                $scope.headsublist = [];
                $scope.headsublist = [];  
                //$scope.$scope.students[0].psub = "";
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;
        $scope.showreport = function (obj) {
            
            
            if ($scope.myForm.$valid) {
                $scope.nameschedule = true;
                if ($scope.obj.oralwrittenschedule == "oral") {
                    $scope.namesubject = false;
                }
                else if ($scope.obj.oralwrittenschedule == "written") {
                    $scope.namesubject = true;
                }
                if ($scope.obj.ismS_Id == undefined)
                {
                    var data = {
                        "flagows": $scope.obj.oralwrittenschedule,
                        "schids": $scope.obj.disid
                    }
                }
                else {
                    var data = {
                        "flagows": $scope.obj.oralwrittenschedule,
                        "schids": $scope.obj.disid,
                        "ISMS_Id": $scope.obj.ismS_Id
                        //"asmclid": $scope.asmcL_Id
                    }
                }
               
                apiService.create("MarksReport/Getreportdetails", data).
                then(function (promise) {
                    
                    $scope.screport = true;
                    var sum = 0;
                    $scope.headsublist = [];
                    if ($scope.obj.oralwrittenschedule == "oral") {
                        var temp_array = [];
                      
                        for (var i = 0; i < promise.fillhead[0].hid ; i++) {
                            var a = i + 1;
                            sum = sum + promise.fillhead[0].hmaxmarks;
                            var newCol = { hid: a, hhead: a, hmaxmarks: promise.fillhead[0].hmaxmarks }
                            temp_array.push(newCol);
                        }
                        $scope.headsublist = temp_array;
                        
                    }
                    else {
                        
                        if ($scope.obj.ismS_Id == 0) {
                            $scope.headsublist = promise.fillhead;
                            angular.forEach(promise.fillhead, function (itm) {
                                sum = sum + itm.hmaxmarks;
                            })
                        }
                        else {
                            for (var i = 0; i < promise.fillhead.length; i++) {
                                if (promise.fillhead[i].hid == $scope.obj.ismS_Id) {
                                    $scope.headsublist.push(promise.fillhead[i]);
                                    sum = promise.fillhead[i].hmaxmarks;
                                    break;
                                }
                            }
                        }
                        
                    }
                    $scope.total = sum;
                    var temp_array = [];
                    var temp_array1 = [];
                    var sum1 = 0;
                    for (var i = 0; i < promise.allreports.length; i++) {                        
                        var newCol = { pid: promise.allreports[i].ismS_Id, marks: promise.allreports[i].paswmS_MarksScored }
                        temp_array.push(newCol);
                        sum1 = sum1 + promise.allreports[i].paswmS_MarksScored;
                        if (i < promise.allreports.length - 1) {
                            if (promise.allreports[i].pasR_Id == promise.allreports[i + 1].pasR_Id)
                                continue;
                        }
                        var fname = promise.allreports[i].pasR_FirstName != null ? promise.allreports[i].pasR_FirstName + " " : "";
                        var mname = promise.allreports[i].pasR_MiddleName != null ? promise.allreports[i].pasR_MiddleName + " " : "";
                        var lname = promise.allreports[i].pasR_LastName != null ? promise.allreports[i].pasR_LastName : "";
                        var classnamem = promise.allreports[i].classname != null ? promise.allreports[i].classname : "";
                        var newCol1 = { sum1: sum1, psub: temp_array, pasR_Id: promise.allreports[i].pasR_Id, regno: promise.allreports[i].regno, name: fname + mname + lname, classname: classnamem }
                        temp_array1.push(newCol1);
                        temp_array = [];
                        sum1 = 0;
                    }
                    $scope.students = temp_array1;
                    if ($scope.students.length > 0 && $scope.headsublist.length == 0) {
                        $scope.headsublistoral = $scope.students[0].psub;
                    }
                    $scope.presentCountgrid = temp_array1.length;
                    if ($scope.students.length > 0 || $scope.students == null) {
                        $scope.screport = true;
                    } else {
                        swal("No Records Found");
                        $scope.screport = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.showreportsrkvs = function (obj) {
            if ($scope.myForm.$valid) {
                $scope.nameschedule = true;
                if ($scope.obj.oralwrittenschedule == "oral") {
                    $scope.namesubject = false;
                }
                else if ($scope.obj.oralwrittenschedule == "written") {
                    $scope.namesubject = true;
                }
                if ($scope.obj.ismS_Id == undefined) {
                    var data = {
                        "flagows": $scope.obj.oralwrittenschedule,
                        "schids": $scope.obj.disid,
                        "ordertype": $scope.hrsrt,
                    };
                }
                else {
                    data = {                       
                        "schids": $scope.obj.disid,
                        "ISMS_Id": $scope.obj.ismS_Id,
                        "order_type": $scope.hrsrt,
                        "CasteCategory_Id": $scope.CasteCategory_Id,
                        "ordertype": $scope.hrsrt
                        //"asmclid": $scope.asmcL_Id
                    };
                }

                apiService.create("MarksReport/Getreportdetailssrkvs", data).
                    then(function (promise) {

                        $scope.screport = true;
                        $scope.ranklist = promise.ranklist;
                        var sum = 0;
                        $scope.headsublist = [];
                        if ($scope.obj.oralwrittenschedule == "oral") {
                            var temp_array = [];
                            //$scope.headsublist = promise.fillhead;
                            for (var i = 0; i < promise.fillhead[0].hid; i++) {
                                var a = i + 1;
                                sum = sum + promise.fillhead[0].hmaxmarks;
                                var newCol = { hid: a, hhead: a, hmaxmarks: promise.fillhead[0].hmaxmarks }
                                temp_array.push(newCol);
                            }
                            $scope.headsublist = temp_array;
                        }
                        else {






                            if ($scope.obj.ismS_Id == 0) {
                                $scope.headsublist = promise.fillhead;
                                angular.forEach(promise.fillhead, function (itm) {
                                    sum = sum + itm.hmaxmarks;
                                })
                            }
                            else {
                                for (var i = 0; i < promise.fillhead.length; i++) {
                                    if (promise.fillhead[i].hid == $scope.obj.ismS_Id) {
                                        $scope.headsublist.push(promise.fillhead[i]);
                                        sum = promise.fillhead[i].hmaxmarks;
                                        break;
                                    }
                                }
                            }

                        }
                        $scope.total = sum;
                        var  temp_array = [];
                        var temp_array1 = [];
                        var sum1 = 0;
                        for (var i = 0; i < promise.allreports.length; i++) {
                            var  newCol = { pid: promise.allreports[i].ismS_Id, marks: promise.allreports[i].paswmS_MarksScored }
                            temp_array.push(newCol);
                            sum1 = sum1 + promise.allreports[i].paswmS_MarksScored;
                            if (i < promise.allreports.length - 1) {
                                if (promise.allreports[i].pasR_Id == promise.allreports[i + 1].pasR_Id)
                                    continue;
                            }
                            var fname = promise.allreports[i].pasR_FirstName != null ? promise.allreports[i].pasR_FirstName + " " : "";
                            var mname = promise.allreports[i].pasR_MiddleName != null ? promise.allreports[i].pasR_MiddleName + " " : "";
                            var lname = promise.allreports[i].pasR_LastName != null ? promise.allreports[i].pasR_LastName : "";
                            var classnamem = promise.allreports[i].classname != null ? promise.allreports[i].classname : "";
                            var newCol1 = {
                                sum1: sum1, psub: temp_array, pasR_Id: promise.allreports[i].pasR_Id, regno: promise.allreports[i].regno, name: fname + mname + lname, classname: classnamem,
                                pasR_Age: promise.allreports[i].pasR_Age, pasR_ConDistrict: promise.allreports[i].pasR_ConDistrict, remark: promise.allreports[i].remark, caste: promise.allreports[i].caste, scheduleddate: promise.allreports[i].scheduleddate
                            }
                            temp_array1.push(newCol1);
                            temp_array = [];
                            sum1 = 0;
                        }
                        $scope.students = temp_array1;
                        if ($scope.students.length > 0 && $scope.headsublist.length == 0) {
                            $scope.headsublistoral = $scope.students[0].psub;
                        }
                        $scope.presentCountgrid = temp_array1.length;
                        $scope.studentsrank = [];
                        if ($scope.students.length > 0 || $scope.students === null) {
                            $scope.screport = true;
                            angular.forEach($scope.ranklist, function (objectt) {
                                angular.forEach($scope.students, function (object) {
                                    if (object.pasR_Id === objectt.PASR_Id) {
                                        object.rank = objectt.RNK;
                                        $scope.studentsrank.push(object);
                                    }                                   
                                });
                            });
                        } else {
                            swal("No Records Found");
                            $scope.screport = false;
                        }
                        if ($scope.hrsrt == 'Rank') {
                            $scope.students = [];
                            $scope.students = $scope.studentsrank;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
            //$scope.obj.oralwrittenschedule = "oral";
            //$scope.oralwrittenschedule1();
            //$scope.obj.disid = "";
            //$scope.obj.pamsU_Id = "";
            //$scope.namesubject = false;
            //$scope.screport = false;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
        };
    }
})();