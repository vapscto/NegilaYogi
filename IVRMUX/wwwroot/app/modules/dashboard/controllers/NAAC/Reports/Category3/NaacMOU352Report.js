(function () {
    'use strict';
    angular
        .module('app')
        .controller('NaacMOU352Report', NaacMOU352Report)

    NaacMOU352Report.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function NaacMOU352Report($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }



        $scope.ListofItems = [];
        $scope.ListofItems = [{ name: "BC0031", program: "Program-E", certificateName: "Automation Certificate", year: "2018-2019" },
        { name: "BC0032", program: "Program-D", certificateName: " Certificate-D", year: "2017-2018" },
        { name: "BC0033", program: "Program-C", certificateName: " Certificate-C", year: "2016-2017" },
        { name: "BC0034", program: "Program-B", certificateName: " Certificate-B", year: "2015-2016" },
        { name: "BC0035", program: "Program-A", certificateName: " Certificate-A", year: "2014-2015" },
        ]

        $scope.cancel = function () {
            $state.reload();

        }
        //$scope.loaddata = function () {

        //    var pageid = 2;
        //    apiService.getURI("NaacMOU352Report/getdata", pageid).then(function (promise) {

        //        $scope.yearlist = promise.allacademicyear;

        //        var s = 0;
        //        angular.forEach($scope.yearlist, function (pp) {
        //            if (s < $scope.noofyear) {
        //                pp.select = true;
        //            }
        //            s += 1;
        //        })


        //    })
        //}


        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("NaacMOU352Report/getdata", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })

            })
        }


        $scope.exportToExcel = function (printSectionId) {
            var excelname = 'Cat 3.7.3.xlsx';
            var exportHref = Excel.tableToExcel(printSectionId, '3.7.3');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }


        //$scope.exportToExcel = function (tableId) {
        //    var exportHref = Excel.tableToExcel(tableId, '3.7.3');
        //    $timeout(function () { location.href = exportHref; }, 1000);
        //}




        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/doc" || input.files[0].type === "application/docx" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //----------End..........!



        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        }


        //$scope.yearlist = [];
        //$scope.yearlist = [
        //    { asmaY_Year: "2018-2019", asmaY_Id: 5 },
        //    { asmaY_Year: "2017-2018", asmaY_Id: 4 },
        //    { asmaY_Year: "2016-2017", asmaY_Id: 3 },
        //    { asmaY_Year: "2015-2016", asmaY_Id: 2 },
        //    { asmaY_Year: "2014-2015", asmaY_Id: 1 },
        //]




        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;



        $scope.isOptionsRequired = function () {
            return !$scope.yearlist.some(function (options) {
                return options.select;
            });
        }

        //======================report.
        $scope.submitted = false;
        $scope.get_report = function () {
          
            $scope.showflag = false;
            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            })
            if ($scope.myForm.$valid) {
                var data = {
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst,
                }
                apiService.create("NaacMOU352Report/get_report", data).then(function (promise) {
                 
                    $scope.reportlist = promise.reportlist;
                    $scope.alldata12 = promise.reportlist2;

                    if (promise.reportlist.length > 0) {
                        angular.forEach($scope.reportlist, function (tt) {
                            $scope.mainArray = [];
                            //var count = 0;
                            angular.forEach($scope.alldata12, function (ss) {
                                if (tt.NCACAW342_Id == ss.ncacaW342_Id) {
                                    $scope.mainArray.push(ss);
                                }
                            })
                            tt.listdata = $scope.mainArray;
                        })
                        console.log($scope.reportlist);
                        $scope.showflag = true;

                    }
                    else {
                        $scope.showflag = false;
                        swal('Record Not Found!');
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


        $scope.get_selcetYear = function (data) {

            var nofyear = Number($scope.noofyear);
            angular.forEach($scope.yearlist, function (tt) {
                tt.select = false;
            })
            var s = 0;
            angular.forEach($scope.yearlist, function (pp) {
                if (s < nofyear) {
                    pp.select = true;
                }
                s += 1;
            })

        }

        $scope.togchkbx = function () {
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
        }

    }
})();

