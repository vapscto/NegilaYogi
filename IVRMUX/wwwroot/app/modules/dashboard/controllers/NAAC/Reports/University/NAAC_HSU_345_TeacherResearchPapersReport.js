
(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC_HSU_345_TeacherResearchPapersReport', NAAC_HSU_345_TeacherResearchPapersReport)

    NAAC_HSU_345_TeacherResearchPapersReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout']
    function NAAC_HSU_345_TeacherResearchPapersReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.submitted = false;

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("NAAC_MC_312_TeachersResearch/getdata", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;
                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })
            })
        }
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
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        }
        $scope.togchkbx = function () {
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
        }
        $scope.printflag = false;
        $scope.Report = function () {
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
                apiService.create("NAAC_MC_312_TeachersResearch/NAAC_MC_345_TeachersResearchReport", data).then(function (promise) {
                 
                    if (promise.govtsclist.length > 0 && promise.govtsclist != null) {
                        $scope.showflag = true;
                        $scope.printflag = true;
                        $scope.govtsclist = promise.govtsclist;
                        $scope.govtsclistfiles = promise.govtsclistfiles;

                        $scope.finalarray = [];
                        angular.forEach($scope.govtsclist, function (gg) {
                            var govtfiles = [];
                            angular.forEach($scope.govtsclistfiles, function (ff) {
                                if (gg.NCHSU345TRP_Id == ff.nchsU345TRP_Id) {
                                    govtfiles.push({ filename: ff.nchsU345TRPF_FileName, filepath: ff.nchsU345TRPF_FilePath, filedesc: ff.nchsU345TRPF_FileDesc })
                                }
                            })
                            $scope.finalarray.push({ YEAR: gg.ASMAY_Year, NCHSU345TRP_PaperTitle: gg.NCHSU345TRP_PaperTitle, NCHSU345TRP_AuthorName: gg.NCHSU345TRP_AuthorName, NCHSU345TRP_DepartmentName: gg.NCHSU345TRP_DepartmentName, NCHSU345TRP_JournalName: gg.NCHSU345TRP_JournalName, NCHSU345TRP_ISSNNumber: gg.NCHSU345TRP_ISSNNumber, NCHSU345TRP_link: gg.NCHSU345TRP_link, NCHSU345TRP_NamesOfIndexingDatabases: gg.NCHSU345TRP_NamesOfIndexingDatabases, FILES: govtfiles })
                        })

                        $scope.yearlist = promise.yearlist;
                        $scope.finalarray1 = [];
                        angular.forEach($scope.yearlist, function (yy) {
                            var temparr = [];
                            angular.forEach($scope.finalarray, function (dd) {
                                if (yy.asmaY_Year == dd.YEAR) {
                                    temparr.push(dd);
                                }


                            })

                            if (temparr.length > 0) {
                                $scope.finalarray1.push({ YEAR: yy.asmaY_Year, dlist: temparr })
                            }

                        })
                        console.log($scope.finalarray);
                    }
                    else {
                        swal('No Records Found!')
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }
    }
})();







