(function () {
    'use strict';
    angular.module('app').controller('V_AppointmentApprovalStatus', V_AppointmentApprovalStatus)

    V_AppointmentApprovalStatus.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', '$q', '$sce']
    function V_AppointmentApprovalStatus($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, $q,$sce) {
        $scope.showgrd = false;

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.search = "";
        $scope.search1 = "";
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage = 15;
        $scope.itemsPerPage1 = 10;
        $scope.obj = {};

        //$scope.sort = function (key) {
        //    $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
        //    $scope.sortKey = key;
        //    var miid = myFactoryvisitor.get();
        //    if (miid > 0) {
        //        $scope.mI_Id = miid;
        //    } else {
        //        $scope.mI_Id = 0;
        //    }
        //};

        //====== loadadata 
        $scope.loadgrid = function () {
            $scope.VisitorList = [];
            $scope.HRME_Id = "";
            $scope.VMAP_Id = "";
            $scope.entry_date = "";
            $scope.metting_date = "";
            $scope.FOMST_IHalfLoginTime = "";
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.status_flg = "";
            $scope.remarks_data = "";
            var id = 1;
            //if ($scope.mI_Id === undefined || $scope.mI_Id === null || $scope.mI_Id === '') {
            //    $scope.mI_Id = 0;
            //}
            apiService.getURI("V_AppointmentApprovalStatus/getDetails/", id).then(function (promise) {
                //$scope.mI_Id = promise.mI_Id;
                $scope.institutionlist = promise.institutionlist;
                $scope.emplist = promise.emplist;
                $scope.griddata = promise.griddata;
                $scope.visitorlist = promise.visitorlist;

                if (promise.visitorlist.length > 0) {
                    $scope.VisitorList = promise.visitorlist;
                }
            });
        };

        $scope.onselectname = function () {
            var obj = {
                "VMAP_Id": $scope.VMAP_Id,
                //"MI_Id": $scope.mI_Id
            };

            apiService.create("V_AppointmentApprovalStatus/Edit/", obj).then(function (promise) {
                $scope.vmaP_Id = promise.editDetails[0].vmaP_Id;
                $scope.entry_date = new Date(promise.editDetails[0].vmaP_EntryDateTime);
                $scope.remarks_data = promise.editDetails[0].vmaP_Remarks;
            });
        };

        $scope.allvisitor = [];
        $scope.edit = function (obj) {
            $scope.sche = "L";
            $scope.VisitorList = [];
            $scope.HRME_Id = "";
           
            $scope.entry_date = "";
            $scope.metting_date = "";
            $scope.FOMST_IHalfLoginTime = "";
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.status_flg = "";
            $scope.remarks_data = "";
            $scope.allvisitor = [];
            $scope.cname = obj.MI_Name;
            $scope.inddate = obj.VMAP_EntryDateTime;
            $scope.piino = obj.VMAP_ToMeet;
            $scope.placee = obj.VMAP_FromPlace;
            $scope.purposee = obj.VMAP_MeetingPurpose;
            $scope.olc = obj.VMAP_MeetingLocation;
            $scope.VMAP_Id = obj.VMAP_Id;
            //$scope.mI_Id = obj.mI_Id;
            $scope.HRME_Id = obj;
            $scope.frmTime = obj.VMAP_RequestFromTime;
            $scope.toTime = obj.VMAP_RequestToTime;
            $scope.lgdate = obj.LoginDate;
            $scope.lgtime = obj.LoginTime1;


            $scope.metting_date = new Date(obj.VMAP_EntryDateTime);
            $scope.FOMST_IHalfLoginTime = moment(obj.VMAP_MeetingTiming, 'h:mm a').format();
            $scope.FOMST_IIHalfLogoutTime = moment(obj.VMAP_MeetingToTime, 'h:mm a').format();
            apiService.create("V_AppointmentApprovalStatus/Editnew/", obj).then(function (promise) {

                $scope.editDetails = promise.editDetails;
                $scope.editfiles = promise.editfiles;
                $scope.extvisitorlist = promise.visitorlist;

                angular.forEach($scope.editDetails, function (rr) {
                    $scope.allvisitor.push({ NAME: rr.vmaP_VisitorName, MB: rr.vmaP_VisitorContactNo, EM: rr.vmaP_VisitorEmailid, ADD: rr.vmaP_FromAddress})

                })

                angular.forEach($scope.extvisitorlist, function (rr) {
                    $scope.allvisitor.push({ NAME: rr.vmapvI_VisitorName, MB: rr.vmapvI_VisitorContactNo, EM: rr.vmapvI_VisitorEmailId, ADD: rr.vmapvI_VisitorAddress })

                })

                $scope.showgrd = true;
            });
        };
        //////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\
        /* Options Files */
        $scope.uploadtecheroptsdocuments1 = [];
        $scope.uploadtecheroptsdocuments = function (input, document) {

            $scope.uploadtecheroptsdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploaddianPhotoopts(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecheroptsdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecheroptsdocuments1[i]);
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
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.addNewsiblingguardopts = function () {
            var newItemNo = $scope.teacherdocuuploadopts.length + 1;

            if (newItemNo <= 10) {
                $scope.teacherdocuuploadopts.push({ 'id': 'Teacheropts1' + newItemNo });
            }
            console.log($scope.teacherdocuuploadopts);
        };

        $scope.removeNewsiblingguardopts = function (index) {
            var newItemNo = $scope.teacherdocuuploadopts.length - 1;
            $scope.teacherdocuuploadopts.splice(index, 1);

            if ($scope.teacherdocuuploadopts.length === 0) {
                //data
            }
        };

        $scope.APID = 0;
        $scope.MID = 0;
        $scope.uploadMOM = function (obj) {
            $scope.APID = obj.VMAP_Id;
            //$scope.MID = obj.mI_Id;
          

        }

        $scope.OnClickOptsFilesUpload = function () {
            debugger
            //if ($scope.myForm.$valid) {
                var data = {
                    "filelist": $scope.teacherdocuuploadopts,
                    "VMAP_Id": $scope.APID,
                    //"MI_Id": $scope.MID,
                };
                apiService.create("V_AppointmentApprovalStatus/sendMOM", data).then(function (promise) {
                    if (promise.returnVal === 'updated') {
                        swal("MoM Sent Successfully");
                        $state.reload();
                    }
                    else {
                        swal("Error");
                    }
                    $("#optsfilesupload").modal({ backdrop: false });

            });
            //} else {
            //    $scope.submitted3 = true;
            //}
        }

        $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];

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
     
        $scope.sche = "";
        $scope.reschedule = function (obj) {
            $scope.sche = "R";
            $scope.VisitorList = [];
            $scope.HRME_Id = "";
           
            $scope.entry_date = "";
            $scope.metting_date = "";
            $scope.FOMST_IHalfLoginTime = "";
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.status_flg = "";
            $scope.remarks_data = "";
            $scope.allvisitor = [];
            $scope.cname = obj.MI_Name;
            $scope.inddate = obj.VMAP_EntryDateTime;
            $scope.piino = obj.VMAP_ToMeet;
            $scope.placee = obj.VMAP_FromPlace;
            $scope.purposee = obj.VMAP_MeetingPurpose;
            $scope.olc = obj.VMAP_MeetingLocation;
            $scope.VMAP_Id = obj.VMAP_Id;
            //$scope.mI_Id = obj.mI_Id;
            $scope.HRME_Id = obj;
            $scope.frmTime = obj.VMAP_RequestFromTime;
            $scope.toTime = obj.VMAP_RequestToTime;
            $scope.metting_date =new Date(obj.VMAP_MeetingDateTime);
            $scope.FOMST_IHalfLoginTime = moment(obj.VMAP_MeetingTiming, 'h:mm a').format();
            $scope.FOMST_IIHalfLogoutTime = moment(obj.VMAP_MeetingToTime, 'h:mm a').format();
            $scope.remarks_data = obj.VMAP_Remarks;
            apiService.create("V_AppointmentApprovalStatus/Editnew/", obj).then(function (promise) {

                $scope.editDetails = promise.editDetails;
                $scope.editfiles = promise.editfiles;
                $scope.extvisitorlist = promise.visitorlist;

                angular.forEach($scope.editDetails, function (rr) {
                    $scope.allvisitor.push({ NAME: rr.vmaP_VisitorName, MB: rr.vmaP_VisitorContactNo, EM: rr.vmaP_VisitorEmailid, ADD: rr.vmaP_FromAddress})

                })

                angular.forEach($scope.extvisitorlist, function (rr) {
                    $scope.allvisitor.push({ NAME: rr.vmapvI_VisitorName, MB: rr.vmapvI_VisitorContactNo, EM: rr.vmapvI_VisitorEmailId, ADD: rr.vmapvI_VisitorAddress })

                })

                $scope.showgrd = true;
            });
        };

        $scope.savefeedback = function () {
            debugger;
            if ($scope.myForm1.$valid) {
                var data = {
                    "VMAP_Id": $scope.VMAP_Idnew,
                    "VMAP_Feedback": $scope.VMAP_Feedback,
                }
                
                apiService.create("V_AppointmentApprovalStatus/savefeedback/", data).then(function (promise) {

                    if (promise != null) {
                        if (promise.returnVal=='up') {
                            swal('Feedback Updated Successfully')
                        }
                        else {
                            swal('Error');
                        }
                    }
                    else {
                        swal('Error');
                    }

                
                    $('#optsfeedback').modal('hide');
                    $("#optsfeedback").modal({ backdrop: false });
                });
            }
            else {
                $scope.submitted1 = true;
            }

        }



        $scope.uploadfeed = function (obj)
        {
            $scope.VMAP_Feedback = '';
            $scope.VMAP_Idnew = obj.VMAP_Id
            apiService.create("V_AppointmentApprovalStatus/getfeedback/", obj).then(function (promise) {
                if (promise.editDetails != null && promise.editDetails.length>0) {
                    $scope.VMAP_Feedback = promise.editDetails[0].vmaP_Feedback;
                }
              

            });
        }


        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.mindate = new Date();
        $scope.submitted = false;
        $scope.submitted1 = false;
        $scope.saveData = function () {

            if ($scope.myForm.$valid) {
                if ($scope.HRME_Id !== undefined) {

                    $scope.remainder_flg_new = "";
                    $scope.remainderdetails = "";
                    $scope.reminder_flag_new = "";

                    if ($scope.status_flg === "Approved") {
                        if ($scope.obj.remainder_flg === "Day") {
                            $scope.remainderdetails = $scope.reminder_days;
                          $scope.remainder_flg_new = $scope.obj.remainder_flg;
                            $scope.reminder_flag_new = $scope.reminder_flag;
                        }

                        else if ($scope.obj.remainder_flg === "Hour") {
                            $scope.remainderdetails = $scope.reminder_hours;
                            $scope.remainder_flg_new = $scope.obj.remainder_flg;
                            $scope.reminder_flag_new = $scope.reminder_flag;
                        }

                        else if ($scope.obj.remainder_flg === "Mints") {
                            $scope.remainderdetails = $scope.reminder_mins;
                            $scope.remainder_flg_new = $scope.obj.remainder_flg;
                            $scope.reminder_flag_new = $scope.reminder_flag;
                        }
                    }

                    var ScheduleTime = $filter('date')($scope.FOMST_IHalfLoginTime, "HH");
                    var ScheduleTimem = $filter('date')($scope.FOMST_IHalfLoginTime, "mm");

                    var obj = {
                        "VMAP_Id": $scope.VMAP_Id,
                        "VMAP_Status": $scope.status_flg,
                        "VMAP_MeetingTiming": $filter('date')($scope.FOMST_IHalfLoginTime, "HH:mm"),
                        "VMAP_MeetingDateTime": new Date($scope.metting_date).toDateString(),
                        "VMAP_HRME_Id": $scope.HRME_Id.hrmE_Id,
                        "VMAP_Remarks": $scope.remarks_data,
                        //"MI_Id": $scope.mI_Id,
                        "VMAP_ReminderFlag": $scope.remainder_flg_new,
                        "VMAP_ReminderSchedule": $scope.remainderdetails,
                        "VMAP_RepeatFlag": $scope.reminder_flag_new,
                        "fhrors": ScheduleTime,
                        "fminutes": ScheduleTimem,
                        "type": $scope.sche,
                        "VMAP_MeetingToTime": $filter('date')($scope.FOMST_IIHalfLogoutTime, "HH:mm"),
                        "VMAP_RescheduleReason": $scope.VMAP_RescheduleReason,
                    };

                    apiService.create("V_AppointmentApprovalStatus/saveData", obj).then(function (promise) {
                        if (promise.returnVal === 'ex') {
                            swal("Already Appoint is fixed for other Visitor");
                           
                        }
                        else if (promise.returnVal === 'updated') {
                            swal("Record Saved Successfully");
                            $state.reload();
                        }
                        else if (promise.returnVal === 'duplicate') {
                            swal("Record already exist");
                            $state.reload();
                        }
                        else if (promise.returnVal === "savingFailed") {
                            swal("Failed to save record");
                            $state.reload();
                        }
                        else if (promise.returnVal === "updateFailed") {
                            swal("Failed to update record");
                            $state.reload();
                        }
                        else {
                            swal("Sorry...something went wrong");
                            $state.reload();
                        }
                       
                    });
                }
                else {
                    swal('Please select Staff Name!');
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.get_date1 = function (days1, secondDate1) {
            var date123 = secondDate1;
            if (days1 !== "" && days1 !== undefined) {
                days1 = Number(days1);
                var firstDate1 = new Date();

                firstDate1 = $filter('date')(firstDate1, "dd/MM/yy");
                secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
                var date2 = new Date($scope.formatString(secondDate1));
                var date1 = new Date($scope.formatString(firstDate1));

                var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));

                if (days1 <= $scope.dayDifference) {

                    var someDate = date123;
                    var remind_date = new Date(someDate);
                    remind_date.setDate(remind_date.getDate() - days1);
                    $scope.reminder_Date = remind_date;
                }
                else {
                    swal("Reminder Date Can't be Before The Today's Date !!!");
                    $scope.reminder_Date = "";
                    $scope.reminder_days = "";
                }
            }
            else {
                $scope.reminder_Date = "";
            }
        };


        $scope.formatString = function (format) {
            var day = parseInt(format.substring(0, 2));
            var month = parseInt(format.substring(3, 5));
            var year = parseInt(format.substring(6, 10));
            var date = new Date(year, month - 1, day);
            return date;
        };

        $scope.changed = function (timedata) {
            //  $scope.VMMV_MeetingDateTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        };

        $scope.get_end_remind1 = function (secondDate1) {
            if ($scope.asmaY_Id !== "") {
                var firstDate1 = new Date();
                firstDate1 = $filter('date')(firstDate1, "dd/MM/yy");
                secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
                var date2 = new Date($scope.formatString(secondDate1));
                var date1 = new Date($scope.formatString(firstDate1));
                var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
                // alert($scope.dayDifference);

                var someDate = new Date();
                // var numberOfDaysToAdd = 6;
                someDate.setDate(someDate.getDate() + $scope.dayDifference);
                //swal("Difference Days :" + $scope.dayDifference + " and Date:" + someDate.toDateString());
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.coeE_EStartDate = "";
            }
            $scope.coeE_EEndDate = "";
        };

        $scope.onChangeRemainder = function (objs) {
            $scope.flag = objs.remainder_flg;
        };

        $scope.onChangeStatus = function (objs) {
            $scope.obj.remainder_flg = "";
            $scope.flag = "";
        };



        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
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