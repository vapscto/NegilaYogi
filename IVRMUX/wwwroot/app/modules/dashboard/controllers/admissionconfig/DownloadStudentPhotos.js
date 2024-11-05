(function () {
    'use strict';
    angular
        .module('app')
        .controller('DocumentviewReportController1', DocumentviewReportController1)

    DocumentviewReportController1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile', '$sce']
    function DocumentviewReportController1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile, $sce) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.submitted = false;
        $scope.reportgrid = false;
        $scope.studentarray = [];
        $scope.BindData = function () {
            apiService.get("Smartcarddetails/getdetails/").then(function (promise) {

                $scope.year_list = promise.academicList;
                $scope.classlists = promise.classlist;
                $scope.sectionDropdown = promise.sectionList;
            });
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.studentarray = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    if ($scope.studentarray.indexOf(itm) === -1) {
                        $scope.studentarray.push(itm);
                    }
                }
                else {
                    $scope.studentarray = [];
                }
            });
        }
        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.AMST_AdmNo).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.studentName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.ASMCL_ClassName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.ASMC_SectionName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }
        $scope.PhotoReport = function () {
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                }
                apiService.create("Smartcarddetails/getstudentdetails", data)
                    .then(function (promise) {

                        if (promise.studentList1 != null && promise.studentList1.length > 0) {
                            $scope.students = promise.studentList1;
                            $scope.reportgrid = true;
                        }
                        else {
                            swal('No Records Found!');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.studentarray.indexOf(SelectedStudentRecord) === -1) {
                $scope.studentarray.push(SelectedStudentRecord);
            }
            else {
                $scope.studentarray.splice($scope.studentarray.indexOf(SelectedStudentRecord), 1);
            }
        }
        //TO clear  data
        $scope.clearid = function () {

            $scope.asmaY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.reportgrid = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
        var studentreg = "";
        var imagedownload = "";
        var docname = "";

        $scope.downloaddirectimage = function (data, idd, typeofphoto) {
            if ($scope.students.length > 0) {
                for (var i = 0; i < $scope.students.length; i++) {
                    if ($scope.students[i].AMST_Id == idd) {
                        studentreg = $scope.students[i].AMST_AdmNo;
                    }
                }
            }

            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '-' + docname + '.jpg'
                    })[0].click();
                })
        }


        $scope.downloadAll = function () {
            var downloadLinks = [];
            angular.forEach($scope.studentarray, function (ig) {
                if (ig.AMST_AdmNo != null) {
                    var imageLink = document.createElement('a');
                    imageLink.href = ig.AMST_Photoname;
                    imageLink.download = 'stud_' + ig.AMST_AdmNo + '.jpg';
                    downloadLinks.push(imageLink);
                }
            });
            function triggerDownloads() {
                if (downloadLinks.length === 0) return;
                var link = downloadLinks.shift();
                link.style.display = 'none';
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                setTimeout(triggerDownloads, 100);
            }
            triggerDownloads();
        };







    }
})();

