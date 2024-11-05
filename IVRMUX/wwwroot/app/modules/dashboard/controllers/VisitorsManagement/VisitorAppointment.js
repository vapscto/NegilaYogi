(function () {
    'use strict';
    angular.module('app').controller('VisitorAppointmentController', VisitorAppointmentController)

    VisitorAppointmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', '$q', '$sce']
    function VisitorAppointmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, $q,$sce) {


        //$scope.valstrtm = function (timedata) {
        //    $scope.totime = timedata;
        //    var hh = $scope.totime.getHours();
        //    var mm = $scope.totime.getMinutes();
        //    $scope.min = timedata;

        //    $scope.min.setMinutes(hh);
        //    $scope.min.setMinutes(mm);
        //    $scope.minlnc = timedata;

        //    $scope.minlnc.setMinutes(hh);
        //    $scope.minlnc.setMinutes(mm);
        //    $scope.VMAP_MeetingToTime = "";

        //}

        //$scope.validatemax = function (maxdata) {
        //    debugger;
        //    // $scope.FOMST_IHalfLoginTime = maxdata;
        //    //$scope.FOMST_IIHalfLogoutTime = "";
        //    if (maxdata >= new Date($scope.meeting_Time)) {
        //        $scope.totimemax = maxdata;
        //        var hh = $scope.totimemax.getHours();
        //        var mm = $scope.totimemax.getMinutes();
        //        $scope.max = maxdata;

        //        $scope.max.setMinutes(hh);
        //        $scope.max.setMinutes(mm);
        //    }
        //    else {
        //        $scope.VMAP_MeetingToTime = $scope.meeting_Time;
        //    }

        //    // $scope.FOMST_IHalfLogoutTime = "";
        //}

       
        $scope.validatemax = function (maxdata) {

            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            var dsttimee = $scope.FOMST_IHalfLoginTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            //var startTime = moment(dsttimee, "HH:mm:ss a");
            //  var endTime = moment(maxdata, "HH:mm:ss a");
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date($scope.today);
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date($scope.today);
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date($scope.today);
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.FOMST_FDWHrMin = $scope.ttst;

            $scope.htmax = new Date($scope.today);
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.FOMST_IHalfLoginTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLogoutTime = "";
            }
        };

        $scope.validateTomintime = function (timedata) {

            //$scope.timedis1 = false;
            //$scope.timedis2 = true;
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.FOMST_IHalfLogoutTime = "";
        };
        $scope.sttud = false;
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.hstep = 1;
        $scope.mstep = 1;

        //ADD MORE VISITOR
        $scope.morevisitor = [{ id: 'viss1' }];

        $scope.addvisitor = function () {
            var newItemNo = $scope.morevisitor.length + 1;

            if (newItemNo <= 10) {
                $scope.morevisitor.push({ 'id': 'viss1' + newItemNo });
            }
        };

        $scope.removeaddvisitor = function (index) {
            var newItemNo = $scope.morevisitor.length - 1;
            $scope.morevisitor.splice(index, 1);

            if ($scope.morevisitor.length === 0) {
                //data
            }
        };

        //
        $scope.HRME_Id = "";
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.loadgrid = function () {
            apiService.getURI("VisitorAppointment/getDetails/", 1).then(function (promise) {
                $scope.VMAP_EntryDateTime = new Date();
                $scope.emplist = promise.emplist;
                $scope.getdata = promise.getdata;
            });
        };
        $scope.effmin = 0;
        $scope.effhrs = 0;
        $scope.submitted = false;
        $scope.saveData = function () {
            if ($scope.myForm.$valid) {
            var counthrs = 0.00;
                if ($scope.effmin == undefined || $scope.effmin == null || $scope.effmin == '') {
                    $scope.effmin = 0;
                }
                if ($scope.effhrs == undefined || $scope.effhrs == null || $scope.effhrs == '') {
                    $scope.effhrs = 0;
                }
                counthrs = (parseFloat($scope.effmin) * 0.0166667).toFixed(2);
                counthrs = parseFloat($scope.effhrs) + parseFloat(counthrs);
                var date = new Date();
                $scope.time = $filter('date')(new Date(), 'HH:mm:ss');

                var obj = {
                    "VMAP_Id": $scope.VMAP_Id,
                    "VMAP_VisitorName": $scope.VMAP_VisitorName,
                    "VMAP_VisitorContactNo": $scope.VMAP_VisitorContactNo,
                    "VMAP_VisitorEmailid": $scope.VMAP_VisitorEmailid,
                    "VMAP_FromPlace": $scope.VMAP_FromPlace,
                    "VMAP_RequestFromTime": $filter('date')($scope.FOMST_IHalfLoginTime, "HH:mm"),
                    "VMAP_RequestToTime": $filter('date')($scope.FOMST_IIHalfLogoutTime, "HH:mm"),
                "VMAP_MeetingDuration": counthrs,
                     "VMAP_ToMeet": $scope.VMAP_ToMeet,
                    "VMAP_VisitTypeFlg": $scope.VMAP_VisitTypeFlg,
                    "VMAP_AuthorisationBy": $scope.VMAP_AuthorisationBy,
                    "VMAP_EntryDateTime": new Date($scope.VMAP_EntryDateTime).toDateString(),
                    "VMAP_MeetingPurpose": $scope.VMAP_MeetingPurpose,
                    "VMAP_PersonsAccompanying": $scope.VMAP_PersonsAccompanying,
                    "VMAP_MeetingLocation": $scope.VMAP_MeetingLocation,
                    "VMAP_FromAddress": $scope.VMAP_FromAddress,
                    "VMAP_Remarks": $scope.VMAP_Remarks,
                    filelist: $scope.materaldocuupload,
                    visitordto: $scope.morevisitor,
                    "VMAP_HRME_Id": $scope.HRME_Id.hrmE_Id,
                };
                apiService.create("VisitorAppointment/saveData", obj).then(function (promise) {
                    if (promise.returnval !== null && promise.duplicate !== null) {
                        if (promise.duplicate === false) {
                            if (promise.returnval21 === "Save") {
                                swal('Record Saved Successfully!!!');
                                $scope.outward = promise.outward;
                                $scope.screport = true;
                                $scope.SchoolLogo = promise.institution[0].mI_Logo;
                                $scope.SchollName = promise.institution[0].mI_Name;
                                $scope.SchollAdd = promise.institution[0].mI_Address1;
                                $scope.sttud = true;
                                $state.reload();
                            }
                            else if (promise.returnval21 === "Not Save") {
                                swal("Record Not Saved Successfully!!");
                            }
                            else if (promise.returnval21 === "Update") {
                                swal("Record Updated Successfully!!");
                                $state.reload();
                            }
                            else if (promise.returnval21 === "Not Update") {
                                swal("Record Not Updated Successfully!!");
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };


        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {
            debugger;
            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    $('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };

        $scope.cancel = function () {
            $scope.OW_Id = 0;
            $scope.OW_Discription = "";
            $scope.OW_From = "";
            $scope.OW_To = "";
            $scope.OW_add = "";
            $scope.OW_Date = "";
            $scope.OW_Remarks = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.screport = false;
            $scope.sttud = false;
            $state.reload();
        };

        $scope.edit = function (obj) {
            var data = {
                "VMAP_Id": obj.vmaP_Id
            };

            apiService.create("VisitorAppointment/EditDetails/", data).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.VMAP_Id = promise.editDetails[0].vmaP_Id;
                $scope.VMAP_VisitorName = promise.editDetails[0].vmaP_VisitorName;
                $scope.VMAP_VisitorContactNo = promise.editDetails[0].vmaP_VisitorContactNo;
                $scope.VMAP_VisitorEmailid = promise.editDetails[0].vmaP_VisitorEmailid;
                $scope.VMAP_ToMeet = promise.editDetails[0].vmaP_ToMeet;
                $scope.VMAP_EntryDateTime = new Date(promise.editDetails[0].vmaP_EntryDateTime);
                $scope.VMAP_FromPlace = promise.editDetails[0].vmaP_FromPlace;
                $scope.VMAP_VisitTypeFlg = promise.editDetails[0].vmaP_VisitTypeFlg;
                $scope.VMAP_MeetingPurpose = promise.editDetails[0].vmaP_MeetingPurpose;
                $scope.VMAP_FromAddress = promise.editDetails[0].vmaP_FromAddress;
                $scope.VMAP_MeetingLocation = promise.editDetails[0].vmaP_MeetingLocation;
                $scope.VMAP_Remarks = promise.editDetails[0].vmaP_Remarks;
                $scope.VMAP_PersonsAccompanying = promise.editDetails[0].vmaP_PersonsAccompanying;
                $scope.VMAP_MeetingDuration = promise.editDetails[0].vmaP_MeetingDuration;
                $scope.VMAP_AuthorisationBy = promise.editDetails[0].vmaP_AuthorisationBy;
                $scope.FOMST_IHalfLoginTime = moment(promise.editDetails[0].vmaP_RequestFromTime, 'h:mm a').format();
                $scope.FOMST_IIHalfLogoutTime = moment(promise.editDetails[0].vmaP_RequestToTime, 'h:mm a').format();

                $scope.HRME_Id = promise.editDetails[0];
                $scope.HRME_Id.hrmE_Id = promise.editDetails[0].hrmE_Id


                var decimaltimetot = $scope.VMAP_MeetingDuration;
                var hrstot = parseInt(Number(decimaltimetot));
                var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
                $scope.effhrs = hrstot;
                $scope.effmin = mintot;
                if (hrstot.toString().length === 1) {
                    hrstot = '0' + hrstot;
                }
                if (mintot.toString().length === 1) {
                    mintot = '0' + mintot;
                }
                $scope.effhrstemp = hrstot;
                $scope.effmintemp = mintot;

                $scope.materaldocuupload = [{ id: 'Materal1' }];
                if (promise.editfiles != null && promise.editfiles.length > 0) {
                    $scope.materaldocuupload = promise.editfiles;
                    angular.forEach($scope.materaldocuupload, function (ddd) {

                        var img = ddd.cfilepath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        ddd.filetype = lastelement;
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                        }
                    })
                }
                $scope.morevisitor = [{ id: 'viss1' }];
                if (promise.extravisitor != null && promise.extravisitor.length>0) {
                    $scope.morevisitor = promise.extravisitor;
                }


                // $scope.VMAP_MeetingDateTime = moment(promise.editDetails[0].vmaP_MeetingDateTime, 'HH:mm').format();
                // $scope.Time_Visit = moment(promise.editDetails[0].time_Visit, 'HH:mm').format();
            });
        };


        $scope.deactive = function (newuser1, SweetAlertt) {
            var mgs = "";
            if (newuser1.vmaP_ActiveFlagvmaP_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("VisitorAppointment/deactivate", newuser1).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + mgs + "d Successfully!!!");
                            }
                            else {
                                swal("Record Not " + mgs + "d Successfully!!!");

                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Print = function () {
            var innerContents = '';
            innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/OutwardPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };
        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("VisitorAppointment/viewuploadflies", obj).then(function (promise) {
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