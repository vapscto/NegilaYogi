(function () {
    'use strict';
    angular
        .module('app')
        .controller('LibTransactionReportController', LibTransactionReportController)

    LibTransactionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout','Excel']
    function LibTransactionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel) {

        $scope.submitted = false;
        $scope.tablediv = false;
        $scope.printd = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        var copty;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.Binddata = function () {
            debugger;
          
            var pageid = 2;
            apiService.getURI("LibTransactionReport/Getdetails", pageid).then(function (promise) {
          


                    $scope.yearlt = promise.yearlist;
                $scope.msterliblist1 = promise.msterliblist1;


                })

        }

        $scope.statuscount = false;
        $scope.alldata = [];
       //---------------Get-Report
        $scope.get_report = function () {
            $scope.alldata = [];
            $scope.finaltotal = 0;
            if ($scope.myForm.$valid) {
                debugger;
                var fromdate1 = $scope.Issue_Datefrm == null ? "" : $filter('date')($scope.Issue_Datefrm, "yyyy-MM-dd");
                var todate1 = $scope.IssueToDateto == null ? "" : $filter('date')($scope.IssueToDateto, "yyyy-MM-dd");
                var data = {
                    "statuscount": $scope.statuscount,
                    "Issue_Date": fromdate1,
                    "IssueToDate": todate1,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "LMAL_Id": $scope.LMAL_Id,
                }

                apiService.create("LibTransactionReport/get_report", data).then(function (promise) {
                    if (promise.alldata.length > 0) {
                        $scope.alldata = promise.alldata;

                        angular.forEach($scope.alldata, function (dd) {

                            $scope.finaltotal += dd.seccount;
                        })
                        
                        if ($scope.statuscount == false) {
                            $scope.classlist = promise.classlist;
                           

                            $scope.subexam_list = [];
                            angular.forEach($scope.alldata, function (sme) {
                                if ($scope.subexam_list.length == 0) {
                                    $scope.subexam_list.push({ ASMCL_Id: sme.ASMCL_Id, ASMCL_ClassName: sme.ASMCL_ClassName });
                                }
                                else if ($scope.subexam_list.length > 0) {
                                    var sub_exm_cnt = 0;
                                    angular.forEach($scope.subexam_list, function (exm) {
                                        if (exm.ASMCL_Id == sme.ASMCL_Id ) {
                                            sub_exm_cnt += 1;
                                        }
                                    })
                                    if (sub_exm_cnt == 0) {
                                        $scope.subexam_list.push({ ASMCL_Id: sme.ASMCL_Id, ASMCL_ClassName: sme.ASMCL_ClassName  });
                                    }
                                }

                            })
                            console.log($scope.subexam_list);


                            $scope.newarray = [];
                            angular.forEach($scope.subexam_list, function (ff) {
                                var clscount = 0;
                                angular.forEach($scope.alldata, function (gg) {
                                  
                                    if (ff.ASMCL_Id == gg.ASMCL_Id) {
                                        clscount += gg.seccount;
                                        $scope.newarray.push({ ASMCL_Id: gg.ASMCL_Id, ASMCL_ClassName: gg.ASMCL_ClassName, ASMS_Id: gg.ASMS_Id, ASMC_SectionName: gg.ASMC_SectionName, seccount: gg.seccount})
                                       
                                    }


                                })

                                $scope.newarray.push({ ASMCL_Id: ff.ASMCL_Id, ASMCL_ClassName:'TOTAL', ASMS_Id: '', ASMC_SectionName: 'TOTAL', seccount: clscount })
                            })
                        }
                     
                       
                    }
                    else {
                        swal('Record Not Available!!!');
                        $state.reload();
                    }
                })

            }
            else {
                $scope.submitted = true;
            }

        }
        //-------------End--Get-Report



        //===========print===========//
        $scope.printData = function () {
            var innerContents = '';

            if ($scope.statuscount == true) {
                innerContents = document.getElementById("printareaId22").innerHTML;
            } else {
                innerContents = document.getElementById("printareaId").innerHTML;
            }


            //var innerContents = document.getElementById("printareaId").innerHTML;
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

        $scope.exportToExcel = function () {

            var tbl = '';

            if ($scope.statuscount == true) {
                tbl = '#table1111';
            } else {
                tbl = '#table11';
            }

            var exportHref = Excel.tableToExcel(tbl, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        }
        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

