﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('naacBookChapterController', naacBookChapterController);

    naacBookChapterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$q', '$sce'];
    function naacBookChapterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $q, $sce) {

        $scope.onLoadGetDataEmployeeType = function () {
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

            $scope.obj = {};
            $scope.HRME_IDSelected = 0;
            var pageid = 2;
            apiService.getURI("naacHrmsDetails/getdetails", pageid).then(function (promise) {

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

        $scope.interacted11 = function (field) {
            return $scope.submitted11;
        };
        //employee
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

        $scope.GetEmployeeBygroupType = function (groupTypeselectedAll) {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;

            //angular.forEach($scope.groupTypedropdown, function (itm) {
            //    itm.selected = toggleStatus;
            //});

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

        $scope.GetEmployeeByDepartment = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            var toggleStatus = $scope.departmentselectedAll;

            //angular.forEach($scope.departmentdropdown, function (itm) {
            //    itm.selected = toggleStatus;
            //});

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
            apiService.create("naacHrmsDetails/get_depts", data).
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
            });
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
            apiService.create("naacHrmsDetails/get_desig", data).
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

                apiService.create("naacHrmsDetails/get_Employe_ob", data).
                    then(function (promise) {
                        $scope.employee = promise.get_emp;
                    });
            }
        };

        $scope.clear = function () {
            $scope.employee = [];
            $scope.obj = {};
            $scope.HRME_IDSelected = 0;
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.GetEmployeeBygroupTypeAll(false);
        };

        $scope.GetEmployeeSelected = function () {
            if ($scope.myForm1.$valid) {
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
        };

        $scope.emp_Sel = function (hrme_id) {
            $scope.HRME_IDSelected = hrme_id.hrmE_Id;

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
                "HRME_Id": $scope.HRME_IDSelected,
                "Type": "All"
            };
            apiService.create("naacHrmsDetails/get_EmployeALLDATA", data).
                then(function (promise) {

                    if (promise.bookChapterlist !== null && promise.bookChapterlist.length > 0) {
                        $scope.documentListBookChapter = promise.bookChapterlist;
                        angular.forEach(promise.bookChapterlist, function (value, key) {
                            if (value.hrebkcP_Document !== null && value.hrebkcP_Document !== undefined) {
                                var filename = value.hrebkcP_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListBookChapter[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                        $scope.gridOptions.data = $scope.documentListBookChapter;
                    }
                    else {
                        swal("No Record Found !!");
                    }
                });
        };
        //BOOK CHAPTER
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrebkcP_BookName', displayName: 'Book Name', enableHiding: false },
                { name: 'hrebkcP_Author', displayName: 'Author', enableHiding: false },
                { name: 'hrebkcP_Title', displayName: 'Title', enableHiding: false },
                { name: 'hrebkcP_PublisherName', displayName: 'Publisher Name', enableHiding: false },
                { name: 'hrebkcP_Editon', displayName: 'Editon', enableHiding: false },
                { name: 'hrebkcP_Place', displayName: 'Place', enableHiding: false },
                { name: 'hrebkcP_Year', displayName: 'Year', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"  data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrebkcP_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrebkcP_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.EditData = function (record) {
            var data = {
                "HREBKCP_Id": record.hrebkcP_Id,
                "TabName": "BookChapterTab"
            };
            apiService.create("naacHrmsDetails/editRecord", data).
                then(function (promise) {
                    if (promise.bookChapterlistedit != null && promise.bookChapterlistedit.length > 0) {
                        $scope.bookchapter = promise.bookChapterlistedit[0];
                        if ($scope.bookchapter.hrebkcP_Document !== null && $scope.bookchapter.hrebkcP_Document !== undefined) {
                            var filename = $scope.bookchapter.hrebkcP_Document.toString();
                            var nameArray = filename.split('.');
                            $scope.bookchapter.extention = nameArray[nameArray.length - 1];
                        }
                    }
                });
        };

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

        $scope.showmodaldetailsBookChapter = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrebkcP_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                $('#preview').attr('src', data.hrebkcP_Document);
            }
            else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', data.hrebkcP_Document);
            }
            else if (extention === "pdf") {
                var imagedownload = data.hrebkcP_Document;
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

        $scope.submitted11 = false;
        $scope.validateBookChapterDetails = function () {
            if ($scope.myForm11.$valid) {
                $scope.documentListBookChapter = [];
                $scope.documentListBookChapter.push($scope.bookchapter);
                var duplicateda = false;
                if ($scope.bookchapter.hrebkcP_Id == null || $scope.bookchapter.hrebkcP_Id == undefined) {
                    angular.forEach($scope.documentListBookChapter, function (value, key) {
                        if ($scope.chkdup_documentsDetailsBookChapter($scope.documentListBookChapter[key], key)) {
                            duplicateda = true;
                            return;
                        }
                    });
                }

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    if (duplicateda) {
                        return;
                    } else {
                        var data = {
                            "HR_Employee_BookChapterArrayDTO": $scope.documentListBookChapter,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "BookChapterTab",
                            "Type": "All"
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
                                        $state.reload();
                                    } else if (promise.retrunMsg === "Update") {
                                        swal("Record Updated Successfully..");
                                        $scope.Otherdetails = false;
                                        $state.reload();
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
                    if ($scope.documentListBookChapter[k].hrebkcP_Document === user.hrebkcP_Document && $scope.documentListBookChapter[k].hrebkcP_Document !== null) {
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
            $scope.bookchapter = "";
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
                        "TabName": "OrientationTab",
                        "Type": "All"
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
                    if ($scope.documentList[k].hreorcO_Document === user.hreorcO_Document && $scope.documentList[k].hreorcO_Document !== null) {
                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        break;
                    }
                }
            }
            return duplicate;
        };
        $scope.CalculateExperience = function (orient) {
            orient.hreorcO_Duration = "";

            var joindate = new Date($filter('date')(new Date(orient.hreorcO_From).toDateString(), "yyyy/MM/dd"));
            var leftdate = new Date($filter('date')(new Date(orient.hreorcO_To).toDateString(), "yyyy/MM/dd"));

            var exp = $scope.CalDate(leftdate, joindate);
            orient.hreorcO_Duration = exp + 1;
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
                $('#preview').attr('src', data.hreorcO_Document);
            }
            else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', data.hreorcO_Document);
            }
            else if (extention === "pdf") {
                var imagedownload = data.hreorcO_Document;
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
        $scope.goPrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };
    }
})();