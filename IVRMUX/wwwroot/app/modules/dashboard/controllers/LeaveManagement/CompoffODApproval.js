(function () {
    'use strict';
    angular
.module('app')
        .controller('CompoffODApprovalController', CompoffODApprovalController)

    CompoffODApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce']
    function CompoffODApprovalController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce) {

        var copty = "";
       
      

        $scope.itemsPerPage = 10;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }

        $scope.currentPage = 1;
        $scope.coptyright = copty;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        $scope.gridleavestatus = {
            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableSelectAll: true,
            enableFiltering: true,
            enableCellEdit: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'hrmE_EmployeeFirstName', displayName: 'Name', enableCellEdit: false, width: 150 },
                { name: 'hrmL_LeaveType', displayName: 'Leave Type', enableCellEdit: false, width: 120 },
                { name: 'hrelaP_FromDate', displayName: 'Date of Work', enableCellEdit: true, width: 120 },
                { name: 'hrelaP_LeaveReason', displayName: 'Reason', enableFiltering: false, width: 200 },
                { name: 'hrelapD_InTime', displayName: 'Start Time', enableFiltering: false, width: 120 },
                { name: 'hrelapD_OutTime', displayName: 'End Time', enableFiltering: false, width: 120 },
                {
                    name: 'hrelaP_SupportingDocument', displayName: 'Document', enableCellEdit: false, width: 120, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-if="row.entity.hrelaP_SupportingDocument != null" ng-model="row.entity.hrelaP_SupportingDocument" ng-click="grid.appScope.viewdocument(row.entity);" title="View Document"> <i class="fa fa-eye text-blue"></i></a>' +
                        '<a href="javascript:void(0)" ng-if="row.entity.hrelaP_SupportingDocument == null" ng-model="row.entity.hrelaP_SupportingDocument" ng- title="No Document"> <i class="fa fa-eye-slash text-red"></i></a>' +
                        '</div>'
                },
                { name: 'hrelaP_ApplicationID', displayName: 'App. ID', enableFiltering: false },
                { name: 'hrmE_Id', displayName: 'Employee Id', enableFiltering: false }
            ]
        };
       
        //$scope.gridleavestatus.onRegisterApi = function (gridApi) {
        //    $scope.gridApi = gridApi;
        //    gridApi.selection.on.rowSelectionChanged($scope, function (row) {
        //        $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
        //    });
        //};
        var InTime = "";
        var OUTTime = "";
        $scope.compoffOdapplyedrecord = [];
        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("LeaveApproval/getRequestStatus", id).then(function (promise) {
                 
                  
                    angular.forEach(promise.get_leavestatus, function (item) {

                         InTime = moment(item.HRELAPD_InTime, 'H:mm ').format();
                         OUTTime = moment(item.HRELAPD_OutTime, 'H:mm ').format();

                        $scope.compoffOdapplyedrecord.push({
                            HRELAP_Id: item.HRELAP_Id, HRME_EmployeeFirstName: item.HRME_EmployeeFirstName,
                            HRML_LeaveName: item.HRML_LeaveName, HRELAP_FromDate: item.HRELAP_FromDate,
                            HRELAP_ToDate: item.HRELAP_ToDate, HRELAPD_InTime: InTime, HRELAPD_OutTime: OUTTime,
                            HRELAP_ApplicationStatus: item.HRELAP_ApplicationStatus, HRELAP_LeaveReason: item.HRELAP_LeaveReason,
                            HRELAPA_Remarks: item.HRELAPA_Remarks, hstep: 1, mstep: 1, ismeridian:false
                        });
                              
                   
                    });

                    console.log(promise.get_leavestatus);
                    angular.forEach($scope.gridleavestatus.data, function (grdobj) {
                        var fdate = grdobj.hrelaP_FromDate.split('T');
                        grdobj.hrelaP_FromDate = fdate[0];
                    });

                    $scope.activityIds = promise.activityIds;
                    if ($scope.count === 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }

                    $scope.fromminDate = new Date(
                        $scope.setmindate.getFullYear(),
                        $scope.setmindate.getMonth() - 12,
                        $scope.setmindate.getDate());
                    //$scope.fromDate = $scope.setmindate;

                    $scope.tominDate = new Date(
                        $scope.fromDate.getFullYear(),
                        $scope.fromDate.getMonth() - 12,
                        $scope.fromDate.getDate());
                    //$scope.toDate = $scope.tominDate;

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




        $scope.redirectpage = function () {
            $state.go('app.leaveapproval');
        };

        $scope.selectedAll1 = "";
        $scope.all_task = function () {
            var checkStatus2 = $scope.obj.alltask;
            angular.forEach($scope.compoffOdapplyedrecord, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {
            $scope.doc_list2 = [];
            $scope.usercheckC2 = $scope.compoffOdapplyedrecord.every(function (record) {
                if (record.selectedd2===true) {
                    $scope.rowsSelected.push({ HRELAP_ApplicationID: record.HRELAP_ApplicationID, HRME_EmployeeFirstName: record.HRME_EmployeeFirstName});
                }
                return record.selectedd2;
            });
        };


        $scope.get_status = function (appstatus) {

            $scope.doc_list2 = [];
            angular.forEach($scope.compoffOdapplyedrecord, function (emp) {
                if (emp.selectedd2 === true) {
                    $scope.doc_list2.push({ HRELAPA_Remarks: emp.HRELAPA_Remarks, HRELAPD_InTime: $filter('date')(emp.HRELAPD_InTime, "HH:mm"), HRELAPD_OutTime: $filter('date')(emp.HRELAPD_OutTime, "HH:mm"), HRELAP_Id: emp.HRELAP_Id, HRME_Id: emp.HRME_Id, Status: appstatus});
                }
            });


            if ($scope.doc_list2.length == 0) {
                swal("Select Atlease One Checkbox");
            }
            else {
            
            if ($scope.myForm.$valid) {
                //var data = {
                //    get_leave_status: $scope.gridApi.selection.getSelectedRows(),
                //    "HRELAPA_Remarks": $scope.remarkstxta,
                //    "HRELAPA_InTime": $filter('date')($scope.hrelapA_InTime, "HH:mm"),
                //    "HRELAPA_OutTime": $filter('date')($scope.hrelapA_OutTime, "HH:mm")
                //};
                var data = {
                    doc_list2: $scope.doc_list2
                    
                };
                apiService.create("LeaveApproval/get_approvestatus", data).
                    then(function (promise) {
                        if (appstatus === 'Approved') {
                            if (promise.message == "Add") {
                                //$scope.gridleavestatus.data = promise.get_leavestatus;
                                swal("Record Approved Successfully...");
                                $state.reload();
                            }
                            else {
                                swal("Something went wrong!!");
                            }
                        }
                        else if (appstatus === 'Rejected'){
                            if (promise.message == "Add") {
                                //$scope.gridleavestatus.data = promise.get_leavestatus;
                                swal("Record Rejected Successfully...");
                                $state.reload();
                            }
                            else {
                                swal("Something went wrong!!");
                            }
                        }
                       
                    });
            }
            else {
                $scope.submitted = true;
            }
            }
        };

        $scope.reject_status = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    get_leave_status: $scope.gridApi.selection.getSelectedRows(),
                    "HRELAPA_Remarks": $scope.remarkstxta,
                    "HRELAPA_InTime": $filter('date')($scope.hrelapA_InTime, "HH:mm"),
                    "HRELAPA_OutTime": $filter('date')($scope.hrelapA_OutTime, "HH:mm")
                };

                apiService.create("LeaveApproval/get_rejectedstatus", data).
                    then(function (promise) {
                        if (promise.message == "Success") {
                            //$scope.gridleavestatus.data = promise.get_leavestatus;
                            swal("Rejected...");
                            $state.reload();
                        }
                        else {
                            swal("Something went wrong!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.fromdate = function (d1, row) {
            $scope.From_Date = d1;
            angular.forEach($scope.gridleavestatus.data, function (grdobj) {
                if (grdobj.hrelaP_Id === row.entity.hrelaP_Id) {
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
            angular.forEach($scope.gridleavestatus.data, function (obj) {
                if (obj.hrelaP_Id === row.entity.hrelaP_Id) {
                    obj.hrelaP_ReportingDate = $scope.minDate;
                }
            });
            //$scope.HRELAP_ReportingDate = $scope.minDate;
        };

        $scope.todate = function (secondDate1, row) {
            $scope.ReportingDate = secondDate1;
            var firstDate1 = $filter('date')(row.entity.fromDate, "dd/MM/yy");
            secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
            var date2 = new Date($scope.formatString(secondDate1));
            var date1 = new Date($scope.formatString(firstDate1));
            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
            angular.forEach($scope.gridleavestatus.data, function (obj) {
                if (obj.hrelaP_Id === row.entity.hrelaP_Id) {
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
            angular.forEach($scope.gridleavestatus.data, function (obj) {
                if (obj.hrelaP_Id === row.entity.hrelaP_Id) {
                    obj.hrelaP_ReportingDate = $scope.minDate;
                }
            });
            //$scope.hrelaP_ReportingDate = $scope.minDate;
        };

        $scope.formatString = function (format) {
            var day = parseInt(format.substring(0, 2));
            var month = parseInt(format.substring(3, 5));
            var year = parseInt(format.substring(6, 10));
            var date = new Date(year, month - 1, day);
            return date;
        };

        $scope.viewdocument = function (obj) {
            if (obj != null) {
                var img = obj.hrelaP_SupportingDocument;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                obj.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.hrelaP_SupportingDocument;
                }
                else if (extention === "jpg" || extention === "jpeg" || extention === "JPEG") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', obj.hrelaP_SupportingDocument);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    var imagedownload = obj.hrelaP_SupportingDocument;
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
            }
        };

        $scope.viewcomment = function (obj) {
            var data = {
                "HRELAP_Id": obj.HRELAP_Id
            };
            apiService.create("LeaveApproval/viewcomment", data).
                then(function (promise) {
                    $scope.HRME_EmployeeFirstName = obj.HRME_EmployeeFirstName;
                    $scope.HRELAP_LeaveReason = obj.HRELAP_LeaveReason;
                    $scope.commentlist = promise.commentlist;
                });
        };

        $scope.validateTomintime1 = function (timedata) {
            $scope.hrelapA_OutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;
            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;
            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            //$scope.hrelapA_InTime = "";
        };

        $scope.validatemax1 = function (maxdata) {
            var dsttimee = $scope.hrelapA_InTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);
            $scope.FOMST_FDWHrMin = $scope.ttst;
            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.hrelapA_InTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;
                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.hrelapA_OutTime = "";
            }
        };


        $scope.validateTomintime = function (timedata) {

            

            $scope.hrelapA_OutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;
            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;
            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            //$scope.hrelapA_InTime = "";
        };

        $scope.validatemax = function (maxdata,index) {
            for (var i = 0; i <= $scope.compoffOdapplyedrecord.length; i++) {
                if (index == i) {
                    var dsttimee = $scope.compoffOdapplyedrecord[0].HRELAPD_InTime;
                }
           
            }
            //var dsttimee = $scope.hrelapA_InTime;
          
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);
            $scope.FOMST_FDWHrMin = $scope.ttst;
            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.hrelapA_InTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;
                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.hrelapA_OutTime = "";
            }
        };

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();