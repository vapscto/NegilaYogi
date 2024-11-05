
(function () {
    'use strict';
    angular.module('app').controller('StaffQRCodeGenerationController', StaffQRCodeGenerationController)
    StaffQRCodeGenerationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'blockUI']
    function StaffQRCodeGenerationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, blockUI) {

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("QRCode_Generation/getalldetails", pageid).then(function (promise) {

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }

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

        //QRcode Genaration
        $scope.submitted = false;
        $scope.show = "";
        $scope.QRCodeGenerate1 = function () {
            $scope.submitted = true;
            if ($scope.stafflistdetails != null && $scope.stafflistdetails.length > 0) {
                angular.forEach($scope.stafflistdetails, function (st) {
                    var value = 0;
                    $scope.show = 1;
                    if (st.HRME_Id != null && st.HRME_Id != "") {
                        value = st.HRME_Id;
                        var valuex = value; // Example selector for an input element with id "qr-text"
                        //$(".qr-code" + st.HRME_Id + "").attr("src", "https://chart.googleapis.com/chart?cht=qr&chl=" + valuex + "&chs=160x160&chld=L|0");
                        $(".qr-code" + st.HRME_Id + "").attr("src", "https://image-charts.com/chart?chs=150x150&cht=qr&chl= "+ valuex +" ");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }

        //SaveQRcode
        $scope.SavestaffQR_Code = function (staff) {
            $scope.Staffqrlistarray1 = [];
            angular.forEach($scope.stafflistdetails, function (dd) {
                var imagge = document.getElementById("qrimage" + dd.HRME_Id + "").src
                if (imagge != null && imagge != "") {
                    $scope.Staffqrlistarray1.push({
                        hrmE_id: dd.HRME_Id,
                        SQRGD_QRCode: imagge
                    })
                }
            });

            var data = {
                "Staffqrlistarray1": $scope.Staffqrlistarray1

            };
            apiService.create("QRCode_Generation/STAFFSaveQR_Code", data).then(function (promise) {
                if (promise.message == 'saved') {
                    swal("Record  Saved Successfully", "", "success");
                    $state.reload();
                }
                if (promise.message == "recordexist") {
                    swal("Record Already Exist", "", "warning");
                    $scope.reload();
                }
            })
        };

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        // retun details
        $scope.employeeDetails = [];


        $scope.currentemployeequalificationDetails = [];
        $scope.employequalify = false;
        $scope.employeedocumentflg = false;
        $scope.employeeclasssubjectflg = false;
        $scope.qualifymsg = false;
        $scope.empdetails = {};
        $scope.empqualifydetails = {};
        //Search employee
        $scope.submitted = false;

        $scope.SearchEmployee = function () {
            $scope.employequalification = [];
            $scope.employeedocument = [];
            $scope.empdetails = {};
            $scope.DesignationName = "";
            $scope.qualifymsg = false;
            $scope.submitted = true;
            $scope.Employee.hrmE_Id = 0;
            var today = moment(new Date());
            $scope.allInOne = [];

            //if ($scope.myForm.$valid) { }
            $scope.institutionDetails = {};
            $scope.empdetails = {};

            $scope.Employee.hrmE_multiId = [];
            angular.forEach($scope.employeedropdown, function (role) {
                if (role.emple) $scope.Employee.hrmE_multiId.push(role.hrmE_Id);
            });

            var data = $scope.Employee;
            apiService.create("QRCode_Generation/getEmployeedetailsBySelection", data).
                then(function (promise) {


                    if (promise.institutionDetails !== null) {
                        $scope.qualifymsg = true;
                        $scope.institutionDetails = promise.institutionDetails;

                        var instuteAddress = "";
                        if ($scope.institutionDetails.mI_Address1 !== null && $scope.institutionDetails.mI_Address1 !== "") {
                            instuteAddress = $scope.institutionDetails.mI_Address1;
                        }
                        if ($scope.institutionDetails.mI_Address2 !== null && $scope.institutionDetails.mI_Address2 !== "") {
                            instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;
                        }
                        if ($scope.institutionDetails.mI_Address3 !== null && $scope.institutionDetails.mI_Address3 !== "") {
                            instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;
                        }
                        $scope.CurrentInstuteAddress = instuteAddress;
                    }

                    angular.forEach(promise.arrayempsList, function (val) {
                        if (val.currentemployeeDetails !== null) {
                            val.empdetails = val.currentemployeeDetails;
                            var eventE = moment(val.empdetails.hrmE_DOB);
                            val.age = moment.duration(today.diff(eventE)).asDays() / 365;
                            //$('#blah').attr('src', val.currentemployeeDetails.hrmE_Photo);
                        }

                        if (val.employequalification !== null && val.employequalification.length > 0) {
                            val.employequalify = true;
                        }
                        else {
                            val.employequalify = false;
                        }

                        if (val.employeedocument !== null && val.employeedocument.length > 0) {
                            val.employeedocumentflg = true;
                        }
                        else {
                            val.employeedocumentflg = false;
                        }

                        if (val.employeeclasssubject !== null && val.employeeclasssubject.length > 0) {
                            val.employeeclasssubjectflg = true;
                        }
                        else {
                            val.employeeclasssubjectflg = false;
                        }
                        $scope.allInOne.push(val);
                    });
                });
            console.log($scope.allInOne);
            //}
        };

        $scope.OnEmployeeChange = function () {
            $scope.qualifymsg = false;
        };

        $scope.all_check_empl = function (empl) {
            var toggleStatus4 = empl;
            angular.forEach($scope.employeedropdown, function (itm) {
                itm.emple = toggleStatus4;
            });
        };

        $scope.searchDiv = true;
        $scope.empDiv = false;
        $scope.cleardata = function () {
            $state.reload();
        }

        //Clear data
        $scope.Employee = {};
        //$scope.cleardata = function () {
        //    $scope.Employee = {};
        //    $scope.employeeDetails = [];
        //    $scope.employequalification = [];
        //    $scope.employeedocument = [];
        //    $scope.empdetails = {};
        //    $scope.searchDiv = true;
        //    $scope.empDiv = false;
        //    $scope.DesignationName = "";
        //    $scope.groupTypeselectedAll = false;
        //    $scope.departmentselectedAll = false;
        //    $scope.designationselectedAll = false;
        //    $scope.submitted = false;
        //    $scope.employequalify = false;
        //    $scope.qualifymsg = false;
        //    $scope.designationselectedAll = false;

        //    $scope.UploadEmployeeProfilePic = [];
        //    $('#blah').removeAttr('src');
        //    $scope.search = "";

        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();
        //    $scope.onLoadGetData();

        //};

        $scope.interacted = function (field) {
            return $scope.submitted;
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

        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });

            $scope.get_depts();
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
            apiService.create("QRCode_Generation/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                });
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


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
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
            apiService.create("QRCode_Generation/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };


        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;

            });

        };


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        };

        $scope.SearchEmployeeDetails = function () {
            var groupTypeselected = [];
            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }
            });
            var employeeTypeselected = [];
            angular.forEach($scope.employeeTypedropdown, function (itm) {
                if (itm.selected) {
                    employeeTypeselected.push(itm.hrmeT_Id);
                }
            });
            var departmentselected = [];
            angular.forEach($scope.departmentdropdown, function (itm) {
                if (itm.selected) {
                    departmentselected.push(itm.hrmD_Id);
                }
            });
            var designationselected = [];
            angular.forEach($scope.designationdropdown, function (itm) {
                if (itm.selected) {
                    designationselected.push(itm.hrmdeS_Id);
                }

            });

            if (groupTypeselected.length === 0 && departmentselected.length === 0 && designationselected.length === 0) {
                swal('Kindly select atleast one record');
                return;
            }
           
            $scope.submitted = false;
            $scope.employeedropdown = [];
            $scope.submitted = true;
            var data = {
                hrmgT_IdList: groupTypeselected,
                hrmD_IdList: departmentselected,
                hrmdeS_IdList: designationselected

            };

            apiService.create("QRCode_Generation/filterEmployeedetailsBySelection", data).
                then(function (promise) {

                    if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                        $scope.employeedropdown = promise.employeedropdown;
                        $scope.searchDiv = false;
                        $scope.empDiv = true;
                    }
                    else {
                        swal('No Record found to display .. !');
                        return;
                    }

                });
        };

        $scope.SearchEmployee = function () {
            var Staffqrlistarray12 = [];
            angular.forEach($scope.employeedropdown, function (itm) {
                if (itm.emple) {
                    Staffqrlistarray12.push({
                        hrmE_id: itm.hrmE_Id,
                    });
                }
            });
            var data = {
                Staffqrlistarray12: Staffqrlistarray12,
                "Flag": "StaffDetails",

            };
            apiService.create("QRCode_Generation/StaffGetdetails", data).then(function (promise) {
                $scope.stafflistdetails = promise.staffList;
                if (promise.staffList.length == 0) {
                    swal("Already QRcode generated Emplyees");
                }
               
            });
        };
        $scope.staffReportDetails = function () {
            var Staffqrlistarray12 = [];
            angular.forEach($scope.employeedropdown, function (itm) {
                if (itm.emple) {
                    Staffqrlistarray12.push({
                        hrmE_id: itm.hrmE_Id,
                    });
                }
            });

            var data = {
                Staffqrlistarray12: Staffqrlistarray12,
                "Flag": "StaffReport",
            };
            apiService.create("QRCode_Generation/StaffGetdetails", data).then(function (promise) {
                $scope.staffreportdetails = promise.staffList;
                $('#myModal').modal('show');
                if (promise.staffList == null || promise.staffList == "") {
                    swal("Not Generated QR Code..");
                    $('#myModal').modal('hide');
                    $state.onLoadGetData();
                }              
            });
        };

        //PRintReport
        $scope.QRPrint = function () {
            var innerContents = document.getElementById("myModal1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                //'<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/idcardprint.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        //$scope.QRCodeGenerate = function () {
        //    var Qrcodegeneration = [];
        //    angular.forEach($scope.employeedropdown, function (itm) {
        //        if (itm.emple) {
        //            Qrcodegeneration.push({
        //                hrmE_id: itm.hrmE_Id,
        //            });
        //        }
        //    });
        //    var data = {
        //        Qrcodegeneration: Qrcodegeneration,
        //        "Flag": "StaffDetailsQR",

        //    };
        //    apiService.create("QRCode_Generation/QRcodegeneration", data).then(function (promise) {
                
               

        //    });
        //};
        $scope.QRCodeGenerate = function () {
            var Qrcodegeneration = [];
            angular.forEach($scope.employeedropdown, function (itm) {
                if (itm.emple) {
                    Qrcodegeneration.push({
                        hrmE_id: itm.hrmE_Id,
                    });
                }
            });
            var data = {
                Qrcodegeneration: Qrcodegeneration,
                "Flag": "StaffDetailsQR",

            };
            apiService.create("QRCode_Generation/QRcodegeneration", data).then(function (promise) {
                $scope.stafflistdetails = promise.staffList;
                if (promise.staffList == null && promise.staffList == "") {
                    swal("Already QRcode generated Emplyees");
                }
                
            });
        };



    }
})();