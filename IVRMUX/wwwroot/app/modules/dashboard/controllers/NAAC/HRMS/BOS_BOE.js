(function () {
    'use strict';
    angular
        .module('app')
        .controller('naacBos_BoeController', naacBos_BoeController);

    naacBos_BoeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$q', '$sce'];
    function naacBos_BoeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $q, $sce) {

        $scope.materaldocuupload = [{ id: 'Materal1' }];

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
            apiService.getURI("naacHrmsDetailsmultifile/getdetails", pageid).then(function (promise) {

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

                if (promise.academicyearlist !== null && promise.academicyearlist.length > 0) {
                    $scope.academicyearlist = promise.academicyearlist;
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
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
            apiService.create("naacHrmsDetailsmultifile/get_depts", data).
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
            apiService.create("naacHrmsDetailsmultifile/get_desig", data).
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

                apiService.create("naacHrmsDetailsmultifile/get_Employe_ob", data).
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
            apiService.create("naacHrmsDetailsmultifile/get_EmployeALLDATA", data).
                then(function (promise) {
                    if (promise.bosboElist !== null && promise.bosboElist.length > 0) {
                        $scope.documentListBOSBOEGrid = promise.bosboElist;
                        angular.forEach(promise.bosboElist, function (value, key) {
                            if (value.hreboS_FromToDate !== null) {
                                $scope.documentListBOSBOEGrid[key].hreboS_FromToDate = new Date(value.hreboS_FromToDate);
                            }
                            else {
                                $scope.documentListBOSBOEGrid[key].hreboS_FromToDate = null;
                            }

                            if (value.hreboS_Document !== null && value.hreboS_Document !== undefined) {
                                var filename = value.hreboS_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.documentListBOSBOEGrid[key].extention = nameArray[nameArray.length - 1];
                            }
                        });
                        $scope.gridOptions.data = $scope.documentListBOSBOEGrid;
                    }
                    else {
                        swal("No Record Found !!");
                    }
                });
        };
        //BOSBOE
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hreboS_Subject', displayName: 'Subject', enableHiding: false },
                { name: 'hreboS_BOSBOEFlg', displayName: 'BOS/BOE', enableHiding: false },
                { name: 'hreboS_UnvCollegeFlg', displayName: 'Univ/College', enableHiding: false },
                { name: 'hreboS_FromToDate', displayName: 'From Date', enableHiding: false },
                { name: 'hreboS_ToDate', displayName: 'To Date', enableHiding: false },
                { name: 'hreboS_Role', displayName: 'Role', enableHiding: false },
                { name: 'hreboS_Year', displayName: 'Year', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" data-toggle="modal" data-target="#popup11" title="View Remark" data-backdrop="static" ng-click="grid.appScope.viewdocument(row.entity);"> <i class="fa fa-eye text-blue"></i></a>' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"  data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hreboS_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hreboS_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.EditData = function (record) {
            var data = {
                "HREBOS_Id": record.hreboS_Id,
                "TabName": "BOSBOETab"
            };
            apiService.create("naacHrmsDetailsmultifile/editRecord", data).
                then(function (promise) {
                    if (promise.bosboElistedit != null && promise.bosboElistedit.length > 0) {
                        $scope.bosboe = promise.bosboElistedit[0];
                        angular.forEach(promise.bosboElistedit, function (value, key) {
                            if (value.hreboS_FromToDate !== null) {
                                $scope.bosboe.hreboS_FromToDate = new Date(value.hreboS_FromToDate);
                            }
                            else {
                                $scope.bosboe.hreboS_FromToDate = null;
                            }

                            if (value.hreboS_ToDate !== null) {
                                $scope.bosboe.hreboS_ToDate = new Date(value.hreboS_ToDate);
                            }
                            else {
                                $scope.bosboe.hreboS_ToDate = null;
                            }

                            if ($scope.bosboe.hreboS_Document !== null && $scope.bosboe.hreboS_Document !== undefined) {
                                var filename = $scope.bosboe.hreboS_Document.toString();
                                var nameArray = filename.split('.');
                                $scope.bosboe.extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }

                    if (promise.bosboEfilelistedit != null && promise.bosboEfilelistedit.length > 0) {
                        $scope.documentListBOSBOE = promise.bosboEfilelistedit;
                        angular.forEach($scope.documentListBOSBOE, function (value) {
                            if (value.nchrebosF_FilePath !== null && value.nchrebosF_FilePath !== undefined) {
                                var filename = value.nchrebosF_FilePath.toString();
                                var nameArray = filename.split('.');
                                value.extention = nameArray[nameArray.length - 1];
                            }
                        });
                    }
                });
        };

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
                document.nchrebosF_FileName = filename;
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
                        data.nchrebosF_FilePath = d;
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
            var filename = data.nchrebosF_FilePath.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg" || extention === "JPEG") {
                $('#preview').attr('src', data.nchrebosF_FilePath);
            }
            else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', data.nchrebosF_FilePath);
            }
            else if (extention === "pdf") {
                var imagedownload = data.nchrebosF_FilePath;
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

        $scope.submitted7 = false;
        $scope.validateBOSBOEDetails = function () {
            if ($scope.myForm7.$valid) {
                $scope.materaldocuupload22 = [];
                if ($scope.documentListBOSBOE.length > 0) {
                    angular.forEach($scope.documentListBOSBOE, function (val) {
                        if (val.nchrebosF_FilePath !== null && val.nchrebosF_FilePath !== undefined) {
                            $scope.materaldocuupload22.push(val);
                        }
                    });
                }

                //$scope.documentListBOSBOE = [];
                //$scope.documentListBOSBOE.push($scope.bosboe);
                //var duplicateda = false;
                //if ($scope.bosboe.hreboS_Id == null || $scope.bosboe.hreboS_Id == undefined) {
                //    angular.forEach($scope.documentListBOSBOE, function (value, key) {
                //        if ($scope.chkdup_documentsDetailsBOSBOE($scope.documentListBOSBOE[key], key)) {
                //            duplicateda = true;
                //            return;
                //        }
                //    });
                //}

                if ($scope.HRME_IDSelected !== null && $scope.HRME_IDSelected > 0) {
                    var data = {
                        "HR_Employee_BOSBOEDTO": $scope.bosboe,
                        "HRME_Id": $scope.HRME_IDSelected,
                        "HR_Employee_BOSBOE_FilesArrayDTO": $scope.materaldocuupload22,
                        "TabName": "BOSBOETab",
                        "Type": "All"
                    };
                    apiService.create("naacHrmsDetailsmultifile/SaveData", data).
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
                                } else if (promise.retrunMsg === "Approved") {
                                    swal("Can't Update Approved Record!");
                                    $scope.Otherdetails = false;
                                    $state.reload();
                                } else {
                                    swal("Something went wrong ..!", 'Kindly contact Administrator');
                                    return;
                                }
                            }
                        });
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
                    if ($scope.documentListBOSBOE[k].hreboS_Document === user.hreboS_Document && $scope.documentListBOSBOE[k].hreboS_Document !== null) {
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
            $scope.bosboe = "";
            $scope.myForm7.$setPristine();
            $scope.myForm7.$setUntouched();
        };

        $scope.onLoadGetBOSBOE = function () {
            $scope.clear_BOSBOE_tab();
            var data = {
                "HRME_Id": $scope.HRME_IDSelected
            };

            apiService.create("naacHrmsDetailsmultifile/getBOSBOEdata", data).then(function (promise) {
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
                        apiService.create("naacHrmsDetailsmultifile/DeleteDocumentRecordBOSBOE", data).
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
                    apiService.create("naacHrmsDetailsmultifile/SaveData", data).
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

            apiService.create("naacHrmsDetailsmultifile/getOrientdata", data).then(function (promise) {
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
                        apiService.create("naacHrmsDetailsmultifile/DeleteDocumentRecordOrientation", data).
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

        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = { "HREBOS_Id": obj.hreboS_Id};
            apiService.create("naacHrmsDetailsmultifile/viewfileremark", data).then(function (promise) {
                if (promise !== null) {
                    $scope.documentCommentlist = promise.documentCommentlist;
                    if (promise.documentCommentlist !== null && promise.documentCommentlist.length > 0) {

                    } else {
                        $('#popup11').modal('hide');
                        swal("No Remarks Found");
                    }
                }
            });
        };

        $scope.viewfilecomment = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = { "NCHREBOSF_Id": obj.nchrebosF_Id };
            apiService.create("naacHrmsDetailsmultifile/viewsubfileremark", data).then(function (promise) {
                if (promise !== null) {
                    $scope.documentsubCommentlist = promise.documentsubCommentlist;
                    if (promise.documentsubCommentlist !== null && promise.documentsubCommentlist.length > 0) {

                    } else {
                        $('#popup22').modal('hide');
                        swal("No Remarks Found");
                    }
                }
            });
        };
    }
})();