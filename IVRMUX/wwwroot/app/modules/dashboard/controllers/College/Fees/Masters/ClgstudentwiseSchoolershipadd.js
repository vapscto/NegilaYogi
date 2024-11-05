(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgstudentwiseSchoolershipaddController', ClgstudentwiseSchoolershipaddController)
    ClgstudentwiseSchoolershipaddController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ClgstudentwiseSchoolershipaddController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var headid;
        var ftiid;

        $scope.finslab1 = [];
        $scope.finslab2 = [];

        $scope.arrays = [];
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

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        if (configsettings.length > 0) {
            var ecsflag = configsettings[0].ispaC_ECSFlag;
            var grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        }


        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
        }

        $scope.categoryshow = true;
        $scope.changeradio = function (abcc) {
            $scope.grigview1 = false;
            $scope.FMG_Id = "";
            $scope.FMCC_Id = "";
            $scope.ASMAY_Id = "";
            var data = {
                "selectiontype": abcc
            }
            apiService.create("CollegeStudentwiseAmtEntry/getalldetailsOnselectiontype", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    $scope.totcountfirst = $scope.thirdgrid.length;
                })
        }

        var amount;
        $scope.commonamount = function (totalgrid, amount, headidfrongrid) {


            for (var j = 0; j <= $scope.commonamountflag.length; j++) {
                var commflagheadid = $scope.commonamountflag[j].fmH_Id;
                for (var i = 0; i <= totalgrid.length; i++) {
                    var headid = totalgrid[i].fmH_Id;
                    if (headidfrongrid == commflagheadid) {
                        amount = amount;
                    }
                    else {
                        break;
                    }

                    if (commflagheadid == headid) {
                        totalgrid[i].fcmaS_Amount = amount;
                    }
                }
            }

        }

        // $scope.selected = {};


        //$scope.namesecs = [{ value: 1, FTDDE_Month: "January" },
        //                  { value: 2, ftddE_Month: "February" },
        //                   { value: 3, FTDDE_Month: "March" },
        //                    { value: 4, FTDDE_Month: "April" },
        //                     { value: 5, FTDDE_Month: "May" },
        //                      { value: 6, FTDDE_Month: "June" },
        //                       { value: 7, FTDDE_Month: "July" },
        //                        { value: 8, FTDDE_Month: "August" },
        //                         { value: 9, FTDDE_Month: "September" },
        //                          { value: 10, FTDDE_Month: "October" },
        //                           { value: 11, FTDDE_Month: "November" },
        //                            { value: 12, FTDDE_Month: "December" },
        //];

        var ASMAY_Year;
        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;


            apiService.create("CollegeStudentwiseAmtEntry/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;
                    $scope.ASMAY_Id = promise.currfillyear[0].asmaY_Id;
                    $scope.Fillcourse = promise.fillcourse;
                    $scope.groupcount = promise.fillgroup;
                    $scope.thirdgrids = promise.savedrecord;
                    $scope.totcountfirst = $scope.thirdgrids.length;

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

            apiService.create("CollegeStudentwiseAmtEntry/selectacademicyear", data).
                then(function (promise) {
                    $scope.Fillcourse = promise.temp;
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
        $scope.regslbary = [];
        $scope.ecsslbary = [];
        $scope.onselectcourse = function () {
            var AMCO_Ids = [];
            //var AMB_Ids = [];
            angular.forEach($scope.Fillcourse, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                AMCO_Ids: AMCO_Ids,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeStudentwiseAmtEntry/selectcourse", data).
                then(function (promise) {


                    if (promise.fillbranch.length > 0) {
                        $scope.Branch = promise.fillbranch;
                    }
                    else {
                        swal("No Records found.!!")
                    }
                })

        };

        $scope.onselectbranch = function (branchid) {

            var AMCO_Ids = [];
            var AMB_Ids = [];
            angular.forEach($scope.Fillcourse, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })

            angular.forEach($scope.Branch, function (ty) {
                if (ty.selectedbranch) {
                    AMB_Ids.push(ty.amB_Id);
                }
            })
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                AMCO_Ids: AMCO_Ids,
                AMB_Ids: AMB_Ids,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeStudentwiseAmtEntry/selectbran", data).
                then(function (promise) {

                    if (promise.fillsemester.length > 0) {

                        $scope.fillsemester = promise.fillsemester;
                    }
                    else {
                        swal("No Records found.!!")
                    }
                })
        };

        $scope.onselectsemester = function (semid) {


            var AMCO_Ids = [];
            var AMB_Ids = [];
            var AMSE_Ids = [];

            $scope.fillstudent = [];

            angular.forEach($scope.Fillcourse, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })

            angular.forEach($scope.Branch, function (ty) {
                if (ty.selectedbranch) {
                    AMB_Ids.push(ty.amB_Id);
                }
            })

            angular.forEach($scope.fillsemester, function (ty) {
                if (ty.selectedsem) {
                    AMSE_Ids.push(ty.amsE_Id);
                }
            })



            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                AMCO_Ids: AMCO_Ids,
                AMB_Ids: AMB_Ids,
                AMSE_Ids: AMSE_Ids,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeStudentwiseAmtEntry/selectsem", data).
                then(function (promise) {

                    if (promise.fillstudent.length > 0) {

                        $scope.fillstudent = promise.fillstudent;
                    }
                    else {
                        swal("No Records found.!!")
                    }
                })

        };

        $scope.editEmployee = {}
        $scope.DeletRecord = function (amountid, amcsT_Id, fcmaS_Id) {
            //  $scope.editEmployee = FMA_Id;
            //  var orgid = $scope.editEmployee;
            var data = {
                "FCSA_Id": amountid,
                "AMCST_Id": amcsT_Id,
                "FCMAS_Id": fcmaS_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
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
                        apiService.create("CollegeStudentwiseAmtEntry/Deletedetails/", data).
                            then(function (promise) {

                                // $scope.thirdgrid = promise.alldata;

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

                                // $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
            //})
        }

        $scope.cleardata = function () {

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


        $scope.toggleAll = function (allchkdata) {
            var toggleStatus = $scope.selectedAll;
            angular.forEach($scope.allgroupheaddata, function (itm) {
                itm.isSelected = toggleStatus;
            });
        }

        var selcount = 0;
        $scope.onclickofconcession = function (allgroupheaddata, userdata) {

            for (var i = 0; i < allgroupheaddata.length; i++) {
                var selectedval = allgroupheaddata[i].isSelected
                if (selectedval == true) {
                    selcount = Number(selcount) + 1;
                }
            }
        }


        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("CollegeStudentwiseAmtEntry/Editdetails", orgid).
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

            apiService.create("CollegeStudentwiseAmtEntry/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                })
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };


        $scope.submitted = false;
        // $scope.savedata = function (totalgrid, finslab1, finslab2) {
        $scope.savedata = function (fillstudent, allgroupheaddata) {

            $scope.submitted = true;

            var AMCST_Ids = [];
            $scope.allgrouphead = [];

            if ($scope.myform.$valid) {


                angular.forEach($scope.fillstudent, function (ty) {
                    if (ty.selectedstd) {

                        $scope.AMCST_Id = ty.amcsT_Id;
                    }
                })

                if ($scope.allgroupheaddata.length > 0) {
                    angular.forEach($scope.allgroupheaddata, function (ty) {

                        if (ty.isSelected) {

                            $scope.allgrouphead.push({
                                FMG_Id: ty.fmG_Id,
                                FMH_Id: ty.fmH_Id,
                                FTI_Id: ty.ftI_Id,
                                FCMAS_Amount: ty.fcmaS_Amount,
                                FCMAS_Id: ty.fcmaS_Id,

                            })
                        }
                    });
                }

                var data = {

                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    savetmpdata: $scope.allgrouphead

                }


                apiService.create("CollegeStudentwiseAmtEntry/savescholershpheaddata", data).then(function (promise) {


                    if (promise.returnval == "updated") {

                        swal('Record Saved/Updated Successfuly');
                    }
                    else {

                        swal('Record Not Updated');
                    }


                    $state.reload();
                })
            }
            else {
                // swal("Please enter Valid date")
                $scope.submitted = true;
            }
        };


        $scope.totgdchkbx = function (totary, tchkbx, index) {


            if (tchkbx == true) {
                $scope.finslab1 = [];
                $scope.finslab2 = [];
                $scope.slab1 = false;
                $scope.slab2 = false;
                $scope.typeofstudentregular = false;
                $scope.typeofstudentecs = false;
                headid = totary[index].fmH_Id;
                ftiid = totary[index].ftI_Id;
                $('#myModal').modal('show');


            }
        }
        $scope.headinstallmentid = function (totalgrid, index) {

            //$scope.fineslb = [{ value: "Per Day", FTFS_FineType: "Per Day" },
            //           { value: "Day Slab", FTFS_FineType: "Day Slab" }
            //];


            //$scope.fineslbecs = [{ value: "Per Day", FTFSE_FineType: "Per Day" },
            //          { value: "Day Slab", FTFSE_FineType: "Day Slab" }
            //];

            $scope.typeofstudentregular = false;

            headid = totalgrid[index].fmH_Id;
            ftiid = totalgrid[index].ftI_Id;
            if ($scope.stuchk == "stfoth" && $scope.ASMAY_Id != "") {
                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "selectiontype": $scope.stuchk
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
            }
            else {
                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "FMCC_Id": $scope.FMCC_Id,
                    "selectiontype": $scope.stuchk,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
            }

            apiService.create("CollegeStudentwiseAmtEntry/getgroupmappedheads/", data).
                then(function (promise) {

                    $scope.savedatatransregular = [];
                    $scope.savedatatransecs = [];
                    var r = 0;
                    var e = 0;
                    if (promise.fineslabdetails.length > 0) {
                        $scope.slab1 = true;
                        if ($scope.stuchk == "stfoth") {
                            $scope.slab2 = false;
                        }
                        else {
                            $scope.slab2 = true;
                        }


                        var lenofarray = promise.fineslabdetails.length;

                        for (var i = 0; i < lenofarray; i++) {
                            if (promise.fineslabdetails[i].fmfS_ECSFlag == "R" && promise.fineslabdetails[i].fmH_Id == headid && promise.fineslabdetails[i].ftI_Id == ftiid) {
                                $scope.savedatatransregular[r] = promise.fineslabdetails[i]
                                $scope.savedatatransregular[r].isSelected = true;
                                r = r + 1;
                            }
                        }
                        //if (promise.fineslabdetailsecs != null) {
                        //    var lenofarrayecs = promise.fineslabdetailsecs.length;
                        //    for (var i = 0; i < lenofarrayecs; i++) {
                        //        if (promise.fineslabdetailsecs[i].fmfS_ECSFlag == "E" && promise.fineslabdetailsecs[i].fmH_Id == headid && promise.fineslabdetailsecs[i].ftI_Id == ftiid) {
                        //            $scope.savedatatransecs[e] = promise.fineslabdetailsecs[i]
                        //            $scope.savedatatransecs[e].isSelected = true;
                        //            e = e + 1;
                        //        }
                        //    }
                        //}



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

                    }
                    else {
                        $scope.slab1 = false;
                        $scope.slab2 = false;
                    }
                })

        }
        $scope.savedatatrans1 = [];
        $scope.savedatatrans2 = [];
        var count = 0;
        var count1 = 0;
        $scope.addtocart = function (finslab1, finslab2) {

            var countofanarray = $scope.savedatatrans1.length;

            angular.forEach($scope.finslab1, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans1.push(user);
                    count = Number(count) + 1
                }
            })

            var countofanarrayecs = $scope.savedatatrans2.length;

            angular.forEach($scope.finslab2, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans2.push(user);
                    count1 = Number(count1) + 1
                }
            })

            var lenofarray = $scope.savedatatrans1.length;
            //lenofarray =Number(lenofarray) -Number(countofanarray);

            for (var i = countofanarray; i < lenofarray; i++) {
                $scope.savedatatrans1[i].FMH_Id = headid;
                $scope.savedatatrans1[i].FTI_ID = ftiid;
            }


            var lenofarrayecs = $scope.savedatatrans2.length;
            //lenofarray =Number(lenofarray) -Number(countofanarray);

            for (var i = countofanarrayecs; i < lenofarrayecs; i++) {
                $scope.savedatatrans2[i].FMH_Id = headid;
                $scope.savedatatrans2[i].FTI_ID = ftiid;
            }



            //$scope.slab1 = false;
            //$scope.slab2 = false;

            //  $scope.dismiss('addtocart');

            if (count == 0) {
                swal("Kindly select any one from Regular slab");
            }
            else {
                swal("Selected Fine Slab is added successfully");
                $scope.typeofstudentregular = "";
                $scope.typeofstudentecs = "";
            }
        }

        $scope.is_optionrequired_course = function () {

            return !$scope.Fillcourse.some(function (options) {
                return options.selectedcourse;
            });
        }

        $scope.is_optionrequired_branch = function () {

            return !$scope.Branch.some(function (options) {
                return options.selectedbranch;
            });
        }
        $scope.is_optionrequired_sem = function () {

            return !$scope.fillsemester.some(function (options) {
                return options.selectedsem;
            });
        }
        $scope.is_optionrequired_std = function () {

            return !$scope.fillstudent.some(function (options) {
                return options.selectedstd;
            });
        }




        $scope.validdate = function (month, day, user123) {
            //$scope.monthcnt
            var dayno;
            if (month != "" || month != undefined) {
                angular.forEach($scope.monthcnt, function (itm1) {
                    if (itm1.fctdD_Month == month) {
                        if (month == 2) {
                            // var date = new Date();
                            //var year = date.getFullYear() -1;
                            // if (year % 4 != 0 || year % 100 == 0 && year % 400 != 0) {
                            var year_str;
                            for (var i = 0; i < ASMAY_Year.length; i++) {
                                //if (i < 4) {
                                //    if (i == 0) {
                                //        name = data[i];
                                //    } else {
                                //        name += data[i];
                                //    }
                                //}
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
                                    itm.fctdD_Day = "";
                                }
                            })

                        }

                    }
                })
            }
            else {
                swal("First Select Month !!!");
                $scope.fctdD_Day = "";
            }



            //$scope.fctdD_Month = month;
            //$scope.fctdD_Day = day;
            //if($scope.fctdD_Month!="" || $scope.fctdD_Month!=undefined)
            //{
            //    if ($scope.fctdD_Month == "2") {
            //        var date = new Date();
            //         var year = date.getFullYear()+1;

            //        // if (year % 4 != 0 || year % 100 == 0 && year % 400 != 0) {
            //             if (year % 4 == 0 || year % 100 != 0 && year % 400 == 0) {
            //                 if (Number($scope.fctdD_Day) > 29) {
            //                     swal("Date Not More Than No.of Days Per Selected Month");
            //                     $scope.fctdD_Day = "";
            //            }
            //        }
            //             else {
            //                 if (Number($scope.fctdD_Day) > 28) {
            //                     swal("Date Not More Than No.of Days Per Selected Month");
            //                     $scope.fctdD_Day = "";
            //                 }
            //        }
            //    }
            //    else if ($scope.fctdD_Month == "1" || $scope.fctdD_Month == "3" || $scope.fctdD_Month == "5" || $scope.fctdD_Month == "7" || $scope.fctdD_Month == "8" || $scope.fctdD_Month == "10" || $scope.fctdD_Month == "12")
            //    {
            //        if(Number($scope.fctdD_Day)>31)
            //        {
            //            swal("Date Not More Than No.of Days Per Selected Month");
            //            $scope.fctdD_Day = "";
            //        }
            //   }
            //    else if ($scope.fctdD_Month == "4" || $scope.fctdD_Month == "6" || $scope.fctdD_Month == "9" || $scope.fctdD_Month == "11") {
            //       if (Number($scope.fctdD_Day) > 30) {
            //           swal("Date Not More Than No.of Days Per Selected Month");
            //           angular.forEach($scope.totalgrid, function (itm) {
            //               if(itm.fmA_Id==user123.fmA_Id)
            //               {
            //                   itm.fctdD_Day = "";
            //               }
            //           })

            //       }
            //   }
            //}
            //else {
            //    swal("First Select Month !!!");
            //    $scope.fctdD_Day = "";
            //}
        }


        $scope.onstudentclick = function (amstid) {


            angular.forEach($scope.fillstudent, function (yu) {
                if (yu.amcsT_Id == amstid && yu.selectedstd == false) {
                    yu.selectedstd = true;
                }
            })

            angular.forEach($scope.fillstudent, function (yu) {
                if (yu.amcsT_Id != amstid) {
                    yu.selectedstd = false;
                }
            })



        }


        $scope.FMCC_Id = "";
        $scope.FMG_Id = "";
        $scope.regslbary = [];
        $scope.ecsslbary = [];
        $scope.submitted = false;
        $scope.optionToggled = function () {

            if ($scope.myform.$valid) {





                $scope.slab1 = true;
                $scope.slab2 = true;



                var AMCO_Ids = [];
                var AMB_Ids = [];
                var AMSE_Ids = [];
                var AMCST_Ids = [];
                angular.forEach($scope.Fillcourse, function (ty) {
                    if (ty.selectedcourse) {
                        AMCO_Ids.push(ty.amcO_Id);
                    }
                })

                angular.forEach($scope.Branch, function (ty) {
                    if (ty.selectedbranch) {
                        AMB_Ids.push(ty.amB_Id);
                    }
                })

                angular.forEach($scope.fillsemester, function (ty) {
                    if (ty.selectedsem) {
                        AMSE_Ids.push(ty.amsE_Id);
                    }
                })
                angular.forEach($scope.fillstudent, function (ty) {
                    if (ty.selectedstd) {
                        AMCST_Ids.push(ty.amcsT_Id);
                    }
                })


                var data = {
                    //"FMG_Id": $scope.FMG_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    AMCO_Ids: AMCO_Ids,
                    AMB_Ids: AMB_Ids,
                    AMSE_Ids: AMSE_Ids,
                    AMCST_Ids: AMCST_Ids,


                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeStudentwiseAmtEntry/getmappedconcessionheads", data).then(function (promise) {

                    if (promise.allgroupheaddata.length > 0 && promise.allgroupheaddata != null) {

                        $scope.allgroupheaddata = promise.allgroupheaddata;
                        $scope.grigview1 = true;
                    }

                    else {
                        swal("No Records found.Kindly map Group with heads!!")
                        $scope.grigview1 = false;
                        $scope.thirdgrids = [];
                    }
                })
            }
            else {
                $scope.submitted = true;
                //swal("You are missing mandatory parameter to select")
            }
        };



    }

})();