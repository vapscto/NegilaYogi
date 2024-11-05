(function () {
    'use strict';
    angular.module('app').controller('PC_IndentController', PC_IndentController)

    PC_IndentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function PC_IndentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.submitted = false;
        $scope.totalgrid = [];
        $scope.PCREQTN_Date = new Date();
        $scope.PCINDENT_Date = new Date();
        $scope.editflag = false;
        $scope.maxdate = new Date();
        $scope.obj = {};
        $scope.obj12 = {};

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Indent/onloaddata", pageid).then(function (promise) {

                $scope.getloaddata = promise.getloaddata;
                $scope.getapproveddata = promise.getapproveddata;
                $scope.getuserinstitution = promise.getuserinstitution;

                $scope.MI_Id = promise.mI_Id;

                angular.forEach($scope.getloaddata, function (d) {
                    d.approvedflag = false;
                    angular.forEach($scope.getapproveddata, function (dd) {
                        if (d.pcindenT_Id === dd.pcindenT_Id) {
                            d.approvedflag = true;
                            d.pcindentaP_Id = dd.pcindentaP_Id;
                            d.pcindentapT_IndentNo = dd.pcindentapT_IndentNo;
                            d.pcindentapT_Date = dd.pcindentapT_Date;
                            d.pcindentapT_SanctionedAmt = dd.pcindentapT_SanctionedAmt;
                        }
                    });

                });
            });
        };

        $scope.OnChangeInstitution = function () {
            $scope.getloaddata = [];
            $scope.getapproveddata = [];
            $scope.PCINDENT_Date_To = null;
            $scope.PCINDENT_Date_From = null;

            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcindent1' }];
            $scope.requisitiondetais = [];
            $scope.requisitionparticulardetais = [];
            $scope.geteditparticularsdata = [];
            $scope.geteditdata = [];
            $scope.obj.all2 = false;
            $scope.obj.all21 = false;
            $scope.obj.all22 = false;

            $scope.PCINDENT_Id = 0;
            $scope.editflag = false;

            var data = {
                "MI_Id": $scope.MI_Id
            };

            apiService.create("PC_Indent/OnChangeInstitution", data).then(function (promise) {
                $scope.getloaddata = promise.getloaddata;
                $scope.getapproveddata = promise.getapproveddata;
                angular.forEach($scope.getloaddata, function (d) {
                    d.approvedflag = false;
                    angular.forEach($scope.getapproveddata, function (dd) {
                        if (d.pcindenT_Id === dd.pcindenT_Id) {
                            d.approvedflag = true;
                            d.pcindentaP_Id = dd.pcindentaP_Id;
                            d.pcindentapT_IndentNo = dd.pcindentapT_IndentNo;
                            d.pcindentapT_Date = dd.pcindentapT_Date;
                            d.pcindentapT_SanctionedAmt = dd.pcindentapT_SanctionedAmt;
                        }
                    });
                });
            });
        };

        $scope.onchangefromdate = function () {
            $scope.PCINDENT_Date_To = null;
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcindent1' }];
            $scope.requisitiondetais = [];
            $scope.requisitionparticulardetais = [];
            $scope.geteditparticularsdata = [];
            $scope.geteditdata = [];
            $scope.obj.all2 = false;
            $scope.obj.all21 = false;
            $scope.obj.all22 = false;

            $scope.PCINDENT_Id = 0;
            $scope.editflag = false;
        };

        $scope.onchangedate = function () {
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcindent1' }];
            $scope.requisitiondetais = [];
            $scope.requisitionparticulardetais = [];
            $scope.geteditparticularsdata = [];
            $scope.geteditdata = [];
            $scope.PCINDENT_Id = 0;
            $scope.obj.all2 = false;
            $scope.obj.all22 = false;
            $scope.obj.all21 = false;
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENT_Date_From": new Date($scope.PCINDENT_Date_From).toDateString(),
                "PCINDENT_Date_To": new Date($scope.PCINDENT_Date_To).toDateString()
            };
            apiService.create("PC_Indent/onchangedate", data).then(function (promise) {
                if (promise !== null) {
                    $scope.requisitiondetais = promise.requisitiondetais;
                    if ($scope.requisitiondetais === null || $scope.requisitiondetais.length === 0) {
                        swal("No Requisition Found For Selected Date");
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

        // Direct
        $scope.toggleAll1 = function (all21) {
            var toggleStatus = all21;
            angular.forEach($scope.requisitionparticulardetais, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.onchangeamount();
        };

        $scope.optionToggled1 = function () {
            $scope.obj.all21 = $scope.requisitionparticulardetais.every(function (itm) { return itm.selected; });
            $scope.onchangeamount();
        };

        // Edit Data
        $scope.toggleAll2 = function (all21) {
            var toggleStatus = all21;
            angular.forEach($scope.geteditparticularsdata, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.onchangeamount();
        };

        $scope.optionToggled2 = function () {
            $scope.obj.all22 = $scope.geteditparticularsdata.every(function (itm) { return itm.selected; });
            $scope.onchangeamount();
        };

        $scope.getrequisitiondetails = function () {
            $scope.obj.all21 = false;
            $scope.tempdetails = [];
            $scope.totalgrid = [];
            $scope.requisitionparticulardetais = [];

            angular.forEach($scope.requisitiondetais, function (dd) {
                if (dd.selected === true) {
                    $scope.tempdetails.push({ PCREQTN_Id: dd.pcreqtN_Id });
                }
            });

            if ($scope.tempdetails.length > 0) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "temp_requisitionid": $scope.tempdetails
                };
                apiService.create("PC_Indent/getrequisitiondetails", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.requisitionparticulardetais = promise.requisitionparticulardetais;
                        if ($scope.requisitionparticulardetais === null || $scope.requisitionparticulardetais.length === 0) {
                            swal("No Particulars Found For Selected Requisitions");
                        } else {
                            $scope.PCINDENT_RequestedAmount = 0.00;
                            $scope.PCINDENT_RequestedAmount_Temp = 0.00;

                            $scope.PCINDENT_ApprovedAmt = 0.00;
                            $scope.PCINDENT_ApprovedAmt_Temp = 0.00;

                            angular.forEach($scope.requisitionparticulardetais, function (dd) {
                                dd.remarks = dd.pcreqtndeT_Remarks;
                                if (dd.pcreqtndeT_Amount !== undefined && dd.pcreqtndeT_Amount !== null && dd.pcreqtndeT_Amount !== "") {
                                    $scope.PCINDENT_RequestedAmount = $scope.PCINDENT_RequestedAmount + parseFloat(dd.pcreqtndeT_Amount);
                                    $scope.PCINDENT_RequestedAmount_Temp = $scope.PCINDENT_RequestedAmount_Temp + parseFloat(dd.pcreqtndeT_Amount);
                                }
                            });
                        }
                    }
                });
            }
        };

        $scope.onchangeamount = function (userobj) {

            $scope.PCINDENT_ApprovedAmt = 0.00;
            $scope.PCINDENT_ApprovedAmt_Temp = 0.00;

            if ($scope.editflag === false) {
                angular.forEach($scope.requisitionparticulardetais, function (dd) {
                    if (dd.selected === true) {
                        if (dd.amount !== undefined && dd.amount !== null && dd.amount !== "") {
                            $scope.PCINDENT_ApprovedAmt = $scope.PCINDENT_ApprovedAmt + parseFloat(dd.amount);
                            $scope.PCINDENT_ApprovedAmt_Temp = $scope.PCINDENT_ApprovedAmt_Temp + parseFloat(dd.amount);
                        }
                    }
                });
            } else {
                angular.forEach($scope.geteditparticularsdata, function (dd) {
                    if (dd.selected === true) {
                        if (dd.amount !== undefined && dd.amount !== null && dd.amount !== "") {
                            $scope.PCINDENT_ApprovedAmt = $scope.PCINDENT_ApprovedAmt + parseFloat(dd.amount);
                            $scope.PCINDENT_ApprovedAmt_Temp = $scope.PCINDENT_ApprovedAmt_Temp + parseFloat(dd.amount);
                        }
                    }
                });
            }
        };

        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {

                $scope.savePC_Indent_DetailsDTO = [];

                if ($scope.editflag === false) {
                    angular.forEach($scope.requisitionparticulardetais, function (ddd) {
                        if (ddd.selected) {
                            $scope.savePC_Indent_DetailsDTO.push({
                                PCMPART_Id: ddd.pcmparT_Id, PCINDENTDET_Amount: ddd.pcreqtndeT_Amount,
                                PCINDENTDET_ApprovedAmt: parseFloat(ddd.amount), PCINDENTDET_Remarks: ddd.remarks, PCINDENTDET_Id: ddd.pcindentdeT_Id,
                                PCREQTN_Id: ddd.pcreqtN_Id
                            });
                        }
                    });

                    if ($scope.savePC_Indent_DetailsDTO.length === 0) {
                        swal("Select Alteast One Record To Save Indent");
                        return;
                    }
                } else {
                    angular.forEach($scope.geteditparticularsdata, function (ddd) {
                        if (ddd.selected) {
                            $scope.savePC_Indent_DetailsDTO.push({
                                PCMPART_Id: ddd.pcmparT_Id, PCINDENTDET_Amount: ddd.pcindentdeT_Amount,
                                PCINDENTDET_ApprovedAmt: parseFloat(ddd.amount), PCINDENTDET_Remarks: ddd.remarks, PCINDENTDET_Id: ddd.pcindentdeT_Id,
                                PCREQTN_Id: ddd.pcreqtN_Id
                            });
                        }
                    });

                    if ($scope.savePC_Indent_DetailsDTO.length === 0) {
                        swal("Select Alteast One Record To Save Indent");
                        return;
                    }
                }

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCINDENT_Id": $scope.PCINDENT_Id,
                    "PCINDENT_Desc": $scope.obj12.PCINDENT_Desc,
                    "PCINDENT_ApprovedAmt": $scope.PCINDENT_ApprovedAmt,
                    "PCINDENT_RequestedAmount": $scope.PCINDENT_RequestedAmount,
                    "PCINDENT_Date": new Date($scope.PCINDENT_Date).toDateString(),
                    "PC_Indent_DetailsDTO": $scope.savePC_Indent_DetailsDTO
                };

                apiService.create("PC_Indent/saverecord", data).then(function (promise) {
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
            $scope.geteditdata = [];
            $scope.geteditparticularsdata = [];
            $scope.requisitiondetais = [];
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENT_Id": user.pcindenT_Id
            };
            apiService.create("PC_Indent/EditData", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditdata !== null && promise.geteditdata.length > 0) {
                        $scope.geteditdata = promise.geteditdata;

                        $scope.geteditparticularsdata = promise.geteditparticularsdata;

                        $scope.obj.all22 = true;
                        angular.forEach($scope.geteditparticularsdata, function (dd) {
                            dd.selected = true;
                            dd.amount = dd.pcindentdeT_ApprovedAmt;
                            dd.remarks = dd.pcindentdeT_Remarks;
                        });

                        $scope.PCINDENT_RequestedAmount_Temp = $scope.geteditdata[0].pcindenT_RequestedAmount;
                        $scope.PCINDENT_RequestedAmount = $scope.geteditdata[0].pcindenT_RequestedAmount;

                        $scope.obj12.PCINDENT_Desc = $scope.geteditdata[0].pcindenT_Desc;



                        $scope.PCINDENT_ApprovedAmt_Temp = $scope.geteditdata[0].pcindenT_SanctionedAmt;
                        $scope.PCINDENT_ApprovedAmt = $scope.geteditdata[0].pcindenT_SanctionedAmt;

                        $scope.PCINDENT_Date_From = new Date($scope.geteditdata[0].pcindenT_Date);
                        $scope.PCINDENT_Date_To = new Date($scope.geteditdata[0].pcindenT_Date);

                        $scope.PCINDENT_Id = $scope.geteditdata[0].pcindenT_Id;

                        $scope.editflag = true;
                    }
                }
            });
        };

        $scope.deactiveY = function (user, SweetAlert) {
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENT_Id": user.pcindenT_Id
            };

            var dystring = "";
            if (user.pcindenT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcindenT_ActiveFlg === false) {
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
                        apiService.create("PC_Indent/deactiveY", data).then(function (promise) {
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
            $scope.indentno = user.pcindenT_IndentNo;
            $scope.editapprovedflag = user.approvedflag;
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENT_Id": user.pcindenT_Id
            };
            apiService.create("PC_Indent/Viewdata", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getviewdata = promise.getviewdata;
                }
            });
        };

        $scope.deactiveparticulars = function (user, SweetAlert) {
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENTDET_Id": user.pcindentdeT_Id,
                "PCINDENT_Id": user.pcindenT_Id
            };

            var dystring = "";
            if (user.pcindentdeT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcindentdeT_ActiveFlg === false) {
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
                        apiService.create("PC_Indent/deactiveparticulars", data).then(function (promise) {
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
            return ($filter('date')(obj.pcindenT_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.departmentname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcindenT_IndentNo)).indexOf(angular.lowercase($scope.search)) >= 0;

        };


        $scope.Viewdataprint = function (user) {
            $scope.getviewdataprint = [];
            $scope.indentno = user.pcindentapT_IndentNo;
            $scope.pcindentapT_Dated = new Date(user.pcindentapT_Date);
            $scope.totalapprovedamt = user.pcindentapT_SanctionedAmt;
            var data = {
                "PCINDENTAP_Id": user.pcindentaP_Id
            };
            apiService.create("PC_Indent_Approval/Viewdata", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getviewdataprint = promise.getviewdata;
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
