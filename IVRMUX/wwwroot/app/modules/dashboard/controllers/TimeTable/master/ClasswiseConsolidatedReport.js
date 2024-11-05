(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClasswiseConsolidatedReport', ClasswiseConsolidatedReport)

    ClasswiseConsolidatedReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function ClasswiseConsolidatedReport($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {

        //$scope.loaddata = function () {
        //    debugger;
        //    var pageid = 2;
        //    apiService.getURI("ClasswiseConsolidatedReport/loaddata", pageid).then(function (promise) {
        //        debugger;
        //        $scope.classlist = promise.classlist;

        //        $scope.sectionlist = promise.sectionlist;
        //    })

        //}
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        var paginationformasters = '';
        var ivrmcofigsettings = [];

        
        var copty;
         ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = [];
      
        $scope.coptyright = copty;
        $scope.ddate = new Date();
         admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;







        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.submitted = false;

        $scope.loaddata = function () {
        
            apiService.getDATA("ClasswiseConsolidatedReport/loaddata").
                then(function (promise) {
                  
                    $scope.classlist = promise.classlist;
                    $scope.yearlist = promise.yearlist;
                    $scope.sectionlist = promise.sectionlist;
                })
        };



        $scope.exportToExcel = function () {

            var table = '';
            
                table = '#A';
            

            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }

        $scope.printData = function () {

            var innerContents = '';
          
            innerContents = document.getElementById("printareaId").innerHTML;
           


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










        $scope.Report = function () {
         

            $scope.submitted = true;
           
          
                // $scope.getEmailsendingConfirmation();


                var sectionId = $scope.ASMS_Id;
                var classId = $scope.ASMCL_Id;
                var yearId = $scope.asmaY_Id;


                if (classId === undefined || classId === "") {
                    classId = 0;
                }
                if (sectionId === undefined || sectionId === "") {
                    sectionId = 0;
                }
                if (yearId === undefined || yearId === "") {
                    yearId = 0;
                }
                if ($scope.tsallorindii === null || $scope.tsallorindii === undefined) {
                    swal("Select The Required Radio Buttons and Fields");
                }
                var data = {

                    "ASMCL_Id": classId,
                    "ASMS_Id": sectionId,
                    "ASMAY_Id": yearId,
                    //"N_ActivityDate": $scope.N_ActivityDate,
                    //"NC_ActivityDate": $scope.NC_ActivityDate,
                    //"radiobutton": $scope.tsallorindii

                };

                apiService.create("ClasswiseConsolidatedReport/Report", data).then(function (promise) {



                    $scope.subject = promise.subject;
                    $scope.day = promise.day;
                    $scope.getreportdata = promise.getreportdata;
                    $scope.mainarray = [];
                    angular.forEach($scope.subject, function (y) {

                        var temp = [];
                        var count = 0;
                        angular.forEach($scope.getreportdata, function (m) {

                            if (y.ismS_Id == m.ISMS_Id) {
                                temp.push(m);
                                count += m.PCOUNT;
                            }

                        })
                        $scope.mainarray.push({
                            subjectname: y.ismS_SubjectName, ismS_Id: y.ismS_Id, ttlist: temp, total: count

                        })

                    })

                    console.log($scope.mainarray);


                })

                //else if (($scope.tsallorindii) === 'I') {
                //    apiService.create("ClasswiseConsolidatedReport/Report", data).then(function (promise) {
                //        debugger;
                //        $scope.alldata2 = promise.alldata2;
                //    })
                //}

           


        }
    }
})();