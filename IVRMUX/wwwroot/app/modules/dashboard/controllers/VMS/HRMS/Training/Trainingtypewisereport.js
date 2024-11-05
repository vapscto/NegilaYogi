(function () {
    'use strict';
    angular.module('app').controller('TrainingtypewisereportController', TrainingtypewisereportController)
    TrainingtypewisereportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TrainingtypewisereportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.obj = {};
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
           
            var pageid = 2;
            apiService.getURI("Trainingtypewisereport/onloaddata", pageid).then(function (promise) {
                $scope.getloaddetails = promise.getloaddetails;
                $scope.emp_list = promise.getloaddetails;
            });
        };

        $scope.monthdays = function () {
            if ($scope.all1 == 1) {
                $scope.days1 = "";
                $scope.days2 = "";
            }
            else if ($scope.all1 == 0) {
                $scope.days = "";
            }
        }



 



        $scope.ShowReport = function () {
            $scope.searchValue = "";
            var data = {
                "HRMETRTY_Id": $scope.obj.hrmetrtY_Id.hrmetrtY_Id,
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            $scope.submitted = true;
           // if ($scope.myForm.$valid) {
                apiService.create("Trainingtypewisereport/getreport", data).
                    then(function (promise) {
                        if (promise.trainingtypewisereport.length > 0) {
                            $scope.trngtpwisrpt = promise.trainingtypewisereport;
                            $scope.presentCountgrid = $scope.trngtpwisrpt.length;
                        }
                        else {
                            swal("No Records Found");
                            $scope.count = 0;
                        }
                    })
          //  }
        };

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Training").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("Training");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
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

        $scope.download = function (hrexttrN_CertificateFilePath) {
            $http.get('https://unsplash.it/200/300', {
                responseType: "arraybuffer"
            })
                .success(function (hrexttrN_CertificateFilePath) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([hrexttrN_CertificateFilePath]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: 'fileName.png'
                    })[0].click();
                })
        }









        

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();

