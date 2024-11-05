(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeeOfferAndExperienceReportBGHSController', EmployeeOfferAndExperienceReportBGHSController)

    EmployeeOfferAndExperienceReportBGHSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EmployeeOfferAndExperienceReportBGHSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
        //Employeee

        $scope.Employee = {};
        $scope.departmentdisble = true;
        $scope.designationdisble = true;
        //$scope.Employee.hrmeR_Current_Date = new Date();

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeOfferAndExperienceReport/getalldetails", pageid).then(function (promise) {

                if (promise.emptypedropdown !== null && promise.emptypedropdown.length > 0) {
                    $scope.emptypedropdown = promise.emptypedropdown;
                }

                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                }

                if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                    $scope.designationdropdown = promise.designationdropdown;
                }

                $scope.Employee.hrmeR_Offer_letter = "offerletter";

                $scope.Employee.hrmeR_Current_Date = new Date();
            });
        };


        $scope.setdoj = function (data) {
            //  $scope.todate = false;
            console.log(data);
            $scope.doiDate = data;
            // $scope.minDatedoi = new Date(
            //$scope.doiDate.getFullYear(),
            //$scope.doiDate.getMonth(),
            //$scope.doiDate.getDate());

            $scope.maxDatedoi = new Date(
                $scope.doiDate.getFullYear(),
                $scope.doiDate.getMonth(),
                $scope.doiDate.getDate());
        };


        $scope.setdoj = function (data) {
            //  $scope.todate = false;
            console.log(data);
            $scope.doiDate = data;
            // $scope.minDatedoi = new Date(
            //$scope.doiDate.getFullYear(),
            //$scope.doiDate.getMonth(),
            //$scope.doiDate.getDate());

            $scope.maxDatedoi = new Date(
                $scope.doiDate.getFullYear(),
                $scope.doiDate.getMonth(),
                $scope.doiDate.getDate());
        };

        $scope.setEmployeeDOJ = function (hrmE_Id) {

            if (hrmE_Id != null && hrmE_Id != undefined && hrmE_Id != "") {

                var empjoindate = $filter('filter')($scope.employeedropdown, function (d) {
                    return d.hrmE_Id === parseInt(hrmE_Id);
                })[0].hrmE_DOJ;
                $scope.Employee.hrmE_DOJ = new Date(empjoindate);

                $scope.Empdoj = $scope.Employee.hrmE_DOJ;

                $scope.maxDatedoi = new Date(
                    $scope.Empdoj.getFullYear(),
                    $scope.Empdoj.getMonth(),
                    $scope.Empdoj.getDate());
            }

        };

        $scope.empdetails = {};
        $scope.empofferandexpletter = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.empdetails = {};
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = $scope.Employee;
                apiService.create("EmployeeOfferAndExperienceReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {

                        if (promise.currentemployeeDetails != null) {
                            $scope.empofferandexpletter = true;
                            $scope.hrmeR_Experience_letter = true;
                            $scope.empdetails = promise.currentemployeeDetails;
                            var EmployeeName = "";
                            if ($scope.empdetails.hrmE_EmployeeFirstName != null && $scope.empdetails.hrmE_EmployeeFirstName != "") {
                                EmployeeName = $scope.empdetails.hrmE_EmployeeFirstName;
                            }
                            if ($scope.empdetails.hrmE_EmployeeMiddleName != null && $scope.empdetails.hrmE_EmployeeMiddleName != "") {
                                EmployeeName = EmployeeName + ' ' + $scope.empdetails.hrmE_EmployeeMiddleName;
                            }
                            if ($scope.empdetails.hrmE_EmployeeLastName != null && $scope.empdetails.hrmE_EmployeeLastName != "") {
                                EmployeeName = EmployeeName + ' ' + $scope.empdetails.hrmE_EmployeeLastName;
                            }
                            //if ($scope.empdetails.hrmE_EmployeeLastName != null && $scope.empdetails.hrmE_EmployeeLastName != "" && $scope.empdetails.hrmE_EmployeeMiddleName != null && $scope.empdetails.hrmE_EmployeeMiddleName != "" && $scope.empdetails.hrmE_EmployeeFirstName != null && $scope.empdetails.hrmE_EmployeeFirstName != "") {
                            //    EmployeeName = EmployeeName + ' ' + $scope.empdetails.hrmE_EmployeeMiddleName + ' ' + $scope.empdetails.hrmE_EmployeeLastName;
                            //}

                            $scope.FullName = EmployeeName;
                            $scope.DesignationName = promise.designationName;
                            //get current date
                            $scope.HRMER_Current_Date = new Date(promise.hrmeR_Current_Date);
                            //get DOI
                            $scope.HRMER_DOI = new Date(promise.hrmeR_DOI);
                            //employee address...
                            $scope.localarea = $scope.empdetails.hrmE_LocArea;
                            $scope.localcity = $scope.empdetails.hrmE_LocCity;
                            // $scope.localcountry = $scope.empdetails.hrmE_LocCountryId;
                            $scope.pincode = $scope.empdetails.hrmE_LocPincode;
                            $scope.state = $scope.empdetails.hrmE_LocStateId;
                            $scope.street = $scope.empdetails.hrmE_LocStreet;
                            $scope.dateofleft = $scope.empdetails.hrmE_DOL;
                            $scope.dateofjoin = $scope.empdetails.hrmE_DOJ;
                        }

                        //institution

                        if (promise.institutionDetails != null) {
                            $scope.institutionDetails = promise.institutionDetails;
                            //  $('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.institutionDetails.mi_id + "/" + "EmployeeProfilePics" + "/" + $scope.institutionDetails.mI_Logo);
                            var instuteAddress = "";
                            if ($scope.institutionDetails.mI_Address1 != null && $scope.institutionDetails.mI_Address1 != "") {
                                instuteAddress = $scope.institutionDetails.mI_Address1;
                            }
                            if ($scope.institutionDetails.mI_Address2 != null && $scope.institutionDetails.mI_Address2 != "") {
                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;
                            }

                            if ($scope.institutionDetails.mI_Address3 != null && $scope.institutionDetails.mI_Address3 != "") {
                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;
                            }
                            $scope.CurrentInstuteAddress = instuteAddress;

                        }

                    });
            }
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
                        $scope.GetEmployeeListByFilterSelection();
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

        $scope.getdesignation = function (grouptypeid, departmentid) {
            var data = {
                "HRMGT_Id": grouptypeid,
                "HRMD_Id": departmentid
            };
            apiService.create("EmployeeRegistration/getdesignation", data).then(function (promise) {
                if (promise != null) {
                    if (promise.designationdropdownlist.length > 0) {
                        $scope.designationdropdownlist = promise.designationdropdownlist;
                        $scope.designationdisble = false;
                        $scope.GetEmployeeListByFilterSelection();
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

        $scope.GetEmployeeListByFilterSelection = function () {

            if ($scope.Employee.HRMDES_Id != undefined && $scope.Employee.hrmD_Id != undefined && $scope.Employee.hrmgT_Id != undefined) {

                var data = $scope.Employee;
                //var data = {
                //    "HRMGT_Id": Employee.hrmgT_Id,
                //    "HRMD_Id": Employee.hrmD_Id,
                //    "HRMDES_Id": Employee.HRMDES_Id
                //}

                apiService.create("EmployeeOfferAndExperienceReport/FilterEmployeeData", data).
                    then(function (promise) {

                        if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                            $scope.employeedropdown = promise.employeedropdown;
                        } else {
                            swal('No Record Found to display..');
                            $scope.employeedropdown = [];

                        }
                    });
            }
            else {
                $scope.employeedropdown = [];
            }

        };

        //Clear data

        $scope.cleardata = function () {
            //
            $scope.totaldaysDis = true;
            $scope.Employee = {};
            $scope.submitted = false;
            $scope.EmployeeDis = false;
            $scope.empofferandexpletter = false;
            $scope.departmentdisble = true;
            $scope.designationdisble = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //    var newWin = window.open();
        //    newWin.document.write(divToPrint.outerHTML);
        //    newWin.print();
        //    newWin.close();
        //    // $state.reload();
        //}

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


    }
})();