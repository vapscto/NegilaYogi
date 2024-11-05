
(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveApplicationController', LeaveApplicationController)
    LeaveApplicationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$window']
    function LeaveApplicationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $window) {
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.itemsPerPage = 5;
        $scope.itemsPerPage2 = 10;
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.myDate = new Date();
        $scope.morevisitor = [];
       
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

        $scope.gridOnlineleave = {
            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableCellEdit: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmL_LeaveName', displayName: 'Leave Type', enableCellEdit: false },
                { name: 'hrelS_TotalLeaves', displayName: 'Total', enableCellEdit: false },
                { name: 'hrelS_CBLeaves', displayName: 'Balance', enableCellEdit: false },
                { name: 'hrelaP_FromDate', displayName: 'From Date', enableCellEdit: false, cellTemplate: '<div class="grid-action-cell">' + '<md-datepicker ng-if="row.entity.hrelS_CBLeaves != 0" ng-model="row.entity.fromDate" md-min-date="grid.appScope.fromminDate" md-max-date="grid.appScope.frommaxDate" ng-change="grid.appScope.fromdate(row.entity.fromDate,row)"  md-placeholder="Enter date"></md-datepicker>' + '</div>' },
                { name: 'hrelaP_ToDate', displayName: 'To Date', enableCellEdit: false, cellTemplate: '<div class="grid-action-cell">' + '<md-datepicker ng-if="row.entity.hrelS_CBLeaves != 0"  ng-model="row.entity.toDate" md-min-date="grid.appScope.tominDate" md-max-date="grid.appScope.tomaxDate"  md-placeholder="Enter date" ng-change="grid.appScope.todate(row.entity.toDate,row)"></md-datepicker>' + '</div>' },
                {
                    name: 'hrelaP_TotalDays', displayName: 'No. of Days', enableCellEdit: false, cellTemplate: '<div class="grid-action-cell">' + '<input type="text" style="width:40%" ng-model="row.entity.hrelaP_TotalDays"/><input type="checkbox" ng-model="row.entity.cklop" value="lopdatahd" ng-change="grid.appScope.Hlfday(row)"><span class="lbl padding-6">Half day' + '</div>'
                }]
        };
        $scope.gridOnlineleave.multiSelect = false;
        $scope.gridOnlineleave.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };



        $scope.loadData = function () {

            var id = 2;
            $scope.absentdpcount = [];
            apiService.getURI("OnlineLeaveApplication/getdetails", id).then(function (promise) {
                $scope.online_leave = promise.online_leave;
                $scope.gridOnlineleave.data = promise.leave_name;
                $scope.leave_name = promise.empleavename;
                $scope.empApprovaleave = promise.getemployeeleavedetails;
                $scope.staff_day_periods = promise.time_Table;
                $scope.substitutestafflist = promise.stafflist;
                $scope.appliedgrid = promise.appliedgrid;

                $scope.morevisitor.push({
                    staff_day_periods: $scope.staff_day_periods
                });

                $scope.absentdpcount = promise.absentdpcount;
                angular.forEach(promise.time_Table, function (user) {

                    $scope.cell_click(user.ttmD_Id, user.ttmP_Id, user.ttmD_DayName, user.ttmP_PeriodName, user.hrmE_Id,
                        user.asmaY_Id, user.asmcL_Id, user.asmS_Id, user.ttfgD_Id, user);



                })

                $scope.year_list = promise.acayear;
                $scope.CurrentYear = promise.academicListdefault;

                for (var i = 0; i < $scope.year_list.length; i++) {
                    name = $scope.year_list[i].asmaY_Id;
                    for (var j = 0; j < $scope.CurrentYear.length; j++) {
                        if (parseInt(name) === parseInt($scope.CurrentYear[j].asmaY_Id)) {
                            $scope.year_list[i].Selected = true;
                            $scope.asmaY_Id = $scope.CurrentYear[j].asmaY_Id;
                        }
                    }
                }


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


                apiService.getURI("OnlineLeaveApplication/getonlineLeavestatus", id).
                    then(function (promise) {

                        $scope.compoffodgridvalues = promise.compoffodgridvalues;
                        $scope.leave_name = promise.leave_name;
                        $scope.temp_arry_grid = promise.leave_name;
                        if ($scope.count === 0) {
                            swal("Data not Found !!");
                            $scope.ckdept = false;
                        }
                    });



                $scope.fromminDate = new Date(
                    $scope.setmindate.getFullYear(),
                    $scope.setmindate.getMonth() - 1,
                    $scope.setmindate.getDate());
                $scope.fromDate = $scope.setmindate;


                $scope.tominDate = new Date(
                    $scope.fromDate.getFullYear(),
                    $scope.fromDate.getMonth(),
                    $scope.fromDate.getDate());
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


            });

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
        //$scope.ApplayLeave = function () {

        //    if ($scope.myForm.$valid) {

        //        var rows = $scope.gridApi.selection.getSelectedRows();
        //        console.log(rows);

        //        var today = new Date().toDateString();
        //        var temp_array = [];

        //        if (rows.length !== 0) {
        //            angular.forEach(rows, function (obj) {
        //                temp_array.push({
        //                    HRELAP_FromDate: new Date(obj.fromDate).toDateString(),
        //                    HRELAP_ToDate: new Date(obj.toDate).toDateString(),
        //                    HRELAP_TotalDays: obj.hrelaP_TotalDays,
        //                    //HRMLY_Id: obj.hrelaP_TotalDays,
        //                });
        //            });

        //            var data = {
        //                "HRELAP_ApplicationDate": today,
        //                "HRELAP_LeaveReason": $scope.HRELAP_LeaveReason,
        //                "HRELAP_ContactNoOnLeave": Number($scope.contact),
        //                "HRELAP_ReportingDate": new Date($scope.HRELAP_ReportingDate).toDateString(),
        //                "HRELT_SupportingDocument": $scope.hrelT_SupportingDocument,
        //                temp_table_data: rows,
        //                frmToDates: temp_array
        //            };
        //        }
        //        else {
        //            swal('Invalid Selection !');
        //        }
        //        apiService.create("OnlineLeaveApplication/save", data).
        //            then(function (promise) {

        //                if (promise.returnmsg === 'Nomapping') {
        //                    swal("Please Create Leave Numbering Using Transaction Numbering Page");
        //                    return;
        //                }
        //                else if (promise.returnval === true) {
        //                    swal('Data successfully Saved');
        //                    $state.reload();
        //                }
        //                else if (promise.returnduplicatestatus === 'Duplicate') {
        //                    swal('Records Already Exist !');
        //                    $state.reload();
        //                }
        //                else if (promise.message === "You Crossed Your Leave Limits") {
        //                    swal('You Crossed Your Leave Limits');
        //                    $state.reload();
        //                }
        //                else if (promise.message === "Leave Already applied on these dates") {
        //                    swal('Leave Already applied on these dates');
        //                    $state.reload();
        //                }

        //                else if (promise.returnval === false) {
        //                    swal('Data Not Saved !');
        //                    $state.reload();
        //                }

        //                //$scope.gridOnlineleave.data = promise.mapped_eventlist;
        //            });
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }

        //};

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
       };


        //$scope.interacted = function (field) {
        //    return $scope.submitted || field.$dirty;
        //};


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




        $scope.submitted2 = false;
        $scope.CompOffODRequest = function (HRELAP_TotalDays) {
            if ($scope.myFormextra.$valid) {

                var today = new Date().toDateString();
                var HRELAP_FromDate = new Date($scope.fromDate).toDateString();

                var data = {
                    "HRELAP_ApplicationDate": today,
                    "HRELAP_FromDate": HRELAP_FromDate,
                    "HRELAP_ToDate": HRELAP_FromDate,
                    "HRELAP_TotalDays": 1,
                    "HRELAP_LeaveReason": $scope.HRELAP_LeaveReason,
                    "HRELAP_ContactNoOnLeave": Number(0),
                    "HRELAP_ReportingDate": HRELAP_FromDate,
                    "HRELT_SupportingDocument": $scope.hrelT_SupportingDocument,
                    "HRML_LeaveType": $scope.leavetype,
                    "HRELAPD_InTime": $filter('date')($scope.hrelapD_InTime, "HH:mm"),
                    "HRELAPD_OutTime": $filter('date')($scope.hrelapD_OutTime, "HH:mm")
                };

                apiService.create("OnlineLeaveApplication/requestleave", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Requested successfully..');
                            //$('#myModal').modal('hide');
                            $state.reload();
                        }
                        else if (promise.returnmsg === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnmsg === 'Leave Not Mapped') {
                            swal('Leave Not Mapped In Master Leave');
                        }
                        else if (promise.returnval === false) {
                            swal('Request Failed !! Kindly contact Administration.');
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
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.editEmployee = {};

        $scope.grid_view = false;
        $scope.abs = false;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.mainstafflist = [];
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.isOptionsRequired = function () {

            return !$scope.staff_list.some(function (options) {
                return options.stf;
            });
        }
        $scope.rpttyp = "Name";
        $scope.onclickloaddata = function () {
            $scope.loaddefault = false;
            $scope.loaddefault1 = false;
            $scope.HRME_Id = "";
            $scope.stf_dy_pds = false;
            $scope.stf_fr_pds = false;
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.stf_fr_pds = false;
        var abst_asmcL_Id = "";
        var abst_asmS_Id = "";
        var abst_ttmD_Id = "";
        var abst_ttmP_Id = "";
        //Deputation Based Requirement Start
        //$scope.cell_click = function (dayid, periodid, day, period, stfid, yrid, clsid, secid, TTFGDC_Id, user) {



        //    $scope.loaddefault1 = false;
        //    $scope.dpcount = [];
        //    abst_asmcL_Id = clsid;
        //    abst_asmS_Id = secid;
        //    abst_ttmD_Id = dayid;
        //    abst_ttmP_Id = periodid;
        //    var TTSD_Date = new Date($scope.myDate).toDateString();
        //    var data = {
        //        "ASMAY_Id": yrid,
        //        "TTMD_Id": dayid,
        //        "TTMP_Id": periodid,
        //        "HRME_Id": stfid,
        //        "TTSD_Date": TTSD_Date,
        //    }
        //    apiService.create("OnlineLeaveApplication/get_free_stfdets", data).
        //        then(function (promise) {

        //            if (promise.time_Table_substitute != "" && promise.time_Table_substitute != null) {
        //                $scope.stf_fr_pds = true;
        //                $scope.loaddefault1 = true;
        //            }

        //            $scope.mainstafflist = [];

        //            angular.forEach(promise.time_Table_substitute, function (eps) {
        //                if ($scope.mainstafflist.length == 0) {
        //                    $scope.mainstafflist.push({ HRME_Id: eps.hrmE_Id, STAFF: eps.staffName, EMPCODE: eps.hrmE_EmployeeCode, DCNT: eps.DCNT, WCNT: eps.WCNT, MCNT: eps.MCNT, YCNT: eps.YCNT, asmaY_Id: eps.asmaY_Id, deviceid: eps.deviceid });
        //                }
        //                else if ($scope.mainstafflist.length > 0) {
        //                    var al_exm_cnt = 0;
        //                    angular.forEach($scope.mainstafflist, function (exm) {
        //                        if (exm.HRME_Id == eps.hrmE_Id) {
        //                            al_exm_cnt += 1;
        //                        }
        //                    })
        //                    if (al_exm_cnt == 0) {
        //                        $scope.mainstafflist.push({ HRME_Id: eps.hrmE_Id, STAFF: eps.staffName, EMPCODE: eps.hrmE_EmployeeCode, DCNT: eps.DCNT, WCNT: eps.WCNT, MCNT: eps.MCNT, YCNT: eps.YCNT, asmaY_Id: eps.asmaY_Id, deviceid: eps.deviceid });
        //                    }
        //                }
        //            })



        //            $scope.dpcount = promise.dpcount;
        //            $scope.absentstfcnt = promise.absentstfcnt;
        //            $scope.weeklycntlist = promise.weeklycntlist;
        //            angular.forEach($scope.mainstafflist, function (dd) {
        //                //depute count
        //                angular.forEach($scope.dpcount, function (xx) {
        //                    if (dd.HRME_Id == xx.HRME_Id) {

        //                        dd.weekcnt = xx.WeekStaffcount;
        //                        dd.mntcnt = xx.MonthStaffcount;
        //                        dd.totalcnt = xx.YearStaffcount;
        //                        dd.daycnt = xx.TodayStaffcount;

        //                    }

        //                })

        //                //absent count
        //                angular.forEach($scope.absentstfcnt, function (xx) {
        //                    if (dd.HRME_Id == xx.HRME_Id) {
        //                        dd.weekcnt1 = xx.WeekStaffcount;
        //                        dd.mntcnt1 = xx.MonthStaffcount;
        //                        dd.totalcnt1 = xx.YearStaffcount;
        //                        dd.daycnt1 = xx.TodayStaffcount;

        //                    }

        //                })


        //                angular.forEach($scope.weeklycntlist, function (xx) {
        //                    if (dd.HRME_Id == xx.HRME_Id) {
        //                        dd.WWCNT = xx.TPCOUNT;
        //                    }

        //                })

        //            })



        //            $scope.periodslst = promise.periodslst;
        //            $scope.ttlistdata = promise.time_Table_substitute;

        //            angular.forEach($scope.mainstafflist, function (zz) {

        //                var templist = [];

        //                angular.forEach($scope.periodslst, function (pp) {
        //                    var clsdetails = '';
        //                    var subdetails = '';

        //                    angular.forEach($scope.ttlistdata, function (tt) {
        //                        if (tt.hrmE_Id == zz.HRME_Id && tt.ttmP_Id == pp.ttmP_Id) {

        //                            if (clsdetails == '') {
        //                                clsdetails = tt.asmcL_ClassName + '&' + tt.asmC_SectionName;
        //                            }
        //                            else {
        //                                clsdetails = clsdetails + ' ' + 'AND' + ' ' + tt.asmcL_ClassName + '&' + tt.asmC_SectionName;
        //                            }

        //                            if (subdetails == '') {
        //                                subdetails = tt.ismS_SubjectName;
        //                            }
        //                            else {
        //                                if (subdetails != tt.ismS_SubjectName) {
        //                                    subdetails = subdetails + ' ' + 'AND' + ' ' + tt.ismS_SubjectName;
        //                                }

        //                            }

        //                        }

        //                    })

        //                    if (clsdetails != '' && subdetails != '') {
        //                        templist.push({ TTMP_Id: pp.ttmP_Id, PR: 'P--' + pp.ttmP_PeriodName, CLS: clsdetails, SUB: subdetails });
        //                    }


        //                })

        //                zz.list = templist;
        //            })

        //            console.log('eeeee');
        //            console.log($scope.mainstafflist);

        //            angular.forEach($scope.mainstafflist, function (dd) {
        //                dd.DAYTL = dd.list.length;
        //                var gg = 0;
        //                var newlist = [];
        //                angular.forEach(dd.list, function (xx) {
        //                    gg += 1;
        //                    if (gg == 1) {
        //                        dd.PR = xx.PR;
        //                        dd.CLS = xx.CLS;
        //                        dd.SUB = xx.SUB;
        //                    }
        //                    else {
        //                        newlist.push(xx);
        //                    }

        //                })
        //                if (newlist.length > 0) {
        //                    dd.list = newlist;
        //                }


        //            })



        //            console.log($scope.staff_free_periods);
        //            if (promise.time_Table_substitute == "" || promise.time_Table_substitute == null) {
        //                swal("No Staffs Are Free   For Selected Day !!!");
        //                $scope.stf_fr_pds = false;
        //                $scope.loaddefault1 = false;
        //            }

        //            //kavita

        //            $scope.staff_day_periods.push({
        //                ttmD_Id: user.ttmD_Id,
        //                ttmP_Id: user.ttmP_Id,
        //                ttmD_DayNam: user.ttmD_DayName,
        //                ttmP_PeriodName: user.ttmP_PeriodName,
        //                hrmE_Id: user.hrmE_Id,
        //                asmaY_Id: user.asmaY_Id,
        //                asmcL_Id: user.asmcL_Id,
        //                asmS_Id: user.asmS_Id,
        //                ttfgD_Id: user.ttfgD_Id,
        //                ttmP_PeriodName: user.ttmP_PeriodName,
        //                ismS_SubjectName: user.ismS_SubjectName,
        //                asmC_SectionName: user.asmC_SectionName,
        //                asmcL_ClassName: user.asmcL_ClassName,
        //                substitutestafflist: $scope.mainstafflist
        //            })
        //        })
        //}
        //Deputation Based Requirement END
        $scope.search = "";
        $scope.asmaY_Id = "";
        $scope.myDate = "";
        $scope.ttmD_Id = "";
        $scope.hrmE_Id = "";
        $scope.stf_dy_pds = false;
        $scope.loaddefault = false;
        $scope.loaddefault1 = false;
        $scope.absentdpcount = [];



        $scope.Changestff = function (user) {
            $scope.leavename = user.leavename;
            $scope.fromdate = user.fromdate;
            $scope.todate = user.todate;
            $scope.leavedate = user.leavedate;
            $scope.approvalname = user.approvalname;
            $scope.subperiod = user.subperiod;
            $scope.HRELAPDD_Id = user.HRELAPDD_Id;
            $scope.periodstatus = user.periodstatus;
            $scope.Remarks = user.Remarks;
            $scope.hrmeid = user.hrmeid;
            $scope.HRELAPD_Id = user.HRELAPD_Id;
            $('#changeperiodwisestaff').modal('show');
        };
            

        $scope.update = function (user) {         
            
                
            var data = {
                "HRELAPDD_Id": $scope.HRELAPDD_Id,
                "HRME_Id": $scope.user.HRME_Id,
                "HRML_LeaveName": $scope.leavename,          
                "HRELAPDD_Date": $scope.leavedate,            
                "HRELAPDD_Period":$scope.subperiod,
                "HRELAPD_Id": $scope.HRELAPD_Id,          
            }
            apiService.create("OnlineLeaveApplication/updatedetails", data).then(function (promise) {
                if (promise.returnval == true) {
                    swal('Record Updated Successfully');
                   
                }
                else {
                        swal('Failed to Update record!! Please check record may already Exist.');
                }
                
            })      
            $scope.submitted = true;
        };
       

       
        $scope.get = function (D) {
            var date12 = new Date(D);
            var day23 = $filter('date')(date12, 'EEE');
            var day123 = $filter('uppercase')(day23);
            var flag = 0;
            for (var i = 0; i < $scope.day_list.length; i++) {
                var day456 = $filter('uppercase')($scope.day_list[i].ttmD_DayName).substring(0, 3);
                if (day123 == day456) {
                    $scope.ttmD_Id = $scope.day_list[i].ttmD_Id;
                    flag = 1;
                }
            }

            $scope.staff_day_periods = [];
            $scope.absentdpcount = [];
            $scope.stf_fr_pds = false;
            $scope.mainstafflist = [];
            if ($scope.asmaY_Id == "" || $scope.myDate == "") {
                swal("Please Select Academic Year And Date");
                $scope.HRME_Id = "";
            }
            else if ($scope.asmaY_Id != "" || $scope.ttmD_Id != "") {
                var TTSD_Date = new Date($scope.myDate).toDateString();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMD_Id": $scope.ttmD_Id,
                    //"HRME_Id": $scope.HRME_Id,
                    "TTSD_Date": TTSD_Date,
                }
                apiService.create("OnlineLeaveApplication/get_period_alloted", data).
                    then(function (promise) {

                        if (promise.time_Table != "" && promise.time_Table != null) {
                            $scope.stf_dy_pds = true;
                            $scope.loaddefault = true;
                            $scope.loaddefault1 = false;
                            $scope.absentdpcount = promise.absentdpcount;

                        }
                        angular.forEach(promise.time_Table, function (user) {

                            $scope.cell_click(user.ttmD_Id, user.ttmP_Id, user.ttmD_DayName, user.ttmP_PeriodName, user.hrmE_Id,
                                user.asmaY_Id, user.asmcL_Id, user.asmS_Id, user.ttfgD_Id, user);



                        })

                        if (promise.time_Table == "" || promise.time_Table == null) {
                            swal("No periods Are Allocated To Selected Staff For Selected Day !!!");
                            $scope.stf_dy_pds = false;
                            $scope.loaddefault = false;
                            $scope.loaddefault1 = false;
                        }

                    })

            }
            if (flag == 0) {
                swal("Please Select Working Days of Week !!!");
                $scope.ttmD_Id = "";
                $scope.myDate = "";
            }
            if ($scope.asmaY_Id != "" && $scope.ttmD_Id != "" && $scope.HRME_Id != "" && $scope.HRME_Id != undefined) {
                $scope.get_period_alloted();
            }
        }


        /*ADD OR REMOVE MULTI VISITORS  */
       // $scope.morevisitor = [{ id: 'viss1' }];
        $scope.addvisitor = function () {
            var newItemNo = $scope.morevisitor.length + 1;
            if (newItemNo <= 10) {
                $scope.morevisitor.push({
                    staff_day_periods: $scope.staff_day_periods
                });
            }
        };

        $scope.removeaddvisitor = function (index) {
            var newItemNo = $scope.morevisitor.length - 1;
            $scope.morevisitor.splice(index, 1);

            if ($scope.morevisitor.length === 0) {
                //data
            }
        };

        $scope.ApplayLeave = function () {

            $scope.submitted = true;
            
            $scope.periodwisesubstituted = [];

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
                }
               //// var HRELAPDD_Date = new Date($scope.morevisitor[0].myDate).toDateString();
               // angular.forEach($scope.staff_day_periods, function (user) {
               //     $scope.classwisesubstituted.push({
               //         HRELAPDD_Period: user.ttmP_PeriodName,
               //         HRME_Id: user.HRME_Id,
               //     })
               // })

                angular.forEach($scope.morevisitor, function (user1) {

                    angular.forEach(user1.staff_day_periods, function (user) {

                        if (user.HRME_Id > 0 && user1.myDate != null) {
                            $scope.periodwisesubstituted.push({

                                HRELAPDD_Date: new Date(user1.myDate).toDateString(),
                                HRME_Id: user.HRME_Id,
                                HRELAPDD_Period: user.ttmP_PeriodName
                            })
                        }                     
                    
                    })
                    
                })
                
               
                var data = {
                    tempemp: $scope.periodwisesubstituted,                
                    "HRELAP_ApplicationDate": today,
                    "HRELAP_LeaveReason": $scope.HRELAP_LeaveReason,
                    "HRELAP_ContactNoOnLeave": Number($scope.contact),
                    "HRELAP_ReportingDate": new Date($scope.HRELAP_ReportingDate).toDateString(),
                    "HRELT_SupportingDocument": $scope.hrelT_SupportingDocument,
                    temp_table_data: rows,
                    frmToDates: temp_array
                    //"TTSD_AbsentStaff": $scope.classwisesubstituted[0].HRME_Id,             
                   
                }

                apiService.create("OnlineLeaveApplication/savedetails", data).
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
                        $scope.BindData();
                    })               
            }
            else {
                $scope.submitted = true;

            }

        };

        $scope.periodwiseapproval = function (user) {           
            $scope.periodwiseapprovallist = [];
            $scope.periodwiseapprovallist = $filter('filter')($scope.appliedgrid, function (d) {
                return d.HRELAP_Id == user.HRELAP_Id;
            });
            if ($scope.periodwiseapprovallist.length > 0) {
                $('#periodwiseapproval').modal('show');
            }
            else {
                swal("Record Not Found")
            }
            
        };


        $scope.viewcomment = function (user) {
            $scope.leavelevelapprovallist = [];
            $scope.leavelevelapprovallist = $filter('filter')($scope.empApprovaleave, function (d) {
                return d.HRELAP_Id == user.HRELAP_Id;
            });
            if ($scope.leavelevelapprovallist.length > 0) {
                $('#myModalgetclasslist').modal('show');
            }
            else {
                swal("Record Not Found")
            }

        };



    }

})();




        


