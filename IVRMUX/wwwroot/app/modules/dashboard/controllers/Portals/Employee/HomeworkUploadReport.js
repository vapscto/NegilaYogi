(function () {
    'use strict';
    angular.module('app')
        .controller('homeworkuploadController', homeworkuploadController)
    homeworkuploadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function homeworkuploadController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }


        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];
        $scope.classarray = [];
        $scope.search = "";

        $scope.loaddata = function () {
            $scope.screport = false;

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 2;
            apiService.getURI("HomeworkUpload/Getdata_class", pageid).
                then(function (promise) {
                    $scope.classlist = promise.classlist;

                })
        };


        //=============== Check box
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.classlist.every(function (itm) { return itm.selected; });
            if ($scope.classarray.indexOf(SelectedStudentRecord) === -1) {
                $scope.classarray.push(SelectedStudentRecord);


            }
            else {
                $scope.classarray.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);

            }

        }

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.classlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.classarray.indexOf(itm) === -1) {
                        $scope.classarray.push({ ASMCL_Id: itm.ASMCL_Id });

                    }
                }
                else {
                    $scope.classarray.splice({ ASMCL_Id: itm.ASMCL_Id });

                }
            });
        }

        $scope.isOptionsRequired = function () {

            return !$scope.classlist.some(function (sec) {
                return sec.selected;
            });
        }

        $scope.submitted = false;
        $scope.showreport = function () {

            if ($scope.myForm.$valid) {
                $scope.classarraynew = [];
                if ($scope.classarray != null || $scope.classarray > 0) {
                    angular.forEach($scope.classlist, function (qq) {
                        if (qq.selected == true) {
                            $scope.classarraynew.push({ ASMCL_Id: qq.ASMCL_Id })
                        }

                    })
                }
                else {
                    $scope.classarraynew = undefined;
                }

                $scope.fromdate = $filter('date')($scope.fromdate, "yyyy-MM-dd");
                $scope.todate = $filter('date')($scope.todate, "yyyy-MM-dd");
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "classarray": $scope.classarraynew,
                    "type": "HomeWork"
                 
                }
                apiService.create("HomeworkUpload/getreport_home", data).
                    then(function (promise) {
                        if (promise.classreportlist > 0 || promise.classreportlist !== null) {
                            $scope.classreportlist = promise.classreportlist;
                        }
                        else {
                            swal('No Data Found!!!')
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };


        //export start
        $scope.exportToExcel = function (tableId) {
            if ($scope.classreportlist !== null && $scope.classreportlist.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };

        $scope.printData = function () {
            if ($scope.classreportlist !== null && $scope.classreportlist.length > 0) {
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

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        $scope.viewData = function (option) {
            var data = {
                "Temp": 1,
                "IHW_Id": option.IHW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            apiService.create("HomeworkUpload/viewData", data)
                .then(function (promise) {

                    if (promise.attachementlist.length > 0) {
                        $scope.attachementlist1 = [];
                        angular.forEach(promise.attachementlist, function (qq) {
                            $scope.img = qq.ihwatT_Attachment;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];

                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                $scope.attachementlist1.push({
                                    ihwatT_FileName: qq.ihwatT_FileName,
                                    ihW_Attachment: qq.ihwatT_Attachment,
                                    ihwatT_Attachment: qq.ihwatT_Attachment
                                })
                            }
                            else {
                                $scope.attachementlist1.push({
                                    ihwatT_FileName: qq.ihwatT_FileName,
                                    ihW_Attachment: 'HyperLink',
                                    ihwatT_Attachment: qq.ihwatT_Attachment
                                })
                            }
                        })

                        $scope.attachementlistnew = $scope.attachementlist1;
                        $scope.docshowary = true;
                        $scope.docshow = false;
                        $('#myModalCoverview').modal('show');
                    }
                    else {
                        swal("No Data Found.")
                    }
                });
        };
    }
})();