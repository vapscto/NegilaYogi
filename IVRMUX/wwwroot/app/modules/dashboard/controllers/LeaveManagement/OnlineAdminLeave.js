﻿
(function () {
    'use strict';
    angular
        .module('app')
        .controller('OnlineAdminLeaveController', OnlineAdminLeaveController)
    OnlineAdminLeaveController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function OnlineAdminLeaveController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.albumNameArraycolumn = [];
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.setmindate = new Date();
        $scope.gridOnlineleave = {
            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableCellEdit: false
        };
        $scope.gridOnlineleave = {
            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableCellEdit: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmL_LeaveName', displayName: 'Leave Type', enableCellEdit: false },
                { name: 'hrelS_TotalLeaves', displayName: 'Total', enableCellEdit: false },
                { name: 'hrelS_CBLeaves', displayName: 'Balance', enableCellEdit: false },
                { name: 'hrelaP_FromDate', displayName: 'From Date', enableCellEdit: false, cellTemplate: '<div class="grid-action-cell">' + '<md-datepicker ng-if="row.entity.hrelS_CBLeaves != 0" ng-model="row.entity.fromDate" md-min-date="grid.appScope.fromminDate" md-max-date="grid.appScope.frommaxDate" ng-change="grid.appScope.fromdate(row.entity.fromDate,row)"  md-placeholder="Enter date"></md-datepicker>' + '</div>' },
                { name: 'hrelaP_ToDate', displayName: 'To Date', enableCellEdit: false, cellTemplate: '<div class="grid-action-cell">' + '<md-datepicker ng-if="row.entity.hrelS_CBLeaves != 0"  ng-model="row.entity.toDate" md-min-date="grid.appScope.tominDate" md-max-date="grid.appScope.tomaxDate"  md-placeholder="Enter date" ng-change="grid.appScope.todate(row.entity.toDate,row)"></md-datepicker>' + '</div>' },
                {
                    //name: 'hrelaP_TotalDays', displayName: 'No. of Days', enableCellEdit: false
                    name: 'hrelaP_TotalDays', displayName: 'No. of Days', enableCellEdit: false, cellTemplate: '<div class="grid-action-cell">' + '<input type="text" style="width:40%" ng-model="row.entity.hrelaP_TotalDays"/><input type="checkbox" ng-model="row.entity.cklop" value="lopdatahd" ng-change="grid.appScope.Hlfday(row)"><span class="lbl padding-6">Half day' + '</div>'
                }]
        };
        $scope.gridOnlineleave.multiSelect = false;
        $scope.gridOnlineleave.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };

        $scope.leavedetails = function (hrmE_Id) {
            $scope.leave_name = [];
            $scope.gridOnlineleave.data = [];
            $scope.contact = "";

            var data = hrmE_Id.hrmE_Id;
            $scope.varhrme = data;
            apiService.getURI("OnlineLeaveApplication/getSingleEmpLeavestatus", data).
                then(function (promise) {
                    $scope.leave_name = promise.leave_name;
                    $scope.gridOnlineleave.data = promise.multi_leave_name;
                    $scope.contact = promise.multi_leave_name[0].hrmemnO_MobileNo;
                    angular.forEach($scope.gridOnlineleave.data, function (grdobj) {
                        grdobj.fromDate = new Date();
                        grdobj.toDate = new Date();
                        if (grdobj.hrelS_CBLeaves !== 0) {
                            grdobj.hrelaP_TotalDays = 1;
                        }
                    });

                    $scope.temp_arry_grid = promise.leave_name;
                    if ($scope.count === 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }
                });
        };

        $scope.loadData = function () {
            var id = 2;
            // $scope.all_check();
            apiService.getURI("OnlineLeaveApplication/getemployeeadmin", id).
                then(function (promise) {

                    $scope.online_leave = promise.online_leave;
                    $scope.hrmE_Id = promise.hrmE_Id;
                    $scope.gridOnlineleave.data = promise.leave_name;
                    angular.forEach($scope.gridOnlineleave.data, function (grdobj) {
                        grdobj.fromDate = new Date();
                        grdobj.toDate = new Date();
                        if (grdobj.hrelS_CBLeaves !== 0) {
                            grdobj.hrelaP_TotalDays = 1;
                        }
                    });

                    $scope.temp_arry_grid = promise.leave_name;
                    if ($scope.count === 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }
                });

            apiService.getURI("OnlineLeaveApplication/getonlineLeavestatus", id).
                then(function (promise) {

                    $scope.leave_name = promise.leave_name;
                    $scope.temp_arry_grid = promise.leave_name;
                    if ($scope.count === 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }
                });
            //  $scope.setmaxdate = new Date();
            $scope.fromminDate = new Date(
                $scope.setmindate.getFullYear(),
                $scope.setmindate.getMonth() - 12,
                $scope.setmindate.getDate());
            $scope.fromDate = $scope.setmindate;
            //$scope.frommaxDate = new Date(
            //$scope.setmaxdate.getFullYear(),
            //$scope.setmaxdate.getMonth(),
            //$scope.setmaxdate.getDate());
            $scope.tominDate = new Date(
                $scope.fromDate.getFullYear(),
                $scope.fromDate.getMonth()-12,
                $scope.fromDate.getDate());
            $scope.toDate = $scope.tominDate;
            // $scope.HRELAP_ReportingDate = $scope.tominDate;

            $scope.minDate = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() + 1);
            $scope.maxDate = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() + 1);
            $scope.HRELAP_ReportingDate = $scope.minDate;
            $scope.loadmodulefeedback();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.gridOnlineleave.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };


        $scope.todate = function (secondDate1, row) {
            $scope.ReportingDate = secondDate1;
            var firstDate1 = $filter('date')(row.entity.fromDate, "dd/MM/yy");
            secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
            var date2 = new Date($scope.formatString(secondDate1));
            var date1 = new Date($scope.formatString(firstDate1));
            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
            angular.forEach($scope.gridOnlineleave.data, function (obj) {
                if (obj.hrelS_Id === row.entity.hrelS_Id) {
                    obj.hrelaP_TotalDays = $scope.dayDifference + 1;
                }
            });

            $scope.minDate = new Date(
                $scope.ReportingDate.getFullYear(),
                $scope.ReportingDate.getMonth(),
                $scope.ReportingDate.getDate() + 1);
            $scope.maxDate = new Date(
                $scope.ReportingDate.getFullYear(),
                $scope.ReportingDate.getMonth(),
                $scope.ReportingDate.getDate() + 1);

            $scope.HRELAP_ReportingDate = $scope.minDate;
        };

        $scope.formatString = function (format) {
            var day = parseInt(format.substring(0, 2));
            var month = parseInt(format.substring(3, 5));
            var year = parseInt(format.substring(6, 10));
            var date = new Date(year, month - 1, day);
            return date;
        };
        $scope.fromdate = function (d1, row) {

            $scope.From_Date = d1;

            angular.forEach($scope.gridOnlineleave.data, function (grdobj) {
                // grdobj.fromDate = new Date();

                if (grdobj.hrelS_Id === row.entity.hrelS_Id) {
                    grdobj.toDate = d1;
                }
                if (grdobj.hrelS_CBLeaves !== 0) {
                    grdobj.hrelaP_TotalDays = 1;
                }

            });
            //row.HRELAP_FromDate = d1;
            //row.fromDate = d1;

            //var firstDate1 = $filter('date')($scope.fromDate, "dd/MM/yy");
            //d1 = $filter('date')(d1, "dd/MM/yy");
            //var date2 = new Date($scope.formatString(d1));
            //var date1 = new Date($scope.formatString(firstDate1));
            //var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            //$scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
            //angular.forEach($scope.gridOnlineleave.data, function (obj) {
            //    if (obj.hrelS_Id == row.entity.hrelS_Id) {
            //        obj.hrelaP_TotalDays = $scope.dayDifference + 1;
            //    }
            //})




            $scope.tominDate = new Date(
                $scope.From_Date.getFullYear(),
                $scope.From_Date.getMonth(),
                $scope.From_Date.getDate());
            $scope.toDate = $scope.tominDate;


            $scope.minDate = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() + 1);
            $scope.maxDate = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() + 1);
            $scope.HRELAP_ReportingDate = $scope.minDate;
        };


        //save
        $scope.submitted = false;
        $scope.ApplayLeave = function () {

            if ($scope.myForm.$valid) {

                var rows = $scope.gridApi.selection.getSelectedRows();
                console.log(rows);

                var today = new Date().toDateString();
                var temp_array = [];

                if (rows.length !== 0) {

                    angular.forEach(rows, function (obj) {
                        temp_array.push({
                            HRELAP_FromDate: new Date(obj.fromDate).toDateString(),
                            HRELAP_ToDate: new Date(obj.toDate).toDateString(),
                            HRELAP_TotalDays: obj.hrelaP_TotalDays,
                            //HRMLY_Id: obj.hrelaP_TotalDays,

                        });

                    });

                    //var obj = {};
                    var data = {
                        "HRELAP_ApplicationDate": today,
                        "HRELAP_LeaveReason": $scope.HRELAP_LeaveReason,
                        "HRELAP_ContactNoOnLeave": Number($scope.contact),
                        "HRELAP_ReportingDate": new Date($scope.HRELAP_ReportingDate).toDateString(),
                        "HRELT_SupportingDocument": $scope.hrelT_SupportingDocument,
                        temp_table_data: rows,
                        frmToDates: temp_array,
                        "HRME_Id": $scope.varhrme,
                    };
                }
                else {
                    swal('Invalid Selection !');
                }
                apiService.create("OnlineLeaveApplication/saveadminLeave", data).
                    then(function (promise) {

                        if (promise.returnmsg === 'Nomapping') {
                            swal("Please Create Leave Numbering Using Transaction Numbering Page");
                            return;
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                            $state.reload();
                        }
                        else if (promise.message === "You Crossed Your Leave Limits") {
                            swal('You Crossed Your Leave Limits');
                            $state.reload();
                        }
                        else if (promise.message === "Leave Already applied on these dates") {
                            swal('Leave Already applied on these dates');
                            $state.reload();
                        }

                        else if (promise.returnval === false) {
                            swal('Data Not Saved !');
                            $state.reload();
                        }

                        $scope.gridOnlineleave.data = promise.mapped_eventlist;
                    });
            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        //$scope.interacted = function (field) {
        //    return $scope.submitted || field.$dirty;
        //};

        $scope.Hlfday = function (row) {
            var rowsss = $scope.gridApi.selection.getSelectedRows();
            var tempdays = row.entity.hrelaP_TotalDays;
            if (rowsss.length > 0) {
                if (row.entity.cklop == true) {
                    row.entity.hrelaP_TotalDays = tempdays - 0.5;
                }
                else if (row.entity.cklop == false) {
                    row.entity.hrelaP_TotalDays = tempdays + 0.5;
                }
            }
        };


        $scope.SelectedFileForUploadzdBOSBOE = [];
        $scope.selectFileforUploadzdBOSBOE = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
                if (($scope.extention === "JPEG" || $scope.extention === "jpg" || $scope.extention === "JPG" || $scope.extention === "pdf" || $scope.extention === "PNG" || $scope.extention === "png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result)

                        $('#documentid') //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE();
                }
                else if ($scope.extention == 'doc' || $scope.extention == 'docx' || $scope.extention == 'xls' || $scope.extention == 'xlsx' || $scope.extention == 'ppt' || $scope.extention == 'pptx') {
                    $window.open($scope.imagepreview)
                }
                else if ($scope.extention !== "JPEG" && $scope.extention !== "jpg" && $scope.extention !== "JPG" && $scope.extention !== "pdf" && $scope.extention !== "PNG" && $scope.extention !== "png") {
                    $('#documentid').removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#documentid').removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBOSBOE() {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdBOSBOE.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBOSBOE[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.hrelT_SupportingDocument = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#documentid').removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBOSBOE = function () {
            //$('#preview').removeAttr('src');
            var filename = $scope.hrelT_SupportingDocument.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "PNG" || $scope.extention === "png") {
                $('#preview').attr('src', $scope.hrelT_SupportingDocument);
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                //$window.open($scope.imagepreview)
                $('#preview').attr('src', $scope.hrelT_SupportingDocument);
            }
            else if ($scope.extention === "pdf") {
                var imagedownload = $scope.hrelT_SupportingDocument;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };


        // deactive
        $scope.deactive = function (item) {
            var dystring = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("OnlineLeaveApplication/ActiveDeactiveRecord", item.hrelaP_Id).then(function (promise) {
                            if (promise.retrunMsg == "updated") {
                                swal("Record " + dystring + "d Successfully!!!");
                            }
                            else {
                                swal("Record Not " + dystring + "d Successfully!!!");
                            }
                            $state.reload();
                        })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }



        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myModal').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);

                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }
        };
        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }


        $scope.content1 = "";
        ///=====================show pdf, img
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;


            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        };


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        ///=====================show pdf, img end
        $scope.loadmodulefeedback = function () {


            var data = {
                "Flag": "FrontOffice"                  // here use your module name (Eg : If Module is COE , then add  "Flag": "COE" )  

            };
            apiService.create("FeedbackTransaction/loadfeedbackquestion", data).then(function (promise) {

                if (promise.feedbackquestion !== undefined && promise.feedbackquestion !== null && promise.feedbackquestion.length > 0) {

                    $scope.feedbackquestion = promise.feedbackquestion;
                    $scope.feedbackoption = promise.feedbackoption;

                    $scope.TempGetFeedbackOption = [];

                    angular.forEach($scope.feedbackquestion, function (fqe) {
                        $scope.TempGetFeedbackOption = [];
                        angular.forEach($scope.feedbackoption, function (fop) {
                            if (fqe.fmtY_Id == fop.fmtY_Id && fqe.fmqE_Id == fop.fmqE_Id) {
                                $scope.TempGetFeedbackOption.push(fop)
                            }
                        });

                        fqe.feedbackoptiondata = $scope.TempGetFeedbackOption;

                    })
                    $("#feedback").modal('show');
                }
            });

        };

        $scope.submitted1 = false;

        $scope.Savefeedback = function () {
            if ($scope.myForm1.$valid) {
                //fp.fmoP_Id > 0 && 
                $scope.temp = [];
                angular.forEach($scope.feedbackquestion, function (fop) {
                    angular.forEach(fop.feedbackoptiondata, function (fp) {
                        if (fop.name == fp.fmoP_Id && fop.fmqE_Id == fp.fmqE_Id && fp.fmoP_FeedbackOptions != "") {
                            $scope.temp.push({
                                name: fp.fmoP_Id,
                                FMOP_FeedbackOptions: fp.fmoP_FeedbackOptions,
                                FMOP_OptionsValue: fp.fmoP_OptionsValue,
                                FMTY_Id: fop.fmtY_Id,
                                FMQE_Id: fp.fmqE_Id,
                                FMQE_FeedbackQuestions: fop.fmqE_FeedbackQuestions,
                                FMQE_FeedbackQRemarks: fop.fmqE_FeedbackQRemarks,
                                FMTY_FeedbackTypeName: fop.fmtY_FeedbackTypeName,
                                FSTTR_FeedBack: fop.fsttR_FeedBack

                            })
                        }

                    });
                });



                var data = {
                    "savemodulefeedback": $scope.temp

                };
                apiService.create("FeedbackTransaction/Savefeedback", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        }
                        else {
                            swal("Failed Saved/Updated Record");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };


    }
})();