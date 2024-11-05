
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffGatePassController', StaffGatePassController)
    StaffGatePassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', '$q', '$window']
    function StaffGatePassController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, $q, $window, ) {

    //StaffGatePassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', '$filter']
    //function StaffGatePassController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter) {

        $scope.staff_printoption = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = "";
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; 
        };
        $scope.staff_gate_pass = false;
        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //$scope.ismeridian = true;
        //$scope.toggleMode = function () {
        //    $scope.ismeridian = !$scope.ismeridian;
        //};

       


        $scope.gphsT_DateTime = new Date();
        $scope.maxdate = new Date();
        $scope.mindate = new Date();

        //========================TO  GEt The Values iN Grid
        $scope.BindData = function () {
            //var pageid = 2;
            apiService.getURI("StaffGatePass/Getdetails", 2).then(function (promise) {
                $scope.filldepartment = promise.filldepartment;
                $scope.filldesignation = promise.filldesignation;
                $scope.emplist = promise.emplist;
                $scope.alldata = promise.alldata;
               
            });
        };

        
        $scope.get_empdetails = function (user) {
            $scope.hrmdeS_Id = 0;
            $scope.hrmdeS_Id = 0;
            for (var i = 0; i < $scope.emplist.length; i++) {
                if ($scope.emplist[i].hrmE_Id == user.hrmE_Id ) {
                    $scope.hrmD_Id = $scope.emplist[i].hrmD_Id;
                    $scope.hrmdeS_Id = $scope.emplist[i].hrmdeS_Id;
                }
            }
        };

        //========================================Get department  Data
        $scope.get_department = function () {

            var data = {
                "HRMD_Id": $scope.hrmD_Id,
            }
            apiService.create("StaffGatePass/getdepchange/", data).then(function (promise) {
                $scope.filldesignation = promise.filldesignation;
            })
        };


        //========================================Get Designation Data
        $scope.get_emp = function () {
            var data = {
                "HRMDES_Id": $scope.hrmdeS_Id,
                "HRMD_Id": $scope.hrmD_Id,
            }

            apiService.create("StaffGatePass/get_staff1", data).then(function (promise) {
               // $scope.emplist = promise.emplist;
            });
        }


        //============================================== TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {


                var outtime = "";
                if ($scope.GPHST_OutTime !== undefined && $scope.GPHST_OutTime !== null && $scope.GPHST_OutTime !== "") {
                    outtime = $filter('date')($scope.GPHST_OutTime, "HH:mm");
                }

                var intime = "";
                if ($scope.GPHST_InTime !== undefined && $scope.GPHST_InTime !== null && $scope.GPHST_InTime !== "") {
                    intime = $filter('date')($scope.GPHST_InTime, "HH:mm");
                }


                var data = {
                    "GPHST_Id": $scope.gphsT_Id,
                    "HRMD_Id": $scope.hrmD_Id,
                    "HRMDES_Id": $scope.hrmdeS_Id,
                    "HRME_Id": $scope.hrmE_Id.hrmE_Id,
                    "GPHST_Remarks": $scope.gphsT_Remarks,
                    "GPHST_DateTime": $scope.gphsT_DateTime,
                    //"GPHST_IDCardNo": $scope.gphsT_IDCardNo,
                    "GPHST_OutTime": outtime,
                    //"GPHST_InTime": intime

                }
                apiService.create("StaffGatePass/saveRecord", data).then(function (promise) {
                    if (promise.returnval != null && promise.dulicate != null) {
                        if (promise.dulicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.gphsT_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully!!!');

                                    $scope.staff_printoption = false;
                                    $scope.SchoolLogo = promise.institution[0].mI_Logo;
                                    $scope.SchollName = promise.institution[0].mI_Name;
                                    $scope.SchollAdd = promise.institution[0].mI_Address1;
                                    $scope.staff_gate_pass = true;
                                    $scope.currentstaffdata = promise.currentstaffdata;
                                    $scope.alldata.length = 0;
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.gphsT_Id > 0) {
                                        swal('Record Not Update Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditRecord = function (EditRecord) {
            $scope.studentlsitdata = [];
            var data = {
                "GPHST_Id": EditRecord.gphsT_Id,
                "HRMDES_Id": EditRecord.hrmdeS_Id,
                "HRMD_Id": EditRecord.hrmD_Id,

            }
            apiService.create("StaffGatePass/EditRecord/", data).then(function (promise) {

                if (promise.editlist.length > 0) {

                    $scope.gphsT_Id = promise.editlist[0].gphsT_Id;
                    $scope.gphsT_IDCardNo = promise.editlist[0].gphsT_IDCardNo;
                    $scope.gphsT_Remarks = promise.editlist[0].gphsT_Remarks;

                    $scope.gphsT_DateTime = new Date(promise.editlist[0].gphsT_DateTime);

                    $scope.hrmD_Id = promise.editlist[0].hrmD_Id;
                    $scope.get_department();

                    $scope.hrmdeS_Id = promise.editlist[0].hrmdeS_Id;
                    $scope.emplist = promise.emplist;

                    $scope.hrmE_Id = promise.editlist[0];

                    if (promise.editlist[0].gphsT_OutTime !== null && promise.editlist[0].gphsT_OutTime !== "") {
                        $scope.GPHST_OutTime = moment(promise.editlist[0].gphsT_OutTime, 'HH:mm').format();
                    }
                    if (promise.editlist[0].gphsT_InTime !== null && promise.editlist[0].gphsT_InTime !== "") {
                        $scope.GPHST_InTime = moment(promise.editlist[0].gphsT_InTime, 'HH:mm').format();
                    }
                }
                else {
                    swal('No Record Found!');
                }
            })
        };

        $scope.PrintGatePass = function (EditRecord) {
            $scope.studentlsitdata = [];
            var data = {
                "GPHST_Id": EditRecord.gphsT_Id,
                "HRMDES_Id": EditRecord.hrmdeS_Id,
                "HRMD_Id": EditRecord.hrmD_Id,

            }
            apiService.create("StaffGatePass/PrintGatePass/", data).then(function (promise) {

                if (promise.currentstaffdata !== null && promise.currentstaffdata.length > 0) {


                    $scope.staff_printoption = false;
                    $scope.SchoolLogo = promise.institution[0].mI_Logo;
                    $scope.SchollName = promise.institution[0].mI_Name;
                    $scope.SchollAdd = promise.institution[0].mI_Address1;
                    $scope.staff_gate_pass = true;
                    $scope.currentstaffdata = promise.currentstaffdata;
                  
                    if (promise.currentstaffdata[0].hrmE_Photo != null && promise.currentstaffdata[0].hrmE_Photo != "") {
                        //$('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.mi_id + "/" + "EmployeeProfilePics" + "/" + promise.employeedetailList[0].hrmE_Photo);
                        $('#blah').attr('src', promise.currentstaffdata[0].hrmE_Photo);

                    }
                    $scope.alldata.length = 0;
                }
                else {
                    swal('No Record Found!');
                }
            })
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.deactivate = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.gphsT_ActiveFlg == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StaffGatePass/deactive", newuser1).then(function (promise) {

                            if (promise.returnval == true) {
                                swal("Record " + mgs + "d Successfully!!!");
                            }
                            else {
                                swal("Record Not " + mgs + "d Successfully!!!");

                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                })
        }

        //===========================cancel
        $scope.cancel = function () {
            $state.reload();
        }

        //$scope.Print = function () {
        //    var innerContents = '';
        //    innerContents = document.getElementById("printSectionIdgirls").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //        '<link type="text/css" media="print" href="css/print/Visitor_Management/InwardReportPdf.css" rel="stylesheet" />' +
        //        '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //    );
        //    popupWinindow.document.close();
        //}


        $scope.Print = function () {
            var innerContents = "";
            innerContents = document.getElementById("printSectionIdgirls").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();