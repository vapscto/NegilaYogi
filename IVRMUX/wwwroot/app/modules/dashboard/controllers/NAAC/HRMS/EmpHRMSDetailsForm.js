(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmpnaacHrmsDetailsController', EmpnaacHrmsDetailsController);

    EmpnaacHrmsDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$q','$sce'];
    function EmpnaacHrmsDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $q, $sce) {

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
        $scope.documentListGroupADetails = [{ id: 'document' }];
        $scope.documentListGroupBDetails = [{ id: 'document' }];
        $scope.examinationList = [{ id: 'document' }];

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.interacted4 = function (field) {
            return $scope.submitted4;
        };

        $scope.interacted5 = function (field) {
            return $scope.submitted5;
        };

        $scope.interacted6 = function (field) {
            return $scope.submitted6;
        };

        $scope.interacted7 = function (field) {
            return $scope.submitted7;
        };

        $scope.interacted8 = function (field) {
            return $scope.submitted8;
        };

        $scope.interacted9 = function (field) {
            return $scope.submitted9;
        };

        $scope.interacted10 = function (field) {
            return $scope.submitted10;
        };

        $scope.interacted11 = function (field) {
            return $scope.submitted11;
        };

        $scope.interacted12 = function (field) {
            return $scope.submitted12;
        };

        $scope.interacted13 = function (field) {
            return $scope.submitted13;
        };

        $scope.interacted14 = function (field) {
            return $scope.submitted14;
        };

        $scope.interacted15 = function (field) {
            return $scope.submitted15;
        };

        $scope.interacted16 = function (field) {
            return $scope.submitted16;
        };

        //$scope.get_employee = function () {

        //    $scope.selectedemptypes = [];
        //    $scope.selectedempdept = [];
        //    $scope.selectedempdesg = [];
        //    angular.forEach($scope.groupTypedropdown, function (role) {
        //        if (role.selected) $scope.selectedemptypes.push(role);
        //    });
        //    angular.forEach($scope.departmentdropdown, function (role) {
        //        if (role.selected) $scope.selectedempdept.push(role);
        //    });
        //    angular.forEach($scope.designationdropdown, function (role) {
        //        if (role.selected) $scope.selectedempdesg.push(role);
        //    });

        //    if ($scope.designationdropdown.length !== 0) {
        //        var data = {
        //            emptypes: $scope.selectedemptypes,
        //            empdept: $scope.selectedempdept,
        //            empdesg: $scope.selectedempdesg
        //        };

        //        apiService.create("naacHrmsDetails/get_Employe_ob", data).
        //            then(function (promise) {
        //                $scope.employee = promise.get_emp;
        //            });
        //    }
        //};

        //$scope.clear = function () {
        //    $scope.employee = [];
        //    $scope.obj = {};
        //    $scope.submitted = false;
        //};

        //$scope.GetEmployeeSelected = function () {
        //    if ($scope.myForm1.$valid) {
        //        $scope.myTabIndex = $scope.myTabIndex + 1;
        //    }
        //};

        $scope.emp_Sel = function () {
            debugger;
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
            $scope.documentListGroupADetails = [{ id: 'document' }];
            $scope.documentListGroupBDetails = [{ id: 'document' }];
            $scope.examinationList = [{ id: 'document' }];

            var data = {
                "HRME_Id": 1,
                "Type": "Employee"
            };
            apiService.create("naacHrmsDetails/empget_EmployeALLDATA", data).
                then(function (promise) {
                    debugger;
                    if (promise.orientlist !== null && promise.orientlist.length > 0) {
                        $scope.documentList = promise.orientlist;
                        //added extra variable for filepath
                        angular.forEach($scope.documentList, function (doc) {
                            doc.filepath = doc.hreorcO_Document;
                        });
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

                            if ($scope.documentList[key].hreorcO_Document !== null && $scope.documentList[key].hreorcO_Document !== undefined) {
                                var filename = $scope.documentList[key].hreorcO_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentList[key].extention = nameArray[nameArray.length - 1];
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
                        debugger;
                        $scope.documentListProfActivity = promise.professionalActivitylist;
                        angular.forEach($scope.documentListProfActivity, function (doc){
                            doc.filepath = doc.hredacT_Document;
                        });
                        angular.forEach(promise.professionalActivitylist, function (value, key) {
                            if (value.hredacT_ActivityDate !== null) {
                                $scope.documentListProfActivity[key].hredacT_ActivityDate = new Date(value.hredacT_ActivityDate);
                            }
                            else {
                                $scope.documentListProfActivity[key].hredacT_ActivityDate = null;
                            }
                            if ($scope.documentListProfActivity[key].hredacT_Document !== null && $scope.documentListProfActivity[key].hredacT_Document !== undefined) {
                                var filename = $scope.documentListProfActivity[key].hredacT_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListProfActivity[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.researchProjectlist !== null && promise.researchProjectlist.length > 0) {
                        $scope.documentListResProject = promise.researchProjectlist;
                        angular.forEach($scope.documentListResProject, function (doc) {
                            doc.filepath = doc.hrerepR_Document;
                        });
                        angular.forEach(promise.researchProjectlist, function (value, key) {
                            if (value.hrerepR_Document !== null && value.hrerepR_Document !== undefined) {
                                var filename = value.hrerepR_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListResProject[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.researchGuidelist !== null && promise.researchGuidelist.length > 0) {
                        $scope.documentListResGuidance = promise.researchGuidelist;
                        angular.forEach($scope.documentListResGuidance, function (doc) {
                            doc.filepath = doc.hreregU_Document;
                        });
                        angular.forEach(promise.researchGuidelist, function (value, key) {
                            if (value.hreregU_Document !== null && value.hreregU_Document !== undefined) {
                                var filename = value.hreregU_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListResGuidance[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.bosboElist !== null && promise.bosboElist.length > 0) {
                        $scope.documentListBOSBOE = promise.bosboElist;
                        angular.forEach($scope.documentListBOSBOE, function (doc) {
                            doc.filepath = doc.hreboS_Document;
                        });
                        angular.forEach(promise.bosboElist, function (value, key) {
                            if (value.hreboS_FromToDate !== null) {
                                $scope.documentListBOSBOE[key].hreboS_FromToDate = new Date(value.hreboS_FromToDate);
                            }
                            else {
                                $scope.documentListBOSBOE[key].hreboS_FromToDate = null;
                            }

                            if (value.hreboS_Document !== null && value.hreboS_Document !== undefined) {
                                var filename = value.hreboS_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListBOSBOE[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.journallist !== null && promise.journallist.length > 0) {
                        $scope.documentListRefJournal = promise.journallist;
                        angular.forEach($scope.documentListRefJournal, function (doc) {
                            doc.filepath = doc.hrejornL_Document;
                        });
                        angular.forEach(promise.journallist, function (value, key) {
                            if (value.hrejornL_Document !== null && value.hrejornL_Document !== undefined) {
                                var filename = value.hrejornL_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListRefJournal[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.examinationlist !== null && promise.examinationlist.length > 0) {
                        $scope.examinationList = promise.examinationlist;
                    }

                    if (promise.conferencelist !== null && promise.conferencelist.length > 0) {
                        $scope.documentListConference = promise.conferencelist;
                    }

                    if (promise.booklist !== null && promise.booklist.length > 0) {
                        $scope.documentListBook = promise.booklist;
                        angular.forEach($scope.documentListBook, function (doc) {
                            doc.filepath = doc.hrebK_Document;
                        });
                        angular.forEach(promise.booklist, function (value, key) {
                            if (value.hrebK_Document !== null && value.hrebK_Document !== undefined) {
                                var filename = value.hrebK_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListBook[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.bookChapterlist !== null && promise.bookChapterlist.length > 0) {
                        $scope.documentListBookChapter = promise.bookChapterlist;
                        angular.forEach($scope.documentListBookChapter, function (doc) {
                            doc.filepath = doc.hrebkcP_Document;
                        });
                        angular.forEach(promise.bookChapterlist, function (value, key) {
                            if (value.hrebkcP_Document !== null && value.hrebkcP_Document !== undefined) {
                                var filename = value.hrebkcP_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListBookChapter[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.commetteelist !== null && promise.commetteelist.length > 0) {
                        $scope.documentListCommettee = promise.commetteelist;
                    }

                    if (promise.otherDetailSlist !== null && promise.otherDetailSlist.length > 0) {
                        $scope.documentListOtherDetails = promise.otherDetailSlist;
                        angular.forEach($scope.documentListOtherDetails, function (doc) {
                            doc.filepath = doc.hreothdeT_Document;
                        });
                        angular.forEach(promise.otherDetailSlist, function (value, key) {
                            if (value.hreothdeT_Document !== null && value.hreothdeT_Document !== undefined) {
                                var filename = value.hreothdeT_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListOtherDetails[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.groupAExamlist !== null && promise.groupAExamlist.length > 0) {
                        $scope.documentListGroupADetails = promise.groupAExamlist;
                    }
                    //$scope.GetGroupAExam();

                    if (promise.groupBExamlist !== null && promise.groupBExamlist.length > 0) {
                        $scope.documentListGroupBDetails = promise.groupBExamlist;
                    }
                });
        };

        //ORIENTATION
        $scope.addNewDocument = function () {
            var newItemNo = $scope.documentList.length + 1;
            if (newItemNo <= 30) {
                $scope.documentList.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocument = function (index, data) {
            var newItemNo = $scope.documentList.length - 1;
            $scope.documentList.splice(index, 1);
            if (data.hreorcO_Id > 0) {
                $scope.DeleteDocumentDataOrientation(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocument(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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
                        "TabName": "OrientationTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                    if ($scope.documentList[k].hreorcO_Document === user.hreorcO_Document && $scope.documentList[k].hreorcO_Document !== null)
                    {
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

        //$scope.showmodaldetails = function (data) {
        //    $('#preview').removeAttr('src');
        //    var filename = data.hreorcO_Document.toString();
        //    var nameArray = filename.split('.');
        //    var extention = nameArray[nameArray.length - 1];
        //    if (extention === "jpg" || extention === "jpeg") {
        //        $('#preview').attr('src', data.hreorcO_Document);
        //    }
        //    else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
        //        //$('#preview').removeAttr('src');
        //        $('#preview').attr('src', data.hreorcO_Document);
        //    }
        //    else if (extention === "pdf") {
        //        var imagedownload = data.hreorcO_Document;
        //        $scope.content = "";
        //        var fileURL = "";
        //        var file = "";
        //        $http.get(imagedownload, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);
        //                $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //};

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
                "HRME_Id": 1
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

        $scope.DeleteDocumentDataOrientation = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordOrientation", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //ORIENTATION

        //Examination
        $scope.addNewExam = function () {
            var newItemNo = $scope.examinationList.length + 1;
            if (newItemNo <= 30) {
                $scope.examinationList.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewExam = function (index, data) {
            var newItemNo = $scope.examinationList.length - 1;
            $scope.examinationList.splice(index, 1);
            if (data.hreorcO_Id > 0) {
                $scope.DeleteDocumentDataExamination(data);
            }
        };

        $scope.submitted16 = false;
        $scope.validateExamDetails = function () {
            if ($scope.myForm16.$valid) {
                var data = {
                    "HR_Employee_ExamDutyDetailsArrayDTO": $scope.examinationList,
                    "TabName": "ExamintionTab",
                    "Type": "Employee"
                };
                apiService.create("naacHrmsDetails/empSaveData", data).
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
            else {
                $scope.submitted16 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.clear_exam_tab = function () {
            $scope.examinationList = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted16 = false;
            $scope.myForm16.$setPristine();
            $scope.myForm16.$setUntouched();
        };

        $scope.DeleteDocumentDataExamination = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordExamination", data).
                            then(function (promise) {
                                debugger;
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //Examination

        //STUDENT ACTIVITY
        $scope.addNewDocumentStuActivity = function () {
            var newItemNo = $scope.documentListStuActivity.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListStuActivity.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentStuActivity = function (index, data) {
            var newItemNo = $scope.documentListStuActivity.length - 1;
            $scope.documentListStuActivity.splice(index, 1);
            if (data.hresacT_Id > 0) {
                $scope.DeleteDocumentDataStuActivity(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentStuActivity(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', data.hresacT_Document);
            }
            else if (extention === "pdf") {
                var imagedownload = data.hresacT_Document;
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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_StudentActivitiesArrayDTO": $scope.documentListStuActivity,
                        "TabName": "StudentActivityTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted3 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsStuActivity = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListStuActivity.length; k++) {
                var arryind = $scope.documentListStuActivity.indexOf($scope.documentListStuActivity[k]);
                if (arryind !== index) {
                    if ($scope.documentListStuActivity[k].hresacT_Document === user.hresacT_Document && $scope.documentListStuActivity[k].hresacT_Document !== null)
                    {
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
                "HRME_Id": 1
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

        $scope.DeleteDocumentDataStuActivity = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordStuActivity", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //STUDENT ACTIVITY

        //PROFESSIONAL ACTIVITY
        $scope.addNewDocumentProfActivity = function () {
            debugger;
            var newItemNo = $scope.documentListProfActivity.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListProfActivity.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentProfActivity = function (index, data) {
            debugger;
            var newItemNo = $scope.documentListProfActivity.length - 1;
            $scope.documentListProfActivity.splice(index, 1);
            if (data.hredacT_Id > 0) {
                $scope.DeleteDocumentDataProfActivity(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentProfActivity(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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
                        data.hredacT_Document = d;
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
            debugger;
            console.log(data);
            $('#preview').removeAttr('src');
            var filename = data.filepath.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            debugger;
            if (extention === "jpg" || extention === "jpeg") {
                $('#preview').attr('src', data.filepath);
            }
            else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', data.filepath);
            }
            else if (extention === "pdf") {
                $('#showpdf').modal('hide');
                var imagedownload = "";
                imagedownload = data.filepath;
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        debugger;
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        pdfId = document.getElementById("pdfId");
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
        };
        $scope.submitted4 = false;
        $scope.validateProfActivityDetails = function () {
            if ($scope.myForm4.$valid) {
                var duplicateda = false;
                angular.forEach($scope.documentListProfActivity, function (value, key) {
                    debugger;
                    if ($scope.chkdup_documentsDetailsProfActivity($scope.documentListProfActivity[key], key)) {
                        duplicateda = true;
                        return;
                    }
                });
                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_DevActivitiesArrayDTO": $scope.documentListProfActivity,
                        "TabName": "ProfessionalActivityTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted4 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsProfActivity = function (user, index) {
            debugger;
            var duplicate = false;
            for (var k = 0; k < $scope.documentListProfActivity.length; k++) {
                var arryind = $scope.documentListProfActivity.indexOf($scope.documentListProfActivity[k]);
                if (arryind !== index) {
                    if ($scope.documentListProfActivity[k].hredacT_Document === user.hredacT_Document && $scope.documentListProfActivity[k].hredacT_Document !== null)
                    {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };

        $scope.clear_ProfActivity_tab = function () {
            debugger;
            $scope.documentListProfActivity = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
        };

        $scope.onLoadGetDataProfessionalActivity = function () {
            $scope.clear_ProfActivity_tab();
            var data = {
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getProfessionalActivitydata", data).then(function (promise) {
                debugger;
                if (promise.professionalActivitylist !== null && promise.professionalActivitylist.length > 0) {
                    $scope.documentListProfActivity = promise.professionalActivitylist;
                    angular.forEach(promise.professionalActivitylist, function (value, key) {
                        if (value.hredacT_ActivityDate !== null) {
                            $scope.documentListProfActivity[key].hredacT_ActivityDate = new Date(value.hredacT_ActivityDate);
                        }
                        else {
                            $scope.documentListProfActivity[key].hredacT_ActivityDate = null;
                        }
                    });
                }
            });
        };

        $scope.DeleteDocumentDataProfActivity = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordProfActivity", data).
                            then(function (promise) {
                                debugger;
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //PROFESSIONAL ACTIVITY

        //RESEARCH PROJECT
        $scope.addNewDocumentResProject = function () {
            var newItemNo = $scope.documentListResProject.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListResProject.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentResProject = function (index, data) {
            var newItemNo = $scope.documentListResProject.length - 1;
            $scope.documentListResProject.splice(index, 1);
            if (data.hrerepR_Id > 0) {
                $scope.DeleteDocumentDataResProj(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentResProject(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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

        //$scope.showmodaldetailsResProject = function (data) {
        //    $('#preview').removeAttr('src');
        //    var filename = data.hrerepR_Document.toString();
        //    var nameArray = filename.split('.');
        //    var extention = nameArray[nameArray.length - 1];
        //    if (extention === "jpg" || extention === "jpeg") {
        //        $('#preview').attr('src', data.hrerepR_Document);
        //    }
        //    else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
        //        //$('#preview').removeAttr('src');
        //        $('#preview').attr('src', data.hrerepR_Document);
        //    }
        //    else if (extention === "pdf") {
        //        var imagedownload = data.hrerepR_Document;
        //        $scope.content = "";
        //        var fileURL = "";
        //        var file = "";
        //        $http.get(imagedownload, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);
        //                $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //};

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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_ResearchProjectsArrayDTO": $scope.documentListResProject,
                        "TabName": "ResearchProjectTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
                        then(function (promise) {
                            debugger;
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
                $scope.submitted5 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsResProject = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListResProject.length; k++) {
                var arryind = $scope.documentListResProject.indexOf($scope.documentListResProject[k]);
                if (arryind !== index) {
                    if ($scope.documentListResProject[k].hrerepR_Document === user.hrerepR_Document && $scope.documentListResProject[k].hrerepR_Document !== null)
                    {
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
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getResearchProjectdata", data).then(function (promise) {
                if (promise.researchProjectlist !== null && promise.researchProjectlist.length > 0) {
                    $scope.documentListResProject = promise.researchProjectlist;
                }
            });
        };

        $scope.DeleteDocumentDataResProj = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordResProj", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //RESEARCH PROJECT

        //RESEARCH GUIDANCE
        $scope.addNewDocumentResGuidance = function () {
            var newItemNo = $scope.documentListResGuidance.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListResGuidance.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentResGuidance = function (index, data) {
            var newItemNo = $scope.documentListResGuidance.length - 1;
            $scope.documentListResGuidance.splice(index, 1);
            if (data.hreregU_Id > 0) {
                $scope.DeleteDocumentDataResGuide(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentResGuidance(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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

        //$scope.showmodaldetailsResGuidance = function (data) {
        //    $('#preview').removeAttr('src');
        //    var filename = data.hreregU_Document.toString();
        //    var nameArray = filename.split('.');
        //    var extention = nameArray[nameArray.length - 1];
        //    if (extention === "jpg" || extention === "jpeg") {
        //        $('#preview').attr('src', data.hreregU_Document);
        //    }
        //    else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
        //        //$('#preview').removeAttr('src');
        //        $('#preview').attr('src', data.hreregU_Document);
        //    }
        //    else if (extention === "pdf") {
        //        var imagedownload = data.hreregU_Document;
        //        $scope.content = "";
        //        var fileURL = "";
        //        var file = "";
        //        $http.get(imagedownload, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);
        //                $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //};

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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_ResearchGuidanceArrayDTO": $scope.documentListResGuidance,
                        "TabName": "ResearchGuidanceTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted6 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsResGuidance = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListResGuidance.length; k++) {
                var arryind = $scope.documentListResGuidance.indexOf($scope.documentListResGuidance[k]);
                if (arryind !== index) {
                    if ($scope.documentListResGuidance[k].hrerepR_Document === user.hrerepR_Document && $scope.documentListResGuidance[k].hrerepR_Document !== null)
                    {
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
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getResearchGuidedata", data).then(function (promise) {
                if (promise.researchGuidelist !== null && promise.researchGuidelist.length > 0) {
                    $scope.documentListResGuidance = promise.researchGuidelist;
                }
            });
        };

        $scope.DeleteDocumentDataResGuide = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordResGuide", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //RESEARCH GUIDANCE

        //BOSBOE
        $scope.addNewDocumentBOSBOE = function () {
            var newItemNo = $scope.documentListBOSBOE.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListBOSBOE.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentBOSBOE = function (index, data) {
            var newItemNo = $scope.documentListBOSBOE.length - 1;
            $scope.documentListBOSBOE.splice(index, 1);
            if (data.hreboS_Id > 0) {
                $scope.DeleteDocumentDataBOSBOE(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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

        //$scope.showmodaldetailsBOSBOE = function (data) {
        //    $('#preview').removeAttr('src');
        //    var filename = data.hreboS_Document.toString();
        //    var nameArray = filename.split('.');
        //    var extention = nameArray[nameArray.length - 1];
        //    if (extention === "jpg" || extention === "jpeg") {
        //        $('#preview').attr('src', data.hreboS_Document);
        //    }
        //    else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
        //        //$('#preview').removeAttr('src');
        //        $('#preview').attr('src', data.hreboS_Document);
        //    }
        //    else if (extention === "pdf") {
        //        var imagedownload = data.hreboS_Document;
        //        $scope.content = "";
        //        var fileURL = "";
        //        var file = "";
        //        $http.get(imagedownload, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);
        //                $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //};

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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_BOSBOEArrayDTO": $scope.documentListBOSBOE,
                        "TabName": "BOSBOETab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted7 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsBOSBOE = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListBOSBOE.length; k++) {
                var arryind = $scope.documentListBOSBOE.indexOf($scope.documentListBOSBOE[k]);
                if (arryind !== index) {
                    if ($scope.documentListBOSBOE[k].hreboS_Document === user.hreboS_Document && $scope.documentListBOSBOE[k].hreboS_Document !== null)
                    {
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
                "HRME_Id": 1
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

        $scope.DeleteDocumentDataBOSBOE = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordBOSBOE", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //BOSBOE

        //JOURNALS
        $scope.addNewDocumentJournal = function () {
            var newItemNo = $scope.documentListRefJournal.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListRefJournal.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentJournal = function (index, data) {
            var newItemNo = $scope.documentListRefJournal.length - 1;
            $scope.documentListRefJournal.splice(index, 1);
            if (data.hrejornL_Id > 0) {
                $scope.DeleteDocumentDataJournal(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentJournal(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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

        //$scope.showmodaldetailsJournal = function (data) {
        //    $('#preview').removeAttr('src');
        //    var filename = data.hrejornL_Document.toString();
        //    var nameArray = filename.split('.');
        //    var extention = nameArray[nameArray.length - 1];
        //    if (extention === "jpg" || extention === "jpeg") {
        //        $('#preview').attr('src', data.hrejornL_Document);
        //    }
        //    else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
        //        //$('#preview').removeAttr('src');
        //        $('#preview').attr('src', data.hrejornL_Document);
        //    }
        //    else if (extention === "pdf") {
        //        var imagedownload = data.hrejornL_Document;
        //        $scope.content = "";
        //        var fileURL = "";
        //        var file = "";
        //        $http.get(imagedownload, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);
        //                $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //};

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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_JournalArrayDTO": $scope.documentListRefJournal,
                        "TabName": "JournalTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted8 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsJournal = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListRefJournal.length; k++) {
                var arryind = $scope.documentListRefJournal.indexOf($scope.documentListRefJournal[k]);
                if (arryind !== index) {
                    if ($scope.documentListRefJournal[k].hrejornL_Document === user.hrejornL_Document && $scope.documentListRefJournal[k].hrejornL_Document !== null)
                    {
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
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getJournaldata", data).then(function (promise) {
                if (promise.journallist !== null && promise.journallist.length > 0) {
                    $scope.documentListRefJournal = promise.journallist;
                }
            });
        };

        $scope.DeleteDocumentDataJournal = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordJournal", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //JOURNALS

        //CONFERENCE
        $scope.addNewDocumentConference = function () {
            var newItemNo = $scope.documentListConference.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListConference.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentConference = function (index, data) {
            var newItemNo = $scope.documentListConference.length - 1;
            $scope.documentListConference.splice(index, 1);
            if (data.hreconF_Id > 0) {
                $scope.DeleteDocumentDataConference(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentConference(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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

        //$scope.showmodaldetailsConference = function (data) {
        //    $('#preview').removeAttr('src');
        //    var filename = data.hreconF_Document.toString();
        //    var nameArray = filename.split('.');
        //    var extention = nameArray[nameArray.length - 1];
        //    if (extention === "jpg" || extention === "jpeg") {
        //        $('#preview').attr('src', data.hreconF_Document);
        //    }
        //    else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
        //        //$('#preview').removeAttr('src');
        //        $('#preview').attr('src', data.hreconF_Document);
        //    }
        //    else if (extention === "pdf") {
        //        var imagedownload = data.hreconF_Document;
        //        $scope.content = "";
        //        var fileURL = "";
        //        var file = "";
        //        $http.get(imagedownload, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);
        //                $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //};

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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_ConferenceArrayDTO": $scope.documentListConference,
                        "TabName": "ConferenceTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted9 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsConference = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListConference.length; k++) {
                var arryind = $scope.documentListConference.indexOf($scope.documentListConference[k]);
                if (arryind !== index) {
                    if ($scope.documentListConference[k].hreconF_Document === user.hreconF_Document && $scope.documentListConference[k].hreconF_Document !== null)
                    {
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
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getConferencedata", data).then(function (promise) {
                if (promise.conferencelist !== null && promise.conferencelist.length > 0) {
                    $scope.documentListConference = promise.conferencelist;
                }
            });
        };

        $scope.DeleteDocumentDataConference = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordConference", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //CONFERENCE

        //BOOK
        $scope.addNewDocumentBook = function () {
            var newItemNo = $scope.documentListBook.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListBook.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentBook = function (index, data) {
            var newItemNo = $scope.documentListBook.length - 1;
            $scope.documentListBook.splice(index, 1);
            if (data.hrebK_Id > 0) {
                $scope.DeleteDocumentDataBook(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBook(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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
                $('#preview').attr('src', data.hrebK_Document);
            }
            else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', data.hrebK_Document);
            }
            else if (extention === "pdf") {
                var imagedownload = data.hrebK_Document;
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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_BookArrayDTO": $scope.documentListBook,
                        "TabName": "BookTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted10 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsBook = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListBook.length; k++) {
                var arryind = $scope.documentListBook.indexOf($scope.documentListBook[k]);
                if (arryind !== index) {
                    if ($scope.documentListBook[k].hrebK_Document === user.hrebK_Document && $scope.documentListBook[k].hrebK_Document !== null)
                    {
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
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getBookdata", data).then(function (promise) {
                if (promise.booklist !== null && promise.booklist.length > 0) {
                    $scope.documentListBook = promise.booklist;
                    angular.forEach($scope.documentListBook, function (doc) {
                        doc.filepath = doc.hrebK_Document;
                    });
                }
            });
        };

        $scope.DeleteDocumentDataBook = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordBook", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //BOOK

        //BOOK CHAPTER
        $scope.addNewDocumentBookChapter = function () {
            var newItemNo = $scope.documentListBookChapter.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListBookChapter.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentBookChapter = function (index, data) {
            var newItemNo = $scope.documentListBookChapter.length - 1;
            $scope.documentListBookChapter.splice(index, 1);
            if (data.hrebkcP_Id > 0) {
                $scope.DeleteDocumentDataBookChapter(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBookChapter(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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

        //$scope.showmodaldetailsBookChapter = function (data) {
        //    $('#preview').removeAttr('src');
        //    var filename = data.hrebkcP_Document.toString();
        //    var nameArray = filename.split('.');
        //    var extention = nameArray[nameArray.length - 1];
        //    if (extention === "jpg" || extention === "jpeg") {
        //        $('#preview').attr('src', data.hrebkcP_Document);
        //    }
        //    else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
        //        //$('#preview').removeAttr('src');
        //        $('#preview').attr('src', data.hrebkcP_Document);
        //    }
        //    else if (extention === "pdf") {
        //        var imagedownload = data.hrebkcP_Document;
        //        $scope.content = "";
        //        var fileURL = "";
        //        var file = "";
        //        $http.get(imagedownload, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);
        //                $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //};

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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_BookChapterArrayDTO": $scope.documentListBookChapter,
                        "TabName": "BookChapterTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted11 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsBookChapter = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListBookChapter.length; k++) {
                var arryind = $scope.documentListBookChapter.indexOf($scope.documentListBookChapter[k]);
                if (arryind !== index) {
                    if ($scope.documentListBookChapter[k].hrebkcP_Document === user.hrebkcP_Document && $scope.documentListBookChapter[k].hrebkcP_Document !== null)
                    {
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
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getBookChapterdata", data).then(function (promise) {
                if (promise.bookChapterlist !== null && promise.bookChapterlist.length > 0) {
                    $scope.documentListBookChapter = promise.bookChapterlist;
                }
            });
        };

        $scope.DeleteDocumentDataBookChapter = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordBookChapter", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //BOOK CHAPTER


        //Commettee
        $scope.addNewDocumentCommettee = function () {
            var newItemNo = $scope.documentListCommettee.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListCommettee.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentCommettee = function (index, data) {
            var newItemNo = $scope.documentListCommettee.length - 1;
            $scope.documentListCommettee.splice(index, 1);
            if (data.hrecoM_Id > 0) {
                $scope.DeleteDocumentDataCommettee(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentCommettee(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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
                $('#preview').attr('src', data.hrecoM_Document);
            }
            else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', data.hrecoM_Document);
            }
            else if (extention === "pdf") {
                var imagedownload = data.hrecoM_Document;
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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_CommitteeArrayDTO": $scope.documentListCommettee,
                        "TabName": "CommetteeTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted12 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsCommettee = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListCommettee.length; k++) {
                var arryind = $scope.documentListCommettee.indexOf($scope.documentListCommettee[k]);
                if (arryind !== index) {
                    if ($scope.documentListCommettee[k].hrecoM_Document === user.hrecoM_Document && $scope.documentListCommettee[k].hrecoM_Document !== null)
                    {
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
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getCommetteedata", data).then(function (promise) {
                if (promise.commetteelist !== null && promise.commetteelist.length > 0) {
                    $scope.documentListCommettee = promise.commetteelist;
                }
            });
        };

        $scope.DeleteDocumentDataCommettee = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordCommettee", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //Commettee

        //OTHER DETAILS
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
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
                document.extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "JPG" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentOtherDetail(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "JPG" && extention !== "pdf") {
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

        //$scope.showmodaldetailsOtherDetail = function (data) {
        //    $('#preview').removeAttr('src');
        //    var filename = data.hreothdeT_Document.toString();
        //    var nameArray = filename.split('.');
        //    var extention = nameArray[nameArray.length - 1];
        //    if (extention === "jpg" || extention === "jpeg") {
        //        $('#preview').attr('src', data.hreothdeT_Document);
        //    }
        //    else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
        //        //$('#preview').removeAttr('src');
        //        $('#preview').attr('src', data.hreothdeT_Document);
        //    }
        //    else if (extention === "pdf") {
        //        var imagedownload = data.hreothdeT_Document;
        //        $scope.content = "";
        //        var fileURL = "";
        //        var file = "";
        //        $http.get(imagedownload, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);
        //                $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //};

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

                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        "HR_Employee_OtherDetailsArrayDTO": $scope.documentListOtherDetails,
                        "TabName": "OtherDetailTab",
                        "Type": "Employee"
                    };
                    apiService.create("naacHrmsDetails/empSaveData", data).
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
                $scope.submitted13 = true;
                $scope.Otherdetails = true;
            }
        };

        $scope.chkdup_documentsDetailsOtherDetail = function (user, index) {
            var duplicate = false;
            for (var k = 0; k < $scope.documentListOtherDetails.length; k++) {
                var arryind = $scope.documentListOtherDetails.indexOf($scope.documentListOtherDetails[k]);
                if (arryind !== index) {
                    if ($scope.documentListOtherDetails[k].hreothdeT_Document === user.hreothdeT_Document && $scope.documentListOtherDetails[k].hreothdeT_Document !== null)
                    {
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
                "HRME_Id": 1
            };

            apiService.create("naacHrmsDetails/getOtherDetaildata", data).then(function (promise) {
                if (promise.otherDetailSlist !== null && promise.otherDetailSlist.length > 0) {
                    $scope.documentListOtherDetails = promise.otherDetailSlist;
                }
            });
        };

        $scope.DeleteDocumentDataOthers = function (data, SweetAlert) {
            var mgs = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee NAAC Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("naacHrmsDetails/DeleteDocumentRecordOthers", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else {
                                        swal("Record Not Deleted", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
        //OTHER DETAILS

        //GROUP A EXAM
        $scope.GetGroupAExam = function (groupa) {
            $scope.examSelectedAll = $scope.documentListGroupADetails.every(function (itm) {
                return itm.selected;
            });

            if (groupa.selected) {
                groupa.disable = false;
            }
            else {
                groupa.disable = true;
            }
        };

        $scope.submitted14 = false;
        $scope.validateGroupADetails = function () {
            if ($scope.myForm14.$valid) {
                var examselected = [];
                angular.forEach($scope.documentListGroupADetails, function (itm) {
                    if (itm.selected) {
                        examselected.push(itm);
                    }
                });

                if (examselected.length === 0) {
                    swal('Kindly select atleast one record');
                    return;
                }

                var data = {
                    "HR_Employee_GroupADetailsArrayDTO": examselected,
                    "TabName": "GroupADetailTab",
                    "Type": "Employee"
                };
                apiService.create("naacHrmsDetails/empSaveData", data).
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
            else {
                $scope.submitted14 = true;
                $scope.Otherdetails = true;
            }
        };
        //GROUP A EXAM

        //GROUP B EXAM
        $scope.GetGroupBExam = function (groupb) {
            $scope.examSelectedAll = $scope.documentListGroupBDetails.every(function (itm) {
                return itm.selected;
            });

            if (groupb.selected) {
                groupb.disable = false;
            }
            else {
                groupb.disable = true;
            }
        };

        $scope.submitted15 = false;
        $scope.validateGroupBDetails = function () {
            if ($scope.myForm15.$valid) {
                var examselected = [];
                angular.forEach($scope.documentListGroupBDetails, function (itm) {
                    if (itm.selected) {
                        examselected.push(itm);
                    }
                });

                if (examselected.length === 0) {
                    swal('Kindly select atleast one record');
                    return;
                }

                var data = {
                    "HR_Employee_GroupBDetailsArrayDTO": examselected,
                    "TabName": "GroupBDetailTab",
                    "Type": "Employee"
                };
                apiService.create("naacHrmsDetails/empSaveData", data).
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
            else {
                $scope.submitted15 = true;
                $scope.Otherdetails = true;
            }
        };
        //GROUP B EXAM

        $scope.CalculateExperience = function (orient) {
            orient.hreorcO_Duration = "";

            var joindate = new Date($filter('date')(new Date(orient.hreorcO_From).toDateString(), "yyyy/MM/dd"));
            var leftdate = new Date($filter('date')(new Date(orient.hreorcO_To).toDateString(), "yyyy/MM/dd"));

            var exp = $scope.CalDate(leftdate, joindate);
            orient.hreorcO_Duration = exp + 1;
        };

        $scope.CalculateDuration = function (conference) {
            conference.hreconF_Duration = "";

            var joindate = new Date($filter('date')(new Date(conference.hreconF_Fromdate).toDateString(), "yyyy/MM/dd"));
            var leftdate = new Date($filter('date')(new Date(conference.hreconF_Todate).toDateString(), "yyyy/MM/dd"));

            var exp = $scope.CalDate(leftdate, joindate);
            conference.hreconF_Duration = exp + 1;
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

    }
})();
