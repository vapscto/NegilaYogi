(function () {
    'use strict';

    angular
        .module('app')
        .controller('Hostel_Student_Gatepass_ReportController', Hostel_Student_Gatepass_ReportController);

    Hostel_Student_Gatepass_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$q']

    function Hostel_Student_Gatepass_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $q) {

        $scope.takescreenshot = false;
        $scope.hrcommentflag = false;
        $scope.disablemonth = false;
        $scope.ddate = new Date();

        $scope.submitted = false;

        $scope.loadData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;

            $scope.all_checkC = function () {
                var checkStatus = $scope.amcsT_Id;
                var count = 0;
                angular.forEach($scope.employees, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });

            }            
            $scope.isOptionsRequired = function () {
                return !$scope.employees.some(function (options) {
                    return options.selected;
                });
            }     
         
            $scope.togchkbxC1 = function () {

                $scope.amcsT_Id = $scope.employees.every(function (options) {
                    return options.selected;
                });
            }

            apiService.getURI("Hostel_Student_Gatepass_Process/onloaddata", pageid).then(function (promise) {
             
                $scope.employees = promise.employees;
            });
        };
        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
        };

        $scope.ShowReportdata = function () {
            $scope.submitted = true;
            $scope.from_date = new Date($scope.fromdate).toDateString();
            $scope.to_date = new Date($scope.todate).toDateString();
            if ($scope.myForm.$valid) {                
                var AMCSTId = [];
                angular.forEach($scope.employees, function (ty) {
                    if (ty.selected) {
                        AMCSTId.push({ AMCST_Id: ty.amcsT_Id });
                    }
                })                
                var data = {                 
                    "AMCSTId": AMCSTId,
                    "fromdate": $scope.from_date,
                    "todate": $scope.to_date
                };

                apiService.create("Hostel_Student_Gatepass_Process/getapprovalreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.approvalReport = promise.approvalReport;

                    }
                        else {
                            swal("No Records Found");
                        $scope.approvalReport = []
                    }
                    

                });
            }

        };

        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


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
            else {
                $window.open($scope.imagepreview)
            }
        };
        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }

        $scope.content1 = "";
        ///=====================show pdf, img
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;


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
        };


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        ///=====================show pdf, img end
        $scope.Print = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExceldetails = function () {

            var excelname = ' HOSTEL STUDENTS GATEPASS REPORT';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#exceltopd', ' HOSTEL STUDENTS GATEPASS REPORT');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

    }
})();


