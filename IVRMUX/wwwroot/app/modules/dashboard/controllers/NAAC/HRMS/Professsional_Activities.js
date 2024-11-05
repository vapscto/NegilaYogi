﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('naacProfessional_ActivitiesController', naacProfessional_ActivitiesController);

    naacProfessional_ActivitiesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$q', '$sce'];
    function naacProfessional_ActivitiesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $q, $sce) {

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

        $scope.interacted4 = function (field) {
            return $scope.submitted4;
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

                    if (promise.professionalActivitylist !== null && promise.professionalActivitylist.length > 0) {
                        $scope.documentListProfActivity = promise.professionalActivitylist;
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
                        $scope.gridOptions.data = $scope.documentListProfActivity;
                    }
                    else {
                        swal("No Record Found !!");
                    }
                });
        };
        //Examination
        //PROFESSIONAL ACTIVITY
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hredacT_DevelopmentActivity', displayName: 'Development Activity', enableHiding: false },
                { name: 'hredacT_ActivityDate', displayName: 'Date', enableHiding: false },
                { name: 'hredacT_OrgAgency', displayName: 'Org Agency', enableHiding: false },
                { name: 'hredacT_Place', displayName: 'Place', enableHiding: false },
                { name: 'hredacT_Duration', displayName: 'Duration', enableHiding: false },
                { name: 'hredacT_Year', displayName: 'Year', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"  data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hredacT_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hredacT_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.EditData = function (record) {
            var data = {
                "HREDACT_Id": record.hredacT_Id,
                "TabName": "ProfessionalActivityTab"
            };
            apiService.create("naacHrmsDetails/editRecord", data).
                then(function (promise) {
                    if (promise.professionalActivitylistedit != null && promise.professionalActivitylistedit.length > 0) {
                        $scope.profActivity = promise.professionalActivitylistedit[0];
                        if ($scope.profActivity.hredacT_Document !== null && $scope.profActivity.hredacT_Document !== undefined) {
                            var filename = $scope.profActivity.hredacT_Document.toString();
                            var nameArray = filename.split('.');
                            $scope.profActivity.extention = nameArray[nameArray.length - 1];
                        }
                    }
                });
        };

        $scope.addNewDocumentProfActivity = function () {
            var newItemNo = $scope.documentListProfActivity.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListProfActivity.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentProfActivity = function (index, data) {
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

        $scope.showmodaldetailsProfActivity = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hredacT_Document.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention === "jpg" || extention === "jpeg") {
                $('#preview').attr('src', data.hredacT_Document);
            }
            else if (extention === "doc" || extention === "docx" || extention === "xls" || extention === "xlsx") {
                $('#preview').attr('src', data.hredacT_Document);
            }
            else if (extention === "pdf") {
                var imagedownload = data.hredacT_Document;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([response], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');

                    });
            }
        };

        $scope.submitted4 = false;
        $scope.validateProfActivityDetails = function () {
            if ($scope.myForm4.$valid) {
                $scope.documentListProfActivity = [];
                $scope.documentListProfActivity.push($scope.profActivity);
                var duplicateda = false;
                if ($scope.profActivity.hredacT_Id == null || $scope.profActivity.hredacT_Id == undefined) {
                    angular.forEach($scope.documentListProfActivity, function (value, key) {
                        if ($scope.chkdup_documentsDetailsProfActivity($scope.documentListProfActivity[key], key)) {
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
                            "HR_Employee_DevActivitiesArrayDTO": $scope.documentListProfActivity,
                            "HRME_Id": $scope.HRME_IDSelected,
                            "TabName": "ProfessionalActivityTab",
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
                    if ($scope.documentListProfActivity[k].hredacT_Document === user.hredacT_Document && $scope.documentListProfActivity[k].hredacT_Document !== null) {
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
            $scope.profActivity = "";
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

        $scope.clear_orient_tab = function () {
            $scope.documentList = [{ id: 'document' }];
            $("#document").val("");
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };









    }
})();