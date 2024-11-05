﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('HrmsNAACReportController', HrmsNAACReportController);

    HrmsNAACReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$q'];
    function HrmsNAACReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $q) {

        // form Object
        $scope.Employee = {};
        $scope.documentList = [{ id: 'document' }];
        $scope.documentListStuActivity = [{ id: 'document' }];
        $scope.documentListProfActivity = [{ id: 'document' }];
        $scope.documentListResProject = [{ id: 'document' }];
        $scope.documentListResGuidance = [{ id: 'document' }];
        $scope.documentListBOSBOE = [{ id: 'document' }];
        $scope.documentListRefJournal = [{ id: 'document' }];
        $scope.documentListConference = [{ id: 'document' }];
        $scope.documentListBook = [{ id: 'document' }];
        $scope.documentListBookChapter = [{ id: 'document' }];
        $scope.documentListCommettee = [{ id: 'document' }];
        $scope.documentListOtherDetails = [{ id: 'document' }];
        var HRME_IDSelected = 0;
        $scope.EmployeeDis = false;
        $scope.submitted = false;

        // Get form Details at onload  
        $scope.onLoadGetDataEmployeeType = function () {
            $scope.obj = {};
            $scope.HRME_IDSelected = 0;
            var pageid = 2;
            apiService.getURI("HrmsNAACReport/getdetails", pageid).then(function (promise) {

                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);
                }

                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                    $scope.designationdropdown = promise.designationdropdown;
                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function (field) {
            $scope.EmployeeDis = false;
        };

        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;

            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });

            $scope.get_depts();
        };

        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;

            });
            $scope.get_desig();
        };

        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.get_employee();
        };

        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids
            };
            apiService.create("HrmsNAACReport/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                });
        };

        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            };
            apiService.create("HrmsNAACReport/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };

        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        };

        $scope.get_employee = function () {

            $scope.selectedemptypes = [];
            $scope.selectedempdept = [];
            $scope.selectedempdesg = [];
            angular.forEach($scope.groupTypedropdown, function (role) {
                if (role.selected) $scope.selectedemptypes.push(role);
            });
            angular.forEach($scope.departmentdropdown, function (role) {
                if (role.selected) $scope.selectedempdept.push(role);
            });
            angular.forEach($scope.designationdropdown, function (role) {
                if (role.selected) $scope.selectedempdesg.push(role);
            });

            if ($scope.designationdropdown.length !== 0) {
                var data = {
                    emptypes: $scope.selectedemptypes,
                    empdept: $scope.selectedempdept,
                    empdesg: $scope.selectedempdesg
                };

                apiService.create("HrmsNAACReport/get_Employe_ob", data).
                    then(function (promise) {
                        $scope.employee = promise.get_emp;
                    });
            }
        };

        $scope.GetEmployeeSelected = function () {
            if ($scope.myForm1.$valid) {
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
        };

       
        $scope.emp_Sel = function (hrme_id) {
            $scope.submitted = true;
            if ($scope.myForm1.$valid) {
                $scope.HRME_IDSelected = hrme_id.hrmE_Id;
                $scope.EmployeeDis = false;

                $scope.documentList = [{ id: 'document' }];
                $scope.documentListStuActivity = [{ id: 'document' }];
                $scope.documentListProfActivity = [{ id: 'document' }];
                $scope.documentListResProject = [{ id: 'document' }];
                $scope.documentListResGuidance = [{ id: 'document' }];
                $scope.documentListBOSBOE = [{ id: 'document' }];
                $scope.documentListRefJournal = [{ id: 'document' }];
                $scope.documentListConference = [{ id: 'document' }];
                $scope.documentListBook = [{ id: 'document' }];
                $scope.documentListBookChapter = [{ id: 'document' }];
                $scope.documentListCommettee = [{ id: 'document' }];
                $scope.documentListOtherDetails = [{ id: 'document' }];
                $scope.examinationList = [{ id: 'document' }];
                $scope.personaldetails = [];
                $scope.qualificationdetails = [];

                $scope.internatiolnalcount = 0;
                $scope.nationalcount = 0;
                $scope.nonrefjoucount = 0;
                $scope.patentcount = 0;
                $scope.bookcount = 0;
                $scope.bookchaptercount = 0;
                $scope.citationcount = 0;
                $scope.hindexcount = 0;
                $scope.itenindexcount = 0;

                $scope.date23 = new Date($scope.obj.assessmentfrom);
                $scope.date34 = new Date($scope.obj.assessmentto);
                var data = {
                    "HRME_Id": $scope.HRME_IDSelected,
                    "assfromdate": $scope.obj.assessmentfrom,
                    "assessmentto": $scope.obj.assessmentto
                };
                apiService.create("HrmsNAACReport/get_EmployeALLDATA", data).
                    then(function (promise) {
                        $scope.internatiolnalcount = promise.internatiolnalcount;
                        $scope.nationalcount = promise.nationalcount;
                        $scope.nonrefjoucount = promise.nonrefjoucount;
                        $scope.patentcount = promise.patentcount;
                        $scope.bookcount = promise.bookcount;
                        $scope.bookchaptercount = promise.bookchaptercount;
                        $scope.citationcount = promise.citationcount;
                        $scope.hindexcount = promise.hindexcount;
                        $scope.itenindexcount = promise.itenindexcount;

                        if (promise.naacPersonalDeatilsDTO !== null && promise.naacPersonalDeatilsDTO.length > 0) {
                            $scope.empPersonaldetails = promise.naacPersonalDeatilsDTO[0];
                        }

                        if (promise.orientlist !== null && promise.orientlist.length > 0) {
                            $scope.documentList = promise.orientlist;
                            angular.forEach(promise.orientlist, function (value, key) {
                                if (value.hreorcO_From !== null) {
                                    $scope.documentList[key].hreorcO_From = new Date(value.hreorcO_From);
                                }
                                else {
                                    $scope.documentList[key].hreorcO_From = null;
                                }
                                if (value.hreorcO_To !== null) {
                                    $scope.documentList[key].hreorcO_To = new Date(value.hreorcO_To);
                                }
                                else {
                                    $scope.documentList[key].hreorcO_To = null;
                                }
                            });
                        }

                        if (promise.studentActivitylist !== null && promise.studentActivitylist.length > 0) {
                            $scope.documentListStuActivity = promise.studentActivitylist;
                            angular.forEach(promise.studentActivitylist, function (value, key) {
                                if (value.hresacT_ActivityDate !== null) {
                                    $scope.documentListStuActivity[key].hresacT_ActivityDate = new Date(value.hresacT_ActivityDate);
                                }
                                else {
                                    $scope.documentListStuActivity[key].hresacT_ActivityDate = null;
                                }
                            });
                        }

                        if (promise.professionalActivitylist !== null && promise.professionalActivitylist.length > 0) {
                            $scope.documentListProfActivity = promise.professionalActivitylist;
                            angular.forEach(promise.professionalActivitylist, function (value, key) {
                                if (value.hresacT_ActivityDate !== null) {
                                    $scope.documentListProfActivity[key].hresacT_ActivityDate = new Date(value.hresacT_ActivityDate);
                                }
                                else {
                                    $scope.documentListProfActivity[key].hresacT_ActivityDate = null;
                                }
                            });
                        }

                        if (promise.researchProjectlist !== null && promise.researchProjectlist.length > 0) {
                            $scope.documentListResProject = promise.researchProjectlist;
                        }

                        if (promise.researchGuidelist !== null && promise.researchGuidelist.length > 0) {
                            $scope.documentListResGuidance = promise.researchGuidelist;
                        }

                        if (promise.bosboElist !== null && promise.bosboElist.length > 0) {
                            $scope.documentListBOSBOE = promise.bosboElist;
                            angular.forEach(promise.bosboElist, function (value, key) {
                                if (value.hreboS_FromToDate !== null) {
                                    $scope.documentListBOSBOE[key].hreboS_FromToDate = new Date(value.hreboS_FromToDate);
                                }
                                else {
                                    $scope.documentListBOSBOE[key].hreboS_FromToDate = null;
                                }
                            });
                        }

                        if (promise.refJournallist !== null && promise.refJournallist.length > 0) {
                            $scope.documentListRefJournal = promise.refJournallist;
                        }

                        if (promise.examinationlist !== null && promise.examinationlist.length > 0) {
                            $scope.examinationlist = promise.examinationlist;
                        }

                        if (promise.nonRefJournallist !== null && promise.nonRefJournallist.length > 0) {
                            $scope.documentListNonRefJournal = promise.nonRefJournallist;
                        }

                        if (promise.conferencelist !== null && promise.conferencelist.length > 0) {
                            $scope.documentListConference = promise.conferencelist;
                        }

                        if (promise.booklist !== null && promise.booklist.length > 0) {
                            $scope.documentListBook = promise.booklist;
                        }

                        if (promise.bookChapterlist !== null && promise.bookChapterlist.length > 0) {
                            $scope.documentListBookChapter = promise.bookChapterlist;
                        }

                        if (promise.commetteelist !== null && promise.commetteelist.length > 0) {
                            $scope.documentListCommettee = promise.commetteelist;
                        }

                        if (promise.otherDetailSlist !== null && promise.otherDetailSlist.length > 0) {
                            $scope.documentListOtherDetails = promise.otherDetailSlist;
                        }

                        if (promise.documentDetails !== null && promise.documentDetails.length > 0) {
                            $scope.documentDetails = promise.documentDetails;
                        }

                        if (promise.qualifctionDetails !== null && promise.qualifctionDetails.length > 0) {
                            $scope.qualifctionDetails = promise.qualifctionDetails;
                        }

                        $scope.EmployeeDis = true;
                    });
            }
        };

        //ORIENTATION
        $scope.addNewDocument = function () {
            var newItemNo = $scope.documentList.length + 1;
            if (newItemNo <= 10) {
                $scope.documentList.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocument = function (index, data) {
            var newItemNo = $scope.documentList.length - 1;
            $scope.documentList.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocument(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        $scope.submitted2 = false;
        $scope.validateOrientationDetails = function () {
            if ($scope.myForm2.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentList, function (value, key) {
                    if ($scope.chkdup_documentsDetails($scope.documentList[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_OrientationCourseArrayDTO": $scope.documentList,
                        "HRME_Id": $scope.HRME_IDSelected,
                        "TabName": "OrientationTab"
                    };
                    apiService.create("naacHrmsDetails/SaveData", data).
                        then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.retrunMsg === "Duplicate") {
                                    swal("Record already exist..!!");
                                    return;
                                } else if (promise.retrunMsg === "false") {
                                    swal("Record Not saved / Updated..", 'Fail');
                                } else if (promise.retrunMsg === "Add") {
                                    swal("Record Saved Successfully..");
                                    $scope.Otherdetails = false;
                                    $scope.myTabIndex = $scope.myTabIndex + 1;
                                } else if (promise.retrunMsg === "Update") {
                                    swal("Record Updated Successfully..");
                                    $scope.Otherdetails = false;
                                    $scope.myTabIndex = $scope.myTabIndex + 1;
                                } else {
                                    swal("Something went wrong ..!", 'Kindly contact Administrator');
                                    return;
                                }
                            }
                        });
                }
            }
            else {
                $scope.submitted2 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetails = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentList.length; k++) {
                var arryind = $scope.documentList.indexOf($scope.documentList[k]);
                if (arryind !== index) {
                    if ($scope.documentList[k].hreorcO_Document === user.hreorcO_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        function UploadEmployeeDocument(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
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
                        data.hreorcO_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetails = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hreorcO_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hreorcO_Document);
                $('#preview').attr('src', data.hreorcO_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.clear_orient_tab = function () {
            $scope.documentList = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };

        $scope.onLoadGetDataOrientation = function () {
            $scope.clear_orient_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getOrientdata", data).then(function (promise) {
                if (promise.orientlist !== null && promise.orientlist.length > 0) {
                    $scope.documentList = promise.orientlist;
                    angular.forEach(promise.orientlist, function (value, key) {
                        if (value.hreorcO_From !== null) {
                            $scope.documentList[key].hreorcO_From = new Date(value.hreorcO_From);
                        }
                        else {
                            $scope.documentList[key].hreorcO_From = null;
                        }
                        if (value.hreorcO_To !== null) {
                            $scope.documentList[key].hreorcO_To = new Date(value.hreorcO_To);
                        }
                        else {
                            $scope.documentList[key].hreorcO_To = null;
                        }
                    });
                }
            });
        };
        //ORIENTATION

        //STUDENT ACTIVITY
        $scope.addNewDocumentStuActivity = function () {
            var newItemNo = $scope.documentListStuActivity.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListStuActivity.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentStuActivity = function (index, data) {
            var newItemNo = $scope.documentListStuActivity.length - 1;
            $scope.documentListStuActivity.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdStuActivity = [];
        $scope.selectFileforUploadzdStuActivity = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdStuActivity = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentStuActivity(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentStuActivity(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdStuActivity.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdStuActivity[i]);
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
                        data.hresacT_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsStuActivity = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hresacT_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted3 = false;
        $scope.validateStuActivityDetails = function () {
            if ($scope.myForm3.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListStuActivity, function (value, key) {
                    if ($scope.chkdup_documentsDetailsStuActivity($scope.documentListStuActivity[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_StudentActivitiesArrayDTO": $scope.documentListStuActivity,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "StudentActivityTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted3 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted3 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsStuActivity = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListStuActivity.length; k++) {
                var arryind = $scope.documentListStuActivity.indexOf($scope.documentListStuActivity[k]);
                if (arryind !== index) {
                    if ($scope.documentListStuActivity[k].hresacT_Document === user.hresacT_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_StuActivity_tab = function () {
            $scope.documentListStuActivity = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
        };

        $scope.onLoadGetDataStudentActivity = function () {
            $scope.clear_StuActivity_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getStudentActivitydata", data).then(function (promise) {
                if (promise.studentActivitylist !== null && promise.studentActivitylist.length > 0) {
                    $scope.documentListStuActivity = promise.studentActivitylist;
                    angular.forEach(promise.studentActivitylist, function (value, key) {
                        if (value.hresacT_ActivityDate !== null) {
                            $scope.documentListStuActivity[key].hresacT_ActivityDate = new Date(value.hresacT_ActivityDate);
                        }
                        else {
                            $scope.documentListStuActivity[key].hresacT_ActivityDate = null;
                        }
                    });
                }
            });
        };
        //STUDENT ACTIVITY

        //PROFESSIONAL ACTIVITY
        $scope.addNewDocumentProfActivity = function () {
            var newItemNo = $scope.documentListProfActivity.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListProfActivity.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentProfActivity = function (index, data) {
            var newItemNo = $scope.documentListProfActivity.length - 1;
            $scope.documentListProfActivity.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdProfActivity = [];
        $scope.selectFileforUploadzdProfActivity = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdProfActivity = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentProfActivity(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentProfActivity(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdProfActivity.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdProfActivity[i]);
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
                        data.hresacT_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsProfActivity = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hresacT_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted4 = false;
        $scope.validateProfActivityDetails = function () {
            if ($scope.myForm4.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListProfActivity, function (value, key) {
                    if ($scope.chkdup_documentsDetailsProfActivity($scope.documentListProfActivity[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_StudentActivitiesArrayDTO": $scope.documentListProfActivity,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "ProfessionalActivityTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted4 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted4 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsProfActivity = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListProfActivity.length; k++) {
                var arryind = $scope.documentListProfActivity.indexOf($scope.documentListProfActivity[k]);
                if (arryind !== index) {
                    if ($scope.documentListProfActivity[k].hresacT_Document === user.hresacT_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_ProfActivity_tab = function () {
            $scope.documentListProfActivity = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
        };

        $scope.onLoadGetDataProfessionalActivity = function () {
            $scope.clear_ProfActivity_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getProfessionalActivitydata", data).then(function (promise) {
                if (promise.professionalActivitylist !== null && promise.professionalActivitylist.length > 0) {
                    $scope.documentListProfActivity = promise.professionalActivitylist;
                    angular.forEach(promise.professionalActivitylist, function (value, key) {
                        if (value.hresacT_ActivityDate !== null) {
                            $scope.documentListProfActivity[key].hresacT_ActivityDate = new Date(value.hresacT_ActivityDate);
                        }
                        else {
                            $scope.documentListProfActivity[key].hresacT_ActivityDate = null;
                        }
                    });
                }
            });
        };
        //PROFESSIONAL ACTIVITY

        //RESEARCH PROJECT
        $scope.addNewDocumentResProject = function () {
            var newItemNo = $scope.documentListResProject.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListResProject.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentResProject = function (index, data) {
            var newItemNo = $scope.documentListResProject.length - 1;
            $scope.documentListResProject.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdResProject = [];
        $scope.selectFileforUploadzdResProject = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdResProject = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentResProject(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentResProject(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdResProject.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdResProject[i]);
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
                        data.hrerepR_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsResProject = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrerepR_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted5 = false;
        $scope.validateResProjectDetails = function () {
            if ($scope.myForm5.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListResProject, function (value, key) {
                    if ($scope.chkdup_documentsDetailsResProject($scope.documentListResProject[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_ResearchProjectsArrayDTO": $scope.documentListResProject,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "ResearchProjectTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted5 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted5 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsResProject = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListResProject.length; k++) {
                var arryind = $scope.documentListResProject.indexOf($scope.documentListResProject[k]);
                if (arryind !== index) {
                    if ($scope.documentListResProject[k].hrerepR_Document === user.hrerepR_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_ResProject_tab = function () {
            $scope.documentListResProject = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted5 = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        };

        $scope.onLoadGetDataResearchProject = function () {
            $scope.clear_ResProject_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getResearchProjectdata", data).then(function (promise) {
                if (promise.researchProjectlist !== null && promise.researchProjectlist.length > 0) {
                    $scope.documentListResProject = promise.researchProjectlist;
                }
            });
        };
        //RESEARCH PROJECT

        //RESEARCH GUIDANCE
        $scope.addNewDocumentResGuidance = function () {
            var newItemNo = $scope.documentListResGuidance.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListResGuidance.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentResGuidance = function (index, data) {
            var newItemNo = $scope.documentListResGuidance.length - 1;
            $scope.documentListResGuidance.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdResGuidance = [];
        $scope.selectFileforUploadzdResGuidance = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdResGuidance = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentResGuidance(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentResGuidance(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdResGuidance.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdResGuidance[i]);
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
                        data.hreregU_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsResGuidance = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hreregU_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted6 = false;
        $scope.validateResGuidanceDetails = function () {
            if ($scope.myForm6.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListResGuidance, function (value, key) {
                    if ($scope.chkdup_documentsDetailsResGuidance($scope.documentListResGuidance[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_ResearchGuidanceArrayDTO": $scope.documentListResGuidance,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "ResearchGuidanceTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted6 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted6 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsResGuidance = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListResGuidance.length; k++) {
                var arryind = $scope.documentListResGuidance.indexOf($scope.documentListResGuidance[k]);
                if (arryind !== index) {
                    if ($scope.documentListResGuidance[k].hrerepR_Document === user.hrerepR_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_ResGuidance_tab = function () {
            $scope.documentListResGuidance = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted6 = false;
            $scope.myForm6.$setPristine();
            $scope.myForm6.$setUntouched();
        };

        $scope.onLoadGetDataResearchGuide = function () {
            $scope.clear_ResGuidance_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getResearchGuidedata", data).then(function (promise) {
                if (promise.researchGuidelist !== null && promise.researchGuidelist.length > 0) {
                    $scope.documentListResGuidance = promise.researchGuidelist;
                }
            });
        };
        //RESEARCH GUIDANCE

        //BOSBOE
        $scope.addNewDocumentBOSBOE = function () {
            var newItemNo = $scope.documentListBOSBOE.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListBOSBOE.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentBOSBOE = function (index, data) {
            var newItemNo = $scope.documentListBOSBOE.length - 1;
            $scope.documentListBOSBOE.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdBOSBOE = [];
        $scope.selectFileforUploadzdBOSBOE = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBOSBOE(data) {
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
                        data.hreboS_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBOSBOE = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hreboS_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted7 = false;
        $scope.validateBOSBOEDetails = function () {
            if ($scope.myForm7.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListBOSBOE, function (value, key) {
                    if ($scope.chkdup_documentsDetailsBOSBOE($scope.documentListBOSBOE[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_BOSBOEArrayDTO": $scope.documentListBOSBOE,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "BOSBOETab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted7 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted7 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsBOSBOE = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListBOSBOE.length; k++) {
                var arryind = $scope.documentListBOSBOE.indexOf($scope.documentListBOSBOE[k]);
                if (arryind !== index) {
                    if ($scope.documentListBOSBOE[k].hreboS_Document === user.hreboS_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_BOSBOE_tab = function () {
            $scope.documentListBOSBOE = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted7 = false;
            $scope.myForm7.$setPristine();
            $scope.myForm7.$setUntouched();
        };

        $scope.onLoadGetBOSBOE = function () {
            $scope.clear_BOSBOE_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getBOSBOEdata", data).then(function (promise) {
                if (promise.bosboElist !== null && promise.bosboElist.length > 0) {
                    $scope.documentListBOSBOE = promise.bosboElist;
                    angular.forEach(promise.bosboElist, function (value, key) {
                        if (value.hreboS_FromToDate !== null) {
                            $scope.documentListBOSBOE[key].hreboS_FromToDate = new Date(value.hreboS_FromToDate);
                        }
                        else {
                            $scope.documentListBOSBOE[key].hreboS_FromToDate = null;
                        }
                    });
                }
            });
        };
        //BOSBOE

        //JOURNALS
        $scope.addNewDocumentJournal = function () {
            var newItemNo = $scope.documentListRefJournal.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListRefJournal.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentJournal = function (index, data) {
            var newItemNo = $scope.documentListRefJournal.length - 1;
            $scope.documentListRefJournal.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdJournal = [];
        $scope.selectFileforUploadzdJournal = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdJournal = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentJournal(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentJournal(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdJournal.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdJournal[i]);
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
                        data.hrejornL_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsJournal = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrejornL_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted8 = false;
        $scope.validateJournalDetails = function () {
            if ($scope.myForm8.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListRefJournal, function (value, key) {
                    if ($scope.chkdup_documentsDetailsJournal($scope.documentListRefJournal[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_JournalArrayDTO": $scope.documentListRefJournal,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "JournalTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted8 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted8 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsJournal = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListRefJournal.length; k++) {
                var arryind = $scope.documentListRefJournal.indexOf($scope.documentListRefJournal[k]);
                if (arryind !== index) {
                    if ($scope.documentListRefJournal[k].hrejornL_Document === user.hrejornL_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_Journal_tab = function () {
            $scope.documentListRefJournal = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted8 = false;
            $scope.myForm8.$setPristine();
            $scope.myForm8.$setUntouched();
        };

        $scope.onLoadGetJournal = function () {
            $scope.clear_Journal_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getJournaldata", data).then(function (promise) {
                if (promise.journallist !== null && promise.journallist.length > 0) {
                    $scope.documentListRefJournal = promise.journallist;
                }
            });
        };
        //JOURNALS

        //CONFERENCE
        $scope.addNewDocumentConference = function () {
            var newItemNo = $scope.documentListConference.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListConference.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentConference = function (index, data) {
            var newItemNo = $scope.documentListConference.length - 1;
            $scope.documentListConference.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdConference = [];
        $scope.selectFileforUploadzdConference = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdConference = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentConference(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentConference(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdConference.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdConference[i]);
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
                        data.hreconF_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsConference = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hreconF_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted9 = false;
        $scope.validateConferenceDetails = function () {
            if ($scope.myForm9.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListConference, function (value, key) {
                    if ($scope.chkdup_documentsDetailsConference($scope.documentListConference[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_ConferenceArrayDTO": $scope.documentListConference,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "ConferenceTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted9 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted9 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsConference = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListConference.length; k++) {
                var arryind = $scope.documentListConference.indexOf($scope.documentListConference[k]);
                if (arryind !== index) {
                    if ($scope.documentListConference[k].hreconF_Document === user.hreconF_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_Conference_tab = function () {
            $scope.documentListConference = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted9 = false;
            $scope.myForm9.$setPristine();
            $scope.myForm9.$setUntouched();
        };

        $scope.onLoadGetConference = function () {
            $scope.clear_Conference_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getConferencedata", data).then(function (promise) {
                if (promise.conferencelist !== null && promise.conferencelist.length > 0) {
                    $scope.documentListConference = promise.conferencelist;
                }
            });
        };
        //CONFERENCE

        //BOOK
        $scope.addNewDocumentBook = function () {
            var newItemNo = $scope.documentListBook.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListBook.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentBook = function (index, data) {
            var newItemNo = $scope.documentListBook.length - 1;
            $scope.documentListBook.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdBook = [];
        $scope.selectFileforUploadzdBook = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdBook = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBook(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBook(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdBook.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBook[i]);
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
                        data.hrebK_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBook = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrebK_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted10 = false;
        $scope.validateBookDetails = function () {
            if ($scope.myForm10.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListBook, function (value, key) {
                    if ($scope.chkdup_documentsDetailsBook($scope.documentListBook[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_BookArrayDTO": $scope.documentListBook,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "BookTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted10 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted10 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsBook = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListBook.length; k++) {
                var arryind = $scope.documentListBook.indexOf($scope.documentListBook[k]);
                if (arryind !== index) {
                    if ($scope.documentListBook[k].hrebK_Document === user.hrebK_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_Book_tab = function () {
            $scope.documentListBook = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted10 = false;
            $scope.myForm10.$setPristine();
            $scope.myForm10.$setUntouched();
        };

        $scope.onLoadGetBook = function () {
            $scope.clear_Book_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getBookdata", data).then(function (promise) {
                if (promise.booklist !== null && promise.booklist.length > 0) {
                    $scope.documentListBook = promise.booklist;
                }
            });
        };
        //BOOK

        //BOOK CHAPTER
        $scope.addNewDocumentBookChapter = function () {
            var newItemNo = $scope.documentListBookChapter.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListBookChapter.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentBookChapter = function (index, data) {
            var newItemNo = $scope.documentListBookChapter.length - 1;
            $scope.documentListBookChapter.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdBookChapter = [];
        $scope.selectFileforUploadzdBookChapter = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdBookChapter = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBookChapter(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBookChapter(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdBookChapter.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBookChapter[i]);
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
                        data.hrebkcP_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBookChapter = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrebkcP_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted11 = false;
        $scope.validateBookChapterDetails = function () {
            if ($scope.myForm11.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListBookChapter, function (value, key) {
                    if ($scope.chkdup_documentsDetailsBookChapter($scope.documentListBookChapter[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_BookChapterArrayDTO": $scope.documentListBookChapter,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "BookChapterTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted11 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted11 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsBookChapter = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListBookChapter.length; k++) {
                var arryind = $scope.documentListBookChapter.indexOf($scope.documentListBookChapter[k]);
                if (arryind !== index) {
                    if ($scope.documentListBookChapter[k].hrebkcP_Document === user.hrebkcP_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_BookChapter_tab = function () {
            $scope.documentListBookChapter = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted11 = false;
            $scope.myForm11.$setPristine();
            $scope.myForm11.$setUntouched();
        };

        $scope.onLoadGetBookChapter = function () {
            $scope.clear_BookChapter_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getBookChapterdata", data).then(function (promise) {
                if (promise.bookChapterlist !== null && promise.bookChapterlist.length > 0) {
                    $scope.documentListBookChapter = promise.bookChapterlist;
                }
            });
        };
        //BOOK CHAPTER


        //Commettee
        $scope.addNewDocumentCommettee = function () {
            var newItemNo = $scope.documentListCommettee.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListCommettee.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentCommettee = function (index, data) {
            var newItemNo = $scope.documentListCommettee.length - 1;
            $scope.documentListCommettee.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdCommettee = [];
        $scope.selectFileforUploadzdCommettee = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdCommettee = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentCommettee(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentCommettee(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdCommettee.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdCommettee[i]);
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
                        data.hrecoM_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsCommettee = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrecoM_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hrecoM_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted12 = false;
        $scope.validateCommetteeDetails = function () {
            if ($scope.myForm12.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListCommettee, function (value, key) {
                    if ($scope.chkdup_documentsDetailsCommettee($scope.documentListCommettee[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_CommitteeArrayDTO": $scope.documentListCommettee,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "CommetteeTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted12 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted12 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsCommettee = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListCommettee.length; k++) {
                var arryind = $scope.documentListCommettee.indexOf($scope.documentListCommettee[k]);
                if (arryind !== index) {
                    if ($scope.documentListCommettee[k].hrecoM_Document === user.hrecoM_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_Commettee_tab = function () {
            $scope.documentListCommettee = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted12 = false;
            $scope.myForm12.$setPristine();
            $scope.myForm12.$setUntouched();
        };

        $scope.onLoadGetCommettee = function () {
            $scope.clear_Commettee_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getCommetteedata", data).then(function (promise) {
                if (promise.commetteelist !== null && promise.commetteelist.length > 0) {
                    $scope.documentListCommettee = promise.commetteelist;
                }
            });
        };
        //Commettee

        //OTHER DETAILS
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }
        };

        $scope.SelectedFileForUploadzdOtherDetail = [];
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentOtherDetail(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentOtherDetail(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
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
                        data.hreothdeT_Document = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsOtherDetail = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hreothdeT_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hresacT_Document);
                $('#preview').attr('src', data.hreothdeT_Document);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.submitted13 = false;
        $scope.validateOtherDetailDetails = function () {
            if ($scope.myForm13.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListOtherDetails, function (value, key) {
                    if ($scope.chkdup_documentsDetailsOtherDetail($scope.documentListOtherDetails[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_OtherDetailsArrayDTO": $scope.documentListOtherDetails,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "OtherDetailTab"
                        };
                        apiService.create("naacHrmsDetails/SaveData", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Duplicate") {
                                        swal("Record already exist..!!");
                                        return;
                                    } else if (promise.retrunMsg === "false") {
                                        swal("Record Not saved / Updated..", 'Fail');
                                    } else if (promise.retrunMsg === "Add") {
                                        swal("Record Saved Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $scope.myTabIndex = $scope.myTabIndex + 1;
                                    } else {
                                        swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        return;
                                    }
                                }
                            });
                    }
                } else {
                    swal("Select Employee..");
                    $scope.submitted13 = false;
                    $scope.myTabIndex = 0;
                }
            }
            else {
                $scope.submitted13 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsOtherDetail = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListOtherDetails.length; k++) {
                var arryind = $scope.documentListOtherDetails.indexOf($scope.documentListOtherDetails[k]);
                if (arryind !== index) {
                    if ($scope.documentListOtherDetails[k].hreothdeT_Document === user.hreothdeT_Document
                    ) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_OtherDetail_tab = function () {
            $scope.documentListOtherDetails = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted13 = false;
            $scope.myForm13.$setPristine();
            $scope.myForm13.$setUntouched();
        };

        $scope.onLoadGetOtherDetail = function () {
            $scope.clear_OtherDetail_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetails/getOtherDetaildata", data).then(function (promise) {
                if (promise.otherDetailSlist !== null && promise.otherDetailSlist.length > 0) {
                    $scope.documentListOtherDetails = promise.otherDetailSlist;
                }
            });
        };
        //OTHER DETAILS

        $scope.CalculateExperience = function (orient) {
            orient.hreorcO_Duration = "";

            var joindate = new Date($filter('date')(new Date(orient.hreorcO_From).toDateString(), "yyyy/MM/dd"));
            var leftdate = new Date($filter('date')(new Date(orient.hreorcO_To).toDateString(), "yyyy/MM/dd"));

            var exp = $scope.CalDate(leftdate, joindate);
            orient.hreorcO_Duration = exp + 1;
        };

        $scope.CalDate = function (date1, date2) {
            var diffnew = new Date(
                date1.getDay() - date2.getDay()
            );
            var oneDay = 24 * 60 * 60 * 1000;
            var message = Math.round(Math.abs((date1.getTime() - date2.getTime()) / (oneDay)));
            return message;
        };

        $scope.goPrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.printToCart = function (Employee) {
            var innerContents = document.getElementById("EmpNaacHRMS").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/EmpPaySlipPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

    }
})();