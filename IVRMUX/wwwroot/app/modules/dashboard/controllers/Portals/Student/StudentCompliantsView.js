(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentCompliantsView', StudentCompliantsView)

    StudentCompliantsView.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function StudentCompliantsView($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.NCACAW342_Id = 0;
        $scope.instit = false;
        $scope.ASCOMP_Date = new Date();
        $scope.maxdate = new Date();
        //======================page load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("StudentCompliantsView/loaddata", pageid).then(function (promise) {
                $scope.allacademicyear = promise.yearlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.studentdivlist = promise.studentdivlist;

                angular.forEach($scope.studentdivlist, function (dd) {
                    if (dd.ASCOMP_FilePath !== null && dd.ASCOMP_FilePath !== "") {
                        var img = dd.ASCOMP_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.filetype = lastelement;
                        console.log("data.filetype : " + dd.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ASCOMP_FilePath;
                        }
                    }
                });

            })
        };

        $scope.report1 = function () {
            $scope.studentdivlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("StudentCompliantsView/report1", data).then(function (promise) {
                if (promise.studentdivlist !== null && promise.studentdivlist.length > 0) {
                    $scope.studentdivlist = promise.studentdivlist;

                    angular.forEach($scope.studentdivlist, function (dd) {
                        if (dd.ASCOMP_FilePath !== null && dd.ASCOMP_FilePath !== "") {
                            var img = dd.ASCOMP_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("data.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ASCOMP_FilePath;
                            }
                        }
                    });
                }
                else {
                    swal("No Record Found...!!!");
                    $scope.studentdivlist = [];
                    $scope.ASMAY_Id = '';
                    $scope.loaddata();
                }
            })
        };

        $scope.search = "";

        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });

})();

