(function () {
    'use strict';
    angular.module('app').controller('VisitorsAuthorisedByController', VisitorsAuthorisedByController)

    VisitorsAuthorisedByController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', '$q', '$sce']
    function VisitorsAuthorisedByController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, $q, $sce) {

        $scope.VMAP_Id = 0;
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.AMVM_Id = 0;
        $scope.SMS_Required = true;
        $scope.SMS_Required_Update = true;
        $scope.Email_Required = true;
        $scope.Email_Required_Update = true;
        $scope.VMMV_ExternalFlg = false;
        $scope.submitted = false;
        $scope.obsupdate = {};
        $scope.obss = {};
        $scope.obj1 = {};
        $scope.obj = {};
        $scope.teacherdocuupload = {};

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.loadgrid = function () {
            apiService.getURI("AddVisitors/getDetails/", 1).then(function (promise) {

                if (promise.count > 0) {
                    $scope.gridoptions = promise.gridoptions;
                    $scope.presentCountgrid = promise.gridoptions.length;
                }
                $scope.emplist = promise.emplist;
                $scope.emplistautjorizedby = promise.emplistautjorizedby;
                // $scope.cancel();
            });
        }

        /*ADD OR REMOVE MULTI VISITORS  */
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
        $scope.obj.VMMV_PersonToMeet = "";
        // $scope.emp1.hrmE_Id = 0;
        $scope.saveData = function () {
            if ($scope.myForm.$valid) {

                $scope.uploaddocments = [];

                angular.forEach($scope.teacherdocuupload, function (dd) {
                    if (dd.VMMVFL_FilePath !== undefined && dd.VMMVFL_FilePath !== null && dd.VMMVFL_FilePath !== "") {
                        $scope.uploaddocments.push(dd);
                    }
                });
                var VMMV_ToMeet = 0;
                if ($scope.VMMV_ExternalFlg == false) {
                    VMMV_ToMeet = $scope.obj1.emp1.hrmE_Id;
                }
                else {
                    VMMV_ToMeet = 0;
                }

                var obj = {
                    "VMMV_Id": $scope.VMMV_Id,
                    "VMMV_VisitorName": $scope.VMMV_VisitorName,
                    "VMMV_VisitorContactNo": $scope.VMMV_VisitorContactNo,
                    "VMMV_VisitorEmailid": $scope.VMMV_VisitorEmailid,
                    "VMMV_IdentityCardType": $scope.VMMV_IdentityCardType,
                    "VMMV_CardNo": $scope.VMMV_CardNo,
                    "VMMV_VisitTypeFlg": $scope.VMMV_VisitTypeFlg,
                    "VMMV_FromPlace": $scope.VMMV_FromPlace,
                    "VMMV_MeetingDuration": $scope.VMMV_MeetingDuration,
                    "VMMV_MeetingDateTime": new Date($scope.VMMV_MeetingDateTime).toDateString(),
                    "VMMV_EntryDateTime": $filter('date')($scope.VMMV_EntryDateTime, "HH:mm"),
                    "VMMV_MeetingLocation": $scope.VMMV_MeetingLocation,
                    "VMMV_MeetingPurpose": $scope.VMMV_MeetingPurpose,
                    "VMMV_VisitorPhoto": $scope.vmmV_VisitorPhoto,
                    "VMMV_PersonToMeet": $scope.obj.VMMV_PersonToMeet,

                    //"VMMV_AuthorisationBy": $scope.VMMV_AuthorisationBy,
                    "VMMV_PersonsAccompanying": $scope.VMMV_PersonsAccompanying,
                    "VMMV_FromAddress": $scope.VMMV_FromAddress,
                    "VMMV_Remarks": $scope.VMMV_Remarks,
                    "VMMV_ToMeet": VMMV_ToMeet,
                    // "VMMV_AuthorisationBy": $scope.obj1.ToAuthHRMEId.hrmE_Id,
                    "SMS_Required": $scope.SMS_Required,
                    "Email_Required": $scope.Email_Required,
                    "VMMV_VehicleNo": $scope.VMMV_VehicleNo,
                    "VMMV_IDCardNo": $scope.VMMV_IDCardNo,
                    "VMMV_DocumentUpload": $scope.VMMV_DocumentUpload,
                    "VMMV_ExternalFlg": $scope.VMMV_ExternalFlg,
                    "VMAP_Id": $scope.VMAP_Id,
                    multivisitor: $scope.morevisitor,
                    uploaddocments: $scope.uploaddocments
                }
                apiService.create("AddVisitors/saveData", obj).then(function (promise) {
                    if (promise.returnVal == 'saved') {
                        if ($scope.SMS_Required === true && $scope.Email_Required === true) {
                            swal("Record Saved Successfully , SMS And Email Sent Successfully");
                            $state.reload();
                        }
                        else if ($scope.SMS_Required === true && $scope.Email_Required === false) {
                            swal("Record Saved Successfully , SMS Sent Successfully");
                            $state.reload();
                        }

                        else if ($scope.SMS_Required === false && $scope.Email_Required === true) {
                            swal("Record Saved Successfully , Email Sent Successfully");
                            $state.reload();
                        } else {
                            swal("Record Saved Successfully");
                            $state.reload();
                        }

                        $scope.vis_list = promise.vis_list;
                        $scope.sttud = true;
                        $scope.screport = true;
                        $scope.SchoolLogo = promise.institution[0].mI_Logo;
                        $scope.SchollName = promise.institution[0].mI_Name;
                        $scope.SchollAdd = promise.institution[0].mI_Address1;
                        $scope.vmmV_Id = promise.vis_list[0].vmmV_Id;
                        $scope.vmmV_VisitorPhoto = promise.vis_list[0].vmmV_VisitorPhoto;
                        $scope.vmmV_VisitorName = promise.vis_list[0].vmmV_VisitorName;
                        $scope.vmmV_FromPlace = promise.vis_list[0].vmmV_FromPlace;
                        $scope.vmmV_MeetingDateTime = new Date(promise.vis_list[0].vmmV_MeetingDateTime);
                        $scope.vmmV_EntryDateTime = promise.vis_list[0].vmmV_EntryDateTime;
                        $scope.empname = promise.vis_list[0].empname;
                        $scope.vmmV_MeetingPurpose = promise.vis_list[0].vmmV_MeetingPurpose;
                        $scope.vmmV_FromAddress = promise.vis_list[0].vmmV_FromAddress;
                        $scope.vmmV_MeetingLocation = promise.vis_list[0].vmmV_MeetingLocation;
                    }
                    else if (promise.returnVal == 'updated') {
                        swal("Record Updated Successfully");
                        $scope.loadgrid();
                    }
                    else if (promise.returnVal == 'duplicate') {
                        swal("Record already exist");
                    }
                    else if (promise.returnVal == "savingFailed") {
                        swal("Failed to save record");
                    }
                    else if (promise.returnVal == "updateFailed") {
                        swal("Failed to update record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.GetMultiVisitorDetails = function (objs) {
            $scope.visitorsname = "";
            $scope.VisitorAdress = "";
            $scope.EmailId = "";
            $scope.ContactNum = "";
            $scope.submitted1 = false;
            var data = {
                "VMMV_Id": objs.vmmV_Id
            };
            apiService.create("AddVisitors/GetMultiVisitorDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getmultivisitordetails = promise.getmultivisitorlist;

                    angular.forEach($scope.getmultivisitordetails, function (dd) {

                        if (dd.vmmvvI_DocumentUpload !== null && dd.vmmvvI_DocumentUpload !== "") {

                            var img = dd.vmmvvI_DocumentUpload;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                data.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.vmmvvI_DocumentUpload;
                            }

                        }
                    });
                }
            });
        };

        $scope.GetVisitorDetails = function (objs) {
            $scope.errormessgedate = "";
            $scope.errormessgedatetime = "";
            $scope.obsupdate.VMMV_ExitDateTimes = "";
            $scope.VMMV_ExitDate = null;
            $scope.MeetingDateTime = null;
            $scope.visitorname = "";
            $scope.sempname = "";
            $scope.VMMV_Id_New = "";
            $scope.maxDate1 = null;
            $scope.minDate1 = null;
            $scope.submitted1 = false;

            var data = {
                "VMMV_Id": objs.vmmV_Id
            };
            apiService.create("AddVisitors/GetVisitorDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.viewdetails = promise.viewdetails;
                    $scope.visitorname = $scope.viewdetails[0].vmmV_VisitorName;
                    $scope.MeetingDateTime = new Date($scope.viewdetails[0].vmmV_MeetingDateTime);
                    $scope.VMMV_Remarkss = $scope.viewdetails[0].vmmV_Remarks;
                    $scope.EntryDateTime = $scope.viewdetails[0].vmmV_EntryDateTime;
                    $scope.sempname = $scope.viewdetails[0].empname;
                    $scope.VMMV_Id_New = $scope.viewdetails[0].vmmV_Id;
                    $scope.obsupdate.VMMV_ExitDateTimes = moment($scope.viewdetails[0].vmmV_ExitDateTime, 'HH:mm').format();

                    if ($scope.viewdetails[0].vmmV_ExitDate !== null) {
                        $scope.VMMV_ExitDate = new Date($scope.viewdetails[0].vmmV_ExitDate);
                    }

                    $scope.maxDate1 = new Date();
                    $scope.minDate1 = new Date($scope.MeetingDateTime);
                }
            });
        };

        $scope.UpdateStatus = function (objs, myForm1) {
            $scope.errormessgedate = "";
            $scope.errormessgedatetime = "";
            var count = 0;

            if (objs.VMMV_ExitDateTimes === undefined || objs.VMMV_ExitDateTimes === null || objs.VMMV_ExitDateTimes === ""
                || objs.VMMV_ExitDateTimes === "Invalid date") {
                $scope.errormessgedatetime = "Select Exit Time";
                count += 1;
            }

            if ($scope.VMMV_ExitDate === undefined || $scope.VMMV_ExitDate === null || $scope.VMMV_ExitDate === ""
                || $scope.VMMV_ExitDate === "Invalid date") {
                $scope.errormessgedate = "Select Exit Date";
                count += 1;
            }
            if (count === 0) {

                var data = {
                    "VMMV_Id": $scope.VMMV_Id_New,
                    "VMMV_ExitDateTime": $filter('date')(objs.VMMV_ExitDateTimes, "HH:mm"),
                    "VMMV_Remarks": $scope.VMMV_Remarkss,
                    "SMS_Required_Update": $scope.SMS_Required_Update,
                    "Email_Required_Update": $scope.Email_Required_Update,
                    "VMMV_ExitDate": new Date($scope.VMMV_ExitDate).toDateString()
                };
                apiService.create("AddVisitors/UpdateStatus", data).then(function (promise) {
                    if (promise.returnVal === 'updated') {
                        swal("Status Updated Successfully");
                        $('#mymodalviewdetailsfirsttab').modal('hide');
                        $scope.loadgrid();
                    }
                });
            }
        };

        $scope.get_empdetails = function (da) {
            $scope.ToMeet = da.hrmE_EmployeeFirstName;
        }

        $scope.edit = function (data) {

            //$scope.AMVM_Id = data;
            var obj = {
                "VMMV_Id": data.vmmV_Id,
            }
            apiService.create("AddVisitors/edit/", obj).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.AMVM_Name = promise.editDetails[0].amvM_Name;
                $scope.AMVM_Contact_No = promise.editDetails[0].amvM_Contact_No;
                $scope.AMVM_Emailid = promise.editDetails[0].amvM_Emailid;
                $scope.AMVM_Card_No = promise.editDetails[0].amvM_Card_No;
                $scope.AMVM_Identity_Type = promise.editDetails[0].amvM_Identity_Type;
                $scope.AMVM_Address = promise.editDetails[0].amvM_Address;
                $scope.AMVM_To_Meet = promise.editDetails[0].amvM_To_Meet;
                $scope.AMVM_Purpose = promise.editDetails[0].amvM_Purpose;
                $scope.Date_Visit = new Date(promise.editDetails[0].date_Visit);
                $scope.Time_Visit = moment(promise.editDetails[0].time_Visit, 'HH:mm').format();
                $scope.AMVM_Remarks = promise.editDetails[0].amvM_Remarks;
                $scope.vmmV_VisitorPhoto = promise.editDetails[0].vmmV_VisitorPhoto;


                $('#blah').attr('src', $scope.editlis[0].vmmV_VisitorPhoto);
            });
        }

        // Search Previous Visitor Details

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };


        $scope.SearchPreviousVisitor = function () {
            var pageid = 2;
            $scope.getpreviousvisitordetails = [];

            $scope.obss.VMMV_Id_New = "";
            $scope.previousvisitorname = "";
            $scope.previousvisitoremailid = "";
            $scope.previousvisitorcontactno = "";
            $scope.previousvisitorfromplace = "";
            $scope.previousvisitorphotot = "";

            apiService.getURI("AddVisitors/SearchPreviousVisitor", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getpreviousvisitorlist = promise.getpreviousvisitorlist;
                    if ($scope.getpreviousvisitorlist !== null && $scope.getpreviousvisitorlist.length > 0) {
                        $('#modalpreviousvisitor').modal('show');
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.OnchangePreviousVisitorname = function (VMMV_Id_New) {
            angular.forEach($scope.getpreviousvisitorlist, function (dd) {
                if (dd.vmmV_Id === Number(VMMV_Id_New.vmmV_Id)) {
                    $scope.previousvisitorname = dd.vmmV_VisitorName;
                    $scope.previousvisitoremailid = dd.vmmV_VisitorEmailid;
                    $scope.previousvisitorcontactno = dd.vmmV_VisitorContactNo;
                    $scope.previousvisitorfromplace = dd.vmmV_FromPlace;
                    $scope.previousvisitorphotot = dd.vmmV_VisitorPhoto;
                }
            });
        };

        $scope.AddPreviousVisitorDetails = function (VMMV_Id_New) {
            if ($scope.myForm2.$valid) {
                var data = {
                    "VMMV_Id": $scope.obss.VMMV_Id_New.vmmV_Id
                };

                apiService.create("AddVisitors/AddPreviousVisitorDetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getpreviousvisitordetails !== null && promise.getpreviousvisitordetails.length > 0) {
                            $('#modalpreviousvisitor').modal('hide');
                            $scope.getpreviousvisitordetails = promise.getpreviousvisitordetails;
                            $scope.getpreviousvisitor_multivisitors = promise.getpreviousvisitor_multivisitors;
                            $scope.getpreviousvisitor_documents = promise.getpreviousvisitor_documents;


                            $scope.VMMV_VisitorName = $scope.getpreviousvisitordetails[0].vmmV_VisitorName;
                            $scope.VMMV_VisitorEmailid = $scope.getpreviousvisitordetails[0].vmmV_VisitorEmailid;
                            $scope.VMMV_FromPlace = $scope.getpreviousvisitordetails[0].vmmV_FromPlace;
                            $scope.VMMV_VisitorContactNo = $scope.getpreviousvisitordetails[0].vmmV_VisitorContactNo;
                            $scope.VMMV_FromAddress = $scope.getpreviousvisitordetails[0].vmmV_FromAddress;
                            $scope.vmmV_VisitorPhoto = $scope.getpreviousvisitordetails[0].vmmV_VisitorPhoto;
                            if ($scope.vmmV_VisitorPhoto !== null && $scope.vmmV_VisitorPhoto !== "") {
                                $('#blah').attr('src', $scope.vmmV_VisitorPhoto);
                            }


                            $scope.morevisitor = [{ id: 'viss1' }];

                            //if ($scope.getpreviousvisitor_multivisitors !== null && $scope.getpreviousvisitor_multivisitors.length > 0) {

                            //    angular.forEach($scope.getpreviousvisitor_multivisitors, function (dd) {

                            //        var quesfiletyped = "";
                            //        var quesdocument_Pathnewd = "";
                            //        if (dd.vmmvvI_DocumentUpload !== null && dd.vmmvvI_DocumentUpload !== null) {
                            //            var img = dd.vmmvvI_DocumentUpload;
                            //            var imagarr = img.split('.');
                            //            var lastelement = imagarr[imagarr.length - 1];
                            //            quesfiletyped = lastelement;                                         
                            //            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            //                quesdocument_Pathnewd = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.vmmvvI_DocumentUpload;
                            //            }
                            //        }

                            //        $scope.morevisitor.push({
                            //            vmmvvI_VisitorName: dd.vmmvvI_VisitorName,
                            //            vmmvvI_VisitorContactNo: dd.vmmvvI_VisitorContactNo, vmmvvI_VisitorEmailId: dd.vmmvvI_VisitorEmailId,
                            //            vmmvvI_VisitorAddress: dd.vmmvvI_VisitorAddress,
                            //            vmmvvI_VisitorPhoto: dd.vmmvvI_VisitorPhoto,
                            //            vmmvvI_DocumentUpload: dd.vmmvvI_DocumentUpload, quesfiletype: quesfiletyped,
                            //            quesdocument_Pathnew: quesdocument_Pathnewd
                            //        });
                            //    });
                            //}
                            $scope.teacherdocuupload = [];
                            $scope.teacherdocuupload = [{ id: 'Teacher1' }];

                            //if ($scope.getpreviousvisitor_documents !== null && $scope.getpreviousvisitor_documents.length > 0) {
                            //    angular.forEach($scope.getpreviousvisitor_documents, function (dd) {

                            //        if (dd.vmmvfL_FilePath !== null && dd.vmmvfL_FilePath !== "") {

                            //            var img = dd.vmmvfL_FilePath;
                            //            var imagarr = img.split('.');
                            //            var lastelement = imagarr[imagarr.length - 1];
                            //            var filetyped = lastelement;
                            //            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            //                var document_Pathnewd = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.vmmvfL_FilePath;
                            //            }
                            //            $scope.teacherdocuupload.push({
                            //                VMMVFL_FilePath: dd.vmmvfL_FilePath, VMMVFL_FileRemarks: dd.vmmvfL_FileRemarks,
                            //                filetype: filetyped, document_Pathnewd: document_Pathnewd, VMMVFL_FileName: dd.vmmvfL_FileName
                            //            });
                            //        }
                            //    });
                            //}
                        }
                    }
                });
            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.GetVisitorAssginDetails = function (objs) {
            var data = {
                "VMMV_Id": objs.vmmV_Id,
                "MI_Id": $scope.mI_Id
            };
            apiService.create("AddVisitors/GetVisitorAssginDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.visitorassigndetails = promise.visitorassigndetails;
                }
            });
        };

        // Appoitment Visitors
        $scope.SearchAppVisitors = function () {
            $scope.appvisitorname = "";
            $scope.appvisitoremailid = "";
            $scope.appvisitortomeet = "";
            $scope.appvisitormeetingdatetime = "";

            $scope.appvisitorcontactno = "";
            $scope.appvisitorfromplace = "";
            $scope.appvisitormettingpurpose = "";
            $scope.appvisitorremarks = "";
            $scope.getappvistiordetails = [];
            $scope.visitorList = [];
            $scope.VMAP_Id = 0;
            $scope.VMAPNew_Id = "";
            $scope.appvisitorstatus = "";
            var data = 2;
            apiService.getURI("AddVisitors/SearchAppVisitors", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getappvistiordetails = promise.getappvistiordetails;
                    if ($scope.getappvistiordetails === null || $scope.getappvistiordetails.length === 0) {
                        swal("No Visitors Found");
                    } else {
                        $('#myModalreadmit').modal('show');
                    }
                }
            });
        };

        $scope.OnchangeVisitorname = function () {
            angular.forEach($scope.getappvistiordetails, function (dd) {
                if (dd.vmaP_Id === parseInt($scope.VMAPNew_Id.vmaP_Id)) {
                    $scope.appvisitorname = dd.vmaP_VisitorName;
                    $scope.appvisitoremailid = dd.vmaP_VisitorEmailid;
                    $scope.appvisitortomeet = dd.vmaP_ToMeet;
                    $scope.appvisitormeetingdatetime = new Date(dd.vmaP_MeetingDateTime);

                    $scope.appvisitorcontactno = dd.vmaP_VisitorContactNo;
                    $scope.appvisitorfromplace = dd.vmaP_FromPlace;
                    $scope.appvisitormettingpurpose = dd.vmaP_MeetingPurpose;
                    $scope.appvisitorremarks = dd.vmaP_Remarks;
                    $scope.appvisitorstatus = dd.vmaP_Status;
                    $scope.VMAP_Id = dd.vmaP_Id;
                }
            });
        };

        $scope.addtocart = function () {
            $scope.getAppointmentdetails = [];
            $scope.getAppointment_visitordetails = [];
            $scope.getAppointment_filesdetails = [];

            if ($scope.myForm2.$valid) {
                $scope.VMMV_Id = 0;

                var data = {
                    "VMAP_Id": $scope.VMAPNew_Id.vmaP_Id
                };

                apiService.create("AddVisitors/GetAppointmentVisitorDetails", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.getAppointmentdetails = promise.getAppointmentdetails;
                        $scope.getAppointment_visitordetails = promise.getAppointment_visitordetails;
                        $scope.getAppointment_filesdetails = promise.getAppointment_filesdetails;

                        angular.forEach($scope.emplist, function (dd) {
                            if (dd.hrmE_Id === $scope.getAppointmentdetails[0].vmaP_HRME_Id) {
                                $scope.getAppointmentdetails[0].hrmE_EmployeeFirstName = dd.hrmE_EmployeeFirstName;
                                $scope.getAppointmentdetails[0].hrmE_Id = dd.hrmE_Id;
                            }
                        });


                        $scope.morevisitor = [];

                        if ($scope.getAppointment_visitordetails !== null && $scope.getAppointment_visitordetails.length > 0) {
                            angular.forEach($scope.getAppointment_visitordetails, function (dd) {
                                $scope.morevisitor.push({
                                    vmmvvI_VisitorName: dd.vmapvI_VisitorName, vmmvvI_VisitorContactNo: dd.vmapvI_VisitorContactNo,
                                    vmmvvI_VisitorEmailId: dd.vmapvI_VisitorEmailId, vmmvvI_VisitorCardNo: '',
                                    vmmvvI_VisitorAddress: dd.vmapvI_VisitorAddress, vmmvvI_Remarks: dd.vmapvI_Remarks,
                                    VMMVVI_IDCardNo: '',
                                });
                            });
                        } else {
                            $scope.morevisitor = [{ id: 'viss1' }];
                        }

                        $scope.VMMV_VisitorName = $scope.getAppointmentdetails[0].vmaP_VisitorName;
                        $scope.VMMV_VisitorContactNo = $scope.getAppointmentdetails[0].vmaP_VisitorContactNo;
                        $scope.VMMV_VisitorEmailid = $scope.getAppointmentdetails[0].vmaP_VisitorEmailid;
                        $scope.VMMV_VisitTypeFlg = $scope.getAppointmentdetails[0].vmaP_VisitTypeFlg;
                        $scope.VMMV_MeetingLocation = $scope.getAppointmentdetails[0].vmaP_MeetingLocation;
                        //$scope.VMMV_MeetingDuration = $scope.getAppointmentdetails[0].vmaP_MeetingDuration;
                        $scope.VMMV_FromPlace = $scope.getAppointmentdetails[0].vmaP_FromPlace;

                        if ($scope.getAppointmentdetails[0].vmaP_MeetingDateTime !== null) {
                            $scope.VMMV_MeetingDateTime = new Date($scope.getAppointmentdetails[0].vmaP_MeetingDateTime);
                            $scope.VMMV_EntryDateTime = moment($scope.getAppointmentdetails[0].vmaP_MeetingTiming, 'HH:mm').format();
                        }
                        $scope.VMMV_MeetingPurpose = $scope.getAppointmentdetails[0].vmaP_MeetingPurpose;
                        $scope.VMMV_FromAddress = $scope.getAppointmentdetails[0].vmaP_FromAddress;
                        $scope.VMMV_PersonsAccompanying = $scope.getAppointmentdetails[0].vmaP_PersonsAccompanying;
                        $scope.VMMV_Remarks = $scope.getAppointmentdetails[0].vmaP_Remarks;
                        $scope.VMAP_Id = $scope.getAppointmentdetails[0].vmaP_Id;
                        $scope.emp1 = $scope.getAppointmentdetails[0];

                        // Files
                        $scope.teacherdocuupload = [];


                        if ($scope.getAppointment_filesdetails !== null && $scope.getAppointment_filesdetails.length > 0) {
                            angular.forEach($scope.getAppointment_filesdetails, function (dd) {
                                if (dd.vmapvF_FilePath !== null && dd.vmapvF_FilePath !== "") {
                                    var img = dd.vmapvF_FilePath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    var filetyped = lastelement;
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        var document_Pathnewd = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.vmapvF_FilePath;
                                    }
                                    $scope.teacherdocuupload.push({
                                        VMMVFL_FilePath: dd.vmapvF_FilePath, VMMVFL_FileRemarks: dd.vmapvF_Filedesc,
                                        filetype: filetyped, document_Pathnewd: document_Pathnewd, VMMVFL_FileName: dd.vmapvF_FileName
                                    });
                                }
                            });
                        } else {
                            $scope.teacherdocuupload = [{ id: 'Teacher1' }];
                        }
                        $('#myModalreadmit').modal('hide');
                    }
                });
            } else {
                $scope.submitted2 = true;
            }
        };


        $scope.removeall = function () {
            $scope.appvisitorname = "";
            $scope.appvisitoremailid = "";
            $scope.appvisitortomeet = "";
            $scope.appvisitormeetingdatetime = "";

            $scope.appvisitorcontactno = "";
            $scope.appvisitorfromplace = "";
            $scope.appvisitormettingpurpose = "";
            $scope.appvisitorremarks = "";
            $scope.getappvistiordetails = [];
            $scope.VMAP_Id = 0;
            $scope.appvisitorstatus = "";
            $('#myModalreadmit').modal('hide');
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
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

        // ACTIVE DEACTIVE
        $scope.deactive = function (newuser1, SweetAlertt) {
            var mgs = "";
            if (newuser1.amvM_ActiveFlag == false) {
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
                        apiService.create("AddVisitors/deactivate", newuser1).then(function (promise) {
                            if (promise.value == true) {
                                if (promise.msg != null) {
                                    swal(promise.msg);
                                    $scope.loadgrid();
                                }
                            }
                            else {
                                swal('Failed to Activate/Deactivate the Record');
                            }
                        })
                    } else {
                        swal("Cancelled");
                    }
                });
        }

        // BLOCK OR UNBLOCK VISITOR DETAILS
        $scope.BlockOrUblockVisitor = function (dd) {
            var mgs = "";
            if (dd.vmmV_BlocekFlg == false) {
                mgs = "Block";
            } else {
                mgs = "Un Block";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " this visitor?",
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
                        apiService.create("AddVisitors/BlockOrUblockVisitor", dd).then(function (promise) {
                            if (promise.returnVal === "updated") {
                                swal("Record Updated Successfully");
                            }
                            else {
                                swal('Failed to update the record');
                            }
                            $state.reload();
                        });
                    } else {
                        swal("Cancelled");
                    }
                });
        };

        // View Visitors Multi Documents 
        $scope.GetVisitorMultiDocuments = function (dd) {

            var data = {
                "VMMV_Id": dd.vmmV_Id
            };

            apiService.create("AddVisitors/GetVisitorMultiDocuments", data).then(function (promise) {
                if (promise !== null) {
                    $scope.viewdocumentdetails = promise.viewdocumentdetails;
                    if ($scope.viewdocumentdetails !== null && $scope.viewdocumentdetails.length > 0) {
                        angular.forEach($scope.viewdocumentdetails, function (dd) {
                            if (dd.vmmvfL_FilePath !== null && dd.vmmvfL_FilePath !== "") {
                                var img = dd.vmmvfL_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.vmmvfL_FilePath;
                                }
                            }
                        });
                    }
                }
            });
        };

        //View Visitor Id Card Details
        $scope.GetVisitorIdCardDetails = function (dd) {

            $scope.VMMV_IDCardReturnedFlg = false;
            $scope.visitoridcardno = "";
            $scope.VMMV_Id_New = undefined;
            $scope.returndatetime = "";
            $scope.VMMV_IDCardReturnedDateTime = null;
            $scope.editidcardDetails = [];
            $scope.editmutlivisitoridcardDetails = [];

            var data = {
                "VMMV_Id": dd.vmmV_Id
            };

            apiService.create("AddVisitors/GetVisitorIdCardDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.editidcardDetails = promise.editidcardDetails;

                    $scope.editmutlivisitoridcardDetails = promise.editmutlivisitoridcardDetails;

                    $scope.visitorname = $scope.editidcardDetails[0].vmmV_VisitorName;
                    $scope.visitoridcardno = $scope.editidcardDetails[0].vmmV_IDCardNo;
                    $scope.VMMV_IDCardReturnedFlg = $scope.editidcardDetails[0].vmmV_IDCardReturnedFlg;
                    $scope.VMMV_Id_New = $scope.editidcardDetails[0].vmmV_Id;

                    if ($scope.editidcardDetails[0].vmmV_IDCardReturnedDateTime !== undefined && $scope.editidcardDetails[0].vmmV_IDCardReturnedDateTime !== null) {

                        $scope.VMMV_IDCardReturnedDateTime = new Date($scope.editidcardDetails[0].vmmV_IDCardReturnedDateTime);
                        $scope.totimehr = $filter('date')($scope.editidcardDetails[0].vmmV_IDCardReturnedDateTime, 'HH');
                        $scope.totimemin = $filter('date')($scope.editidcardDetails[0].vmmV_IDCardReturnedDateTime, 'mm');
                        $scope.totimesec = $filter('date')($scope.editidcardDetails[0].vmmV_IDCardReturnedDateTime, 'ss');

                        var totime = new Date();
                        totime.setHours($scope.totimehr);
                        totime.setMinutes($scope.totimemin);
                        totime.setSeconds($scope.totimesec);
                        $scope.totimebind = totime;
                        $scope.returndatetime = $scope.totimebind;
                    }


                    angular.forEach($scope.editmutlivisitoridcardDetails, function (dd) {

                        if (dd.vmmvvI_IDCardReturnedFlg === null) {
                            dd.vmmvvI_IDCardReturnedFlg = false;
                        }

                        if (dd.vmmvvI_IDCardReturnedDateTime !== undefined && dd.vmmvvI_IDCardReturnedDateTime !== null) {

                            dd.vmmvvI_IDCardReturnedDateTime = new Date(dd.vmmvvI_IDCardReturnedDateTime);
                            $scope.totimehr = $filter('date')(dd.vmmvvI_IDCardReturnedDateTime, 'HH');
                            $scope.totimemin = $filter('date')(dd.vmmvvI_IDCardReturnedDateTime, 'mm');
                            $scope.totimesec = $filter('date')(dd.vmmvvI_IDCardReturnedDateTime, 'ss');

                            var totime = new Date();
                            totime.setHours($scope.totimehr);
                            totime.setMinutes($scope.totimemin);
                            totime.setSeconds($scope.totimesec);
                            $scope.totimebind = totime;
                            dd.visitor_returndatetime = $scope.totimebind;
                        }
                    });
                }
            });
        };

        $scope.UpdateIDCardDetails = function () {

            angular.forEach($scope.editmutlivisitoridcardDetails, function (dd) {
                if (dd.visitor_returndatetime !== undefined && dd.visitor_returndatetime !== null) {
                    $scope.totimehr = $filter('date')(dd.visitor_returndatetime, 'HH');
                    $scope.totimemin = $filter('date')(dd.visitor_returndatetime, 'mm');
                    $scope.totimesec = $filter('date')(dd.visitor_returndatetime, 'ss');
                    dd.totimehr = $scope.totimehr;
                    dd.totimemin = $scope.totimemin;
                    dd.totimesec = $scope.totimesec;
                }
            });


            var data = {
                "VMMV_Id": $scope.VMMV_Id_New,
                "VMMV_IDCardReturnedFlg": $scope.VMMV_IDCardReturnedFlg,
                "VMMV_IDCardReturnedDateTime": $scope.VMMV_IDCardReturnedDateTime,
                "return_hh": $filter('date')($scope.returndatetime, "HH"),
                "return_mm": $filter('date')($scope.returndatetime, "mm"),
                multivisitor: $scope.editmutlivisitoridcardDetails,
            };

            apiService.create("AddVisitors/UpdateIDCardDetails", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnVal === "Update") {
                        swal("Details Updated");
                        $('#viewidcarddetails').modal('hide');
                        $scope.loadgrid();
                    } else {
                        swal("Failed To Update");
                    }
                }
            });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.showGuardianSign = function (data) {
            $('#preview').attr('src', data.vmmvvI_VisitorPhoto);
        };

        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            //$('img').bind('contextmenu', function (e) {
            //    return false;
            //});          
            $('#myModalimg').modal('show');
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

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmtR_Resources;
            $scope.videdfd = data.lpmtR_Resources;
            $scope.movie = { src: data.lpmtR_Resources };
            $scope.movie1 = { src: data.lpmtR_Resources };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmtR_Resources });
            console.log($scope.view_videos);
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd, type) {

            var studentreg = idd;
            $scope.examstart_time = true;
            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.' + type
                    })[0].click();
                });
        };

        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.teacherdocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.teacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.teacherdocuupload);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.teacherdocuupload.length - 1;
            $scope.teacherdocuupload.splice(index, 1);

            if ($scope.teacherdocuupload.length === 0) {
                //data
            }
        };

        //upload Employee Profile pic
        $scope.UploadEmployeeProfilePic = [];
        $scope.uploadEmployeeProfilePic = function (input, document) {
            $scope.UploadEmployeeProfilePic = input.files;
            $('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofile(document);
                }
                else if (input.files[0].type !== "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    angular.element("input[type='file']").val(null);
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    angular.element("input[type='file']").val(null);
                    return;
                }
            }
        };
        function UploadEmployeeprofile(data) {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadEmployeeProfilePic.length; i++) {
                formData.append("File", $scope.UploadEmployeeProfilePic[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/EmployeeDocumentUpload/UploadEmployeeprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.vmmV_VisitorPhoto = d;
                    } else {
                        swal('File Storage Path Not Found ..!!');
                        angular.element("input[type='file']").val(null);
                    }
                })
                .error(function () {
                    $('#blah').removeAttr('src');
                    defer.reject("File Upload Failed!");
                    angular.element("input[type='file']").val(null);
                });
        }


        // Multi Visitor Wise Profile Pic Upload
        $scope.UploadEmployeeProfilePicvi = [];
        $scope.uploadEmployeeProfilePicvis = function (input, document, index) {
            $scope.UploadEmployeeProfilePicvi = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        // $('#blah').attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofilevisit(document);
                }
                else if (input.files[0].type !== "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    angular.element("input[type='file']").val(null);
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    angular.element("input[type='file']").val(null);
                    return;
                }
            }
        };
        function UploadEmployeeprofilevisit(data) {

            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadEmployeeProfilePicvi.length; i++) {
                formData.append("File", $scope.UploadEmployeeProfilePicvi[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/EmployeeDocumentUpload/UploadEmployeeprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        data.vmmvvI_VisitorPhoto = d;
                    } else {
                        swal('File Storage Path Not Found ..!!');
                        angular.element("input[type='file']").val(null);
                    }
                })
                .error(function () {
                    // $('#blah').removeAttr('src');
                    defer.reject("File Upload Failed!");
                    angular.element("input[type='file']").val(null);
                });
        }


        //Multi Visitor Documents
        $scope.UploadMultiVisitorDocument1 = [];
        $scope.UploadMultiVisitorDocument = function (input, document) {

            $scope.UploadMultiVisitorDocument1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].size <= 31457280) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg") {
                        UploadMultiVisitorDoc(document);
                    }
                    else if (input.files[0].type === "application/pdf") {
                        UploadMultiVisitorDoc(document);
                    }
                    else if (input.files[0].type === "application/msword") {
                        UploadMultiVisitorDoc(document);
                    }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                        UploadMultiVisitorDoc(document);
                    }
                    else {
                        swal("Upload  Pdf, Doc, Image Files Only");
                    }
                } else {
                    swal("Upload File Size Should Be Less Than 30MB");
                }
            }
        };
        function UploadMultiVisitorDoc(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.UploadMultiVisitorDocument1.length; i++) {
                formData.append("File", $scope.UploadMultiVisitorDocument1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/visitordocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.vmmvvI_DocumentUpload = d;
                    $('#').attr('src', data.vmmvvI_DocumentUpload);
                    var img = data.vmmvvI_DocumentUpload;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.quesfiletype = lastelement;
                    console.log("data : " + data);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.vmmvvI_DocumentUpload;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }


        // Single Visitor Documents
        $scope.UploadSingleVisitorDocument1 = [];
        $scope.UploadSingleVisitorDocument = function (input, document) {

            $scope.UploadSingleVisitorDocument1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].size <= 31457280) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg") {
                        UploadSingleVisitorDoc(document);
                    }
                    else if (input.files[0].type === "application/pdf") {
                        UploadSingleVisitorDoc(document);
                    }
                    else if (input.files[0].type === "application/msword") {
                        UploadSingleVisitorDoc(document);
                    }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                        UploadSingleVisitorDoc(document);
                    }
                    else {
                        swal("Upload  Pdf, Doc, Image Files Only");
                    }
                } else {
                    swal("Upload File Size Should Be Less Than 30MB");
                }
            }
        };
        function UploadSingleVisitorDoc(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.UploadSingleVisitorDocument1.length; i++) {
                formData.append("File", $scope.UploadSingleVisitorDocument1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/visitordocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.VMMV_DocumentUpload = d;
                    $('#').attr('src', $scope.VMMV_DocumentUpload);
                    var img = $scope.VMMV_DocumentUpload;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.quesfiletype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.VMMV_DocumentUpload;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }


        //Over all Visitors Documents Upload
        $scope.uploadvisitordocuments1 = [];
        $scope.uploadvisitordocuments = function (input, document) {

            $scope.uploadvisitordocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                //$scope.size = input.files[0].size;
                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
                {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploadVisitorDocPhoto(document);
                }
                else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploadVisitorDocPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadvisitordocuments1.length; i++) {
                formData.append("File", $scope.uploadvisitordocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/visitordocuments", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.VMMVFL_FilePath = d;
                    data.VMMVFL_FileName = $scope.filename;
                    $('#').attr('src', data.VMMVFL_FilePath);
                    var img = data.VMMVFL_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.VMMVFL_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
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