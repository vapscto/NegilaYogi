(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsCandidateListController', vmsCandidateListController);

    vmsCandidateListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', 'dashboardService', 'uiGridExporterService', 'uiGridExporterConstants', '$q'];
    function vmsCandidateListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, uiCalendarConfig, superCache, dashboardService, uiGridExporterService, uiGridExporterConstants, $q) {

        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();


        $scope.viewby = 5;
        $scope.currentPage = 4;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5;
        $scope.jobedit = {};
        $scope.jobedit.hrcD_Id = 0;
        $scope.jobedit.mI_Id = 0;
        $scope.Editable = false;

        $scope.Qualification = true;
        $scope.Experiance = true;
        $scope.Family = true;
        $scope.Language = true;

        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 10,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrcD_FullName', displayName: 'NAME', enableHiding: false },
                { name: 'hrmJ_JobTiTle', displayName: 'JOB TITLE', enableHiding: false },
                { name: 'hrcD_ExpFrom', displayName: 'EXPERIENCE', enableHiding: false },
                { name: 'applydate', displayName: 'APPLICATION DATE', enableHiding: false },
                { name: 'hrcD_RecruitmentStatus', displayName: 'STATUS', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'ACTION', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true">Edit</i></a>' +
                        '</div>'
                }
            ],
            exporterCsvFilename: 'CandidateList.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.exportCsv = function () {
            var grid = $scope.gridApi.grid;
            var rowTypes = uiGridExporterConstants.ALL;
            var colTypes = uiGridExporterConstants.ALL;
            uiGridExporterService.csvExport(grid, rowTypes, colTypes);
        };

        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };

        $scope.pageChanged = function () {
            console.log('Page changed to: ' + $scope.currentPage);
        };

        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1;
        };

        $scope.addjob = function () {
            $state.go('app.addCandidate');
        };

        $scope.onLoadGetData = function () {
            $scope.Editable = false;
            $scope.taskDate = new Date($scope.today);
            var pageid = 2;
            apiService.getURI("CandidateListVMS/getalldetails", pageid).then(function (promise) {
                if (promise.vmsmrfList !== null && promise.vmsmrfList.length > 0) {
                    $scope.gridOptions.data = promise.vmsmrfList;
                    $scope.candidateslist = promise.vmsmrfList;

                    if (promise.masterPosTypeList !== null && promise.masterPosTypeList.length > 0) {
                        $scope.masterPosTypeList = promise.masterPosTypeList;
                    }

                    if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                        $scope.masterQualification = promise.masterQualification;
                    }

                    if (promise.masterGender !== null && promise.masterGender.length > 0) {
                        $scope.masterGender = promise.masterGender;
                    }

                    if (promise.masterjob !== null && promise.masterjob.length > 0) {
                        $scope.masterjob = promise.masterjob;
                    }

                    if (promise.mastercountry !== null && promise.mastercountry.length > 0) {
                        $scope.mastercountry = promise.mastercountry;
                    }

                    if (promise.masterReligion !== null && promise.masterReligion.length > 0) {
                        $scope.masterReligion = promise.masterReligion;
                    }

                    if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                        $scope.masterCaste = promise.masterCaste;
                    }

                    if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                        $scope.mastermaritalstatus = promise.mastermaritalstatus;
                    }

                    $scope.mrfCand = promise.vmsmrfList[0];
                }
            });
        };

        $scope.EditData = function (jobid) {
            $scope.jobedit.hrcD_Id = jobid.hrcD_Id;
            var pageid = $scope.jobedit.hrcD_Id;
            apiService.getURI("CandidateListVMS/editRecord", pageid).
                then(function (promise) {

                    $scope.qualificationDetails = [{ id: 'qualification' }];
                    $scope.experienceDetails = [{ id: 'experience' }];
                    $scope.familyDetails = [{ id: 'family' }];
                    $scope.languageDetails = [{ id: 'language' }];
                    $scope.myTabIndex = 0;
                    $('#blah').removeAttr('src');

                    $scope.mrfCand = promise.vmsEditValue[0];

                    if (promise.qualificationlist != null && promise.qualificationlist.length > 0) {
                        $scope.qualificationDetails = promise.qualificationlist;
                    }

                    if (promise.experiencelist != null && promise.experiencelist.length > 0) {
                        $scope.experienceDetails = promise.experiencelist;

                        $scope.EmployeeExperience = 1;
                        angular.forEach(promise.experiencelist, function (value, key) {

                            if (value.hrcexP_From != null) {
                                $scope.experienceDetails[key].hrcexP_From = new Date(value.hrcexP_From);
                            }
                            else {
                                $scope.experienceDetails[key].hrcexP_From = null;
                            }

                            if (value.hrcexP_To != null) {
                                $scope.experienceDetails[key].hrcexP_To = new Date(value.hrcexP_To);
                            }
                            else {
                                $scope.experienceDetails[key].hrcexP_To = null;
                            }
                        });
                    }
                    else { $scope.EmployeeExperience = 0; }

                    if (promise.familylist != null && promise.familylist.length > 0) {
                        $scope.familyDetails = promise.familylist;
                    }

                    if (promise.languagelist != null && promise.languagelist.length > 0) {
                        $scope.languageDetails = promise.languagelist;
                    }
                    
                    if (promise.vmsEditValue[0].hrcD_DOB !== null) {
                        $scope.mrfCand.hrcD_DOB = new Date(promise.vmsEditValue[0].hrcD_DOB);
                    }
                    else {
                        $scope.mrfCand.hrcD_DOB = null;
                    }

                    if (promise.vmsEditValue[0].hrcD_AppDate !== null) {
                        $scope.mrfCand.hrcD_AppDate = new Date(promise.vmsEditValue[0].hrcD_AppDate);
                    }
                    else {
                        $scope.mrfCand.hrcD_AppDate = null;
                    }

                    if (promise.vmsEditValue[0].hrcD_InterviewDate !== null) {
                        $scope.mrfCand.hrcD_InterviewDate = new Date(promise.vmsEditValue[0].hrcD_InterviewDate);
                    }
                    else {
                        $scope.mrfCand.hrcD_InterviewDate = null;
                    }

                    if (promise.vmsEditValue[0].hrcD_Photo != null && promise.vmsEditValue[0].hrcD_Photo != "") {
                        $('#blah').attr('src', promise.vmsEditValue[0].hrcD_Photo);
                    }

                    $scope.Editable = true;
                    $scope.Qualification = false;
                    $scope.Experiance = false;
                    $scope.Family = false;
                    $scope.Language = false;
                    //$state.go('app.vmsaddjob');
                });
        };

        $scope.cancel = function () {
            $scope.Editable = false;
            //$scope.onLoadGetData();
        };

        $scope.savejob = function () {
            if ($scope.myForm.$valid) {
                //$scope.mrfCand.hrmqC_RecruitmentStatus = "New";
                var data = $scope.mrfCand;
                apiService.create("AddCandidateVMS/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record Updated Successfully..");
                                $scope.jobedit.hrcD_Id = promise.hrcD_Id;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.jobedit.hrcD_Id = promise.hrcD_Id;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.jobedit.hrcD_Id = promise.hrcD_Id;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf" || extention === "PDF") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocument(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf" && extention !== "PDF") {
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

        function UploadEmployeeDocument(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Addcandidate", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        data.hrcD_Resume = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.UploadEmployeeProfilePic = [];
        $scope.uploadEmployeeProfilePic = function (input, document) {
            $scope.UploadEmployeeProfilePic = input.files;
            $('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofile(document);
                }
                else if (input.files[0].type != "image/jpeg") {
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
                    if (d != "PathNotFound") {
                        $scope.mrfCand.hrcD_Photo = d;
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

        $scope.submitted = false;
        $scope.submitted3 = false;
        $scope.submitted4 = false;
        $scope.submitted2 = false;
        $scope.submitted5 = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2 || field.$dirty;
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3 || field.$dirty;
        };

        $scope.interacted4 = function (field) {
            return $scope.submitted4 || field.$dirty;
        };

        $scope.interacted5 = function (field) {
            return $scope.submitted5 || field.$dirty;
        };

        $scope.qualificationDetails = [{ id: 'qualification' }];
        $scope.addNewQualification = function () {
            var newItemNo = $scope.qualificationDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.qualificationDetails.push({ 'id': 'qualification' + newItemNo });
            }
        };

        $scope.removeNewQualification = function (index, data) {
            var newItemNo = $scope.qualificationDetails.length - 1;
            $scope.qualificationDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };


        $scope.Qualification_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateQualificationDetails = function () {
            if ($scope.myForm3.$valid) {
                var data = {
                    Employeedto: $scope.jobedit,
                    QualificationsDTO: $scope.qualificationDetails
                };
                apiService.create("AddCandidateVMS/addQualification", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record Updated Successfully..");
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted3 = true;
            }
        };

        $scope.clear_third_tab = function (data) {
            $scope.qualificationDetails = [{ id: 'qualification' }];
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
        };

        $scope.experienceDetails = [{ id: 'experience' }];
        $scope.addNewExperience = function () {
            var newItemNo = $scope.experienceDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.experienceDetails.push({ 'id': 'experience' + newItemNo });
                $scope.experienceDetails[newItemNo - 1].exitDateDis = true;
            }
        };

        $scope.removeNewExperience = function (index, data) {
            var newItemNo = $scope.experienceDetails.length - 1;
            $scope.experienceDetails.splice(index, 1);
            if (data.hrmeE_Id > 0) {
                $scope.DeleteExperienceData(data);
            }
        };

        $scope.EmployeeExperience = 0;

        $scope.validateExperiencePrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateExperiencedetails = function () {
            if ($scope.myForm4.$valid) {
                var data = {
                    Employeedto: $scope.jobedit,
                    ExperienceDTO: $scope.experienceDetails
                };
                apiService.create("AddCandidateVMS/addexperience", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record Updated Successfully..");
                                $scope.Family = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Family = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Family = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted4 = true;
            }
        };

        $scope.clear_fourth_tab = function () {
            $scope.experienceDetails = [{ id: 'experience' }];
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
        };

        //FAMILY DETAILS
        $scope.familyDetails = [{ id: 'family' }];
        $scope.addNewfamily = function () {
            var newItemNo = $scope.familyDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.familyDetails.push({ 'id': 'family' + newItemNo });
            }
        };

        $scope.removeNewfamily = function (index, data) {
            var newItemNo = $scope.familyDetails.length - 1;
            $scope.familyDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };

        $scope.family_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validatefamilyDetails = function () {
            if ($scope.myForm2.$valid) {
                var data = {
                    Employeedto: $scope.jobedit,
                    FamilyDTO: $scope.familyDetails
                };
                apiService.create("AddCandidateVMS/addfamily", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record Updated Successfully..");
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.clear_second_tab = function (data) {
            $scope.familyDetails = [{ id: 'family' }];
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };
        //FAMILY DETAILS

        //LANGUAGE DETAILS
        $scope.languageDetails = [{ id: 'language' }];
        $scope.addNewLanguage = function () {
            var newItemNo = $scope.languageDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.languageDetails.push({ 'id': 'language' + newItemNo });
            }
        };

        $scope.removeNewLanguage = function (index, data) {
            var newItemNo = $scope.languageDetails.length - 1;
            $scope.languageDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };

        $scope.language_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validatelanguageDetails = function () {
            if ($scope.myForm5.$valid) {
                var data = {
                    Employeedto: $scope.jobedit,
                    LanguagesDTO: $scope.languageDetails
                };
                apiService.create("AddCandidateVMS/addlanguage", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record Updated Successfully..");
                                $scope.Editable = false;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Editable = false;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Editable = false;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }                            
                        }
                    });
            }
            else {
                $scope.submitted5 = true;
            }
        };

        $scope.clear_fifth_tab = function (data) {
            $scope.languageDetails = [{ id: 'language' }];
            $scope.submitted5 = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        };
        //LANGUAGE DETAILS

        $scope.getaddress = function () {
            if ($scope.mrfCand.copyaddress == true) {
                $scope.mrfCand.hrcD_AddressPermanent = $scope.mrfCand.hrcD_AddressLocal;
            }
        };

        $scope.printcandidatelist = function () {
            var innerContents = document.getElementById("candidatelist").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.notification = function () {
            if ($scope.mrfCand.welcomenotice == true) { $scope.mrfCand.thanksnotice = false; }
            if ($scope.mrfCand.thanksnotice == true) { $scope.mrfCand.welcomenotice = false; }
        };
    }

})();