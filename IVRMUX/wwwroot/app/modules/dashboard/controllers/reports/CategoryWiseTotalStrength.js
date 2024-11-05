(function () {
    'use strict';
    angular
.module('app')
        .controller('CategoryWiseTotalStrengthController', CategoryWiseTotalStrengthController)

    CategoryWiseTotalStrengthController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function CategoryWiseTotalStrengthController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.printstudents = [];
        $scope.printstudents1 = [];
        $scope.printstudentscategory = [];
        $scope.printstudents1category = [];
        var _date = new Date();

        $scope.today_date = _date

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.class = true;
        //$scope.userPrivileges = "";
        //var pageid = $stateParams.pageId;

        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.searchValue2 = '';
        $scope.searchValue3 = '';

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.submitted = false;
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            var pageid = 1;
            apiService.getURI("CategoryWiseTotalStrength/getalldetails", pageid).
        then(function (promise) {
            
            $scope.yearlst = promise.accyear;
            $scope.arrclasslist = promise.accclasss;
            $scope.arrseclist = promise.accsec;
            $scope.headertest = promise.acccaste;
            $scope.headertest1 = promise.castecategory;

            $scope.columnsTest = [];
            $scope.columnsTest1 = [];
        })
        }

        $scope.report123 = false;
        $scope.clssec = true;
        $scope.clssec = true;
        $scope.albumNameArraycolumn = [];
        $scope.albumNameArraycolumn1 = [];

        $scope.addColumn = function (role) {
            $scope.all_header = $scope.headertest.every(function (itm)
            { return itm.Selected; });
            
            if (role.Selected === true) {
                $scope.albumNameArraycolumn.push(role);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
            }
        };

        $scope.Toggle_header = function () {
            
            var toggleStatus1 = $scope.all_header;
            angular.forEach($scope.headertest, function (itm) {
                itm.Selected = toggleStatus1;

            });
        }

        $scope.Toggle_header1 = function () {
            
            var toggleStatus12 = $scope.all_header1;
            angular.forEach($scope.headertest1, function (itm1) {
                itm1.Selected1 = toggleStatus12;

            });
        }

        $scope.addColumn1 = function (role1) {
            $scope.all_header1 = $scope.headertest1.every(function (itm1)
            { return itm1.Selected; });
            
            if (role1.Selected === true) {
                $scope.albumNameArraycolumn1.push(role1);
            }
            else {
                var som = $scope.albumNameArraycolumn1.indexOf(role1);
                $scope.columnsTest1.splice($scope.albumNameArraycolumn1.indexOf(role1), 1);
                $scope.albumNameArraycolumn1.splice($scope.albumNameArraycolumn1.indexOf(role1), 1);
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

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
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm)
            { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.toggleAll2 = function () {
            
            var toggleStatus1 = $scope.all1;
            angular.forEach($scope.studentsindi, function (itm1) {
                itm1.Selected = toggleStatus1;
                if ($scope.all1 == true) {
                    if ($scope.printstudents1.indexOf(itm1) === -1) {
                        $scope.printstudents1.push(itm1);
                    }
                }
                else {
                    $scope.printstudents1.splice(itm1);
                }
            });
        }

        $scope.optionToggled2 = function (SelectedStudentRecord, index) {
            $scope.all1 = $scope.studentsindi.every(function (itm1)
            { return itm1.Selected; });
            if ($scope.printstudents1.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents1.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents1.splice($scope.printstudents1.indexOf(SelectedStudentRecord), 1);
            }

        }


        $scope.toggleAllcategory = function () {
            
            var toggleStatus = $scope.allcategory;
            angular.forEach($scope.studentscategory, function (itm2) {
                itm2.Selected = toggleStatus;
                if ($scope.allcategory == true) {
                    if ($scope.printstudentscategory.indexOf(itm2) === -1) {
                        $scope.printstudentscategory.push(itm2);
                    }
                }
                else {
                    $scope.printstudentscategory.splice(itm2);
                }
            });
        }

        $scope.optionToggledcategory = function (SelectedStudentRecord, index) {
            $scope.allcategory = $scope.studentscategory.every(function (itm2)
            { return itm2.Selected; });
            if ($scope.printstudentscategory.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudentscategory.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudentscategory.splice($scope.printstudentscategory.indexOf(SelectedStudentRecord), 1);
            }

        }


        $scope.toggleAll2category = function () {
            
            var toggleStatus = $scope.all1category;
            angular.forEach($scope.studentsindicategory, function (itm) {
                itm.Selected = toggleStatus;
                if ($scope.all1category == true) {
                    if ($scope.printstudents1category.indexOf(itm) === -1) {
                        $scope.printstudents1category.push(itm);
                    }
                }
                else {
                    $scope.printstudents1category.splice(itm);
                }
            });
        }

        $scope.optionToggled2category = function (SelectedStudentRecord, index) {
            $scope.all1category = $scope.studentsindicategory.every(function (itm)
            { return itm.Selected; });
            if ($scope.printstudents1category.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents1category.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents1category.splice($scope.printstudents1category.indexOf(SelectedStudentRecord), 1);
            }

        }


        //Exporting data(table id will be passed)    
        $scope.exportToExcel = function () {

            if ($scope.casteorcategory == '1') {
                if ($scope.categorystudent == '1') {
                    if ($scope.printstudentscategory !== null && $scope.printstudentscategory.length > 0) {
                        var exportHref = Excel.tableToExcel('#table2', 'sheet name');
                        $timeout(function () { location.href = exportHref; }, 100);
                    }
                    else {
                        swal("Please Select Records to be Exported");
                    }
                }
                else if ($scope.categorystudent == '2') {
                    if ($scope.printstudents1category !== null && $scope.printstudents1category.length > 0) {
                        var exportHref = Excel.tableToExcel('#table3', 'sheet name');
                        $timeout(function () { location.href = exportHref; }, 100);
                    }
                    else {
                        swal("Please Select Records to be Exported");
                    }
                }
            }
            else {
                if ($scope.tctemporperm == '1') {
                    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                        var exportHref = Excel.tableToExcel('#table', 'sheet name');
                        $timeout(function () { location.href = exportHref; }, 100);
                    }
                    else {
                        swal("Please Select Records to be Exported");
                    }
                }
                else if ($scope.tctemporperm == '2') {
                    if ($scope.printstudents1 !== null && $scope.printstudents1.length > 0) {
                        var exportHref = Excel.tableToExcel('#table1', 'sheet name');
                        $timeout(function () { location.href = exportHref; }, 100);
                    }
                    else {
                        swal("Please Select Records to be Exported");
                    }
                }
            }
        }


        //Printing the data (div id will be passed)
        $scope.printData = function () {
            if ($scope.casteorcategory == '1') {
                if ($scope.categorystudent == '1') {
                    if ($scope.printstudentscategory !== null && $scope.printstudentscategory.length > 0) {
                        var innerContents = document.getElementById("printSectionIdcategory").innerHTML;
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
                else {
                    if ($scope.printstudents1category !== null && $scope.printstudents1category.length > 0) {
                        var innerContents = document.getElementById("printSectionId1category").innerHTML;
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
            }
            else {
                if ($scope.tctemporperm == '1') {
                    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
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
                    else {
                        swal("Please Select Records to be Printed");
                    }
                }
                else {
                    if ($scope.printstudents1 !== null && $scope.printstudents1.length > 0) {
                        var innerContents = document.getElementById("printSectionId1").innerHTML;
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
            }
        }


        $scope.onclickloaddata = function () {
            if ($scope.TC_allorind == "1") {
                $scope.class = true;
                $scope.submitted = false;
                $scope.report123 = false;
                $scope.indi = false;
                $scope.ASMAY = "";
                for (var i = 0; i < $scope.headertest.length; i++) {
                    $scope.headertest[i].Selected = false;
                }
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.all_header = false;
            }
            else if ($scope.TC_allorind === "2") {
                $scope.indi = false;
                $scope.class = false;
                $scope.submitted = false;
                $scope.report123 = false;
                $scope.ASMAY = "";
                for (var i = 0; i < $scope.headertest.length; i++) {
                    $scope.headertest[i].Selected = false;
                }
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.all_header = false;
            }
        };

        $scope.cateorstudent = function () {
            if ($scope.tctemporperm == '1') {
                $scope.indi = false;
                $scope.report123 = false;
            }
            else {
                $scope.indi = false;
                $scope.report123 = false;
            }
        }

        $scope.isOptionsRequired = function () {
            return !$scope.headertest.some(function (options) {
                return options.Selected;
            });
        }

        $scope.isOptionsRequired1 = function () {
            return !$scope.headertest1.some(function (options) {
                return options.Selected1;
            });
        }

        $scope.ShowReport = function () {
            $scope.printstudents = [];
            $scope.search = "";
            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.headertest, function (role) {
                    if (!!role.Selected) $scope.albumNameArraycolumn.push({
                        columnID: role.imC_Id,
                        columnName: role.imC_CasteName
                    });
                })

                $scope.albumNameArraycolumn1 = [];
                angular.forEach($scope.headertest1, function (role1) {
                    if (!!role1.Selected) $scope.albumNameArraycolumn1.push({
                        castecategoryid: role1.imcC_Id,
                        castecategoryname: role1.imcC_CategoryName
                    });
                })
                if ($scope.casteorcategory == '1') {
                    if ($scope.TC_allorind == '1') {
                        $scope.ASMCL = 0;
                        $scope.ASMC = 0;
                    }
                    var data = {
                        "ASMAY_Id": $scope.ASMAY,
                        "tcperortemp": $scope.tctemporperm, //caste or student 
                        "ASMCL_Id": $scope.ASMCL,
                        "ASMS_Id": $scope.ASMC,
                        "tcallorindi": $scope.TC_allorind, //ALL OR INDIVIDUAL                     
                        "TempararyArrayheadListcastecategory": $scope.albumNameArraycolumn1,
                        "casteorcategory": $scope.casteorcategory, //caste or category
                        "categorystudent": $scope.categorystudent //category or student
                    }
                } else {
                    if ($scope.TC_allorind == '1') {
                        $scope.ASMCL = 0;
                        $scope.ASMC = 0;
                    }
                    var data = {
                        "ASMAY_Id": $scope.ASMAY,
                        "tcperortemp": $scope.tctemporperm, //caste or student 
                        "ASMCL_Id": $scope.ASMCL,
                        "ASMS_Id": $scope.ASMC,
                        "tcallorindi": $scope.TC_allorind, //ALL OR INDIVIDUAL
                        "TempararyArrayheadList": $scope.albumNameArraycolumn,
                        "casteorcategory": $scope.casteorcategory, //caste or category
                        "categorystudent": $scope.categorystudent //category or student
                    }
                }

                if ($scope.TC_allorind == '1') {
                    apiService.create("CategoryWiseTotalStrength/Getreportdetails", data).
                    then(function (promise) {
                        
                        if (promise.report != null && promise.report.length > 0) {

                            if ($scope.casteorcategory == '1') {

                                if ($scope.categorystudent == '1') {
                                    $scope.report1231 = true;
                                    $scope.indi1 = false;

                                    $scope.report123 = false;
                                    $scope.indi = false;

                                    $scope.studentscategory = promise.report;
                                    $scope.presentCountgrid = $scope.studentscategory.length;
                                }
                                else {
                                    $scope.report1231 = false;
                                    $scope.indi1 = true;

                                    $scope.report123 = false;
                                    $scope.indi = false;

                                    $scope.studentsindicategory = promise.report;
                                    $scope.presentCountgrid = $scope.studentsindicategory.length;
                                }
                                $scope.print_flag = false;
                                $scope.excel_flag = false;

                            }
                            else {
                                if ($scope.tctemporperm == '1') {
                                    $scope.report123 = true;
                                    $scope.indi = false;

                                    $scope.report1231 = false;
                                    $scope.indi1 = false;

                                    $scope.students = promise.report;
                                    $scope.presentCountgrid = $scope.students.length;
                                } else {
                                    $scope.report123 = false;
                                    $scope.indi = true;

                                    $scope.report1231 = false;
                                    $scope.indi1 = false;

                                    $scope.studentsindi = promise.report;
                                    $scope.presentCountgrid = $scope.studentsindi.length;
                                }
                                $scope.print_flag = false;
                                $scope.excel_flag = false;
                            }
                        }
                        else {
                            swal(" Record Not Found. !!");
                            $state.reload();
                        }
                        //$scope.ASMAY = "";
                        $scope.ASMCL = "";
                        $scope.ASMC = "";
                    })

                } else if ($scope.TC_allorind == '2') {
                    apiService.create("CategoryWiseTotalStrength/Getreportdetails", data).
                    then(function (promise) {
                        if (promise.report.length > 0 && promise.report != null) {

                            if ($scope.casteorcategory == '1') {

                                if ($scope.categorystudent == '1') {
                                    $scope.report123 = false;
                                    $scope.indi = false;

                                    $scope.report1231 = true;
                                    $scope.indi1 = false;

                                    $scope.studentscategory = promise.report;
                                    $scope.presentCountgrid = $scope.studentscategory.length;
                                } else {
                                    $scope.report123 = false;
                                    $scope.indi = false;

                                    $scope.report1231 = false;
                                    $scope.indi1 = true;

                                    $scope.studentsindicategory = promise.report;
                                    $scope.presentCountgrid = $scope.studentsindicategory.length;
                                }
                                $scope.print_flag = false;
                                $scope.excel_flag = false;
                            }
                            else {
                                if ($scope.tctemporperm == '1') {
                                    $scope.report123 = true;
                                    $scope.indi = false;

                                    $scope.report1231 = false;
                                    $scope.indi1 = false;

                                    $scope.students = promise.report;
                                    $scope.presentCountgrid = $scope.students.length;
                                } else {
                                    $scope.report123 = false;
                                    $scope.indi = true;

                                    $scope.report1231 = false;
                                    $scope.indi1 = false;

                                    $scope.studentsindi = promise.report;
                                    $scope.presentCountgrid = $scope.studentsindi.length;
                                }
                                $scope.print_flag = false;
                                $scope.excel_flag = false;
                            }
                        }
                        else {
                            swal(" Record Not Found. !!");
                            $state.reload();
                        }
                    })

                } else {
                    swal("Please Select The Required Fields");
                    $scope.report123 = false;
                }
            } else {
                $scope.submitted = true;
            }
        };

        $scope.clear = function () {
            $state.reload();
            $scope.grid_flag = false;
        }

        $scope.casteorcategorywise = function () {
            if ($scope.casteorcategory == '1') {
                $scope.report123 = false;
                $scope.indi = false;
                $scope.report1231 = false;
                $scope.indi1 = false;

                $scope.ASMAY = "";
                $scope.ASMCL = "";
                $scope.ASMC = "";
                for (var i = 0; i < $scope.headertest.length; i++) {
                    $scope.headertest[i].Selected = false;
                }

                $scope.all_header = false;

                for (var i = 0; i < $scope.headertest1.length; i++) {
                    $scope.headertest1[i].Selected = false;
                }
                $scope.all_header1 = false;

            }
            else {
                $scope.report123 = false;
                $scope.indi = false;
                $scope.report1231 = false;
                $scope.indi1 = false;

                $scope.ASMAY = "";
                $scope.ASMCL = "";
                $scope.ASMC = "";
                for (var i = 0; i < $scope.headertest.length; i++) {
                    $scope.headertest[i].Selected = false;
                }

                $scope.all_header = false;
                for (var i = 0; i < $scope.headertest1.length; i++) {
                    $scope.headertest1[i].Selected = false;
                }
                $scope.all_header1 = false;
            }

        }
    }
})();

