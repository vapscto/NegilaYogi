
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_ItemConsumptionController', INV_ItemConsumptionController);
    INV_ItemConsumptionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_ItemConsumptionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var date = new Date();
        $scope.invmiC_ICDate = date;
        $scope.transgrid = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addicrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i === $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.removeicrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletegrnrows(data);
            }
        };
        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_ItemConsumption/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_Store = promise.get_Store;
                    //  $scope.get_item = promise.get_item;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_Department = promise.get_Department;
                    $scope.get_Student_Cls_Sec = promise.get_Student_Cls_Sec;
                    $scope.get_Product = promise.get_Product;
                    $scope.get_itemconsumption = promise.get_itemconsumption;
                    $scope.presentCountgrid = $scope.get_itemconsumption.length;
                });
        };
        //====================================== RADIO CHANGE
        $scope.userchange = function () {
            $scope.get_employee = "";
            $scope.get_Department = "";
            $scope.get_Student_Cls_Sec = "";
            if ($scope.invmiC_StuOtherFlg === 'Staff') {
                $scope.transgrid = false;
            }
            else {
                $scope.transgrid = true;
            }
            $scope.loaddata();
        };

        //================== Select All Check & Validation 
        $scope.togchkbx = function () {
            $scope.staffCk = $scope.get_employee.every(function (stff) {
                return stff.hrmeid;
            });
        };
        $scope.isOptionsRequired = function () {
            if ($scope.invmiC_StuOtherFlg === 'Staff') {
                return !$scope.get_employee.some(function (options) {
                    return options.hrmeid;
                });
            } else {
                return false;
            }
        };
        $scope.all_check = function (abcd) {
            $scope.staffCk = abcd;
            var toggleStatus = $scope.staffCk;
            angular.forEach($scope.get_employee, function (stf) {
                stf.hrmeid = toggleStatus;
            });
        };


        //=============================================== Get Item on store Change
        $scope.storeChange = function () {
            var data = {
                "INVMST_Id": $scope.invmsT_Id
            };
            apiService.create("INV_T_Sales/getitem", data).
                then(function (promise) {

                    if (promise.get_item.length > 0) {
                        $scope.get_item = promise.get_item;
                    }
                    else {
                        swal('For Selected Store, No Item found..!!');
                        $scope.get_item.length = 0;
                    }
                });
        };

        //============================================= Edit grid Checkbox

        $scope.oneditCheck = function (objice) {
            if (objice.checkedvalue === true) {
                $scope.onitemchange(objice);
            }
        };
        //=====================================Get Item deatils On item Change
        $scope.onitemchange = function (objid) {
            var avstock = 0;
            var data = {};
            if ($scope.editS === true) {
                data = {
                    "INVMI_Id": objid.invmI_Id,
                    "INVSTO_SalesRate": objid.invstO_SalesRate,
                    "INVMST_Id": $scope.invmsT_Id
                };
            }
            else {
                data = {
                    "INVMI_Id": objid.invmI_Id.INVMI_Id,
                    "INVSTO_SalesRate": objid.invmI_Id.INVSTO_SalesRate,
                    "INVMST_Id": $scope.invmsT_Id
                };
            }

            apiService.create("INV_T_Sales/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    $scope.availablestock = promise.availablestock;
                    angular.forEach($scope.availablestock, function (ast) {
                        avstock += ast.AvaiableStock;
                    });
                    $scope.availableitems = avstock;
                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id === objid.itrS_Id) {
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;
                            obj.invtiC_ICQty = 0.00;
                        }
                    });

                });
        };

        $scope.showtrans = function () {
            var stucnt = 0;
            if ($scope.get_employee.length > 0) {
                angular.forEach($scope.get_employee, function (staf) {
                    if (staf.hrmeid === true) {
                        stucnt += 1;
                    }
                });
                if (stucnt > 0) {
                    $scope.transgrid = true;
                }
                else {
                    swal("Select Atleast One Checkbox..!!");
                }
            }

        };
        //=====================================Count Amount

        $scope.countAmt = function (objstk, items) {
            var availablestok = 0;
            var stucnt = 0;
            var qqty = 0;
            var ttqqty = 0;
            $scope.qty = 0;
            if ($scope.get_employee.length > 0) {
                $scope.transgrid = true;
                if ($scope.invmiC_StuOtherFlg == 'Staff') {
                    angular.forEach($scope.get_employee, function (stf) {
                        if (stf.hrmeid === true) {
                            stucnt += 1;
                        }
                    });
                }


                if ($scope.invmiC_StuOtherFlg != 'Department') {

                    
                        $scope.transgrid = true;
                        if ($scope.invmiC_StuOtherFlg == 'Student') {
                            angular.forEach($scope.get_Studentlist, function (stf) {
                                if (stf.secck === true) {
                                    stucnt += 1;
                                }
                            });

                        }
                    
                    if (stucnt > 0) {
                        qqty = parseFloat(objstk.icqty);
                        ttqqty = qqty * stucnt;
                        objstk.invtiC_ICQty = ttqqty;
                        $scope.qty = ttqqty;
                    }
                    else {
                        swal("Select Atleast One Checkbox..!!");
                        objstk.invtiC_ICQty = "";
                    }
                }
            }

            


            else {
                $scope.qty = parseFloat(objstk.invtiC_ICQty);
            }
            availablestok = parseFloat($scope.availableitems);
            if ($scope.qty > availablestok) {
                swal("Please Check Available Stock...!!");
                objstk.invtiC_ICQty = ""; $scope.qty = "";
            }
        };
        //==================
        $scope.classSectionC = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("INV_ItemConsumption/getsection", data).then(function (promise) {
                $scope.get_sectionlist = promise.get_sectionlist;
            })
        }
        $scope.classSectionCS = function () {
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "FlagsC": ""
            }
            apiService.create("INV_ItemConsumption/getstudent", data).then(function (promise) {
                $scope.get_Studentlist = promise.get_Student;
                $scope.transgrid = false;
            })
        }

        $scope.togchkbxCS = function () {
            $scope.usercheckCS = $scope.get_Studentlist.every(function (options) {
                return options.secck;
            });
        };
        $scope.all_checkCS = function () {
            var checkStatus2 = $scope.usercheckCS;
            angular.forEach($scope.get_Studentlist, function (itm) {
                itm.secck = checkStatus2;
            });
        };
        //$scope.all_checkCS = function (adasd) {
        //    $scope.usercheckCS = $scope.usercheckCS;
        //    var toggleStatus = $scope.usercheckCS;
        //    angular.forEach($scope.get_Studentlist, function (sec) {
        //        sec.secck = toggleStatus;
        //    });
        //};
        $scope.showtrans1 = function () {
            var stucnt = 0;
            if ($scope.get_Studentlist.length > 0) {
                angular.forEach($scope.get_Studentlist, function (cl) {
                    if (cl.secck === true) {
                        stucnt += 1;
                    }

                });
                if (stucnt > 0) {
                    $scope.transgrid = true;
                }
                else {
                    swal("Select Atleast One Checkbox..!!");

                }

            }

        };

        $scope.isOptionsRequired1 = function () {
            if ($scope.invmsL_StuOtherFlg === 'Student') {
                return !$scope.get_Studentlist.some(function (options) {
                    return options.secck;
                });
            } else {
                return false;
            }

        };
        //===================================== SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {};
                $scope.arrayIC = [];
                if ($scope.editS === true) {
                    angular.forEach($scope.transrows, function (ic) {
                        $scope.arrayIC.push({
                            invtiC_Id: ic.invtiC_Id, invmI_Id: ic.invmI_Id, invmuoM_Id: ic.invmuoM_Id, invmP_Id: ic.invmP_Id, invtiC_BatchNo: ic.invtiC_BatchNo, invtiC_ICQty: ic.invtiC_ICQty, invtiC_Naration: ic.invtiC_Naration
                        });
                    });
                }
                else {
                    angular.forEach($scope.transrows, function (ic) {
                        $scope.arrayIC.push({
                            invtiC_Id: ic.invtiC_Id, invmI_Id: ic.invmI_Id.INVMI_Id, invtiC_ICPrice: ic.invmI_Id.INVSTO_SalesRate, invmuoM_Id: ic.invmuoM_Id, invmP_Id: ic.invmP_Id, invtiC_BatchNo: ic.invtiC_BatchNo, invtiC_ICQty: ic.invtiC_ICQty, invtiC_Naration: ic.invtiC_Naration
                        });
                    });
                }
                if ($scope.invmiC_StuOtherFlg === "Staff") {
                    $scope.arrayStaff = [];
                    angular.forEach($scope.get_employee, function (ep) {
                        if (ep.hrmeid === true) {
                            $scope.arrayStaff.push(ep);
                        }
                    });
                    data = {
                        "INVMIC_Id": $scope.invmiC_Id,
                        "INVMST_Id": $scope.invmsT_Id,
                        "INVMIC_StuOtherFlg": $scope.invmiC_StuOtherFlg,
                        "INVMIC_ICNo": $scope.invmiC_ICNo,
                        "INVMIC_ICDate": $scope.invmiC_ICDate,
                        "INVMIC_Remarks": $scope.invmiC_Remarks,
                        "arrayIC": $scope.arrayIC,
                        "arrayStaff": $scope.arrayStaff
                    };
                }
                else if ($scope.invmiC_StuOtherFlg === "Department") {
                    $scope.dept_id = $scope.obj.hrmD_Id.hrmD_Id;
                    data = {
                        "INVMIC_Id": $scope.invmiC_Id,
                        "INVMST_Id": $scope.invmsT_Id,
                        "INVMIC_StuOtherFlg": $scope.invmiC_StuOtherFlg,
                        "INVMIC_ICNo": $scope.invmiC_ICNo,
                        "INVMIC_ICDate": $scope.invmiC_ICDate,
                        "INVMIC_Remarks": $scope.invmiC_Remarks,
                        "HRMD_Id": $scope.dept_id,
                        "arrayIC": $scope.arrayIC
                    };
                }
                else if ($scope.invmiC_StuOtherFlg === "Student") {
                    $scope.arrayStudentname = [];
                    angular.forEach($scope.get_Studentlist, function (cl) {
                        if (cl.secck === true) {
                            $scope.arrayStudentname.push({ AMST_Id: cl.amsT_Id });
                        }

                    });

                   // $scope.student_id = $scope.obj.amsT_Id.amsT_Id;
                    data = {
                        "INVMIC_Id": $scope.invmiC_Id,
                        "INVMST_Id": $scope.invmsT_Id,
                        "INVMIC_StuOtherFlg": $scope.invmiC_StuOtherFlg,
                        "INVMIC_ICNo": $scope.invmiC_ICNo,
                        "INVMIC_ICDate": $scope.invmiC_ICDate,
                        "INVMIC_Remarks": $scope.invmiC_Remarks,
                        //  "AMST_Id": $scope.student_id,
                        "arrayIC": $scope.arrayIC,
                        arrayStudentname: $scope.arrayStudentname,
                         "FlagsC": "S"
                    };
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_ItemConsumption/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invmiC_Id === 0 || promise.invmiC_Id < 0) {
                            if (promise.returnduplicatestatus === 'Updated') {
                                swal('Record saved successfully / Stock Updated');
                            }
                            if (promise.returnduplicatestatus === 'notUpdated') {
                                swal('Record saved successfully / Failed to Update Stock');
                            }
                            // swal('Record saved successfully');
                        }
                        else if (promise.invmiC_Id > 0) {
                            if (promise.returnduplicatestatus === 'Updated') {
                                swal('Record updated successfully / Stock Updated');
                            }
                            if (promise.returnduplicatestatus === 'notUpdated') {
                                swal('Record updated successfully / Failed to Update Stock');
                            }
                            // swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmiC_Id === 0 || promise.invmiC_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmiC_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                    $scope.submitted = false;
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMIC_Id = item.invmiC_Id;
            var dystring = "";
            if (item.invmiC_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmiC_ActiveFlg === false) {
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
                        apiService.create("INV_ItemConsumption/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.edit = function (item) {
            $scope.get_ICdetails = [];
            $scope.get_employee = [];
            $scope.invmiC_StuOtherFlg = "";
            $scope.transrows = [];
            $scope.invmsT_Id = item.invmsT_Id;
            $scope.invmiC_Id = item.invmiC_Id;
            $scope.invmiC_ICNo = item.invmiC_ICNo;
            $scope.invmiC_ICDate = new Date(item.invmiC_ICDate);
            $scope.editS = true;
            var invmiC_Id = item.invmiC_Id;
            var data = {
                "INVMIC_Id": invmiC_Id,
                "userflag": item.invmiC_StuOtherFlg
            };
            apiService.create("INV_ItemConsumption/getICDetails", data).
                then(function (promise) {

                    $scope.get_ICdetails = promise.get_ICdetails;
                    $scope.get_ICuser = promise.get_ICuser;
                    $scope.userfg = $scope.get_ICuser[0].INVMIC_StuOtherFlg;
                    if ($scope.userfg === "Staff") {
                        $scope.staffuser = $scope.get_ICuser;
                        $scope.invmiC_StuOtherFlg = "Staff";
                        angular.forEach($scope.staffuser, function (obju) {
                            $scope.get_employee.push({ hrmE_Id: obju.HRME_Id, employeename: obju.username, hrmE_EmployeeCode: obju.HRME_EmployeeCode, hrmeid: true, staffCk: true });
                        });
                    }
                    else if ($scope.userfg === "Department") {
                        $scope.hrmD_DepartmentName = $scope.get_ICuser[0].username;
                        $scope.hrmD_Id = $scope.get_ICuser[0].hrmD_Id;
                        $scope.invmiC_StuOtherFlg = "Department";
                    }
                    else if ($scope.userfg === "Student") {
                        $scope.studentname = $scope.get_ICuser[0].username;
                        $scope.amsT_AdmNo = $scope.get_ICuser[0].AMST_AdmNo;
                        $scope.amsT_Id = $scope.get_ICuser[0].amsT_Id;
                        $scope.invmiC_StuOtherFlg = "Student";
                    }
                    $scope.cnt = 0;
                    angular.forEach($scope.get_ICdetails, function (objic) {
                        $scope.cnt = $scope.cnt + 1;
                        var newItemNo = $scope.cnt;
                        $scope.transrows.push({
                            invtiC_Id: objic.invtiC_Id, invmI_Id: objic.invmI_Id, invmI_ItemName: objic.invmI_ItemName, invmuoM_Id: objic.invmuoM_Id, invstO_SalesRate: objic.invtiC_ICPrice, invmuoM_UOMName: objic.invmuoM_UOMName, invmP_Id: objic.invmP_Id, invmP_ProductName: objic.invmP_ProductName, invtiC_BatchNo: objic.invtiC_BatchNo, invtiC_ICQty: objic.invtiC_ICQty, invtiC_Naration: objic.invtiC_Naration, 'itrS_Id': 'trans' + newItemNo
                        });
                    });
                    $scope.showtrans(); $scope.showtrans1();
                });
        };
        //================================== View Model
        $scope.onviewform = function (id) {
            $scope.userflg = "";
            $scope.icuser = "";
            $scope.admno = "";
            $scope.staffuser = [];
            var data = {
                "INVMIC_Id": id.invmiC_Id,
                "userflag": id.invmiC_StuOtherFlg
            };
            apiService.create("INV_ItemConsumption/getICDetails", data).
                then(function (promise) {
                    $scope.get_ICdetails = promise.get_ICdetails;
                    $scope.get_ICuser = promise.get_ICuser;
                    $scope.userfg = $scope.get_ICuser[0].INVMIC_StuOtherFlg;
                    if ($scope.userfg === "Staff") {
                        $scope.staffuser = $scope.get_ICuser;
                    }
                    else if ($scope.userfg === "Department") {
                        $scope.icuser = $scope.get_ICuser[0].username;
                        $scope.userflg = "Department";
                    }
                    else if ($scope.userfg === "Student") {
                        $scope.icuser = $scope.get_ICuser[0].username;
                        $scope.admno = $scope.get_ICuser[0].amsT_AdmNo;
                        $scope.userflg = "Student";
                    }
                });
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue1 = '';
        $scope.searchValueC = "";
        $scope.searchValueCS = "";
    }
})();