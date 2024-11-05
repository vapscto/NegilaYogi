(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeExit_ProcessController', employeeExitProcessController)
    employeeExitProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q']
    function employeeExitProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q) {
        //======================================================
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.ismresgmrE_Id = "";

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.searchValue2 = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.searchValue)) >= 0

        };
        $scope.filterValue1 = function (obj1) {

            return (angular.lowercase(obj1.employeename)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };
        $scope.filterValue2 = function (obj2) {

            return (angular.lowercase(obj2.employeename)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        $scope.grid_flag = false;
        //priview document
        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.document_Path);
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

        $scope.onview = function (filepath) {
            $('#showpdf').attr('src', filepath.document_Path);
        };
        $scope.onview = function (filepath, filename) {

            var imagedownload = filepath.document_Path;
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
        //===============
       
        
        //===========================Upload======================================
        $scope.SelectedFileForUploadzdd = [];
        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzdd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type === "application/msword") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }

                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };



        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.SelectedFileForUploadzdd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", data.ismresgmcL_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.document_Path = d;
                    $scope.document_Path1 = d;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }
        //==============================add no of days==============

        //$scope.relieving_days = function (sss) {
        //    $scope.dddd = sss;
        //    var noOfDays = "";
        //    var dateNew = "";
        //    var dateNew1 = "";
        //    $scope.Tentative_Leaving_Date = "";

        //    Date.prototype.AddDays = function (noOfDays) {
        //        this.setTime(this.getTime() + (noOfDays * (1000 * 60 * 60 * 24)));
        //        return this;
        //    }

        //    dateNew = sss;
        //    dateNew1 = sss;

        //    noOfDays = 90;

        //    $scope.newdate = dateNew.AddDays(noOfDays);
        //    $scope.Tentative_Leaving_Date = new Date($scope.newdate);
        //    $scope.ISMRESG_NoticePeriod = 90;

        //}

        $scope.relieving_days = function () {
            $scope.reliving_date = new Date($scope.ISMRESG_ResignationDate.getFullYear(), $scope.ISMRESG_ResignationDate.getMonth(), $scope.ISMRESG_ResignationDate.getDate() + 90);
            $scope.Tentative_Leaving_Date = new Date($scope.reliving_date);

            if ($scope.ISMRESG_ResignationDate != undefined && $scope.ISMRESG_ResignationDate != "") {
                $scope.ISMRESG_ResignationDate = new Date($scope.ISMRESG_ResignationDate).toDateString();
            }
            else {
                $scope.ISMRESG_ResignationDate = "";
            }            
        };

        //===============================AllDataLoad======================
        $scope.AllDataLoad = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = paginationformasters;
            var pageid = 2;
            apiService.getURI("Exit_Employee/Load_all_data", pageid).then(function (promise) {   
                $scope.employee_list_dd = promise.employee_list_dd;
                $scope.reason_list_dd = promise.reason_list_dd;
                $scope.exit_employee_list = promise.exit_employee_list;
                $scope.relieving_emp_dd = promise.relieving_emp_dd;
                $scope.relieving_emp_dd1 = promise.relieving_emp_dd1;
                $scope.relieving_check_list = promise.relieving_check_list;
                $scope.exit_print_list = promise.exit_print_list;
                $scope.presentCountgrid = $scope.exit_employee_list.length;
                $scope.presentCountgrid1 = $scope.relieving_check_list.length;
                $scope.presentCountgrid2 = $scope.exit_print_list.length;
                $scope.ISMRESG_NoticePeriod = 90;
                $scope.departmentCodeName = promise.departmentCodeName;
            });
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.Clearid1 = function () {
            // $state.reload();
            $scope.hrmE_Id2 = '';
            $scope.ismresgmrEId2 = -1;
            $scope.AllDataLoad();
        };
        $scope.Clearid2 = function () {
            // $state.reload();
            $scope.ismresgmrEId1 = '';
            $scope.searchValue2 = '';
            $scope.ismresgmrEId3 = -1;
            $scope.AllDataLoad();
        };
        $scope.view_photo = function (ee) {
            var id = ee.ISMRESG_Id;
            apiService.getURI("Exit_Employee/download_doc", id).then(function (promise) {
                $scope.download_photo = promise.download_photo;
            });
        };
        var imagedownload = "";
        var docname = "";
        //$scope.downloaddirectimage = function (data, name) {

        //    var doc_name = name;

        //    $scope.imagedownload = data;
        //    imagedownload = data;
           

        //    $http.get(imagedownload, {
        //        responseType: "arraybuffer"
        //    })
        //        .success(function (data) {
        //            var anchor = angular.element('<a/>');
        //            var blob = new Blob([data]);
        //            anchor.attr({
        //                href: window.URL.createObjectURL(blob),
        //                target: '_blank',
        //                download: doc_name 
        //                // download: doc_name + '-' + docname + '.pdf' 
        //            })[0].click();
        //        });
        //};
        
        var docname = "";

        $scope.downloaddirectimage = function (data, idd) {

            var studentreg = idd;

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
                        download: studentreg + '.jpg' 
                    })[0].click();
                });
        };
        //=======================get data===============

        $scope.GetAllRelData = function () {
            var getid = $scope.hrmE_Id2;
            apiService.getURI("Exit_Employee/GetAllRelData", getid).then(function (promise) {
                $scope.DateOfJoining = promise.getrelievingchecklist[0].hrmE_DOJ;
                $scope.Department = promise.getrelievingchecklist[0].hrmD_DepartmentName;
                $scope.Designation = promise.getrelievingchecklist[0].hrmdeS_DesignationName;
                $scope.RelievingDate = new Date(promise.getrelievingchecklist[0].ismresG_TentativeLeavingDate);
                $scope.ismresgmrEId = promise.getrelievingchecklist[0].ismresG_Id;
                $scope.ismresgmrEId2 = promise.getrelievingchecklist[0].ismresG_Id;
                $scope.mI_Name = promise.getrelievingchecklist[0].mI_Name;
                $scope.documentList = promise.doc_list;

            });
        };

        $scope.GetAllRelData1 = function () {
            var getid = $scope.hrmE_Id1;
            apiService.getURI("Exit_Employee/GetAllRelData1", getid).then(function (promise) {
                $scope.DateOfJoining1 = promise.getrelievingchecklist1[0].hrmE_DOJ;
                $scope.Department1 = promise.getrelievingchecklist1[0].hrmD_DepartmentName;
                $scope.Designation1 = promise.getrelievingchecklist1[0].hrmdeS_DesignationName;
                $scope.RelievingDate1 = new Date(promise.getrelievingchecklist1[0].ismresG_TentativeLeavingDate);
                $scope.ismresgmrEId1 = promise.getrelievingchecklist1[0].ismresG_Id;
                $scope.ismresgmrEId3 = promise.getrelievingchecklist1[0].ismresG_Id;
                $scope.MI_Name = promise.getrelievingchecklist1[0].mI_Name;


            });
        };

        //========================save/edit=======================
        //var numberOfDaysToAdd = 90;
        //$scope.newdate = $scope.mydate.setDate($scope.mydate.getDate() + numberOfDaysToAdd);
        //$scope.newdate = {};
        //var dt = $filter('date')(new Date(), "yyyy-MM-dd");
        //alert(dt);
        //$scope.newdate=dt.setDate(dt.getDate() + 90);


        $scope.submitted = false;
        $scope.Allsavedata = function () {
            var mm = $scope.dddd;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMRESG_Id": $scope.ismresgId_EDIT,
                    "HRME_Id": $scope.hrmE_Id,
                    "ISMRESG_ResignationDate": $scope.ISMRESG_ResignationDate,
                    "ISMRESGMRE_Id": $scope.ismresgmrE_Id,
                    "ISMRESG_NoticePeriod": $scope.ISMRESG_NoticePeriod,
                    "ISMRESG_TentativeLeavingDate": $scope.Tentative_Leaving_Date,
                    "ISMRESG_Remarks": $scope.ISMRESG_Remarks
                };
                apiService.create("Exit_Employee/Exit_empl_SaveEdit", data).then(function (promise) {
                    if (promise.returnval == "Add") {
                        swal('Record Saved Successfully');
                    }
                    else if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                    }
                    else if (promise.returnval == "Exist") {
                        swal('Record Already Exist');
                    }
                    else {
                        swal('Operation Failed!');
                    }
                    $state.reload();
                });
            }
            else {

                $scope.submitted = true;
            }
        };

        //========================Edit=============================
        $scope.update_AccRej = function () {
            $scope.submittedapp = false;
            if ($scope.myFormapp.$valid) {
                var data = {
                    "ISMRESG_MgmtApprRejFlg": $scope.ISMRESG_MgmtApprRejFlg,
                    "ISMRESG_AccRejDate": $scope.ISMRESG_AccRejDate,
                    "ISMRESG_ManagementRemarks": $scope.ISMRESG_ManagementRemarks,
                    "ISMRESG_Id": $scope.ismresgId
                };
                apiService.create("Exit_Employee/Exit_empl_AccRej", data).then(function (promise) {
                    if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                    }
                    else {
                        swal('Operation Failed!');
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submittedapp = true;
            }
        };
        $scope.c_approve_new = function (ss) {
            var id = ss.ismresG_Id;
            apiService.getURI("Exit_Employee/c_approve_new", id).then(function (promise) {
                if (promise.returnval === "Duplicate") {
                    swal("Already Resignation Letter Generated")
                    $state.reload();
                }
                else {
                    $scope.ismresgId = promise.ismresG_Id1;
                }
            });
        };
        $scope.edit = function (ss) {
            var id = ss.ismresG_Id;
            apiService.getURI("Exit_Employee/Edit_Employee", id).then(function (promise) {
                if (promise.returnval === "Duplicate") {
                    swal("Already Resignation Letter Generated");
                    $state.reload();
                }
                else {
                    $scope.hrmE_Id = promise.exit_employee_details[0].hrmE_Id;
                    $scope.employeename = promise.exit_employee_details[0].employeename;
                    $scope.ISMRESG_ResignationDate = new Date(promise.exit_employee_details[0].ismresG_ResignationDate);
                    $scope.ismresgmrE_Id = promise.exit_employee_details[0].ismresgmrE_Id;
                    $scope.ismresgmrE_ResignationReasons = promise.exit_employee_details[0].ismresgmrE_ResignationReasons;
                    $scope.ISMRESG_NoticePeriod = promise.exit_employee_details[0].ismresG_NoticePeriod;
                    $scope.Tentative_Leaving_Date = new Date(promise.exit_employee_details[0].ismresG_TentativeLeavingDate);
                    $scope.ISMRESG_Remarks = promise.exit_employee_details[0].ismresG_Remarks;
                    $scope.ismresgId_EDIT = promise.exit_employee_details[0].ismresG_Id;
                }
            });
        };
        $scope.c_approve = function (ss) {
            var id = ss.ismresG_Id;
            apiService.getURI("Exit_Employee/c_approve_new", id).then(function (promise) {
                if (promise.returnval === "Duplicate") {
                    swal("Already Resignation Letter Generated")
                    $state.reload();
                    $("#myModalgetvealuationlist").hide();

                }
                else {
                    $scope.ismresgId = promise.ismresG_Id1;
                    $("#myModalgetvealuationlist").show();

                }
            });

        };
        //=================\
        $scope.submitted1 = false;
        $scope.Savedata_td = function () {
            $scope.doc_list2 = [];
            $scope.doc_list3 = [];
            //$scope.doc_list3 = $scope.doc_list;
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                angular.forEach($scope.documentList, function (emp) {
                    if (emp.selectedd2 === true || emp.document_Path != null) {
                        $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                    }
                });
                var data = {
                    "ISMRESG_Id": $scope.ismresgmrEId,
                    doc_list2: $scope.doc_list2
                };

                apiService.create("Exit_Employee/Savedata_td", data).then(function (promise) {
                    if (promise.returnval = "Add") {
                        swal('Record Saved Successfully');
                    }
                    else if (promise.returnval = "Update") {
                        swal('Record Updated Successfully');
                    }
                    else {
                        swal('Operation Failed!');
                    }
                    $scope.hrmE_Id2 = '';
                    $scope.ismresgmrEId2 = -1;
                    $scope.AllDataLoad();
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };
        //===========================================
        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {
            $scope.usercheckC2 = $scope.doc_list.every(function (options) {
                return options.selectedd2;
            });
        };

        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        };

        //===================================== save and generate Report===========================

        $scope.Savedata_print = function () {
            $scope.submitted2 = true;
            if ($scope.myForm2.$valid) {
                var data = {
                    "HRME_Id": $scope.hrmE_Id1

                };

                apiService.create("Exit_Employee/Savedata_print", data)
                    .then(function (promise) {
                        if (promise.return_p === "Update") {
                            swal('Report Generate Successfully');

                        }
                        else if (promise.return_p === "Duplicate") {
                            swal('Report Already Generated');
                        }

                        else {
                            swal('Report Not Generate !');
                        }
                        $scope.ismresgmrEId1 = '';
                        $scope.searchValue2 = '';
                        $scope.ismresgmrEId3 = -1;
                        $scope.AllDataLoad();

                    });
            }
            else {
                $scope.submitted2 = true;
            }
        };
        $scope.Print_preview = function (ww) {
            var hrmeid = ww.hrmE_Id
            apiService.getURI("Exit_Employee/print_exit_employee", hrmeid).then(function (promise) {
                $scope.employeename_p = promise.exit_employee_print_report[0].employeename_p;
                $scope.HRME_EmployeeCode_p = promise.exit_employee_print_report[0].HRME_EmployeeCode_p;
                $scope.HRMDES_DesignationName_p = promise.exit_employee_print_report[0].HRMDES_DesignationName_p;
                $scope.HRME_DOJ_p = promise.exit_employee_print_report[0].HRME_DOJ_p;
                $scope.year_p = promise.exit_employee_print_report[0].year_p;
                $scope.month_p = promise.exit_employee_print_report[0].month_p;
                $scope.company_Name_p = promise.exit_employee_print_report[0].company_Name_p;
                $scope.ISMRESG_AccRejDate_p = promise.exit_employee_print_report[0].ISMRESG_AccRejDate_p;
                $scope.ISMRESG_TentativeLeavingDate_p = promise.exit_employee_print_report[0].ISMRESG_TentativeLeavingDate_p;
                $scope.Gdate_p = promise.exit_employee_print_report[0].Gdate_p;
                $scope.exit_print_list2 = promise.exit_print_list2

            });
        };
        $scope.view_rel = function (ww) {
            var hrmeid = ww.HRME_Id
            apiService.getURI("Exit_Employee/relieving_exit_employee_view", hrmeid).then(function (promise) {
                $scope.exit_print_list1 = promise.exit_print_list1

            });
        };

        $scope.print = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };
        //=====================================
        $scope.edit_rel = function (ss) {
            var pagid = ss.HRME_Id
            apiService.getURI("Exit_Employee/edit_relieving", pagid).then(function (promise) {
                $scope.employeename1 = promise.relieving_check_edit_dd[0].employeename;
                $scope.hrmE_Id2 = promise.relieving_check_edit_dd[0].hrmE_Id;
                $scope.DateOfJoining = new Date(promise.relieving_check_edit_dd[0].hrmE_DOJ);
                $scope.Department = promise.relieving_check_edit_dd[0].hrmD_DepartmentName;
                $scope.Designation = promise.relieving_check_edit_dd[0].hrmdeS_DesignationName;
                $scope.RelievingDate = new Date(promise.relieving_check_edit_dd[0].ismresG_TentativeLeavingDate);
                $scope.ismresgmrEId = promise.relieving_check_edit_dd[0].ismresG_Id;
                $scope.documentList = promise.relieving_check_edit;
                if ($scope.documentList.length > 0) {
                    $scope.document = {};
                    $scope.documentList = promise.relieving_check_edit;
                    angular.forEach(promise.documentList, function (value, key) {
                        $('#' + value.ismresgmcL_Id).attr('src', value.document_Path);
                    });
                }
                if ($scope.documentList.length > 0) {
                    angular.forEach($scope.documentList, function (ss) {
                        if (ss.ismresgcL_Id > 0) {
                            ss.selectedd2 = true;
                        }
                    });
                }
            });
        };
        $scope.selectedAll1 = "";
        $scope.all_task = function () {
            var checkStatus2 = $scope.alltask;
            angular.forEach($scope.documentList, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };
        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {

            $scope.alltask = $scope.documentList.every(function (options) {
                return options.selectedd2;
            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2 || field.$dirty;
        };
        $scope.interactedapp = function (field) {
            return $scope.submittedapp || field.$dirty;
        };
    }
})();