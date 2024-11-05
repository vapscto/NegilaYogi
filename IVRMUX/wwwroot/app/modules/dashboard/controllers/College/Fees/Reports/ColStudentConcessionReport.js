
angular.module('app').controller('ColStudentConcessionReportController', ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', function ($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {


    $scope.ddate = {};
    $scope.ddate = new Date();
    $scope.usrname = localStorage.getItem('username');

    var paginationformasters;
    var copty;
    var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
    if (ivrmcofigsettings.length > 0) {
        paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
    }
    else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
        paginationformasters = 5;
    }
    $scope.itemsPerPage = paginationformasters;
    $scope.coptyright = copty;
    var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
    if (admfigsettings.length > 0) {
        var logopath = admfigsettings[0].asC_Logo_Path;
    }

    $scope.imgname = logopath;
  //  $scope.itemsPerPage = paginationformasters;
    $scope.currentPage2 = 1;

    
    $scope.loaddata = function () {
        $scope.Grid_View = false;
        var pageid = 1;

        apiService.getURI("CollegeFeeDetails/GetYearList", pageid).
            then(function (promise) {

                $scope.yearlist = promise.yearlist;
                $scope.sectionlist = promise.sectionlist;
                $scope.quotalist = promise.quotalist;
               
            })
    }

    $scope.get_courses = function () {
        $scope.msg = '';

        $scope.amcO_Id = "";
        $scope.courselist = [];
        $scope.amB_Id = ''
        $scope.branchlist = [];
        $scope.semesterlist = [];
        $scope.amsE_Id = '';
        $scope.show_flag = false;
        if ($scope.asmaY_Id != undefined && $scope.asmaY_Id != "") {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("CollegeFeeDetails/get_courses", data).then(function (promise) {
                $scope.courselist = promise.courselist;

                $scope.grouplst = promise.fillmastergroup;
                $scope.headlst = promise.fillmasterhead;
                if ($scope.grouplst.length == 0) {
                    $scope.msg = "No Groups"
                }
                // $scope.amcO_Id = "";
                if ($scope.courselist.length == 0 || $scope.courselist == null) {
                    swal('For Selected Year Courses Are Not Available!!!');

                }
            })
        }
        else {
            $scope.courselist = [];
            $scope.amcO_Id = "";
        }
        $scope.show_btn = false;
        $scope.show_cancel = false;

        $scope.show_grid = false;
    };


    $scope.get_branches = function () {

        $scope.amB_Id = ''
        $scope.branchlist = [];
        $scope.semesterlist = [];
        $scope.amsE_Id = '';
        var data = {
            "ASMAY_Id": $scope.asmaY_Id,
            "AMCO_Id": $scope.amcO_Id
        }
        apiService.create("CollegeFeeDetails/get_branches", data).then(function (promise) {
            $scope.branchlist = promise.branchlist;
            // $scope.amB_Id = "";

            if (($scope.branchlist.length == 0 || $scope.branchlist == null ) && ($scope.amcO_Id > 0)) {
                swal('For Selected Course Branches Are Not Available!!!');
                $scope.show_btn = false;
                $scope.show_cancel = false;
                $scope.show_grid = false;
            }
        })
    }


    $scope.get_semisters = function () {

        var data = {
            "ASMAY_Id": $scope.asmaY_Id,
            "AMCO_Id": $scope.amcO_Id,
            "AMB_Id": $scope.amB_Id
        }
        apiService.create("CollegeFeeDetails/get_semisters", data).
            then(function (promise) {
                $scope.semesterlist = promise.semesterlist;
                // $scope.amsE_Id = "";
                if (($scope.semesterlist.length == 0 || $scope.semesterlist == null) && ($scope.amcO_Id > 0)) {
                    swal('For Selected Branch Semesters Are Not Available!!!');
                    $scope.show_btn = false;
                    $scope.show_cancel = false;
                    $scope.show_grid = false;
                }
            })
    };


    $scope.searchValue1 = "";
    $scope.search_box1 = function () {
        if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
            $scope.searc_button1 = false;
        }
        else {
            $scope.searc_button1 = true;
        }
    }


    $scope.savedata = function () {
        
        $scope.show_flag = false;
        $scope.submitted = true;
        var totB_p = 0;
        if ($scope.myForm.$valid) {
            
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCST_Id": 0,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "ACMS_Id": $scope.acmS_Id,
                 
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeFeeDetails/get_concession_report", data).
                    then(function (promise) {
                        if (promise.studentreport != null && promise.studentreport != "") {
                            $scope.export_flag = true;
                            $scope.print_flag = true;
                            $scope.show_grid = true;
                            $scope.Recordlength2 = promise.studentreport.length;
                            $scope.feedetails = promise.feedetails;
                            $scope.StudentReport = promise.studentreport;

                            angular.forEach($scope.StudentReport, function (gp) {
                                totB_p += gp.paid;
                            })
                            $scope.totB_p = totB_p;
                        }

                        else {
                            swal("No Record Found");
                            $scope.show_grid = false;
                            $scope.export_flag = false;
                            $scope.print_flag = false;

                        }
                    })
          

        }
        else {
            $scope.submitted = true;
        }
    };

    $scope.cancel = function () {
        $state.reload();
    }



    $scope.get_total_student_print = function () {
        var totB_p = 0;
        angular.forEach($scope.printdatatable, function (gp) {
            totB_p += gp.paid;
        })
        $scope.totB_p = totB_p;
    }



    $scope.toggleAll = function () {
        debugger;
        $scope.printdatatable = [];
        var toggleStatus = $scope.all;
        angular.forEach($scope.filterValue1, function (itm) {
            itm.selected = toggleStatus;
            if ($scope.all == true) {
                $scope.printdatatable.push(itm);
            }
            else {
                $scope.printdatatable.splice(itm);
            }
        });
        $scope.get_total_student_print();

    }



    $scope.optionToggled = function (SelectedStudentRecord, index) {
        debugger;
        $scope.all = $scope.StudentReport.every(function (itm) { return itm.selected; });

        if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
            $scope.printdatatable.push(SelectedStudentRecord);
        }
        else {
            $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
        }
        if ($scope.printdatatable.length > 0) {
            $scope.showbutton = true;
        }
        else {
            $scope.showbutton = false;
        }
        $scope.get_total_student_print();
    }
    

    $scope.exportToExcel = function (table1) {
        debugger;
        if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
            var exportHref = Excel.tableToExcel(table1, 'WireWorkbenchDataExport');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        }
        else {
            swal("Please select records to be Exported");
        }
    }

    $scope.printData = function (printSectionId) {
        debugger;
        if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        else {
            swal("Please Select Records to be Printed");
        }
    }

    


}]);
