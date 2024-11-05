(function () {
    'use strict';
    angular
        .module('app')
        .controller('TodaysAppointment', TodaysAppointment)

    TodaysAppointment.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function TodaysAppointment($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

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



        $scope.searchValue = "";

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchValue2 = "";

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey2 == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey2 = key;
        }

        $scope.onselectradio = function () {
            $scope.Cumureport = false;
            $scope.Cumureport1 = false;
            $scope.screport = false;
            $scope.export = false;
        }
        $scope.griddata = [];
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.radiotype = "Approved";
        //===============loaddata 
        $scope.loadgrid = function () {
            apiService.getURI("V_AppointmentApprovalReport/loaddatatoday/", 5).then(function (promise) {
                debugger;
                $scope.startfromdate = new Date(promise.vmaP_MeetingDateTime);
                if (promise.viewlist.length > 0) {

                    $scope.griddata = promise.viewlist;
                    $scope.Cumureport = true;
                    $scope.Cumureport1 = false;

                    $scope.screport = true;
                    $scope.export = true;
                }
                else {
                    //swal("No Records Found");

                    $scope.screport = false;
                    $scope.export = false;
                    $scope.Cumureport = false;
                    $scope.Cumureport1 = false;
                }
            });
        }

       

        // ======================to get Report
        $scope.submitted = false;
        $scope.ondatechange = function () {
            $scope.griddata = [];
            $scope.submitted = true;
      
          

            var fromdate1 = $scope.startfromdate == null ? "" : $filter('date')($scope.startfromdate, "yyyy-MM-dd");
         

            if ($scope.myForm.$valid) {

                var data = {
                    "VMAP_MeetingDateTime": fromdate1,
                }

                apiService.create("V_AppointmentApprovalReport/ondatechange", data).
                    then(function (promise) {

                        if (promise.viewlist.length > 0) {

                            $scope.griddata = promise.viewlist;
                            $scope.Cumureport = true;
                            $scope.Cumureport1 = false;

                            $scope.screport = true;
                            $scope.export = true;
                        }
                        else {
                            //swal("No Records Found");

                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            $scope.Cumureport1 = false;
                        }

                    })
            }

        };
        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        //for print
        $scope.Print = function () {
            var innerContents = '';           
            innerContents = document.getElementById("printSectionId").innerHTML;        
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/Visitor_Management/InwardReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        // end for print

        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');

            $timeout(function () { location.href = exportHref; }, 100);
        }
     
        $scope.cancel = function () {          
            $state.reload();

        };

        $scope.togchkbx = function () {
            $scope.institutionlist.every(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.institutionlist.some(function (options) {
                return options.select;
            });
        }
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.institutionlist, function (itm) {
                itm.select = checkStatus;
            });
        }


        $scope.reschedule = function (obj) {
            $scope.sche = "R";
            $scope.VisitorList = [];
            $scope.HRME_Id = "";

            $scope.entry_date = "";
            $scope.metting_date = "";
            $scope.meeting_Time = "";
            $scope.VMAP_MeetingToTime = "";
            $scope.status_flg = "";
            $scope.remarks_data = "";
            $scope.allvisitor = [];
            $scope.cname = obj.MI_Name;
            $scope.inddate = obj.VMAP_EntryDateTime;
            $scope.piino = obj.empname;
            $scope.placee = obj.VMAP_FromPlace;
            $scope.purposee = obj.VMAP_MeetingPurpose;
            $scope.olc = obj.VMAP_MeetingLocation;
            $scope.VMAP_Id = obj.VMAP_Id;
            $scope.mI_Id = obj.mI_Id;
            $scope.HRME_Id = obj;
            $scope.metting_date = new Date(obj.VMAP_MeetingDateTime);
            //$scope.meeting_Time = moment(obj.VMAP_MeetingTiming, 'h:mm a').format();
            //$scope.VMAP_MeetingToTime = moment(obj.VMAP_MeetingToTime, 'h:mm a').format();

            $scope.meeting_Time = obj.VMAP_MeetingTiming;
            $scope.VMAP_MeetingToTime = obj.VMAP_MeetingToTime;
            $scope.remarks_data = obj.VMAP_Remarks;
            apiService.create("V_AppointmentApprovalStatus/Editnew/", obj).then(function (promise) {

                $scope.editDetails = promise.editDetails;
                $scope.editfiles = promise.editfiles;
                $scope.extvisitorlist = promise.visitorlist;

                angular.forEach($scope.editDetails, function (rr) {
                    $scope.allvisitor.push({ NAME: rr.vmaP_VisitorName, MB: rr.vmaP_VisitorContactNo, EM: rr.vmaP_VisitorEmailid, ADD: rr.vmaP_FromAddress })

                })

                angular.forEach($scope.extvisitorlist, function (rr) {
                    $scope.allvisitor.push({ NAME: rr.vmapvI_VisitorName, MB: rr.vmapvI_VisitorContactNo, EM: rr.vmapvI_VisitorEmailId, ADD: rr.vmapvI_VisitorAddress })

                })

                $scope.showgrd = true;
            });
        };



        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("V_AppointmentApprovalStatus/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    debugger;
                    $scope.uploadfilesdetails = promise.editfiles;
                    if (promise.editfiles !== null && promise.editfiles.length > 0) {
                        $scope.uploaddocfiles = promise.editfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.onview = function (filepath, filename) {
            debugger;
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        };



        $scope.deleteuploadfile = function (obj) {

            var data = {
                "VMAPVF_Id": obj.cfileid
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("VisitorAppointment/deleteuploadfile", data).then(function (promise) {
                            //if (promise.already_cnt === true) {
                            //    swal("You Can Not Deactivate This Record,It Has Dependency");
                            //}
                            //else {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                            //}
                            //$('#popup11').modal('hide');

                            $scope.viewdocument($scope.objdata)
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };



        //////end////

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