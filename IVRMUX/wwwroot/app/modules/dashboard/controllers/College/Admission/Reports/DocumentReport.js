
(function () {
    'use strict';
    angular.module('app').controller('DocumentReportController', DocumentReportController)

    DocumentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile', 'Excel','$timeout']
    function DocumentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile, Excel, $timeout) {
        $scope.userPrivileges = "";
        $scope.obj = {};
        $scope.obj.amsmD_Id = "";
        $scope.reportgrid = false;
        $scope.search = '';
        $scope.search1 = '';
        $scope.report = false;
        var copty;
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }

        var logopath = "";
        var configsettings = JSON.parse(localStorage.getItem("configsettings"));
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.coptyright = copty;

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage = 1;

        //  $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage1 = 1;

        $scope.submitted = false;
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("CollegeDocumentReport/getdetails", pageid).then(function (promise) {
                $scope.yearlst = promise.getyear;
                $scope.document = promise.getdocument;
            });
        };

        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.obj.stdid = "";
            $scope.courselist = [];
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $scope.sectionlist = [];
            $scope.studentdetails = [];
            $scope.getreport = [];
            $scope.pages1 = [];
            $scope.pages = [];
            $scope.studen = false;
            $scope.doct = false;
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("CollegeDocumentReport/onchangeyear", data).then(function (promise) {
                $scope.courselist = promise.getcourse;

            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.obj.stdid = "";
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $scope.sectioname = [];
            $scope.studentdetails = [];            
            $scope.pages1 = [];
            $scope.pages = [];
            $scope.studen = false;
            $scope.doct = false;
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("CollegeDocumentReport/onchangecourse", data).then(function (promise) {
                $scope.branchlist = promise.getbranch;

            });
        };

        $scope.onchangebranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.obj.stdid = "";
            $scope.semesterlist = [];
            $scope.sectionlist = [];
            $scope.studentdetails = [];           
            $scope.pages1 = [];
            $scope.pages = [];
            $scope.studen = false;
            $scope.doct = false;
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("CollegeDocumentReport/onchangebranch", data).then(function (promise) {
                $scope.semesterlist = promise.getsemester;

            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.obj.stdid = "";
            $scope.sectionlist = [];
            $scope.studentdetails = [];          
            $scope.pages1 = [];
            $scope.pages = [];
            $scope.studen = false;
            $scope.doct = false;
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("CollegeDocumentReport/onchangesemester", data).then(function (promise) {
                $scope.sectionlist = promise.getsection;

            });
        };

        $scope.onchangesection = function () {
            $scope.obj.stdid = "";
            $scope.getstudent = [];            
            $scope.pages1 = [];
            $scope.pages = [];    
            $scope.studen = false;
            $scope.report = false;
            $scope.doct = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id
            };
            apiService.create("CollegeDocumentReport/onchangesection", data).then(function (promise) {
                $scope.studentdetails = promise.getstudent;

            });
        };

        $scope.getreportdetails = function (obj) {
            $scope.studen = false;
            $scope.doct = false;
            $scope.pages1 = [];
            $scope.pages = [];
            $scope.report = false;
            if ($scope.myForm.$valid) {               
                var doc;
                var amsid;
                if ($scope.TC_allorind === "1") {
                    obj.casteorcategory = 0;
                    doc = 0;
                    amsid = obj.stdid;
                }
                else {
                    amsid = 0;
                    doc = obj.amsmD_Idqwe;
                }
                var data = {
                    "AMCST_Id": amsid,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "STDORDOC": $scope.TC_allorind,
                    "AMSMD_Id": doc
                };

                apiService.create("CollegeDocumentReport/getreportdetails", data).then(function (promise) {
                    if (promise.getreport.length > 0) {
                        if ($scope.TC_allorind === "1") {
                            $scope.doct = false;
                            $scope.studen = true;
                            $scope.pages = promise.getreport;
                            $scope.presentCountgrid = $scope.pages.length;
                            $scope.reportgrid = true;
                            $scope.studentordoc1 = false;
                            $scope.studentordoc = true;
                            $scope.report = true;
                            if (promise.getreport.length > 0) {
                                angular.forEach($scope.pages, function (obj1) {
                                    $('#').attr('src', obj1.docpath);
                                    var img = obj1.docpath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    obj1.filetype = lastelement;
                                });
                            }

                            angular.forEach($scope.yearlst, function (obj3) {
                                if (obj3.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yeardis = obj3.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.courselist, function (obj3) {
                                if (obj3.amcO_Id === parseInt($scope.AMCO_Id)) {
                                    $scope.coursename = obj3.amcO_CourseName;
                                }
                            });

                            angular.forEach($scope.branchlist, function (obj3) {
                                if (obj3.amB_Id === parseInt($scope.AMB_Id)) {
                                    $scope.branchname = obj3.amB_BranchName;
                                }
                            });

                            angular.forEach($scope.semesterlist, function (obj3) {
                                if (obj3.amsE_Id === parseInt($scope.AMSE_Id)) {
                                    $scope.semestername = obj3.amsE_SEMName;
                                }
                            });

                            angular.forEach($scope.sectionlist, function (obj3) {
                                if (obj3.acmS_Id === parseInt($scope.ACMS_Id)) {
                                    $scope.sectioname = obj3.acmS_SectionName;
                                }
                            });

                            angular.forEach($scope.studentdetails, function (obj3) {
                                if (obj3.amcsT_Id === parseInt(amsid)) {
                                    $scope.studentnam = obj3.studentname;
                                }
                            });
                        }

                        else if ($scope.TC_allorind === "2") {
                            $scope.doct = true;
                            $scope.studen = false;
                            $scope.studentordoc1 = true;
                            $scope.studentordoc = false;
                            $scope.pages1 = promise.getreport;
                            $scope.presentCountgrid1 = $scope.pages1.length;
                            $scope.reportgrid = true;
                            $scope.report = true;
                            if (promise.getreport.length > 0) {
                                angular.forEach($scope.pages1, function (obj1) {
                                    $('#').attr('src', obj1.docpath);
                                    var img = obj1.docpath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    obj1.filetype = lastelement;
                                });
                            }

                            angular.forEach($scope.yearlst, function (obj3) {
                                if (obj3.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yeardis = obj3.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.courselist, function (obj3) {
                                if (obj3.amcO_Id === parseInt($scope.AMCO_Id)) {
                                    $scope.coursename = obj3.amcO_CourseName;
                                }
                            });

                            angular.forEach($scope.branchlist, function (obj3) {
                                if (obj3.amB_Id === parseInt($scope.AMB_Id)) {
                                    $scope.branchname = obj3.amB_BranchName;
                                }
                            });

                            angular.forEach($scope.semesterlist, function (obj3) {
                                if (obj3.amsE_Id === parseInt($scope.AMSE_Id)) {
                                    $scope.semestername = obj3.amsE_SEMName;
                                }
                            });

                            angular.forEach($scope.sectionlist, function (obj3) {
                                if (obj3.acmS_Id === parseInt($scope.ACMS_Id)) {
                                    $scope.sectioname = obj3.acmS_SectionName;
                                }
                            });

                            angular.forEach($scope.document, function (obj3) {
                                if (obj3.amsmD_Id === parseInt(doc)) {
                                    $scope.studentnam = obj3.amsmD_DocumentName;
                                }
                            });
                        }

                    }
                    else {
                        swal("No Records Found");
                        $scope.doct = false;
                        $scope.studen = false;
                        $scope.studentordoc1 = false;
                        $scope.studentordoc = false;
                    }
                });

            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clearid = function () {
            $state.reload();
        };

        $scope.doclist = [];

        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.docpath);
        };


        $scope.print = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.TC_allorind === "1") {

                innerContents = document.getElementById("printSectionId1").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();

            }
            else if ($scope.TC_allorind === "2") {

                innerContents = document.getElementById("printSectionId2").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = "";
            var excelname = "";
            if ($scope.TC_allorind === "1") {
                exportHref = Excel.tableToExcel('#printSectionId1excel', 'Student Document Report');
                excelname = "Student Document Report.xls";             
            }
            if ($scope.TC_allorind === "2") {
                exportHref = Excel.tableToExcel('#printSectionId2excel', 'Document Wise Report');
                excelname = "Document Wise Report.xls";
            }
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };


        //for student wise 
        $scope.optionToggleddate = function (SelectedStudentRecord, index) {
            $scope.all2date = $scope.filterValue1.every(function (itm) { return itm.selected; });
            if ($scope.printdatatabledate.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatabledate.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatabledate.splice($scope.printdatatabledate.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatabledate = [];
            }
        };

        $scope.printdatatabledate = [];
        $scope.toggleAlldate = function () {

            $scope.printdatatabledate = [];
            var toggleStatus1 = $scope.all2date;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus1;
                if ($scope.all2date === true) {
                    $scope.printdatatabledate.push(itm);
                }
                else {
                    $scope.printdatatabledate.splice(itm);
                    $scope.printdatatabledate = [];
                }
            });
        };


        //for document wise 
        $scope.optionToggleddate1 = function (SelectedStudentRecord, index) {
            $scope.all2date1 = $scope.filterValue11.every(function (itm) { return itm.selected; });
            if ($scope.printdatatabledate1.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatabledate1.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatabledate1.splice($scope.printdatatabledate1.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatabledate1 = [];
            }
        };

        $scope.printdatatabledate1 = [];
        $scope.toggleAlldate1 = function () {
            $scope.printdatatabledate1 = [];
            var toggleStatus1 = $scope.all2date1;
            angular.forEach($scope.filterValue11, function (itm) {
                itm.selected = toggleStatus1;
                if ($scope.all2date1 === true) {
                    $scope.printdatatabledate1.push(itm);
                }
                else {
                    $scope.printdatatabledate1.splice(itm);
                    $scope.printdatatabledate1 = [];
                }
            });
        };



        $scope.filterValuesearch = function (obj12) {
            return (angular.lowercase(obj12.docname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj12.statuss)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj12.submited)).indexOf(angular.lowercase($scope.search)) >= 0
        };

        $scope.searchfiltervalue = function (obj32) {
            return (angular.lowercase(obj32.studentnam)).indexOf(angular.lowercase($scope.search1)) >= 0 ||
                (angular.lowercase(obj32.statuss)).indexOf(angular.lowercase($scope.search1)) >= 0 ||
                (angular.lowercase(obj32.submited)).indexOf(angular.lowercase($scope.search1)) >= 0
        };
    }
})();