(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeAmountEntryStthomasController', FeeAmountEntryStthomasController)
    FeeAmountEntryStthomasController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function FeeAmountEntryStthomasController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams) {


        $scope.typeofstudentregular = false;
        $scope.typeofstudentecs = false;
        $scope.finslab1 = [];
        $scope.finslab1 = [{ id: 'document' }];
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var headid;
        var ftiid;

        $scope.fineslb = [{ value: "Per Day", FTFS_FineType: "Per Day" },
        { value: "Day Slab", FTFS_FineType: "Day Slab" }
        ];


        $scope.fineslbecs = [{ value: "Per Day", FTFSE_FineType: "Per Day" },
        { value: "Day Slab", FTFSE_FineType: "Day Slab" }
        ];

        $scope.typeofstudentregular = false;

        $scope.sortKey = "fmA_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchthird = "";
        $scope.ecsmonthdisable = false;
        $scope.ecsdatedisable = false;
        $scope.commonamountflag = [];

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        if (configsettings.length > 0) {
            var ecsflag = configsettings[0].ispaC_ECSFlag;
        }
        $scope.savedisable = true;

        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }


        $scope.commonamount = function (totalgrid, amount, headidfrongrid) {

            for (var j = 0; j <= $scope.commonamountflag.length; j++) {
                var commflagheadid = $scope.commonamountflag[j].fmH_Id;
                for (var i = 0; i <= totalgrid.length; i++) {
                    var headid = totalgrid[i].fmH_Id
                    if (headidfrongrid == commflagheadid) {
                        amount = amount;
                    }
                    else {
                        break;
                    }

                    if (commflagheadid == headid) {
                        totalgrid[i].fmA_Amount = amount;
                    }
                }
            }

        }



        var ASMAY_Year;
        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters
            var pageid = 1;
            apiService.getURI("FeeAmountEntryStthomas/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;

                    $scope.groupcount = promise.fillmastergroup;
                    $scope.headcount = promise.fillmasterhead;

                    console.log($scope.selected);
                    $scope.installmentcount = promise.fillinstallment;
                    $scope.totalgrid = promise.fillcompany;
                    $scope.categorycount = promise.fillcategory;
                    $scope.thirdgrid = promise.alldata;

                    $scope.monthcnt = promise.fillmonth;
                    $scope.monthecscnt = promise.fillmonthecs;

                    ASMAY_Year = promise.asmaY_Year;

                    $scope.totcountfirst = $scope.thirdgrid.length;

                    if (promise.feeconfiguration.length > 0) {
                        $scope.FMC_EableStaffTrans = promise.feeconfiguration[0].fmC_EableStaffTrans;
                        $scope.FMC_EableOtherStudentTrans = promise.feeconfiguration[0].fmC_EableOtherStudentTrans;
                    }
                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.addNew = function (totalgrid) {
            $scope.totalgrid.push({
                'FMH_Id': totalgrid.FMH_Id,
                'FMI_Id': totalgrid.FMI_Id,
                'FYGHM_FineApplicableFlag': totalgrid.FYGHM_FineApplicableFlag,
                'FYGHM_Common_AmountFlag': totalgrid.FYGHM_Common_AmountFlag,
                'FYGHM_ActiveFlag': totalgrid.FYGHM_ActiveFlag,
            });
            $scope.PD = {};
        };


        $scope.selectacademicyear = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeAmountEntryStthomas/selectacademicyear", data).
                then(function (promise) {
                    $scope.categorycount = promise.fillcategory;
                    $scope.groupcount = promise.fillmastergroup;
                    $scope.thirdgrid = promise.alldata;
                })
        }

        $scope.remove = function (totalgrid) {
            var newDataList = [];
            $scope.selectedAll = false;
            angular.forEach($scope.totalgrid, function (selected) {
                if (!selected.selected) {
                    newDataList.push(selected);
                }
            });
            $scope.totalgrid = newDataList;
        };
        $scope.FMCC_Id = "";
        $scope.FMG_Id = "";
        // $scope.selected = {};
        $scope.regslbary = [];
        $scope.ecsslbary = [];
        $scope.onselectgroup = function (groupcount) {

            if ($scope.FMCC_Id == "" && $scope.stuchk == 'stud') {
                swal("Select Category First");
                $scope.FMG_Id = "";
            }
            else if ($scope.FMCC_Id != "" && $scope.FMG_Id != "") {
                $scope.slab1 = true;
                $scope.slab2 = true;

                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "FMCC_Id": $scope.FMCC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "selectiontype": $scope.stuchk
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
            }


            apiService.create("FeeAmountEntryStthomas/getgroupmappedheads", data).
                then(function (promise) {

                    var amountentryarray = [];
                    $scope.plannerid = [];
                    if (promise.allgroupheaddata.length > 0) {

                        angular.forEach(promise.allgroupheaddata, function (obj) {
                            amountentryarray.push(obj);
                        })

                        angular.forEach(promise.newlyupdatedrec, function (obj1) {
                            var already_cnt = 0;
                            angular.forEach(promise.allgroupheaddata, function (obj) {
                                if (obj1.fmH_Id == obj.fmH_Id) {
                                    already_cnt += 1;
                                }
                            })
                            if (already_cnt == 0) {
                                amountentryarray.push(obj1);
                            }
                        })

                        promise.allgroupheaddata = amountentryarray;
                        //}

                        $scope.grigview1 = true;
                        $scope.regslbary = promise.fineslabdetails;
                        $scope.ecsslbary = promise.fineslabdetailsecs;

                        $scope.get_approvalreport = [];
                        // $scope.totalgrid = promise.allgroupheaddata;
                        $scope.instllmentdetails = promise.instllmentdetails;

                        if (promise.allgroupheaddata.length > 0) {
                            $scope.totalgrid = promise.allgroupheaddata;
                            angular.forEach($scope.totalgrid, function (dev) {
                                if ($scope.plannerid.length === 0) {

                                    $scope.plannerid.push({
                                        FMH_Id: dev.fmH_Id,
                                        fmH_FeeName: dev.fmH_FeeName,
                                        fyghM_FineApplicableFlag: dev.fyghM_FineApplicableFlag,
                                        showdetails: dev.showdetails

                                    });
                                } else if ($scope.plannerid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.plannerid, function (emp) {
                                        if (emp.FMH_Id === dev.fmH_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.plannerid.push({

                                            FMH_Id: dev.fmH_Id,
                                            fmH_FeeName: dev.fmH_FeeName,
                                            fyghM_FineApplicableFlag: dev.fyghM_FineApplicableFlag,
                                            showdetails: dev.showdetails
                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.plannerid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.totalgrid, function (dd) {
                                    if (dd.fmH_Id === ddd.FMH_Id) {
                                        $scope.templist.push({
                                            ftI_Id: dd.ftI_Id,
                                            fmA_Amount: dd.fmA_Amount,
                                            fmA_Id: dd.fmA_Id,
                                            FMA_DueDate: new Date(dd.fmA_DueDate),
                                            FMA_PartialRebateApplicableDate: new Date(dd.fmA_PartialRebateApplicableDate),
                                            ftddE_DueDate: new Date(dd.ftddE_DueDate),
                                            fyghM_FineApplicableFlag: dd.fyghM_FineApplicableFlag,
                                            ftI_Name: dd.fmI_Name,
                                            fmH_Id:dd.fmH_Id
                                        });
                                    }
                                });
                                ddd.feeinstallmentdata = $scope.templist;
                            });
                        }

                        $scope.saveflg = true;



                        if ($scope.regslbary.length > 0 || $scope.ecsslbary.length > 0) {
                            angular.forEach($scope.regslbary, function (rgobj) {
                                angular.forEach($scope.totalgrid, function (totgdobj) {
                                    if (rgobj.fmH_Id == totgdobj.fmH_Id && rgobj.ftI_Id == totgdobj.ftI_Id) {
                                        totgdobj.details = true;
                                        totgdobj.showdetails = true;
                                    }
                                });
                            })
                            angular.forEach($scope.ecsslbary, function (ecsobj) {
                                angular.forEach($scope.totalgrid, function (totgdobj) {
                                    if (ecsobj.fmH_Id == totgdobj.fmH_Id && ecsobj.ftI_Id == totgdobj.ftI_Id) {
                                        totgdobj.details = true;
                                        totgdobj.showdetails = true;
                                    }
                                });
                            })
                        }

                        angular.forEach($scope.totalgrid, function (obj) {
                            obj.ftddE_Day = parseInt(obj.ftddE_Day);

                            obj.fmA_DueDate = new Date(obj.fmA_DueDate);

                            obj.ftddE_DueDate = new Date(obj.ftddE_DueDate);

                            obj.fmA_PartialRebateApplicableDate = new Date(obj.fmA_PartialRebateApplicableDate);

                        })
                        console.log($scope.totalgrid);
                        $scope.commonamountflag = promise.commountamountflag;

                        $scope.ispaC_ECSFlag = ecsflag;
                    }

                    else {
                        swal("No Records found.Kindly map Group with heads!!")
                        $scope.grigview1 = false;
                    }
                })

        };

        $scope.editEmployee = {}
        $scope.DeletRecord = function (amountid, selectiontype) {

            var data = {
                "FMA_Id": amountid,
                "selectiontype": selectiontype
            }
            var amtid = amountid;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeAmountEntryStthomas/Deletedetails/", data).
                            then(function (promise) {


                                if (promise.returnval == "true") {
                                    swal('Record Deleted Successfully');
                                }
                                else if (promise.returnval == "false") {
                                    swal('Contact Administrator');
                                }
                                else if (promise.returnval == "RecordExists") {
                                    swal('Data has already been used in Transactions!! So record cannot be deleted');
                                }
                                $scope.formload();

                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });

        }

        $scope.cleardata = function () {

            $scope.searchthird = "";
            $scope.ASMAY_Id = "";
            $scope.FMG_Id = "";
            $scope.FMCC_Id = "";
            $scope.grigview1 = false;
            $state.reload();
        }

        $scope.slab1 = true;
        $scope.slab2 = true;

        var regval = "";
        var ecsval = "";
        $scope.cancell = function () {

            $scope.typeofstudentregular = false;
            $scope.typeofstudentecs = false;
            $scope.finslab1 = [];
            $scope.finslab2 = [];
            $scope.slab1 = false;
            $scope.slab2 = false;
        }
        $scope.clicktypeofstudent = function (typeofstudentregular, typeofstudentecs) {

            var typeofcatreg = typeofstudentregular;
            var typeofcatecs = typeofstudentecs;

            if (typeofcatreg == true) {
                $scope.slab1 = true;
                regval = "R";
            }
            else {
                $scope.slab1 = false;
            }

            if (typeofcatecs == true) {
                $scope.slab2 = true;
                ecsval = "E";
            }
            else {
                $scope.slab2 = false;
            }

            var data = {
                "regularfalg": regval,
                "ecsflag": ecsval
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeAmountEntryStthomas/paymentdetails", data).
                then(function (promise) {

                    if (typeofcatreg == true) {
                        $scope.finslab1 = promise.fillslab;
                        console.log($scope.finslab1);
                    } else {
                        $scope.finslab1 = [];
                    }
                    if (typeofcatecs == true) {
                        $scope.finslab2 = promise.fillslabecs;
                    }
                    else {
                        $scope.finslab2 = [];
                    }

                })

        }

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;

            apiService.getURI("FeeAmountEntryStthomas/Editdetails", orgid).
                then(function (promise) {
                    $scope.addnewbtn = false;
                    $scope.FMG_Id = promise.alldata[0].fmG_Id;
                    $scope.totalgrid.headcount.selected.FMH_Id = promise.alldata[0].fmH_Id;
                    $scope.totalgrid.installmentcount.FMI_Id = promise.alldata[0].fmI_Id;
                })
        }

        $scope.filterby = function () {
            var entereddata = $scope.search;

            var data = {
                "FMG_GroupName": $scope.searchthird,
                "FMH_FeeName": $scope.typethird
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeAmountEntryStthomas/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                })
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };


        $scope.submitted = false;
        $scope.savedata = function () {

            $scope.submitted = true;

            if ($scope.myform.$valid) {
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Save Record?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Save it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            console.log($scope.plannerid);
                            $scope.totalgrd = [];

                            angular.forEach($scope.plannerid, function (obj) {
                                angular.forEach(obj.feeinstallmentdata, function (obj1) {
                                    $scope.fineslabdet = [];
                                    //savedatatrans1
                                    angular.forEach($scope.savedatatrans1, function (obj2) {

                                        

                                        if (obj1.fmH_Id == obj2.fmH_Id && obj1.ftI_Id == obj2.ftI_Id) {
                                            $scope.fineslabdet.push({
                                                FMH_ID: obj2.fmH_Id,
                                                FTI_ID: obj2.ftI_Id,
                                                FMFS_Duedate: obj2.fmfs_duedate,
                                                FTFS_Id: obj2.ftfS_Id,
                                                FTFS_FineType: obj2.ftfS_FineType,
                                                FTFS_Amount: obj2.ftfS_Amount
                                                
                                            })

                                        }
                                    })

                                        if ($scope.fineslabdet.length > 0) {
                                            $scope.totalgrd.push({

                                                FMA_Id: obj1.fmA_Id,
                                                FMH_Id: obj1.fmH_Id,
                                                FTI_Id: obj1.ftI_Id,
                                                FMA_Amount: obj1.fmA_Amount,
                                                FMA_DueDate: new Date($scope.fineslabdet[0].FMFS_Duedate).toDateString(),
                                                FTDDE_DueDate: new Date($scope.fineslabdet[0].FMFS_Duedate).toDateString(),
                                                savefineslabreg: $scope.fineslabdet
                                            })
                                        }
                                        else {
                                            $scope.totalgrd.push({

                                                FMA_Id: obj1.fmA_Id,
                                                FMH_Id: obj1.fmH_Id,
                                                FTI_Id: obj1.ftI_Id,
                                                FMA_Amount: obj1.fmA_Amount,
                                                FMA_DueDate: new Date().toDateString(),
                                                FTDDE_DueDate: new Date().toDateString(),
                                                savefineslabreg: $scope.fineslabdet
                                            })
                                        }

                                  
                                })
                            })


                            console.log($scope.totalgrd);
                            var data = {
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "FMCC_Id": $scope.FMCC_Id,
                                "FMG_Id": $scope.FMG_Id,
                                savetmpdata: $scope.totalgrd,
                             

                            }

                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }

                        }
                        apiService.create("FeeAmountEntryStthomas/", data).
                            then(function (promise) {

                                if (promise.returnval != "false") {
                                    if (promise.returnval == "used") {
                                        $scope.addnewbtn = true;
                                        swal('Record already Used');
                                    }
                                    else {
                                        $scope.addnewbtn = true;
                                        swal('Record ' + promise.amtentrystatus + ' Successfully');
                                    }

                                }

                                else {
                                    $scope.addnewbtn = true;
                                    swal('Contact Administrator');
                                }
                                $state.reload();
                            })

                    });
            }
            else {

                $scope.submitted = true;
            }
        };


        $scope.totgdchkbx = function (totary, tchkbx, index, ftiname, ftiid, fmH_Id) {

            $scope.finslab1 = [];
            $scope.finslab1 = [{ id: 'document' }];
            if (tchkbx == true) {
                $scope.finslab1 = [];
                $scope.finslab2 = [];
                $scope.slab1 = false;
                $scope.slab2 = false;

                $scope.typeofstudentregular = false;
                $scope.typeofstudentecs = false;

                headid = fmH_Id;
                ftiid = ftiid;
                $scope.ftiname = ftiname;
                $('#myModal').modal('show');


                $scope.finslab1.push({
                    fmH_ID: fmH_Id,
                    ftI_ID: ftiid,
                    ftfS_Amount: 0,
                    fmfs_duedate: new Date()
                })

            }
        }
        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.finslab1.length - 1;
            $scope.finslab1.splice(index, 1);

        };
        $scope.addNewDocumentOtherDetail = function (fmH_Id, ftiid) {
            var newItemNo = $scope.finslab1.length + 1;
            if (newItemNo <= 30) {
                $scope.finslab1.push({
                    fmH_Id: fmH_Id,
                    ftI_Id: ftiid,
                    ftfS_Amount: 0,
                    fmfs_duedate: new Date()
                })
            }
        };
        $scope.headinstallmentid = function (ftI_Id, fmH_Id, ftiname) {

            $scope.finslab1 = [];
         //   $scope.savedatatrans1 = [];
            $scope.typeofstudentregular = false;

            headid = fmH_Id;
            ftiid = ftI_Id;

            var data = {
                "FMG_Id": $scope.FMG_Id,
                "FMCC_Id": $scope.FMCC_Id,
                "selectiontype": $scope.stuchk,
                "ASMAY_Id": $scope.ASMAY_Id,
                "FTI_Id": ftI_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeAmountEntryStthomas/getgroupmappedheads/", data).
                then(function (promise) {

                    $scope.savedatatransregular = [];
                    $scope.savedatatransecs = [];
                    var r = 0;
                    var e = 0;
                    if (promise.fineslabdetails.length > 0) {
                        $scope.slab1 = true;

                        $scope.slab2 = true;

                        angular.forEach(promise.fineslabdetails, function (user) {
                            user.fmfs_duedate = new Date(user.ftfS_Date)

                        })
                        var lenofarray = promise.fineslabdetails.length;

                        for (var i = 0; i < lenofarray; i++) {
                            if (promise.fineslabdetails[i].fmH_Id == headid && promise.fineslabdetails[i].ftI_Id == ftiid) {
                                $scope.savedatatransregular[r] = promise.fineslabdetails[i];
                                $scope.savedatatransregular[r].isSelected = true;
                                r = r + 1;
                            }
                        }



                        if ($scope.savedatatransregular.length > 0) {
                            $scope.typeofstudentregular = true;
                            $scope.slab1 = true;
                            $scope.slab2 = false;
                            $scope.finslab1 = $scope.savedatatransregular;

                        }
                        if ($scope.savedatatransecs != null) {
                            if ($scope.savedatatransecs.length > 0) {
                                $scope.typeofstudentecs = true;
                                $scope.slab2 = true;
                                $scope.slab1 = true;
                                $scope.finslab2 = $scope.savedatatransecs;
                            }
                        }
                        $scope.ftiname = ftiname;
                    }     

                    // Sudashan 25-11-2023

                    else if ($scope.savedatatrans1.length > 0) {

                        angular.forEach($scope.savedatatrans1, function (user) {
                            if (user.ftI_Id == ftiid) {

                                $scope.finslab1.push({
                                    fmH_Id: user.fmH_Id,
                                    ftI_Id: user.ftI_Id,
                                    ftfS_Amount: user.ftfS_Amount,
                                    ftfS_FineType: user.ftfS_FineType,
                                    fmfs_duedate: new Date(user.fmfs_duedate)
                                })

                            }

                        })

                        if ($scope.finslab1.length == 0) {

                            $scope.finslab1.push({
                                fmH_Id: fmH_Id,
                                ftI_Id: ftiid,
                                ftfS_Amount: 0,
                                fmfs_duedate: new Date()
                            })
                        }

                        $scope.ftiname = ftiname;
                    }


                    else {
                        $scope.slab1 = false;
                        $scope.slab2 = false;
                        $scope.finslab1.push({
                            fmH_Id: fmH_Id,
                            ftI_Id: ftiid,
                            ftfS_Amount: 0,
                            fmfs_duedate: new Date()
                        })
                        $scope.ftiname = ftiname;
                    }
                })

        }
        $scope.savedatatrans1 = [];
        $scope.savedatatrans2 = [];
        var count = 0;
        var count1 = 0;
        $scope.addtocart = function (finslab1) {
            var countofanarray = $scope.savedatatrans1.length;

            if ($scope.savedatatrans1.length > 0) {

                var tempArr = [];
                angular.forEach($scope.savedatatrans1, function (value) {
                    if (ftiid != value.ftI_Id) {
                        tempArr.push(value);
                    }
                });
                $scope.savedatatrans1 = tempArr;

            }

            angular.forEach($scope.finslab1, function (user) {               
                    $scope.savedatatrans1.push(user);
                    count = Number(count) + 1
             
            })
           
            //plannerid
            //angular.forEach($scope.plannerid, function (user) {

            //    angular.forEach(user.feeinstallmentdata, function (user) {

            //    })
            //})
            $scope.finslab1 = [];
            $scope.finslab1 = [{ id: 'document' }];      

            if (count == 0) {
                swal("Kindly select any one from Regular slab");
            }
            else {
                swal("Selected Fine Slab is added successfully");
                $scope.typeofstudentregular = "";
                $scope.typeofstudentecs = "";
            }
        }

        $scope.validdate = function (month, day, user123) {

            var dayno;
            if (month != "" || month != undefined) {
                angular.forEach($scope.monthcnt, function (itm1) {
                    if (itm1.ftdD_Month == month) {
                        if (month == 2) {

                            var year_str;
                            for (var i = 0; i < ASMAY_Year.length; i++) {

                                if (i > 4) {
                                    if (i == 5) {
                                        year_str = ASMAY_Year[5];
                                    } else {
                                        year_str += ASMAY_Year[i];
                                    }
                                }
                            }
                            var year = Number(year_str);
                            if (year % 4 == 0 || year % 100 != 0 && year % 400 == 0) {
                                dayno = itm1.amM_Month_Max_Days + 1;
                            }
                            else {
                                dayno = itm1.amM_Month_Max_Days;
                            }
                        }
                        else {
                            dayno = itm1.amM_Month_Max_Days;
                        }
                        if (Number(day) > dayno) {
                            swal("Date Not More Than No.of Days Per Selected Month");
                            angular.forEach($scope.totalgrid, function (itm) {
                                if (itm.fmA_Id == user123.fmA_Id) {
                                    itm.ftdD_Day = "";
                                }
                            })

                        }

                    }
                })
            }
            else {
                swal("First Select Month !!!");
                $scope.ftdD_Day = "";
            }




        }

    }

})();