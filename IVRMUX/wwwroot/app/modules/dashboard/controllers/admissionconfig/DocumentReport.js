
(function () {
    'use strict';
    angular
.module('app')
.controller('DocumentViewReportAdmController', DocumentViewReportAdmController)

    DocumentViewReportAdmController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function DocumentViewReportAdmController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {
        $scope.userPrivileges = "";
        $scope.obj = {};
        $scope.obj.amsmD_Id = "";
        $scope.reportgrid = false;
        $scope.search = '';
        $scope.search1 = '';

        var copty;
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage = 1;

       //  $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage1 = 1;

        $scope.onclickloaddata = function () {
            if ($scope.TC_allorind == '2') {
                $scope.amsmD_Id = "";
            }
        }
        $scope.submitted = false;
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            

            apiService.get("DocumentViewReportAdm/getdetails/2").then(function (promise) {
                $scope.yearlst = promise.year;
                $scope.document = promise.document;
                $scope.class = promise.classname;
                $scope.section = promise.section;
            })
        };

        $scope.onSelectGetStudent = function () {
            var data = {
                "asmaY_Id": $scope.asmaY_Id,
                "asmcL_Id": $scope.asmcL_Id,
                "asmS_Id": $scope.asmS_Id
            }
            apiService.create("DocumentViewReportAdm/getstudent/", data).then(function (promise) {
                if (promise.studentdetails.length > 0) {
                    $scope.studentdetails = promise.studentdetails;
                }
                else {
                    swal("No Records Found");
                }
            })
        }

        $scope.getreport = function (obj) {
            
            var doc;
            var amsid;
            if ($scope.TC_allorind == "1") {
                obj.casteorcategory = 0;
                doc = 0;
                amsid = obj.stdid;
            }
            else {               
                amsid = 0;
                doc = obj.amsmD_Idqwe;
            }


            if ($scope.myForm.$valid) {
                var data = {
                    "AMST_Id": obj.stdid,
                    "asmaY_Id": $scope.asmaY_Id,
                    "asmcL_Id": $scope.asmcL_Id,
                    "asmS_Id": $scope.asmS_Id,
                    "STDORDOC": $scope.TC_allorind,
                    "SUBORNOT": obj.casteorcategory,
                    "AMSMD_Id":doc,

                }
                apiService.create("DocumentViewReportAdm/getreport/", data).then(function (promise) {
                    if (promise.studentlistreport.length > 0) {

                        if ($scope.TC_allorind == "1") {
                            $scope.doct = false;
                            $scope.studen = true;
                            $scope.pages = promise.studentlistreport;
                            $scope.presentCountgrid = $scope.pages.length;
                            $scope.reportgrid = true;
                            $scope.studentordoc1 = false;
                            $scope.studentordoc = true;

                            if (promise.studentlistreport.length > 0) {
                                angular.forEach($scope.pages, function (obj1) {
                                    $('#').attr('src', obj1.docpath);
                                    var img = obj1.docpath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    obj1.filetype = lastelement;
                                })
                            }

                            angular.forEach($scope.class, function (obj) {
                                if (obj.asmcL_Id == $scope.asmcL_Id) {
                                    $scope.classnamedis = obj.asmcL_ClassName;
                                }
                            })

                            angular.forEach($scope.section, function (obj2) {
                                if (obj2.asmS_Id == $scope.asmS_Id) {
                                    $scope.sectiondis = obj2.asmC_SectionName;
                                }
                            })

                            angular.forEach($scope.yearlst, function (obj3) {
                                if (obj3.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.yeardis = obj3.asmaY_Year;
                                }
                            })

                            $scope.studentnam = promise.studentlistreport[0].studentnam;
                        }
                        else if ($scope.TC_allorind == "2") {
                            $scope.doct = true;
                            $scope.studen = false;
                            $scope.studentordoc1 = true;
                            $scope.studentordoc = false;
                            $scope.pages1 = promise.studentlistreport;
                            $scope.presentCountgrid1 = $scope.pages1.length;
                            $scope.reportgrid = true;

                            if (promise.studentlistreport.length > 0) {
                                angular.forEach($scope.pages1, function (obj1) {
                                    $('#').attr('src', obj1.docpath);
                                    var img = obj1.docpath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    obj1.filetype = lastelement;
                                })
                            }

                            angular.forEach($scope.class, function (obj) {
                                if (obj.asmcL_Id == $scope.asmcL_Id) {
                                    $scope.classnamedis = obj.asmcL_ClassName;
                                }
                            })

                            angular.forEach($scope.section, function (obj2) {
                                if (obj2.asmS_Id == $scope.asmS_Id) {
                                    $scope.sectiondis = obj2.asmC_SectionName;
                                }
                            })

                            angular.forEach($scope.yearlst, function (obj3) {
                                if (obj3.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.yeardis = obj3.asmaY_Year;
                                }
                            })

                            $scope.studentnam = promise.studentlistreport[0].docname;
                        }                       

                    }
                    else {
                        swal("No Records Found");
                        $scope.doct = false;
                        $scope.studen = false;
                        $scope.studentordoc1 = false;
                        $scope.studentordoc = false;
                    }
                })

            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        
        $scope.clearid = function () {
            $state.reload();
        };

        $scope.doclist = [];

        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.docpath);
        }

        //for student wise
        $scope.print = function () {
            
            if ($scope.TC_allorind == "1") {
                if ($scope.printdatatabledate !== null && $scope.printdatatabledate.length > 0) {
                    var innerContents = document.getElementById("printSectionId1").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.write('<html><head>' +
                 '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
             '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
              '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
             '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
             );
                    popupWinindow.document.close();
                }
                else {
                    swal("Please Select Atleast One Record ");
                }
            }
            else if ($scope.TC_allorind == "2") {
                if ($scope.printdatatabledate1 !== null && $scope.printdatatabledate1.length > 0) {
                    var innerContents = document.getElementById("printSectionId2").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.write('<html><head>' +
                 '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
             '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
              '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
             '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
             );
                    popupWinindow.document.close();
                }
                else {
                    swal("Please Select Atleast One Record ");
                }
            }


           
        }


        //for student wise 
        $scope.optionToggleddate = function (SelectedStudentRecord, index) {
            $scope.all2date = $scope.filterValue1.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatabledate.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatabledate.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatabledate.splice($scope.printdatatabledate.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatabledate = [];
            }
        }

        $scope.printdatatabledate = [];
        $scope.toggleAlldate = function () {
            
            $scope.printdatatabledate = [];
            var toggleStatus1 = $scope.all2date;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus1;
                if ($scope.all2date == true) {
                    $scope.printdatatabledate.push(itm);
                }
                else {
                    $scope.printdatatabledate.splice(itm);
                    $scope.printdatatabledate = [];
                }
            });
        }


        //for document wise 
        $scope.optionToggleddate1 = function (SelectedStudentRecord, index) {
            $scope.all2date1 = $scope.filterValue11.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatabledate1.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatabledate1.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatabledate1.splice($scope.printdatatabledate1.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatabledate1 = [];
            }
        }

        $scope.printdatatabledate1 = [];
        $scope.toggleAlldate1 = function () {
            
            $scope.printdatatabledate1 = [];
            var toggleStatus1 = $scope.all2date1;
            angular.forEach($scope.filterValue11, function (itm) {
                itm.selected = toggleStatus1;
                if ($scope.all2date1 == true) {
                    $scope.printdatatabledate1.push(itm);
                }
                else {
                    $scope.printdatatabledate1.splice(itm);
                    $scope.printdatatabledate1 = [];
                }
            });
        }



        $scope.filterValuesearch = function (obj12) {
            return (angular.lowercase(obj12.docname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj12.statuss)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj12.submited)).indexOf(angular.lowercase($scope.search)) >= 0
        }

        $scope.searchfiltervalue = function (obj32) {
            return (angular.lowercase(obj32.studentnam)).indexOf(angular.lowercase($scope.search1)) >= 0 ||
                (angular.lowercase(obj32.statuss)).indexOf(angular.lowercase($scope.search1)) >= 0 ||
                (angular.lowercase(obj32.submited)).indexOf(angular.lowercase($scope.search1)) >= 0
        }
    }
})();