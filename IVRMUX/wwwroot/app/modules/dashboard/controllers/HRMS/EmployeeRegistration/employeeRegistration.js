(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeeRegistrationController', employeeRegistrationController)

    employeeRegistrationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function employeeRegistrationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {

        $scope.myTabIndex = 0;
        $scope.RetirementYrs = 0;
        $scope.obj = {};
        var imagedownload = "";
        $scope.Employee = {};
        $scope.Employee.hrmE_Id = 0;
        var docname = "";
        var empcode = "";
        $scope.Salary = {};
        $scope.Salary.hreeD_Percentage = '0.00';
        $scope.Salary.hreeD_Amount = '0.00';
        $scope.Salary.hrmeD_AppPercent = '0.00';
        $scope.Salary.hrmeD_Percent = '0.00';
        $scope.Salary.hrmeD_Details = '';
        $scope.Salary.hrmeD_Id = 0;
        $scope.Salary.hreeD_ActiveFlag = true;
        $scope.transferto = false;

//$scope.Employee.hrmE_Photo="https://bdcampusstrg.blob.core.windows.net/files/profile.png";
$('#blah').attr('src', "https://bdcampusstrg.blob.core.windows.net/files/profile.png");

        // Disable Tabs
        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        }
        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        }

        //GAUTAM
        $scope.reasonchange = function (reason) {
            angular.forEach($scope.leavingreasondropdownlist, function (inst) {
                if (inst.hrmlreA_LeavingReason == reason) {
                    if (inst.hrmlreA_TransferredFlg == true) {
                        $scope.transferto = true;
                    }
                }
            });    
        };

        $scope.downloaddirectimage = function (data) {
            $scope.imagedownload = data.hrmE_Photo;
            imagedownload = data.hrmE_Photo;
            docname = "Photo";
            empcode = data.hrmE_EmployeeCode;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
            .success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: empcode + '-' + docname + '.jpg'
                })[0].click();
            });
        };

        $scope.address = true;
        $scope.Qualification = true;
        $scope.Experience = true;
        $scope.DocumentUpload = true;
        $scope.Medicaldetails = true;
        $scope.Otherdetails = true;
        $scope.Salarydetails = true;
        $scope.castedisble = true;
        $scope.castecatdisble = true;
        $scope.departmentdisble = true;
        $scope.designationdisble = true;

     //   $scope.maxDateDOJ = new Date();
    //    $scope.maxDatePF = new Date();
    //    $scope.maxDateESI = new Date();
     //   $scope.maxDateQDate = new Date();
     //   $scope.maxDateExit = new Date();
     //   $scope.maxDateJoin = new Date();
        //data table list

        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableHiding: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmE_EmployeeFirstName', displayName: ' First Name', enableHiding: false },
                { name: 'hrmE_EmployeeMiddleName', displayName: 'Middle Name', enableHiding: false },
                { name: 'hrmE_EmployeeLastName', displayName: 'Last Name', enableHiding: false },
                { name: 'hrmE_EmployeeCode', displayName: 'Employee Code', enableHiding: false },
                {
                    field: 'ids', name: 'photo',
                    displayName: 'Employee Photo', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.downloaddirectimage(row.entity);"> <i class="fa fa-cloud-download" style="color:blue;" aria-hidden="true"></i> Download</a>' +
                        '</div>'
                },
                { name: 'hrmE_EmployeeOrder', displayName: 'Employee Order', enableHiding: false },
                {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hrmE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hrmE_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                 '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        //student form validation


        $scope.submitted1 = false;
        $scope.isduplicate = false;


        //Save First Tab Data

        $scope.validateStuDet = function (obj) {

            if ($scope.myForm1.$valid) {

                if ($scope.Employee.hrmE_DOB != undefined && $scope.Employee.hrmE_DOB != "") {
                    $scope.Employee.hrmE_DOB = new Date($scope.Employee.hrmE_DOB).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOB = "";
                }

                if ($scope.Employee.hrmE_DOJ != undefined && $scope.Employee.hrmE_DOJ != "") {
                    $scope.Employee.hrmE_DOJ = new Date($scope.Employee.hrmE_DOJ).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOJ = "";
                }

                if ($scope.Employee.hrmE_PFDate != undefined && $scope.Employee.hrmE_PFDate != "") {
                    $scope.Employee.hrmE_PFDate = new Date($scope.Employee.hrmE_PFDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_PFDate = "";
                }

                if ($scope.Employee.hrmE_ExpectedRetirementDate != undefined && $scope.Employee.hrmE_ExpectedRetirementDate != "") {
                    $scope.Employee.hrmE_ExpectedRetirementDate = new Date($scope.Employee.hrmE_ExpectedRetirementDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_ExpectedRetirementDate = "";
                }

                if ($scope.Employee.hrmE_ESIDate != undefined && $scope.Employee.hrmE_ESIDate != "") {
                    $scope.Employee.hrmE_ESIDate = new Date($scope.Employee.hrmE_ESIDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_ESIDate = "";
                }

                if ($scope.Employee.hrmE_DOL != undefined && $scope.Employee.hrmE_DOL != "") {
                    $scope.Employee.hrmE_DOL = new Date($scope.Employee.hrmE_DOL).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOL = "";
                }

                if ($scope.Employee.hrmE_DOC != undefined && $scope.Employee.hrmE_DOC != "") {
                    $scope.Employee.hrmE_DOC = new Date($scope.Employee.hrmE_DOC).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOC = "";
                }

                var mobilesstd = [];
                angular.forEach($scope.mobilesstd, function (mobile) {
                    if (mobile.hrmemnO_MobileNo != undefined && mobile.hrmemnO_MobileNo != "") {
                        mobilesstd.push(mobile);
                    }
                });

                var emailsstd = [];
                angular.forEach($scope.emailsstd, function (email) {
                    if (email.hrmeM_EmailId != undefined && email.hrmeM_EmailId != "") {
                        emailsstd.push(email);
                    }
                });

                var employeebankdel = [];
                angular.forEach($scope.employeebankDetails, function (employeebank) {
                    if (employeebank.hrmeB_AccountNo != undefined && employeebank.hrmeB_AccountNo != "") {
                        employeebankdel.push(employeebank);
                    }
                });

                if (mobilesstd.length > 0 && mobilesstd !== null) {
                    angular.forEach(mobilesstd, function (mobile) {
                        if (mobile.hrmemnO_DeFaultFlag === 'default') {
                            $scope.Employee.hrmE_MobileNo = mobile.hrmemnO_MobileNo;
                        }
                    });
                    if ($scope.Employee.hrmE_MobileNo === null || $scope.Employee.hrmE_MobileNo === 0) {
                        $scope.Employee.hrmE_MobileNo = mobilesstd[0].hrmemnO_MobileNo;
                    }
                }

                var data = {
                    Employeedto: $scope.Employee,
                    mobile_list_dto: mobilesstd,
                    email_list_dto: emailsstd,
                    employeebankDTO: employeebankdel,
                    "TabName": "FirstTab"
                };

                apiService.create("EmployeeRegistration/", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            } else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            } else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");

                                $scope.Employee = promise.employeedto;

                                $scope.Employee.hrmE_Id = promise.employeedto.hrmE_Id;
                                $scope.Employee.mI_Id = promise.employeedto.mI_Id;

                                $scope.address = false;
                                $scope.Qualification = false;
                                $scope.Experience = false;
                                $scope.DocumentUpload = false;
                                $scope.Medicaldetails = false;
                                $scope.Otherdetails = false;
                                $scope.Salarydetails = false;
                                $scope.GetTableData();

                                $scope.myTabIndex = $scope.myTabIndex + 1;

                            } else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Employee = promise.employeedto;
                                $scope.Employee.hrmE_Id = promise.employeedto.hrmE_Id;
                                $scope.Employee.mI_Id = promise.employeedto.mI_Id;
                                $scope.GetTableData();
                                $scope.address = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;

                            } else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }

                            // $scope.cancel();

                            //  $scope.onLoadGetData();
                        }
                    })
            } else {

                $scope.submitted1 = true;

                return;
            }

        }
        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.employeedetailListOrder) {
                    $scope.employeedetailListOrder[index].hrmE_EmployeeOrder = Number(index) + 1;

                }
            }
        };
        $scope.GetTableData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeRegistration/getalldetails", pageid).then(function (promise) {

                if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                    $scope.gridOptions.data = promise.employeedetailList;

                    $scope.employeedetailListOrder = promise.employeedetailList;
                }



            })
        }


        //address form validation
        $scope.submitted2 = false;
        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        $scope.validateadd = function () {
            if ($scope.myForm2.$valid) {

                // communication address data
                $scope.Employee.hrmE_LocStreet = $scope.ComAddress.hrmE_LocStreet;
                $scope.Employee.hrmE_LocArea = $scope.ComAddress.hrmE_LocArea;
                $scope.Employee.hrmE_LocCountryId = $scope.ComAddress.hrmE_LocCountryId;
                $scope.Employee.hrmE_LocStateId = $scope.ComAddress.hrmE_LocStateId;
                $scope.Employee.hrmE_LocCity = $scope.ComAddress.hrmE_LocCity;
                $scope.Employee.hrmE_LocPincode = $scope.ComAddress.hrmE_LocPincode;

                // Permenent address data
                $scope.Employee.hrmE_PerStreet = $scope.PerAddress.hrmE_PerStreet;
                $scope.Employee.hrmE_PerArea = $scope.PerAddress.hrmE_PerArea;
                $scope.Employee.hrmE_PerCountryId = $scope.PerAddress.hrmE_PerCountryId;
                $scope.Employee.hrmE_PerStateId = $scope.PerAddress.hrmE_PerStateId;
                $scope.Employee.hrmE_PerCity = $scope.PerAddress.hrmE_PerCity;
                $scope.Employee.hrmE_PerPincode = $scope.PerAddress.hrmE_PerPincode;

                if ($scope.Employee.hrmE_DOB != undefined && $scope.Employee.hrmE_DOB != "") {
                    $scope.Employee.hrmE_DOB = new Date($scope.Employee.hrmE_DOB).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOB = "";
                }

                if ($scope.Employee.hrmE_DOJ != undefined && $scope.Employee.hrmE_DOJ != "") {
                    $scope.Employee.hrmE_DOJ = new Date($scope.Employee.hrmE_DOJ).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOJ = "";
                }

                if ($scope.Employee.hrmE_PFDate != undefined && $scope.Employee.hrmE_PFDate != "") {
                    $scope.Employee.hrmE_PFDate = new Date($scope.Employee.hrmE_PFDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_PFDate = "";
                }

                if ($scope.Employee.hrmE_ExpectedRetirementDate != undefined && $scope.Employee.hrmE_ExpectedRetirementDate != "") {
                    $scope.Employee.hrmE_ExpectedRetirementDate = new Date($scope.Employee.hrmE_ExpectedRetirementDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_ExpectedRetirementDate = "";
                }

                if ($scope.Employee.hrmE_ESIDate != undefined && $scope.Employee.hrmE_ESIDate != "") {
                    $scope.Employee.hrmE_ESIDate = new Date($scope.Employee.hrmE_ESIDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_ESIDate = "";
                }

                var data = {
                    Employeedto: $scope.Employee,
                    //mobile_list_dto: $scope.mobilesstd,
                    // email_list_dto: $scope.emailsstd,
                    "TabName": "AddressTab"
                }
                apiService.create("EmployeeRegistration/", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            } else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            } else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");

                                $scope.Qualification = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;

                            } else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");

                                $scope.Qualification = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;

                            } else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }

                            // $scope.cancel();

                            // $scope.onLoadGetData();
                        }
                    })

            }
            else {
                $scope.submitted2 = true;
                return;
            }
        }

        //Qualification form validation
        $scope.submitted3 = false;
        $scope.Qualification_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateQualificationDetails = function () {
            if ($scope.myForm3.$valid) {

                var duplicateda = false;

                angular.forEach($scope.qualificationDetails, function (value, key) {

                    if ($scope.qualificationDetails[key].hrmeQ_Date != undefined && $scope.qualificationDetails[key].hrmeQ_Date != "") {
                        $scope.qualificationDetails[key].hrmeQ_Date = new Date($scope.qualificationDetails[key].hrmeQ_Date).toDateString();
                    }
                    else {
                        $scope.qualificationDetails[key].hrmeQ_Date = "";
                    }

                    if ($scope.chkdup_qualificationDetails($scope.qualificationDetails[key], key)) {
                        duplicateda = true;
                        return;
                    }

                });
                if (duplicateda) {
                    return;
                } else {
                    var data = {
                        Employeedto: $scope.Employee,
                        EmployeeQulaificationDTO: $scope.qualificationDetails,
                        "TabName": "QualificationTab"
                    }
                    apiService.create("EmployeeRegistration/", data).
                        then(function (promise) {

                            if (promise.retrunMsg !== "") {

                                if (promise.retrunMsg === "Duplicate") {
                                    swal("Record already exist..!!");
                                    return;
                                } else if (promise.retrunMsg === "false") {
                                    swal("Record Not saved / Updated..", 'Fail');
                                } else if (promise.retrunMsg === "Add") {
                                    swal("Record Saved Successfully..");

                                    $scope.Experience = false;
                                    $scope.myTabIndex = $scope.myTabIndex + 1;

                                } else if (promise.retrunMsg === "Update") {
                                    swal("Record Updated Successfully..");

                                    $scope.Experience = false;
                                    $scope.myTabIndex = $scope.myTabIndex + 1;

                                } else {
                                    swal("Something went wrong ..!", 'Kindly contact Administrator');
                                    return;
                                }

                                //  $scope.cancel();

                            }
                        })
                }



            }
            else {
                $scope.submitted3 = true;
                // $scope.Experience = true;
            }
        }


        //Experience form validation
        $scope.EmployeeExperience = 0;
        $scope.submitted4 = false;
        $scope.validateExperiencePrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }


        $scope.validateExperiencedetails = function () {
            if ($scope.myForm4.$valid) {
                var duplicateda = false;
                var experienceDetails = [];

                if ($scope.EmployeeExperience == 1) {
                    angular.forEach($scope.experienceDetails, function (value, key) {

                        if ($scope.experienceDetails[key].hrmeE_JoinDate != undefined && $scope.experienceDetails[key].hrmeE_JoinDate != "") {
                            $scope.experienceDetails[key].hrmeE_JoinDate = new Date($scope.experienceDetails[key].hrmeE_JoinDate).toDateString();
                        }
                        else {
                            $scope.experienceDetails[key].hrmeE_JoinDate = "";
                        }

                        if ($scope.experienceDetails[key].hrmeE_ExitDate != undefined && $scope.experienceDetails[key].hrmeE_ExitDate != "") {
                            $scope.experienceDetails[key].hrmeE_ExitDate = new Date($scope.experienceDetails[key].hrmeE_ExitDate).toDateString();
                        }
                        else {
                            $scope.experienceDetails[key].hrmeE_ExitDate = "";
                        }

                        if ($scope.chkdup_experianceDetails($scope.experienceDetails[key], key)) {
                            duplicateda = true;
                            return;
                        }

                    });

                    experienceDetails = $scope.experienceDetails;
                }
                else {
                    experienceDetails = [];
                }

                if (duplicateda) {
                    return;
                } else {

                    var data = {
                        Employeedto: $scope.Employee,
                        EmployeeExperienceDTO: experienceDetails,
                        "TabName": "ExperienceTab"
                    }
                    apiService.create("EmployeeRegistration/", data).
                        then(function (promise) {

                            if (promise.retrunMsg !== "") {

                                if (promise.retrunMsg === "Duplicate") {
                                    swal("Record already exist..!!");
                                    return;
                                } else if (promise.retrunMsg === "false") {
                                    swal("Record Not saved / Updated..", 'Fail');
                                } else if (promise.retrunMsg === "Add") {
                                    swal("Record Saved Successfully..");

                                    $scope.DocumentUpload = false;
                                    $scope.myTabIndex = $scope.myTabIndex + 1;

                                } else if (promise.retrunMsg === "Update") {
                                    swal("Record Updated Successfully..");

                                    $scope.DocumentUpload = false;
                                    $scope.myTabIndex = $scope.myTabIndex + 1;

                                } else {
                                    swal("Something went wrong ..!", 'Kindly contact Administrator');
                                    return;
                                }


                            } else {
                                $scope.DocumentUpload = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                        })

                }
            }
            else {
                $scope.submitted4 = true;
                $scope.DocumentUpload = true;
            }
        }


        // Document Form validation
        $scope.submitted5 = false;
        $scope.previous_document = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        $scope.validateDocumentdetails = function () {
            if ($scope.myForm5.$valid) {

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
                        Employeedto: $scope.Employee,
                        EmployeeDocumentDTO: $scope.documentList,
                        "TabName": "DocumentTab"
                    }
                    apiService.create("EmployeeRegistration/", data).
                        then(function (promise) {

                            if (promise.retrunMsg !== "") {

                                if (promise.retrunMsg === "Duplicate") {
                                    swal("Record already exist..!!");
                                    return;
                                } else if (promise.retrunMsg === "false") {
                                    swal("Record Not saved / Updated..", 'Fail');
                                } else if (promise.retrunMsg === "Add") {
                                    swal("Record Saved Successfully..");

                                    $scope.Medicaldetails = false;
                                    $scope.myTabIndex = $scope.myTabIndex + 1;

                                } else if (promise.retrunMsg === "Update") {
                                    swal("Record Updated Successfully..");

                                    $scope.Medicaldetails = false;
                                    $scope.myTabIndex = $scope.myTabIndex + 1;
                                } else {
                                    swal("Something went wrong ..!", 'Kindly contact Administrator');
                                    return;
                                }

                                // $scope.cancel();

                            }
                        })
                }
            }
            else {
                $scope.submitted5 = true;
                $scope.Medicaldetails = true;
            }
        }

        //MedicalDetails Form Validation

        $scope.submittedMedicaldetails = false;
        $scope.previous_Medicaldetails = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateMedicaldetails = function () {
            if ($scope.myFormMedicaldetails.$valid) {
                if ($scope.Employee.hrmE_DOB != undefined && $scope.Employee.hrmE_DOB != "") {
                    $scope.Employee.hrmE_DOB = new Date($scope.Employee.hrmE_DOB).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOB = "";
                }

                if ($scope.Employee.hrmE_DOJ != undefined && $scope.Employee.hrmE_DOJ != "") {
                    $scope.Employee.hrmE_DOJ = new Date($scope.Employee.hrmE_DOJ).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOJ = "";
                }

                if ($scope.Employee.hrmE_PFDate != undefined && $scope.Employee.hrmE_PFDate != "") {
                    $scope.Employee.hrmE_PFDate = new Date($scope.Employee.hrmE_PFDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_PFDate = "";
                }

                if ($scope.Employee.hrmE_ExpectedRetirementDate != undefined && $scope.Employee.hrmE_ExpectedRetirementDate != "") {
                    $scope.Employee.hrmE_ExpectedRetirementDate = new Date($scope.Employee.hrmE_ExpectedRetirementDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_ExpectedRetirementDate = "";
                }

                if ($scope.Employee.hrmE_ESIDate != undefined && $scope.Employee.hrmE_ESIDate != "") {
                    $scope.Employee.hrmE_ESIDate = new Date($scope.Employee.hrmE_ESIDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_ESIDate = "";
                }

                if ($scope.Employee.hrmE_DOL != undefined && $scope.Employee.hrmE_DOL != "") {
                    $scope.Employee.hrmE_DOL = new Date($scope.Employee.hrmE_DOL).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOL = "";
                }

                if ($scope.Employee.hrmE_DOC != undefined && $scope.Employee.hrmE_DOC != "") {
                    $scope.Employee.hrmE_DOC = new Date($scope.Employee.hrmE_DOC).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOC = "";
                }

                var data = {
                    Employeedto: $scope.Employee,
                    "TabName": "MedicalTab"
                };
                apiService.create("EmployeeRegistration/", data).
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
                $scope.submittedMedicaldetails = true;
                $scope.Otherdetails = true;
            }
        };


        // Otherdetails Form validation

        $scope.submittedOtherdetails = false;
        $scope.previous_Otherdetails = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }
        $scope.validateOtherdetails = function () {

            if ($scope.myFormOtherdetails.$valid) {

                if ($scope.Employee.hrmE_DOB != undefined && $scope.Employee.hrmE_DOB != "") {
                    $scope.Employee.hrmE_DOB = new Date($scope.Employee.hrmE_DOB).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOB = "";
                }

                if ($scope.Employee.hrmE_DOJ != undefined && $scope.Employee.hrmE_DOJ != "") {
                    $scope.Employee.hrmE_DOJ = new Date($scope.Employee.hrmE_DOJ).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOJ = "";
                }

                if ($scope.Employee.hrmE_PFDate != undefined && $scope.Employee.hrmE_PFDate != "") {
                    $scope.Employee.hrmE_PFDate = new Date($scope.Employee.hrmE_PFDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_PFDate = "";
                }

                if ($scope.Employee.hrmE_ExpectedRetirementDate != undefined && $scope.Employee.hrmE_ExpectedRetirementDate != "") {
                    $scope.Employee.hrmE_ExpectedRetirementDate = new Date($scope.Employee.hrmE_ExpectedRetirementDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_ExpectedRetirementDate = "";
                }

                if ($scope.Employee.hrmE_ESIDate != undefined && $scope.Employee.hrmE_ESIDate != "") {
                    $scope.Employee.hrmE_ESIDate = new Date($scope.Employee.hrmE_ESIDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_ESIDate = "";
                }

                if ($scope.Employee.hrmE_DOL != undefined && $scope.Employee.hrmE_DOL != "") {
                    $scope.Employee.hrmE_DOL = new Date($scope.Employee.hrmE_DOL).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOL = "";
                }

                if ($scope.Employee.hrmE_DOC != undefined && $scope.Employee.hrmE_DOC != "") {
                    $scope.Employee.hrmE_DOC = new Date($scope.Employee.hrmE_DOC).toDateString();
                }
                else {
                    $scope.Employee.hrmE_DOC = "";
                }
                if ($scope.Employee.hrmE_PensionStoppedDate != undefined && $scope.Employee.hrmE_PensionStoppedDate != "") {
                    $scope.Employee.hrmE_PensionStoppedDate = new Date($scope.Employee.hrmE_PensionStoppedDate).toDateString();
                }
                else {
                    $scope.Employee.hrmE_PensionStoppedDate = "";
                }

                var HRME_RetiredFlg = 0;
                if ($scope.Employee.hrmE_RetiredFlg == true)
                {
                    HRME_RetiredFlg = 1;
                }

                var data = {
                    Employeedto: $scope.Employee,
                    //"HRME_DOC": $scope.Employee.hrmE_DOC,
                    "TabName": "OtherTab",
                   // "HRME_RetiredFlg": $scope.hrmE_RetiredFlg
                }
                apiService.create("EmployeeRegistration/", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            } else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            } else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");

                                $scope.Salarydetails = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;

                            } else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");

                                $scope.Salarydetails = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                                $scope.getSalaryDetails();
                            } else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }

                            //    $scope.cancel();

                        }
                    })

            }
            else {
                $scope.submittedOtherdetails = true;
                $scope.Salarydetails = true;
            }
        }

        $scope.submittedSalarydetails = false;
        $scope.previous_Salarydetails = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        //Salary details form validation
        $scope.submittedSalarydetails = false;
        $scope.saveSalaryDetails = function () {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {



                var selectedEarning = [];
                if ($scope.Salary.hrmeD_Id > 0) {
                    selectedEarning.push($scope.Salary);
                }
                else {
                    $scope.Salary.hreeD_Percentage = '0.00';
                    $scope.Salary.hreeD_Amount = '0.00';
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                    $scope.Salary.hrmeD_Id = 0;
                    $scope.Salary.hreeD_ActiveFlag = true;
                }

                if ($scope.earningList !== null && $scope.earningList.length > 0) {

                    angular.forEach($scope.earningList, function (earning) {
                        if (earning.hreeD_ActiveFlag) {
                            earning.hreeD_ActiveFlag = true;
                        } else {
                            earning.hreeD_ActiveFlag = false;
                        }
                        selectedEarning.push(earning);


                    });

                }
                
                var selectedDetection = [];

                if ($scope.detectionList !== null && $scope.detectionList.length > 0) {

                    angular.forEach($scope.detectionList, function (detection) {
                        if (detection.hreeD_ActiveFlag) {
                            detection.hreeD_ActiveFlag = true;

                        } else {
                            detection.hreeD_ActiveFlag = false;
                        }

                        selectedDetection.push(detection);
                    });

                }

                var selectedArrear = [];

                if ($scope.arrearList !== null && $scope.arrearList.length > 0) {

                    angular.forEach($scope.arrearList, function (arrear) {
                        if (arrear.hreeD_ActiveFlag) {

                            arrear.hreeD_ActiveFlag = true;
                        } else {
                            arrear.hreeD_ActiveFlag = false;
                        }

                        selectedArrear.push(arrear);

                    });

                }

                if (selectedEarning.length === 0 && selectedDetection.length === 0) {
                    swal('Kindly select atleast one record from Earning / Deduction');
                    return;
                }

                var data = {
                    Employeedto: $scope.Employee,
                    EarningDTO: selectedEarning,
                    DeductionDTO: selectedDetection,
                    ArrearDTO: selectedArrear,
                    "TabName": "SalaryTab"
                }
                apiService.create("EmployeeRegistration/", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            } else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            } else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            } else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                            } else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }

                            $scope.cancel();

                            $scope.onLoadGetData();
                        }
                    })
            }

        };

        $scope.interacted1 = function (field) {

            return $scope.submitted1; //|| field.$dirty;
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

        $scope.interacted6 = function (field) {

            return $scope.submittedOtherdetails || field.$dirty;
        };
        $scope.interacted7 = function (field) {
            //if (field.$dirty==undefined) {
            //    console.log(field.$error);
            //}


            return $scope.submittedSalarydetails;
        };


        //

        //Clear functionality
        $scope.clear_first_tab = function (data) {

            for (var i = 0; i < $scope.mobilesstd.length; i++) {
                $scope.mobilesstd[i].hrmemnO_MobileNo = "";
                $scope.mobilesstd[i].hrmemnO_DeFaultFlag = "";
                $scope.mobilesstd.length = 1;
                $scope.disabled = false;
                // $scope.mobilesstd = "";
            }
            for (var i = 0; i < $scope.emailsstd.length; i++) {
                $scope.emailsstd[i].hrmeM_EmailId = "";
                $scope.emailsstd[i].hrmeM_DeFaultFlag = "";
                $scope.emailsstd.length = 1;
                $scope.disabled2 = false;

            }

            $scope.employeebankDetails = [{ id: 'employeebank' }];
            $scope.disabled3 = false;

            $scope.Employee = {};
            $scope.Employee.hrmE_Id = 0;
            $scope.ShowSpouseDetails = false;
            $scope.UploadEmployeeProfilePic = [];
            $('#blah').removeAttr('src');
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();

            $scope.address = true;
            $scope.Qualification = true;
            $scope.Experience = true;
            $scope.DocumentUpload = true;
            $scope.Otherdetails = true;
            $scope.Salarydetails = true;
            $scope.Medicaldetails = true;

            $scope.castecatdisble = true;
            $scope.departmentdisble = true;
            $scope.designationdisble = true;
        };


        $scope.clear_second_tab = function (data) {
            $scope.chkbox_addressDis = true;
            $scope.ComAddress = {};
            $scope.PerAddress = {};
            $scope.PermanentDis = false;
            $scope.CommunicationAdDis = false;

            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();

        };

        $scope.clear_third_tab = function (data) {
            debugger;
            $scope.qualificationDetails = [{ id: 'qualification' }];
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();

        };

        $scope.clear_fourth_tab = function () {

            $scope.experienceDetails = [{ id: 'experience' }];
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();

        };
        $scope.clear_fifth_tab = function () {

            $scope.documentList = [{ id: 'document' }];
            $("#document").val("");
            //  $('#document').removeAttr('src');

            $scope.submitted5 = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        };


        $scope.clear_Otherdetails_tab = function (data) {

            $scope.Employee.hrmE_BiometricCode = "";
            $scope.Employee.hrmE_RFCardId = "";
            $scope.Employee.hrmE_LeavingReason = "";
            $scope.submittedOtherdetails = false;
            $scope.myFormOtherdetails.$setPristine();
            $scope.myFormOtherdetails.$setUntouched();

        };

        $scope.clear_Salarydetails_tab = function (data) {

            $scope.Salary.hreeD_Percentage = '0.00';
            $scope.Salary.hreeD_Amount = '0.00';
            $scope.Salary.hrmeD_AppPercent = '0.00';
            $scope.Salary.hrmeD_Percent = '0.00';
            $scope.Salary.hrmeD_Details = '';
            //$scope.Salary.hreeD_ActiveFlag = true;

            angular.forEach($scope.earningList, function (value, key) {

                $scope.earningList[key].hreeD_ActiveFlag = false;
                $scope.earningList[key].hreeD_Amount = '0.00';
                $scope.earningList[key].hreeD_Percentage = '0.00';

            });
            angular.forEach($scope.detectionList, function (value, key) {

                $scope.detectionList[key].hreeD_ActiveFlag = false;
                $scope.detectionList[key].hreeD_Amount = '0.00';
                $scope.detectionList[key].hreeD_Percentage = '0.00';

            });
            angular.forEach($scope.arrearList, function (value, key) {

                $scope.arrearList[key].hreeD_ActiveFlag = false;
                $scope.arrearList[key].hreeD_Amount = '0.00';
                $scope.arrearList[key].hreeD_Percentage = '0.00';

            });

            $scope.netSalary = 0;
            $scope.EarningTotal = 0;
            $scope.DeductionTotal = 0;
            $scope.ArrearTotal = 0;

            //$scope.detectionList =[];
            //$scope.detectionList = [];
            $scope.submittedSalarydetails = false;
            $scope.myFormSalarydetails.$setPristine();
            $scope.myFormSalarydetails.$setUntouched();


        };


        //address copy 
        $scope.PermanentDis = false;
        $scope.CommunicationAdDis = false;
        $scope.address_copy = function (da) {
            if (da === 1) {
                $scope.PermanentDis = true;
                $scope.CommunicationAdDis = true;
                $scope.PerAddress.hrmE_PerStreet = $scope.ComAddress.hrmE_LocStreet;
                $scope.PerAddress.hrmE_PerArea = $scope.ComAddress.hrmE_LocArea;
                $scope.PerAddress.hrmE_PerCountryId = $scope.ComAddress.hrmE_LocCountryId;
                //$scope.PerAddress.hrmE_PerStateId = $scope.ComAddress.hrmE_LocStateId;
                $scope.PerAddress.hrmE_PerCity = $scope.ComAddress.hrmE_LocCity;
                $scope.PerAddress.hrmE_PerPincode = $scope.ComAddress.hrmE_LocPincode;

                var PerCountryId = $scope.ComAddress.hrmE_LocCountryId;
                var PerStateId = $scope.ComAddress.hrmE_LocStateId;
                if ((PerCountryId !== null && PerCountryId !== "") && (PerStateId !== null && PerStateId !== "")) {

                    getSelectedPerState(PerCountryId, PerStateId);
                }

            }
            else {
                $scope.PermanentDis = false;
                $scope.CommunicationAdDis = false;
                $scope.PerAddress.hrmE_PerStreet = "";
                $scope.PerAddress.hrmE_PerArea = "";
                $scope.PerAddress.hrmE_PerCountryId = "";
                $scope.PerAddress.hrmE_PerStateId = "";
                $scope.PerAddress.hrmE_PerCity = "";
                $scope.PerAddress.hrmE_PerPincode = "";
            }

        };


        //dynamic table for Employee Bank Details


        $scope.employeebankDetails = [{ id: 'employeebank' }];
        $scope.addNewEmployeebank = function () {
            var newItemNo = $scope.employeebankDetails.length + 1;

            if (newItemNo <= 5) {
                $scope.employeebankDetails.push({ 'id': 'employeebank' + newItemNo });
            }
        };


        //removing Employee bank details
        $scope.delmsrd = [];
        $scope.removeNewEmployeebank = function (index, curval1std) {

            if (curval1std.hrmeB_Id != undefined) {

                var data =
                {
                    "HRMEB_Id": curval1std.hrmeB_Id
                };
                swal({
                    title: "Are you sure?",
                    text: "Do you want to delete the record?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },

                    function (isConfirm) {
                        if (isConfirm) {


                            apiService.create("EmployeeRegistration/del_bankAccount", data)


                                .then(function (promise) {
                                    if (promise.retrunMsg == "Deleted") {
                                        swal('Record Deleted Successfully');

                                        var newItemNostd2 = $scope.employeebankDetails.length - 1;
                                        if (newItemNostd2 !== 0) {
                                            $scope.delmsrd = $scope.employeebankDetails.splice(index, 1);
                                        }

                                    }

                                });
                        }
                        else {
                            swal(" Cancelled", "Ok");
                            // $scope.onLoadGetData();
                        }
                    });
            }

            if (curval1std.hrmeB_Id == undefined) {
                var newItemNostd2 = $scope.employeebankDetails.length - 1;
                if (newItemNostd2 !== 0) {
                    $scope.delmsrd = $scope.employeebankDetails.splice(index, 1);
                }
            }

        };








        //dynamic table for qualification

        // $scope.maxDateQDate = new Date();

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


            //if ($scope.qualificationDetails.length === 0) {
            //}
        };


        //dynamic table for Experience

        $scope.experienceDetails = [{ id: 'experience' }];

        $scope.experienceDetails[0].exitDateDis = true;

        // $scope.maxDateJoin = new Date();

        $scope.addNewExperience = function () {
            var newItemNo = $scope.experienceDetails.length + 1;

            if (newItemNo <= 10) {
                $scope.experienceDetails.push({ 'id': 'experience' + newItemNo });

                $scope.experienceDetails[newItemNo - 1].exitDateDis = true;

                // experience.exitDateDis
            }
        };

        $scope.removeNewExperience = function (index, data) {
            var newItemNo = $scope.experienceDetails.length - 1;
            $scope.experienceDetails.splice(index, 1);
            if (data.hrmeE_Id > 0) {
                $scope.DeleteExperienceData(data);
            }

            //if ($scope.experienceDetails.length === 0) {
            //}
        };

        //Check Date of join
        $scope.checkDOBSelected = function (hrmE_DOJ) {

            //if ($scope.Employee.hrmE_DOB != undefined && $scope.Employee.hrmE_DOB != "") {

            //    var hrmE_DOB = new Date($filter('date')(new Date($scope.Employee.hrmE_DOB).toDateString(), "yyyy/MM/dd"));
            //    var hrmE_DOJ = new Date($filter('date')(new Date(hrmE_DOJ).toDateString(), "yyyy/MM/dd"));

            //    if (hrmE_DOJ > hrmE_DOB) {

            //        $scope.minDateDOL = new Date(hrmE_DOJ);
            //        $scope.maxDateDOL = new Date();
            //        $scope.maxDateQDate = hrmE_DOJ;


            //        //$scope.maxDateJoin = hrmE_DOJ;
            //        $scope.maxDateJoin = new Date(
            //   hrmE_DOJ.getFullYear(),// - parseInt($scope.RetirementYrs),   
            //            hrmE_DOJ.getMonth(),
            //   hrmE_DOJ.getDate() - 1
            //   );
            //    } else {
            //        swal('Date Of join should be greater than Date of Birth.. !!');
            //        $scope.Employee.hrmE_DOJ = "";
            //        return;
            //    }

            //}
            //else {
            //    swal('Kindly select Date Of Birth First .. !!');
            //    $scope.Employee.hrmE_DOJ = "";
            //    return;
            //}
        };

        // $scope.checkDOBSelected = function (hrmE_DOJ) {
        //    
        //    if ($scope.Employee.hrmE_DOB != undefined && $scope.Employee.hrmE_DOB != "") {

        //        var hrmE_DOB = new Date($filter('date')(new Date($scope.Employee.hrmE_DOB).toDateString(), "yyyy/MM/dd"));
        //        var hrmE_DOJ = new Date($filter('date')(new Date(hrmE_DOJ).toDateString(), "yyyy/MM/dd"));

        //        if (hrmE_DOJ > hrmE_DOB) {

        //            $scope.minDateDOL = new Date(hrmE_DOJ);
        //            $scope.maxDateDOL = new Date();
        //            $scope.maxDateQDate = hrmE_DOJ;

        //        } else {
        //            swal('Date Of join should be greater than Date of Birth.. !!');
        //            $scope.Employee.hrmE_DOJ = "";
        //            return;
        //        }

        //    }
        //    else {
        //        swal('Kindly select Date Of Birth First .. !!');
        //        $scope.Employee.hrmE_DOJ = "";
        //        return;
        //    }
        //}
        // Left Date

        $scope.checkDOJSelected = function (hrmE_DOL) {

            if ($scope.Employee.hrmE_DOJ != undefined && $scope.Employee.hrmE_DOJ != "") {
                var hrmE_DOJ = new Date($filter('date')(new Date($scope.Employee.hrmE_DOJ).toDateString(), "yyyy/MM/dd"));
                var hrmE_DOL = new Date($filter('date')(new Date(hrmE_DOL).toDateString(), "yyyy/MM/dd"));

                if (hrmE_DOL > hrmE_DOJ) {

                } else {
                    swal('Date Of Left should be greater than Date of join.. !!');
                    $scope.Employee.hrmE_DOL = "";
                    return;
                }

            }
            else {
                swal('Kindly select Date Of join First .. !!');
                $scope.Employee.hrmE_DOL = "";
                return;
            }
        }

        $scope.checkDOCSelected = function (hrmE_DOC) {
            if ($scope.Employee.hrmE_DOJ != undefined && $scope.Employee.hrmE_DOJ != "") {
                //var hrmE_DOJ = new Date($filter('date')(new Date($scope.Employee.hrmE_DOJ).toDateString(), "yyyy/MM/dd"));
                //var hrmE_DOC = new Date($filter('date')(new Date(hrmE_DOC).toDateString(), "yyyy/MM/dd"));
                //if (hrmE_DOC > hrmE_DOJ) { }
                //else {
                //    swal('Date of Confirmation should be greater than Date of Join..!!');
                //    $scope.Employee.hrmE_DOC = "";
                //    return;
                //}
            }
            else {
                swal('Kindly select Date of join First .. !!');
                $scope.Employee.hrmE_DOC = "";
                return;
            }
        };


        //Check PF Date 
        $scope.checkPFSelected = function (hrmE_PFDate) {

            if ($scope.Employee.hrmE_DOB != undefined && $scope.Employee.hrmE_DOB != "") {

                var hrmE_DOB = new Date($filter('date')(new Date($scope.Employee.hrmE_DOB).toDateString(), "yyyy/MM/dd"));
                var hrmE_PFDate = new Date($filter('date')(new Date(hrmE_PFDate).toDateString(), "yyyy/MM/dd"));

                if (hrmE_PFDate > hrmE_DOB) {

                } else {
                    swal('PF Date should be greater than Date of Birth.. !!');
                    $scope.Employee.hrmE_PFDate = "";
                    return;
                }

            }
            else {
                swal('Kindly select Date Of Birth First .. !!');
                $scope.Employee.hrmE_PFDate = "";
                return;
            }
        };

        //ESI Selected



        $scope.checkESISelected = function (hrmE_ESIDate) {

            if ($scope.Employee.hrmE_DOB != undefined && $scope.Employee.hrmE_DOB != "") {

                var hrmE_DOB = new Date($filter('date')(new Date($scope.Employee.hrmE_DOB).toDateString(), "yyyy/MM/dd"));
                var hrmE_ESIDate = new Date($filter('date')(new Date(hrmE_ESIDate).toDateString(), "yyyy/MM/dd"));

                if (hrmE_ESIDate > hrmE_DOB) {

                } else {
                    swal('ESI Date should be greater than Date of Birth.. !!');
                    $scope.Employee.hrmE_ESIDate = "";
                    return;
                }

            }
            else {
                swal('Kindly select Date Of Birth First .. !!');
                $scope.Employee.hrmE_ESIDate = "";
                return;
            }
        };





        //dynamic table for Document

        $scope.documentList = [{ id: 'document' }];
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


            //if ($scope.documentList.length === 0) {
            //}
        };

        $scope.ComAddress = {};
        $scope.PerAddress = {};
        $scope.configurationDetails = {};

        $scope.RetirementDateDis = true;
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeRegistration/getalldetails", pageid).then(function (promise) {

                if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                    $scope.gridOptions.data = promise.employeedetailList;

                    $scope.employeedetailListOrder = promise.employeedetailList;
                }

                //dropdown list

                $scope.employeeTypedropdownlist = promise.employeeTypedropdownlist;
                $scope.groupTypedropdownlist = promise.groupTypedropdownlist;
                //$scope.departmentdropdownlist = promise.departmentdropdownlist;
                //$scope.designationdropdownlist = promise.designationdropdownlist;
                $scope.gradedropdownlist = promise.gradedropdownlist;
                $scope.genderdropdownlist = promise.genderdropdownlist;
                $scope.maritalStatusdropdownlist = promise.maritalStatusdropdownlist;

                $scope.religiondropdownlist = promise.religiondropdownlist;
                $scope.casteCategoryedropdownlist = promise.casteCategorydropdownlist;

                $scope.castedropdownlist = promise.castedropdownlist;
                $scope.onloadcastedropdownlist = promise.castedropdownlist;


                $scope.percountrydropdownlist = promise.countrydropdownlist;
                $scope.comcountrydropdownlist = promise.countrydropdownlist;

                $scope.coursedropdownlist = promise.coursedropdownlist;
                $scope.specialisedropdownlist = promise.specialisationList;
                $scope.leavingreasondropdownlist = promise.leavingReasonList;
                $scope.institutiondropdownlist = promise.institutiondropdownlist;

                $scope.bankdropdownlist = promise.bankdropdownlist;

                $scope.mi_id = promise.mI_Id;
                $scope.earningList = promise.earningList;



                angular.forEach($scope.earningList, function (value, key) {
                    $scope.earningList[key].hreeD_ActiveFlag = true;

                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.earningList[key].hreeD_Percentage = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hreeD_Amount = '0.00';
                        $scope.earningList[key].hrmeD_Percent = value.hrmeD_AmountPercent;

                        $scope.earningList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        //percentOff

                        $scope.earningList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;


                        $scope.earningList[key].AmountDis = true;
                        $scope.earningList[key].PercentDis = false;

                    }
                    else {
                        $scope.earningList[key].hreeD_Percentage = '0.00';
                        $scope.earningList[key].hreeD_Amount = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = '0.00';

                        $scope.earningList[key].hrmeD_Percent = '0.00';

                        $scope.earningList[key].hrmeD_Details = '';

                        $scope.earningList[key].AmountDis = false;
                        $scope.earningList[key].PercentDis = true;
                    }

                });


                //deduction list

                $scope.detectionList = promise.detectionList;

                angular.forEach($scope.detectionList, function (value, key) {

                    $scope.detectionList[key].hreeD_ActiveFlag = true;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.detectionList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hreeD_Amount = '0.00';
                        $scope.detectionList[key].hreeD_Percentage = value.hrmeD_AmountPercent;

                        $scope.detectionList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;

                        $scope.detectionList[key].AmountDis = true;
                        $scope.detectionList[key].PercentDis = false;

                    }
                    else {
                        $scope.detectionList[key].hrmeD_Percent = '0.00';

                        $scope.detectionList[key].hreeD_Amount = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hreeD_Percentage = '0.00';

                        $scope.detectionList[key].hrmeD_Details = '';

                        $scope.detectionList[key].AmountDis = false;
                        $scope.detectionList[key].PercentDis = true;
                    }

                });



                $scope.arrearList = promise.arrearList;



                angular.forEach($scope.arrearList, function (value, key) {
                    $scope.arrearList[key].hreeD_ActiveFlag = true;

                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.arrearList[key].hreeD_Percentage = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hreeD_Amount = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = value.hrmeD_AmountPercent;

                        $scope.arrearList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        //percentOff

                        $scope.arrearList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;

                        $scope.arrearList[key].AmountDis = true;
                        $scope.arrearList[key].PercentDis = false;

                    }
                    else {
                        $scope.arrearList[key].hreeD_Percentage = '0.00';
                        $scope.arrearList[key].hreeD_Amount = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = '0.00';

                        $scope.arrearList[key].hrmeD_Percent = '0.00';

                        $scope.arrearList[key].hrmeD_Details = '';

                        $scope.arrearList[key].AmountDis = false;
                        $scope.arrearList[key].PercentDis = true;
                    }

                });
                if (promise.grossList[0] != null && promise.grossList[0] != undefined) {
                    $scope.Salary = promise.grossList[0];
                    $scope.Salary.hreeD_ActiveFlag = true;
                    // 

                    $scope.Salary.hreeD_Percentage = '0.00';
                    $scope.Salary.hreeD_Amount = $scope.Salary.hrmeD_AmountPercent;
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';


                } else {
                    $scope.Salary.hreeD_Percentage = '0.00';
                    $scope.Salary.hreeD_Amount = '0.00';
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                    $scope.Salary.hrmeD_Id = 0;
                    $scope.Salary.hreeD_ActiveFlag = true;
                }



                //
                if (promise.configurationDetails !== null) {

                    $scope.RetirementYrs = promise.configurationDetails.hrC_RetirementYrs;
                    $scope.hrC_AsPerEmpFlag = promise.configurationDetails.hrC_AsPerEmpFlag;

                    $scope.configurationDetails = promise.configurationDetails;


                }



                var mydateMin = new Date();
                var mydateMax = new Date();

                //$scope.minDateDOB = new Date(
                //    mydateMin.getFullYear() - 200,// - parseInt($scope.RetirementYrs),
                //    mydateMin.getMonth(),
                //    mydateMin.getDate()
                //    );


                //$scope.maxDateDOB = new Date(
                //  mydateMax.getFullYear() - 18,
                //  mydateMax.getMonth(),
                //  mydateMax.getDate()
                //  );

            });
        };


        $scope.getEarningTotal = function () {

            var total = 0;
            for (var i = 0; i < $scope.earningList.length; i++) {

                if ($scope.earningList[i].hreeD_ActiveFlag) {

                    var product = $scope.earningList[i];
                    if (product.hreeD_ApplicableMaxValue > 0) {
                        total += parseFloat(product.hreeD_ApplicableMaxValue);
                    }
                    else {
                        total += product.hreeD_Amount;
                    }

                    
                }


            }
            return total;
        };



        $scope.getDeductionTotal = function () {

            var total = 0;
            for (var i = 0; i < $scope.detectionList.length; i++) {
                if ($scope.detectionList[i].hreeD_ActiveFlag) {
                    var product = $scope.detectionList[i];
                    if (product.hreeD_ApplicableMaxValue > 0) {
                        total += parseFloat(product.hreeD_ApplicableMaxValue);
                    }
                    else {
                        total += product.hreeD_Amount;
                    }
                }

            }
            return total;
        };



        $scope.getArrearTotal = function () {

            var total = 0;
            for (var i = 0; i < $scope.arrearList.length; i++) {
                if ($scope.arrearList[i].hreeD_ActiveFlag) {
                    var product = $scope.arrearList[i];
                    if (product.hreeD_ApplicableMaxValue > 0) {
                        total += parseFloat(product.hreeD_ApplicableMaxValue);
                    }
                    else {
                        total += product.hreeD_Amount;
                    }
                }

            }
            return total;
        };



        // clear form data
        $scope.cancel = function () {

            if ($scope.myTabIndex !== 0) {
                $scope.myTabIndex = 0;
            }

            $scope.clear_first_tab();
            $scope.clear_second_tab();
            $scope.clear_third_tab();
            $scope.clear_fourth_tab();
            $scope.clear_fifth_tab();
            $scope.clear_Otherdetails_tab();
            $scope.clear_Salarydetails_tab();
            $scope.clear_Medicaldetails_tab();
            // $scope.myForm.$setPristine();
            // $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();

        };


        // Edit Single Record
        $scope.EditData = function (record) {
            $scope.Employee = {};
            $('#blah').removeAttr('src');

            $scope.castedisble = false;
            $scope.castedropdownlist = [];
            $scope.address = true;
            $scope.Qualification = true;
            $scope.Experience = true;
            $scope.DocumentUpload = true;
            $scope.Medicaldetails = true;
            $scope.Otherdetails = true;
            $scope.Salarydetails = true;


            $scope.ComAddress = {};
            $scope.PerAddress = {};

            $scope.employeebankDetails = [{ id: 'employeebank' }];

            $scope.experienceDetails = [{ id: 'experience' }];
            $scope.qualificationDetails = [{ id: 'qualification' }];
            $scope.documentList = [{ id: 'document' }];

            $scope.EmployeeExperience = 0;

            angular.forEach($scope.earningList, function (value, key) {

                // $scope.earningList[key].Selected = false;

                $scope.earningList[key].hreeD_ActiveFlag = false;

                $scope.earningList[key].hreeD_Amount = '0.00';
                $scope.earningList[key].hreeD_Percentage = '0.00';
            });



            angular.forEach($scope.detectionList, function (value, key) {

                // $scope.detectionList[key].Selected = false;
                $scope.detectionList[key].hreeD_ActiveFlag = false;

                $scope.detectionList[key].hreeD_Amount = '0.00';
                $scope.detectionList[key].hreeD_Percentage = '0.00';
            });


            angular.forEach($scope.arrearList, function (value, key) {

                // $scope.arrearList[key].Selected = false;
                $scope.arrearList[key].hreeD_ActiveFlag = false;

                $scope.arrearList[key].hreeD_Amount = '0.00';
                $scope.arrearList[key].hreeD_Percentage = '0.00';
            });


            // $scope.Salary = {};

            if ($scope.myTabIndex !== 0) {
                $scope.myTabIndex = 0;
            }

            $scope.ShowSpouseDetails = false;

            var id = record.hrmE_Id;
            apiService.getURI("EmployeeRegistration/editRecord", id).
                then(function (promise) {
                    if (promise.employeedetailList != null && promise.employeedetailList.length > 0) {

                        $scope.address = false;
                        $scope.Qualification = false;
                        $scope.Experience = false;
                        $scope.DocumentUpload = false;
                        $scope.Medicaldetails = false;
                        $scope.Otherdetails = false;
                        $scope.Salarydetails = false;
                        $scope.departmentdisble = false;
                        $scope.designationdisble = false;
                        $scope.castecatdisble = false;

                        $scope.Employee = promise.employeedetailList[0];
                        $scope.Employee.mI_Id = promise.employeedetailList[0].mI_Id;
                        if ($scope.Employee.ivrmmmS_Id !== null && $scope.Employee.ivrmmmS_Id !== "") {
                            $scope.setMaritalStatus($scope.Employee.ivrmmmS_Id);
                        }
                        
                        getSelectedcaste(promise.employeedetailList[0].casteCategoryId, promise.employeedetailList[0].casteId); 
                     //   $scope.getcaste(promise.employeedetailList[0].casteId);
                       // $scope.Employee.casteId = promise.employeedetailList[0].casteId;

                        $scope.mi_id = promise.employeedetailList[0].mI_Id;
                        if (promise.employeedetailList[0].hrmE_Photo != null && promise.employeedetailList[0].hrmE_Photo != "") {
                            //$('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeProfilePics" + "/" + promise.employeedetailList[0].hrmE_Photo);
                            $('#blah').attr('src', promise.employeedetailList[0].hrmE_Photo);

                        }

                        if (promise.employeedetailList[0].hrmE_DOB != null) {
                            $scope.Employee.hrmE_DOB = new Date(promise.employeedetailList[0].hrmE_DOB);
                            //  $scope.calcage($scope.Employee.hrmE_DOB);
                        }
                        else {
                            $scope.Employee.hrmE_DOB = null;
                        }


                        if (promise.employeedetailList[0].hrmE_DOJ != null) {
                            $scope.Employee.hrmE_DOJ = new Date(promise.employeedetailList[0].hrmE_DOJ);
                            //  $scope.checkDOBSelected($scope.Employee.hrmE_DOJ);
                        }
                        else {
                            $scope.Employee.hrmE_DOJ = null;
                        }

                        if (promise.employeedetailList[0].hrmE_DOL != null) {
                            $scope.Employee.hrmE_DOL = new Date(promise.employeedetailList[0].hrmE_DOL);
                        }
                        else {
                            $scope.Employee.hrmE_DOL = null;
                        }

                        if (promise.employeedetailList[0].hrmE_DOC != null) {
                            $scope.Employee.hrmE_DOC = new Date(promise.employeedetailList[0].hrmE_DOC);
                        } else {
                            $scope.Employee.hrmE_DOC = null;
                        }

                        // $scope.Employee.hrmE_DOL = new Date(promise.employeedetailList[0].hrmE_DOL);

                        $scope.chkbox_addressDis = false;


                        if (promise.employeedetailList[0].hrmE_ExpectedRetirementDate != null) {
                            $scope.Employee.hrmE_ExpectedRetirementDate = new Date(promise.employeedetailList[0].hrmE_ExpectedRetirementDate);
                        }
                        else {
                            $scope.Employee.hrmE_ExpectedRetirementDate = null;
                        }

                        if (promise.employeedetailList[0].hrmE_PFDate != null) {
                            $scope.Employee.hrmE_PFDate = new Date(promise.employeedetailList[0].hrmE_PFDate);
                        }
                        else {
                            $scope.Employee.hrmE_PFDate = null;
                        }

                        if (promise.employeedetailList[0].hrmE_ESIDate != null) {
                            $scope.Employee.hrmE_ESIDate = new Date(promise.employeedetailList[0].hrmE_ESIDate);
                        }
                        else {
                            $scope.Employee.hrmE_ESIDate = null;
                        }


                        if (promise.employeedetailList[0].hrmE_PFApplicableFlag == 1) {
                            $scope.Employee.hrmE_PFApplicableFlag = true;
                        } else {
                            $scope.Employee.hrmE_PFApplicableFlag = false;
                        }

                        if (promise.employeedetailList[0].hrmE_PFMaxFlag == 1) {
                            $scope.Employee.hrmE_PFMaxFlag = true;
                        } else {
                            $scope.Employee.hrmE_PFMaxFlag = false;
                        }

                        if (promise.employeedetailList[0].hrmE_PFFixedFlag == 1) {
                            $scope.Employee.hrmE_PFFixedFlag = true;
                        } else {
                            $scope.Employee.hrmE_PFFixedFlag = false;
                        }

                        if (promise.employeedetailList[0].hrmE_SubstituteFlag == 1) {
                            $scope.Employee.hrmE_SubstituteFlag = true;
                        } else {
                            $scope.Employee.hrmE_SubstituteFlag = false;
                        }

                        if (promise.employeedetailList[0].hrmE_LeftFlag == 1) {
                            $scope.Employee.hrmE_LeftFlag = true;
                        } else {
                            $scope.Employee.hrmE_LeftFlag = false;
                        }
                        if (promise.employeedetailList[0].hrmE_RetiredFlg == 1) {
                            $scope.Employee.hrmE_RetiredFlg = true;
                        } else {
                            $scope.Employee.hrmE_RetiredFlg = false;
                        }
                        if (promise.employeedetailList[0].hrmE_ExcPunch == 1) {
                            $scope.Employee.hrmE_ExcPunch = true;
                        } else {
                            $scope.Employee.hrmE_ExcPunch = false;
                        }
                        if (promise.employeedetailList[0].hrmE_PensionStoppedDate != null) {
                            $scope.Employee.hrmE_PensionStoppedDate = new Date(promise.employeedetailList[0].hrmE_PensionStoppedDate);
                        }
                        else {
                            $scope.Employee.hrmE_PensionStoppedDate = null;
                        }

                        //Contact Details

                        // communication address data
                        $scope.ComAddress.hrmE_LocStreet = promise.employeedetailList[0].hrmE_LocStreet;
                        $scope.ComAddress.hrmE_LocArea = promise.employeedetailList[0].hrmE_LocArea;
                        $scope.ComAddress.hrmE_LocCountryId = promise.employeedetailList[0].hrmE_LocCountryId;
                        // $scope.ComAddress.hrmE_LocStateId = promise.employeedetailList[0].hrmE_LocStateId;
                        $scope.ComAddress.hrmE_LocCity = promise.employeedetailList[0].hrmE_LocCity;
                        $scope.ComAddress.hrmE_LocPincode = promise.employeedetailList[0].hrmE_LocPincode;

                        var LocCountryId = promise.employeedetailList[0].hrmE_LocCountryId;
                        var LocStateId = promise.employeedetailList[0].hrmE_LocStateId;

                        if ((LocCountryId != null && LocCountryId != "") && (LocStateId != null && LocStateId != "")) {

                            getSelectedCommState(LocCountryId, LocStateId);
                        }

                        // Permenent address data
                        $scope.PerAddress.hrmE_PerStreet = promise.employeedetailList[0].hrmE_PerStreet;
                        $scope.PerAddress.hrmE_PerArea = promise.employeedetailList[0].hrmE_PerArea;
                        $scope.PerAddress.hrmE_PerCountryId = promise.employeedetailList[0].hrmE_PerCountryId;                        
                        $scope.PerAddress.hrmE_PerCity = promise.employeedetailList[0].hrmE_PerCity;
                        $scope.PerAddress.hrmE_PerPincode = promise.employeedetailList[0].hrmE_PerPincode;

                        var PerCountryId = promise.employeedetailList[0].hrmE_PerCountryId;
                        var PerStateId = promise.employeedetailList[0].hrmE_PerStateId;

                        if ((PerCountryId != null && PerCountryId != "") && (PerStateId != null && PerStateId != "")) {

                            getSelectedPerState(PerCountryId, PerStateId);
                        }


                        if (($scope.PerAddress.hrmE_PerStreet == $scope.ComAddress.hrmE_LocStreet) && ($scope.PerAddress.hrmE_PerArea == $scope.ComAddress.hrmE_LocArea) && ($scope.PerAddress.hrmE_PerCountryId == $scope.ComAddress.hrmE_LocCountryId) && ($scope.PerAddress.hrmE_PerStateId == $scope.ComAddress.hrmE_LocStateId) && ($scope.PerAddress.hrmE_PerCity == $scope.ComAddress.hrmE_LocCity) && ($scope.PerAddress.hrmE_PerPincode == $scope.ComAddress.hrmE_LocPincode)) {

                            $scope.ComAddress.chkbox_address = 1;
                            $scope.PermanentDis = true;
                            $scope.CommunicationAdDis = true;
                        }
                        else {
                            $scope.ComAddress.chkbox_address = 0;
                            $scope.PermanentDis = false;
                            $scope.CommunicationAdDis = false;
                        }


                    }

                    //employee bank details
                    if (promise.employeebankDetails != null && promise.employeebankDetails.length > 0) {
                        $scope.employeebankDetails = promise.employeebankDetails;

                        var newItemNo2 = $scope.employeebankDetails.length;
                        for (var i = 0; i < newItemNo2; i++) {
                            if ($scope.employeebankDetails[i].hrmeB_ActiveFlag == "default") {
                                $scope.disabled3 = true;
                                break;
                            }
                            else {
                                $scope.disabled3 = false;
                            }
                        }

                    }

                    //MB
                    

                    if ($scope.Employee.hrmE_DOB!=null) {
                        $scope.calcage($scope.Employee.hrmE_DOB);
                    }

                   // $scope.checkDOBSelected($scope.Employee.hrmE_DOJ);
                    //MB

                    //Qualification details
                    if (promise.qualificationDetails != null && promise.qualificationDetails.length > 0) {

                        $scope.qualificationDetails = promise.qualificationDetails;

                        angular.forEach(promise.qualificationDetails, function (value, key) {

                            // $scope.qualificationDetails[key].SpecialisationFlag = value.hrmC_Id;

                            if (value.hrmeQ_Date != null) {
                                $scope.qualificationDetails[key].hrmeQ_Date = new Date(value.hrmeQ_Date);
                            }
                            else {
                                $scope.qualificationDetails[key].hrmeQ_Date = null;
                            }
                            //$scope.qulyr($scope.qualificationDetails[key].hrmeQ_YearOfPassing, $scope.qualificationDetails[key]);
                        });
                    }

                    //Experience details

                    if (promise.experienceDetails != null && promise.experienceDetails.length > 0) {

                        $scope.experienceDetails = promise.experienceDetails;

                        $scope.EmployeeExperience = 1;
                        angular.forEach(promise.experienceDetails, function (value, key) {

                            if (value.hrmeE_JoinDate != null) {
                                $scope.experienceDetails[key].hrmeE_JoinDate = new Date(value.hrmeE_JoinDate);
                            }
                            else {
                                $scope.experienceDetails[key].hrmeE_JoinDate = null;
                            }

                            if (value.hrmeE_ExitDate != null) {
                                $scope.experienceDetails[key].hrmeE_ExitDate = new Date(value.hrmeE_ExitDate);
                            }
                            else {
                                $scope.experienceDetails[key].hrmeE_ExitDate = null;
                            }
                        });
                    } else {
                        $scope.EmployeeExperience = 0;
                    }

                    //Documents Details

                    if (promise.documentList != null && promise.documentList.length > 0) {

                        $scope.documentList = promise.documentList;
                        //
                        //angular.forEach($scope.documentList, function (value, key) {

                        //    //$('#' + value.hrmedS_Id).attr('src', value.hrmedS_DocumentImageName);

                        //  //  document.getElementById(""+ value.hrmedS_Id+"").innerHTML = value.hrmedS_DocumentImageName;

                        //});
                   }



                    //-----------------------  Fetch Multiple mobile no and Email Id --------------
                    // Fetch Multiple mobile no and Email Id 

                    if (promise.selectedmobile_list_dto != null && promise.selectedmobile_list_dto.length > 0) {
                        $scope.mobilesstd = promise.selectedmobile_list_dto;

                        var newItemNo1 = $scope.mobilesstd.length;
                        for (var i = 0; i < newItemNo1; i++) {
                            if ($scope.mobilesstd[i].hrmemnO_DeFaultFlag == "default") {
                                $scope.disabled = true;
                                break;
                            }
                            else {
                                $scope.disabled = false;
                            }
                        }

                    }
                    else {
                        $scope.mobilesstd.length = 1;
                        for (var i = 0; i < $scope.mobilesstd.length; i++) {
                            $scope.mobilesstd[i].hrmE_Id = 0;
                            $scope.mobilesstd[i].hrmemnO_Id = 0;
                            $scope.mobilesstd[i].hrmemnO_MobileNo = "";
                            $scope.mobilesstd[i].hrmemnO_DeFaultFlag = "";

                        }


                    }

                    //Employee email
                    if (promise.selectedemail_list_dto != null && promise.selectedemail_list_dto.length > 0) {
                        $scope.emailsstd = promise.selectedemail_list_dto;

                        var newItemNo2 = $scope.emailsstd.length;
                        for (var i = 0; i < newItemNo2; i++) {
                            if ($scope.emailsstd[i].hrmeM_DeFaultFlag == "default") {
                                $scope.disabled2 = true;
                                break;
                            }
                            else {
                                $scope.disabled2 = false;
                            }
                        }

                    }
                    else {
                        $scope.emailsstd.length = 1;

                        for (var i = 0; i < $scope.emailsstd.length; i++) {
                            $scope.emailsstd[i].hrmE_Id = 0;
                            $scope.emailsstd[i].hrmeeM_Id = 0;
                            $scope.emailsstd[i].hrmeM_EmailId = "";
                            $scope.emailsstd[i].hrmeM_DeFaultFlag = "";
                        }


                    }

              
                })
        }





        //get permenent address state while editing
        function getSelectedPerState(countryidd, stateid) {
            apiService.getURI("EmployeeRegistration/getstateDropdownByCountryId", countryidd).then(function (promise) {
                $scope.allPerState = [{ "ivrmmS_Id": 0, "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.PerAddress.hrmE_PerStateId = sts;
                $scope.data = promise.allState;
                $scope.allPerState.push.apply($scope.allPerState, $scope.data);
            })
        }

        //get Communication address state while editing
        function getSelectedCommState(countryidd, stateid) {
            apiService.getURI("EmployeeRegistration/getstateDropdownByCountryId", countryidd).then(function (promise) {
                $scope.allComState = [{ "ivrmmS_Id": 0, "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.ComAddress.hrmE_LocStateId = sts;

                $scope.data2 = promise.allState;
                $scope.allComState.push.apply($scope.allComState, $scope.data2);
            })
        }

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmE_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.getURI("EmployeeRegistration/ActiveDeactiveRecord", data.hrmE_Id).
                   then(function (promise) {


                       if (promise.retrunMsg !== "") {

                           if (promise.retrunMsg === "Activated") {
                               swal("Record Activated successfully");
                           }
                           else if (promise.retrunMsg === "Deactivated") {
                               swal("Record Deactivated successfully");
                           }
                           else {
                               swal("Record Not Activated/Deactivated", 'Fail');
                           }

                           if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                               // $scope.currentPage = 1;
                               // $scope.itemsPerPage = 10;

                               // $scope.employeedetailList = promise.employeedetailList;
                               $scope.gridOptions.data = promise.employeedetailList;
                           }
                       }

                   })
               }
               else {
                   swal(" Cancelled", "Ok");
               }
           }

           );
        }

        //Get Communication Satate by country
        $scope.onSelectGetComState = function (countryidd) {
            if (countryidd != "" && countryidd != undefined) {
                apiService.getURI("EmployeeRegistration/getstateDropdownByCountryId", countryidd).then(function (promise) {
                    $scope.allComState = promise.allState;
                })
            }

        }

        //Get Permanet Satate by country
        $scope.onSelectGetPerState = function (countryidd) {
            if (countryidd != "" && countryidd != undefined) {
                apiService.getURI("EmployeeRegistration/getstateDropdownByCountryId", countryidd).then(function (promise) {
                    $scope.allPerState = promise.allState;
                })
            }
        }

        //upload Employee Profile pic

        $scope.UploadEmployeeProfilePic = [];

        $scope.uploadEmployeeProfilePic = function (input, document) {
            $scope.UploadEmployeeProfilePic = input.files;
            $('#blah').removeAttr('src');

            if (input.files && input.files[0]) {

                if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/jpg" || input.files[0].type == "image/gif" || input.files[0].type == "image/png" || input.files[0].type == "image/jfif" || input.files[0].type == "image/EPS") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();  

                    reader.onload = function (e) {

                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofile();

                }
                else if (input.files[0].type != "image/jpeg" || input.files[0].type != "image/jpg" || input.files[0].type != "image/gif" || input.files[0].type != "image/png" || input.files[0].type != "image/jfif" || input.files[0].type != "image/EPS") {
                    swal("Please Upload the jpeg/jpg/gif/png/jfif/EPS format file");
                    angular.element("input[type='file']").val(null);
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    angular.element("input[type='file']").val(null);
                    return;
                }

            }
        }
        function UploadEmployeeprofile() {

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
                    $scope.Employee.hrmE_Photo = d;
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

        //Document Uploading Function by Prashant Sindhol //

        $scope.SelectedFileForUploadzd = [];

        $scope.selectFileforUploadzd = function (input, document) {

            $('#' + document.id).removeAttr('src');

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                var filename = input.files[0].name.toString();

                var nameArray = filename.split('.');

                var extention = nameArray[nameArray.length - 1];


                if ((extention == "JPEG" || extention == "jpg" || extention == "docx" || extention == "doc" || extention == "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocument(document);

                }
                else if (extention == "JPEG" && extention != "jpg" && extention != "docx" && extention != "doc" && extention != "pdf") {

                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        }
        function UploadEmployeeDocument(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/EmployeeDocumentUpload/UploadEmployeeDocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
            .success(function (d) {

                defer.resolve(d);
                if (d != "PathNotFound") {
                    data.hrmedS_DocumentImageName = d;
                } else {
                    swal('Document Storage Path Not Found ..!!');
                }

                //
            })
            .error(function () {
                $('#' + data.id).removeAttr('src');
                defer.reject("File Upload Failed!");
            });

        }
        //Check duplicate Employee Order  
        $scope.checkDuplicateEmployeeOrder = function (EmployeeOrder) {

            if (EmployeeOrder != "" && EmployeeOrder != undefined) {

                var EmployeeOrderdata = {
                    "HRME_EmployeeOrder": EmployeeOrder,
                    "Type": "EmployeeOrder",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmployeeOrderdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {
                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Employee Order is Already Exist !');
                                $scope.Employee.hrmE_EmployeeOrder = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })

            }
        }


        //Check duplicate Mobile Number
        $scope.checkDuplicatemobileNo = function (mobileNo) {
            if (mobileNo != "" && mobileNo != undefined && mobileNo.length == 10) {

                var mobdata = {
                    "HRME_MobileNo": mobileNo,
                    "Type": "mobileNo",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", mobdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {
                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Mobile Number is Already Exist !');
                                $scope.Employee.hrmE_MobileNo = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })

            }
        }


        //Check duplicate EmailId
        $scope.checkDuplicateEmailID = function (EmailID) {

            if (EmailID != "" && EmailID != undefined) {

                var re = /\S+@\S+\.\S+/;

                if (re.test(EmailID)) {

                    var EmailIDdata = {
                        "HRME_EmailId": EmailID,
                        "Type": "EmailID",
                        "HRME_Id": $scope.Employee.hrmE_Id
                    }
                    apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                        then(function (promise) {

                            if (promise.retrunMsg != "") {

                                if (promise.retrunMsg == "Duplicate") {
                                    swal('Duplicate !', 'Email-Id is Already Exist !');
                                    $scope.Employee.hrmE_EmailId = "";
                                    return;

                                } else if (promise.retrunMsg == "error") {

                                    swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                    return;
                                }
                            }
                        })
                }



            }

        }


        //Check duplicate AdharNumber
        $scope.checkDuplicateAdharNumber = function (AadharCardNo) {
            if (AadharCardNo != "" && AadharCardNo != undefined && AadharCardNo.length == 12) {

                var mobdata = {
                    "HRME_AadharCardNo": AadharCardNo,
                    "Type": "AadharCardNo",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", mobdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {
                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Aadhar Card Number is Already Exist !');
                                $scope.Employee.hrmE_AadharCardNo = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })

            }

        }


        //Check duplicate Pan Number

        // $scope.panCardRegex = '/[A-Z]{5}\d{4}[A-Z]{1}/i';

        $scope.checkDuplicatePanNumber = function (res) {

            var PanNumber = res.toUpperCase();

            if (PanNumber != "" && PanNumber != undefined && PanNumber.length == 10) {
                var mobdata = {
                    "HRME_PANCardNo": PanNumber,
                    "Type": "PANNumber",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", mobdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {
                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'PAN Number is Already Exist !');
                                $scope.Employee.hrmE_PANCardNo = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })

            }

        }


        //Spouse details

        //Check duplicate Employee Order  
        $scope.checkDuplicateEmployeeSpouseMobileNo = function (hrmE_SpouseMobileNo) {

            if (hrmE_SpouseMobileNo != "" && hrmE_SpouseMobileNo != undefined) {

                var EmployeeSpouseMobileNo = {
                    "HRME_SpouseMobileNo": hrmE_SpouseMobileNo,
                    "Type": "SpouseMobileNo",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmployeeSpouseMobileNo).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {
                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', ' MobileNo is Already Exist !');
                                $scope.Employee.hrmE_SpouseMobileNo = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })

            }
        }


        //

        //Check duplicate SpouseEmailId
        $scope.checkDuplicateEmployeeSpouseEmailId = function (hrmE_SpouseEmailId) {

            if (hrmE_SpouseEmailId != "" && hrmE_SpouseEmailId != undefined) {

                var re = /\S+@\S+\.\S+/;

                if (re.test(hrmE_SpouseEmailId)) {

                    var EmailIDdata = {
                        "HRME_SpouseEmailId": hrmE_SpouseEmailId,
                        "Type": "SpouseEmailId",
                        "HRME_Id": $scope.Employee.hrmE_Id
                    }
                    apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                        then(function (promise) {

                            if (promise.retrunMsg != "") {

                                if (promise.retrunMsg == "Duplicate") {
                                    swal('Duplicate !', 'Email-Id is Already Exist !');
                                    $scope.Employee.hrmE_SpouseEmailId = "";
                                    return;

                                } else if (promise.retrunMsg == "error") {

                                    swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                    return;
                                }
                            }
                        })
                }
            }

        }



        //Check duplicate BiometricCode
        $scope.checkDuplicateEmployeeBiometricCode = function (hrmE_BiometricCode) {

            if (hrmE_BiometricCode != "" && hrmE_BiometricCode != undefined) {

                var EmailIDdata = {
                    "HRME_BiometricCode": hrmE_BiometricCode,
                    "Type": "BiometricCode",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Biometric Code is Already Exist !');
                                $scope.Employee.hrmE_BiometricCode = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })
            }
        }



        //Check duplicate EmployeeCode
        $scope.checkDuplicateEmployeeEmployeeCode = function (hrmE_EmployeeCode) {

            if (hrmE_EmployeeCode != "" && hrmE_EmployeeCode != undefined) {

                var EmailIDdata = {
                    "HRME_EmployeeCode": hrmE_EmployeeCode,
                    "Type": "EmployeeCode",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Employee Code is Already Exist !');
                                $scope.Employee.hrmE_EmployeeCode = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })
            }
        }


        //Check duplicate RFCardId
        $scope.checkDuplicateEmployeeRFCardId = function (hrmE_RFCardId) {

            if (hrmE_RFCardId != "" && hrmE_RFCardId != undefined) {

                var EmailIDdata = {
                    "HRME_RFCardId": hrmE_RFCardId,
                    "Type": "RFCardId",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Employee RF Card Id is Already Exist !');
                                $scope.Employee.hrmE_RFCardId = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })
            }
        }


        //Check duplicate PFAccNo  checkDuplicateEmployeeUANNo
        $scope.checkDuplicateEmployeePFAccNo = function (hrmE_PFAccNo) {

            if (hrmE_PFAccNo != "" && hrmE_PFAccNo != undefined) {

                var EmailIDdata = {
                    "HRME_PFAccNo": hrmE_PFAccNo,
                    "Type": "PFAccNo",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Employee PF Acc No. is Already Exist !');
                                $scope.Employee.hrmE_PFAccNo = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })
            }
        }


        //Check duplicate UANNo  
        $scope.checkDuplicateEmployeeUANNo = function (hrmE_UINumber) {

            if (hrmE_UINumber != "" && hrmE_UINumber != undefined) {

                var EmailIDdata = {
                    "HRME_UINumber": hrmE_UINumber,
                    "Type": "UINumber",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Employee UI Numbe. is Already Exist !');
                                $scope.Employee.hrmE_UINumber = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })
            }
        }


        //Check duplicate ESIAccNo
        $scope.checkDuplicateEmployeeESIAccNo = function (hrmE_ESIAccNo) {

            if (hrmE_ESIAccNo != "" && hrmE_ESIAccNo != undefined) {

                var EmailIDdata = {
                    "HRME_ESIAccNo": hrmE_ESIAccNo,
                    "Type": "ESIAccNo",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Employee ESI Acc No. is Already Exist !');
                                $scope.Employee.hrmE_ESIAccNo = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })
            }
        }


        //Check duplicate GratuityAccNo
        $scope.checkDuplicateEmployeeGratuityAccNo = function (hrmE_GratuityAccNo) {

            if (hrmE_GratuityAccNo != "" && hrmE_GratuityAccNo != undefined) {

                var EmailIDdata = {
                    "HRME_GratuityAccNo": hrmE_GratuityAccNo,
                    "Type": "GratuityAccNo",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Employee Gratuity Acc No. is Already Exist !');
                                $scope.Employee.hrmE_GratuityAccNo = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })
            }
        }


        //Check duplicate NationalSSN
        $scope.checkDuplicateEmployeeNationalSSN = function (hrmE_NationalSSN) {

            if (hrmE_NationalSSN != "" && hrmE_NationalSSN != undefined && hrmE_NationalSSN.length == 9) {

                var EmailIDdata = {
                    "HRME_NationalSSN": hrmE_NationalSSN,
                    "Type": "NationalSSN",
                    "HRME_Id": $scope.Employee.hrmE_Id
                }
                apiService.create("EmployeeRegistration/validateData", EmailIDdata).
                    then(function (promise) {

                        if (promise.retrunMsg != "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal('Duplicate !', 'Employee National SSN. is Already Exist !');
                                $scope.Employee.hrmE_NationalSSN = "";
                                return;

                            } else if (promise.retrunMsg == "error") {

                                swal('Something went Wrong', 'Kindly Contact to Administrator!');
                                return;
                            }
                        }
                    })
            }
        }


        ////hemalekha
        //$scope.qulyr = function (qulyrr, obj) {
        //    
        //    var tsdob = $scope.Employee.hrmE_DOB;
        //    var ttsdob = tsdob.getFullYear();
        //    var tsdoj = $scope.Employee.hrmE_DOJ;
        //    var ttsdoj = tsdoj.getFullYear();
        //    var qulyrrrr = parseInt(qulyrr);
          
        //    if (ttsdob < qulyrrrr && qulyrrrr <= ttsdoj) {
        //        var year_diff = ttsdob + 16;
        //        if (qulyrrrr >= year_diff)
        //        {
        //            var quldatee = new Date(qulyrr);

        //            //$scope.maxDateQDatequl = new Date(
        //            //  quldatee.getFullYear(),
        //            //  quldatee.getMonth() + 12,
        //            //  quldatee.getDate()
        //            //  );
        //            //$scope.minDateQDatequl = new Date(
        //            //quldatee.getFullYear(),
        //            //quldatee.getMonth(),
        //            //quldatee.getDate()
        //            //);
        //        }
        //        else {
        //            swal("Please Enter Valid Year Of Passing.Eg.16 years from DOB");
        //            obj.hrmeQ_YearOfPassing = "";
        //        }
        //            $scope.maxDateQDatequl = new Date(
        //              quldatee.getFullYear(),
        //              quldatee.getMonth()+12,
        //              quldatee.getDate()-1
        //              );
        //            $scope.minDateQDatequl = new Date(
        //            quldatee.getFullYear(),
        //            quldatee.getMonth(),
        //            quldatee.getDate()
        //            );
             
               
        //    }
        //    else {

        //      //  swal("Please Enter Valid Year Of Passing.");
        //        //  angular.foreach(qualificationDetails)
        //        // $scope.hrmeQ_YearOfPassing = "";

        //        //obj.hrmeQ_YearOfPassing = "";
        //        //   $scope.eerormsg = "Year should be between DOB and DOJ";
        //    }

        //}






        //hemalekha

        $scope.calcage = function (Bdate) {
            
            var yearRetirement = Bdate.getFullYear() + parseInt($scope.RetirementYrs);
            var monthRetirement = Bdate.getMonth();
            var dateRetirement = Bdate.getDate();

            $scope.Employee.hrmE_ExpectedRetirementDate = new Date(yearRetirement,
                               monthRetirement,
                               dateRetirement
                               );


            //Set Qualification minimum date
          //  $scope.minDateQDate = Bdate;
            //$scope.maxDateQDate = Bdate;
            //   $scope.maxDateDOJ = Bdate;

            //$scope.minDateJoin = new Date(
            //     Bdate.getFullYear() + 18,// - parseInt($scope.RetirementYrs),
            //     Bdate.getMonth(),
            //     Bdate.getDate()
            //     );

           // $scope.minDateJoin = Bdate;

            //Set Experience minimum date
            //  $scope.minDateJoin = Bdate;




        }



        //// Experience join date

        $scope.SetExitDateData = function (experience) {

        //    experience.hrmeE_NoOfYears = "";
        //    experience.hrmeE_NoOfMonths = "";
$scope.minDateExitDate=experience.hrmeE_JoinDate
            if (experience.hrmeE_JoinDate != "" && experience.hrmeE_JoinDate != undefined) {

               experience.exitDateDis = false;

        //        $scope.ToDate = experience.hrmeE_JoinDate;
        //        $scope.minDateExit = new Date(
        //        $scope.ToDate.getFullYear(),
        //        $scope.ToDate.getMonth(),
        //        $scope.ToDate.getDate());

        //        $scope.maxDateExit = new Date();
        //        experience.hrmeE_ExitDate = "";


           }

        }

        //Calculate Experience
        $scope.NoOfYearsDis = true;
        $scope.NoOfMonthsDis = true;

        // $scope.exitDateDis = true;

        $scope.CalculateExperience = function (experience) {

            experience.hrmeE_NoOfYears = "";
            experience.hrmeE_NoOfMonths = "";

            var joindate = new Date($filter('date')(new Date(experience.hrmeE_JoinDate).toDateString(), "yyyy/MM/dd"));
            var leftdate = new Date($filter('date')(new Date(experience.hrmeE_ExitDate).toDateString(), "yyyy/MM/dd"));

            var exp = $scope.CalDate(leftdate, joindate);

            exp = exp.split(",");

            experience.hrmeE_NoOfYears = exp[0];
            experience.hrmeE_NoOfMonths = exp[1];
            // alert(exp)
            // return exp;
            if (experience.hrmeE_ExitDate > experience.hrmeE_JoinDate) {

            } else {
                swal('Date Of Leaving should be greater than Date of join.. !!');
                $scope.experience.hrmeE_ExitDate = "";
                return;
            }
        }

        $scope.CalDate = function (date1, date2) {



            var diffnew = new Date(
                  date1.getFullYear() - date2.getFullYear(),
                  date1.getMonth() - date2.getMonth(),
                  (date1.getDate() + 1) - (date2.getDate())
              );

            //console.log(diffnew.getYear(), "Year(s),", diffnew.getMonth(), "Month(s), and", diffnew.getDate(), "Days.");

            //var isLeap = !(new Date(diffnew.getYear(), 1, 29).getMonth() - 1);

            //console.log(isLeap);
            var message = diffnew.getYear() + "," + diffnew.getMonth();

            return message
        };


        $scope.SetCGPAorPerFlag = function (qualification) {
            if (qualification.hrmeQ_CGPAOrPerFlag != "") {

                if (qualification.hrmeQ_CGPAOrPerFlag == "CGPA") {

                    qualification.hrmeQ_Percentage = "";
                    $scope.myForm3.$setPristine();
                    $scope.myForm3.$setUntouched();

                }
                else if (qualification.hrmeQ_CGPAOrPerFlag == "Percentage") {

                    qualification.hrmeQ_CGPA = "";

                    $scope.myForm3.$setPristine();
                    $scope.myForm3.$setUntouched();

                }

            } else {
                qualification.hrmeQ_CGPA = "";
                qualification.hrmeQ_Percentage = "";
            }


        }


        $scope.chkbox_addressDis = true;

        $scope.checkboxValidate = function (ComAddress) {

            if ((ComAddress.hrmE_LocStreet != "" && ComAddress.hrmE_LocStreet != undefined) && (ComAddress.hrmE_LocCountryId != "" && ComAddress.hrmE_LocCountryId != undefined) && (ComAddress.hrmE_LocStateId != "" && ComAddress.hrmE_LocStateId != undefined) && (ComAddress.hrmE_LocCity != "" && ComAddress.hrmE_LocCity != undefined) && (ComAddress.hrmE_LocPincode != "" && ComAddress.hrmE_LocPincode != undefined)) {
                $scope.chkbox_addressDis = false;
            }
            else {
                $scope.chkbox_addressDis = true;
            }

        }


        //preview document

        $scope.showmodaldetails = function (data) {

            $('#preview').removeAttr('src');
            var filename = data.hrmedS_DocumentImageName.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];
            if (extention == "jpg" || extention == "jpeg")
            {
                // $('#preview').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeDocuments" + "/" + data.hrmedS_DocumentImageName);
                $('#preview').attr('src', data.hrmedS_DocumentImageName);
            }
            else if (extention == "doc" || extention == "docx")
            {
                $('#preview').removeAttr('src');
            }
            else if (extention == "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
                //var innerContents = document.getElementById("pdffileDiv").innerHTML;
                //var popupWinindow = window.open('');
                //popupWinindow.document.open();
                //popupWinindow.document.write('<html><head>' +
                //'</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                //popupWinindow.document.close();
            }
        }


        //$scope.netSalary = 0;
        //$scope.EarningTotal = 0;
        //$scope.DeductionTotal = 0;
        //$scope.ArrearTotal = 0;
        // Onload Get Employee Salary Details

        $scope.getSalaryDetails = function () {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {
                $scope.netSalary = 0;
                $scope.EarningTotal = 0;
                $scope.DeductionTotal = 0;
                $scope.ArrearTotal = 0;

                var data = $scope.Employee;
                apiService.create("EmployeeRegistration/getEmployeeSalaryDetails", data).
                    then(function (promise) {
                        //earning & deduction details

                        if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {

                            var employeeEarningsDeductionsDetails = promise.employeeEarningsDeductionsDetails;


                            var EarningDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Earning';
                            });

                            var DeductionDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Deduction';
                            });

                            var ArrearDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Arrear';
                            });

                            var GrossDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Gross';
                            });
                            
                            if (GrossDetails != null && GrossDetails.length > 0) {
                                $scope.Salary = GrossDetails[0];
                                $scope.Salary.hreeD_ActiveFlag = GrossDetails[0].hreeD_ActiveFlag;

                            } else {
                                $scope.Salary.hreeD_Percentage = '0.00';
                                $scope.Salary.hreeD_Amount = '0.00';
                                $scope.Salary.hrmeD_AppPercent = '0.00';
                                $scope.Salary.hrmeD_Percent = '0.00';
                                $scope.Salary.hrmeD_Details = '';
                                $scope.Salary.hrmeD_Id = 0;
                                $scope.Salary.hreeD_ActiveFlag = true;
                            }

                            //Earning List
                            var totalEarning = 0;
                            angular.forEach($scope.earningList, function (value, key) {

                                angular.forEach(EarningDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        // $scope.earningList[key].Selected = true;
                                        $scope.earningList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.earningList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.earningList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.earningList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.earningList[key].hrmE_Id = value1.hrmE_Id;
                                        $scope.earningList[key].hreeD_ApplicableMaxValue = value1.hreeD_ApplicableMaxValue;
                                        $scope.earningList[key].hreeD_MaxApplicableFlg = value1.hreeD_MaxApplicableFlg;

                                        totalEarning = totalEarning + value1.hreeD_Amount;

                                    }

                                });


                            });


                            //deductionlist
                            var totalDeduction = 0;
                            angular.forEach($scope.detectionList, function (value, key) {




                                angular.forEach(DeductionDetails, function (value1, key1) {
                                    
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        //  $scope.detectionList[key].Selected = true;
                                        $scope.detectionList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.detectionList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.detectionList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.detectionList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.detectionList[key].hreeD_ApplicableMaxValue = value1.hreeD_ApplicableMaxValue;
                                        $scope.detectionList[key].hreeD_MaxApplicableFlg = value1.hreeD_MaxApplicableFlg;
                                        $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                        if (value.hrmeD_EDTypeFlag == 'PF') {
                                            if ($scope.Employee.hrmE_PFFixedFlag) {
                                                $scope.detectionList[key].AmountDis = false;
                                                $scope.detectionList[key].PercentDis = false;
                                            } else {
                                                if (value.hrmeD_AmountPercentFlag == "Percentage") {
                                                    $scope.detectionList[key].AmountDis = true;
                                                    $scope.detectionList[key].PercentDis = false;

                                                } else {
                                                    $scope.detectionList[key].AmountDis = false;
                                                    $scope.detectionList[key].PercentDis = true;
                                                }
                                            }
                                        }

                                        totalDeduction = totalDeduction + value1.hreeD_Amount;
                                    }

                                });


                            });


                            //arrearlist
                            var totalArrear = 0;
                            angular.forEach($scope.arrearList, function (value, key) {

                                angular.forEach(ArrearDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        // $scope.arrearList[key].Selected = true;
                                        $scope.arrearList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.arrearList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.arrearList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.arrearList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.arrearList[key].hreeD_ApplicableMaxValue = value1.hreeD_ApplicableMaxValue;
                                        $scope.arrearList[key].hreeD_MaxApplicableFlg = value1.hreeD_MaxApplicableFlg;
                                        $scope.arrearList[key].hrmE_Id = value1.hrmE_Id;

                                        totalArrear = totalArrear + value1.hreeD_Amount;
                                    }

                                });

                            });

                            $scope.EarningTotal = $scope.getEarningTotal();
                            $scope.DeductionTotal = $scope.getDeductionTotal();
                            $scope.ArrearTotal = $scope.getArrearTotal();

                            $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;

                        }


                    });
            }



        }


        $scope.getEmployeeSalaryDetailsByHead = function (data) {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {

                $scope.netSalary = 0;
                $scope.EarningTotal = 0;
                $scope.DeductionTotal = 0;
                $scope.ArrearTotal = 0;
                
                apiService.create("EmployeeRegistration/getEmployeeSalaryDetailsByHead", data).
                    then(function (promise) {
                        //earning & deduction details

                        if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {

                            var employeeEarningsDeductionsDetails = promise.employeeEarningsDeductionsDetails;


                            var EarningDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Earning';
                            });

                            var DeductionDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Deduction';
                            });

                            var ArrearDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Arrear';
                            });

                            var GrossDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Gross';
                            });
                            
                            if (GrossDetails != null && GrossDetails.length > 0) {
                                $scope.Salary = GrossDetails[0];
                                $scope.Salary.hreeD_ActiveFlag = GrossDetails[0].hreeD_ActiveFlag;

                            } else {
                                $scope.Salary.hreeD_Percentage = '0.00';
                                $scope.Salary.hreeD_Amount = '0.00';
                                $scope.Salary.hrmeD_AppPercent = '0.00';
                                $scope.Salary.hrmeD_Percent = '0.00';
                                $scope.Salary.hreeD_ApplicableMaxValue = '0.00';
                                $scope.Salary.hrmeD_Details = '';
                                $scope.Salary.hrmeD_Id = 0;
                                $scope.Salary.hreeD_ActiveFlag = true;
                            }

                            var totalEarning = 0;
                            angular.forEach($scope.earningList, function (value, key) {

                                angular.forEach(EarningDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        // $scope.earningList[key].Selected = true;
                                        $scope.earningList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.earningList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.earningList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.earningList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        //$scope.earningList[key].hreeD_ApplicableMaxValue = value1.hreeD_ApplicableMaxValue;
                                        //$scope.earningList[key].hreeD_MaxApplicableFlg = value1.hreeD_MaxApplicableFlg;
                                        $scope.earningList[key].hrmE_Id = value1.hrmE_Id;

                                        totalEarning = totalEarning + value1.hreeD_Amount;
                                    }

                                });


                            });


                            //deductionlist
                            var totalDeduction = 0;
                            angular.forEach($scope.detectionList, function (value, key) {

                                angular.forEach(DeductionDetails, function (value1, key1) {
                                    
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        //  $scope.detectionList[key].Selected = true;
                                        $scope.detectionList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.detectionList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.detectionList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.detectionList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        //$scope.detectionList[key].hreeD_ApplicableMaxValue = value1.hreeD_ApplicableMaxValue;
                                        //$scope.detectionList[key].hreeD_MaxApplicableFlg = value1.hreeD_MaxApplicableFlg;
                                        $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                        if (value.hrmeD_EDTypeFlag == 'PF') {
                                            if ($scope.Employee.hrmE_PFFixedFlag) {
                                                $scope.detectionList[key].AmountDis = false;
                                                $scope.detectionList[key].PercentDis = false;
                                            } else {
                                                if (value.hrmeD_AmountPercentFlag == "Percentage") {
                                                    $scope.detectionList[key].AmountDis = true;
                                                    $scope.detectionList[key].PercentDis = false;

                                                } else {
                                                    $scope.detectionList[key].AmountDis = false;
                                                    $scope.detectionList[key].PercentDis = true;
                                                }
                                            }
                                        }

                                        totalDeduction = totalDeduction + value1.hreeD_Amount;
                                    }

                                });


                            });


                            //arrearlist
                            var totalArrear = 0;
                            angular.forEach($scope.arrearList, function (value, key) {

                                angular.forEach(ArrearDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        // $scope.arrearList[key].Selected = true;
                                        $scope.arrearList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.arrearList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.arrearList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.arrearList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        //$scope.arrearList[key].hreeD_ApplicableMaxValue = value1.hreeD_ApplicableMaxValue;
                                        //$scope.arrearList[key].hreeD_MaxApplicableFlg = value1.hreeD_MaxApplicableFlg;
                                        $scope.arrearList[key].hrmE_Id = value1.hrmE_Id;

                                        totalArrear = totalArrear + value1.hreeD_Amount;
                                    }

                                });

                            });


                            $scope.EarningTotal = $scope.getEarningTotal();
                            $scope.DeductionTotal = $scope.getDeductionTotal();
                            $scope.ArrearTotal = $scope.getArrearTotal();

                            $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;

                        }


                    });


            }

        }





        //Set Employee Order

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement

                }
            };

        };
        $scope.init = function () {

            $scope.resetLists();

        };
        $scope.init();

        $scope.setEmployeeOrder = function (orderarray) {


            angular.forEach(orderarray, function (value, key) {
                if (value.hrmE_Id !== 0) {
                    orderarray[key].hrmE_EmployeeOrder = key + 1;
                }
            });
            var data = {
                EmporderDTO: orderarray,
            }
            apiService.create("EmployeeRegistration/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.onLoadGetData();
                    }


                });
        }


        //RemoveQualificationDetail

        $scope.DeleteQualificationData = function (data, SweetAlert) {


            var mgs = "Delete";
            var confirmmgs = "Deleted";

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee Qualification Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.create("EmployeeRegistration/DeleteQualificationRecord", data).
                   then(function (promise) {


                       if (promise.retrunMsg !== "") {

                           if (promise.retrunMsg === "Deleted") {
                               swal("Record Deleted successfully");
                           }

                           else {
                               swal("Record Not Deleted", 'Fail');
                           }

                       }

                   })
               }
               else {
                   swal(" Cancelled", "Ok");
               }
           }

           );


        }

        //DeleteExperienceData

        $scope.DeleteExperienceData = function (data, SweetAlert) {


            var mgs = "Delete";
            var confirmmgs = "Deleted";

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee Experience Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.create("EmployeeRegistration/DeleteExperienceRecord", data).
                   then(function (promise) {


                       if (promise.retrunMsg !== "") {

                           if (promise.retrunMsg === "Deleted") {
                               swal("Record Deleted successfully");
                           }

                           else {
                               swal("Record Not Deleted", 'Fail');
                           }

                       }

                   })
               }
               else {
                   swal(" Cancelled", "Ok");
               }
           }

           );


        }

        //DeleteDocumentData

        $scope.DeleteDocumentData = function (data, SweetAlert) {


            var mgs = "Delete";
            var confirmmgs = "Deleted";

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Employee Document Details ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.create("EmployeeRegistration/DeleteDocumentRecord", data).
                   then(function (promise) {


                       if (promise.retrunMsg !== "") {

                           if (promise.retrunMsg === "Deleted") {
                               swal("Record Deleted successfully");
                           }

                           else {
                               swal("Record Not Deleted", 'Fail');
                           }

                       }

                   })
               }
               else {
                   swal(" Cancelled", "Ok");
               }
           }

           );
        }

        $scope.ShowSpouseDetails = false;
        $scope.setMaritalStatus = function (ivrmmmS_Id) {

            if (ivrmmmS_Id != undefined && ivrmmmS_Id != "") {

                var single_object = $filter('filter')($scope.maritalStatusdropdownlist, function (d) {
                    return d.ivrmmmS_Id === parseInt(ivrmmmS_Id);
                })[0].ivrmmmS_MaritalStatus;

                if (single_object.toUpperCase() === 'MARRIED') {

                    $scope.ShowSpouseDetails = true;

                } else {
                    $scope.ShowSpouseDetails = false;
                }

            } else {
                $scope.ShowSpouseDetails = false;
            }


        }


        $scope.getEmployeeExperience = function (Experienceflag) {

            if (Experienceflag == 0) {
                $scope.experienceDetails = [{ id: 'experience' }];
                $scope.submitted4 = false;
                $scope.myForm4.$setPristine();
                $scope.myForm4.$setUntouched();
            }

        }


        //Added by sudeep

        var vm = this;

        vm.gridOptions = {};
        var HostName = location.host;
        $scope.import_func = function () {
            $window.location.href = 'http://' + HostName + '/#/app/EmployeeDataImport/';
        }

        //excel import----------------------------------------------------
        $scope.SelectedFileForUploadzd = [];

        $scope.upload_emp_details = function (input) {

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                }
                else {
                    swal("Please Upload the .xlsx file");
                    return;
                }
            }


        }

        //-----------------------------------------------------------------

        $scope.chkdup_mob = function (user, index) {
            
            
            if (user.hrmemnO_MobileNo != "" && user.hrmemnO_MobileNo != undefined && user.hrmemnO_MobileNo != null) {

                var dublicate = false;
                for (var k = 0; k < $scope.mobilesstd.length; k++) {
                    var roll = parseInt(user.hrmemnO_MobileNo);
                    var arryind = $scope.mobilesstd.indexOf($scope.mobilesstd[k]);
                    //console.log(arryind);
                    if (arryind != index) {
                        if ($scope.mobilesstd[k].hrmemnO_MobileNo == roll) {

                            swal("Mobile Number Already Chosen");
                            $scope.mobilesstd[index].hrmemnO_MobileNo = "";
                            dublicate = true;
                            break;
                        }

                    }
                }
                if (!dublicate) {
                    var data = user;

                    apiService.create("EmployeeRegistration/chk_dup_mob", data)


                               .then(function (promise) {
                                   if (promise.tcflagexists == "Mobile Number Already Exists") {
                                       swal('Mobile Number Already Exists', 'Please Enter Different Mobile Number');

                                        //$scope.mobilesstd[index].hrmemnO_MobileNo = "";

                                   }

                               })
                }

               
            }



        };

        $scope.chkdup_email = function (user, index) {

            if ($scope.hrmeM_EmailId != null || $scope.hrmeM_EmailId != "" || $scope.hrmeM_EmailId != undefined) {
                for (var k = 0; k < $scope.emailsstd.length; k++) {
                    var roll = user.hrmeM_EmailId;
                    var arryind = $scope.emailsstd.indexOf($scope.emailsstd[k]);
                    //console.log(arryind);
                    if (arryind != index) {
                        if ($scope.emailsstd[k].hrmeM_EmailId == roll) {

                            swal("Eamil_Id Already Chosen");
                            // $scope.emailsstd[index].hrmeM_EmailId = "";

                            break;
                        }

                    }

                    var data =
                        {
                            "HRMEM_EmailId": user.hrmeM_EmailId
                        }
                    apiService.create("EmployeeRegistration/chk_dup_email", data)


                               .then(function (promise) {
                                   if (promise.tcflagexists == "Email_Id Already Exists") {
                                       swal('Email_Id Already Exists', 'Please Enter Different Email_Id');

                                       // $scope.emailsstd[index].hrmeM_EmailId = "";

                                   }

                               })
                }
            }

        };

        //adding mobile number

        $scope.mobilesstd = [{ id: 'mobilestd1' }];
      //  $scope.selmobsstd = [{ id: 'selmobilestd1' }];
        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.mobilesstd.length + 1;
            if (newItemNostd <= 5) {
                $scope.mobilesstd.push({ 'id': 'mobilestd' + newItemNostd });
            }


            if (newItemNostd == 4) {
                $scope.myForm1.$setPristine();
            }

        };

        //removing mobile number student
        $scope.delmsrd = [];
        $scope.removeNewMobile1std = function (index, curval1std) {

            if (curval1std.hrmemnO_Id != undefined) {

                var data =
                                                        {
                                                            "HRMEMNO_Id": curval1std.hrmemnO_Id
                                                        }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to delete the record?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },

                function (isConfirm) {
                    if (isConfirm) {


                        apiService.create("EmployeeRegistration/del_mob", data)


                                   .then(function (promise) {
                                       if (promise.retrunMsg == "Deleted") {
                                           swal('Record Deleted Successfully');

                                           var newItemNostd2 = $scope.mobilesstd.length - 1;
                                           if (newItemNostd2 !== 0) {
                                               $scope.delmsrd = $scope.mobilesstd.splice(index, 1);
                                           }

                                       }

                                   })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                        // $scope.onLoadGetData();
                    }
                })
            }

            if (curval1std.hrmemnO_Id == undefined) {
                var newItemNostd2 = $scope.mobilesstd.length - 1;
                if (newItemNostd2 !== 0) {
                    $scope.delmsrd = $scope.mobilesstd.splice(index, 1);
                }
            }

        };

        //---------------------------------------------------------------

        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd' + newItemNostd2 });
            }
        };

        $scope.removeNewEmail1std = function (index, id) {

            if (id.hrmeeM_Id != undefined) {

                var data =
                                                         {
                                                             "HRMEEM_Id": id.hrmeeM_Id
                                                         }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to delete the record?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },

                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("EmployeeRegistration/del_email", data)


                                   .then(function (promise) {
                                       if (promise.retrunMsg == "Deleted") {
                                           swal('Record Deleted Successfully');

                                           var newItemNostd = $scope.emailsstd.length - 1;
                                           if (newItemNostd !== 0) {
                                               $scope.delmsrd = $scope.emailsstd.splice(index, 1);


                                           }
                                       }

                                   })
                    }

                    else {
                        swal(" Cancelled", "Ok");

                    }
                })
            }


            if (id.hrmeeM_Id == undefined) {
                var newItemNostd = $scope.emailsstd.length - 1;
                if (newItemNostd !== 0) {
                    $scope.delmsrd = $scope.emailsstd.splice(index, 1);


                }
            }

        };


        $scope.defchange_mob = function (moM_Flag) {
            var newItemNo1 = $scope.mobilesstd.length;
            for (var i = 0; i < newItemNo1; i++) {
                if ($scope.mobilesstd[i].hrmemnO_DeFaultFlag == "default") {
                    $scope.disabled = true;
                    break;
                }
                else {
                    $scope.disabled = false;
                }
            }
        }


        $scope.defchange_email = function (moM_Flag) {

            var newItemNo2 = $scope.emailsstd.length;
            for (var i = 0; i < newItemNo2; i++) {
                if ($scope.emailsstd[i].hrmeM_DeFaultFlag == "default") {
                    $scope.disabled2 = true;
                    break;
                }
                else {
                    $scope.disabled2 = false;
                }
            }
        }

        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };


        $scope.validateSpecialization = function (data) {
            angular.forEach($scope.qualificationDetails, function (obj1) {
                angular.forEach($scope.coursedropdownlist, function (obj2) {
                    if (parseInt(obj1.hrmC_Id) == obj2.hrmC_Id) {
                        if (obj2.hrmC_SpecialisationFlag == true) {
                            obj1.reqfield = true;

                        }
                        else {
                            obj1.reqfield = false;
                        }
                    }
                });
            });
        };


        $scope.defchange_bank = function (moM_Flag) {

            var newItemNo2 = $scope.employeebankDetails.length;
            for (var i = 0; i < newItemNo2; i++) {
                if ($scope.employeebankDetails[i].hrmeB_ActiveFlag == "default") {
                    $scope.disabled3 = true;
                    break;
                }
                else {
                    $scope.disabled3 = false;
                }
            }
        };



        $scope.chkdup_bankdetails = function (user, index) {
            if ($scope.myForm1.$valid) {
            if (user.hrmeB_AccountNo != "" || user.hrmeB_AccountNo != undefined || user.hrmeB_AccountNo != null) {
                var dublicate = false;
                for (var k = 0; k < $scope.employeebankDetails.length; k++) {
                    var roll = parseInt(user.hrmeB_AccountNo);
                    var arryind = $scope.employeebankDetails.indexOf($scope.employeebankDetails[k]);
                    //console.log(arryind);
                    if (arryind != index) {
                        if ($scope.employeebankDetails[k].hrmeB_AccountNo == roll) {

                            swal("Account Number Already Chosen");
                            $scope.employeebankDetails[index].hrmeB_AccountNo = "";
                            dublicate = true;
                            break;
                        }
                    }
                }
                if (!dublicate) {
                    var data = user;

                    apiService.create("EmployeeRegistration/duplicate_bankAccountNo", data)
                        .then(function (promise) {
                            if (promise.tcflagexists == "Account Number Already Exists") {
                                swal('Account Number Already Exists', 'Please Enter Different Account Number');
                                $scope.employeebankDetails[index].hrmeB_AccountNo = "";
                            }
                        });
                }
            }
            } else {
                $scope.submitted1 = true;
                return;
            }
        };

        
        $scope.getcaste = function (imcC_Id) {
            var data = {
                "CasteCategoryId": imcC_Id
            };
            apiService.create("EmployeeRegistration/getcaste", data).then(function (promise) {
                if (promise != null) {
                    if (promise.castedropdownlist.length > 0) {
                        $scope.castedropdownlist = promise.castedropdownlist;
                        $scope.castedisble = false;
                    }
                    else {
                        swal("No Caste is mapped to selected Caste Category");
                        $scope.castedisble = true;
                        $scope.castedropdownlist = [];
                    }
                }
                else {
                    swal("No Caste is mapped to selected Caste Category");
                    $scope.castedisble = true;
                    $scope.castedropdownlist = [];
                }
            });
        };

        $scope.getcastecatgory = function (religionId) {
            var data = {
                "ReligionId": religionId
            };
            apiService.create("EmployeeRegistration/getcastecatgory", data).then(function (promise) {
                if (promise != null) {
                    if (promise.casteCategorydropdownlist.length > 0) {
                        $scope.casteCategoryedropdownlist = promise.casteCategorydropdownlist;
                        $scope.castecatdisble = false;
                    }
                    else {
                        swal("No CasteCategory is mapped to selected Religion");
                        $scope.casteCategoryedropdownlist = [];
                        $scope.castecatdisble = true;
                    }
                }
                else {
                    swal("No CasteCategory is mapped to selected Religion");
                    $scope.castecatdisble = true;
                    $scope.casteCategoryedropdownlist = [];
                }
            });
        };

        $scope.getdepartment = function (grouptypeid) {
            var data = {
                "HRMGT_Id": grouptypeid
            };
            apiService.create("EmployeeRegistration/getdepartment", data).then(function (promise) {
                if (promise != null) {
                    if (promise.departmentdropdownlist.length > 0) {
                        $scope.departmentdropdownlist = promise.departmentdropdownlist;
                        $scope.departmentdisble = false;
                    }
                    else {
                        swal("No Department is mapped to selected Group");
                        $scope.departmentdropdownlist = [];
                        $scope.departmentdisble = true;
                    }
                }
                else {
                    swal("No Department is mapped to selected Group");
                    $scope.departmentdisble = true;
                    $scope.departmentdropdownlist = [];
                }
            });
        };

        $scope.getdesignation = function (grouptypeid,departmentid) {
            var data = {
                "HRMGT_Id": grouptypeid,
                "HRMD_Id": departmentid
            };
            apiService.create("EmployeeRegistration/getdesignation", data).then(function (promise) {
                if (promise != null) {
                    if (promise.designationdropdownlist.length > 0) {
                        $scope.designationdropdownlist = promise.designationdropdownlist;
                        $scope.designationdisble = false;
                    }
                    else {
                        swal("No Designation is mapped to selected Department");
                        $scope.designationdropdownlist = [];
                        $scope.designationdisble = true;
                    }
                }
                else {
                    swal("No Designation is mapped to selected Department");
                    $scope.designationdisble = true;
                    $scope.designationdropdownlist = [];
                }
            });
        };

        //get Communication address state while editing
        function getSelectedcaste(imcC_Id,imc_id) {
            $scope.castedropdownlist = [{ "imC_Id": "", "imC_CasteName": "--Select--" }];
            var cast_Id = Number(imc_id);

            var casteArray = $filter('filter')($scope.onloadcastedropdownlist, function (d) {
                return d.imcC_Id === parseInt(imcC_Id);
            });

            
            if (casteArray.length > 0) {

                $scope.castedisble = false;
                $scope.castedata = casteArray;
                $scope.castedropdownlist.push.apply($scope.castedropdownlist, $scope.castedata);
                $scope.Employee.casteId = cast_Id;
                $scope.Employee.casteCategoryId = imcC_Id;

            }
            else {
                swal("No Caste is mapped to selected Caste Category");
                $scope.castedisble = true;
                $scope.castedropdownlist = [];
            }
        }


        // duplicate  qualification Details 
        $scope.chkdup_qualificationDetails = function (user, index) {
            var duplicate = false;

            for (var k = 0; k < $scope.qualificationDetails.length; k++) {

                var arryind = $scope.qualificationDetails.indexOf($scope.qualificationDetails[k]);
                //console.log(arryind);
                if (arryind != index) {
                    if ($scope.qualificationDetails[k].hrmE_QualificationName == user.hrmE_QualificationName
                          && $scope.qualificationDetails[k].hrmC_Id == user.hrmC_Id &&
                          $scope.qualificationDetails[k].hrmeQ_CollegeName == user.hrmeQ_CollegeName &&
                          $scope.qualificationDetails[k].hrmeQ_UniversityName == user.hrmeQ_UniversityName &&
                          $scope.qualificationDetails[k].hrmeQ_YearOfPassing == user.hrmeQ_YearOfPassing &&
                          $scope.qualificationDetails[k].hrmeQ_RegistrationNo == user.hrmeQ_RegistrationNo &&
                          $scope.qualificationDetails[k].hrmeQ_Result == user.hrmeQ_Result &&
                          $scope.qualificationDetails[k].hrmeQ_CGPAOrPerFlag == user.hrmeQ_CGPAOrPerFlag &&
                          $scope.qualificationDetails[k].hrmeQ_CGPA == user.hrmeQ_CGPA &&
                          $scope.qualificationDetails[k].hrmeQ_Percentage == user.hrmeQ_Percentage &&
                          $scope.qualificationDetails[k].hrmeQ_AreaOfSpecialisation == user.hrmeQ_AreaOfSpecialisation &&
                          $scope.qualificationDetails[k].hrmeQ_MedicalCouncil == user.hrmeQ_MedicalCouncil //&&
                        //  new Date($filter('date')(new Date($scope.qualificationDetails[k].hrmeQ_Date).toDateString(), "yyyy/MM/dd")) == new Date($filter('date')(new Date(user.hrmeQ_Date).toDateString(), "yyyy/MM/dd"))
                        )
                    {
                        
                        swal("Multiple Qualification Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                       //$scope.qualificationDetails[k].hrmE_QualificationName ="";
                       //$scope.qualificationDetails[k].hrmC_Id ="";
                       //$scope.qualificationDetails[k].hrmeQ_CollegeName ="";
                       //$scope.qualificationDetails[k].hrmeQ_UniversityName ="";
                       //$scope.qualificationDetails[k].hrmeQ_YearOfPassing ="";
                       //$scope.qualificationDetails[k].hrmeQ_RegistrationNo ="";
                       //$scope.qualificationDetails[k].hrmeQ_Result ="";
                       //$scope.qualificationDetails[k].hrmeQ_CGPAOrPerFlag = "";
                       //$scope.qualificationDetails[k].hrmeQ_CGPA ="";
                       //$scope.qualificationDetails[k].hrmeQ_Percentage ="";
                       //$scope.qualificationDetails[k].hrmeQ_AreaOfSpecialisation = "";
                       //$scope.qualificationDetails[k].hrmeQ_MedicalCouncil = "";
                        break;
                    }

                }

            }
            return duplicate;

        };


        // duplicate  Experiance Details 
        $scope.chkdup_experianceDetails = function (user, index) {
            var duplicate = false;

            for (var k = 0; k < $scope.experienceDetails.length; k++) {

                var arryind = $scope.experienceDetails.indexOf($scope.experienceDetails[k]);
                //console.log(arryind);
                if (arryind != index) {
                    if ($scope.experienceDetails[k].hrmeE_OrganisationName == user.hrmeE_OrganisationName
                          && $scope.experienceDetails[k].hrmeE_OrganisationAddress == user.hrmeE_OrganisationAddress &&
                          $scope.experienceDetails[k].hrmeE_Department == user.hrmeE_Department &&
                          $scope.experienceDetails[k].hrmeE_Designation == user.hrmeE_Designation &&
                          $scope.experienceDetails[k].hrmeE_AnnualSalary == user.hrmeE_AnnualSalary &&
                          $scope.experienceDetails[k].hrmeE_ReasonForLeaving == user.hrmeE_ReasonForLeaving

                        //&&

                        //  $scope.experienceDetails[k].hrmeE_JoinDate == user.hrmeE_JoinDate &&
                        //  $scope.experienceDetails[k].hrmeE_ExitDate == user.hrmeE_ExitDate &&
                        //  $scope.experienceDetails[k].hrmeE_NoOfYears == user.hrmeE_NoOfYears &&
                        //  $scope.experienceDetails[k].hrmeE_NoOfMonths == user.hrmeE_NoOfMonths &&
                        //  $scope.experienceDetails[k].hrmeQ_AreaOfSpecialisation == user.hrmeQ_AreaOfSpecialisation &&
                        //  $scope.experienceDetails[k].hrmeQ_MedicalCouncil == user.hrmeQ_MedicalCouncil //&&
                        //  new Date($filter('date')(new Date($scope.experienceDetails[k].hrmeQ_Date).toDateString(), "yyyy/MM/dd")) == new Date($filter('date')(new Date(user.hrmeQ_Date).toDateString(), "yyyy/MM/dd"))
                        ) {

                        swal("Multiple Experience Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;
                        
                        break;
                    }

                }

            }
            return duplicate;

        };


        // duplicate  Documents Details 
        $scope.chkdup_documentsDetails = function (user, index) {
            var duplicate = false;

            for (var k = 0; k < $scope.documentList.length; k++) {

                var arryind = $scope.documentList.indexOf($scope.documentList[k]);
                //console.log(arryind);
                if (arryind != index) {
                    if ($scope.documentList[k].hrmedS_DocumentName == user.hrmedS_DocumentName
                          && $scope.documentList[k].hrmedS_DucumentDescription == user.hrmedS_DucumentDescription
                        //&&
                        //  $scope.documentList[k].hrmeE_Department == user.hrmeE_Department 
                        ) {

                        swal("Multiple Document Details are Same", 'Kindly update to proceed..!!');
                        duplicate = true;

                        break;
                    }

                }

            }
            return duplicate;

        };








    }
})();

dashboard.directive('ngMouseWheelUp', function () {
    return function (scope, element, attrs) {
        element.bind("DOMMouseScroll mousewheel onmousewheel", function (event) {

            // cross-browser wheel delta
            var event = window.event || event; // old IE support
            var delta = Math.max(-1, Math.min(1, (event.wheelDelta || -event.detail)));

            if (delta > 0) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngMouseWheelUp);
                });

                // for IE
                event.returnValue = false;
                // for Chrome and Firefox
                if (event.preventDefault) event.preventDefault();
            }
        });
    };
});


dashboard.directive('ngMouseWheelDown', function () {
    return function (scope, element, attrs) {
        element.bind("DOMMouseScroll mousewheel onmousewheel", function (event) {

            // cross-browser wheel delta
            var event = window.event || event; // old IE support
            var delta = Math.max(-1, Math.min(1, (event.wheelDelta || -event.detail)));

            if (delta < 0) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngMouseWheelDown);
                });

                // for IE
                event.returnValue = false;
                // for Chrome and Firefox
                if (event.preventDefault) event.preventDefault();
            }
        });
    };
});
