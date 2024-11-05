(function () {
    'use strict';
    angular
.module('app')
.controller('FeeOpeningBalanceController', FeeOpeningBalanceController123)

    FeeOpeningBalanceController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeOpeningBalanceController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.rndind = "All";
        $scope.individual_Name_Regno = false;
        $scope.individual_Student = false;
        $scope.categorywise = false;
        $scope.classwise = true;
        $scope.filterdata = "NameRegNo";
        $scope.filterdata1 = "Refunable";
        $scope.stustatus = "Active";
        $scope.pda_fees = "fees";
        $scope.DateM = new Date();
        $scope.totcountsearch = 0;
        $scope.sortKey = "fmoB_Id";
        $scope.reverse = true;

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        $scope.cfg = {};
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeOpeningBalance/getalldetails123", pageid).
        then(function (promise) {
            
            $scope.yearlist = promise.acayear;

            $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

            $scope.sectiondrpre = promise.sectionlist;
            $scope.clsdrpdown = promise.classlist;
            $scope.class_Category_List = promise.class_Category_List;
            $scope.students_list = promise.tempararyArrayhEADListnew;
            $scope.test = promise.tempararyArrayhEADListnew;
            $scope.students_list_1 = promise.reportdatelist;
            $scope.totcountfirst = promise.tempararyArrayhEADListnew.length;
            $scope.headlst = promise.fillmasterhead;
            $scope.grouplst = promise.fillmastergroup;
            $scope.onclickload();
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        })
        }


        $scope.changeacademicyear = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }

            apiService.create("FeeOpeningBalance/onselectacademicyear", data).
                then(function (promise) {

                    if (promise.tempararyArrayhEADListnew.length > 0) {
                        $scope.students_list = promise.tempararyArrayhEADListnew;
                        $scope.totcountfirst = promise.tempararyArrayhEADListnew.length;
                    }
                    else {
                        swal("No Records Found")
                        $scope.students_list = {};
                    }

                })
        }


        //$scope.onclickload = function () {
        //    if ($scope.pda_fees == "fees") {
        //        $scope.students_list = promise.TempararyArrayhEADListnew;
        //    }
        //    else {
        //        $scope.students_list = promise.reportdatelist;
        //    }
           
        //};

        $scope.onclickload = function () {
            if ($scope.pda_fees == "fees") {
                $scope.students_list = $scope.test;
            }
            else {
                $scope.students_list = $scope.students_list_1;
            }

        };




        $scope.onclickloaddata = function ()
        {
            if ($scope.rndind == "All") {
                //$scope.rbnsforall = true;
                $scope.individual_Name_Regno = false;
                //$scope.rbnsNameforall = true;    
                $scope.individual_Student = false;
                $scope.classwise = true;
                $scope.categorywise = false;

            }
            else if ($scope.rndind == "Individual") {
                //$scope.rbnsforall = false;
                $scope.individual_Name_Regno = true;
                // $scope.rbnsNameforall = false;
                $scope.individual_Student = true;
                $scope.classwise = true;
                $scope.categorywise = false;

            }
            else if ($scope.rndind == "Category") {
                //$scope.rbnsforall = false;
                $scope.individual_Name_Regno = false;
                // $scope.rbnsNameforall = false;
                $scope.individual_Student = false;
                $scope.classwise = false;
                $scope.categorywise = true;
            }
        };
        $scope.onselectclass = function () {
            
            var data = {
                "asmay_id": $scope.cfg.ASMAY_Id,
                "ASMCL_Id": $scope.clsdrp,
                //  "Adm_no_name": $scope.radio_button,
            }
            apiService.create("FeeOpeningBalance/getclshead/", data).then(function (promise) {
                
                if (promise.sectionlist != null && promise.sectionlist != "") {
                    $scope.sectiondrpre = promise.sectionlist;
                    $scope.headlst = promise.fillmasterhead;
                }
                else {
                    swal("No Section Found Kindly select Another Class/Year");
                    $scope.fee_head_flag = true;
                    $scope.fee_head = false;
                }

            });
        }
        $scope.onselectgroup = function () {
            $scope.installlst = [];
            var data = {
                "FMG_Id": $scope.fmG_Id,
                "FMH_Id": $scope.fmH_Id,
                "asmay_id": $scope.cfg.ASMAY_Id,
            }
            apiService.create("FeeOpeningBalance/getgroup/", data).then(function (promise) {
                
                if (promise.fillinstallment != null && promise.fillinstallment != "") {

                    if (promise.fillinstallment.length == 1) {
                        $scope.installmentshow = false;
                        $scope.ftI_Id = promise.fillinstallment[0].ftI_Id;
                        $scope.installlst.push({
                            ftI_Id: promise.fillinstallment[0].ftI_Id,
                            selected: true,
                            ftI_Name: promise.fillinstallment[0].ftI_Name
                        })
                    }
                    else {
                        $scope.installmentshow = true;
                        $scope.installlst = promise.fillinstallment;
                    }
                  //  $scope.installlst = promise.fillinstallment;
                }
               

            });
        }
        $scope.onselecthead = function (headlst) {
            
            var data = {
                "FMH_Id": $scope.fmH_Id,
                "asmay_id": $scope.cfg.ASMAY_Id,
            }
            apiService.create("FeeOpeningBalance/gethead/", data).then(function (promise) {
                
                if (promise.fillmastergroup != null && promise.fillmastergroup != "")
                {
                    if (promise.fillmastergroup.length == 1)
                    {
                        $scope.groupshow = false;
                        $scope.fmG_Id = promise.fillmastergroup[0].fmG_Id;
                        $scope.onselectgroup();
                    }
                    else {
                        $scope.groupshow = true;
                        $scope.grouplst = promise.fillmastergroup;
                    }
                   

                   // $scope.installlst = promise.fillinstallment;
                }
                //else {
                //    swal("No Fee-Installment Found Kindly select Another Year");
                //}
            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.ShowReportdata = function () {
            
            if ($scope.myForm.$valid) {
                $scope.albumNameArray = [];
                angular.forEach($scope.installlst, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push({ ftI_Id: role.ftI_Id });
                })
                if ($scope.albumNameArray.length === 0) {
                    swal('Select Installment!');
                }
                else {

                    if ($scope.rndind == "All") {

                        var data = {
                            "typeofrpt": $scope.rndind,
                            "asmay_id": $scope.cfg.ASMAY_Id,
                            "fillclasflg": $scope.clsdrp,
                            "fillseccls": $scope.sectiondrp,
                            "studenttype": $scope.stustatus,
                            "FMG_Id": $scope.fmG_Id,
                            "FMH_Id": $scope.fmH_Id,
                            TempararyArrayhEADListnew: $scope.albumNameArray

                            //"FTI_Id": $scope.ftI_Id,
                        }
                    }
                    else if ($scope.rndind == "Individual") {
                        var data = {
                            "typeofrpt": $scope.rndind,
                            "asmay_id": $scope.cfg.ASMAY_Id,
                            "fillclasflg": $scope.clsdrp,
                            "fillseccls": $scope.sectiondrp,
                            "Amst_Id": $scope.Amst_Id,
                            "studenttype": $scope.stustatus,
                            "FMG_Id": $scope.fmG_Id,
                            "FMH_Id": $scope.fmH_Id,
                            TempararyArrayhEADListnew: $scope.albumNameArray
                            // "FTI_Id": $scope.ftI_Id,
                        }

                    }
                    else if ($scope.rndind == "Category") {
                        var data = {
                            "typeofrpt": $scope.rndind,
                            "asmay_id": $scope.cfg.ASMAY_Id,
                            "studenttype": $scope.stustatus,
                            "fmcC_Id": $scope.fmcC_Id,
                            "FMG_Id": $scope.fmG_Id,
                            "FMH_Id": $scope.fmH_Id,
                            TempararyArrayhEADListnew: $scope.albumNameArray
                            //"FTI_Id": $scope.ftI_Id,
                        }
                    }

                    apiService.create("FeeOpeningBalance/getreport/", data).
                        then(function (promise) {
                            if (promise.saveheadlst != null && promise.saveheadlst != "") {

                                $scope.Balance_report = true;
                                $scope.fee_clear = false;
                                $scope.print_flag = false;
                                $scope.students = promise.saveheadlst;
                                $scope.returntxt = promise.returntxt;
                                if (promise.tempararyArrayhEADListnew != null && promise.tempararyArrayhEADListnew != "") {
                                    $scope.students_list = promise.tempararyArrayhEADListnew;
                                }

                                if ($scope.students.length > 0) {
                                    for (var i = 0; i < $scope.students.length; i++) {
                                        if ($scope.students[i].fmoB_Student_Due > 0 || $scope.students[i].fmoB_Institution_Due > 0) {
                                            $scope.students[i].checkedvalue = true;
                                        }
                                    }
                                }
                            }
                            else {
                                swal("No Record Found");
                                $scope.Balance_report = false;
                                $scope.print_flag = true;
                                $scope.Clearid();
                            }
                        })
                }
            }
            else {
                $scope.submitted = true;

            }
        }
        //search start

        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;

                if ($scope.search123 == "3" || $scope.search123 == "4") {
                    $scope.txt = false;
                    $scope.numbr = true;
                    // $scope.dat = false;

                }
                    //else if ($scope.search123 == "4") {

                    //    $scope.txt = false;
                    //    $scope.numbr = false;
                    //    $scope.dat = true;

                    //}
                else {
                    $scope.txt = true;
                    $scope.numbr = false;
                    // $scope.dat = false;

                }
                $scope.searchtxt = "";
                //   $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }
        $scope.ShowSearch_Report = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "")
            { 
                if ($scope.search123 == "3" || $scope.search123 == "4") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr
                    }
                }                   
                else {
                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeOpeningBalance/searching", data).
            then(function (promise) {
                
                $scope.students_list = promise.tempararyArrayhEADListnew;
                $scope.totcountfirst = promise.tempararyArrayhEADListnew.length;
               
                if (promise.tempararyArrayhEADListnew == null || promise.tempararyArrayhEADListnew == "") {
                    swal("Record Does Not Exist For Searched Data !!!!!!")
                }
            })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.clearsearch = function () {

            $state.reload();
            $scope.search123 = "";
            $scope.search_flag = false;
            $scope.searchtxt = "";
            $scope.searchnumbr = "";
            $scope.searchdat = "";
        }
        //search end
        $scope.onselectmodeof = function () {
            
            if ($scope.rndind == "All" && $scope.Balance_report == true && $scope.status != null && $scope.status != "") {
                $scope.ShowReportdata();
            }
            else {
                if ($scope.clsdrp != null && $scope.clsdrp != "") {
                    var data = {
                        "asmay_id": $scope.cfg.ASMAY_Id,
                        "filterinitialdata": $scope.filterdata,
                        "fillseccls": $scope.sectiondrp,
                        "fillclasflg": $scope.clsdrp,
                        "studenttype": $scope.stustatus,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeOpeningBalance/getgroupmappedheads", data).
               then(function (promise) {
                   
                   if (promise.admsudentslist != null && promise.admsudentslist != "") {
                       $scope.studentlst = promise.admsudentslist;
                       $scope.Amst_Id = "";

                   }
                   else {
                       if ($scope.rndind == "Individual") {
                           swal("No Student Found");
                       }
                       $scope.studentlst = "";
                   }
               })
                }
                else {
                    swal("Select Class");
                }
            }
        };
        $scope.onselectrefund = function () {
            
           
                    var data = {
                        "asmay_id": $scope.cfg.ASMAY_Id,
                        "filterrefund": $scope.filterdata1,
                        "fillseccls": $scope.sectiondrp,
                        "fillclasflg": $scope.clsdrp,
                        "studenttype": $scope.stustatus,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeOpeningBalance/getrefund", data).
               then(function (promise) {
                   
                   if (promise.fillmasterhead != null && promise.fillmasterhead != "") {
                       $scope.headlst = promise.fillmasterhead;
                   }
                   else {
                       swal("No Fee-Head Found Kindly select Another Year");
                   }
                   //else {
                   //    if ($scope.rndind == "Individual") {
                   //        swal("No Student Found");
                   //    }
                   //    $scope.studentlst = "";
                   //}
               })
        };
        $scope.clear_fee_balance = function () {
            //if ($scope.rndind == "All") {
            //}
            //else if ($scope.rndind == "Individual") {
            //    $scope.Amst_Id = "";
            //}
            //$scope.asmaY_Id = "";
            //$scope.clsdrp = "";
            //$scope.sectiondrp = "";
            //$scope.fmH_Id = "";
            //$scope.fee_head_flag = true;
            //$scope.fee_head = false;
            //$scope.DateM = "";
            //$scope.pda_fees = "fees";
            //$scope.status = "Active";
            //$scope.Balance_report = false;
            //$scope.print_flag = true;
            //$scope.headcount = "";
            //$scope.studentlst = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();

            $state.reload();

        }
        $scope.submitted = false;
        $scope.students1 = [];
        $scope.savedata = function (pagesrecord) {
            
            if ($scope.myForm.$valid) {

                $scope.albumNameArray = [];
                angular.forEach($scope.installlst, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push({ ftI_Id: role.ftI_Id });
                })

               

                if ($scope.all == true) {
                    angular.forEach(pagesrecord, function (student) {
                        $scope.students1.push(student);
                    });
                } else {
                    angular.forEach(pagesrecord, function (student) {
                        if (student.checkedvalue == true) {
                            $scope.students1.push(student);
                        }
                    });
                }

                $scope.albumNameArray1 = [];
                var flag_cnt = 0;
                angular.forEach($scope.students, function (role) {
                    if (!!role.checkedvalue) {
                        $scope.albumNameArray1.push(role);
                        if ((Number(role.fmoB_Institution_Due) == 0 && Number(role.fmoB_Student_Due) == 0)) {
                            flag_cnt += 1;
                        }
                    }

                })
                //if ($scope.albumNameArray1.length === 0) {
                //    swal('Please Select altleast one Record')
                //}
                //else if (flag_cnt > 0) {
                //    swal('Kindly Enter Amount For Selected Students');
                //}
                //else {
                    ////////if ($scope.AMST_SOL == 'S') {
                    ////////    $scope.AMST_SOL_activate = 'D';
                    ////////}
                    ////////else {
                    ////////    $scope.AMST_SOL_activate = 'S';
                    ////////}
                    $scope.DateM = new Date($scope.DateM).toDateString();
                    var data = {
                        "returntxt": $scope.returntxt,
                        "asmay_id": $scope.cfg.ASMAY_Id,
                        "FMOB_EntryDate": $scope.DateM,
                        "filterrefund": $scope.filterdata1,
                        "FMG_Id": $scope.fmG_Id,
                        "FMH_Id": $scope.fmH_Id,
                        "FTI_Id": $scope.ftI_Id,
                        "fillbusroutestudents": $scope.pda_fees,
                        savetmpdata: $scope.students1,
                        TempararyArrayhEADListnew: $scope.albumNameArray
                    }
                    
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    if ($scope.fswO_Id > 0) {
                        var disfun = "Update";
                    }
                    else {
                        var disfun = "Save";
                    }

                    swal({
                        title: "Are you sure?",
                        text: "Do You Want To " + disfun + " Record? ",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + disfun + " it",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false,
                        showLoaderOnConfirm: true,

                    },
function (isConfirm) {
    if (isConfirm) {

        apiService.create("FeeOpeningBalance/savedata/", data).
               then(function (promise) {
                   if (promise.returntxt == "nocategoryandamountentry") {
                       swal('You are missing Amount entry/Category');
                   }
                   else if (promise.returntxt == "contactadministrator") {
                       swal('Please contact administrator');

                   }
                   else if (promise.returnval == true) {
                       swal('Record Saved/Updated Successfully');
                   }
                   else {
                       swal('Request Failed');
                   }
                   $state.reload();
                   //  $scope.loaddata();
                   //if (promise.returnval == true) {
                   //    swal('Record ' + promise.returntxt + ' Successfully', 'success');
                   //    //$scope.Clearid();
                   //    $state.reload();
                   //    $scope.loaddata();
                   //}
                   //else {
                   //    if (promise.fsscount > 0)
                   //    {
                   //        if (promise.fsscount ==1)
                   //            swal('Mapping have not done Properly for this Records');
                   //        else
                   //            swal('Mapping have not done for ' + promise.fsscount + ' Records');
                   //    }
                   //    else
                   //    {
                   //        swal('Record Not ' + promise.returntxt + ' Successfully', 'Failed');
                   //    }

                   // }
                   //$scope.students_list = promise.tempararyArrayhEADListnew;
                   //$scope.ASMAY_Id = "";
                   //$scope.ASMCL_Id = "";
                   //$scope.ASMC_Id = "";
                   //$scope.students = studclear;
                   //$scope.AMST_SOL_activate = "";
                   //$scope.AMST_SOL_deactivate = "";
                   //$scope.AMST_SOL = "";
               })
        
    }
    else {
        swal("Record saved Failed", "Failed");
    }


});


                   
              //  }
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }
        $scope.delete = function (DeleteRecord, SweetAlert) {
            
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
                    $scope.deleteId = DeleteRecord.fmoB_Id;
                    var MdeleteId = $scope.deleteId;

                    var data = {
                        "FMOB_Id": $scope.deleteId,
                        "fillbusroutestudents": $scope.pda_fees,
                    }


                    apiService.create("FeeOpeningBalance/DeleteEntry", data).
                        then(function (promise) {
                            
                            if (promise.returnval == true) {
                                swal('Record Deleted Successfully');
                                //$scope.Clearid();
                                $state.reload();
                                $scope.loaddata();
                            }
                            else {
                                if (promise.returntxt == "a") {
                                    swal('Amount have already Refunded/Adjusted');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                    $scope.loaddata();
                                }
                                
                            }
                        })
                    
                    //  $scope.$apply();
                    //  swal("Record Deleted Successfully");
                    // $state.reload();
                    // $state.reload();
                    //$scope.BindData();
                }
                else {
                    swal(" Cancelled");
                    $scope.loaddata();
                    $state.reload();
                }
            });
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }

        $scope.optionToggled = function (user) {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
        }
    }
    })();

