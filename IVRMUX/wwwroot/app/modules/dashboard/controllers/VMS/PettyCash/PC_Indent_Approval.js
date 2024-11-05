(function () {
    'use strict';
    angular.module('app').controller('PC_Indent_ApprovalController', PC_Indent_ApprovalController)

    PC_Indent_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function PC_Indent_ApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.submitted = false;
        $scope.totalgrid = [];
        $scope.PCREQTN_Date = new Date();
        $scope.PCINDENT_Date = new Date();
        $scope.PCINDENTAPT_Date = new Date();
        $scope.editflag = true;
        $scope.maxdate = new Date();
        $scope.obj = {};
        $scope.obj12 = {};

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Indent_Approval/onloaddata", pageid).then(function (promise) {
                $scope.getloaddata = promise.getloaddata;

                $scope.getuserinstitution = promise.getuserinstitution;
                $scope.MI_Id = promise.mI_Id;

            });
        };

        $scope.OnChangeInstitution = function () {

            $scope.PCINDENT_Date_To = null;
            $scope.PCINDENT_Date_From = null;
            $scope.getloaddata = [];

            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcindentapp1' }];
            $scope.identdetails = [];
            $scope.identparticulardetails = [];
            $scope.obj.all2 = false;
            $scope.obj.all21 = false;

            var data = {
                "MI_Id": $scope.MI_Id
            };

            apiService.create("PC_Indent_Approval/OnChangeInstitution", data).then(function (promise) {
                $scope.getloaddata = promise.getloaddata;              
            });
        };

        $scope.onchangefromdate = function () {
            $scope.PCINDENT_Date_To = null;
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcindentapp1' }];
            $scope.identdetails = [];
            $scope.identparticulardetails = [];
            $scope.obj.all2 = false;
            $scope.obj.all21 = false;
        };

        $scope.onchangedate = function () {
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcindentapp1' }];
            $scope.identdetails = [];
            $scope.identparticulardetails = [];
            $scope.obj.all2 = false;
            $scope.obj.all21 = false;
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENT_Date_From": new Date($scope.PCINDENT_Date_From).toDateString(),
                "PCINDENT_Date_To": new Date($scope.PCINDENT_Date_To).toDateString()
            };
            apiService.create("PC_Indent_Approval/onchangedate", data).then(function (promise) {
                if (promise !== null) {
                    $scope.indentdetails = promise.indentdetails;
                    if ($scope.indentdetails === null || $scope.indentdetails.length === 0) {
                        swal("No Indent Found For Selected Date");
                    }
                }
            });
        };

        $scope.toggleAll = function (all2) {
            var toggleStatus = all2;
            angular.forEach($scope.requisitiondetais, function (itm) {
                itm.selected = toggleStatus;
            });

            $scope.getrequisitiondetails();
        };

        $scope.optionToggled = function () {
            $scope.obj.all2 = $scope.requisitiondetais.every(function (itm) { return itm.selected; });

            $scope.getrequisitiondetails();
        };

        $scope.toggleAll1 = function (all21) {
            var toggleStatus = all21;
            angular.forEach($scope.indentparticulardetails, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.optionToggled1 = function () {
            $scope.obj.all21 = $scope.indentparticulardetails.every(function (itm) { return itm.selected; });
        };

        $scope.getindentdetails = function (PCINDENT_Ids) {
            $scope.obj.all21 = false;
            $scope.tempdetails = [];
            $scope.totalgrid = [];
            $scope.identparticulardetails = [];
            $scope.indentdetailstemp = [];

            $scope.PCINDENT_Id_Temp = PCINDENT_Ids;
            $scope.tempdetails.push({ PCINDENT_Id: PCINDENT_Ids });

            angular.forEach($scope.indentdetails, function (dd) {
                if (dd.pcindenT_Id === parseInt(PCINDENT_Ids)) {
                    $scope.indentdetailstemp.push(dd);
                    $scope.PCINDENTAPT_RequestedAmount_Temp = dd.pcindenT_RequestedAmount;
                    $scope.PCINDENTAPT_RequestedAmount = dd.pcindenT_RequestedAmount;
                    $scope.pcindenT_IndentNo = dd.pcindenT_IndentNo;
                }
            });

            if ($scope.tempdetails.length > 0) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "temp_indentid": $scope.tempdetails
                };
                apiService.create("PC_Indent_Approval/getindentdetails", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.indentparticulardetails = promise.indentparticulardetails;
                        if ($scope.indentparticulardetails === null || $scope.indentparticulardetails.length === 0) {
                            swal("No Particulars Found For Selected Indent");
                        } else {
                            angular.forEach($scope.indentparticulardetails, function (dd) {
                                dd.remarks = dd.pcindentdeT_Remarks;
                            });
                        }
                    }
                });
            }
        };

        $scope.onchangeamount = function (userobj) {
            $scope.PCINDENTAPT_SanctionedAmt = 0.00;
            $scope.PCINDENTAPT_SanctionedAmt_Temp = 0.00;

            angular.forEach($scope.indentparticulardetails, function (dd) {
                if (dd.selected === true) {
                    if (dd.amount !== undefined && dd.amount !== null && dd.amount !== "") {
                        $scope.PCINDENTAPT_SanctionedAmt = $scope.PCINDENTAPT_SanctionedAmt + parseFloat(dd.amount);
                        $scope.PCINDENTAPT_SanctionedAmt_Temp = $scope.PCINDENTAPT_SanctionedAmt_Temp + parseFloat(dd.amount);
                    }
                }
            });
        };

        $scope.saverecord = function (PCINDENT_Idss) {
            if ($scope.myForm.$valid) {
                $scope.savePC_Indent_Approved_Details_DTO = [];
                angular.forEach($scope.indentparticulardetails, function (ddd) {
                    if (ddd.selected) {
                        $scope.savePC_Indent_Approved_Details_DTO.push({
                            PCMPART_Id: ddd.pcmparT_Id, PCINDENTAPDT_RequestedAmount: ddd.pcindentdeT_Amount,
                            PCINDENTAPDT_SanctionedAmt: parseFloat(ddd.amount), PCINDENTAPDT_Remarks: ddd.remarks, PCINDENTAPDT_Id: ddd.pcindentapdT_Id
                        });
                    }
                });

                if ($scope.savePC_Indent_Approved_Details_DTO.length === 0) {
                    swal("Select Alteast One Record To Save Indent Approval");
                    return;
                }

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCINDENTAP_Id": $scope.PCINDENTAP_Id,
                    "PCINDENT_Id": $scope.PCINDENT_Id_Temp,
                    "PCINDENTAPT_IndentNo": $scope.pcindenT_IndentNo,
                    "PCINDENTAPT_Desc": $scope.obj12.PCINDENTAPT_Desc,
                    "PCINDENTAPT_SanctionedAmt": $scope.PCINDENTAPT_SanctionedAmt,
                    "PCINDENTAPT_RequestedAmount": $scope.PCINDENTAPT_RequestedAmount,
                    "PCINDENTAPT_Date": new Date($scope.PCINDENTAPT_Date).toDateString(),
                    "PC_Indent_Approved_Details_DTO": $scope.savePC_Indent_Approved_Details_DTO
                };

                apiService.create("PC_Indent_Approval/saverecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (user) {
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCREQTN_Id": user.pcreqtN_Id
            };
            apiService.create("PC_Indent_Approval/EditData", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditdetails !== null && promise.geteditdetails.length > 0) {
                        $scope.geteditdept = promise.geteditdept;
                        $scope.HRMD_Id = $scope.geteditdept[0];

                        $scope.getemp = promise.geteditemp;
                        $scope.geteditsavedemp = promise.geteditsavedemp;
                        $scope.HRME_Id = $scope.geteditsavedemp[0];

                        $scope.PCREQTN_Purpose = promise.geteditdetails[0].pcreqtN_Purpose;
                        $scope.PCREQTN_Date = new Date(promise.geteditdetails[0].pcreqtN_Date);
                        $scope.PCREQTN_TotAmount = promise.geteditdetails[0].pcreqtN_TotAmount;
                        $scope.PCREQTN_TotAmounttemp = promise.geteditdetails[0].pcreqtN_TotAmount;
                        $scope.PCREQTN_Id = promise.geteditdetails[0].pcreqtN_Id;

                        $scope.geteditsavedparticulars = promise.geteditsavedparticulars;

                        $scope.totalgrid = [];

                        angular.forEach($scope.geteditsavedparticulars, function (dd) {
                            $scope.totalgrid.push({
                                PCMPART_Id: dd, amount: dd.pcreqtndeT_Amount, remarks: dd.pcreqtndeT_Remarks,
                                pcreqtndeT_Id: dd.pcreqtndeT_Id
                            });
                        });

                        $scope.editflag = true;
                    }
                }

            });
        };

        $scope.deactiveY = function (user, SweetAlert) {
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCREQTN_Id": user.pcreqtN_Id
            };

            var dystring = "";
            if (user.pcreqtN_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcreqtN_ActiveFlg === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("PC_Indent_Approval/deactiveY", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.Viewdata = function (user) {
            $scope.indentno = user.pcindentapT_IndentNo;
            $scope.pcindentapT_Dated = new Date(user.pcindentapT_Date);
            $scope.totalapprovedamt = user.pcindentapT_SanctionedAmt;
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENTAP_Id": user.pcindentaP_Id
            };
            apiService.create("PC_Indent_Approval/Viewdata", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getviewdata = promise.getviewdata;
                    $scope.getinstitutiondetails = promise.getinstitutiondetails;

                    $scope.imagenew = "";
                    $scope.imagenew = $scope.getinstitutiondetails[0].mI_Logo;

                    $scope.printviewdata = [];

                    angular.forEach($scope.getviewdata, function (dd) {
                        if (dd.pcindentdeT_ActiveFlg === true) {
                            $scope.printviewdata.push(dd);
                        }                    
                    });

                }
            });
        };

        $scope.deactiveparticulars = function (user, SweetAlert) {
            var data = {
                "MI_Id": $scope.MI_Id,
                "pcreqtndeT_Id": user.pcreqtndeT_Id,
                "PCREQTN_Id": user.pcreqtN_Id
            };

            var dystring = "";
            if (user.pcreqtndeT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcreqtndeT_ActiveFlg === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("PC_Indent_Approval/deactiveparticulars", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.getviewdata = promise.getviewdata;
                                $scope.printviewdata = [];

                                angular.forEach($scope.getviewdata, function (dd) {
                                    if (dd.pcindentdeT_ActiveFlg === true) {
                                        $scope.printviewdata.push(dd);
                                    }
                                });
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.totalgrid = [{ id: 'pcreq1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.totalgrid.length + 1;
            if (newItemNo <= 10) {
                $scope.requisitionparticulardetais.push({ 'id': 'pcreq' + newItemNo });
            }
            console.log($scope.totalgrid);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.totalgrid.length - 1;
            $scope.requisitionparticulardetais.splice(index, 1);

            if ($scope.requisitionparticulardetais.length === 0) {
                //data
            }

            $scope.PCREQTN_TotAmount = 0.00;
            $scope.PCREQTN_TotAmounttemp = 0.00;
            angular.forEach($scope.totalgrid, function (dd) {
                if (dd.amount !== undefined && dd.amount !== null && dd.amount !== "") {
                    $scope.PCREQTN_TotAmount = $scope.PCREQTN_TotAmount + parseFloat(dd.amount);
                    $scope.PCREQTN_TotAmounttemp = $scope.PCREQTN_TotAmounttemp + parseFloat(dd.amount);
                }
            });
        };

        $scope.search = '';

        $scope.filterValue1 = function (obj) {
            return ($filter('date')(obj.pcindentapT_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.departmentname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcindentapT_IndentNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.obj12.pcindentapT_Desc)).indexOf(angular.lowercase($scope.search)) >= 0;
        };

        $scope.Print = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };
    }
})();
