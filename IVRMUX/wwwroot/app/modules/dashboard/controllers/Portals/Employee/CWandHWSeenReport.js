(function () {
    'use strict';
    angular.module('app')
        .controller('HomeworkUploadController', HomeworkUploadController)
    HomeworkUploadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function HomeworkUploadController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.FromDate = new Date();
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
        if (ivrmcofigsettings.length > 0) {
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
        $scope.sectionarray = [];
        $scope.search = "";

        $scope.loaddata = function () {
            $scope.screport = false;

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;
            var pageid = 2;
            apiService.getURI("HomeworkUpload/Getdata_class", pageid).
                then(function (promise) {
                    $scope.classlist = promise.classlist;
                    $scope.sectionlist = promise.sectionlist;


                })
        };


        

        //$scope.al_checkclass = function (all, ASMCL_Id) {

        //    $scope.classlistarray = [];
        //    $scope.obj.usercheckCC = all;

        //    var toggleStatus = $scope.obj.usercheckCC;
        //    angular.forEach($scope.classlist, function (role) {
        //        role.selected = toggleStatus;
        //    });


        //    $scope.classlistarray = [];
        //    angular.forEach($scope.classlist, function (qq) {
        //        if (qq.selected == true) {
        //            $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
        //        }
        //    });


        //    if ($scope.obj.usercheckCC == true) {
        //        $scope.getsection();
        //        $scope.classflag = true;
        //    }
        //    else {
        //        $scope.sectionlist = [];

        //    }

        //}
        $scope.getsection = function () {

            $scope.classlistarray = [];

            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.ASMCL_Id })
                }

            });

            if ($scope.classlistarray != null) {
                $scope.classflag = true;
            }


            
            var data = {
               
                "classarray": $scope.classlistarray,

            }
            apiService.create("HomeworkUpload/getsection", data).then(function (promise) {
                $scope.studentlist1 = [];
                $scope.studentlist = [];
                $scope.sectionlist = promise.sectionlist;
                if ($scope.sectionlist.length > 0 || $scope.sectionlist != null) {
                    $scope.sectionlist = promise.sectionlist;
                }
                else {
                    swal('No Data Found!!!')
                }
            })
            // }
        }
        $scope.toggleAll = function () {
            $scope.classlistarray = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.classlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.classlistarray.indexOf(itm) === -1) {
                        $scope.classlistarray.push({ ASMCL_Id: itm.ASMCL_Id });

                    }
                }
                else {
                    $scope.classlistarray.splice({ ASMCL_Id: itm.ASMCL_Id });

                }

               
            });
            $scope.getsection();
        }

        $scope.isOptionsRequired = function () {

            return !$scope.classlist.some(function (sec) {
                return sec.selected;
            });
        }

        $scope.all_checkC = function () {
            $scope.sectionlistarray = [];
            var toggleStatus = $scope.alll;
            angular.forEach($scope.sectionlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.sectionlistarray.indexOf(itm) === -1) {
                        $scope.sectionlistarray.push({ asmS_Id: itm.asmS_Id });

                    }
                }
                else {
                    $scope.sectionlistarray.splice({ asmS_Id: itm.asmS_Id });

                }


            });
           
        }

        $scope.getsectionitem = function () {

            $scope.sectionlistarray = [];

            angular.forEach($scope.sectionlist, function (aa) {
                if (aa.selected == true) {
                    $scope.sectionlistarray.push({ ASMCL_Id: aa.ASMCL_Id })
                }

            });
            
        }


        $scope.submitted = false;

       
        $scope.showreport = function () {

            if ($scope.myForm.$valid) {
                $scope.classarraynew = [];

                $scope.sectionarraynew = [];
                if ($scope.classlistarray != null || $scope.classlistarray > 0) {
                    angular.forEach($scope.classlist, function (qq) {
                        if (qq.selected == true) {
                            $scope.classarraynew.push({ ASMCL_Id: qq.ASMCL_Id })
                        }

                    })
                }
                else {
                    $scope.classarraynew = undefined;
                }

                if ($scope.sectionlistarray != null || $scope.sectionlistarray > 0) {
                    angular.forEach($scope.sectionlist, function (qq) {
                        if (qq.selected == true) {
                            $scope.sectionarraynew.push({ asmS_Id: qq.asmS_Id })
                        }

                    })
                }
                else {
                    $scope.sectionarraynew = undefined;
                }

                $scope.fromdate = $filter('date')($scope.fromdate, "yyyy-MM-dd");
                $scope.todate = $filter('date')($scope.todate, "yyyy-MM-dd");
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "classarray": $scope.classarraynew,
                    "sectionarray": $scope.sectionarraynew,
                    "flag": $scope.optionflag
                }
                apiService.create("HomeworkUpload/getseenreport", data).
                    then(function (promise) {
                        if (promise.seen_unseenlist.length > 0 && promise.seen_unseenlist != null) {
                            $scope.seen_unseenlist = promise.seen_unseenlist;
                            $scope.reportlist = promise.reportlist;
                         
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

        $scope.showdetails_seen = function (user) {

            //stu_id = stu
          
            var seen = 1;
            //if ($scope.optionflag = 'Homework') {
            //    id=user.IHW_Id
            //}
            //else if ($scope.optionflag = 'classwork') {
            //    id = user.ICW_Id
            //}
            //else if ($scope.optionflag = 'NoticeBoard') {
            //    id = user.INTB_Id
            //}
            
            var data = {
               
                "ASMCL_Id": user.ASMCL_Id,
                "ASMS_Id": user.ASMS_Id,
                "flag": $scope.optionflag,
                "seen_Topicid": user.topic_id,
                "seen_unseen": seen

            }

                apiService.create("HomeworkUpload/Getdataview_seen", data).
                then(function (promise) {
                    $scope.view_array = promise.view_array;

                })
            // $('#popup111').modal('show');
            $('#myModall').modal('show');
        }

        $scope.showdetails_unseen = function (user) {

            //stu_id = stu
            
            var unseen = 0;
            //if ($scope.optionflag = 'Homework') {
            //    id=user.IHW_Id
            //}
            //else if ($scope.optionflag = 'classwork') {
            //    id = user.ICW_Id
            //}
            //else if ($scope.optionflag = 'NoticeBoard') {
            //    id = user.INTB_Id
            //}

            var data = {

                "ASMCL_Id": user.ASMCL_Id,
                "ASMS_Id": user.ASMS_Id,
                "flag": $scope.optionflag,
                "seen_Topicid": user.topic_id,
                "seen_unseen": unseen

            }

            apiService.create("HomeworkUpload/Getdataview_seen", data).
                then(function (promise) {
                    $scope.view_array = promise.view_array;

                })
            // $('#popup111').modal('show');
            $('#myModall').modal('show');
        }

        //export start
        $scope.exportToExcel = function (tableId) {
            if ($scope.seen_unseenlist !== null && $scope.seen_unseenlist.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
          
        };
        $scope.clear = function () {
            $scope.seen_unseenlist = [];
        }
        //add by raj
        $scope.exportToExcel1 = function (tableId) {
            if ($scope.view_array !== null && $scope.view_array.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            //else {
            //    swal("Please select records to be Exported");
            //}

        };



        $scope.printData = function () {
            if ($scope.seen_unseenlist !== null && $scope.seen_unseenlist.length > 0) {
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

        //add by raj
        $scope.printData1 = function (table1) {
            if ($scope.view_array !== null && $scope.view_array.length > 0) {
                var innerContents = document.getElementById("table1").innerHTML;
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
        //add by raj download
        //Pdf download
        $scope.download = function () {
            html2canvas(document.getElementById('myModal'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            image: data,
                            width: 500,
                            height: 600
                        }]
                    };
                    pdfMake.createPdf(docDefinition).download("RECEIPT.pdf");
                }
            });
        };



        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //add by Raj


    }
})();